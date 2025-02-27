<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="CertificateChange.aspx.cs" Inherits="Mgt_CertificateChange" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../JS/zTree_v3/js/jquery.blockUI.js"></script>
    <script language="javascript" type="text/javascript">
        $(function () {

            $(".youpi").datepicker({
                changeYear : true,
                yearRange : "2010:2050",
                dateFormat: 'yy/mm/dd'
            });
        });

    </script>
    <style type="text/css">
        #ContentPlaceHolder1_btn_OK {
            background-color: gray;
        }
    </style>
    <script type="text/javascript">

        
        function ca() {
            $('input[id="ContentPlaceHolder1_gv_SetValue_txt_AllRecover_1"]').on('change', function () {
                var myDate = new Date($(this).val());
                var CertPublicDate;
                var CertEndDate;
                var CertStartDate;
                var UnitChose;
                myDate.setFullYear(myDate.getFullYear() + 6);
                myDate.setDate(myDate.getDate() - 1);
                myDate.setMonth(myDate.getMonth() );
                var curr_day = myDate.getDate();
                var curr_month = myDate.getMonth()+ 1;
                var curr_year = myDate.getFullYear();
                $('input[class="CertStartDate"').val($(this).val());
                $('input[class="CertEndDate"').val(curr_year + '/' + 12 + '/' + 31);
                $('input[class="CertStartDate"').each(function () {
                    CertPublicDate = ($(this).val());
                });
                $('input[class="CertEndDate"').each(function () {
                    CertEndDate = ($(this).val());
                });
                $('input[class="CertPublicDate"').each(function () {
                    CertStartDate = ($(this).val());
                });
                $('select[class="UnitChose"').each(function () {
                    UnitChose = ($(this).val());
                });            
                if (CertPublicDate != '' && CertEndDate != '' && CertStartDate != '' && UnitChose !='') {
                    $("#ContentPlaceHolder1_btn_OK").attr("disabled", false);
                    $("#ContentPlaceHolder1_btn_OK").css("background-color", "#f9bf3b");
                }
                else {
                    $("#ContentPlaceHolder1_btn_OK").attr("disabled", true);
                    $("#ContentPlaceHolder1_btn_OK").css("background-color", "gray");
                }
            });
            $('input[id="ContentPlaceHolder1_gv_SetValue_txt_AllRecover"]').on('change', function () {
                var CertPublicDate_1;
                var CertEndDate_1;
                var CertStartDate_1;              
                $('input[class="CertPublicDate"').val($(this).val());

                $('input[class="CertStartDate"').each(function () {
                    CertPublicDate_1 = ($(this).val());
                });
                $('input[class="CertEndDate"').each(function () {
                    CertEndDate_1 = ($(this).val());
                });
                $('input[class="CertPublicDate"').each(function () {
                    CertStartDate_1 = ($(this).val());
                });
                $('select[class="UnitChose"').each(function () {
                    UnitChose = ($(this).val());
                });
                if (CertPublicDate_1 != '' && CertEndDate_1 != '' && CertStartDate_1 != '' && UnitChose !='') {
                    $("#ContentPlaceHolder1_btn_OK").attr("disabled", false);
                    $("#ContentPlaceHolder1_btn_OK").css("background-color", "#f9bf3b");
                }
                else {
                    $("#ContentPlaceHolder1_btn_OK").attr("disabled", true);
                    $("#ContentPlaceHolder1_btn_OK").css("background-color", "gray");
                }
            });

            $('input[id="ContentPlaceHolder1_gv_SetValue_txt_CertEndDate"]').on('change', function () {
                var CertPublicDate_1;
                var CertEndDate_1;
                var CertStartDate_1;              
                $('input[class="CertEndDate"').val($(this).val());

                $('input[class="CertStartDate"').each(function () {
                    CertPublicDate_1 = ($(this).val());
                });
                $('input[class="CertPublicDate"').each(function () {
                    CertEndDate_1 = ($(this).val());
                });
                $('input[class="CertPublicDate"').each(function () {
                    CertStartDate_1 = ($(this).val());
                });
                $('select[class="UnitChose"').each(function () {
                    UnitChose = ($(this).val());
                });
                if (CertPublicDate_1 != '' && CertEndDate_1 != '' && CertStartDate_1 != '' && UnitChose !='') {
                    $("#ContentPlaceHolder1_btn_OK").attr("disabled", false);
                    $("#ContentPlaceHolder1_btn_OK").css("background-color", "#f9bf3b");
                }
                else {
                    $("#ContentPlaceHolder1_btn_OK").attr("disabled", true);
                    $("#ContentPlaceHolder1_btn_OK").css("background-color", "gray");
                }
            });
         
      
        };
        $(function () {
            var Aselect = $("#ContentPlaceHolder1_gv_SetValue_ddl_AuditH");
            $(Aselect).change(function () {
                $("td select").val(Aselect.val());
                var CertPublicDate_1;
                var CertEndDate_1;
                var CertStartDate_1;              
                //$('input[class="CertPublicDate"').val($(this).val());

                $('input[class="CertStartDate"').each(function () {
                    CertPublicDate_1 = ($(this).val());
                });
                $('input[class="CertEndDate"').each(function () {
                    CertEndDate_1 = ($(this).val());
                });
                $('input[class="CertPublicDate"').each(function () {
                    CertStartDate_1 = ($(this).val());
                });
                $('select[class="UnitChose"').each(function () {
                    UnitChose = ($(this).val());
                });
                if (CertPublicDate_1 != '' && CertEndDate_1 != '' && CertStartDate_1 != '' && UnitChose !='') {
                    $("#ContentPlaceHolder1_btn_OK").attr("disabled", false);
                    $("#ContentPlaceHolder1_btn_OK").css("background-color", "#f9bf3b");
                }
                else {
                    $("#ContentPlaceHolder1_btn_OK").attr("disabled", true);
                    $("#ContentPlaceHolder1_btn_OK").css("background-color", "gray");
                }
            });
        });
    </script>
    <script type="text/javascript">
        function OKClick() {
            if ($('input[class="CertStartDate"').val() > $('input[class="CertPublicDate"').val()) {
                alert('公告日不得大於首發日');
                event.preventDefault();
            }
            else if ($('input[class="CertPublicDate"').val() > $('input[class="CertStartDate"').val()) {
                alert('首發日不得大於公告日');
                event.preventDefault();
            }
            else { }
        }
    </script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('input[class="CertPublicDate"').focusout(function () {
                $('input[class="CertPublicDate"').attr("disabled", true);
                $('input[class="CertPublicDate"').attr("disabled", false);
                var DateType = dateValidationCheck($(this).val());
                if ($(this).val() == '') {
                    alert('不得空值');
                }
                else if (DateType == false) {
                    alert("請輸入 YYYY/MM/DD 日期格式");
                }
                var ErrorCount = 0;
                var CertPublicDate = document.getElementsByClassName('CertPublicDate');
                var CertPublicDateLen = CertPublicDate.length;
                var CertEndDate = document.getElementsByClassName('CertEndDate');
                var CertStartDate = document.getElementsByClassName('CertStartDate');
                for (var i = 0; i < CertPublicDateLen; i++) {
                    if (document.getElementsByClassName('Unit')[i].value == '') {
                        alert('Error')
                        ErrorCount++;
                    }
                    if (CertPublicDate[i].value == '') {
                        ErrorCount++;
                    }
                    if (CertEndDate[i].value == '') {
                        ErrorCount++;
                    }
                    if (CertStartDate[i].value == '') {
                        ErrorCount++;
                    }
                    if (dateValidationCheck(CertPublicDate[i].value) == false) {
                        ErrorCount++;
                    }
                    if (dateValidationCheck(CertEndDate[i].value) == false) {
                        ErrorCount++;
                    }
                    if (dateValidationCheck(CertStartDate[i].value) == false) {
                        ErrorCount++;
                    }
                }
                if (ErrorCount > 0) {
                    $("#ContentPlaceHolder1_btn_OK").attr("disabled", true);
                    $("#ContentPlaceHolder1_btn_OK").css("background-color", "gray");

                    return;
                }
                else {
                    $("#ContentPlaceHolder1_btn_OK").attr("disabled", false);
                    $("#ContentPlaceHolder1_btn_OK").css("background-color", "#f9bf3b");

                }

                console.log(ErrorCount);
            });
        });




        $(document).ready(function () {
            $('input[class="CertEndDate"').focusout(function () {
                $('input[class="CertEndDate"').attr("disabled", true);
                $('input[class="CertEndDate"').attr("disabled", false);
                var DateType = dateValidationCheck($(this).val());
                if ($(this).val() == '') {
                    alert('不得空值');
                }
                else if (DateType == false) {
                    alert("請輸入 YYYY/MM/DD 日期格式");
                }
                var ErrorCount = 0;
                var CertPublicDate = document.getElementsByClassName('CertPublicDate');
                var Cunit = document.getElementsByClassName('Unit');
                var CertEndDate = document.getElementsByClassName('CertEndDate');
                var CertEndDateLen = CertEndDate.length;
                var CertStartDate = document.getElementsByClassName('CertStartDate');
                for (var i = 0; i < CertEndDateLen; i++) {
                    if (document.getElementsByClassName('Unit')[i].value == '') {
                        ErrorCount++;
                    }
                    if (CertPublicDate[i].value == '') {
                        ErrorCount++;
                    }
                    if (CertEndDate[i].value == '') {
                        ErrorCount++;
                    }
                    if (CertStartDate[i].value == '') {
                        ErrorCount++;
                    }
                    if (dateValidationCheck(CertPublicDate[i].value) == false) {
                        ErrorCount++;
                    }
                    if (dateValidationCheck(CertEndDate[i].value) == false) {
                        ErrorCount++;
                    }
                    if (dateValidationCheck(CertStartDate[i].value) == false) {
                        ErrorCount++;
                    }
                }
                if (ErrorCount > 0) {
                    $("#ContentPlaceHolder1_btn_OK").attr("disabled", true);
                    $("#ContentPlaceHolder1_btn_OK").css("background-color", "gray");
                    $('input[class="youpiss"').attr("disabled", false);
                    return;
                }
                else {
                    $("#ContentPlaceHolder1_btn_OK").attr("disabled", false);
                    $("#ContentPlaceHolder1_btn_OK").css("background-color", "#f9bf3b");
                    $('input[class="youpiss"').attr("disabled", false);
                }
                console.log(ErrorCount);
            });
        });
        $(document).ready(function () {
            $('input[class="CertStartDate"').focusout(function () {
                $('input[class="CertStartDate"').attr("disabled", true);
                $('input[class="CertStartDate"').attr("disabled", false);
                var DateType = dateValidationCheck($(this).val());
                if ($(this).val() == '') {
                    alert('不得空值');
                }
                else if (DateType == false) {
                    alert("請輸入 YYYY/MM/DD 日期格式");
                }
                var ErrorCount = 0;
                var CertPublicDate = document.getElementsByClassName('CertPublicDate');
                var Cunit = document.getElementsByClassName('Unit');
                var CertEndDate = document.getElementsByClassName('CertEndDate');
                var CertStartDate = document.getElementsByClassName('CertStartDate');
                var CertStartDateLen = CertStartDate.length;
                for (var i = 0; i < CertStartDateLen; i++) {
                    if (document.getElementsByClassName('Unit')[i].value == '') {
                        ErrorCount++;
                    }
                    if (CertPublicDate[i].value == '') {
                        ErrorCount++;
                    }
                    if (CertEndDate[i].value == '') {
                        ErrorCount++;
                    }
                    if (CertStartDate[i].value == '') {
                        ErrorCount++;
                    }
                    if (dateValidationCheck(CertPublicDate[i].value) == false) {
                        ErrorCount++;
                    }
                    if (dateValidationCheck(CertEndDate[i].value) == false) {
                        ErrorCount++;
                    }
                    if (dateValidationCheck(CertStartDate[i].value) == false) {
                        ErrorCount++;
                    }
                }
                if (ErrorCount > 0) {
                    $("#ContentPlaceHolder1_btn_OK").attr("disabled", true);
                    $("#ContentPlaceHolder1_btn_OK").css("background-color", "gray");
                    $('input[class="youpiss"').attr("disabled", false);
                    return;
                }
                else {
                    $("#ContentPlaceHolder1_btn_OK").attr("disabled", false);
                    $("#ContentPlaceHolder1_btn_OK").css("background-color", "#f9bf3b");
                    $('input[class="youpiss"').attr("disabled", false);
                }
                console.log(ErrorCount);
            });
        });

        $(function () {

            var ErrorCount = 0;
            var CertPublicDate = document.getElementsByClassName('CertPublicDate');
            var Cunit = document.getElementsByClassName('Unit');
            var CertEndDate = document.getElementsByClassName('CertEndDate');
            var CertStartDate = document.getElementsByClassName('CertStartDate');
            var CertStartDateLen = CertStartDate.length;
            for (var i = 0; i < CertStartDateLen; i++) {
                if (Cunit[i].value == '') {
                    ErrorCount++;
                }
                if (CertPublicDate[i].value == '') {
                    ErrorCount++;
                }
                if (CertEndDate[i].value == '') {
                    ErrorCount++;
                }
                if (CertStartDate[i].value == '') {
                    ErrorCount++;
                }
                if (dateValidationCheck(CertPublicDate[i].value) == false) {
                    ErrorCount++;
                }
                if (dateValidationCheck(CertEndDate[i].value) == false) {
                    ErrorCount++;
                }
                if (dateValidationCheck(CertStartDate[i].value) == false) {
                    ErrorCount++;
                }

            }

            if (ErrorCount > 0) {
                $("#ContentPlaceHolder1_btn_OK").attr("disabled", true);
                $("#ContentPlaceHolder1_btn_OK").css("background-color", "gray");
                $('input[class="youpiss"').attr("disabled", false);
                return;
            }
            else {
                $("#ContentPlaceHolder1_btn_OK").attr("disabled", false);
                $("#ContentPlaceHolder1_btn_OK").css("background-color", "#f9bf3b");
                $('input[class="youpiss"').attr("disabled", false);
            }

        })

        function dateValidationCheck(str) {
            var re = new RegExp("^([0-9]{4})[./]{1}([0-9]{1,2})[./]{1}([0-9]{1,2})$");
            var strDataValue;
            var infoValidation = true;
            if ((strDataValue = re.exec(str)) != null) {
                var i;
                i = parseFloat(strDataValue[1]);
                if (i <= 0 || i > 9999) { /*年*/
                    infoValidation = false;
                }
                i = parseFloat(strDataValue[2]);
                if (i <= 0 || i > 12) { /*月*/
                    infoValidation = false;
                }
                i = parseFloat(strDataValue[3]);
                if (i <= 0 || i > 31) { /*日*/
                    infoValidation = false;
                }
            } else {
                infoValidation = false;
            }
            return infoValidation;
        }
      
        

       
    </script>
    <script type="text/javascript">
        $(function () {
            $("#ContentPlaceHolder1_btn_OK").click(function () {
                $.blockUI({
                    message: '<h1>讀取中...</h1>'
                });
            });//end click

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <legend>基本資料
        </legend>
        <asp:GridView ID="gv_BasicData" runat="server" AutoGenerateColumns="false" OnRowCreated="gv_BasicData_RowCreated">
            <Columns>
                 <asp:BoundField HeaderText="排序" DataField="Sort" />
                <asp:BoundField HeaderText="姓名" DataField="PName" />
                <asp:BoundField HeaderText="身分證" DataField="PersonID" />
                <asp:BoundField HeaderText="電話" DataField="PTel" />
                <asp:BoundField HeaderText="身分別" DataField="RoleName" />
                <asp:BoundField HeaderText="信箱" DataField="PMail" />
               <asp:BoundField HeaderText="是否有舊證書" DataField="CertSNO" />
                <asp:BoundField HeaderText="CtypeSNO" DataField="CtypeSNO" />
                <asp:BoundField HeaderText="舊證書" DataField="CTypeName" />
                  <asp:BoundField HeaderText="證書號碼" DataField="CertID" />
            </Columns>
        </asp:GridView>
    </fieldset>
    <fieldset>
        <legend>學分狀態
        </legend>
        <asp:GridView ID="gv_CertificateStatus" runat="server" AutoGenerateColumns="false">
            <Columns>
                 <asp:BoundField HeaderText="排序" DataField="Sort" />
                <asp:BoundField HeaderText="姓名" DataField="PName" />
                <%--<asp:BoundField HeaderText="身分別" DataField="RoleName" />--%>
                <asp:BoundField HeaderText="身分證" DataField="PersonID" />
                <asp:BoundField HeaderText="課程規劃名稱" DataField="PlanName" />
                <asp:BoundField HeaderText="參考年度" DataField="CYear" />
                <asp:BoundField HeaderText="已取得/目標積分" DataField="AllHour" />
                <asp:BoundField HeaderText="可取得的證書" DataField="CTypeName" />
            </Columns>
        </asp:GridView>
    </fieldset>
    <fieldset>
        <legend>證書更換類型
        </legend>

        <%--        <asp:DropDownList ID="ddl_Type" runat="server" DataTextField="" DataValueField="" OnSelectedIndexChanged="ddl_Type_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Selected="True" Text="請選擇"></asp:ListItem>
            <asp:ListItem Text="更換證書到期日" Value="0"></asp:ListItem>
            <asp:ListItem Text="更換證書字號" Value="1"></asp:ListItem>
        </asp:DropDownList>--%>

        <%--<asp:DropDownList ID="ddl_CtypeName" runat="server" DataTextField="CtypeString" DataValueField="CTypeSNO" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddl_CtypeName_SelectedIndexChanged"></asp:DropDownList>--%>
        <p></p>

        <asp:GridView ID="gv_SetValue" runat="server" AutoGenerateColumns="false"  OnRowDataBound="gv_SetValue_RowDataBound" OnRowCreated="gv_SetValue_RowCreated">
            <Columns>
                 <asp:BoundField HeaderText="排序" DataField="Sort" />
                <asp:BoundField HeaderText="姓名" DataField="PName" />
                <asp:BoundField HeaderText="身分證" DataField="PersonID" />
                <asp:TemplateField HeaderText="證書字號">
                    <ItemTemplate>
                        <asp:TextBox ID="txt_CertID" runat="server" Enabled="false"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle Width="200px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="證書首發日">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                    <HeaderTemplate>
                        <asp:TextBox ID="txt_AllRecover" CssClass="youpi" onfocusout="ca()" runat="server" Width="200px" placeholder="證書首發日 例:2019/06/01"></asp:TextBox>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="CertPublicDate" CssClass="CertPublicDate" Width="200px" runat="server"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="證書公告日">
                    <HeaderTemplate>
                        <asp:TextBox ID="txt_AllRecover_1" CssClass="youpi" onfocusout="ca()" runat="server" Width="200px" placeholder="證書公告日 例:2019/06/01"></asp:TextBox>
                    </HeaderTemplate>
                    <HeaderStyle Width="200px" />
                    <ItemTemplate>
                        <asp:TextBox ID="CertStartDate" CssClass="CertStartDate" Width="200px" runat="server"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="證書到期日" HeaderStyle-Width="200px">
                     <HeaderTemplate>
                        <asp:TextBox ID="txt_CertEndDate" CssClass="youpi" onfocusout="ca()" runat="server" Width="200px" placeholder="證書到期日 例:2019/06/01"></asp:TextBox>
                    </HeaderTemplate>
                    <HeaderStyle Width="200px" />
                    <ItemTemplate>
                        <asp:TextBox ID="CertEndDate" CssClass="CertEndDate" Width="200px" runat="server"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="可取得的證書" DataField="CTypeName" />
                <asp:BoundField HeaderText="發證單位" DataField="CUnitName" />
                <asp:BoundField HeaderText="CtypeSEQ" DataField="CtypeSEQ" />
                <asp:BoundField HeaderText="CunitSNO" DataField="CunitSNO" />
                <asp:BoundField HeaderText="CTypeSNO" DataField="CTypeSNO" />
                <asp:TemplateField HeaderText="發證單位" ItemStyle-HorizontalAlign="center">
                        <HeaderTemplate>
                            <asp:DropDownList ID="ddl_AuditH" runat="server" CssClass="UnitChose" DataTextField="CUnitName" DataValueField="CUnitPairSNO">
                            </asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:DropDownList ID="ddl_AuditItem" CssClass="Unit" DataTextField="CUnitName" DataValueField="CUnitPairSNO" onchange="AdmitResult();" runat="server">
                            </asp:DropDownList>
                        </ItemTemplate>

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                 <%--<asp:BoundField HeaderText="oldC" DataField="oldC" />--%>
                 <%--<asp:BoundField HeaderText="PrestoredNumber" DataField="PrestoredNumber" />--%>
                 <asp:BoundField HeaderText="PersonSNO" DataField="PersonSNO" />
   
            </Columns>
        </asp:GridView>
    </fieldset>
    <div class="center">
        <asp:Button ID="btn_OK" runat="server" Text="確定" OnClick="btn_OK_Click" OnClientClick="OKClick()" Enabled="false" />
        <%-- <asp:Button ID="Button1" runat="server" Text="111"   OnClientClick="OKClick()"/>--%>
        <asp:Button ID="btn_Cancel" runat="server" Text="取消" OnClick="btn_Cancel_Click" OnClientClick="javascript:window.close()" />
        <asp:Button ID="btn_Export" runat="server" Text="清冊匯出" OnClick="btn_Export_Click" OnClientClick="btnExportClick()" />
    </div>
    <asp:Label ID="lb_hidden" Style="visibility: hidden" runat="server" Text=""></asp:Label>

</asp:Content>

