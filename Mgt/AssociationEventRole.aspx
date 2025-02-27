﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="AssociationEventRole.aspx.cs" Inherits="Mgt_AssociationEventRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="path txtS mb20">現在位置：<a href="#">系統功能管理</a> <i class="fa fa-angle-right"></i><a href="EventRole.aspx">學會報名管理</a></div>

    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                規則名稱<asp:TextBox ID="txt_RoleName" runat="server" class="w2"></asp:TextBox>
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                <input name="btnInsert" type="button" value="新增" onclick="location.href = 'AssociationEventRole_AE.aspx?Work=N'" />
            </div>
        </fieldset>
    </div>


    <asp:GridView ID="gv_AssociationEventRole" runat="server" AutoGenerateColumns="False" OnRowCreated="gv_AssociationEventRole_RowCreated">
        <Columns>
            <asp:BoundField HeaderText="AERSNO" DataField="AERSNO" />
            <asp:BoundField HeaderText="序號" DataField="ROW_NO" />
            <asp:BoundField HeaderText="課程名稱" DataField="AERName">
                <ItemStyle Width="500px" />
            </asp:BoundField>
            <asp:BoundField HeaderText="課程類別1" DataField="Class1" />                     
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
                   
                     <a href='./AssociationEventRoleDetail.aspx?sno=<%#Eval("AERSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
               
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <a href='./EventRole_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("AERSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick="btnDEL_Click" CommandArgument='<%# Eval("AERSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>

                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />

    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
</asp:Content>

