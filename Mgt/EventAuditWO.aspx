<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="EventAuditWO.aspx.cs" Inherits="Mgt_EventAuditWO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1><i class="fa fa-book"></i>個人學習歷程</h1>


    <table>
        <tr>
            <th>類別1</th>
            <th>類別2</th>
            <th>單元</th>
            <th>課程名稱</th>
            <th>授課方式</th>
            <th>時數</th>
            <th>課程完成日</th>
        </tr>
        <asp:Repeater ID="rpt_Learning" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%# Eval("Class1") %></td>
                    <td><%# Eval("Class2") %></td>
                    <td><%# Eval("UnitName") %></td>
                    <td><%# Eval("CourseName") %></td>
                    <td><%# Eval("CType") %></td>
                    <td><%# Eval("CHour") %></td>
                    <td><%# Eval("FinishedDate") %></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    
    <h1><i class="fa fa-book"></i>個人學習成績</h1>


    <table>
        <tr>
            <th>e-Learning測驗名稱</th>
            <th>測驗分數</th>
            <th>測驗日期</th>
            <th>通過分數</th>
            <th>是否通過</th>
       </tr>
        <asp:Repeater ID="Learning" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%# Eval("QuizName") %></td>
                    <td><%# Eval("Score") %></td>
                    <td><%# Eval("ExamDate") %></td>
                    <td><%# Eval("PassScore") %></td>
                    <td><%# Eval("Pass") %></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>

