<%@ Page Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="CourseNote.aspx.cs" Inherits="Web_CourseNote" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function EXClick(ELSCode) {
            $.ajax({
                type: "POST",
                url: "CourseNote.aspx/AutoSignBindData",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ ELSCode: ELSCode }),
                dataType: "json",
                success: function (data) {
                    if (data.d == 'false') {
                        alert('請先登入系統');
                    } else {
                        var newWindow = window.open(data.d, "_blank");
                    }

                }
            });
        }
        function FileIpload() {
            $.ajax({
                type: "POST",
                url: "CourseNote.aspx/fileUpload",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d == 'false') {
                        alert('請先登入系統');
                    } else {
                        var newWindows = window.open('fileUpload.aspx', '_blank', config = 'height=300px,width=500px');
                    }

                }
            });
        }
    </script>
    <style type="text/css">
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
            /*opacity: 0.6;
            filter: alpha(opacity=60);*/
            position: fixed;
            text-align: center;
            line-height: 25px;
            display: none;
        }

        area:hover {
            background-color: yellow;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style>
    table a {
        color:#000000 ;
    }
    .width160 {
        width: 160px;;
    }
    .fa-star {
        color: #FF0000;
    }
    </style>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">戒菸服務人員訓練流程圖</li>
        </ol>
    </nav>
    <div class="container">
        <ul class="nav nav-tabs">
            <li class="nav-item">
                <a class="nav-link" data-toggle="tab" href="#home">(醫師)新訓</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" data-toggle="tab" href="#board">(藥師)新訓</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" data-toggle="tab" href="#download">(其他人員及公衛師)新訓</a>
            </li>
             <li class="nav-item">
                <a class="nav-link" data-toggle="tab" href="#home1">(醫師)證書(明)展延</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" data-toggle="tab" href="#board2">(非醫師)證書(明)展延</a>
            </li>
         
        </ul>
        <div class="tab-content" style="padding: 10px;">
            <div class="tab-pane active" id="home">
                <img src="../Images/醫生0001.jpg" class="img-fluid" alt="Responsive image">
            </div>
            <div class="tab-pane" id="board">
                <img src="../Images/藥師0001.jpg" class="img-fluid" alt="Responsive image">
            </div>
            <div class="tab-pane" id="download">
                <img src="../Images/其他0001.jpg" class="img-fluid" alt="Responsive image">
            </div>
            <div class="tab-pane" id="home1">
                <img src="../Images/01-1.戒菸服務人員訓練流程圖_關閉醫師繼續教育舊換新課程_1140120v1改_page-0002.jpg" class="img-fluid" alt="Responsive image">
            </div>
            <div class="tab-pane" id="board2">
                <img src="../Images/01-1.戒菸服務人員訓練流程圖_關閉醫師繼續教育舊換新課程_1140120v1改_page-0003.jpg" class="img-fluid" alt="Responsive image">
            </div>
    
        </div>
    </div>

</asp:Content>

