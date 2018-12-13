<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrescDetail.aspx.cs" Inherits="Forms_PrescDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>订单详情</title>
    <link href="../Css/base.css" rel="stylesheet" />
    <link href="../Css/style.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/dmini.js"></script>
    <script src="../Scripts/layui/layui.all.js"></script>
    <script src="../Scripts/layui/layui.js"></script>
    <link href="../Scripts/layui/css/layui.css" rel="stylesheet"/>
    <script src="../Scripts/vbar/js/vbar.js"></script>
    <script src="../Scripts/text.js"></script>
</head>
<body>
    <div class="hide-gdt">
        <div class="main gwcbg">
            <div class="dindan">
                <ul>
                    <li>
                        <i class="ico"></i>
                        <div class="tit">订单编号：<%=Prescno %></div>
                        <%=MainJS %>
                    </li>
                </ul>
                <div class="paybtn" id="paybtn" onclick="gotoPay()">
                    <p class="hj">合计￥<%=Costs.ToString("f2") %></p>
                    <p class="btns">立即支付</p>
                </div>
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
                            <li  <%=LocDisJS %>><a href="../Forms/Drugs_02.aspx"><i class="ico ico3"></i></a>货位</li>
                            <%--<li><a href="../Forms/Drugs_03.aspx"><i class="ico ico4"></i></a></li>--%>
                            <li><a href="../Forms/Drugs_04.aspx"><i class="ico ico5"></i></a>导航</li>
                            <li><a href="../Forms/Drugs_05.aspx"><i class="ico ico7"></i></a>查询</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div class="zfbox" id="payform">
        	<em class="close" onclick="comfirm()">×</em>
        	<div class="zfti">请选择支付方式</div>
            <div class="erm" id="QR">

            </div>
            <div class="ftoa">
            	<ul>
                	<li>
                    	<span class="img"><img src="../Images/zfok_07.jpg"/></span>
                        <span>微信支付</span>
                        <span class="sel"><img id="type1" onclick="chagePayType(1)" src="../Images/zka.jpg"/></span>
                    </li>
                    <li>
                    	<span class="img"><img src="../Images/zfok_14.jpg"/></span>
                        <span>支付宝支付</span>
                        <span class="sel"><img id="type2" onclick="chagePayType(2)" src="../Images/zkb.jpg"/></span>
                    </li>
                    <li>
                    	<span class="img"><img src="../Images/zfok_18.jpg"/></span>
                        <span>设备扫码</span>
                        <span class="sel"><img id="type3"  onclick="chagePayType(3)" src="../Images/zkb.jpg"/></span>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</body>
