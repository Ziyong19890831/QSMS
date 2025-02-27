<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="Account.aspx.cs" Inherits="Web_Account" %>

<%@ Register Src="~/Web/WUC_Country.ascx" TagPrefix="uc1" TagName="WUC_Country" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../JS/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../JS/PswValid.js"></script>
    <link type="text/css"  rel="stylesheet"  href="../CSS/jquery-ui.css" />

    <script src="../JS/zTree_v3/js/jquery.blockUI.js"></script>

    <script type="text/javascript">
       
        $(function () {

            $('#show_password').click(function () {
                // 如果是勾選則...
                if (this.checked) {
                    $(<%= txt_Pswd.ClientID%>).prop('type', 'text');
                    $(<%= txt_cPswd.ClientID%>).prop('type', 'text');
                } else {
                    $(<%= txt_Pswd.ClientID%>).prop('type', 'password');
                    $(<%= txt_cPswd.ClientID%>).prop('type', 'password');
                }
            });

            $(".datepicker").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'yy-mm-dd',
                yearRange: "-80:+0"
            }).blur(function () {
                val = $(this).val();
                val1 = Date.parse(val);
                if (isNaN(val1) == true && val !== '') {
                    $(this).val('');
                }
            });

        });


    </script>


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        table a {
            color: #000000;
        }

        .width160 {
            width: 160px;
        }

        .fa-star {
            color: #FF0000;
        }
    </style>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="../">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">戒菸服務醫事人員帳號申請</li>
        </ol>
    </nav>

    <div class="row mt30">
        <div class="col-12">
            <h5>戒菸服務醫事人員帳號申請
            </h5>
        </div>
        <div class="col-12">
            <div class="alert alert-danger" role="alert">
                部分資料申請後不可變更，如有任何問題請洽管理員。
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-12">
            <table class="table table-striped">
                <tr>
                    <th>
                        <i class="fa fa-star"></i>
                        執業機構
                    </th>
                    <td>
                        <asp:UpdatePanel ID="upl_ddl" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="HF_OrganSNO" runat="server" Value=""></asp:HiddenField>
                                <asp:Label ID="lb_OrganCodeName" runat="server" Text="" Style="color: #019858;"></asp:Label>
                                <span id="orgmsg"></span>
                                <asp:Panel ID="Panel_Organ" runat="server">
                                    <div>
                                        <span>輸入任職機構代碼-目前待業中請填000</span>
                                        <input type="text" id="txt_Organ" class="form-control" runat="server" onfocusout="orgcheck(this.value)" />
                                        <asp:Label ID="msgOrgan" runat="server" Text=""></asp:Label>
