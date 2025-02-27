<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/Dialog.master" CodeFile="FileBrower.aspx.cs" Inherits="Mgt_FileBrower" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <script type="text/javascript" src="../js/jquery.masonry.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();
            $('#tabs-1').imagesLoaded(function () {
                $('#tabs-1').masonry({        
                    itemSelector: '.content_box',
                    columnWidth: 0,
                    animate:true
                });
            });
        });
    </script>

    <style>

        body { font-family: 'Microsoft JhengHei', '微軟正黑體'; }

        .fbox {
            border:1px dashed #6fb92d;
            margin:10px;
            float:left;
        }
        .tlt{
            width:200px;
            background-color:#6fb92d;
            color:white;
            padding:5px;
            overflow-x:hidden;
            text-overflow : ellipsis;
        }
        .inbox div{
            width:200px;
            height:210px;
        }
        .inbox :hover {
            background-color:#fffbdf;
            text-decoration:none;
            cursor:pointer;
        }
        .fbox .footerbox {
            width:200px;
            background-color:#efefef;
            text-align:center;
            font-size:24px;
            padding:3px;
        }
        .fbox .imgblock{
            text-align:center;
            padding:10px;
        }
        .fbox img{
            width:180px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="path txtS mb20">現在位置：<i class="fa fa-angle-right"></i> 檔案管理</div>

    
    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <div class="left w8">
                檔案上傳：<asp:FileUpload ID="fileup_New" runat="server" />
            </div>
            <div class="right">
                <asp:Button ID="btnUpload" runat="server" Text="上傳" OnClick="btnUpload_Click" />
                <asp:Button ID="btndelete" runat="server" Text="刪除選取的" OnClick="btndelete_Click"  />
            </div>
        </fieldset>
    </div>


    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">圖檔</a></li>
            <li><a href="#tabs-2">一般檔案</a></li>
        </ul>

        <div id="tabs-1" style="height:300px; overflow-y:auto;">
            <%=MyImages %>
        </div>
        
        <div id="tabs-2">
            <table class="tbl">
                <%=MyFiles %>
            </table>
        </div>

    </div>

    
    <script>
        function select(fileUrl) {
            window.opener.CKEDITOR.tools.callFunction(1, fileUrl);
            window.close();
        }
        function check(obj) {
            if($("#" + obj).attr("checked")=="checked")
                $("#" + obj).removeAttr("checked");
            else
                $("#" + obj).attr("checked", "checked");
        }
    </script>


</asp:Content>
