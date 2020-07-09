# coding=utf-8
import logging
import re
import sqlite3
import time
from time import sleep
from bs4 import BeautifulSoup          # For processing HTML
from bs4 import BeautifulStoneSoup     # For processing XML
import bs4                             # To get everything
from pyquery import PyQuery as pq
from selenium import webdriver
from selenium.webdriver.common.keys import Keys
# 导入 Select 类
from selenium.webdriver.support.ui import Select


def spiderList(driver, url, source):

    # driver.get(url)
    # while True:
    soup = bs(driver.page_source, "lxml")
    names = soup.find_all("a", {"class": "tit"})
    for i in range(len(names)):
        print('开始抓取 %s 页面内容。' % names[i].attrs['href'].strip())
        logger.info('开始抓取 %s 页面内容。' % names[i].attrs['href'].strip())
        spiderPaper(names[i].attrs['href'], source)
        sleep(3)


def spiderPaper(url, source):

    driver.get(url)
    # element = driver.find_element_by_xpath('label:nth-child(1) > i')
    element = driver.find_elements_by_css_selector('label:nth-child(2) > i')
    element[0].click()
    # print(element.is_selected())
    # element.send_keys(Keys.SPACE)
    # driver.find_element_by_id('each-done-show').click()
    btn = driver.find_elements_by_class_name('start-btn')
    # print(btn)
    btn[0].click()
    html = driver.page_source
    data = str(pq(html))
    re_rule = r'<span class="sub-tit">Question 1 Of (.*?)</span>'
    count_text = re.findall(re_rule, data, re.S)
    print('当前试卷总提数： %s。' % count_text[0])
    logger.info('当前试卷总提数： %s。' % count_text[0])
    question_count = int(count_text[0])

    soup = bs(driver.page_source, "lxml")
    paper_id = url.split('/')[5].strip()
    title = soup.find("span", {"class": "tit"}
                      ).get_text().replace('|', '').strip()
    # 插入 paper
    insertPaper(paper_id, title, source)

    # 选择题
    optElements = driver.find_elements_by_class_name('opt-area')
    if(len(optElements) > 0):
        for i in range(1, question_count + 1):
            print('开始抓取第 %s 道题。' % i)
            logger.info('开始抓取第 %s 道题。' % i)
            spiderChoice(driver, url, paper_id, i)
    else:
        for i in range(1, question_count + 1):
            print('开始抓取第 %s 道题。' % i)
            logger.info('开始抓取第 %s 道题。' % i)
            spiderChoice(driver, url, paper_id, i)
        # # 点击第一个答案
        # driver.find_elements_by_xpath(
        #     '//*[@id="practise"]/div[2]/div/div[1]/div[1]/dl/dd[1]')[0].click()
        # # 点击下一步显示正确但
        # driver.find_elements_by_class_name('next-confirm')[0].click()
        # soup = bs(driver.page_source, "lxml")
        # answers = soup.find_all("dl", {"class": "opt-list select-one"})
        # print('抓取答案 %s。' % answers)
        # rightAnswer = soup.find_all("dd", {"class": "tk-right"})
        # print('抓取正确答案 %s。' % rightAnswer)
        # # 点击下一步进入下一题
        # driver.find_elements_by_class_name('next-confirm')[0].click()


