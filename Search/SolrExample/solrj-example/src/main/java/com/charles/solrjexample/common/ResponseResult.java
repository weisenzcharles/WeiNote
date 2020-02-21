package com.charles.solrjexample.common;


/**
 * 返回实体对象
 *
 * @param <T>
 */
public class ResponseResult {
    public ResponseResult() {

    }

    public ResponseResult(boolean result) {
        this.result = result;
    }

    public ResponseResult(boolean result, String msg) {
        this.result = result;
        this.msg = msg;
    }

    /**
     * 返回代码，默认 200 为应答成功
     */
    private int code = ResponseResultCodeEnum.SUCCESS.getCode();
    /**
     * 返回结果
     */
    private Boolean result;

    /**
     * 返回结果
     */
    private String msg;

    public Integer getCode() {
        return this.code;
    }

    public void setCode(Integer value) {
        this.code = value;
    }

    public Boolean getResult() {
        return this.result;
    }

    public void setResult(Boolean value) {
        this.result = value;
    }

    public String getMsg() {
        return this.msg;
    }

    public void setMsg(String value) {
        this.msg = value;
    }

}