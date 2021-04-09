package org.charles.learning.springmongodemo.utils;

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