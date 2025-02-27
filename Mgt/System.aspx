<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="System.aspx.cs" Inherits="Mgt_System" %>

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


    <div class="path txtS mb20">現在位置：<a href="#">系統功能管理</a> <i class="fa fa-angle-right"></i><a href="System.aspx">系統代碼設定管理</a></div>
    <%--<wuc:uc1 runat="server" />--%>
    <div class="both mb20">
        <div class="left mb20">
            系統名稱
                        <asp:TextBox ID="txt_SysName" runat="server"></asp:TextBox>
            系統代碼
                        <asp:TextBox ID="txt_SysID" runat="server" type="text"></asp:TextBox>
        </div>
        <div class="right">

            <input name="btnSearch" type="button" value="查詢" runat="server" onserverclick="btnSearch_Click" />
            <%
                if (userInfo.AdminIsInsert == true)
                {
            %>
            <input name="btnInsert" type="button" value="新增" onclick="location.href = 'System_AE.aspx?Work=N'" />
            <%   
                }
            %>
        </div>
    </div>

    <asp:GridView ID="gv_Notice" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="SYSTEM_ID" HeaderText="系統編號" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="SYSTEM_NAME" HeaderText="系統名稱" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="SYSTEM_INFO" HeaderText="系統簡述" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="建檔日期時間" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lbl_Cdate" runat="server" Text='<%#  Eval("CreateDT")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="CUNAME" HeaderText="建檔人員" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="修改日期時間" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lbl_Edate" runat="server" Text='<%# Eval("ModifyDT")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="MUNAME" HeaderText="修改人員" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="是否啟用" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# ((int)Eval("ISEnable") < 1) ? "停權" : "啟用" %>'></asp:Label>
                </ItemTemplate>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <%
                        if (userInfo.AdminIsUpdate == true)
                        {
                    %>
                    <a href='./System_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("SYSTEMSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
                    <%   
                        }
                        else
                        {
                    %>
                         無權限
                        <% 
                            }
                        %>
                </ItemTemplate>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <%
                        if (userInfo.AdminIsDelete == true)
                        {
                    %>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("SYSTEMSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                    <%   
                        }
                        else
                        {
                    %>
                         無權限
                        <% 
                            }
                        %>
                </ItemTemplate>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>

    <div style="margin-top: 50px">
        <asp:Label ID="lbl_msg" Visible="false" runat="server" Text="查無資料..."></asp:Label>
    </div>
    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    
    
    
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />


</asp:Content>

