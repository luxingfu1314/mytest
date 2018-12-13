function deleteitem(refmainitemid) {
    var index = layer.confirm("<h1 class='layerhight'>" + "是否确认删除该商品？" + "</h1>", {
        area: ['500px', '300px'],
        btn: ["确定", "取消"],
        cancel: function (index, layero) {
            layer.close(index);
        }
    },
        function () {
            layer.close(index);
            //do something
            $.ajax({
                url: "../Ajax/ShoppingCart.ashx",
                data: { optype: '2', drugid: refmainitemid },
                dataType: 'json', //服务器返回json格式数据
                type: 'post', //HTTP请求类型	
                timeout: 8000,
                success: function (data) {
                    if (data.StatusCode != '200') {
                        layer.alert("<h1 class='layerhight'>" + data.Message + "</h1>",
                            {
                                area: ['500px', 'auto']
                            });
                        return;
                    }
                    else {
                        //
                        var element = document.getElementById(refmainitemid);
                        element.parentNode.removeChild(element);
                        //重新加载页面
                        update();
                    }
                },
                error: function (data) {
                    layer.alert("<h1 class='layerhight'>" + "error" + data + "</h1>",
                        {
                            area: ['500px', 'auto']
                        });
                }
            });
        },
        function () {
            layer.close(index);
        });
}

function checkAll(e) {
    var _checkflg = 0;
    var _class = $(e).attr('class');
    if (_class.indexOf("ico11") != -1) {
        _checkflg = 1;
    }
    $.ajax({
        url: "../Ajax/ShoppingCart.ashx",
        data: { optype: '5', checkflg: _checkflg },
        dataType: 'json', //服务器返回json格式数据
        type: 'post', //HTTP请求类型	
        timeout: 8000,
        success: function (data) {
            if (data.StatusCode != '200') {
                layer.alert("<h1 class='layerhight'>" + data.Message + "</h1>",
                    {
                        area: ['500px', 'auto']
                    });
                return;
            }
            else {
                //勾选状态
                if (_checkflg == 1) {
                    $('.ico11').each(
                        function () {
                            //全选
                            $(this).attr('class', 'ico ico10');
                        }
                    );
                }
                else {
                    $('.ico10').each(
                        function () {
                            //全选
                            $(this).attr('class', 'ico ico11');
                        }
                    );
                }
                //重新加载页面
                update();
            }
        },
        error: function (data) {
            layer.alert("<h1 class='layerhight'>" + "error" + data + "</h1>",
                {
                    area: ['500px', 'auto']
                });
        }
    });
};

function checkItemStatus(e) {
    var _checkflg = 0;
    var _class = $(e).attr('class');
    if (_class.indexOf("ico11") != -1) {
        _checkflg=1;
    }
    //获取药品id
    var id = $(e).attr('refmainitemid');
    //
    $.ajax({
        url: "../Ajax/ShoppingCart.ashx",
        data: { optype: '4', drugid: id, checkflg: _checkflg },
        dataType: 'json', //服务器返回json格式数据
        type: 'post', //HTTP请求类型	
        timeout: 8000,
        success: function (data) {
            if (data.StatusCode != '200') {
                layer.alert("<h1 class='layerhight'>" + data.Message + "</h1>",
                    {
                        area: ['500px', 'auto']
                    });
                return;
            }
            else {
                //勾选状态
                if (_checkflg == 1) {
                    $(e).attr('class', 'ico ico10');
                }
                else {
                    $(e).attr('class', 'ico ico11');
                }
                //重新加载页面
                update();
            }
        },
        error: function (data) {
            layer.alert("<h1 class='layerhight'>" + "error" + data + "</h1>",
                {
                    area: ['500px', 'auto']
                });
        }
    });
};

function deleteAll(){
    $.ajax({
        url: "../Ajax/ShoppingCart.ashx",
        data: { optype: '3' },
        dataType: 'json', //服务器返回json格式数据
        type: 'post', //HTTP请求类型	
        timeout: 8000,
        success: function (data) {
            if (data.StatusCode != '200') {
                layer.alert("<h1 class='layerhight'>" + data.Message + "</h1>",
                    {
                        area: ['500px', 'auto']
                    });
                return;
            }
            else {
                //
                $('.ico11').each(
                    function () {
                        //排除全选
                        var itemid = $(this).attr('refmainitemid');
                        if (itemid) {
                            var element = document.getElementById(itemid);
                            element.parentNode.removeChild(element);
                        }
                    }
                );
                //重新加载页面
                update();
                //location.reload();
            }
        },
        error: function (data) {
            layer.alert("<h1 class='layerhight'>" + "error" + data + "</h1>",
                {
                    area: ['500px', 'auto']
                });
        }
    });
};

