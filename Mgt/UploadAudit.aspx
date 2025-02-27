<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="UploadAudit.aspx.cs" Inherits="Mgt_UploadAudit" %>

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
    <div class="path txtS mb20">現在位置：<a href="#">證書課程管理</a> <i class="fa fa-angle-right"></i><a href="UploadAudit.aspx">作業審核</a></div>
    <fieldset>
        <legend>功能列</legend>
        <div class="left w8">
            姓名<asp:TextBox ID="txt_Name" runat="server"></asp:TextBox>
            身分證<asp:TextBox ID="txt_PersonID" runat="server"></asp:TextBox>
           <%-- 課程<asp:DropDownList ID="ddl_CourseSNO" DataTextField="" DataValueField="" runat="server"></asp:DropDownList>
            狀態<asp:DropDownList ID="ddl_Audit" runat="server"></asp:DropDownList>--%>
        </div>
        <div class="right">
            <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
            <%--<asp:Button ID="btnExport" runat="server" Text="匯出" OnClick="btnExport_Click" />--%>
        </div>
    </fieldset>
    <asp:GridView ID="gv_LearningRecord" runat="server" AutoGenerateColumns="False" OnRowCreated="gv_LearningRecord_RowCreated">
        <Columns>
            <asp:BoundField HeaderText="序號" DataField="ROW_NO">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField HeaderText="姓名" DataField="PName" />
            <asp:BoundField HeaderText="身分證" DataField="PersonID" />
            <asp:BoundField HeaderText="PersonSNO" DataField="PersonSNO" />
            <asp:BoundField HeaderText="課程代號" DataField="CourseSNO" />
            <asp:BoundField HeaderText="課程名稱" DataField="CourseName" />
            <asp:BoundField HeaderText="是否審核" DataField="Audit" />
  <%--          <asp:TemplateField HeaderText="學員身分證">
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("PersonID_encryption") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>--%>
            <%--<asp:BoundField HeaderText="學員身分證" DataField="PersonID" />--%>
            <%--<asp:BoundField HeaderText="課程完成日" DataField="FinishedDate" />--%>
            <asp:TemplateField HeaderText="審核" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                   <asp:LinkButton ID="lk_Link" runat="server" OnClick="lk_Link_Click" CssClass="fa fa-pen-square"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
</asp:Content>

