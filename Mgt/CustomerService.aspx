<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="CustomerService.aspx.cs" Inherits="Mgt_CustomerService" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="path txtS mb20">現在位置：<a href="#">系統功能管理</a> <i class="fa fa-angle-right"></i><a href="CustomerService.aspx">客服查詢</a></div>
    <fieldset>
        <legend>查詢條件</legend>
        身分證<asp:TextBox ID="txt_PersonID" runat="server"></asp:TextBox>
         <div class="Right btns">
        <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click" />      
    </div>
    </fieldset>
    <asp:GridView ID="gv_Account" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField HeaderText="帳號" DataField="PAccount" />
            <asp:BoundField HeaderText="密碼" DataField="PPWD" />
        </Columns>
    </asp:GridView>
</asp:Content>

