package org.charles.learning.springdruiddemo.config;

import com.alibaba.druid.filter.stat.StatFilter;
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