function imgERROR(t, i) {
    t.src = "../Images/" + i;
};

function cartminusitem(e, tag) {
    var inputObject = $(e).parent().find('.text');
    var oldValue = inputObject.val();
    inputObject.val(parseInt(oldValue) - 1);
    cartprompt(inputObject, tag);
};

function cartadditem(e, tag) {
    var inputObject = $(e).parent().find('.text');
    var oldValue = inputObject.val();
    inputObject.val(parseInt(oldValue) + 1);
    cartprompt(inputObject, tag);
};

function cartprompt(e, tag) {
    //做输入校验处理，如果输入字符串，如果小于0，如何提示，如何处理
    var tip = $(e).parent().parent().find('.amount-msg');
    var oldValue = tip.find('span').html();

    var re = /^[0-9]+[0-9]*]*$/;
    var newValue = $(e).val();
    //获取药品id
    var id = $(e).attr('refmainitemid');
    //
    if (newValue < 0 || isNaN(newValue) || !re.test(newValue)) {
        layer.alert("<h1 class='layerhight'>" + "输入格式有误！" + "</h1>",
            {
                area: ['500px', 'auto']
            });
        $(e).val(oldValue);
        return;
    }
    else if (newValue == '0') {
        confirmDelete(id, newValue, oldValue,e, tip);
    }
    else {
        cartprompt_1(id, newValue, oldValue,e, tip);
    }
};

function cartprompt_1(id, newValue, oldValue, e, tip) {
    //
    var catalimit = $('#' + id).attr('catal');
    var limitBuy = $('#' + id).attr('limitbuy');
    var storage = $('#' + id).attr('storage');
    if (parseInt(newValue) < parseInt(limitBuy) && parseInt(limitBuy) > 0) {
        layer.alert("<h1 class='layerhight'>" + "购买数量不能低于" + limitBuy + "件！" + "</h1>",
            {
                area: ['500px', 'auto']
            });
        $(e).val(limitBuy);
        return;
    }
    if (parseInt(newValue) > parseInt(catalimit) && parseInt(catalimit) > 0) {
        layer.alert("<h1 class='layerhight'>" + "购买数量不能超过" + catalimit + "件！" + "</h1>",
            {
                area: ['500px', 'auto']
            });
        $(e).val(oldValue);
        return;
    }
    if (parseInt(newValue) > parseInt(storage)) {
        layer.alert("<h1 class='layerhight'>" + "库存不足！" + "</h1>",
            {
                area: ['500px', 'auto']
            });
        $(e).val(oldValue);
        return;
    }
    //记录上次数据
    tip.find('span').html(newValue);
    //获取id
    //修改数量
    $.ajax({
        url: "../Ajax/ShoppingCart.ashx",
        data: { optype: '4', drugid: id, quantity: newValue },
        dataType: 'json', //服务器返回json格式数据
        type: 'post', //HTTP请求类型	
        timeout: 8000,
        success: function (data) {
            if (data.StatusCode != '200') {
                layer.alert("<h1 class='layerhight'>" + data.Message + "</h1>",
                    {
                        area: ['500px', 'auto']
                    });
                return;
            }
            else {
                //重新加载页面
                update();
            }
        },
        error: function (data) {
            layer.alert("<h1 class='layerhight'>" + "error" + data + "</h1>",
                {
                    area: ['500px', 'auto']
                });
        }
    });
}

function confirmDelete(id, newValue, oldValue, e, tip) {
    var index = layer.confirm("<h1 class='layerhight'>" + "是否确认删除该商品？" + "</h1>", {
        area: ['500px', '300px'],
        btn: ["确定", "取消"],
        cancel: function (index, layero) {
            layer.close(index);
            $(e).val(oldValue);
        }
    },
        function () {
            layer.close(index);
            //do something
            $.ajax({
                url: "../Ajax/ShoppingCart.ashx",
                data: { optype: '2', drugid: id },
                dataType: 'json', //服务器返回json格式数据
                type: 'post', //HTTP请求类型	
                timeout: 8000,
                success: function (data) {
                    if (data.StatusCode != '200') {
                        layer.alert("<h1 class='layerhight'>" + data.Message + "</h1>",
                            {
                                area: ['500px', 'auto']
                            });
                        return;
                    }
                    else {
                        //
                        var element = document.getElementById(id);
                        element.parentNode.removeChild(element);
                        //重新加载页面
                        update();
                    }
                },
                error: function (data) {
                    layer.alert("<h1 class='layerhight'>" + "error" + data + "</h1>",
                        {
                            area: ['500px', 'auto']
                        });
                }
            });
        },
        function () {
            layer.close(index);
            $(e).val(oldValue);
        });
}

