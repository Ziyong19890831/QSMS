<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="UploadAudit_AE.aspx.cs" Inherits="Mgt_UploadAudit_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link id="themecss" rel="stylesheet" type="text/css" href="//www.shieldui.com/shared/components/latest/css/light/all.min.css" />
    <script type="text/javascript" src="//www.shieldui.com/shared/components/latest/js/shieldui-all.min.js"></script>
    <style>
        .dis{
            pointer-events:none;
        }
    </style>


    <script type="text/javascript">
        //修正寄送通知版面跑掉，原因是新版Jquery移除jQuery.browser()方法
        jQuery.browser = {};
        (function () {
            jQuery.browser.msie = false;
            jQuery.browser.version = 0;
            if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
                jQuery.browser.msie = true;
                jQuery.browser.version = RegExp.$1;
            }
        })();

        $(function () {
            $("#tabs").tabs();
        });

        jQuery(function ($) {

            $("#ContentPlaceHolder1_editor_Mail").shieldEditor({
                commands: [
                    "formatBlock", "removeFormat",
                    "fontName", "fontSize", "foreColor", "backColor",
                    "bold", "italic", "underline", "strikeThrough", "subscript", "superscript",
                    "justifyLeft", "justifyCenter", "justifyRight", "justifyFull",
                    "insertUnorderedList", "insertOrderedList",
                    "indent", "outdent",
                    "createLink", "unlink", "insertImage",
                    "viewHtml"
                ]
            });
            $("#ContentPlaceHolder1_editor_Sys").shieldEditor({
                commands: [
                    "formatBlock", "removeFormat",
                    "fontName", "fontSize", "foreColor", "backColor",
                    "bold", "italic", "underline", "strikeThrough", "subscript", "superscript",
                    "justifyLeft", "justifyCenter", "justifyRight", "justifyFull",
                    "insertUnorderedList", "insertOrderedList",
                    "indent", "outdent",
                    "createLink", "unlink", "insertImage",
                    "viewHtml"
                ]
            });
        });

        function sendTODO() {
            var person = $("#ContentPlaceHolder1_Label1").text();
            var content = $("#editor").swidget().value();
            var system = $("#ContentPlaceHolder1_ddl_SystemName").val();
            var name = $("#todoName").val();
            var saJson = JSON.stringify({ person: person, content: content, system: system, name: name });


            if (name == "" || content == "" || system == "") {
                alert("資料輸入不完全!")
            }
            else {

                $.ajax({
                    type: "POST",
                    async: false,
                    dataType: "json",
                    url: "FormPaperAjax.aspx/insertTODO",
                    contentType: 'application/json; charset=UTF-8',
                    data: saJson,
                    success: function (msg) {
                        alert("發送成功");
                        location.reload();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr.responseText);
                    }
                });
            }


        }

    </script>

    <script type="text/javascript">

        $(function () {

            //td裡的radiobutton勾選event
            var Aselect = $("#ContentPlaceHolder1_gv_EventD_ddl_AuditH");
            $(Aselect).change(function () {
                //alert(Aselect.val());
                $("td select").val(Aselect.val());
                AdmitResult()
                //alert(2);
            });

            //$("td select").change(function () {
            //    switch ($(this).val()) {
            //        case "0": $(this).css("color", ""); break;
            //        case "1": $(this).css("color", "blue"); break;
            //        case "2": $(this).css("color", "red"); break;
            //        default: break;
            //    }
            //});
            AdmitResult()

        });

        function AdmitResult() {

            var unAdmit = 0;
            var admitted = 0;
            var waiting = 0;
            var Chack = 0;
            var Ready = 0;
            var Cancel = 0;

            $("td select").each(function () {
                var selectItem = $("#" + this.id);
                switch (selectItem.val()) {
                    case "0":
                        selectItem.css("color", "");
                        unAdmit = unAdmit + 1;

                        break;
                    case "1":
                        selectItem.css("color", "blue");
                        admitted = admitted + 1;

                        break;
                    case "2":
                        selectItem.css("color", "red");
                        waiting = waiting + 1;
                        break;
                    case "3":
                        selectItem.css("color", "GoldenRod");
                        Chack = Chack + 1;
                        break;
                    case "4":
                        selectItem.css("color", "green");
                        Ready = Ready + 1;
                        break;
                    case "5":
                        selectItem.css("color", "SaddleBrown");
                        Cancel = Cancel + 1;
                        break;
                    default: break;
                }
            });
            $("#ContentPlaceHolder1_lbl_UnAdmit").text(unAdmit);
            $("#ContentPlaceHolder1_lbl_Admitted").text(admitted);
            $("#ContentPlaceHolder1_lbl_Waiting").text(waiting);
            $("#ContentPlaceHolder1_lbl_Chack").text(Chack);
            $("#ContentPlaceHolder1_lbl_Ready").text(Ready);
            $("#ContentPlaceHolder1_lbl_cancel").text(Cancel);
        }

        function CheckData() {
            var r = confirm("確定要審核[" + $("#ContentPlaceHolder1_lbl_EventName").text() + "]的核可名單？");
            if (r == true) {
                var result = parseInt($("#ContentPlaceHolder1_lbl_UnAdmit").text());
                if (result > 0) {
                    alert('尚有未審核名單');
                    event.preventDefault();
                }
            }
            else {
                event.preventDefault();
            }

        }

    </script>



    <style>
        .ui-tabs .ui-tabs-nav li {
            padding: 0 5px;
        }

        h1 {
            font-family: 'Microsoft JhengHei';
        }

        .MarginTop {
            margin-top: 10px;
        }

        .tab_content {
            padding: 20px 0;
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
            text-align: center;
            line-height: 25px;
            display: none;
        }

        FIELDSET {
            margin: 5px;
            border: 1px solid silver;
            padding: 8px;
            border-radius: 4px;
        }

        LEGEND {
            padding: 2px;
        }
    </style>


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="both mb20">
        現在位置：<a href="#">課程管理</a> <i class="fa fa-angle-right"></i><a href="#">作業審核-審核</a>
        <div class="right">
            <%--<input type="button" value="寄送通知" onclick="showMSG()" />--%>
            <asp:Button ID="btn_Back" Text="返回作業審核" OnClick="btn_Back_Click" runat="server" />
        </div>
    </div>

    <fieldset>
        <legend>審核作業</legend>
        <div class="path txtS mb20">
            審核中：<asp:Label ID="lbl_UnAdmit" runat="server" Text="0"></asp:Label>次
           ,通過：<asp:Label ID="lbl_Admitted" runat="server" Text="0"></asp:Label>次
           ,不通過：<asp:Label ID="lbl_Waiting" runat="server" Text="0"></asp:Label>次

              <%--<div class="right mb20">
                  <input name="btnSend" type="submit" runat="server" value="送出審核名單" onclick="CheckData();" onserverclick="btnOK_Click" />
              </div>--%>
        </div>
    </fieldset>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">

        <ContentTemplate>
            <asp:GridView ID="gv_EventD" runat="server" AutoGenerateColumns="False" Font-Size="14px" OnRowCommand="gv_EventD_RowCommand" OnRowDataBound="gv_EventD_RowDataBound" OnRowCreated="gv_EventD_RowCreated">
                <Columns>

                    <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-CssClass="center">
                        <ItemStyle CssClass="center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="ULSNO" DataField="ULSNO" />
                    <asp:BoundField HeaderText="姓名" DataField="PName" />
                    <asp:BoundField HeaderText="身分證號" DataField="PersonID" />
                    <asp:BoundField HeaderText="Email" DataField="PMail" />
                    <asp:BoundField HeaderText="課程代號" DataField="CourseSNO" />
                    <asp:BoundField HeaderText="課程名稱" DataField="CourseName" />
                    <asp:BoundField HeaderText="上傳日期" DataField="CreateDT" />
                    <asp:BoundField HeaderText="URL" DataField="URL" />
                    <asp:TemplateField HeaderText="審核狀態" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddl_AuditItem" onchange="AdmitResult();" SelectedValue='<%# Bind("Audit") %>' runat="server" autopostback="false">
                                <asp:ListItem Value="0" Text="審核中" />
                                <asp:ListItem Value="1" Text="通過" />
                                <asp:ListItem Value="2" Text="不通過" />
                            </asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="通知狀態" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <%# (Eval("Notice").ToString() == "True") ? "已通知" : "未通知"%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="備註" DataField="Note" />
                    <asp:TemplateField HeaderText="檔案" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <a href="<%# Eval("URL") %>" target="_blank">下載</a>
                            <%--<asp:Button ID="Btn_Downloads" runat="server" Text="下載檔案" OnClick="Lk_Downloads_Click" CommandName="Downloads"></asp:Button>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="審核結果" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Button ID="Btn_Send" runat="server" Text='送出' OnClick="Btn_Send_Click" OnClientClick="showMSG()" CommandName="result" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="PersonSNO" DataField="PersonSNO" />
                     <asp:TemplateField HeaderText="寄件內容" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Button ID="Btn_Content" runat="server" Text='查看' OnClick="Btn_Send_Click" CommandName="ReadMail" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <asp:Label ID="lb_Mail" runat="server" Visible="false"></asp:Label>

    <!--只有按下按鈕時才會出現-->

    <div id="c_content" class="ContentDiv">

        <div style="background-color: white; opacity: 0.95; padding: 20px 30px; border-radius: 5px; margin: 100px auto 0 auto; z-index: 999; width: 700px; height: 550px;">

            <a onclick="hideMSG()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

            <h1><i class="fa fa-at" aria-hidden="true"></i>寄送通知</h1>
            <asp:Label ID="lb_ULSNO" runat="server"></asp:Label>
            <%--<input type="button" value="取消" onclick="hideLogin()" style="float: right;" />--%>
            <div id="tabs" style="height: 390px; margin-bottom: 20px; margin-top: 10px;">
                <ul>
                    <li><a href="#tabs-1">寄送Email通知</a></li>
                    <%--Role_view判斷是否為資拓人員或國建署人員--%>
                  <%--  <li id="Role_view" runat="server" visible="false"><a href="#tabs-2">寄送簡訊通知</a></li>
                    <li><a href="#tabs-3">寄送站內通知</a></li>--%>
                </ul>
                <div id="tabs-1">
                    <div></div>
                    <textarea id="editor_Mail" runat="server"></textarea>
                    <div class="MarginTop">
                        <asp:Button ID="btnSendMail" runat="server" Text="寄送信件" OnClick="btnSendMail_Click" />
                    </div>
                </div>
                <%--Tabs_2是用來包tabs-2，判斷是否為資拓人員或國建署人員--%>
               <%-- <div id="Tabs_2" runat="server" visible="false">
                    <div id="tabs-2">
                        <div>
                            <asp:TextBox ID="txt_SMS" runat="server" type="text" TextMode="MultiLine" Width="100%" Height="100px" Text=""></asp:TextBox>
                        </div>
                        <div class="MarginTop">
                            <asp:Button ID="btnSendSMS" runat="server" Text="寄送簡訊" OnClick="btnSendSMS_Click" />
                        </div>
                    </div>
                </div>
                <div id="tabs-3">
                    <textarea id="editor_Sys" runat="server"></textarea>
                    <div class="MarginTop">
                        <asp:Button ID="btnSendOwnsite" runat="server" Text="寄送站內訊息" OnClick="btnSendOwnsite_click" />
                    </div>
                </div>--%>
            </div>
        </div>
    </div>



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
            var winvar = window.open('./EventManager.aspx?sno=' + qstring, 'winname', 'width=1200 height=550 location=no,menubar=no status=no,toolbar=no');

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

