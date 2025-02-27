<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="TafmHis.aspx.cs" Inherits="Mgt_TifmHis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        $(function () {
            $(".datepicker").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="path txtS mb20">現在位置：<a href="#">報表作業</a> <i class="fa fa-angle-right"></i><a href="Questionnaire.aspx">歷史紀錄查詢</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                主題名稱<asp:TextBox ID="txt_Object" runat="server"></asp:TextBox><br />
                刊物名稱<asp:TextBox ID="txt_Theme" runat="server"></asp:TextBox><br />
                姓名<asp:TextBox ID="txt_PName" runat="server"></asp:TextBox><br /> 
                身分證<asp:TextBox ID="txt_PersonID" runat="server"></asp:TextBox><br />               
                醫師證號<asp:TextBox ID="txt_DCNumber" runat="server"></asp:TextBox><br />
                專科暨訓練證照名稱<asp:TextBox ID="txt_CName" runat="server"></asp:TextBox><br />
                專科暨訓練證照證號<asp:TextBox ID="txt_CNumber" runat="server"></asp:TextBox><br />
                E-Mail<asp:TextBox ID="txt_Email" runat="server"></asp:TextBox><br />
                通訊課程日期
                <asp:TextBox ID="txt_SFinishedDate" class="datepicker" runat="server" type="text"></asp:TextBox> - 
                <asp:TextBox ID="txt_EFinishedDate" class="datepicker" runat="server" type="text"></asp:TextBox>
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
            </div>
        </fieldset>
    </div>


 
</asp:Content>

