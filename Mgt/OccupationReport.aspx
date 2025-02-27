<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="OccupationReport.aspx.cs" Inherits="Mgt_OccupationReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $("#accordion").accordion();
        });       
        $(function () {
            $("#tabs").tabs();
        });
    </script>
     <style type="text/css">
        .ui-tabs .ui-tabs-nav li {
            padding: 0 5px;
            text-align:left;
        }
        .ContentDiv {
            z-index: 999;
            border: none;
            margin: 0px;
            padding: 0px;
            width: 100%;
            height: 100%;
            top: 0px;
            left: 0px;
            background-color: rgba(0, 0, 0, 0.6);
            /*opacity: 0.6;
            filter: alpha(opacity=60);*/
            position: fixed;
            text-align: left;
            line-height: 25px;
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="path txtS mb20">現在位置：<a href="#">報表作業</a> <i class="fa fa-angle-right"></i><a href="OccupationReport.aspx">證書管理統計報表</a>
        <div class="right">
            <%--<input type="button" value="?" onclick="showMSG()" />--%>
        </div>
    </div>
    <div id="accordion">
<%--        <h3>職類統計</h3>
        <div class="both mb20">
            <fieldset>
                <legend>功能列</legend>
                <div class="left w10">
                    證書到期年度：<asp:TextBox ID="txt_Year" runat="server" class="required datepicker date w1"></asp:TextBox>
                    <div class="right">
                        <asp:Button ID="btnExport" runat="server" Text="匯出" OnClick="btnExport_Click" />
                    </div>
                </div>
            </fieldset>
            <fieldset>
                <legend>增加條件</legend>
                <div class="left w10">
                    <asp:CheckBox ID="chk_PrsnContract" runat="server" />為戒菸服務合約人員
                </div>
            </fieldset>
        </div>--%>
        <h3>證書統計</h3>
        <div class="both mb20">
            <fieldset>
                <legend>功能列</legend>
                <div class="left w10">
                    證書到期年度：<asp:TextBox ID="txt_YearTwo" runat="server" class="required datepicker date w1"></asp:TextBox>(請輸入西元年)
                    <div class="right">
                        <asp:Button ID="btn_YearTwo" runat="server" Text="匯出" OnClick="btnCity_Click" />
                    </div>
                </div>
            </fieldset>
        </div>
    </div>
            <!--只有按下按鈕時才會出現-->

    <div id="c_content" class="ContentDiv">

        <div style="background-color: white; opacity: 0.95; padding: 20px 30px; border-radius: 5px; margin: 100px auto 0 auto; z-index: 999; width: 700px; height: 550px;">

            <a onclick="hideMSG()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

            <h1>TIPS</h1>
            <p style="margin-left: 10px">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                </asp:UpdatePanel>
            </p>
            <div id="tabs" style="height: 390px; margin-bottom: 20px; margin-top: 10px;">
                <ul>
                    <%--<li><a href="#tabs-1" >職類統計</a></li>--%>
                    <li id="Role_view" runat="server" visible="false"><a href="#tabs-2">寄送簡訊通知</a></li>
                    <li><a href="#tabs-3">縣市統計</a></li>
                </ul>
                <div id="tabs-1">
<%--                    <div  >
                        ※本功能可匯出以下資料：<br />
                        1、按職類及按季統計輸入年度之證書到期人數、已換證人數、未換證人數及換證率。<br />
                        2、按職類統計輸入年度後四年之證書到期人數。<br />
                        3、可增加勾選「為戒菸服務合約人員」條件。<br />
                    </div>--%>
                </div>
                <div id="Tabs_2" runat="server" visible="false">
                    <div id="tabs-2">
                        <div>
                            <asp:TextBox ID="txt_SMS" runat="server" type="text" TextMode="MultiLine" Width="100%" Height="100px" Text=""></asp:TextBox>
                        </div>
                        <div class="MarginTop">
                            <asp:Button ID="btnSendSMS" runat="server" Text="寄送簡訊" />
                        </div>
                    </div>
                </div>
                <div id="tabs-3">
                    <div class="MarginTop">
                        ※本功能可匯出以下資料：<br />
                        按職類按執登縣市統計輸入年度之證書到期人數，匯出結果會按季別分頁顯示。<br />
                    </div>
                </div>
            </div>


        </div>
    </div>
    <!--只有按下按鈕時才會出現-->
    <script type="text/javascript">

        function alertkick() {
            alert('簽退成功');
        }

        function showNote() {
            $("#Note_content").show("fade");
        }
        function hideNote() {
            $("#Note_content").hide("fade");
        }

        function showMSG() {
            $("#c_content").show("fade");
        }

        function hideMSG() {
            $("#c_content").hide("fade");
        }

        function showOpen() {
            alert(1);
        }
        function getParameterByName(name, url) {
            if (!url) url = window.location.href;

            name = name.replace(/[\[\]]/g, '\\$&');

            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);

            if (!results) return null;

            if (!results[2]) return '';
            var qstring = results[2];
            var winvar = window.open('./EventInvite.aspx?sno=' + qstring, 'winname', 'width=1200 height=550 location=no,menubar=no status=no,toolbar=no');

        }
        function Manager(name, url) {
            if (!url) url = window.location.href;

            name = name.replace(/[\[\]]/g, '\\$&');

            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);

            if (!results) return null;

            if (!results[2]) return '';
            var qstring = results[2];
            var twenty = getUrlVars()['twenty'];
            if (twenty == 1) {//22縣市學分管理

                var winvar = window.open('./EventManager.aspx?sno=' + qstring + '&twenty=1', 'winname', 'width=1200 height=550 location=no,menubar=no status=no,toolbar=no');
            }
            else {
                var winvar = window.open('./EventManager.aspx?sno=' + qstring, 'winname', 'width=1200 height=550 location=no,menubar=no status=no,toolbar=no');
            }


        }
        function getUrlVars() {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        }
    </script>
       <script type="text/javascript">
           //頁簽切換
           $(function () {

               // 預設顯示第一個 Tab
               var _showTab = 0;
               $('.abgne_tab').each(function () {

                   // 目前的頁籤區塊
                   var $tab = $(this);

                   var $defaultLi = $('ul.tabs li', $tab).eq(_showTab).addClass('active');
                   $($defaultLi.find('a').attr('href')).siblings().hide();

                   // 當 li 頁籤被點擊時...
                   // 若要改成滑鼠移到 li 頁籤就切換時, 把 click 改成 mouseover
                   $('ul.tabs li', $tab).click(function () {
                       // 找出 li 中的超連結 href(#id)
                       var $this = $(this),
                           _clickTab = $this.find('a').attr('href');
                       // 把目前點擊到的 li 頁籤加上 .active
                       // 並把兄弟元素中有 .active 的都移除 class
                       $this.addClass('active').siblings('.active').removeClass('active');
                       // 淡入相對應的內容並隱藏兄弟元素
                       $(_clickTab).stop(false, true).fadeIn().siblings().hide();

                       return false;
                   }).find('a').focus(function () {
                       this.blur();
                   });
               });
           });
       </script>
</asp:Content>



