package com.charles.solrjexample.common;

public enum ResponseResultCodeEnum {
    /**
     * 默认成功。
     */
    SUCCESS(200, "请求成功"),
    /**
     * 请求失败。
     */
    ERROR(400, "请求失败");

    private ResponseResultCodeEnum(int code, String description) {
        this.code = code;
        this.description = description;
    }

    private final String description; // 错误码描述
    private final int code; // 错误码

    public int getCode() {
        return code;
    }

    public String getDescription() {
        return description;
    }
}
