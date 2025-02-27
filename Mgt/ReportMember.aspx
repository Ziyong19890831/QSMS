<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="ReportMember.aspx.cs" Inherits="Mgt_ReportMember" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
        $(function () {
            $("#accordion").accordion();
        });
        $(function () {
            $(".datepicker").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });

    </script>
    <style type="text/css">
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

        #D {
            background: #ccc;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="path txtS mb20">現在位置：<a href="#">報表作業</a> <i class="fa fa-angle-right"></i><a href="ReportMember.aspx">學員名冊</a></div>

    <div class="right">
        <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
        <asp:Button ID="btnExport" runat="server" Text="總表匯出" OnClick="btnExport_Click" />
    </div>
    <asp:GridView ID="gv_Course" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField HeaderText="序號" DataField="ROW_NO">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField HeaderText="單位代碼" DataField="OrganCode" />
            <asp:BoundField HeaderText="單位名稱" DataField="OrganName" />
            <asp:BoundField HeaderText="角色" DataField="RoleName" />
            <asp:BoundField HeaderText="姓名" DataField="PName" />
            <asp:BoundField HeaderText="身分證" DataField="PersonID_encryption" />
            <asp:BoundField HeaderText="電話" DataField="PTel_O" />
            <asp:BoundField HeaderText="手機" DataField="PPhone" />
            <asp:BoundField HeaderText="信箱" DataField="PMail" ItemStyle-Width="100" ItemStyle-Wrap="true" />
            <asp:BoundField HeaderText="狀態" DataField="MName" />
            <asp:TemplateField HeaderText="查看" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <a href="#" onclick="var winvar = window.open('./ReportMemberDetail.aspx?sno=<%#Eval("PersonSNO").ToString() %>','winname','width=1200 height=550 location=no,menubar=no status=no,toolbar=no');"><i class="fa fa-pen-square"></i></a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <a href="#" onclick="var winvar = window.open('./Personnel_AE.aspx?a=&n=NN&link=1&sno=<%#Eval("PersonSNO").ToString() %>','winname','width=1200 height=550 location=no,menubar=no status=no,toolbar=no');"><i class="fa fa-pen-square"></i></a>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

    <div class="both mb20">
        <div id="accordion">
            <h3>基本資料</h3>
            <div>

                <asp:UpdatePanel ID="upl_ddl" runat="server" UpdateMode="Conditional" style="display: inline;">
                    <ContentTemplate>
                        行政區
                         <% if (userInfo.RoleOrganType != "A")
                             { %>
                        <asp:DropDownList ID="ddl_AreaCodeA" runat="server" OnSelectedIndexChanged="ddl_AreaCodeA_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                        </asp:DropDownList>
                        <% } %>
                        <asp:DropDownList ID="ddl_AreaCodeB" runat="server" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                            <asp:ListItem Value="" Text="請先選擇縣市行政區"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddl_AreaCodeA" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                自填單位代碼<asp:TextBox ID="txt_UnitCode" runat="server" class="w1"></asp:TextBox>
                自填單位名稱<asp:TextBox ID="txt_UnitName" runat="server" class="w1"></asp:TextBox>
                <br />

                學員姓名<asp:TextBox ID="txt_PName" runat="server" class="w1"></asp:TextBox>
                學員帳號<asp:TextBox ID="txt_PAccount" runat="server" class="w1"></asp:TextBox><br />
                角色<asp:DropDownList ID="ddl_Role" runat="server" DataValueField="RoleSNO" DataTextField="RoleName"></asp:DropDownList>
                角色別2<asp:DropDownList ID="ddl_Role2" runat="server" DataValueField="RESNO" DataTextField="REName"></asp:DropDownList>
                會員狀態<asp:DropDownList ID="ddl_Stutas" runat="server">
                    <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                    <asp:ListItem Text="正常" Value="0"></asp:ListItem>
                    <asp:ListItem Text="欠繳" Value="1"></asp:ListItem>
                    <asp:ListItem Text="不詳" Value="3"></asp:ListItem>
                    <asp:ListItem Text="退會(原)" Value="4"></asp:ListItem>
                </asp:DropDownList>
                性別<asp:DropDownList ID="ddl_Sex" runat="server">
                    <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                    <asp:ListItem Text="女" Value="0"></asp:ListItem>
                    <asp:ListItem Text="男" Value="1"></asp:ListItem>
                </asp:DropDownList>
                出生年月日<asp:TextBox ID="txt_Birthday" runat="server" class="datepicker w1"></asp:TextBox><br />
                學員帳號開始日期<asp:TextBox ID="txt_Accountday" runat="server" class="datepicker w1"></asp:TextBox>
                身分證<asp:TextBox ID="txt_PersonID" runat="server" class="w1"></asp:TextBox>
                <br />
                E-mail<asp:TextBox ID="txt_mail" runat="server" class="w1" Width="200px"></asp:TextBox><br />
                服務單位科別<asp:DropDownList ID="ddl_TSSNO" DataTextField="TsTypeName" DataValueField="TSSNO" runat="server"></asp:DropDownList>
                學員狀態<asp:DropDownList ID="ddl_Status" runat="server" DataTextField="MName" DataValueField="MStatusSNO"></asp:DropDownList>
                <asp:Panel ID="P_userinfo" runat="server" Visible="true">
                    通訊地址_縣市<asp:CheckBox ID="chk_City" runat="server" OnCheckedChanged="chk_City_CheckedChanged" AutoPostBack="true" />
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            通訊地址_鄉鎮區<asp:CheckBox ID="chk_City1" runat="server" Enabled="false" AutoPostBack="true" OnCheckedChanged="chk_City1_CheckedChanged" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chk_City" />
                        </Triggers>

                    </asp:UpdatePanel>
                </asp:Panel>

            </div>
            <h3>戒菸證書資料</h3>
            <div>
                加入證書表<asp:CheckBox ID="cb_Certificate" runat="server" AutoPostBack="true" />
                證書名稱<asp:DropDownList ID="ddl_Certificate" runat="server" DataTextField="CTypeName" DataValueField="CTypeSNO"></asp:DropDownList><br />
                證號<asp:TextBox ID="txt_CertS" runat="server"></asp:TextBox>~<asp:TextBox ID="txt_CertE" runat="server"></asp:TextBox><br />
                證書首發日<asp:TextBox ID="txt_CertPDateS" Class="datepicker" runat="server"></asp:TextBox>~<asp:TextBox ID="txt_CertPDateE" Class="datepicker" runat="server"></asp:TextBox><br />
                證書公告日<asp:TextBox ID="txt_CertSDateS" Class="datepicker" runat="server"></asp:TextBox>~<asp:TextBox ID="txt_CertSDateE" Class="datepicker" runat="server"></asp:TextBox><br />
                證書到期日<asp:TextBox ID="txt_CertEDateS" Class="datepicker" runat="server"></asp:TextBox>~<asp:TextBox ID="txt_CertEDateE" Class="datepicker" runat="server"></asp:TextBox>
                查詢最新一筆<asp:CheckBox ID="CheckBox1" runat="server" Enabled="false" AutoPostBack="true" />
                查詢證書歷程資料<asp:CheckBox ID="CheckBox2" runat="server" Enabled="false" AutoPostBack="true" />
                <br />
                證書排序：<asp:RadioButtonList ID="rbl_CertificateGroup" runat="server">
                    <asp:ListItem Value="0" Text="取得時間"></asp:ListItem>
                    <asp:ListItem Value="0" Text="公開日"></asp:ListItem>
                    <asp:ListItem Value="0" Text="首發日"></asp:ListItem>
                    <asp:ListItem Value="0" Text="到期日"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <h3>醫事人員表</h3>
            <div>
                畢業學校<asp:TextBox ID="txt_GradSchool" runat="server"></asp:TextBox>
                學位<asp:TextBox ID="txt_Degree" runat="server"></asp:TextBox><br />
                職稱<asp:TextBox ID="txt_Jtype" runat="server"></asp:TextBox><br />
                證書字號<asp:TextBox ID="txt_JCN" runat="server"></asp:TextBox>
                證書核發日期<asp:TextBox ID="txt_JDate" Class="datepicker" runat="server"></asp:TextBox><br />
                執照有效期間(起)<asp:TextBox ID="txt_VSDate" Class="datepicker" runat="server"></asp:TextBox><br />
                執照有效期間(訖)<asp:TextBox ID="txt_VEDate" Class="datepicker" runat="server"></asp:TextBox><br />
                執業執照字號<asp:TextBox ID="txt_LCN" runat="server"></asp:TextBox>
                執業執照是否有效<asp:DropDownList ID="ddl_Lvalid" runat="server">
                    <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                    <asp:ListItem Text="有效" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="無效" Value="N"></asp:ListItem>
                </asp:DropDownList>
                執業狀態<asp:DropDownList ID="ddl_LStatus" runat="server">
                    <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                    <asp:ListItem Text="執業" Value="執業"></asp:ListItem>
                    <asp:ListItem Text="歇業" Value="歇業"></asp:ListItem>
                </asp:DropDownList><br />
                執業登記科別<asp:TextBox ID="txt_LRType" runat="server"></asp:TextBox>
                部定專科科別<asp:TextBox ID="txt_LSType" runat="server"></asp:TextBox>
            </div>
            <h3>課程</h3>
            <div>
                <asp:CheckBox ID="cb_Enable" runat="server" Text="啟用課程資料" AutoPostBack="true" OnCheckedChanged="cb_Enable_CheckedChanged" />
                <asp:Panel ID="P_Learning" runat="server" Enabled="false">
                    Elearning課程名稱<asp:TextBox ID="txt_ElearningCourseName" runat="server"></asp:TextBox>
                    Elearning課程規劃名稱<asp:TextBox ID="txt_ElearningPLanName" runat="server"></asp:TextBox><br />
                    課程觀看完成日<asp:TextBox ID="txt_EViewSDate" Class="datepicker" runat="server"></asp:TextBox>~<asp:TextBox ID="txt_EViewEDate" Class="datepicker" runat="server"></asp:TextBox><br />
                    測驗完成日   
                        <asp:TextBox ID="txt_ETestSDate" Class="datepicker" runat="server"></asp:TextBox>~<asp:TextBox ID="txt_ETestEDate" Class="datepicker" runat="server"></asp:TextBox><br />
                    滿意度完成日 
                        <asp:TextBox ID="txt_EFeedSDate" Class="datepicker" runat="server"></asp:TextBox>~<asp:TextBox ID="txt_EFeedEDate" Class="datepicker" runat="server"></asp:TextBox>
                    測驗是否通過 
                        <asp:DropDownList ID="ddl_IsPass" runat="server">
                            <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                            <asp:ListItem Text="沒通過" Value="0"></asp:ListItem>
                            <asp:ListItem Text="通過" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                </asp:Panel>

            </div>
            <h3>機構合約</h3>
            <div>
                <asp:CheckBox ID="cb_Contract" runat="server" Text="啟用機構合約" AutoPostBack="true" Checked="false" OnCheckedChanged="cb_Contract_CheckedChanged" />
            </div>
        </div>

        <asp:Label ID="lb_chkl_City" runat="server"></asp:Label><br />
        <asp:Label ID="lb_chkl_City_Code" runat="server"></asp:Label>
        <asp:Label ID="lb_chkl_City1" runat="server"></asp:Label>
        <!--只有按下按鈕時才會出現-->
        <div id="Note_content" class="ContentDiv">

            <div style="background-color: white; opacity: 0.95; padding: 20px 30px; border-radius: 5px; margin: 100px auto 0 auto; z-index: 999; width: 700px; height: 550px; overflow: auto">
                <a onclick="hideNote()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>
                <h1><i class="fa fa-at" aria-hidden="true"></i>通訊地址_縣市</h1>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:CheckBoxList ID="chkl_City" Name="City" runat="server" AutoPostBack="True" RepeatColumns="4" DataTextField="AREA_NAME" DataValueField="AREA_CODE"></asp:CheckBoxList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="chk_City" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="MarginTop">
                    <asp:Button ID="btnOK" runat="server" Text="確認" OnClick="btnOK_Click" />
                </div>
            </div>

        </div>

        <!--只有按下按鈕時才會出現-->
        <div id="Note1_content" class="ContentDiv">
            <div style="background-color: white; opacity: 0.95; padding: 20px 30px; border-radius: 5px; margin: 100px auto 0 auto; z-index: 999; width: 700px; height: 550px; overflow: auto;">
                <a onclick="hideNotea()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>
                <h1><i class="fa fa-at" aria-hidden="true"></i>通訊地址_鄉鎮區</h1>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:CheckBoxList ID="CheckBoxList1" Name="city1" runat="server" AutoPostBack="True" RepeatColumns="4" DataTextField="AREA_NAME" DataValueField="AREA_CODE"></asp:CheckBoxList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="chk_City1" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="MarginTop">
                    <asp:Button ID="btnOK1" runat="server" Text="確認" OnClick="btnOK1_Click" />
                </div>
            </div>
        </div>




    </div>

    <script type="text/javascript">
        $('#ContentPlaceHolder1_chk_City').change(function () {
            if ($(this).is(':checked')) {
                showMSGa();
            }
            else {
                $('#ContentPlaceHolder1_lb_chkl_City').text('');
                $('#ContentPlaceHolder1_lb_chkl_City_Code').text('');
            }
        });
        $('#ContentPlaceHolder1_chk_City1').change(function () {
            if ($(this).is(':checked')) {
                showMSG();
            }
            else {
                $('#ContentPlaceHolder1_lb_chkl_City1').text('');
            }
        });
        function showMSGa() {
            $("#Note_content").show("fade");
        }

        function showMSG() {
            if ($('#ContentPlaceHolder1_chk_City1').is(':checked')) {
                $("#Note1_content").show("fade");
            }

        }
        function hideNote() {
            $("#Note_content").hide("fade");
            $('#ContentPlaceHolder1_chkl_City').find("input[type='Checkbox']").each(function () {
                if ($(this).is(':checked')) {

                }
                else {

                    $("#ContentPlaceHolder1_chk_City").prop("checked", false);
                }

            })

        }
        function hideNotea() {
            $("#Note1_content").hide("fade");
            $('#ContentPlaceHolder1_CheckBoxList1').find("input[type='Checkbox']").each(function () {
                if ($(this).is(':checked')) {

                }
                else {

                    $("#ContentPlaceHolder1_chk_City1").prop("checked", false);
                }

            })
        }
    </script>
</asp:Content>

