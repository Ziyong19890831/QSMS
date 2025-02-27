<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="ReportTest.aspx.cs" Inherits="Mgt_ReportTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
        $(function () {

            $(".datepicker").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'yy-mm-dd'
            }).blur(function () {
                val = $(this).val();
                val1 = Date.parse(val);
                if (isNaN(val1) == true && val !== '') {
                    $(this).val('');
                }
            });

        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="path txtS mb20">現在位置：<a href="#">報表作業</a> <i class="fa fa-angle-right" aria-hidden="true"></i><a href="ReportTest.aspx">實體測驗成績報表</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                學員名稱<asp:TextBox ID="txt_PName" runat="server"></asp:TextBox>
                身分證<asp:TextBox ID="txt_PersonID" runat="server"></asp:TextBox>
                <br />
                測驗日期<asp:TextBox ID="Date_start" runat="server" class="datepicker date"></asp:TextBox>～<asp:TextBox ID="Date_End" runat="server" class="datepicker date"></asp:TextBox>
                <br />
                測驗名稱<asp:DropDownList ID="dd1_ClassName" runat="server" DataValueField="CourseSNO" DataTextField="CourseName"></asp:DropDownList>
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
            <asp:BoundField HeaderText="測驗名稱" DataField="CourseName" />
            <asp:BoundField HeaderText="學員名稱" DataField="PName" />
            <asp:BoundField HeaderText="學員身分證" DataField="PersonID_encryption" />
            <%--<asp:BoundField HeaderText="成績" DataField="Score"/>--%>
            <asp:BoundField HeaderText="測驗日期" DataField="ExamDate" DataFormatString="{0:d}"/>
            <%--<asp:BoundField HeaderText="是否通過" DataField="IsPass" />--%>
        </Columns>
    </asp:GridView>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>

