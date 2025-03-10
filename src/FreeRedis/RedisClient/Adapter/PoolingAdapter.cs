﻿using FreeRedis.Internal;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace FreeRedis
{
    partial class RedisClient
    {
        internal class PoolingAdapter : BaseAdapter
        {
            internal readonly IdleBus<RedisClientPool> _ib;
            internal readonly string _masterHost;
            readonly bool _rw_splitting;
            readonly bool _is_single;

            public PoolingAdapter(RedisClient topOwner, ConnectionStringBuilder connectionString, params ConnectionStringBuilder[] slaveConnectionStrings)
            {
                UseType = UseType.Pooling;
                TopOwner = topOwner;
                _masterHost = connectionString.Host;
                _rw_splitting = slaveConnectionStrings?.Any() == true;
                _is_single = !_rw_splitting && connectionString.MaxPoolSize == 1;

                _ib = new IdleBus<RedisClientPool>(TimeSpan.FromMinutes(10));
                _ib.Register(_masterHost, () => new RedisClientPool(connectionString, null, TopOwner));

                if (_rw_splitting)
                    foreach (var slave in slaveConnectionStrings)
                        _ib.TryRegister($"slave_{slave.Host}", () => new RedisClientPool(slave, null, TopOwner));

#if isasync
                _asyncManager = new AsyncRedisSocket.Manager(this);
#endif
            }

            bool isdisposed = false;
            public override void Dispose()
            {
                foreach (var key in _ib.GetKeys())
                {
                    var pool = _ib.Get(key);
                    TopOwner.Unavailable?.Invoke(TopOwner, new UnavailableEventArgs(pool.Key, pool));
                }
                isdisposed = true;
                _ib.Dispose();
            }

            public override void Refersh(IRedisSocket redisSocket)
            {
                if (isdisposed) return;
                var tmprds = redisSocket as DefaultRedisSocket.TempProxyRedisSocket;
                if (tmprds != null) _ib.Get(tmprds._poolkey);
            }
            public override IRedisSocket GetRedisSocket(CommandPacket cmd)
            {
                var poolkey = GetIdleBusKey(cmd);
                var pool = _ib.Get(poolkey);
                var cli = pool.Get();
                var rds = cli.Value.Adapter.GetRedisSocket(null);
                var rdsproxy = DefaultRedisSocket.CreateTempProxy(rds, () => pool.Return(cli));
                rdsproxy._poolkey = poolkey;
                rdsproxy._pool = pool;
                return rdsproxy;
            }
            public override TValue AdapterCall<TValue>(CommandPacket cmd, Func<RedisResult, TValue> parse)
            {
                return TopOwner.LogCall(cmd, () =>
                {
                    RedisResult rt = null;
                    var protocolRetry = false;
                    using (var rds = GetRedisSocket(cmd))
                    {
                        var getTime = DateTime.Now;
                        try
                        {
                            rds.Write(cmd);
                            rt = rds.Read(cmd);
                        }
                        catch (ProtocolViolationException)
                        {
                            var pool = (rds as DefaultRedisSocket.TempProxyRedisSocket)._pool;
                            rds.ReleaseSocket();
                            cmd._protocolErrorTryCount++;
                            if (cmd._protocolErrorTryCount <= pool._policy._connectionStringBuilder.Retry)
                                protocolRetry = true;
                            else
                            {
                                if (cmd.IsReadOnlyCommand() == false || cmd._protocolErrorTryCount > 1) throw;
                                protocolRetry = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            var pool = (rds as DefaultRedisSocket.TempProxyRedisSocket)._pool;
                            if (pool?.SetUnavailable(ex, getTime) == true)
                            {
                            }
                            throw;
                        }
                    }
                    if (protocolRetry) return AdapterCall(cmd, parse);
                    return parse(rt);
                });
            }
#if isasync
            AsyncRedisSocket.Manager _asyncManager;
            public override Task<TValue> AdapterCallAsync<TValue>(CommandPacket cmd, Func<RedisResult, TValue> parse)
            {
                return TopOwner.LogCallAsync(cmd, async () =>
                {
                    var asyncRds = _asyncManager.GetAsyncRedisSocket(cmd);
                    try
                    {
                        var rt = await asyncRds.WriteAsync(cmd);
                        return parse(rt);
                    }
                    catch (ProtocolViolationException)
                    {
                        var pool = (asyncRds._rds as DefaultRedisSocket.TempProxyRedisSocket)?._pool;
                        cmd._protocolErrorTryCount++;
                        if (pool != null && cmd._protocolErrorTryCount <= pool._policy._connectionStringBuilder.Retry)
                            return await AdapterCallAsync(cmd, parse);
                        else
                        {
                            if (cmd.IsReadOnlyCommand() == false || cmd._protocolErrorTryCount > 1) throw;
                            return await AdapterCallAsync(cmd, parse);
                        }
                    }
                });
            }
#endif

            string GetIdleBusKey(CommandPacket cmd)
            {
                if (cmd != null && (_rw_splitting || !_is_single))
                {
                    var cmdset = CommandSets.Get(cmd._command);
                    if (cmdset != null)
                    {
                        if (!_is_single && (cmdset.Status & CommandSets.LocalStatus.check_single) == CommandSets.LocalStatus.check_single)
                            throw new RedisClientException($"Method cannot be used in {UseType} mode. You can set \"max pool size=1\", but it is not singleton mode.");

                        if (_rw_splitting &&
                            ((cmdset.Tag & CommandSets.ServerTag.read) == CommandSets.ServerTag.read ||
                            (cmdset.Flag & CommandSets.ServerFlag.@readonly) == CommandSets.ServerFlag.@readonly))
                        {
                            var rndkeys = _ib.GetKeys(v => v == null || v.IsAvailable && v._policy._connectionStringBuilder.Host != _masterHost);
                            if (rndkeys.Any())
                            {
                                var rndkey = rndkeys[_rnd.Value.Next(0, rndkeys.Length)];
                                return rndkey;
                            }
                        }
                    }
                }
                return _masterHost;
            }
        }
    }
}