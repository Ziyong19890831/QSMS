<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="SearchCode.aspx.cs" Inherits="Mgt_SearchCode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="gv_Code" runat="server" AutoGenerateColumns="false" Visible="false">
        <Columns>
            <asp:BoundField DataField="CourseSNO" HeaderText="課程代號" />
             <asp:BoundField DataField="MVal" HeaderText="課程類型" />
            <asp:BoundField DataField="CourseName" HeaderText="課程名稱" />
        </Columns>
    </asp:GridView>
     <asp:GridView ID="gv_PClassSNO" runat="server" AutoGenerateColumns="false" Visible="false">
        <Columns>
            <asp:BoundField DataField="PClassSNO" HeaderText="課程規劃代號" />
            <asp:BoundField DataField="PlanName" HeaderText="課程規劃名稱" />
        </Columns>
    </asp:GridView>
</asp:Content>

