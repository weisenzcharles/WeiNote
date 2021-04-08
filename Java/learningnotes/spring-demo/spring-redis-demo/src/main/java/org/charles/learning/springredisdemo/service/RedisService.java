package org.charles.learning.springredisdemo.service;

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