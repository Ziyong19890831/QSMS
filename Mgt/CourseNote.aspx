<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="CourseNote.aspx.cs" Inherits="Mgt_CourseNote" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path txtS mb20">現在位置：<a href="#">課程管理</a> <i class="fa fa-angle-right"></i><a href="CourseNote.aspx">課程說明</a></div>

    <table>
        <tr>
            <th><i class="fa fa-star"></i>說明內文</th>
            <td colspan="3">
                <asp:Label ID="lbl_CourseTitle" runat="server" Text="最多200字元" ></asp:Label>
                <br />
                <asp:TextBox ID="txt_CourseTitle" runat="server" MaxLength="200" class="w10" TextMode="MultiLine" Rows="5"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_Title" runat="server" ErrorMessage="說明內文不得為空" ControlToValidate="txt_CourseTitle" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
            <td align="center">
                <asp:Button ID="btnOK" runat="server" Text="修改" OnClick="btnOK_Click" />
            </td>
        </tr>
    </table>


    <asp:GridView ID="gv_Course" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" />
            <asp:BoundField DataField="RoleName" HeaderText="角色名稱" />
            <asp:BoundField DataField="CourseFile" HeaderText="課程說明檔案" />
            <asp:TemplateField HeaderText="上傳">
                <ItemTemplate>
                    <a href="#" onclick="var winvar = window.open('./CourseNoteUpload.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("RoleSNO").ToString() %>','winname','width=750 height=260 location=no,menubar=no status=no,toolbar=no');">檔案上傳</a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>


    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
    
     

</asp:Content>
