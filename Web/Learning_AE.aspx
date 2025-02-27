<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="Learning_AE.aspx.cs" Inherits="Web_Learning_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr id="tbl_CoursePlanningClass" runat="server">
            <th>課程類別</th>
            <th>適用人員</th>
            <th>課程名稱</th>
            <th>授課方式</th>
            <th>參考年度</th>
            <th>時數</th>
            <th>取得積分</th>     
        </tr>
        <asp:Repeater ID="rpt_CoursePlanningClass" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%# Eval("Class1") %></td>
                    <td><%# Eval("RoleName") %></td>
                    <td><%# Eval("CourseName") %></td>
                    <td><%# Eval("Ctype") %></td>
                    <td class="center"><%# Eval("CStartYear") + "-" + Eval("CEndYear") %></td> 
                    <td class="center"><%# Eval("CHour") %></td>
                    <td class="center"><%# Eval("O").ToString()==""?"未取得":"已取得" %></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>

    </table>
</asp:Content>

