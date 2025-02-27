<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="System_AE.aspx.cs" Inherits="Mgt_System_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $.datepicker.setDefaults($.datepicker.regional["zh-TW"]);
    </script>

    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />


    <table>
        <tr>
            <th><i class="fa fa-star"></i>系統代碼</th>
            <td colspan="3">
                <asp:Label ID="lbl_sysid" runat="server" Text="最多3字元" Font-Size="Medium"></asp:Label>
                <br />
                <asp:TextBox ID="txt_sysid" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="系統代碼不得為空" ControlToValidate="txt_sysid" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>系統名稱</th>
            <td colspan="3">
                <asp:Label ID="lbl_sysname" runat="server" Text="最多50字元" Font-Size="Medium" class="w10"></asp:Label>
                <br />
                <asp:TextBox ID="txt_sysname" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_Title" runat="server" ErrorMessage="系統名稱不得為空" ControlToValidate="txt_sysname" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>系統簡述</th>
            <td colspan="3">
                <asp:Label ID="lbl_Info" runat="server" Text="最多800字元"></asp:Label>
                <asp:TextBox ID="txt_Info" runat="server" TextMode="MultiLine" Rows="5" class="w10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>是否啟用</th>
            <td colspan="3">
                <asp:DropDownList ID="DropDownList1" runat="server">
                    <asp:ListItem Value="0">停權</asp:ListItem>
                    <asp:ListItem Value="1">啟用</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>

    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="修改" OnClick="btnOK_Click" />
        <input name="btnCancel" type="button" value="取消" onclick="location.href='System.aspx';" />
    </div>

</asp:Content>


