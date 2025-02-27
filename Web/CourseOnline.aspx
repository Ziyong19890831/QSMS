<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="CourseOnline.aspx.cs" Inherits="Web_CourseOnline" %>

<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
     
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
        $(document).ready(function () {
            $('.ann_header')
                .hover(function () {
                    cursorChange(this);
                })
                .click(function () {
                    foldToggle(this);
                });
            //.trigger('click');

            $('.ann_sub_header')
                .hover(function () {
                    cursorChange(this);
                })
                .click(function () {
                    foldToggle(this);
                })
                .trigger('click');  // 預設是折疊起來

        });
        // 打開or摺疊選單
        function foldToggle(element) {
            $(element).next('ul').slideToggle();
        }
        // 讓游標移到標題上時，圖案會變成手指
        function cursorChange(element, cursorType) {
            $(element).css('cursor', 'pointer');
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style>
    .table-data ul {
        list-style: none;
        padding: 0px;
        margin: 0px;
    }
    .table-data ul.cul {
        list-style: none;
        padding: 10px 0px 10px 20px;
        margin: 0px;
    }
    .bigi {
        font-size: 26px;
    }
    </style>

    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">線上課程</li>
        </ol>
    </nav>
    <asp:Label ID="lb_1" Text="" runat="server"></asp:Label>
    <div class="row mt30">
        <div class="col-12">
            <h5>
                線上課程
            </h5>
        </div>
        <div class="col-12">
            <div class="alert alert-success" role="alert">
                1.點擊各「課程類別」文字，可展開確認該課程類別下，目前有哪些課程是有線上課程可上的。<br />
                2.點擊「按此上課去」可前往課程頁面，若點擊無反應，請先解除「彈跳式視窗封鎖」功能。
            </div>
        </div>
    </div>
   
    <table class="table table-striped table-data">
        <tr>
            <th>課程類別</th>
            <th></th>
        </tr>
        <asp:Repeater ID="rpt_CourseOnline" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <ul>
                            <li>
                                <h5 class="ann_sub_header"><%# Eval("ELName") %></h5>
                                <ul class="cul">
                                    <asp:Repeater ID="rpt_list" runat="server" DataSource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("myrelation") %>'>
                                        <ItemTemplate>
                                            <li><i class="fas fa-book"></i><%# DataBinder.Eval(Container.DataItem, "[\"ELSName\"]")%></li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </li>
                        </ul>
                    </td>
                    <td class="center">
                        <asp:LinkButton ID='btnOpenEL' runat="server" OnClientClick='<%# "return AutoSign(" + Eval("ELCode") + ");return false;" %>' OnClick='btnOpenEL_Click' CommandArgument='<%# Eval("ELCode")%>'>
                            <i class="bigi fa fa-chalkboard-teacher"></i>
                            <br />
                            按此上課去
                        </asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />

    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
    <script>
       
    </script>
</asp:Content>

