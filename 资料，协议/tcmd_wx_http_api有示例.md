### 说明（及Token）
###### 接口请求地址
http://test.zy360.com/tcmd/Cclient/www/
<br/><br/>

HTTP请求加解密<br/>

一、HTTP请求(POST)

HTTP Header 传参:<br/>
在http请求头中发送以下内容，所有请求均需要携带（部分字段在登录接口不用传）<br/>
<table>
<tr>
	<td><b>HTTP头名称</b></td>
	<td><b>说明</b></td>
	<td><b>必须</b></td>
</tr>
<tr>
	<td>X-BJ-C-AccessKeyID</td>
	<td>Access Key ID（<a href="#access_key">详见下文赋值</a>）</td>
	<td>是</td>
</tr>
<tr>
	<td>X-BJ-C-DeviceID</td>
	<td>设备唯一号（UUID）</td>
	<td>是</td>
</tr>
<tr>
	<td>X-BJ-C-Timestamp</td>
	<td>时间戳</td>
	<td>是</td>
</tr>
<tr>
	<td>X-BJ-C-Version</td>
	<td>版本号， 例1.0</td>
	<td>是</td>
</tr>
<tr>
	<td>X-BJ-C-Signature</td>
	<td>签名串（<a href="#sign">详见下文说明</a>）</td>
	<td>是</td>
</tr>
</table>	

<br/>
<a name="access_key">Access Key说明</a>
<table>
<tr>
	<td><b>Access Key ID</b></td>
	<td><b>Access Key</b></td>
</tr>
<tr>
	<td>fGPeLhMkEbfsKfzCX24f</td>
	<td>CpDRdMFnsyNaD9YQpNaLpqmnKfRfikpWTJNthws2</td>
</tr>
</table>

<br/>

#### <a name="sign">签名串说明</a>

**StringToSign** = timestamp + deviceid + AccessKey + version + url(不含参数) + (参数name+值value)按字典序拼接 + Access Key ID

\*注意: *如果 参数值 长度超过400个字符, 则只取前 400 个字符作为被 StringToSign 的一部分被拼接, 一个汉字当做一个字符*

**Signature(签名串) = Binary2HEX(HMAC-SHA256(SigningKey, StringToSign))**

###### SigningKey说明：

SigningKey : vPFonKCCmDfxcCicRvXTTpY2muXcMhL9NmwheBbX

<br/><br/>
HTTP Body 传参:<br/>
详见每个接口的传参，在需要加密处理的接口增加timestamp和nonce(7位)参数

加密方式：<br/>
 1. 所有请求参数按字典排序并拼接（JSON格式)<br/>
 2. 进行3DES加密<br/>
 3. 进行Base64加密<br/>
 4. 把生成的字符串作为参数param的值放在HTTP请求BODY中
<br/>
(备注：3DES加密方式为CBC模式和PKCS7填充方式）
<br/><br/>

<a name="3des">3DES说明</a>
<table>
<tr>
	<td><b>Key</b></td>
	<td><b>IV</b></td>
</tr>
<tr>
	<td>xEu1p;LP7iZmxKyTtA*HV9pS</td>
	<td>j0?2UK+p</td>
</tr>
</table>

### 举例：<br/>
传参值为：<br/>
{<br/>
	"version":"1.0.1",<br/>
	"devicesn":"RK-73106856",<br/>
	"code":"10001",<br/>
	"timestamp":"1499050943",<br/>
	"nonce":"fuhs0i2"<br/>
}
<br/>

HTTP Header值依次为:<br/>
X-BJ-C-AccessKeyID:fGPeLhMkEbfsKfzCX24f<br/>
X-BJ-C-DeviceID:EA6DEFFC-3A79-5396-A833-C837929C79C3<br/>
X-BJ-C-Timestamp:1499050943<br/>
X-BJ-C-Version:1.0<br/>
X-BJ-C-Signature:（各步骤值如下）<br/>
1. (参数name+值value)按字典序拼接<br/>
code10001devicesnRK-73106856noncefuhs0i2timestamp1499050943version1.0.1<br/>
2. StringToSign = 1499050943EA6DEFFC-3A79-5396-A833-C837929C79C3CpDRdMFnsyNaD9YQpNaLpqmnKfRfikpWTJNthws21.0http://test.zy360.com/tcmd/Cclient/www/Rs/monitorcode10001devicesnRK-73106856noncefuhs0i2timestamp1499050943version1.0.1fGPeLhMkEbfsKfzCX24f<br/>
3. 256哈希后【DATA】<br/>
<2e0794a5 089853ae bc265ba0 54317b89 65607a2e 7e0d3b8c ffa3e3c5 ad830411><br/>
4. 转16进制后<br/>
X-BJ-C-Signature:2e0794a5089853aebc265ba054317b8965607a2e7e0d3b8cffa3e3c5ad830411
<br/><br/>

HTTP Body:<br/>
生成传参各步骤依次为：<br/>
1. 按字典排序并拼接(JSON)<br/>
{"code":"10001","devicesn":"RK-73106856","nonce":"fuhs0i2","timestamp":"1499050943","version":"1.0.1"}<br/>
2. 3DES加密并做Base64后<br/>
+eN/LJOJbK6OqW7HNf7kVhkZV0fCHPIZtBfy/HkE4ogE8LHuWsV2Kb1XiyUsDXik2yST3FU1wHS+ASxXAbThhvNUsIv1mgqxxfr4SwoFhimDDZVRAlD28oRi5V07IN0rp3hIXd8tqsk=
3. 把生成的值作为param参数
param:
<br/><br/>

二、HTTP回复

接口返回的数据进行了3DES加密，并做了BASE64编码，收到response后需要做BASE64解码和3DES解密，解密后为json格式的字符串（可转换成字典或数组）<br/>

<br/><br/>

### 接口列表

1. 文章相关接口

~~1.0 <a href="#1.0">单独上传一张图片</a>	POST /Article/uploadOnePicture<br/>
1.1 <a href="#1.1">获取首页轮播图</a>	POST /Article/circulationLogoes<br/>
1.2 <a href="#1.2">获取首页文章分类</a>	POST /Article/articleCategory<br/>~~
1.3 <a href="#1.3">文章列表</a>	POST /Article/articles<br/>
~~1.4 <a href="#1.4">根据分类获取文章</a>	POST /Article/articlesByCategory<br/>
1.4-1 <a href="#1.4-1">获取热门分类文章列表</a>	POST /Article/getHotArticles<br/>
1.4-2 <a href="#1.4-2">获取周边分类文章列表</a>	POST /Article/getAroundArticles<br/>
1.4-3 <a href="#1.4-3">提交文章</a>	POST /Article/submitArticles<br/>
1.5 <a href="#1.5">根据文章id获取评论列表</a>~~	POST /Article/commentsByarticleId<br/>
1.6 <a href="#1.6">文章详情</a>	POST /Article/article<br/>
~~1.7 <a href="#1.7">给文章提交评论</a>	POST /Article/submitComment<br/>~~
1.8<a href="#1.8">文章点赞</a>	POST /Article/like<br/>

2. 用户接口

2.0 <a href="#2.0">获取验证码</a>	POST /User/identifyCode<br/>
2.1 <a href="#2.1">用户登录</a>	        POST /User/login<br/>
2.2 <a href="#2.2">完善个人资料信息</a> POST /User/updateProfile<br/>
~~2.2-1 <a href="#2.2-1">提交完善个人资料信息</a> POST /User/submitMyselfMessage<br/>~~
2.3 <a href="#2.3">获取用户信息</a>	POST /User/profile<br/>
~~2.3-1 <a href="#2.3-1">提交个人主页中个人简介</a>	POST /User/setIntroduce<br/>
2.3-2 <a href="#2.3-2">提交个人主页中擅长</a>	POST /User/setSpecialize<br/>
2.4 <a href="#2.4">我的粉丝</a>         POST /User/myFans<br/>~~
2.5 <a href="#2.5">我的收藏</a>         POST /User/collections<br/>
~~2.6 <a href="#2.6">用户资质认证</a>         POST /User/qualification<br/>
2.6-1 <a href="#2.6-1">判断用户是否认证通过</a>     POST /User/isQualification<br/>~~
2.7 <a href="#2.7">添加收藏/取消收藏</a>	POST /User/collect<br/>
~~2.8 <a href="#2.8">搜索接口(返回医生列表)</a>         POST /User/seachDoctorAndArticle<br/>
2.8-1 <a href="#2.8-1">搜索接口（返回文章列表）</a>   POST /User/seachDoctorAndArticle<br/>
2.9 <a href="#2.9">我的关注</a>         POST /User/myAttention<br/>
2.91 <a href="#2.91">我的消息</a>         POST /User/getAllUserMessage<br/>
2.91-1 <a href="#2.91-1">返回我的消息未读消息总数</a>         POST /User/getAllNotReadUserMessage<br/>
2.92 <a href="#2.92">更改消息查看状态</a>         POST /User/updateMessageStatus<br/>
2.93 <a href="#2.93">单独上传用户头像</a>         POST /User/headUpload<br/>
2.94 <a href="#2.94">获取区域</a>         POST /User/provinces<br/>
2.95 <a href="#2.95">我的文章</a>         POST /User/getUserArticleList<br/>
2.96 <a href="#2.96">登录以后显示用户的信息</a>  POST /User/getUserMsg<br/>~~
2.97 <a href="#2.97">意见反馈</a>         POST /User/feedback<br/>
~~2.98 <a href="#2.98">登录用户查看医生的文章列表</a>         POST /User/getUserALLArticle<br/>
2.99 <a href="#2.99">获取医生的所有医友评价列表</a>         POST /User/getALLDoctorComment<br/>
2.10 <a href="#2.10">根据常见病获取医生列表</a>            POST /User/doctorsBySymptom~~<br/>
2.11 <a href="#2.11">我的会诊列表</a>         POST /User/consultations<br/>
2.12 <a href="#2.12">会诊详情</a>		POST /User/consultation<br/>
2.13 <a href="#2.13">绑定设备</a>		POST /User/bind<br/>
2.14 <a href="#2.14">注册</a>		POST /User/register<br/>
2.15 <a href="#2.15">就诊人列表</a>	POST /User/patients<br/>
2.15 <a href="#2.16">添加就诊人</a>	POST /User/addPatient<br/>

3.医患沟通
    
~~3.1 <a href="#3.1">提交问题</a>             POST /Question/submitQuestion<br/>
3.2 <a href="#3.2">问题列表</a>             POST /Question/Questions<br/>
3.3 <a href="#3.3">问答详情评论列表</a>	   POST /Question/questionDetail<br/>
3.4 <a href="#3.4">我的问题(我发布的问题和评论的问题)</a>             POST /Question/userQuestions<br/>
3.5 <a href="#3.5">对问题回复</a>           POST /Question/replyQuestion<br/>
3.6 <a href="#3.6">获取问答详情界面问题信息</a>           POST /Question/getOneQuestion<br/>
3.7 <a href="#3.7">常见病列表</a>			POST /Symptom/symptoms<br/>
3.8 <a href="#3.8">常见病详情</a>			POS  /Symptom/symptom<br/>~~
3.9 <a href="#3.9">医生问诊列表（医生端）</a>			POST /Doctor/inquirys<br/>
3.10 <a href="#3.10">医生问诊详情（医生端）</a>		POST /Doctor/inquiryDetail<br/>
3.11 <a href="#3.11">患者提交问诊</a>		POST /Inquiry/inquiry<br/>
3.12 <a href="#3.12">回复问诊</a>		POST /Inquiry/reply<br/>
3.13 <a href="#3.13">问诊列表（患者端）</a>	POST /Patient/inquirys<br/>
3.14 <a href="#3.14">问诊详情（患者端）</a>	POST /Patient/inquiryDetail<br/>

~~4. 用户关注接口~~

~~4.1<a href="#4.1">点击关注接口(医生和 分类是同一个接口)</a>	POST /user/setAttention<br/>
4.2<a href="#4.2">返回所有已认证的医生</a>                       POST /user/getAllDoctor<br/>~~


5. 病例接口

5.1<a href="#5.1">病例列表</a>	POST /Cases/cases<br/>
5.2<a href="#5.2">病例详情</a> POST /Cases/case<br/>
5.3<a href="#5.3">药品列表</a> POST /Cases/drugs<br/>
5.4<a href="#5.4">药品详情</a> POST /Cases/drug<br/>

6. 会诊接口

6.1<a href="#6.1">医生列表</a>	POST /Doctor/doctors<br/>
6.2<a href="#6.2">医生详情</a> POST /Doctor/doctor<br/>
6.3<a href="#6.3">提交会诊</a> POST /Doctor/consultation<br/>
6.4<a href="#6.4">会诊回复</a> POST /Doctor/consultationReply<br/>

7. 医学检查接口

7.1<a href="#7.1">检查报告列表</a>	POST /Medical/reports<br/>
7.2<a href="#7.2">检查报告详情</a> POST /Medical/report<br/>
7.3<a href="#7.3">心电图数据</a>	POST /Medical/ecg<br/>
7.4<a href="#7.4">检查报告列表（CIS）</a>	POST /Medical/cisReports<br/>
7.5<a href="#7.5">检查报告详情（CIS）</a>	POST /Medical/cisReport<br/>
7.6<a href="#7.6">身体各部位及主症列表</a>	POST /Medical/bodyPartsSymptoms<br/>
7.7<a href="#7.7">可能疾病列表</a>	POST /Medical/diseases<br/>

8. 在线学习接口

8.1<a href="#8.1">课程列表</a>	POST /Article/courses<br/>
8.2<a href="#8.2">课程详情</a> POST /Article/course<br/>
8.3<a href="#8.3">医生的课程</a> POST /Doctor/courses<br/>
8.4<a href="#8.4">课时URL</a>	POST /Article/lessonURL<br/>
8.5<a href="#8.5">课程全部课时</a> POST /Article/lessons<br/>

9. 电子病历

9.1 <a href="#9.1">医生的患者列表（CIS端实现）</a>	POST /Doctor/patients<br/>
9.2 <a href="#9.2">就诊记录（CIS端实现）</a> POST /Patient/visRecords<br/>
9.3 <a href="#9.3">患者详情（CIS端实现）</a>	POST /Patient/profile<br/>
9.4 <a href="#9.4">就诊详情（CIS端实现）</a>	POST /Patient/visDetail<br/>
9.5 <a href="#9.5">患者的检查报告列表</a>	POST /Patient/reports<br/>

10. 串口通信

10.1 <a href="#10.1">软件安装验证</a>	POST /Rs/verify<br/>
10.2 <a href="#10.2">设备监控（心跳包）</a>	POST /Rs/monitor<br/>
10.3 <a href="#10.3">上报数据</a>	POST /Rs/report<br/>
10.4 <a href="#10.4">版本号</a>	POST /Rs/version<br/>

<br/><hr><br/>

### <a name="1.0">1.0 单独上传一张图片</a>

###### 请求接口
POST /Article/uploadOnePicture

###### 请求参数
	{
		picture:""      //base64 去除逗号之前的部分 
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": {
                'imgUrl':""             //上传图片地址
            }
	    "message": "操作成功"
	}

