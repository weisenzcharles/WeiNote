# -*- coding: utf-8 -*-
import pysolr

SOLR_URL = 'http://localhost:8983/solr/posts'


def add():
    """
    添加
    """
    result = solr.add([
        {
            'id': '20000',
            'post_title': 'test-title-20000',
            'post_name': 'test-name-20000',
            'post_excerpt': 'test-excerpt-20000',
            'post_content': 'test-content-20000',
            'post_date': '2019-06-18 14:56:55',
        },
        {
            'id': '20001',
            'post_title': 'test-title-20001',
            'post_name': 'test-name-20001',
            'post_excerpt': 'test-excerpt-20001',
            'post_content': 'test-content-20001',
            'post_date': '2019-06-18 14:56:55',
        }
    ])
    result = solr.commit()
    results = solr.search(q='id: 20001')
    print(results.docs)


def delete():
    """
    删除
    """
    solr.delete(q='id: 20001')
    result = solr.commit()
    results = solr.search(q='id: 20001')
    print(results.docs)


def update():
    """
    更新
    """
    solr.add([
        {
            'id': '20000',
            'post_title': 'test-title-updated',
            'post_name': 'test-name-updated',
            'post_excerpt': 'test-excerpt-updated',
            'post_content': 'test-content-updated',
            'post_date': '2019-06-18 15:00:00',
        }
    ])
    result = solr.commit()
    results = solr.search(q='id: 20000')
    print(results.docs)


def query():
    """
    查询
    """
    results = solr.search('苹果')
    print(results.docs)


if __name__ == "__main__":
    solr = pysolr.Solr(SOLR_URL)
    add()
    delete()
    update()
    query()
