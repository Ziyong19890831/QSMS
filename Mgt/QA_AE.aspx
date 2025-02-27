<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="QA_AE.aspx.cs" Inherits="Mgt_QA_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />

    <table>
        <tr>
            <th><i class="fa fa-star"></i>系統</th>
            <td><asp:DropDownList ID="ddl_SystemName" runat="server" DataTextField="SYSTEM_NAME" DataValueField="SYSTEM_ID"  ></asp:DropDownList></td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>類型</th>
            <td><asp:DropDownList ID="ddl_Class" runat="server" DataValueField="QACSNO" DataTextField="Name"></asp:DropDownList></td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>問題</th>
            <td>
                <asp:Label ID="lbl_Title" runat="server" Text="最多50字元"></asp:Label>
                <asp:TextBox ID="txt_Title" runat="server" class="w10"></asp:TextBox>
                <br />
                <asp:RequiredFieldValidator ID="rfv_Title" runat="server" ControlToValidate="txt_Title" ErrorMessage="問題不得為空" ForeColor="Red"></asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>回答:</th>
            <td>
                <asp:Label ID="lbl_Info" runat="server" Text="最多4000字元"></asp:Label>
                <asp:TextBox ID="txt_Info" runat="server" TextMode="MultiLine" class="w10" Rows="5"></asp:TextBox>
                <br />
                <asp:RequiredFieldValidator ID="rfv_Info" runat="server" ControlToValidate="txt_Info" ErrorMessage="回答不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>

    <div class="center btns">
        <asp:Button ID="ButtonOK" runat="server" Text="修改" OnClick="ButtonOK_Click" />
        <input name="btnCancel" type="button" value="取消" onclick="location.href='QA.aspx';" />
    </div>
>
</asp:Content>

