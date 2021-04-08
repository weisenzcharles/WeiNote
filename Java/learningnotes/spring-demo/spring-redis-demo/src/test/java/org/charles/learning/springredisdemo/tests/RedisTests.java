package org.charles.learning.springredisdemo.tests;



import org.charles.learning.springredisdemo.service.RedisService;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;


import java.io.IOException;
import javax.annotation.Resource;

import org.junit.Test;
import org.junit.runner.RunWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.junit4.SpringRunner;
import redis.clients.jedis.Jedis;

import static org.assertj.core.api.AssertionsForClassTypes.assertThat;

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