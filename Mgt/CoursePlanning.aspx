<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="CoursePlanning.aspx.cs" Inherits="Mgt_CoursePlanning" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

 
   <div class="path txtS mb20">現在位置：<a href="#">課程管理</a> <i class="fa fa-angle-right"></i><a href="CoursePlanning.aspx">課程類別及規劃</a></div>
   
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                課程規劃名稱<asp:TextBox ID="txt_PlanName" runat="server" class="w2"></asp:TextBox>
                對應證書<asp:DropDownList ID="ddl_CType" runat="server" DataValueField="CTypeSNO" DataTextField="CTypeName"></asp:DropDownList>
                啟用<asp:DropDownList ID="ddl_IsEnable" runat="server"></asp:DropDownList>
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                <% if (userInfo.AdminIsInsert == true) { %>
                <input name="btnInsert" type="button" value="新增" onclick="location.href = 'CoursePlanning_AE.aspx?Work=N'" />
                <% } %>
            </div>
        </fieldset>
    </div>


    <asp:GridView ID="gv_CourseClass" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="課程規劃名稱" DataField="PlanName" />
            <asp:BoundField HeaderText="適用起迄年度" DataField="CYear" />
            <asp:BoundField HeaderText="啟用" DataField="IsEnables" />
            <asp:BoundField HeaderText="對應證書" DataField="CTypeName" />
            <asp:BoundField HeaderText="總時數" DataField="sumHour" />
            <asp:BoundField HeaderText="課程數" DataField="countCourse" />
            <asp:BoundField HeaderText="目標積分" DataField="TargetIntegral" />
            <asp:BoundField HeaderText="適用對象" DataField="CRole" /> 
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsUpdate == true) { %>
                    <a href='./CoursePlanning_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("PClassSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
                    <% } else { %>
                        無權限
                    <% } %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsDelete == true) { %>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("PClassSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
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

