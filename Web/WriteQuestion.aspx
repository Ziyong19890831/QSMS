<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="WriteQuestion.aspx.cs" Inherits="Web_WriteQuestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">問卷填寫</li>
            <asp:HiddenField ID="hf_PersonSNO" runat="server" />
            <asp:HiddenField ID="hf_Query" runat="server" />            
        </ol>
    </nav>
    <div class="row">
        <div class="col-12">
            問卷名稱:<asp:Label ID="lb_PaperName" ForeColor="#0099cc" Font-Bold="true" runat="server"></asp:Label>
        </div>
        <div class="col-12">
            問卷說明:<asp:Label ID="lb_PaperDetail" runat="server"></asp:Label>
        </div>
    </div>
    <asp:Repeater ID="rpt_QA" OnItemDataBound="rpt_QA_ItemDataBound" runat="server">
        <ItemTemplate>
            <div style="background-color: #d8d8d8; padding: 5px; border-left: 10px solid #808080">
                <i style="color: white" class="fa fa-pencil-square"></i><a style="font-weight: bold; margin-left: 5px; font-size: 13pt"><%# Eval("QuestionName") %></a>
                <br />
                <asp:Label ID="Label5" Font-Size="10pt" runat="server" Text='<%# Eval("QuestionDetail") %>'></asp:Label>
                <asp:Label ID="lbl_QID" Visible="false" runat="server" Text='<%# Eval("QuestionID") %>'></asp:Label>
            </div>
            <div style="height: auto; margin-bottom: 5px; margin-top: 5px; padding-left: 30px;">
                <table id="mytable" style="border: unset">
                    <asp:Repeater ID="rpt_option" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td style="border: unset; width: 5%">
                                    <input class="bLarger" type="radio" name='<%# Eval("QuestionID") %>' value='<%# Eval("QuestionID") %>/<%# Eval("OptionID") %>' />
                                </td>
                                <td style="border: unset; width: auto">
                                    <%# Eval("OptionName") %>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <hr />

    <script>

        function saveQuestion() {

            
            var r = confirm("是否送出問卷?");
            if (r == true) {
                var totle = "";
                $('#mytable input[type="radio"]').each(function () {
                    if ($(this).prop('checked') == true) {
                        totle = totle + $(this).val() + ',';
                        console.log(totle);
                    }
                });

                totle = totle.substring(0, totle.length - 1);

                if (totle == "") {
                    alert("不得交白卷!");
                }
                else {
                    var PersonSno = $("#ContentPlaceHolder1_hf_PersonSNO").val();
                    var PaperID = $("#ContentPlaceHolder1_hf_Query").val();
                    var sJson = JSON.stringify({ totle: totle, PersonSno: PersonSno, PaperID: PaperID });


                    $.ajax({
                        type: "POST",
                        async: false,
                        dataType: "json",
                        url: "../Mgt/FormPaperAjax.aspx/UserAddAnswer",
                        contentType: 'application/json; charset=UTF-8',
                        data: sJson,
                        success: function (msg) {
                            alert("填寫完成!感謝您願意花寶貴時間幫我們填寫問卷!!")
                            location.href = "../Web/Notice.aspx";
                        },
                        beforeSend: function () {
                            $('#saveQ').attr('disabled', true);

                        },
                        complete: function () {
                            $('#saveQ').attr('disabled', false);
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            console.log(xhr.responseText);
                        }
                    });
                }
            } else {

            }




        }

    </script>

    <div style="text-align: center">
        <input id="saveQ"  onclick="saveQuestion()" class="btn btn-success" runat="server" value="送交" type="button" />
    </div>


</asp:Content>

