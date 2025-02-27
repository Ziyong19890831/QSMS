<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="PersonDataLog.aspx.cs" Inherits="Mgt_PersonDataLog" %>

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
    <div class="path txtS mb20">現在位置：<a href="#">帳號管理</a> <i class="fa fa-angle-right"></i><a href="PersonDataLog.aspx">資料修改歷程</a></div>
    <h1><i class="fa fa-newspaper"></i>資料修改歷程</h1>
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                學員名稱<asp:TextBox ID="txt_Person" runat="server"></asp:TextBox>
                身分證<asp:TextBox ID="txt_PersonID" runat="server"></asp:TextBox>    
                修改日期
                <asp:TextBox ID="txt_SDate" class="datepicker" runat="server" type="text"></asp:TextBox> - 
                <asp:TextBox ID="txt_EDate" class="datepicker" runat="server" type="text"></asp:TextBox>
                <br />
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
            </div>
        </fieldset>
    </div>
    <asp:GridView ID="gv_PersonDataLog" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField HeaderText="序號" DataField="ROW_NO">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>            
            <asp:BoundField HeaderText="學員名稱" DataField="PName" />
            <asp:BoundField HeaderText="修改人員" DataField="SName" />
            <asp:BoundField HeaderText="修改項目" DataField="ColumnName" />
            <asp:BoundField HeaderText="修改日期" DataField="CreateDT" />            
            <asp:TemplateField HeaderText="學員身分證">
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server"  Text='<%# Bind("PersonID_encryption") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
</asp:Content>

