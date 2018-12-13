<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShoppingCart.aspx.cs" Inherits="Form_ShoppingCart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>购物车</title>
    <meta name="keywords" content="" />
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <link rel="stylesheet" href="../Css/base.css" />
    <link rel="stylesheet" href="../Css/style.css" />
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/dmini.js"></script>
    <script src="../Scripts/cart.js"></script>
    <script src="../Scripts/layui/layui.all.js"></script>
    <script src="../Scripts/layui/layui.js"></script>
    <link href="../Scripts/layui/css/layui.css" rel="stylesheet" />
</head>
<body>
    <div class="hide-gdt">
        <div class="main gwcbg">
            <div class="gwclist">
                <div class="gcts">
                    <div>
                        <table>
                            <tr>
                                <th></th>
                                <th>商品信息</th>
                                <th>金额</th>
                                <th>数量</th>
                                <th>小计</th>
                                <th>操作</th>
                            </tr>
                            <%=jsSB.ToString() %>
                        </table>
                    </div>
                    <div class="cunt">
                        <span><i class="<%=CheckAll %>" onclick="checkAll(this);" id="allselect"></i>全选</span>
                        <span class="gray9"><a href="javascript:void(0);" onclick="deleteAll();">清空购物车</a></span>
                        <span>总价:<i class="red" id="allprice">￥<%=totalPrice.ToString("f2")%></i></span>
                        <%=GetChargeJs() %>
                    </div>
                </div>
            </div>
            <div style="height: 160px;"></div>
            <%--<div class="tenav">
                <div class="nav">
                    <div class="lit"><a href="javascript:history.go(-1);"><i class="ico ico8"></i></a></div>
                    <div class="rit"><a href="javascript:history.go(-1);"><i class="ico ico9"></i></a></div>
                </div>
            </div>--%>
            <div class="enav" <%=EnavDisJS %>>
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
            <%--身份证--%>
            <object id="CVR_IDCard" name="CVR_IDCard" classid="clsid:10946843-7507-44FE-ACE8-2B3483D179B7"
                width="0" height="0">
            </object>
        </div>
        <div class="emdo">
            <ul>
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
    var interval = 0;
    var consisposno ='<%=ConsisNoList%>';
    var posno ='<%=PosNo%>'

    $(function () {
        //监听
        if (interval > 0) {
            setInterval(timerWeb, interval);
        }
        //加载图片
        LoadPics();
    });
    //加载图片
    function LoadPics() {
        //加载图片
        $($("i[name='cart2Checkbox']")).each(
            function () {
                //获取药品id
                var drugid = $(this).attr('refmainitemid');
                //获取src
                $.ajax("../Ajax/LoadDrugMedia.ashx", {
                    //async: false,
                    data: {
                        optype: 1,
                        drugid: escape(drugid),
                        id: escape(drugid),
                        partype: 0,
                        parname: escape("主视图"),
                        grade: escape("C"),
                        method: "GetDrugPic",
                        classname: "DrugInfo"
                    },
                    dataType: 'json', //服务器返回json格式数据
                    type: 'post', //HTTP请求类型	
                    timeout: 8000,
                    success: function (data) {
                        var imagelist = $('#' + data.key).find('img');
                        for (let j = 0; j < imagelist.length; j++) {
                            $(imagelist[j]).attr("src", "data:image/jpg;base64," + data.value);
                        }
                        //layer.alert(data);
                        if (data && data.value == '0') {

                            $.ajax("../Ajax/LoadDrugMedia.ashx", {
                                //async: false,
                                data: {
                                    optype: 0,
                                    drugid: escape(drugid),
                                    partype: 0,
                                    parname: escape("主视图"),
                                    grade: escape("C"),
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

                    }
                });
            })
    }

</script>
