<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="ReadMail.aspx.cs" Inherits="Mgt_ReadMail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h1><i class="fa fa-at" aria-hidden="true"></i>寄件內容</h1>
    <%--<asp:TextBox TextMode="MultiLine" ID="txt_Content" Enabled="false" runat="server" Width="550px" Height="200px"></asp:TextBox>--%>
    <asp:Panel BorderWidth="1" runat="server" ID="P1">
        <asp:Label ID="lb_Mailcontent" runat="server"></asp:Label>
    </asp:Panel>
</asp:Content>

