<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="ECoursePlanningDetail.aspx.cs" Inherits="Mgt_ECoursePlanningDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="gv_EcourseDetail" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="CTypeName" HeaderText="對應證書" />
            <asp:BoundField DataField="CRole" HeaderText="適用人員" />
            <asp:BoundField DataField="PlanName" HeaderText="繼續教育課程規劃" />
            <asp:BoundField DataField="Compulsory_Entity" HeaderText="必修實體學分" />
            <asp:BoundField DataField="Compulsory_Practical" HeaderText="必修實習學分" />
            <asp:BoundField DataField="Compulsory_Communication" HeaderText="必修通訊學分" />
            <asp:BoundField DataField="Compulsory_Online" HeaderText="必修線上學分" />
            <asp:BoundField DataField="StartTime" HeaderText="報名期間(起)" />
            <asp:BoundField DataField="EndTime" HeaderText="報名期間(迄)" />
        </Columns>
    </asp:GridView>
</asp:Content>

