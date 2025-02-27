<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="UploadClass_AE.aspx.cs" Inherits="Mgt_UploadClass_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="Work" runat="server" />

    <table>
        <tr>
            <th><i class="fa fa-star"></i>名稱</th>
            <td>
                <asp:Label ID="lbl_Name" runat="server" Text="最多60字元"></asp:Label>
                <br />
                <asp:TextBox ID="txt_Name" runat="server" class="w10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_Upload" runat="server" ControlToValidate="txt_Name" ErrorMessage="不可為空" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>所屬系統</th>
            <td>
                <asp:DropDownList ID="ddl_SystemName" runat="server" DataTextField="SYSTEM_NAME" DataValueField="SYSTEM_ID"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>排序</th>
            <td>
                <asp:DropDownList ID="ddl_OrderSeq" runat="server" DataTextField="DLCNAME" DataValueField="DLCSNO">
                    <asp:ListItem Value="0">無</asp:ListItem>
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>

    </table>


    <div class="center btns">
        <asp:Button ID="ButtonOK" runat="server" Text="修改" OnClick="ButtonOK_Click" />
        <input name="btnCancel" type="button" value="取消" onclick="location.href='UploadClass.aspx';" />
    </div>

</asp:Content>

