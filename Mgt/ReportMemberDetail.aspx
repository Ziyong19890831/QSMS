<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/Dialog.master" CodeFile="ReportMemberDetail.aspx.cs" Inherits="Mgt_ReportMemberDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $("#accordion-1").accordion();
            $("#accordion-2").accordion();
            $("#tabs").tabs();
        });
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%@ Register Src="~/Web/WUC_Country.ascx" TagPrefix="uc1" TagName="WUC_Country" %>
    <div class="path txtS mb20">現在位置：<a href="#">報表作業</a> <i class="fa fa-angle-right"></i>學員名冊</div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="both mb20">
        <asp:HiddenField ID="hf_Core" runat="server" />
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                學員名稱：<asp:Label ID="lb_PName" runat="server"></asp:Label>
                <span></span>
                身分證：<asp:Label ID="lb_PersonID" runat="server"></asp:Label>
            </div>
            <div class="right">
                <asp:Button ID="btnEdit" runat="server" Text="修改資料" OnClick="btnEdit_Click" />
            </div>
        </fieldset>
    </div>



    <div id="tabs" style="margin-bottom: 10px;">
        <ul>
            <%if (userInfo.RoleSNO != "14")
                { %>
            <li class="hide"><a href="#tabs-1">學員資料</a></li>
            <%} %>

            <li class="hide"><a href="#tabs-2">已取得證明</a></li>
            <%if (userInfo.RoleSNO != "14")
                { %>
            <li class="hide"><a href="#tabs-3">新訓資料</a></li>
            <li class="hide"><a href="#tabs-4">繼續教育資料</a></li>
            <li class="hide"><a href="#tabs-5">課程報名紀錄</a></li>
            <li class="hide"><a href="#tabs-6">E-Learning上課紀錄</a></li>
            <li class="hide"><a href="#tabs-7">E-Learning測驗紀錄</a></li>
            <li class="hide"><a href="#tabs-8">E-Learning時數總表</a></li>
            <li class="hide"><a href="#tabs-9">E-Learning滿意度填寫</a></li>

            <%} %>
            <li class="hide"><a href="#tabs-10">合約狀態</a></li>
        </ul>
        <%if (userInfo.RoleSNO != "14")
            { %>
        <div id="tabs-1">
            <table>
                <tr>
                    <th>使用者姓名</th>
                    <td>
                        <label id="txt_Name" name="name" type="text" maxlength="50" runat="server" readonly="readonly" />
                    </td>
                    <th>帳號</th>
                    <td>
                        <label id="txt_Account" name="name" type="text" maxlength="50" runat="server" readonly="readonly" />
                    </td>

                </tr>
                <tr>
                    <th>自填醫事機構代碼</th>
                    <td>
                        <label id="txt_OrganCode" name="name" type="text" maxlength="50" runat="server" readonly="readonly" />
                    </td>
                    <th>自填醫事機構名稱</th>
                    <td>
                        <label id="txt_OrganName" name="name" type="text" maxlength="50" runat="server" readonly="readonly" />
                    </td>

                </tr>
                <tr>
                    <th>角色別</th>
                    <td>
                        <label id="lb_Role" name="name" type="text" maxlength="50" runat="server" readonly="readonly" />
                    </td>
                    <th>角色別2</th>
                    <td>
                        <label id="lb_Role2" name="name" type="text" maxlength="50" runat="server" readonly="readonly" />
                    </td>

                </tr>
                <tr>
                    <th>身分證/居留證</th>
                    <td>
                        <label id="txt_Personid" name="lbl_PersionId" type="text" runat="server" readonly="readonly" />
                    </td>
                    <th>學歷</th>
                    <td>
                        <label id="txt_degree" name="txt_degree" type="text" runat="server" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <th>Email</th>
                    <td colspan="3">
                        <label type="text" id="txt_Mail" runat="server" maxlength="100" readonly="readonly" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <th>國籍</th>
                    <td>
                        <label id="txt_Country" type="text" runat="server" readonly="readonly" />
                    </td>
                    <th>出生年月日</th>
                    <td>
                        <label id="txt_Birthday" type="text" runat="server" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <th>通訊地址</th>
                    <td colspan="3">
                        <label id="txt_ZipCode" type="text" class="number" placeholder="區號" maxlength="5" runat="server" style="width: 50px;" readonly="readonly" />
                        <label id="txt_Addr" type="text" class="w6" maxlength="50" runat="server" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <th>通訊電話</th>
                    <td>
                        <label type="text" id="txt_Tel" maxlength="50" runat="server" readonly="readonly" />
                        <br />
                    </td>
                    <th>手機</th>
                    <td>
                        <label type="text" id="txt_Phone" maxlength="50" runat="server" readonly="readonly" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <th>帳號狀態</th>
                    <td>
                        <asp:DropDownList ID="ddl_IsEnable" Enabled="false" runat="server">
                            <asp:ListItem Text="啟用" Value="1" />
                            <asp:ListItem Text="停用" Value="0" />
                        </asp:DropDownList>
                    </td>
                    <th>學員狀態</th>
                    <td>
                        <asp:DropDownList ID="ddl_Status" Enabled="false" runat="server" DataTextField="MName" DataValueField="MStatusSNO"></asp:DropDownList>
                    </td>
                </tr>
                <tr>

                    <th>備註</th>
                    <td colspan="3">
                        <label type="text" id="lb_Note" maxlength="50" runat="server" readonly="readonly" />
                        <br />
                    </td>
                </tr>
            </table>



            <div class="both mb20" id="div_MP" runat="server">
                <fieldset>
                    <legend>以下是從醫事人員資料表帶入的</legend>
                    <table>
                        <asp:Panel ID="ForDoctor" runat="server" Visible="false">
                            <tr id="Doctor2" runat="server">
                                <th>專科</th>
                                <td colspan="3">
                                    <asp:Label ID="lb_LSType" runat="server"></asp:Label></td>
                            </tr>
                            <tr id="Doctor3" runat="server">
                                <th>專科證書字號</th>
                                <td colspan="3">
                                    <asp:Label ID="lb_LSCN" runat="server"></asp:Label></td>
                            </tr>
                            <tr id="Doctor1" runat="server">
                                <th>執業科別</th>
                                <td colspan="3">
                                    <asp:Label ID="lb_LRtype" runat="server"></asp:Label></td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="ForOther" runat="server" Visible="false">
                            <tr id="Tr1" runat="server">
                                <th>職業類別</th>
                                <td colspan="3">
                                    <asp:Label ID="lb_TJobType" runat="server"></asp:Label></td>
                            </tr>
                            <tr id="Tr2" runat="server">
                                <th>服務科別</th>
                                <td colspan="3">
                                    <asp:Label ID="lb_TSType" runat="server"></asp:Label></td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <th>個人證書</th>
                            <td colspan="3">
                                <asp:Label ID="lb_JCN" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <th class="w2">執業執照字號</th>
                            <td class="w3">
                                <asp:Label ID="lb_LCN" runat="server"></asp:Label></td>
                            <th class="w2">執照有效日期</th>
                            <td class="w3">
                                <asp:Label ID="lb_VEDate" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <th>執業執照是否有效</th>
                            <td>
                                <asp:Label ID="lb_LValid" runat="server"></asp:Label></td>
                            <th>執業狀態</th>
                            <td>
                                <asp:Label ID="lb_LStatus" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <th>機構狀態</th>
                            <td>
                                <asp:Label ID="lb_AbortDate" runat="server"></asp:Label></td>
                            <th>機構類別</th>
                            <td>
                                <asp:Label ID="lb_organClassName" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <th>機構代碼/名稱</th>
                            <td colspan="3">
                                <asp:Label ID="lb_OrganCode" runat="server"></asp:Label>/
                                <asp:Label ID="lb_OrganName" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <th>機構聯絡電話</th>
                            <td colspan="3">
                                <asp:Label ID="lb_OrganTel" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <th>機構地址</th>
                            <td colspan="3">
                                <asp:Label ID="lb_OrganAddr" runat="server"></asp:Label></td>
                        </tr>

                        <tr>
                            <th>備註</th>
                            <td colspan="3">
                                <asp:Label ID="lb_MPNote" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <th class="w2">醫事人員資料庫上傳日期</th>
                            <td class="w3">
                                <asp:Label ID="lb_OwnerUpdateDate" runat="server"></asp:Label></td>
                            <th class="w2">人員執業資料申請/變更日期</th>
                            <td class="w3">
                                <asp:Label ID="lb_GovUpdateDate" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </fieldset>
            </div>

        </div>
        <%} %>
        <div id="tabs-2">
            <div id="accordion-1">
                <h3>有效證明</h3>
                <div>
                    <asp:GridView ID="gv_Certificate" runat="server" AutoGenerateColumns="false" OnRowCreated="gv_Certificate_RowCreated">
                        <Columns>

                           <%-- <asp:BoundField HeaderText="SysChange" DataField="SysChange" />--%>
                            <asp:BoundField HeaderText="證明類型" DataField="CTypeName" />
                            <asp:BoundField HeaderText="首發日期" DataField="CertPublicDate" />
                            <asp:BoundField HeaderText="公告日期" DataField="CertStartDate" />
                            <asp:BoundField HeaderText="到期日期" DataField="CertEndDate" />
                            <asp:BoundField HeaderText="展延" DataField="CertExt" />
                            <asp:BoundField HeaderText="備註" DataField="Note" />
                            <asp:TemplateField HeaderText="下載">
                                <ItemTemplate>
                                    <asp:LinkButton ID="Btn_Print" runat="server" ForeColor="Blue" CommandArgument='<%# Eval("CertSNO") %>' OnClick="Btn_Print_Click">下載</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Label ID="lb_Certificate" runat="server" Visible="false">#尚無取得任何證書</asp:Label>
                </div>
                 <h3>無效證明</h3>
                <div>
                    <div>
                        <asp:GridView ID="gv_Certificate_N" runat="server" AutoGenerateColumns="false" OnRowCreated="gv_Certificate_N_RowCreated">
                            <Columns>
                                <asp:BoundField HeaderText="證明類型" DataField="CTypeName" />
                                <asp:BoundField HeaderText="首發日期" DataField="CertPublicDate" />
                                <asp:BoundField HeaderText="公告日期" DataField="CertStartDate" />
                                <asp:BoundField HeaderText="到期日期" DataField="CertEndDate" />
                                <asp:BoundField HeaderText="展延" DataField="CertExt" />
                                <asp:BoundField HeaderText="備註" DataField="Note" />
                            </Columns>
                        </asp:GridView>
                        <asp:Label ID="lb_CertificateN" runat="server" Visible="false">#尚無任何到期戒菸證書</asp:Label>
                    </div>
                </div>
            </div>


        </div>
        <%if (userInfo.RoleSNO != "14")
            { %>
        <div id="tabs-3">
            <table class="tbl">
                <tr id="tbl_CoursePlanningClass" runat="server">
                    <th>課程規劃名稱</th>
                    <th>參考年度</th>
                    <th>已取得/目標時數</th>
                    <th>可取得的證書</th>
                    <th>時數證明下載(pdf)</th>
                    <th>時數證明下載(word)</th>
                </tr>
                <asp:Repeater ID="rpt_CoursePlanningClass" runat="server">
                    <ItemTemplate>
                        <tr>
                            <asp:Label ID="dd" runat="server" Visible="false" Text='<%# Eval("PClassSNO") %>'></asp:Label>
                            <td>
                                <i id="P1" runat="server" class="fas fa-plus" style="float: left"></i><i id="M1" runat="server" class="fas fa-minus" style="float: left"></i>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="LK_integral" runat="server" CssClass="Link" ForeColor="Blue" OnClientClick="showMSG()" CommandArgument='<%# Eval("PClassSNO") %>' OnClick="LK_integral_Click"><%# Eval("PlanName") %></asp:LinkButton>
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
                            <td class="center">
                                <asp:LinkButton ID="btn_prove_word" runat="server" Text="下載" OnClick="btn_prove_word_Click" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Label ID="lb_CoursePlanningClass" runat="server" Visible="false">#尚無時數紀錄</asp:Label>
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="c_content" class="ContentDiv" runat="server" visible="false">

                    <div>

                        <a onclick="hideMSG()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                        <h1><i class="fa fa-at" aria-hidden="true"></i>時數統計</h1>
                        <table>
                            <tr id="Tr3" runat="server">
                                <th>課程類別</th>
                                <th>授課方式</th>
                                <th>應取得</th>
                                <th>已取得</th>
                                <th>未取得</th>

                            </tr>
                            <asp:Repeater ID="rpt_integralS" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("課程類別") %></td>
                                        <td><%# Eval("授課方式") %></td>
                                        <td><%# Eval("應取得") %></td>
                                        <td>
                                            <asp:LinkButton ID="LK_Done" Class="Done" runat="server" ForeColor="Blue" OnClientClick="showMSGS()" CommandArgument='<%#Eval("PClassSNO") + "," +Eval("Class1")+ "," +Eval("CType")%>' OnClick="LK_Done_Click"><%# Eval("已取得") %></asp:LinkButton></td>
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

                        <h1><i class="fa fa-at" aria-hidden="true"></i>時數紀錄明細</h1>
                        <table>
                            <tr id="Tr4" runat="server">
                                <th>課程類別</th>
                                <th>課程名稱</th>
                                <th>授課方式</th>
                                <th>參考年度</th>
                                <th>時數</th>
                                <th>取得時數</th>
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
                <div id="Div3" class="ContentDivS" runat="server" visible="false">

                    <div>

                        <a onclick="hideMSGSMA()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                        <h1><i class="fa fa-at" aria-hidden="true"></i>時數紀錄明細</h1>
                        <table>
                            <tr id="Tr5" runat="server">
                                <th>上課日期</th>
                                <th>課程名稱</th>

                                <th>辦理單位</th>
                                <th>上課地點</th>
                                <th>時數</th>
                                <th>取得時數</th>
                            </tr>
                            <asp:Repeater ID="Repeater2" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td class="center"><%#Eval("ExamDate") %></td>
                                        <td><%# Eval("CourseName") %></td>

                                        <td class="center"><%# Eval("Unit") %></td>
                                        <td class="center"><%# Eval("ClassLocation") %></td>

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

        <div id="tabs-4">
           
            <table class="tbl">
                <tr id="tbl_ECoursePlanningClass" runat="server" visible="true">
                    <th>課程規劃名稱</th>
                    <th>可認證/目標時數</th>
                    <th>可取得的證書</th>
                    <th>時數證明下載(pdf)</th>
                    <th>時數證明下載(word)</th>
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
                          
                            <td class="center">
                                <%# Eval("PClassTotalHr").ToString()=="" ? "0" : Eval("PClassTotalHr") %> / 
                                <%# Eval("sumHours").ToString()=="" ? "-" : Eval("sumHours") %>
                            </td>
                            <td><%# Eval("CTypeName") %></td>
                            <td class="center">
                                <asp:LinkButton ID="btn_Eprove" runat="server" Text="下載" OnClick="btn_Eprove_Click" /></td>
                            <td class="center">
                                <asp:LinkButton ID="btn_Eprove_word" runat="server" Text="下載" OnClick="btn_Eprove_word_Click" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Label ID="lb_ECoursePlanningClass" runat="server" Visible="false">#尚無時數紀錄</asp:Label>
        </div>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <%--繼續教育積分統計--%>
                <div id="EIntegralSUM" class="ContentDiv" runat="server" visible="false">



                    <a onclick="hideMSGSM()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                    <h1><i class="fa fa-at" aria-hidden="true"></i>繼續教育時數統計</h1>
                   <%-- <h4>此課程Elearning時數只認證<asp:Label ID="lb_ElearnLimit" runat="server" ForeColor="Red"></asp:Label>時數</h4>--%>
                    <table>
                        <thead>
                            <tr>
                                <th colspan="2" style="border: 1px solid white;">Elearning</th>
                                <th rowspan="2" style="border: 1px solid white;">實體時數上傳</th>
                            </tr>
                            <tr>
                                <th>已取得</th>
                                <th>未取得</th>
                            </tr>
                        </thead>
                        <asp:Repeater ID="rpt_EintegralS" runat="server">
                            <ItemTemplate>
                                <tr>

                                    <td>
                                        <asp:LinkButton ID="LK_EDone" ForeColor="Blue" runat="server" OnClientClick="showMSGS()" CommandArgument='<%#Eval("PClassSNO") + "," +Eval("Class1")+","+"1"%>' OnClick="LK_EDone_Click"><%# Eval("已取得") %></asp:LinkButton></td>
                                    <td>
                                        <asp:LinkButton ID="LK_ENotDone" ForeColor="Blue" runat="server" OnClientClick="showMSGS()" CommandArgument='<%#Eval("PClassSNO") + "," +Eval("Class1")+","+"1"%>' OnClick="LK_ENotDone_Click"><%# Eval("未取得") %></asp:LinkButton></td>
                                    <td>
                                        <asp:LinkButton ID="LK_Entity" ForeColor="Blue" runat="server" OnClientClick="showMSGS()" CommandArgument='<%#Eval("EPClassSNO")%>' OnClick="LK_Entity_Click"><%# Eval("實體積分上傳") %></asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>

                </div>
                <%--繼續教育時數紀錄明細--%>
                <div id="EIntegralDetail" class="ContentDivS" runat="server" visible="false">



                    <a onclick="hideMSGSMSM()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                    <h1><i class="fa fa-at" aria-hidden="true"></i>繼續教育時數紀錄明細</h1>
                    <table>
                        <tr id="Tr7" runat="server">
                            <th>課程類別</th>
                            <th>課程名稱</th>
                            <th>授課方式</th>
                            <th>參考年度</th>
                            <th>時數</th>
                            <th>取得時數</th>
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
                <div id="EUploadDetail" class="ContentDiv" runat="server" visible="false">


                    <a onclick="hideMSGSMS()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

                    <h1><i class="fa fa-at" aria-hidden="true"></i>繼續教育時數上傳統計紀錄</h1>
                    <table>
                        <tr id="Tr8" runat="server">
                            <th>課程名稱</th>
                            <th>時數</th>
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

        <div id="tabs-5">
            <table class="tbl">
                <tr id="tbl_Event" runat="server">
                    <th>報名名稱</th>
                    <th>狀態</th>
                    <th>報名日期</th>
                </tr>
                <asp:Repeater ID="Event" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("EventName") %></td>
                            <td><%# Eval("Audit") %></td>
                            <td><%# Eval("CreateDT") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Label ID="lb_Event" runat="server" Visible="false">#尚無課程及活動報名紀錄</asp:Label>
        </div>

        <div id="tabs-6">
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
        </div>

        <div id="tabs-7">
            <table class="tbl">
                <tr id="Tr9" runat="server">
                    <th>e-Learning測驗主題	</th>
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

        <div id="tabs_8" runat="server">
            <div id="tabs-8">
                <span style="color: red"><i class="fa fa-star"></i>此記錄需先通過測驗成績，才會顯示在此。</span>
                <table class="tbl">

                    <tr id="tbl_LearningScore" runat="server">
                        <th>課程名稱</th>
                        <th>證書類型</th>
                        <th>課程類型</th>
                        <th>測驗</th>
                        <th>滿意度調查</th>
                        <th>是否觀看</th>
                        <th>是否取得</th>
                    </tr>
                    <asp:Repeater ID="LearningReport" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("CourseName") %></td>
                                <td><%# Eval("CTypeName") %></td>
                                <td><%# Eval("MVal") %></td>
                                <td><%# Eval("Exam") %></td>
                                <td><%# Eval("Feedback") %></td>
                                <td><%# Eval("Record") %></td>
                                <td><%# Eval("ISNO") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <asp:Label ID="lb_LearningReport" runat="server" Visible="false">#尚無E-Learning時數總表</</asp:Label>
            </div>
        </div>

        <div id="Div2" runat="server">
            <div id="tabs-9">
                <span style="color: red"><i class="fa fa-star"></i>此記錄需先填寫E-Learning滿意度，才會顯示在此。</span>
                <table class="tbl">

                    <tr id="Tr10" runat="server">
                        <th>e-Learning測驗主題</th>
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
        <%} %>
        <%if (userInfo.IsAdmin == true)
            { %>
        <div id="tabs-10">
            <div id="accordion-2">
                <h3>醫事機構合約</h3>
                <div>
                    <table class="tbl">
                        <tr id="Tr11" runat="server">
                            <th>機構代碼</th>
                            <th>機構名稱</th>
                            <th>合約種類</th>
                            <th>合約起始日</th>
                            <th>合約結束日</th>
                            <th>是否治療</th>
                            <th>是否衛教</th>
                        </tr>
                        <asp:Repeater ID="Rpt_SMKContract" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("HospID") %></td>
                                    <td><%# Eval("OrganName") %></td>
                                    <td><%# Eval("SMKContractName") %></td>
                                    <td><%# Eval("PrsnStartDate") %></td>
                                    <td><%# Eval("PrsnEndDate") %></td>
                                    <td><%# Eval("CouldTreat").ToString()=="0"?"否":"是" %></td>
                                    <td><%# Eval("CouldInstruct").ToString()=="0"?"否":"是" %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <asp:Label ID="lb_SMKContract" runat="server" Visible="false">#尚無醫事機構合約</</asp:Label>
                </div>
                <h3>調劑藥局合約</h3>
                <div>
                    <table class="tbl">
                        <tr id="Tr6" runat="server">
                            <th>機構代碼</th>
                            <th>機構名稱</th>
                            <th>是否為調劑藥局</th>
                        </tr>
                        <asp:Repeater ID="Rpt_Pharmacy" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("OrganCode") %></td>
                                    <td><%# Eval("HospName") %></td>
                                    <td><%# Eval("CheckOrg").ToString()==""||Eval("CheckOrg").ToString()==null?"否":"是" %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <asp:Label ID="lb_Pharmacy" runat="server" Visible="false">#尚無調劑藥局合約</</asp:Label>
                </div>
            </div>
        </div>
        <%} %>
    </div>

    <div class="center btns">
        <input name="btnCancel" type="button" value="關閉視窗" onclick="window.close();" />
    </div>
    <script type="text/javascript">

