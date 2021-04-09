package org.charles.learning.springcheduleddemo;

import java.text.SimpleDateFormat;
import java.util.Date;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.scheduling.annotation.EnableScheduling;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;

@Component
// 启用定时任务
@EnableScheduling
public class ScheduledTasks {

    private static final Logger log = LoggerFactory.getLogger(ScheduledTasks.class);

    private static final SimpleDateFormat dateFormat = new SimpleDateFormat("HH:mm:ss");

    // 每 5 秒执行一次任务。
    @Scheduled(cron = "0/5 * * * * ?")
    public void performingTasks() {
        log.info("执行定时任务时间：{}", dateFormat.format(new Date()));
    }
}