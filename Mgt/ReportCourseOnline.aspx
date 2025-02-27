<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="ReportCourseOnline.aspx.cs" Inherits="Mgt_ReportCourseOnline" %>

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

    <div class="path txtS mb20">現在位置：<a href="#">報表作業</a> <i class="fa fa-angle-right" aria-hidden="true"></i><a href="ReportCourseOnline.aspx">線上課程統計報表</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                課程名稱<asp:TextBox ID="txt_CourseName" runat="server"></asp:TextBox>  
                單元<asp:TextBox ID="txt_UnitName" runat="server"></asp:TextBox>
                <br />
                <span style="color:red; font-weight:bold;">註：<br />1.本"線上課程統計報表"為不分學員身分別之統計紀錄，含西醫生、牙醫師、藥師、衛教師，以及不分區。<br />2.每整點更新一次</span>
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                <asp:Button ID="btnExport" runat="server" Text="匯出" OnClick="btnExport_Click" />
            </div>
        </fieldset>
    </div>


    <asp:GridView ID="gv_Course" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField HeaderText="序號" DataField="ROW_NO">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField HeaderText="課程名稱" DataField="CourseName"/>
            <asp:BoundField HeaderText="E-Learning課程名稱" DataField="ELSName"/>
            <asp:BoundField HeaderText="已上課人數" DataField="LearnCount" />
        </Columns>
    </asp:GridView>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>


