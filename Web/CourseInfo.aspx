<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Web.master" AutoEventWireup="true" CodeFile="CourseInfo.aspx.cs" Inherits="Web_CourseInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path mb20">目前位置：<a href="Notice.aspx">課程資訊</a></div>
    <h1><i class="fa fa-newspaper"></i>課程資訊                     
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
            <th>課程類別1</th>
            <th>課程類別2</th>
            <th>單元</th>
            <th>課程名稱</th>
            <th>授課方式</th>
            <th>時數</th>
        </tr>
        <asp:Repeater ID="rpt_CourseInfo" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%# Eval("Class1") %></td>
                    <td><%# Eval("Class2") %></td>
                    <td><%# Eval("UnitName") %></td>
                    <td><%# Eval("CourseName") %></td>
                    <td><%# Eval("CType") %></td>
                    <td><%# Eval("CHour") %></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>



    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>

