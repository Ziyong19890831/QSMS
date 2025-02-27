﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Personnel.aspx.cs" Inherits="Mgt_Personnel" %>

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

    <div class="path txtS mb20">現在位置：<a href="#">帳號管理</a> <i class="fa fa-angle-right"></i><a href="Personnel.aspx">學員帳號設定</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                <% if (userInfo.RoleOrganType == "S") { %>
                <asp:UpdatePanel ID="upl_ddl" runat="server" UpdateMode="Conditional" style="display: inline;">
                    <ContentTemplate>
                        行政區
                        <asp:DropDownList ID="ddl_AreaCodeA" runat="server" OnSelectedIndexChanged="ddl_AreaCodeA_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddl_AreaCodeB" runat="server" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                            <asp:ListItem Value="" Text="請先選擇縣市行政區"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddl_AreaCodeA" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                單位代碼<asp:TextBox ID="txt_OrganCode" runat="server" class="w1"></asp:TextBox>
                單位名稱<asp:TextBox ID="txt_OrganName" runat="server" class="w1"></asp:TextBox>
                <br />
                <% } %>
                角色<asp:DropDownList ID="ddl_Role" runat="server" DataValueField="RoleSNO" DataTextField="RoleName"></asp:DropDownList>
                帳號<asp:TextBox ID="txt_PAccount" runat="server" class="w1"></asp:TextBox>
                姓名<asp:TextBox ID="txt_PName" runat="server" class="w1"></asp:TextBox>
                身分證<asp:TextBox ID="txt_PersonID" runat="server" class="w1"></asp:TextBox>
                <br />
                E-mail<asp:TextBox ID="txt_mail" runat="server" class="w5"></asp:TextBox><br />
                服務單位科別<asp:DropDownList ID="ddl_TSSNO" DataTextField="TsTypeName" DataValueField="TSSNO" runat="server"></asp:DropDownList>
                學員狀態 <asp:DropDownList ID="ddl_Status" runat="server" DataTextField="MName" DataValueField="MStatusSNO"></asp:DropDownList>
            </div>
            <div class="right mt-10">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click"/>
                <% if (userInfo.AdminIsInsert == true) { %>
                <input name="btnInsert" type="button" value="新增學員" onclick="location.href = 'Personnel_AE.aspx?Work=N'"/>
                <% } %>
            </div>
        </fieldset>
    </div>

    <asp:GridView ID="gv_Person" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="OrganCode" HeaderText="單位代碼" />
            <asp:BoundField DataField="AreaName" HeaderText="行政區" />
            <asp:BoundField DataField="OrganName" HeaderText="單位名稱" HeaderStyle-Width="155px"/>
            <asp:BoundField DataField="RoleName" HeaderText="使用者角色" />
            <asp:BoundField DataField="PAccount" HeaderText="使用者帳號" />
            <asp:BoundField DataField="PName" HeaderText="使用者姓名" />
            <%--<asp:BoundField DataField="PTel" HeaderText="聯絡電話" />--%>
            <asp:BoundField DataField="PMail" HeaderText="電子郵件" HeaderStyle-Width="100px" />
            <asp:BoundField DataField="PStatus" HeaderText="帳號狀態" />
            <asp:BoundField DataField="MName" HeaderText="學員狀態" />
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if(userInfo.AdminIsUpdate == true) { %>
                    <a href='./Personnel_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("PersonSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
                    <% } else { %>
                        無權限
                    <% } %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsDelete == true) { %>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("PersonSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                    <% } else { %>
                        無權限
                    <% } %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>

    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:HiddenField ID="txt_EditRole" runat="server" />
    <asp:HiddenField ID="txt_AddRole" runat="server" />
    <asp:HiddenField ID="txt_DelRole" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>

