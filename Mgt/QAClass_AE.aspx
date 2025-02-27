<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="QAClass_AE.aspx.cs" Inherits="Mgt_QAClass_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />

    <table>
        <tr>
            <th><i class="fa fa-star"></i>分類名稱</th>
            <td colspan="3">
                <asp:Label ID="lbl_Name" runat="server" Text="最多100字元" Font-Size="Medium"></asp:Label>
                <br />
                <asp:TextBox ID="txt_Name" runat="server" class="w10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_Title" runat="server" ErrorMessage="標題不得為空" ControlToValidate="txt_Name" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>註記</th>
            <td colspan="3">
                <asp:Label ID="lbl_Note" runat="server" Text="最多500字元"></asp:Label>
                <br />
                &nbsp;<asp:TextBox ID="txt_Note" runat="server" TextMode="MultiLine" class="w10" Rows="5"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_Info" runat="server" ControlToValidate="txt_Note" ErrorMessage="內容不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>


    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="修改" OnClick="btnOK_Click"/>
        <input name="btnCancel" type="button" value="取消" onclick="location.href='QAClass.aspx';"/>
    </div>


</asp:Content>

