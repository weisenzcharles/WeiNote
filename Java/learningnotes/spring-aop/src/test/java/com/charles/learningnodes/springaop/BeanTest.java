package com.charles.learningnodes.springaop;

import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

import java.util.ArrayList;

public class BeanTest {
    public static void main(String[] args) {
        ArrayList<Integer> li = new ArrayList<Integer>();
        ArrayList<Float> lf = new ArrayList<Float>();
        if (li.getClass() == lf.getClass()) { // 泛型擦除，两个 List 类型是一样的
            System.out.println("6666");
        }

        ArrayList arrayList = new ArrayList();

        arrayList.add("ss");

        ApplicationContext applicationContext  = new ClassPathXmlApplicationContext("test.xml");
    }
}
