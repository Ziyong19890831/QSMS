<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/Dialog.master" CodeFile="CertificateAudit_AE.aspx.cs" Inherits="Mgt_CertificateAudit_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="path txtS mb20">現在位置：<a href="#">證書/積分管理</a> <i class="fa fa-angle-right"></i><a href="#">證書審核</a></div>

    <div class="left mb20">
        學員名稱：<asp:Label ID="lbl_Pname" runat="server" Text="Label"></asp:Label><br />
<%--        已完成的課程規劃：<asp:Label ID="lbl_PlanName" runat="server" Text="Label"></asp:Label><br />
        可取得的對應證書：<asp:Label ID="lbl_CTypeName" runat="server" Text="Label"></asp:Label>--%>
    </div>
  
    <asp:GridView ID="gv_Cerificate" runat="server" AutoGenerateColumns="False" Font-Size="14px">
        <Columns>
            <%--<asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-CssClass="center" />--%>
            <asp:BoundField HeaderText="課程規劃名稱" DataField="PlanName"></asp:BoundField>
            <asp:BoundField HeaderText="適用年度(起)" DataField="CStartYear"></asp:BoundField>
            <asp:BoundField HeaderText="適用年度(迄)" DataField="CEndYear"></asp:BoundField>
            <asp:BoundField HeaderText="可取得的證書" DataField="CTypeName"></asp:BoundField>
            <asp:BoundField HeaderText="已取得" DataField="PClassTotalHr"></asp:BoundField>
            <asp:BoundField HeaderText="證書總積分" DataField="sumHours"></asp:BoundField>
             <asp:BoundField HeaderText="目標積分" DataField="TargetIntegral"></asp:BoundField>
            

        </Columns>
    </asp:GridView>

    <div class="center btns">
        <input name="btnCancel" type="button" value="關閉" onclick="window.close();" />
    </div>

</asp:Content>
