<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="EventAudit.aspx.cs" Inherits="Mgt_Event_AE" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--        <link href="../JS/shieldui/all.min.css" rel="stylesheet" />
    <script src="../JS/shieldui/jquery-1.11.1.min.js"></script>
    <script src="../JS/shieldui/shieldui-all.min.js"></script>--%>

    <link id="themecss" rel="stylesheet" type="text/css" href="//www.shieldui.com/shared/components/latest/css/light/all.min.css" />
    <%--<script type="text/javascript" src="//www.shieldui.com/shared/components/latest/js/jquery-1.11.1.min.js"></script>--%>
    <%--<script type="text/javascript" src="//www.shieldui.com/shared/components/latest/js/shieldui-all.min.js"></script>--%>



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
        $(function () {
            $("#tabout").tabs();
        });

        //jQuery(function ($) {

        //    $("#ContentPlaceHolder1_editor_Mail").shieldEditor({
        //        commands: [
        //            "formatBlock", "removeFormat",
        //            "fontName", "fontSize", "foreColor", "backColor",
        //            "bold", "italic", "underline", "strikeThrough", "subscript", "superscript",
        //            "justifyLeft", "justifyCenter", "justifyRight", "justifyFull",
        //            "insertUnorderedList", "insertOrderedList",
        //            "indent", "outdent",
        //            "createLink", "unlink", "insertImage",
        //            "viewHtml"
        //        ]
        //    });
        //    $("#ContentPlaceHolder1_editor_Sys").shieldEditor({
        //        commands: [
        //            "formatBlock", "removeFormat",
        //            "fontName", "fontSize", "foreColor", "backColor",
        //            "bold", "italic", "underline", "strikeThrough", "subscript", "superscript",
        //            "justifyLeft", "justifyCenter", "justifyRight", "justifyFull",
        //            "insertUnorderedList", "insertOrderedList",
        //            "indent", "outdent",
        //            "createLink", "unlink", "insertImage",
        //            "viewHtml"
        //        ]
        //    });
        //});

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
                window.location='./Notice.aspx';
            }


        }

    </script>

    <script type="text/javascript">

        $(function () {

            //td裡的radiobutton勾選event
            var Aselect = $("#ContentPlaceHolder1_gv_EventD_ddl_AuditH");
            $(Aselect).change(function () {
                if (Aselect.val() == "--審核狀態--" || Aselect.val() == "--通過狀態--") {
                    alert("不得選取此狀態");
                    event.preventDefault();
                }
                else {
                    $("td select").val(Aselect.val());
                    AdmitResult()
                }


            });

        });
        //$(function () {

        //    //td裡的radiobutton勾選event
        //    var Aselect = $(".ddlItem");
        //    $(Aselect).change(function () {
        //        if (Aselect.val() == "--審核狀態--" || Aselect.val() == "--通過狀態--") {
        //            alert("不得選取此狀態");
        //            event.preventDefault();
        //        }
        //        else {
        //            $("td select").val(Aselect.val());
        //            AdmitResult()
        //        }


        //    });

        //});

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
            else
            {
                event.preventDefault();
            }

        }

    </script>


    <style type="text/css">
        .ui-tabs .ui-tabout .ui-tabs-nav li {
            padding: 0 5px;
        }

        h1 {
            font-family: 'Microsoft JhengHei';
        }

        .MarginTop {
            margin-top: 10px;
        }
        .MarginBottom {
            margin-bottom: 10px;
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
            overflow-y: auto;
        }
        .PreviewDiv{
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
            line-height: 25px;
            display: none;
            overflow-y: auto;
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
        img {
            width: 400px;
        }
    </style>


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
   
    <div class="path txtS mb20"><asp:Label ID="lb_Map1" runat="server" Text="現在位置：新訓課程管理"></asp:Label><i class="fa fa-angle-right"></i><asp:Label ID="lb_Map2" runat="server" Text="新訓課程報名管理"></asp:Label><i class="fa fa-angle-right"></i><a style="color:#027f3f" href="#">審核</a>
        
        <div class="right">
            <input type="button" value="寄送通知" onclick="showMSG()" />
            <input type="button" value="協助報名" onclick="getParameterByName('sno')" runat="server" id="btn_invite" />
            <%--<asp:Button ID="btn_invite" runat="server" OnClientClick="getParameterByName('sno')" OnClick="btn_invite_Click" Text="協助報名"/>--%>
            <asp:Button ID="Invite_download" runat="server" Text="報名下載" type="button" OnClick="Invite_download_Click" />
            <input type="button" value="名單管理" onclick="Manager('sno')" />
            <asp:Button ID="btn_Back" Text="返回報名管理" OnClick="btn_Back_Click" runat="server" />

        </div>
    </div>

    <fieldset>
        <legend>活動報名資料</legend>
        <div class="path txtS mb20">
            目前活動：<asp:Label ID="lbl_EventName" runat="server" Text="Label"></asp:Label>
            <br />
            報名上限：<asp:Label ID="lbl_Limit" runat="server" Text="0"></asp:Label>人
           ,預計錄取：<asp:Label ID="lbl_Admit" runat="server" Text="0"></asp:Label>人
           ,已報名：<asp:Label ID="lbl_AdmitCtn" runat="server" Text="0"></asp:Label>人
            <div class="right mb20">
                管理員您好，請先審核錄取及備取名單後，再進行訊息派送。
            </div>
        </div>
    </fieldset>
    <fieldset>
        <legend>審核作業</legend>
        <div class="path txtS mb20">
            未審：<asp:Label ID="lbl_UnAdmit" runat="server" Text="0"></asp:Label>人
           ,錄取：<asp:Label ID="lbl_Admitted" runat="server" Text="0"></asp:Label>人
           ,未錄取：<asp:Label ID="lbl_Waiting" runat="server" Text="0"></asp:Label>人
           ,審核中：<asp:Label ID="lbl_Chack" runat="server" Text="0"></asp:Label>人
           ,備取：<asp:Label ID="lbl_Ready" runat="server" Text="0"></asp:Label>人
           ,取消：<asp:Label ID="lbl_cancel" runat="server" Text="0"></asp:Label>人
              <div class="right mb20">
                  <input name="btnSend" type="submit" runat="server" value="送出審核名單" onclick="CheckData();" onserverclick="btnOK_Click" />
              </div>
        </div>
    </fieldset>
    <fieldset>
        <legend>功能列</legend>
        <div class="path txtS mb20">
            姓名<asp:TextBox ID="txt_Name" runat="server"></asp:TextBox>
            醫事證號<asp:TextBox ID="txt_JSN" runat="server"></asp:TextBox>
            手機<asp:TextBox ID="txt_Phone" runat="server"></asp:TextBox><br />
            信箱<asp:TextBox ID="txt_Mail" runat="server"></asp:TextBox>
            身分證號<asp:TextBox ID="txt_personID" runat="server"></asp:TextBox>
            審查狀態<asp:DropDownList ID="ddl_Status" runat="server" DataTextField="Mval" DataValueField="PVal"></asp:DropDownList>
            報名類別<asp:DropDownList ID="ddl_EventInvite" runat="server" DataTextField="Mval" DataValueField="PVal">  </asp:DropDownList>
            <div class="right mb20">
                <%--<input name="btnSearch" type="submit" runat="server" value="搜尋" onclick="CheckData();" onserverclick="Unnamed_ServerClick" />--%>
                <asp:Button ID="Btn_Search" Text="搜尋" runat="server" OnClick="Btn_Search_Click" />
            </div>
        </div>
    </fieldset>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gv_EventD" runat="server" AutoGenerateColumns="False" Font-Size="14px" OnRowCommand="gv_EventD_RowCommand" OnRowDataBound="gv_EventD_RowDataBound" OnRowCreated="gv_EventD_RowCreated">
                <Columns>
                    <asp:BoundField DataField="CompletionClass1" HeaderText="CompletionClass1" />
                     <asp:BoundField DataField="CompletionCertificateType" HeaderText="CompletionCertificateType" />
                    <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-CssClass="center">
                        <ItemStyle CssClass="center"></ItemStyle>
                    </asp:BoundField>
                    <%--<asp:BoundField HeaderText="學員名稱" DataField="PName" HtmlEncode="true"/>--%>
                    <%--            <asp:HyperLinkField DataTextField="PName" DataNavigateUrlFields="PersonSNO"  DataNavigateUrlFormatString="~/IndesignPurchase/detailsview.aspx?Vendor_id={0}"
            HeaderText="學員名稱" ItemStyle-Width = "150" Target="_parent" NavigateUrl="window.open('~/IndesignPurchase/detailsview.aspx');" />--%>
                    <asp:TemplateField HeaderText="姓名" SortExpression="CompanyName">
                        <ItemTemplate>
                            <a href="#" style="color: blue" onclick="javascript:window.open('ReportMemberDetail.aspx?sno=<%# Eval("PersonSNO") %>','','width=1000,height=500');"><%# Eval("PName") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="身分證號" DataField="PersonID" />
                    <asp:BoundField HeaderText="醫事證號" DataField="JCN" />
                    <asp:BoundField HeaderText="角色" DataField="RoleName" />
                    <asp:BoundField HeaderText="手機" DataField="PPhone" />
                    <asp:BoundField HeaderText="電話" DataField="PTel" />
                     <asp:BoundField HeaderText="報名類別" DataField="EventInvite" />
                    <asp:BoundField HeaderText="信箱" DataField="PMail" />
                    <%--<asp:BoundField HeaderText="通知方式" DataField="EventNotice" />--%>
                    <asp:BoundField HeaderText="膳食" DataField="Meals" />
                    <asp:BoundField HeaderText="報名時間" DataField="ApplyDT" />
                    <asp:TemplateField HeaderText="審核狀態" ItemStyle-HorizontalAlign="center">
                        <HeaderTemplate>
                            <asp:DropDownList ID="ddl_AuditH" runat="server">
                                <asp:ListItem  Text="--審核狀態--" />
                                <asp:ListItem Value="0" Text="未審" />
                                <asp:ListItem Value="1" Text="錄取" />
                                <asp:ListItem Value="2" Text="未錄取" />
                                <asp:ListItem Value="3" Text="審核中" />
                                <asp:ListItem Value="4" Text="備取" />
                                <asp:ListItem Value="5" Text="取消" />
                                <asp:ListItem Text="--通過狀態--"/>
                                <asp:ListItem Value="6" Text="未通過" />
                                <asp:ListItem Value="7" Text="通過" />
                            </asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DropDownList ID="ddl_AuditItem" onchange="AdmitResult();" Class="ddlItem" SelectedValue='<%# Bind("EventAuditVal") %>' runat="server">
                                <asp:ListItem  Text="--審核狀態--" />
                                <asp:ListItem Value="0" Text="未審" />
                                <asp:ListItem Value="1" Text="錄取" />
                                <asp:ListItem Value="2" Text="未錄取" />
                                <asp:ListItem Value="3" Text="審核中" />
                                <asp:ListItem Value="4" Text="備取" />
                                <asp:ListItem Value="5" Text="取消" />
                                <asp:ListItem Text="--通過狀態--" />
                                <asp:ListItem Value="6" Text="未通過" />
                                <asp:ListItem Value="7" Text="通過" />
                            </asp:DropDownList>
                        </ItemTemplate>

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="通知狀態" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <%# (Eval("Notice").ToString() == "True") ? "已通知" : "未通知"%>
                            <asp:HiddenField ID="hid_EventDid" Value='<%# Bind("EventDSNO") %>' runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="檔案"  ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <%# Eval("DLOADNAME") %>
                            <div style="padding-left: 20px;"><%# getFiles(Eval("DLOADURL").ToString()) %></div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="備註" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_Note" runat="server" OnTextChanged="txt_Note_TextChanged" Text='<%# Eval("Note") %>'></asp:TextBox>
                            <asp:Button ID="Btn_Note" runat="server" Text='儲存'  CommandName="getdata" /><asp:Label ID="lb_msg" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="簽退">
                        <ItemTemplate>
                            <%--<asp:Button ID="Btn_Out" runat="server" Text='簽退' OnClick="Btn_Out_Click" CommandName="out"   />--%>
<%--                            <input type="button" value="簽退" onclick="showOUT()" />--%>
                              <asp:Button ID="gvAdd" runat="server" Text='簽退' onclick='<%# "javascript:return showOUT(" + Eval("EventDSNO")+","+ Eval("PersonSNO") +","+ Eval("EventAuditVal") +");" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="姓名" DataField="PName" />
                    <asp:BoundField HeaderText="EventDSNO" DataField="EventDSNO" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>


    <!--只有按下按鈕時才會出現-->
    <div id="c_content" class="ContentDiv">

        <div style="background-color: white; opacity: 0.95; padding: 20px 30px; border-radius: 5px; margin: 100px auto 0 auto; z-index: 999; width: 700px; height: 550px;">

            <a onclick="hideMSG()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

            <h1><i class="fa fa-at" aria-hidden="true"></i>寄送通知</h1>
            <%--<input type="button" value="取消" onclick="hideLogin()" style="float: right;" />--%>
            <p style="margin-left: 10px">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        傳送對象：
                        <asp:DropDownList ID="Send_List1" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="Send_List1_SelectedIndexChanged">
                            <%--Value設定是參照資料庫config --%>
                            <asp:ListItem Value="0" Text="請選擇"></asp:ListItem>
                            <asp:ListItem Value="2" Text="未錄取"></asp:ListItem>
                            <asp:ListItem Value="1" Text="錄取"></asp:ListItem>
                            <asp:ListItem Value="4" Text="備取"></asp:ListItem>
                            <asp:ListItem Value="3" Text="單筆寄送"></asp:ListItem>
                        </asp:DropDownList>
                        報名人員：<asp:DropDownList ID="Join_list" runat="server" Width="100px" Enabled="false"></asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <div id="tabs" style="height: 390px; margin-bottom: 20px; margin-top: 10px;">
                <ul>
                    <li><a href="#tabs-1">寄送Email通知</a></li>
                    <%--Role_view判斷是否為資拓人員或國建署人員--%>
                    <li id="Role_view" runat="server" visible="false"><a href="#tabs-2">寄送簡訊通知</a></li>
                    <li><a href="#tabs-3">寄送站內通知</a></li>
                </ul>
                <div id="tabs-1">
                    <div><textarea cols="20" rows="20" id="editor_Mail" runat="server"></textarea></div>
                    <div class="MarginTop">
                        <asp:Button ID="btnSendMail" runat="server" Text="寄送信件" OnClick="btnSendMail_ClickAsync" />
                        <input name="btnPreview" type="button" value="預覽寄送" onclick="showMail()" />
                    </div>
                    <div> <span style="color:red">寄送Email通知為排程寄送，非立即寄送，每天排程09:00/13:00/17:00寄送</span></div>                           
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
                    <textarea  cols="20" rows="20"  id="editor_Sys" runat="server"></textarea>
                    <div class="MarginTop">
                        <asp:Button ID="btnSendOwnsite" runat="server" Text="寄送站內訊息" OnClick="btnSendOwnsite_click" />
                    </div>
                </div>

            </div>


        </div>
    </div>
    <!--只有按下按鈕時才會出現-->
    <!--只有按下按鈕時才會出現-->
    <div id="c_out" class="ContentDiv">

        <div style="background-color: white; opacity: 0.95; padding: 20px 30px; border-radius: 5px; margin: 100px auto 0 auto; z-index: 999; width: 700px; height: 550px;">

            <a onclick="hideOUT()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

            <h1><i class="fa fa-at" aria-hidden="true"></i>簽退通知</h1>
            <%--<input type="button" value="取消" onclick="hideLogin()" style="float: right;" />--%>
            <p style="margin-left: 10px">
               
            </p>
            <div id="tabout" style="height: 390px; margin-bottom: 20px; margin-top: 10px;">
                <ul>
                    <li><a href="#tabout-1">寄送Email通知</a></li>
                    <%--Role_view判斷是否為資拓人員或國建署人員--%>
                    <li id="Li1" runat="server" visible="false"><a href="#tabout-2">寄送簡訊通知</a></li>
                </ul>
                <div id="tabout-1">
                    <div></div>
                    <textarea  cols="20" rows="20"  id="editor_Out" runat="server"></textarea>
                    <div class="MarginTop">
                        <%--<asp:Button ID="Button1" runat="server" Text="寄送信件" OnClick="btnSendMail_ClickAsync" />--%>
                        <asp:HiddenField ID="hideventDsno"  runat="server" />
                        <asp:HiddenField ID="hidpersonsno"  runat="server" />
                        <asp:HiddenField ID="hidemail"  runat="server" />
                        <asp:Button ID="Btn_Out" runat="server" Text='寄送信件' OnClick="Btn_Out_Click" />
                        <input name="btnPreview" type="button" value="取消寄送" onclick="hideOUT()" />
                    </div>
                </div>
            </div>

        </div>
    </div>
    <!--只有按下按鈕時才會出現-->
    <!--只有按下預覽時才會出現-->
    <div id="Preview" class="PreviewDiv">

        <div style="background-color: white; opacity: 0.95; padding: 20px 30px; border-radius: 5px; margin: 100px auto 0 auto; z-index: 999; width: 700px; height: 550px;">

            <a onclick="hidePreview()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

            <h1 style="text-align:center"><i class="fa fa-at" aria-hidden="true"></i>寄送通知預覽</h1>
            <div id="PreviewContent" ></div>

<%--            <div class="MarginBottom">
                <asp:Button ID="Button1" runat="server" Text="寄送信件" OnClick="btnSendMail_ClickAsync" />
            </div>--%>

        </div>
    </div>
    <!--只有按下預覽時才會出現-->


    <div id="Note_content" class="ContentDiv">

        <div style="background-color: white; opacity: 0.95; padding: 20px 30px; border-radius: 5px; margin: 100px auto 0 auto; z-index: 999; width: 700px; height: 350px;">
            <a onclick="hideNote()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>
            <h1><i class="fa fa-at" aria-hidden="true"></i>審核備註</h1>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <asp:TextBox ID="txt_Note" runat="server" TextMode="MultiLine" Width="640px" Height="215px"></asp:TextBox>
                    <asp:Button ID="btn_NoteSave" runat="server" Text="儲存" Class="center" />
                </ContentTemplate>
            </asp:UpdatePanel>
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
        function showOUT(Eventsno, Personsno, EventAuditVal) {
            //alert(EventAuditVal);
            if (EventAuditVal==2) {
                $("#ContentPlaceHolder1_hideventDsno").val(Eventsno);
                $("#ContentPlaceHolder1_hidpersonsno").val(Personsno);
                $("#c_out").show("fade");
            } else {
                alert("審核狀態為「未錄取」且有送出審核名單儲存後，才可簽退!!!")
            }
        }

        function hideOUT() {
            $("#c_out").hide("fade");
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

                var winvar = window.open('./EventManager.aspx?sno=' + qstring+'&twenty=1', 'winname', 'width=1200 height=550 location=no,menubar=no status=no,toolbar=no');
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
        function showMail() {
            var Mail = $('.ck-editor__main>div').html();
            Mail = Mail.replace(/<svg xmlns="http:\/\/www.w3.org\/2000\/svg" viewBox="0 0 10 8"><path d="M9.055.263v3.972h-6.77M1 4.216l2-2.038m-2 2 2 2.038"><\/path><\/svg>/g, '');
            Mail = Mail.replace(/<path fill-opacity=".254" d="M1 1h14v14H1z"><\/path>/g, '');
            Mail = Mail.replace(/<path fill-opacity=".256" d="M1 1h14v14H1z"><\/path>/g, '');
            Mail = Mail.replace(/<svg class="ck ck-icon" viewBox="0 0 16 16"><path d="M4 0v1H1v3H0V.5A.5.5 0 0 1 .5 0H4zm8 0h3.5a.5.5 0 0 1 .5.5V4h-1V1h-3V0zM4 16H.5a.5.5 0 0 1-.5-.5V12h1v3h3v1zm8 0v-1h3v-3h1v3.5a.5.5 0 0 1-.5.5H12z"><\/path><g class="ck-icon__selected-indicator"><path d="M7 0h2v1H7V0zM0 7h1v2H0V7zm15 0h1v2h-1V7zm-8 8h2v1H7v-1z"><\/path><\/g><\/svg>/g, '');
            Mail = Mail.replace(/<div class="ck ck-reset_all ck-widget__resizer" style="height:350px;left:0px;top:0px;width:400px;"><div class="ck-widget__resizer__handle ck-widget__resizer__handle-top-left"><\/div ><div class="ck-widget__resizer__handle ck-widget__resizer__handle-top-right"><\/div><div class="ck-widget__resizer__handle ck-widget__resizer__handle-bottom-right"><\/div><div class="ck-widget__resizer__handle ck-widget__resizer__handle-bottom-left"><\/div><div class="ck ck-size-view" style="display: none;"><\/div><\/div >/g, '');

            document.getElementById('PreviewContent').innerHTML = Mail;
            $("#Preview").show("fade");
        }
        function hidePreview() {
            $("#Preview").hide("fade");
        }
        //function CheckVsS(value) {
        //    var ss = document.getElementById('ContentPlaceHolder1_hid_vsS').value;
        //    if (value == ss && ss != '') {
        //        $('#SMSmsg').css("color", "	#019858");
        //        $('#SMSmsg').text("✔");
        //    }
        //    else {
        //        $('#SMSmsg').text("驗證碼錯誤");
        //        $('#SMSmsg').css("color", "red");
        //        errorCount += 1;
        //    }

        //}
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

         <script type="text/javascript">
             // This sample still does not showcase all CKEditor 5 features (!)
             // Visit https://ckeditor.com/docs/ckeditor5/latest/features/index.html to browse all the features.
             CKEDITOR.ClassicEditor.create(document.getElementById('<%=editor_Mail.ClientID %>'), {
                 // https://ckeditor.com/docs/ckeditor5/latest/features/toolbar/toolbar.html#extended-toolbar-configuration-format
                 toolbar: {
                     items: ['ckfinder', '|',
                         'exportPDF', 'exportWord', '|',
                         'findAndReplace', 'selectAll', '|',
                         'heading', '|',
                         'bold', 'italic', 'strikethrough', 'underline', 'code', 'subscript', 'superscript', 'removeFormat', '|',
                         'bulletedList', 'numberedList', 'todoList', '|',
                         'outdent', 'indent', '|',
                         'undo', 'redo',
                         '-',
                         'fontSize', 'fontFamily', 'fontColor', 'fontBackgroundColor', 'highlight', '|',
                         'alignment', '|',
                         'link', 'insertImage', 'blockQuote', 'insertTable', 'mediaEmbed', 'codeBlock', 'htmlEmbed', '|',
                         'specialCharacters', 'horizontalLine', 'pageBreak', '|',
                         'textPartLanguage', '|',
                         'sourceEditing'
                     ],
                     shouldNotGroupWhenFull: true
                 },
                 // Changing the language of the interface requires loading the language file using the <script> tag.
                 language: 'tw',
                 list: {
                     properties: {
                         styles: true,
                         startIndex: true,
                         reversed: true
                     }
                 },
                 // https://ckeditor.com/docs/ckeditor5/latest/features/headings.html#configuration
                 heading: {
                     options: [
                         { model: 'paragraph', title: 'Paragraph', class: 'ck-heading_paragraph' },
                         { model: 'heading1', view: 'h1', title: 'Heading 1', class: 'ck-heading_heading1' },
                         { model: 'heading2', view: 'h2', title: 'Heading 2', class: 'ck-heading_heading2' },
                         { model: 'heading3', view: 'h3', title: 'Heading 3', class: 'ck-heading_heading3' },
                         { model: 'heading4', view: 'h4', title: 'Heading 4', class: 'ck-heading_heading4' },
                         { model: 'heading5', view: 'h5', title: 'Heading 5', class: 'ck-heading_heading5' },
                         { model: 'heading6', view: 'h6', title: 'Heading 6', class: 'ck-heading_heading6' }
                     ]
                 },
                 // https://ckeditor.com/docs/ckeditor5/latest/features/editor-placeholder.html#using-the-editor-configuration
/*                 placeholder: 'Welcome to CKEditor 5!',*/
                 // https://ckeditor.com/docs/ckeditor5/latest/features/font.html#configuring-the-font-family-feature
                 fontFamily: {
                     options: [
                         'default',
                         'Arial, Helvetica, sans-serif',
                         'Courier New, Courier, monospace',
                         'Georgia, serif',
                         'Lucida Sans Unicode, Lucida Grande, sans-serif',
                         'Tahoma, Geneva, sans-serif',
                         'Times New Roman, Times, serif',
                         'Trebuchet MS, Helvetica, sans-serif',
                         'Verdana, Geneva, sans-serif',
                         'Times New Roman,times, serif' ,
                     ],
                     supportAllValues: true
                 },
                 // https://ckeditor.com/docs/ckeditor5/latest/features/font.html#configuring-the-font-size-feature
                 fontSize: {
                     options: [10, 12, 14, 'default', 18, 20, 22],
                     supportAllValues: true
                 },
                 // Be careful with the setting below. It instructs CKEditor to accept ALL HTML markup.
                 // https://ckeditor.com/docs/ckeditor5/latest/features/general-html-support.html#enabling-all-html-features
                 htmlSupport: {
                     allow: [
                         {
                             name: /.*/,
                             attributes: true,
                             classes: true,
                             styles: true
                         }
                     ]
                 },
                 // Be careful with enabling previews
                 // https://ckeditor.com/docs/ckeditor5/latest/features/html-embed.html#content-previews
                 htmlEmbed: {
                     showPreviews: true
                 },
                 // https://ckeditor.com/docs/ckeditor5/latest/features/link.html#custom-link-attributes-decorators
                 link: {
                     decorators: {
                         addTargetToExternalLinks: true,
                         defaultProtocol: 'https://',
                         toggleDownloadable: {
                             mode: 'manual',
                             label: 'Downloadable',
                             attributes: {
                                 download: 'file'
                             }
                         }
                     }
                 },
                 // https://ckeditor.com/docs/ckeditor5/latest/features/mentions.html#configuration
                 mention: {
                     feeds: [
                         {
                             marker: '@',
                             feed: [
                                 '@apple', '@bears', '@brownie', '@cake', '@cake', '@candy', '@canes', '@chocolate', '@cookie', '@cotton', '@cream',
                                 '@cupcake', '@danish', '@donut', '@dragée', '@fruitcake', '@gingerbread', '@gummi', '@ice', '@jelly-o',
                                 '@liquorice', '@macaroon', '@marzipan', '@oat', '@pie', '@plum', '@pudding', '@sesame', '@snaps', '@soufflé',
                                 '@sugar', '@sweet', '@topping', '@wafer'
                             ],
                             minimumCharacters: 1
                         }
                     ]
                 },
                 // The "super-build" contains more premium features that require additional configuration, disable them below.
                 // Do not turn them on unless you read the documentation and know how to configure them and setup the editor.
                 removePlugins: [
                     // These two are commercial, but you can try them out without registering to a trial.
                     // 'ExportPdf',
                     // 'ExportWord',



                     // This sample uses the Base64UploadAdapter to handle image uploads as it requires no configuration.
                     // https://ckeditor.com/docs/ckeditor5/latest/features/images/image-upload/base64-upload-adapter.html
                     // Storing images as Base64 is usually a very bad idea.
                     // Replace it on production website with other solutions:
                     // https://ckeditor.com/docs/ckeditor5/latest/features/images/image-upload/image-upload.html
                     // 'Base64UploadAdapter',
                     'RealTimeCollaborativeComments',
                     'RealTimeCollaborativeTrackChanges',
                     'RealTimeCollaborativeRevisionHistory',
                     'PresenceList',
                     'Comments',
                     'TrackChanges',
                     'TrackChangesData',
                     'RevisionHistory',
                     'Pagination',
                     'WProofreader',
                     // Careful, with the Mathtype plugin CKEditor will not load when loading this sample
                     // from a local file system (file://) - load this site via HTTP server if you enable MathType
                     'MathType'
                 ]
             });
             CKEDITOR.ClassicEditor.create(document.getElementById('<%=editor_Out.ClientID %>'), {
                 // https://ckeditor.com/docs/ckeditor5/latest/features/toolbar/toolbar.html#extended-toolbar-configuration-format
                 toolbar: {
                     items: ['ckfinder', '|',
                         'exportPDF', 'exportWord', '|',
                         'findAndReplace', 'selectAll', '|',
                         'heading', '|',
                         'bold', 'italic', 'strikethrough', 'underline', 'code', 'subscript', 'superscript', 'removeFormat', '|',
                         'bulletedList', 'numberedList', 'todoList', '|',
                         'outdent', 'indent', '|',
                         'undo', 'redo',
                         '-',
                         'fontSize', 'fontFamily', 'fontColor', 'fontBackgroundColor', 'highlight', '|',
                         'alignment', '|',
                         'link', 'insertImage', 'blockQuote', 'insertTable', 'mediaEmbed', 'codeBlock', 'htmlEmbed', '|',
                         'specialCharacters', 'horizontalLine', 'pageBreak', '|',
                         'textPartLanguage', '|',
                         'sourceEditing'
                     ],
                     contentInsert: 'In line',
                     shouldNotGroupWhenFull: true
                 },
                 // Changing the language of the interface requires loading the language file using the <script> tag.
                 language: 'tw',
                 list: {
                     properties: {
                         styles: true,
                         startIndex: true,
                         reversed: true
                     }
                 },
                 // https://ckeditor.com/docs/ckeditor5/latest/features/headings.html#configuration
                 heading: {
                     options: [
                         { model: 'paragraph', title: 'Paragraph', class: 'ck-heading_paragraph' },
                         { model: 'heading1', view: 'h1', title: 'Heading 1', class: 'ck-heading_heading1' },
                         { model: 'heading2', view: 'h2', title: 'Heading 2', class: 'ck-heading_heading2' },
                         { model: 'heading3', view: 'h3', title: 'Heading 3', class: 'ck-heading_heading3' },
                         { model: 'heading4', view: 'h4', title: 'Heading 4', class: 'ck-heading_heading4' },
                         { model: 'heading5', view: 'h5', title: 'Heading 5', class: 'ck-heading_heading5' },
                         { model: 'heading6', view: 'h6', title: 'Heading 6', class: 'ck-heading_heading6' }
                     ]
                 },
                 // https://ckeditor.com/docs/ckeditor5/latest/features/editor-placeholder.html#using-the-editor-configuration
                 //placeholder: 'Welcome to CKEditor 5!',
                 // https://ckeditor.com/docs/ckeditor5/latest/features/font.html#configuring-the-font-family-feature
                 fontFamily: {
                     options: [
                         'default',
                         'Arial, Helvetica, sans-serif',
                         'Courier New, Courier, monospace',
                         'Georgia, serif',
                         'Lucida Sans Unicode, Lucida Grande, sans-serif',
                         'Tahoma, Geneva, sans-serif',
                         'Times New Roman, Times, serif',
                         'Trebuchet MS, Helvetica, sans-serif',
                         'Verdana, Geneva, sans-serif'
                     ],
                     supportAllValues: true
                 },
                 // https://ckeditor.com/docs/ckeditor5/latest/features/font.html#configuring-the-font-size-feature
                 fontSize: {
                     options: [10, 12, 14, 'default', 18, 20, 22],
                     supportAllValues: true
                 },
                 // Be careful with the setting below. It instructs CKEditor to accept ALL HTML markup.
                 // https://ckeditor.com/docs/ckeditor5/latest/features/general-html-support.html#enabling-all-html-features
                 htmlSupport: {
                     allow: [
                         {
                             name: /.*/,
                             attributes: true,
                             classes: true,
                             styles: true
                         }
                     ]
                 },
                 // Be careful with enabling previews
                 // https://ckeditor.com/docs/ckeditor5/latest/features/html-embed.html#content-previews
                 htmlEmbed: {
                     showPreviews: true
                 },
                 // https://ckeditor.com/docs/ckeditor5/latest/features/link.html#custom-link-attributes-decorators
                 link: {
                     decorators: {
                         addTargetToExternalLinks: true,
                         defaultProtocol: 'https://',
                         toggleDownloadable: {
                             mode: 'manual',
                             label: 'Downloadable',
                             attributes: {
                                 download: 'file'
                             }
                         }
                     }
                 },
                 // https://ckeditor.com/docs/ckeditor5/latest/features/mentions.html#configuration
                 mention: {
                     feeds: [
                         {
                             marker: '@',
                             feed: [
                                 '@apple', '@bears', '@brownie', '@cake', '@cake', '@candy', '@canes', '@chocolate', '@cookie', '@cotton', '@cream',
                                 '@cupcake', '@danish', '@donut', '@dragée', '@fruitcake', '@gingerbread', '@gummi', '@ice', '@jelly-o',
                                 '@liquorice', '@macaroon', '@marzipan', '@oat', '@pie', '@plum', '@pudding', '@sesame', '@snaps', '@soufflé',
                                 '@sugar', '@sweet', '@topping', '@wafer'
                             ],
                             minimumCharacters: 1
                         }
                     ]
                 },
                 // The "super-build" contains more premium features that require additional configuration, disable them below.
                 // Do not turn them on unless you read the documentation and know how to configure them and setup the editor.
                 removePlugins: [
                     // These two are commercial, but you can try them out without registering to a trial.
                     // 'ExportPdf',
                     // 'ExportWord',



                     // This sample uses the Base64UploadAdapter to handle image uploads as it requires no configuration.
                     // https://ckeditor.com/docs/ckeditor5/latest/features/images/image-upload/base64-upload-adapter.html
                     // Storing images as Base64 is usually a very bad idea.
                     // Replace it on production website with other solutions:
                     // https://ckeditor.com/docs/ckeditor5/latest/features/images/image-upload/image-upload.html
                     // 'Base64UploadAdapter',
                     'RealTimeCollaborativeComments',
                     'RealTimeCollaborativeTrackChanges',
                     'RealTimeCollaborativeRevisionHistory',
                     'PresenceList',
                     'Comments',
                     'TrackChanges',
                     'TrackChangesData',
                     'RevisionHistory',
                     'Pagination',
                     'WProofreader',
                     // Careful, with the Mathtype plugin CKEditor will not load when loading this sample
                     // from a local file system (file://) - load this site via HTTP server if you enable MathType
                     'MathType'
                 ]
             });
         </script>
       <script type="text/javascript">
           // This sample still does not showcase all CKEditor 5 features (!)
           // Visit https://ckeditor.com/docs/ckeditor5/latest/features/index.html to browse all the features.
           CKEDITOR.ClassicEditor.create(document.getElementById('<%=editor_Sys.ClientID %>'), {
               // https://ckeditor.com/docs/ckeditor5/latest/features/toolbar/toolbar.html#extended-toolbar-configuration-format
               toolbar: {
                   items: ['ckfinder', '|',
                       'exportPDF', 'exportWord', '|',
                       'findAndReplace', 'selectAll', '|',
                       'heading', '|',
                       'bold', 'italic', 'strikethrough', 'underline', 'code', 'subscript', 'superscript', 'removeFormat', '|',
                       'bulletedList', 'numberedList', 'todoList', '|',
                       'outdent', 'indent', '|',
                       'undo', 'redo',
                       '-',
                       'fontSize', 'fontFamily', 'fontColor', 'fontBackgroundColor', 'highlight', '|',
                       'alignment', '|',
                       'link', 'insertImage', 'blockQuote', 'insertTable', 'mediaEmbed', 'codeBlock', 'htmlEmbed', '|',
                       'specialCharacters', 'horizontalLine', 'pageBreak', '|',
                       'textPartLanguage', '|',
                       'sourceEditing'
                   ],
                   contentInsert: 'In line',
                   shouldNotGroupWhenFull: true
               },
               // Changing the language of the interface requires loading the language file using the <script> tag.
               language: 'tw',
               list: {
                   properties: {
                       styles: true,
                       startIndex: true,
                       reversed: true
                   }
               },
               // https://ckeditor.com/docs/ckeditor5/latest/features/headings.html#configuration
               heading: {
                   options: [
                       { model: 'paragraph', title: 'Paragraph', class: 'ck-heading_paragraph' },
                       { model: 'heading1', view: 'h1', title: 'Heading 1', class: 'ck-heading_heading1' },
                       { model: 'heading2', view: 'h2', title: 'Heading 2', class: 'ck-heading_heading2' },
                       { model: 'heading3', view: 'h3', title: 'Heading 3', class: 'ck-heading_heading3' },
                       { model: 'heading4', view: 'h4', title: 'Heading 4', class: 'ck-heading_heading4' },
                       { model: 'heading5', view: 'h5', title: 'Heading 5', class: 'ck-heading_heading5' },
                       { model: 'heading6', view: 'h6', title: 'Heading 6', class: 'ck-heading_heading6' }
                   ]
               },
               // https://ckeditor.com/docs/ckeditor5/latest/features/editor-placeholder.html#using-the-editor-configuration
               //placeholder: 'Welcome to CKEditor 5!',
               // https://ckeditor.com/docs/ckeditor5/latest/features/font.html#configuring-the-font-family-feature
               fontFamily: {
                   options: [
                       'default',
                       'Arial, Helvetica, sans-serif',
                       'Courier New, Courier, monospace',
                       'Georgia, serif',
                       'Lucida Sans Unicode, Lucida Grande, sans-serif',
                       'Tahoma, Geneva, sans-serif',
                       'Times New Roman, Times, serif',
                       'Trebuchet MS, Helvetica, sans-serif',
                       'Verdana, Geneva, sans-serif'
                   ],
                   supportAllValues: true
               },
               // https://ckeditor.com/docs/ckeditor5/latest/features/font.html#configuring-the-font-size-feature
               fontSize: {
                   options: [10, 12, 14, 'default', 18, 20, 22],
                   supportAllValues: true
               },
               // Be careful with the setting below. It instructs CKEditor to accept ALL HTML markup.
               // https://ckeditor.com/docs/ckeditor5/latest/features/general-html-support.html#enabling-all-html-features
               htmlSupport: {
                   allow: [
                       {
                           name: /.*/,
                           attributes: true,
                           classes: true,
                           styles: true
                       }
                   ]
               },
               // Be careful with enabling previews
               // https://ckeditor.com/docs/ckeditor5/latest/features/html-embed.html#content-previews
               htmlEmbed: {
                   showPreviews: true
               },
               // https://ckeditor.com/docs/ckeditor5/latest/features/link.html#custom-link-attributes-decorators
               link: {
                   decorators: {
                       addTargetToExternalLinks: true,
                       defaultProtocol: 'https://',
                       toggleDownloadable: {
                           mode: 'manual',
                           label: 'Downloadable',
                           attributes: {
                               download: 'file'
                           }
                       }
                   }
               },
               // https://ckeditor.com/docs/ckeditor5/latest/features/mentions.html#configuration
               mention: {
                   feeds: [
                       {
                           marker: '@',
                           feed: [
                               '@apple', '@bears', '@brownie', '@cake', '@cake', '@candy', '@canes', '@chocolate', '@cookie', '@cotton', '@cream',
                               '@cupcake', '@danish', '@donut', '@dragée', '@fruitcake', '@gingerbread', '@gummi', '@ice', '@jelly-o',
                               '@liquorice', '@macaroon', '@marzipan', '@oat', '@pie', '@plum', '@pudding', '@sesame', '@snaps', '@soufflé',
                               '@sugar', '@sweet', '@topping', '@wafer'
                           ],
                           minimumCharacters: 1
                       }
                   ]
               },
               // The "super-build" contains more premium features that require additional configuration, disable them below.
               // Do not turn them on unless you read the documentation and know how to configure them and setup the editor.
               removePlugins: [
                   // These two are commercial, but you can try them out without registering to a trial.
                   // 'ExportPdf',
                   // 'ExportWord',



                   // This sample uses the Base64UploadAdapter to handle image uploads as it requires no configuration.
                   // https://ckeditor.com/docs/ckeditor5/latest/features/images/image-upload/base64-upload-adapter.html
                   // Storing images as Base64 is usually a very bad idea.
                   // Replace it on production website with other solutions:
                   // https://ckeditor.com/docs/ckeditor5/latest/features/images/image-upload/image-upload.html
                   // 'Base64UploadAdapter',
                   'RealTimeCollaborativeComments',
                   'RealTimeCollaborativeTrackChanges',
                   'RealTimeCollaborativeRevisionHistory',
                   'PresenceList',
                   'Comments',
                   'TrackChanges',
                   'TrackChangesData',
                   'RevisionHistory',
                   'Pagination',
                   'WProofreader',
                   // Careful, with the Mathtype plugin CKEditor will not load when loading this sample
                   // from a local file system (file://) - load this site via HTTP server if you enable MathType
                   'MathType'
               ]
           });
       </script>
</asp:Content>

