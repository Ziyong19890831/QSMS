<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="FeedBack.aspx.cs" Inherits="Mgt_FeedBack" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
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

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path txtS mb20">現在位置：<a href="#">常用服務管理</a> <i class="fa fa-angle-right"></i><a href="Feedback.aspx">意見回饋作業</a></div>
    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                關鍵字<asp:TextBox ID="txt_SearchTitle" runat="server" class="w2"></asp:TextBox>
                 Email<asp:TextBox ID="txt_mail" runat="server" Width="250px"></asp:TextBox>
                填寫時間<asp:TextBox ID="txt_searchDateStart" runat="server" type="text" class="w1"></asp:TextBox>～
                   <asp:TextBox ID="txt_searchDateEnd" runat="server" type="text" class="w1"></asp:TextBox>
            </div>
            <div class="right">
                <input name="btnSearch" type="button" value="查詢" runat="server" onserverclick="btnSearch_Click" />
            </div>
        </fieldset>
    </div>

    <asp:GridView ID="gv_Feedback" runat="server" AutoGenerateColumns="False" OnRowDataBound="gv_Feedback_RowDataBound">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="CreateDT" HeaderText="填寫時間" />
            <asp:BoundField DataField="FBTYPE" HeaderText="類型" />
            <asp:BoundField DataField="Name" HeaderText="姓名" />
            <asp:BoundField DataField="Rank" HeaderText="身分" />
            <asp:BoundField DataField="Explain" HeaderText="內容">
                <ItemStyle Font-Size="14px"/>
            </asp:BoundField>
            <asp:BoundField DataField="Tel" HeaderText="電話" />
            <asp:BoundField DataField="Email" HeaderText="信箱" />
            <asp:BoundField DataField="FeedBackDate" HeaderText="回覆時間" />
            <asp:BoundField DataField="Response" HeaderText="回覆狀態" />
            <asp:TemplateField HeaderText="刪除" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsDelete == true) { %>
                    <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("FBSNO")%>'><i class="fa fa-trash"></i></asp:LinkButton>
                    <% } else { %> 
                        無權限 
                    <% } %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="回覆" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsDelete == true ) { %>
                   
                     <a href="#"  onclick="var winvar = window.open('./FeedBack_AE.aspx?sno=<%#Eval("FBSNO").ToString() %>','winname','width=1200 height=550 location=no,menubar=no status=no,toolbar=no');"><i class="fa fa-pen-square"></i></a>
                     
                        <% } else { %> 
                        無權限 
                    <% } %>
                       
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
                        <asp:TemplateField HeaderText="查看" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <% if (userInfo.AdminIsDelete == true) { %>
                    <a href="#" onclick="var winvar = window.open('./FeedBack_Detail.aspx?sno=<%#Eval("FBSNO").ToString() %>','winname','width=1200 height=550 location=no,menubar=no status=no,toolbar=no');"><i class="fa fa-pen-square"></i></a>
                    <% } else { %> 
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

