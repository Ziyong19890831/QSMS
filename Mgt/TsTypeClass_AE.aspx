<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="TsTypeClass_AE.aspx.cs" Inherits="Mgt_TsTypeClass_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
            <th class="auto-style1"><i class="fa fa-star"></i>適用人員</th>
            <td>
                <asp:DropDownList ID="ddl_Role" runat="server" DataTextField="RoleName" DataValueField="RoleSNO" ></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="不得為空" ForeColor="Red" ControlToValidate="txt_Name"></asp:RequiredFieldValidator>
            </td>
                 </tr>
                <tr>
            <th class="auto-style1"><i class="fa fa-star"></i>是否啟用</th>
            <td>
               <asp:CheckBox ID="chk_IsEnable" runat="server" />
            </td>
        </tr>
    </table>

    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click" Text="確認" />
        <input name="btnCancel" type="button" value="取消" onclick="location.href='TsTypeClass.aspx';" />
    </div>
</asp:Content>

