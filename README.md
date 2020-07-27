Haceau-Calculator
=================
![Language](https://img.shields.io/badge/Language-C%23-blue.svg?style=flat-square) ![.Net Core](https://img.shields.io/badge/.Net&nbsp;Core-3.1-blue.svg?style=flat-square)

[![Gitee](https://img.shields.io/badge/Gitee-辰落火辉Haceau-red.svg?style=flat-square)](https://gitee.com/haceau/Haceau-Calculator)
[![Github](https://img.shields.io/badge/Github-HaceauZoac-blue.svg?style=flat-square)](https://github.com/Haceau-Zoac/Haceau-Calculator)

简介
---
该项目为使用C#编写的计算器。当前版本为v1.4.0。支持小括号、加减乘除、去除小数的除、取余、幂运算、正负数运算和函数。开源协议为MIT。保持更新中。

类
---
Calculator：计算器类，实现计算功能。

Tools：工具类，一些与计算器无关的功能在这里实现。

* Calculator以及Tools类可以直接拿走进行开发（前提：遵守MIT许可证）。

使用
---
* 加：1 + 2 = 3
* 正：+1 + (+2) = 3
* 减：1 - 2 = -1
* 负：-1 - (-2) = 1
* 乘：1 * 2 = 2
* 幂：1 ^ 2 或 1 ** 2 = 2
* 除：1 / 2 = 0.5
* 取余：1 % 2 = 1
* 除（去除小数）：1 // 2 = 0
* 函数：\<函数名>(值)
* 函数列表：abs、acos、acosh、asin、atan、atanh、cbrt、ceiling、cos、cosh、exp、floor、ilogb、log、log10、log2、round、sign、sin、sinh、sqrt、tan、tanh、truncate

待办清单
------
|完成版本|内容|
|---|---|
|1.1.0|支持正/负数运算|
|1.2.0|支持取余运算|
|1.3.0|支持幂运算和去小数位的除运算|
|取消，使用小括号嵌套代替|支持中括号|
|1.4.0|支持函数|
|-|长期维护|