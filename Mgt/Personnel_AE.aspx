<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Personnel_AE.aspx.cs" Inherits="Mgt_Personnel_AE" %>

<%@ Register Src="~/Web/WUC_Country.ascx" TagPrefix="uc1" TagName="WUC_Country" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        function showTxtPSW(v) {
            if (v == 1) {
                document.getElementById("btn_ResetPSW").style.display = "none";
                document.getElementById("btn_CancelPSW").style.display = "block";
                document.getElementById("<%=txt_PWD.ClientID %>").style.display = "block";
            } else {
                document.getElementById("btn_ResetPSW").style.display = "block";
                document.getElementById("btn_CancelPSW").style.display = "none";
                document.getElementById("<%=txt_PWD.ClientID %>").style.display = "none";
            } 
            document.getElementById("<%=txt_PWD.ClientID %>").value = "";
        }

        $(function () {

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

            if ($("#ContentPlaceHolder1_Work").val() == "NEW") {
                $(".NewData").remove();
            }
        });

    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_PID" runat="server" />


    <div class="both mb20" id="div_DataInfo" runat="server" visible="false">
        <fieldset>
            <legend>資料異動訊息</legend>
            <div class="left w8">
                <asp:Label ID="lb_DataInfo" runat="server" Text=""></asp:Label><p></p>
                 <asp:Label ID="lb_UserInfo" runat="server" Text=""></asp:Label>
            </div>
            <div class="right mt-10">
            </div>
        </fieldset>
    </div>



    <table>
        <tr>
            <th><i class="fa fa-star"></i>執業醫事機構</th>
            <td colspan="3">
                <asp:UpdatePanel ID="upl_ddl" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        請輸入或選擇執業單位：
                        <asp:HiddenField ID="HF_OrganSNO" runat="server" Value=""></asp:HiddenField>
                        <asp:Label ID="lb_OrganCodeName" runat="server" Text="" Style="color: #019858;"></asp:Label>
                        <asp:Panel ID="Panel_Organ" runat="server">
                            <div style="margin-left: 20px;">
                                <span>輸入執業醫事機構代碼</span>
                                <input type="text" id="txt_Organ" style="width: 200px; height: 29px;" runat="server" onfocusout="orgcheck(this.value)" />
                                <asp:Label ID="msgOrgan" runat="server" Text=""></asp:Label>
                                <span id="orgmsg"></span>
                            </div>
                            <div style="margin-left: 20px;">
                                <span>或選擇執業醫事機構</span>
                                <asp:DropDownList ID="ddl_AreaCodeA" runat="server" OnSelectedIndexChanged="ddl_AreaCodeA_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddl_AreaCodeB" runat="server" OnSelectedIndexChanged="ddl_AreaCodeB_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                                    <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddl_OrganCode" runat="server" OnSelectedIndexChanged="ddl_OrganCode_SelectedIndexChanged" AutoPostBack="true" DataValueField="OrganSNO" DataTextField="ORGAN_NAME">
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
               <p> <asp:Label runat="server" ID="lb_HiddenOrganName" ForeColor="Red"></asp:Label> </p>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>角色別</th>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddl_Role" runat="server" AutoPostBack="True" class="required" DataTextField="RoleName" DataValueField="RoleSNO" OnSelectedIndexChanged="ddl_Role_SelectedIndexChanged"></asp:DropDownList>
                        <span id="rolemsg"></span>
                        <div id="Guardian" runat="server" visible="false">
                            執業科別
                                <input type="text" name="txt_TJobType" id="txt_TJobType" runat="server" maxlength="20" />
                            服務科別
                                <input type="text" name="txt_TSType" id="txt_TSType" runat="server" maxlength="50" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddl_Role" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                <p><asp:Label runat="server" ID="lb_HiddenRole" ForeColor="Red"></asp:Label> </p>
                <p><asp:Label runat="server" ID="lb_HiddenTJobType" ForeColor="Red"></asp:Label> </p>
                <p><asp:Label runat="server" ID="lb_HiddenTSTypeRole13" ForeColor="Red"></asp:Label> </p>
            </td>
             <th><i class="fa fa-star"></i>服務科別</th>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" >
                        <ContentTemplate>
                            <asp:DropDownList ID="ddl_TSType" runat="server" DataValueField="TsSNO" DataTextField="TsTypeName" AutoPostBack="true" OnSelectedIndexChanged="ddl_TSType_SelectedIndexChanged"></asp:DropDownList>
                           
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddl_Role" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                                 <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                           <asp:TextBox ID="txt_TSNote" runat="server" TextMode="MultiLine" Enabled="false" Width="100%" Height="50px" Visible="false"></asp:TextBox>
                           
                        </ContentTemplate>
                        <Triggers>
                            
                            <asp:AsyncPostBackTrigger ControlID="ddl_TSType" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <p><asp:Label runat="server" ID="lb_HiddenTSType" ForeColor="Red"></asp:Label> </p>
                </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>帳號</th>
            <td>
                <input style="display:none" type="text" name="txt_Account" />
                <input id="txt_Account" class="required" type="text" maxlength="50" runat="server" onfocusout="acccheck(this.value)" style="ime-mode: disabled" onkeyup="value=value.replace(/[^\w\.\/]/ig,'')"/>
                    <span id="accmsg" class="ErrorMsg"></span>
                <p><asp:Label runat="server" ID="lb_HiddenAccount" ForeColor="Red"></asp:Label> </p>
            </td>
            <th><i class="fa fa-star"></i>使用者密碼</th>
            <td>
               <input type="password" style="display:none" />
                <asp:TextBox ID="txt_PWD" runat="server" MaxLength="50" name="txt_PWD" TextMode="Password"></asp:TextBox>
                <div id="PasswordPanel" runat="server" style="float: left;">
                    <input id="btn_ResetPSW" type="button" value="重置密碼" style="padding: 5px 10px; font-size: 16px;" onclick="showTxtPSW(1)" />
                    <input id="btn_CancelPSW" type="button" value="取消重置" style="padding: 5px 10px; font-size: 16px; display: none;" onclick="showTxtPSW(0)" />
                    <span id="Ppwdmsg" class="ErrorMsg"></span>
                </div>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>使用者姓名</th>
            <td>
              <input id="txt_Name" class="required" name="name" type="text" maxlength="50" runat="server" onfocusout="namecheck(this.value)" />
                    <span id="namemsg" class="ErrorMsg"></span>
                <p><asp:Label runat="server" ID="lb_HiddenName" ForeColor="Red"></asp:Label> </p>
            </td>
            <th><i class="fa fa-star"></i>身分證/居留證</th>
            <td>
                    <input id="txt_Personid" class="required" name="lbl_PersionId" type="text" maxlength="11" onfocusout="pidcheck(this.value)" runat="server" />
                    <span id="pidmsg" class="ErrorMsg"></span>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>Email</th>
            <td>
                <!-- fake fields are a workaround for chrome autofill getting the wrong fields -->
                <input style="display:none" type="text" name="txt_Mail"/>
                
                <input type="text" name="txt_Mail" id="txt_Mail" class="required email w8" runat="server" maxlength="100" onfocusout="mailcheck(this.value)" />
               
                <span id="mailmsg" class="ErrorMsg"></span>
                <p><asp:Label runat="server" ID="lb_HiddenMail" ForeColor="Red"></asp:Label> </p>
            </td>
            <th>學歷</th>
            <td>
                <input id="txt_degree" name="txt_degree" type="text" maxlength="50" runat="server" />
                <p><asp:Label runat="server" ID="lb_Hiddendegree" ForeColor="Red"></asp:Label> </p>
            </td>
        </tr>
        <tr>
            <th>國籍</th>
            <td>
                <uc1:WUC_Country runat="server" ID="WUC_Country" />
                 <p><asp:Label runat="server" ID="lb_HiddenCountry" ForeColor="Red"></asp:Label> </p>
            </td>
            <th>出生年月日</th>
            <td>
                <input id="txt_Birthday" class="datepicker date" type="text" runat="server" />
                <p><asp:Label runat="server" ID="lb_HiddenBirthday" ForeColor="Red"></asp:Label> </p>
            </td>
        </tr>
        <tr>
            <th>通訊地址</th>
            <td colspan="3">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" style="float: left">
                    <ContentTemplate>
                        郵區：
                        <input id="txt_ZipCode" type="text" disabled="disabled" class="number" placeholder="區號" maxlength="5" runat="server" style="width: 80px;" />
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

                <input id="txt_Addr" class="w3" type="text" maxlength="50" runat="server" /><br /><br />
                <asp:Label runat="server" ID="lb_HiddenAddr" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr id="Psex" runat="server">
            <th>性別</th>
            <td colspan="3">
                <asp:RadioButtonList ID="rbl_Sex" runat="server" >
                    <asp:ListItem Text="男" Value="1"></asp:ListItem>
                    <asp:ListItem Text="女" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
                <asp:Label runat="server" ID="lb_Hiddensex" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>通訊電話</th>
            <td>
                <input type="text" name="txt_Tel" id="txt_Tel" maxlength="50" runat="server" placeholder="Ex：02-12345678" /><br />
                <asp:Label runat="server" ID="lb_HiddenTel" ForeColor="Red"></asp:Label>
            </td>
            <th>手機</th>
            <td>
                <input type="text" name="txt_Phone" id="txt_Phone" maxlength="50" runat="server" placeholder="Ex：0912345678" />
                 <asp:Label runat="server" ID="lb_HiddenPhone" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>備註</th>
            <td colspan="3">
                <input type="text" class="w10" name="txt_Note" id="txt_Note" runat="server" /><br />
                <asp:Label runat="server" ID="lb_HiddenNote" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>帳號狀態</th>
            <td>
                <asp:DropDownList ID="ddl_IsEnable" runat="server">
                    <asp:ListItem Text="啟用" Value="1" />
                    <asp:ListItem Text="停用" Value="0" />
                </asp:DropDownList>
                <asp:Label runat="server" ID="lb_HiddenIsEnable" ForeColor="Red"></asp:Label>
            </td>
            <th><i class="fa fa-star"></i>學員狀態</th>
            <td>
                <asp:DropDownList ID="ddl_Status" runat="server" DataTextField="MName" DataValueField="MStatusSNO"></asp:DropDownList>
                <asp:Label runat="server" ID="lb_HiddenStatus" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>



     <div class="both mb20" id="div_MP" runat="server">
                <fieldset>
                    <legend>以下是從醫事人員資料表帶入的</legend>
                    <table>
                        
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
                    </table>
                </fieldset>
            </div>



    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click" OnClientClick="checkinput()"  CausesValidation="false"/>
        <asp:Button ID="btn_Prview" runat="server" Text="預覽" OnClick="btn_Prview_Click" visible="true"/>
        <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" />
       
    </div>


   <script type="text/javascript">

       //點送出時的欄位資料確認
       var errorCount = 0;

       if (location.search == '?Work=N') {
           function checkinput() {
               errorCount = 0;
               CheckTextError("<%=HF_OrganSNO.ClientID %>", "orgmsg", 20);
               CheckTextError("<%=ddl_Role.ClientID %>", "rolemsg", 50);
               CheckTextError("<%=txt_Account.ClientID %>", "accmsg", 50);
               acccheck($("#<%=txt_Account.ClientID %>").val());
               CheckTextError("<%=txt_PWD.ClientID %>", "Ppwdmsg", 50);
               CheckTextError("<%=txt_Name.ClientID %>", "namemsg", 50);
               CheckTextError("<%=txt_Personid.ClientID %>", "pidmsg", 50);
               pidcheck($("#<%=txt_Personid.ClientID %>").val());
               CheckTextError("<%=txt_Mail.ClientID %>", "mailmsg", 50);
               mailcheck($("#<%=txt_Mail.ClientID %>").val());
               pswcheck();


               if (errorCount > 0) {

                   event.preventDefault();
               }

           }
       }
       else {
           //function checkinput() {
           //    //尚未完成
           //    if ($('.ErrorMsg').css("color", "red").length) {

           //        event.preventDefault();
           //    }
           //}
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
        }

        function mailcheck(dInput) {

            var emailcheck = "";

            var emailRegxp = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z]+$/;
            if (emailRegxp.test(dInput) != true) {
                emailcheck = "no";
            }

            if (dInput != "" && emailcheck == "") {

                $('#mailmsg').show();
                $.ajax({
                    url: "AccountAjax.aspx",
                    type: 'POST',
                    async:false,
                    data: { account: dInput, personid: "#" },
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
                if (dInput != "" && emailcheck == "no") {
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
                    async :false,
                    data: { account: "0", personid: dInput },
                    success: function (result) {
                        //先查身分證或居留證是否重複
                        if (result == "可使用") {
                           
                            if (dInput.length == 10) {
                                if (verifyId(dInput) == true) {
                                    $('#pidmsg').css("color", "	#019858");
                                    $('#pidmsg').text("✔" + result);
                                    $('#<%=txt_Personid.ClientID %>').css("border", "1px solid #ccc");
                                 
                                }
                                else {
                                    
                                    alert('錯誤');
                                    $('#pidmsg').text("不符合身分證或居留證規則");
                                    $('#pidmsg').css("color", "red");
                                    $('#<%=txt_Personid.ClientID %>').css("border", "1px dotted #f84b4b");
                                    errorCount += 1;
                                   
                                }
                            }
                            else {
                               alert('錯誤');
                                $('#pidmsg').text("不符合身分證或居留證字元長度規則");
                                $('#pidmsg').css("color", "red");
                                $('#<%=txt_Personid.ClientID %>').css("border", "1px dotted #f84b4b");
                                 errorCount += 1;
                            }

                        }
                        else {
                          
                            $('#pidmsg').css("color", "red");
                            $('#pidmsg').text(result);
                            $('#<%=txt_Personid.ClientID %>').css("border", "1px dotted #f84b4b");
                            errorCount += 1;
                        }

                    }
                });

           }
       }

       function pswcheck() {
           var msg = "";
           if ($('#strength_L').css("background-color") == "rgb(253, 220, 151)") {
               msg += "密碼強度過弱請修改!\n";
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

   </script>


</asp:Content>

