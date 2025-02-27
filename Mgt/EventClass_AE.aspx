<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="EventClass_AE.aspx.cs" Inherits="Mgt_EventClass_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
        <style type="text/css">
        .mydropdownlist {
            float: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />

    <br />

    <table>
        <tr>
            <th><i class="fa fa-star"></i>分類名稱</th>
            <td colspan="3">
                <asp:Label ID="lbl_Name" runat="server" Text="最多50字元" Font-Size="Medium"></asp:Label>
                <br />
                <asp:TextBox ID="txt_Name" runat="server" class="w10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_Title" runat="server" ErrorMessage="標題不得為空" ControlToValidate="txt_Name" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
          <tr>
            <th><i class="fa fa-star"></i>活動分類</th>
            <td colspan="3">
                <asp:DropDownList ID="ddl_Class1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_Class1_SelectedIndexChanged" CssClass="mydropdownlist" DataTextField="MVal" DataValueField="PVal">
                </asp:DropDownList>
                <asp:UpdatePanel runat="server" ID="UP_1">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddl_Class2" runat="server" AutoPostBack="true"  CssClass="mydropdownlist" OnSelectedIndexChanged="ddl_Class2_SelectedIndexChanged" DataTextField="MVal" DataValueField="PVal">
                        </asp:DropDownList>
                        <br />
                        <br />
                        <asp:Label ID="lb_ddl" runat="server"></asp:Label>
                        <asp:RadioButtonList runat="server" ID="Rd_1" Visible="false">
                            <asp:ListItem Text="是" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="否"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:Label ID="lb_ddl_1" runat="server"></asp:Label>
                        <asp:RadioButtonList runat="server" ID="Rd_2" Visible="false">
                            <asp:ListItem Text="是" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="否"></asp:ListItem>
                        </asp:RadioButtonList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddl_Class1" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddl_Class2" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
    </table>


    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="修改" OnClick="btnOK_Click"/>
        <input name="btnCancel" type="button" value="取消" onclick="location.href='EventClass.aspx';"/>
    </div>



</asp:Content>

