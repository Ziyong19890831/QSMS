<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Report.aspx.cs" Inherits="Mgt_Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        table {
            border-collapse: collapse;
        }
        table, td, th {
            border: 1px solid black;

        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server" >
     <div class="path txtS mb20">現在位置：<a href="#">報表作業 </a> <i class="fa fa-angle-right"></i><a href="Report.aspx">總表匯出</a></div>

     <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left">
               <asp:DropDownList runat="server" ID="ddl_RoleName" DataTextField="RoleName" DataValueField="RoleGroup"></asp:DropDownList>
                <asp:Panel ID="Export_Panel" runat="server"  >
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                    <asp:GridView ID="gv_Report" runat="server" AutoGenerateColumns="true" showHeader="false" GridLines="None">
                    </asp:GridView>
                </asp:Panel>
            </div>
            <div class="left">
                <asp:Button ID="btn_report" runat="server" OnClick="btn_report_Click" Text="匯出"/>
            </div>
        </fieldset>
     </div>
   
</asp:Content>

