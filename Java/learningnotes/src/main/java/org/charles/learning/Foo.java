package org.charles.learning;


//public class Foo {
//    public static void main(String[] args) {
//        boolean 吃过饭没 = 2; // 直接编译的话javac会报错
//        if (吃过饭没) System.out.println("吃了");
//        if (true == 吃过饭没) System.out.println("真吃了");
//    }
//}


import java.sql.Array;
import java.util.ArrayList;

public class Foo {
    static boolean boolValue;
    public static void main(String[] args) {

        ArrayList arrayList = new ArrayList();

        boolValue = true; // 将这个true替换为2或者3，再看看打印结果
        if (boolValue) System.out.println("Hello, Java!");
        if (boolValue == true) System.out.println("Hello, JVM!");
    }
}