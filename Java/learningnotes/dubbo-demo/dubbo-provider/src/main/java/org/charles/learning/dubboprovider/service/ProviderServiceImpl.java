package org.charles.learning.dubboprovider.service;

import com.alibaba.dubbo.rpc.RpcContext;
import org.charles.learning.dubboservice.DubboService;
import org.springframework.stereotype.Service;

import java.text.SimpleDateFormat;
import java.util.Date;

@Service("demoService")
public class ProviderServiceImpl implements DubboService {

    @Override
    public String sayHello(String name) {
        System.out.println("[" + new SimpleDateFormat("HH:mm:ss").format(new Date()) + "] Hello " + name + ", request from consumer: " + RpcContext.getContext().getRemoteAddress());
        return "Hello " + name + ", response form provider: " + RpcContext.getContext().getLocalAddress();
    }
}