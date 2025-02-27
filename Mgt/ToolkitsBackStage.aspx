<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="ToolkitsBackStage.aspx.cs" Inherits="Mgt_ToolkitsBackStage" %>

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

    <div class="path txtS mb20">現在位置：<a href="#">系統功能管理</a> <i class="fa fa-angle-right"></i><a href="ToolkitsBackStage.aspx">教材</a></div>

    <div class="both mb20">
       
                <fieldset>
                    <legend>功能列</legend>
                    <div class="left w8">
                        <asp:DropDownList ID="ddl_stageClass" runat="server" DataTextField="MVal" DataValueField="Pval"></asp:DropDownList>
                        <asp:DropDownList ID="ddl_stage" runat="server" DataTextField="MVal" DataValueField="Pval"></asp:DropDownList>
                        <asp:DropDownList ID="ddl_TkType" runat="server" DataTextField="MVal" DataValueField="Pval"></asp:DropDownList>
                        檔案名稱<asp:TextBox ID="txt_fileName" runat="server"></asp:TextBox>
                    </div>
                    <div class="right">
                        <asp:Button ID="btn_search" runat="server" OnClick="btn_search_Click" Text="查詢" />
                         <asp:Button ID="btnExport" runat="server" Text="匯出" OnClick="btnExport_Click" />
                        <input name="btnInsert" type="button" value="新增" onclick="location.href = 'ToolkitsBackStage_AE.aspx?Work=N'" />
                    </div>
                </fieldset>
                </div>
            

    <asp:GridView ID="gv_Course" runat="server" AutoGenerateColumns="False" OnRowCreated="gv_Course_RowCreated">
        <Columns>

            <asp:BoundField HeaderText="TkURL" DataField="TkURL" />
            <asp:ImageField HeaderText="預覽" DataImageUrlField="TkURL_pic" ItemStyle-Width="100" ControlStyle-Width="100" ControlStyle-Height="120">
            </asp:ImageField>
            <asp:BoundField HeaderText="適用性" DataField="stageClassName" />
            <asp:BoundField HeaderText="期別" DataField="stageName" />
            <asp:BoundField HeaderText="科別" DataField="TktypeName" />
            <asp:BoundField HeaderText="檔案名稱" DataField="TkName" />
            <asp:BoundField HeaderText="副檔名" DataField="Extension" />
            <asp:BoundField HeaderText="上傳日期" DataField="上傳日期" />
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsUpdate == true)
                        { %>
                    <a href='./ToolkitsBackStage_AE.aspx?TkSNO=<%#Eval("TkSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
                    <% }
                        else
                        { %> 
                        無權限 
                    <% } %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsDelete == true)
                        { %>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("TkSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                    <% }
                        else
                        { %> 
                        無權限 
                    <% } %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:BoundField HeaderText="下載次數" DataField="Dcount" ItemStyle-HorizontalAlign="Center" />
        </Columns>
    </asp:GridView>


                <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
                <asp:HiddenField ID="txt_Page" runat="server" />

                <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
          
  
</asp:Content>