function update() {
    //更新每个药品的金额信息
    var totalPrice = 0;
    $('.ico11').each(
        function () {
            //获取药品id价格与单价
            var itemid = $(this).attr('refmainitemid');
            if (itemid) {
                var price = $('#' + itemid).attr('price');
                var quantity = $('#v_' + itemid).val();
                //根据数量更新金额
                price = parseFloat(price);
                quantity = parseInt(quantity);
                var price1 = price * quantity;
                var price2 = price1.toFixed(2);
                $('#tp_' + itemid).html('￥'+price2);
                //总价
                totalPrice = totalPrice + price1;
            }
        }
    );
    totalPrice = totalPrice.toFixed(2);
    $('#allprice').html('￥' +totalPrice);
};

function checkPrescStatus(e) {
    var _checkflg = 0;
    var _class = $(e).attr('class');
    if (_class.indexOf("ico11") != -1) {
        _checkflg = 1;
    }
    //获取药品id
    var id = $(e).attr('refmainitemid');
    //
    $.ajax({
        url: "../Ajax/ShoppingCart.ashx",
        data: { optype: '4', drugid: id, checkflg: _checkflg },
        dataType: 'json', //服务器返回json格式数据
        type: 'post', //HTTP请求类型	
        timeout: 8000,
        success: function (data) {
            if (data.StatusCode != '200') {
                layer.alert("<h1 class='layerhight'>" + data.Message + "</h1>",
                    {
                        area: ['500px', 'auto']
                    });
            }
            else {
                //勾选状态
                if (_checkflg == 1) {
                    $(e).attr('class', 'ico ico10');
                }
                else {
                    $(e).attr('class', 'ico ico11');
                }
                //重新加载页面
                update();
            }
        },
        error: function (data) {
            layer.alert("<h1 class='layerhight'>" + "error" + data + "</h1>",
                {
                    area: ['500px', 'auto']
                });
        }
    });
};



//手动发药
function manPresc() {
    //
    operationType = -1;
    indexWait = LoadWaitNoCancel("手动发药订单处理中，请耐心等候！", 5 * 60 * 1000);
    //
    Init();
    //获取结算数据
    GetManChartInfo();
    //手动发药
    if (druginfo.length > 0) {
        //创建手动发药订单并发药
        manPrescProdure();
    }
    else {
        operationType = 1;
        layer.close(indexWait);
        layer.alert("<h1 class='layerhight'>" + "购物车中暂无勾选商品！" + "</h1>",
            {
                area: ['500px', 'auto']
            });
    }
}

function GetManChartInfo() {
    $('.ico11').each(
        function () {
            //获取药品id价格与单价
            var itemid = $(this).attr('refmainitemid');
            if (itemid) {
                druginfo = druginfo + itemid + ",";
                var quantity = $('#v_' + itemid).val();
                druginfo = druginfo + quantity + ";";
            }
        }
    );
    if (druginfo.length > 0) {
        druginfo = druginfo.slice(0, druginfo.length - 1);
    }
}

function manPrescProdure() {
    //
    $.ajax({
        url: "../Ajax/HttpHandler.ashx",
        data: {
            posno: posno,
            consisposno: consisposno,
            druginfo: druginfo,//drugid列表
            //
            method: "ManPrescProdure",
            classname: "PrescInfo"
        },
        dataType: 'json', //服务器返回json格式数据
        type: 'post', //HTTP请求类型	
        timeout: 8000,
        success: function (data) {
            if (data) {
                if (data.code == '200') {
                    //
                    prescno = data.prescno;
                    //监控发药状态
                    timerIndex=setInterval(monitor, 1000);
                }
                else {
                    operationType = 1;
                    layer.close(indexWait);
                    layer.alert("<h1 class='layerhight'>" + "手动发药订单创建失败1，请重新提交！" + "</h1>",
                        {
                            area: ['500px', 'auto']
                        });
                }
            }
        },
        error: function (xhr, type, errorThrown) {
            operationType = 1;
            layer.close(indexWait);
            layer.alert("<h1 class='layerhight'>" + "手动发药订单创建失败2，请重新提交！" + "</h1>",
                {
                    area: ['500px', 'auto']
                });
        }
    });
}
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
            method: "GetManSendOpflg",
            classname: "PrescInfo"
        },
        dataType: 'json', //服务器返回json格式数据
        type: 'post', //HTTP请求类型	
        timeout: 8000,
        success: function (data) {

            if (data.opflg == 3) {
                //
                operationType = 1;
                layer.close(indexWait);
                //
                clearcart();
                //清空购物车,转到货位信息界面
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
            window.location.href = "../Forms/Drugs_02.aspx?";
        },
        error: function (data) {

        }
    });
}


