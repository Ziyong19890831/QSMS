<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Web.master" AutoEventWireup="true" CodeFile="_Blank.aspx.cs" Inherits="Web_Blank" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path mb20">目前位置：<a href="Notice.aspx">公告事項</a></div>
    <h1><i class="fa fa-newspaper"></i>公告事項                     
            <div class="blockSearch right">
                <asp:Panel ID="panel_Search" runat="server" DefaultButton="btnSearch">
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel ID="upl_DDL" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            分類查詢：
                            <asp:DropDownList ID="ddl_SystemName" runat="server" AutoPostBack="true" DataTextField="SYSTEM_NAME" DataValueField="SYSTEM_ID"></asp:DropDownList>
                            <asp:DropDownList ID="ddl_Notice_Class" runat="server" AutoPostBack="true" DataTextField="Name" DataValueField="NoticeCSNO"></asp:DropDownList>
                            <input type="text" placeholder="請輸入要查詢的關鍵字" id="txtSearch" runat="server" style="width: 180px" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddl_Notice_Class" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                </asp:Panel>
            </div>
    </h1>


    <table>
        <tr>
            <th class="w2 center">系統</th>
            <th class="w1 center">分類</th>
            <th>標題</th>
            <th class="w2 center">發布日期</th>
        </tr>
        <asp:Repeater ID="rpt_Notice" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%# Eval("SYSTEM_NAME") %></td>
                    <td><%# Eval("ClassName") %></td>
                    <td><a href="Notice_AE.aspx?sno=<%# Eval("NoticeSNO") %>"><%# Eval("Title") %></a></td>
                    <td class="date" style="color: black"><%#Convert.ToDateTime(Eval("SDate")).ToString("yyyy-MM-dd") %></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>


</asp:Content>

