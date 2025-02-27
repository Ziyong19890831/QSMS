<%@ Page Language="C#" AutoEventWireup="true" CodeFile="APSAuditCheck.aspx.cs" Inherits="Mgt_APSAuditCheck" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title></title>

    <script>

        function closemodel() {
            $("#dialog-Option").dialog("close");
        }

        function changeStatus() {
            var state = $("#AuditStatus").val();
            if (state == "N" || state == "S")                
                $("#reason").css("display", "block");
            else $("#reason").css("display", "none");
        }

        function saveAudit() {

            var state1 = $("#Label2").text();
            var id = <%=Request.Form["qid"].ToString()%>;
            var addU = <%=Request.Form["user"].ToString()%>;
            var note = $("#reason").val();


            var state = $("#AuditStatus").val();

            var optionGroup = "";
            $('#CheckBoxList1 input').each(function (i, el) {
                if (this.checked) {
                    optionGroup += $(this).val() + ",";
                }

            });
            optionGroup = optionGroup.substring(0, optionGroup.length - 1);


            var sJson = JSON.stringify({ state: state, id: id, optionGroup: optionGroup, addU: addU , note: note });

            if (state1 != $("#AuditStatus").val() || optionGroup != "") {

                $.ajax({
                    type: "POST",
                    async: false,
                    dataType: "json",
                    url: "FormPaperAjax.aspx/UpdateAudit",
                    contentType: 'application/json; charset=UTF-8',
                    data: sJson,
                    success: function (msg) {
                        if (msg.d == "success") {
                            alert('儲存成功');
                        }
                        else if (msg.d == "failure") {
                            alert('儲存失敗');
                        } else {
                            //alert("msg:"+msg.d);
                        }
                        location.reload();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr.responseText);
                    }
                });
            }
            else {
                $("#dialog-Option").dialog("close");
            }


        }

    </script>


</head>

<body>

    <form id="form1" runat="server" style="font-family:'微軟正黑體';">

        <div style="margin-bottom: 10px; font-size: 12pt; line-height: 30px">



            <table>
                <tr>
                    <th style="width: 30%;">申請日期</th>
                    <td><asp:Label ID="lb_ApplyDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>申請人系統</th>
                    <td><asp:Label ID="lb_ApplySys" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>申請人單位</th>
                    <td><asp:Label ID="lb_ApplyOrgan" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>申請人名稱</th>
                    <td><asp:Label ID="lb_ApplyName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>申請人帳號</th>
                    <td><asp:Label ID="lb_ApplyAccount" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>申請人E-Mail</th>
                    <td><asp:Label ID="lb_ApplyMail" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>申請人電話</th>
                    <td><asp:Label ID="lb_ApplyTel" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>申請人手機:</th>
                    <td><asp:Label ID="lb_ApplyPhone" runat="server"></asp:Label></td>
                </tr>
            </table>


            <asp:Panel ID="Panel1" runat="server">
                <div>角色選單設定：</div>
                <div><asp:Label ID="Label2" runat="server"></asp:Label>
                    <asp:CheckBoxList ID="CheckBoxList1" RepeatColumns="4" runat="server" RepeatDirection="Horizontal"></asp:CheckBoxList>
                </div>
            </asp:Panel>

            
            <div>審核狀態：
                <select id="AuditStatus" onchange="changeStatus()" runat="server">
                    <option value="D">系統審核中(X)</option>
                    <option value="N">停權(X)</option>
                    <option value="Y">審核通過</option>
                    <option value="S">審核退回</option>
                </select>
            </div>

            <div id="reason" style="display:none;">原因：
                <asp:TextBox ID="txt_Note" runat="server" MaxLength="50"></asp:TextBox>
            </div>

            <asp:Label ID="Label1" runat="server" Font-Bold="true" Font-Size="14pt" ForeColor="Red" Text="#選擇'核退'按存檔，後續將不得修改審核狀態"></asp:Label>
            <asp:Label ID="lb_ApplyStatus" runat="server" Style="visibility: hidden" Text="Label"></asp:Label>
        </div>


        <hr style="color: #d4d4d4" />
        <div style="float: right;">
            <input style="background-color: #f6f6f6; color: #454545" type="button" onclick='saveAudit();' id="insertvalue" value="存檔" />
            <input style="background-color: #f6f6f6; color: #454545" type="button" onclick='closemodel();' id="option_dilog" value="取消" />
        </div>

    </form>

</body>
</html>
