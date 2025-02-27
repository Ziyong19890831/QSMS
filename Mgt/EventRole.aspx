<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="EventRole.aspx.cs" Inherits="Mgt_EventRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="path txtS mb20">現在位置：<a href="#">系統功能管理</a> <i class="fa fa-angle-right"></i><a href="EventRole.aspx">協會報名管理</a></div>

    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                規則名稱<asp:TextBox ID="txt_RoleName" runat="server" class="w2"></asp:TextBox>
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                <input name="btnInsert" type="button" value="新增" onclick="location.href = 'EventRole_AE.aspx?Work=N'" />
            </div>
        </fieldset>
    </div>


    <asp:GridView ID="gv_EventRole" runat="server" AutoGenerateColumns="False" OnRowCreated="gv_EventRole_RowCreated">
        <Columns>
            <asp:BoundField HeaderText="ERSNO" DataField="ERSNO" />
            <asp:BoundField HeaderText="序號" DataField="ROW_NO" />
            <asp:BoundField HeaderText="課程名稱" DataField="ERName">
                <ItemStyle Width="500px" />
            </asp:BoundField>
            <asp:BoundField HeaderText="課程類別1" DataField="Type" />                     
            <asp:BoundField HeaderText="啟用" DataField="IsEnable" />
             <asp:TemplateField HeaderText="新增規則" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                     <asp:LinkButton ID="lk_Link" runat="server" OnClick="lk_Link_Click" CssClass="fa fa-pen-square"></asp:LinkButton>
                      <%--<a href='./EventRoleDetail.aspx?Work=N&sno=<%#Eval("ERSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>--%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="修改規則" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                   
                     <a href='./EventRoleDetail.aspx?sno=<%#Eval("ERSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
               
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <a href='./EventRole_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("ERSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick="btnDEL_Click" CommandArgument='<%# Eval("ERSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>

                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />

    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
</asp:Content>

