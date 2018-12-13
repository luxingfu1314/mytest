<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpLoad.aspx.cs" Inherits="Forms_UpLoad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>文件上传</title>
    <script src="../Scripts/jquery-1.10.2.js"></script>
    <script src="../Scripts/layui/layui.all.js"></script>
    <script src="../Scripts/layui/layui.js"></script>
    <link href="../Scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="../Css/upload.css" rel="stylesheet" />
</head>
<body style="background-color:aliceblue">
    <div class="main">
        <div style="background-color:azure;padding:40px;text-align:center">
            <span style="font-size:48px;font-weight:bold;margin:auto;line-height:60px">网站首页文件上传</span>
        </div>
        <div class="div" id="Logo">
            <label class="lables">Logo文件：</label>
            <i class="ico ico11" name="checkbox" onclick='checkItemStatus(this)'></i>
            <input type="file" name="file" class="filebox" onchange="checkPic(this)" />
        </div>
        <div class="div" id="Video">
            <label class="lables">视频文件：</label>
            <i class="ico ico11" name="checkbox" onclick='checkItemStatus(this)'></i>
            <input type="file" name="file" class="filebox"  onchange="checkVideo(this)" />
        </div>
        <div class="div" id="file">
            <label class="lables">友情提醒：</label>
            <i class="ico ico11" name="checkbox" onclick='checkItemStatus(this)'></i>
            <input type="file"  name="file" class="filebox" onchange="checkFile(this)" />
        </div>
        <div>
            <div class="div">
                <label class="lables">展播图片：</label>
                <i class="ico ico12" onclick="AddPic()"></i>
                <div style="padding-left:100px" id="Pic">
                </div>
            </div>
        </div>
        <div>
            <div class="div">
                <span class="lables">二维码图：</span>
                <i class="ico ico12" onclick="AddQr()"></i>(建议使用3个)
                <div style="padding-left:100px" id="QR">
                </div>
            </div>
        </div>
        <div style="background-color:azure;height:160px;" >
            <button class="btn" onclick="UpLoad()">批量上传</button>
        </div>
    </div>
    <script>
        $(function () {
            AddPic();
            AddQr();
        });
        function AddPic() {
            var appendjs = "";
            appendjs = "<div class=\"divn\">"
            appendjs += "<i class=\"ico ico11\" name = \"checkbox\" onclick = \"checkItemStatus(this)\" ></i >";
            appendjs += "<input type=\"file\" name=\"file\" class=\"filebox\"  onchange=\"checkPic(this)\"/>";
            //appendjs += "<i class=\"ico ico13\" onclick=\"Delete(this)\"></i>";
            appendjs += "</div>";
            //
            var picbox = $("#Pic");
            picbox.append(appendjs);
        }
        
        function AddQr() {
            var appendjs = "";
            appendjs = "<div class=\"divn\">"
            appendjs += "<i class=\"ico ico11\" name = \"checkbox\" onclick = \"checkItemStatus(this)\" ></i >";
            appendjs += "<input type=\"file\" name=\"file\" class=\"filebox\"  onchange=\"checkPic(this)\"/>";
            appendjs += "<input type=\"text\" name=\"text\" class=\"qrname\" placeholder=\"二维码名称...\" />";
            //appendjs += "<i class=\"ico ico13\" onclick=\"Delete(this)\"></i>";
            appendjs += "</div>";
            //
            var qrbox = $("#QR");
            qrbox.append(appendjs);
        }

        function Delete(v) {
            //获取父类
            var element = v.parentNode;
            //从爷爷类删除父类
            element.parentNode.removeChild(element);
        }

        function checkPic(o) {

            if (o.value == '') return;
            //
            if (o.value.indexOf('.png') <= -1 && o.value.indexOf('.jpg') <= -1 && o.value.indexOf('.jpeg') <= -1) {
                layer.alert("<h1 class='layerhight'>" + "文件格式有误，仅可上传PNG、JPG、JPEG格式文件！" + "</h1>",
                    {
                        area: ['500px', 'auto']
                    });
                o.value = '';
            }
        }

        function checkFile(o) {

            if (o.value == '') return;
            //
            if (o.value.indexOf('.txt') <= -1) {
                layer.alert("<h1 class='layerhight'>" + "文件格式有误，仅可上传TXT格式文件！" + "</h1>",
                    {
                        area: ['500px', 'auto']
                    });
                o.value = '';
            }
        }

        function checkVideo(o) {

            if (o.value == '') return;
            //
            if (o.value.indexOf('.mp4') <= -1) {
                layer.alert("<h1 class='layerhight'>" + "文件格式有误，仅可上传MP4格式文件！" + "</h1>",
                    {
                        area: ['500px', 'auto']
                    });
                o.value = '';
            }
        }

        function checkItemStatus(e) {
            var _checkflg = 0;
            var _class = $(e).attr('class');
            if (_class.indexOf("ico11") != -1) {
                _checkflg = 1;
            }
            //勾选状态
            if (_checkflg == 1) {
                $(e).attr('class', 'ico ico10');
            }
            else {
                $(e).attr('class', 'ico ico11');
            }
        };

        function UpLoad() {
            //提取数据
            GetFiles();
            if (!hasdata) {
                layer.alert("<h1 class='layerhight'>" + "上传数据不存在！" + "</h1>",
                    {
                        area: ['500px', 'auto']
                    });
                return;
            }
            //
            $.ajax({
                url: "../Ajax/UpLoad.ashx",
                type: "post",
                data: formData,
                /**
                *必须false才会自动加上正确的Content-Type
                */
                contentType: false,
                /**
                * 必须false才会避开jQuery对 formdata 的默认处理
                * XMLHttpRequest会对 formdata 进行正确的处理
                */
                processData: false,
                success: function (data) {
                    if (data.status == "true") {
                        layer.confirm("<h1 class='layerhight'>" + "上传成功！" + "</h1>",
                            {
                                area: ['500px', 'auto']
                            });
                    }
                    if (data.status == "error") {
                        layer.alert("<h1 class='layerhight'>" + data.msg + "</h1>",
                            {
                                area: ['500px', 'auto']
                            });
                    }
                },
                error: function () {
                    layer.alert("<h1 class='layerhight'>" + "上传失败！" + "</h1>",
                        {
                            area: ['500px', 'auto']
                        });
                }
            });
        }
        //
        var hasdata = false;
        var formData = new FormData();
        var picindex = 1;
        var qrindex = 1;
        //
        function GetFiles() {
            //
            hasdata = false;
            formData = new FormData();
            picindex = 1;
            qrindex = 1;
            //
            $('.ico11').each(
                function () {
                    //父节点下寻找文件
                    var parent = this.parentNode;
                    //数据
                    var file = $(parent).find(":input[name='file']")[0].files[0];
                    if (file != null) {
                        //获取id
                        var id = parent.id;
                        if (id == "") {
                            var gradeparent = parent.parentNode;
                            //获取id
                            var id = gradeparent.id;
                        }
                        //
                        var name = id;
                        //
                        if (id == "Pic") {
                            name = "Pic" + picindex;
                            picindex++;
                        }
                        else if (id == "QR") {
                            if (qrindex <= 9) {
                                //获取二维码名称
                                var text = $(parent).find(":input[name='text']")[0].value;
                                //
                                name = "QR" + qrindex + text;
                                qrindex++;
                            }
                        }
                        else if (id == "file") {
                            name = "友情提示";
                        }
                        //后缀名
                        var upFileName = file.name;
                        var index1 = upFileName.lastIndexOf(".");
                        var index2 = upFileName.length;
                        var suffix = upFileName.substring(index1 + 1, index2);//后缀名
                        //
                        formData.append(name, file, name + "." + suffix);
                        hasdata = true;
                    }
                });
     }
    </script>
</body>
</html>
