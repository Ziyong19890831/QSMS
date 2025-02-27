<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" Async="true" AutoEventWireup="true" CodeFile="SendSMS.aspx.cs" Inherits="Mgt_SendSMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="path txtS mb20">現在位置：<a href="#">常用服務管理</a> <i class="fa fa-angle-right"></i><a href="SendSMS.aspx">簡訊發送</a></div>
        <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />

        <fieldset>
            <legend>功能列</legend>
            <div style=" margin-top:-30px">
                <br />
                目前剩餘點數：<label id="SMS_point" runat="server"></label>
                <span style="color:red; font-weight:bold;"><br />可用 , 分隔開，複數傳送 例如：0910123456,0910456789</span>
            </div>
        </fieldset>

    <table>
<%--        <tr>
            <th><i class="fa fa-star"></i>標題</th>
            <td colspan="3">
                <asp:Label ID="lbl_Title" runat="server" Text="最多50字元" Font-Size="Medium"></asp:Label>
                <br />
                <asp:TextBox ID="txt_Title" runat="server" class="w10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_Title" runat="server" ErrorMessage="標題不得為空" ControlToValidate="txt_Title" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>--%>
        <tr>
            <th><i class="fa fa-star"></i>電話</th>      
            <td colspan="3"><asp:TextBox ID="txt_phone" runat="server" class="w10"></asp:TextBox></td>      
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>內容</th>
            <td colspan="3">
               <asp:TextBox ID="txt_SMS" runat="server" type="text" TextMode="MultiLine" Width="100%" Height="100px" Text=""></asp:TextBox>
 
            </td>
        </tr>
    </table>

    <div class="center btns">
         <asp:Button ID="btnSendSMS" runat="server" Text="寄送簡訊" OnClick="btnSendSMS_Click" />
    </div>
    

</asp:Content>

