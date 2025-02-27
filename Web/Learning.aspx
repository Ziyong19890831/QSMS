<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="Learning.aspx.cs" Inherits="Web_Learning" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();
            var CheckBack = $.qs.get("Check");
            if (CheckBack == 1) {
                 $("#tabs").tabs("option", "active", 3);
            };
           
        });
        String.prototype.equalsIgnoreCase = function (str) {
            ///<summary>忽略大小写比较两个字符串是否相等并返回。语法：string1.equalsIgnoreCase(str)</summary>
            ///<param name="str" type="string">要参与比较的另一字符串。</param>
            ///<returns type="boolean">字符串是否忽略大小写相等。</returns>
            return (this.toUpperCase() == str.toUpperCase());
        };


        $.qs = function () {
            ///<summary>处理 QueryString 的对象。语法：$.qs</summary>
        };
        $.qs.get = function (paramName) {
            var result = null;
            var search = location.search;
            if (search.substr(0, 1) == "?") {
                search = search.substr(1);
            }
            var params = search.split("&");
            for (var i = 0; i < params.length; i++) {
                var pos = params[i].indexOf("=");
                if (pos > 0 && params[i].substr(0, pos).equalsIgnoreCase(paramName)) {
                    try {
                        result = decodeURIComponent(params[i].substr(pos + 1));
                    }
                    catch (ex) {
                        result = params[i].substr(pos + 1);
                    }
                    break;
                }
            }

            return result;
        };
       
    </script>

    <style>
        .tbl {
            font-size: 14px;
        }

        .both mb20 {
            background-color: white;
        }

        .buttonD {
            border: 1px solid #c5c5c5;
            background: #f6f6f6;
            font-weight: normal;
            pointer-events: none;
            padding: .4em 1em;
            display: inline-block;
            position: relative;
            line-height: normal;
            margin-right: .1em;
            cursor: pointer;
            vertical-align: middle;
            text-align: center;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path mb20">目前位置：<a href="Learning.aspx">個人學習歷程</a></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div style="text-align: right">
        <span style="color: red; font-weight: bold; text-align: right">註：每三十分鐘更新一次</span>

    </div>
    <h1><i class="fa fa-book"></i>個人學習歷程</h1>

    <div class="both mb20">
        <asp:HiddenField ID="hf_Core" runat="server" /><%--檢查核心完訓證明是否符合條件(PClassTotalHr-sumHours<=0 and CtypeSNO=10 || =11)--%>
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                學員名稱：<asp:Label ID="lb_PName" runat="server"></asp:Label>
                <p></p>
                身分證：<asp:Label ID="lb_PersonID" runat="server"></asp:Label>
            </div>

                        <div class="right">

                <%--<asp:DropDownList ID="ddl_CoursePlanning" runat="server" DataTextField="PlanName"  DataValueField="PClassSNO">
              
                </asp:DropDownList>
                <asp:Button ID="btn_IntegralPrint" Text="證書積分列印" runat="server" OnClick="btn_IntegralPrint_Click" />
                <asp:Button ID="btn_EIntegralPrint" Text="繼續教育積分列印" runat="server" OnClick="btn_EIntegralPrint_Click" />
   --%>
                <asp:Button ID="btn_Auto" runat="server" Text="積分更新" OnClick="btn_Auto_Click" />
                
            </div>
        </fieldset>
    </div>
    <div id="tabs" style="margin-bottom: 10px;">
        <ul>

            <li class="hide"><a href="#tabs-1">已取得的證書</a></li>
            <li><a href="#tabs-2">證書積分資料</a></li>
            <li class="hide"><a href="#tabs-3">繼續教育積分資料</a></li>
            <li class="hide"><a href="#tabs-4">課程及活動報名紀錄</a></li>
            <li class="hide"><a href="#tabs-5">E-Learning上課紀錄</a></li>
            <li class="hide"><a href="#tabs-6">E-Learning測驗紀錄</a></li>
            <li class="hide"><a href="#tabs-7">E-Learning滿意度填寫</a></li>
        </ul>



        <div id="tabs-1">
            <asp:GridView ID="gv_Certificate" runat="server" AutoGenerateColumns="false" OnRowCreated="gv_Certificate_RowCreated">
                <Columns>
                    <asp:BoundField HeaderText="SysChange" DataField="SysChange" />
                    <asp:BoundField HeaderText="證號" DataField="OCTypeString" DataFormatString="{0:D6}" HtmlEncode="False" />
                    <asp:BoundField HeaderText="證號" DataField="OCTypeString" />
                    <asp:BoundField HeaderText="證書類型" DataField="CTypeName" />
                    <asp:BoundField HeaderText="發證單位" DataField="CUnitName" />
                    <asp:BoundField HeaderText="首發日期" DataField="CertPublicDate" />
                    <%--<asp:BoundField HeaderText="公告日期" DataField="CertStartDate" />--%>
                    <asp:BoundField HeaderText="到期日期" DataField="CertEndDate" />
                    <asp:BoundField HeaderText="展延" DataField="CertExt" />
                    <asp:BoundField HeaderText="備註" DataField="Note" />
                    <asp:TemplateField HeaderText="下載">
                        <ItemTemplate>
                            <asp:LinkButton ID="Btn_Print" ForeColor="Blue" runat="server" CommandArgument='<%# Eval("CertSNO") %>' OnClick="Btn_Print_Click">下載</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Label ID="Label1" runat="server"></asp:Label>
            <asp:Label ID="lb_Certificate" runat="server" Visible="false">#尚無取得證書</asp:Label>
        </div>


        <div id="tabs-2">

            <table class="tbl">
                <tr id="tbl_CoursePlanningClass" runat="server">
                    <th>課程規劃名稱</th>
                    <th>參考年度</th>
                    <th>已取得/目標積分</th>
                    <th>可取得的證書</th>
                    <th>積分證明下載</th>
                </tr>
                <asp:Repeater ID="rpt_CoursePlanningClass" runat="server" OnItemDataBound="rpt_CoursePlanningClass_ItemDataBound">
                    <ItemTemplate>

                        <tr>
                            <asp:Label ID="dd" runat="server" Visible="false" Text='<%# Eval("PClassSNO") %>'></asp:Label>
                            <td>
                                <i id="P1" runat="server" class="fas fa-plus" style="float: left"></i><i id="M1" runat="server" class="fas fa-minus" style="float: left"></i>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="LK_integral" runat="server" ForeColor="Blue" OnClientClick="showMSG()" CommandArgument='<%# Eval("PClassSNO") %>' OnClick="LK_integral_Click"><%# Eval("PlanName") %></asp:LinkButton>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td class="center"><%# Eval("CStartYear") + "-" + Eval("CEndYear") %></td>
                            <td class="center">
                                <%# Eval("PClassTotalHr").ToString()=="" ? "0" : Eval("PClassTotalHr") %> / 
                                <%# Eval("sumHours").ToString()=="" ? "-" : Eval("sumHours") %>
                            </td>
                            <td><%# Eval("CTypeName") %></td>
                            <td class="center">
                                <asp:LinkButton ID="btn_prove" runat="server" Text="下載" OnClick="btn_prove_Click" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Label ID="lb_CoursePlanningClass" runat="server" Visible="false">#尚無積分紀錄</asp:Label>
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div id="c_content" class="ContentDiv" runat="server" visible="false">

                    <div>

                        <a onclick="hideMSG()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                        <h1><i class="fa fa-at" aria-hidden="true"></i>積分統計</h1>
                        <table>
                            <tr id="Tr3" runat="server">
                                <th>課程類別</th>
                                <th>授課方式</th>
                                <th>應取得</th>
                                <th>已取得</th>
                                <th>未取得</th>
                                <th>作業上傳</th>
                            </tr>
                            <asp:Repeater ID="rpt_integralS" runat="server" OnItemCommand="rpt_integralS_ItemCommand" OnItemDataBound="rpt_integralS_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lb_Class1" runat="server" Text='<%# Eval("課程類別") %>'></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lb_CType" runat="server" Text='<%# Eval("授課方式") %>'></asp:Label></td>
                                        <td><%# Eval("應取得") %></td>
                                        <td>
                                            <asp:LinkButton ID="LK_Done" runat="server" ForeColor="Blue" OnClientClick="showMSGS()" CommandArgument='<%#Eval("PClassSNO") + "," +Eval("Class1")+ "," +Eval("CType")%>' OnClick="LK_Done_Click"><%# Eval("已取得") %></asp:LinkButton></i></td>
                                        <td>
                                            <asp:LinkButton ID="LK_NotDone" runat="server" ForeColor="Blue" OnClientClick="showMSGS()" CommandArgument='<%#Eval("PClassSNO") + "," +Eval("Class1")+ "," +Eval("CType")%>' OnClick="LK_NotDone_Click"><%# Eval("未取得") %></asp:LinkButton></td>
                                        <td>
                                            <asp:LinkButton ID="LK_Upload" CssClass="ui-button" runat="server" ForeColor="Red" Text="作業上傳" CommandName="Upload" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>

                <div id="Div1" class="ContentDivS" runat="server" visible="false">

                    <div>

                        <a onclick="hideMSGS()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                        <h1><i class="fa fa-at" aria-hidden="true"></i>積分紀錄明細</h1>
                        <table>
                            <tr id="Tr4" runat="server">
                                <th>課程類別</th>
                                <th>課程名稱</th>
                                <th>授課方式</th>
                                <th>參考年度</th>
                                <th>時數</th>
                                <th>取得積分</th>
                            </tr>
                            <asp:Repeater ID="Repeater1" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Class1") %></td>
                                        <td><%# Eval("CourseName") %></td>
                                        <td><%# Eval("Ctype") %></td>
                                        <td class="center"><%# Eval("CStartYear") + "-" + Eval("CEndYear") %></td>
                                        <td class="center"><%# Eval("CHour") %></td>
                                        <td class="center"><%# Eval("積分") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="rpt_CoursePlanningClass" />

            </Triggers>
        </asp:UpdatePanel>


        <div id="tabs-3">

            <table class="tbl">
                <tr id="tbl_ECoursePlanningClass" runat="server">
                    <th>課程規劃名稱</th>
                    <th>參考年度</th>
                    <th>可認證/目標積分</th>
                    <%--<th>可取得的證書</th>--%>
                    <th>積分證明下載</th>
                </tr>
                <asp:Repeater ID="rpt_ECoursePlanningClass" runat="server">
                    <ItemTemplate>
                        <tr>
                            <asp:Label ID="CC" runat="server" Visible="false" Text='<%# Eval("PClassSNO") %>'></asp:Label>
                            <td>
                                <i id="P1" runat="server" class="fas fa-plus" style="float: left"></i><i id="M1" runat="server" class="fas fa-minus" style="float: left"></i>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="LK_Eintegral" runat="server" ForeColor="Blue" OnClientClick="showMSG()" CommandArgument='<%#Eval("PClassSNO") + "," +Eval("EPClassSNO")%>' OnClick="LK_Eintegral_Click"><%# Eval("PlanName") %></asp:LinkButton>
                                    </ContentTemplate>

                                </asp:UpdatePanel>
                            </td>
                            <td class="center"><%# Eval("CStartYear") + "-" + Eval("CEndYear") %></td>
                            <td class="center">
                                <%# Eval("PClassTotalHr").ToString()=="" ? "0" : Eval("PClassTotalHr") %> / 
                                <%# Eval("sumHours").ToString()=="" ? "-" : Eval("sumHours") %>
                            </td>
                            <%--<td><%# Eval("CTypeName") %></td>--%>
                            <td class="center">
                                <asp:LinkButton ID="btn_Eprove" runat="server" Text="下載" OnClick="btn_Eprove_Click" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Label ID="lb_ECoursePlanningClass" runat="server" Visible="false">#尚無積分紀錄</asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <%--繼續教育積分統計--%>
                <div id="EIntegralSUM" class="ContentDiv" runat="server" visible="false">



                    <a onclick="hideMSGSM()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                    <h1><i class="fa fa-at" aria-hidden="true"></i>繼續教育Elearning積分統計</h1><h4>此課程Elearning學分只認證<asp:Label ID="lb_ElearnLimit" runat="server" ForeColor="Red"></asp:Label>學分</h4>
                    <table>
                        <tr id="Tr6" runat="server">
                            <th>課程類別</th>
                            <th>授課方式</th>

                            <th>已取得</th>
                            <th>未取得</th>

                        </tr>
                        <asp:Repeater ID="rpt_EintegralS" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("課程類別") %></td>
                                    <td><%# Eval("授課方式") %></td>

                                    <td>
                                        <asp:LinkButton ID="LK_EDone" ForeColor="Blue" runat="server" OnClientClick="showMSGS()" CommandArgument='<%#Eval("PClassSNO") + "," +Eval("Class1")+ "," +Eval("CType")%>' OnClick="LK_EDone_Click"><%# Eval("已取得") %></asp:LinkButton></td>
                                    <td>
                                        <asp:LinkButton ID="LK_ENotDone" ForeColor="Blue" runat="server" OnClientClick="showMSGS()" CommandArgument='<%#Eval("PClassSNO") + "," +Eval("Class1")+ "," +Eval("CType")%>' OnClick="LK_ENotDone_Click"><%# Eval("未取得") %></asp:LinkButton></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>

                </div>
                <%--繼續教育積分紀錄明細--%>
                <div id="EIntegralDetail" class="ContentDivS" runat="server" visible="false">



                    <a onclick="hideMSGSMSM()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                    <h1><i class="fa fa-at" aria-hidden="true"></i>繼續教育積分紀錄明細</h1>
                    <table>
                        <tr id="Tr7" runat="server">
                            <th>課程類別</th>
                            <th>課程名稱</th>
                            <th>授課方式</th>
                            <th>參考年度</th>
                            <th>時數</th>
                            <th>取得積分</th>
                        </tr>
                        <asp:Repeater ID="Repeater4" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("Class1") %></td>
                                    <td><%# Eval("CourseName") %></td>
                                    <td><%# Eval("Ctype") %></td>
                                    <td class="center"><%# Eval("CStartYear") + "-" + Eval("CEndYear") %></td>
                                    <td class="center"><%# Eval("CHour") %></td>
                                    <td class="center"><%# Eval("積分") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>

                </div>
                <%--繼續教育積分上傳統計--%>
                <div id="EUploadSUM" class="ContentDiv" runat="server" visible="false">



                    <a onclick="hideMSGSMSMS()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                    <h1><i class="fa fa-at" aria-hidden="true"></i>繼續教育積分上傳統計</h1>
                    <table>
                        <tr id="Tr5" runat="server">
                            <th>繼續教育課程名稱</th>
                            <th>通訊</th>
                            <th>線上</th>
                            <th>實體</th>
                            <th>實習</th>
                        </tr>
                        <asp:Repeater ID="rpt_EUploadintegralS" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("PlanName") %></td>
                                    <td>
                                        <asp:LinkButton ID="LK_C" runat="server" ForeColor="Blue" OnClientClick="showMSGS()" CommandArgument='<%#Eval("EPClassSNO")%>' OnClick="LK_C_Click"><%# Eval("通訊") %></asp:LinkButton></td>
                                    <td>
                                        <asp:LinkButton ID="LK_O" runat="server" ForeColor="Blue" OnClientClick="showMSGS()" CommandArgument='<%#Eval("EPClassSNO")%>' OnClick="LK_O_Click"><%# Eval("線上") %></asp:LinkButton></td>
                                    <td>
                                        <asp:LinkButton ID="LK_E" runat="server" ForeColor="Blue" OnClientClick="showMSGS()" CommandArgument='<%#Eval("EPClassSNO")%>' OnClick="LK_E_Click"><%# Eval("實體") %></asp:LinkButton></td>
                                    <td>
                                        <asp:LinkButton ID="LK_P" runat="server" ForeColor="Blue" OnClientClick="showMSGS()" CommandArgument='<%#Eval("EPClassSNO")%>' OnClick="LK_P_Click"><%# Eval("實習") %></asp:LinkButton></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>

                </div>
                <%--繼續教育積分上傳明細--%>
                <div id="EUploadDetail" class="ContentDiv" runat="server" visible="false">


                    <a onclick="hideMSGSMS()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                    <h1><i class="fa fa-at" aria-hidden="true"></i>繼續教育積分上傳統計紀錄</h1>
                    <table>
                        <tr id="Tr8" runat="server">
                            <th>課程名稱</th>
                            <th>學分</th>
                            <th>上傳時間</th>

                        </tr>
                        <asp:Repeater ID="rpt_EUploadintegralDetail" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("CourseName") %></td>
                                    <td><%# Eval("Integral") %></td>
                                    <td><%# Eval("CreateDT") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>

                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="rpt_ECoursePlanningClass" />
            </Triggers>
        </asp:UpdatePanel>
        <div id="tabs-4">
            <table class="tbl">
                <tr id="tbl_Event" runat="server">
                    <th>報名名稱</th>
                    <th>狀態</th>
                    <th>報名日期</th>
                </tr>
                <asp:Repeater ID="Event" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><a target="_parent" href="javascript:void(0)" onclick="window.open('Event_Preview.aspx?Sno=<%# Eval("EventSNO") %>', '', 'width=500,height=500');"><p style="color:blue"><%# Eval("EventName") %></p></a></td>
                            <td><%# Eval("Audit") %></td>
                            <td><%# Eval("CreateDT") %></td>                          
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Label ID="lb_Event" runat="server" Visible="false">#尚無課程及活動報名紀錄</asp:Label>
        </div>

        <div id="tabs-5">

            <table class="tbl">
                <tr id="tbl_LearningRecord" runat="server">
                    <th>E-Learning主題名稱</th>
                    <th>E-Learning課程名稱</th>
                    <th>節數</th>
                    <th>課程完成日</th>
                </tr>
                <asp:Repeater ID="LearningRecord" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("ELName") %></td>
                            <td><%# Eval("ELSName") %></td>
                            <td><%# Eval("ELSPart") %></td>
                            <td><%# Eval("FinishedDate") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Label ID="lb_LearningRecord" runat="server" Visible="false">#尚無E-Learning上課紀錄</asp:Label>
            <div class="center">
                <asp:Button ID="btn_pdf" runat="server" Text="下載" OnClick="btn_pdf_Click" />
            </div>
        </div>

        <div id="tabs-6">
            <table class="tbl">
                <tr id="tbl_LearningScore" runat="server">
                    <th>e-Learning測驗主題</th>
                    <th>測驗名稱</th>
                    <th>測驗分數</th>
                    <th>測驗日期</th>
                    <th>通過分數</th>
                    <th>是否通過</th>
                </tr>
                <asp:Repeater ID="LearningScore" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("ELName") %></td>
                            <td><%# Eval("QuizName") %></td>
                            <td><%# Eval("Score") %></td>
                            <td><%# Eval("ExamDate") %></td>
                            <td><%# Eval("PassScore") %></td>
                            <td><%# Eval("Pass") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Label ID="lb_LearningScore" runat="server" Visible="false">#尚無E-Learning測驗紀錄</</asp:Label>
        </div>
        <div id="Div2" runat="server">
            <div id="tabs-7">
                <span style="color: red"><i class="fa fa-star"></i>此記錄需先填寫E-Learning滿意度，才會顯示在此。</span>
                <table class="tbl">

                    <tr id="Tr10" runat="server">
                        <th>E-Learning測驗主題</th>
                        <th>E-Learning課程名稱</th>
                        <th>完成日期</th>
                    </tr>
                    <asp:Repeater ID="rpt_FeedBack" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("ELName") %></td>
                                <td><%# Eval("ELSName") %></td>
                                <td><%# Eval("CompletedDate") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <asp:Label ID="lb_FeedBack" runat="server" Visible="false">#尚無E-Learning滿意度填寫</</asp:Label>
            </div>
        </div>
    </div>


    <script type="text/javascript">
        $('#ContentPlaceHolder1_rpt_CoursePlanningClass_M1_0').hide();
        $('#ContentPlaceHolder1_rpt_CoursePlanningClass_M1_1').hide();
        $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_0').hide();
        $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_1').hide();
        window.onerror = function () {
            return true;
        }

        function showMSG() {
            $("#c_content").show("fade");
        }
        function showMSGS() {
            $("#Div1").show("fade");

        }
        function hideMSGS() {
            //$("#c_content").hide("fade");
            document.getElementById("ContentPlaceHolder1_Div1").style.display = "none";//隐藏

        }
        function hideMSGSM() {
            //$("#c_content").hide("fade");
            document.getElementById("ContentPlaceHolder1_EIntegralSUM").style.display = "none";//隐藏
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_P1_0').show();
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_0').hide();
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_P1_1').show();
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_1').hide();
        }
        function hideMSGSMS() {
            //$("#c_content").hide("fade");
            document.getElementById("ContentPlaceHolder1_EUploadDetail").style.display = "none";//隐藏

        }
        function hideMSGSMSM() {
            //$("#c_content").hide("fade");
            document.getElementById("ContentPlaceHolder1_EIntegralDetail").style.display = "none";//隐藏

        }
        function hideMSGSMSMS() {
            //$("#c_content").hide("fade");
            document.getElementById("ContentPlaceHolder1_EUploadSUM").style.display = "none";//隐藏

        }
        function hideMSG() {
            //$("#c_content").hide("fade");
            document.getElementById("ContentPlaceHolder1_c_content").style.display = "none";//隐藏
            $('#ContentPlaceHolder1_rpt_CoursePlanningClass_P1_0').show();
            $('#ContentPlaceHolder1_rpt_CoursePlanningClass_M1_0').hide();
            $('#ContentPlaceHolder1_rpt_CoursePlanningClass_P1_1').show();
            $('#ContentPlaceHolder1_rpt_CoursePlanningClass_M1_1').hide();

        }

        $(".hide").click(function () {
            try {
                document.getElementById("ContentPlaceHolder1_c_content").style.display = "none";//隐藏
            }
            catch (err) {
                console.log(err);
            }
        });
        $(".hide").click(function () {
            try {
                document.getElementById("ContentPlaceHolder1_Div1").style.display = "none";//隐藏
            }
            catch (err) {
                console.log(err);
            }
        }
        );
        $(".hide").click(function () {
            try {
                document.getElementById("ContentPlaceHolder1_EIntegralSUM").style.display = "none";//隐藏
            }
            catch (err) {
                console.log(err);
            }
        }
        );
        $(".hide").click(function () {
            try {
                document.getElementById("ContentPlaceHolder1_EIntegralDetail").style.display = "none";//隐藏
            }
            catch (err) {
                console.log(err);
            }
        }
        );
        $(".hide").click(function () {
            try {
                document.getElementById("ContentPlaceHolder1_EUploadSUM").style.display = "none";//隐藏
            }
            catch (err) {
                console.log(err);
            }
        }
        );
        $(".hide").click(function () {
            try {
                document.getElementById("ContentPlaceHolder1_EUploadDetail").style.display = "none";//隐藏
            }
            catch (err) {
                console.log(err);
            }
        }
        );

    </script>
    <script type="text/javascript"> 
        $(document).ready(function () {
            $('#ContentPlaceHolder1_rpt_CoursePlanningClass_LK_integral_0').click(function (index) {
                $('#ContentPlaceHolder1_rpt_CoursePlanningClass_P1_0').hide();
                $('#ContentPlaceHolder1_rpt_CoursePlanningClass_M1_0').show();
            });
            $('#ContentPlaceHolder1_rpt_CoursePlanningClass_LK_integral_1').click(function (index) {
                $('#ContentPlaceHolder1_rpt_CoursePlanningClass_P1_1').hide();
                $('#ContentPlaceHolder1_rpt_CoursePlanningClass_M1_1').show();
            });
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_LK_Eintegral_0').click(function (index) {
                $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_P1_0').hide();
                $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_0').show();
            });
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_LK_Eintegral_1').click(function (index) {
                $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_P1_1').hide();
                $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_1').show();
            });
        });

        var parameter = Sys.WebForms.PageRequestManager.getInstance();

        parameter.add_endRequest(function () {
            $('#ContentPlaceHolder1_rpt_CoursePlanningClass_LK_integral_0').click(function (index) {
                $('#ContentPlaceHolder1_rpt_CoursePlanningClass_P1_0').hide();
                $('#ContentPlaceHolder1_rpt_CoursePlanningClass_M1_0').show();
            });
            $('#ContentPlaceHolder1_rpt_CoursePlanningClass_LK_integral_1').click(function (index) {
                $('#ContentPlaceHolder1_rpt_CoursePlanningClass_P1_1').hide();
                $('#ContentPlaceHolder1_rpt_CoursePlanningClass_M1_1').show();
            });
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_LK_Eintegral_0').click(function (index) {
                $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_P1_0').hide();
                $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_0').show();
            });
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_LK_Eintegral_1').click(function (index) {
                $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_P1_1').hide();
                $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_1').show();
            });
        });
    </script>
</asp:Content>

