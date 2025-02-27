<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="EventRole_AE.aspx.cs" Inherits="Mgt_EventRole_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />

    <table>
        <tr>
            <th><i class="fa fa-star"></i>規則名稱</th>
            <td>
                <asp:Label ID="lbl_CourseName" runat="server" Text="最多50字元"></asp:Label>
                <br />
                <asp:TextBox ID="txt_ERName" class="required w10" runat="server" MaxLength="50"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>課程類別</th>
            <td>
                <asp:DropDownList ID="ddl_Class1" class="required" runat="server" DataValueField="PVal" DataTextField="MVal" />實體
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>是否啟用</th>
            <td>
                <asp:CheckBox ID="chk_IsEnable" runat="server" Text="是" />
            </td>
        </tr>

    </table>
        <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click"/>
        <input name="btnCancel" type="button" value="取消" onclick="location.href='EventRole.aspx';"/>
    </div>
</asp:Content>

