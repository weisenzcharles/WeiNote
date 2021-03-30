package org.charles.learning.dubboconsumer.service;

import org.charles.learning.dubboservice.DubboService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;


@Service("consumerService")
public class ConsumerService {

    @Autowired
    private DubboService dubboService;

    public void sayHello(String name) {
        String hello = dubboService.sayHello(name); // 执行消费远程方法
        System.out.println(hello); // 显示调用结果
    }

}
