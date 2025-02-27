<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="PageLink_AE.aspx.cs" Inherits="Mgt_PageLink_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function checkDIR(pObj) {
            var obj = document.getElementById("<%=ddl_PPLinkSNO.ClientID%>");
            if (pObj.value == "1") {
                obj.disabled = "disabled";
            } else {
                obj.disabled = "";
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_PLinkSNO" runat="server" />
    <asp:HiddenField ID="txt_ISDIR_S" runat="server" />

    <table>
        <tr>
            <th><i class="fa fa-star"></i>頁面名稱</th>
            <td>
                <asp:Label ID="lbl_Name" runat="server" Text="最多60字元"></asp:Label><br />
                <asp:TextBox ID="txt_PLinkName" runat="server" MaxLength="60" class="w10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>頁面網址</th>
            <td>
                <asp:Label ID="lbl_PLinkUrl" runat="server" Text="最多200字元"></asp:Label><br />
                <asp:TextBox ID="txt_PLinkUrl" runat="server" MaxLength="200" class="w10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>頁面類型</th>
            <td>
                <asp:DropDownList ID="ddl_ISDIR" runat="server" onchange="checkDIR(this);"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>狀態</th>
            <td>
                <asp:DropDownList ID="ddl_ISENABLE" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>頁面父節點</th>
            <td>
                <asp:DropDownList ID="ddl_PPLinkSNO" runat="server" DataTextField="PLINKNAME" DataValueField="PLINKSNO"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>順序</th>
            <td>
                <asp:TextBox ID="txt_Order" runat="server" MaxLength="3"></asp:TextBox>
            </td>
        </tr>
    </table>

    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click" />
        <input name="btnCancel" type="button" value="取消" onclick="location.href = 'PageLink.aspx';" />
    </div>

</asp:Content>

