<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Newtogo_AE.aspx.cs" Inherits="Mgt_Newtogo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />

    <div class="path txtS mb20">現在位置：<a href="#">資源管理</a> <i class="fa fa-angle-right"></i><a href="Newtogo.aspx">新手上路說明文件上傳</a></div>


    <table>
        <tr id="A" runat="server">
            <th>目前新手上路檔案</th>
            <td colspan="3">
                <asp:Literal ID="lit_Newtogo" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>適用人員</th>
            <td colspan="3">
                <asp:CheckBoxList ID="cb_Role" class="required" runat="server" RepeatColumns="4" DataTextField="RoleName" DataValueField="RoleSNO" RepeatLayout="Table" />
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>上傳檔案</th>
            <td colspan="3">
                <asp:FileUpload ID="fileup_New" runat="server" />
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>檔案名稱</th>
            <td colspan="3">
                <asp:TextBox ID="txt_Name" MaxLength="50" runat="server" class="w10"></asp:TextBox>
            </td>
        </tr>
    </table>


    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="修改" OnClick="btnOK_Click"/>
        <input name="btnCancel" type="button" value="取消" onclick="location.href = 'Newtogo.aspx'"/>
    </div>

</asp:Content>
