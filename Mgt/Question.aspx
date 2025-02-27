<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Question.aspx.cs" Inherits="Mgt_Question" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/resources/demos/style.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type='text/javascript' src='https://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js'></script>
    <script type='text/javascript' src='https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.18/jquery-ui.min.js'></script>
    <script type="text/javascript">


        $(function () {



            $(".sort").sortable({
                opacity: 0.6,    //拖曳時透明
                cursor: 'pointer',  //游標設定
                axis: 'y',       //只能垂直拖曳
                update: function () {
                    console.log($("input#test-log").val($('.sort').sortable('serialize')))
                    $("input#test-log").val($('.sort').sortable('serialize'));

                }
            });

            $("#dialog-INSERT").dialog({
                autoOpen: false,
                resizable: false,
                height: "auto",
                position: ['center', 50],
                width: 400,
                modal: true,
                buttons: {
                    "確定": function () {
                        var formname = $("#<%= TextBox1.ClientID%>").val();
                        var content = $("#<%= TextBox3.ClientID%>").val();
                        var paperID = $("#<%= Label5.ClientID%>").text();
                        var skin = $("#<%= DropDownList2.ClientID%>").val();
                        var qcount = $("#<%= TextBox6.ClientID%>").val();


                        var sJson = JSON.stringify({ formname: formname, content: content, paperID: paperID, skin: skin, qcount: qcount });


                        if (formname == "") {
                            alert("請輸入名稱");
                        }
                        else {

                            $.ajax({
                                type: "POST",
                                async: false,
                                dataType: "json",
                                url: "FormPaperAjax.aspx/insertQuestion",
                                contentType: 'application/json; charset=UTF-8',
                                data: sJson,
                                success: function (msg) {
                                    location.reload();
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                    console.log(xhr.responseText);
                                }
                            });

                        }
                    },
                    "取消": function () {
                        $(this).dialog("close");
                    }
                }
            });

            $("#opener").on("click", function () {
                $("#dialog-INSERT").dialog("open");
            });

            $("#dialog-Edit").dialog({
                autoOpen: false,
                resizable: false,
                height: "auto",
                width: 400,
                position: ['center', 50],
                modal: true,
                buttons: {
                    "確定": function () {
                        var formname = $("#<%= TextBox2.ClientID%>").val();
                        var personid = $("#<%= Label1.ClientID%>").val();
                        var content = $("#<%= TextBox4.ClientID%>").val();
                        var paperID = $("#<%= Label5.ClientID%>").text();
                        var skin = $("#<%= DropDownList1.ClientID%>").val();
                        var qcount = $("#<%= TextBox5.ClientID%>").val();

                        var saJson = JSON.stringify({ formname: formname, personid: personid, content: content, paperID: paperID, skin: skin, qcount: qcount});

                        if (formname == "") {
                            alert("請輸入名稱");
                        }
                        else {

                            $.ajax({
                                type: "POST",
                                async: false,
                                dataType: "json",
                                url: "FormPaperAjax.aspx/updateQuestion",
                                contentType: 'application/json; charset=UTF-8',
                                data: saJson,
                                success: function (msg) {
                                    location.reload();
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                    console.log(xhr.responseText);
                                }
                            });

                        }
                    },
                    "取消": function () {
                        $(this).dialog("close");

                    }
                }
            });


            $("#dialog-Option").dialog({
                autoOpen: false,
                resizable: false,
                height: "auto",
                position: ['center', 50],
                width: 600,
                modal: true

            });


            var isUse = $("#<%= Label6.ClientID%>").text();

            if (isUse == "1") {
                $("#savesortt").prop('disabled', true);
                $("#savesortt").css('background-color', "#ccc");
                $("#opener").prop('disabled', true);
                $("#opener").css('background-color', "#ccc");
                $(".ui-button").hide();
                $("#<%= TextBox2.ClientID%>").prop('disabled', true);
                $("#<%= TextBox4.ClientID%>").prop('disabled', true);


            }

           

            


            $('#ContentPlaceHolder1_DropDownList1').on('change', function () {
                
                if (this.value == 2) {
                    $("#qcount").css("visibility", "unset");
                }
                else {
                    $("#qcount").css("visibility", "hidden");
                }
            })

            $('#ContentPlaceHolder1_DropDownList2').on('change', function () {
                if (this.value == 2) {
                    $("#icount").css("visibility", "unset");
                }
                else {
                    $("#icount").css("visibility", "hidden");
                }
            })


        });



    </script>

   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   

        <div class="path txtS mb20">
            現在位置：<a href="#">系統服務管理</a>
            <i class="fa fa-angle-right" aria-hidden="true"></i>
            <a style="color: #96BC33" href="FormPaper.aspx">問券表單管理</a>
            <i class="fa fa-angle-right" aria-hidden="true"></i>
            <asp:Label ID="Label8" runat="server" Text="表單題目管理"></asp:Label>
        </div>
        <%--<wuc:uc1 runat="server" />--%>
        <div class="both mb20">
            <div class="left mb20">
            </div>
            <div class="right mt-10" style="margin-top: 5px; padding-right: 5px;">
                <input id="BackPaper" value="返回問卷表單" type="button" onclick="location.href = '../Mgt/FormPaper.aspx'" />
                <input id="opener" value="新增題目" type="button" />
            </div>
        </div>
        <div style="border: 1px solid #bcbcbc; padding: 15px;">
            表單:<asp:Label ID="Label2" Font-Size="25pt" runat="server" Text="Label"></asp:Label><br />
            <br />
            <a style="color: #bababa">問卷說明:</a>
            <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
        </div>

        <%--<textarea id="Label3" style="margin-bottom:-57px;margin-top:10PX" runat="server" readonly="readonly" rows="4" cols="50"></textarea> <br /><br /><br />--%>
        <hr />
        <asp:Label ID="Label4" runat="server" Text="#請點右上方【新增】按鈕新增題目"></asp:Label>
        <script>
            function opdial(name, id, dddd, eeee, qcount) {
                try {
                    $("#dialog-Edit").dialog("open");
                    $("#<%= TextBox2.ClientID%>").val(name);
                     $("#<%= TextBox4.ClientID%>").val(dddd);
                $("#<%= DropDownList1.ClientID%>").val(eeee);
                $("#<%= TextBox5.ClientID%>").val(qcount)

                $("#ContentPlaceHolder1_Label1").val(id);

                if ($("#<%= DropDownList1.ClientID%>").val() == 2) {
                    $("#qcount").css("visibility", "unset");
                }
                else {
                    $("#qcount").css("visibility", "hidden");
                }
                }
                catch (err) {
                  console.log(err.message);
                }
               

            }

            function savesort() {

                //確認共有幾題
                var rowCount = $('#question_table tr').length;
                var paperID = $("#<%= Label5.ClientID%>").text();
                var sortGroup = "";

                $('#question_table tr').each(function (i, el) {

                    //找QuestionID
                    var $tds = $(this).find('#qid'),
                        id = $tds.eq(0).text();
                    if (id != "") {
                        //依順序接入
                        sortGroup = sortGroup + id + ",";
                    }

                });
                sortGroup = sortGroup.substring(0, sortGroup.length - 1);


                var sJson = JSON.stringify({ sortGroup: sortGroup, paperID: paperID });
                $.ajax({
                    type: "POST",
                    async: false,
                    dataType: "json",
                    url: "FormPaperAjax.aspx/updatesort",
                    contentType: 'application/json; charset=UTF-8',
                    data: sJson,
                    success: function (msg) {
                        alert("儲存成功");
                    },
                  
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr.responseText);
                    }




                });

            }

            function opdialop(id) {
                $("#dialog-Option").dialog("open");
                var isUse = $("#<%= Label6.ClientID%>").text();
                $.ajax({
                    url: "Option.aspx",
                    type: 'POST',
                    data: { qid: id, isUse: isUse },
                    success: function (result) {
                        $("#myZone5").html(result);
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr.responseText);
                    }
                });

            }

        </script>

        <asp:Panel ID="Panel1" runat="server">
            <div style="float: left; color: #a3a3a3">
                可用滑鼠拉動表格上下調整排序  <i class="fa fa-sort" aria-hidden="true"></i>
                <input name="" id="savesortt" type="button" onclick="savesort();" value="儲存排序" />
            </div>
            <div>
                <table id="question_table" style="text-align: center">
                    <thead>
                        <tr>
                            <th style="visibility: collapse"></th>
                            <th>題目名稱</th>
                            <th>題目說明</th>
                            <th>題目類型</th>
                            <th>最少選擇(多選題用)</th>
                            <th>修改</th>
                            <th>選項</th>
                            <%-- <th>複製</th>--%>
                            <th>刪除</th>

                        </tr>
                    </thead>
                    <tbody class="sort">
                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td style="visibility: collapse">
                                        <a id="qid"><%# Eval("QuestionID").ToString()%></a>
                                    </td>
                                    <td>
                                        <%# Eval("QuestionName").ToString()%></td>
                                    <td><%# Eval("QuestionDetail").ToString()%></td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("skin").ToString()%>'></asp:Label></td>
                                     <td>
                                        <asp:Label ID="Label9" runat="server" Text='<%# Eval("Qcount").ToString()%>'></asp:Label></td>
                                    <td>
                                        <input type="button" onclick="opdial(this.name,<%# Eval("QuestionID").ToString()%>,'<%# Eval("QuestionDetail").ToString()%>',<%# Eval("skin").ToString()%>,<%# Eval("qcount").ToString()%>);" id="Editopener" name="<%# Eval("QuestionName").ToString()%>" value="修改" />
                                    </td>
                                    <td>
                                        <input style="background-color: #32612b;visibility:<%# Eval("skin").ToString()=="0" || Eval("skin").ToString()=="3" ? "hidden" : "unset"  %>" type="button" onclick="opdialop(<%# Eval("QuestionID").ToString()%>);" id="option_dilog" value="設定" />
                                    </td>
                                    <%-- <td>

                                    </td>--%>
                                    <td>
                                        <%
                                            if (userInfo.AdminIsDelete == true)
                                            {
                                        %>
                                        <asp:LinkButton ID='btnDEL' runat="server" OnClientClick="return confirm('是否刪除?');" OnClick='btnDEL_Click' CommandArgument='<%# Eval("QuestionID")%>'><i class="fa fa-trash" aria-hidden="true"></i></asp:LinkButton>
                                        <%   
                                            }
                                            else
                                            {
                                        %>
                             無權限
                        <% 
                            }
                        %>
                                    </td>

                                </tr>

                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>


        </asp:Panel>


        <div id="dialog-INSERT" title="新增題目">
            <p>題目名稱:<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></p>
            <p>題目類型:<asp:DropDownList ID="DropDownList2" runat="server">
                <asp:ListItem Value="0">問答題</asp:ListItem>
                <asp:ListItem Value="1">單選題</asp:ListItem>
                <asp:ListItem Value="2">多選題</asp:ListItem>
                <asp:ListItem Value="3">簡單輸入題</asp:ListItem>
                </asp:DropDownList></p>
             <p id="icount" style="visibility:hidden">
                最少選幾題:<asp:TextBox onkeyup="value=value.replace(/[^\d]/g,'') " ID="TextBox6" runat="server"></asp:TextBox>
            </p>
            <p>題目說明:<asp:TextBox ID="TextBox3" Width="300px" Height="120px" TextMode="MultiLine" runat="server"></asp:TextBox></p>

        </div>

        <div id="dialog-Edit" title="修改問卷表單">
            <p>
                題目名稱:<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                <asp:Label ID="Label1" runat="server" Style="visibility: hidden" Text="Label"></asp:Label>
            </p>
            <p>題目類型:<asp:DropDownList ID="DropDownList1" runat="server">
                <asp:ListItem Value="0">問答題</asp:ListItem>
                <asp:ListItem Value="1">單選題</asp:ListItem>
                <asp:ListItem Value="2">多選題</asp:ListItem>
                <asp:ListItem Value="3">簡單輸入題</asp:ListItem>
                </asp:DropDownList></p>
            <p id="qcount" style="visibility:hidden">
                最少選幾題:<asp:TextBox onkeyup="value=value.replace(/[^\d]/g,'') " ID="TextBox5" runat="server"></asp:TextBox>
            </p>
            <p>題目說明:<asp:TextBox ID="TextBox4" Width="300px" Height="120px" TextMode="MultiLine" runat="server"></asp:TextBox></p>

        </div>


        <div id="dialog-Option" title="選項管理">

            <div id="myZone5"></div>
 
        </div>


        <asp:Label ID="Label6" runat="server" Style="visibility: hidden" Text="Label"></asp:Label>
        <asp:Label ID="Label5" runat="server" Style="visibility: hidden" Text="Label"></asp:Label>

        
        
        
        <asp:HiddenField ID="txt_SearchRole" runat="server" />
   
</asp:Content>

