<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/Dialog.master" CodeFile="CourseNoteUpload.aspx.cs" Inherits="Mgt_CourseNoteUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


        <div class="path txtS mb20">現在位置：<a href="#">課程說明</a> <i class="fa fa-angle-right"></i>課程說明檔案上傳</div>
        <table>
            <tr>
                <th><i class="fa fa-star"></i>課程檔案名稱</th>
                <td colspan="3">
                    <asp:Label ID="lbl_CourseFile" runat="server" Text="最多200字元" Font-Size="Medium"></asp:Label>
                </td>
            </tr>
            <tr>
                <th><i class="fa fa-star"></i>角色名稱</th>
                <td colspan="3">
                    <asp:Label ID="lbl_RoleName" runat="server" Text="最多200字元" Font-Size="Medium"></asp:Label>
                </td>
            </tr>
            <tr>
                <th><i class="fa fa-star"></i>上傳檔案</th>
                <td colspan="3">
                    <asp:FileUpload ID="fileup_New" runat="server" />
                </td>

            </tr>

        </table>
        <div class="center btns">
            <asp:Button ID="btnOK" runat="server" Text="修改" OnClick="btnOK_Click" />
            <input name="btnCancel" type="button" value="關閉" onclick="window.close();" />
        </div>
        <br />

    
</asp:Content>
