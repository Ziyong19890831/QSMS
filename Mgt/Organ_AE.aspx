<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Organ_AE.aspx.cs" Inherits="Mgt_Organ_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <%-- <script type="text/javascript">
        function OnfocueOut() {
            var CCode = document.getElementById("ContentPlaceHolder1_hf_Code").value
            if (CCode == "True") {
                alert("已使用");
            }
            else {
                alert("未使用");
            }
        }
    </script>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="sc" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />
    <table>
        <tr>
            <th>單位代碼</th>
            <td>
                <asp:Label ID="lbl_Code" runat="server" Text="最多20字元"></asp:Label>
                <br />
                <asp:UpdatePanel ID="up" runat="server">
                    <ContentTemplate>
                          <asp:TextBox ID="txt_Code" runat="server" MaxLength="20" AutoPostBack="true" OnTextChanged="txt_Code_TextChanged" onfocusout="OnfocueOut()" ></asp:TextBox><br />
                <asp:HiddenField ID="hf_Code" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
              
            </td>
        </tr>
        <tr>
            <th>單位名稱</th>
            <td>
                <asp:Label ID="lbl_Name" runat="server" Text="最多100字元"></asp:Label>
                <br />
                <asp:TextBox ID="txt_Name" runat="server" MaxLength="100" class="w10"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <th>單位行政區</th>
            <td>
                <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
                <asp:UpdatePanel ID="upl_ddl" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddl_AreaCodeA" runat="server" OnSelectedIndexChanged="ddl_AreaCodeA_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddl_AreaCodeB" runat="server" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                            <asp:ListItem Text="請先選擇縣市行政區" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddl_AreaCodeA" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <th>聯絡地址</th>
            <td>
                <asp:Label ID="lbl_Addr" runat="server" Text="最多60字元"></asp:Label>
                <br />
                <asp:TextBox ID="txt_Addr" runat="server" MaxLength="60" class="w10"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <th>聯絡電話</th>
            <td>
                <asp:Label ID="lbl_Tel" runat="server" Text="最多50字元 ex:04-22345678"></asp:Label>
                <br />
                <asp:TextBox ID="txt_Tel" runat="server" MaxLength="50" CssClass="tele w10"></asp:TextBox><br />
            </td>
        </tr>
    </table>

    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click"/>
        <input name="btnCancel" type="button" value="取消" onclick="location.href='Organ.aspx';"/>
    </div>

</asp:Content>

