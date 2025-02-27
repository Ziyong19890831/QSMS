<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="ForgetPassword.aspx.cs" Inherits="Web_ForgetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
    table a {
        color:#000000 ;
    }
    .width160 {
        width: 160px;
    }
    .fa-star {
        color: #FF0000;
    }
    </style>
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">重置密碼</li>
        </ol>
    </nav>
    <div class="row mt30">
        <div class="col-12">
            <h5>
                <i class="fa fa-key"></i>
                重置密碼
            </h5>
        </div>
    </div>
    <div class="row mt10">
        <div class="col-12">
            <table class="table table-striped">
                <tr>
                    <th class="width160">
                        <i class="fa fa-star"></i>
                        身分證
                    </th>
                    <td>
                        <input name="txt_PersonID" id="txt_PersonID" class="form-control" runat="server" style="width: 100%" />
                        <asp:RequiredFieldValidator ID="rfv_PersonID" runat="server" ControlToValidate="txt_PersonID" ErrorMessage="不可為空" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th class="width160">
                        <i class="fa fa-star"></i>
                        帳號
                    </th>
                    <td>
                        <input name="txt_Acc" id="txt_Acc" runat="server" class="form-control" style="width: 100%" />
                        <asp:RequiredFieldValidator ID="rfv_Acc" runat="server" ControlToValidate="txt_Acc" ErrorMessage="不可為空" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th class="width160">
                        <i class="fa fa-star"></i>
                        信箱
                    </th>
                    <td>
                        <input name="txt_Mail" id="txt_Mail" runat="server" class="form-control" style="width: 100%" />
                        <asp:RequiredFieldValidator ID="rfv_Mail" runat="server" ControlToValidate="txt_Mail" ErrorMessage="不可為空" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
        </div>
        <div class="col-12 center">
            <input name="btn_Reset" type="button" class="btn btn-success" value="寄送重置密碼信件" runat="server" onserverclick="btn_Reset_Click" />
            <input name="btn_Cancel" type="button" class="btn btn-light"  value="取消" onclick="location.href = 'Notice.aspx'" />
        </div>
    </div>
</asp:Content>

