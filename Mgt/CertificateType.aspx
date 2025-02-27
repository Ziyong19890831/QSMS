<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="CertificateType.aspx.cs" Inherits="Mgt_CertificateType" %>

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

    <div class="path txtS mb20">現在位置：<a href="#">證書/積分管理</a> <i class="fa fa-angle-right"></i><a href="CertificateType.aspx">證書類別管理</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                證書名稱<asp:TextBox ID="txt_CertificateType" runat="server" class="w2"></asp:TextBox>
                <span style="color:red; font-weight:bold;"><br />註：@符號為字號所匯入之位置</span>
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                <% if (userInfo.AdminIsInsert == true) { %>
                <input name="btnInsert" type="button" value="新增" onclick="location.href = 'CertificateType_AE.aspx?Work=N'" />
                <% } %>
            </div>
        </fieldset>
    </div>


    <asp:GridView ID="gv_Course" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField HeaderText="序號" DataField="ROW_NO" />
            <asp:BoundField HeaderText="證書名稱" DataField="CTypeName" />
            <asp:BoundField HeaderText="適用人員" DataField="RoleBindName" />
            <asp:BoundField HeaderText="證書列印格式" DataField="CTypeFile" />
            <asp:BoundField HeaderText="證書字號" DataField="CTypeString" />
            <%--<asp:BoundField HeaderText="流水編號" DataField="CTypeSEQ" />--%>
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsUpdate == true) { %>
                    <a href='./CertificateType_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("CTypeSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
                    <% } else { %> 
                        無權限 
                    <% } %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsDelete == true) { %>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("CTypeSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                    <% } else { %> 
                        無權限 
                    <% } %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />

    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>

