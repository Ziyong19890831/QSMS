<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="Feedback.aspx.cs" Inherits="Web_Feedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
              .vCode{
            float: right; margin: 3px 5px 0 0; height: 27px; width:auto;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">意見回饋</li>
        </ol>
    </nav>

    <div class="row mt30">
        <div class="col-12">
            <h5>
                意見回饋
            </h5>
        </div>
        <div class="col-12">
            <div class="alert alert-success" role="alert">
                請填寫以下表格，填寫完畢請送出，謝謝！
            </div>
        </div>
    </div>

    <table class="table table-striped table-data">
        <tr>
            <th class="w2"><i class="fa fa-star"></i>意見類型</th>
            <td>
                <asp:RadioButtonList ID="rbl" runat="server" class="" RepeatColumns="6" RepeatLayout="Flow" >
                    <asp:ListItem Text="系統建議" Selected="True" />
                    <asp:ListItem Text="線上課程" />
                    <asp:ListItem Text="其他" />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>您的姓名</th>
            <td>
                <asp:TextBox ID="txt_Name" class="form-control" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>您的E-Mail</th>
            <td>
                <asp:TextBox ID="txt_Email" CssClass="required email form-control" runat="server" MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_Email" ErrorMessage="聯絡Email不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txt_Email" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>您的聯絡電話</th>
            <td>
                <asp:TextBox ID="txt_Phone" class="form-control" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>說明</th>
            <td>
                <asp:TextBox ID="txt_Explain" CssClass="required w10 form-control" runat="server" TextMode="MultiLine" Rows="10" MaxLength="500"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <th><i class="fa fa-star"></i>驗證碼</th>
            <td>
                <input name="input" type="text" class="form-control"  placeholder="驗證碼" id="txt_Verification_Right" runat="server" style="width:160px;float: left;margin-right: 10px;"  />
                 <image id="img_CheckCode" border="0" src="CheckCode.aspx" alt="驗證碼" style="float: left; margin: 3px 5px 0 0; height: 27px;"></image>
                <input type="button" class="btn btn-dark" onclick="img_CheckCode.src = 'CheckCode.aspx?' + Math.random();" value="重新整理" style="float:left;" />
               
            </td>
        </tr>
    </table>

    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" class="btn btn-primary" Text="送出" OnClick="btn_submit_Click" />
        <input type="button" value="重新填寫" class="btn btn-light" onclick="location.href = 'Feedback.aspx'" causesvalidation="false" />
    </div>

</asp:Content>



