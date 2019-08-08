### 介绍
JPA（Java Persistence API）Java 持久化 API，是 Java 持久化的标准规范，Hibernate 是持久化规范的技术实现，而 Spring Data JPA 是在 Hibernate 基础上封装的一款框架。
第一次使用 Spring JPA 的时候，感觉这东西简直就是神器，几乎不需要写什么关于数据库访问的代码一个基本的 CURD 的功能就出来了。在这篇文章中，我们将介绍 Spring Boot 整合 JPA 使用多个数据源的方法。
开发环境：
- Spring Boot 2.0.5
- Spring Data JPA 2.0.5
- MySQL 5.6
- JDK 8
- IDEA 2018.3
- Windows 10
### 引入依赖
首先我们要 Spring Boot 引入 `spring-boot-starter-data-jpa` 依赖。

Maven 配置：
```xml
   <dependency>
        <groupId>org.springframework.boot</groupId>
        <artifactId>spring-boot-starter-data-jpa</artifactId>
    </dependency>
    <dependency>
        <groupId>org.springframework.boot</groupId>
        <artifactId>spring-boot-starter-web</artifactId>
    </dependency>
    <dependency>
        <groupId>org.springframework.boot</groupId>
        <artifactId>spring-boot-devtools</artifactId>
        <scope>runtime</scope>
    </dependency>
    <dependency>
        <groupId>mysql</groupId>
        <artifactId>mysql-connector-java</artifactId>
        <scope>runtime</scope>
    </dependency>
```
Gradle 配置：
```bash
compile group: 'org.springframework.boot', name: 'spring-boot-starter-data-jpa', version: '2.0.5.RELEASE'
compile group: 'org.springframework.boot', name: 'spring-boot-starter-web', version: '2.0.5.RELEASE'
compile group: 'org.springframework.boot', name: 'spring-boot-devtools', version: '2.0.5.RELEASE'
compile group: 'mysql', name: 'mysql-connector-java', version: '6.0.6'
```
### 配置数据源
Spring Boot 提供了使用 application.properties 或 application.yml 文件配置项目属性的方法。我比较习惯使用 application.yml 文件，所以这里我只列出 application.yml 文件的写法。
```bash
spring:
  datasource:
    product:
      driver-class-name: com.mysql.jdbc.Driver
      jdbc-url: jdbc:mysql://127.0.0.1:3306/product?useUnicode=true&characterEncoding=utf-8&serverTimezone=UTC&autoReconnect=true&useSSL=false&zeroDateTimeBehavior=convertToNull
      username: root
      password: test123$
    customer:
      driver-class-name: com.mysql.jdbc.Driver
      jdbc-url: jdbc:mysql://127.0.0.1:3306/customer?useUnicode=true&characterEncoding=utf-8&serverTimezone=UTC&autoReconnect=true&useSSL=false&zeroDateTimeBehavior=convertToNull
      username: root
      password: test123$
  jpa:
    generate-ddl: true
```
配置好 application.yml 文件后分别在数据库创建 customer 和 product 数据库。
### 添加实体（Entity）类
客户实体：
```java
package com.springboot.jpa.customer.models;

import javax.persistence.*;

@Entity
public class Customer {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;
    @Column(unique = true, nullable = false)
    private String email;
    private String firstName;
    private String lastName;

    protected Customer() {
    }

    public Customer(String email, String firstName, String lastName) {
        this.email = email;
        this.firstName = firstName;
        this.lastName = lastName;
    }

    @Override
    public String toString() {
        return String.format("Customer[id=%d, firstName='%s', lastName='%s',email='%s']", id, firstName, lastName, email);
    }

    public Integer getId() {
        return id;
    }

    public String getEmail() {
        return email;
    }

    public String getFirstName() {
        return firstName;
    }

    public String getLastName() {
        return lastName;
    }
}
```
产品实体：
```java
package com.springboot.jpa.product.models;

import javax.persistence.*;

@Entity
public class Product {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;

    @Column(nullable = false)
    private String code;
    private String name;
    private double price;


    protected Product() {
    }

    public Product(String code, String name, double price) {
        this.code = code;
        this.name = name;
        this.price = price;
    }

    @Override
    public String toString() {
        return String.format("Product[id=%d, code='%s', name='%s', price='%s']", id, code, name, price);
    }

    public int getId() {
        return id;
    }

    public String getCode() {
        return code;
    }

    public String getName() {
        return name;
    }

    public double getPrice() {
        return price;
    }
}

```
### 添加数据仓库（Repository）类
客户 Repository：
```java
package com.springboot.jpa.customer.repository;

import com.springboot.jpa.customer.models.Customer;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface CustomerRepository extends JpaRepository<Customer, Integer> {
}
```
产品 Repository：
```java
package com.springboot.jpa.product.repository;

import com.springboot.jpa.product.models.Product;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface ProductRepository extends JpaRepository<Product, Integer> {
}
```

