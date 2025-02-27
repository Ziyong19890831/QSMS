<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="PersonnelAdmin_AE.aspx.cs" Inherits="Mgt_Personnel_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">


        $(function () {

            $(".datepicker").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'yy-mm-dd'
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


    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_PID" runat="server" />

    <div class="both mb20" id="div_DataInfo" runat="server" visible="false">
        <fieldset>
            <legend>資料異動訊息</legend>
            <div class="left w8">
                <asp:Label ID="lb_DataInfo" runat="server" Text="" ></asp:Label>
            </div>
            <div class="right mt-10">
            </div>
        </fieldset>
    </div>


    <table>
        <tr>
            <th><i class="fa fa-star"></i>角色別</th>
            <td colspan="3">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddl_Role" class="required" runat="server" DataTextField="RoleName" DataValueField="RoleSNO"></asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>所屬單位</th>
            <td colspan="3">
                <asp:UpdatePanel ID="upl_ddl" runat="server">
                    <ContentTemplate>
                        請輸入或選擇所屬單位：
                        <asp:HiddenField ID="HF_OrganSNO" runat="server" Value=""></asp:HiddenField>
                        <asp:Label ID="lb_OrganCodeName"  runat="server" Text="" Style="color: #019858;"></asp:Label>
                        <asp:Panel ID="Panel_Organ" runat="server">
                            <div style="margin-left: 50px;">
                                <span>輸入所屬單位代碼</span>
                                <input type="text" id="txt_Organ" style="width: 200px; height: 29px;" runat="server" onfocusout="orgcheck(this.value)" />
                                <asp:Label ID="msgOrgan" runat="server" Text=""></asp:Label>
                                <span id="orgmsg"></span>
                            </div>
                            <div style="margin-left: 50px;">
                                <span>或選擇所屬單位</span>
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
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>管理者帳號</th>
            <td>
                <input id="txt_Account" style="margin-bottom: 10px;" class="required" name="" type="text" maxlength="50" runat="server" />
            </td>
            <th><i class="fa fa-star"></i>管理者密碼</th>
            <td>
                <input type="password" name="name" value="" style="display:none;" title="檔自動填入用"  />
                <asp:TextBox ID="txt_PWD" runat="server" MaxLength="50" TextMode="Password"></asp:TextBox>
                <div id="PasswordPanel" runat="server" style="float: left;">
                    <input id="btn_ResetPSW" type="button" value="重置密碼" style="padding: 5px 10px; font-size: 16px;" onclick="showTxtPSW(1)" />
                    <input id="btn_CancelPSW" type="button" value="取消重置" style="padding: 5px 10px; font-size: 16px; display: none;" onclick="showTxtPSW(0)" />
                </div>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>管理者姓名</th>
            <td>
                <input id="txt_Name" style="margin-bottom: 10px;" class="required" name="name" type="text" maxlength="20" runat="server" />
            </td>
            <th>身分證字號</th>
            <td>
                <input id="txt_Personid" style="margin-bottom: 10px;" name="lbl_PersionId" type="text" maxlength="10" runat="server" />
            </td>
        </tr>
        <tr>
            <th>電話(宅)</th>
            <td>
                <input type="text" name="txt_Tel" id="txt_Tel" maxlength="50" runat="server" placeholder="Ex：02-12345678" />
            </td>
            <th>手機</th>
            <td>
                <input type="text" name="txt_Phone" id="txt_Phone" maxlength="50" runat="server" placeholder="Ex：0912-345678" />
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>電子郵件</th>
            <td colspan="3">
                <input type="text" class="required email" name="txt_Mail" id="txt_Mail" runat="server" maxlength="100" onfocusout="mailcheck(this.value)" /><br />
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>帳號狀態</th>
            <td colspan="3">
                <asp:DropDownList ID="ddl_IsEnable" runat="server">
                    <asp:ListItem Text="啟用" Value="1" />
                    <asp:ListItem Text="停用" Value="0" />
                </asp:DropDownList>
            </td>
        </tr>

    </table>


    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click"/>
        <input name="btnCancel" type="button" value="取消" onclick="location.href = 'PersonnelAdmin.aspx';"/>
    </div>


    <script>
        
        function orgcheck(dInput) {
            if (dInput != "") {
                $('#ContentPlaceHolder1_txt_Organ').css("border", "1px solid #ccc");

                $('#orgmsg').show();
                $.ajax({
                    url: "../Web/AccountAjax.aspx",
                    type: 'POST',
                    data: { orgid: dInput },
                    success: function (result) {
                        var rst = result.split(",");
                        if (rst[0] == "可使用") {
                            $('#ContentPlaceHolder1_txt_Organ').css("border", "1px solid #ccc");
                            $('#orgmsg').css("color", "	#019858");
                            $('#orgmsg').text("✔");
                            document.getElementById("ContentPlaceHolder1_HF_OrganSNO").value = rst[1];
                            document.getElementById("ContentPlaceHolder1_lb_OrganCodeName").innerText = rst[2];
                            //document.getElementById("ContentPlaceHolder1_lb_OrganName").innerText = "-" + rst[2];
                        }
                        else {
                            $('#ContentPlaceHolder1_txt_Organ').css("border", "1px dotted #f84b4b");
                            $('#orgmsg').css("color", "red");
                            $('#orgmsg').text(result);
                            return false;
                        }

                    }
                });

            }
        }

        function showTxtPSW(v) {
            if (v == 1) {
                document.getElementById("btn_ResetPSW").style.display = "none";
                document.getElementById("btn_CancelPSW").style.display = "block";
                document.getElementById("ContentPlaceHolder1_txt_PWD").style.display = "block";
            } else {
                document.getElementById("btn_ResetPSW").style.display = "block";
                document.getElementById("btn_CancelPSW").style.display = "none";
                document.getElementById("ContentPlaceHolder1_txt_PWD").style.display = "none";
            }
            document.getElementById("ContentPlaceHolder1_txt_PWD").value = "";
        }


    </script>


</asp:Content>

