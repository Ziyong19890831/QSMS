<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/Dialog.master" CodeFile="UserAPSAudit_AE.aspx.cs" Inherits="Mgt_UserAPSAudit_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="path txtS mb20">現在位置：<a href="#">帳號管理</a> <i class="fa fa-angle-right"></i>帳號申請審核</div>

    <table style="font-size:14px;">
        <tr>
            <th class="w3"></th>
            <th class="w3">申請資料</th>
            <th class="w4">醫事人員資料庫</th>
        </tr>
        <tr>
            <th>帳號</th>
            <td colspan="2"><asp:Label ID="lbl_PAccount" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <th>申請時間/更新時間</th>
            <td><asp:Label ID="lbl_CreateDate" runat="server"></asp:Label></td>
            <td><asp:Label ID="lbr_ModifyDate" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <th>單位代碼</th>
            <td><asp:Label ID="lbl_OrganCode" runat="server"></asp:Label></td>
            <td><asp:Label ID="lbr_OrganCode" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <th>單位名稱</th>
            <td><asp:Label ID="lbl_OrganName" runat="server"></asp:Label></td>
            <td><asp:Label ID="lbr_OrganName" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <th>學員名稱</th>
            <td><asp:Label ID="lbl_PName" runat="server"></asp:Label></td>
            <td><asp:Label ID="lbr_PName" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <th>使用者角色/職業</th>
            <td><asp:Label ID="lbl_RoleName" runat="server"></asp:Label></td>
            <td><asp:Label ID="lbr_JType" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <th>證書字號</th>
            <td>-</td>
            <td><asp:Label ID="lbr_JCN" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <th>證書核發日期</th>
            <td>-</td>
            <td><asp:Label ID="lbr_JDate" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <th>有效期間</th>
            <td>-</td>
            <td><asp:Label ID="lbr_VDate" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>變更學員狀態</th>
            <td colspan="2">
                <asp:DropDownList ID="ddl_Status" class="required" runat="server" DataValueField="MStatusSNO" DataTextField="MName"></asp:DropDownList>
            </td>
        </tr>
    </table>

    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="儲存變更" OnClick="btnOK_Click" />
        <input name="btnCancel" type="button" value="取消" onclick="window.close();" />
    </div>
  

</asp:Content>
