package org.charles.learning.threaddemo;

public class ThreadCall {

    public static void main(String[] args) {
        System.out.println("当前线程：" + Thread.currentThread().getId());
        method1();
    }

    public static void method1() {
        System.out.println("当前线程：" + Thread.currentThread().getId());
        method2();
    }

    public static void method2() {
        System.out.println("当前线程：" + Thread.currentThread().getId());
        method3();
    }

    public static void method3() {
        System.out.println("当前线程：" + Thread.currentThread().getId());
        method4();
    }

    public static void method4() {
        System.out.println("当前线程：" + Thread.currentThread().getId());
        method5();
    }

    public static void method5() {
        System.out.println("当前线程：" + Thread.currentThread().getId());
    }
}
