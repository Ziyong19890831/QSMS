<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="Personnel_AE.aspx.cs" Inherits="Web_Personnel_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../JS/jquery-ui.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../CSS/jquery-ui.css" />
    <script type="text/javascript">

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

    <style>
    table a {
        color:#000000 ;
    }
    width160 {
        width: 160px;;
    }
    .fa-star {
        color: #FF0000;
    }
    </style>
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">學員資料</li>
        </ol>
    </nav>

    <h5 class="pt30"><i class="fa fa-question-circle"></i>學員資料</h5>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_PID" runat="server" />
    <asp:HiddenField ID="hf_OrganCode" runat="server" />
    <div class="row">
        <div class="col-12">
            <table class="table table-striped">
                <tr>
                    <th class="width160"><i class="fa fa-star"></i>任職單位</th>
                    <td>
                        <asp:UpdatePanel ID="upl_ddl" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="HF_OrganSNO" runat="server" Value=""></asp:HiddenField>
                                <asp:Label ID="lb_OrganCodeName" runat="server" Text="" Style="color: #019858;"></asp:Label><br />
                                <asp:Label ID="Label1" runat="server" Text="" >若任職單位有變動可自行更改，若暫無執登請填000</asp:Label>
                                <asp:Panel ID="Panel_Organ" runat="server">
                                    <div>
                                        <span>輸入任職機構代碼</span>
                                        <input type="text" id="txt_Organ" class="form-control" runat="server" onfocusout="orgcheck(this.value)" />
                                        <asp:Label ID="msgOrgan" runat="server" Text=""></asp:Label>
                                        <span id="orgmsg"></span>
 <%--                                       <div id="Person_Experience" style="display: none" runat="server">
                                            <span>任職機關/機構</span>
                                            <asp:TextBox runat="server" TextMode="MultiLine" class="form-control" ID="txt_Experience" cols="40" Rows="3"></asp:TextBox>
                                        </div>--%>
                                    </div>
                                    <div>
                                        <span>或選擇任職機構</span>
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
                <%--<tr>
                    <th class="width160"><i class="fa fa-star"></i>任職單位之部門	</th>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList CssClass="form-control" ID="ddl_Job" runat="server" DataTextField="JobName" DataValueField="JSNO" AutoPostBack="true" OnSelectedIndexChanged="ddl_Job_SelectedIndexChanged"></asp:DropDownList>
                                
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_PJNote" class="form-control" runat="server" TextMode="MultiLine" cols="40" Rows="3" Visible="false"></asp:TextBox>
                                
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_Job" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>--%>
                <tr>
                    <th><i class="fa fa-star"></i>服務科別</th>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_TsType" class="form-control" runat="server" AutoPostBack="true" DataValueField="TSSNO" DataTextField="TsTypeName" OnSelectedIndexChanged="ddl_TsType_SelectedIndexChanged"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_TSNote" class="form-control" runat="server" TextMode="MultiLine" cols="40" Rows="3"  Visible="false"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_TSType" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>                        
                    </td>
                </tr>
                <tr>
                    <th class="width160">角色別</th>
                     <td>
                <asp:Label ID="lb_Role"  runat="server"></asp:Label> 
                <% if (RoleName == "衛教師") {  %>
                     執業科別<asp:Label ID="txt_TJobType" runat="server"></asp:Label>
                     服務科別<asp:Label ID="txt_TSType" runat="server"></asp:Label>
                <% }  %>
            </td>
                    </tr>
                <tr>
            <th class="width160">角色別2</th>
            <td><asp:Label ID="lb_Role2" runat="server"></asp:Label></td>                 
                </tr>
                <tr>
                    <th class="width160">帳號</th>
                    <td>
                         <asp:Label ID="lb_Account"  runat="server"></asp:Label> 
                    </td>
                    </tr>
                <tr>
                    <th class="width160">使用者姓名</th>
                    <td><asp:Label ID="txt_Name" runat="server"></asp:Label></td>
                    </tr>
                <tr>
                    <th class="width160">身分證字號</th>
                    <td><asp:Label ID="txt_Personid" runat="server"></asp:Label></td>
                </tr>

                <tr>
                    <th class="width160"><i class="fa fa-star"></i>Email</th>
                    <td>
                        <input type="text" id="txt_Mail" class="form-control" runat="server" maxlength="100" onfocusout="mailcheck(this.value)" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_Mail" ErrorMessage="內容不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <span id="mailmsg"></span>
                    </td>
                </tr>
                <tr>
                    <th class="width160">學歷</th>
                    <td><input id="txt_degree" type="text" class="form-control" maxlength="50" runat="server" /></td>
                </tr>
                <tr>
                    <th class="width160">國籍</th>
                    <td><input id="txt_country" type="text" class="form-control" maxlength="100" runat="server" /></td>
                    </tr>
                    <tr>
                    <th class="width160"><i class="fa fa-star"></i>出生年月日</th>
                    <td><input id="txt_Birthday" class="datepicker date form-control" type="text" runat="server" /> <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_Birthday" ErrorMessage="內容不得為空" ForeColor="Red"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <th class="width160"><i class="fa fa-star"></i>通訊地址</th>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" style="float: left">
                            <ContentTemplate>
                                                        <input id="txt_contact_ZipCode" type="text" disabled="disabled" class="form-control number" placeholder="區號" maxlength="5" runat="server" style="width:160px;" />
                        市/縣： 
                        <asp:DropDownList ID="ddl_AddressC" runat="server" OnSelectedIndexChanged="ddl_AddressC_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME"></asp:DropDownList>
                        區/鎮/鄉：
                        <asp:DropDownList ID="ddl_AddressD" runat="server" OnSelectedIndexChanged="ddl_AddressB_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME"></asp:DropDownList>
                        <input id="txt_contact_address" type="text" class="form-control" maxlength="50"  runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_contact_address" ErrorMessage="地址不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_contact_ZipCode" ErrorMessage="區號不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th class="width160">通訊電話</th>
                    <td>
                        <input type="text" id="txt_Tel" maxlength="50" class="form-control" runat="server" placeholder="Ex：02-12345678" />
                    </td>
                    </tr>
                <tr>
                    <th class="width160"><i class="fa fa-star"></i>手機</th>
                    <td>
                        <input type="text" id="txt_Phone" maxlength="10" class="form-control" runat="server" placeholder="Ex：0912345678" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_Phone" ErrorMessage="內容不得為空" ForeColor="Red"></asp:RequiredFieldValidator><br />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"  ControlToValidate="txt_Phone" ErrorMessage="請輸入10個數字" ForeColor="Red"  ValidationExpression="\d{10}"></asp:RegularExpressionValidator> 
                    </td>
                </tr>
            
            </table>

        </div>
    </div>

 <div class="both mb20" id="div_MP" runat="server" visible="false">
        <fieldset>
            <legend>以下是從醫事人員資料表帶入的</legend>
            <table>
                <tr>
                    <th class="w2">執業執照字號</th>
                    <td class="w3"><asp:Label ID="lb_LCN" runat="server"></asp:Label></td>
                    <th class="w2">執照有效日期</th>
                    <td class="w3"><asp:Label ID="lb_VEDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>執業執照是否有效</th>
                    <td><asp:Label ID="lb_LValid" runat="server"></asp:Label></td>
                    <th>執業狀態</th>
                    <td><asp:Label ID="lb_LStatus" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>機構狀態</th>
                    <td><asp:Label ID="lb_AbortDate" runat="server"></asp:Label></td>
                    <th>機構類別</th>
                    <td><asp:Label ID="lb_organClassName" runat="server"></asp:Label></td>
                </tr>
                <tr >
                    <th>機構代碼/名稱</th>
                    <td colspan="3"><asp:Label ID="lb_OrganCode" runat="server"></asp:Label>/<asp:Label ID="lb_OrganName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>機構聯絡電話</th>
                    <td colspan="3"><asp:Label ID="lb_OrganTel" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>機構地址</th>
                    <td colspan="3"><asp:Label ID="lb_OrganAddr" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>備註</th>
                    <td colspan="3"><asp:Label ID="lb_Note" runat="server"></asp:Label></td>
                </tr>
                <% if(userInfo.RoleSNO == "10") {  %>
                <tr>
                    <th>執業科別</th>
                    <td colspan="3"><asp:Label ID="lb_LRtype" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>專科</th>
                    <td colspan="3"><asp:Label ID="lb_LSType" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>專科證書字號</th>
                    <td colspan="3"><asp:Label ID="lb_LSCN" runat="server"></asp:Label></td>
                </tr>
                <% } %>
            </table>
        </fieldset>
    </div>    <div class="row mt10">
        <div class="col-12 center">
            <input name="btn_submit" type="submit" value="修改" class="btn btn-success" style="width: 200px;"  runat="server" onserverclick="btn_submit_Click"  />
        </div>
    </div>
    <script type="text/javascript">
        var errorCount = 0;
        function checkinput() {
            errorCount = 0;
            mailcheck($("#<%=txt_Mail.ClientID %>").val());

            if (errorCount > 0) {

                event.preventDefault();
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
<%--            if (dInput == '000') {
                $('#<%=Person_Experience.ClientID %>').css('display', 'block')
            }
            else {
                $('#<%=Person_Experience.ClientID %>').css('display', 'none')
            }--%>
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
                $.ajax({
                    url: "AccountAjax.aspx",
                    type: 'POST',
                    async: false,
                    data: { account: dInput, pmail: dInput },
                    success: function (result) {
                        debugger;
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
    </script>
</asp:Content>

