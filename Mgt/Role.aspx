<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Role.aspx.cs" Inherits="Mgt_Role" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path txtS mb20">現在位置：<a href="#">系統功能管理</a> <i class="fa fa-angle-right"></i><a href="Role.aspx">角色管理</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                角色名稱<asp:TextBox ID="txt_RoleName" runat="server" class="w2"></asp:TextBox>
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click"/>
                <% if (userInfo.AdminIsInsert == true) { %>
                <input name="btnInsert" type="button" value="新增" onclick="location.href = 'Role_AE.aspx?Work=N'"/>
                <% } %>
            </div>
        </fieldset>

        <fieldset>
            <legend>角色單位分類說明</legend>
            <div class="left w5">
                S:[向下管理]的依據為比自己[角色層級]還要大的角色帳號<br />
                A:[縣市管理]管理的依據為相同的Organ.AreaCodeA<br />
                無管理權限:一般學員或學員
            </div>
            <div class="left w5">
                B:[區域管理]管理的依據為相同的Organ.AreaCodeB<br />
                U:[協會管理]僅能管與自己相同RoleGroup的一般帳號
            </div>
        </fieldset>
 
    </div>

    <asp:GridView ID="gv_Role" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="RoleName" HeaderText="角色名稱" />
            <asp:BoundField DataField="RoleOrganType" HeaderText="角色單位分類" />
            <asp:BoundField DataField="RoleLevel" HeaderText="角色等級" />
            <asp:BoundField DataField="RoleGroup" HeaderText="角色群組" />
            <asp:BoundField DataField="IsAdminN" HeaderText="管理者" />
            <asp:TemplateField HeaderText="修改角色" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsUpdate == true) { %>
                    <a href='./Role_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("RoleSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
                    <% } else { %>
                        無權限
                    <% } %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="修改對應系統功能" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsUpdate == true) { %>
                        <asp:Panel runat="server" Visible='<%# Eval("IsAdmin") %>'>  
                            <a href='./RoleMenu_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("RoleSNO").ToString() %>'>
                                <i class="fa fa-pen-square"></i></a>       
                        </asp:Panel>
                    <%--<a href='./RoleMenu_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("RoleSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>--%>
                    <% } else { %>
                        無權限
                    <% } %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsDelete == true) { %>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("RoleSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
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

