<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Drugs_02.aspx.cs" Inherits="Form_Drugs_02" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>货位列表</title>
    <meta name="keywords" content=""/>
    <meta name="description" content=""/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" /> 
    <link rel="stylesheet" href="../Css/base.css"/>
    <link rel="stylesheet" href="../Css/style.css"/>
    <script src="../Scripts/dmini.js"></script>
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../SoftKey/vk_loader.js?vk_layout=CN%20Chinese%20Simpl.%20Pinyin&vk_skin=flat_gray"></script>
    <script src="../SoftKey/jquery-1.8.2.min.js"></script>
</head>
<body>
    <div class="hide-gdt">
        <div class="main">
            <div class="search search2">
                <i class="ico ico14"></i>
                <input type="text" id="searchtext" class="text" placeholder="请输入药品名或拼音首字母" onfocus='test();' onblur="VirtualKeyboard.toggle('searchtext','softkey');" />
                <button class="btn" onclick="Search()">查询</button>
            </div>
            <div id="softkey" style="height: 1px; margin-left: 200px"></div>
            <div class="huojia">
                <ul class="clearfix" id="drugboxlist">
                    <%--此处填充药品货位信息--%>
                </ul>
            </div>
            <div class="enav">
                <div class="nav">
                    <div class="pages">
                        <a href="#" id="PreviousStep" onclick="SearchPage(-1)" class="btn">上一页</a>
                        <span class="text"><em id="curPageNo">1</em> / <em id="AllPageNo">1</em>页</span>
                        <a href="#" id="NextStep" onclick="SearchPage(1)" class="btn">下一页</a>
                    </div>
        
                </div>
            </div>
            <div class="pagehi"></div>
        </div>
        <div class="emdo">
            <ul>
                <li <%=CartDisJS %>>
                    <a href="../Forms/ShoppingCart.aspx" title="购物车">
                        <img src="../Images/nm1.png" /><em><%=Quantity%></em><span>购物车</span></a>
                </li>
                <li>
                    <a href="javascript:;" onclick="goTop()" title="返回顶部">
                        <img src="../Images/nm2.png" /><span>置顶</span></a>
                </li>
            </ul>
        </div>
    </div>
</body>
</html>
<script>

    var consisposno = '<%=ConsisNoList%>';
    var interval = 0;
    //当前页
    var curPage = 1;
    //每页展示数量
    var perCount = 40;
    //总页数
    var pageNum = 0;
    //查询参数
    var pinyin = "";
    $(function () {
        //监听
        if (interval > 0) {
            setInterval(timerWeb, interval);
        }
        //加载数据量
        Search();
    });
    //查询总页数，并展示第一页
    function Search() {
        pinyin = $('#searchtext').val();
        //ajax 获取数据，判定是否为叶子
        $.ajax("../Ajax/HttpHandler.ashx", {
            //async: false,
            data: {
                consisposno: escape(consisposno),
                pinyin: escape(pinyin),
                percount: escape(perCount),
                method: "GetDrugCount",
                classname: "DrugInfo"
            },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {
                //赋值总页数
                pageNum = data.pageNum;
                //查询第一页
                curPage = 1;
                //更新页面总页数
                $('#AllPageNo').html(pageNum);
                //
                SearchPage(0);
            },
            error: function (xhr, type, errorThrown) {
                //alert("error");
            }
        });
    }

    function SearchPage(step)
    {
        curPage = curPage + step;
        //更新当前页数
        $('#curPageNo').html(curPage);
        //首页
        if (curPage == 1) {
            $('#PreviousStep').attr("disabled", "disabled");
        }
        else
        {
            $('#PreviousStep').removeAttr("disabled");
        }
        //末页
        if (curPage == pageNum) {
            $('#NextStep').attr("disabled", "disabled");
        }
        else
        {
            $('#NextStep').removeAttr("disabled");
        }
        //查询当前页药品数据
        var control = $('#drugboxlist');
        //清空控件内容
        control.html("");
        var src = "../Images/200.png";
        //
        //ajax 获取数据
        $.ajax("../Ajax/HttpHandler.ashx", {
            //async: false,
            data: {
                consisposno: escape(consisposno),
                pinyin: escape(pinyin),
                curpage: escape(curPage),
                percount: escape(perCount),
                method: "GetPageDrugs",
                classname: "DrugInfo"
            },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {
                var item='';
                //加载文本
                for (var i = 0; i < data.length; i++) {
                    //
                    var drugid = data[i].drugid;
                    //
                    item += "<li>";
                    item += "<div class='img'>";
                    //药品图片
                    item += "<a href = 'DrugDetail.aspx?id=" + drugid + "' target='_self'>";
                    item += "<img id='pic_"+i+"' src = '" + src + "' drugid='" + drugid + "'/>";
                    item += "</a>";
                    item += "</div>";
                    item += "<div class=\"jg\">";
                    item += "<p class='xj'>￥"+ data[i].price + "</p>";
                    item += "</div>";
                    item += "</li>";
                }
                //
                var yu = data.length % 4;
                if (yu != 0) {
                    for (var i = 0; i < 4-yu; i++) {
                        item += "<li>";
                    item += "<div class='img'>";
                    item += "</div>";
                    item += "</li>";
                    }
                }
                //放数据
                control.html(item);
                //加载图片
                LoadPics();
            },
            error: function (xhr, type, errorThrown) {
                //alert("error");
            }
        });
    }

    //加载图片
    function LoadPics() {
        //加载图片
        var imagelist = $(document).find('img');
        
        for (let j = 0; j < imagelist.length; j++) {
            //获取drugid
            var drugid = $(imagelist[j]).attr('drugid');
            if (drugid) {
                var id = $(imagelist[j]).attr('id');
                //获取src
                $.ajax("../Ajax/LoadDrugMedia.ashx", {
                    //async: false,
                    data: {
                        optype: 1,
                        drugid: escape(drugid),
                        id: escape(id),
                        partype: 0,
                        parname: escape("主视图"),
                        grade: escape("B"),
                        method: "GetDrugPic",
                        classname: "DrugInfo"
                    },
                    dataType: 'json', //服务器返回json格式数据
                    type: 'post', //HTTP请求类型	
                    timeout: 8000,
                    success: function (data) {
                        $('#'+data.key).attr("src", "data:image/jpg;base64," + data.value);
                        //layer.alert(data);
                        if (data&&data.value=='0') {
                            $.ajax("../Ajax/LoadDrugMedia.ashx", {
                                //async: false,
                                data: {
                                    optype: 0,
                                    drugid: escape(drugid),
                                    partype: 0,
                                    parname: escape("主视图"),
                                    grade: escape("B"),
                                    value: data.value
                                },
                                dataType: 'json', //服务器返回json格式数据
                                type: 'post', //HTTP请求类型	
                                timeout: 8000,
                                success: function (data) {
                                },
                                error: function (xhr, type, errorThrown) {
                                    //alert("error");
                                }
                            });
                        }
                    },
                    error: function (xhr, type, errorThrown) {
                        //无图片
                    }
                });
            }
        }
    }

    function test() {
            VirtualKeyboard.toggle('searchtext', 'softkey');
            $("#kb_langselector,#kb_mappingselector,#copyrights").css("display", "none");
        }
</script>
