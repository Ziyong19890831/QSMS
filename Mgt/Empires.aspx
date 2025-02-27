<%@ Page Language="VB" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Empires.aspx.vb" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 
 
    <div class="path txtS mb20">現在位置：<a href="#">迷霧森林</a> <i class="fa fa-angle-right"></i><a href="Empires.aspx">世紀帝國</a></div>
   
    <div class="both mb20">
        <fieldset>
            <legend>對戰功能列</legend>
            <div class="left w8">
                <p>請配置士兵對戰隊形</p>
            </div>
            <div class="right">
                <asp:Button ID="Button1" runat="server" class="btn btn-lg btn-success" Text="對戰" />
            </div>
            <div class="w10">
                <asp:TextBox ID="T_input" runat="server" TextMode="MultiLine" Rows="10" class="w10" style="font-size:18px;"></asp:TextBox>
            </div>
        </fieldset>
    </div>

    <div>
        <%=Result %>
    </div>

</asp:Content>