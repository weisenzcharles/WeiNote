# coding=utf-8
import logging
import re
import sqlite3
import time
from time import sleep
from bs4 import BeautifulSoup as bs
from pyquery import PyQuery as pq
from docx import Document
from docx.shared import Inches
import requests
# 导入 xlwt 库
import xlwt
from enum import Enum

Specification = Enum('specification', ('镜头结构', '视角（APS-C 画幅）', '视角（35mm）', '光圈叶片数', '最小光圈', '最近对焦距离',
                                       '最大放大倍率', '滤镜尺寸', '尺寸', '重量', '体积', '最大直径', '滤色片口径', '卡口', '配件', '版本号'))
Mount = Enum('Mount', ('E-Mount', 'F-Mount', 'EF-Mount', 'L-Mount'))


def spiderLensesInfo(wb, concept_node: str):
    '''
    处理镜头信息
    '''

    print('')


def spiderLenses(wb, sheet, concept_node: str):
    '''
    处理镜头列表
    '''

    lenses_nodes = concept_node.find_all('li')
    for i in range(0, len(lenses_nodes)):
        print('=================================================')
        print('镜头名称', '=>', lenses_nodes[i].find('b').get_text())
        url = 'http://www.sigma-photo.com.cn' + \
            lenses_nodes[i].find('a').attrs['href'] + 'specifications/'
        request = requests.get(url)
        soup = bs(request.text, "html.parser", from_encoding="utf-8")
        specification_node = soup.find('section', id='specifications')
        name = soup.find('h1', class_='fac-item-nav-header')
        sheet.write(i, 0, name)
        tables_node = specification_node.find_all('tr')
        # print('节点总数：' + str(len(tables_node)))
        for j in range(0, len(tables_node)):
            # print(tables_node[j].find_all('td'))
            td_nodes = tables_node[j].find_all('td')
            if len(td_nodes) > 1:
                specification_name = td_nodes[0].get_text()
                specification_text = td_nodes[1].get_text()
            else:
                specification_name = tables_node[j].find('th').get_text()
                specification_text = td_nodes[0].get_text()

            print(specification_name, '=>', specification_text)
            # for name, member in Specification.__members__.items():
            #     print(name, '=>', member, ',', member.value)
            #     sheet.write(i, j + 1, tables_node[j].get_text())


def spiderConcept(note: str):
    """ 
    处理镜头分类 
    """
    print('抓取列表：%s' % note)


def replaceText(text: str):
    """
    格式化文本
    """
    # 匹配英文临近中文
    reg = '/([A-Za-z])((<[^<]*>)*[\u4e00-\u9fa5]+)/gi'
    text = text.replace(reg, "$1 $2")
    # 匹配数字临近中文
    p2 = '/([0-9])([\u4e00-\u9fa5]+)/gi'
    text = text.replace(p2, "$1 $2")
    # 匹配中文临近数字
    p3 = '/([\u4e00-\u9fa5]+)([0-9])/gi'
    text = text.replace(p3, "$1 $2")
    p4 = '/([\u4e00-\u9fa5]+(<[^<]*>)*)([A-Za-z])/gi'
    text = text.replace(p4, "$1 $3")


if __name__ == "__main__":
    logger = logging.getLogger(__name__)
    logger.setLevel(level=logging.INFO)
    handler = logging.FileHandler("sigmalenses_log.txt")
    handler.setLevel(logging.INFO)
    formatter = logging.Formatter(
        '%(asctime)s - %(name)s - %(levelname)s - %(message)s')
    handler.setFormatter(formatter)
    logger.addHandler(handler)

    logger.info("Spider Start.")

    request = requests.get('http://www.sigma-photo.com.cn/cs/lenses/')
    if request.status_code == 200:
        # 创建一个 BeautifulSoup 解析对象
        soup = bs(request.text, "html.parser", from_encoding="utf-8")
        # 创建 xls 文件对象
        wb = xlwt.Workbook()
        section_node = soup.find('section', class_='c-page-content')
        # print(section_node.name, section_node['class'], section_node.get_text())
        concept_nodes = section_node.find_all('section')
        for i in range(0, len(concept_nodes)):
            print(concept_nodes[i].find('h1'))
            # 新增两个表单页
            sheet = wb.add_sheet(concept_nodes[i].find('h1').get_text())
            spiderLenses(wb, sheet, concept_nodes[i])

        # 保存文件
        wb.save('sigmalenses.xls')
    else:
        print('请求失败了！')
