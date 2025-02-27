<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Option.aspx.cs" Inherits="Mgt_Option" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <script type="text/javascript">

        function RandomNumber() { //取得編號
            var array1 = new Array("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z");
            var Str = "";
            for (var i = 1; i <= 10; i++) {
                index = Math.floor(Math.random() * array1.length);
                Str = Str + array1[index];
            }

            return Str;

        }

        if ($("#Label2").text() == "1") {
            $("#insertvalue").hide();
        }

        function AddRow() {
            var name = $("#txt_name").val();
            var id = RandomNumber();

            if (name.indexOf(',') == -1) {

            
            if (name == "") {
                alert("無填寫選項!")
            }
            else {
                $('#myTable').append('<tr id="' + id + '"><td  id="qid" style="width:94%">' + name + '</td style="width:5%"> <td><input style="background-color:black" onclick="deletemo(this.name)" name="' + id + '"  type="button" value="X" /></td></tr>')
                $("#txt_name").val("");
                }
            }
            else {
               alert("文字中不得含有','符號")
            }
            
        }
        function closemodel() {
            $("#dialog-Option").dialog("close");
        }

        function deletemo(abc) {
            $('#' + abc).remove();
        }

        function saveoption() {

            var optionGroup = ""; 
            
            $('#myTable tr').each(function (i, el) {
                    var $tds = $(this).find('#qid'),
                        id = $tds.eq(0).text();
                    if (id != "") {
                        optionGroup = optionGroup + id + ",";
                    }

                });
                optionGroup = optionGroup.substring(0, optionGroup.length - 1);
                

                var QuestionID = $("#Label2").text();

                var sJson = JSON.stringify({ optionGroup: optionGroup, QuestionID: QuestionID });
                $.ajax({
                    type: "POST",
                    async: false,
                    dataType: "json",
                    url: "FormPaperAjax.aspx/insertOption",
                    contentType: 'application/json; charset=UTF-8',
                    data: sJson,
                    success: function (msg) {
                        alert('儲存成功');
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr.responseText);
                    }
                });
           
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-bottom:10px">
            題目:<asp:Label Font-Bold="true" ID="Label1" runat="server" Text="Label"></asp:Label>
            <asp:Label ID="Label2" style="visibility:hidden" runat="server" Text="Label"></asp:Label>
            <asp:Label ID="Label3" style="visibility:hidden" runat="server" Text="Label"></asp:Label>
            <hr />
            <table id="myTable">
                <asp:Repeater ID="Repeater1" runat="server">
                    <ItemTemplate>
                        <tr id="<%# Eval("OptionID").ToString()%>">
                            <td id="qid" style="width:94%"> <%# Eval("OptionName").ToString()%></td>
                            <td style="width:5%"><input style="background-color:black" onclick="deletemo(this.name)" name="<%# Eval("OptionID").ToString()%>" type="button" value="X" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
           <input type="text" id="txt_name" style="width:80%" placeholder="請輸入選項名稱" /> <input type="button" style="width:16%;background-color:#5cb85c" value="新增" id="btnAdd" onclick="AddRow()" />
        </div>
        <hr style="color:#d4d4d4" />
        <div style="float:right;">
            <input style="background-color:#f6f6f6;color:#454545" type="button" onclick='saveoption();' id="insertvalue" value="存檔" />
             <input style="background-color:#f6f6f6;color:#454545" type="button" onclick='closemodel();' id="option_dilog" value="取消" />
        </div>
    </form>
</body>
</html>
