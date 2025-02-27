<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="Download.aspx.cs" Inherits="Web_Download" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style>
    table a {
        color:#000000 ;
    }
    </style>
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">下載專區</li>
        </ol>
    </nav>

    <h5 class="pt30"><i class="fa fa-cloud-download-alt"></i>下載專區</h5>
    <div class="alert alert-primary" role="alert">
        <div class="row">
            <div class="col-12">
                分類查詢：
            </div>
            <div class="col-12">
                <div class="form-row">
                    <div class="form-group col-md-3">
                        <asp:DropDownList ID="ddl_Download_Class" runat="server" AutoPostBack="true" DataTextField="DLCNAME" DataValueField="DLCSNO" class="form-control"></asp:DropDownList>
                    </div>
                    <div class="form-group col-md-3">
                        <asp:DropDownList ID="dd2_RoleName" runat="server" AutoPostBack="true" DataTextField="RoleName" DataValueField="RoleGroup" class="form-control"></asp:DropDownList>
                    </div>

                    <div class="form-group col-md-3">
                        <input type="text" placeholder="請輸入要查詢的關鍵字" class="form-control" id="txtSearch" runat="server" style="width: 180px" />
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Button ID="btnSearch" class="btn btn-info" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-12">
            <table class="table table-striped">
                <tr>
                    <th class="canter">檔案分類</th>
                    <th>檔案</th>
                    <th>說明</th>
                    <th class="center">適用人員</th>
                    <th class="w2 center">日期</th>
                </tr>
                <asp:Repeater ID="rpt_DLOAD" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("DLCNAME") %></td>
                            <td><%# Eval("DLOADNAME") %><br />

                                <div style="padding-left: 20px;"><%# getFiles(Eval("DLOADURL").ToString()) %></div>
                            </td>
                            <td style="word-break: break-word"><%# Eval("DLOADNote") %></td>
                            <td><%# Eval("RoleBindName") %></td>
                            <td class="date center" style="color: black"><%#Convert.ToDateTime(Eval("CreateDT")).ToString("yyyy-MM-dd") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>

    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>

