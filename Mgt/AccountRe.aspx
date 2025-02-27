<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="AccountRe.aspx.cs" Inherits="Mgt_AccountRe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
        $(function () {
            $(".datepicker").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="path txtS mb20">現在位置：<a href="#">系統功能管理</a> <i class="fa fa-angle-right"></i><a href="AccountRe.aspx">帳號復權</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                身分證<asp:TextBox ID="txt_searchID" runat="server" class="w2"></asp:TextBox>
            </div>
            <div class="right">
                <input name="btnSearch" type="button" value="查詢" runat="server" onserverclick="btnSearch_Click" />
            </div>
        </fieldset>
         <asp:GridView ID="gv_AccountRe" runat="server" AutoGenerateColumns="False" OnRowCommand="gv_AccountRe_RowCommand" OnRowCreated="gv_AccountRe_RowCreated" OnRowDataBound="gv_AccountRe_RowDataBound">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="PName" HeaderText="姓名" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
           <asp:BoundField DataField="PersonID_encryption" HeaderText="身分證" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
            <asp:BoundField DataField="RoleName" HeaderText="身份" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
            <asp:BoundField DataField="LoginError" HeaderText="錯誤次數" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
          <asp:BoundField DataField="LoginErrorTime" HeaderText="最後錯誤登入時間" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
             <asp:TemplateField HeaderText="復權" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                     <asp:Button ID="Btn_Grant" runat="server" Text="恢復"  CommandName="getdata" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
             <asp:BoundField DataField="PersonID" HeaderText="PersonID" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
        </Columns>
    </asp:GridView>


   <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

    </div>
</asp:Content>

