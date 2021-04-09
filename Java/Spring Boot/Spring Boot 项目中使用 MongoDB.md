MongoDB 是一个基于分布式文件存储的数据库。由 C++ 语言编写。旨在为 WEB 应用提供可扩展的高性能数据存储解决方案。MongoDB 是一个介于关系数据库和非关系数据库之间的产品，是非关系数据库当中功能最丰富，最像关系数据库的。

首先为项目添加 MongoDB 支持。

Gradle：
```text
compile("org.springframework.boot:spring-boot-starter-data-mongodb:2.0.5.RELEASE")
compile("org.mongodb:mongo-java-driver:3.6.4")
```
Maven：
```xml
<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-data-mongodb</artifactId>
    <version>2.0.5.RELEASE</version>
</dependency>
<dependency>
    <groupId>org.mongodb</groupId>
    <artifactId>mongo-java-driver</artifactId>
    <version>3.6.4</version>
</dependency>
```
MongoTools 类：
```java
package org.charles.learning.springmongodemo.util;

import com.mongodb.MongoClient;
import com.mongodb.MongoCredential;
import com.mongodb.ServerAddress;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoDatabase;
import java.util.ArrayList;
import java.util.List;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Configuration;

@Configuration
public class MongoTools {

  // mongo环境变量
  private static String host;
  private static String port;
  private static String dbname;
  private static String username;
  private static String password;

  public static String getHost() {
    return host;
  }

  @Value("${spring.data.mongodb.host}")
  public void setHost(String host) {
    MongoTools.host = host;
  }

  public static String getPort() {
    return port;
  }

  @Value("${spring.data.mongodb.port}")
  public void setPort(String port) {
    MongoTools.port = port;
  }

  public static String getDbname() {
    return dbname;
  }

  @Value("${spring.data.mongodb.database}")
  public void setDbname(String dbname) {
    MongoTools.dbname = dbname;
  }

  public static String getUsername() {
    return username;
  }

  @Value("${spring.data.mongodb.username}")
  public void setUsername(String username) {
    MongoTools.username = username;
  }

  public static String getPassword() {
    return password;
  }

  @Value("${spring.data.mongodb.password}")
  public void setPassword(String password) {
    MongoTools.password = password;
  }

  private static MongoClient client = null;
  private static MongoDatabase db = null;
  private static MongoCollection collection;

  public MongoTools() {
  }

  /**
   * 获取数据库.
   * @return
   */
  public static MongoDatabase getDb() {
    try {
      if (client == null) {
        init();
      }
      //连接到数据库
      db = client.getDatabase(dbname);
    } catch (Exception e) {
      e.printStackTrace();
    }
    return db;
  }

  private static void init() {
    System.out.println("com.mongodb.client.MongoCollection");
    ServerAddress sa = new ServerAddress(host, Integer.parseInt(port));
    List<ServerAddress> sas = new ArrayList<>();
    sas.add(sa);
    MongoCredential mc = MongoCredential.createCredential(username, dbname, password.toCharArray());
    List<MongoCredential> mcs = new ArrayList<>();
    mcs.add(mc);
    //连接服务
    client = new MongoClient(sas, mcs);
  }

  /**
   * 建立连接.
   * @param collectionName 数据库
   * @return
   */
  public static MongoCollection getCollection(String collectionName) {
    getDb();
    collection = db.getCollection(collectionName);
    return collection;
  }
}
```
MongoService 类：
```java
package org.charles.learning.springmongodemo.core;


import com.mongodb.MongoNamespace;
import com.mongodb.client.FindIterable;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.MongoCursor;
import com.mongodb.client.model.Collation;
import org.charles.learning.springmongodemo.util.MongoTools;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;
import javax.annotation.Resource;

import org.bson.Document;
import org.springframework.data.domain.Sort;
import org.springframework.data.domain.Sort.Direction;
import org.springframework.data.domain.Sort.Order;
import org.springframework.data.mongodb.core.MongoTemplate;
import org.springframework.data.mongodb.core.mapreduce.GroupBy;
import org.springframework.data.mongodb.core.mapreduce.GroupByResults;
import org.springframework.data.mongodb.core.query.Criteria;
import org.springframework.data.mongodb.core.query.Update;
import org.springframework.stereotype.Service;


@Service
public class MongoService {

    @Resource
    private MongoTemplate mongoTemplate;

    /**
     * 根据key/value计算符合条件的数据数量.
     *
     * @param provalues      value
     * @param collectionName 集合名
     */
    public long countBy(String[] prokeys, Object[] provalues, String collectionName) {
        org.springframework.data.mongodb.core.query.Query query =
                new org.springframework.data.mongodb.core.query.Query();
        Criteria criteria = new Criteria();
        Criteria[] criterias = new Criteria[prokeys.length];
        for (int i = 0; i < prokeys.length; i++) {
            criterias[i] = Criteria.where(prokeys[i].toString()).is(provalues[i]);
        }
        criteria.andOperator(criterias);
        query.addCriteria(criteria);
        long result = mongoTemplate.count(query, collectionName);
        return result;
    }

    /**
     * 查询大于指定条件的数据量.
     *
     * @param prokeys           key
     * @param provalues         value
     * @param greaterThanKeys   大于条件的key
     * @param greaterThanValues 大于条件的value
     * @param collectionName    集合名
     */
    public long countByGt(String[] prokeys, Object[] provalues,
                          String[] greaterThanKeys,
                          Object[] greaterThanValues, String collectionName) {
        org.springframework.data.mongodb.core.query.Query query =
                new org.springframework.data.mongodb.core.query.Query();
        for (int i = 0; i < prokeys.length; i++) {
            query.addCriteria(Criteria.where(prokeys[i]).is(provalues[i]));
        }
        for (int i = 0; i < greaterThanKeys.length; i++) {
            query.addCriteria(Criteria.where(greaterThanKeys[i]).gt(greaterThanValues[i]));
        }
        return mongoTemplate.count(query, collectionName);
    }

    /**
     * 根据key/value查找符合条件的一条数据.
     *
     * @param prokeys        key
     * @param provalues      value
     * @param collectionName 集合名
     * @param orderBy        排序字段
     * @param order          1：升序 -1：降序
     */
    public Map findOne(String[] prokeys, Object[] provalues, String collectionName,
                       String orderBy, Integer order) {
        MongoCollection coll = MongoTools.getCollection(collectionName);
        Map m = new HashMap<>();
        Document doc = new Document();
        for (int i = 0; i < prokeys.length; i++) {
            if (i == 0) {
                doc.put(prokeys[i], provalues[i]);
            } else {
                doc.append(prokeys[i], provalues[i]);
            }
        }
        Document sort = new Document();
        if (orderBy != null) {
            sort = new Document(orderBy, order);
        }
        FindIterable iter = coll.find(doc).skip(0).limit(1).sort(sort);
        MongoCursor cursor = iter.iterator();
        if (cursor.hasNext()) {
            m = (Map) cursor.next();
            m.remove("_id");
        }
        return m;
    }

    /**
     * 根据key/value获取bean.
     *
     * @param querys         key/value
     * @param collectionName 集合名
     */
    public Object findOne(Map<String, Object> querys, String collectionName, Class clazz) {
        org.springframework.data.mongodb.core.query.Query query =
                new org.springframework.data.mongodb.core.query.Query();
        for (Map.Entry e : querys.entrySet()) {
            query.addCriteria(Criteria.where(e.getKey().toString()).is(e.getValue()));
        }
        return mongoTemplate.findOne(query, clazz, collectionName);
    }

    /**
     * 根据key/value获取bean.
     *
     * @param querys         key/value
     * @param collectionName 集合名
     */
    public <T> T findEntity(Map<String, Object> querys, String collectionName, Class<T> clazz) {
        org.springframework.data.mongodb.core.query.Query query =
                new org.springframework.data.mongodb.core.query.Query();
        for (Map.Entry e : querys.entrySet()) {
            query.addCriteria(Criteria.where(e.getKey().toString()).is(e.getValue()));
        }
        return mongoTemplate.findOne(query, clazz, collectionName);
    }

    /**
     * 根据key/value查找符合条件的一条数据（取出指定字段）.
     *
     * @param prokeys        key
     * @param provalues      value
     * @param includeFields  指定字段集合
     * @param collectionName 集合名
     */
    public Map findOneFilterFeilds(String[] prokeys, Object[] provalues, String[] includeFields,
                                   String collectionName) {
        MongoCollection coll = MongoTools.getCollection(collectionName);
        Map m = new HashMap<>();
        Document doc = new Document();
        Document filter = new Document();
        for (int i = 0; i < prokeys.length; i++) {
            if (i == 0) {
                doc.put(prokeys[i], provalues[i]);
            } else {
                doc.append(prokeys[i], provalues[i]);
            }
        }
        for (int i = 0; i < includeFields.length; i++) {
            filter.append(includeFields[i], true);
        }
        FindIterable iter = coll.find(doc).projection(filter).skip(0).limit(1);
        MongoCursor cursor = iter.iterator();
        if (cursor.hasNext()) {
            m = (Map) cursor.next();
            m.remove("_id");
        }
        return m;
    }

    /**
     * 指定key/value分页取出数据.
     *
     * @param prokeys        key
     * @param provalues      value
     * @param collectionName 集合名
     * @param skip           跳过前面n条数据
     * @param size           取m条数据
     * @param orderBy        排序字段
     * @param order          1：升序 -1：降序
     */
    public List<Map<String, Object>> findByPage(String[] prokeys, Object[] provalues,
                                                String collectionName,
                                                int skip, int size, String orderBy, Integer order) {
        MongoCollection coll = MongoTools.getCollection(collectionName);
        List<Map<String, Object>> list = new ArrayList<>();
        Document doc = new Document();
        if (prokeys != null && provalues != null) {
            for (int i = 0; i < prokeys.length; i++) {
                doc.append(prokeys[i], provalues[i]);
            }
        }
        long totalCount = coll.count();
        FindIterable iter = coll.find(doc).skip(skip).limit(size).sort(new Document(orderBy, order));
        MongoCursor cursor = iter.iterator();
        while (cursor.hasNext()) {
            Map m = (Map) cursor.next();
            m.remove("_id");
            list.add(m);
        }
        return list;
    }

    /**
     * 指定key/value分页取出数据.
     *
     * @param filter         筛选条件
     * @param collectionName 集合名
     * @param skip           跳过前面n条数据
     * @param size           取m条数据
     * @param orderBy        排序字段
     * @param order          1：升序 -1：降序
     */
    public Map<String, Object> findByPage(Map<String, Object> filter, Map<String, Object> childFilter,
                                          String collectionName, int skip, int size,
                                          String orderBy, Integer order) {
        MongoCollection coll = MongoTools.getCollection(collectionName);
        List<Map<String, Object>> list = new ArrayList<>();
        Document doc = new Document();
        List<Document> docList = new ArrayList<Document>();
        Set<String> keySet = filter.keySet();
        for (String key : keySet) {
            docList.add(new Document(key, filter.get(key)));
        }
        Collation.Builder numericOrdering = Collation.builder().locale("zh").numericOrdering(true);
        Collation collation = numericOrdering.build();
        FindIterable<Document> findIterable = coll.find(new Document("$and", docList)).skip(skip)
                .limit(size).collation(collation).sort(new Document(orderBy, order));
        long totalCount = coll.count(new Document("$and", docList));
        if (childFilter.size() != 0) {
            // 子级查找
            List<Document> childList = new ArrayList<>();
            Set<String> childKeySet = childFilter.keySet();
            for (String key : childKeySet) {
                childList.add(new Document(key, childFilter.get(key)));
            }
            findIterable = coll.find(new Document("$and", childList)).skip(skip)
                    .limit(size).collation(collation).sort(new Document(orderBy, order));
            totalCount = coll.count(new Document("$and", childList));
        }
        MongoCursor cursor = findIterable.iterator();
        while (cursor.hasNext()) {
            Map m = (Map) cursor.next();
            m.remove("_id");
            m.remove("_class");
            list.add(m);
        }
        Map<String, Object> totalRecords = new HashMap();
        totalRecords.put("paperList", list);
        totalRecords.put("totalRecords", totalCount);
        return totalRecords;
    }

    /**
     * 指定key/value分页取出数据.
     */
    public <T> List<T> findByPage(int pageNum, int size, String collectionName, Class<T> clazz) {
        if (pageNum < 1) {
            pageNum = 1;
        }
        org.springframework.data.mongodb.core.query.Query query =
                new org.springframework.data.mongodb.core.query.Query();
        query.skip((pageNum - 1) * size);
        query.limit(size);
        return mongoTemplate.find(query, clazz, collectionName);
    }

    /**
     * 指定key/value取出所有数据，使用in条件，传入Object[]类型数组.
     *
     * @param prokeys        key
     * @param provalues      value
     * @param collectionName 集合名
     * @param orderBy        排序字段
     * @param order          1：升序 -1：降序
     */
    public List<Map> findAllWithIn(String[] prokeys, Object[] provalues,
                                   String collectionName, String orderBy, Integer order) {
        org.springframework.data.mongodb.core.query.Query query =
                new org.springframework.data.mongodb.core.query.Query();
        for (int i = 0; i < prokeys.length; i++) {
            if (provalues[i] instanceof Object[]) {
                query.addCriteria(Criteria.where(prokeys[i]).in((Object[]) provalues[i]));
            } else {
                query.addCriteria(Criteria.where(prokeys[i]).is(provalues[i]));
            }
        }
        if (orderBy != null && order != null) {
            Direction direction;
            if (order < 0) {
                direction = Direction.DESC;
            } else {
                direction = Direction.ASC;
            }
            query.with(new Sort(new Order(direction, orderBy)));
        }
        return mongoTemplate.find(query, Map.class, collectionName);
    }

    /**
     * 指定key/value取出所有数据，使用in条件，传入Object[]类型数组.
     *
     * @param prokeys        key
     * @param provalues      value
     * @param collectionName 集合名
     * @param orderBy        排序字段
     * @param order          1：升序 -1：降序
     */
    public List findAllWithIn(String[] prokeys, Object[] provalues,
                              String collectionName, Class entityClass,
                              String orderBy, Integer order) {
        org.springframework.data.mongodb.core.query.Query query =
                new org.springframework.data.mongodb.core.query.Query();
        for (int i = 0; i < prokeys.length; i++) {
            if (provalues[i] instanceof Object[]) {
                query.addCriteria(Criteria.where(prokeys[i]).in((Object[]) provalues[i]));
            } else {
                query.addCriteria(Criteria.where(prokeys[i]).is(provalues[i]));
            }
        }
        if (orderBy != null && order != null) {
            Direction direction;
            if (order < 0) {
                direction = Direction.DESC;
            } else {
                direction = Direction.ASC;
            }
            query.with(new Sort(new Order(direction, orderBy)));
        }
        return mongoTemplate.find(query, entityClass, collectionName);
    }

    /**
     * 指定key/value取出所有数据.
     *
     * @param prokeys        key
     * @param provalues      value
     * @param collectionName 集合名
     * @param orderBy        排序字段
     * @param order          1：升序 -1：降序
     */
    public List<Map<String, Object>> findAll(String[] prokeys, Object[] provalues,
                                             String collectionName, String orderBy, Integer order) {
        MongoCollection coll = MongoTools.getCollection(collectionName);
        List<Map<String, Object>> list = new ArrayList<>();
        Document doc = new Document();
        for (int i = 0; i < prokeys.length; i++) {
            doc.append(prokeys[i], provalues[i]);
        }
        FindIterable iter = coll.find(doc).sort(new Document(orderBy, order));
        MongoCursor cursor = iter.iterator();
        while (cursor.hasNext()) {
            Map m = (Map) cursor.next();
            m.remove("_id");
            list.add(m);
        }
        return list;
    }

    /**
     * 指定key/value取出所有数据.
     *
     * @param filter         筛选条件
     * @param collectionName 集合名
     * @param orderBy        排序字段
     * @param order          1：升序 -1：降序
     */
    public List<Map<String, Object>> findAll(Map<String, Object> filter,
                                             Map<String, Object> childFilter,
                                             String collectionName, String orderBy, Integer order) {
        MongoCollection coll = MongoTools.getCollection(collectionName);
        List<Map<String, Object>> list = new ArrayList<>();
        List<Document> docList = new ArrayList<>();
        Set<String> keySet = filter.keySet();
        for (String key : keySet) {
            docList.add(new Document(key, filter.get(key)));
        }
        Collation.Builder numericOrdering = Collation.builder().locale("zh").numericOrdering(true);
        Collation collation = numericOrdering.build();
        FindIterable<Document> findIterable = coll.find(new Document("$and", docList))
                .collation(collation).sort(new Document(orderBy, order));
        if (childFilter.size() != 0) {
            // 子级查找
            List<Document> childList = new ArrayList<>();
            Set<String> childKeySet = childFilter.keySet();
            for (String key : childKeySet) {
                childList.add(new Document(key, childFilter.get(key)));
            }
            findIterable = coll.find(new Document("$and", childList)).collation(collation)
                    .sort(new Document(orderBy, order));
        }
        MongoCursor cursor = findIterable.iterator();
        while (cursor.hasNext()) {
            Map m = (Map) cursor.next();
            m.remove("_id");
            m.remove("_class");
            list.add(m);
        }
        return list;
    }

    /**
     * 指定key/value取出所有数据（指定字段且多排序）.
     *
     * @param prokeys        key
     * @param provalues      value
     * @param includeFields  指定字段
     * @param collectionName 集合名
     * @param orderBys       排序字段
     * @param orders         1：升序 -1：降序
     */
    public List<Map<String, Object>> findAllByArrOrderAndProjection(
            String[] prokeys, Object[] provalues, String[] includeFields,
            String collectionName, String[] orderBys,
            Integer[] orders) {
        List<Map<String, Object>> list = new ArrayList<>();
        Document doc = new Document();
        Document filter = new Document();
        Document order = new Document();
        for (int i = 0; i < prokeys.length; i++) {
            doc.append(prokeys[i], provalues[i]);
        }
        for (int i = 0; i < orderBys.length; i++) {
            order.append(orderBys[i], orders[i]);
        }
        for (int i = 0; i < includeFields.length; i++) {
            filter.append(includeFields[i], true);
        }
        MongoCollection coll = MongoTools.getCollection(collectionName);
        FindIterable iter = coll.find(doc).projection(filter).sort(order);
        MongoCursor cursor = iter.iterator();
        while (cursor.hasNext()) {
            Map m = (Map) cursor.next();
            m.remove("_id");
            list.add(m);
        }
        return list;
    }

    /**
     * 根据key/value获取bean.
     *
     * @param prokeys        key
     * @param provalues      value
     * @param collectionName 集合名
     * @param countField     累计字段
     * @param groupBy        分组字段
     */
    public List<Map<String, Object>> findAllByGroup(
            String[] prokeys, Object[] provalues, String collectionName,
            String countField, String groupBy) {
        Criteria criteria = new Criteria();
        Criteria[] criterias = new Criteria[prokeys.length];
        for (int i = 0; i < prokeys.length; i++) {
            criterias[i] = Criteria.where(prokeys[i].toString()).is(provalues[i]);
        }
        if (prokeys.length > 0) {
            criteria.andOperator(criterias);
        }
        GroupBy gb = new GroupBy(groupBy);
        if (countField != null && countField != "") {
            gb = gb.initialDocument("{count: 0}")
                    .reduceFunction("function (doc,pre){pre.count += doc." + countField + ";}");
        }
        GroupByResults groupByResults = mongoTemplate.group(criteria, collectionName, gb, Map.class);
        return (List<Map<String, Object>>) groupByResults.getRawResults().get("retval");
    }

    /**
     * 添加/更新符合条件的数据.
     *
     * @param querys         查询条件集合
     * @param updates        更新条件集合
     * @param collectionName 集合名
     */
    public void upsert(Map<String, Object> querys, Map<String, Object> updates,
                       String collectionName) {
        org.springframework.data.mongodb.core.query.Query query =
                new org.springframework.data.mongodb.core.query.Query();
        for (Map.Entry e : querys.entrySet()) {
            query.addCriteria(Criteria.where(e.getKey().toString()).is(e.getValue()));
        }
        Update update = new Update();
        for (Map.Entry e : updates.entrySet()) {
            update.set(e.getKey().toString(), e.getValue());
        }
        // 新版本entityClass必传
        mongoTemplate.upsert(query, update, Map.class, collectionName);
    }

    /**
     * 更新指定key的数据.
     *
     * @param prokeys        待更新字段名
     * @param provalues      待更新字段值
     * @param collectionName 集合名
     */
    public void updateOne(String key, Integer id, String[] prokeys, Object[] provalues,
                          String collectionName) {
        org.springframework.data.mongodb.core.query.Query query =
                new org.springframework.data.mongodb.core.query.Query();
        Criteria criteria = new Criteria(key);
        criteria.is(id);
        Update update = new Update();
        for (int i = 0; i < prokeys.length; i++) {
            update.set(prokeys[i].toString(), provalues[i]);
        }
        query.addCriteria(criteria);
        mongoTemplate.updateFirst(query, update, Map.class, collectionName);
    }

    /**
     * 插入map.
     *
     * @param collectionName 集合名
     */
    public void insert(Map<String, Object> map, String collectionName) throws IllegalAccessException {
        mongoTemplate.insert(map, collectionName);
    }

    /**
     * 插入bean.
     *
     * @param collectionName 集合名
     */
    public void insertMongo(Object object, String collectionName) {
        mongoTemplate.insert(object, collectionName);
    }

    /**
     * 删除指定key的数据.
     *
     * @param id             key值
     * @param collectionName 集合名
     */
    public void removeOne(String key, Object id, String collectionName, Class clazz) {
        org.springframework.data.mongodb.core.query.Query query =
                new org.springframework.data.mongodb.core.query.Query();
        Criteria criteria = new Criteria(key);
        criteria.is(id);
        query.addCriteria(criteria);
        mongoTemplate.remove(query, clazz, collectionName);
    }

    /**
     * 重命名集合.
     */
    public void renameCollection(String oldCollection, String newCollection) {
        if (!mongoTemplate.collectionExists(oldCollection)) {
            return;
        }
        if (mongoTemplate.collectionExists(newCollection)) {
            mongoTemplate.dropCollection(newCollection);
        }
        MongoCollection mongoCollection = mongoTemplate.getCollection(oldCollection);
        mongoCollection
                .renameCollection(new MongoNamespace(mongoTemplate.getDb().getName(), newCollection));
    }

    /**
     * 删除collection.
     */
    public void removeCollection(String collection) {
        if (!mongoTemplate.collectionExists(collection)) {
            return;
        } else {
            mongoTemplate.dropCollection(collection);
        }
    }
}
```
yml 文件配置：
```yml
spring:
  data:
    mongodb:
      host: 127.0.0.1
      port: 27017
      database: testdb
      username: test
      password: password
```