//滚动条
var indexWait = -1;
//
//结算数据列表
//drugid列表
var druginfo = '';
//总金额
var totalPrice = 0;
//是否含有处方药
var containPrescription = 0;
//是否含单轨药
var containSigleTrack = 0;
//是否含双轨药
var containDoubleTrack = 0;
//是否含麻黄碱
var containEphedrine = 0;
//含黄麻碱数量
var EphedrineQuantity = 0;
//订单号
var prescno = '';
//base64图片
var prescPic = '';
//身份证号
var IDNumber = '';
//姓名
var Name = '';
//性别
var Sex = '';
//出生日期
var Birthday = '';
//病史
var medicalHistory = '';
//过敏史
var allergyHistory = '';
//电话号码
var phoneNo = '';

//去结算
function gotoCheckout() {
    //
    operationType = -1;
    indexWait = LoadWaitNoCancel("数据处理中，请耐心等候！", 5 * 60 * 1000);
    //
    Init();
    //获取结算数据
    GetChartInfo();
    //去结算
    if (druginfo.length > 0) {
        //是否含有处方药
        Check();
    }
    else {
        operationType = 1;
        layer.close(indexWait);
        layer.alert("<h1 class='layerhight'>" + "购物车中暂无勾选商品！" + "</h1>",
            {
                area: ['500px', 'auto']
            });
    }
};
//初始化数据
function Init() {
    //drugid列表（按顺序）
    druginfo = '';
    //总金额
    totalPrice = 0;
    //是否含有处方药
    containPrescription = 0;
    //是否含双轨药
    containDoubleTrack = 0;
    //是否含单轨药
    containSigleTrack = 0;
    //是否含麻黄碱
    containEphedrine = 0;
    //含黄麻碱数量
    EphedrineQuantity = '';
    //订单号
    prescno = '';
    //Base64图片
    prescPic = '';
    //身份证号
    identityCard = '';
    //身份证号
    IDNumber = '';
    //姓名
    Name = '';
    //性别
    Sex = '';
    //出生日期
    Birthday = '';
    //病史
    medicalHistory = '';
    //
    allergyHistory = '';
    //电话号码
    phoneNo = '';
}
//获取结算数据
function GetChartInfo() {
    $('.ico11').each(
        function () {
                //获取药品id价格与单价
            var itemid = $(this).attr('refmainitemid');
            if (itemid) {
                druginfo = druginfo + itemid + ",";
                var quantity = $('#v_' + itemid).val();
                druginfo = druginfo + quantity + ",";
                var price = $('#' + itemid).attr('price');
                druginfo = druginfo + price + ";";
                price = parseFloat(price);
                quantity = parseInt(quantity);
                //
                var prescription = $('#' + itemid).attr('Prescription');//是否处方药
                if (prescription == 'Y') {
                    containPrescription = 1;
                    //是否为双轨
                    var doubleTrack = $('#' + itemid).attr('DoubleTrack');//是否处方药
                    if (doubleTrack == 'Y') {
                        containDoubleTrack = 1;
                    }
                    else {
                        containSigleTrack = 1;
                    }
                }
                else {
                    //是否含黄麻碱
                    var ephedrine = $('#' + itemid).attr('Ephedrine');//是否处方药
                    if (ephedrine == 'Y') {
                        containEphedrine = 1;
                        //含黄麻碱数量
                        EphedrineQuantity = EphedrineQuantity + quantity;
                    }
                }

                //根据数量计算金额
                price = parseFloat(price);
                quantity = parseInt(quantity);
                var price1 = price * quantity;
                //总价
                totalPrice = totalPrice + price1;
            }
        }
    );
    if (druginfo.length > 0) {
        druginfo = druginfo.slice(0, druginfo.length - 1);
    }
}
//核查并分配道路处理
function Check() {
    //核查
    //含黄麻碱且选择数量超过两件
    if (containEphedrine == 1 && EphedrineQuantity > 2) {
        operationType = 1;
        layer.close(indexWait);
        layer.alert("<h1 class='layerhight'>" + "您所选含有黄麻碱药品数量超限，每日不得超过两件！" + "</h1>",
            {
                area: ['500px', 'auto']
            });
        return;
    }
    //OTC非黄麻碱及非药品类
    if (containPrescription == 0 && containEphedrine == 0) {
        Road1();
    }
    //OTC黄麻碱，身份证录入并核查路
    else if (containPrescription == 0 && containEphedrine == 1) {
        Road2();
    }
    //含处方药单轨，OTC非黄麻碱，处方拍照并核查路
    else if (containPrescription == 1 && containDoubleTrack == 0 && containEphedrine == 0) {
        Road3();
    }
    //含处方药单轨，OTC黄麻碱，处方拍照、身份证录入并核查路
    else if (containPrescription == 1 && containDoubleTrack == 0 && containEphedrine == 1) {
        Road4();
    }
    //含处方药双轨，OTC非黄麻碱，身份证录入并核查路
    else if (containDoubleTrack == 1 && containSigleTrack == 0 && containEphedrine == 0) {
        Road5();
    }
    //含处方药双轨，OTC黄麻碱，身份证录入并核查路
    else if (containDoubleTrack == 1 && containSigleTrack == 0&& containEphedrine == 1) {
        Road6();
    }
    //含处方药单、双轨，OTC非黄麻碱，处方拍照、身份证录入并核查路
    else if (containDoubleTrack == 1 && containEphedrine == 0) {
        Road7();
    }
    //含处方药单、双轨，OTC黄麻碱，处方拍照、身份证录入并核查路
    else if (containDoubleTrack == 1 && containEphedrine == 1) {
        Road8();
    }
    else {
        operationType = 1;
        layer.close(indexWait);
    }
}

