<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Upload.aspx.cs" Inherits="Mgt_Upload" %>

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

    <div class="path txtS mb20">現在位置：<a href="#">常用服務管理</a> <i class="fa fa-angle-right"></i><a href="Upload.aspx">下載專區作業</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                檔案名稱<asp:TextBox ID="txt_searchTitle" placeholder="請輸入要查詢的關鍵字" Style="width: 180px" runat="server"></asp:TextBox>
                分類查詢<asp:DropDownList ID="ddl_Download_Class" runat="server" AutoPostBack="true" DataTextField="DLCNAME" DataValueField="DLCSNO"></asp:DropDownList>
                適用人員<asp:DropDownList ID="ddl_setRoleName" runat="server" AutoPostBack="true" DataTextField="RoleName" DataValueField="RoleGroup"></asp:DropDownList>
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                <% if (userInfo.AdminIsInsert == true) { %>
                <input name="btnInsert" type="button" value="新增" onclick="location.href = 'Upload_AE.aspx?Work=N'" />
                <% } %>
            </div>
        </fieldset>
    </div>

    <asp:GridView ID="gv_Upload" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="DLCNAME" HeaderText="類別"></asp:BoundField>
              <asp:BoundField DataField="RoleBindName" HeaderText="適用人員"></asp:BoundField>
            <asp:BoundField DataField="OrderSeq" HeaderText="置頂順序" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="DLOADNAME" HeaderText="類別"></asp:BoundField>
            <asp:TemplateField HeaderText="檔案數" ItemStyle-HorizontalAlign="Center" >
                <ItemTemplate>
                    <div><%# getFilesCount(Eval("DLOADURL").ToString()) %></div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CreateDT" HeaderText="上傳日期" />
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsUpdate == true) { %>
                    <a href='./Upload_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("DLOADSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
                    <% } else { %>
                        無權限
                    <% } %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsDelete == true) { %>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("DLOADSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
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

