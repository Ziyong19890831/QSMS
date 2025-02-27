<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="FileUpload.aspx.cs" Inherits="Web_FileUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="sc1" runat="server"></asp:ScriptManager>
        <fieldset>
            <legend>上傳</legend>
            <div class="left w8">

                <asp:UpdatePanel ID="upl_ddl" runat="server">
                    <ContentTemplate>
                        課程規劃類別：
                        <asp:DropDownList ID="ddl_CoursePlanningClass" runat="server" AutoPostBack="true"
                            DataValueField="PClassSNO" DataTextField="PlanName" OnSelectedIndexChanged="ddl_CoursePlanningClass_SelectedIndexChanged">
                        </asp:DropDownList>
                        <br />
                        欲上傳的課程:
                        <asp:DropDownList ID="ddl_CourseName" runat="server" DataValueField="CourseSNO" DataTextField="CourseName" OnSelectedIndexChanged="ddl_CourseName_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
               <i class="fa fa-star">請將所有檔案壓縮成壓縮檔(.zip)再上傳!</i>
                <br />    
                <asp:FileUpload ID="file_Upload" runat="server" />
            </div>
     
            <asp:TextBox ID="txt_Note" TextMode="MultiLine" runat="server" Width="300" Height="100" placeholder="備註"></asp:TextBox>
            
        </fieldset>
    <div class="center">
                <asp:Button ID="btnUpload" runat="server" Text="上傳" OnClick="btnUpload_Click" />
            </div>
</asp:Content>