<%--                                        <div id="Person_Experience" style="display: none">
                                            <span>任職機關/機構</span>
                                            <asp:TextBox runat="server" TextMode="MultiLine" class="form-control" ID="txt_Experience" cols="40" Rows="3"></asp:TextBox>
                                        </div>--%>
                                    </div>
                                    <div>或</div>
                                    <div>
                                        <p>選擇任職機構(若查無執業機構資料，請洽詢本系統客服)</p>
                                        <asp:DropDownList CssClass="form-control" ID="ddl_AreaCodeA" runat="server" OnSelectedIndexChanged="ddl_AreaCodeA_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                                        </asp:DropDownList>
                                        <asp:DropDownList CssClass="form-control" ID="ddl_AreaCodeB" runat="server" OnSelectedIndexChanged="ddl_AreaCodeB_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                                            <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList CssClass="form-control" ID="ddl_OrganCode" runat="server" OnSelectedIndexChanged="ddl_OrganCode_SelectedIndexChanged" AutoPostBack="true" DataValueField="OrganSNO" DataTextField="ORGAN_NAME">
                                            <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_AreaCodeA" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddl_AreaCodeB" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>角色別</th>
                        <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddl_Role" runat="server" AutoPostBack="True" class="required" DataTextField="RoleName" DataValueField="RoleSNO" OnSelectedIndexChanged="ddl_Role_SelectedIndexChanged"></asp:DropDownList>
                            <span id="rolemsg"></span>
                            <div id="Guardian" runat="server" visible="false">
                                執業科別
                                <input type="text" name="txt_TJobType" id="txt_TJobType" runat="server" maxlength="20" class="w6" />
                                <br />服務科別
                                <input type="text" name="txt_TSType" id="txt_TSType" runat="server" maxlength="50" class="w6" />
                            </div>
                            <div id="RoleException" runat="server" visible="false">
                                角色別2<br />
                                <asp:DropDownList ID="ddl_RoleExceprion" runat="server" DataTextField="REName" DataValueField="RESNO"></asp:DropDownList>
                                <br />
                                角色別職業說明
                                <asp:TextBox TextMode="MultiLine" ID="txt_RoleException" runat="server" Width="100%" Height="50px"></asp:TextBox>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddl_Role" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                </tr>
                 <tr>
                    <th><i class="fa fa-star"></i>服務科別</th>
                    <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                        <ContentTemplate>
                            <asp:DropDownList ID="ddl_TSType" runat="server" DataValueField="TsSNO" DataTextField="TsTypeName" AutoPostBack="true" OnSelectedIndexChanged="ddl_TSType_SelectedIndexChanged"></asp:DropDownList>
                           
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddl_Role" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                                 <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                           <asp:TextBox ID="txt_TSNote" runat="server" TextMode="MultiLine" Enabled="false" Width="100%" Height="50px" Visible="false"></asp:TextBox>
                           
                        </ContentTemplate>
                        <Triggers>
                            
                            <asp:AsyncPostBackTrigger ControlID="ddl_TSType" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    </td>
                </tr>
                 <tr>
                    <th><i class="fa fa-star"></i>帳號</th>
                    <td colspan="3">
                        <input id="txt_Account" class="required form-control" type="text" maxlength="50" runat="server" onfocusout="acccheck(this.value)" style="ime-mode: disabled" onkeyup="value=value.replace(/[^\w\.\/]/ig,'')" />
                        <span id="accmsg"></span>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>使用者密碼</th>
                    <td colspan="3">
                        <input id="txt_Pswd" onfocusout="pswChkStrength('ContentPlaceHolder1_txt_Pswd', 'ContentPlaceHolder1_txt_cPswd', 'Ppwdmsg', 'pwdmsg', 'strength');"
                            class="form-control required" name="name" type="password" runat="server" />
                        <span id="Ppwdmsg"></span>
                        <br />
                        <div style="border: 3px solid #c1c1c1; border-radius: 30px; text-align: center; width: 200px; height: 29px; background-color: #eeeeee">
                            <div style="width: 64.6px; float: left; border-radius: 30px 0px 0px 30px;" id="strength_L">弱</div>
                            <div style="width: 64.6px; float: left; border-radius: 0px;" id="strength_M">中</div>
                            <div style="width: 64.7px; float: left; border-radius: 0px 30px 30px 0px;" id="strength_H">強</div>
                        </div>
                        <br />
                        <a style="color: red; font-size: 12pt">註：*強度"中"以上"，並且密碼條件必須包含大寫英文加小寫英文及數字混合，長度大於8位。
                            <br />
                            例：1234AaBb
                        </a>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>再輸入一次密碼</th>
                    <td colspan="3">
                        <input id="txt_cPswd" class="required form-control" name="name" type="password"
                            onfocusout="pswChkInputIsSame('ContentPlaceHolder1_txt_Pswd', 'ContentPlaceHolder1_txt_cPswd', 'pwdmsg');" runat="server" />
                        <label class="Cviewpwd">
                            顯示密碼<input type="checkbox" id="show_password" />
                            <span class="checkmark"></span>
                        </label>
                        <span id="pwdmsg"></span>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>使用者姓名</th>
                    <td>
                        <input id="txt_Name" class="required form-control" name="name" type="text" maxlength="50" runat="server" onfocusout="isPureChinese(this.value)" />
                        <span id="namemsg"></span>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>身分證/居留證</th>
                    <td>
                        <input id="txt_Personid" class="required form-control" name="lbl_PersionId" type="text" maxlength="11" onfocusout="pidcheck(this.value)" runat="server" />
                        <span id="pidmsg"></span>
                    </td>
                </tr>
               <%-- <tr>
                    <th><i class="fa fa-star"></i>任職單位之部門</th>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList CssClass="form-control" ID="ddl_Job" runat="server" DataTextField="JobName" DataValueField="JSNO" AutoPostBack="true" OnSelectedIndexChanged="ddl_Job_SelectedIndexChanged"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_PJNote" class="form-control" runat="server" TextMode="MultiLine" cols="40" Rows="3" Visible="false"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_Job" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <span id="Jobmsg"></span>
                    </td>
                </tr>--%>
               
                
               
                
                <tr>
                    <th><i class="fa fa-star"></i>Email</th>
                    <td>
                        <input type="text" name="txt_Mail" id="txt_Mail" class="required email w10 form-control" runat="server" maxlength="100" onfocusout="mailcheck(this.value)" />
                        <span id="mailmsg"></span>
                            
                        <asp:UpdatePanel ID="Up_Send" runat="server">
                            <ContentTemplate>
                                 <asp:Button ID="btn_Send" runat="server" Text="發送驗證" class="btn btn-info" OnClick="btn_Send_Click" />
                                <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Enabled="false"></asp:Timer>
                                <asp:Label ID="timeLabel" runat="server"></asp:Label>
                                <asp:HiddenField ID="hid_vs" runat="server" />
                                <asp:Label ID="lb_A" runat="server" Text="發送次數："></asp:Label>
                                <asp:Label ID="lb_Count" runat="server" Text="0"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btn_Send" />
                            </Triggers>
                        </asp:UpdatePanel>

                        <span id="Numbermsg"></span>
                        <br />
                        <h6>驗證碼：</h6>
                        <asp:TextBox ID="txt_Number" runat="server" class="form-control" width="200px" onfocusout="CheckVs(this.value)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>學歷</th>
                    <td>
                        <input id="txt_degree" name="txt_degree" class="form-control" type="text" maxlength="50" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>國籍</th>
                    <td>
                        <%--<uc1:WUC_Country runat="server" CssClass="form-control" ID="WUC_Country" />--%>
                        <asp:DropDownList ID="ddl_Country" runat="server" DataTextField="Mval" DataValueField="PVal"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>出生年月日</th>
                    <td>
                        <input id="txt_Birthday" class="datepicker date form-control" type="text" runat="server" />
                        <span id="Birthdaymsg"></span>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>通訊地址</th>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" style="float: left">
                            <ContentTemplate>
                                郵區：
                                <input id="txt_ZipCode" type="text" disabled="disabled" class="number form-control" placeholder="區號" maxlength="5" runat="server" style="width: 100px;" />
                                <span id="ZipCodemsg"></span>
                                市/縣：
                                <asp:DropDownList ID="ddl_AddressA" runat="server" OnSelectedIndexChanged="ddl_AddressA_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                                </asp:DropDownList>
                                區/鎮/鄉：<asp:DropDownList ID="ddl_AddressB" runat="server" OnSelectedIndexChanged="ddl_AddressB_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                                    <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_AddressA" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>

                        <input id="txt_Addr" class="w3 form-control" type="text" maxlength="50" runat="server" style="float: left" />
                        <span id="Addrmsg"></span>
                    </td>
                </tr>
                <tr>
                    <th>通訊電話</th>
                    <td>
                        <input type="text" name="txt_Tel" id="txt_Tel" class="form-control" maxlength="50" runat="server" placeholder="Ex：02-12345678" /><br />
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>手機</th>
                    <td>
                        <input type="text" name="txt_Phone" id="txt_Phone" maxlength="10" class="form-control" runat="server" onfocusout="CheckPhoneInputLength(this.value,10)" placeholder="Ex：0912345678" /><br />
                        <span id="Phonemsg"></span>
                         <asp:Timer runat="server" ID="timer" Interval="50000" OnTick="timer_Tick"></asp:Timer>
                        <asp:UpdatePanel ID="up" runat="server">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="timer" EventName="Tick" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Button ID="btn_SMSSend" runat="server" Text="發送驗證" class="btn btn-info" OnClick="btn_SMSSend_Click" />
                                <asp:UpdatePanel ID="Up_SMS" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lb_Sms" runat="server" Text="發送次數："></asp:Label>
                                        <asp:Label ID="lb_SCount" runat="server" Text="0"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btn_SMSSend" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:HiddenField ID="hid_vsS" runat="server" />
                                <span id="SMSmsg"></span>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br>
                        <h6>驗證碼：</h6>
                        <asp:TextBox ID="txt_SMSR" runat="server" class="form-control" Width="200px" onfocusout="CheckVsS(this.value)"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>

        <div class="col-12 center">
            <input id="btn_submit" name="" type="submit" class="btn btn-success" style="width: 200px" value="送出申請" runat="server" onclick="checkinput()" onserverclick="btn_submit_ServerClick" />
        </div>
    </div>
     
    <script type="text/javascript">

        //點送出時的欄位資料確認
        var errorCount = 0;
        function checkinput() {
            errorCount = 0;
            CheckTextError("<%=HF_OrganSNO.ClientID %>", "orgmsg", 20);
            CheckTextError("<%=txt_ZipCode.ClientID %>", "ZipCodemsg", 5);
            CheckTextError("<%=txt_Phone.ClientID %>", "Phonemsg", 10);
            CheckTextError("<%=txt_Addr.ClientID %>", "Addrmsg", 100);
            CheckTextError("<%=ddl_Role.ClientID %>", "rolemsg", 50);
<%--            CheckTextError("<%=ddl_Job.ClientID %>", "Jobmsg", 50);--%>
            <%--CheckTextError("<%=ddl_TSType.ClientID %>", "TSTypemsg", 50);--%>
            CheckTextError("<%=txt_Account.ClientID %>", "accmsg", 50);
            acccheck($("#<%=txt_Account.ClientID %>").val());
            CheckTextError("<%=txt_Pswd.ClientID %>", "Ppwdmsg", 50);
            CheckTextError("<%=txt_cPswd.ClientID %>", "pwdmsg", 50);
            CheckTextError("<%=txt_Name.ClientID %>", "namemsg", 50);
            CheckTextError("<%=txt_Personid.ClientID %>", "pidmsg", 50);
            CheckTextError("<%=txt_Birthday.ClientID %>", "Birthdaymsg", 50);
            pidcheck($("#<%=txt_Personid.ClientID %>").val());
            CheckTextError("<%=txt_Mail.ClientID %>", "mailmsg", 50);
            mailcheck($("#<%=txt_Mail.ClientID %>").val());
            pswcheck();
            CheckVs($("#<%=txt_Number.ClientID %>").val());
            CheckVsS($("#<%=txt_SMSR.ClientID %>").val());

            if (errorCount > 0) {

                event.preventDefault();
            }
        }


        function CheckTextError(name, msglbl, maxLen) {
            if (document.getElementById(name)) {
                if ($("#" + name).val() == "") {
                    $('#' + name).css("border", "1px dotted #f84b4b");
                    $('#' + msglbl).css("color", "red");
                    $('#' + msglbl).text("這是必填的欄位");
                    $('#' + msglbl).show();
                    errorCount += 1;
                }

                else {
                    $('#' + name).css("border", "1px solid #ccc");
                    $('#' + msglbl).hide();
                    //長度檢查
                    CheckInputLength(name, msglbl, $("#" + name).val(), maxLen);
                }
            }
            else {
                $('#' + msglbl).css("color", "red");
                $('#' + msglbl).text("這是必填的欄位");
                $('#' + msglbl).show();
                errorCount += 1;
            }
        }

        function CheckInputLength(name, msglbl, val, maxLen) {
            if (val.length > maxLen) {
                console.log(name);
                $("#" + name).val('');
                $('#' + msglbl).css("color", "red");
                $('#' + msglbl).text("長度太長");
                $('#' + msglbl).show();

                errorCount += 1;
            }
            else {
                $('#' + name).css("border", "1px solid #ccc");
                $('#' + msglbl).hide();
            }
        }
        function CheckPhoneInputLength(val, maxLen) {

            if (val.length != maxLen) {
                $("#" + 'ContentPlaceHolder1_txt_Phone').val('');
                $('#' + 'Phonemsg').css("color", "red");
                $('#' + 'Phonemsg').text("長度需等於10碼");
                $('#' + 'Phonemsg').show();

                errorCount += 1;

            }
            else {
                $('#' + 'ContentPlaceHolder1_txt_Phone').css("border", "1px solid #ccc");
                $('#' + 'Phonemsg').hide();
            }

        }






        //當申請欄位輸入資料時觸發此段script

        function acccheck(dInput) {

            if (dInput != "") {
                $('#<%=txt_Account.ClientID %>').css("border", "1px solid #ccc");

                var accRegxp = /[^A-Za-z0-9_]/;
                var acccheck = "";
                if (accRegxp.test(dInput) != false) {
                    acccheck = "no";
                }

                if (acccheck == "") {
                    if (dInput.length >= 5) {

                        $('#accmsg').show();
                        $.ajax({
                            url: "AccountAjax.aspx",
                            type: 'POST',
                            async: false,
                            data: { account: dInput, personid: "0" },
                            success: function (result) {
                                if (dInput.length < 5) {
                                    errorCount += 1
                                    $('#<%=txt_Account.ClientID %>').css("border", "1px solid #ccc");
                                    $('#accmsg').css("color", "red");
                                    $('#accmsg').text("帳號長度不得小於5");
                                }
                                else if (result == "可使用") {
                                    $('#<%=txt_Account.ClientID %>').css("border", "1px solid #ccc");
                                    $('#accmsg').css("color", "	#019858");
                                    $('#accmsg').text("✔" + result);
                                }
                                else {
                                    errorCount += 1;
                                    $('#<%=txt_Account.ClientID %>').css("border", "1px dotted #f84b4b");
                                    $('#accmsg').css("color", "red");
                                    $('#accmsg').text(result);
                                }
                            }
                        });
                    } else {
                        errorCount += 1;
                        $('#<%=txt_Account.ClientID %>').css("border", "1px dotted #f84b4b");
                        $('#accmsg').css("color", "red");
                        $('#accmsg').text("字數太少，至少5字元。");
                    }
                } else {

                    errorCount += 1;
                    $('#<%=txt_Account.ClientID %>').css("border", "1px dotted #f84b4b");
                    $('#accmsg').css("color", "red");
                    $('#accmsg').text("不可輸入非規定的字元。");
                }
            }
        }

        function orgcheck(dInput) {

            if (dInput != "") {
                $('#Person_Experience').css('display', 'none')
                $('#<%=txt_Organ.ClientID %>').css("border", "1px solid #ccc");

                $('#orgmsg').show();
                $.ajax({
                    url: "AccountAjax.aspx",
                    type: 'POST',
                    data: { orgid: dInput },
                    success: function (result) {
                        var rst = result.split(",");
                        if (rst[0] == "可使用") {
                            $('#<%=txt_Organ.ClientID %>').css("border", "1px solid #ccc");
                            //$('#orgmsg').css("color", "#019858");
                            $('#orgmsg').text("");
                            //$('#orgmsg').text("✔");
                            document.getElementById("<%=HF_OrganSNO.ClientID %>").value = rst[1];
                            document.getElementById("<%=lb_OrganCodeName.ClientID %>").innerText = rst[2] + "(可使用)";
                        }
                        else {
                            errorCount += 1;
                            $('#<%=txt_Organ.ClientID %>').css("border", "1px dotted #f84b4b");
                            $('#orgmsg').css("color", "red");
                            $('#orgmsg').text(result);
                        }

                    }
                });

            }
            //if (dInput == '000') {
            //    $('#Person_Experience').css('display', 'block')
            //}
            if (dInput == '') {
                $('#Person_Experience').css('display', 'none')
            }
        }

        function mailcheck(dInput) {

            var emailcheck = "";

            var emailRegxp = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z]+$/;
            if (emailRegxp.test(dInput) != true) {
                emailcheck = "no";
            }

            if (dInput != "" && emailcheck == "") {

                $('#mailmsg').show();
                if (dInput == 'sample@email.tst') {
                    errorCount += 1;
                    $('#<%=txt_Mail.ClientID %>').css("border", "1px dotted #f84b4b");
                    $('#mailmsg').css("color", "red");
                    $('#mailmsg').text("EMail格式錯誤");
                    //$('#ContentPlaceHolder1_btn_Send').attr('disable', true);
                    document.getElementById("ContentPlaceHolder1_btn_Send").disabled = true;
                    return;
                }
                //else {
                //    document.getElementById("ContentPlaceHolder1_btn_Send").disabled = false;
                //}

                $.ajax({
                    url: "AccountAjax.aspx",
                    type: 'POST',
                    async: false,
                    data: { pmail: dInput },
                    success: function (result) {
                        if (result == "可使用") {
                            $('#<%=txt_Mail.ClientID %>').css("border", "1px solid #ccc");
                            $('#mailmsg').css("color", "#019858");
                            $('#mailmsg').text("✔" + result);
                        }
                        else {
                            errorCount += 1;
                            $('#<%=txt_Mail.ClientID %>').css("border", "1px dotted #f84b4b");
                            $('#mailmsg').css("color", "red");
                            $('#mailmsg').text(result);
                        }
                    }
                });

            }
            else {
                if ((dInput != "" && emailcheck == "no")) {
                    errorCount += 1;
                    $('#<%=txt_Mail.ClientID %>').css("border", "1px dotted #f84b4b");
                    $('#mailmsg').css("color", "red");
                    $('#mailmsg').text("EMail格式錯誤");
                }
            }
        }

        //身分證驗證
        function pidcheck(dInput) {
            if (dInput != "") {

                $('#<%=txt_Personid.ClientID %>').css("border", "1px solid #ccc");
                $('#pidmsg').show();
                $.ajax({
                    url: "AccountAjax.aspx",
                    type: 'POST',
                    async: false,
                    data: { account: "0", personid: dInput },
                    success: function (result) {
                        debugger
                        //先查身分證或居留證是否重複
                        if (result == "可使用") {

                            if (dInput.length == 10) {

                                $('#pidmsg').css("color", "	#019858");
                                $('#pidmsg').text("✔" + result);
                                $('#<%=txt_Personid.ClientID %>').css("border", "1px solid #ccc");

                            }                            
                        }
                        else {
                            $('#pidmsg').text(result);
                            $('#pidmsg').css("color", "red");
                            $('#<%=txt_Personid.ClientID %>').css("border", "1px dotted #f84b4b");
                            errorCount += 1;
                        }
                    }
                }
                )
            }
        }

        function pswcheck() {
            var msg = "";
            if ($('#strength_L').css("background-color") == "rgb(253, 220, 151)") {
                msg += "密碼強度過弱請修改!\n";
                errorCount += 1;
            }
            if ($(<%= txt_cPswd.ClientID%>).val() != $(<%= txt_Pswd.ClientID%>).val()) {
                msg += "與密碼不相同!\n";
                errorCount += 1;
            }
            if (msg != "") {
                alert(msg);
            }
        }

        function twidcheck(value) {
            var a = new Array('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'X', 'Y', 'W', 'Z', 'I', 'O');
            var b = new Array(1, 9, 8, 7, 6, 5, 4, 3, 2, 1);
            var c = new Array(2);
            var d; var e; var f; var g = 0;
            var h = /^[a-z](1|2)\d{8}$/i;
            if (value.search(h) == -1) {
                return false;
            } else {
                d = value.charAt(0).toUpperCase();
                f = value.charAt(9);
            }
            for (var i = 0; i < 26; i++) {
                if (d == a[i]) {
                    e = i + 10; //10
                    c[0] = Math.floor(e / 10); //1
                    c[1] = e - (c[0] * 10); //10-(1*10)
                    break;
                }
            }
            for (var i = 0; i < b.length; i++) {
                if (i < 2) {
                    g += c[i] * b[i];
                } else {
                    g += parseInt(value.charAt(i - 1)) * b[i];
                }
            }
            if ((g % 10) == f) {
                return true;
            }
            if ((10 - (g % 10)) != f) {
                return false;
            }
            return true;
        }

        function fridcheck(value) {
            if (! /^[a-zA-Z]{1}[a-dA-D1-2]{1}[0-9]{8}$/.test(value)) {
                return false;
            }
            else {
                var id_ = value.toUpperCase();
                var sum = 0;
                var str1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                var str2 = "1011121314151617341819202122352324252627282932303133";
                var t1 = str2.substr(str1.indexOf(id_.substr(0, 1)) * 2, 2);
                var t2 = str2.substr(str1.indexOf(id_.substr(1, 1)) * 2, 2);
                sum = t1.substr(0, 1) * 1 + t1.substr(1, 1) * 9;
                sum += (t2 % 10) * 8;
                var t10 = id_.substr(9, 1);
                for (t_i = 3; t_i <= 9; t_i++) {
                    sum += id_.substr(t_i - 1, 1) * (10 - t_i);
                }
                (sum % 10 == 0) ? t10_ = 0 : t10_ = 10 - (sum % 10);
                return (t10_ == t10) ? true : false;
            }
        }

        function verifyId(id) {

            id = id.trim();

            if (id.length != 10) {
                console.log("Fail，長度不正確");
                return false;
            }


            let countyCode = id.charCodeAt(0);
            if (countyCode < 65 | countyCode > 90) {
                console.log("Fail，字首英文代號，縣市不正確");
                return false;
            }

            let genderCode = id.charCodeAt(1);
            console.log(id.charCodeAt(1));
            if (genderCode != 49 && genderCode != 50 && genderCode != 56 && genderCode != 57) {
                console.log("Fail，性別代碼不正確");
                return false;
            }

            let serialCode = id.slice(2)
            for (let i in serialCode) {
                let c = serialCode.charCodeAt(i);
                if (c < 48 | c > 57) {
                    console.log("Fail，數字區出現非數字字元");
                    return false;
                }
            }

            let conver = "ABCDEFGHJKLMNPQRSTUVXYWZIO"
            let weights = [1, 9, 8, 7, 6, 5, 4, 3, 2, 1, 1]

            id = String(conver.indexOf(id[0]) + 10) + id.slice(1);

            checkSum = 0
            for (let i = 0; i < id.length; i++) {
                c = parseInt(id[i])
                w = weights[i]
                checkSum += c * w
            }

            verification = checkSum % 10 == 0

            if (verification) {
                return true;
            } else {
                return false;
            }

            return verification
        }

        function CheckVs(value) {
            var ss = document.getElementById('ContentPlaceHolder1_hid_vs').value;
            if (value == ss && ss != '') {
                $('#Numbermsg').css("color", "	#019858");
                $('#Numbermsg').text("✔");
            }
            else {
                $('#Numbermsg').text("驗證碼錯誤");
                $('#Numbermsg').css("color", "red");
                errorCount += 1;
            }

        }
        function CheckVsS(value) {
            var ss = document.getElementById('ContentPlaceHolder1_hid_vsS').value;
            if (value == ss && ss != '') {
                $('#SMSmsg').css("color", "	#019858");
                $('#SMSmsg').text("✔");
            }
            else {
                $('#SMSmsg').text("驗證碼錯誤");
                $('#SMSmsg').css("color", "red");
                errorCount += 1;
            }

        }
        function isPureChinese(input) {

            var reg = /^[\u4E00-\u9FA5]+$/
            if (reg.test(input)) {

                $('#namemsg').css("color", "#019858");
                $('#namemsg').text("✔");

            }
            else {

                $('#namemsg').text("請填寫中文姓名");
                $('#namemsg').css("color", "red");
                errorCount += 1;
            }
        }
    </script>

</asp:Content>

