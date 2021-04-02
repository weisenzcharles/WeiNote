package org.charles.learning.threaddemo;

public class ThreadTest {

    public static void main(String[] args) {
        // 打印当前线程名称
        System.out.println("当前线程 Id：" + Thread.currentThread().getId());
        for (int i = 0; i < 10; i++) {
            Thread thread = new Thread(new PrintString("正在执行线程：", 100 * i));
            thread.start();
        }
        System.out.println("线程执行完毕，当前线程 Id：" + Thread.currentThread().getId());
    }

    static class PrintString implements Runnable {
        private String text;
        private long interval;

        public PrintString(String text, long interval) {
            this.text = text;
            this.interval = interval;
        }

        @Override
        public void run() {
//            System.out.println("当前线程 Id：" + Thread.currentThread().getId());
            try {
                Thread.sleep(interval);
                System.out.println(text + Thread.currentThread().getId());
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }
}