</script>
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

            $("i").each(function (index) {
                console.log(index + ': ' + $(this).text());
            });

        }
        function hideMSGSMA() {
            //$("#c_content").hide("fade");
            document.getElementById("ContentPlaceHolder1_Div3").style.display = "none";//隐藏

        }
        function showMSGS() {
            $("#Div1").show("fade");

        }
        function hideMSGS() {

            document.getElementById("ContentPlaceHolder1_Div1").style.display = "none";//隐藏

        }
        function hideMSGSM() {
            document.getElementById("ContentPlaceHolder1_EIntegralSUM").style.display = "none";//隐藏
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_P1_0').show();
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_0').hide();
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_P1_1').show();
            $('#ContentPlaceHolder1_rpt_ECoursePlanningClass_M1_1').hide();
        }
        function hideMSGSMS() {
            document.getElementById("ContentPlaceHolder1_EUploadDetail").style.display = "none";//隐藏

        }
        function hideMSGSMSM() {
            document.getElementById("ContentPlaceHolder1_EIntegralDetail").style.display = "none";//隐藏

        }
        function hideMSGSMSMS() {
            document.getElementById("ContentPlaceHolder1_EUploadSUM").style.display = "none";//隐藏

        }
        function hideMSG() {
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

            }
        });
        $(".hide").click(function () {
            try {
                document.getElementById("ContentPlaceHolder1_Div1").style.display = "none";//隐藏
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
                document.getElementById("ContentPlaceHolder1_EUploadSUM").style.display = "none";//隐藏
            }
            catch (err) {

            }
        }
        );
        $(".hide").click(function () {
            try {
                document.getElementById("ContentPlaceHolder1_EUploadDetail").style.display = "none";//隐藏
            }
            catch (err) {

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