def spiderChoice(driver, url, paper_id, question_number):
    """
    抓取选择题。

    :param driver: webdriver.

    :param url: 链接地址.

    :param paper_id: 试卷 Id.

    :param question_number: 试题编号.
    """
    # 延迟 1 秒执行
    sleep(1)

    '''
    一个问题 opt-list select-one
    两个问题 opt-area select-more
    三个问题 opt-area select-more
    '''
    soup = bs(driver.page_source, "lxml")
    # 多选
    multiplelist = soup.find_all("ul", {"class": "s-options-list"})
    print('------------- 多选题 %s -------------。' % len(multiplelist))
    # 单选
    answerlist = soup.find_all("dl", {"class": "opt-list"})
    print('------------- 单选题 %s -------------。' % len(answerlist))
    # 判断是否是多选题
    if(len(multiplelist) == 0):
        if(len(answerlist) == 1):
            logger.info('点击单选题第一个答案。')
            # 点击第一个答案
            driver.find_elements_by_xpath(
                '//*[@id="practise"]/div[2]/div/div[1]/div[1]/dl/dd[1]')[0].click()
            print('点击单选题第一个答案。')
        else:
            logger.info('循环点击单选题第一个答案。')
            # 循环点击第一个答案
            for i in range(1, len(answerlist) + 1):
                xpath = '//*[@id="practise"]/div[2]/div/div[1]/div[1]/dl[' + \
                    str(i) + ']/dd[1]'
                driver.find_elements_by_xpath(xpath)[0].click()
            print('循环点击单选题第一个答案。')
    else:
        logger.info('点击多选题第一个答案。')
        # driver.find_elements_by_tag_name('input')[0].click()
        driver.find_elements_by_class_name('opt-text')[0].click()
        # 点击第一个答案
        # driver.find_elements_by_xpath(
        #     '//*[@id="practise"]/div[2]/div/div[1]/ul/li[1]')[0].click()
        # //*[@id="practise"]/div[2]/div/div[1]/ul/li[1]/label/span
        print('点击多选题第一个答案。')
    print('点击下一步提交答案。')
    driver.find_elements_by_class_name('js-next')[0].click()

    soup = bs(driver.page_source, "lxml")
    # paper_id = url.split('/')[4]
    # title = soup.find("span", {"class": "tit"}).get_text().replace('|', '').strip()
    # print('题目标题 %s。' % title)
    text = soup.find("p", {"class": "sub-text"})
    print('问题题目 %s。' % text.get_text().strip())
    logger.info('问题题目 %s。' % text.get_text().strip())
    intro = soup.find("p", {"class": "select-intro"})
    print('问题介绍 %s。' % intro.get_text().strip())
    logger.info('问题介绍 %s。' % intro.get_text().strip())
    # 判断是否是单选
    if(len(multiplelist) == 0):
        print('开始抓取单选题。')
        logger.info('开始抓取单选题。')
        answerlist = soup.find_all("dl", {"class": "opt-list"})
        # 循环获取每个小题的答案
        for i in range(0, len(answerlist)):
            insertQuestion(question_number, text.get_text().strip(), intro.get_text().strip(), 'single', 0,
                           paper_id, i)
            print('Blank %s 小题答案。' % i)
            logger.info('Blank %s 小题答案。' % i)
            answers = answerlist[i].find_all("dd")
            # print('Blank 答案：%s 。' % answerlist[i])
            for j in range(0, len(answers)):
                insertAnswer(answers[j].get_text().strip(),
                             j, 0, question_number, '', paper_id)
                print('第 %s 个答案：' % str(j + 1))
                logger.info('第 %s 个答案：' % str(j + 1))
                print(answers[j].get_text().strip())
            # 正确答案
            right_answer = answerlist[i].find("dd", {"class": "tk-right"})
            if(right_answer != None):
                updateAnswer(right_answer.get_text().strip(),
                             1, question_number, paper_id)
                print('正确答案 %s。' % right_answer.get_text().strip())
                logger.info('正确答案 %s。' % right_answer.get_text().strip())
    # 否则是多选
    else:
        print('开始抓取多选题。')
        logger.info('开始抓取多选题。')
        multiplelist = soup.find_all("ul", {"class": "s-options-list"})
        for i in range(0, len(multiplelist)):
            insertQuestion(question_number, text.get_text().strip(), intro.get_text().strip(), 'multiple', 0,
                           paper_id, i)
            answers = multiplelist[i].find_all("li")
            # print('多选题答案：%s 。' % answers)
            for j in range(0, len(answers)):
                insertAnswer(answers[j].find(
                    'span', {'class': 'opt-text'}).get_text().strip(), j, 0, question_number, '', paper_id)
                print('第 %s 个答案：' % str(j + 1))
                print(answers[j].find(
                    'span', {'class': 'opt-text'}).get_text().strip())
                logger.info('第 %s 个答案：' % str(j + 1))
                logger.info(answers[j].find(
                    'span', {'class': 'opt-text'}).get_text().strip())
                # .find('span', {'class': 'opt-text'}).get_text()
            # 正确答案
            right_answers = multiplelist[i].find_all("li", {"class": "right"})
            for k in range(0, len(right_answers)):
                updateAnswer(right_answers[k].find(
                    'span', {'class': 'opt-text'}).get_text().strip(), 1, question_number, paper_id)
                print('正确答案 %s。' % right_answers[k].find(
                    'span', {'class': 'opt-text'}).get_text().strip())
                logger.info('正确答案 %s。' % right_answers[k].find(
                    'span', {'class': 'opt-text'}).get_text().strip())
    # 点击下一步进入下一题
    driver.find_elements_by_class_name('js-next')[0].click()


