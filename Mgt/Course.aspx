<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Course.aspx.cs" Inherits="Mgt_Course" %>

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

    <div class="path txtS mb20">現在位置：<a href="#">課程管理</a> <i class="fa fa-angle-right"></i><a href="Course.aspx">課程維護</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                課程名稱<asp:TextBox ID="txt_CourseName" runat="server" class="w2"></asp:TextBox>
                <asp:UpdatePanel ID="upl_ddl" runat="server" UpdateMode="Conditional" style="display: inline;">
                    <ContentTemplate>
                        課程類別1<asp:DropDownList ID="ddl_Class1" runat="server" DataValueField="PVal" DataTextField="MVal"></asp:DropDownList>
                      <%--  課程類別2<asp:DropDownList ID="ddl_Class2" runat="server" DataValueField="PVal" DataTextField="MVal"></asp:DropDownList>--%>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <%--單元<asp:TextBox ID="txt_UnitName" runat="server" class="w2"></asp:TextBox>--%>
                授課方式<asp:DropDownList ID="ddl_Ctype" runat="server" DataValueField="PVal" DataTextField="MVal"></asp:DropDownList>
                適用人員<asp:DropDownList ID="ddl_Rolename" runat="server" DataValueField="RoleSNO" DataTextField="RoleName"></asp:DropDownList>
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                <% if (userInfo.AdminIsInsert == true) { %>
                <input name="btnInsert" type="button" value="新增" onclick="location.href = 'Course_AE.aspx?Work=N'" />
                <% } %>
            </div>
        </fieldset>
    </div>


    <asp:GridView ID="gv_Course" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField HeaderText="序號" DataField="ROW_NO" />
            <asp:BoundField HeaderText="課程類別1" DataField="Class1" />
            <asp:BoundField HeaderText="適用人員" DataField="RoleName" />
            <%--<asp:BoundField HeaderText="單元" DataField="UnitName" />--%>
            <asp:BoundField HeaderText="課程名稱" DataField="CourseName">
                <ItemStyle Width="500px" />
            </asp:BoundField>
            <asp:BoundField HeaderText="授課方式" DataField="Ctype" />
            <asp:BoundField HeaderText="時數" DataField="CHour" />
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsUpdate == true) { %>
                    <a href='./Course_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("CourseSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
                    <% } else { %> 
                        無權限 
                    <% } %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsDelete == true) { %>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("CourseSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                    <% } else { %> 
                        無權限 
                    <% } %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />

    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>

