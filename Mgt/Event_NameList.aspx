<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/Dialog.master" CodeFile="Event_NameList.aspx.cs" Inherits="Mgt_Event_NameList" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path txtS mb20">現在位置：<a href="#">帳號管理</a> <i class="fa fa-angle-right"></i>登記名單</div>

    <asp:GridView ID="gv_EventD" runat="server" AutoGenerateColumns="False" Font-Size="14px">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-CssClass="center">
                <ItemStyle CssClass="center" Width="50px"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="學員名稱" DataField="PName"></asp:BoundField>
            <asp:BoundField HeaderText="角色別" DataField="RoleName"></asp:BoundField>
            <asp:BoundField HeaderText="手機" DataField="PPhone"></asp:BoundField>
            <asp:BoundField HeaderText="電話" DataField="PTel"></asp:BoundField>
            <asp:BoundField HeaderText="信箱" DataField="PMail"></asp:BoundField>
            <asp:BoundField HeaderText="通知方式" DataField="EventNotice"></asp:BoundField>
            <asp:BoundField HeaderText="報名時間" DataField="ApplyDT"></asp:BoundField>

        </Columns>
    </asp:GridView>

    <div class="center btns">
        <input name="btnCancel" type="button" value="關閉" onclick="window.close();" />
    </div>
</asp:Content>