//OTC非黄麻碱，直接下单路
function Road1() {
    //下单，并去付款页
    SaveBillInfo1();
}
//OTC黄麻碱，身份证录入并核查路
function Road2() {
    operationType = 1;
    layer.close(indexWait);
    var index1 = layer.confirm("<h1 class='layerhight'>" + "您当前勾选含黄麻碱药品，需扫描身份证实名制购买，是否继续操作？" + "</h1>", {
        area: ['500px', '300px'],
        btn: ["确定", "取消"],
        cancel: function (index, layero) {
            layer.close(index1);
        }
    },
        function () {
            layer.close(index1);
            //do something
            TackIdentityCard(CheckEphAndSave);
        },
        function () {
            layer.close(index1);
        });
}
//含处方药单轨，OTC非黄麻碱，处方拍照
function Road3() {
    operationType = 1;
    layer.close(indexWait);
    var index1 = layer.confirm("<h1 class='layerhight'>" + "您当前购选商品含有处方药品，需对纸质处方进行拍照，数据上传后需等待处方审核结果，该过程预计需5-15分钟，是否继续操作？" + "</h1>", {
        area: ['500px', '300px'],
        btn: ["确定", "取消"],
        cancel: function (index, layero) {
            layer.close(index1);
        }
    },
        function () {
            layer.close(index1);
            //do something
            PicAndSave();
        },
        function () {
            layer.close(index1);
        });
}
//含处方药单轨，OTC黄麻碱，处方拍照、身份证录入
function Road4() {
    operationType = 1;
    layer.close(indexWait);
    var index1 = layer.confirm("<h1 class='layerhight'>" + "您当前勾选商品含有处方及黄麻碱药品，需扫描身份证实名制购买、对纸质处方进行拍照，数据上传后需等待处方审核结果，该过程预计需5-15分钟，是否继续操作？" + "</h1>", {
        area: ['500px', '300px'],
        btn: ["确定", "取消"],
        cancel: function (index, layero) {
            layer.close(index1);
        }
    },
        function () {
            layer.close(index1);
            //do something
            TackIdentityCard(CheckEphAndPicAndSave);
        },
        function () {
            layer.close(index1);
        });
}
//含处方药双轨，OTC非黄麻碱，身份证、病史录入（不核查），并核查路
function Road5() {
    operationType = 1;
    layer.close(indexWait);
    var index1 = layer.confirm("<h1 class='layerhight'>" + "您当前勾选商品含有处方药品，需录入顾客信息，是否继续操作？" + "</h1>", {
        area: ['500px', '300px'],
        btn: ["确定", "取消"],
        cancel: function (index, layero) {
            layer.close(index1);
        }
    },
        function () {
            layer.close(index1);
            //do something
            TackUserInfo2(SaveBillInfo1);
        },
        function () {
            layer.close(index1);
        });
}
//含处方药双轨，OTC黄麻碱，身份证录入、病史录入并核查路
function Road6() {
    operationType = 1;
    layer.close(indexWait);
    var index1 = layer.confirm("<h1 class='layerhight'>" + "您当前勾选商品含有处方及黄麻碱药品，需扫描身份证实名制购买，是否继续操作？" + "</h1>", {
        area: ['500px', '300px'],
        btn: ["确定", "取消"],
        cancel: function (index, layero) {
            layer.close(index1);
        }
    },
        function () {
            layer.close(index1);
            //do something
            TackUserInfo1(CheckEphAndSave);
        },
        function () {
            layer.close(index1);
        });
}
//含处方药单轨、双轨，OTC非黄麻碱，处方拍照、身份证录入（不核查）、病史录入并核查路
function Road7() {
    operationType = 1;
    layer.close(indexWait);
    var index1 = layer.confirm("<h1 class='layerhight'>" + "您当前勾选商品含有处方药品，对纸质处方进行拍照、录入顾客信息，数据上传后需等待处方审核结果，该过程预计需5-15分钟，是否继续操作？" + "</h1>", {
        area: ['500px', '300px'],
        btn: ["确定", "取消"],
        cancel: function (index, layero) {
            layer.close(index1);
        }
    },
        function () {
            layer.close(index1);
            //do something
            TackUserInfo2(PicAndSave);
        },
        function () {
            layer.close(index1);
        });
}
//含处方药单轨、双轨，OTC黄麻碱，处方拍照、身份证录入、病史录入并核查路
function Road8() {
    operationType = 1;
    layer.close(indexWait);
    var index1 = layer.confirm("<h1 class='layerhight'>" + "您当前勾选商品含有处方药品，需扫描身份证实名制购买、对纸质处方进行拍照，数据上传后需等待处方审核结果，该过程预计需5-15分钟，是否继续操作？" + "</h1>", {
        area: ['500px', '300px'],
        btn: ["确定", "取消"],
        cancel: function (index, layero) {
            layer.close(index1);
        }
    },
        function () {
            layer.close(index1);
            //do something
            TackUserInfo1(CheckEphAndPicAndSave);
        },
        function () {
            layer.close(index1);
        });
}
//黄麻碱审核后，直接下单
function CheckEphAndSave() {
    //核查黄麻碱，若通过则直接下单
    CheckEphedrine(SaveBillInfo1);
}
//黄麻碱审核后，拍照并提交审核,通过后下单
function CheckEphAndPicAndSave() {
    CheckEphedrine(PicAndSave);
}
//拍照并提交审核,通过后下单
function PicAndSave() {
    TackPictrue(SaveBillInfo2);
}

