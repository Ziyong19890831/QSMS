<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="PageLink.aspx.cs" Inherits="Mgt_PageLink" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path txtS mb20">現在位置：<a href="#">系統功能管理</a> <i class="fa fa-angle-right"></i><a href="PageLink.aspx">系統選單配置設定</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                頁面名稱<asp:TextBox ID="txt_PLinkName" runat="server" class="w2"></asp:TextBox>
                頁面類型<asp:DropDownList ID="ddl_ISDIR" runat="server">
                    <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                    <asp:ListItem Text="頁面" Value="0"></asp:ListItem>
                    <asp:ListItem Text="資料夾" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click"/>
                <% if (userInfo.AdminIsInsert == true) { %>
                <input name="btnInsert" type="button" value="新增" onclick="location.href = 'PageLink_AE.aspx?Work=N'"/>
                <% } %>
            </div>
        </fieldset>
    </div>

    <asp:GridView ID="gv_PageLink" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="PLINKNAME" HeaderText="頁面名稱" />
            <asp:BoundField DataField="PLINKURL" HeaderText="頁面網址" />
            <asp:TemplateField HeaderText="頁面類型">
                <ItemTemplate>
                    <%#Eval("ISDIR").ToString().Equals("1")?"資料夾":"頁面" %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="GROUPORDER" HeaderText="群組順序" />
            <asp:BoundField DataField="PLINKORDER" HeaderText="順序" />
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if(userInfo.AdminIsUpdate == true) { %>
                    <a href='./PageLink_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("PLINKSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
                    <% } else { %>
                        無權限
                    <% } %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsDelete == true) { %>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("PLINKSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                    <% } else { %>
                        無權限
                    <% } %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>

    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />

    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>

