<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Web.master" AutoEventWireup="true" CodeFile="Personnel_ElearningRecord.aspx.cs" Inherits="Web_Personnel_ElearningRecord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
        $(function () {
            $(".datepicker").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
    </script>
    <style type="text/css">
        body {
            margin: 0;
            font-family: Arial, Helvetica, sans-serif;
        }

        .topnav {
            overflow: hidden;
            background-color: #768e60;
        }

            .topnav a {
                float: left;
                color: #f2f2f2;
                text-align: center;
                padding: 14px 16px;
                text-decoration: none;
                font-size: 17px;
            }

                .topnav a:hover {
                    background-color: #ddd;
                    color: black;
                }

                .topnav a.active {
                    background-color: #4CAF50;
                    color: white;
                }

        marquee {
            font-size: 16px;
            font-weight: 800;
            color: red;
            font-family: sans-serif;
        }
          .ui-tabs .ui-tabs-nav li {
            padding: 0 5px;
        }

        h1 {
            font-family: 'Microsoft JhengHei';
        }

        .MarginTop {
            margin-top: 10px;
        }

        .tab_content {
            padding: 20px 0;
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

        FIELDSET {
            margin: 5px;
            border: 1px solid silver;
            padding: 8px;
            border-radius: 4px;
        }

        LEGEND {
            padding: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <h1><i class="fa fa-book"></i>Elearning紀錄</h1>

    <div class="topnav">
        <a href="./PersonnelSite.aspx">課程規劃進度條</a>
        <a href="./PersonnelSite_EventRecord.aspx">報名紀錄</a>
        <a href="./PersonnelSite_Integral.aspx">證書積分狀態</a>
        <a href="./PersonnelSite_EIntegral.aspx">繼續教育積分狀態</a>
        <a class="active" href="./Personnel_ElearningRecord.aspx">Elearning紀錄</a>
        <a href="./PersonnelSite_Calendar.aspx">行事曆</a>
    </div>
    <div class="both mb20" style="background-color: white; height: 600px; margin-top: 30px">
        <span style="color: red"><i class="fa fa-star"></i>此記錄需先通過測驗成績，才會顯示在此，當測驗/滿意度/觀看紀錄皆完成時，約半小時後積分將自動計算匯入。</span>
        <asp:UpdatePanel ID="Up1" runat="server">
            <ContentTemplate>


                <asp:GridView ID="gv_LearningRecord" runat="server" AutoGenerateColumns="False" OnRowCreated="gv_LearningRecord_RowCreated">
                    <Columns>
                        <%--<asp:BoundField HeaderText="證書類型" DataField="CTypeName" />--%>
                        <asp:BoundField HeaderText="課程名稱" DataField="CourseName" />
                        <asp:BoundField HeaderText="課程類型" DataField="Mval" />
                        <asp:BoundField HeaderText="課程積分" DataField="ISNO" />
                        <asp:TemplateField HeaderStyle-BackColor="#6699ff" HeaderStyle-BorderColor="#6699ff">
                            <HeaderTemplate>                                
                                 測驗
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ForeColor="blue" CommandArgument='<%# Eval("ELSCode") %>' ID="LB_Exam" Text='<%# Eval("Exam") %>' OnClientClick="showNote()" OnClick="LB_Exam_Click"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-BackColor="#6699ff" HeaderStyle-BorderColor="#6699ff">
                            <HeaderTemplate>
                                滿意度
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ForeColor="blue" CommandArgument='<%# Eval("ELSCode") %>' ID="LB_Feedback" Text='<%# Eval("Feedback") %>' OnClientClick="showNote()" OnClick="LB_Feedback_Click"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField HeaderStyle-BackColor="#6699ff" HeaderStyle-BorderColor="#6699ff">
                            <HeaderTemplate>
                                觀看紀錄
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ForeColor="blue" CommandArgument='<%# Eval("ELSCode") %>' ID="LB_Record" Text='<%# Eval("Record") %>' OnClientClick="showNote()" OnClick="LB_Record_Click"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>


                <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
                <asp:HiddenField ID="txt_Page" runat="server" />
                <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <div id="Note_content" class="ContentDiv">

            <div style="background-color: white; opacity: 0.95; padding: 20px 30px; border-radius: 5px; margin: 100px auto 0 auto; z-index: 999; width: 700px; height: 350px;">
                <a onclick="hideNote()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>
                <h1><i class="fa fa-at" aria-hidden="true"></i>記錄明細</h1>

                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <table class="tbl">
                            <tr id="Tr" runat="server" visible="false">
                                <th>課程名稱</th>
                                <th>節數</th>
                                <th>完成日期</th>
                            </tr>
                            <asp:Repeater ID="rpt_ECoursePlanningClass" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td class="center"><%# Eval("ELSName")%></td>
                                        <td class="center"><%# Eval("ELSPart")%></td>
                                        <td class="center"><%# Eval("FinishedDate")%></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        <asp:Label ID="lb_Notice" runat="server" Text="#無資料" Visible="false"></asp:Label>
                    </ContentTemplate>
         
                </asp:UpdatePanel>


                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table class="tbl">
                            <tr id="Tr1" runat="server" visible="false">                               
                                <th>測驗名稱</th>
                                <th>測驗分數</th>
                                <th>測驗日期</th>
                                <th>通過分數</th>
                                <th>是否通過</th>
                            </tr>
                            <asp:Repeater ID="LearningScore" runat="server">
                                <ItemTemplate>
                                    <tr>                                       
                                        <td><%# Eval("QuizName") %></td>
                                        <td><%# Eval("Score") %></td>
                                        <td><%# Eval("ExamDate") %></td>
                                        <td><%# Eval("PassScore") %></td>
                                        <td><%# Eval("Pass") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>


                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table class="tbl">
                            <tr id="Tr2" runat="server" visible="false">
                                <th>E-Learning測驗主題</th>
                                <th>E-Learning課程名稱</th>
                                <th>完成日期</th>
                            </tr>
                            <asp:Repeater ID="rpt_FeedBack" runat="server" >
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("ELName") %></td>
                                        <td><%# Eval("ELSName") %></td>
                                        <td><%# Eval("CompletedDate") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>




            </div>
        </div>
    </div>
    <script type="text/javascript">
        function showNote() {
            $("#Note_content").show("fade");
        }
        function hideNote() {
            $("#Note_content").hide("fade");
        }

    </script>
</asp:Content>

