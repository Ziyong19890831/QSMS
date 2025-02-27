<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Notice.aspx.cs" Inherits="Mgt_Notice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
        function _goPage2(pageNumber_his) {
            document.getElementById("<%=txt_Page_his.ClientID%>").value = pageNumber_his;
            document.getElementById("<%=btnPage_his.ClientID%>").click();
        }
         function _goPage1(pageNumber_word) {
            document.getElementById("<%=txt_Page_word.ClientID%>").value = pageNumber_word;
            document.getElementById("<%=btnPage_word.ClientID%>").click();
        }
        $(function () {
            $("#<%=txt_searchDateStart.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
            $("#<%=txt_searchDateEnd.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });

    </script>
    <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();

        });
        function _goPage(pageNumber) {

            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
        function _goPage2(pageNumber) {
            document.getElementById("<%=txt_Page_his.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage_his.ClientID%>").click();
        }
        function _goPage1(pageNumber) {
         
            document.getElementById("<%=txt_Page_word.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage_word.ClientID%>").click();
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
    <div class="path txtS mb20">現在位置：<a href="#">常用服務管理</a> <i class="fa fa-angle-right"></i><a href="Notice.aspx">公告管理</a></div>
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                關鍵字<asp:TextBox ID="txt_searchTitle" runat="server" class="w2"></asp:TextBox>
                刊登期間(起)<asp:TextBox ID="txt_searchDateStart" runat="server" type="text"></asp:TextBox>
                刊登期間(迄)<asp:TextBox ID="txt_searchDateEnd" runat="server" type="text"></asp:TextBox>
                <%--系統<asp:DropDownList ID="ddl_SystemName" runat="server" DataTextField="SYSTEM_NAME" DataValueField="SYSTEM_ID"></asp:DropDownList>--%>
                    分類<asp:DropDownList ID="ddl_Class" runat="server" DataTextField="Name" DataValueField="NoticeCSNO"></asp:DropDownList>
            </div>
            <div class="right">
                <input name="btnSearch" type="button" value="查詢" runat="server" onserverclick="btnSearch_Click" />
                <% if (userInfo.AdminIsInsert == true)
                    { %>
                <input name="btnInsert" type="button" value="新增" onclick="location.href = 'Notice_AE.aspx?Work=N'" />
                <% } %>
            </div>
        </fieldset>
    </div>
    <div id="tabs" style="margin-bottom: 10px;">
        <ul>
            <li><a href="#tabs-1">公告事項</a></li>
           <%-- <li><a href="#tabs-2">文獻</a></li>--%>
            <li><a href="#tabs-3">歷史公告事項</a></li>
        </ul>
    
    <div id="tabs-1">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
        <asp:GridView ID="gv_Notice" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="ROW_NO" HeaderText="序號"></asp:BoundField>
                <asp:BoundField DataField="ClassName" HeaderText="類型"></asp:BoundField>
                <asp:TemplateField HeaderText="刊登期間(起)" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbl_Sdate" runat="server" Text='<%# Convert.ToDateTime(Eval("SDate")).ToString("yyyy/MM/dd")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="刊登期間(迄)" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbl_Edate" runat="server" Text='<%# Convert.ToDateTime(Eval("EDate")).ToString("yyyy/MM/dd")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="標題">
                    <ItemTemplate>
                        <a href="#" style="color: blue" onclick="javascript:window.open('Notice_Preview.aspx?sno=<%# Eval("NoticeSNO") %>','','width=1000,height=500');"><%# Eval("Title") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <%-- <asp:BoundField DataField="Title" HeaderText="標題"></asp:BoundField>--%>
                <asp:BoundField DataField="PName" HeaderText="發表者"></asp:BoundField>
                <asp:BoundField DataField="OrderSeq" HeaderText="排序" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <% if (userInfo.AdminIsUpdate == true)
                            { %>
                        <a href='./notice_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("NoticeSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
                        <% }
                            else
                            { %>
                        無權限
                    <% } %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <% if (userInfo.AdminIsUpdate == true)
                            { %>
                        <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("NoticeSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                        <% }
                            else
                            { %>
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
                 </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="gv_Notice" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div id="tabs-2">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gv_word" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="ROW_NO" HeaderText="序號"></asp:BoundField>
                <asp:BoundField DataField="ClassName" HeaderText="類型"></asp:BoundField>
                <asp:TemplateField HeaderText="刊登期間(起)" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbl_Sdate" runat="server" Text='<%# Convert.ToDateTime(Eval("SDate")).ToString("yyyy/MM/dd")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="刊登期間(迄)" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbl_Edate" runat="server" Text='<%# Convert.ToDateTime(Eval("EDate")).ToString("yyyy/MM/dd")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="標題">
                    <ItemTemplate>
                        <a href="#" style="color: blue" onclick="javascript:window.open('Notice_Preview.aspx?sno=<%# Eval("NoticeSNO") %>','','width=1000,height=500');"><%# Eval("Title") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <%-- <asp:BoundField DataField="Title" HeaderText="標題"></asp:BoundField>--%>
                <asp:BoundField DataField="PName" HeaderText="發表者"></asp:BoundField>
                <asp:BoundField DataField="OrderSeq" HeaderText="排序" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <% if (userInfo.AdminIsUpdate == true)
                            { %>
                        <a href='./notice_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("NoticeSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
                        <% }
                            else
                            { %>
                        無權限
                    <% } %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <% if (userInfo.AdminIsUpdate == true)
                            { %>
                        <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("NoticeSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                        <% }
                            else
                            { %>
                        無權限
                    <% } %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
                <asp:Literal ID="ltl_PageNumber_word" runat="server"></asp:Literal>
                <asp:HiddenField ID="txt_Page_word" runat="server" />
                <asp:Button ID="btnPage_word" runat="server" Text="查詢" OnClick="btnPage_word_Click" Style="display: none;" />
            </ContentTemplate>
              <Triggers>
                <asp:PostBackTrigger ControlID="gv_word" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div id="tabs-3">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gv_Notice_his" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="ROW_NO" HeaderText="序號"></asp:BoundField>
                        <asp:BoundField DataField="ClassName" HeaderText="類型"></asp:BoundField>
                        <asp:TemplateField HeaderText="刊登期間" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lbl_date" runat="server" Text='<%# Convert.ToDateTime(Eval("SDate")).ToString("yyyy/MM/dd")+"-"+ Convert.ToDateTime(Eval("EDate")).ToString("yyyy/MM/dd")%>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="標題">
                            <ItemTemplate>
                                <a href="#" style="color: blue" onclick="javascript:window.open('Notice_Preview.aspx?sno=<%# Eval("NoticeSNO") %>','','width=1000,height=500');"><%# Eval("Title") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:BoundField DataField="Title" HeaderText="標題"></asp:BoundField>--%>
                        <asp:BoundField DataField="PName" HeaderText="發表者"></asp:BoundField>
                        <asp:BoundField DataField="OrderSeq" HeaderText="排序" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <% if (userInfo.AdminIsUpdate == true)
                                    { %>
                                <a href='./notice_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("NoticeSNO").ToString() %>'><i class="fa fa-pen-square"></i></a>
                                <% }
                                    else
                                    { %>
                        無權限
                    <% } %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <% if (userInfo.AdminIsUpdate == true)
                                    { %>
                                <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("NoticeSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                                <% }
                                    else
                                    { %>
                        無權限
                    <% } %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
                <asp:Literal ID="ltl_PageNumber_his" runat="server"></asp:Literal>
                <asp:HiddenField ID="txt_Page_his" runat="server" />
                <asp:Button ID="btnPage_his" runat="server" Text="查詢" OnClick="btnPage_his_Click" Style="display: none;" />

            </ContentTemplate>
              <Triggers>
                <asp:PostBackTrigger ControlID="gv_Notice_his" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </div>
</asp:Content>

