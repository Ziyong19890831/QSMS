<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="Personnel_PWD.aspx.cs" Inherits="Web_Personnel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <%--    <nav aria-label="breadcrumb">
=======
        
<<<<<<< HEAD



=======
    <div class="row">
        <table class="table table-striped">
            <tr>
                <th><i class="fa fa-star"></i>舊密碼</th>
                <td>
                    <input class="form-control" id="txt_OldPswd" onfocusout="pwdChkOldS('<%=PACCOld %>',this.id, 'pwdcheckmsg')" type="password" runat="server" onkeypress="detectCapsLock(event)"/><span id="pwdcheckmsg" ></span>
                    <br />
                    <div id = "capStatus" style = "display:none"><font color = 'red'>請注意 ！Caps Lock 鍵目前為開啟狀態。</font></div> 
                    <input  type="checkbox" id="OldPwd" />顯示密碼
                    <input class="form-control" type="button" onclick="HrefFunction()" value="忘記密碼" />
                </td>
            </tr>
            <tr>
                <th><i class="fa fa-star"></i>新密碼</th>
                <td>
                    <input class="form-control" type="password" name="txt_NewPWD" id="txt_NewPWD" onfocusout="pswChkStrength('ContentPlaceHolder1_txt_NewPWD', 'ContentPlaceHolder1_txt_OKPWD', 'Ppwdmsg', 'pwdmsg', 'strength');" runat="server" style="width: 100%" onkeypress="RdetectCapsLock(event)" />
                    顯示密碼<input type="checkbox" id="show_password_F" />
                    <div id = "RcapStatus" style = "display:none"><font color = 'red'>請注意 ！Caps Lock 鍵目前為開啟狀態。</font></div> 
                    <span id="Ppwdmsg"></span>
                    密碼強度:<span style="color: red; font-size: 10pt">*請將密碼通過強度"中"以上"，長度不得小於八碼，須包含大小寫英文、數字。</span>
                    <div style="border: 3px solid #c1c1c1; border-radius: 30px; text-align: center; width: 200px; height: 29px; background-color: #eeeeee">
                        <div style="width: 64.6px; float: left; border-radius: 30px 0px 0px 30px;" id="strength_L">弱</div>
                        <div style="width: 64.6px; float: left; border-radius: 0px;" id="strength_M">中</div>
                        <div style="width: 64.7px; float: left; border-radius: 0px 30px 30px 0px;" id="strength_H">強</div>
                    </div>
                </td>
            </tr>
            <tr>
                <th><i class="fa fa-star"></i>確認新密碼</th>
                <td>
                    <input class="form-control" type="password" name="txt_OKPWD" id="txt_OKPWD" onfocusout="pswChkInputIsSame('ContentPlaceHolder1_txt_NewPWD', 'ContentPlaceHolder1_txt_OKPWD', 'pwdmsg');" runat="server" style="width: 100%" onkeypress="LdetectCapsLock(event)"/>
                    <div id = "LcapStatus" style = "display:none"><font color = 'red'>請注意 ！Caps Lock 鍵目前為開啟狀態。</font></div> 
                    <span id="pwdmsg" style="color: red; visibility: hidden">與密碼不相同</span>
                </td>
            </tr>
        </table>


        <input id="btn_submit" class="form-control" name="btn_Update" type="submit" value="修改" runat="server" onclick="checkinput()" onserverclick="btn_Update_Click" causesvalidation="false" />
