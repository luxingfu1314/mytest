<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DrugDetail.aspx.cs" Inherits="Form_DrugDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>药品详情</title>
    <meta name="keywords" content=""/>
    <meta name="description" content=""/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" /> 
    <link rel="stylesheet" href="../Css/base.css"/>
    <link rel="stylesheet" href="../Css/style.css"/>
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/dmini.js"></script>
    <script src="../Scripts/layui/layui.all.js"></script>
    <script src="../Scripts/layui/layui.js"></script>
    <link href="../Scripts/layui/css/layui.css" rel="stylesheet" />
    <script src="../Scripts/text.js"></script>
</head>
<body>
    <div class="hide-gdt">
        <div class="main gwcbg">
            <div class="yaox">
                <div class="xqig">
                    <%=DrugJS %>
                    <div class="sulj" <%=CartDisJS %>>
                        <span class="m">数量：</span>
                        <div class="scc">
                            <i class="ico ico12" onclick="Additem(this)"></i>
                            <i class="ico ico13" onclick="Minusitem(this)"></i>
                            <input type="text" class="text" value="1" onchange="Prompt(this)" />
                        </div>
                        <div class="amount-msg" style="display: none;">
                            <span style="color: red;">1</span>
                            <em></em>
                        </div>
                        <span class="n">库存：<%=GetStorage() %></span>
                    </div>
                    
                </div>
                <div class="cf clearfix">
                    <%=InstructionsJS %>
                </div>
                <div class="vbtn mt40" >
                   <%--  <%=CartDisJS %>--%>
                        <a href="#" onclick="CheckAddCart()" class="btn">加入购物车</a>
                        <a href="#" onclick="CheckGotoCart()" class="btn buy">立即购买</a>
                </div>
            </div>
            <div style="height:160px;"></div>
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
        </div>
        <div class="emdo"  id="rightFloat">
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
    var drugid = "<%=Drugid%>";
    var catalimit =<%=Catal%>;
    var limitBuy =<%=Limitbuy%>;
    var storage = <%=Storage%>;
    var CurQ =<%=CurDrugQuantity%>;
    var interval = 0;
    var _3DFlg=<%=_3DFlg%>;
    $(function () {
        //监听
        if (interval > 0) {
            setInterval(timerWeb, interval);
        }
        goHomePageTimer();
        //加载图片
        if (_3DFlg == 1) {
            LoadModelUrl();
        }
        else {
            LoadPic();
        }
    });
    //加载图片
    function LoadPic() {
        //加载图片
        //获取src
        $.ajax("../Ajax/LoadDrugMedia.ashx", {
            //async: false,
            data: {
                optype: 1,
                drugid: escape(drugid),
                partype: 0,
                parname: escape("主视图"),
                grade: escape("A"),
                method: "GetDrugPic",
                classname: "DrugInfo"
            },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {
                $('#' + drugid).attr("src", "data:image/jpg;base64," + data.value);
                //layer.alert(data);
                if (data && data.value == '0') {
                    //async: false,
                        $.ajax("../Ajax/LoadDrugMedia.ashx", {
                            data: {
                                optype: 0,
                                drugid: escape(drugid),
                                partype: 0,
                                parname: escape("主视图"),
                                grade: escape("A"),
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

    //加载模型
    function LoadModelUrl() {
       //获取src
        $.ajax("../Ajax/LoadDrugMedia.ashx", {
           //async: false,
           data: {
               optype: 1,
               drugid: escape(drugid),
               partype: 1,
               parname: escape("3D模型"),
               grade: escape("A"),
               method: "GetDrugModel",
               classname: "DrugInfo"
           },
           dataType: 'json', //服务器返回json格式数据
           type: 'post', //HTTP请求类型	
           timeout: 80000,
           success: function (data) {
               //加载文件
               document.getElementById("ifm").src = data.value;//Url地址
               //
               if (data.flg == '0') {
                   $.ajax("../Ajax/LoadDrugMedia.ashx", {
                       //async: false,
                       data: {
                           optype: 0,
                           drugid: escape(drugid),
                           partype: 1,
                           parname: escape("3D模型"),
                           grade: escape("A"),
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
               alert(errorThrown);
           }
        });

    }

    //
    function CheckAddCart() {
        if (<%=BuyFlag%><= 0) {
            layer.alert("<h1 class='layerhight'>"+"此商品尚未开放销售，请选购其他商品！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
        }
        else if (<%=priceError%>>= 1) {
            layer.alert("<h1 class='layerhight'>"+"此商品价格存在异常，请联系药店相关人员！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
        }
        else {
            AddCart();
        }
    }
    //
    function CheckGotoCart() {
        if (<%=BuyFlag%><= 0) {
            layer.alert("<h1 class='layerhight'>"+"此商品尚未开放销售，请选购其他商品！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
        }
        else if (<%=priceError%>>= 1) {
            layer.alert("<h1 class='layerhight'>"+"此商品价格存在异常，请联系药店相关人员！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
        }
        else {
            GotoCart();
        }
    }

    //加入购物车
    function AddCart() {
        var inputObject = $(document).find('.text');
        var quantity = inputObject.val();

        //库存核查
        if (parseInt(quantity) + CurQ ><%=Storage%>) {
            layer.alert("<h1 class='layerhight'>"+"库存不足！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
            return;
        }
        //最大购买量核查
        if (<%=Catal%>> 0 && parseInt(quantity) + CurQ ><%=Catal%>) {
            layer.alert("<h1 class='layerhight'>"+"超出您的最大购买量！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
            return;
        }
        //最小购买量核查
        if (<%=Limitbuy%>> 0 && parseInt(quantity) + CurQ <<%=Limitbuy%>) {
            layer.alert("<h1 class='layerhight'>"+"低于您的最小购买量！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
            return;
        }

        //加入购物车
        $.ajax({
            url: "../Ajax/ShoppingCart.ashx",
            data: {
                optype: '1',
                drugid: drugid,
                quantity: quantity
            },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {
                //获取购物车量
                $.ajax({
                    url: "../Ajax/ShoppingCart.ashx",
                    data: { optype: '6' },
                    dataType: 'json', //服务器返回json格式数据
                    type: 'post', //HTTP请求类型	
                    timeout: 8000,
                    success: function (data) {
                        $("#rightFloat").find('em').html(data.Message);
                    },
                    error: function (data) {
                        $("#rightFloat").find('em').html(parseInt(quantity) + CurQ);
                    }
                });
                //
                layer.msg("<h1 class='layerhight'>" +data.Message + "</h1>",
                    {area: ['400px', 'auto']}
                );
            },
            error: function (data) {
                layer.alert("<h1 class='layerhight'>"+"error!" + data+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
            }
        });
    }
    //立即购买
    function GotoCart() {
        var inputObject = $(document).find('.text');
        var quantity = inputObject.val();

        //库存核查
        if (parseInt(quantity) + CurQ ><%=Storage%>) {
            layer.alert("<h1 class='layerhight'>"+"库存不足！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
            return;
        }
        //最大购买量核查
        if (<%=Catal%>> 0 && parseInt(quantity) + CurQ ><%=Catal%>) {
            layer.alert("<h1 class='layerhight'>"+"超出您的最大购买量！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
            return;
        }
        //最小购买量核查
        if (<%=Limitbuy%>> 0 && parseInt(quantity) + CurQ <<%=Limitbuy%>) {
            layer.alert("<h1 class='layerhight'>"+"低于您的最小购买量！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
            return;
        }

        //加入购物车
        $.ajax({
            url: "../Ajax/ShoppingCart.ashx",
            data: { optype: '1', drugid: drugid, quantity: quantity },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {
                //if (data.StatusCode != 200) {
                //    alert(data.Message);
                //}
                //弹到购物车页面
                window.location.href = '../Forms/ShoppingCart.aspx';
            },
            error: function (data) {
                layer.alert("<h1 class='layerhight'>"+"error!"+data+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
            }
        });

        //加载图片，核查是否存在3D图片，是否存在说明书
    }
</script>
