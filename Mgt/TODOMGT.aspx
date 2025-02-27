<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="TODOMGT.aspx.cs" Inherits="Mgt_TODOMGT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link id="themecss" rel="stylesheet" type="text/css" href="//www.shieldui.com/shared/components/latest/css/light/all.min.css" />
    <script type="text/javascript" src="//www.shieldui.com/shared/components/latest/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="//www.shieldui.com/shared/components/latest/js/shieldui-all.min.js"></script>
    <script type="text/javascript">
        jQuery(function ($) {
            $("#editor").shieldEditor({
                commands: [
                    "formatBlock", "removeFormat",
                    "fontName", "fontSize", "foreColor", "backColor",
                    "bold", "italic", "underline", "strikeThrough", "subscript", "superscript",
                    "justifyLeft", "justifyCenter", "justifyRight", "justifyFull",
                    "insertUnorderedList", "insertOrderedList",
                    "indent", "outdent",
                    "createLink", "unlink", "insertImage",
                    "viewHtml"
                ]
            });
        });

        function sendTODO() {
            var person = $("#ContentPlaceHolder1_Label1").text();
            var content = $("#editor").swidget().value();
            var system = $("#ContentPlaceHolder1_ddl_SystemName").val();
            var name = $("#todoName").val();
            var saJson = JSON.stringify({ person: person, content: content, system: system, name: name });
            alert(saJson);
            
            if (name == "" || content == "" || system == "") {
                alert("資料輸入不完全!")
            }
            else {
                
                $.ajax({
                    type: "POST",
                    async: false,
                    dataType: "json",
                    url: "FormPaperAjax.aspx/insertTODO",
                    contentType: 'application/json; charset=UTF-8',
                    data: saJson,
                    success: function (msg) {
                        alert("發送成功");
                        location.reload();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr.responseText);
                    }
                });
            }


        }
    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

        <div class="path txtS mb20">現在位置：<a href="TODOMGT.aspx">系統訊息</a></div>
        <div class="both mb20">
            <div class="left mb20">
                選擇發送人
                        <asp:DropDownList ID="ddl_SystemName" runat="server" DataTextField="RoleName" DataValueField="RoleGroup"></asp:DropDownList>
                <asp:Label ID="Label1" Style="visibility: hidden" runat="server" Text=""></asp:Label>
            </div>
            <input name="btnSearch" onclick="sendTODO()" type="button" value="發送"/>
        </div>
        <input id="todoName" type="text" style="width: 300px;" placeholder="請輸入待辦事項名稱" />
        <br />
        <textarea id="editor"></textarea>


</asp:Content>



