<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Drugs_04_2.aspx.cs" Inherits="Form_Drugs_04_2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>药品列表</title>
    <meta name="keywords" content=""/>
    <meta name="description" content=""/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" /> 
    <link rel="stylesheet" href="../Css/base.css"/>
    <link rel="stylesheet" href="../Css/style.css"/>
    <script src="../Scripts/dmini.js"></script>
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/layui/layui.all.js"></script>
    <script src="../Scripts/layui/layui.js"></script>
    <link href="../Scripts/layui/css/layui.css" rel="stylesheet" />
</head>
<body>
    <div class="hide-gdt">
        <div class="main">
            <div class="dhnv">
                <span>您的位置：</span>
                <a href="../Forms/HomePage.aspx">首页</a><u>></u>
                <%=NavigationJs %>
            </div>
            <div class="yptj">
                <ul class="clearfix">
                    <%=MainJS %>
                </ul>
            </div>
            <div style="height:160px;"></div>
            <div class="enav">
                <div class="nav">
                    <%--<div class="lit"><a href="javascript:history.go(-1);"><i class="ico ico8"></i></a></div>
                    <div class="rit"><a href="javascript:history.go(-1);"><i class="ico ico9"></i></a></div>--%>
                    <div class="vco" <%=VcoWidth %>>
                        <ul class="clearfix">
                            <li><a href="../Forms/HomePage.aspx"><i class="ico ico1"></i></a>主页</li>
                            <li><a href="../Forms/Drugs_01.aspx"><i class="ico ico2"></i></a>分类</li>
                            <li <%=LocDisJS %>><a href="../Forms/Drugs_02.aspx"><i class="ico ico3"></i></a>货位</li>
                            <%--<li><a href="../Forms/Drugs_03.aspx"><i class="ico ico4"></i></a></li>--%>
                            <li><a href="../Forms/Drugs_04.aspx"><i class="ico ico5"></i></a>导航</li>
                            <li><a href="../Forms/Drugs_05.aspx"><i class="ico ico7"></i></a>查询</li>
                        </ul>
                    </div>
                </div>
            </div>
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
                <li>
                    <a href="javascript:history.go(-1);" title="返回上一页">
                        <img src="../Images/nm3.png" /><span>返回</span></a>
                </li>
            </ul>
        </div>
    </div>
</body>
</html>
<script>
    var interval =0;
    $(function () {
        //监听
        if (interval > 0) {
            setInterval(timerWeb, interval);
        }
        goHomePageTimer();
        //加载图片
        LoadPics();
    });
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
</script>
