<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Upload_AE.aspx.cs" Inherits="Mgt_Upload_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_PID" runat="server" />
    <asp:HiddenField ID="txt_URL" runat="server" />
    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


    <table>
        <tr>
            <th>排序</th>
            <td>
                <asp:DropDownList ID="ddl_OrderSeq" runat="server"  DataTextField="DLCNAME" DataValueField="DLCSNO">
                    <asp:ListItem Value="">無</asp:ListItem>
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>名稱</th>
            <td>
                <asp:Label ID="lbl_Title" runat="server" Text="最多60字元"></asp:Label>
                <br />
                <asp:TextBox ID="txt_Title" runat="server" class="w10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_Upload" runat="server" ControlToValidate="txt_Title" ErrorMessage="不可為空" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>適用人員</th>
            <td>
                <asp:CheckBoxList ID="cb_Role" class="required" runat="server" RepeatColumns="3" DataTextField="RoleName" DataValueField="RoleSNO" RepeatLayout="Table" />
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>檔案分類</th>
            <td>
                <asp:DropDownList ID="ddl_Download_Class" runat="server"  DataTextField="DLCNAME" DataValueField="DLCSNO"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>說明</th>
            <td>
                <asp:Label ID="Label1" runat="server" Text="最多500字元"></asp:Label>
                <br />
                <asp:TextBox ID="txt_Note" runat="server" class="w10" Rows="5" TextMode="MultiLine"></asp:TextBox>
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
                        </div>
                        <div>
                            <asp:Button ID="btn_delfile2" runat="server" Text="刪除" OnClick="btn_delfile_Click" CommandArgument="2" Visible="false" />
                            <asp:Literal ID="lt_file2" runat="server"></asp:Literal>
                            <asp:FileUpload ID="fileup_Document2" runat="server" />
                        </div>
                        <div>
                            <asp:Button ID="btn_delfile3" runat="server" Text="刪除" OnClick="btn_delfile_Click" CommandArgument="3" Visible="false" />
                            <asp:Literal ID="lt_file3" runat="server"></asp:Literal>
                            <asp:FileUpload ID="fileup_Document3" runat="server" />
                        </div>
                        <div>
                            <asp:Button ID="btn_delfile4" runat="server" Text="刪除" OnClick="btn_delfile_Click" CommandArgument="4" Visible="false" />
                            <asp:Literal ID="lt_file4" runat="server"></asp:Literal>
                            <asp:FileUpload ID="fileup_Document4" runat="server" />
                        </div>
                        <div>
                            <asp:Button ID="btn_delfile5" runat="server" Text="刪除" OnClick="btn_delfile_Click" CommandArgument="5" Visible="false" />
                            <asp:Literal ID="lt_file5" runat="server"></asp:Literal>
                            <asp:FileUpload ID="fileup_Document5" runat="server" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <div class="center btns">
        <asp:Button ID="ButtonOK" runat="server" Text="修改" OnClick="ButtonOK_Click"/>
        <input name="btnCancel" type="button" value="取消" onclick="location.href='Upload.aspx';"/>
    </div>

</asp:Content>