<br/>

### <a name="1.1">1.1 获取首页轮播图</a>

###### 请求接口
POST /Article/circulationLogoes

###### 请求参数
	{
		
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": [                                                 //没有数据则 data= "";
	        {
	            "id": "8",                                        //图片id
	            "logo": "MC4xe52b.jpg"                            //图片
                    "logoUrl":""                                      //图片跳转地址
	        },
	     	......
	    ],
	    "message": "操作成功"
	}

<br/>
	
### <a name="1.2">1.2 获取首页文章分类</a>

###### 请求接口
POST /Article/articleCategory

###### 请求参数
	{
		
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": [                  //没有数据则 data= "";
	        {
	            "id": "4",         // 分类id
	            "name": "文章"     // 分类名称
                    "imgUrl":""        // 分类图片
                    "attentionNum":"0",//关注数量
                    "articleNum":"1"   //文章数量
	        },
	        ......
	    ],
	    "message": "操作成功"
	}

<br/>

### <a name="1.3">1.3 文章列表</a>

###### 请求接口
POST /Article/articles

###### 请求参数
	{
            token:TOKEN
            "key":"推拿"	// 搜索关键词
            "page":"0"  	//分页
            "category":"1"	// 类别(获取所有文章时不传)
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": [
	    	{
	    		"id": "2",				// 文章id
	    		"title": "北漂打拼固然要紧",		// 文章标题
	    		"icon":"http://xx.xx.xx/xx/xx.png"	//文章图标
	    		"author":"北京中医药大学"			// 发布者
	    		"eventDate": "2016-03-06",   		// 活动时间
	    		"city":"北京",				// 活动地点
	        },
	        ......
	    ],
	    "message": "操作成功"
	}

<br/>

### <a name="1.4">1.4 根据分类获取文章</a>

###### 请求接口
POST /Article/articlesByCategory

###### 请求参数
	{
                "token":""                                  // 有就传，没有不传
		"categoryID":"3",                          // 分类id
                "page":""                                   //分页
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": {                                //没有文章则 data= "";
                "categoryName":"名家访谈",
                "isFans":"0"                        //是否是粉丝 （0 未登录,不显示 1是 2不是）
                "articles":[
                    { 
                        "id": "2",                          // 文章id     
                        "title": "北漂打拼固然要紧",         // 文章标题
                        "imgUrl":"http://test.zy360.com/tcmd/Cdashboard/www/upload/articleLogo/160727/11/MC41MDkyMDkwMCAxNDY5NTkxODkz579831557c56b.png"  //文章列表图片 
                        "content": "阿斯蒂芬阿斯蒂芬",       // 文章内容 
                        "doctorID":"2"                      // 作者ID                      
                        "author": "大师傅",                 // 作者
                        "level":"副主任医师",               //  医生级别
                        "logo":""                           // 医生头像
                        "isLike":""                         // 是否点赞 1:已点赞  0 未点赞
                        "addtime": "2016-03-06 12:45:07",   // 时间
                        "totallikes": "1",                  // 点赞数量
                        "commentcount": "2"                 // 品论数量
                    },
                    ......
                ]
	    },
	    "message": "操作成功"
	}

<br/>

### <a href="#1.4-1">1.4-1 获取热门分类文章列表</a>

###### 请求接口
POST /Article/getHotArticles

###### 请求参数
	{
            page:"" //分页
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": [                                  //没有文章则 data= "";
	        { 
	            "id": "2",                          // 文章id     
	            "title": "北漂打拼固然要紧",         // 文章标题
                    "imgUrl":"http://test.zy360.com/tcmd/Cdashboard/www/upload/articleLogo/160727/11/MC41MDkyMDkwMCAxNDY5NTkxODkz579831557c56b.png"  //文章列表图片 
	            "content": "阿斯蒂芬阿斯蒂芬",       // 文章内容
                    "doctorID":"2"                      // 作者ID      
	            "author": "大师傅",                 // 作者
                    "level":""                         //  医生级别
                    "logo":"http://test.zy360.com/tcmd/Cdashboard/www/upload/articleLogo/160727/11/MC41MDkyMDkwMCAxNDY5NTkxODkz579831557c56b.png"  // 医生头像
	            "addtime": "2016-03-06 12:45:07",   // 时间
	            "totallikes": "1",                  // 点赞数量
	            "commentcount": "2"                 // 品论数量
                    "isLike":""                         // 是否点赞 1:已点赞  0 未点赞
                    
	        },
	        ......
	    ],
	    "message": "操作成功"
	}

<br/>


### <a href="#1.4-2">1.4-2 获取周边分类文章列表</a>

###### 请求接口
POST /Article/getAroundArticles

###### 请求参数
	{
            page:"" //分页
            latitude:"" 精度
            longitude:""    维度
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": [                                  //没有文章则 data= "";
	        { 
	            "id": "2",                          // 文章id     
	            "title": "北漂打拼固然要紧",        // 文章标题
                    "imgUrl":"http://test.zy360.com/tcmd/Cdashboard/www/upload/articleLogo/160727/11/MC41MDkyMDkwMCAxNDY5NTkxODkz579831557c56b.png"  //文章列表图片 
	            "content": "阿斯蒂芬阿斯蒂芬",      // 文章内容
                    "doctorID":"2"                      // 作者ID   
	            "author": "大师傅",                 // 作者
                    "level":"副主任医师",               //  医生级别
                    "logo":""                           // 医生头像
	            "addtime": "2016-03-06 12:45:07",   // 时间
	            "totallikes": "1",                  // 点赞数量
	            "commentcount": "2"                 // 品论数量
                    "isLike":""                         // 是否点赞 1:已点赞  0 未点赞
	        },
	        ......
	    ],
	    "message": "操作成功"
	}

<br/>


### <a href="#1.4-3">1.4-3 提交文章</a>

###### 请求接口
POST /Article/submitArticles

###### 请求参数
	{
            token:""            //token字符串
            categoryID:""       //分类ID
            title:""            //文章标题
            content:""          //内容
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": {
                'articleID':"1"             //文章ID
            },                               
	    "message": "操作成功"
	}

<br/>





### <a name="1.5">1.5 根据文章id獲取評論</a>

###### 请求接口
POST /Article/commentsByarticleId

###### 请求参数
	{
		"articleID":"3",                          // 文章id（必传）
		"page":                                   // 分頁數
                "commentID":""                            // 值存在，则返回第一条为 该条（可为空）。
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": {
	        "list": [
	            {
	                "id": "1",                                // 評論id
	                "uid": "1",                               // 評論者id
	                "articleID": "1",                         // 文章id
	                "content": "我要找到你，不管南北东西啊",  // 評論內容
	                "logo": "",                               // 評論圖片
	                "addtime": "2016-06-06 07:19:16",         // 評論時間
	                "name": "王立新",                         // 評論者姓名
	                "avatar": ""                              // 評論者頭像
	            }，
	            ......
	        ], 
	        "totalcomment": "0",                              // 評論數量
	        "totalcollect": "0"                               // 收藏數量
	    },
	    "message": "操作成功"
	}
<br/>



### <a name="1.6">1.6 文章详情</a>

###### 请求接口
POST /Article/article

###### 请求参数
	{
		"token":TOKEN
		"articleid":"1",		// 文章id	
	}
	
###### 返回值
	{
	    "code": 200,
	    "data":{
	    	"title":"我要找到你",		// 文章标题
	    	"author":"北京中医药大学",		// 发布者
	    	"city":"北京",			// 活动地点
	    	"eventDate":"2017-05-06"	// 活动时间
	    	"totallikes": "1",		// 点赞数量
	    	"content":"xxxxx",		// 文章内容
	    	"reading":"19",			// 阅读量
	    	"islike":"1",				// 是否已点赞 =1,已点赞;=0,未点赞
	    	"addtime":"2016-06-02 06:22:21",//发表时间
	    },
	    "message": "操作成功"
	}
<br/>


### <a name="2.7">2.7 收藏/取消收藏</a>

###### 请求接口
POST /User/collect

###### 请求参数
	{
	"token":TOKEN                                
	"contentid":"1",		// 内容id
	"act":"1",			// 1:收藏 2:取消收藏
	}
	
