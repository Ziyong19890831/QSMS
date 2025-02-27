<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="QS_Manager.aspx.cs" Inherits="Mgt_QS_Manager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
             document.getElementById("<%=btnPage.ClientID%>").click();
        }
        function _goPage1(pageNumber) {
            document.getElementById("<%=txt_EPage.ClientID%>").value = pageNumber;
            document.getElementById("<%=btn_EPage.ClientID%>").click();
        }
        $(function () {
            $(".datepicker").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
        $(function () {
            $("#ContentPlaceHolder1_txt_IntegralS").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
        $(function () {
            $("#ContentPlaceHolder1_txt_IntegralE").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
        $(function () {
            $("#ContentPlaceHolder1_txt_CertificateS").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
        $(function () {
            $("#ContentPlaceHolder1_txt_CertificateE").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
        $(document).ready(function () {
            var tabIndex = $("#<%= hidCurrentTab.ClientID %>").val();
              $("#tabs").tabs({ select: function (event, ui) {
                  $("#<%= hidCurrentTab.ClientID %>").val(ui.index);
              }
                  , selected: tabIndex
              });
        });
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path txtS mb20">現在位置：<a href="#">證書課程管理</a> <i class="fa fa-angle-right"></i><a href="QS_Manager.aspx">學分管理</a></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="hidCurrentTab" runat="server" />
    <div id="tabs" style="margin-bottom: 10px;">
        <ul>
            <li><a href="#tabs-1">線上課程積分</a></li>
            <li><a href="#tabs-2">積分上傳</a></li>
        </ul>

        <div id="tabs-1">
            <asp:UpdatePanel ID="Up1" runat="server">
                <ContentTemplate>
                    <div class="both mb20">
                        <fieldset>
                            <legend>功能列</legend>
                            <div class="left w10">
                                學員名稱<asp:TextBox ID="txt_Person" runat="server"></asp:TextBox>
                                身分證<asp:TextBox ID="txt_PersonID" runat="server"></asp:TextBox>
                                課程名稱<asp:TextBox ID="txt_CourseName" runat="server"></asp:TextBox><br />
                                積分取得日期<asp:TextBox ID="txt_IntegralS" runat="server"></asp:TextBox>~<asp:TextBox ID="txt_IntegralE" runat="server"></asp:TextBox><br />
                                證書到期日<asp:TextBox ID="txt_CertificateS" runat="server"></asp:TextBox>~<asp:TextBox ID="txt_CertificateE" runat="server"></asp:TextBox><br />
                                <div class="right">
                                    <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                                    <asp:Button ID="btnExport" runat="server" Text="匯出" OnClick="btnExport_Click" />

                                </div>
                            </div>

                        </fieldset>
                    </div>
                    <asp:GridView ID="gv_EManager" runat="server" AutoGenerateColumns="False" OnRowCreated="gv_LearningRecord_RowCreated">
                        <Columns>
                            <asp:BoundField HeaderText="序號" DataField="ROW_NO">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="ISNO" DataField="ISNO" />
                            <asp:BoundField HeaderText="身份證號" DataField="PersonID_encryption" />
                            <asp:BoundField HeaderText="姓名" DataField="PName" />
                            <asp:TemplateField HeaderText="學分">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_Integral" BackColor="DarkGray" runat="server" Text='<%# Eval("Integral").ToString()%>' Width="50" Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="課程名稱" DataField="CourseName" />
                            <asp:BoundField HeaderText="證書名稱" DataField="CTypeName" />
                            <asp:TemplateField HeaderText="編輯">
                                <ItemTemplate>
                                    <asp:Button ID="btn_Update" runat="server" Text="編輯" OnClick="btn_Update_Click" CommandName="getdata" />
                                    <asp:Button ID="btn_Update_1" runat="server" Text="更新" OnClick="btn_Update_1_Click" Visible="false" CommandName="getdata" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="刪除">
                                <ItemTemplate>
                                    <asp:LinkButton ID='btnEDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick="btnEDEL_Click" CommandArgument='<%# Eval("ISNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Literal ID="ltl_PageNumberss" runat="server"></asp:Literal>
                    <asp:HiddenField ID="txt_Page" runat="server" />
                    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="gv_EManager" />
                </Triggers>
            </asp:UpdatePanel>

        </div>
        <div id="tabs-2">
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
                                <asp:Label ID="lb_Count" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                            <div class="right">
                                <asp:Button ID="btn_ESearch" runat="server" OnClick="btn_ESearch_Click" Text="查詢" />
                                <asp:Button ID="btn_EExport" runat="server" OnClick="btn_EExport_Click" Text="匯出" />
                            </div>
                        </fieldset>
                    </div>
                    <asp:GridView ID="gv_Elearning" runat="server" AutoGenerateColumns="false" OnRowCreated="gv_Elearning_RowCreated">
                        <Columns>
                            <asp:BoundField HeaderText="姓名" DataField="Pname" />
                            <asp:BoundField HeaderText="身份證" DataField="PersonID_encryption" />
                            <asp:BoundField HeaderText="課程名稱" DataField="CourseName" />
                            <asp:TemplateField HeaderText="學分">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_EIntegral" BackColor="DarkGray" runat="server" Text='<%# Eval("Integral").ToString()%>' Width="50" Enabled="false"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="學分取得時間" DataField="CreateDT" />
                            <asp:BoundField HeaderText="是否使用" DataField="IsUsed" />
                            <asp:TemplateField HeaderText="編輯">
                                <ItemTemplate>
                                    <asp:Button ID="btn_EUpdate" runat="server" OnClick="btn_EUpdate_Click" Text="編輯" CommandName="getdata" />
                                    <asp:Button ID="btn_EUpdate_1" runat="server" OnClick="btn_EUpdate_1_Click" Text="更新" Visible="false" CommandName="getdata" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="刪除">
                                <ItemTemplate>
                                    <asp:LinkButton ID='btnEDEL_1' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick="btnEDEL_1_Click" CommandArgument='<%# Eval("ISNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Literal ID="ltl_EPageNumber" runat="server"></asp:Literal>
                    <asp:HiddenField ID="txt_EPage" runat="server" />
                    <asp:Button ID="btn_EPage" runat="server" Text="查詢" OnClick="btn_EPage_Click" Style="display: none;" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="gv_Elearning" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>


</asp:Content>


