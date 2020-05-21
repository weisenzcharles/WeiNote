# 前言
公司的项目以前一直使用 CSRedis 这个类库来操作 Redis，最近增加了一些新功能，会存储一些比较大的数据，内测的时候发现其中有两台服务器会莫名的报错 `Unexpected response type: Status (expecting Bulk)` 和 `Connection was not opened`，最后定位到问题是 Redis 写入和读取数据的时候发生的错误，弄了两台新服务器重新部署还是没有解决，在 GitHub 上向作者发了 [issues](https://github.com/2881099/csredis/issues/253)，作者说升级类库可以解决，尝试了一下也没有解决，无奈之下只好写了个小程序用 StackExchange.Redis 在服务器上做读写测试，发现没有任何问题，防止耽误上线只好换成了 StackExchange.Redis，经过两天内部测试，一切操作均未发现异常。
# 封装
RedisClient 类：
```csharp
    /// <summary>
    /// 封装 Redis 相关操作的方法类。
    /// </summary>
    public class RedisClient : IRedisClient
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;

        /// <summary>
        /// 初始化 <see cref="RedisClient"/> 类的新实例。
        /// </summary>
        /// <param name="connectionMultiplexer">连接多路复用器。</param>
        public RedisClient(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            if (_connectionMultiplexer != null && _connectionMultiplexer.IsConnected)
            {
                _database = _connectionMultiplexer.GetDatabase();
            }
            else
            {
                throw new Exception("Redis is not Connected");
            }

        }

        #region 同步方法...

        /// <summary>
        /// 添加一个字符串对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiry">过期时间（时间间隔）。</param>
        /// <returns>返回是否执行成功。</returns>
        public bool Set(string key, string value, TimeSpan? expiry = null)
        {
            return _database.StringSet(key, value, expiry);
        }

        /// <summary>
        /// 添加一个字符串对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">值。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>返回是否执行成功。</returns>
        public bool Set(string key, string value, int seconds)
        {
            TimeSpan expiry = TimeSpan.FromSeconds(seconds);
            return _database.StringSet(key, value, expiry);
        }

        /// <summary>
        /// 添加一个对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">值。</param>
        /// <param name="expiry">过期时间（时间间隔）。</param>
        /// <returns>返回是否执行成功。</returns>
        public bool Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            var data = JsonConvert.SerializeObject(value);
            return _database.StringSet(key, data, expiry);
        }

        /// <summary>
        /// 添加一个对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">值。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>返回是否执行成功。</returns>
        public bool Set<T>(string key, T value, int seconds)
        {
            TimeSpan expiry = TimeSpan.FromSeconds(seconds);
            var data = JsonConvert.SerializeObject(value);
            return _database.StringSet(key, data, expiry);
        }

        /// <summary>
        /// 获取一个对象。
        /// </summary>
        /// <param name="key">值。</param>
        /// <returns>返回对象的值。</returns>
        public T Get<T>(string key)
        {
            string json = _database.StringGet(key);
            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }
            T entity = JsonConvert.DeserializeObject<T>(json);
            return entity;
        }

        /// <summary>
        /// 获取一个字符串对象。
        /// </summary>
        /// <param name="key">值。</param>
        /// <returns>返回对象的值。</returns>
        public string Get(string key)
        {
            return _database.StringGet(key);
        }

        /// <summary>
        /// 删除一个对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>返回是否执行成功。</returns>
        public bool Delete(string key)
        {
            return _database.KeyDelete(key);
        }

        /// <summary>
        /// 返回键是否存在。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>返回键是否存在。</returns>
        public bool Exists(string key)
        {
            return _database.KeyExists(key);
        }

        /// <summary>
        /// 设置一个键的过期时间。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="expiry">过期时间（时间间隔）。</param>
        /// <returns>返回是否执行成功。</returns>
        public bool SetExpire(string key, TimeSpan? expiry)
        {
            return _database.KeyExpire(key, expiry);
        }

        /// <summary>
        /// 设置一个键的过期时间。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>返回是否执行成功。</returns>
        public bool SetExpire(string key, int seconds)
        {
            TimeSpan expiry = TimeSpan.FromSeconds(seconds);
            return _database.KeyExpire(key, expiry);
        }

        #endregion

        #region 异步方法...

        /// <summary>
        /// 异步添加一个字符串对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiry">过期时间（时间间隔）。</param>
        /// <returns>返回是否执行成功。</returns>
        public async Task<bool> SetAsync(string key, string value, TimeSpan? expiry = null)
        {
            return await _database.StringSetAsync(key, value, expiry);
        }

        /// <summary>
        /// 异步添加一个字符串对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>返回是否执行成功。</returns>
        public async Task<bool> SetAsync(string key, string value, int seconds)
        {
            TimeSpan expiry = TimeSpan.FromSeconds(seconds);
            return await _database.StringSetAsync(key, value, expiry);
        }

        /// <summary>
        /// 异步添加一个对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">值。</param>
        /// <returns>返回是否执行成功。</returns>
        public async Task<bool> SetAsync<T>(string key, T value)
        {
            var data = JsonConvert.SerializeObject(value);
            return await _database.StringSetAsync(key, data);
        }

        /// <summary>
        /// 异步获取一个对象。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="key">值。</param>
        /// <returns>返回对象的值。</returns>
        public async Task<T> GetAsync<T>(string key)
        {
            string json = await _database.StringGetAsync(key);
            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }
            T entity = JsonConvert.DeserializeObject<T>(json);
            return entity;
        }

        /// <summary>
        /// 异步获取一个字符串对象。
        /// </summary>
        /// <param name="key">值。</param>
        /// <returns>返回对象的值。</returns>
        public async Task<string> GetAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        /// <summary>
        /// 异步删除一个对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>返回是否执行成功。</returns>
        public async Task<bool> DeleteAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        /// <summary>
        /// 异步设置一个键的过期时间。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>返回是否执行成功。</returns>
        public async Task<bool> SetExpireAsync(string key, int seconds)
        {
            TimeSpan expiry = TimeSpan.FromSeconds(seconds);
            return await _database.KeyExpireAsync(key, expiry);
        }

        /// <summary>
        /// 异步设置一个键的过期时间。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="expiry">过期时间（时间间隔）。</param>
        /// <returns>返回是否执行成功。</returns>
        public async Task<bool> SetExpireAsync(string key, TimeSpan? expiry)
        {
            return await _database.KeyExpireAsync(key, expiry);
        }

        #endregion

        #region 分布式锁...

        /// <summary>
        /// 分布式锁 Token。
        /// </summary>
        private static readonly RedisValue LockToken = Environment.MachineName;

        /// <summary>
        /// 获取锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>是否已锁。</returns>
        public bool Lock(string key, int seconds)
        {
            return _database.LockTake(key, LockToken, TimeSpan.FromSeconds(seconds));
        }

        /// <summary>
        /// 释放锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <returns>是否成功。</returns>
        public bool UnLock(string key)
        {
            return _database.LockRelease(key, LockToken);
        }

        /// <summary>
        /// 异步获取锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>是否成功。</returns>
        public async Task<bool> LockAsync(string key, int seconds)
        {
            return await _database.LockTakeAsync(key, LockToken, TimeSpan.FromSeconds(seconds));
        }

        /// <summary>
        /// 异步释放锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <returns>是否成功。</returns>
        public async Task<bool> UnLockAsync(string key)
        {
            return await _database.LockReleaseAsync(key, LockToken);
        }

        #endregion
    }
```
IRedisClient 类：
```csharp
    /// <summary>
    /// 封装 Redis 相关操作的方法。
    /// </summary>
    public interface IRedisClient
    {
        /// <summary>
        /// 添加一个字符串对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiry">过期时间（时间间隔）。</param>
        /// <returns>返回是否执行成功。</returns>
        bool Set(string key, string value, TimeSpan? expiry = null);

        /// <summary>
        /// 添加一个字符串对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">值。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>返回是否执行成功。</returns>
        bool Set(string key, string value, int seconds);

        /// <summary>
        /// 添加一个对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">值。</param>
        /// <param name="expiry">过期时间（时间间隔）。</param>
        /// <returns>返回是否执行成功。</returns>
        bool Set<T>(string key, T value, TimeSpan? expiry = null);

        /// <summary>
        /// 添加一个对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">值。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>返回是否执行成功。</returns>
        bool Set<T>(string key, T value, int seconds);

        /// <summary>
        /// 获取一个对象。
        /// </summary>
        /// <param name="key">值。</param>
        /// <returns>返回对象的值。</returns>
        T Get<T>(string key);

        /// <summary>
        /// 获取一个字符串对象。
        /// </summary>
        /// <param name="key">值。</param>
        /// <returns>返回对象的值。</returns>
        string Get(string key);

        /// <summary>
        /// 删除一个对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>返回是否执行成功。</returns>
        bool Delete(string key);

        /// <summary>
        /// 返回键是否存在。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>返回键是否存在。</returns>
        bool Exists(string key);

        /// <summary>
        /// 设置一个键的过期时间。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="expiry">过期时间（时间间隔）。</param>
        /// <returns>返回是否执行成功。</returns>
        bool SetExpire(string key, TimeSpan? expiry);

        /// <summary>
        /// 设置一个键的过期时间。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>返回是否执行成功。</returns>
        bool SetExpire(string key, int seconds);

        /// <summary>
        /// 异步添加一个字符串对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="expiry">过期时间（时间间隔）。</param>
        /// <returns>返回是否执行成功。</returns>
        Task<bool> SetAsync(string key, string value, TimeSpan? expiry = null);

        /// <summary>
        /// 异步添加一个字符串对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="value">值。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>返回是否执行成功。</returns>
        Task<bool> SetAsync(string key, string value, int seconds);

        /// <summary>
        /// 异步添加一个对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">值。</param>
        /// <returns>返回是否执行成功。</returns>
        Task<bool> SetAsync<T>(string key, T value);

        /// <summary>
        /// 异步获取一个对象。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="key">值。</param>
        /// <returns>返回对象的值。</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 异步获取一个字符串对象。
        /// </summary>
        /// <param name="key">值。</param>
        /// <returns>返回对象的值。</returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// 异步删除一个对象。
        /// </summary>
        /// <param name="key">键。</param>
        /// <returns>返回是否执行成功。</returns>
        Task<bool> DeleteAsync(string key);

        /// <summary>
        /// 异步设置一个键的过期时间。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>返回是否执行成功。</returns>
        Task<bool> SetExpireAsync(string key, int seconds);

        /// <summary>
        /// 异步设置一个键的过期时间。
        /// </summary>
        /// <param name="key">键。</param>
        /// <param name="expiry">过期时间（时间间隔）。</param>
        /// <returns>返回是否执行成功。</returns>
        Task<bool> SetExpireAsync(string key, TimeSpan? expiry);

        #region 分布式锁...

        /// <summary>
        /// 获取锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>是否已锁。</returns>
        bool Lock(string key, int seconds);

        /// <summary>
        /// 释放锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <returns>是否成功。</returns>
        bool UnLock(string key);

        /// <summary>
        /// 异步获取锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <param name="seconds">过期时间（秒）。</param>
        /// <returns>是否成功。</returns>
        Task<bool> LockAsync(string key, int seconds);

        /// <summary>
        /// 异步释放锁。
        /// </summary>
        /// <param name="key">锁名称。</param>
        /// <returns>是否成功。</returns>
        Task<bool> UnLockAsync(string key);

        #endregion

    }
```
### 服务注册和配置
```csharp
        services.AddTransient<IRedisClient, RedisClient>();
        services.AddTransient<IConnectionMultiplexer, ConnectionMultiplexer>();
        services.AddTransient<IConnectionMultiplexer>(a =>
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(redisConfig.url);
            options.Password = redisConfig.pass;
            string configuration = "{0},$UNLINK=,abortConnect=false,defaultDatabase={1},ssl=false,ConnectTimeout={2},allowAdmin=true,connectRetry={3},password={4}";
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(string.Format(configuration, redisConfig.url, 0, 1800, 3, redisConfig.pass));
            return connectionMultiplexer;
        });
```
# 使用
直接在使用的类里构造注入就可以使用了。
```csharp
    [SwaggerTag("User", Description = "用户管理")]
    [Authorize]
    public class UserController : ApiBaseController
    {
        readonly IUserService _userService;
        readonly IUserRoleService _userRoleService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRedisClient _redisClient;
        private const string keyPrefix = "token:";

        public UserController(IUserService userService, IUserRoleService userRoleService, ILogger<UserController> logger, IMapper mapper, IRedisClient redisClient)
        {
            _userService = userService;
            _logger = logger;
            _userRoleService = userRoleService;
            _mapper = mapper;
            _redisClient = redisClient;
        }
        /// <summary>
        /// 设置 Redis 数据。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("redis/set")]
        public ApiResult<Dictionary<string, bool>> SetRedisData(string key, string value)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(7 * 24 * 60 * 60);
            var flag = _redisClient.Set(key, value, timeSpan);
            Dictionary<string, bool> res = new Dictionary<string, bool>
            {
                { "ok", flag }
            };
            return ApiResult<Dictionary<string, bool>>.Current.UpdateSuccess(res);
        }

    }
```
关于锁的使用参考了 [axel10 大神的文章](https://www.cnblogs.com/axel10/p/9001800.html)，需要注意的是一定要禁用 `UNLINK`，不然会报 `StackExchange.Redis.RedisServerException:“EXECABORT Transaction discarded because of previous errors.”` 这个错误，`UNLINK` 需要 Redis 4.0 以上的版本才支持。