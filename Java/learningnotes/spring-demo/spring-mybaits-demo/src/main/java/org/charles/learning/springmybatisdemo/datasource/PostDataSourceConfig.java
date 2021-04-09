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