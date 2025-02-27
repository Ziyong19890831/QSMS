<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="ReportCertificate.aspx.cs" Inherits="Mgt_ReportCertificate" %>
<%@ Register Assembly="System.Web" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../JS/zTree_v3/js/jquery.blockUI.js"></script>
    <script type="text/javascript">        
        // 分頁功能函數
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }

        function _goPage1(pageNumber) {
            document.getElementById("<%=txt_NPage.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnNPage.ClientID%>").click();
        }

        // 日期驗證
        const MIN_DATE = new Date('2022-11-01');

        function validateDate(dateField) {
            const selectedDate = new Date(dateField.value);
            if (selectedDate < MIN_DATE) {
                alert('因當前課綱自【2022/11/01】才實施，故不允許輸入此日期');
                dateField.value = ''; // 清空欄位
                return false;
            }
            return true;
        }

        $(document).ready(function () {
            // 註冊非同步請求結束後處理
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().beginAsyncPostBack();

            function EndRequestHandler(sender, args) {
                // 初始化日期選擇器
                $("#<%=txt_SPublicDate.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd' });
                $("#<%=txt_EPublicDate.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd' });
                $("#<%=txt_SEndDate.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd' });
                $("#<%=txt_EEndDate.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd' });
                $("#<%=txt_NSPublicDate.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd' });
                $("#<%=txt_NSEndDate.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd' });
                $("#<%=txt_NEEndDate.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd' });
                $("#<%=txt_StartS.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd' });
                $("#<%=txt_StartE.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd' });
                $("#<%=txt_services.ClientID%>").datepicker({ dateFormat: 'yy-mm-dd' });
                
                // 為 txt_GCStartS 添加日期驗證
                $("#<%=txt_GCStartS.ClientID%>").datepicker({
                    dateFormat: 'yy-mm-dd',
                    onSelect: function (dateText, inst) {
                        if ($('#ContentPlaceHolder1_txt_GCStartS').val() != '') {
                            $("#<%=btn_ExportGCTable.ClientID%>, #<%=btn_ExportGCSearch.ClientID%>")
                                .prop('disabled', false)
                                .css({
                                    'background-color': '#FFA500',
                                    'cursor': 'pointer',
                                    'opacity': '1'
                                });
                            validateDate(this);
                        }
                    },
                    onClose: function (dateText, inst) {
                        validateDate(this);
                    }
                });

                // 為 txt_GCStartE 添加日期驗證
                $("#<%=txt_GCStartE.ClientID%>").datepicker({
                    dateFormat: 'yy-mm-dd',
                    onSelect: function (dateText, inst) {
                        if ($('#ContentPlaceHolder1_txt_GCStartS').val() == '') {
                            alert('請輸入起日');
                            $("#<%=btn_ExportGCTable.ClientID%>, #<%=btn_ExportGCSearch.ClientID%>")
                                .prop('disabled', true)
                                .css({
                                    'background-color': '#cccccc',
                                    'cursor': 'not-allowed',
                                    'opacity': '0.6'
                                });
                        }
                        validateDate(this);
                    },
                    onClose: function (dateText, inst) {
                        validateDate(this);
                    }
                });

                // 手動輸入驗證
                $("#<%=txt_GCStartS.ClientID%>").change(function () {
                    validateDate(this);
                });

                $("#<%=txt_GCStartE.ClientID%>").change(function () {
                    validateDate(this);
                });
            }
        });

        // 手風琴初始化
        $(function () {
            $("#accordion").accordion();
        });

        // 驗證證書選擇
        function ValidateModuleList(source, args) {
            if ($('#ContentPlaceHolder1_chk_Certificates').is(":checked")) {
                var chkListModules = document.getElementById('<%= CBL_Certificates.ClientID %>');
                var chkListinputs = chkListModules.getElementsByTagName("input");
                for (var i = 0; i < chkListinputs.length; i++) {
                    if (chkListinputs[i].checked) {
                        args.IsValid = true;
                        $("#Note_content").hide("fade");
                        return;
                    }
                }
                args.IsValid = false;
                $('#ContentPlaceHolder1_chk_Certificates').attr('checked', false);
            }
        }
    </script>

    <style type="text/css">
        /* 歷史記錄勾選框樣式 */
        #ContentPlaceHolder1_chk_his {
            margin-top: 10px;
        }

        /* 內容區塊樣式 */
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
            position: fixed;
            text-align: center;
            line-height: 25px;
            display: none;
        }

        /* 標籤樣式 */
        #ContentPlaceHolder1_Lb_CerNum {
            padding: 0 50px;
            margin: 0 50px;
            display: inline-block;
        }

        #ContentPlaceHolder1_lb_ServiceCount {
            padding: 0 50px;
            margin: 0 50px;
            display: inline-block;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- 頁面導覽 -->
    <div class="path txtS mb20">現在位置：<a href="#">報表作業</a> <i class="fa fa-angle-right"></i><a href="ReportCertificate.aspx">證書名冊</a></div>
    
    <!-- Script管理器 -->
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <!-- 證書資料表格 -->
    <asp:GridView ID="gv_Course" runat="server" AutoGenerateColumns="False">
        <columns>
            <asp:BoundField HeaderText="序號" DataField="ROW_NO">
                <itemstyle horizontalalign="Center" />
            </asp:BoundField>
            <asp:BoundField HeaderText="證號" DataField="CertID" />
            <asp:BoundField HeaderText="證書類型" DataField="CTypeName" />
            <asp:BoundField HeaderText="發證單位" DataField="CUnitName" />
            <asp:BoundField HeaderText="學員名稱" DataField="PName" />
            <asp:BoundField HeaderText="身分證" DataField="PersonID_encryption" />
            <asp:BoundField HeaderText="首發日期" DataField="CertPublicDate" />
            <asp:BoundField HeaderText="公告日期" DataField="CertStartDate" />
            <asp:BoundField HeaderText="到期日期" DataField="CertEndDate" />
            <asp:BoundField HeaderText="展延" DataField="CertExt">
                <itemstyle horizontalalign="Center" />
            </asp:BoundField>
        </columns>
    </asp:GridView>

    <!-- 分頁控制項 -->
    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
    
    <!-- 手風琴區域開始 -->
    <div class="both mb20">
        <div id="accordion">
            <!-- 證書名冊區段 -->
            <h3>證書名冊</h3>
            <div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <contenttemplate>
                        <div class="both mb20" style="height: 380px">
                            <fieldset>
                                <legend>功能列</legend>
                                <div class="left w8">
                                    角色<asp:DropDownList ID="ddl_Role" runat="server" DataValueField="RoleSNO" DataTextField="RoleName"></asp:DropDownList>
                                    證書類型<asp:DropDownList ID="ddl_CType" runat="server" DataValueField="CTypeSNO" DataTextField="CTypeName"></asp:DropDownList>
                                    學員名稱<asp:TextBox ID="txt_PName" runat="server" class="w1"></asp:TextBox>
                                    身分證<asp:TextBox ID="txt_PersonID" runat="server" class="w1"></asp:TextBox><br />
                                    展延<asp:DropDownList ID="ddl_CertExt" runat="server" DataValueField="RoleSNO" DataTextField="RoleName"></asp:DropDownList>
                                    首發日期<asp:TextBox ID="txt_SPublicDate" class="datepicker w1" runat="server" type="text"></asp:TextBox>-
                                    <asp:TextBox ID="txt_EPublicDate" class="datepicker w1" runat="server" type="text"></asp:TextBox>
                                    到期日期   
                                    <asp:TextBox ID="txt_SEndDate" class="datepicker w1" runat="server" type="text"></asp:TextBox>-
                                    <asp:TextBox ID="txt_EEndDate" class="datepicker w1" runat="server" type="text"></asp:TextBox>
                                    <br />
                                    <asp:CheckBox ID="chk_his" runat="server" CssClass="" Text="不查詢過期證書" />
                                    <asp:CheckBox ID="chk_Open" runat="server" Text="開啟進階功能" OnCheckedChanged="chk_Open_CheckedChanged" AutoPostBack="true" />
                                    <asp:CheckBox ID="chk_Certificates" runat="server" AutoPostBack="true" OnCheckedChanged="chk_Certificates_CheckedChanged" Text="複選證書" />
                                </div>
                                <div class="right">
                                    <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                                    <asp:Button ID="btnExport" runat="server" Text="匯出" OnClick="btnExport_Click" />
                                </div>
                                <asp:Panel ID="p1" runat="server" Class="left w8" Visible="false">
                                    <asp:FileUpload ID="file_Upload" runat="server" />
                                    <asp:Button ID="btnECertificate" runat="server" Text="匯出電子證書" OnClick="btnECertificate_Click" />
                                </asp:Panel>
                                <asp:Panel ID="p2" runat="server" Class="center w8" Visible="false">
                                    <asp:CheckBoxList ID="CBL_Certificates" AutoPostBack="True" RepeatColumns="6" RepeatDirection="Horizontal" runat="server" DataTextField="CtypeName" DataValueField="CtypeSNO"></asp:CheckBoxList>
                                    <asp:CustomValidator runat="server" ID="cvmodulelist" ClientValidationFunction="ValidateModuleList" ErrorMessage="請至少選擇一項" ForeColor="Red"></asp:CustomValidator>
                                </asp:Panel>
                            </fieldset>
                        </div>
                    </contenttemplate>
                    <triggers>
                        <asp:PostBackTrigger ControlID="btnPage"></asp:PostBackTrigger>
                        <asp:PostBackTrigger ControlID="btnExport"></asp:PostBackTrigger>
                        <asp:PostBackTrigger ControlID="btnSearch"></asp:PostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="chk_Certificates"></asp:AsyncPostBackTrigger>
                    </triggers>
                </asp:UpdatePanel>
            </div>
            
            <!-- 相關數據統計區段 -->
            <h3>相關數據統計</h3>
            <div>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <contenttemplate>
                        <!-- 縣市舊制證書統計 -->
                        <fieldset>
                            <legend>各縣市舊制證書取得人數統計(原「培訓認證人數統計」)</legend>
                            公告日期
                            <asp:TextBox ID="txt_StartS" class="datepicker w1" runat="server" type="text"></asp:TextBox>-
                            <asp:TextBox ID="txt_StartE" class="datepicker w1" runat="server" type="text"></asp:TextBox>
                            <asp:Button ID="btn_ExportCTable" runat="server" Text="匯出EXCEL" OnClick="btn_ExportCTable_Click" />
                            <asp:Button ID="btn_ExportCTableODS" runat="server" Text="匯出ODS" OnClick="btn_ExportCTableODS_Click" />
                        </fieldset>
                        
                        <!-- 新訓取得證書人數 -->
                        <fieldset>
                            <legend>新訓取得證書人數</legend>
                            取得日期
                            <asp:TextBox ID="txt_GCStartS" class="datepicker w1" runat="server" type="text"></asp:TextBox>-
                            <asp:TextBox ID="txt_GCStartE" class="datepicker w1" runat="server" type="text"></asp:TextBox>
                            共<asp:Label ID="Lb_CerNum" runat="server"></asp:Label>人
                            <asp:Button ID="btn_ExportGCSearch" runat="server" Text="查詢" OnClick="btn_ExportGCSearch_Click" />
                            <asp:Button ID="btn_ExportGCTable" runat="server" Text="匯出" OnClick="btn_ExportGCTable_Click" />
                        </fieldset>
                        
                        <!-- 具戒菸服務資格人數 -->
                        <fieldset>
                            <legend>具戒菸服務資格人數</legend>
                            截止日期
                           
                            <asp:TextBox ID="txt_services" class="datepicker w1" runat="server" type="text"></asp:TextBox>
                            共<asp:Label ID="lb_ServiceCount" runat="server"></asp:Label>人
                            <asp:Button ID="btn_ExportServiceSearch" runat="server" Text="查詢" OnClick="btn_ExportServiceSearch_Click" />
                            <asp:Button ID="btn_ExportServiceTable" runat="server" Text="匯出" OnClick="btn_ExportServiceTable_Click" />
                        </fieldset>
                    </contenttemplate>
                </asp:UpdatePanel>
            </div>
            
            <!-- 身分證上傳查詢區段 -->
            <h3>身分證上傳查詢</h3>
            <div>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <contenttemplate>
                        <fieldset>
                            <legend>功能列</legend>
                            <asp:FileUpload ID="FU_load" runat="server" />
                            <asp:Button ID="btn_Upload" Text="上傳身分證" runat="server" OnClick="btn_Upload_Click" />
                            <asp:Button ID="btn_Templte" runat="server" OnClick="btn_Templte_Click" Text="範本下載" />
                        </fieldset>
                    </contenttemplate>
                    <triggers>
                        <asp:PostBackTrigger ControlID="btn_Templte"></asp:PostBackTrigger>
                        <asp:PostBackTrigger ControlID="btn_Upload"></asp:PostBackTrigger>
                    </triggers>
                </asp:UpdatePanel>
            </div>
            
            <!-- 未申請帳號之證書名冊區段 -->
            <h3>未申請帳號之證書名冊</h3>
            <div>
                <asp:UpdatePanel ID="Up1" runat="server">
                    <contenttemplate>
                        <div class="both mb20">
                            <fieldset>
                                <legend>功能列</legend>
                                <div class="left w8">
                                    角色<asp:DropDownList ID="ddl_NRole" runat="server" DataValueField="RoleSNO" DataTextField="RoleName"></asp:DropDownList>
                                    證書類型<asp:DropDownList ID="ddl_NCType" runat="server" DataValueField="CTypeSNO" DataTextField="CTypeName"></asp:DropDownList>
                                    學員名稱<asp:TextBox ID="txt_NPName" runat="server" class="w1"></asp:TextBox>
                                    身分證<asp:TextBox ID="txt_NPersonID" runat="server" class="w1"></asp:TextBox><br />
                                    展延<asp:DropDownList ID="ddl_NCertExt" runat="server" DataValueField="RoleSNO" DataTextField="RoleName"></asp:DropDownList>
                                    首發日期
                                    <asp:TextBox ID="txt_NSPublicDate" class="datepicker w1" runat="server" type="text"></asp:TextBox>- 
                                    <asp:TextBox ID="txt_NEPublicDate" class="datepicker w1" runat="server" type="text"></asp:TextBox>
                                    到期日期
                                    <asp:TextBox ID="txt_NSEndDate" class="datepicker w1" runat="server" type="text"></asp:TextBox>- 
                                    <asp:TextBox ID="txt_NEEndDate" class="datepicker w1" runat="server" type="text"></asp:TextBox>
                                    <br />
                                    <asp:CheckBox ID="chk_Nhis" runat="server" CssClass="" Text="不查詢過期證書" />
                                    <p style="color: red">如有姓名為空白，則此證書之身分證無法繫結至醫事人員資料庫</p>
                                </div>
                                <div class="right">
                                    <asp:Button ID="btnNSearch" runat="server" Text="查詢" OnClick="btnNSearch_Click" />
                                    <%--<asp:Button ID="btnNExport" runat="server" Text="匯出" OnClick="btnNExport_Click" />--%>
                                </div>
                            </fieldset>
                        </div>
                        
                        <!-- 未申請帳號證書資料表格 -->
                        <asp:GridView ID="gv_NCourse" runat="server" AutoGenerateColumns="False">
                            <columns>
                                <asp:BoundField HeaderText="序號" DataField="ROW_NO">
                                    <itemstyle horizontalalign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="證號" DataField="CertID" />
                                <asp:BoundField HeaderText="證書類型" DataField="CTypeName" />
                                <asp:BoundField HeaderText="發證單位" DataField="CUnitName" />
                                <asp:BoundField HeaderText="學員名稱" DataField="PName" />
                                <asp:BoundField HeaderText="身分證" DataField="PersonID_encryption" />
                                <asp:BoundField HeaderText="首發日期" DataField="CertPublicDate" />
                                <asp:BoundField HeaderText="公告日期" DataField="CertStartDate" />
                                <asp:BoundField HeaderText="到期日期" DataField="CertEndDate" />
                                <%--<asp:BoundField HeaderText="展延" DataField="CertExt">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>--%>
                            </columns>
                        </asp:GridView>
                        
                        <!-- 未申請帳號證書分頁控制項 -->
                        <asp:Literal ID="ltl_NPageNumber" runat="server"></asp:Literal>
                        <asp:HiddenField ID="txt_NPage" runat="server" />
                        <asp:Button ID="btnNPage" runat="server" Text="查詢" OnClick="btnNPage_Click" Style="display: none;" />
                    </contenttemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>