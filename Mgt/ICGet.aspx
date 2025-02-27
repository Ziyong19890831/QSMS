<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="ICGet.aspx.cs" Inherits="Mgt_ICGet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">
        $(function () {

            $(".w1").datepicker({
                changeYear : true,
                yearRange : "2010:2050",
                dateFormat: 'yy/mm/dd'
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <legend>積分塞入</legend>
        身分證<asp:TextBox ID="txt_PersonID_I" runat="server" ></asp:TextBox>
        課程規劃<asp:DropDownList runat="server" ID="ddl_CoursePlanningClass" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
        類別<asp:DropDownList runat="server" DataTextField="Mval" ID="ddl_Type" DataValueField="Pval"></asp:DropDownList>
        <asp:Button ID="btn_InsertI" runat="server" Text="執行" OnClick="btn_InsertI_Click" />
    </fieldset>
    <fieldset>
        <legend>證書塞入</legend>
        身分證<asp:TextBox ID="txt_PersonID_C" runat="server"></asp:TextBox>
        字號<asp:TextBox ID="txt_CertID" runat="server"></asp:TextBox>
        證書種類<asp:DropDownList runat="server" DataTextField="CtypeName" ID="ddl_Certificate" DataValueField="CTypeSNO"></asp:DropDownList>
        到期日<asp:TextBox runat="server"  ID="CertEnddate" class="w1" ></asp:TextBox>
         <asp:Button ID="btn_InsertC" runat="server" Text="執行" OnClick="btn_InsertC_Click" />
    </fieldset>


</asp:Content>

