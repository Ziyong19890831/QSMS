<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true"
    CodeFile="SystemPageLink_AE.aspx.cs" Inherits="Mgt_SystemPageLink_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hidsno" runat="server" />
    <asp:HiddenField ID="hidst" runat="server" />
    <table>
        <tr>
            <th>
                <i class="fa fa-star" aria-hidden="true" style="color: red;"></i>頁面別名
            </th>
            <td>
                <asp:Label ID="lbl_Alias" runat="server" Text="最多60字元"></asp:Label><br />
                <asp:TextBox ID="txt_PLinkAlias" runat="server" MaxLength="60"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                <i class="fa fa-star" aria-hidden="true" style="color: red;"></i>頁面名稱
            </th>
            <td>
                <asp:Label ID="lbl_Name" runat="server" Text="最多60字元"></asp:Label><br />
                <asp:TextBox ID="txt_PLinkName" runat="server" MaxLength="60"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                <i class="fa fa-star" aria-hidden="true" style="color: red;"></i>頁面網址
            </th>
            <td>
                <asp:Label ID="lbl_PLinkUrl" runat="server" Text="最多200字元"></asp:Label><br />
                <asp:TextBox ID="txt_PLinkUrl" runat="server" MaxLength="200"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                <i class="fa fa-star" aria-hidden="true" style="color: red;"></i>狀態
            </th>
            <td>
                <asp:DropDownList ID="ddl_ISENABLE" runat="server">
                    <asp:ListItem Text='啟用' Value='1' Selected="True"></asp:ListItem>
                    <asp:ListItem Text='停用' Value='0'></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click" Style="padding: 10px;
            font-size: 16px;" />
        <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" Style="padding: 10px;
            font-size: 16px;" />
    </div>
</asp:Content>
