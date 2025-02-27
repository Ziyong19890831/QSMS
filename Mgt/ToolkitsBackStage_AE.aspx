<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="ToolkitsBackStage_AE.aspx.cs" Inherits="ToolkitsBackStage_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


    <table>
        <tr class="w3">
            <th>適用性</th>
            <td>
                <%--<asp:DropDownList ID="ddl_stageClass" runat="server" DataValueField="PVal" DataTextField="MVal"></asp:DropDownList>--%>
                <asp:CheckBoxList ID="CBL_StageClass" runat="server" DataTextField="MVal" DataValueField="PVal" RepeatColumns="3"></asp:CheckBoxList>
                <%--<asp:RequiredFieldValidator ID="rfv_stageClass" runat="server" ErrorMessage="適用性不得為空" ControlToValidate="ddl_stageClass" ForeColor="Red"></asp:RequiredFieldValidator>--%>
            </td>
        </tr>
        <tr>
            <th>科別</th>
            <td>
                <asp:CheckBoxList ID="CHB_TkType" runat="server" DataTextField="MVal" DataValueField="PVal" RepeatColumns="5"></asp:CheckBoxList>
                <%--<asp:DropDownList ID="ddl_TkType" class="required" runat="server" DataValueField="PVal" DataTextField="MVal" />--%>
                
            </td>
        </tr>
        <tr>
            <th>期別</th>
            <td>
                <asp:CheckBoxList ID="CBL_Stage" DataTextField="MVal" DataValueField="PVal" runat="server" RepeatColumns="5"></asp:CheckBoxList>
                <%--  <asp:DropDownList ID="ddl_stage" class="required" runat="server" DataValueField="PVal" DataTextField="MVal" />
                 <asp:RequiredFieldValidator ID="rfv_stage" runat="server" ErrorMessage="期別不得為空" ControlToValidate="ddl_stage" ForeColor="Red"></asp:RequiredFieldValidator>--%>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>教材名稱</th>
            <td>
                <asp:Label ID="lbl_FileName" runat="server" Text="最多30字元"></asp:Label>
                <br />
                <asp:TextBox ID="txt_FileName" class="required w10" runat="server" MaxLength="50"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="rfv_FileName" runat="server" ErrorMessage="教材名稱不得為空" ControlToValidate="txt_FileName" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>是否啟用</th>
            <td>
                <asp:CheckBox ID="chk_IsEnable" runat="server" Text="是" />
            </td>
        </tr>
        <tr>
            <th>檔案</th>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div>
                            <asp:Button ID="btn_delfile1" runat="server" Text="刪除" OnClick="btn_delfile_Click" CommandArgument="1" Visible="false" />
                            <asp:Literal ID="lt_file1" runat="server"></asp:Literal>
                            <asp:FileUpload ID="fileup_Document1" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="檔案不得為空" ControlToValidate="fileup_Document1" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <th>預覽圖</th>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div>
                            <asp:Button ID="btn_delfile2" runat="server" Text="刪除" OnClick="btn_delfile2_Click" CommandArgument="1" Visible="false" />
                            <asp:Literal ID="lt_file2" runat="server"></asp:Literal>
                            <asp:FileUpload ID="fileup_Document2" runat="server" />

                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>


            <th>備註</th>
            <td>
                <asp:TextBox ID="txt_Note" runat="server" Height="100px" Width="100%" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click" />
        <input name="btnCancel" type="button" value="取消" onclick="location.href = 'ToolkitsBackStage.aspx';" />
    </div>
</asp:Content>

