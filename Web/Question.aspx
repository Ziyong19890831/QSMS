<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="Question.aspx.cs" Inherits="Web_Question" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            //location.href = "?page=" + pageNumber + "#mainContent";
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 41px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path mb20">目前位置：<a href="Question.aspx">問卷填寫</a></div>
    <h1><i class="fa fa-edit"></i>問卷填寫
           
    </h1>
    <asp:Panel ID="Panel1" runat="server">
        <table style="text-align: center">
            <tr>
                <th>問卷名稱</th>
                <th id="tee2" runat="server"></th>
                <th id="tee3" runat="server"></th>
                <th runat="server" visible="false"></th>
            </tr>


            <asp:Repeater ID="rpt_QA" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>

                            <a style="font-weight: bold"><%# Eval("PaperName") %></a>
                            (<asp:Label ID="Label2" runat="server" Text='<%# Eval("uWrite") %>'></asp:Label>)

                        </td>


                        <td id="teee" runat="server">
                            <a href='./WriteQuestion.aspx?sno=<%#Eval("PaperID").ToString() %>'>
                                <i style="color: #3f3f3f" class="	fa fa-pencil"></i>前往填寫</a>
                        </td>
                        <td id="teee1" runat="server">
                            <a href='./OKQuestion.aspx?sno=<%#Eval("PaperID").ToString() %>'>
                                <i style="color: #3f3f3f" class="	fa fa-eye"></i>查看填寫結果</a>
                        </td>
                        <td runat="server" visible="false">
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("uWrite") %>'></asp:Label></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>

        </table>
    </asp:Panel>
    <asp:Label ID="lbl_msg" runat="server" Visible="false" Text="目前沒有問券填寫!"></asp:Label>

    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>


