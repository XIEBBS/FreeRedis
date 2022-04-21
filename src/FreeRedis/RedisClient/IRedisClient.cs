﻿using System;
using System.Collections.Generic;
using System.IO;

namespace FreeRedis
{
    public interface IRedisClient
    {
        List<Func<IInterceptor>> Interceptors { get; }

        event EventHandler<ConnectedEventArgs> Connected;
        event EventHandler<NoticeEventArgs> Notice;
        event EventHandler<UnavailableEventArgs> Unavailable;

        string[] AclCat(string categoryname = null);
        long AclDelUser(params string[] username);
        string AclGenPass(int bits = 0);
        AclGetUserResult AclGetUser(string username = "default");
        string[] AclList();
        void AclLoad();
        LogResult[] AclLog(long count = 0);
        void AclSave();
        void AclSetUser(string username, params string[] rule);
        string[] AclUsers();
        string AclWhoami();
        long Append<T>(string key, T value);
        void Auth(string password);
        void Auth(string username, string password);
        bool BfAdd(string key, string item);
        bool BfExists(string key, string item);
        Dictionary<string, string> BfInfo(string key);
        string BfInsert(string key, string[] items, long? capacity = null, string error = null, int expansion = 2, bool noCreate = false, bool nonScaling = false);
        string BfLoadChunk(string key, long iter, byte[] data);
        bool[] BfMAdd(string key, string[] items);
        bool[] BfMExists(string key, string[] items);
        string BfReserve(string key, decimal errorRate, long capacity, int expansion = 2, bool nonScaling = false);
        ScanResult<byte[]> BfScanDump(string key, long iter);
        string BgRewriteAof();
        string BgSave(string schedule = null);
        long BitCount(string key, long start, long end);
        long BitOp(BitOpOperation operation, string destkey, params string[] keys);
        long BitPos(string key, bool bit, long? start = null, long? end = null);
        string BLPop(string key, int timeoutSeconds);
        KeyValue<string> BLPop(string[] keys, int timeoutSeconds);
        T BLPop<T>(string key, int timeoutSeconds);
        KeyValue<T> BLPop<T>(string[] keys, int timeoutSeconds);
        string BRPop(string key, int timeoutSeconds);
        KeyValue<string> BRPop(string[] keys, int timeoutSeconds);
        T BRPop<T>(string key, int timeoutSeconds);
        KeyValue<T> BRPop<T>(string[] keys, int timeoutSeconds);
        string BRPopLPush(string source, string destination, int timeoutSeconds);
        T BRPopLPush<T>(string source, string destination, int timeoutSeconds);
        ZMember BZPopMax(string key, int timeoutSeconds);
        KeyValue<ZMember> BZPopMax(string[] keys, int timeoutSeconds);
        ZMember BZPopMin(string key, int timeoutSeconds);
        KeyValue<ZMember> BZPopMin(string[] keys, int timeoutSeconds);
        object Call(CommandPacket cmd);
        bool CfAdd(string key, string item);
        bool CfAddNx(string key, string item);
        long CfCount(string key, string item);
        bool CfDel(string key, string item);
        bool CfExists(string key, string item);
        Dictionary<string, string> CfInfo(string key);
        string CfInsert(string key, string[] items, long? capacity = null, bool noCreate = false);
        string CfInsertNx(string key, string[] items, long? capacity = null, bool noCreate = false);
        string CfLoadChunk(string key, long iter, byte[] data);
        string CfReserve(string key, long capacity, long? bucketSize = null, long? maxIterations = null, int? expansion = null);
        ScanResult<byte[]> CfScanDump(string key, long iter);
        void ClientCaching(Confirm confirm);
        string ClientGetName();
        long ClientGetRedir();
        long ClientId();
        void ClientKill(string ipport);
        long ClientKill(string ipport, long? clientid, ClientType? type, string username, string addr, Confirm? skipme);
        string ClientList(ClientType? type = null);
        void ClientPause(long timeoutMilliseconds);
        void ClientReply(ClientReplyType type);
        void ClientSetName(string connectionName);
        void ClientTracking(bool on_off, long? redirect, string[] prefix, bool bcast, bool optin, bool optout, bool noloop);
        bool ClientUnBlock(long clientid, ClientUnBlockType? type = null);
        long[] CmsIncrBy(string key, Dictionary<string, long> itemIncrements);
        long CmsIncrBy(string key, string item, long increment);
        Dictionary<string, string> CmsInfo(string key);
        string CmsInitByDim(string key, long width, long depth);
        string CmsInitByProb(string key, decimal error, decimal probability);
        string CmsMerge(string dest, long numKeys, string[] src, long[] weights);
        long[] CmsQuery(string key, string[] items);
        object[] Command();
        long CommandCount();
        string[] CommandGetKeys(params string[] command);
        object[] CommandInfo(params string[] command);
        Dictionary<string, string> ConfigGet(string parameter);
        void ConfigResetStat();
        void ConfigRewrite();
        void ConfigSet<T>(string parameter, T value);
        long DbSize();
        string DebugObject(string key);
        long Decr(string key);
        long DecrBy(string key, long decrement);
        long Del(params string[] keys);
        void Dispose();
        byte[] Dump(string key);
        string Echo(string message);
        object Eval(string script, string[] keys = null, params object[] arguments);
        object EvalSha(string sha1, string[] keys = null, params object[] arguments);
        bool Exists(string key);
        long Exists(string[] keys);
        bool Expire(string key, int seconds);
        bool Expire(string key, TimeSpan expire);
        bool ExpireAt(string key, DateTime timestamp);
        void FlushAll(bool isasync = false);
        void FlushDb(bool isasync = false);
        long GeoAdd(string key, params GeoMember[] members);
        decimal? GeoDist(string key, string member1, string member2, GeoUnit unit = GeoUnit.m);
        string GeoHash(string key, string member);
        string[] GeoHash(string key, string[] members);
        GeoMember GeoPos(string key, string member);
        GeoMember[] GeoPos(string key, string[] members);
        GeoRadiusResult[] GeoRadius(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, bool withdoord = false, bool withdist = false, bool withhash = false, long? count = null, Collation? collation = null);
        GeoRadiusResult[] GeoRadiusByMember(string key, string member, decimal radius, GeoUnit unit = GeoUnit.m, bool withdoord = false, bool withdist = false, bool withhash = false, long? count = null, Collation? collation = null);
        long GeoRadiusByMemberStore(string key, string member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, Collation? collation = null, string storekey = null, string storedistkey = null);
        long GeoRadiusStore(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, Collation? collation = null, string storekey = null, string storedistkey = null);
        string Get(string key);
        void Get(string key, Stream destination, int bufferSize = 1024);
        T Get<T>(string key);
        bool GetBit(string key, long offset);
        RedisClient.DatabaseHook GetDatabase(int? index = null);
        string GetRange(string key, long start, long end);
        T GetRange<T>(string key, long start, long end);
        string GetSet<T>(string key, T value);
        long HDel(string key, params string[] fields);
        Dictionary<string, object> Hello(string protover, string username = null, string password = null, string clientname = null);
        bool HExists(string key, string field);
        string HGet(string key, string field);
        T HGet<T>(string key, string field);
        Dictionary<string, string> HGetAll(string key);
        Dictionary<string, T> HGetAll<T>(string key);
        long HIncrBy(string key, string field, long increment);
        decimal HIncrByFloat(string key, string field, decimal increment);
        string[] HKeys(string key);
        long HLen(string key);
        string[] HMGet(string key, params string[] fields);
        T[] HMGet<T>(string key, params string[] fields);
        void HMSet<T>(string key, Dictionary<string, T> keyValues);
        void HMSet<T>(string key, string field, T value, params object[] fieldValues);
        ScanResult<string> HScan(string key, long cursor, string pattern, long count);
        long HSet<T>(string key, Dictionary<string, T> keyValues);
        long HSet<T>(string key, string field, T value, params object[] fieldValues);
        bool HSetNx<T>(string key, string field, T value);
        long HStrLen(string key, string field);
        string[] HVals(string key);
        T[] HVals<T>(string key);
        long Incr(string key);
        long IncrBy(string key, long increment);
        decimal IncrByFloat(string key, decimal increment);
        string Info(string section = null);
        string[] Keys(string pattern);
        DateTime LastSave();
        string LatencyDoctor();
        string LIndex(string key, long index);
        T LIndex<T>(string key, long index);
        long LInsert(string key, InsertDirection direction, object pivot, object element);
        long LLen(string key);
        RedisClient.LockController Lock(string name, int timeoutSeconds, bool autoDelay = true);
        string LPop(string key);
        T LPop<T>(string key);
        long LPos<T>(string key, T element, int rank = 0);
        long[] LPos<T>(string key, T element, int rank, int count, int maxLen);
        long LPush(string key, params object[] elements);
        long LPushX(string key, params object[] elements);
        string[] LRange(string key, long start, long stop);
        T[] LRange<T>(string key, long start, long stop);
        long LRem<T>(string key, long count, T element);
        void LSet<T>(string key, long index, T element);
        void LTrim(string key, long start, long stop);
        string MemoryDoctor();
        string MemoryMallocStats();
        void MemoryPurge();
        Dictionary<string, object> MemoryStats();
        long MemoryUsage(string key, long count = 0);
        string[] MGet(params string[] keys);
        T[] MGet<T>(params string[] keys);
        void Migrate(string host, int port, string key, int destinationDb, long timeoutMilliseconds, bool copy, bool replace, string authPassword, string auth2Username, string auth2Password, string[] keys);
        bool Move(string key, int db);
        void MSet(string key, object value, params object[] keyValues);
        void MSet<T>(Dictionary<string, T> keyValues);
        bool MSetNx(string key, object value, params object[] keyValues);
        bool MSetNx<T>(Dictionary<string, T> keyValues);
        RedisClient.TransactionHook Multi();
        string ObjectEncoding(string key);
        long? ObjectFreq(string key);
        long ObjectIdleTime(string key);
        long? ObjectRefCount(string key);
        bool Persist(string key);
        bool PExpire(string key, int milliseconds);
        bool PExpireAt(string key, DateTime timestamp);
        bool PfAdd(string key, params object[] elements);
        long PfCount(params string[] keys);
        void PfMerge(string destkey, params string[] sourcekeys);
        string Ping(string message = null);
        void PSetEx<T>(string key, long milliseconds, T value);
        IDisposable PSubscribe(string pattern, Action<string, object> handler);
        IDisposable PSubscribe(string[] pattern, Action<string, object> handler);
        long PTtl(string key);
        long Publish(string channel, string message);
        string[] PubSubChannels(string pattern);
        long PubSubNumPat(string message);
        long PubSubNumSub(string channel);
        long[] PubSubNumSub(string[] channels);
        void PUnSubscribe(params string[] pattern);
        void Quit();
        string RandomKey();
        void Rename(string key, string newkey);
        bool RenameNx(string key, string newkey);
        void ReplicaOf(string host, int port);
        void Restore(string key, byte[] serializedValue);
        void Restore(string key, int ttl, byte[] serializedValue, bool replace = false, bool absTtl = false, int? idleTimeSeconds = null, decimal? frequency = null);
        RoleResult Role();
        string RPop(string key);
        T RPop<T>(string key);
        string RPopLPush(string source, string destination);
        T RPopLPush<T>(string source, string destination);
        long RPush(string key, params object[] elements);
        long RPushX(string key, params object[] elements);
        long SAdd(string key, params object[] members);
        void Save();
        ScanResult<string> Scan(long cursor, string pattern, long count, string type);
        IEnumerable<string[]> Scan(string pattern, long count, string type);
        long SCard(string key);
        bool ScriptExists(string sha1);
        bool[] ScriptExists(string[] sha1);
        void ScriptFlush();
        void ScriptKill();
        string ScriptLoad(string script);
        string[] SDiff(params string[] keys);
        T[] SDiff<T>(params string[] keys);
        long SDiffStore(string destination, params string[] keys);
        void Select(int index);
        void Set<T>(string key, T value, int timeoutSeconds = 0);
        void Set<T>(string key, T value, bool keepTtl);
        void Set<T>(string key, T value, TimeSpan timeout);
        string Set<T>(string key, T value, TimeSpan timeout, bool keepTtl, bool nx, bool xx, bool get);
        long SetBit(string key, long offset, bool value);
        void SetEx<T>(string key, int seconds, T value);
        bool SetNx<T>(string key, T value);
        bool SetNx<T>(string key, T value, int timeoutSeconds);
        bool SetNx<T>(string key, T value, TimeSpan timeout);
        long SetRange<T>(string key, long offset, T value);
        bool SetXx<T>(string key, T value, int timeoutSeconds = 0);
        bool SetXx<T>(string key, T value, bool keepTtl);
        bool SetXx<T>(string key, T value, TimeSpan timeout);
        string[] SInter(params string[] keys);
        T[] SInter<T>(params string[] keys);
        long SInterStore(string destination, params string[] keys);
        bool SIsMember<T>(string key, T member);
        void SlaveOf(string host, int port);
        object SlowLog(string subcommand, params string[] argument);
        string[] SMembers(string key);
        T[] SMembers<T>(string key);
        bool[] SMIsMember<T>(string key, params object[] members);
        bool SMove<T>(string source, string destination, T member);
        string[] Sort(string key, string byPattern = null, long offset = 0, long count = 0, string[] getPatterns = null, Collation? collation = null, bool alpha = false);
        long SortStore(string storeDestination, string key, string byPattern = null, long offset = 0, long count = 0, string[] getPatterns = null, Collation? collation = null, bool alpha = false);
        string SPop(string key);
        string[] SPop(string key, int count);
        T SPop<T>(string key);
        T[] SPop<T>(string key, int count);
        string SRandMember(string key);
        string[] SRandMember(string key, int count);
        T SRandMember<T>(string key);
        T[] SRandMember<T>(string key, int count);
        long SRem(string key, params object[] members);
        ScanResult<string> SScan(string key, long cursor, string pattern, long count);
        RedisClient.PipelineHook StartPipe();
        long StrLen(string key);
        IDisposable Subscribe(string channel, Action<string, object> handler);
        IDisposable Subscribe(string[] channels, Action<string, object> handler);
        string[] SUnion(params string[] keys);
        T[] SUnion<T>(params string[] keys);
        long SUnionStore(string destination, params string[] keys);
        void SwapDb(int index1, int index2);
        DateTime Time();
        string[] TopkAdd(string key, string[] items);
        long[] TopkCount(string key, string[] items);
        string[] TopkIncrBy(string key, Dictionary<string, long> itemIncrements);
        string TopkIncrBy(string key, string item, long increment);
        Dictionary<string, string> TopkInfo(string key);
        string[] TopkList(string key);
        bool[] TopkQuery(string key, string[] items);
        string TopkReserve(string key, long topk, long width, long depth, decimal decay);
        long Touch(params string[] keys);
        long Ttl(string key);
        KeyType Type(string key);
        long UnLink(params string[] keys);
        void UnSubscribe(params string[] channels);
        long Wait(long numreplicas, long timeoutMilliseconds);
        long XAck(string key, string group, params string[] id);
        string XAdd<T>(string key, Dictionary<string, T> fieldValues);
        string XAdd<T>(string key, long maxlen, string id, Dictionary<string, T> fieldValues);
        string XAdd<T>(string key, long maxlen, string id, string field, T value, params object[] fieldValues);
        string XAdd<T>(string key, string field, T value, params object[] fieldValues);
        StreamsEntry[] XClaim(string key, string group, string consumer, long minIdleTime, params string[] id);
        StreamsEntry[] XClaim(string key, string group, string consumer, long minIdleTime, string[] id, long idle, long retryCount, bool force);
        string[] XClaimJustId(string key, string group, string consumer, long minIdleTime, params string[] id);
        string[] XClaimJustId(string key, string group, string consumer, long minIdleTime, string[] id, long idle, long retryCount, bool force);
        long XDel(string key, params string[] id);
        void XGroupCreate(string key, string group, string id = "$", bool MkStream = false);
        void XGroupCreateConsumer(string key, string group, string consumer);
        long XGroupDelConsumer(string key, string group, string consumer);
        bool XGroupDestroy(string key, string group);
        void XGroupSetId(string key, string group, string id = "$");
        StreamsXInfoConsumersResult[] XInfoConsumers(string key, string group);
        StreamsXInfoGroupsResult[] XInfoGroups(string key);
        StreamsXInfoStreamResult XInfoStream(string key);
        StreamsXInfoStreamFullResult XInfoStreamFull(string key, long count = 10);
        long XLen(string key);
        StreamsXPendingResult XPending(string key, string group);
        StreamsXPendingConsumerResult[] XPending(string key, string group, string start, string end, long count, string consumer = null);
        StreamsEntry[] XRange(string key, string start, string end, long count = -1);
        StreamsEntryResult[] XRead(long count, long block, Dictionary<string, string> keyIds);
        StreamsEntryResult[] XRead(long count, long block, string key, string id, params string[] keyIds);
        StreamsEntry XRead(long block, string key, string id);
        StreamsEntryResult[] XReadGroup(string group, string consumer, long count, long block, bool noack, Dictionary<string, string> keyIds);
        StreamsEntryResult[] XReadGroup(string group, string consumer, long count, long block, bool noack, string key, string id, params string[] keyIds);
        StreamsEntry XReadGroup(string group, string consumer, long block, string key, string id);
        StreamsEntry[] XRevRange(string key, string end, string start, long count = -1);
        long XTrim(string key, long count);
        long ZAdd(string key, decimal score, string member, params object[] scoreMembers);
        long ZAdd(string key, ZMember[] scoreMembers, ZAddThan? than = null, bool ch = false);
        long ZAddNx(string key, decimal score, string member, params object[] scoreMembers);
        long ZAddNx(string key, ZMember[] scoreMembers, ZAddThan? than = null, bool ch = false);
        long ZAddXx(string key, decimal score, string member, params object[] scoreMembers);
        long ZAddXx(string key, ZMember[] scoreMembers, ZAddThan? than = null, bool ch = false);
        long ZCard(string key);
        long ZCount(string key, decimal min, decimal max);
        long ZCount(string key, string min, string max);
        decimal ZIncrBy(string key, decimal increment, string member);
        long ZInterStore(string destination, string[] keys, int[] weights = null, ZAggregate? aggregate = null);
        long ZLexCount(string key, string min, string max);
        ZMember ZPopMax(string key);
        ZMember[] ZPopMax(string key, int count);
        ZMember ZPopMin(string key);
        ZMember[] ZPopMin(string key, int count);
        string[] ZRandMember(string key, int count, bool repetition);
        ZMember[] ZRandMemberWithScores(string key, int count, bool repetition);
        string[] ZRange(string key, decimal start, decimal stop);
        string[] ZRangeByLex(string key, string min, string max, int offset = 0, int count = 0);
        string[] ZRangeByScore(string key, decimal min, decimal max, int offset = 0, int count = 0);
        string[] ZRangeByScore(string key, string min, string max, int offset = 0, int count = 0);
        ZMember[] ZRangeByScoreWithScores(string key, decimal min, decimal max, int offset = 0, int count = 0);
        ZMember[] ZRangeByScoreWithScores(string key, string min, string max, int offset = 0, int count = 0);
        ZMember[] ZRangeWithScores(string key, decimal start, decimal stop);
        long? ZRank(string key, string member);
        long ZRem(string key, params string[] members);
        long ZRemRangeByLex(string key, string min, string max);
        long ZRemRangeByRank(string key, long start, long stop);
        long ZRemRangeByScore(string key, decimal min, decimal max);
        long ZRemRangeByScore(string key, string min, string max);
        string[] ZRevRange(string key, decimal start, decimal stop);
        string[] ZRevRangeByLex(string key, decimal max, decimal min, int offset = 0, int count = 0);
        string[] ZRevRangeByLex(string key, string max, string min, int offset = 0, int count = 0);
        string[] ZRevRangeByScore(string key, decimal max, decimal min, int offset = 0, int count = 0);
        string[] ZRevRangeByScore(string key, string max, string min, int offset = 0, int count = 0);
        ZMember[] ZRevRangeByScoreWithScores(string key, decimal max, decimal min, int offset = 0, int count = 0);
        ZMember[] ZRevRangeByScoreWithScores(string key, string max, string min, int offset = 0, int count = 0);
        ZMember[] ZRevRangeWithScores(string key, decimal start, decimal stop);
        long ZRevRank(string key, string member);
        decimal? ZScore(string key, string member);
        long ZUnionStore(string destination, string[] keys, int[] weights = null, ZAggregate? aggregate = null);
    }
}