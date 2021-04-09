Druid 是一个非常好用的数据库连接池，但是他的好并不止体现在作为一个连接池加快数据访问性能上和连接管理上，他带有一个强大的监控工具：Druid Monitor。不仅可以监控数据源和慢查询，还可以监控 Web 应用、URI 监控、Session 监控、Spring 监控。

### 1、引入依赖
在 Spring Boot 项目中加入 `druid-spring-boot-starter` 依赖
Maven：

```java
<dependency>
   <groupId>com.alibaba</groupId>
   <artifactId>druid-spring-boot-starter</artifactId>
   <version>1.1.16</version>
</dependency>
```

Gradle：
```java
compile 'com.alibaba:druid-spring-boot-starter:1.1.16'
```
### 2、配置 application.yml
```java
spring:
  datasource:
    druid:
      driver-class-name: com.mysql.cj.jdbc.Driver
      url: jdbc:mysql://127.0.0.1:3306/test?useUnicode=true&characterEncoding=utf-8&serverTimezone=UTC&autoReconnect=true&useSSL=false&zeroDateTimeBehavior=convertToNull
      username: root
      password: test123$
      # 下面为连接池的补充设置，应用到上面所有数据源中
      # 初始化大小，最小，最大
      initialSize: 1
      minIdle: 3
      maxActive: 20
      # 配置获取连接等待超时的时间
      maxWait: 60000
      # 配置间隔多久才进行一次检测，检测需要关闭的空闲连接，单位是毫秒
      timeBetweenEvictionRunsMillis: 60000
      # 配置一个连接在池中最小生存的时间，单位是毫秒
      minEvictableIdleTimeMillis: 30000
      validationQuery: select 'x'
      testWhileIdle: true
      testOnBorrow: false
      testOnReturn: false
      # 打开 PSCache，并且指定每个连接上 PSCache 的大小
      poolPreparedStatements: true
      maxPoolPreparedStatementPerConnectionSize: 20
      # 配置监控统计拦截的 filters，去掉后监控界面 sql 无法统计，'wall'用于防火墙
      filters: stat,wall,slf4j
      # 通过 connectProperties 属性来打开 mergeSql 功能；慢 SQL 记录
      connectionProperties: druid.stat.mergeSql=true;druid.stat.slowSqlMillis=5000
      # 合并多个 DruidDataSource 的监控数据
      useGlobalDataSourceStat: true
    type: com.alibaba.druid.pool.DruidDataSource
```
属性说明：

> spring.datasource.druid.max-active # 最大连接数
> spring.datasource.druid.initial-size # 初始化大小
> spring.datasource.druid.min-idle # 最小连接数
> spring.datasource.druid.max-wait # 获取连接等待超时时间
> spring.datasource.druid.time-between-eviction-runs-millis # 间隔多久才进行一次检测，检测需要关闭的空闲连接，单位是毫秒
> spring.datasource.druid.min-evictable-idle-time-millis # 一个连接在池中最小生存的时间，单位是毫秒
> spring.datasource.druid.filters=config,stat,wall,log4j  # 配置监控统计拦截的 filters，去掉后监控界面 SQL 无法进行统计，wall 用于防火墙

### 3、配置 DruidMonitorConfig 类
DruidMonitorConfig 类：
```java

import com.alibaba.druid.support.http.StatViewServlet;
import com.alibaba.druid.support.http.WebStatFilter;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.boot.web.servlet.FilterRegistrationBean;
import org.springframework.boot.web.servlet.ServletRegistrationBean;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import com.alibaba.druid.spring.boot.autoconfigure.DruidDataSourceBuilder;
import com.alibaba.druid.support.http.StatViewServlet;
import com.alibaba.druid.support.http.WebStatFilter;
import org.springframework.context.annotation.Primary;

import javax.sql.DataSource;

@Configuration
public class DruidMonitorConfig {

    @Primary
    @Bean
    @ConfigurationProperties("spring.datasource.druid")
    public DataSource druidDataSource() {
        return DruidDataSourceBuilder.create().build();
    }

    /**
     * 注册 ServletRegistrationBean
     *
     * @return
     */
    @Bean
    public ServletRegistrationBean registrationBean() {
        ServletRegistrationBean bean = new ServletRegistrationBean(new StatViewServlet(), "/druid/*");
        /** 初始化参数配置，initParams**/
        // 白名单
        bean.addInitParameter("allow", "127.0.0.1");// 多个 ip 逗号隔开
        // IP 黑名单 (存在共同时，deny 优先于 allow) : 如果满足 deny 的话提示:Sorry, you are not permitted to view this page.
        // bean.addInitParameter("deny", "192.168.1.73");
        // 登录查看信息的账号密码.
        bean.addInitParameter("loginUsername", "admin");
        bean.addInitParameter("loginPassword", "123456");
        // 是否能够重置数据.
        bean.addInitParameter("resetEnable", "true");
        return bean;
    }

    /**
     * 注册 FilterRegistrationBean
     *
     * @return
     */
    @Bean
    public FilterRegistrationBean druidStatFilter() {
        FilterRegistrationBean bean = new FilterRegistrationBean(new WebStatFilter());
        //添加过滤规则.
        bean.addUrlPatterns("/*");
        //添加不需要忽略的格式信息.
        bean.addInitParameter("exclusions", "*.js,*.gif,*.jpg,*.png,*.css,*.ico,/druid/*");
        return bean;
    }

}
```
到此 Spring Boot 项目集成 Druid 监控完成了，启动 Spring Boot 应用程序，打开浏览器，输入：http://localhost:8080/druid/index.html， 登录后即可看到 Druid 的监控界面。