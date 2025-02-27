<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="EventAuditNote.aspx.cs" Inherits="Mgt_EventAuditNote" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <fieldset>
        <legend>活動管理備註</legend>
        <asp:TextBox ID="txt_Note" runat="server" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
        </fieldset>
</asp:Content>

