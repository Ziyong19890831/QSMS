<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="Notice_AE.aspx.cs" Inherits="Web_Notice_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="../">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">公告事項</li>
        </ol>
    </nav>
    <div class="row">
        <div class="col-12">
            <div class="tab-content" id="myTableData">
                <table class="table table-striped">
                    <tr>
                        <th class="w7 txtL">
                            <asp:Label ID="lb_Name" class="control-label" runat="server"></asp:Label></th>
                        <th class="w3 txtL">
                            <asp:Label ID="lb_SDate" class="control-label" runat="server"></asp:Label></th>
                    </tr>
                    <tr>
                        <td colspan="2" class="padding20">
                            <asp:Label ID="lb_Title" class="control-label" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="padding20">
                            <asp:Label ID="lb_Info" class="control-label" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <div class="center pages both w10">
        <asp:Button runat="server" class="btn btn-success" ID="btn_Back" Text="回上頁" OnClick="btn_Back_Click" />
        
    </div>

</asp:Content>

