<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true"
    CodeFile="SystemOrganRole_AE.aspx.cs" Inherits="Mgt_SystemOrganRole_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function CheckAllCheckBox(obj, name) {
            var elements = document.forms[0].elements;
            for (var i = 0; i < elements.length; i++) {
                if (elements[i].type == 'checkbox') {
                    if (elements[i].name.indexOf(name) > -1) {
                        elements[i].checked = obj.checked;
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hidsno" runat="server" />
    <asp:HiddenField ID="hidst" runat="server" />
    單位名稱：<asp:Label ID="lbl_RoleName" runat="server" Text=""></asp:Label><br />
    <asp:GridView ID="gv_RoleMenuAe" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField Visible="False">
                <ItemTemplate>
                    <asp:Label ID="SRID" runat="server" Text='<%# Eval("SRID")%>'></asp:Label>
                </ItemTemplate>
                <HeaderTemplate>
                    SRID
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <%# Eval("SRNAME").ToString()%>
                </ItemTemplate>
                <HeaderTemplate>
                    角色名稱
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:TemplateField> 
            <asp:TemplateField HeaderText="權限" ItemStyle-HorizontalAlign="center">
                <HeaderTemplate>
                    <asp:CheckBox ID="CheckAllview" runat="server" onclick="javascript: CheckAllCheckBox(this,'chkISVIEW');"
                        Text="權限" ToolTip="按一次全選，再按一次取消全選" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkISVIEW" runat="server" Checked='<%# (Eval("ISVIEW").ToString() == "1")?true:false%>'>
                    </asp:CheckBox>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click" Style="padding: 10px;
        font-size: 16px;" />
    <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" Style="padding: 10px;
        font-size: 16px;" />
</asp:Content>
