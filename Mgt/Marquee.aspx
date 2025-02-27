<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Marquee.aspx.cs" Inherits="Mgt_Marquee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path txtS mb20">現在位置：<a href="#">系統功能管理</a> <i class="fa fa-angle-right"></i><a href="Marquee.aspx">跑馬燈文字修改</a></div>

    <table>
        <tr>
            <th><i class="fa fa-star"></i>跑馬燈</th>
            <td>
                <asp:Label ID="lbl_Marquee" runat="server" Text="最多500字元"></asp:Label>
                <br />
                <asp:TextBox ID="txt_Marquee" runat="server" TextMode="MultiLine" cols="45" Rows="5"></asp:TextBox>
            </td>
        </tr>
    </table>

    <br />
    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click" />
    </div>

</asp:Content>

