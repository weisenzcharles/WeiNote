在项目开发中我们经常需要一些定时任务来处理一些特殊的任务，比如定时检查订单的状态、定时同步数据等等。

在 Spring Boot 中使用 @Scheduled 注解创建定时任务非常简单，只需要两步操作就可以创建一个定时任务：

> 1、在定时任务类上增加 `@EnableScheduling` 注解
> 2、在要执行任务的方法上增加 `@Scheduled` 注解

下面是我使用 cron 表达式创建一个简单的定时任务：
```Java
import java.text.SimpleDateFormat;
import java.util.Date;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
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
```
启动运行项目以后控制台会每隔五秒打印一条记录。
```Bash
2019-07-25 11:12:00.343  INFO 15988 --- [pool-1-thread-1] c.m.s.s.a.controller.ScheduledTasks      : 执行定时任务时间：11:12:00
2019-07-25 11:12:05.343  INFO 15988 --- [pool-1-thread-1] c.m.s.s.a.controller.ScheduledTasks      : 执行定时任务时间：11:12:05
2019-07-25 11:12:10.343  INFO 15988 --- [pool-1-thread-1] c.m.s.s.a.controller.ScheduledTasks      : 执行定时任务时间：11:12:10
2019-07-25 11:12:15.343  INFO 15988 --- [pool-1-thread-1] c.m.s.s.a.controller.ScheduledTasks      : 执行定时任务时间：11:12:15
2019-07-25 11:12:20.343  INFO 15988 --- [pool-1-thread-1] c.m.s.s.a.controller.ScheduledTasks      : 执行定时任务时间：11:12:20
2019-07-25 11:12:25.342  INFO 15988 --- [pool-1-thread-1] c.m.s.s.a.controller.ScheduledTasks      : 执行定时任务时间：11:12:25
```
一些常用的 Cron 表达式例子

| 表达式 | 说明 |
| ---- | ---- |
| 0/2 * * * * ? | 表示每 2 秒执行任务 |
| 0 0/2 * * * ? | 表示每 2 分钟执行任务 |
| 0 0 2 1 * ? | 表示在每月的 1 日的凌晨 2 点调整任务 |
| 0 15 10 ? * MON-FRI | 表示周一到周五每天上午 10:15 执行作业 |
| 0 15 10 ? 6L 2002-2006 | 表示 2002-2006 年的每个月的最后一个星期五上午 10:15 执行作 |
| 0 0 10,14,16 * * ? | 每天上午 10 点，下午 2 点，4 点  |
| 0 0/30 9-17 * * ? | 朝九晚五工作时间内每半小时  |
| 0 0 12 ? * WED | 表示每个星期三中午 12 点  |
| 0 0 12 * * ? | 每天中午 12 点触发  |
| 0 15 10 ? * * | 每天上午 10:15 触发  |
| 0 15 10 * * ? | 每天上午 10:15 触发  |
| 0 15 10 * * ? | 每天上午 10:15 触发  |
| 0 15 10 * * ? 2005 | 2005 年的每天上午 10:15 触发  |
| 0 * 14 * * ? | 在每天下午 2 点到下午 2:59 期间的每 1 分钟触发 |
| 0 0/5 14 * * ? | 在每天下午 2 点到下午 2:55 期间的每 5 分钟触发 |
| 0 0/5 14,18 * * ? | 在每天下午 2 点到 2:55 期间和下午 6 点到 6:55 期间的每 5 分钟触发 |
| 0 0-5 14 * * ? | 在每天下午 2 点到下午 2:05 期间的每 1 分钟触发 |
| 0 10,44 14 ? 3 WED | 每年三月的星期三的下午 2:10 和 2:44 触发 |
| 0 15 10 ? * MON-FRI | 周一至周五的上午 10:15 触发  |
| 0 15 10 15 * ? | 每月 15 日上午 10:15 触发  |
| 0 15 10 L * ? | 每月最后一日的上午 10:15 触发  |
| 0 15 10 ? * 6L | 每月的最后一个星期五上午 10:15 触发  |
| 0 15 10 ? * 6L 2002-2005 | 2002 年至 2005 年的每月的最后一个星期五上午 10:15 触发 |
| 0 15 10 ? * 6#3 | 每月的第三个星期五上午 10:15 触发 |

`@Scheduled` 除了支持 cron 表达式以外还有很多的其他使用方法：

```Java
// 上一次开始执行时间点后 1 秒再次执行。
@Scheduled(fixedRate = 1000)

// 上一次执行完毕时间点后 1 秒再次执行。
@Scheduled(fixedDelay = 1000)

// 第一次延迟 2 秒执行，然后在上一次执行完毕时间点后 1 秒再次执行。
@Scheduled(initialDelay = 2000, fixedDelay = 1000)
```

网上也有一些 Cron 测试工具可以验证定时任务的执行时间：
1、https://tool.lu/crontab/
2、http://cron.qqe2.com/