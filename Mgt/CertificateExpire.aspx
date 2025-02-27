<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="CertificateExpire.aspx.cs" Async="true" Inherits="Mgt_CertificateExpire" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link id="themecss" rel="stylesheet" type="text/css" href="//www.shieldui.com/shared/components/latest/css/light/all.min.css" />
    <script type="text/javascript" src="//www.shieldui.com/shared/components/latest/js/shieldui-all.min.js"></script>

    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>
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


    <style type="text/css">
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
    <div class="path txtS mb20">現在位置：<a href="#">繼續教育管理</a> <i class="fa fa-angle-right"></i><a href="CertificateExpire.aspx">繼續教育證書審核</a></div>

    <div class="right">
        <input type="button" value="批次寄送通知" onclick="showMSG()" />
        <asp:Button ID="btnExport" runat="server" Text="匯出" OnClick="btnExport_Click" />

    </div>
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <fieldset>
                <legend>單一查詢</legend>
                姓名<asp:TextBox ID="txt_PName" runat="server"></asp:TextBox><br />
                身分證<asp:TextBox ID="txt_PersonID" runat="server"></asp:TextBox><br />
                課程規劃名稱<asp:TextBox ID="txt_EventName" runat="server"></asp:TextBox><br />
                證書名稱
                <asp:DropDownList ID="ddl_CType" class="required" runat="server" DataValueField="CTypeSNO" DataTextField="CTypeName" />
                <div class="right">
                    <input name="btnSearch" type="button" value="查詢" runat="server" onserverclick="btnSearch_Click" />
                </div>
            </fieldset>
            <fieldset>
                <legend>批次上傳查詢</legend>
                <div class="left w8">
                    Excel檔案:       
                <asp:FileUpload ID="file_Upload" runat="server" />
                </div>
                <div class="right">
                    <asp:Button ID="btnDownload" runat="server" Text="下載格式" OnClick="btnDownload_Click" />
                    <asp:Button ID="btnUpload" runat="server" Text="上傳&查詢" OnClick="btnUpload_Click" />
                </div>
            </fieldset>
            <div class="right">
                <asp:Button ID="btnGrant" runat="server" Text="批次授予" OnClick="btnGrant_Click" />
            </div>
        </fieldset>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lb_PerrsonID" runat="server" Visible="false"></asp:Label>
            <fieldset id="ProblemList" runat="server">
                <legend>問題名單</legend>
                <asp:GridView ID="gv_NotInExcel" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="PersonID" HeaderText="身分證" />
                        <asp:BoundField DataField="stutas" HeaderText="狀態" />
                    </Columns>
                </asp:GridView>
            </fieldset>
            <fieldset id="NoproblenList" runat="server">
                <legend>正確名單</legend>
                <asp:GridView ID="gv_Event" runat="server" AutoGenerateColumns="False" OnRowCommand="gv_Event_RowCommand" OnRowCreated="gv_Event_RowCreated">
                    <Columns>
                        <asp:TemplateField HeaderText="*" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Wrap="False" />
                            <ItemStyle Wrap="False" />
                            <HeaderTemplate>
                                <asp:CheckBox ID="CheckAllItem" runat="server" onclick="javascript: CheckAllItem(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox" AutoCallBack="true" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Sort" HeaderText="序號" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PersonSNO" HeaderText="PersonSNO" />
                        <asp:BoundField DataField="EPClassSNO" HeaderText="EPClassSNO" />
                        <asp:BoundField DataField="CTypeSNO" HeaderText="CTypeSNO" />
                        <asp:TemplateField HeaderText="姓名" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <a href="#" style="color: blue" onclick="javascript:window.open('ReportMemberDetail.aspx?sno=<%# Eval("PersonSNO") %>','','width=1000,height=500');"><%# Eval("PName") %></a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PersonID" HeaderText="身分證" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="繼續教育課程規劃名稱">
                            <ItemTemplate>
                                <%# Eval("PlanName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="總積分" DataField="TotalIntegrals" />
                        <asp:BoundField DataField="ElearningIntegral" HeaderText="Elearning學分" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                        <asp:BoundField DataField="線上學分" HeaderText="學員線上學分" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                        <asp:BoundField DataField="實體學分" HeaderText="學員實體學分" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                        <asp:BoundField DataField="實習學分" HeaderText="學員實習學分" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                        <asp:BoundField DataField="通訊學分" HeaderText="學員通訊學分" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                        <asp:BoundField DataField="CTypeName" HeaderText="目標證書" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <asp:Panel ID="page_Panel" runat="server">
            <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
            <asp:HiddenField ID="txt_Page" runat="server" />
            <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
                </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Label ID="lb_NoOne" runat="server" ForeColor="Red"></asp:Label>
    <div id="c_content" class="ContentDiv">

        <div style="background-color: white; opacity: 0.95; padding: 20px 30px; border-radius: 5px; margin: 100px auto 0 auto; z-index: 999; width: 650px; height: 600px;">

            <a onclick="hideMSG()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

            <h1><i class="fa fa-at" aria-hidden="true"></i>寄送通知</h1>
            <div class="both mb20" style="text-align: left">
                <fieldset>
                    <legend>備註</legend>
                    筆數一共：<asp:Label ID="count" runat="server"></asp:Label><br />
                    <span style="color: red; font-weight: bold">此功能為查詢出來之結果批次寄送</span>
                </fieldset>
            </div>
            <div id="tabs" style="height: 390px; margin-bottom: 20px; margin-top: 10px;">
                <ul>
                    <li><a href="#tabs-1">寄送Email通知</a></li>
                    <%--Role_view判斷是否為資拓人員或國建署人員--%>
                    <li id="Role_view" runat="server" visible="false"><a href="#tabs-2">寄送簡訊通知</a></li>
                    <li><a href="#tabs-3">寄送站內通知</a></li>
                </ul>
                <div id="tabs-1">
                    <div></div>
                    <textarea id="editor_Mail" runat="server"></textarea>
                    <div class="MarginTop">
                        <asp:Button ID="btnSendMail" runat="server" Text="寄送信件" OnClick="btnSendMail_Click" />
                    </div>
                </div>
                <%--Tabs_2是用來包tabs-2，判斷是否為資拓人員或國建署人員--%>
                <div id="Tabs_2" runat="server" visible="false">
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
                </div>
            </div>


        </div>
    </div>


    <script type="text/javascript">
        function showMSG() {
            $("#c_content").show("fade");
        }

        function hideMSG() {
            $("#c_content").hide("fade");
        }

    </script>
    <script type="text/javascript">	
        function CheckAllItem(Check) {
            elm = document.forms[0];  //取得form表單
            for (i = 0; i < elm.length; i++) {
                if (elm[i].type == "checkbox" && elm[i].id != Check.id) //若為checkbox，並且ID與表頭CheckBox不同。表示為明細的CheckBox
                {
                    if (elm.elements[i].checked != Check.checked)  //若明細的CheckBox的checked狀態與表頭CheckBox不同
                    {
                        elm.elements[i].click();  //明細的CheckBox執行click        
                    }
                }
            }
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

