Redis（REmote DIctionary Server）是一个由 Salvatore Sanfilippo 写的 key-value 存储系统。Redis 是一个开源的使用 ANSI C 语言编写、遵守 BSD 协议、支持网络、可基于内存亦可持久化的日志型、Key-Value 数据库，并提供多种语言的 API。它通常被称为数据结构服务器，因为值（value）可以是 字符串（String）、哈希（Hash）, 列表（list）、集合（sets）和 有序集合（sorted sets）等类型。

首先为项目添加 Redis 支持。

Gradle：
```text
  // redis
  compile group: 'org.springframework.boot', name: 'spring-boot-starter-data-redis', version: '2.1.1.RELEASE'
  compile group: 'redis.clients', name: 'jedis', version: '3.0.0'
```

```xml
<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-data-redis</artifactId>
    <version>2.1.1.RELEASE</version>
</dependency>
<dependency>
    <groupId>redis.clients</groupId>
    <artifactId>jedis</artifactId>
    <version>3.0.0</version>
</dependency>
```

RedisConfig 类：
```java
package org.charles.learning.springredisdemo.core.config;

import com.fasterxml.jackson.annotation.JsonAutoDetect;
import com.fasterxml.jackson.annotation.PropertyAccessor;
import com.fasterxml.jackson.databind.ObjectMapper;

import java.time.Duration;
import java.util.HashMap;
import java.util.Map;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.cache.CacheManager;
import org.springframework.cache.annotation.CachingConfigurerSupport;
import org.springframework.cache.annotation.EnableCaching;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.redis.cache.RedisCacheConfiguration;
import org.springframework.data.redis.cache.RedisCacheManager;
import org.springframework.data.redis.cache.RedisCacheWriter;
import org.springframework.data.redis.connection.RedisConnectionFactory;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.data.redis.serializer.Jackson2JsonRedisSerializer;
import org.springframework.data.redis.serializer.RedisSerializationContext;
import org.springframework.data.redis.serializer.StringRedisSerializer;

import redis.clients.jedis.Jedis;
import redis.clients.jedis.JedisPool;
import redis.clients.jedis.JedisPoolConfig;


@Configuration
@EnableCaching
public class RedisConfig extends CachingConfigurerSupport {

  /**
   * RedisTemplate配置.
   */
  @Bean
  public RedisTemplate<String, Object> redisTemplate(RedisConnectionFactory factory) {
    RedisTemplate<String, Object> redisTemplate = new RedisTemplate();
    redisTemplate.setConnectionFactory(factory);
    Jackson2JsonRedisSerializer jackson2JsonRedisSerializer = this.getSerializer();
    // 设置key/value格式
    redisTemplate.setKeySerializer(new StringRedisSerializer());
    redisTemplate.setValueSerializer(jackson2JsonRedisSerializer);
    redisTemplate.setHashKeySerializer(new StringRedisSerializer());
    redisTemplate.setHashValueSerializer(jackson2JsonRedisSerializer);
    redisTemplate.afterPropertiesSet();
    return redisTemplate;
  }

  /**
   * 设置redis缓存策略.
   *
   * @param factory 链接工厂
   * @return
   */
  @Bean
  public CacheManager cacheManager(RedisConnectionFactory factory) {
    return new RedisCacheManager(
            RedisCacheWriter.nonLockingRedisCacheWriter(factory),
            // 默认策略，未配置的 key 会使用这个 8小时
            this.getRedisCacheConfigurationWithTtl(60 * 60 * 48)
    //this.getRedisCacheConfigurationMap() // 指定 key 策略
    );
  }

  private Map<String, RedisCacheConfiguration> getRedisCacheConfigurationMap() {

    Map<String, RedisCacheConfiguration> redisCacheConfigurationMap = new HashMap<>();
    redisCacheConfigurationMap.put("UserInfoList", this.getRedisCacheConfigurationWithTtl(3000));
    redisCacheConfigurationMap.put("UserInfoListAnother",
            this.getRedisCacheConfigurationWithTtl(18000));

    return redisCacheConfigurationMap;
  }

  private RedisCacheConfiguration getRedisCacheConfigurationWithTtl(Integer seconds) {

    Jackson2JsonRedisSerializer<Object> jackson2JsonRedisSerializer = this.getSerializer();
    RedisCacheConfiguration redisCacheConfiguration = RedisCacheConfiguration.defaultCacheConfig();
    redisCacheConfiguration = redisCacheConfiguration.serializeValuesWith(
            RedisSerializationContext
                    .SerializationPair
                    .fromSerializer(jackson2JsonRedisSerializer)
    ).entryTtl(Duration.ofSeconds(seconds));

    return redisCacheConfiguration;
  }

  /**
   * 配置序列化策略.
   * @return 策略
   */
  public Jackson2JsonRedisSerializer getSerializer() {
    Jackson2JsonRedisSerializer<Object> jackson2JsonRedisSerializer =
            new Jackson2JsonRedisSerializer<>(Object.class);
    ObjectMapper om = new ObjectMapper();
    om.setVisibility(PropertyAccessor.ALL, JsonAutoDetect.Visibility.ANY);
    om.enableDefaultTyping(ObjectMapper.DefaultTyping.NON_FINAL);
    jackson2JsonRedisSerializer.setObjectMapper(om);
    return jackson2JsonRedisSerializer;
  }

}
```
RedisService 类：
```java
package org.charles.learning.springredisdemo.core;

import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.concurrent.TimeUnit;
import javax.annotation.Resource;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.stereotype.Service;

@Service
public class RedisService {

  @Resource
  private RedisTemplate redisTemplate;

  /**
   * value设置.
   */
  public boolean set(String key, Object value) {
    boolean result = false;
    try {
      redisTemplate.opsForValue().set(key, value);
      result = true;
    } catch (Exception e) {
      e.printStackTrace();
    }
    return result;
  }

  /**
   * value设置时效时间.
   */
  public boolean set(String key, Object value, Long expireTime) {
    boolean result = false;
    try {
      redisTemplate.opsForValue().set(key, value);
      redisTemplate.expire(key, expireTime, TimeUnit.SECONDS);
      result = true;
    } catch (Exception e) {
      e.printStackTrace();
    }
    return result;
  }

  /**
   * value获取.
   */
  public Object get(String key) {
    return redisTemplate.opsForValue().get(key);
  }

  /**
   * 批量删除对应value.
   */
  public void remove(String... keys) {
    for (String key : keys) {
      if (exists(key)) {
        redisTemplate.delete(key);
      }
    }
  }

  /**
   * 判断缓存中是否有对应的value.
   */
  public boolean exists(String key) {
    return redisTemplate.hasKey(key);
  }

  /**
   * 哈希设置.
   */
  public void setHash(String key, Map<String, Object> value) {
    redisTemplate.opsForHash().putAll(key, value);
  }

  /**
   * 哈希获取.
   */
  public Map<Object, Object> getHash(String key) {
    return redisTemplate.opsForHash().entries(key);
  }

  /**
   * 列表设置.
   */
  public void setList(String key, List<Object> value) {
    redisTemplate.delete(key);
    redisTemplate.opsForList().leftPushAll(key, value);
  }

  /**
   * 列表获取.
   */
  public List<Object> getList(String key, int start, int end) {
    return redisTemplate.opsForList().range(key, start, end);
  }

  /**
   * 列表获取全部.
   */
  public List<Object> getList(String key) {
    return redisTemplate.opsForList().range(key, 0, -1);
  }

  /**
   * 集合设置.
   */
  public void setSet(String key, Set<Object> set) {
    redisTemplate.delete(key);
    redisTemplate.opsForSet().add(key, set);
  }

  /**
   * 集合获取.
   */
  public Set<Object> getSet(String key) {
    return redisTemplate.opsForSet().members(key);
  }

  /**
   * 有序集合设置.
   */
  public void setZSet(String key, Set<Object> set) {
    redisTemplate.delete(key);
    redisTemplate.opsForZSet().add(key, set);
  }

  /**
   * 有序集合获取.
   */
  public Set<Object> getZSet(String key, int start, int end) {
    return redisTemplate.opsForZSet().range(key, start, end);
  }

  /**
   * 有序集合获取全部.
   */
  public Set<Object> getZSet(String key) {
    return redisTemplate.opsForZSet().range(key, 0, -1);
  }
}

```
yml 配置：
```yml
spring:
  redis:
    database: 0
    host: 127.0.0.1
    port: 8739
    password: 123456
    timeout: 5000
    lettuce:
      shutdown-timeout: 200
      pool:
        max-active: 500
        max-idle: 100
        min-idle: 50
        max-wait: 2000
```
使用方法：
```java
package org.charles.learning.springredisdemo;

import static com.google.common.truth.Truth.assertThat;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.charles.learning.springredisdemo.core.RedisService;
import java.io.IOException;
import javax.annotation.Resource;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.junit4.SpringRunner;
import redis.clients.jedis.Jedis;

@RunWith(SpringRunner.class)
@SpringBootTest
public class RedisTests {

  @Autowired
  private RedisService redisService;
  @Resource
  private Jedis jedis;

  @Test
  public void testService() throws IllegalAccessException {
    redisService.set("name", "xyz");
    Object value = redisService.get("name");
    assertThat(value).isNotNull();
  }

  @Test
  public void testRedisMessage() throws IOException {
    String message = "{\"name\":\"xyz\"}";
    JsonNode jsonNode = new ObjectMapper().readTree(message);
    jedis.lpush("message-test", jsonNode.toString());
    String messages = jedis.rpop("message-test");
    assertThat(messages).isNotNull();
  }

}
```