<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="RepeaterTest.aspx.cs" Inherits="Mgt_RepeaterTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--    <ul id="Menu" class="MenuBarHorizontal nav L7 w10" style="display:none;">--%>
    <asp:Repeater ID="rpt_dir" runat="server" OnItemDataBound="rpt_Link_ItemDataBound">
        <ItemTemplate>
            <%-- <li>--%>
                  <a class="MenuBarItemSubmenu" href="#"><i class="fa fa-newspaper-o" aria-hidden="true"></i>  <%# Eval("PLINKNAME")%></a>
                              <ul>
                                  <asp:Repeater ID="rpt_link" runat="server">
                                  <ItemTemplate>
                                       <li><a href="<%# Eval("PLINKURL") %>" target="_blank"><%# Eval("PLINKNAME") %></a></li>
                                  </ItemTemplate>
                                  </asp:Repeater>
                              </ul>
              <%-- </li>--%>
                      
        </ItemTemplate>
    </asp:Repeater>
        <%--</ul>--%>
</asp:Content>

