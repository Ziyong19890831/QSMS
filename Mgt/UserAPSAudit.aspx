<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="UserAPSAudit.aspx.cs" Inherits="Mgt_UserAPSAudit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="path txtS mb20">現在位置：<a href="#">帳號管理</a> <i class="fa fa-angle-right"></i><a href="UserAPS.aspx">醫事人員審核</a></div>
    
    <div class="both mb20">
        
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                <% if (userInfo.RoleOrganType == "S") { %>
                <asp:UpdatePanel ID="upl_ddl" runat="server" UpdateMode="Conditional" style="display: inline;">
                    <ContentTemplate>
                        行政區
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
                <% } %>
                單位代碼<asp:TextBox ID="txt_OrganCode" runat="server" class="w1"></asp:TextBox>
                單位名稱<asp:TextBox ID="txt_OrganName" runat="server" class="w1"></asp:TextBox>
                <br />
                學員帳號<asp:TextBox ID="txt_PAccount" runat="server" class="w1"></asp:TextBox>
                學員身分證<asp:TextBox ID="txt_PID" runat="server" class="w1"></asp:TextBox>
                學員姓名<asp:TextBox ID="txt_PName" runat="server" class="w1"></asp:TextBox>
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click"/>
            </div>
        </fieldset>
        
    </div>

    <asp:GridView ID="gv_Person" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="OrganCode" HeaderText="單位代碼" />
            <asp:BoundField DataField="AreaName" HeaderText="行政區" />
            <asp:BoundField DataField="OrganName" HeaderText="單位名稱" />
            <asp:BoundField DataField="RoleName" HeaderText="角色" />
            <asp:BoundField DataField="PAccount" HeaderText="帳號" />
            <asp:BoundField DataField="PersonID_encryption" HeaderText="身分證" />
            <asp:BoundField DataField="PName" HeaderText="姓名" />
            <asp:TemplateField HeaderText="審核" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsUpdate == true) { %>
                     <a href="#" onclick="var winvar = window.open('./UserAPSAudit_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("PersonSNO").ToString() %>','winname','width=800 height=550 location=no,menubar=no status=no,toolbar=no');"><i class="fa fa-pen-square"></i></a>
                    <% } else { %>
                       無權限
                    <% } %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>

    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:HiddenField ID="txt_EditRole" runat="server" />
    <asp:HiddenField ID="txt_AddRole" runat="server" />
    <asp:HiddenField ID="txt_DelRole" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>



