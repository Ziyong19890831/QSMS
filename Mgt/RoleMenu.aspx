<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="RoleMenu.aspx.cs" Inherits="Mgt_RoleMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            //location.href = "?page=" + pageNumber + "#mainContent";
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path txtS mb20">現在位置：<a href="#">角色管理</a> <i class="fa fa-angle-right" aria-hidden="true"></i><a href="RoleMenu.aspx">角色對應系統功能設定</a></div>
    <div class="both mb20">
        <div class="left mb20">
            角色名稱<asp:TextBox ID="txt_RoleName" runat="server"></asp:TextBox><br />
        </div>
        <div class="right">
            <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" Style="padding: 10px; font-size: 16px;" />
        </div>
    </div>

    <asp:GridView ID="gv_RoleMenu" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="RoleName" HeaderText="角色名稱" />
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <a href='./RoleMenu_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("RoleSNO").ToString() %>'><i class="fa fa-pen-square" aria-hidden="true"></i></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" Visible="false">
                <ItemTemplate>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("RoleSNO")%>'><i class="fa fa-trash" aria-hidden="true"></i>刪除</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>

    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
    
</asp:Content>

