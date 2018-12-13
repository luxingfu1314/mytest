<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaySuccess.aspx.cs" Inherits="Forms_PaySuccess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>支付成功</title>
    <link href="../Css/base.css" rel="stylesheet" />
    <link href="../Css/style.css" rel="stylesheet" />
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
            <div class="payok">
                <p class="img">
                    <img src="../images/payok.png" /></p>
                <p class="ti">支付成功!</p>
                <%--<p class="ci">设备开始发药，请勿离开！</p>--%>
            </div>
            <div class="enav">
                <div class="nav">
<%--                    <div class="lit"><a href="javascript:history.go(-1);"><i class="ico ico8"></i></a></div>
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
        <%--打印机--%>
        <div style="display:none">
            <object id="PrintStatus" CLASSID="clsid:044F0BF3-9131-4692-A8FF-DF374BA183F7"
                codebase="../../OCX/Msprintsdk.ocx#version=1,0,0,0"
                width="100" height="100"></object>
        </div>
    </div>
</body>
</html>
<script>
    var prescno = '<%=Prescno%>';
    var posno = '<%=PosNo%>';
    var yxzflg = '<%=Flg%>';
    var A3 = '<%=A3%>';
    var A4 = '<%=A4%>';
    var A5 = '<%=A5%>';

    var closeFlg = false;//是否为主动关闭网页
    var curopflg = 0;
    //0已付款,未配药，1配药中，2已配药,开始发药，3，发药结束
    $(function () {
        operationType = -1;
        indexWait = LoadWaitNoCancelFun("设备正在发药，请耐心等候！", 5 * 60 * 1000,EndSendDrug);
        //如果含处方药，则监听通知药先知
        if (yxzflg == "1") {
            yaoxianzhi();//通知药先知已付款
        }
        //发药
        senddrug();
    });

    //离开页面提醒
    $(window).on('beforeunload', function () {
        if (closeFlg) {
            return '离开此页可能导致发药失败或信息丢失，是否确认离开此页？';
        }
    });

    //药先知返回发药状态
    function yaoxianzhi() {
        $.ajax("../Ajax/HttpHandler.ashx", {
            data:{
                orderNo: prescno,
                orderState: curopflg,
                //
                method: "SendYaoXZ",
                classname: "PrescInfo"
            },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                operationType = 1;
                layer.close(indexWait);
            }
        });
    }
    
    //发药
    function senddrug() {
        //201,获取发药窗口，208
        $.ajax({
            url: "../Ajax/HttpHandler.ashx",
            data: {
                prescno: prescno,
                posno: posno,
                //
                method: "SendDrugs",
                classname: "PrescInfo"
            },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 80000,
            success: function (data) {
                //监控发药状态
                timerIndex=setInterval(monitor, 1000);
            },
            error: function (data) {
                operationType = 1;
                layer.close(indexWait);
                //
                layer.alert("<h1 class='layerhight'>"+"发药设备故障，请联系药店相关人员！"+"</h1>",
                        {
                            area: ['500px', 'auto']
                        });
            }
        });
    }
    //发药监控
    var monitorBusy = false;
    function monitor() {
        //
        if (monitorBusy) return;
        monitorBusy = true;
        //获取发药状态
        $.ajax({
            url: "../Ajax/HttpHandler.ashx",
            data: {
                prescno: prescno,
                //
                method: "GetSendOpflg",
                classname: "PrescInfo"
            },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {
                
                if (data.opflg != curopflg) {
                    //
                    if (data.opflg == 1 || data.opflg == 2 || data.opflg == 3) {
                        curopflg = data.opflg;
                        if (yxzflg == "1") {
                            yaoxianzhi();
                        }
                        //发药完成
                        if (data.opflg >= 3) {
                            //关闭发药监控
                            clearInterval(timerIndex);
                            //关闭弹出层
                            operationType = 1;
                            layer.close(indexWait);
                        }
                    }
                }
                monitorBusy = false;
            },
            error: function (data) {
                operationType = 1;
                layer.close(indexWait);
                monitorBusy = false;
            }
        });
    }


    //成功与否结束时开始打印凭条和发送ERP订单
    function EndSendDrug() {
        //发送ERP销售订单
        senderp();
        //打印清单
        print();
        //
        closeFlg = true;
        window.location.href = "../Forms/HomePage.aspx?";
    }
    //ERP销售订单
    function senderp() {
        $.ajax("../Ajax/HttpHandler.ashx", {
            async: false,
            data: {
                prescno: prescno,
                //
                method: "SendERP",
                classname: "PrescInfo"
            },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });
    }
    //获取打印数据
    function print() {
        //
        $.ajax("../Ajax/HttpHandler.ashx", {
            async: false,
            data: {
                prescno: prescno,
                //
                method: "GetPrintData",
                classname: "PrescInfo"
            },
            dataType: 'json', //服务器返回json格式数据
            type: 'post', //HTTP请求类型	
            timeout: 8000,
            success: function (data) {
                printdata(data);
            },
             error: function (XMLHttpRequest, textStatus, errorThrown) {
                
            }
        });
    }
    //打印清单
    function printdata(data) {
        //打印初始化
        PrintStatus.SetPrintport("COM4", 38400);
        var res = PrintStatus.SetInit();
        if (res == 1) {
            //打印机初始化失败
            layer.alert("<h1 class='layerhight'>" + "打印机初始化失败！" + "</h1>",
            {
                area: ['500px', 'auto']
            });
            return;
        }
        //打印数据
        PrintStatus.PrintString("北京医保全新大药房有限责任公司景顺东街店");
        PrintStatus.PrintString("");
        PrintStatus.PrintString("销售订单："+prescno);
        PrintStatus.PrintString("门店编号："+A3+"  POS机号："+A4);
        PrintStatus.PrintString("收银员："+A5);
        PrintStatus.PrintString("日期：" + data.prescdate);
        PrintStatus.PrintString("==========================");
        PrintStatus.PrintString("名称  规格  单位  厂家  批号  实价  数量  金额");
        PrintStatus.PrintString("==========================");
        //
        for (var i = 0; i < data.drugs.length; i++) {
            PrintStatus.PrintString("/" + data.drugs[i].drug_name+"/" + data.drugs[i].drug_spec+"/" + data.drugs[i].unit);
            PrintStatus.PrintString(data.drugs[i].firm_name);
            PrintStatus.PrintString(data.drugs[i].makeno);
            PrintStatus.PrintString("实价：" + data.drugs[i].price.toFixed(2) +"  "+ data.drugs[i].quantity.toFixed(2) +"  "+ data.drugs[i].costs.toFixed(2));
            PrintStatus.PrintString("------------------------------------------------");
        }
        PrintStatus.PrintString("本单合计品种数量：" + data.rows);
        PrintStatus.PrintString("应收金额："+data.costs.toFixed(2)+"  优惠：0.00");
        PrintStatus.PrintString("实收金额："+data.payments.toFixed(2)+"  找零：0.00");
        PrintStatus.PrintString("==========================");
        PrintStatus.PrintString("地址：北京市朝阳区京顺东街6号院2号楼1层1081");
        PrintStatus.PrintString("电话：010-8430838");
        PrintStatus.PrintString("问您提示：为保证顾客的用药安全，");
        PrintStatus.PrintString("药品售出无质量问题不得退货，敬请谅解！");
        //
        PrintStatus.PrintFeedline(5);
        PrintStatus.SetClean();
        PrintStatus.PrintCutpaper(0);
        //关闭端口
        PrintStatus.SetClose();
    }

</script>