</html>
<script>
    var interval =0;
    var prescno = '<%=Prescno%>';
    var payments =<%=Costs%>;
    var ok = 0;
    var flg = '<%=Flg%>';

    $(function () {
        //监听
        if (interval > 0) {
            setInterval(timerWeb, interval);
        }
        //加载图片
        LoadPics();
    });
    //离开页面提醒
    window.onbeforeunload = function (e) {
        if (ok == 0) {
            window.event.returnValue = "离开此页可能导致订单信息丢失，是否确认离开此页？";
        }
    }
    //加载图片
    function LoadPics() {
        //加载图片
        var imagelist = $('.dindan').find('img');
        for (let j = 0; j < imagelist.length; j++) {
            //获取药品id
            var drugid = $(imagelist[j]).attr('drugid');
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
                    grade: escape("C"),
                    method: "GetDrugPic",
                    classname: "DrugInfo"
                },
                dataType: 'json', //服务器返回json格式数据
                type: 'post', //HTTP请求类型	
                timeout: 8000,
                success: function (data) {

                    $('#' + data.key).attr("src", "data:image/jpg;base64," + data.value);
                    //layer.alert(data);
                    if (data && data.value == '0') {

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

                }
            });
        }
    }
    var paying = false;
    function gotoPay() {
        //
        if (paying) return;
        //
        $('#payform').show();
        paying = true;
        //微信
        GetWXPay();
        //关闭
        cancelmonitor = setInterval(closePay, 5 * 60 * 1000);
    }

    var monitor;//自动监听

    var cancelmonitor;//倒计时五分钟关闭

    var devicemonitor;//倒计时一分钟关闭
    var deviceFlg;
    var billno = '';//付款单据号

    var index = 0;

    var barcode = '';//付款码

    function chagePayType(e) {
        //清空控件内容
        $('#QR').html("");
        //全部取消选中
        var imagelist = $('.sel').find('img');
        for (var i = 0; i < imagelist.length; i++) {
            $(imagelist[i]).attr("src", "../Images/zkb.jpg");
        }
        //当前选中
        $("#type"+e).attr("src", "../Images/zka.jpg");
        //取消所有监听
        clearInterval(monitor);
        //打开设备或者生成二维码
        if (e == 1) {
            //微信
            GetWXPay();
        }
        else if (e == 2) {
            //支付宝
            GetZFBPay();
        }
        else if (e == 3) {
            //设备扫码
            GetBarcode();
        }
    }

    //获取米雅预支付的二维码
    function GetWXPay() {
        //
        index = index + 1;
        billno = prescno + "" + index;
        //
        $.ajax("../Ajax/HttpHandler.ashx", {
            //async: false,
            data: {
                type: 1,
                billno: billno,
                costs: payments * 100,
                //
                method: "GetPayPic",
                classname: "PrescInfo"
            },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {
                $('#QR').html("<img src=\"data:image/jpg;base64," + data.value + "\"/>");
                //monitor = setInterval(waitPay, 1000);
                monitor = setInterval(searchPay, 1000);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //
                layer.alert("<h1 class='layerhight'>"+"微信支付二维码创建失败,请重新结算！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
            }
        });
    }

    //获取米雅预支付的二维码
    function GetZFBPay() {
        //
        index = index + 1;
        billno = prescno + "" + index;
        //
        $.ajax("../Ajax/HttpHandler.ashx", {
            //async: false,
            data: {
                type: 3,
                billno: billno,
                costs: payments * 100,
                //
                method: "GetPayPic",
                classname: "PrescInfo"
            },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {
                $('#QR').html("<img src=\"data:image/jpg;base64," + data.value + "\"/>");
                //monitor = setInterval(waitPay, 1000);
                monitor = setInterval(searchPay, 1000);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //
                layer.alert("<h1 class='layerhight'>"+"支付宝支付二维码创建失败,请重新结算！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
            }
        });
    }

    //设备扫码
    function GetBarcode() {
        //
        $('#QR').html("<span>请展示付款码并置于扫码区域！</span>");
        //
        deviceFlg = 1;
        //倒计时一分钟
         devicemonitor = setInterval(devicetimeout, 1*60*1000);
        //调用设备获取barcode
        connect();
    }
    //设备扫码倒计时
    function devicetimeout() {
        clearInterval(devicemonitor);
        //关闭连接
        closes();
        //
        deviceFlg = 0;
    }

    //获取米雅支付的字符串
    function GetDevicePay() {
        if (deviceFlg == 0) {
            layer.alert("<h1 class='layerhight'>"+"扫码超时，请重新选择支付方式！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
            return;
        }
        else {
            index = index + 1;
            billno = prescno + "" + index;
            //
            $.ajax("../Ajax/HttpHandler.ashx", {
                //async: false,
                data: {
                    barcode: barcode,
                    billno: billno,
                    costs: payments * 100,
                    //costs: 1,
                    //
                    method: "GetPay",
                    classname: "PrescInfo"
                },
                dataType: 'json', //服务器返回json格式数据
                type: 'post', //HTTP请求类型	
                timeout: 8000,
                success: function (data) {
                    if (data.opflg == '2') {
                        //关闭监听
                        closePay();
                        layer.msg("<h1 class='layerhight'>" +"支付成功！" + "</h1>",
                            { area: ['400px', 'auto'] }
                        );
                        //付款成功
                        clearcart();
                    }
                    //等待监控查询付款状态
                    if (data.opflg == '0') {
                        billno = data.billno;
                        //监听付款状态
                        monitor = setInterval(searchPay, 1000);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //
                    layer.alert("<h1 class='layerhight'>"+"支付失败,请重新结算！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
                    //关闭监听
                    closePay();
                }
            });
        }
    }
    var isBusy = false;
    //必要时，监控付款状态
    function searchPay() {
        //
        if (isBusy) return;
        isBusy = true;
        //
        $.ajax("../Ajax/HttpHandler.ashx", {
            //async: false,
            data: {
                billno: billno,
                //
                method: "SearchPay",
                classname: "PrescInfo"
            },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {
                if (data.opflg == '2') {
                    //关闭监听
                    closePay();
                    //layer.msg("<h1 class='layerhight'>" +"支付成功！" + "</h1>",
                        //    { area: ['400px', 'auto'] }
                        //);
                    //付款成功
                    clearcart();
                }
                //
                isBusy = false;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //
                layer.alert("<h1 class='layerhight'>"+"支付失败,请重新结算！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
                //关闭监听
                closePay();
                //
                isBusy = false;
            }
        });
    }

    //手动关闭付款页面时提示
    function comfirm() {
        var index1 = layer.confirm("<h1 class='layerhight'>" + "尚未支付成功，是否确认取消支付？" + "</h1>", {
            area: ['500px', 'auto'],
            btn: ["确定", "取消"],
            cancel: function (index, layero) {
                layer.close(index1);
            }
        },
            function () {
                layer.close(index1);
                //do something
                closePay();

            },
            function () {
                layer.close(index1);
            });
    }

    //监控付款状态
    function waitPay() {
        //ajax查询付款结果
        $.ajax({
            url: "../Ajax/HttpHandler.ashx",
            data: {
                prescno: prescno,
                //
                method: "GetPayOpflg",
                classname: "PrescInfo"
            },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {
                if (data) {
                    if (data.opflg == '2') {
                        //关闭监听
                        closePay();
                        //layer.msg("<h1 class='layerhight'>" +"支付成功！" + "</h1>",
                        //    { area: ['400px', 'auto'] }
                        //);
                        //付款成功
                        clearcart();
                    }
                    else if (data.opflg == '1') {
                        //支付失败
                    }
                }
            },
            error: function (xhr, type, errorThrown) {

            }
        });
    }

    //清空购物车
    function clearcart() {
        $.ajax({
            //async: false,
            url: "../Ajax/ShoppingCart.ashx",
            data: { optype: '3' },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {
                //
                ok = 1;
                window.location.href = "../Forms/PaySuccess.aspx?prescno=" + prescno + "&flg="+flg;
            },
            error: function (data) {
                //
                ok = 1;
                window.location.href = "../Forms/PaySuccess.aspx?prescno=" + prescno + "&flg="+flg;
            }
        });
    }

    //关闭支付页面
    function closePay() {
        paying = false;
        $('#payform').hide();
        //
        clearInterval(monitor);
        //取消监听
        clearInterval(cancelmonitor);
    }



    //连接设备
    function connect() {
        //开启
        timeout_flg = false;
        //
        vbar_open("localhost", "2693");
        vbar_register_webstatus_callback(websocketstatus);
        vbar_register_devicestatus_callback(websocketdevstatus);
        vbar_register_decode_callback(coderesult);
    }
    //扫码数据返回
    function coderesult(sym) {
        //响一声
        vbar_beep(1);
        //关闭设备
        closes();
        //二维码数据
        barcode = sym;
        GetDevicePay();
    }
    //设备状态返回（后续操作）
    function websocketdevstatus(status) {
        if (status == true) {
            //已连接
            //扫码类型
            //二维码
            vbar_addtype(1);
            //开灯
            lighton();
        }
        else {
            //未连接
            ////连接失败后，一直尝试连接，直到成功
            //connect();
            layer.alert("<h1 class='layerhight'>"+"二维码扫描设备连接失败！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
        }
    }
    //websocket连接状态返回（好像是如果失败则不停反馈）
    function websocketstatus(status) {
        if (status == true) {
            //已连接
        }
        else {
            //未连接
        }
    }
    //亮灯
    function lighton() {
        vbar_backlight(true);
    }
    //关灯
    function lightoff() {
        vbar_backlight(false);
    }
    //关闭服务
    function closes() {
        //关灯
        lightoff();
        //关服务
        vbar_close();
    }

</script>
