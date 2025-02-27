<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Audit.aspx.cs" Inherits="Mgt_Audit" %>

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

    <div class="path txtS mb20">現在位置：<a href="#">人員帳號管理</a> <i class="fa fa-angle-right" aria-hidden="true"></i><a href="Audit.aspx">公告管理</a></div>
    <%--<wuc:uc1 runat="server" />--%>
    <div class="both mb20">
    </div>

    <asp:GridView ID="gv_Audit" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="Name" HeaderText="申請人" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="Email" HeaderText="電子郵件" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="Phone" HeaderText="手機" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="Tel" HeaderText="電話" ItemStyle-HorizontalAlign="Center" />
            <asp:TemplateField HeaderText="文件" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <a href='../Upload/Web/<%#Eval("FileURL").ToString() %>' target="_blank"><i class="fa fa-pen-square" aria-hidden="true"></i></a>
                </ItemTemplate>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="申請日期" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <%# Convert.ToDateTime(Eval("CreateDate")).ToString("yyyy/MM/dd") %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />


</asp:Content>

