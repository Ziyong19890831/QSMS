<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="ExperienceManager_AE.aspx.cs" Inherits="Mgt_ExperienceManager_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <div class="path txtS mb20">現在位置：<a href="#">常用服務管理</a> <i class="fa fa-angle-right"></i><a href="ExperienceManager.aspx">師資經歷管理</a><i class="fa fa-angle-right"></i><a href="ExperienceManager_AE.aspx">師資經歷管理-新增</a></div>
    <asp:ScriptManager ID="SC" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="Up" runat="server">
        <ContentTemplate>
            <table>
                <thead>
                    <tr>
                        <th>方案師資身分</th>
                        <td>
                            <asp:DropDownList ID="ddl_TrainRoleType" runat="server" >
                                <asp:ListItem Value="1" Text="指導員" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="2" Text="專業師資"></asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <th>身分證</th>
                        <td>
                            <asp:TextBox ID="txt_PersonID" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>師資類別</th>
                        <td>
                            <asp:DropDownList ID="ddl_TCName" runat="server" AutoPostBack="true" DataTextField="TCName" DataValueField="TrainType" OnSelectedIndexChanged="ddl_TCName_SelectedIndexChanged"></asp:DropDownList></td>
                    </tr>
                   
                    <tr>
                        <th>方案編號</th>
                        <td>
                           <asp:DropDownList ID="ddl_TrainPlanNumber" runat="server" DataTextField="TrainPlan" DataValueField="TrainPlan"></asp:DropDownList>
                    </tr>

                </thead>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="新增" OnClick="btnOK_Click" />
        <input name="btnCancel" type="button" value="取消" onclick="location.href='ExperienceManager.aspx'" />
    </div>
</asp:Content>

