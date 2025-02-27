<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="RecordLog.aspx.cs" Inherits="Mgt_RecordLog" %>

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
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="path txtS mb20">現在位置：<a href="#">證書/積分管理</a> <i class="fa fa-angle-right"></i><a href="RecordLog.aspx">積分記錄列印</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                學員名稱<asp:TextBox ID="txt_Person" runat="server"></asp:TextBox>
                身分證<asp:TextBox ID="txt_PersonID" runat="server"></asp:TextBox>
                課程規劃名稱<asp:TextBox ID="txt_PlanName" runat="server"></asp:TextBox>
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
            <asp:BoundField HeaderText="學員名稱" DataField="PName" />
            <asp:BoundField HeaderText="身分證" DataField="PersonID_encryption" />
            <asp:BoundField HeaderText="課程規劃名稱" DataField="PlanName" />
            <asp:TemplateField HeaderText="已取得/總積分" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                     <%# Eval("PClassTotalHr") %>/ <%# Eval("sumHours") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="可取得的證書" DataField="CtypeName" />
        </Columns>
    </asp:GridView>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
</asp:Content>

