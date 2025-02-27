<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="SiteMap.aspx.cs" Inherits="Web_SiteMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style>
    table a {
        color:#000000 ;
    }
    </style>
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">網站地圖</li>
        </ol>
    </nav>

    <h5 class="pt30"><i class="fa fa-map-marker"></i>網站地圖</h5>
    <div class="alert alert-primary" role="alert">
        <div class="row">
            <div class="col-12">
                關鍵字查詢：
            </div>
            <div class="col-12">
                <div class="form-row">
                    <div class="form-group col-md-4">
                        <input type="text" placeholder="請輸入要查詢的關鍵字" class="form-control" id="txtSearch" runat="server" />
                    </div>
                    <div class="form-group col-md-4">
                        <input type="button" class="btn btn-info" onserverclick="btn_Search_ServerClick" value="查詢" id="btn_Search"  runat="server"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <% 
        //未登入的Menu
        if (Session["QSMS_UserInfo"] != null)
        {
    %>
    <table class="table table-striped mb10">
        <tr>
            <th>
                <h5>學員服務</h5>
            </th>
        </tr>
        <tr>
            <td>
                <ul>
                    <asp:Repeater ID="rpt_SiteMapG0" runat="server">
                        <ItemTemplate>
                            <li><a href="<%#Eval("URL") %>"><%# Eval("LinkName") %></a></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </td>
        </tr>
    </table>
    <% } %>
    <table class="table table-striped mb10">
        <tr>
            <th><h5>學習課程</h5></th>
        </tr>
        <tr>
            <td>
                <ul>
                    <asp:Repeater ID="rpt_SiteMapG1" runat="server">
                        <ItemTemplate>
                                <li><a href="<%#Eval("URL") %>"><%# Eval("LinkName") %></a></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </td>
        </tr>
    </table>

    <table class="table table-striped mb10">
        <tr>
            <th><h5>服務項目</h5></th>
        </tr>
        <tr>
            <td>
                <ul>
                    <asp:Repeater ID="rpt_SiteMapG2" runat="server">
                        <ItemTemplate>
                                <li><a href="<%#Eval("URL") %>"><%# Eval("LinkName") %></a></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </td>
        </tr>
    </table>
</asp:Content>

