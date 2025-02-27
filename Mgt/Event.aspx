<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Event.aspx.cs" Inherits="Mgt_Event" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
        $(function () {
            $("#<%=txt_searchDate_star.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
         $(function () {
            $("#<%=txt_searchDate_End.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
         });
        function OpenWindows() {
            var open=window.open('AutoSendMail.aspx', '', 'width=1000,height=600');
        }
        function LinkRoleGroupLink() {
            var winvar = window.open('./EventRoleGroupLog.aspx', 'winname', 'width=1200 height=550 location=no,menubar=no status=no,toolbar=no');
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path txtS mb20">現在位置：<a href="#">新訓課程報名管理</a> <i class="fa fa-angle-right"></i>
        <a href="Event.aspx">新訓課程報名管理</a>    </div>
        <div class="right"> <input name="btnDownload" type="button" value="講師名單" runat="server" style="text-align:right" onserverclick="btnDownload_Click" />

    </div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                標題關鍵字<asp:TextBox ID="txt_searchTitle" runat="server" class="w2"></asp:TextBox>
                報名日期(起)<asp:TextBox ID="txt_searchDate_star" runat="server" type="text" class="w1"></asp:TextBox>
                報名日期(迄)<asp:TextBox ID="txt_searchDate_End" runat="server" type="text" class="w1"></asp:TextBox><br />
                課程編號<asp:TextBox ID="txt_EventGroup" runat="server" Width="20px" Enabled="false" BackColor="Gray" Visible="false"></asp:TextBox><asp:TextBox ID="txt_GroupNum" runat="server" class="w1"></asp:TextBox>
                課程類別<asp:DropDownList ID="ddl_Class2" runat="server" DataTextField="Mval" DataValueField="PVal"></asp:DropDownList>
                <%--分類<asp:DropDownList ID="ddl_Class" runat="server" DataTextField="ClassName" DataValueField="EventCSNO"></asp:DropDownList>--%>
            </div>
            <div class="right">
                <input type="button" onclick="LinkRoleGroupLink()" value="歷史編號查詢" />
                <input name="btnSearch" type="button" value="查詢" runat="server" onserverclick="btnSearch_Click" />
                <% if (userInfo.AdminIsInsert == true) { %>
                <input name="btnInsert" type="button" value="新增" onclick="location.href = 'Event_AE.aspx?Work=N'" />
               <%-- <%if (userInfo.IsAdmin == true)
                    { %>
                <input name="btn_SendMail" type="button" value="寄送郵件" onclick="OpenWindows()" id="btn_SendMail" />
                <%} %>--%>
                <% } %>
            </div>
        </fieldset>
    </div>

    <asp:GridView ID="gv_Event" runat="server" AutoGenerateColumns="False" OnRowDataBound="gv_Event_RowDataBound">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
<%--            <asp:BoundField DataField="Class1" HeaderText="認證分類" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>--%>
                        <asp:BoundField DataField="Class2" HeaderText="課程類別" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="報名日期(起)" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lbl_date_Star" runat="server" Text='<%# Convert.ToDateTime(Eval("StartTime")).ToString("yyyy/MM/dd")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="報名日期(迄)" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lbl_date_End" runat="server" Text='<%# Convert.ToDateTime(Eval("EndTime")).ToString("yyyy/MM/dd")%>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="EventName" HeaderText="課程標題"></asp:BoundField>
            <asp:BoundField DataField="CountLimit" HeaderText="上限" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="PCount" HeaderText="報名人數" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="審核" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>

                    <%# ( Convert.ToDateTime(Eval("StartTime")) <= DateTime.Today                          
                          && userInfo.AdminIsUpdate == true) ?
                        "<a href='./EventAudit.aspx?a=" +Request.QueryString["a"] + "&n=NN&sno=" + Eval("EventSNO").ToString() + "'><i class='fa fa-pen-square' aria-hidden='true'></i></a>"                  
                        : "尚未開放"
                    %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsUpdate == true) { %>
                    <a href='./Event_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("EventSNO").ToString() %>&csno=<%#Eval("id").ToString() %>'><i class="fa fa-pen-square"></i></a>
                    <% } else { %>
                         無權限
                    <% } %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsDelete == true) { %>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("EventSNO") + "," +  Eval("id")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                    <% }  else { %> 
                        無權限 
                    <% } %>
                </ItemTemplate>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
           <%-- <asp:TemplateField HeaderText="通知" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsUpdate == true) { %>
                    <a href="#"  onclick="var winvar = window.open('./SendMail.aspx?sno=<%# Eval("EventSNO") %>','winname','width=800 height=550 location=no,menubar=no status=no,toolbar=no');"><i class="fa fa-pen-square"></i></a>
                    <% } else { %>
                         無權限
                    <% } %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>--%>
             <asp:TemplateField HeaderText="複製" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsDelete == true) { %>
                    <asp:LinkButton ID='btnCopy' runat="server" OnClientClick="return confirm('是否複製?');" OnClick="btnCopy_Click" CommandArgument='<%# Eval("EventSNO")+"," +  Eval("EventBSNO")%>'><i class="fa fa-pen-square"></i></asp:LinkButton>
                    <% }  else { %> 
                        無權限 
                    <% } %>
                </ItemTemplate>

                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />

    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />



</asp:Content>