def insertPaper(paper_id, paper_name, paper_source, paper_type='1041', paper_subject='1162'):
    '''
    插入试卷。
    '''
    connect = sqlite3.connect('gre.db')
    # papers = [
    #     ("5021", "测试试卷 01", "1041", "1047", "1032"),
    #     ("5022", "测试试卷 02", "1041", "1047", "1032"),
    #     ("5023", "测试试卷 03", "1041", "1047", "1032")
    # ]
    papers = [(paper_id, paper_name, paper_type, paper_subject, paper_source)]
    print('插入试卷：%s。' % papers)
    logger.info('插入试卷：%s。' % papers)
    # print(papers)
    # Create the table
    # connect.execute("create table paper(paper_id, paper_name, paper_type, paper_subject, paper_source)")

    # Fill the table （这里使用 PySqlite 提供的占用符格式，提高安全性）
    connect.executemany(
        "insert into paper(paper_id, paper_name, paper_type, paper_subject, paper_source) values (?, ?, ?, ?, ?)", papers)

    connect.commit()
    # Print the table contents （使用迭代的方法获取查询结果）
    # for row in connect.execute("select paper_id, paper_name, paper_type, paper_subject, paper_source from paper"):
    #     print(row)


def insertQuestion(question_number, question_title, question_intro, question_type, question_parent_id, question_paper_id, question_order):
    '''
    插入问题。
    '''
    connect = sqlite3.connect('gre.db')
    questions = [(question_number, question_title, question_intro,
                  question_type, question_parent_id, question_paper_id, question_order)]
    print('插入问题：%s。' % questions)
    logger.info('插入问题：%s。' % questions)
    # questions = [
    #     ("5021", "测试试卷 01", "1041", "1047", "1032"),
    #     ("5022", "测试试卷 02", "1041", "1047", "1032"),
    #     ("5023", "测试试卷 03", "1041", "1047", "1032")
    # ]
    # Create the table
    # connect.execute("create table question(paper_id, paper_name, paper_type, paper_subject, paper_source)")

    # Fill the table （这里使用 PySqlite 提供的占用符格式，提高安全性）
    connect.executemany(
        "insert into question(question_number, question_title, question_intro, question_type, question_parent_id, question_paper_id, question_order) values (?, ?, ?, ?, ?, ?, ?)", questions)

    connect.commit()


