<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="PersonnelSite_Integral.aspx.cs" Inherits="Web_Personnel_Integral" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="hf_Core" runat="server" />
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="../">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">個人首頁</li>
        </ol>
    </nav>
    <ul class="nav nav-tabs" id="myMember" role="tablist">
        <li class="nav-item">
            <a class="nav-link active" id="tabs-a-1" data-toggle="tab" href="#tabs-1" role="tab" aria-controls="tabs-1" aria-selected="true">證明</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="tabs-a-2" data-toggle="tab" href="#tabs-2" role="tab" aria-controls="tabs-2" aria-selected="false">證明時數狀態</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="tabs-a-3" data-toggle="tab" href="#tabs-3" role="tab" aria-controls="tabs-3" aria-selected="false">繼續教育時數狀態</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="tabs-a-4" data-toggle="tab" href="#tabs-4" role="tab" aria-controls="tabs-4" aria-selected="false">Elearning紀錄</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="tabs-a-5" data-toggle="tab" href="#tabs-5" role="tab" aria-controls="tabs-5" aria-selected="false">課程報名紀錄</a>
        </li>
    </ul>
    <div class="tab-content" id="myTableData">
        <div class="tab-pane fade mt10 show active " id="tabs-1" role="tabpanel" aria-labelledby="tabs-a-1">
            <asp:Label ID="lb_Certificate" runat="server" Visible="false">#有效證明</asp:Label>
           <%-- <asp:UpdatePanel ID="Up1" runat="server">--%>
                <ContentTemplate>

                    <table class="table table-striped">
                        <tr id="tbl_Certificate" runat="server" Visible="false">
                            <th style="width: 15%">電子檔</th>
                            <th style="width: 25%">證明類型</th>
                            <th style="width: 10%">首發日期</th>
                            <th style="width: 10%">到期日期</th>
                        </tr>
                         <asp:Repeater ID="rpt_CertificateCommon" runat="server" Visible="false">
                            <ItemTemplate>
                                <tr>
                                   <asp:Label ID="lb_CTypeSNO" runat="server" Visible="false"  Text='<%# Eval("CTypeSNO") %>'></asp:Label>
                                    <td>
                                        <asp:LinkButton ID="Btn_Print" runat="server" Visible="true" CommandArgument='<%# Eval("CertSNO") %>' OnClick="Btn_Print_Click">下載</asp:LinkButton>
                                    </td>
                                    <td><%# Eval("CtypeName") %></td>
                                    <td><%# Convert.ToDateTime(Eval("CertStartDate")).ToString("yyyy/MM/dd") %></td>
                                    <td><%# Convert.ToDateTime(Eval("CertEndDate")).ToString("yyyy/MM/dd") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Repeater ID="rpt_CertificateDown" runat="server" Visible="false">
                            <ItemTemplate>
                                <tr>
                                   <asp:Label ID="lb_CTypeSNO" runat="server" Visible="false" Text='<%# Eval("CTypeSNO") %>'></asp:Label>
                                    <td>
                                        <asp:LinkButton ID="btn_prove" runat="server" Visible="true" CommandArgument='<%# Eval("CertSNO") %>' OnClick="Btn_Print_Click">下載</asp:LinkButton>
                                    </td>
                                    <td><%# Eval("CtypeName") %></td>
                                    <td><%# Convert.ToDateTime(Eval("CertStartDate")).ToString("yyyy/MM/dd") %></td>
                                    <td><%# Convert.ToDateTime(Eval("CertEndDate")).ToString("yyyy/MM/dd") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Repeater ID="rpt_Certificate" runat="server" Visible="false">
                            <ItemTemplate>
                                <tr>
                                    <asp:Label ID="lb_CTypeSNO" runat="server" Visible="false" Text='<%# Eval("CTypeSNO") %>'></asp:Label>
                                    <td></td>
                                    <td><%# Eval("CtypeName") %></td>
                                    <td><%# Convert.ToDateTime(Eval("CertStartDate")).ToString("yyyy/MM/dd") %></td>
                                    <td><%# Convert.ToDateTime(Eval("CertEndDate")).ToString("yyyy/MM/dd") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <asp:Label ID="lb_CertificateN" runat="server" Visible="false">#無效證明</asp:Label>
                    <table class="table table-striped">
                        <tr id="tbl_CertificateN" runat="server" Visible="false">
                            <th style="width: 15%">電子檔</th>
                            <th style="width: 25%">證明類型</th>
                            <th style="width: 10%">首發日期</th>
                            <th style="width: 10%">到期日期</th>
                        </tr>
                        <asp:Repeater ID="rpt_CertificateN" runat="server" Visible="false">
                            <ItemTemplate>
                                <tr>
                                    <asp:Label ID="lb_CTypeSNO" runat="server" Visible="false" Text='<%# Eval("CTypeSNO") %>'></asp:Label>
                                    <td></td>
                                    <td><%# Eval("CtypeName") %></td>
                                    <td><%# Convert.ToDateTime(Eval("CertStartDate")).ToString("yyyy/MM/dd") %></td>
                                    <td><%# Convert.ToDateTime(Eval("CertEndDate")).ToString("yyyy/MM/dd") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </ContentTemplate>
           <%-- </asp:UpdatePanel>--%>
            <asp:Label ID="lb_CertificateNone" runat="server" Visible="true">#尚未取得證明</asp:Label>
        </div>
        <div class="tab-pane fade mt10" id="tabs-2" role="tabpanel" aria-labelledby="tabs-a-2">
            <table class="table table-striped">
                <tr id="tbl_CoursePlanningClass" runat="server">
                    <th>課程規劃名稱</th>
                    <th>已取得/目標時數</th>
                    <th>可取得的證明</th>
                    <th>時數證明</th>
                </tr>
                <asp:Repeater ID="rpt_CoursePlanningClass" runat="server" OnItemDataBound="rpt_CoursePlanningClass_ItemDataBound">
                    <ItemTemplate>

                        <tr>
                            <asp:Label ID="dd" runat="server" Visible="false" Text='<%# Eval("PClassSNO") %>'></asp:Label>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="LK_integral" runat="server" ForeColor="Blue" OnClientClick="showMSG()" CommandArgument='<%# Eval("PClassSNO") %>' OnClick="LK_integral_Click"><%# Eval("PlanName") %></asp:LinkButton>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <%# Eval("PClassTotalHr").ToString()=="" ? "0" : Eval("PClassTotalHr") %> / 
                                <%# Eval("sumHours").ToString()=="" ? "-" : Eval("sumHours") %>
                            </td>
                            <td><%# Eval("CTypeName") %></td>
                            <td> <asp:LinkButton ID="btn_prove" runat="server" Text="下載" OnClick="btn_prove_Click" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Label ID="lb_CoursePlanningClass" runat="server" Visible="true">#尚無時數紀錄</asp:Label>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <div id="c_content" class="ContentDiv" runat="server" visible="false">

                        <div>

                            <a onclick="hideMSG()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                            <h5><i class="fa fa-at" aria-hidden="true"></i>時數統計</h5>
                            <table class="table table-striped">
                                <tr id="Tr3" runat="server">
                                    <th>課程類別</th>
                                    <th>應取得</th>
                                    <th>已取得</th>
                                     <th>未取得</th>
                                </tr>
                                <asp:Repeater ID="rpt_integralS" runat="server" OnItemCommand="rpt_integralS_ItemCommand" OnItemDataBound="rpt_integralS_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lb_Class1" runat="server" Text='<%# Eval("課程類別") %>'></asp:Label></td>
                                            <td><%# Eval("應取得") %></td>
                                            <td>
                                                <asp:LinkButton ID="LK_Done" runat="server" ForeColor="Blue" OnClientClick="showMSGS()" CommandArgument='<%#Eval("PClassSNO") + "," +Eval("Class1")+ "," +Eval("CType")%>' OnClick="LK_Done_Click"><%# Eval("已取得") %></asp:LinkButton></i></td>
                                                                                <td>
                                        <asp:LinkButton ID="LK_NotDone" runat="server" ForeColor="Blue" OnClientClick="showMSGS()" CommandArgument='<%#Eval("PClassSNO") + "," +Eval("Class1")+ "," +Eval("CType")%>' OnClick="LK_NotDone_Click"><%# Eval("未取得") %></asp:LinkButton></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>

                    <div id="Div1" class="ContentDivS" runat="server" visible="false">

                        <div>

                            <a onclick="hideMSGS()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                            <h5><i class="fa fa-at" aria-hidden="true"></i>時數紀錄明細</h5>
                            <table class="table table-striped">
                                <tr id="Tr4" runat="server">                       
                                    <th>課程名稱</th>
                                    <th>授課方式</th> 
                                    <th>上課日期</th>
                                    <th>辦理單位</th>
                                    <th>上課地點</th>
                                </tr>
                                <asp:Repeater ID="Repeater1" runat="server">
                                    <ItemTemplate>
                                        <tr>                                            
                                            <td><%# Eval("CourseName") %></td>
                                            <td><%# Eval("Ctype") %></td>
                                            <td><%# Eval("上課日期") %></td>  
                                            <td><%# Eval("辦理單位") %></td>  
                                            <td><%# Eval("上課地點") %></td>  
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>
                    <div id="Div2" class="ContentDivS" runat="server" visible="false">

                        <div>

                            <a onclick="hideMSGSM()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                            <h5><i class="fa fa-at" aria-hidden="true"></i>時數紀錄明細</h5>
                            <table class="table table-striped">
                                <tr id="Tr1" runat="server">
                                    <th>課程名稱</th>
                                </tr>
                                <asp:Repeater ID="Repeater2" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("CourseName") %></td>
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
        </div>
        <div class="tab-pane fade mt10" id="tabs-3" role="tabpanel" aria-labelledby="tabs-a-3">
            <table class="table table-striped">
                 <asp:Label ID="lb_Elearn" runat="server" style="color: red" Visible="true"></asp:Label>
                <tr id="tbl_ECoursePlanningClass" runat="server">
                    <th>課程規劃名稱</th>
                    <th>已取得/目標時數</th>
                    <th>可取得的證明</th>
                    <th>時數證明</th>
                </tr>
                <asp:Repeater ID="rpt_ECoursePlanningClass" runat="server" OnItemDataBound="rpt_ECoursePlanningClass_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <asp:Label ID="CC" runat="server" Visible="false" Text='<%# Eval("PClassSNO") %>'></asp:Label>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="LK_Eintegral" runat="server" ForeColor="Blue" OnClientClick="showMSG()" CommandArgument='<%#Eval("PClassSNO") + "," +Eval("EPClassSNO")%>' OnClick="LK_Eintegral_Click"><%# Eval("PlanName") %></asp:LinkButton>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <%# Eval("PClassTotalHr").ToString()=="" ? "0" : Eval("PClassTotalHr") %> / 
                                <%# Eval("sumHours").ToString()=="" ? "-" : Eval("sumHours") %>
                            </td>
                            <td><%# Eval("CTypeName") %></td>
                            <td> <asp:LinkButton ID="btn_Eprove" runat="server" CommandArgument='<%#Eval("PClassSNO") + "," +Eval("EPClassSNO")%>' Text="下載" OnClick="btn_Eprove_Click" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Label ID="lb_ECoursePlanningClass" runat="server" Visible="false">#尚無時數紀錄</asp:Label>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="EIntegralSUM" class="ContentDiv" runat="server" visible="false">
                        <div>
                            <a onclick="hideMSGSM()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>
                            <h5><i class="fa fa-at" aria-hidden="true"></i>時數統計</h5>
                            <asp:Label ID="Label1" runat="server" style="color: red" Visible="true"></asp:Label>
                            <table class="table table-striped">
                                    <tr id="Tr2" runat="server">
                                    <th>課程類別</th>
                                    <th>授課方式</th>
                                    <th>已取得</th>
                                     <th>尚可取得</th>
                                </tr>
                                <asp:Repeater ID="rpt_EintegralS" runat="server" OnItemCommand="rpt_EintegralS_ItemCommand" OnItemDataBound="rpt_EintegralS_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lb_Class1" runat="server" Text='<%# Eval("課程類別") %>'></asp:Label></td>
                                            <td>線上</td>
                                              <td>
                                                <asp:LinkButton ID="LK_EDone" runat="server" ForeColor="Blue" OnClientClick="showMSGS()" CommandArgument='<%#Eval("PClassSNO") + "," +Eval("Class1")+","+"1"%>' OnClick="LK_EDone_Click"><%# Eval("已取得") %></asp:LinkButton></td>
                                            <td>
                                                <asp:LinkButton ID="LK_ENotDone" runat="server" ForeColor="Blue" OnClientClick="showMSGS()" CommandArgument='<%#Eval("PClassSNO") + "," +Eval("Class1")+","+"1"%>' OnClick="LK_ENotDone_Click"><%# Eval("尚可取得") %></asp:LinkButton></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lb_Class2" runat="server" Text='<%# Eval("課程類別") %>'></asp:Label></td>
                                            <td>非線上(例:實體/實習...等)</td>
                                            <td>
                                                <asp:LinkButton ID="LK_Entity" runat="server" ForeColor="Blue" OnClientClick="showMSGS()" CommandArgument='<%#Eval("EPClassSNO")%>' OnClick="LK_Entity_Click"><%# Eval("實體積分上傳") %></asp:LinkButton></i></td>
                                            <td></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>
                    <div id="EIntegralDetail" class="ContentDivS" runat="server" visible="false">

                        <div>

                            <a onclick="hideMSGSMSM()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                            <h5><i class="fa fa-at" aria-hidden="true"></i>繼續教育時數紀錄明細</h5>
                            <asp:Label ID="Label2" runat="server" style="color: red" Visible="true"></asp:Label>
                            <table class="table table-striped">
                                <tr id="Tr5" runat="server" visible="false">                       
                                    <th>課程名稱</th>
                                    <th>時數</th>
                                    <th>授課方式</th> 
                                    <th>上課日期</th>
                                    <th>辦理單位</th>
                                    <th>上課地點</th>            
                                </tr>
                                <tr id="Tr7" runat="server" visible="false">                       
                                    <th>課程名稱</th>
                                    <th>授課方式</th>           
                                </tr>
                                <asp:Repeater ID="Repeater3" runat="server">
                                    <ItemTemplate>
                                        <tr>                                            
                                            <td><%# Eval("CourseName") %></td>
                                            <td><%# Eval("Ctype") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                 <asp:Repeater ID="Repeater4" runat="server">
                                    <ItemTemplate>
                                        <tr>                                            
                                            <td><%# Eval("CourseName") %></td>
                                            <td><%# Eval("Integral") %></td>
                                            <td><%# Eval("授課方式") %></td>   
                                            <td><%# Eval("CDate") %></td>  
                                            <td><%# Eval("Unit") %></td>  
                                            <td><%# Eval("ClassLocation") %></td>  
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>
                    <div id="Div3" class="ContentDivS" runat="server" visible="false">

                        <div>

                            <a onclick="EhideMSGSMSM()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                            <h5><i class="fa fa-at" aria-hidden="true"></i>繼續教育時數紀錄明細</h5>
                            <asp:Label ID="Label3" runat="server" style="color: red" Visible="true"></asp:Label>
                            <table class="table table-striped">
                                <tr id="Tr6" runat="server">                       
                                    <th>課程名稱</th>             
                                </tr>
                                <asp:Repeater ID="Repeater5" runat="server">
                                    <ItemTemplate>
                                        <tr>                                            
                                            <td><%# Eval("CourseName") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="rpt_ECoursePlanningClass" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="tab-pane fade mt10" id="tabs-4" role="tabpanel" aria-labelledby="tabs-a-4">
            <span style="color: red"><i class="fa fa-star"></i>當測驗/滿意度/觀看紀錄皆完成時，約半小時後時數將自動計算匯入。</span>
                        <table class="table table-striped">
                <tr id="tbl_Elearning" runat="server">
                    <th>課程名稱</th>
                    <th>課程類型</th>
                    <th>課程時數</th>
                    <th>測驗</th>
                    <th>滿意度</th>
                    <th>觀看紀錄</th>
                </tr>
                            <asp:Repeater ID="rpt_Elearning" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("CourseName") %></td>
                                    <td><%# Eval("CtypeName") %></td>
                                    <td><%# Eval("MVal") %></td>
                                    <td><%# Eval("Exam") %></td>
                                    <td><%# Eval("Feedback") %></td>
                                    <td><%# Eval("Record") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        </table>
                                <asp:Label ID="lb_Elearning" runat="server" Visible="false">#尚無Elearning紀錄</asp:Label>
 <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>


                <asp:GridView ID="gv_LearningRecord" runat="server" AutoGenerateColumns="False" OnRowCreated="gv_LearningRecord_RowCreated">
                    <Columns>
                        <asp:BoundField HeaderText="課程名稱" DataField="CourseName" />
                        <asp:BoundField HeaderText="課程類型" DataField="Mval" />
                        <asp:BoundField HeaderText="課程時數" DataField="ISNO" />
                     
                    </Columns>
                </asp:GridView>


                <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
                <asp:HiddenField ID="txt_Page" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
            </div>
                <div class="tab-pane fade mt10" id="tabs-5" role="tabpanel" aria-labelledby="tabs-a-5">
                     <table class="table table-striped">
                <tr id="tbl_Event" runat="server">
                    <th>報名課程名稱</th>
                    <th>狀態</th>
                    <th>報名時間</th>
                </tr>
                            <asp:Repeater ID="rpt_Event" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><a target="_parent" href="javascript:void(0)" onclick="window.open('Event_Preview.aspx?Sno=<%# Eval("EventSNO") %>', '', 'width=500,height=500');">
                                        <p style="color: blue"><%# Eval("EventName") %></p>
                                    </a></td>
                                    <td><%# Eval("Audit") %></td>
                                    <td><%# Eval("CreateDT") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        </table>
                    <asp:Label ID="lb_Event" runat="server" Visible="false">#尚無活動報名紀錄</asp:Label>
            </div>
    </div>
    <script type="text/javascript"> 
        $('#ContentPlaceHolder1_rpt_CoursePlanningClass_M1_0').hide();
        $('#ContentPlaceHolder1_rpt_CoursePlanningClass_M1_1').hide();
        $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_0').hide();
        $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_1').hide();
        function showMSG() {
            $("#c_content").show("fade");
        }
        function EshowMSG() {
            $("#Ec_content").show("fade");
        }
        function showMSGS() {
            $("#Div1").show("fade");

        }
        function EshowMSGS() {
            $("#Div3").show("fade");

        }
        function hideMSGS() {
            //$("#c_content").hide("fade");
            document.getElementById("ContentPlaceHolder1_Div1").style.display = "none";//隐藏

        }
        function hideMSGSM() {
            //$("#c_content").hide("fade");
            document.getElementById("ContentPlaceHolder1_Div2").style.display = "none";//隐藏
            document.getElementById("ContentPlaceHolder1_EIntegralSUM").style.display = "none";//隐藏
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_P1_0').show();
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_0').hide();
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_P1_1').show();
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_1').hide();
        }
        function EhideMSGS() {
            //$("#c_content").hide("fade");
            document.getElementById("ContentPlaceHolder1_Div3").style.display = "none";//隐藏

        }
        function EhideMSGSM() {
            //$("#c_content").hide("fade");
            document.getElementById("ContentPlaceHolder1_Div4").style.display = "none";//隐藏

        }
        function hideMSG() {
            //$("#c_content").hide("fade");
            document.getElementById("ContentPlaceHolder1_c_content").style.display = "none";//隐藏
            $('#ContentPlaceHolder1_rpt_CoursePlanningClass_P1_0').show();
            $('#ContentPlaceHolder1_rpt_CoursePlanningClass_M1_0').hide();
            $('#ContentPlaceHolder1_rpt_CoursePlanningClass_P1_1').show();
            $('#ContentPlaceHolder1_rpt_CoursePlanningClass_M1_1').hide();

        }
        function EhideMSG() {
            //$("#c_content").hide("fade");
            document.getElementById("ContentPlaceHolder1_Ec_content").style.display = "none";//隐藏
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_P1_0').show();
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_0').hide();
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_P1_1').show();
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_1').hide();

        }
        function hideMSGSMSM() {
            document.getElementById("ContentPlaceHolder1_EIntegralDetail").style.display = "none";//隐藏
        }
        function EhideMSGSMSM() {
            document.getElementById("ContentPlaceHolder1_Div3").style.display = "none";//隐藏v
        }
        $(document).ready(function () {
            $('#ContentPlaceHolder1_rpt_CoursePlanningClass_LK_integral_0').click(function (index) {
                $('#ContentPlaceHolder1_rpt_CoursePlanningClass_P1_0').hide();
                $('#ContentPlaceHolder1_rpt_CoursePlanningClass_M1_0').show();
            });
            $('#ContentPlaceHolder1_rpt_CoursePlanningClass_LK_integral_1').click(function (index) {
                $('#ContentPlaceHolder1_rpt_CoursePlanningClass_P1_1').hide();
                $('#ContentPlaceHolder1_rpt_CoursePlanningClass_M1_1').show();
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
        });
        $(".hide").click(function () {
            try {
                document.getElementById("ContentPlaceHolder1_EIntegralSUM").style.display = "none";//隐藏
            }
            catch (err) {

            }
        }
        );
        $(".hide").click(function () {
            try {
                document.getElementById("ContentPlaceHolder1_EIntegralDetail").style.display = "none";//隐藏
            }
            catch (err) {

            }
        }
        );
        $(".hide").click(function () {
            try {
                document.getElementById("ContentPlaceHolder1_Div3").style.display = "none";//隐藏
            }
            catch (err) {

            }
        }
        );
    </script>
</asp:Content>

