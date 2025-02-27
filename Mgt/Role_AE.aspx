<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Role_AE.aspx.cs" Inherits="Mgt_Role_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_RoleID" runat="server" />
    <table>
        <tr>
            <th><i class="fa fa-star"></i>角色名稱</th>
            <td>
                <asp:Label ID="lbl_RoleName" runat="server" Text="最多50字元"></asp:Label><br />
                <asp:TextBox ID="txt_RoleName" runat="server" MaxLength="50" class="w10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>角色層級</th>
            <td>
                <asp:Label ID="Label1" runat="server" Text="請輸入數字(數字越小，權限越大)"></asp:Label><br />
                <asp:TextBox ID="txt_RoleLevel" runat="server" MaxLength="3" CssClass="number"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>角色群組</th>
            <td>
                <asp:Label ID="Label2" runat="server" Text="請輸入數字(相同數字代表同一群組)"></asp:Label><br />
                <asp:TextBox ID="txt_RoleGroup" runat="server" MaxLength="3" CssClass="number"></asp:TextBox>
            </td>
        </tr>  
        
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

        <tr>
            <th><i class="fa fa-star"></i>是否為管理者</th>
            <td>
                <asp:DropDownList ID="ddl_Admin" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_Admin_SelectedIndexChanged">
                    <asp:ListItem Value="0">否(一般學員)</asp:ListItem>
                    <asp:ListItem Value="1">是(有管理權限)</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr runat="server" id="tr_RoleOrganType" visible="false">
            <th><i class="fa fa-star"></i>角色單位分類</th>
            <td>
                <asp:DropDownList ID="ddl_RoleOrganType" runat="server" DataValueField="PVal" DataTextField="MVal"></asp:DropDownList>
                <%--<asp:DropDownList ID="ddl_OrganType" runat="server" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                    <asp:ListItem Value="">無管理權限</asp:ListItem>
                    <asp:ListItem Value="S">S:向下管理</asp:ListItem>
                    <asp:ListItem Value="A">A:縣市管理</asp:ListItem>
                    <asp:ListItem Value="B">B:區域管理</asp:ListItem>
                    <asp:ListItem Value="U">U:協會管理</asp:ListItem>
                </asp:DropDownList>--%><br />
                <span>
                    S:[向下管理]的依據為比自己[角色層級]還要大的角色帳號<br />
                    A:[縣市管理]管理的依據為相同的Organ.AreaCodeA<br />
                    B:[區域管理]管理的依據為相同的Organ.AreaCodeB<br />
                    U:[協會管理]僅能管與自己相同RoleGroup的一般帳號
                    無管理權限:一般學員或學員
                </span>
            </td>
        </tr>

    </ContentTemplate>
</asp:UpdatePanel>           

    </table>

    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click"/>
        <input name="btnCancel" type="button" value="取消" onclick="location.href = 'Role.aspx';"/>
    </div>

</asp:Content>