//OTC核查麻黄碱
function CheckEphedrine(fun) {
    $.ajax({
        url: "../Ajax/HttpHandler.ashx",
        data: {
            identity: escape(IDNumber),
            method: "CheckEphedrine",
            classname: "PrescInfo"
        },
        dataType: 'json', //服务器返回json格式数据
        type: 'post', //HTTP请求类型	
        timeout: 8000,
        success: function (data) {
            if (data) {
                if (data.code == '200') {
                    //当日未购买
                    fun();
                }
                else {
                    layer.alert("<h1 class='layerhight'>" + "您今日已购买含黄麻碱药品,此类药品一天仅能购买一次！" + "</h1>",
                        {
                            area: ['500px', 'auto']
                        });
                }
            }
        },
        error: function (xhr, type, errorThrown) {
            layer.alert("<h1 class='layerhight'>" + "OTC核查麻黄碱失败，请重新提交！" + "</h1>",
                {
                    area: ['500px', 'auto']
                });
        }
    });
}
//拍照
function TackPictrue(fun) {
    var data = window.showModalDialog("TakePics.html", window, "dialogHeight:400px;dialogWidth:420px;status:no;scroll:no");

    if (data != null) {
        if (data == "") {
            layer.alert("<h1 class='layerhight'>" + "拍照数据未获得，请重新拍照！" + "</h1>",
                {
                    area: ['500px', 'auto']
                });
            return;
        }
        //
        prescPic = data;
        //
        fun();
    }
}
//身份证
function TackIdentityCard(fun) {
    //
    var WaitText = '请将身份证置于身份证识别区！';
    operationType = -1;
    indexWait = LoadWait(WaitText, 60000);//1分钟后自动退出
    //
    var readerState = CVR_IDCard.ListReaderCard();
    ////测试使用/////////////////////
    //IDNumber = "320322198911063658";//身份证号
    //Name = "刘栋";//姓名
    //Sex = "男";//性别
    //Birthday = "1989-11-06";//出生日期
    //operationType = 1;
    //layer.close(indexWait);
    //fun();
    //////////////////////////////
    //
    if (readerState == 1) {
        //连接正常，启动监听
        timerIndex = setInterval("ReadCard(" + fun + ")", 1000); //每秒执行一次
    }
    else if (readerState == 0) {
        operationType = 1;
        layer.close(indexWait);
        layer.alert("<h1 class='layerhight'>" + "无读卡器连接！" + "</h1>",
            {
                area: ['500px', 'auto']
            });
        return;
    }
}
var readcardbusy = false;
function ReadCard(fun) {
    if (readcardbusy) return;
    readcardbusy = true;
    var strReadResult = CVR_IDCard.ReadCard();
    if (strReadResult == "0") {
        operationType = 1;
        layer.close(indexWait);
        //
        IDNumber = CVR_IDCard.CardNo;//身份证号
        Name = CVR_IDCard.Name;//姓名
        Sex = CVR_IDCard.Sex;//性别
        Birthday = CVR_IDCard.Born;//出生日期
        readcardbusy = false;
        fun();
    }
    else {
        readcardbusy = false;
    }
}
//病史及真实身份信息
function TackUserInfo1(fun) {
    layer.open({
        type: 2,
        area: ['700px', '845px'],
        title: '顾客信息录入',
        scrollbar: false,
        btn: ['确认', '取消'],
        content: '../Forms/UserInfo_1.html',
        yes: function (index, layero) {
            ////测试使用/////////////////////
            //IDNumber = "320322198911063658";//身份证号
            //Name = "刘栋";//姓名
            //Sex = "男";//性别
            //Birthday = "1989-11-06";//出生日期
            //operationType = 1;
            //layer.close(indexWait);
            //fun();
            var iframeWin = window[layero.find('iframe')[0]['name']];
            //
            var ret = iframeWin.tack();
            //
            var diseasehistory = iframeWin.diseasehistory;
            var allergyhistory = iframeWin.allergyhistory;
            var phoneno = iframeWin.phoneno;
            var idcard = iframeWin.idcard;
            var name = iframeWin.name;
            var sex = iframeWin.sex;
            var born = iframeWin.born;
            //
            if (ret==1) {
                //成功
                IDNumber = idcard;//身份证号
                Name = name;//姓名
                Sex = sex;//性别
                Birthday = born;//出生日期
                medicalHistory = diseasehistory;
                allergyHistory = allergyhistory;
                phoneNo = phoneno;
                //
                layer.close(index);
                fun();
            }
        },
        bt2: function (index) {
            layer.close(index);
        }
    })
}
//病史及自己书写的身份信息
function TackUserInfo2(fun) {
    layer.open({
        type: 2,
        area: ['700px', '845px'],
        title: '顾客信息录入',
        scrollbar: false,
        btn: ['确认', '取消'],
        content: '../Forms/UserInfo_2.html',
        yes: function (index, layero) {
            //////测试使用/////////////////////
            //IDNumber = "320322198911063658";//身份证号
            //Name = "刘栋";//姓名
            //Sex = "男";//性别
            //Birthday = "1989-11-06";//出生日期
            //operationType = 1;
            //layer.close(indexWait);
            //fun();
            var iframeWin = window[layero.find('iframe')[0]['name']];
            //
            var ret = iframeWin.tack();
            //
            var diseasehistory = iframeWin.diseasehistory;
            var allergyhistory = iframeWin.allergyhistory;
            var phoneno = iframeWin.phoneno;
            var idcard = iframeWin.idcard;
            var name = iframeWin.name;
            var sex = iframeWin.sex;
            var born = iframeWin.born;
            if (ret == 1) {
                //成功
                IDNumber = idcard;//身份证号
                Name = name;//姓名
                Sex = sex;//性别
                Birthday = born;//出生日期
                medicalHistory = diseasehistory;
                allergyHistory = allergyhistory;
                phoneNo = phoneno;
                //
                layer.close(index);
                fun();
            }
        },
        bt2: function (index) {
            layer.close(index);
        }
    })
}

