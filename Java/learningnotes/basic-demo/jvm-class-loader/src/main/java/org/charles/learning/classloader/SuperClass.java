package org.charles.learning.classloader;

/**
 通过子类引用父类的类变量不会触发子类的初始化操作
 */
public class SuperClass {

    public static String value = "superClass value";

    static {
        System.out.println("SuperClass init!");
    }
}

 class SubClass extends SuperClass implements SuperInter{

    static {
        System.out.println("SubClass init!");
    }
}

  interface  SuperInter{

}

class ClassLoaderTest {

    static {
        System.out.println("InitTest init!");// main 第一个初始化
    }

    public static void main(String[] args) {
        System.out.println(SubClass.value);
    }
}