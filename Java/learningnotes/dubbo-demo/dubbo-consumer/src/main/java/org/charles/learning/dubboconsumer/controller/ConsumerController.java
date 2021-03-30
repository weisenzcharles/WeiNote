package org.charles.learning.dubboconsumer.controller;

import org.charles.learning.dubboservice.DubboService;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import javax.annotation.Resource;

@RestController
public class ConsumerController {

    @Resource
    private DubboService dubboService;

    @RequestMapping("/sayHello")
    public String sayHello(@RequestParam String name) {
        return dubboService.sayHello(name);
    }
}