<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="QA_AE.aspx.cs" Inherits="Web_QA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">Q&A</li>
        </ol>
    </nav>
    <div class="col-12 table-responsive">
        <asp:Repeater ID="rpt_QA" runat="server">
            <ItemTemplate>
                <table class="table table-striped">
                    <tr>
                        <th class="w7 txtL">分類：<%# Eval("Name") %></th>
                        <th class="w3 txtL">發布日期：<%#Convert.ToDateTime(Eval("CreateDT")).ToString("yyyy-MM-dd") %></th>
                    </tr>
                    <tr>
                        <th class="txtL" colspan="2 ">標題：<%# Eval("Title") %></th>
                    </tr>
                    <tr>
                        <td colspan="2" class="padding20" style="word-break:break-all">
                            <span><%# Eval("Info") %></span>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="col-12 center">
        <input type="button" class="btn btn-success" value="回上頁" onclick="location.href='QA.aspx';" />
    </div>
    

</asp:Content>

