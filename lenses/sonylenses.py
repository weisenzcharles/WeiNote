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

Api_Url = 'https://www.sonystyle.com.cn/pim/out?method=findMasterDataList&sku8Ds=P32848486'


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
    # a = concept_node.find_all('img')
    # print(a)
    sku_list = []
    for i in range(0, len(lenses_nodes)):
        print('=================================================')
        print('镜头名称', '=>', lenses_nodes[i].find(
            'a', class_='pd_title').get_text(), i)
        div = lenses_nodes[i].find('div', 'block-item-div')
        # print(div.attrs['sku'])
        sku_list.append(div.attrs['sku'])
        # alt = lenses_nodes[i].find('img').attrs['alt']
    for sku in sku_list:
        print('=>', sku)
        # url = 'https://www.sonystyle.com.cn/products/lenses/' + alt.lower() + '/' + \
        #     alt.lower()+'.html'
        # if url is None:
        #     request = requests.get(url)
        #     print(url)
        #     soup = bs(request.text, "html.parser", from_encoding="utf-8")
        #     specification_node = soup.find('section', id='specifications')
        #     name = soup.find('h1', class_='fac-item-nav-header')
        #     sheet.write(i, 0, name)
        #     tables_node = specification_node.find_all('tr')
        #     # print('节点总数：' + str(len(tables_node)))
        #     for j in range(0, len(tables_node)):
        #         # print(tables_node[j].find_all('td'))
        #         td_nodes = tables_node[j].find_all('td')
        #         if len(td_nodes) > 1:
        #             specification_name = td_nodes[0].get_text()
        #             specification_text = td_nodes[1].get_text()
        #         else:
        #             specification_name = tables_node[j].find('th').get_text()
        #             specification_text = td_nodes[0].get_text()

        #         print(specification_name, '=>', specification_text)
        #         # for name, member in Specification.__members__.items():
        #         #     print(name, '=>', member, ',', member.value)
        #         #     sheet.write(i, j + 1, tables_node[j].get_text())


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

    request = requests.get(
        'https://www.sonystyle.com.cn/products/lenses/index.html')
    if request.status_code == 200:
        # 创建一个 BeautifulSoup 对象
        soup = bs(request.text, "html.parser", from_encoding="utf-8")
        # 创建文件
        wb = xlwt.Workbook()
        section_node = soup.find('div', class_='slt_index_right fr')
        concept_nodes = section_node.find_all(
            'div', class_='product-list-box no-query section')
        for i in range(0, len(concept_nodes)):
            print(concept_nodes[i].find('h2').get_text())
            # 新增表单页
            sheet = wb.add_sheet(concept_nodes[i].find('h2').get_text())
            spiderLenses(wb, sheet, concept_nodes[i])

        # 保存文件
        wb.save('sonylenses.xls')
    else:
        print('请求失败了！')