###### 返回值
	{
	    "code": 200,
	    "data":"",
	    "message": "操作成功"
	}
<br/>


### <a name="1.7">1.7 提交文章评论</a>

###### 请求接口
POST /Article/submitComment

###### 请求参数
	{
                "token":""                                  // 有就传没有不穿
		"articleID":"1",                            // 文章id
                "content":" ",                              // 评论内容
	}
	
###### 返回值
	{
	    "code": 200,
            "data":{
                "commentID":3,          //评论ID
                "docID":"1",            //医生ID
                "content":"呵呵",       //内容
                "addtime":"2016-08-02 12:08:37",    //添加时间
                "docName":"王立新",     //医生姓名
                "docLevel":"副主任医师"  //医生级别
                "avatar":"http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4xMzk3NDQwMCAxNDY0OTM2MDQ35751266f22237.jpg"
            },
            "message": "操作成功"
	}
<br/>

### <a name="1.8">1.8 文章点赞</a>

###### 请求接口
POST /Article/like

###### 请求参数
	{
		"token":TOKEN,
		"articleid":"1",		// 文章id
		"action":"1"			// (取消)点赞	=1,点赞；=2,取消点赞
	}
	
###### 返回值
	{
		"code":200,
		"data":"",
		"message": "操作成功"
	}
<br/>


### <a name="2.0">2.0 获取验证码</a>

##### 【需做加解密处理】
###### 请求接口
POST /User/identifyCode

###### 请求参数
	{
		"phoneNum": "13000000000",				// 手机号
	}
	
###### 返回值
	{
		"code": "200",						// 请求结果状态码
		"data": "",
		"message": "操作成功"
	}
<br/>

### <a name="2.1">2.1 用户登录</a>

##### 【需做加解密处理】
###### 请求接口
POST /User/login

###### 请求参数
	{
		"phone":"13051443735",				// 手机号
		"code":"123654",				// 验证码
		"openid":""					// 微信id
		"longitude":"40.231117",			// 经度
		"latitude":"116.142833"				// 纬度
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": {
		    "name": "",					// 姓名
		    "avatar": "http://xx.xx.xx/xx/xx.png",	// 用户头像
		    "sex": "1",            			// 性别 =1,男 =2,女
		    "age":"29",					// 用户年龄
		    "token": TOKEN,				// token
		}
	    "message": "操作成功"
	}

<br/>

### <a name="2.2">2.2 完善个人资料信息</a>

###### 请求接口
POST /User/updateProfile<br/>

###### 请求参数
	{
		"token":TOKEN,                	// token
		"name": "王立新",              	// 姓名
		"sex": "1",			// 性别, =1,男;=2,女
		"birthday": "2016-06-22",	// 出生日期
		"avatar":DATA,			// 头像Base64
		"province":"北京"          	// 省,从微信接口获取
		"city":"北京"              	// 城市，微信接口获取
		"hospital": "北京协和医院",	// 医院/诊所
		"level": "1",			// 职称, =1,住院医师;=2,主治医师;=3,副主任医师;=4,主任医师;
		"department":"内科",		// 科室
		"introduce":"个人简介",		// 个人简介
		"specialize":"擅长",		// 擅长
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": {
	    	"avatar":"http://xx.xx.xx/www/xx/xx.png"		// 头像
	    },
	    "message": "操作成功"
	}

<br/>

### <a name="2.2-1">2.2-1 提交完善个人资料信息</a>

###### 请求接口
POST /user/submitMyselfMessage<br/>

###### 请求参数
	{
		"token":"",                                     // token
                "name": "王立新",                               // 姓名
	        "gender": "male",                               // 性别
	        "birthdate": "2016-06-22",                      // 出生日期
                "provinceID":""                                 // 省ID
                "cityID":""                                     // 市ID
	        "hospital": "北京协和医院",                     // 医院
	        "level": "1",                                   // 职称1'住院医师',2'主治医师',3'副主任医师',4'主任医师'
                "department":""                                 //科室
                "administration":""                             //行政级别
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": {},    	    
	    "message": "操作成功"
	}

<br/>


### <a name="2.3">2.3 获取用户信息</a>

###### 请求接口
POST /User/profile

###### 请求参数 
	{
		"token":TOKEN					// token
		"openid":OPENID					// 微信id，没有token时传
	}
###### 返回值
	{
        "code":200,
        "data":{
        	"name":"王志新",				// 姓名
        	"phone":"13051443735",			// 手机号
        	"sex":"1",				// 性别 =1,男;=2,女;
        	"birthdate":"1980-03-27",		// 生日
        	"age":"29",				// 年龄
        	"avatar":"http://xx.xx.xx/xx/xx.png",	// 头像
        	"isDoctor":"1",				// 是否医生
        	"hospital":"上海中医医院",			// 所在医院/诊所(患者不返回)
        	"department":"内科",			// 科室(患者不返回)
        	"level":"1",				// 职称(患者不返回) =1,住院;=2,主治;=3,副主任;=4,主任;
        	"introduce":"个人简介“,			// 个人简介(患者不返回)
        	"specialize":"擅长",			// 擅长(患者不返回)
        	"device":[				// 绑定的设备(患者不返回)
        		"deow918203cds",		// 设备编码
        		"lwosk92k4k22k",
        		...
        	]
        },
        "message":"操作成功"
    }

<br/>

### <a name="2.3-1">2.3-1 提交个人主页中个人简介</a>

###### 请求接口
POST /User/setIntroduce

###### 请求参数
	{
		"token":"",                        // token
                "introduce":""                     // 个人简介
	}
###### 返回值
	{
	    "code": 200,
	    "data": [],
	    "message": "操作成功"
	}

<br/>


### <a name="2.3-2">2.3-2 提交个人主页中擅长</a>

###### 请求接口
POST /User/setSpecialize

###### 请求参数
	{
		"token":"",                        // token
                "specialize":""                     // 擅长
	}
###### 返回值
	{
	    "code": 200,
	    "data": [],
	    "message": "操作成功"
	}

<br/>




### <a name="2.4">2.4 我的粉丝</a>

###### 请求接口
POST /User/myFans

###### 请求参数
	{
		"token":"",                           // token
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": [
	        {
	            "fansID": "2",                 // 粉丝id
	            "name": "张丽",                // 粉丝
	            "avatar": "36.jpg",            // 头像 
	            "hospital": "协和",            // 医院
	            "level": "初级职称",           // 职称
                    "authentication": "4",         // 当前粉丝是否认证医生（1,已认证，2未通过，3未认证,4,待审核）
                    "isConcern": 1                 //是否关注粉丝（1 已关注  0未关注）
	        },
	        ......
	    ],
	    "message": "操作成功"
	}

<br/>

### <a name="2.5">2.5 我的收藏</a>

###### 请求接口
POST /User/collections

###### 请求参数
	{
		"token":TOKEN,			// token
		"page:"0"			// 页数 10条/页
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": [
	        { 
	            "contentid": "1",			// 内容id
	            "title":"中西医结合缓解颈椎病引起的头晕",	// 标题
	            "sex":"1"				// 患者性别 =1,男;=2,女
	            "age":"29"				// 患者年龄
	            "disease":"颈椎病"			// 所患疾病
	            "symptoms":"头晕,浑身乏力,"		// 主要症状
	            "diagnosis":"1、颈4-7椎体骨质增生；颈"	// 临床诊断
	        }
	    ],
	    "message": "操作成功"
	}

<br/>


### <a name="2.6">2.6 用户资质认证</a>

###### 请求接口
POST /user/qualification

###### 请求参数
	{
            "token":"",                           // token
            "name":""                             // 用户姓名
            "avatar":""                           // 用户头像 base64
            "picture":""                          // base64数据流 数组
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": [ ],
	    "message": "操作成功"
	}

<br/>

### <a name="2.6-1">2.6-1 判断用户是否认证通过</a>

###### 请求接口
POST /user/isQualification

###### 请求参数
	{
            "token":"",                           // token
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": [
                "isQualification":"1"            // 1 已认证  0未认证
            ],
	    "message": "操作成功"
	}

<br/>

### <a name="2.7">2.7 收藏</a>
###### 请求接口
POST /User/collect

###### 请求参数
	{
		"token":TOKEN,                     // token
		"caseid":"23",                     // 病例id
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": "",
	    "message": "操作成功"
	}

<br/>



### <a name="2.8">2.8 搜索接口(返回医生列表)</a>
###### 请求接口
POST /user/seachDoctorAndArticle

###### 请求参数
	{
		"keywords":"23",                // 搜索关键词（医生名 擅长  ，文章标题， 文章）
		"type":"1",                     // 1返回医生列表 2返回文章列表
		"page":"0",                     // 分页（单页10条）
	}
	
###### 返回值
	{
	    "code": 200,
	     "data": [
                {
                    "docID": "2",           //医生ID
                    "name": "张丽",         //医生名称
                    "avatar": "http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4yNTgwMTMwMCAxNDY0OTM2MDM5575126673f036.jpg", //医生头像
                    "attentionNum": "11",           //关注数量
                    "level": "主治医师",            // 医生级别
                    "specialize": "阿萨德法师打发撒旦法阿斯蒂芬阿斯蒂芬阿斯蒂芬..."         // 医生擅长
                }
                。。。。
            ],
	    "message": "操作成功"
	}

<br/>

### <a name="2.8-1">2.8-1 搜索接口(返回文章列表)</a>
###### 请求接口
POST /user/seachDoctorAndArticle

###### 请求参数
	{
                "token":""                      // 非必传（可有可无 ，判断是否点赞）
		"keywords":"23",                // 搜索关键词（医生名 擅长  ，文章标题， 文章）
		"type":"2",                     // 1返回医生列表 2返回文章列表
		"page":"0",                     // 分页（单页10条）
	}
	
###### 返回值
	{
	    "code": 200,
	     "data": [
                {
                    "articleID":"3",            //文章id
                    "title":"测试",             // 文章标题
                    "content":"有些人爱到忘了形 结果落的一败涂地 有些人永远在憧憬 却只差一步距离 问世间什么最美丽 爱情绝对是个奇迹 我明白会有一颗心 在远方等我靠近 喔我要找到你 不管南北东西 直觉会给我指引 若是爱上你 别问...",
                    "docID":""                  // 医生ID
                    "docName":"王立新",         // 医生名称
                    "avatar":"http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4xMzk3NDQwMCAxNDY0OTM2MDQ35751266f22237.jpg",  //医生头像
                    "level":"副主任医师",       // 医生级别
                    "addtime":"07-27 15:01",    // 添加时间
                    "likes":"0",                // 点赞数量
                    "usercomment":"0",          // 医生评论
                    "imgUrl":"http://test.zy360.com/tcmd/Cdashboard/www/upload/ueditor/image/20160801/1470048217881685.png"         //文章缩略图
                    "isLike":"0"                // 0 未登录
                },
                。。。。
            ],
	    "message": "操作成功"
	}

<br/>


### <a name="2.9">2.9 我的关注</a>
###### 请求接口
POST /user/myAttention

###### 请求参数
	{
		token:""        
	}
	
###### 返回值
        {
            "code": 200,
            "data": {
                "doctor": [     //医生列表
                    {   
                        "docID": "2",                       //医生ID
                        "docName": "张丽",                  //医生姓名
                        "attentions": "11",                 //医生关注数量
                        "level": "主治医师",                //医生级别
                        "avatar": "http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4yNTgwMTMwMCAxNDY0OTM2MDM5575126673f036.jpg"      //医生头想
                    },
                    .......
                ],
                "category": [     //分类列表
                    {
                        "categoryID": "1",                  //分类ID
                        "categoryName": "名家访谈",          //分类名称
                        "attentions": "0",                  // 关注数量
                        "avatar": "http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorCategoryLog/160722/10/MC4xOTQwNTYwMCAxNDY5MTU2MjE357918b792f65b.png"  //分类图片
                    }
                    。。。。。
                ]
            },
            "message": "操作成功"
        }

