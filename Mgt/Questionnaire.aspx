﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Questionnaire.aspx.cs" Inherits="Mgt_Questionnaire" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
        $(function () {
            $(".datepicker").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="path txtS mb20">現在位置：<a href="#">報表作業</a> <i class="fa fa-angle-right"></i><a href="Questionnaire.aspx">滿意度統計報表</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                學員名稱<asp:TextBox ID="txt_Person" runat="server"></asp:TextBox>
                身分證<asp:TextBox ID="txt_PersonID" runat="server"></asp:TextBox>
                課程名稱<asp:TextBox ID="txt_CourseName" runat="server"></asp:TextBox>
                <br />
                完成日期
                <asp:TextBox ID="txt_SFinishedDate" class="datepicker" runat="server" type="text"></asp:TextBox> - 
                <asp:TextBox ID="txt_EFinishedDate" class="datepicker" runat="server" type="text"></asp:TextBox>
            </div>
            <div class="right">
                 <%--<asp:Button ID="btn_ExportDetail" OnClick="btn_ExportDetail_Click" runat="server" Text="填寫結果下載"  />--%>
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                <asp:Button ID="btnExport" runat="server" Text="匯出" OnClick="btnExport_Click" />
            </div>
        </fieldset>
    </div>


    <asp:GridView ID="gv_LearningRecord" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField HeaderText="序號" DataField="ROW_NO">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField HeaderText="E-Learning課程名稱" DataField="ELSName" />
            <asp:BoundField HeaderText="身分證" DataField="PersonID_encryption" />
            <asp:BoundField HeaderText="填寫人姓名" DataField="PName" />
            <asp:BoundField HeaderText="完成日期" DataField="CompletedDate" />
            <asp:TemplateField>
               <ItemTemplate>
                    <a href="#" onclick="var winvar = window.open('./QuestionnaireDetail.aspx?sno=<%#Eval("PersonSNO").ToString() %>&esno=<%# Eval("ELScode") %>','winname','width=1200 height=550 location=no,menubar=no status=no,toolbar=no');"><i class="fa fa-pen-square"></i>詳細資料</a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>

