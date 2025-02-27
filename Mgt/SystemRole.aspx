<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="SystemRole.aspx.cs" Inherits="Mgt_SystemRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=hidPage.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path txtS mb20">
        現在位置：<a href="#">系統功能管理</a> <i class="fa fa-angle-right" aria-hidden="true"></i>
        <a href="#">角色管理</a>
    </div>
    <div class="both mb20">
        <div class="left mb20">
            角色名稱<asp:TextBox ID="txt_SRNAME" runat="server"></asp:TextBox>
            角色類別<asp:DropDownList ID="ddl_OrganLevel" runat="server" DataTextField="LEVEL_NAME" DataValueField="LEVEL_CODE"></asp:DropDownList>
        </div>
        <div class="right">
            <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" Style="padding: 10px; font-size: 16px;" />
            <asp:Button ID="btnInsert" runat="server" Text="新增" OnClick="btnInsert_Click" Style="padding: 10px; font-size: 16px;" />
        </div>
    </div>
    <asp:GridView ID="gv_Role" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="SRNAME" HeaderText="角色名稱" />
            <asp:BoundField DataField="LEVEL_NAME" HeaderText="角色類別" />
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <a href='./SystemRole_AE.aspx?sno=<%#Eval("SRID").ToString() %>&st=<%=Request.QueryString["st"] %>'>
                        <i class="fa fa-pen-square" aria-hidden="true"></i></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="修改權限" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <a href='./SystemRoleMenu_AE.aspx?sno=<%#Eval("SRID").ToString() %>&st=<%=Request.QueryString["st"] %>'>
                        <i class="fa fa-pen-square" aria-hidden="true"></i></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');"
                        OnClick='btnDEL_Click' CommandArgument='<%# Eval("SRID")%>'><i class="fa fa-trash" aria-hidden="true"></i></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="hidPage" runat="server" />
    <asp:HiddenField ID="hidSystem" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>