### <a name="2.91">2.91 我的消息</a>
###### 请求接口
POST /user/getAllUserMessage

###### 请求参数
	{
		token:""        
	}
	
###### 返回值
{
    "code":200,
    "data":[
        {
            "id":"7",                                       //消息ID
            "type":"7",                                     //1关注（messID为0 ） 2评论文章 3赞文章   5回复问答问题  6回复问答评论（给人回复）7 收藏文章 9系统消息（messID为0 activeID为0)
            "replyID": "0",                                 // 评论ID （只有type = 2,5,6才有值）
            "messID":"1",                                   //根据type区分  （其余为文章ID或 问答ID）
            "activeID":"1",                                 //主动用户ID(相当于粉丝ID)
            "avatar":"http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4xMzk3NDQwMCAxNDY0OTM2MDQ35751266f22237.jpg",  //用户头像
            "name":"王立新"                                  //用户姓名
            "content":"评论了您的文章"我要找到你"...",        //消息内容
            "addTime":"2016-08-02 16:16:24",                //消息添加时间
            "isSee":"0",                                    // 0未查看  1以查看 
        },
        {
            "id":"8",
            "type":"7",                                     //1关注 （messID为0 ）2评论文章 3赞文章   5回复问答问题  6回复问答评论（给人回复）7 收藏文章 9系统消息
            "messID":"1",
            "activeID":"1",                                 
            "content":"攒了您的文章"我要找到你"...",
            "addTime":"2016-08-02 16:16:38",
            "isSee":"0",                                    // 0未查看  1以查看 
            "avatar":"http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4xMzk3NDQwMCAxNDY0OTM2MDQ35751266f22237.jpg",
            "name":"王立新"
        },
        {
            "id":"9",
            "type":"7",                                     //1关注（messID为0 ） 2评论文章 3赞文章  5回复问答问题  6回复问答评论（给人回复）7 收藏文章 9系统消息
            "messID":"1",
            "activeID":"1",
            "content":"评论了您的文章"我要找到你"...",
            "addTime":"2016-08-02 16:16:42",
            "isSee":"0",
            "avatar":"http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4xMzk3NDQwMCAxNDY0OTM2MDQ35751266f22237.jpg",  
            "name":"王立新"
        }
    ],
    "message":"操作成功"
}



### <a name="2.91-1">2.91-1 返回我的消息未读消息总数</a>
###### 请求接口
POST /user/getAllNotReadUserMessage

###### 请求参数
	{
		token:""        
	}
	
###### 返回值
        {
            "code":200,
            "data":{
                "num":0                 //未读消息总数
            },
            "message":"操作成功"
        }



### <a name="2.92">2.92 更改消息查看状态</a>
###### 请求接口
POST /user/updateMessageStatus

###### 请求参数
	{
		"token":"",                           // token
		"messageID":"1",                      // 消息ID
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": "操作成功",
	    "message": "操作成功"
	}

<br/>


### <a name="2.93">2.93 单独上传用户头像</a>
###### 请求接口
POST /user/headUpload

###### 请求参数
	{
		"token":"",                         // token
		"picture":"1",                      // 图片base64
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": {
                "imgUrl":""                         //头像地址
            },
	    "message": "操作成功"
	}

<br/>


### <a name="2.94">2.94 获取区域</a>

###### 请求接口
POST /User/provinces

###### 请求参数
    {

    }
	
###### 返回值
    {
    "code":200,
    "data":[
        {
            "areaID":"1",
            "areaName":"北京市",
            "children":[
                {
                    "areaID":"52",
                    "areaName":"平谷区",
                    "parentID":"1"
                },
                {
                    "areaID":"53",
                    "areaName":"密云县",
                    "parentID":"1"
                }
            ]
        },
        {
            "areaID":"3",
            "areaName":"河北省",
            "children":[
                {
                    "areaID":"75",
                    "areaName":"秦皇岛市",
                    "parentID":"3"
                }
            ]
        }
    ],
    "message":"操作成功"
}

###### ***注：北京(1)、上海(21)、天津(42)、重庆(62)四个直辖市显示区，其他省份显示到二级城市即可；***

### <a name="2.95">2.95 我的文章列表</a>
###### 请求接口
POST /user/getUserArticleList

###### 请求参数
	{
		"token":"",                         // token
                "page":"0"                          // 分页 12
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": [
                {
                    "articleID": "2",               //文章ID
                    "docID":""                      //作者ID 
                    "docName": "wwww",              //医生姓名    
                    "level": "主治医师",            //医生级别
                    "avatar": "http://test.zy360.com/tcmd/Cdashboard/www/upload/userLogo/160805/06/MC43NTM5NDQwMCAxNDcwMzkyMDE457a466d2b816c.jpg",      //医生头像
                    "addtime": "03-06 12:45",       //发表时间
                    "title": "北漂打拼固然要紧，且行且珍膝",        //文章标题
                    "likeNum": "3",                 //点赞数
                    "usercomment": "2",             //用户评论数
                    "imgUrl": "http://test.zy360.com/tcmd/Cdashboard/www/upload/articleLogo/160727/11/MC41MDkyMDkwMCAxNDY5NTkxODkz579831557c56b.png",       //文章列表图片
                    "isLike": "1"                   //是否点赞 1已点赞  2未点赞
                },
                ........
            ],
	    "message": "操作成功"
	}

<br/>

### <a name="2.96">2.96 登录以后显示用户的信息</a>
###### 请求接口
POST /user/getUserMsg

###### 请求参数
	{
		"token":"",                         // token
	}
	
###### 返回值
	{
            "code": 200,
            "data": {
                "uid": "2",                         //用户ID
                "name": "wwww",                     //用户姓名
                "avatar": "http://test.zy360.com/tcmd/Cdashboard/www/upload/userLogo/160805/06/MC43NTM5NDQwMCAxNDcwMzkyMDE457a466d2b816c.jpg",      //用户头像
                "authentication": "4",              //是否认证（是否认证1,已认证，2未通过，3未认证,4,待审核）不等于1的都没认证
                "fansNum": "1"                      //粉丝数量
            },
            "message": "操作成功"
        }

<br/>


### <a name="2.97">2.97 意见反馈</a>
###### 请求接口
POST /User/feedback

###### 请求参数
	{
            "token":TOKEN,			// token
            "content":""			//提交内容（255个字以内）
	}
###### 返回值
	{
		"code": 200,
		"data": "",
		"message": "操作成功"
   }

<br/>


### <a name="2.98">2.98 登录用户查看医生的文章列表</a>
###### 请求接口
POST /user/getUserALLArticle

###### 请求参数
	{
            "token":"",                     // token（可不传）
            "uid":""                        // 相当于要查看的医生ID 
            "page"：""                      // 分页
	}
###### 返回值
	{
            "code": 200,
            "data": [
                {
                    "articleID":"2",            //文章ID
                    "docID":""                  //文章作者ID
                    "docName":"wwww",           //医生姓名
                    "level":"主治医师",         //医生级别
                    "avatar":"http://test.zy360.com/tcmd/dashboard/www/upload/userLogo/160805/06/MC43NTM5NDQwMCAxNDcwMzkyMDE457a466d2b816c.jpg",    //医生头像
                    "addtime":"03-06 12:45",    //添加时间
                    "title":"北漂打拼固然要紧，且行且珍膝", //文章标题
                    "likeNum":"3",              //点赞
                    "usercomment":"2",          //评论数量
                    "imgUrl":"http://test.zy360.com/tcmd/Cdashboard/www/upload/articleLogo/160727/11/MC41MDkyMDkwMCAxNDY5NTkxODkz579831557c56b.png",    //文章列表图片
                    "isLike":"1"                //是否点赞 0未登录 不显示，1已点赞 ，2未点赞
                },
                。。。
            ]
            "message": "操作成功"
        }

<br/>


### <a name="2.99">2.99 获取医生的所有医友评价列表</a>
###### 请求接口
POST /user/getALLDoctorComment

###### 请求参数
	{
            "uid":"",                     // 被查看的用户ID
            "page":""                       // 分页12
	}
###### 返回值
	{
            "code": 200,
            "data": [
                {
                    "commentID":"34",          //评论ID
                    "articleID":"2",           //评论文章
                    "docID":""                 //医生ID
                    "docName":"wwww",          //评论医生
                    "avatar":"http://test.zy360.com/tcmd/Cdashboard/www/upload/userLogo/160805/06/MC43NTM5NDQwMCAxNDcwMzkyMDE457a466d2b816c.jpg",   //头像
                    "level":"主治医师",         //医生级别
                    "content":"我来来",         //评论内容
                    "addtime":"2016-08-03 13:53"    //时间
                },
                。。。
            ]
            "message": "操作成功"
        }

<br/>

### <a name="2.10">2.10 根据常见病获取医生列表</a>
###### 请求接口
POST /user/doctorsBySymptom

###### 请求参数
	{
		token:""  
		sid:1						// 常见病id      
		page:0						// 页数，从0开始
	}
	
###### 返回值
        {
            "code": 200,
            "data": {
                "doctor": [     //医生列表
                    {   
                        "docID": "2",                      //医生ID
                        "docName": "张丽",                  //医生姓名
                        "attentions": "11",                //医生关注数量
                        "level": "主治医师",                 //医生级别
                        "avatar": "http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4yNTgwMTMwMCAxNDY0OTM2MDM5575126673f036.jpg"      //医生头像
                    },
                    .......
                ],
            },
            "message": "操作成功"
        }
<br/>

### <a name="2.11">2.11 我的会诊列表</a>
###### 请求接口
POST /User/consultations

###### 请求参数
	{
		"token":"",                         // token
       	"page":"0"                          // 分页, 10/每页
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": [
	    	{
	    	"id": "2",					// ID
	    	"doctor_name": "王五",				// 会诊医生姓名
	    	"doctor_level": "主治医师",			// 医生级别
	    	"doctor_avatar": "http://xx.xx.xx/www/xx/xx.jpg",// 医生头像
	    	"doctor_hospital":"上海中医医院",			// 所在医院/诊所
	    	"doctor_department":"内科",			// 医生科室
	    	"reportid":"1BCFD13FB65745AA969A747D3563B875",	// 检查报告id（非报告解读，返回空）
	    	"patient_sex":"1",				// 患者性别, =1,男;=2,女
	    	"patient_age":"29",				// 患者年龄
	    	"content":"52岁患头痛2月了，一直治疗...",		// 会诊内容
	    	"addtime": "03-06 12:45",       		// 问诊时间
	    	"fromme":"1",					// 是否我发起的 =1,是;=0,不是
	    	}
	    	........
	    ],
	    "message": "操作成功"
	}

<br/>

### <a name="2.12">2.12 会诊详情</a>
###### 请求接口
POST /User/consultation

###### 请求参数
	{
		"token":"",                         // token
       	"consultationid":"11"               // 会诊id
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": {
	    	"fromme": "1",						// 是否我提问的 =1,是;=0,否;
	    	"doctor_avatar": "http://xx.xx.xx/www/xx/xx.jpg",	// 提问医生头像
	    	"patient_sex":"1",					// 患者性别, =1,男;=2,女
	    	"patient_age":"29",					// 患者年龄
	    	"content":"52岁患头痛2月了，一直治疗...",			// 会诊内容
	    	"pics": [						// 图片
	    		"http://xx.xx.xx/xx/xx.png",
	    		"http://xx.xx.xx/xx/xx.png",
	   			...
	    	],
	    	"addtime":"1489722366",					// 发布时间
	    	
	    	detail:[						// 会诊记录
	    		"fromme": "1",					// 是否我的回复
	    		"avatar": "http://xx.xx.xx/www/xx/xx.jpg",	// 医生头像
	    		"content":"稍等，我仔细看下...",			// 回复内容
	    		"pic":"http://xx.xx.xx/www/xx/xx.png",		// 回复的图片
	    		"addtime":"1489722366"				// 回复时间
	    	]
	    },
	    "message": "操作成功"
	}

