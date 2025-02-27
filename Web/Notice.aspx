

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="Notice.aspx.cs" Inherits="Web_Notice" ValidateRequest="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style>
        table a {
            color: #000000;
        }
    </style>
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">公告事項</li>
        </ol>
    </nav>

    <asp:HiddenField ID="hdnSelectedTab" runat="server" Value="0" />

    <div class="alert alert-primary" role="alert">
        <div class="row">
            <div class="col-12">
                分類查詢：
            </div>
            <div class="col-12">
                <div class="form-row">
                    <div class="form-group col-md-3">
                        <asp:DropDownList ID="ddl_Notice_Class" runat="server" DataTextField="Name" DataValueField="NoticeCSNO" class="form-control"></asp:DropDownList>
                    </div>
                    <div class="form-group col-md-3">
                        <asp:DropDownList ID="dd2_RoleName" runat="server" DataTextField="RoleName" DataValueField="RoleGroup" class="form-control"></asp:DropDownList>
                    </div>

                    <div class="form-group col-md-3">
                        <input type="text" placeholder="請輸入要查詢的關鍵字" id="txtSearch" runat="server" class="form-control" />
                    </div>
                    <div class="form-group col-md-3">
                        <input type="button" onserverclick="btnSearch_Click" runat="server" value="查詢" class="form-control" />
                    </div>
                </div>
            </div>
        </div>
    </div>



    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>

    <ul class="nav nav-tabs" id="myMember" role="tablist">
        <li class="nav-item">
            <a class="nav-link active" id="tabs-a-4" data-toggle="tab" href="#tabs-4" role="tab" aria-controls="tabs-1" aria-selected="true">公告事項</a>
        </li>
<%--        <li class="nav-item">
            <a class="nav-link" id="tabs-a-5" data-toggle="tab" href="#tabs-5" role="tab" aria-controls="tabs-2" aria-selected="false">文獻</a>
        </li>--%>
        <li class="nav-item">
            <a class="nav-link" id="tabs-a-3" data-toggle="tab" href="#tabs-3" role="tab" aria-controls="tabs-3" aria-selected="false">歷史公告事項</a>
        </li>
    </ul>
    <div class="tab-content" id="myTableData">
        <div class="tab-pane fade show active mt10" id="tabs-4" role="tabpanel" aria-labelledby="tabs-a-4">

            <asp:UpdatePanel ID="Up1" runat="server">
                <ContentTemplate>

                    <table class="table table-striped">
                        <tr>
                            <th class="w1 center" style="width: 15%">類別</th>
                            <th style="width: 15%">人員</th>
                            <th style="width: 55%">標題</th>
                            <th class="w2 center" style="width: 15%">日期</th>
                        </tr>
                        <asp:Repeater ID="rpt_NoticeMore" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("ClassName") %></td>
                                    <td><%# Eval("RoleBindName") %></td>
                                    <td <%# Eval("OrderSeq").ToString() != "" ? "class='td_news'" : "" %>><a href="Notice_AE.aspx?sno=<%# Eval("NoticeSNO") %>"><%# Eval("Title") %></a></td>
                                    <td><a href="#"><%#Convert.ToDateTime(Eval("SDate")).ToString("yyyy-MM-dd") %></a></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>

                    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
                    <asp:HiddenField ID="txt_Page" runat="server" />
                    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
                </ContentTemplate>
            </asp:UpdatePanel>


        </div>
        <div class="tab-pane fade mt10" id="tabs-5" role="tabpanel" aria-labelledby="tabs-a-5">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="table table-striped">
                        <tr>
                            <th class="w1 center" style="width: 15%">類別</th>
                            <th style="width: 15%">人員</th>
                            <th style="width: 55%">標題</th>
                            <th class="w2 center" style="width: 15%">日期</th>
                        </tr>
                        <asp:Repeater ID="rpt_Word" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("ClassName") %></td>
                                    <td><%# Eval("RoleBindName") %></td>
                                    <td <%# Eval("OrderSeq").ToString() != "" ? "class='td_news'" : "" %>><a href="Notice_AE.aspx?sno=<%# Eval("NoticeSNO") %>"><%# Eval("Title") %></a></td>
                                    <td><a href="#"><%#Convert.ToDateTime(Eval("SDate")).ToString("yyyy-MM-dd") %></a></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>

                    <asp:Literal ID="ltl_PageNumber_word" runat="server"></asp:Literal>
                    <asp:HiddenField ID="txt_Page_word" runat="server" />
                    <asp:Button ID="btnPage_word" runat="server" Text="查詢" OnClick="btnPage_word_Click" Style="display: none;" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="tab-pane fade mt10" id="tabs-3" role="tabpanel" aria-labelledby="tabs-a-3">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <table class="table table-striped">
                        <tr>
                            <th class="w1 center" style="width: 15%">類別</th>
                            <th style="width: 15%">人員</th>
                            <th style="width: 55%">標題</th>
                            <th class="w2 center" style="width: 15%">日期</th>
                        </tr>
                        <asp:Repeater ID="rpt_NoticeMore_his" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("ClassName") %></td>
                                    <td><%# Eval("RoleBindName") %></td>
                                    <td <%# Eval("OrderSeq").ToString() != "" %>><a href="Notice_AE.aspx?sno=<%# Eval("NoticeSNO") %>"><%# Eval("Title") %></a></td>
                                    <td><a href="#"><%#Convert.ToDateTime(Eval("SDate")).ToString("yyyy-MM-dd") %></a></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>

                    <asp:Literal ID="ltl_PageNumber_his" runat="server"></asp:Literal>
                    <asp:HiddenField ID="txt_Page_his" runat="server" />
                    <asp:Button ID="btnPage_his" runat="server" Text="查詢" OnClick="btnPage_his_Click" Style="display: none;" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <script type="text/javascript">
     
        //$(function () {
            
        //    location.reload();
          
        //});
        function _goPage(pageNumber) {

            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
                    document.getElementById("<%=btnPage.ClientID%>").click();
        }
        function _goPage2(pageNumber) {
            document.getElementById("<%=txt_Page_his.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage_his.ClientID%>").click();
        }
        function _goPage1(pageNumber) {
         
            document.getElementById("<%=txt_Page_word.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage_word.ClientID%>").click();
        }
    </script>

</asp:Content>