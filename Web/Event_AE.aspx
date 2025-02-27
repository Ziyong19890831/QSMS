<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="Event_AE.aspx.cs" Inherits="Web_Event_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .btn_ApplyCss {
            border: 1px solid #999999;
            background-color: #cccccc;
        }

        lbl_NotDone {
            color: red;
        }

        #ContentPlaceHolder1_gv_CertificateSNO {
            margin-bottom: 0px;
            margin-top: 40px;
        }

        table {
            table-layout: fixed;
            word-break: keep-all; 
        }

        td {
            table-layout: fixed;
            word-wrap: break-word;
        }
        input{
            color:black;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">課程報名</li>
        </ol>
    </nav>
    <div class="row">
        <div class="col-12">
            <div class="tab-content" id="myTableData">
                <table class="table table-striped">
                    <tr>
                        <th>認證分類</th>
                        <th>課程分類</th>
                        <th>課程名稱</th>
                        <th>報名時間</th>
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
                    <table class="table table-striped">
                        <tr>
                            <th colspan="4" style="text-align:center">適用人員</th>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lb_RoleBindName" runat="server" class="control-label"></asp:Label></td>
                        </tr>
                        <tr>
                            <th colspan="4" style="text-align:center">說明</th>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lb_Note" runat="server" class="control-label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th>錄取人數</th>
                            <td>
                                <asp:Label ID="lb_CountAudit" runat="server" class="control-label"></asp:Label></td>
                            <th>證明類別</th>
                            <td>
                                <asp:Label ID="lb_QTypeName" runat="server" class="control-label"></asp:Label></td>
                        </tr>
                        <tr>
                            <th>主辦單位</th>
                            <td>
                                <asp:Label ID="lb_Host" runat="server" class="control-label"></asp:Label></td>
                            <th>課程費用</th>
                            <td>
                                <asp:Label ID="lb_ActiveCost" runat="server" class="control-label"></asp:Label></td>
                        </tr>
                        <tr>
                            <th>聯絡人</th>
                            <td colspan="3">
                                <asp:Label ID="lb_CPerson" runat="server" class="control-label"></asp:Label></td>
                        </tr>
                        <tr>
                            <th>電話</th>
                            <td colspan="3"> 
                                <asp:Label ID="lb_CPhone" runat="server" class="control-label"></asp:Label></td>
                        </tr>
                        <tr>
                            <th>Email</th>
                            <td colspan="3">
                                <asp:Label ID="lb_Cmail" runat="server" class="control-label"></asp:Label></td>
                        </tr>
                        <tr>
                            <th>聯絡地址</th>
                            <td colspan="3">
                                <asp:Label ID="lb_Address" runat="server" class="control-label"></asp:Label></td>
                        </tr>
                         </table>
                    <table class="table table-striped">
                        <tr>
                            <th colspan="4" style="text-align:center">課程資訊</th>
                        </tr>
                         <tr>
                            <th>上課日期</th>
                            <td>
                                <asp:Label ID="lb_CEdate" runat="server" ></asp:Label></td>
                            <th>上課時間</th>
                            <td>
                                <asp:Label ID="lb_CEtime" runat="server" ></asp:Label></td>
                        </tr>
                         <tr>
                            <th>上課時數:</th>
                            <td>
                                <asp:Label ID="lb_Hour" runat="server" class="control-label"></asp:Label></td>
                            <th>上課節數:</th>
                            <td>
                                <asp:Label ID="lb_Class" runat="server" class="control-label"></asp:Label></td>
                        </tr>
                         <tr>
                            <th>課程場次:</th>
                            <td>
                                <asp:Label ID="lb_EventNote" runat="server" class="control-label"></asp:Label></td>
                        </tr>
                    </table>
                </div>
                <div class="center">
                    <asp:Button ID="btn_Apply" name="btn_Apply" runat="server" Text="報名" OnClick="btn_Apply_Click" />


                    <asp:LinkButton ID='btnDEL' class="form-control btn-success" style="text-align:center" Visible="false" runat="server" OnClientClick="return confirm('是否取消報名?');"
                        OnClick='btnDEL_Click'><i class="fa fa-trash"></i>您已報名過，點此可取消報名。</asp:LinkButton>
                    
                    <input type="button" value="回上頁" onclick="location.href = 'Event.aspx';" />
                    <asp:Label ID="lb_Msg" class="form-control" style="text-align:center"  Visible="false" runat="server" Text="報名人數已額滿"></asp:Label>
                </div>
                <br />
                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
                <asp:PlaceHolder ID="PlaceHolder_Logic" runat="server"></asp:PlaceHolder>
                <asp:PlaceHolder ID="PlaceHolder3" runat="server"></asp:PlaceHolder>
                <asp:PlaceHolder ID="PlaceHolder4" runat="server"></asp:PlaceHolder>
                <asp:PlaceHolder ID="PlaceHolder5" runat="server"></asp:PlaceHolder>
                <table class="table table-striped">
                    <tr id="Tr4" runat="server" visible="false">
                        <th>課程名稱</th>
                        <th>是否已修</th>
                    </tr>
                    <asp:Repeater ID="Repeater1" runat="server">
                        <itemtemplate>
                            <tr>
                                <td><%# Eval("課程名稱") %></td>
                                <td><%# Eval("是否已修") %></td>
                            </tr>
                        </itemtemplate>
                    </asp:Repeater>
                </table>
                <table class="table table-striped">
                    <tr id="Tr1" runat="server" visible="false">
                        <th>證書名稱</th>
                        <th>是否取得證明</th>
                         <th>是否有在期限內</th>
                    </tr>
                    <asp:Repeater ID="Repeater2" runat="server">
                        <itemtemplate>
                            <tr>
                                <td><%# Eval("證書名稱") %></td>
                                <td><%# Eval("是否取得證明") %></td>
                                <td><%# Eval("是否有在期限內") %></td>
                            </tr>
                        </itemtemplate>
                    </asp:Repeater>
                </table>
            </div>

            <div class="row">
                <div class="col-12">
                    <table class="table table-striped">
                        <tr>
                            <th>已報名人數</th>
                            <td>
                                <asp:Label ID="lbl_Count" runat="server" Text=""></asp:Label>人</td>
                        </tr>
                        <tr id="tr_CountAdmit" runat="server">
                            <th>預計錄取人數</th>
                            <td>
                                <asp:Label ID="lb_CountAdmit" runat="server" Text=""></asp:Label>人
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        </div>
   
</asp:Content>

