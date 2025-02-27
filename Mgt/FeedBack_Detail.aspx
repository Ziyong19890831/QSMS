<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="FeedBack_Detail.aspx.cs" Inherits="Mgt_FeedBack_Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <div class="path txtS mb20">現在位置：<a href="#">常用服務管理</a> <i class="fa fa-angle-right"></i><a href="Feedback.aspx">意見回饋作業</a></div>
      <table >
          <tr class="w3">
              <th><i class="fa fa-star"></i>回覆狀態</th>
              <td>                     
                  <asp:Label ID="lb_ReplyStutas" runat="server"></asp:Label>
              </td>
          </tr>      
          <tr >
              <th><i class="fa fa-star"></i>回覆時間</th>
              <td>
                  <asp:Label ID="lb_FeedBackDate" runat="server"></asp:Label>
              </td>
          </tr>
          <tr id="forward" runat="server" visible="false">
              <th><i class="fa fa-star"></i>轉寄mail</th>
              <td>
                  <asp:Label ID="lb_Forward" runat="server"></asp:Label>
              </td>
          </tr>
          <tr >
              <th><i class="fa fa-star"></i>回覆主題</th>
              <td>
                  <asp:Label ID="lb_ReplyTheme" runat="server" ></asp:Label>
              </td>
          </tr>
          <tr>
              <th><i class="fa fa-star"></i>回覆內容</th>
              <td>
                  <asp:Label ID="lb_ReplyContent" runat="server" ></asp:Label>
              </td>
          </tr>
           <tr>
              <th><i class="fa fa-star"></i>回覆人員</th>
              <td>
                  <asp:Label ID="lb_ReplyPerson" runat="server" ></asp:Label>
              </td>
          </tr>
      </table>
</asp:Content>

