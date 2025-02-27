<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="UrlClass_AE.aspx.cs" Inherits="Mgt_UrlClass_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_No" runat="server" />

    <table>
        <tr>
            <th class="auto-style1"><i class="fa fa-star"></i>分類名稱</th>
            <td>
                <asp:Label ID="lbl_Name" runat="server" Text="最多20字元"></asp:Label>
                <asp:TextBox ID="txt_Name" runat="server" class="w10"></asp:TextBox>
                <br />
                <asp:RequiredFieldValidator ID="rfv_Name" runat="server" ErrorMessage="不得為空" ForeColor="Red" ControlToValidate="txt_Name"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th class="auto-style1">註記</th>
            <td>
                <asp:Label ID="lbl_Note" runat="server" Text="最多100位元"></asp:Label>
                <asp:TextBox ID="txt_Note" runat="server" TextMode="MultiLine" class="w10" Rows="5"></asp:TextBox>
                <br />
                <asp:RequiredFieldValidator ID="rfv_Note" runat="server" ErrorMessage="不得為空" ForeColor="Red" ControlToValidate="txt_Note"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>

    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click" Text="確認" />
        <input name="btnCancel" type="button" value="取消" onclick="location.href='UrlClass.aspx';" />
    </div>

</asp:Content>

