<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="UrlClass.aspx.cs" Inherits="Mgt_UrlClass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path txtS mb20">現在位置：<a href="#">常用服務管理</a> <i class="fa fa-angle-right"></i><a href="UrlClass.aspx">連結分類設定</a></div>

    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                關鍵字<asp:TextBox ID="txt_Search" runat="server" class="w2"></asp:TextBox>
            </div>
            <div class="right">
                <input name="" type="button" value="查詢" runat="server" onserverclick="btnSearch_Click" />
                <% if (userInfo.AdminIsInsert == true) { %>
                <input name="btnInsert" type="button" value="新增" onclick="location.href = 'UrlClass_AE.aspx?Work=N'" />
                <% } %>
            </div>
        </fieldset>
    </div>


    <asp:GridView ID="gv_UrlClass" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" />
            <asp:BoundField DataField="Name" HeaderText="名稱" />
            <asp:BoundField DataField="Note" HeaderText="註記" />
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsUpdate == true) { %>
                    <a href='./UrlClass_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&No=<%# Eval("URLCSNO").ToString()%>'><i class="fa fa-pen-square"></i></a>
                    <% } else { %>
                        無權限
                    <% } %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsUpdate == true) { %>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("URLCSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                    <% } else { %>
                        無權限
                    <% } %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>


    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />



    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />


</asp:Content>

