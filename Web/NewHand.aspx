<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="NewHand.aspx.cs" Inherits="Web_NewHand" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">新手上路</li>
        </ol>
    </nav>

    <h5 class="pt30"><i class="fa fa-address-book"></i>新手上路</h5>
    <div class="alert alert-primary" role="alert">
        <div class="row">
            <div class="col-12">
                分類查詢：
            </div>
            <div class="col-12">
                <div class="form-row">
                    <div class="form-group col-md-4">
                        <asp:DropDownList ID="dd2_RoleName" runat="server" AutoPostBack="true" DataTextField="RoleName" DataValueField="RoleGroup" class="form-control"></asp:DropDownList>
                    </div>
                    <div class="form-group col-md-4">
                        <input type="text" placeholder="請輸入要查詢的關鍵字" class="form-control" id="txtSearch" runat="server" />
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Button ID="btnSearch" class="btn btn-info" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <table class="table table-striped">
        <tr>
            <th class="w2">適用人員</th>
            <th>檔案</th>
            <th class="w2">發布日期</th>
        </tr>
        <asp:Repeater ID="rpt_NewHand" runat="server">
            <ItemTemplate>
                <tr>
                    <td><%# Eval("RoleBindName") %></td>
                    <td><a style="color: blue" target="_blank" href="../NewHand/<%# Eval("NHPath") %>"><%# Eval("NHName") %></a></td>
                    <td class="date" style="color: black"><%#Convert.ToDateTime(Eval("CreateDT")).ToString("yyyy-MM-dd") %></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>

