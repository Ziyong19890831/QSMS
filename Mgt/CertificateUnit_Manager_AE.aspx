<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="CertificateUnit_Manager_AE.aspx.cs" Inherits="Mgt_CertificateUnit_Manager_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <asp:ScriptManager ID="SM" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="Up1" runat="server">
        <ContentTemplate>
             <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />
    <br />

            <table>
                <tr>
                    <th><i class="fa fa-star"></i>管理單位</th>
                    <td>
                        <asp:TextBox ID="txt_UnitName" class="required w10" MaxLength="50" runat="server"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rfv_Title" runat="server" ErrorMessage="管理單位名稱不得為空" ControlToValidate="txt_UnitName" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>是否為管理者</th>
                    <td>
                        <asp:CheckBox ID="chk_admin" runat="server" Text="是" OnCheckedChanged="chk_admin_CheckedChanged" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>發證單位</th>
                    <td>
                        <asp:CheckBoxList ID="cb_Role" class="required" runat="server" RepeatColumns="4" DataTextField="CunitName" DataValueField="CunitSNO" RepeatLayout="Table" Enabled="false" />
                        <span id="rolemsg"></span>
                    </td>
                </tr>


            </table>

           

        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="center btns">
                <asp:Button ID="btnOK" runat="server" Text="修改" OnClientClick="checkinput();" OnClick="btnOK_Click" />
                <input name="btnCancel" type="button" value="取消" onclick="location.href = 'CertificateUnit_Manager.aspx';" />
            </div>


    <script type="text/javascript">
        $('#ContentPlaceHolder1_chk_admin').change(function () {
            $('#ContentPlaceHolder1_cb_Role input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);

            });
        });
        function checkinput() {
            var chktrue;
            $('#ContentPlaceHolder1_chk_admin input[type=checkbox]:checked').each(function () {
                chktrue = true;
                return false;
            });
            var valuelist = '';
            var isSelect;
            //共用檢核是否空值 
            $('#ContentPlaceHolder1_cb_Role input[type=checkbox]:checked').each(function () {
                isSelect = true;
                return false;
            });
            if (chktrue == true) {
                if (isSelect != true) {
                    event.preventDefault();
                    alert('請勾選發證單位');
                }
            } else {
                return
            }
        }
    </script>
</asp:Content>

