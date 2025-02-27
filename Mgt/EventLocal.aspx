<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="EventLocal.aspx.cs" Inherits="Mgt_EventLocal" %>

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
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path txtS mb20">現在位置：<a href="#">課程管理</a> <i class="fa fa-angle-right"></i><a href="EventLocal.aspx">報名管理-22縣市</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                標題關鍵字<asp:TextBox ID="txt_searchTitle" runat="server" class="w2"></asp:TextBox>
                報名日期(起)<asp:TextBox ID="txt_searchDate_star" runat="server" type="text" class="w1"></asp:TextBox>
                報名日期(迄)<asp:TextBox ID="txt_searchDate_End" runat="server" type="text" class="w1"></asp:TextBox>
                <%--分類<asp:DropDownList ID="ddl_Class" runat="server" DataTextField="ClassName" DataValueField="EventCSNO"></asp:DropDownList>--%>
            </div>
            <div class="right">
                <input name="btnSearch" type="button" value="查詢" runat="server" onserverclick="btnSearch_Click" />
                <% if (userInfo.AdminIsInsert == true) { %>
                <input name="btnInsert" type="button" value="新增" onclick="location.href = 'EventLocal_AE.aspx?Work=N'" />
                <% } %>
            </div>
        </fieldset>
    </div>

    <asp:GridView ID="gv_Event" runat="server" AutoGenerateColumns="False" OnRowDataBound="gv_Event_RowDataBound">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="Class1" HeaderText="認證分類" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
                        <asp:BoundField DataField="Class2" HeaderText="課程分類" ItemStyle-HorizontalAlign="Center">
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
            <asp:BoundField DataField="EventName" HeaderText="活動標題"></asp:BoundField>
            <asp:BoundField DataField="CPerosn" HeaderText="主辦人" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="CountLimit" HeaderText="上限" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="PCount" HeaderText="報名人數" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
       <%--     <asp:TemplateField HeaderText="名單" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <%# ((int)Eval("PCount") > 0) ?
                        "<a href='#' onclick=\"var winvar = window.open('./Event_NameList.aspx?a=" + Request.QueryString["a"] + "&n=NN&sno=" + Eval("EventSNO").ToString() + "','winname','width=850 height=420 location=no,menubar=no status=no,toolbar=no')\";>" 
                        +"<i class='fa fa-address-book' aria-hidden='true'></i></a>" : "尚未有人報名"
                    %>
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="審核" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>

                    <%# ( Convert.ToDateTime(Eval("StartTime")) <= DateTime.Today                          
                          && userInfo.AdminIsUpdate == true) ?
                        "<a href='./EventAudit.aspx?a=" +Request.QueryString["a"] + "&n=NN&sno=" + Eval("EventSNO").ToString() + "&twenty=1'><i class='fa fa-pen-square' aria-hidden='true'></i></a>"                  
                        : "尚未到期"
                    %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsUpdate == true) { %>
                    <a href='./EventLocal_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("EventSNO").ToString() %>&csno=<%#Eval("id").ToString() %>'><i class="fa fa-pen-square"></i></a>
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
        </Columns>

    </asp:GridView>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />

    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />



</asp:Content>

