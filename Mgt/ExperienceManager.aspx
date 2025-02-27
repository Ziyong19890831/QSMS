<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="ExperienceManager.aspx.cs" Inherits="Mgt_ExperienceManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
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
        function showMSG() {
            $("#c_content").show("fade");
        }

        function hideMSG() {
            $("#c_content").hide("fade");
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
            /*    text-align: cen*/ ter;
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
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="path txtS mb20">現在位置：<a href="#">常用服務管理</a> <i class="fa fa-angle-right"></i><a href="ExperienceManager.aspx">師資經歷管理</a></div>
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                姓名<asp:TextBox ID="txt_searchPName" runat="server" class="w2"></asp:TextBox>
                身分證<asp:TextBox ID="txt_searchPersonID" runat="server" class="w2"></asp:TextBox>
                方案編號 <asp:TextBox ID="txt_searchTrainPlanNumber" runat="server" class="w2"></asp:TextBox>
                <% if (userInfo.RoleLevel == "0" || userInfo.RoleLevel == "1" || userInfo.RoleLevel == "2")
                    { %>
                方案<asp:DropDownList ID="ddl_TrainPlanNumber" runat="server" DataTextField="TrainPlanNumber" DataValueField="TrainPlanNumber"></asp:DropDownList>
                <% } %>
            </div>
            <div class="right">
                <input id="btnSearch" type="button" value="查詢" runat="server" onserverclick="btnSearch_ServerClick" />
                <% if (userInfo.AdminIsInsert == true)
                    { %>
                <input name="btnInsert" type="button" value="新增" onclick="location.href = 'ExperienceManager_AE.aspx?Work=N'" />
                <input type="button" value="寄送通知" onclick="showMSG()" /> <%}%>
            </div>
        </fieldset>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gv_Experience" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <%--<asp:TemplateField HeaderText="*" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Wrap="False" />
                            <ItemStyle Wrap="False" />
                            <HeaderTemplate>
                                <asp:CheckBox ID="CheckAllItem" runat="server" onclick="javascript: CheckAllItem(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox" AutoCallBack="true" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:BoundField DataField="ROW_NO" HeaderText="序號"></asp:BoundField>
                        <asp:BoundField DataField="TCName" HeaderText="師資類別" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                        <asp:BoundField DataField="PName" HeaderText="姓名"></asp:BoundField>
                        <asp:BoundField DataField="PersonID_encryption" HeaderText="身分證"></asp:BoundField>
                        <asp:BoundField DataField="TrainPlanNumber" HeaderText="方案代號"></asp:BoundField>
                        <asp:BoundField DataField="Register" HeaderText="是否註冊"></asp:BoundField>
                        <asp:BoundField DataField="IsTrain" HeaderText="是否回訓"></asp:BoundField>
                        <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick="btnDEL_Click" CommandArgument='<%# Eval("PersonID") + "," +  Eval("TrainType")+ ","+ Eval("TrainPlanNumber") + "," + Eval("TrainRoleType")+ "," + Eval("PName")+ "," + Eval("PMail")%> '><i class="fa fa-trash"></i></asp:LinkButton>

                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
                <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
                <asp:HiddenField ID="txt_Page" runat="server" />
                <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="gv_Experience" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <!--只有按下按鈕時才會出現-->
    <div id="c_content" class="ContentDiv">

        <div style="background-color: white; opacity: 0.95; padding: 20px 30px; border-radius: 5px; margin: 100px auto 0 auto; z-index: 999; width: 700px; height: 550px;">

            <a onclick="hideMSG()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

            <h1><i class="fa fa-at" style="text-align: center" aria-hidden="true"></i>寄送通知</h1>
            <%--<input type="button" value="取消" onclick="hideLogin()" style="float: right;" />--%>
            <%--<p style="margin-left: 10px">--%>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    名單類別代號：<asp:TextBox ID="txt_Person" Height="150px" Width="500px" runat="server"></asp:TextBox><p></p>
                    信件內容：<asp:TextBox ID="txt_MailContent" Height="150px" Width="500px" runat="server"></asp:TextBox>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="MarginTop" style="text-align: center">
                <asp:Button ID="btnSendMail" runat="server" Text="寄送信件" OnClick="btnSendMail_Click" />
            </div>
        </div>
    </div>
    <!--只有按下按鈕時才會出現-->

</asp:Content>

