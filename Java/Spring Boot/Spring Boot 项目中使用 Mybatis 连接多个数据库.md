在项目开发中我们经常需要从多个数据库中查询数据，目前在 Java 领域应用比较广泛的就是 Mybatis，MyBatis 是一款优秀的持久层框架，它支持定制化 SQL、存储过程以及高级映射。MyBatis 避免了几乎所有的 JDBC 代码和手动设置参数以及获取结果集。MyBatis 可以使用简单的 XML 或注解来配置和映射原生信息，将接口和 Java 的 POJOs 映射成数据库中的记录。

下面我们就来看下在项目中使用 MyBatis 连接多个数据库的办法。

首先在 application.yml 中配置连接字符串，这里需要注意的是多数据库的写法和标准写法有点不一样：
```yaml
datasource:
    post:
      driver-class-name: com.microsoft.sqlserver.jdbc.SQLServerDriver
      jdbc-url: jdbc:sqlserver://test.charles.com:1433;DatabaseName=postdata;useunicode=true;characterEncoding=UTF-8
      username: testuser
      password: user123456
    user:
      driver-class-name: com.mysql.jdbc.Driver
      jdbc-url: jdbc:mysql://test.charles.com/userdata?useUnicode=true&characterEncoding=UTF-8&useSSL=false
      username: testuser
      password: user123456
```
数据库驱动：
```xml
<!--sqlserver-->
<dependency>
    <groupId>com.microsoft.sqlserver</groupId>
    <artifactId>sqljdbc4</artifactId>
    <version>4.0</version>
</dependency>
<!--mysql-->
<dependency>
    <groupId>mysql</groupId>
    <artifactId>mysql-connector-java</artifactId>
    <version>5.1.10</version>
</dependency>
```
 User 模块的数据库配置：
```java
package org.charles.learning.springmybatisdemo.datasource;

import org.apache.ibatis.session.SqlSessionFactory;
import org.mybatis.spring.SqlSessionFactoryBean;
import org.mybatis.spring.SqlSessionTemplate;
import org.mybatis.spring.annotation.MapperScan;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.boot.jdbc.DataSourceBuilder;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.io.support.PathMatchingResourcePatternResolver;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;

import javax.sql.DataSource;

@Configuration
@MapperScan(basePackages = "org.charles.learning.springmybatisdemo.mapper.user", sqlSessionTemplateRef = "userSqlSessionTemplate")
public class UserDataSourceConfig {

    @Bean(name = "userDataSource")
    @ConfigurationProperties(prefix = "spring.datasource.user")
    public DataSource userDataSource() {
        return DataSourceBuilder.create().build();
    }

    @Bean(name = "userSqlSessionFactory")
    public SqlSessionFactory userSqlSessionFactory(@Qualifier("userDataSource") DataSource dataSource) throws Exception {
        SqlSessionFactoryBean bean = new SqlSessionFactoryBean();
        bean.setDataSource(dataSource);
        bean.setMapperLocations(new PathMatchingResourcePatternResolver().getResources("classpath:mybatis/mapper/user/*.xml"));
        return bean.getObject();
    }

    @Bean(name = "userTransactionManager")
    public DataSourceTransactionManager userTransactionManager(@Qualifier("userDataSource") DataSource dataSource) {
        return new DataSourceTransactionManager(dataSource);
    }

    @Bean(name = "userSqlSessionTemplate")
    public SqlSessionTemplate userSqlSessionTemplate(@Qualifier("userSqlSessionFactory") SqlSessionFactory sqlSessionFactory) throws Exception {
        return new SqlSessionTemplate(sqlSessionFactory);
    }
}
```
 Post 模块的数据库配置：
```java
package org.charles.learning.springmybatisdemo.datasource;

import org.apache.ibatis.session.SqlSessionFactory;
import org.mybatis.spring.SqlSessionFactoryBean;
import org.mybatis.spring.SqlSessionTemplate;
import org.mybatis.spring.annotation.MapperScan;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.boot.jdbc.DataSourceBuilder;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.io.support.PathMatchingResourcePatternResolver;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;

import javax.sql.DataSource;

@Configuration
@MapperScan(basePackages = "org.charles.learning.springmybatisdemo.mapper.post", sqlSessionTemplateRef = "postSqlSessionTemplate")
public class PostDataSourceConfig {

    @Bean(name = "postDataSource")
    @ConfigurationProperties(prefix = "spring.datasource.post")
    public DataSource postDataSource() {
        return DataSourceBuilder.create().build();
    }

    @Bean(name = "postSqlSessionFactory")
    public SqlSessionFactory postSqlSessionFactory(@Qualifier("postDataSource") DataSource dataSource) throws Exception {
        SqlSessionFactoryBean bean = new SqlSessionFactoryBean();
        bean.setDataSource(dataSource);
        bean.setMapperLocations(new PathMatchingResourcePatternResolver().getResources("classpath:mybatis/mapper/post/*.xml"));
        return bean.getObject();
    }

    @Bean(name = "postTransactionManager")
    public DataSourceTransactionManager postTransactionManager(@Qualifier("postDataSource") DataSource dataSource) {
        return new DataSourceTransactionManager(dataSource);
    }

    @Bean(name = "postSqlSessionTemplate")
    public SqlSessionTemplate postSqlSessionTemplate(@Qualifier("postSqlSessionFactory") SqlSessionFactory sqlSessionFactory) throws Exception {
        return new SqlSessionTemplate(sqlSessionFactory);
    }
}
```
在项目的 mybatis/mapper/post/ 和 mybatis/mapper/user/ 创建对应的 mapper.xml 文件和对应的 Mapper 接口。

程序会根据配置自动扫描对应的目录和映射。