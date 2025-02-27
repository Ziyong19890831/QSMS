<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="QA.aspx.cs" Inherits="Web_QA" %>

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
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <style>
        table a {
            color: #000000;
        }
    </style>
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">Q&A</li>
        </ol>
    </nav>

    <h5 class="pt30"><i class="fa fa-question-circle"></i>Q&A</h5>

    <div class="alert alert-primary" role="alert">
        <div class="row">
            <div class="col-12">
                分類查詢：
            </div>
            <div class="col-12">
                <div class="form-row">
                    <div class="form-group col-md-4">
                        <asp:DropDownList ID="ddl_QAClass" runat="server" DataTextField="Name" DataValueField="QACSNO" class="form-control"></asp:DropDownList>
                    </div>

                    <div class="form-group col-md-4">
                        <asp:TextBox ID="txt_Search" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-4">
                        <asp:Button ID="btnSearch" class="btn btn-info" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="col-12 table-responsive">
    <table class="table table-striped">
        <tr>
            <th class="w2">日期</th>
            <th>分類</th>
            <th>標題</th>
        </tr>
        <asp:Repeater ID="rpt_QA" runat="server">
            <ItemTemplate>
                <tr>
                    <td class="date" style="color: black"><%#Convert.ToDateTime(Eval("CreateDT")).ToString("yyyy-MM-dd") %></td>
                    <td><%# Eval("ClassName") %></td>
                    <td><a href="QA_AE.aspx?sno=<%# Eval("QASNO") %>"><%# Eval("Title") %></a></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
        </div>

    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>

