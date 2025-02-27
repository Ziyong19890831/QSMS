<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="LoginLog.aspx.cs" Inherits="Mgt_LoginLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>
    <script type="text/javascript">

        $(function () {        

            $(".datepicker").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'yy-mm-dd'
            }).blur(function () {
                val = $(this).val();
                val1 = Date.parse(val);
                if (isNaN(val1) == true && val !== '') {
                    $(this).val('');
                }
            });

        });


    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="path txtS mb20">現在位置：<a href="#">帳號管理</a> <i class="fa fa-angle-right" aria-hidden="true"></i><a href="Personnel.aspx">登入紀錄</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                <% if (userInfo.RoleOrganType == "S") { %>
                <asp:UpdatePanel ID="upl_ddl" runat="server" UpdateMode="Conditional" style="display: inline;">
                    <ContentTemplate>
                        行政區：
                        <asp:DropDownList ID="ddl_AreaCodeA" runat="server" OnSelectedIndexChanged="ddl_AreaCodeA_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddl_AreaCodeB" runat="server" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                            <asp:ListItem Value="" Text="請先選擇縣市行政區"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddl_AreaCodeA" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                單位代碼<asp:TextBox ID="txt_OrganCode" runat="server"></asp:TextBox>
                單位名稱<asp:TextBox ID="txt_OrganName" runat="server"></asp:TextBox>
                <br />
                <% } %>
                使用者帳號<asp:TextBox ID="txt_PAccount" runat="server"></asp:TextBox>
                使用者姓名<asp:TextBox ID="txt_PName" runat="server"></asp:TextBox>
                管理者<asp:DropDownList ID="ddl_IsAdmin" runat="server">
                         <asp:ListItem Value="" Text="請選擇"></asp:ListItem>
                         <asp:ListItem Value="0" Text="一般使用者"></asp:ListItem>
                         <asp:ListItem Value="1" Text="管理者"></asp:ListItem>
                    <asp:ListItem Value="2" Text="學會"></asp:ListItem>
                     </asp:DropDownList>
                登入狀態<asp:DropDownList ID="ddl_LoginStatus" runat="server">
                        <asp:ListItem Value=""  Text="請選擇"></asp:ListItem>
                        <%--<asp:ListItem Value="0000" Text="驗證成功"></asp:ListItem>--%>
                        <asp:ListItem Value="0001" Text="登入成功"></asp:ListItem>
                        <%--<asp:ListItem Value="4001" Text="認證碼錯誤"></asp:ListItem>--%>
                        <%--<asp:ListItem Value="4002" Text="帳號不存在"></asp:ListItem>--%>
                        <asp:ListItem Value="4003" Text="密碼錯誤"></asp:ListItem>
                        <asp:ListItem Value="4004" Text="帳號鎖定"></asp:ListItem>
                        <asp:ListItem Value="4005" Text="帳號停用中"></asp:ListItem>
                        <asp:ListItem Value="5001" Text="帳號審核中"></asp:ListItem>
                     </asp:DropDownList>
                <br />
                登入時間
                <asp:TextBox ID="LoginStart" Class="datepicker" runat="server"></asp:TextBox>
                ～<asp:TextBox ID="LoginEnd" Class="datepicker" runat="server"></asp:TextBox>
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                <asp:Button ID="btnExport" runat="server" Text="匯出" OnClick="btnExport_Click" />
                <%--<% if (userInfo.AdminIsInsert == true) { %>
                <input name="btnInsert" type="button" value="新增" onclick="location.href = 'Personnel_AE.aspx?Work=N'" />
                <% } %>--%>
            </div>
        </fieldset>
    </div>

    <asp:GridView ID="gv_Person" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="OrganCode" HeaderText="單位代碼" />
            <asp:BoundField DataField="AreaName" HeaderText="行政區" />
            <asp:BoundField DataField="OrganName" HeaderText="單位名稱" />
            <asp:BoundField DataField="RoleName" HeaderText="使用者角色" />
            <asp:BoundField DataField="PAccount" HeaderText="使用者帳號" />
            <asp:BoundField DataField="PName" HeaderText="使用者姓名" />
            <asp:BoundField DataField="LoginTime" HeaderText="登入時間" />
            <asp:BoundField DataField="LoginInfo" HeaderText="登入狀態" />
        </Columns>
    </asp:GridView>

    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>

