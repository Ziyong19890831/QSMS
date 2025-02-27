<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Config.aspx.cs" Inherits="Mgt_Config" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path txtS mb20">現在位置：<a href="#">系統功能管理</a> <i class="fa fa-angle-right"></i><a href="Notice.aspx">系統參數設定</a></div>
    
<%--    <div class="both mb20">
        <fieldset>
            <legend>注意！注意！注意！注意！注意！</legend>
            <div class="notetxt">
                請勿變更任何設定！！呵呵，你也不能變更。
            </div>
        </fieldset>
    </div--%>

    <asp:GridView ID="gv_Config" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="PID" HeaderText="參數名稱"></asp:BoundField>
            <asp:BoundField DataField="PGroup" HeaderText="參數群組"></asp:BoundField>
            <asp:TemplateField HeaderText="設定值" >
                <ItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("PVal") %>' MaxLength="500"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="對應值">
                <ItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("MVal") %>' MaxLength="500"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="說明">
                <ItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Note") %>' MaxLength="500"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
         
    <div class="center btns">
        <%--<asp:Button ID="btnPage" runat="server" Text="儲存" OnClick="btnSave_Click" />--%>
    </div>

</asp:Content>