<br/>

### <a name="2.13">2.13 绑定设备</a>
###### 请求接口
POST /User/bind

###### 请求参数
	{
		"openid":OPENID,				// 微信id
		"patientid":"12",			// 患者id（患者绑定设备时传）
		"device":"dkw8duej374jd8k"			// 设备编码
	}
###### 返回值
	{
	    "code": 200,
	    "data": "",
	    "message": "操作成功"
	}

<br/>

### <a name="2.14">2.14 用户注册</a>
###### 请求接口
POST /User/register

###### 请求参数
	{
		"openid":OPENID,			// 微信id
		"phone":"13051443735",			// 手机号
		"code":"123456",			// 验证码
		"identity":"1",				// 身份 =1,医生;=2,患者;
		"name":"王立新",				// 姓名
		"sex":"1",				// 性别 =1,男;=2,女;
		"birthday":"1990-01-01",		// 生日
		"hospital":"上海中医医院",			// 所属医院/诊所（可选）
		"province":"上海",			// 地区(微信接口获取)
		"city":"上海"				// 城市（微信接口获取）
		"longitude":"40.231117",		// 经度
		"latitude":"116.142833"			// 纬度
	}
###### 返回值
	{
	    "code": 200,
	    "data": "",
	    "message": "操作成功"
	}

<br/>

### <a name="2.15">2.15 就诊人列表</a>
###### 请求接口
POST /User/patients

###### 请求参数
	{
		"openid":OPENID,			// 微信id
	}
###### 返回值
	{
	    "code": 200,
	    "data": [					// 就诊人列表
	    	"patientid": "1",			// 就诊人id
	    	"name": "李飞",				// 就诊人姓名
	    	"sex":"1",				// 就诊人性别 =1,男;=2，女
	    	"age":"29",				// 年龄
	    ],
	    "message": "操作成功"
	}

<br/>

### <a name="2.16">2.16 添加就诊人</a>
###### 请求接口
POST /User/addPatient

###### 请求参数
	{
		"openid":OPENID,			// 微信id
		"name":"王立新",				// 姓名
		"sex":"1",				// 性别 =1,男;=2,女;
		"birthday":"1990-01-01",		// 生日
	}
###### 返回值
	{
	    "code": 200,
	    "data": "",
	    "message": "操作成功"
	}

<br/>

### <a name="3.1">3.1 提交问题</a>
###### 请求接口
POST /Question/submitQuestion

###### 请求参数
	{
		"token":"",                           // token
		"content":"23",                       // 问题内容
		"title":                              // 问题标题
		"logo":                               // 问题图片
                "picture":""                          // base64一维数组(最多5张图片)
	}
	
###### 返回值
	{
	    "code": 200,
	    "data":{
                "questionID":"1"                    //提交的问题ID
            },
	    "message": "操作成功"
	}

<br/>

### <a name="3.2">3.2 问题列表</a>
###### 请求接口
POST /Question/Questions

###### 请求参数
	{
                "keywords":""                 //  关键词（可传可不穿）
		"page":                       //  分页 0 第一页 1第二页。。。
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": [ 
                {
                    "id":"3",                               // 问题id
                    "uid":"1",                              // 用户ID
                    "title":"",                             // 标题
                    "content":"asdfas",                     // 内容
                    "addTime":"1个月前",                     // 提问时间
                    "reviewsNumber":"0",                    // 评论数量
                    "name":"王立新",                         //  用户名称
                    "avatar":"http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4xMzk3NDQwMCAxNDY0OTM2MDQ35751266f22237.jpg",
                                                            // 用户头像
                    "gender":"male",                        // 性别 male男 female女
                    "imgUrl":[                              // 问答上传图片 
                        "http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4yNTgwMTMwMCAxNDY0OTM2MDM5575126673f036.jpg"
                        。。。。
                    ]
                },
                ......
            ],
	    "message": "操作成功"
	}

<br/>

### <a name="3.3">3.3 问答详情评论列表</a>
###### 请求接口
POST /Question/questionDetail

###### 请求参数
	{
		"questionID":                 //  问题id    （必传）
		"page":                       //  分页 0 第一页 1第二页。。。
                "replyID":""                  //  值存在，则返回第一条为 该条（可为空）。
	}
	
###### 返回值
	
	{
	    "code": 200,
	    "data": [
                {
                    "id":"7",                               //回复ID
                    "questionID":"1",                       // 问答ID
                    "uid":"1",                              // 回复人id
                    "name":"王立新",                         // 回复人姓名  （没有 ，可能为电话）
                    "avatar":"http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4xMzk3NDQwMCAxNDY0OTM2MDQ35751266f22237.jpg",  	   // 回复人头像
                    "addTime":"2016-06-24 08:23:25",        //回复时间
                    "content":"阿斯蒂芬",                   // 回复内容
                    "replyuid":"1",                        //  对谁回复    (0就是回复问题，>0 回复用户)
                    "replyName":"王立新"                    // 如果replyuid >0 则为回复用户的姓名(姓名不存在用电话) 如果replyuid == 0 则为空 (没有，可能为电话)
                },
	        ......
	    ],
	    "message": "操作成功"
	}
	

<br/>


### <a name="3.5">3.5 对问题评论</a>
###### 请求接口
POST /Question/replyQuestion

###### 请求参数
	{
		"token":                 //  token字符串
		"questionID":            //  问题id
                "replyuid"               //  0：对问答进行评论   >0 则对用户进行评论
		"content":               //  回复内容
	}
	
###### 返回值
	
	{
	    "code": 200,
	    "data": "操作成功",
	    "message": "操作成功"
	}
	
<br/>

### <a name="3.4">3.4 我的问题</a>
###### 请求接口
POST /Question/userQuestions

###### 请求参数
	{
		"token":                 //  token字符串
		"page":                  //  分页 0 第一页 1第二页。。。
	}
	
###### 返回值
	{
	    "code": 200,
	    "data": [ 
                {
                    "id":"3",                               // 问题id
                    "uid":"1",                              // 用户ID
                    "title":"",                             // 标题
                    "content":"asdfas",                     // 内容
                    "addTime":"1个月前",                    // 提问时间
                    "reviewsNumber":"0",                    // 评论数量
                    "name":"王立新",                        //  用户名称
                    "avatar":"http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4xMzk3NDQwMCAxNDY0OTM2MDQ35751266f22237.jpg",
                                                            // 用户头像
                    "gender":"male",                        // 性别 male男 female女
                    "imgUrl":[                              // 问答上传图片 
                        "http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4yNTgwMTMwMCAxNDY0OTM2MDM5575126673f036.jpg"
                        。。。。
                    ]
                },
                ......
            ],
	    "message": "操作成功"
	}
	

<br/>

### <a name="3.6">3.6 获取问答详情界面问题信息</a>
###### 请求接口
POST /Question/getOneQuestion

###### 请求参数
	{
		"questionID":"1"                            //  问答ID
	}
	
###### 返回值
	{
	    "code": 200,
	    "data":{ 
                    "id":"3",                               // 问题id
                    "uid":"1",                              // 用户ID
                    "title":"",                             // 标题
                    "content":"asdfas",                     // 内容
                    "addTime":"1个月前",                    // 提问时间
                    "reviewsNumber":"0",                    // 评论数量
                    "name":"王立新",                        //  用户名称
                    "avatar":"http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4xMzk3NDQwMCAxNDY0OTM2MDQ35751266f22237.jpg",
                                                            // 用户头像
                    "gender":"male",                        // 性别 male男 female女
                    "imgUrl":[                              // 问答上传图片 
                        "http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4yNTgwMTMwMCAxNDY0OTM2MDM5575126673f036.jpg"
                        。。。。
                    ]
            },
	    "message": "操作成功"
	}

<br/>

### <a name="3.7">3.7 常见病列表</a>
###### 请求接口
POST /Symptom/symptoms

###### 请求参数
	{
		"num":"1"                            //  返回的常见病数量，=0为返回所有
		"homeShow":"1"						 // 是否显示在首页的，=1为是，=0为不是
	}
	
###### 返回值
	{
	    "code": 200,
	    "data":{ 
                    "id":"3",                              // id
                    "name":"",                             // 名称
                    "icon":"http://...../../..jsp",        // 图标
            },
	    "message": "操作成功"
	}

<br/>

### <a name="3.8">3.8 常见病详情</a>
###### 请求接口
POST /Symptom/symptom

###### 请求参数
	{
		"sid":"1"                            //  常见病id
	}
	
###### 返回值
	{
	    "code": 200,
	    "data":{ 
                    "name":"",                             // 名称
                    "icon":"http://...../../..jpg",        // 图标
                    "desc":"xxxxxxxxx",	                   // 常见表现
            },
	    "message": "操作成功"
	}

<br/>

### <a name="3.9">3.9 医生问诊列表</a>
###### 请求接口
POST /Doctor/inquirys

###### 请求参数
	{
		"token":TOKEN,			// 用户token
		"key":"李飞"			// 搜索关键词
		"page":"0"			// 页数
	}
	
###### 返回值
	{
	    "code": 200,
	    "data":[
	    	{
	    		"id":"123",				// 如果fr为空，则为本济患者端id；如不为空，则为诊所CIS的问诊id
	    		"name":"李飞",				// 患者姓名
	    		"avatar":"http://...../../..jpg",	// 患者头像
	    		"sex":"1",	                   	// 患者性别 =1,男;=2,女
	    		"age":"39",				// 患者年龄
	    		"content":"",				// 最后一次回复的内容 =0时为图片
	    		"replytime":"1489722366",		// 最后一次回复的时间
	    		"fr":"",				// CIS系统的诊所id；为空则为本济患者端问诊；
          	},
          	......
       ]
	    "message": "操作成功"
	}

<br/>

### <a name="3.10">3.10 问诊详情</a>
###### 请求接口
POST /Doctor/inquiryDetail

###### 请求参数
	{
		"token":TOKEN,			// 用户token
		"inquiryid":12			// 问诊id
		"page":0			// 当前页数
	}
	
###### 返回值
	{
	    "code": 200,
	    "data":{
	    	"detail":[
	    		{
	    		"avatar":"http://...../../..jpg",        // 回复人的头像
	    		"content":"xxxxxxxxx",			// 回复的内容
	    		"pics":["http://...../../..jpg",....],	// 回复的图片
	    		"fromme":0,				// =1，为我回复的；=0，对方回复
	    		"replytime":"1489722366",		// 回复的时间
          		},
          		......
       	]
       	"patientid":"23",				// 患者id
       	"name":"姓名",					// 患者姓名
       	"age":"29",					// 患者年龄
       	"sex":"1"					// 患者性别 =1,男;=2,女;
       },
	    "message": "操作成功"
	}

<br/>

### <a name="3.11">3.11 患者提交问诊</a>

###### 请求接口
POST /Inquiry/inquiry

