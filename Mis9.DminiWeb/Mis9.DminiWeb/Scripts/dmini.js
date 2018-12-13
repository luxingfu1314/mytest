function Minusitem(e, tag) {
    var inputObject = $(e).parent().find('.text');
    var oldValue = inputObject.val();
    inputObject.val(parseInt(oldValue) - 1);
    Prompt(inputObject, tag);
};

function Additem(e, tag) {
    var inputObject = $(e).parent().find('.text');
    var oldValue = inputObject.val();
    inputObject.val(parseInt(oldValue) + 1);
    Prompt(inputObject, tag);
};

function Prompt(e, tag) {
    //做输入校验处理，如果输入字符串，如果小于0，如何提示，如何处理
    var tip = $(e).parent().parent().find('.amount-msg');
    var oldValue = tip.find('span').html();
    var re = /^[0-9]+[0-9]*]*$/;
    var newValue = $(e).val();
    if (newValue < 0 || isNaN(newValue) || !re.test(newValue)) {
        layer.alert("<h1 class='layerhight'>" + "输入格式有误！" + "</h1>",
            {
                area: ['500px', 'auto']
            });
        $(e).val(oldValue);
        return;
    }
    else if (newValue == 0) {
        layer.alert("<h1 class='layerhight'>" + "购买数量需大于0！" + "</h1>",
            {
                area: ['500px', 'auto']
            });
        $(e).val(oldValue);
        return;
    }
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
}

function showInstructions(e) {
    var tip = $(e).parent().parent().parent().find('.cfri');
    var newValue = $(e).attr('value');
    tip.find('span').html(newValue);
    //除了自己的
    $(e).addClass("current").siblings().removeClass("current");
}

function imgERROR(t, i) {
    t.src = "../Images/" + i;
};

function goTop() {
    $("html,body").animate({ scrollTop: 0 }, 500);
  }; 

var timeoutGoHomePage = -1;
function goHomePageTimer() {
    timeoutGoHomePage=setInterval(goHomePage, 5*60*1000);
  }; 

function goHomePage() {
    clearInterval(timeoutGoHomePage);
    window.location.href = "../Forms/HomePage.aspx?";
  }; 


function timerWeb() {
    //监听药品详情展示查询
    layer.msg("<h1 class='layerhight'>" + "弹出测试！" + "</h1>",
        { area: ['400px', 'auto'] });
};


//操作指令，启动及关闭前必须要置于-1；状态值：-1操作超时、0取消、1成功
var operationType = -1;
//循环监听id，若不存在不影响
var timerIndex;
//循环监听id，若不存在不影响
var timerIndex2;
//可取消滚动条
function LoadWait(WaitText, Timeout) {
    var index = layer.msg(
        '<h1 class="WaitIng">' + WaitText + '</h1>'//样式需要你自己定义，或者直接写内容
        , {
            zIndex: 20161231//更改窗口层次
            ,area: ['500px', 'auto']
            , icon: 16
            , time: Timeout//自动关闭
            , anim: 1
            , shade: [0.2, '#FFF']
            , skin: 'layer_my_msg_load'
            , btn: ['取消']
            , btn1: function (index, layero) {
                operationType = 0;
                layer.close(index);
            }
            , end: function () {
                clearInterval(timerIndex);
                clearInterval(timerIndex2);
                if (operationType == -1) {
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
//滚动条
function LoadWaitNoCancel(WaitText, Timeout) {
    var index = layer.msg(
        '<h2 class="WaitIng">' + WaitText + '</h2>'//样式需要你自己定义，或者直接写内容
        , {
            zIndex: 20161231//更改窗口层次
            , area: ['500px', 'auto']
            , icon: 16
            , time: Timeout//自动关闭
            , anim: 1
            , shade: [0.2, '#FFF']
            , skin: 'layer_my_msg_load'
            , end: function () {
                clearInterval(timerIndex);
                clearInterval(timerIndex2);
                if (operationType == -1) {
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
//滚动条含结束后执行的方法
function LoadWaitNoCancelFun(WaitText, Timeout, fun) {
    var index = layer.msg(
        '<h2 class="WaitIng">' + WaitText + '</h2>'//样式需要你自己定义，或者直接写内容
        , {
            zIndex: 20161231//更改窗口层次
            , area: ['500px', 'auto']
            , icon: 16
            , time: Timeout//自动关闭
            , anim: 1
            , shade: [0.2, '#FFF']
            , skin: 'layer_my_msg_load'
            , end: function () {
                clearInterval(timerIndex);
                clearInterval(timerIndex2);
                if (operationType == -1) {
                    layer.alert("<h1 class='layerhight'>" + '操作超时！' + "</h1>",
                        {
                            area: ['500px', 'auto']
                        });
                }
                //
                fun();
            }
        }
    );
    return index;
}
//
//禁止右键操作
document.oncontextmenu = function rightClick() { return false; };
//禁止拖拽
document.ondragstart = "return false";
//禁止双击
document.ondblclick = "return false";





