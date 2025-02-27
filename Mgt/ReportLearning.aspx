<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="ReportLearning.aspx.cs" Inherits="Mgt_ReportLearning" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="path txtS mb20">現在位置：<a href="#">報表作業</a> <i class="fa fa-angle-right"></i><a href="ReportCourseOnline.aspx">線上課程記錄報表</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                學員名稱<asp:TextBox ID="txt_Person" runat="server"></asp:TextBox>
                身分證<asp:TextBox ID="txt_PersonID" runat="server"></asp:TextBox>
                課程名稱<asp:TextBox ID="txt_CourseName" runat="server"></asp:TextBox>
                <br />
                完成日期
                <asp:TextBox ID="txt_SFinishedDate" class="datepicker" runat="server" type="text"></asp:TextBox> - 
                <asp:TextBox ID="txt_EFinishedDate" class="datepicker" runat="server" type="text"></asp:TextBox>
                <br />
                Elearning主題名稱<asp:DropDownList ID="ddl_Elearning" DataTextField="ELName" DataValueField="ELCode" runat="server"></asp:DropDownList>
                                <span style="color:red; font-weight:bold;"><br />註：每整點更新一次</span>
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                <asp:Button ID="btnExport" runat="server" Text="匯出" OnClick="btnExport_Click" />
            </div>
        </fieldset>
    </div>


    <asp:GridView ID="gv_LearningRecord" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField HeaderText="序號" DataField="ROW_NO">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField HeaderText="E-Learning主題名稱" DataField="ELName" />
            <asp:BoundField HeaderText="E-Learning課程" DataField="ELSName" />
            <asp:BoundField HeaderText="完成節數" DataField="ELSPart" />
            <asp:BoundField HeaderText="學員名稱" DataField="PName" />
            <asp:TemplateField HeaderText="學員身分證">
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server"  Text='<%# Bind("PersonID_encryption") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:BoundField HeaderText="學員身分證" DataField="PersonID" />--%>
            <asp:BoundField HeaderText="課程完成日" DataField="FinishedDate" />
        </Columns>
    </asp:GridView>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>