>>>>>>> b89a605f6772872fc7d9a49d543c5df82d920d2a--%>
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">修改密碼</li>
        </ol>
    </nav>
    <div class="row mt30">
        <div class="col-12">
            <h5>
                <i class="fa fa-envelope"></i>
                修改密碼
            </h5>
        </div>
    </div>
    <div class="row mt10">
        <div class="col-12">
            <table class="table table-striped">
                <tr>
                    <th class="w3"><i class="fa fa-star"></i>舊密碼</th>
                    <td style="text-align: left">
                        <input id="txt_OldPswd" onfocusout="pwdChkOldS('<%=PACCOld %>',this.id, 'pwdcheckmsg')" class="w10 form-control" type="password" runat="server" onkeypress="detectCapsLock(event)" /><span id="pwdcheckmsg"></span>
                        <div id="capStatus" style="display: none"><font color='red'>請注意 ！Caps Lock 鍵目前為開啟狀態。</font></div>
                        <input type="checkbox" id="OldPwd" />
                        顯示密碼
                        <br />
                        <input type="button" onclick="HrefFunction()" class="btn btn-dark" value="忘記密碼" />
                    </td>
                </tr>
                <tr>
                    <th class="w3"><i class="fa fa-star"></i>新密碼</th>
                    <td style="text-align: left">
                        <input type="password" name="txt_NewPWD" id="txt_NewPWD" class="form-control" onfocusout="pswChkStrength('ContentPlaceHolder1_txt_NewPWD', 'ContentPlaceHolder1_txt_OKPWD', 'Ppwdmsg', 'pwdmsg', 'strength');" runat="server" style="width: 100%" onkeypress="RdetectCapsLock(event)" />
                        <input type="checkbox" id="show_password_F" />
                        顯示密碼
                        <div id="RcapStatus" style="display: none"><font color='red'>請注意 ！Caps Lock 鍵目前為開啟狀態。</font></div>
                        <br />
                        <span id="Ppwdmsg"></span>
                        <br />
                        密碼強度:<span style="color: red; font-size: 10pt">*請將密碼通過強度"中"以上"，長度不得小於八碼，須包含大小寫英文、數字。</span>
                        <div style="border: 3px solid #c1c1c1; border-radius: 30px; text-align: center; width: 200px; height: 29px; background-color: #eeeeee">
                            <div style="width: 64.6px; float: left; border-radius: 30px 0px 0px 30px;" id="strength_L">弱</div>
                            <div style="width: 64.6px; float: left; border-radius: 0px;" id="strength_M">中</div>
                            <div style="width: 64.7px; float: left; border-radius: 0px 30px 30px 0px;" id="strength_H">強</div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th class="w3"><i class="fa fa-star"></i>確認新密碼</th>
                    <td style="text-align: left">
                        <input type="password" name="txt_OKPWD" id="txt_OKPWD" class="form-control" onfocusout="pswChkInputIsSame('ContentPlaceHolder1_txt_NewPWD', 'ContentPlaceHolder1_txt_OKPWD', 'pwdmsg');" runat="server" style="width: 100%" onkeypress="LdetectCapsLock(event)" />
                        <br />
                        <div id="LcapStatus" style="display: none"><font color='red'>請注意 ！Caps Lock 鍵目前為開啟狀態。</font></div>
                        <br />
                        <span id="pwdmsg" style="color: red; visibility: hidden">與密碼不相同</span>
                    </td>
                </tr>
            </table>

        </div>
        <div class="col-12 center">
            <input id="btn_submit" name="btn_Update" type="submit" class="btn btn-success" value="修改" runat="server" onclick="checkinput()" onserverclick="btn_Update_Click" causesvalidation="false" style="width: 160px;" />
        </div>
    </div>


    <script>

        function checkinput() {

            CheckTextError('txt_OldPWD', 'oldPwdmsg', 50);
            CheckTextError('txt_NewPWD', 'Ppwdmsg', 50);
            CheckTextError('txt_OKPWD', 'pwdmsg', 50);

            if ($(<%= txt_NewPWD.ClientID%>).val() != $(<%= txt_OKPWD.ClientID%>).val()) {
                event.preventDefault();
            }

            if ($('#strength_L').css("background-color") == "rgb(253, 220, 151)") {
                alert("密碼強度過弱請修改!");
                event.preventDefault();
            }
        }


    </script>
    <script type="text/javascript">

        function pwdChkOldS(dInput) {

            var ACC = "<%= PACCOld %>";
            var checkPsw = $("#ContentPlaceHolder1_txt_OldPswd").val();

            if (dInput != "") {
                $('#pwdcheckmsg').show();
                $.ajax({
                    url: "AccountAjax.aspx",
                    type: 'POST',
                    data: { pwd: checkPsw, account: ACC, personid: "ABC" },
                    success: function (result) {
                        if (result == "密碼正確") {
                            $('#txt_OldPswd').css("border", "1px solid #ccc");
                            $('#pwdcheckmsg').css("color", "	#019858");
                            $('#pwdcheckmsg').text("✔" + result);
                        }
                        else {
                            var results = "密碼不正確"
                            $('#txt_OldPswd').css("border", "1px dotted #f84b4b");
                            $('#pwdcheckmsg').css("color", "red");
                            $('#pwdcheckmsg').text(results);

                            return false;
                        }
                    }
                });
            }
            else if (dInput == "" || dInput == null) {
                $('#pwdcheckmsg').text("這是必填的欄位 ");
                $('#pwdcheckmsg').css("color", "red");
                qucss("ContentPlaceHolder1_txt_OldPswd", "pwdcheckmsg", "S")
            }
            else {
                $('#pwdcheckmsg').hide();
            }


        }
        function detectCapsLock(e) {
            valueCapsLock = e.keyCode ? e.keyCode : e.which;
            valueShift = e.shiftKey ? e.shiftKey : ((valueCapsLock == 16) ? true : false);
            if (((valueCapsLock >= 65 && valueCapsLock <= 90) && !valueShift) || ((valueCapsLock >= 97 && valueCapsLock <= 122) && valueShift))
                document.getElementById('capStatus').style.display = 'block';
            else
                document.getElementById('capStatus').style.display = 'none';
        }
        function RdetectCapsLock(e) {
            valueCapsLock = e.keyCode ? e.keyCode : e.which;
            valueShift = e.shiftKey ? e.shiftKey : ((valueCapsLock == 16) ? true : false);
            if (((valueCapsLock >= 65 && valueCapsLock <= 90) && !valueShift) || ((valueCapsLock >= 97 && valueCapsLock <= 122) && valueShift))
                document.getElementById('RcapStatus').style.display = 'block';
            else
                document.getElementById('RcapStatus').style.display = 'none';
        }
        function LdetectCapsLock(e) {
            valueCapsLock = e.keyCode ? e.keyCode : e.which;
            valueShift = e.shiftKey ? e.shiftKey : ((valueCapsLock == 16) ? true : false);
            if (((valueCapsLock >= 65 && valueCapsLock <= 90) && !valueShift) || ((valueCapsLock >= 97 && valueCapsLock <= 122) && valueShift))
                document.getElementById('LcapStatus').style.display = 'block';
            else
                document.getElementById('LcapStatus').style.display = 'none';
        }
        $('#OldPwd').click(function () {
            // 如果是勾選則...
            if (this.checked) {
                $(<%= txt_OldPswd.ClientID%>).prop('type', 'text');

            } else {
                $(<%= txt_OldPswd.ClientID%>).prop('type', 'password');

            }
        });
        $('#show_password_F').click(function () {
            // 如果是勾選則...
            if (this.checked) {
                $(<%= txt_NewPWD.ClientID%>).prop('type', 'text');
                $(<%= txt_OKPWD.ClientID%>).prop('type', 'text');
            } else {
                $(<%= txt_NewPWD.ClientID%>).prop('type', 'password');
                $(<%= txt_OKPWD.ClientID%>).prop('type', 'password');
            }
        });
    </script>
</asp:Content>

