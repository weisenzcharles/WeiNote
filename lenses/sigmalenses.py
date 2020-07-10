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


def spiderSource(url):
    print('抓取列表：%s' % url)

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

        section_node = soup.find('section', class_='c-page-content')
        # print(section_node.name, section_node['class'], section_node.get_text())
        concept_nodes = section_node.find_all('section')
        for i in range(0, len(concept_nodes)):
            print(concept_nodes[i].name)
        # 创建 xls 文件对象
        wb = xlwt.Workbook()
        # 新增两个表单页
        sh1 = wb.add_sheet('成绩')
        # 保存文件
        wb.save('sigmalenses.xls')
    else:
        print('请求失败了！')