//下单并转至付款页面
function SaveBillInfo1() {
    SaveBillInfo(2, GotoPresc);
}
//下单，等待审核反馈
function SaveBillInfo2() {
    SaveBillInfo(0, SendCheck);
}
//存储数据并创建单号信息
function SaveBillInfo(opflg, fun) {
    //
    operationType = -1;
    indexWait = LoadWaitNoCancel("订单信息创建中...！", 1 * 60 * 1000);
    //
    $.ajax({
        url: "../Ajax/HttpHandler.ashx",
        data: {
            identity: IDNumber,//身份证号
            name: Name,//姓名
            sex: Sex,//性别
            birthday: Birthday,//出生日期
            phoneno: phoneNo,//电话号码
            costs: totalPrice,//总金额
            medicalHistory: medicalHistory,//病史
            allergyHistory: allergyHistory,//过敏史
            //
            prescPic: prescPic,//Base64图片
            //
            druginfo: druginfo,//drugid列表
            //
            opflg: opflg,
            //
            method: "SavePrescInfo",
            classname: "PrescInfo"
        },
        dataType: 'json', //服务器返回json格式数据
        type: 'post', //HTTP请求类型	
        timeout: 8000,
        success: function (data) {
            if (data) {
                if (data.code == '200') {
                    //订单编号
                    prescno = data.prescno;
                    operationType = 1;
                    layer.close(indexWait);
                    fun();
                }
                else {
                    operationType = 1;
                    layer.close(indexWait);
                    layer.alert("<h1 class='layerhight'>" + "订单提交失败1，请重新提交！" + "</h1>",
                        {
                            area: ['500px', 'auto']
                        });
                }
            }
        },
        error: function (xhr, type, errorThrown) {
            operationType = 1;
            layer.close(indexWait);
            layer.alert("<h1 class='layerhight'>" + "订单提交失败2，请重新提交！" + "</h1>",
                {
                    area: ['500px', 'auto']
                });
        }
    });
}
//发送审方数据并开启监控
function SendCheck() {
    //
    operationType = -1;
    indexWait = LoadWaitNoCancel("审方数据上传中...！", 5 * 1000);
    //获取上传数据
    $.ajax({
        //async: false,
        url: "../Ajax/HttpHandler.ashx",
        data: {
            prescno: prescno,
            //
            method: "SendYaoXZCheck",
            classname: "PrescInfo"
        },
        dataType: 'json', //服务器返回json格式数据
        type: 'post', //HTTP请求类型	
        timeout: 50000,
        success: function (data) {
            if (data.code == 200) {
                operationType = 1;
                layer.close(indexWait);
                //启动监控
                var timer = setInterval(WaitCheck, 5 * 1000);
                //处方审核
                var WaitText = '处方审核中，审核所需时间可能较长，请耐心等待！';
                _operationType = -1;
                indexWait = _LoadWait(WaitText, 900000, timer);//15分钟后自动退出
            }
            else {
                operationType = 1;
                _operationType = 1;
                layer.close(indexWait);
                layer.alert("<h1 class='layerhight'>" + "订单提交审核失败，无法进行审核！" + "</h1>",
                    {
                        area: ['500px', 'auto']
                    });
            }
        },
        error: function (xhr, type, errorThrown) {
            operationType = 1;
            layer.close(indexWait);
            layer.alert("<h1 class='layerhight'>" + "订单提取失败，无法进行审核！" + "</h1>",
                {
                    area: ['500px', 'auto']
                });
        }
    });
}
//等待审核
var checkbusy = false;
function WaitCheck() {
    if (checkbusy) return;
    checkbusy = true;
    //监听
    //ajax查询审核结果
    $.ajax({
        url: "../Ajax/HttpHandler.ashx",
        data: {
            prescno: prescno,
            //
            method: "GetYaoXZResult",
            classname: "PrescInfo"
        },
        dataType: 'json', //服务器返回json格式数据
        type: 'post', //HTTP请求类型	
        timeout: 8000,
        success: function (data) {
            if (data) {
                if (data.opflg == '1') {
                    //订单审核失败
                    _operationType = 1;
                    layer.close(indexWait);
                    layer.alert("<h1 class='layerhight'>" + "驳回原因：" + data.retmsg + "</h1>",
                        {
                            area: ['500px', 'auto']
                        });
                }
                if (data.opflg == '2') {
                    //订单审核成功
                    _operationType = 1;
                    layer.close(indexWait);
                    //
                    window.location.href = "../Forms/PrescDetail.aspx?prescno=" + prescno + "&flg=1";
                }
            }
            checkbusy = false;
        },
        error: function (xhr, type, errorThrown) {
            checkbusy = false;
        }
    });
}
//去付款
function GotoPresc() {
    window.location.href = "../Forms/PrescDetail.aspx?prescno=" + prescno + "&flg=0";
}

//操作指令，启动及关闭前必须要置于-1；状态值：-1操作超时、0取消、1成功
var _operationType = -1;
//可取消滚动条
function _LoadWait(WaitText, Timeout, timer) {
    var index = layer.msg(
        '<h1 class="WaitIng">' + WaitText + '</h1>'//样式需要你自己定义，或者直接写内容
        , {
            zIndex: 20161231//更改窗口层次
            , area: ['500px', 'auto']
            , icon: 16
            , time: Timeout//自动关闭
            , anim: 1
            , shade: [0.2, '#FFF']
            , skin: 'layer_my_msg_load'
            , btn: ['取消']
            , btn1: function (index, layero) {
                _operationType = 0;
                layer.close(index);
            }
            , end: function () {
                clearInterval(timer);
                if (_operationType == -1) {
                    layer.alert("<h1 class='layerhight'>" + '操作超时！' + "</h1>",
                        {
                            area: ['500px', 'auto']
                        });
                }
            }
        }
    );
    return index;
}



