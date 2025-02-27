<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="CertificateType_AE.aspx.cs" Inherits="Mgt_CertificateType_AE" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        .auto-style1 {
            width: 285px;
        }
    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />



    <table>
        <tr>
            <th class="auto-style1"><i class="fa fa-star"></i>證書名稱</th>
            <td>
                <asp:Label ID="Label1" runat="server" Text="最多50字元" class="w10"></asp:Label>
                <br />
                <asp:TextBox ID="txt_CertificateType" class="required" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
<%--        <tr>
            <th class="auto-style1"><i class="fa fa-star"></i>證書適用人員</th>
            <td>
                <asp:DropDownList ID="ddl_Role" runat="server" DataValueField="RoleSNO" DataTextField="RoleName"></asp:DropDownList>
            </td>
        </tr>--%>
        <tr>
            <th><i class="fa fa-star"></i>適用對象</th>
            <td>
                <asp:CheckBoxList ID="cb_Role" class="required" runat="server" RepeatColumns="3" DataTextField="RoleName" DataValueField="RoleSNO" RepeatLayout="Table" />
                <span id="rolemsg"></span>
            </td>
        </tr>
        <tr>
            <th class="auto-style1">證書列印格式</th>
            <td>
                <asp:Label ID="lb_CTypeFile" runat="server" Text="最多20字元"></asp:Label>
                <br />
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </td>
        </tr>
        <tr>
            <th ><i class="fa fa-star"></i>證書字號</th>
            <td>
                <asp:Label ID="lbl_CTypeString" runat="server" Text="最多20字元"></asp:Label>
                <br />
                <asp:TextBox ID="txt_CTypeString" class="required" runat="server" MaxLength="20"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <th ><i class="fa fa-star"></i>流水編號</th>
            <td>
                <asp:TextBox ID="txt_CTypeSEQ" class="required" runat="server" onkeyup="this.value=this.value.replace(/\D/g,'')" Width="100" ></asp:TextBox>
            </td>
        </tr>
    </table>


    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click"/>
        <input name="btnCancel" type="button" value="取消" onclick="location.href='CertificateType.aspx';"/>
    </div>

</asp:Content>

