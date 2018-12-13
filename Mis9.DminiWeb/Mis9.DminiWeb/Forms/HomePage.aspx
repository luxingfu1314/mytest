<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HomePage.aspx.cs" Inherits="Forms_HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>自助售药机</title>
    <meta name="keywords" content="" />
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <link rel="stylesheet" href="../Css/base.css" />
    <link rel="stylesheet" href="../Css/style.css" />
    <link href="../Css/dmini.css" rel="stylesheet" />
    <script src="../Scripts/dmini.js"></script>
    <link href="../Css/video-js.css" rel="stylesheet" />
    <script src="../Scripts/video/video.min.js"></script>
    <script src="../Scripts/video/videojs-ie8.min.js"></script>
    <script src="../Scripts/jquery-1.10.2.js"></script>
    <script src="../Scripts/text.js"></script>
</head>
<body>
    <div class="hide-gdt">
        <div class="main index">
            <div class="logovt">
                <div class="logoimg">
                    <img src="<%=GetLogo() %>" width="270" height="102" /></div>
                <div class="time">
                    <div>
                        <span style="margin:auto"><%=GetDayString() %></span>
                    </div>
                    <div>
                        <span style="margin:auto"><%=GetWeather() %></span>
                        <%--//
                    <iframe name="weather_inc" src="http://i.tianqi.com/index.php?c=code&id=11&site=24&color=%23FFFFFF&num=2" width="226" height="36" scrolling="no" security="restricted" sandbox=""></iframe>--%>
                    </div>
                </div>
            </div>
            <div class="advd clearfix">
                <div class="advew">
                    <video id="my-video" class="video-js" controls preload="auto" width="571" height="400"
                        poster="MY_VIDEO_POSTER.jpg" data-setup="{}">
                        <source src="../UserImage/Video.mp4" type="video/mp4" />
                    </video>
                </div>
                <div class="adimg" id="tab">
                    <%=PicJs() %>
                </div>
            </div>
            <div class="wxts">
                <span>
                    <img src="../Images/isj_18.png" width="84" height="70" />
                </span>
                <%=GetYQTS() %>
            </div>
            <div class="fuit clearfix">
                <div class="dsor">
                    <div class="ds1" <%=MarginJS %>>
                        <p class="img">
                            <a href="../Forms/Drugs_01.aspx">
                                <img src="../Images/i3-3.png" width="596" height="240" /></a>
                        </p>
                        <div class="tex">
                            <a href="../Forms/Drugs_01.aspx">
                                <p class="ti">分类查询</p>
                                <p class="ci">Drug classification</p>
                            </a>
                        </div>
                    </div>
                    <div class="ds1" <%=LocDisJS %>>
                        <p class="img">
                            <a href="../Forms/Drugs_02.aspx">
                                <img src="../Images/i2-3.png" width="596" height="240" /></a>
                        </p>
                        <div class="tex">
                            <a href="../Forms/Drugs_02.aspx">
                                <p class="ti">货位信息</p>
                                <p class="ci">Cargo information</p>
                            </a>
                        </div>
                    </div>
                    <%--<div class="ds1" <%=MarginJS %>>
                        <p class="img">
                            <a href="../Forms/Drugs_03.aspx">
                                <img src="../Images/i3-2.png" width="596" height="193" /></a>
                        </p>
                        <div class="tex">
                            <a href="../Forms/Drugs_03.aspx">
                                <p class="ti">药品推荐</p>
                                <p class="ci">Drug recommendation</p>
                            </a>
                        </div>
                    </div>--%>
                    <div class="ds1" <%=MarginJS %>>
                        <p class="img">
                            <a href="../Forms/Drugs_04.aspx">
                                <img src="../Images/i4-3.png" width="596" height="240" /></a>
                        </p>
                        <div class="tex">
                            <a href="../Forms/Drugs_04.aspx">
                                <p class="ti">人体导航</p>
                                <p class="ci">Human navigation</p>
                            </a>
                        </div>
                    </div>
                    <div class="ds1" <%=MarginJS %>>
                        <p class="img">
                            <a href="../Forms/Drugs_05.aspx">
                                <img src="../Images/i7-3.png" width="596" height="240" /></a>
                        </p>
                        <div class="tex">
                            <a href="../Forms/Drugs_05.aspx">
                                <p class="ti">药品查询</p>
                                <p class="ci">Search Drugs</p>
                            </a>
                        </div>
                    </div>
                </div>
                <div class="erm">
                    <ul>
                        <%=GetQRJS() %>
                        <%--<li>
                            <p class="img">
                                <img src="../UserImage/QR1.png" width="242" height="242" /></p>
                            <p class="mt20">二维码1</p>
                        </li>
                        <li>
                            <p class="img">
                                <img src="../UserImage/QR2.png" width="242" height="242" /></p>
                            <p class="mt20">二维码2</p>
                        </li>
                        <li>
                            <p class="img">
                                <img src="../UserImage/QR3.png" width="242" height="242" /></p>
                            <p class="mt20">二维码3</p>
                        </li>--%>
                    </ul>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        
        var myPlayer = videojs('my-video');
        videojs("my-video").ready(function () {
            var myPlayer = this;
            myPlayer.play();
        });
        window.onload = function () {

            var images = $('#tab').find('img');
            var pos = 0;
            var len = images.length;

            setInterval(function () {

                images[pos].style.display = 'none';
                pos = ++pos == len ? 0 : pos;
                images[pos].style.display = 'inline';

            }, 2000);

        };
    </script>
</body>
</html>