###### 请求参数
	{
		"token":TOKEN,				// token
		"name":"李飞",				// 患者姓名
		"sex":"1",				// 患者性别 =1,男;=2,女;（如果是医生发起，不传）
		"birthday":"1987-03-01",// 患者生日
		"content":"52岁患头痛2月了，一直治疗.."	// 问诊内容
		"pics":DATA,				// 问诊图片(Base64一维数组,最多5张)；如通过诊所公众号问诊且有上传图片，传'1'即可
		"clinic":"DF98I8K2L90K26",		// CIS系统诊所id，如从诊所公众号提交问诊时传
		"inquiryid":"32"			// CIS系统问诊id
		"avatar":"http://xxx/xx/xx.png"		// 患者头像，通过诊所公众号提交时需要传；无头像为空；
	}
###### 返回值
	{
	    "code": 200,
	    "data": "",
	    "message": "操作成功"
	}

<br/>

### <a name="3.12">3.12 问诊回复</a>

###### 请求接口
POST /Inquiry/reply

###### 请求参数
	{
		"token":TOKEN,				// token
		"isPatient":"1",			// 是否患者回复 =1,是 =0,否
		"inquiryid":"21",			// 问诊id；clinic不为空时（通过诊所公众号回复），值为CIS系统的问诊id
		"content":"52岁患头痛2月了，一直治疗.."	// 回复内容
		"pic":DATA,				// 回复的图片（每次一张）；如通过诊所公众号回复且有值，传'1'
		"clinic":"DF98I8K2L90K26",		// CIS系统诊所id，如从诊所公众号提交问诊时传
	}
###### 返回值
	{
	    "code": 200,
	    "data": "",
	    "message": "操作成功"
	}

<br/>

### <a name="3.13">3.13 问诊列表（患者端）</a>
###### 请求接口
POST /Patient/inquirys

###### 请求参数
	{
		"token":TOKEN,			// 用户token
		"page":"0"			// 页数
	}
	
###### 返回值
	{
	    "code": 200,
	    "data":[
	    	{
	    		"id":"123",				// 问诊id
	    		"name":"李飞",				// 最后一次回复的医生姓名，无医生回复为患者问诊姓名
	    		"avatar":"http://...../../..jpg",	// 头像
	    		"sex":"1",	                   	// 患者性别 =1,男;=2,女(如已有医生回复，不返回)
	    		"age":"39",				// 患者年龄（如已有医生回复，不返回）
	    		"content":"",				// 最后一次回复的内容 =0时为图片
	    		"replytime":"1489722366",		// 最后一次回复的时间
	    		"level":"1"				// 医生级别,=1,住院;=2,主治;=3,副主任;=4,主任;（无医生回复不返）
          	},
          	......
       ]
	    "message": "操作成功"
	}

<br/>

### <a name="3.14">3.14 问诊详情（患者端）</a>
###### 请求接口
POST /Patient/inquiryDetail

###### 请求参数
	{
		"token":TOKEN,			// 用户token
		"inquiryid":12			// 问诊id
		"page":0			// 当前页数
	}
	
###### 返回值
	{
	    "code": 200,
	    "data":{
	    	"detail":[
	    		{
	    		"id":"123",				// 回复人id
	    		"avatar":"http://...../../..jpg",       // 回复人的头像
	    		"content":"xxxxxxxxx",			// 回复的内容
	    		"pics":["http://...../../..jpg",....],	// 回复的图片
	    		"fromme":0,				// =1，为我回复的；=0，对方回复
	    		"replytime":"1489722366",		// 回复的时间
          		},
          		......
       	]
       	"age":"29",					// 患者年龄
       	"sex":"1"					// 患者性别 =1,男;=2,女;
       	"content":"xxx",				// 问诊内容
       },
	    "message": "操作成功"
	}

<br/>

### <a name="4.1">4.1 点击关注接口（医生和分类 用同一个接口）</a>

###### 请求接口
POST /User/setAttention

###### 请求参数
	{
		"token":"",                           // token
		"attentionID":                        // type= 1 为医生ID 2为分类ID
                "type":""                             // 1医生  2分类
	}
###### 返回值
	{
	    "code": 200,
	    "data":"操作成功",
	    "message": "操作成功"
	}

<br/>


### <a name="4.2">4.2 返回所有已认证的医生</a>

###### 请求接口
POST /User/getAllDoctor

###### 请求参数
	{
            "token":"",             // token
            "page":""               //分页
	}
###### 返回值
	{
            "code":200,
            "data":[
                {
                    "id":"2",                   //医生ID
                    "name":"张丽",              //医生姓名
                    "avatar":"http://test.zy360.com/tcmd/Cdashboard/www/upload/doctorlog/160603/02/MC4yNTgwMTMwMCAxNDY0OTM2MDM5575126673f036.jpg",      //医生头像
                    "attentionNum":"10",        //关注人数
                    "level":"主治医师",          //医生级别
                    "specialize":"阿萨德法师打发撒旦法阿斯蒂芬阿斯蒂芬阿斯蒂芬..."      //擅长
                }
                。。。。。
            ],
            "message":"操作成功"
        }

<br/>

### <a name="5.1">5.1 病例列表</a>

###### 请求接口
POST /Cases/cases

###### 请求参数
	{
		"token":TOKEN,			// token
		"key":"颈椎"			// 搜索关键词
		"page:"0"			// 页数 10条/页
	}
###### 返回值
	{
	    "code": 200,
	    "data":[
	    	{
	    		"id":"2",				// id
	    		"title":"中西医结合缓解颈椎病引起的头晕",	// 标题
	    		"sex":"1",				// 患者性别 =1,男;=2,女
	    		"age":"29",				// 患者年龄
	    		"disease":"颈椎病",			// 所患疾病
	    		"symptoms":"头晕,浑身乏力,"		// 主要症状
	    		"diagnosis":"1、颈4-7椎体骨质增生；颈"	// 临床诊断
	    	}
	    	。。。。。
	    ],
	    "message": "操作成功"
	}

<br/>

### <a name="5.2">5.2 病例详情</a>

###### 请求接口
POST /Cases/case

###### 请求参数
	{
		"token":TOKEN,                // token
		"caseid":                     // 病例id
	}
###### 返回值
	{
	    "code": 200,
	    "data": {
	    	"title":"中西医结合缓解颈椎病引起的头晕",	// 标题
	    	"sex":"1",				// 患者性别 =1,男;=2,女
	    	"age":"29",				// 患者年龄
	    	"disease":"颈椎病",			// 所患疾病
	    	"symptoms":"头晕,浑身乏力,"		// 主要症状
	    	"present_illness":"4月前不明原因出现.."	// 现病史
	    	"past_illness":"否认疫区、疫水接触史.."	// 既往史
	    	"personal_medical":"出生并长大于原籍.."	// 个人史
	    	"family_medical":"否认家族遗传病、传染.."	// 家族史
	    	"examination":"血常规、尿常规、大便常.."	// 医学检查
	    	"diagnosis":"1、颈4-7椎体骨质增生；颈.."	// 临床诊断
	    	"prescription":"锁阳固精丸 2盒六味地黄丸 1瓶.."	// 处方
	    	"otc":[			// 处方用药
	    		{
	    			"id":"12",		// 药品id
	    			"name":"六味地黄丸",	// 药品名称
	    			"icon":"http://xx.xx.xx/xx/xx/xx.png",	// 药品图标
	    			"company":"河南省宛西制药股份有限公司",	// 制药企业
	    			"shop":"1",		// 是否可进货 =0，不可；=1，可；
	    		}
	    		......
	    	],
	    	"orders":"注意休息，多喝水，不要劳累"	// 医嘱
	    	"collection":"1",			// 收藏状态 =0，未收藏；=1，已收藏；
	    },
	    "message": "操作成功"
	}

<br/>

### <a name="5.3">5.3 药品列表</a>

###### 请求接口
POST /Cases/drugs

###### 请求参数
	{
		"token":TOKEN,		// token
		"key":"颈椎病"		// 搜索关键词，药品或疾病
		"page":"0"		// 页数 10条/页
	}
###### 返回值
	{
	    "code": 200,
	    "data":[
	    	{
	    	"id":"12",				// 药品id
	    	"name":"六味地黄丸",			// 药品名称
	    	"icon":"http://xx.xx.xx/xx/xx/xx.png",	// 药品图标
	    	"company":"河南省宛西制药股份有限公司",	// 制药企业
	    	"effect":"温肾固精。用于肾阳不足..",	// 功效主治
	    	"contraindications":"尚不明确",		// 禁忌
	    	"shop":"1",				// 是否可进货 =0，不可；=1，可；
	    	}
	    		......
	    ],
	    "message": "操作成功"
	}

<br/>

### <a name="5.4">5.4 药品详情</a>

###### 请求接口
POST /Cases/drug

###### 请求参数
	{
		"token":TOKEN,		  // token
		"drugid":"12"		  // 药品id
	}
###### 返回值
	{
	    "code": 200,
	    "data":{
	    	"name":"六味地黄丸",			// 药品名称
	    	"icon":"http://xx.xx.xx/xx/xx/xx.png",	// 药品图标
	    	"company":"河南省宛西制药股份有限公司",	// 制药企业
	    	"component":"锁阳、肉苁蓉(蒸)、制巴..",	// 成分
	    	"effect":"温肾固精。用于肾阳不足..",	// 功效主治
	    	"usage":"口服。一次1丸，一日2次。",		// 用法用量
	    	"untoward_effect":"尚不明确",		// 不良反应
	    	"contraindications":"尚不明确",		// 禁忌
	    	"attention":"1.忌不易消化食物..",		// 注意事项
	    	"interactions":"如与其他药物同时使用可能会发..",// 药物相互作用
	    	"approval_number":"国药准字Z11020033",	// 批准文号
	    	"price":"0.0",				// 购买价格
	    	"discount":"0.9"			// 购买折扣
	    },
	    "message": "操作成功"
	}

<br/>

### <a name="6.1">6.1 医生列表</a>

###### 请求接口
POST /Doctor/doctors

###### 请求参数
	{
		"token":TOKEN,		// token
		"key":"王五"		// 搜索关键词
	}
###### 返回值
	{
	    "code": 200,
	    "data":[
	    	{
	    	"id":"12",				// 医生id
	    	"name":"王五",				// 医生名称
	    	"avatar":"http://xx.xx.xx/xx/xx/xx.png",	// 医生头像
	    	"level":"主任医师",			// 职称
	    	"hospital":"上海中医医院",			// 所在医院/诊所
	    	"department":"内科",			// 科室
	    	"specialize":"伙伴们介绍了如何在终结...",	// 擅长
	    	}
	    		......
	    ],
	    "message": "操作成功"
	}

<br/>

### <a name="6.2">6.2 医生详情</a>

###### 请求接口
POST /Doctor/doctor

###### 请求参数
	{
		"token":TOKEN,		  // token
		"doctorid":"12"		  // 医生id
	}
###### 返回值
	{
	    "code": 200,
	    "data":{
	    	"name":"王五",				// 医生名称
	    	"avatar":"http://xx.xx.xx/xx/xx/xx.png",	// 医生头像
	    	"level":"主任医师",			// 职称
	    	"hospital":"上海中医医院",			// 所在医院/诊所
	    	"department":"内科",			// 科室
	    	"introduce":"内科主任医师...",		// 简介
	    	"specialize":"伙伴们介绍了如何在终结...",	// 擅长
	    },
	    "message": "操作成功"
	}

<br/>


### <a name="6.3">6.3 提交会诊</a>

###### 请求接口
POST /Doctor/consultation

###### 请求参数
	{
		"token":TOKEN,				// token
		"consultation":"21",			// 接收会诊的医生id
		"patient_sex":"1",			// 患者性别 =1,男;=2,女;
		"patient_age":"29",			// 患者年龄
		"content":"52岁患头痛2月了，一直治疗.."	// 会诊内容
		"pics":DATA,				// 会诊图片(Base64一维数组,最多5张)
		"reportid":"1BCFD13FB65745AA969A747D3563B875"	// 检查报告id（专家解读时传）
	}