def insertAnswer(answer_text, answer_order, answer_right, answer_question_number, answer_question_id, answer_paper_id):
    '''
    插入答案。
    '''
    connect = sqlite3.connect('gre.db')
    answers = [(answer_text, answer_order, answer_right, answer_question_number,
                answer_question_id, answer_paper_id)]
    print('插入答案：%s。' % answers)
    logger.info('插入答案：%s。' % answers)
    # answers = [
    #     ("5021", "测试试卷 01", "1041", "1047", "1032"),
    #     ("5022", "测试试卷 02", "1041", "1047", "1032"),
    #     ("5023", "测试试卷 03", "1041", "1047", "1032")
    # ]
    # Create the table
    # connect.execute("create table question(paper_id, paper_name, paper_type, paper_subject, paper_source)")

    # Fill the table （这里使用 PySqlite 提供的占用符格式，提高安全性）
    connect.executemany(
        "insert into answer(answer_text, answer_order, answer_right, answer_question_number, answer_question_id, answer_paper_id) values (?, ?, ?, ?, ?, ?)", answers)

    connect.commit()


def updateAnswer(answer_text, answer_right, answer_question_number, answer_paper_id):
    '''
    更新答案
    '''
    connect = sqlite3.connect('gre.db')

    answers = [(answer_text, answer_right,
                answer_question_number, answer_paper_id)]
    print('更新答案：%s' % answers)
    logger.info('更新答案：%s。' % answers)
    # connect.executemany(
    # "update answer set answer_right = ? where answer_text = ? and answer_question_number = ? and answer_paper_id = ?", answers)
    sql = "update answer set answer_right = " + str(answer_right) + " where answer_text = '" + answer_text + \
        "' and answer_question_number = " + \
        str(answer_question_number) + \
        " and answer_paper_id = '" + answer_paper_id + "'"
    print(sql)
    connect.execute(sql)

    connect.commit()


def spiderSource(url):
    print('抓取列表：%s' % url)


if __name__ == "__main__":
    logger = logging.getLogger(__name__)
    logger.setLevel(level=logging.INFO)
    handler = logging.FileHandler("superlearn_log.txt")
    handler.setLevel(logging.INFO)
    formatter = logging.Formatter(
        '%(asctime)s - %(name)s - %(levelname)s - %(message)s')
    handler.setFormatter(formatter)
    logger.addHandler(handler)

    logger.info("Spider Start.")

    # 创建浏览器对象
    driver = webdriver.Chrome()
    urls = {
        "Powerprepplus": "http://gre.kmf.com/practise/tc/40",
        "The Princeton Review": "http://gre.kmf.com/practise/tc/10",
        "Kaplan": "http://gre.kmf.com/practise/tc/11",
        "Barron": "http://gre.kmf.com/practise/tc/13",
        "Magoosh": "http://gre.kmf.com/practise/tc/17",
    }

    for key in urls.keys():
        url = urls.get(key)
        # url = 'http://gre.kmf.com/practise/tc/40'   # Powerprepplus
        # url = 'http://gre.kmf.com/practise/tc/10'   # The Princeton Review
        # url = 'http://gre.kmf.com/practise/tc/11'   # Kaplan
        # url = 'http://gre.kmf.com/practise/tc/13'   # Barron
        # url = 'http://gre.kmf.com/practise/tc/17'   # Magoosh
        driver.get(url)
        # 生成当前页面快照
        driver.save_screenshot("screenshot.png")
        soup = bs(driver.page_source, "lxml")
        ets = soup.find("div", {"class": "ets-protocol"})
        # print('确认按钮数量：%s' % ets.attrs['style'])
        if(ets != None):
            if(ets.attrs['style'] != 'display: none;'):
                driver.find_element_by_class_name(
                    'ets-protocol-confirm').click()
        pages = soup.select("a.kmf-page")
        print('共 %s 页' % pages)
        for pageNo in range(len(pages)):

            driver.get(url)
            if(pageNo != 0):
                for i in range(0, pageNo):
                    driver.find_element_by_css_selector('.next').click()
            print('开始抓取第 %s 页' % pageNo)
            spiderList(driver, url, key)
    print('----------------------------抓取结束----------------------------')
    driver.quit()
    logger.info("Spider execute finish.")
    
