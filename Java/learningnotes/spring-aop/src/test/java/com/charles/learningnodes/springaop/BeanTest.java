package com.charles.learningnodes.springaop;

import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

public class BeanTest {
    public static void main(String[] args) {
        ApplicationContext applicationContext  = new ClassPathXmlApplicationContext("test.xml");
    }
}
