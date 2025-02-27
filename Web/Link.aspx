<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="Link.aspx.cs" Inherits="Web_Link" %>

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
            <li class="breadcrumb-item active" aria-current="page">相關連結</li>
        </ol>
    </nav>

    <h5 class="pt30"><i class="fa fa-link"></i>相關連結</h5>
    <div class="alert alert-primary" role="alert">
        <div class="row">
            <div class="col-12">
                分類查詢：
            </div>
            <div class="col-12">
                <div class="form-row">
                    <div class="form-group col-md-4">
                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:UpdatePanel ID="upl_DDL" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_Url_Class" runat="server" DataTextField="Name" class="form-control">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_Url_Class" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Button ID="btnSearch" class="btn btn-info" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <asp:Repeater ID="rpt_Link" runat="server" OnItemDataBound="rpt_Link_ItemDataBound">
            <ItemTemplate>
                <div class="col-12 col-md-6">
                    <div>
                        <h5>
                            <i class="fa fa-angle-double-right" style="color: #96BC33">
                            <%# Eval("Name")%></i>
                        </h5>
                    </div>
                    <ul class="L2 w10 left mb30">
                        <asp:Repeater ID="rpt_SubLink" runat="server">
                            <ItemTemplate>
                                <li>
                                    <%--<div class="img logos" style="background-image: url(../Images/logo201.png)"></div>--%>
                                    <a href="<%# Eval("Url") %>" target="_blank" rel="noopener noreferrer"><%# Eval("Name") %></a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

</asp:Content>
