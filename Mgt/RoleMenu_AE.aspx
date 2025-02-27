<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="RoleMenu_AE.aspx.cs" Inherits="Mgt_RoleMenu_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function CheckAllCheckBox(obj, name) {
            var elements = document.forms[0].elements;
            for (var i = 0; i < elements.length; i++) {
                if (elements[i].type == 'checkbox') {
                    if (elements[i].name.indexOf(name) > -1) {
                        elements[i].checked = obj.checked;
                    }
                }
            }
        }
    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_RoleID" runat="server" />

    角色名稱：<asp:Label ID="lbl_RoleName" runat="server" Text=""></asp:Label><br />

    <asp:GridView ID="gv_RoleMenuAe" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField Visible="False">
                <ItemTemplate>
                    <asp:Label ID="PLINKSNO" runat="server" Text='<%# Eval("PLINKSNO")%>'></asp:Label>
                </ItemTemplate>
                <HeaderTemplate>
                    PLINKSNO
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField Visible="False">
                <ItemTemplate>
                    <asp:Label ID="PPLINKSNO" runat="server" Text='<%# Eval("PPLINKSNO")%>'></asp:Label>
                </ItemTemplate>
                <HeaderTemplate>
                    PPLINKSNO
                </HeaderTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <%# (Eval("ISDIR").ToString() == "1")?Eval("PLINKNAME").ToString():""%>
                </ItemTemplate>
                <HeaderTemplate>
                    資料夾名稱
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <%# (Eval("ISDIR").ToString() == "1")?"":Eval("PLINKNAME").ToString()%>
                </ItemTemplate>
                <HeaderTemplate>
                    頁面名稱
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <%# (Eval("ISENABLE").ToString() == "1")?"啟用":"隱藏"%>
                </ItemTemplate>
                <HeaderTemplate>狀態</HeaderTemplate>
                <ItemStyle HorizontalAlign="center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="檢視" ItemStyle-HorizontalAlign="center">
                <HeaderTemplate>
                    <asp:CheckBox ID="CheckAllview" runat="server" onclick="javascript: CheckAllCheckBox(this,'chkISVIEW');" Text="檢視" ToolTip="按一次全選，再按一次取消全選" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkISVIEW" runat="server" Checked='<%# (Eval("ISVIEW").ToString() == "1")?true:false%>'></asp:CheckBox>
                    <font size="2">視</font>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="新增" ItemStyle-HorizontalAlign="center">
                <HeaderTemplate>
                    <asp:CheckBox ID="CheckAllUPDATE" runat="server" onclick="javascript: CheckAllCheckBox(this,'chkISUPDATE');" Text="修改" ToolTip="按一次全選，再按一次取消全選" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkISUPDATE" runat="server" Checked='<%# (Eval("ISUPDATE").ToString() == "1")?true:false%>'></asp:CheckBox>
                    <font size="2">修</font>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="center">
                <HeaderTemplate>
                    <asp:CheckBox ID="CheckAllINSERT" runat="server" onclick="javascript: CheckAllCheckBox(this,'chkISINSERT');" Text="新增" ToolTip="按一次全選，再按一次取消全選" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkISINSERT" runat="server" Checked='<%# (Eval("ISINSERT").ToString() == "1")?true:false%>'></asp:CheckBox>
                    <font size="2">增</font>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="center">
                <HeaderTemplate>
                    <asp:CheckBox ID="CheckAllDELETE" runat="server" onclick="javascript: CheckAllCheckBox(this,'chkISDELETE');" Text="刪除" ToolTip="按一次全選，再按一次取消全選" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkISDELETE" runat="server" Checked='<%# (Eval("ISDELETE").ToString() == "1")?true:false%>'></asp:CheckBox>
                    <font size="2">刪</font>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click"/>
    <input name="btnCancel" type="button" value="取消" onclick="location.href='Role.aspx';"/>

</asp:Content>

