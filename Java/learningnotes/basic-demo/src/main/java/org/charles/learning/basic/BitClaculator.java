package org.charles.learning.basic;

public class BitClaculator {
    public static void main(String[] args) {
        // 二进制 1111 0000
        int a = 0xF0;
        // 二进制 1111 0100
        int b = 0xF4;
        // 二进制 1111 1000
        int c = 0xF8;
        // 二进制 1111 1111
        int d = 0xFF;

        System.out.println("---------------- 原始数值 ----------------");
        System.out.println(a);
        System.out.println(b);
        System.out.println(c);
        System.out.println(d);

        System.out.println("---------------- 计算结果 ----------------");
        System.out.println(a ^ b);
        System.out.println(b | c);
        System.out.println(c & d);
        // 取反
        System.out.println(~d);
    }

}