###### 返回值
	{
	    "code": 200,
	    "data": "",
	    "message": "操作成功"
	}

<br/>

### <a name="6.4">6.4 会诊回复</a>

###### 请求接口
POST /Doctor/consultationReply

###### 请求参数
	{
		"token":TOKEN,				// token
		"consultationid":"21",			// 会诊id
		"content":"52岁患头痛2月了，一直治疗.."	// 会诊内容
		"pic":DATA,				// 回复的图片（每次一张）
	}
###### 返回值
	{
	    "code": 200,
	    "data": "",
	    "message": "操作成功"
	}

<br/>

### <a name="7.1">7.1 检查报告列表</a>

###### 请求接口
POST /Medical/reports

###### 请求参数
	{
		"token":TOKEN,		// token
		"page":"0"		// 页数 10条/页
	}
###### 返回值
	{
	    "code": 200,
	    "data":[
	    	{
	    	"id":"1BCFD13FB65745AA969A747D3563B875",// 报告id
	    	"name":"王波",				// 患者姓名
	    	"sex":"1",				// 患者性别
	    	"age":"29",				// 患者年龄
	    	"checktime":"14091293948",		// 检查日期
	    	}
	    		......
	    ],
	    "message": "操作成功"
	}

<br/>

### <a name="7.2">7.2 检查报告详情</a>

###### 请求接口
POST /Medical/report

###### 请求参数
	{
		"token":TOKEN,		  // token
		"reportid":"1BCFD13FB65745AA969A747D3563B875"	// 报告id
	}
###### 返回值
	{
	    "code": 200,
	    "data":{
	    	"name":"王波",				// 患者姓名
	    	"checktime":"14091293948",		// 检查时间
	    	"sex":"1",				// 患者性别
	    	"age":"29",				// 患者年龄
	    	"report":{				// 报告详情（不同检查项目不同，按顺序显示）
	    		"心率":"100",
	    		"心房率":"100次/分",
	    		"心室率":"100次/分",
	    		"电轴":"100",
	    		"P-R间期":"1.8秒",
	    		"QRS间期":"1.8秒",
	    		"Q-T间期":"1.8秒",
	    		"诊断":"整体...."
	    		"心电图":"1"			// 是否有心电图数据；=1，为有；无该字段或=0，为无；
	    	},
	    	"diseases":[				// 匹配的疾病；如果没有不返回；
	    	{
	    		"id":"12",			// 疾病id
	    		"name":"头痛",			// 疾病名称
	    		"abnormals":{			// 异常的指标
	    			"HGB":{			// 指标名称
	    				"val":"37.1",		// 指标的值
	    				"result":"1"		// 偏高或偏低 =1,偏高;=2,偏低;=3,异常
	    			}
	    			......
	    		}
	    	},
	    	......
	    	]
	    },
	    "message": "操作成功"
	}

<br/>

### <a name="7.3">7.3 心电图数据</a>

###### 请求接口
POST /Medical/ecg

###### 请求参数
	{
		"token":TOKEN,					// token
		"idcard":"1101xxxxxxxx",			// 身份证号（诊所公众号请求时传）
		"reportid":"1BCFD13FB65745AA969A747D3563B875"	// 报告id
		"clinic":"DF98I8K2L90K26"			// 诊所id（没有不传）
	}
###### 返回值
	{
	    "code": 200,
	    "data":[
	    	"V4":"8 9 7 4 8 12 12 15 19 16 16 15 15 13 15 21 13 10 8 11 4 2 -4 0 5 7 ...",			// 指标及对应的值
	    	"V5":"8 9 7 4 8 12 12 15 19 16 16 15 15 13 15 21 13 10 8 11 4 2 -4 0 5 7 ...",
	    	.....
	    ],
	    "message": "操作成功"
	}

<br/>


### <a name="7.4">7.4 检查报告列表(CIS)</a>

###### 请求接口
POST /Medical/cisReports

###### 请求参数
	{
		"clinic":CLINIC,		// 诊所id
		"page":"0"			// 页数 20条/页
		"key":"王波"			// 搜索词
	}
###### 返回值
	{
	    "code": 200,
	    "data":
	    {
	    	"total":"23",					// 总数量
	    	"reports":[ {
	    		"id":"1BCFD13FB65745AA969A747D3563B875",// 报告id
	    		"name":"王波",				// 患者姓名
	    		"sex":"1",				// 患者性别
	    		"age":"29",				// 患者年龄
	    		"checktime":"14091293948",		// 检查日期
	    		}
	    		......
	    	],
	    }
	    "message": "操作成功"
	}

<br/>

### <a name="7.5">7.5 检查报告详情(CIS)</a>

###### 请求接口
POST /Medical/cisReport

###### 请求参数
	{
		"clinic":CLINIC,		  // 诊所id
		"reportid":"1BCFD13FB65745AA969A747D3563B875"	// 报告id
	}
###### 返回值
	{
	    "code": 200,
	    "data":{
	    	"name":"王波",				// 患者姓名
	    	"checktime":"14091293948",		// 检查时间
	    	"sex":"1",				// 患者性别
	    	"age":"29",				// 患者年龄
	    	"report":{				// 报告详情（不同检查项目不同，按顺序显示）
	    		"心率":"100",
	    		"心房率":"100次/分",
	    		"心室率":"100次/分",
	    		"电轴":"100",
	    		"P-R间期":"1.8秒",
	    		"QRS间期":"1.8秒",
	    		"Q-T间期":"1.8秒",
	    		"诊断":"整体...."
	    		"心电图":"1"			// 是否有心电图数据；=1，为有；无该字段或=0，为无；
	    	}
	    },
	    "message": "操作成功"
	}

<br/>

### <a name="7.6">7.6 身体各部位及主症列表</a>

###### 请求接口
POST /Medical/bodyPartsSymptoms

###### 请求参数
	{
		"token":TOKEN
	}
###### 返回值
	{
	    "code": 200,
	    "data":{
	    	"40": {						// 身体部位id
	    		"bid":"40",				// 部位id
	    		"name":"全身",				// 部位名称
	    		"pid":"0",				// 父级部位id
	    		"sort":"1",				// 排序
	    		"sortcode":"0001",			// 编号
	    		"parts":{				// 子部位
	    			"65": {				// 子部位id
	    				"bid":"40",		// 部位id
	    				"name":"全身",		// 部位名称
	    				"pid":"0",		// 父级部位id
	    				"sort":"1",		// 排序
	    				"sortcode":"0001",	// 编号
	    				"symptoms": {		// 症状
	    					"62":"多汗",
	    					"419":"夜间盗汗",
	    					......
	    				}
	    			}
	    			......
	    		}
	    	}
	    	......
	    },
	    "message": "操作成功"
	}

<br/>

### <a name="7.7">7.7 可能疾病列表</a>

###### 请求接口
POST /Medical/diseases

###### 请求参数
	{
		"token":TOKEN,
		"reportid":"1BCFD13FB65745AA969A747D3563B875"	// 报告id
		"symptoms":"40-65,40-419"		// 选择的症状
		"page":"0"					// 当前页
	}
###### 返回值
	{
	    "code": 200,
	    "data":[
	    {
	    	"id":"12",			// 疾病id
	    	"name":"头痛",			// 疾病名称
	    	"remark":"xxxx",		// 疾病介绍
	    },
	    ......
	    ],
	    "message": "操作成功"
	}

<br/>

### <a name="8.1">8.1 课程列表</a>

###### 请求接口
POST /Article/courses

###### 请求参数
	{
		"token":TOKEN,		// token
		"page":"0"		// 页数 10条/页
		"key":"王波"		// 搜索关键词或医生姓名
	}
###### 返回值
	{
	    "code": 200,
	    "data":[
	    	{
	    	"id":"12",				// 课程id
	    	"title":"经方新传",			// 检查名称
	    	"author":"王波",				// 作者
	    	"views":"2981",				// 观看次数
	    	"price":"0.00",				// 价格, =0,显示为’免费‘
	    	"pic":"http://xx../xx/xx.png",		// 预览图
	    	}
	    	......
	    ],
	    "message": "操作成功"
	}

<br/>

### <a name="8.2">8.2 课程详情</a>

###### 请求接口
POST /Article/course

###### 请求参数
	{
		"token":TOKEN,		  // token
		"courseid":"12"		  // 课程id
	}
###### 返回值
	{
	    "code": 200,
	    "data":{
	    	"title":"经方新解",			// 课程标题
	    	"pic":"http://xx.xx.xx/xx/xx/xx.png",	// 预览图
	    	"intro":"xxxxxx",			// 课程简介
	    	"views":"28171",			// 查看次数
	    	"price":"0.00",				// 价格 =0,显示为‘免费’
	    	"lessons":[				// 课时
	    		"order": "1",			// 序号
	    		"title":"前言",			// 标题
	    		"duration":"02'23""		// 时长
	    	],
	    	"lessonsNum":"12",			// 课时总数
	    	"doctor":"21",				// 作者id
	    	"author":"王波",				// 作者
	    	"avatar":"http://xx.xx.xx/xx/xx/xx.png",// 头像
	    	"level":"1",				// 职称(患者不返回) =1,住院;=2,主治;=3,副主任;=4,主任;
	    	"hospital":"石家庄第一医院",		// 所在医院/诊所
	    },
	    "message": "操作成功"
	}

<br/>

### <a name="8.3">8.3 医生的课程（含个人简介）</a>

###### 请求接口
POST /Doctor/courses

###### 请求参数
	{
		"token":TOKEN,		  // token
		"doctorid":"12"		  // 医生id
	}
###### 返回值
	{
	    "code": 200,
	    "data":{
	    	"name":"王波",				// 医生姓名
	    	"avatar":"http://xx.xx.xx/xx/xx/xx.png",// 头像
	    	"level":"1",				// 职称(患者不返回) =1,住院;=2,主治;=3,副主任;=4,主任;
	    	"hospital":"石家庄第一医院",		// 所在医院/诊所
	    	"introduce":"xxxxxx",			// 个人简介
	    	"specialize":"xxxxxx",			// 个人擅长
	    	"courses":[				// 课程
	    		"id":"12",			// 课程id
	    		"title":"前言",			// 标题
	    		"views":"2837"			// 查看次数
	    		"price":"0.00"			// 价格 =0，显示为免费
	    		"pic":"http://xx.xx.xx/xx/xx/xx.png",	// 预览图
	    	]
	    },
	    "message": "操作成功"
	}

<br/>

### <a name="8.4">8.4 课时URL</a>

###### 请求接口
POST /Article/lessonURL

###### 请求参数
	{
		"token":TOKEN,		  // token
		"courseid":"12"		  // 课程id
		"order":"1"		// 课时序号
	}
###### 返回值
	{
	    "code": 200,
	    "data":{
	    	"url":"http://xx.xx/xx.mp4",				// 课时URL
	    },
	    "message": "操作成功"
	}

<br/>

### <a name="8.5">8.5 课程全部课时</a>

###### 请求接口
POST /Article/lessons

###### 请求参数
	{
		"token":TOKEN,		  // token
		"courseid":"12"		  // 课程id
	}
###### 返回值
	{
	    "code": 200,
	    "data":[
	    	{
	    	"order": "1",			// 序号
	    	"title":"前言",			// 标题
	    	"duration":"02'23""		// 时长
	    	},
	    	......
	    ],
	    "message": "操作成功"
	}

<br/>

### <a name="9.1">9.1 患者列表</a>

###### 请求接口
POST /Doctor/patients

