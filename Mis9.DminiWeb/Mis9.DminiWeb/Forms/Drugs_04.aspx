<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Drugs_04.aspx.cs" Inherits="Form_Drugs_04 " %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>人体导航</title>
    <meta name="keywords" content=""/>
    <meta name="description" content=""/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" /> 
    <link rel="stylesheet" href="../Css/base.css"/>
    <link rel="stylesheet" href="../Css/style.css"/>
    <script src="../Scripts/layui/layui.all.js"></script>
    <script src="../Scripts/layui/layui.js"></script>
    <link href="../Scripts/layui/css/layui.css" rel="stylesheet" />
    <script src="../Scripts/dmini.js"></script>
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/text.js"></script>
</head>
<body>
    <div class="hide-gdt">
        <div class="main">
            <div class="ren">
                <img id="pic" src="../Images/man.png" onclick="javascript:void(0);" usemap="#map" width="622" height="1197"  />
                <map name="map" id="map">
                    
                </map>
            </div>
            <div class="sex">
                <ul>
                    <%=SexJS %>
                </ul>
            </div>
            <div class="buw" <%=BuwStyle%>>
                <div class="bw">
                    <ul>
                        <%=SystemJS %>
                    </ul>
                </div>
            </div>
            <div class="tenav">
                <div class="nav">
                    <%--<div class="lit"><a href="javascript:history.go(-1);"><i class="ico ico8"></i></a></div>--%>
                    <%--<div class="rit"><a href="javascript:history.go(-1);"><i class="ico ico9"></i></a></div>--%>
                </div>
            </div>
            <div class="enav">
                <div class="nav">
                    <%--<div class="lit"><a href="javascript:history.go(-1);"><i class="ico ico8"></i></a></div>--%>
                    <%--<div class="rit"><a href="javascript:history.go(-1);"><i class="ico ico9"></i></a></div>--%>
                    <div class="vco" <%=VcoWidth %>>
                        <ul class="clearfix">
                            <li><a href="../Forms/HomePage.aspx"><i class="ico ico1"></i></a>主页</li>
                            <li><a href="../Forms/Drugs_01.aspx"><i class="ico ico2"></i></a>分类</li>
                            <li <%=LocDisJS %>><a href="../Forms/Drugs_02.aspx"><i class="ico ico3"></i></a>货位</li>
                            <%--<li><a href="../Forms/Drugs_03.aspx"><i class="ico ico4"></i></a></li>--%>
                            <li class="cur"><i class="ico ico5"></i>导航</li>
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
    var consisposno = '<%=ConsisNoList%>';
    var indexWait = -1;
    $(function () {
        //监听
        if (interval > 0) {
            setInterval(timerWeb, interval);
        }
        goHomePageTimer();
        //加载图片及热点
        LoadPic();
    });

    function TypeClick(e) {
        //是否是已选中
        var _class = $(e).attr('class');
        //
        if (typeof (_class) != "undefined" && _class!=''&&_class.indexOf("cur") != -1) {
            return;
        }
        else {
            //移除其他类为cur的
            var inputObject = $(e).parent().find('.cur');
            inputObject.attr('class','');
            //给自己的类加cur
            $(e).attr('class', 'cur');
            //更新图片并更新热点
            LoadPic();
        }
    }
    //
    function LoadPic() {
        //加载提示
        var WaitText = '数据加载中...';
        operationType = -1;
        indexWait = LoadWaitNoCancel(WaitText,60000);
        //获取当前的sexcode
        var sexcode = "";
        var sexlist = $(document).find('.sex');
        if (sexlist.length > 0) {
            var cursexlist = $(sexlist[0]).find('.cur');
            if (cursexlist.length > 0) {
                sexcode = $(cursexlist[0]).attr('sexcode');
            }
            else {
                operationType = 1;
                layer.close(indexWait);
                return;
            }
        }
        else {
            operationType = 1;
            layer.close(indexWait);
            return;
        }
        //获取当前的systemcode
        var systemcode = "";
        var bwlist = $(document).find('.bw');
        if (bwlist.length > 0) {
            var curbwlist = $(bwlist[0]).find('.cur');
            if (curbwlist.length > 0) {
                systemcode = $(curbwlist[0]).attr('systemcode');
            }
            else {
                operationType = 1;
                layer.close(indexWait);
                return;
            }
        }
        else {
            operationType = 1;
            layer.close(indexWait);
            return;
        }
        //加载图片
        $.ajax("../Ajax/CacheHandler.ashx", {
            //async: false,
            data: {
                optype: 1,
                odertype: 1,
                sexcode: sexcode,
                systemcode: systemcode,
                method: "GetHotspotPic",
                classname: "HotspotInfo"
            },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {
                $('#pic').attr("src", "data:image/png;base64," + data.value);
                //更新热点
                UpdateSpots(sexcode,systemcode);
                //layer.alert(data);
                //alert(data.flg);
                if (data && data.flg == '0') {
                    //async: false,
                        $.ajax("../Ajax/CacheHandler.ashx", {
                            data: {
                                optype: 1,
                                odertype: 0,
                                sexcode: sexcode,
                                systemcode: systemcode,
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
                operationType = 1;
                layer.close(indexWait);
            }
        });
    }
    //更新热点
    function UpdateSpots(sexcode, systemcode) {
        var control = $('#map');
        //清空控件内容
        control.html("");
        //ajax 获取数据，判定是否为叶子
        $.ajax("../Ajax/HttpHandler.ashx", {
            //async: false,
            data: {
                consisposno: consisposno,
                sexcode: sexcode,
                systemcode: systemcode,
                method: "GetHotspotList",
                classname: "HotspotInfo"
            },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {
                var item='';
                for (var i = 0; i < data.length; i++) {
                    var typecode = data[i].typecode;
                    var leafflg = data[i].leafflg;
                    var spotname = data[i].spotname;
                    var shapename = data[i].shapename;
                    var coords = data[i].coords;
                    var drugcount=data[i].drugcount;
                    //
                    var href = "../Forms/Drugs_04_1.aspx?type=" + typecode;
                    if (leafflg == "1")
                        href = "../Forms/Drugs_04_2.aspx?type=" + typecode;
                    if( drugcount>0)
                        item += "<area shape='" + shapename + "' coords='" + coords + "' href='" + href + "' alt='" + spotname + "' />";
                    else
                        item += "<area shape='" + shapename + "' coords='" + coords + "' onclick='alertno()' />";
                }
                //放数据
                control.html(item);
                operationType = 1;
                layer.close(indexWait);
            },
            error: function (xhr, type, errorThrown) {
                //alert("error");
                operationType = 1;
                layer.close(indexWait);
            }
        });
    }
    function alertno() {
        layer.msg("<h1 class='layerhight'>" +"此部位暂无疾病及药品数据！" + "</h1>",
            { area: ['400px', 'auto'] }
        );
    }
    
</script>
