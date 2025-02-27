<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Web.master" AutoEventWireup="true" CodeFile="Notice_More.aspx.cs" Inherits="Web_Notice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function _goPage(pageNumber) {
            //location.href = "?page=" + pageNumber + "#mainContent";
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="path mb20">目前位置：<a href="Notice.aspx">公告事項</a></div>
    <h1><i class="fa fa-newspaper" aria-hidden="true"></i>公告事項                     
            <div class="blockSearch right">
                分類查詢：
                <asp:DropDownList ID="ddl_SystemName" runat="server" DataTextField="SYSTEM_NAME" DataValueField="SYSTEM_ID"></asp:DropDownList>
                <asp:DropDownList ID="ddl_Notice_Class" runat="server" DataTextField="Name" DataValueField="NoticeCSNO"></asp:DropDownList>
                <input type="text" placeholder="請輸入要查詢的關鍵字" id="txtSearch" runat="server" />
                <input type="button" onserverclick="btnSearch_Click" runat="server" value="查詢" />
                <%-- <i class="fa fa-search" aria-hidden="true"</i>--%>
            </div>
    </h1>

    <table>
        <tr>
            <th class="w2 center">系統</th>
            <th class="w1 center">分類</th>
            <th>標題</th>
            <th class="w2 center">發布日期</th>
        </tr>
        <asp:Repeater ID="rpt_NoticeMore" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%# Eval("SYSTEM_NAME") %></td>
                    <td><a href="#"><%# Eval("ClassName") %></a></td>
                    <td><a href="Notice_AE.aspx?sno=<%# Eval("NoticeSNO") %>"><%# Eval("Title") %></a></td>
                    <td class="date"><a href="#"><%#Convert.ToDateTime(Eval("SDate")).ToString("yyyy-MM-dd") %></a></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>

