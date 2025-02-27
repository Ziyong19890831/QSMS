<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="GetPAccount.aspx.cs" Inherits="Mgt_GetPAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style>
    table a {
        color:#000000 ;
    }
    .width160 {
        width: 160px;
    }
    .fa-star {
        color: #FF0000;
    }
    </style>
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">取得帳號</li>
        </ol>
    </nav>

    <div class="row mt30">
        <div class="col-12">
            <h5>
                <i class="fa fa-key"></i>
                取得帳號
            </h5>
        </div>
    </div>

    <asp:ScriptManager ID="SM" runat="server"></asp:ScriptManager>

    <div class="row">
        <div class="col-12">

            <table class="table table-striped">

                <tr>
                    <th class="width160">
                        <i class="fa fa-star"></i>
                        任職機構
                    </th>
                    <td>
                        <asp:UpdatePanel ID="upl_ddl" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>                               
                                <asp:HiddenField ID="HF_OrganSNO" runat="server" Value=""></asp:HiddenField>
                                <asp:Label ID="lb_OrganCodeName" runat="server" Text="" Style="color: #019858;"></asp:Label>
                                <span id="orgmsg"></span>
                                <asp:Panel ID="Panel_Organ" runat="server">
                                    <div>
                                        <span>輸入任職機構代碼-目前待業中請填000 <span style="color:red;font-weight:bold">若當時註冊帳號無執登，請填000</span></span>
                                        <input type="text" id="txt_Organ" class="form-control" runat="server" onfocusout="orgcheck(this.value)" />
                                        <asp:Label ID="msgOrgan" runat="server" Text=""></asp:Label>
                                    </div>
                                    <div>或</div>
                                    <div>
                                        <span>選擇任職機構</span>
                                        <asp:DropDownList ID="ddl_AreaCodeA" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddl_AreaCodeA_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_AreaCodeB" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddl_AreaCodeB_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                                            <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_OrganCode" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddl_OrganCode_SelectedIndexChanged" AutoPostBack="true" DataValueField="OrganSNO" DataTextField="ORGAN_NAME">
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
                    <th class="width160">
                        <i class="fa fa-star"></i>
                        身分證
                    </th>
                    <td>
                        <input name="txt_PersonID" id="txt_PersonID" class="form-control" runat="server" />
                        <asp:RequiredFieldValidator ID="rfv_PersonID" runat="server" ControlToValidate="txt_PersonID" ErrorMessage="不可為空" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th class="width160">
                        <i class="fa fa-star"></i>
                        信箱
                    </th>
                    <td>
                        <input name="txt_Mail" id="txt_Mail" class="form-control" runat="server" />
                        <asp:RequiredFieldValidator ID="rfv_Mail" runat="server" ControlToValidate="txt_Mail" ErrorMessage="不可為空" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>

            </table>


        </div>


        <div class="col-12 center">
            <asp:Button ID="btn_Reset" class="btn btn-success" Text="寄送取得帳號信件" runat="server" OnClick="btn_Reset_Click" />
            <input name="btn_Cancel" class="btn btn-light"  type="button" value="取消" onclick="location.href = 'Notice.aspx'" />
        </div>
    </div>

    
    <script>

        var errorCount = 0;
        $("#ContentPlaceHolder1_btn_Reset").click(function () {

            CheckTextError("<%=HF_OrganSNO.ClientID %>", "orgmsg", 20);
            if (errorCount > 0) {

                event.preventDefault();
            }
        });

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
    </script>
</asp:Content>

