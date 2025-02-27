<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="QS_EManager.aspx.cs" Inherits="Mgt_QS_EManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_EPage.ClientID%>").value = pageNumber;
            document.getElementById("<%=btn_EPage.ClientID%>").click();
        }
        function _goPage1(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
        $(function () {
            $(".datepicker").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
        $(function () {
            $("#ContentPlaceHolder1_txt_OnlineDateS").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
        $(function () {
            $("#ContentPlaceHolder1_txt_OnlineDateE").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
        
    </script>
    <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();

        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path txtS mb20">現在位置：<a href="#">繼續教育管理</a> <i class="fa fa-angle-right"></i><a href="QS_EManager.aspx">繼續教育學分管理</a></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="tabs" style="margin-bottom: 10px;">
        <ul>
            <li><a href="#tabs-1">繼續教育線上課程積分</a></li>
            <li><a href="#tabs-2">繼續教育積分上傳</a></li>
        </ul>
        <div id="tabs-1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="both mb20">
                        <fieldset>
                            <legend>功能列</legend>
                            <div class="left w8">
                                學員名稱<asp:TextBox ID="txt_EPname" runat="server"></asp:TextBox>
                                身分證<asp:TextBox ID="txt_EPersonID" runat="server"></asp:TextBox>
                                課程名稱<asp:TextBox ID="txt_ECourseName" runat="server"></asp:TextBox><br />
                                學分取得時間<asp:TextBox ID="txt_OnlineDateS" runat="server"></asp:TextBox>～<asp:TextBox ID="txt_OnlineDateE" runat="server"></asp:TextBox>
                               
                            </div>
                            <div class="right">
                                <asp:Button ID="btn_ESearch" runat="server" Text="查詢" OnClick="btn_ESearch_Click" />
                                <asp:Button ID="btn_EExport" runat="server" Text="匯出" OnClick="btn_EExport_Click" />
                            </div>
                        </fieldset>
                    </div>
                    <asp:GridView ID="gv_Elearning" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="姓名" DataField="Pname" />
                            <asp:BoundField HeaderText="身份證" DataField="PersonID_encryption" />
                            <asp:BoundField HeaderText="課程名稱" DataField="CourseName" />
                            <asp:BoundField HeaderText="學分" DataField="學分" />
                            <asp:BoundField HeaderText="學分取得時間" DataField="CreateDT" />
                            <asp:BoundField HeaderText="是否使用" DataField="IsUsed" />
                        </Columns>
                    </asp:GridView>
                    <asp:Literal ID="ltl_EPageNumber" runat="server"></asp:Literal>
                    <asp:HiddenField ID="txt_EPage" runat="server" />
                    <asp:Button ID="btn_EPage" runat="server" Text="查詢" OnClick="btn_EPage_Click" Style="display: none;" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div id="tabs-2">
                <asp:UpdatePanel ID="Up1" runat="server">
                    <ContentTemplate>
                        <div class="both mb20">
                            <fieldset>
                                <legend>功能列</legend>
                                <div class="left w8">
                                    學員名稱<asp:TextBox ID="txt_Person" runat="server"></asp:TextBox>
                                    身分證<asp:TextBox ID="txt_PersonID" runat="server"></asp:TextBox>
                                    課程名稱<asp:TextBox ID="txt_CourseName" runat="server"></asp:TextBox><br />
                                   
                                </div>
                                <div class="right">
                                    <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                                    <asp:Button ID="btnExport" runat="server" Text="匯出" OnClick="btnExport_Click" />
                                </div>
                            </fieldset>
                        </div>
                        <asp:GridView ID="gv_EManager" runat="server" AutoGenerateColumns="False" OnRowCreated="gv_LearningRecord_RowCreated">
                            <Columns>
                                <asp:BoundField HeaderText="序號" DataField="ROW_NO">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="EISNO" DataField="EISNO" />
                                <asp:BoundField HeaderText="身份證號" DataField="PersonID_encryption" />
                                <asp:BoundField HeaderText="姓名" DataField="PName" />
                                <asp:BoundField HeaderText="課程名稱" DataField="CourseName" />
                                <asp:BoundField HeaderText="證書名稱" DataField="CTypeName" />
                                <asp:BoundField HeaderText="日期" DataField="CDate" />
                                <asp:TemplateField HeaderText="學分">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Integral" BackColor="DarkGray" runat="server" Text='<%# Eval("Integral").ToString()%>' Width="50" Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="類別" DataField="CType" />
                                <asp:BoundField HeaderText="是否有效" DataField="effective" />
                                <asp:BoundField HeaderText="是否使用" DataField="isUsedStatus" />
                                <asp:BoundField HeaderText="證書到期日" DataField="CertEndDate" />
                                <asp:TemplateField HeaderText="編輯">
                                    <ItemTemplate>
                                        <asp:Button ID="btn_Update" runat="server" Text="編輯" OnClick="btn_Update_Click" CommandName="getdata" />
                                        <asp:Button ID="btn_Update_1" runat="server" Text="更新" OnClick="btn_Update_1_Click" Visible="false" CommandName="getdata" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:Literal ID="ltl_PageNumberss" runat="server"></asp:Literal>
                        <asp:HiddenField ID="txt_Page" runat="server" />
                        <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>

</asp:Content>


