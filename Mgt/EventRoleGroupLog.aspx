<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="EventRoleGroupLog.aspx.cs" Inherits="Mgt_EventRoleGroupLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
        <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
              document.getElementById("<%=btnPage.ClientID%>").click();
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:GridView ID="gv_RoleGroupLog" runat="server" AutoGenerateColumns="false" OnRowCreated="gv_RoleGroupLog_RowCreated">
                <Columns>
                    <asp:BoundField HeaderText="活動名稱" DataField="EventName" ItemStyle-CssClass="center" />
                    <asp:BoundField HeaderText="群組編號	" DataField="EventGroupLog" />
                   
                </Columns>
            </asp:GridView>
            <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
            <asp:HiddenField ID="txt_Page" runat="server" />
            <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
           <asp:Label ID="lb_Hint" runat="server"></asp:Label>

</asp:Content>

