<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="QuestionnaireDetail.aspx.cs" Inherits="Mgt_QuestionnaireDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
        <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table class="tbl">
        <tr id="tbl_QuestionnaireDetail" runat="server">
            <th>姓名</th>
            <th>身分證</th>
            <th>身份</th>
            <th>類別</th>
            <th>e-Learning課程名稱</th>
            <th>問卷題目</th>
            <th>學員答案</th>
        </tr>
        <asp:Repeater ID="rpt_QuestionnaireDetail" runat="server">
            <ItemTemplate>
                <tr>
                    <td class="center"><%# Eval("PName") %></td>
                    <td class="center"><%# Eval("PersonID") %></td>
                    <td class="center"><%# Eval("RoleName") %></td>
                    <td class="center"><%# Eval("Class") %></td>
                    <td class="center"><%# Eval("ELSName") %></td>
                    <td class="center"><%# Eval("QName") %></td>
                    <td class="center"><%# Eval("Ans") %></td>
                   
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
     <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
</asp:Content>

