# 線上學習專區 API

API 金鑰

```
UoLgyT3cLMeM9jAu0smB
```

## 課程單元資料

Method: **GET**

Url:

```
https://e-quitsmoking.hpa.gov.tw/qsms-api/courses/list?key=UoLgyT3cLMeM9jAu0smB
```

## 所有學習紀錄

Method: **GET**

Url:

```
https://e-quitsmoking.hpa.gov.tw/qsms-api/completions/list?key=UoLgyT3cLMeM9jAu0smB&startDate=起始日期&endDate=結束日期

startDate (選擇性) 格式範例：2018-02-01
endDate (選擇性) 格式範例：2018-02-01
```

## 查詢某學員學習紀錄 (使用身分證字號)

Method: **POST**

Parameters:

```
idNumber (必要)
startDate (選擇性) 格式範例：2018-02-01
endDate (選擇性) 格式範例：2018-02-01
```

Url:

```
https://e-quitsmoking.hpa.gov.tw/qsms-api/completions/query?key=UoLgyT3cLMeM9jAu0smB
```

## 取得自動登入 URL

Method: **POST**

Parameters:

```
firstName (必要) 名字
lastName (必要) 姓氏
username (必要) 帳號*身分證字號*
email (必要) 電子郵件
idNumber (必要) 身分證字號
courseId (選擇性) 課程Id，如果有填，登入後會自動導向課程頁面
```

Url:

```
http://hpaqstest.mydevhost.com/qsms-api/sso/generate-url?key=UoLgyT3cLMeM9jAu0smB
```

## 取得測驗成績

Method: **POST**

Parameters:

```
idNumber (非必要)
startDate (必要) 格式範例：2018-02-01
endDate (必要) 格式範例：2018-02-01
```

Url:

```
https://e-quitsmoking.hpa.gov.tw/qsms-api/grades/list?key=UoLgyT3cLMeM9jAu0smB
```

## 滿意度調查列表

Method: **GET**

Url:

```
https://e-quitsmoking.hpa.gov.tw/qsms-api/feedback/list?key=UoLgyT3cLMeM9jAu0smB
```

## 滿意度調查問題回答

Method: **POST**

Parameters:

```
startDate (非必要) 格式範例：2018-02-01
endDate (非必要) 格式範例：2018-02-01
```

Url:

```
https://e-quitsmoking.hpa.gov.tw/qsms-api/feedback/answers?key=UoLgyT3cLMeM9jAu0smB
```

備註:

```
回答的數值：

1為「非常滿意」、2為「滿意」、 3為「普通」、 4為「不滿意」、 5為「很不滿意」
```

## 滿意度調查 - 依身分證字號查詢已填過的調查

Method: **POST**

Parameters:

```
idNumber (必要) 身分證字號
startDate (非必要) 格式範例：2018-02-01
endDate (非必要) 格式範例：2018-02-01
```

Url:

```
https://e-quitsmoking.hpa.gov.tw/qsms-api/feedback/query?key=UoLgyT3cLMeM9jAu0smB
```

備註:

```
回答的數值：

1為「非常滿意」、2為「滿意」、 3為「普通」、 4為「不滿意」、 5為「很不滿意」
```
