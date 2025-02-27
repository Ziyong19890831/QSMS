<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Web.master" AutoEventWireup="true" CodeFile="Certificate.aspx.cs" Inherits="Web_Certificate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style>
        #tbl td { font-size:14px; }

    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path mb20">目前位置：<a href="Certificate.aspx">證書資料</a></div>


    <h1><i class="fa fa-certificate"></i>已取得的證書</h1>

    <div class="alert alert-info">
      <asp:Label ID="NoteCertificate" runat="server" Text="null"></asp:Label>
    </div>

    <table id="tbl">
        <tr>
            <th>證號</th>
            <th>證書類型</th>
            <th>發證單位</th>
            <th>首發日期</th>
            <th>公告日期</th>
            <th>到期日期</th>
            <th>展延</th>
            <th>預覽</th>
        </tr>
        <asp:Repeater ID="rpt_Notice" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%# Eval("CertID") %></td>
                    <td><%# Eval("CTypeName") %></td>
                    <td><%# Eval("CUnitName") %></td>
                    <td class="smallfont"><%# Eval("CertPublicDate") %></td>
                    <td class="smallfont"><%# Eval("CertStartDate") %></td>
                    <td class="smallfont"><%# Eval("CertEndDate") %></td>
                    <td><%# Eval("CertExt") %></td>
                    <td>see</td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    
    <h1><i class="fa fa-star"></i>積分資料</h1>

    <div class="alert alert-warning">
        <asp:Label ID="NoteScore" runat="server" Text="null"></asp:Label>
    </div>

    <table>
        <tr>
            <th>課程規劃名稱</th>
            <th>參考年度</th>
            <th>已取得/總積分</th>
            <th>可取得的證書</th>
        </tr>
        <asp:Repeater ID="rpt_CoursePlanningClass" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%# Eval("PlanName") %></td>
                    <td class="center"><%# Eval("CStartYear") + "-" + Eval("CEndYear") %></td>
                    <td class="center">
                        <%# Eval("PClassTotalHr").ToString()=="" ? "0" : Eval("PClassTotalHr") %> / 
                        <%# Eval("sumHours").ToString()=="" ? "-" : Eval("sumHours") %>
                    </td>
                    <td><%# Eval("CTypeName") %></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>   


</asp:Content>

