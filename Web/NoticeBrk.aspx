<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Web.master" AutoEventWireup="true" CodeFile="Notice.aspx.cs" Inherits="Web_Notice" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        .td_news {
            background-image: url('../images/new2.gif');
            background-size: 35px;
            background-position-x: right;
            background-position-y: top;
        }

        .ContentDiv {
            z-index: 999;
            border: none;
            margin: 0px;
            padding: 0px;
            width: 100%;
            height: 100%;
            top: 0px;
            left: 0px;
            background-color: rgba(0, 0, 0, 0.6);
            /*opacity: 0.6;
            filter: alpha(opacity=60);*/
            position: fixed;
            text-align: center;
            line-height: 25px;
            display: none;
        }
    </style>

    <script type="text/javascript">
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
    <script type="text/javascript">
        function getUrlVars() {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        }
        $(function () {
            $("#tabs").tabs({
                activate: function () {
                    var selectedTab = $('#tabs').tabs('option', 'active');
                    console.log(selectedTab);
                    $("#<%= hdnSelectedTab.ClientID %>").val(selectedTab);
                },
                active: document.getElementById('<%= hdnSelectedTab.ClientID %>').value
            });
            var date = getUrlVars()["data"];
            if (date == 0) {
                $('#tabs').tabs('select', '#tabs-1');
            }
            if (date == 1) {
                $('#tabs').tabs('select', '#tabs-2');
            }
            if (date == 2) {
                $('#tabs').tabs('select', '#tabs-3');
            }
        });

        $(function()  {
            $("#Note_content").show("fade");
        });
        function hideNote() {
            $("#Note_content").hide("fade");
        };
        function Link() {
            window.open('https://forms.gle/ADrCd7gYXoiWdqWi8', '_blank');
            $("#Note_content").hide("fade");
        };
        function LinkToToolkits() {
            window.open('toolkits.aspx');
            $("#Note_content").hide("fade");
        };
        
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="path mb20">目前位置：<a href="Notice.aspx">公告事項</a></div>
   <asp:HiddenField ID="hdnSelectedTab" runat="server" Value="0" />
  
    <h1><i class="fa fa-newspaper"></i>公告事項    
    <div class="blockSearch right">
        分類查詢：
            <asp:DropDownList ID="ddl_Notice_Class" runat="server" DataTextField="Name" DataValueField="NoticeCSNO"></asp:DropDownList>
        <asp:DropDownList ID="dd2_RoleName" runat="server" DataTextField="RoleName" DataValueField="RoleGroup"></asp:DropDownList>
        <input type="text" placeholder="請輸入要查詢的關鍵字" id="txtSearch" runat="server" />
        <input type="button" onserverclick="btnSearch_Click" runat="server" value="查詢" />
    </div>
    </h1>



     <asp:ScriptManager ID="SM1" runat="server"></asp:ScriptManager>
     

    <div id="tabs" style="margin-bottom: 10px;">
        <ul>
            <li><a href="#tabs-1">公告事項</a></li>
            <li><a href="#tabs-2">文獻</a></li>
            <li><a href="#tabs-3">歷史公告事項</a></li>
        </ul>
        <div id="tabs-1">
            <asp:UpdatePanel ID="Up1" runat="server">
                <ContentTemplate>
                     
                    <table>
                        <tr>
                            <th class="w1 center" style="width:15%">類別</th>
                            <th style="width:15%">人員</th>
                            <th style="width:55%">標題</th>
                            <th class="w2 center" style="width:15%">日期</th>
                        </tr>
                        <asp:Repeater ID="rpt_NoticeMore" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("ClassName") %></td>
                                    <td><%# Eval("RoleBindName") %></td>
                                    <td <%# Eval("OrderSeq").ToString() != "" ? "class='td_news'" : "" %>><a href="Notice_AE.aspx?sno=<%# Eval("NoticeSNO") %>"><%# Eval("Title") %></a></td>
                                    <td><a href="#"><%#Convert.ToDateTime(Eval("SDate")).ToString("yyyy-MM-dd") %></a></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                     
                    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
                    <asp:HiddenField ID="txt_Page" runat="server" />
                    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
               </ContentTemplate>
            </asp:UpdatePanel>
       </div>
        <div id="tabs-2">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <th class="w1 center" style="width:15%">類別</th>
                            <th style="width:15%">人員</th>
                            <th style="width:55%">標題</th>
                            <th class="w2 center" style="width:15%">日期</th>
                        </tr>
                        <asp:Repeater ID="rpt_Word" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("ClassName") %></td>
                                    <td><%# Eval("RoleBindName") %></td>
                                    <td <%# Eval("OrderSeq").ToString() != "" ? "class='td_news'" : "" %>><a href="Notice_AE.aspx?sno=<%# Eval("NoticeSNO") %>"><%# Eval("Title") %></a></td>
                                    <td><a href="#"><%#Convert.ToDateTime(Eval("SDate")).ToString("yyyy-MM-dd") %></a></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                 
                    <asp:Literal ID="ltl_PageNumber_word" runat="server"></asp:Literal>
                    <asp:HiddenField ID="txt_Page_word" runat="server" />
                    <asp:Button ID="btnPage_word" runat="server" Text="查詢" OnClick="btnPage_word_Click" Style="display: none;" />
                   </ContentTemplate>
            </asp:UpdatePanel>
       </div> 
        <div id="tabs-3">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <th class="w1 center" style="width:15%">類別</th>
                            <th style="width:15%">人員</th>
                            <th style="width:55%">標題</th>
                            <th class="w2 center" style="width:15%">日期</th>
                        </tr>
                        <asp:Repeater ID="rpt_NoticeMore_his" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("ClassName") %></td>
                                    <td><%# Eval("RoleBindName") %></td>
                                    <td <%# Eval("OrderSeq").ToString() != "" %>><a href="Notice_AE.aspx?sno=<%# Eval("NoticeSNO") %>"><%# Eval("Title") %></a></td>
                                    <td><a href="#"><%#Convert.ToDateTime(Eval("SDate")).ToString("yyyy-MM-dd") %></a></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                         
                    <asp:Literal ID="ltl_PageNumber_his" runat="server"></asp:Literal>
                    <asp:HiddenField ID="txt_Page_his" runat="server" />
                    <asp:Button ID="btnPage_his" runat="server" Text="查詢" OnClick="btnPage_his_Click" Style="display: none;" />
         </ContentTemplate>
            </asp:UpdatePanel>
        </div>
                 
             
    </div>
     <%-- <div id="Note_content" class="ContentDiv">

        <div style="background-color: white; opacity: 0.95; padding: 20px 30px; border-radius: 5px; margin: 100px auto 0 auto; z-index: 999; width: 500px; height: 250px;">
            <a onclick="hideNote()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>
            <h1><i class="fa fa-at" aria-hidden="true"></i>問券調查</h1>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <p>套裝教材歡迎下載使用後填寫問卷</p>
                    <asp:Button ID="btn_question" runat="server" OnClick="btn_question_Click" Text="前往問卷調查" OnClientClick="Link()" Width="120px" Height="50px"  Font-Bold="true" />
                    <asp:Button ID="btn_Toolkits" runat="server" OnClick="btn_question_Click" Text="前往教材下載" OnClientClick="LinkToToolkits()" Width="120px" Height="50px"  Font-Bold="true" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>--%>

</asp:Content>