###### 请求参数
	{
		"clinic":"DF98I8K2L90K26"	// CIS诊所id
		"code":"10002"			// 医生在该诊所的编码
		"key":"李飞"			// 搜索关键词
	}
###### 返回值
	{
	    "code": 200,
	    "data":[
	    	{
	    	"id":"12",				// 患者id
	    	"name":"李飞",				// 姓名
	    	"age":"39",				// 年龄
	    	"sex":"1",				// 性别 =1，男；=2，女；
	    	"avatar":"http://xx../xx/xx.png",	// 头像
	    	"idcard":"1101xxxxxxxx"			// 身份证号
	    	}
	    	......
	    ],
	    "message": "操作成功"
	}

<br/>

### <a name="9.2">9.2 就诊记录</a>

###### 请求接口
POST /Patient/visRecords

###### 请求参数
	{
		"patientid":"12"		// CIS患者id
	}
###### 返回值
	{
	    "code": 200,
	    "data":[
	    	{
	    	"id":"12",				// 记录id
	    	"disease":"颈椎病",			// 诊断
	    	"symptoms":"头晕,肢体麻木",		// 症状
	    	"prescription":"xxxxxxxxxxxxx",		// 处方；
	    	"vistime":"1401902837",			// 就诊时间
	    	}
	    	......
	    ],
	    "message": "操作成功"
	}

<br/>

### <a name="9.3">9.3 患者详情资料</a>

###### 请求接口
POST /Patient/profile

###### 请求参数
	{
		"patientid":"12"		// CIS患者id
	}
###### 返回值
	{
	    "code": 200,
	    "data":
	    {
	    	"name":"李飞",				// 姓名
	    	"age":"39",				// 年龄
	    	"sex":"1",				// 性别 =1，男；=2，女；
	    	"avatar":"http://xx../xx/xx.png",	// 头像
	    	"idcard":"1101xxxxxxxx",		// 身份证号
	    	"past_illness":"否认疫区、疫水接触史..",	// 既往史
	    	"personal_medical":"出生并长大于原籍..",	// 个人史
	    	"family_medical":"否认家族遗传病、传染..",	// 家族史
	    },
	    "message": "操作成功"
	}

<br/>

### <a name="9.4">9.4 患者就诊详情</a>

###### 请求接口
POST /Patient/visDetail

###### 请求参数
	{
		"patientid":"12"		// CIS患者id
	}
###### 返回值
	{
	    "code": 200,
	    "data":
	    {
	    	"disease":"颈椎病",				// 所患疾病
    		"symptoms":"头晕,浑身乏力,"		// 主要症状
    		"present_illness":"4月前不明原因出现.."	// 现病史
    		"past_illness":"否认疫区、疫水接触史.."	// 既往史
    		"personal_medical":"出生并长大于原籍.."	// 个人史
    		"family_medical":"否认家族遗传病、传染.."	// 家族史
    		"examination":"血常规、尿常规、大便常.."	// 医学检查
    		"diagnosis":"1、颈4-7椎体骨质增生；颈.."	// 临床诊断
    		"prescription":"锁阳固精丸 2盒六味地黄丸 1瓶.."	// 处方
    		"orders":"注意休息，多喝水，不要劳累"	// 医嘱
	    },
	    "message": "操作成功"
	}

<br/>

### <a name="9.5">9.5 患者检查报告列表</a>

###### 请求接口
POST /Patient/reports

###### 请求参数
	{
		"token":TOKEN,			// token
		"isPatient":"1",		// 是否患者 =1,是 =0,否
		"idcard":"1101.."		// 身份证号，多个身份证号用,分割
		"clinic":"DF98I8K2L90K26"	// CIS诊所id
		"page":"0"			// 页数
	}
###### 返回值
	{
	    "code": 200,
	    "data":[
	    	{
	    	"id":"1BCFD13FB65745AA969A747D3563B875",// 报告id
	    	"name":"王波",				// 患者姓名
	    	"sex":"1",				// 患者性别
	    	"age":"29",				// 患者年龄
	    	"checktime":"14091293948",		// 检查日期
	    	}
	    		......
	    ],
	    "message": "操作成功"
	}

<br/>

### <a name="10.1">10.1 软件安装验证</a>

###### 请求接口
POST /Rs/verify

###### 请求参数
	{
		"devicesn":"DF98I8K2L90K26"			// 设备序列号
		"uuid":"2cec6109-5bcf-45a3-ba1b-978b041c037f"	// 网关id
		"code":"IL98IL23OP"				// 验证串码
	}
###### 返回值
	{
	    "code": 200,
	    "data": "",
	    "message": "操作成功"
	}

<br/>

### <a name="10.2">10.2 设备监控</a>

###### 请求接口
POST /Rs/monitor

###### 请求参数
	{
		"devicesn":"DF98I8K2L90K26"	// 设备序列号
		"version":"1.0.3",		// 设备软件当前版本
		"code":"0"			// 0=正常；10001=未连接检验仪器；
	}
###### 返回值
	{
	    "code": 200,
	    "data": "",
	    "message": "操作成功"
	}

<br/>

### <a name="10.3">10.3 上报数据</a>

###### 请求接口
POST /Rs/report

###### 请求参数
	{
		"devicesn":"DF98I8K2L90K26"	// 设备序列号
		"datas":DATAS			// json格式的字符串（例：{"SampleInfo_SampleID":"1","SampleInfo_Mode":"0",.......}）
	}
###### 返回值
	{
	    "code": 200,
	    "data": "",
	    "message": "操作成功"
	}
###### 字段数据说明
	{
	 ----- 血常规 -----
	"SampleInfo_SampleID":"1",		// 样本编号
	"SampleInfo_Mode":"0",			// 测量模式
	"SampleInfo_TestTime":"2017-06-27	 20:47:41",		// 检测时间
	"SampleInfo_Name":"",			// 受检人姓名
	"SampleInfo_Group":"0",			// 组别 ??
	"SampleInfo_Gender":"0",		// 受检人性别 =0,未知 =1, =2, ??
	"SampleInfo_AgeVal":"0",		// 受检人年龄
	"SampleInfo_AgeType":"0",		// 受检人年龄类型 ??
	"SampleInfo_Dept":"",			// 送检科室
	"SampleInfo_ChartNo":"",		// 病历号
	"SampleInfo_BedNo":"",			// 床号
	"SampleInfo_Sender":"",			// 送检者
	"SampleInfo_Tester":"",			// 检验者
	"SampleInfo_Checker":"",		// 审核者
	"WBC_Val":"0.0",			// 白细胞数目
	"WBC_Low":"4.0",			// 白细胞参考范围-低值
	"WBC_High":"10.0",			// 白细胞参考范围-高值
	"WBC_Unit":"10^9/L",			// 白细胞数目单位
	"WBC_Flag":null,			// ??
	"LymphValue_Val":"***.*",		// 淋巴细胞数目
	"LymphValue_Low":"0.8",			// 淋巴细胞数目参考范围-低值
	"LymphValue_High":"4.0",		// 淋巴细胞数目参考范围-高值
	"LymphValue_Unit":"10^9/L",		// 淋巴细胞数目单位
	"LymphValue_Flag":null,			// ??
	"MidValue_Val":"***.*",			// 中间细胞数目
	"MidValue_Low":"0.1",
	"MidValue_High":"1.5",
	"MidValue_Unit":"10^9/L",
	"MidValue_Flag":null,
	"GranValue_Val":"***.*",		// 中性粒细胞数目
	"GranValue_Low":"2.0",
	"GranValue_High":"7.0",
	"GranValue_Unit":"10^9/L",
	"GranValue_Flag":null,
	"LymphPercentage_Val":"**.*",		// 淋巴细胞百分比
	"LymphPercentage_Low":"20.0",
	"LymphPercentage_High":"40.0",
	"LymphPercentage_Unit":"%",
	"LymphPercentage_Flag":null,
	"MidPercentage_Val":"**.*",		// 中间细胞百分比
	"MidPercentage_Low":"3.0",
	"MidPercentage_High":"15.0",
	"MidPercentage_Unit":"%",
	"MidPercentage_Flag":null,
	"GranPercentage_Val":"**.*",		// 中性粒细胞百分比
	"GranPercentage_Low":"50.0",
	"GranPercentage_High":"70.0",
	"GranPercentage_Unit":"%",
	"GranPercentage_Flag":null,
	"HGB_Val":"0",				// 血红蛋白
	"HGB_Low":"110",
	"HGB_High":"160",
	"HGB_Unit":"g/L",
	"HGB_Flag":null,
	"RBC_Val":"0.00",			// 红细胞数目
	"RBC_Low":"3.50",
	"RBC_High":"5.50",
	"RBC_Unit":"10^12/L",
	"RBC_Flag":null,
	"HCT_Val":"0.0",			// 红细胞压积
	"HCT_Low":"37.0",
	"HCT_High":"54.0",
	"HCT_Unit":"%",
	"HCT_Flag":null,
	"MCV_Val":"***.*",			// 平均红细胞体积
	"MCV_Low":"80.0",
	"MCV_High":"100.0",
	"MCV_Unit":"fL",
	"MCV_Flag":null,
	"MCH_Val":"***.*",			// 平均红细胞血红蛋白含量
	"MCH_Low":"27.0",
	"MCH_High":"34.0",
	"MCH_Unit":"pg",
	"MCH_Flag":null,
	"MCHC_Val":"****",			// 平均红细胞血红蛋白浓度
	"MCHC_Low":"320",
	"MCHC_High":"360",
	"MCHC_Unit":"g/L",
	"MCHC_Flag":null,
	"RDWCV_Val":"**.*",			// 红细胞分布宽度变异系数
	"RDWCV_Low":"11.0",
	"RDWCV_High":"16.0",
	"RDWCV_Unit":"%",
	"RDWCV_Flag":null,
	"RDWSD_Val":"***.*",			// 红细胞分布宽度标准差
	"RDWSD_Low":"35.0",
	"RDWSD_High":"56.0",
	"RDWSD_Unit":"fL",
	"RDWSD_Flag":null,
	"PLT_Val":"0",				// 血小板数目
	"PLT_Low":"100",
	"PLT_High":"300",
	"PLT_Unit":"10^9/L",
	"PLT_Flag":null,
	"MPV_Val":"**.*",			// 平均血小板体积
	"MPV_Low":"6.5",
	"MPV_High":"12.0",
	"MPV_Unit":"fL",
	"MPV_Flag":null,
	"PDW_Val":null,				// 血小板分布宽度
	"PDW_Low":null,
	"PDW_High":null,
	"PDW_Unit":null,
	"PDW_Flag":null,
	"PCT_Val":".***",			// 血小板压积
	"PCT_Low":"0.108",
	"PCT_High":"0.282",
	"PCT_Unit":"%",
	"PCT_Flag":null,
	"AlarmFlag_Rm":"0",			// 直方图报警
	"AlarmFlag_R1":"0",
	"AlarmFlag_R2":"0",
	"AlarmFlag_R3":"0",
	"AlarmFlag_R4":"0",
	"AlarmFlag_Pm":"0",
	"AlarmFlag_Ps":"0",
	"AlarmFlag_P1":"0"
	}
<br/>

### <a name="10.4">10.4 版本号</a>

###### 请求接口（获取软件当前版本号，用于自动更新）
POST /Rs/version

###### 请求参数
	{
		"devicesn":"DF98I8K2L90K26"	// 设备序列号
	}
###### 返回值
	{
	    "code": 200,
	    "data": {
	    	"version":"1.0.4",
	    	"url":"http://xx.xx.xx/xxxxxx"		// 更新包下载地址
	    },
	    "message": "操作成功"
	}

<br/>