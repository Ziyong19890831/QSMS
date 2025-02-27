<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Urls_AE.aspx.cs" Inherits="Mgt_Urls_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_No" runat="server" />  

    <table>
        <tr>
            <th><i class="fa fa-star"></i>標題</th>
            <td  colspan="3">
                <asp:Label ID="lbl_Name" runat="server" Text="最多50字元"></asp:Label>
                <br />
                <asp:TextBox ID="txt_Name" runat="server" class="w10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_Name" runat="server" ControlToValidate="txt_Name" ErrorMessage="不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>分類</th>
            <td>
                <asp:DropDownList ID="ddl_Class" runat="server" DataValueField="URLCSNO" DataTextField="Name"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>連結</th>
            <td>
                <asp:Label ID="lbl_Url" runat="server" Text="最多300字元"></asp:Label>
                <br />
                <asp:TextBox ID="txt_Url" runat="server" class="w10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_Url" runat="server" ControlToValidate="txt_Url" ErrorMessage="不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    <div class="center btns">
        <asp:Button ID="ButtonOK" runat="server" OnClick="ButtonOK_Click" Text="修改" />
        <input name="btnCancel" type="button" value="取消" onclick="location.href='Urls.aspx';" />
    </div>
    
</asp:Content>

