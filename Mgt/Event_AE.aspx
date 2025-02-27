<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Event_AE.aspx.cs" Async="true" Inherits="Mgt_Event_AE" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../ckeditor/ckeditor.js"></script>
    <script type="text/javascript">

        $(function () {
            $("#<%=txt_CStartDay_F.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
            $("#<%=txt_CEndDay_F.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });

            $("#<%=txt_SignS_F.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
            $("#<%=txt_SignE_F.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });

        });


        $.datepicker.setDefaults($.datepicker.regional["zh-TW"]);

        $(function () {
            var select = document.getElementById('ContentPlaceHolder1_ddl_Class2');

            for (i = 0; i < select.length; i++) {
                if (select.options[i].value == '0') {
                    select.remove(i)
                }
            }
        });
        
        function LinkRoleGroupLink() {
            var winvar = window.open('./EventRoleGroupLog.aspx', 'winname', 'width=1200 height=550 location=no,menubar=no status=no,toolbar=no');
        }
    </script>
    <style type="text/css">
        .mydropdownlist {
            float: left;
        }

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
            line-height: 25px;
            display: none;
            overflow-y: auto;
        }
/*        img {
            width: 400px;
        }*/
        .diloagtable{
             background-color: gray ;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="path txtS mb20">現在位置：課程管理<i class="fa fa-angle-right"></i>報名管理<i class="fa fa-angle-right"></i><a href="#"><asp:Label ID="lb_Mark" runat="server" Text="新增"></asp:Label></a></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />
    <asp:HiddenField ID="lb_EventIdentity" runat="server" />
    <br />

            <table>
                <tr>
                    <th><i class="fa fa-star"></i>課程編號</th>
                    <td colspan="3">
                        <asp:Label runat="server" ID="lb_Code"></asp:Label>-
                        <asp:TextBox ID="txt_EventGNO" runat="server" placeholder="限輸入五位數字" Height="35px" class="w1"></asp:TextBox>
                        <input type="button" onclick="LinkRoleGroupLink()" value="歷史編號查詢" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"  ControlToValidate="txt_EventGNO" ErrorMessage="請輸入5個數字"  ValidationExpression="\d{5}" ForeColor="Red"></asp:RegularExpressionValidator> 
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="編號查詢不得為空" ControlToValidate="txt_EventGNO" ForeColor="Red"></asp:RequiredFieldValidator>
                        
                    </td>
<%--                    <th><i class="fa fa-star"></i>Webbex</th>
                    <td>                        
                        <asp:TextBox ID="txt_Webbex" runat="server" Height="35px" class="w3"></asp:TextBox>
                        
                    </td>--%>
                </tr>
    
                <tr>
                    <th><i class="fa fa-star"></i>課程名稱</th>
                    <td colspan="3">
                        <br />
                        <asp:TextBox ID="txt_Title" runat="server" placeholder="最多50字元" Height="35px" class="w10"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv_Title" runat="server" ErrorMessage="課程名稱不得為空" ControlToValidate="txt_Title" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>適用人員</th>
                    <td colspan="3">
                        <asp:CheckBoxList ID="cb_Role" class="required" runat="server" RepeatColumns="4" DataTextField="RoleName" DataValueField="RoleSNO" RepeatLayout="Table" OnSelectedIndexChanged="cb_Role_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>課程類別</th>
                    <td colspan="3">
                        <asp:UpdatePanel runat="server" ID="UPClass">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_EventClass" runat="server" AutoPostBack="true" DataTextField="ClassName" DataValueField="EventCSNO">
                                </asp:DropDownList>      
                               <%-- <asp:DropDownList ID="ddl_EventRole" runat="server" AutoPostBack="true" DataTextField="ERName" DataValueField="ERSNO">
                                </asp:DropDownList>--%>
                                <asp:DropDownList ID="ddl_Class1" runat="server" AutoPostBack="true" CssClass="mydropdownlist" DataTextField="MVal" DataValueField="PVal">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddl_Class2" runat="server" AutoPostBack="true" Enable="false" CssClass="mydropdownlist" DataTextField="MVal" DataValueField="PVal">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddl_Class2" ErrorMessage="內容不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_Class1" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th>報名規則</th>
                    <td colspan="3">
                        <asp:CheckBox ID="chk_cancel" runat="server" Text="取消報名限制" />
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>內容</th>
                    <td colspan="3">
                        <textarea name="editor1" id="editor1" rows="50" cols="80" runat="server"></textarea>
                        <%--<asp:RequiredFieldValidator ID="rfv_Info" runat="server" ControlToValidate="editor1" ErrorMessage="內容不得為空" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>

                <tr>
                    <th><i class="fa fa-star"></i>可報名人數</th>
                    <td>
                        <asp:Label ID="lb_CountLimit" runat="server" Text="如有填寫，則限制，如填寫0，則無限制人數。" Font-Size="Smaller"></asp:Label>
                        <br />
                        <asp:TextBox ID="txt_CountLimit" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="rev_CL" runat="server" ControlToValidate="txt_CountLimit" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txt_CountLimit" ErrorMessage="內容不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    <th><i class="fa fa-star"></i>錄取人數</th>
                    <td>
                        <asp:TextBox ID="txt_CountAdmit" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv_CA" runat="server" ControlToValidate="txt_CountAdmit" ErrorMessage="錄取人數不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="rev_CA" runat="server" ControlToValidate="txt_CountAdmit" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>認定時數</th>
                    <td>
                        <asp:TextBox ID="txt_TargetHour" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txt_TargetHour" ErrorMessage="認定時數不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ControlToValidate="txt_TargetHour" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>

                    </td>
                    <th><i class="fa fa-star"></i>證明類別</th>
                    <td>
                        <asp:TextBox ID="txt_QTypeName" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_QTypeName" ErrorMessage="證書類別不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>課程費用</th>
                    <td>
                        <asp:TextBox ID="txt_ActiveCost" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator20" runat="server" ControlToValidate="txt_ActiveCost" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>
                    </td>
                    <th>提供膳食</th>
                    <td>
                        <asp:CheckBox ID="chk_Meals" runat="server" Text="是" />
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>報名期間(起)</th>
                    <td>
                        <asp:Label ID="Label12" runat="server" Text="格式範例:2017-01-01" Font-Size="Smaller"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txt_SignS_F" ErrorMessage="開始日期不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server" ControlToValidate="txt_SignS_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}"></asp:RegularExpressionValidator>
                        <br />
                        <asp:TextBox ID="txt_SignS_F" runat="server"></asp:TextBox>
                    </td>
                    <th><i class="fa fa-star"></i>報名期間(迄)</th>
                    <td>
                        <asp:Label ID="Label13" runat="server" Text="格式範例:2017-12-31" Font-Size="Smaller"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="結束日期不得為空" ControlToValidate="txt_SignE_F" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator17" runat="server" ControlToValidate="txt_SignE_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}"></asp:RegularExpressionValidator>


                        <br />
                        <asp:TextBox ID="txt_SignE_F" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>主辦單位</th>
                    <td>
                        <asp:TextBox ID="txt_Host" runat="server" placeholder="最多50字元"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ControlToValidate="txt_Host" ErrorMessage="主辦單位不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    <th><i class="fa fa-star"></i>聯絡人</th>
                    <td>
                        <asp:TextBox ID="txt_CPerosn" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_CPerosn" ErrorMessage="聯絡人不得為空" ForeColor="Red"></asp:RequiredFieldValidator>

                    </td>

                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>聯絡電話</th>
                    <td>
                        <asp:TextBox ID="txt_contact_C" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_contact_C" ErrorMessage="連絡電話不得為空" ForeColor="Red"></asp:RequiredFieldValidator>

                    </td>
                    <th><i class="fa fa-star"></i>聯絡Email</th>
                    <td>
                        <asp:TextBox ID="txt_contact_mail" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_contact_mail" ErrorMessage="聯絡Email不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txt_contact_mail" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>聯絡地址</th>
                    <td>
                        <asp:DropDownList ID="ddl_codeAreaA_Address" runat="server" CssClass="mydropdownlist" OnSelectedIndexChanged="ddl_codeAreaA_Address_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                        </asp:DropDownList>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_codeAreaB_Address" runat="server" CssClass="mydropdownlist" DataValueField="AREA_CODE" DataTextField="AREA_NAME" AutoPostBack="true">
                                    <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_codeAreaA_Address" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:TextBox ID="txt_CAddress" runat="server" CssClass="w5"></asp:TextBox>
                        <div style="display: -webkit-inline-box">
                            <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="ddl_codeAreaA_Address" InitialValue="" ErrorMessage="縣市不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddl_codeAreaB_Address" InitialValue="" ErrorMessage="市區不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txt_CAddress" ErrorMessage="聯絡地址不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                    <th>開放前台查詢</th>
                    <td>
                        <asp:CheckBox ID="Chk_Enable" runat="server" Text="是" />
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>上課日期起</th>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="格式範例:2017-01-01" Font-Size="Smaller"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_CStartDay_F" ErrorMessage="上課日期不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt_CStartDay_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}"></asp:RegularExpressionValidator>
                        <br />
                        <asp:TextBox ID="txt_CStartDay_F" runat="server"></asp:TextBox>
                    </td>
                    <th><i class="fa fa-star"></i>上課日期迄</th>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="格式範例:2017-01-01" Font-Size="Smaller"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="結束日期不得為空" ControlToValidate="txt_CEndDay_F" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txt_CEndDay_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}"></asp:RegularExpressionValidator>
                        <br />
                        <asp:TextBox ID="txt_CEndDay_F" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>上課時間起</th>
                    <td>
                        <asp:Label ID="Label10" runat="server" Text="格式範例:01:01" Font-Size="Smaller"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txt_CStratTime_F" ErrorMessage="開始日期不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server" ControlToValidate="txt_CStratTime_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$"></asp:RegularExpressionValidator>
                        <br />
                        <asp:TextBox ID="txt_CStratTime_F" runat="server"></asp:TextBox>
                    </td>
                    <th><i class="fa fa-star"></i>上課時間迄</th>
                    <td>
                        <asp:Label ID="Label11" runat="server" Text="格式範例:23:59" Font-Size="Smaller"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="結束日期不得為空" ControlToValidate="txt_CEndTime_F" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server" ControlToValidate="txt_CEndTime_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$"></asp:RegularExpressionValidator>
                        <br />
                        <asp:TextBox ID="txt_CEndTime_F" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>上課時數</th>
                    <td>
                        <asp:TextBox ID="txt_CHour_F" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txt_CHour_F" ErrorMessage="上課時數不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator18" runat="server" ControlToValidate="txt_CHour_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>
                    </td>
                    <th><i class="fa fa-star"></i>上課節數</th>
                    <td>
                        <asp:TextBox ID="txt_CCount_F" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="上課節數不得為空" ControlToValidate="txt_CCount_F" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator19" runat="server" ControlToValidate="txt_CCount_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>課程場次</th>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_AreaCodeA_F" runat="server" CssClass="mydropdownlist" OnSelectedIndexChanged="ddl_AreaCodeA_F_SelectedIndexChanged" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                        </asp:DropDownList>
                        <asp:TextBox ID="txt_ActiveArea_F" runat="server" CssClass="w5"></asp:TextBox>
                        <div style="display: -webkit-inline-box">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddl_AreaCodeA_F" InitialValue="" ErrorMessage="縣市不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                          
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txt_ActiveArea_F" ErrorMessage="聯絡地址不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>          
            </table>


    <div class="center btns">
        <asp:Button ID="btnPreview" runat="server" Visible="true" Text="預覽" OnClick="btnPreview_Click" />
        <asp:Button ID="btnOK" runat="server" Text="修改" OnClick="btnOK_Click" />
        <input name="btnCancel" type="button" value="取消" onclick="Back();" />
    </div>
     <div id="c_content" class="ContentDiv">

        <div style="background-color: white; opacity: 0.95; padding: 20px 30px; border-radius: 5px; margin: 100px auto 0 auto; z-index: 999; width: 700px; height: auto;">

            <a onclick="hideMSG()" style="float: right; font-size: 26px; color: gray;"><i class="fa fa-times-circle" aria-hidden="true"></i></a>

            <h1><i class="fa fa-at" aria-hidden="true"></i>課程報名-預覽</h1>
            <div class="tab-content" id="myTableData">
                <table class="table table-striped ">
                    <tr>
                        <th  class="diloagtable">認證分類</th>
                        <th  class="diloagtable">課程分類</th>
                        <th  class="diloagtable">課程名稱</th>
                        <th  class="diloagtable">報名時間</th>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lb_Class1" class="control-label" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lb_Class2" class="control-label" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lb_EventName" class="control-label" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lb_Time" class="control-label" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div class="col-12">
                    <table class="table table-striped" style="table-layout:fixed;word-break:break-all;overflow:auto;">
                        <tr>
                            <th colspan="4" style="text-align: center" class="diloagtable">適用人員</th>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lb_RoleBindName" runat="server" ></asp:Label></td>
                        </tr>
                        <tr>
                            <th colspan="4" style="text-align: center ; max-width:100%"  class="diloagtable">說明</th>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lb_Note" runat="server" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th  class="diloagtable">錄取人數</th>
                            <td>
                                <asp:Label ID="lb_CountAudit" runat="server" class="control-label"></asp:Label></td>
                            <th  class="diloagtable">證明類別</th>
                            <td>
                                <asp:Label ID="lb_QTypeName" runat="server" class="control-label"></asp:Label></td>
                        </tr>
                        <tr>
                            <th  class="diloagtable">主辦單位</th>
                            <td>
                                <asp:Label ID="lb_Host" runat="server" class="control-label"></asp:Label></td>
                            <th  class="diloagtable">課程費用</th>
                            <td>
                                <asp:Label ID="lb_ActiveCost" runat="server" class="control-label"></asp:Label></td>
                        </tr>
                        <tr>
                            <th  class="diloagtable">聯絡人</th>
                            <td>
                                <asp:Label ID="lb_CPerson" runat="server" class="control-label"></asp:Label></td>
                            <th  class="diloagtable">聯絡電話</th>
                            <td>
                                <asp:Label ID="lb_CPhone" runat="server" class="control-label"></asp:Label></td>
                        </tr>
                        <tr>
                            <th  class="diloagtable">聯絡Email</th>
                            <td>
                                <asp:Label ID="lb_Cmail" runat="server" class="control-label"></asp:Label></td>
                            <th  class="diloagtable">聯絡地址</th>
                            <td>
                                <asp:Label ID="lb_Address" runat="server" class="control-label"></asp:Label></td>
                        </tr>
                    </table>
                    <table class="table table-striped">
                        <tr>
                            <th colspan="4" style="text-align: center"  class="diloagtable">課程資訊</th>
                        </tr>
                        <tr>
                            <th class="diloagtable">上課日期起、迄:</th>
                            <td>
                                <asp:Label ID="lb_CEdate" runat="server" class="control-label"></asp:Label></td>
                            <th class="diloagtable">上課時間起、迄:</th>
                            <td>
                                <asp:Label ID="lb_CEtime" runat="server" class="control-label" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <th class="diloagtable">上課時數:</th>
                            <td>
                                <asp:Label ID="lb_Hour" runat="server" class="control-label"></asp:Label></td>
                            <th class="diloagtable">上課節數:</th>
                            <td>
                                <asp:Label ID="lb_Class" runat="server" class="control-label"></asp:Label></td>
                        </tr>
                        <tr>
                            <th class="diloagtable">課程場次:</th>
                            <td colspan="8">
                                <asp:Label ID="lb_EventNote" runat="server" class="control-label"></asp:Label></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
        <script type="text/javascript">


            //CKEDITOR.replace("ctl00$ContentPlaceHolder1$editor1", { filebrowserBrowseUrl: 'FileBrower.aspx?type=File' });
            //CKEDITOR.replace("ctl00$ContentPlaceHolder1$editor_Mail", { filebrowserBrowseUrl: 'FileBrower.aspx?type=File' });

            function Back() {
                history.back();
                //window.location.href = './Event.aspx';
            }
            function showMSG() {
                var Role = "";
                var R1 = $("input:checkbox[name*='ctl00$ContentPlaceHolder1$cb_Role$0']:checked").val();
                var R2 = $("input:checkbox[name*='ctl00$ContentPlaceHolder1$cb_Role$1']:checked").val();
                var R3 = $("input:checkbox[name*='ctl00$ContentPlaceHolder1$cb_Role$2']:checked").val();
                var R4 = $("input:checkbox[name*='ctl00$ContentPlaceHolder1$cb_Role$3']:checked").val();
                if (R1 == "10") {
                    Role += "西醫師,";
                }
                if (R2 == "11") {
                    Role += "牙醫師,";
                }
                if (R3 == "12") {
                    Role += "藥師,";
                }
                if (R4 == "13") {
                    Role += "其他醫事人員及公衛師,";
                }
                if (Role != "") {
                    Role = Role.substring(0, Role.length - 1);
                }
                
                var editor = $('.ck-editor__main>div').html();
                //$.ajax({
                //    type: "GET",
                //    url: "Event_AE.aspx/GetData?user=" + editor,
                //    data: { note: editor},
                //    contentType: "application/json; charset=utf-8",
                //    dataType: "json",
                //    success: function (data) { alert(data.d); }
                //});
                /*sendEditor(editor);*/
                editor = editor.replace(/<svg xmlns="http:\/\/www.w3.org\/2000\/svg" viewBox="0 0 10 8"><path d="M9.055.263v3.972h-6.77M1 4.216l2-2.038m-2 2 2 2.038"><\/path><\/svg>/g, '');
                editor = editor.replace(/<path fill-opacity=".254" d="M1 1h14v14H1z"><\/path>/g, '');
                editor = editor.replace(/<path fill-opacity=".256" d="M1 1h14v14H1z"><\/path>/g, '');
                editor = editor.replace(/<svg class="ck ck-icon" viewBox="0 0 16 16"><path d="M4 0v1H1v3H0V.5A.5.5 0 0 1 .5 0H4zm8 0h3.5a.5.5 0 0 1 .5.5V4h-1V1h-3V0zM4 16H.5a.5.5 0 0 1-.5-.5V12h1v3h3v1zm8 0v-1h3v-3h1v3.5a.5.5 0 0 1-.5.5H12z"><\/path><g class="ck-icon__selected-indicator"><path d="M7 0h2v1H7V0zM0 7h1v2H0V7zm15 0h1v2h-1V7zm-8 8h2v1H7v-1z"><\/path><\/g><\/svg>/g, '');
                //editor = editor.replace(/<div class="ck ck-reset_all ck-widget__resizer" style="height:350px;left:0px;top:0px;width:400px;"><div class="ck-widget__resizer__handle ck-widget__resizer__handle-top-left"><\/div ><div class="ck-widget__resizer__handle ck-widget__resizer__handle-top-right"><\/div><div class="ck-widget__resizer__handle ck-widget__resizer__handle-bottom-right"><\/div><div class="ck-widget__resizer__handle ck-widget__resizer__handle-bottom-left"><\/div><div class="ck ck-size-view" style="display: none;"><\/div><\/div >/g, '');
                //editor = editor.replace(/<div class="ck ck-reset_all ck-widget__resizer" style="height:350px;left:0px;top:0px;width:400px;"><div class="ck-widget__resizer__handle ck-widget__resizer__handle-top-left"><\/div ><div class="ck-widget__resizer__handle ck-widget__resizer__handle-top-right"><\/div><div class="ck-widget__resizer__handle ck-widget__resizer__handle-bottom-right"><\/div><div class="ck-widget__resizer__handle ck-widget__resizer__handle-bottom-left"><\/div><div class="ck ck-size-view" style="display: none;"><\/div><\/div >/g, '');
                //editor = editor.replace(/<div class="ck ck-reset_all ck-widget__resizer" style="height:477px;left:0px;top:0px;width:545px;"><div class="ck-widget__resizer__handle ck-widget__resizer__handle-top-left"><\/div ><div class="ck-widget__resizer__handle ck-widget__resizer__handle-top-right"><\/div><div class="ck-widget__resizer__handle ck-widget__resizer__handle-bottom-right"><\/div><div class="ck-widget__resizer__handle ck-widget__resizer__handle-bottom-left"><\/div><div class="ck ck-size-view" style="display: none;"><\/div><\/div >/g, '');
                //editor = editor.replace('<div class="ck ck-reset_all ck-widget__resizer" style="height:350px;left:0px;top:0px;width:400px;"><div class="ck-widget__resizer__handle ck-widget__resizer__handle-top-left"></div><div class="ck-widget__resizer__handle ck-widget__resizer__handle-top-right"></div><div class="ck-widget__resizer__handle ck-widget__resizer__handle-bottom-right"></div><div class="ck-widget__resizer__handle ck-widget__resizer__handle-bottom-left"></div><div class="ck ck-size-view" style="display: none;"></div></div>', '');
                //editor = editor.replace('<div class="ck ck-reset_all ck-widget__resizer" style="height:500px;left:0px;top:0px;width:400px;"><div class="ck-widget__resizer__handle ck-widget__resizer__handle-top-left"></div><div class="ck-widget__resizer__handle ck-widget__resizer__handle-top-right"></div><div class="ck-widget__resizer__handle ck-widget__resizer__handle-bottom-right"></div><div class="ck-widget__resizer__handle ck-widget__resizer__handle-bottom-left"></div><div class="ck ck-size-view" style="display: none;"></div></div>','');
                var EventClass = $("#ContentPlaceHolder1_ddl_Class1").find("option:selected").text();
                var ClassType= $("#ContentPlaceHolder1_ddl_Class2").find("option:selected").text();
                var CDate = $("#ContentPlaceHolder1_txt_SignS_F").val() + "~" + $("#ContentPlaceHolder1_txt_SignE_F").val();
                var CSign = $("#ContentPlaceHolder1_txt_CStartDay_F").val() + "~" + $("#ContentPlaceHolder1_txt_CEndDay_F").val();
                var CTime = $("#ContentPlaceHolder1_txt_CStratTime_F").val()+"~"+$("#ContentPlaceHolder1_txt_CEndTime_F").val();
                var Title = $('#ContentPlaceHolder1_txt_Title').val();
                var CountAudit = $('#ContentPlaceHolder1_txt_CountAdmit').val();
                var QTypeName = $('#ContentPlaceHolder1_txt_QTypeName').val();
                var Host = $("#ContentPlaceHolder1_txt_Host").val();
                var ActiveCost = $("#ContentPlaceHolder1_txt_ActiveCost").val();
                var CPerosn = $("#ContentPlaceHolder1_txt_CPerosn").val();
                var contact_C = $("#ContentPlaceHolder1_txt_contact_C").val();
                var contact_mail = $("#ContentPlaceHolder1_txt_contact_mail").val();
                var CAddress = $("#ContentPlaceHolder1_ddl_codeAreaA_Address").find("option:selected").text() + $("#ContentPlaceHolder1_ddl_codeAreaB_Address").find("option:selected").text()+$("#ContentPlaceHolder1_txt_CAddress").val();
                var CHour = $("#ContentPlaceHolder1_txt_CHour_F").val();
                var CCount = $("#ContentPlaceHolder1_txt_CCount_F").val();
                var Area = $("#ContentPlaceHolder1_ddl_AreaCodeA_F").find("option:selected").text() + $("#ContentPlaceHolder1_txt_ActiveArea_F").val();
                $("#ContentPlaceHolder1_lb_Class1").text(EventClass);
                $("#ContentPlaceHolder1_lb_Class2").text(ClassType);
                $("#ContentPlaceHolder1_lb_CEdate").text(CSign);
                $("#ContentPlaceHolder1_lb_CEtime").text(CTime);
                $("#ContentPlaceHolder1_lb_Time").text(CDate);
                $("#ContentPlaceHolder1_lb_EventName").text(Title);
                $("#ContentPlaceHolder1_lb_CountAudit").text(CountAudit);
                $("#ContentPlaceHolder1_lb_QTypeName").text(QTypeName);
                $("#ContentPlaceHolder1_lb_Host").text(Host);
                $("#ContentPlaceHolder1_lb_ActiveCost").text(ActiveCost);
                $("#ContentPlaceHolder1_lb_CPerson").text(CPerosn);
                $("#ContentPlaceHolder1_lb_CPhone").text(contact_C);
                $("#ContentPlaceHolder1_lb_Cmail").text(contact_mail);
                $("#ContentPlaceHolder1_lb_Address").text(CAddress);
                $("#ContentPlaceHolder1_lb_Hour").text(CHour);
                $("#ContentPlaceHolder1_lb_Class").text(CCount);
                $("#ContentPlaceHolder1_lb_EventNote").text(Area);
                $("#ContentPlaceHolder1_lb_RoleBindName").text(Role);
                document.getElementById('ContentPlaceHolder1_lb_Note').innerHTML = editor;
                /*document.getElementById('ContentPlaceHolder1_lb_Note').remove(".ck-reset_all");*/
                $("#ContentPlaceHolder1_lb_Note").remove(".ck-reset_all");
                //$(".ck-reset_all").remove();
                $("#c_content").show("fade");
            }
            function hideMSG() {
                $("#c_content").hide("fade");
            }
            function showNote() {
                $("#Note_content").show("fade");

            }
            function getUrlVars() {
                var vars = [], hash;
                var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
                for (var i = 0; i < hashes.length; i++) {
                    hash = hashes[i].split('=');
                    vars.push(hash[0]);
                    vars[hash[0]] = hash[1];
                }
                return vars;
            }
            function hideNote() {
                $("#Note_content").hide("fade");
            }
        </script>
      <script type="text/jscript" src="https://cdn.ckeditor.com/ckeditor5/34.1.0/super-build/ckeditor.js"></script>
         <script type="text/javascript">
             // This sample still does not showcase all CKEditor 5 features (!)
             // Visit https://ckeditor.com/docs/ckeditor5/latest/features/index.html to browse all the features.
             CKEDITOR.ClassicEditor.create(document.getElementById("ContentPlaceHolder1_editor1"), {
                 // https://ckeditor.com/docs/ckeditor5/latest/features/toolbar/toolbar.html#extended-toolbar-configuration-format
                 toolbar: {
                     items: ['ckfinder', '|',
                         'exportPDF', 'exportWord', '|',
                         'findAndReplace', 'selectAll', '|',
                         'heading', '|',
                         'bold', 'italic', 'strikethrough', 'underline', 'code', 'subscript', 'superscript', 'removeFormat', '|',
                         'bulletedList', 'numberedList', 'todoList', '|',
                         'outdent', 'indent', '|',
                         'undo', 'redo',
                         '-',
                         'fontSize', 'fontFamily', 'fontColor', 'fontBackgroundColor', 'highlight', '|',
                         'alignment', '|',
                         'link', 'insertImage', 'blockQuote', 'insertTable', 'mediaEmbed', 'codeBlock', 'htmlEmbed', '|',
                         'specialCharacters', 'horizontalLine', 'pageBreak', '|',
                         'textPartLanguage', '|',
                         'sourceEditing'
                     ],
                     shouldNotGroupWhenFull: true
                 },
                 // Changing the language of the interface requires loading the language file using the <script> tag.
                 language: 'tw',
                 list: {
                     properties: {
                         styles: true,
                         startIndex: true,
                         reversed: true
                     }
                 },
                 // https://ckeditor.com/docs/ckeditor5/latest/features/headings.html#configuration
                 heading: {
                     options: [
                         { model: 'paragraph', title: 'Paragraph', class: 'ck-heading_paragraph' },
                         { model: 'heading1', view: 'h1', title: 'Heading 1', class: 'ck-heading_heading1' },
                         { model: 'heading2', view: 'h2', title: 'Heading 2', class: 'ck-heading_heading2' },
                         { model: 'heading3', view: 'h3', title: 'Heading 3', class: 'ck-heading_heading3' },
                         { model: 'heading4', view: 'h4', title: 'Heading 4', class: 'ck-heading_heading4' },
                         { model: 'heading5', view: 'h5', title: 'Heading 5', class: 'ck-heading_heading5' },
                         { model: 'heading6', view: 'h6', title: 'Heading 6', class: 'ck-heading_heading6' }
                     ]
                 },
                 // https://ckeditor.com/docs/ckeditor5/latest/features/editor-placeholder.html#using-the-editor-configuration
                 placeholder: 'Welcome to CKEditor 5!',
                 // https://ckeditor.com/docs/ckeditor5/latest/features/font.html#configuring-the-font-family-feature
                 fontFamily: {
                     options: [
                         'default',
                         'Arial, Helvetica, sans-serif',
                         'Courier New, Courier, monospace',
                         'Georgia, serif',
                         'Lucida Sans Unicode, Lucida Grande, sans-serif',
                         'Tahoma, Geneva, sans-serif',
                         'Times New Roman, Times, serif',
                         'Trebuchet MS, Helvetica, sans-serif',
                         'Verdana, Geneva, sans-serif'
                     ],
                     supportAllValues: true
                 },
                 // https://ckeditor.com/docs/ckeditor5/latest/features/font.html#configuring-the-font-size-feature
                 fontSize: {
                     options: [10, 12, 14, 'default', 18, 20, 22],
                     supportAllValues: true
                 },
                 // Be careful with the setting below. It instructs CKEditor to accept ALL HTML markup.
                 // https://ckeditor.com/docs/ckeditor5/latest/features/general-html-support.html#enabling-all-html-features
                 htmlSupport: {
                     allow: [
                         {
                             name: /.*/,
                             attributes: true,
                             classes: true,
                             styles: true
                         }
                     ]
                 },
                 // Be careful with enabling previews
                 // https://ckeditor.com/docs/ckeditor5/latest/features/html-embed.html#content-previews
                 htmlEmbed: {
                     showPreviews: true
                 },
                 // https://ckeditor.com/docs/ckeditor5/latest/features/link.html#custom-link-attributes-decorators
                 link: {
                     decorators: {
                         addTargetToExternalLinks: true,
                         defaultProtocol: 'https://',
                         toggleDownloadable: {
                             mode: 'manual',
                             label: 'Downloadable',
                             attributes: {
                                 download: 'file'
                             }
                         }
                     }
                 },
                 // https://ckeditor.com/docs/ckeditor5/latest/features/mentions.html#configuration
                 mention: {
                     feeds: [
                         {
                             marker: '@',
                             feed: [
                                 '@apple', '@bears', '@brownie', '@cake', '@cake', '@candy', '@canes', '@chocolate', '@cookie', '@cotton', '@cream',
                                 '@cupcake', '@danish', '@donut', '@dragée', '@fruitcake', '@gingerbread', '@gummi', '@ice', '@jelly-o',
                                 '@liquorice', '@macaroon', '@marzipan', '@oat', '@pie', '@plum', '@pudding', '@sesame', '@snaps', '@soufflé',
                                 '@sugar', '@sweet', '@topping', '@wafer'
                             ],
                             minimumCharacters: 1
                         }
                     ]
                 },
                 // The "super-build" contains more premium features that require additional configuration, disable them below.
                 // Do not turn them on unless you read the documentation and know how to configure them and setup the editor.
                 removePlugins: [
                     // These two are commercial, but you can try them out without registering to a trial.
                     // 'ExportPdf',
                     // 'ExportWord',



                     // This sample uses the Base64UploadAdapter to handle image uploads as it requires no configuration.
                     // https://ckeditor.com/docs/ckeditor5/latest/features/images/image-upload/base64-upload-adapter.html
                     // Storing images as Base64 is usually a very bad idea.
                     // Replace it on production website with other solutions:
                     // https://ckeditor.com/docs/ckeditor5/latest/features/images/image-upload/image-upload.html
                     // 'Base64UploadAdapter',
                     'RealTimeCollaborativeComments',
                     'RealTimeCollaborativeTrackChanges',
                     'RealTimeCollaborativeRevisionHistory',
                     'PresenceList',
                     'Comments',
                     'TrackChanges',
                     'TrackChangesData',
                     'RevisionHistory',
                     'Pagination',
                     'WProofreader',
                     // Careful, with the Mathtype plugin CKEditor will not load when loading this sample
                     // from a local file system (file://) - load this site via HTTP server if you enable MathType
                     'MathType'
                 ]
             });
         </script>
</asp:Content>

