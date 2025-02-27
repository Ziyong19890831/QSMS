<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Web.master" AutoEventWireup="true" CodeFile="UToDo.aspx.cs" Inherits="Web_UToDo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../JS/jquery.js"></script>
    <script type="text/javascript">
        var $jq = jQuery.noConflict(true); //為jquery.min.js有衝突多寫這一行出來,把$符號全換成$jq
    </script>

    <script type="text/javascript">

        $jq(function () {

            $("#sysTable input[type=checkbox]").change(function () { //選到的申請系統跳出該系統申請帳號頁面
                if (this.checked) {

                    var Name = "<%= Name %>";
                    var PersonID = "<%= PersonID %>";
                    var Email = "<%= Email %>";

                    var inputAry = [];
                    inputAry.push("<input type='hidden' name='Name' value='" + Name + "' />");
                    inputAry.push("<input type='hidden' name='PersonID' value='" + PersonID + "' />");
                    inputAry.push("<input type='hidden' name='Email' value='" + Email + "' />");

                    var formAry = [];
                    formAry.push("<form method='post' target='winname'  action='" + this.value + "' >");
                    formAry.push(inputAry.join(""));
                    formAry.push("</form>");


                    var formTag = formAry.join("");
                    window.open("", "winname", "width=1000 height=800 location,menubar=yes status,toolbar=no");
                    $(formTag).appendTo('body').submit().remove();




                }
            });
        });

        function _goPage(pageNumber) {
            //location.href = "?page=" + pageNumber + "#mainContent";
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }

    </script>


    <style type="text/css">
        input.bLarger {
            width: 19px;
            height: 19px;
        }

        .tooltip {
            position: relative;
            display: inline-block;
            font-size: 15pt;
        }

            .tooltip .tooltiptext {
                visibility: hidden;
                width: 350px;
                font-size: 13pt;
                background-color: black;
                color: #fff;
                text-align: center;
                border-radius: 6px;
                padding: 5px 0;
                /* Position the tooltip */
                position: absolute;
                z-index: 1;
            }

            .tooltip:hover .tooltiptext {
                visibility: visible;
            }

        .AP_system {
            width: 18.5%;
        }

        @media screen and (max-width: 700px) {
            .AP_system {
                width: 46.5%;
            }
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="path mb20">目前位置：<a href="UToDo.aspx">申請系統狀態</a></div>
    <h1><i class="fa fa-calendar-check-o" aria-hidden="true"></i>申請系統狀態
           
    </h1>

    <table>
        <tr>
            <th class="w2 center">申請日期</th>
            <th>申請系統名稱</th>
            <th>申請狀態</th>
        </tr>
        <asp:Repeater ID="rpt_QA" runat="server">
            <ItemTemplate>
                <tr>
                    <%--<td><%# Eval("SYSTEM_NAME") %></td>--%>
                    <td class="date" style="color: black"><%#Convert.ToDateTime(Eval("CreateDT")).ToString("yyyy-MM-dd") %></td>
                    <td><%# Eval("SYSTEM_NAME") %></td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("SysPAccountIsUser") %>'></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>

    </table>

    <asp:Panel ID="Panel1" Visible="true" runat="server">

        <div style="padding: 5px 0px 5px 20px" class="step">
            <h3><i class="fa fa-pencil" aria-hidden="true"></i>申請新系統</h3>
        </div>
        <div style="border-top: 2px solid #96BC33; margin-top: -8px; margin-bottom: 9px;">
        </div>
        <div style="border: 1px solid #bbbbbb; border-left: 5px solid #808080; padding: 5px; color: white; background-color: #bbbbbb; font-size: 13pt"><b>申請步驟</b></div>
        <div style="border: 1px solid #bbbbbb; padding: 5px;">
            1.
                <div class="tooltip">
                    <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>
                    <span class="tooltiptext">
                        <a style="color: yellow">如果勾選，未跳出申請網站視窗?
                            <br />
                            請照下列步驟執行</a><br />
                        <br />
                        <h2>開啟彈出式視窗</h2>
                        1.在電腦上開啟 Chrome。<br />
                        2.依序按一下右上角的「更多」圖示 更多 接著 [設定]。<br />
                        3.按一下底部的 [進階]。<br />
                        4.在「隱私權和安全性」之下，按一下 [內容設定]。<br />
                        5.按一下 [彈出式視窗]。<br />
                        6.在畫面頂端將設定切換成 [允許] 。<br />
                        <hr />
                        或者瀏覽器右上角出現
                    <img src='https://lh3.googleusercontent.com/AMyVlZvDgRv0mgWYka34TM51DWuQClPk69GTbnlRzXHq46Ae7YfC8YojPxnECzOjrtI=h18' />
                        <br />
                        點下按鈕勾選一律顯示本網站的彈出式視窗，按下完成重新整理即可顯示
                    </span>
                </div>
            勾選您要申請系統，跳出視窗進到各系統申請相關系統身分。
                <br />
            2.申請後，按下送出。<br />
            3.等待系統管理員審核通過<br />
            4.即可登入相關入口。<br />

        </div>
        請選擇申請的系統:<br />
        <div id="sysTable" style="width: 100%; position: relative">
            <asp:Repeater ID="rptsystemm" runat="server">
                <ItemTemplate>
                    <div class="AP_system" style="text-align: center; min-height: 140px; border: 1px solid #bbbbbb; margin: 5px; padding-top: 15px; float: left; position: relative">


                        <asp:Image ID="image2" runat="server" Width="40px" Height="40px" ImageUrl=' <%# Eval("unIcon","../Images/Icon/{0}") %> ' />
                        <br />
                        <b>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("SYSTEM_NAME") %>'></asp:Label>
                        </b>
                        <br />

                        <div style="position: absolute; left: 4%; bottom: 5px; width: 92%; background-color: #e6e6e6">
                            <input class="bLarger" runat="server" type="checkbox" id="selectit" sid='<%# Eval("SYSTEM_ID") %>' name="selectit" value='<%# Eval("AP_ApplyURL") %>' />
                        </div>
                    </div>

                </ItemTemplate>
            </asp:Repeater>

        </div>
        <div style="width: 100%; float: left; text-align: center">

            <script type="text/javascript">
                function check() {
                    var txt;
                    var r = confirm("確認已完成申請勾選系統身分?!");
                    if (r == true) {
                    } else {

                        window.event.returnValue = false;
                    }
                }
            </script>

            <input id="btn_submit" name="" type="submit" style="width: 100px" value="送出" onclick="check();" runat="server" onserverclick="btn_submit_ServerClick" causesvalidation="false" />
        </div>

    </asp:Panel>

    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>


