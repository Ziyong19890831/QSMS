<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="EventInvite.aspx.cs" Inherits="Mgt_EventInvite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
              document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>
    <style>
        .margins{
            margin-left:30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>



            <fieldset>
                <legend>功能列</legend>
                姓名<asp:TextBox ID="txt_PName" runat="server" ></asp:TextBox>
                身分證<asp:TextBox ID="txt_PersonID" runat="server"></asp:TextBox><br />
                <div class="right">
                    <asp:Button ID="btn_InviteSearch"  runat="server" OnClick="btn_InviteSearch_Click" Text="搜尋"  />
                </div>
            </fieldset>
            <asp:GridView ID="gv_Invite" runat="server" AutoGenerateColumns="false" DataKeyNames="RoleSNO,PersonSNO,PName,PersonID" OnRowCreated="gv_Invite_RowCreated">
                <Columns>
                    <asp:BoundField HeaderText="姓名" DataField="PName" ItemStyle-CssClass="center" />
                    <asp:BoundField HeaderText="身分證號" DataField="PersonID" />
                    <asp:BoundField HeaderText="PersonSNO" DataField="PersonSNO" />                 
                    <asp:BoundField HeaderText="角色別" DataField="RoleSNO" />
                    <asp:BoundField HeaderText="手機" DataField="PPhone" />
                    <asp:BoundField HeaderText="電話" DataField="PTel" />
                    <asp:BoundField HeaderText="信箱" DataField="PMail" />
                    <asp:TemplateField HeaderText="勾選">
                        <ItemTemplate>
                            <asp:CheckBox ID="Chk_Invite" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
            <asp:HiddenField ID="txt_Page" runat="server" />
            <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
            <div class="center">
              
                <asp:Button ID="btn_Insert" AutoPostBack="true" runat="server" Text="報名" OnClick="btn_Insert_Click" Visible="false" />
                <asp:Button ID="btn_Cancel" runat="server" Text="取消" OnClick="btn_Cancel_Click" />   
                  <asp:Button ID="btn_Nocondition" AutoPostBack="true" runat="server" Text="無條件報名" OnClick="btn_Nocondition_Click" CssClass="margins" />
            </div>

</asp:Content>

