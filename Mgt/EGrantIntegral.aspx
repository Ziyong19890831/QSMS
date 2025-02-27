<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="EGrantIntegral.aspx.cs" Inherits="Mgt_EGrantIntegral" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="path txtS mb20">現在位置：<a href="#">系統功能管理</a> <i class="fa fa-angle-right"></i><a href="RecordLog.aspx">繼續教育積分授予</a></div>

    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                學員名稱<asp:TextBox ID="txt_Person" runat="server"></asp:TextBox>
                身分證<asp:TextBox ID="txt_PersonID" runat="server"></asp:TextBox><br />
                課程名稱<asp:TextBox ID="txt_CourseName" runat="server" Width="300px"></asp:TextBox><br />
                證書類型<asp:DropDownList ID="ddl_CtypeName" runat="server" DataTextField="CTypeName" DataValueField="CTypeSNO"></asp:DropDownList>
            </div>
            <div class="right">
                <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
            </div>
        </fieldset>
    </div>

    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gv_LearningRecord" runat="server" AutoPostback="Flash" AutoGenerateColumns="False" OnRowCommand="gv_LearningRecord_RowCommand" OnRowCreated="gv_LearningRecord_RowCreated" OnRowDataBound="gv_LearningRecord_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="序號" DataField="ROW_NO">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CourseSNO" />
                    <asp:BoundField HeaderText="學員名稱" DataField="PName" />
                    <asp:BoundField DataField="PersonSNO" />
                    <asp:BoundField HeaderText="身分證" DataField="PersonID_encryption" />
                    <asp:BoundField HeaderText="證書類型" DataField="CTypeName" />
                    <asp:BoundField HeaderText="課程名稱" DataField="CourseName" />
                    <asp:BoundField HeaderText="測驗" DataField="Exam" />
                    <asp:BoundField HeaderText="滿意度" DataField="Feedback" />
                    <asp:BoundField HeaderText="觀看紀錄" DataField="Record" />
                    <asp:TemplateField HeaderText="授予積分" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>


                            <%--<asp:LinkButton ID="Btn_Grant" runat="server" Text='<%# Bind("ISNO") %>'   onclick="Btn_Grant_Click" CommandName="getdata" ></asp:LinkButton>--%>
                            <asp:Button ID="Btn_Grant" runat="server" Text='<%# Bind("ISNO") %>' OnClick="Btn_Grant_Click" CommandName="getdata" />

                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
            <asp:HiddenField ID="txt_Page" runat="server" />
            <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
        </ContentTemplate>
       
    </asp:UpdatePanel>

</asp:Content>

