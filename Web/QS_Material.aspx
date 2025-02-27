<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Web.master" AutoEventWireup="true" CodeFile="QS_Material.aspx.cs" Inherits="Web_QS_Material" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
        <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="path mb20">目前位置：<a href="Download.aspx">教材專區</a></div>
    <h1><i class="fa fa-cloud-download-alt"></i>教材專區
        <div class="blockSearch right">
            教材分類查詢：
            <asp:DropDownList ID="ddl_Download_Class" runat="server" AutoPostBack="true" DataTextField="DLCNAME" DataValueField="DLCSNO"></asp:DropDownList>
            
            <input type="text" placeholder="請輸入要查詢的關鍵字" id="txtSearch" runat="server" style="width: 180px" />
            <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
        </div>
    </h1>


    <table>
        <tr>
            <th class="canter" style="width:10%">檔案分類</th>
            <th style="width:30%">檔案</th>
            <th style="width:40%">說明</th>
            <th class="center" style="width:10%">適用人員</th>
            <th class="w2 center" style="width:10%">日期</th>
        </tr>
        <asp:Repeater ID="rpt_DLOAD" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%# Eval("DLCNAME") %></td>
                    <td><%# Eval("DLOADNAME") %><br />

                        <div style="padding-left:20px;"><%# getFiles(Eval("DLOADURL").ToString()) %></div>
                    </td>
                    <td><%# Eval("DLOADNote") %></td>
                    <td><%# Eval("RoleBindName") %></td>
                    <td class="date center" style="color: black"><%#Convert.ToDateTime(Eval("CreateDT")).ToString("yyyy-MM-dd") %></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
</asp:Content>

