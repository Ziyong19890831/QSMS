<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="CertificateAudit_AE_Review.aspx.cs" Inherits="Mgt_CertificateAudit_AE_Review" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="path txtS mb20">現在位置：<a href="#">證書/積分管理</a> <i class="fa fa-angle-right"></i><a href="#">證書審核</a></div>

    <div class="left mb20">
        學員名稱：<asp:Label ID="lbl_Pname" runat="server" Text="Label"></asp:Label><br />
        可取得的對應證書：<asp:Label ID="lbl_CTypeName" runat="server" Text="Label"></asp:Label><br />
        證號：<asp:TextBox ID="txt_CertID" runat="server"></asp:TextBox>
    </div>
  
    <asp:GridView ID="gv_Cerificate_Review" runat="server" AutoGenerateColumns="False" Font-Size="14px">
        <Columns>
            <asp:BoundField HeaderText="課程名稱" DataField="CourseName"></asp:BoundField>
            <asp:BoundField HeaderText="已取得" DataField="PClassTotalHr"></asp:BoundField>
            <asp:BoundField HeaderText="總積分" DataField="Phours"></asp:BoundField>
        </Columns>
    </asp:GridView>

    <div class="center btns">
        <input name="btnInsert" id="btnInsert" runat="server" onserverclick="btnInsert_ServerClick" type="button" value="授予" />
        <input name="btnCancel" type="button" value="關閉" onclick="window.close();" />
    </div>
</asp:Content>

