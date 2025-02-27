<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Mgt_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    
<%--    <div style="float:left; width:35%; text-align:right;">
        <img src="../Images/img01.png" />
    </div>
    <div style="float:left;width:60%;">--%>
        <div class="notetxt" style="margin-top:50px;">
            親愛的管理員 <%=userInfo.UserName %> 您好！
        </div>
        <div class="notetxt">
            請點選上方選單列進行作業！
        </div>
<%--    </div>--%>

</asp:Content>

