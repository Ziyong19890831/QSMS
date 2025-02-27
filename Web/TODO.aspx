<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="TODO.aspx.cs" Inherits="Web_TODO" %>

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
            <li class="breadcrumb-item active" aria-current="page">站內訊息</li>
        </ol>
    </nav>

    <h5 class="pt30"><i class="fa fa-question-circle"></i>站內訊息</h5>
    <div class="alert alert-primary" role="alert">
        <div class="row">
            <div class="col-12">
                <div class="form-row">
                    <div class="form-group col-md-4">
                        標題搜尋：
                        <asp:TextBox ID="txt_Search" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-4">
                        分類查詢：
                        <asp:DropDownList ID="ddl_Choice" runat="server" class="form-control">
                            <asp:ListItem Text="全部訊息" Value="0"></asp:ListItem>
                            <asp:ListItem Text="未讀訊息" Value="1"></asp:ListItem>
                            <asp:ListItem Text="已讀訊息" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-4">
                        <br>
                        <input type="button" class="btn btn-info" onserverclick="Unnamed_ServerClick" runat="server" value="查詢" />
                    </div>
                </div>
            </div>
        </div>
    </div>


    <asp:Panel ID="Panel1" runat="server">
        <table class="table table-striped">
            <tr>
                <th style="text-align: left">系統訊息標題</th>
                <th style="text-align: left">發送者</th>
                <th style="text-align: left"></th>
                <th style="text-align: left">收件日期</th>
            </tr>
            <asp:Repeater ID="rpt_QA" runat="server">
                <ItemTemplate>
                    <tr id="teee" runat="server">
                        <td>
                            <asp:Label ID="Label1" Visible="false" runat="server" Font-Size="15pt" Text='<i style="color:#82C082" class="fa fa-envelope"></i>'></asp:Label>
                            <asp:Label ID="Label3" Visible="false" runat="server" Font-Size="15pt" Text='<i style="color:#82C082" class="fa fa-envelope-open"></i>'></asp:Label>

                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("TODOTITLE") %>'></asp:Label>
                        </td>
                        <td><%# Eval("PName").ToString()=="管理階級權限" ? "系統" : Eval("PName")  %>
                            <asp:Label ID="Label2" Visible="false" runat="server" Text='<%# Eval("STATE") %>'></asp:Label>
                        </td>
                        <td>
                            <a href='./TODOLIST.aspx?sno=<%#Eval("TODOSNO").ToString() %>'>查看內容<i style="color: #3f3f3f" class="fa fa-eye"></i></a>
                            <td class="date" style="color: black; border: none;"><%#Convert.ToDateTime(Eval("Createdate")).ToString("yyyy-MM-dd") %></td>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>

        </table>

    </asp:Panel>

    <asp:Label ID="lbl_msg" runat="server" Visible="false" Text="目前無待辦事項!"></asp:Label>
    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>


