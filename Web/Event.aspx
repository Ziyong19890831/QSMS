<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="Event.aspx.cs" Inherits="Web_Event" %>

<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../JS/jquery-ui.min.js"></script>
    <link rel="stylesheet" type="text/css" href="../CSS/jquery-ui.css" />
    <script type="text/javascript">
        $(function () {

            $(".datepicker").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'yy-mm-dd',
                yearRange: "-80:+0"
            }).blur(function () {
                val = $(this).val();
                val1 = Date.parse(val);
                if (isNaN(val1) == true && val !== '') {
                    $(this).val('');
                }
            });

        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">課程報名</li>
        </ol>
    </nav>
    <asp:ScriptManager ID="sc" runat="server"></asp:ScriptManager>
    <div class="alert alert-primary" role="alert">
        <div class="row">
            <div class="col-12">
                分類查詢：
            </div>
            <div class="col-12">
                <div class="form-row">
                    <div class="form-group col-md-3">
                        <asp:DropDownList ID="ddl_AddressA" class="form-control" runat="server" OnSelectedIndexChanged="ddl_AddressA_SelectedIndexChanged" DataValueField="AREA_CODE" DataTextField="AREA_NAME"></asp:DropDownList>
                    </div>
                    <div class="form-group col-md-3">
                        <asp:DropDownList ID="ddl_CourseClass4" class="form-control" runat="server" DataTextField="Mval" DataValueField="PVal"></asp:DropDownList>
                    </div>

                    <div class="form-group col-md-3">
                        <asp:DropDownList ID="ddl_Event_Class" class="form-control" runat="server" DataTextField="Mval" DataValueField="PVal"></asp:DropDownList>
                    </div>
                    <div class="form-group col-md-3">
                        <asp:DropDownList ID="dd2_RoleName" class="form-control" runat="server" DataTextField="RoleName" DataValueField="RoleSNO"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-12">
                <div class="form-row">
                    
                    <div class="form-group col-md-4">
                        <input type="text" placeholder="報名期間(起)" id="STime" class="datepicker date form-control" runat="server" />
                    </div>
                    <div class="form-group col-md-4">
                        <input type="text" placeholder="報名期間(迄)" id="ETime" class="datepicker date form-control" runat="server" />
                    </div>
                    <div class="form-group col-md-4">
                        <input type="text" placeholder="請輸入要查詢的關鍵字" id="txtSearch" class="form-control" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12">
                <div class="form-row">
                    <div class="form-group col-md-4">
                        <input type="text" placeholder="上課期間(起)" id="SCDay" class="datepicker date form-control" runat="server" />
                    </div>
                    <div class="form-group col-md-4">
                        <input type="text" placeholder="上課期間(迄)" id="ECDay" runat="server" class="datepicker date form-control" />
                    </div>
                    <div class="form-group col-md-4">
                        <asp:Button ID="btnSearch" runat="server" class="form-control btn-primary" Text="查詢" OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="tab-content" id="myTableData">
        <div class="tab-pane fade show active mt10">
            <table class="table table-striped">
                <tr>
                    <th>認證分類</th>
                    <th>課程分類</th>
                   <%-- <th>適用人員</th>--%>
                    <th>課程名稱</th>
                    <th>報名期間</th>
                    <th>上課日期</th>
                    <th>報名</th>
                </tr>
                <asp:Repeater ID="rpt_Notice" runat="server">
                    <ItemTemplate>
                        <tr>          
                            <td><%# Eval("Class1") %></td>
                            <td><%# Eval("Class2") %></td>
                            <%--<td><%# Eval("RoleBindName") %></td>--%>
                            <td style="width: 180px"><%# Eval("EventName") %></td>                      
                            <td class="date center" style="color: black"><%#Convert.ToDateTime(Eval("StartTime")).ToString("yyyy-MM-dd") %><br />
                                <span>|</span><br />
                                <%#Convert.ToDateTime(Eval("EndTime")).ToString("yyyy-MM-dd") %></td>
                            <td><%# Convert.ToDateTime(Eval("CDay")).ToString("yyyy-MM-dd")%></td>
                            <td class="center">
                                <%--<asp:LinkButton ID="LK_Link" runat="server" OnClick="LK_Link_Click" ><i class="fa fa-user-plus">我要報名</i></asp:LinkButton>--%>
                                <a href="Event_AE.aspx?sno=<%# Eval("EventSNO") %><%# Eval("PClassSNO").ToString()=="" ? "" :"&psno="+ Eval("PClassSNO")  %><%# Eval("ERSNO").ToString()=="" ? "" :"&ersno="+ Eval("ERSNO")  %>"
                                    <i class="fa fa-user-plus"></i>
                                    <span>我要報名</span>
                                </a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>

    </div>



</asp:Content>

