<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="TODOLIST.aspx.cs" Inherits="Web_TODOLIST" %>

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
            <li class="breadcrumb-item active" aria-current="page">站內訊息</li>
        </ol>
    </nav>

    <h5 class="pt30"><i class="fa fa-question-circle"></i>站內訊息</h5>
    <div class="alert alert-primary" role="alert">
        <div class="row">
            <div class="col-12">
                <asp:Label ID="lbl_ptitle" runat="server" Text=""></asp:Label>
                <div style="float: right">
                    <input type="button" class="btn btn-info" onclick="location.href = 'TODO.aspx'" value="返回代辦" />
                </div>
            </div>
        </div>
    </div>

    <div style="border: 1px solid #d4d4d4; width: 100%; padding: 10px 10px 100px 10px;background-color:white;margin-bottom: 200px;">
        <div style="float: left; border: 1px solid #d4d4d4; background-color: #cccccc; color: #808080; text-align: center; font-size: 40pt; width: 50px; height: 50px; padding: 10px;">
            <i class="fa fa-user"></i>
        </div>
        <a style="font-size: 12pt; float: left; font-weight: bold; margin-left: 10px;">
            <asp:Label ID="lbl_postname" runat="server" Text=""></asp:Label><br />
            <span style="font-size: 10pt; font-weight: 400;">寄給我</span>
        </a>
        <a style="float: right; font-size: 10pt">收件日期:<asp:Label ID="lbl_date" runat="server" Text=""></asp:Label></a>
        <br />
        <br />
        <div style="margin-bottom: 20px;"></div>
        <hr />
        <div style="width: 100%">
            <div style="width: 80%;">
                <asp:Label ID="lbl_content" runat="server" Text=""></asp:Label>
            </div>
        </div>

    </div>


</asp:Content>


