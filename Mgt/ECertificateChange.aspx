<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="ECertificateChange.aspx.cs" Inherits="Mgt_ECertificateChange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../JS/zTree_v3/js/jquery.blockUI.js"></script>
    <script language="javascript" type="text/javascript">

        $(function () {

            $(".youpi").datepicker({

                changeYear: true,
                yearRange: "2000:2050",
                yearSuffix: "年",
                dateFormat: "yy/mm/dd",
                shortYearCutoff: "+10",
              
            });
           
        });

    </script>
    <style type="text/css">
        #ContentPlaceHolder1_btn_OK {
            background-color: gray;
        }
    </style>
    <script type="text/javascript">
        Date.prototype.yyyymmdd = function () {
            var mm = this.getMonth() + 1; // getMonth() is zero-based
            var dd = this.getDate();

            return [this.getFullYear(),
            (mm > 9 ? '' : '0') + mm,
            (dd > 9 ? '' : '0') + dd
            ].join('');
        };

        function ca() {
            $('input[id="ContentPlaceHolder1_gv_SetValue_txt_AllRecover"]').on('change', function () {
                var myDate = new Date($(this).val());
                //myDate.yyyymmdd();
                // console.log(myDate)
                if (isNaN(myDate)) {
                    return;
                }
                myDate.setFullYear(myDate.getFullYear() + 6);
                myDate.setDate(myDate.getDate() - 1);
                myDate.setMonth(myDate.getMonth());
                var curr_day = myDate.getDate();               
                var curr_month = myDate.getMonth()+1;
                var curr_year = myDate.getFullYear();
                $('input[class="youpiss"').val($(this).val());
                $('input[class="CertEndDate"').val(curr_year + '/' + 12 + '/' + 31);
                $("#ContentPlaceHolder1_btn_OK").attr("disabled", false);
                $("#ContentPlaceHolder1_btn_OK").css("background-color", "#f9bf3b");
               
               
            });
        };
        function cb() {
            $('input[id="ContentPlaceHolder1_gv_SetValue_txt_AllRecoverb"]').on('change', function () {
                var myDate = new Date($(this).val());

                if (isNaN(myDate)) {
                    return;
                }
                 var curr_year = myDate.getFullYear();
                $('input[class="CertEndDate"').val(curr_year+ '/' + 12 + '/' + 31);
                $("#ContentPlaceHolder1_btn_OK").attr("disabled", false);
                $("#ContentPlaceHolder1_btn_OK").css("background-color", "#f9bf3b");
            });
        };
    </script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('input[class="youpiss"').focusout(function () {
                $('input[class="youpiss"').attr("disabled", true);
                $(this).attr("disabled", false);
                var DateType = dateValidationCheck($(this).val());
                if ($(this).val() == '') {
                    alert('不得空值');
                }
                else if (DateType == false) {
                    alert("請輸入 YYYY/MM/DD 日期格式");
                }
                var ErrorCount = 0;
                var CertPublicDate = document.getElementsByClassName('youpiss');
                var CertPublicDateLen = CertPublicDate.length;
                var CertEndDate = document.getElementsByClassName('CertEndDate');
                for (var i = 0; i < CertPublicDateLen; i++) {
                    if (CertPublicDate[i].value == '') {
                        ErrorCount++;
                    }
                    if (CertEndDate[i].value == '') {
                        ErrorCount++;
                    }
                    if (dateValidationCheck(CertPublicDate[i].value) == false) {
                        ErrorCount++;
                    }
                    if (dateValidationCheck(CertEndDate[i].value) == false) {
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
            var CertPublicDate = document.getElementsByClassName('youpiss');
            var CertPublicDateLen = CertPublicDate.length;
            var CertEndDate = document.getElementsByClassName('CertEndDate');
            for (var i = 0; i < CertPublicDateLen; i++) {
                if (CertPublicDate[i].value == '') {
                    ErrorCount++;
                }
                if (CertEndDate[i].value == '') {
                    ErrorCount++;
                }
                if (dateValidationCheck(CertPublicDate[i].value) == false) {
                    ErrorCount++;
                }
                if (dateValidationCheck(CertEndDate[i].value) == false) {
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
        $(document).ready(function () {
            $('input[class="CertEndDate"').focusout(function () {
                //$('input[class="CertEndDate"').attr("disabled", true);
                //$(this).attr("disabled", false);
                var DateType = dateValidationCheck($(this).val());
                if ($(this).val() == '') {
                    alert('不得空值');
                }
                else if (DateType == false) {
                    alert("請輸入 YYYY/MM/DD 日期格式");
                }
                var ErrorCount = 0;
                var CertPublicDate = document.getElementsByClassName('youpiss');
                var CertEndDateLen = CertPublicDate.length;
                var CertEndDate = document.getElementsByClassName('CertEndDate');
                for (var i = 0; i < CertEndDateLen; i++) {
                    if (CertPublicDate[i].value == '') {
                        ErrorCount++;
                    }
                    if (CertEndDate[i].value == '') {
                        ErrorCount++;
                    }
                    if (dateValidationCheck(CertPublicDate[i].value) == false) {
                        ErrorCount++;
                    }
                    if (dateValidationCheck(CertEndDate[i].value) == false) {
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
        <asp:GridView ID="gv_BasicData" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField HeaderText="姓名" DataField="PName" />
                <asp:BoundField HeaderText="身分證" DataField="PersonID" />
                <asp:BoundField HeaderText="電話" DataField="PTel" />
                <asp:BoundField HeaderText="信箱" DataField="PMail" />
            </Columns>
        </asp:GridView>
    </fieldset>
    <fieldset>
        <legend>證書狀態
        </legend>
        <asp:GridView ID="gv_CertificateStatus" runat="server" AutoGenerateColumns="false" OnRowCreated="gv_CertificateStatus_RowCreated">
            <Columns>
                <asp:BoundField HeaderText="CertSNO" DataField="CertSNO" />
                <asp:BoundField HeaderText="姓名" DataField="PName" />
                <asp:BoundField HeaderText="證書名稱" DataField="CTypeName" />
                <asp:BoundField HeaderText="證書首發" DataField="CertPublicDate" />
                <asp:BoundField HeaderText="證書公告" DataField="CertStartDate" />
                <asp:BoundField HeaderText="證書到期" DataField="CertEndDate" />
                <asp:BoundField HeaderText="發證單位" DataField="CUnitName" />
            </Columns>
        </asp:GridView>
    </fieldset>
    <fieldset>
        <legend>證書更換類型
        </legend>

        <asp:DropDownList ID="ddl_Type" runat="server" DataTextField="" DataValueField="" OnSelectedIndexChanged="ddl_Type_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Selected="True" Text="請選擇"></asp:ListItem>
            <asp:ListItem Text="更換證書到期日" Value="0"></asp:ListItem>
            <asp:ListItem Text="更換證書字號" Value="1"></asp:ListItem>
        </asp:DropDownList>

        <asp:DropDownList ID="ddl_CtypeName" runat="server" DataTextField="CtypeString" DataValueField="CTypeSNO" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddl_CtypeName_SelectedIndexChanged"></asp:DropDownList>




        <asp:Label ID="lb_Count" runat="server" Text="上傳人數與勾選人數不符" ForeColor="Red" Visible="false"></asp:Label>

        <asp:GridView ID="gv_ScoreUpload" runat="server" AutoGenerateColumns="False">

            <Columns>
                <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="PName" HeaderText="學員名稱" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="PersonID" HeaderText="身分證" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="CertID" HeaderText="證書字號" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="StartDate" HeaderText="證書起始日" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="PublicDate" HeaderText="證書公告日" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="EndDate" HeaderText="證書到期日" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="執行訊息">
                    <ItemTemplate>
                        <span style='<%#Eval("style")%>'><%#Eval("Status")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>


        <asp:GridView ID="gv_SetValue" runat="server" AutoGenerateColumns="false" OnRowDataBound="gv_SetValue_RowDataBound" OnRowCreated="gv_SetValue_RowCreated">
            <Columns>
                <asp:BoundField HeaderText="姓名" DataField="PName" />
                <asp:BoundField HeaderText="身分證" DataField="PersonID" />
                <asp:BoundField HeaderText="CertID" DataField="CertID" />
                <asp:BoundField HeaderText="CTypeSNO" DataField="CTypeSNO" />
                <asp:BoundField HeaderText="CUnitSNO" DataField="CUnitSNO" />
                <asp:TemplateField HeaderText="證書字號">
                    <ItemTemplate>
                        <asp:TextBox ID="txt_CtypeString" Enabled="false" runat="server"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle Width="200px" />
                </asp:TemplateField>
                <asp:BoundField HeaderText="證書字號" DataField="CtypeString" />    
                <asp:BoundField HeaderText="證書首發日" DataField="CertPublicDate" />
                <asp:TemplateField HeaderText="證書公告日">

                    <HeaderTemplate>
                        <asp:TextBox ID="txt_AllRecover" CssClass="youpi" onfocusout="ca()" runat="server" placeholder="證書公告日 例:2019/06/01"></asp:TextBox>

                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txt_CertStartDate" CssClass="youpiss" runat="server"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="證書到期日">

                    <HeaderTemplate>
                        <asp:TextBox ID="txt_AllRecoverb" CssClass="youpi" onfocusout="cb()" placeholder="證書到期日 例:2019/06/01" runat="server"></asp:TextBox>

                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txt_CertEndDate" CssClass="CertEndDate" runat="server"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="CertSNO" DataField="CertSNO" />
              
                <asp:BoundField HeaderText="預存號碼" DataField="PrestoredNumber" />
                  <asp:BoundField HeaderText="發證單位代號" DataField="CUnitSNO" />
            </Columns>
        </asp:GridView>

    </fieldset>

    <div class="center">
        <asp:Button ID="btn_OK" runat="server" Text="確定" OnClick="btn_OK_Click" Enabled="false" />
        <asp:Button ID="btn_Cancel" runat="server" Text="取消" OnClick="btn_Cancel_Click" OnClientClick="javascript:window.close()" />
         <asp:Button ID="btn_Export" runat="server" Text="清冊匯出" OnClick="btn_Export_Click" />
    </div>
    <asp:Label ID="lb_hidden" Style="visibility: hidden" runat="server" Text=""></asp:Label>


</asp:Content>

