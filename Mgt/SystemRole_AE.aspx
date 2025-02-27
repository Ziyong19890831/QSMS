<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true"
    CodeFile="SystemRole_AE.aspx.cs" Inherits="Mgt_SystemRole_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hidsno" runat="server" />
    <asp:HiddenField ID="hidst" runat="server" />
    <table>
        <tr>
            <th>
                <i class="fa fa-star" aria-hidden="true" style="color: red;"></i>角色名稱
            </th>
            <td>
                <asp:Label ID="lbl_SRNAME" runat="server" Text="最多50字元"></asp:Label><br />
                <asp:TextBox ID="txt_SRNAME" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                <i class="fa fa-star" aria-hidden="true" style="color: red;"></i>角色類別
            </th>
            <td> 
               <asp:DropDownList ID="ddl_OrganLevel" runat="server" DataTextField="LEVEL_NAME" DataValueField="LEVEL_CODE"></asp:DropDownList>
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
