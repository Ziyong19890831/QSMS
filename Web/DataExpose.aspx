<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Web.master" AutoEventWireup="true" CodeFile="DataExpose.aspx.cs" Inherits="Web_DataExpose" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path mb20">目前位置：<a href="DataExpose.aspx">資料揭露</a></div>
    <h1>資料揭露</h1>
    <ul class="listNews">
        <asp:Repeater ID="rpt_Data" runat="server">
            <ItemTemplate>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</asp:Content>

