<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="Event_Preview.aspx.cs" Inherits="Web_Event_Preview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <h1><i class="fa fa-newspaper"></i>活動報名</h1>

    <asp:Repeater ID="rpt_Event" runat="server">
        <ItemTemplate>
            <table>
             <%--   <tr>
                    <th class="w7 txtL">分類：<%# Eval("Name") %></th>
                    <th class="w3 txtL">發布日期：<%#Convert.ToDateTime(Eval("SDate")).ToString("yyyy-MM-dd") %></th>
                </tr>--%>
                <tr>
                    <th class="txtL" colspan="2 ">活動名稱：<%# Eval("EventName") %></th>
                </tr>
                <tr>
                    <td colspan="2" class="padding20">
                        <%# HttpUtility.HtmlDecode(Eval("Note").ToString()) %>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
    <div class="center btns">  
        <input name="btnCancel" type="button" value="關閉視窗" onclick="window.close();" />
    </div> 
</asp:Content>

