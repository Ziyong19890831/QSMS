<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/Mgt.master" CodeFile="CertificatePrint.aspx.cs" Inherits="Mgt_CertificatePrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="path txtS mb20">現在位置：<a href="#">證書/積分管理</a> <i class="fa fa-angle-right"></i><a href="CertificatePrint.aspx">證書列印</a></div>

    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                對應證書<asp:DropDownList ID="ddl_CType" runat="server" DataValueField="CTypeSNO" DataTextField="CTypeName"></asp:DropDownList>
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                <asp:Button ID="btnPring" runat="server" Text="查詢結果列印" OnClick="btnPdf_Click" />
            </div>
        </fieldset>
    </div>

    <asp:GridView ID="gv_Certificate" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="學員名稱" DataField="PName" />
            <asp:BoundField HeaderText="證號" DataField="CertID" />          
            <asp:BoundField HeaderText="證書類型" DataField="CTypeName" />
            <asp:BoundField HeaderText="發證單位" DataField="CUnitName" />
            <asp:BoundField HeaderText="首發日期" DataField="CertPublicDate" />
            <asp:BoundField HeaderText="公告日期" DataField="CertStartDate" />
            <asp:BoundField HeaderText="到期日期" DataField="CertEndDate" />
        </Columns>
    </asp:GridView>

    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />

    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>

