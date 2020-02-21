package com.charles.solrjexample.common;


import java.util.List;

/**
 * 通用分页实体对象
 *
 * @param <T>
 */
public class ResponsePagedResult<T> extends ResponseResult {
    /**
     * 列表
     */
    private List<T> DataList;
    /**
     * 总数
     */
    private Integer Count;
    /**
     * 当前页
     */
    private Integer PageIndex;
    /**
     * 分页大小
     */
    private Integer PageSize;

    private Object Extend;

    public ResponsePagedResult() {

    }

    public void setExtend(Object extend) {
        Extend = extend;
    }

    public Object getExtend() {
        return Extend;
    }

    public List<T> getList() {
        return DataList;
    }

    public void setList(List<T> value) {
        this.DataList = value;
    }

    public Integer getCount() {
        return Count;
    }

    public void setCount(Integer value) {
        this.Count = value;
    }

    public Integer getPageIndex() {
        return PageIndex;
    }

    public void setPageIndex(Integer value) {
        this.PageIndex = value;
    }

    public Integer getPageSize() {
        return PageSize;
    }

    public void setPageSize(Integer value) {
        this.PageSize = value;
    }
}