### 添加配置（Config）类
客户配置：
```java
package com.springboot.jpa.customer.config;

import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.boot.jdbc.DataSourceBuilder;
import org.springframework.boot.orm.jpa.EntityManagerFactoryBuilder;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Primary;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;
import org.springframework.orm.jpa.JpaTransactionManager;
import org.springframework.orm.jpa.LocalContainerEntityManagerFactoryBean;
import org.springframework.transaction.PlatformTransactionManager;
import org.springframework.transaction.annotation.EnableTransactionManagement;

import javax.persistence.EntityManagerFactory;
import javax.sql.DataSource;

@Configuration
@EnableTransactionManagement
@EnableJpaRepositories(entityManagerFactoryRef = "customerEntityManagerFactory", transactionManagerRef = "customerTransactionManager", basePackages = {"com.springboot.jpa.customer.repository"})
public class CustomerConfig {

    @Primary
    @Bean(name = "customerDataSource")
    @ConfigurationProperties(prefix = "spring.datasource.customer")
    public DataSource customerDataSource() {
        return DataSourceBuilder.create().build();
    }

    @Primary
    @Bean(name = "customerEntityManagerFactory")
    public LocalContainerEntityManagerFactoryBean entityManagerFactory(EntityManagerFactoryBuilder builder, @Qualifier("customerDataSource") DataSource dataSource) {
        return builder.dataSource(dataSource).packages("com.springboot.jpa.customer.data").persistenceUnit("customer").build();
    }

    @Primary
    @Bean(name = "customerTransactionManager")
    public PlatformTransactionManager customerTransactionManager(@Qualifier("customerEntityManagerFactory") EntityManagerFactory customerEntityManagerFactory) {
        return new JpaTransactionManager(customerEntityManagerFactory);
    }
}
```
产品配置：
```java
package com.springboot.jpa.product.config;


import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.boot.jdbc.DataSourceBuilder;
import org.springframework.boot.orm.jpa.EntityManagerFactoryBuilder;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;
import org.springframework.orm.jpa.JpaTransactionManager;
import org.springframework.orm.jpa.LocalContainerEntityManagerFactoryBean;
import org.springframework.transaction.PlatformTransactionManager;
import org.springframework.transaction.annotation.EnableTransactionManagement;

import javax.persistence.EntityManagerFactory;
import javax.sql.DataSource;

@Configuration
@EnableTransactionManagement
@EnableJpaRepositories(entityManagerFactoryRef = "productEntityManagerFactory", transactionManagerRef = "productTransactionManager", basePackages = {"com.springboot.jpa.product.repository"}
)
public class ProductConfig {

    @Bean(name = "productDataSource")
    @ConfigurationProperties(prefix = "spring.datasource.product")
    public DataSource dataSource() {
        return DataSourceBuilder.create().build();
    }

    @Bean(name = "productEntityManagerFactory")
    public LocalContainerEntityManagerFactoryBean barEntityManagerFactory(EntityManagerFactoryBuilder builder, @Qualifier("productDataSource") DataSource dataSource) {
        return builder.dataSource(dataSource).packages("com.springboot.jpa.product.data").persistenceUnit("product").build();
    }

    @Bean(name = "productTransactionManager")
    public PlatformTransactionManager productTransactionManager(@Qualifier("productEntityManagerFactory") EntityManagerFactory productEntityManagerFactory) {
        return new JpaTransactionManager(productEntityManagerFactory);
    }
}
```
项目结构：
```bash
src/main/java
- com.springboot.jpa
      - product
        - config
        - models
        - repository
      - customer
        - config
        - models
        - repository
```
### 添加测试类
客户测试类 CustomerDataSourcesTests：
```java
package com.springboot.jpa;

import com.springboot.jpa.customer.repository.CustomerRepository;
import com.springboot.jpa.customer.models.Customer;
import org.junit.runner.RunWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.junit4.SpringRunner;
import org.springframework.transaction.annotation.Transactional;

import org.junit.Test;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertNotNull;

@RunWith(SpringRunner.class)
@SpringBootTest
public class CustomerDataSourcesTests {

    @Autowired
    private CustomerRepository customerRepository;

    @Test
    @Transactional("customerTransactionManager")
    public void createCustomer() {

        Customer customer = new Customer("master@weilog.net", "Charles", "Zhang");
        customer = customerRepository.save(customer);
        assertNotNull(customerRepository.findById(customer.getId()));
        assertEquals(customerRepository.findById(customer.getId()).get().getEmail(), "master@weilog.net");
    }
}
```
产品测试类 ProductDataSourcesTests：
```java
package com.springboot.jpa;

import com.springboot.jpa.product.models.Product;
import com.springboot.jpa.product.repository.ProductRepository;
import org.junit.runner.RunWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.context.junit4.SpringRunner;
import org.springframework.transaction.annotation.Transactional;

import org.junit.Test;

import static org.junit.Assert.assertNotNull;

@RunWith(SpringRunner.class)
@SpringBootTest
public class ProductDataSourcesTests {

    @Autowired
    private ProductRepository productRepository;

    @Test
    @Transactional("productTransactionManager")
    public void createProduct() {
        Product product = new Product("10000", "Book", 80.0);
        product = productRepository.save(product);

        assertNotNull(productRepository.findById(product.getId()));
    }

}
```
### 测试
分别运行两个测试类通过后，查询数据库。
客户表：
```bash
mysql> SELECT * FROM customermodel;
+----+-------------------+-----------+----------+
| id | email             | firstName | lastName |
+----+-------------------+-----------+----------+
|  1 | master@weilog.net | Charles   | Zhang    |
+----+-------------------+-----------+----------+
1 row in set
```
产品表：
```bash
mysql> SELECT * FROM productmodel;
+----+-------+------+-------+
| id | code  | name | price |
+----+-------+------+-------+
|  1 | 10000 | Book |    80 |
+----+-------+------+-------+
1 row in set
```