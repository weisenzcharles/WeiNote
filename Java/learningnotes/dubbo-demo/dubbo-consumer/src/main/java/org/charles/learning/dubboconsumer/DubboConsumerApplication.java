package org.charles.learning.dubboconsumer;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.EnableAutoConfiguration;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import javax.annotation.Resource;
import java.io.IOException;

@SpringBootApplication
public class DubboConsumerApplication {

    public static void main(String[] args) {
        SpringApplication.run(DubboConsumerApplication.class, args);
    }


//    @RestController
//    public class DemoConsumerController {
//
//        @Resource
//        private DemoService demoService;
//
//        @RequestMapping("/sayHello")
//        public String sayHello(@RequestParam String name) {
//            return demoService.sayHello(name);
//        }
//    }
}