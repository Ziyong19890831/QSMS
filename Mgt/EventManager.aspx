<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="EventManager.aspx.cs" Inherits="Mgt_EventManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

</script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>名單管理</h1>
    <fieldset> 
        <legend>EXCEL-前後測成績上傳</legend>
        Excel檔案:       
                <asp:FileUpload ID="file_Upload" runat="server" />
        <div class="right">
            <asp:Button ID="btnDownload" runat="server" Text="下載格式" OnClick="btnDownload_Click" />
            <br />
            <asp:Button ID="btnUpload" runat="server" Text="上傳" OnClick="btnUpload_Click" />
        </div>
    </fieldset>
    <asp:GridView ID="gv_EventD" runat="server" AutoGenerateColumns="False" Font-Size="14px" DataKeyNames="PersonSNO" OnRowCreated="gv_EventD_RowCreated">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-CssClass="center">
                <ItemStyle CssClass="center"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="學員姓名" SortExpression="CompanyName">
                <ItemTemplate>
                    <a href="#" style="color: blue" onclick="javascript:window.open('ReportMemberDetail.aspx?sno=<%# Eval("PersonSNO") %>','','width=1000,height=500');"><%# Eval("PName") %></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="身分證號" DataField="PersonID" />
            <asp:BoundField HeaderText="PersonSNO" DataField="PersonSNO" />
            <asp:BoundField HeaderText="醫事證號" DataField="JCN" />
            <asp:BoundField HeaderText="角色別" DataField="RoleName" />
            <asp:BoundField HeaderText="手機" DataField="PPhone" />
            <asp:BoundField HeaderText="電話" DataField="PTel" />
            <asp:BoundField HeaderText="信箱" DataField="PMail" />
            <asp:BoundField HeaderText="通知方式" DataField="EventNotice" />
            <asp:BoundField HeaderText="報名時間" DataField="ApplyDT" />
            <asp:TemplateField HeaderText="審核狀態" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:DropDownList ID="ddl_AuditItem" onchange="AdmitResult();" SelectedValue='<%# Bind("EventAuditVal") %>' runat="server" Enabled="false">
                        <asp:ListItem Value="0" Text="未審" />
                        <asp:ListItem Value="1" Text="錄取" />
                        <asp:ListItem Value="2" Text="未錄取" />
                        <asp:ListItem Value="3" Text="審核中" />
                        <asp:ListItem Value="4" Text="備取" />
                        <asp:ListItem Value="5" Text="取消" />
                    </asp:DropDownList>
                </ItemTemplate>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="通知狀態" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <%# (Eval("Notice").ToString() == "True") ? "已通知" : "未通知"%>
                    <asp:HiddenField ID="hid_EventDid" Value='<%# Bind("EventDSNO") %>' runat="server" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="上課紀錄" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:DropDownList ID="ddl_ClassRecordItem" runat="server">
                        <asp:ListItem Value="0" Text="未到" />
                        <asp:ListItem Value="1" Text="有到" />
                    </asp:DropDownList>
                </ItemTemplate>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="前測成績" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:TextBox ID="txt_BGrade" runat="server" Width="35px"></asp:TextBox>
                </ItemTemplate>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="後測成績" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:TextBox ID="txt_AGrade" runat="server" Width="35px"></asp:TextBox>
                </ItemTemplate>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
           <%-- <asp:BoundField HeaderText="學分" DataField="Integral" />
            <asp:BoundField HeaderText="學分狀態" DataField="IsUsed" />--%>
            <%--<asp:TemplateField HeaderText="學分" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:TextBox ID="txt_Integral" runat="server" Width="35px" Enabled="false"></asp:TextBox>
                </ItemTemplate>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>--%>
            <%--<asp:TemplateField HeaderText="學分備註" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:TextBox ID="txt_Note" runat="server" Width="80px" Height="40" TextMode="MultiLine"></asp:TextBox>
                </ItemTemplate>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="審核備註" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:TextBox ID="txt_AuditNote" runat="server" Width="80px" Height="40" TextMode="MultiLine"></asp:TextBox>
                </ItemTemplate>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="修改" OnClick="btnOK_Click" />
        <input name="btnCancel" type="button" value="關閉" onclick="window.close();" />
        <asp:Button ID="Invite_download" runat="server" Text="報名下載" type="button" OnClick="Invite_download_Click" />
    </div>
</asp:Content>

