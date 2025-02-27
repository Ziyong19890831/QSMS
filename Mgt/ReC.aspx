<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="ReC.aspx.cs" Inherits="Mgt_RePerson" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
       <div class="both mb20">
        <fieldset>
            <legend>上傳</legend>
            <div class="left w8">
                Excel檔案:       
                <asp:FileUpload ID="file_Upload" runat="server" />
            </div>
            <div class="right">
                <asp:Button ID="btnUpload" runat="server" Text="上傳" OnClick="btnUpload_Click" />
            </div>
        </fieldset>
    </div>
</asp:Content>





