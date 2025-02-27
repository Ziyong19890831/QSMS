<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="Event_Apply.aspx.cs" Inherits="Web_Feedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">


        $(function chkInfo() {

            var noticetype = $("input[name='ctl00$ContentPlaceHolder1$rbl_NoticeType']:checked").val();

            switch (noticetype) {

                case "1":
                    if ($("#<%=lbl_PPhone.ClientID%>").text() == "") {
                        alert("您的手機尚未填寫，故無法選擇[簡訊]發送通知，請至學員專區>學員資料進行修改。");
                        event.preventDefault();
                    }
                    break;
                case "2":

                    if ($("#<%=lbl_PMail.ClientID%>").text() == "") {
                        alert("您的手機尚未填寫，故無法選擇[信箱]發送通知，請至學員專區>學員資料進行修改。");
                        event.preventDefault();
                    }
                    break;
                case "3":
                    if ($("#<%=lbl_PTel.ClientID%>").text() == "") {
                        alert("您的手機尚未填寫，故無法選擇[電話]通知，請至學員專區>學員資料進行修改。");
                        event.preventDefault();
                    }
                    break;
                default: event.preventDefault(); return; break;
            }
        })

    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">課程及活動報名表格</li>
        </ol>
    </nav>

    <div class="row mt30">

        <div class="col-12">
            <div class="alert alert-danger" role="alert">
                請確認您的基本資料是否正確，謝謝！（如資料為空白或有誤，請至學員專區>學員資料進行修改）
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-12">
            <table class="table table-striped">
                <tr>
                    <th>活動名稱</th>
                    <td >
                        <asp:Label ID="lbl_EventName" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th>您的姓名</th>
                    <td >
                        <asp:Label ID="lbl_PName" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th>您的E-Mail</th>
                    <td >
                        <asp:Label ID="lbl_PMail" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th>您的手機</th>
                    <td >
                        <asp:Label ID="lbl_PPhone" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th>您的電話</th>
                    <td >
                        <asp:Label ID="lbl_PTel" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th>報名錄取通知方式</th>
                    <td >
                        <asp:Label ID="lbl_Notice" runat="server" Text="信箱 "></asp:Label><asp:Label runat="server" Text="(請確保E-Mail可正常收信)" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <asp:Panel ID="P_Meals" runat="server" Visible="false">
                    <tr>
                        <th><i class="fa fa-star"></i>餐點種類</th>
                        <td >
                            <asp:RadioButtonList ID="rbl_Meals" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ToolTip="按一次全選，再按一次取消全選">
                                <asp:ListItem Value="0" Text="不需要" Selected="True" />
                                <asp:ListItem Value="1" Text="葷食" />
                                <asp:ListItem Value="2" Text="素食" />

                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </asp:Panel>
               
            </table>
            <div class="row">
                <div class="col-12">
                    <table class="table table-striped">
                        <tr>
                            <th>1.若活動有需要相關文件以示證明再上傳<br />
                                2.若有多個檔案，請壓縮成一個檔案後再上傳</th>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div>
                                            <asp:Button ID="btn_delfile1" runat="server" Text="刪除" OnClick="btn_delfile_Click" CommandArgument="1" Visible="false" />
                                            <asp:Literal ID="lt_file1" runat="server"></asp:Literal>
                                            <asp:FileUpload ID="fileup_Document1" runat="server" />
                                            <%--<asp:RequiredFieldValidator ID="rfv_Title" runat="server" ErrorMessage="請上傳至少一個檔案" ControlToValidate="fileup_Document1" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                        </div>
                                        <div>
                                            <asp:Button ID="btn_delfile2" runat="server" Text="刪除" OnClick="btn_delfile_Click" CommandArgument="2" Visible="false" />
                                            <asp:Literal ID="lt_file2" runat="server"></asp:Literal>
                                            <asp:FileUpload ID="fileup_Document2" runat="server" />
                                        </div>
                                        <div>
                                            <asp:Button ID="btn_delfile3" runat="server" Text="刪除" OnClick="btn_delfile_Click" CommandArgument="3" Visible="false" />
                                            <asp:Literal ID="lt_file3" runat="server"></asp:Literal>
                                            <asp:FileUpload ID="fileup_Document3" runat="server" />
                                        </div>
                                        <div>
                                            <asp:Button ID="btn_delfile4" runat="server" Text="刪除" OnClick="btn_delfile_Click" CommandArgument="4" Visible="false" />
                                            <asp:Literal ID="lt_file4" runat="server"></asp:Literal>
                                            <asp:FileUpload ID="fileup_Document4" runat="server" />
                                        </div>
                                        <div>
                                            <asp:Button ID="btn_delfile5" runat="server" Text="刪除" OnClick="btn_delfile_Click" CommandArgument="5" Visible="false" />
                                            <asp:Literal ID="lt_file5" runat="server"></asp:Literal>
                                            <asp:FileUpload ID="fileup_Document5" runat="server" />
                                        </div>

                                        <div>
                                            <asp:Button ID="btn_delfile6" runat="server" Text="刪除" OnClick="btn_delfile_Click" CommandArgument="1" Visible="false" />
                                            <asp:Literal ID="lt_file6" runat="server"></asp:Literal>
                                            <asp:FileUpload ID="fileup_Document6" runat="server" />
                                            <%--                    <asp:RequiredFieldValidator runat="server" ForeColor="Red" Display="Dynamic" ErrorMessage="請上傳證書" ControlToValidate="fileup_Document6">
                    </asp:RequiredFieldValidator>--%>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <div class="center btns">
                        <input type="submit" class="form-control btn-success" id="btn_submit" name="btn_submit" value="確認報名" runat="server" onserverclick="btn_submit_Click" onclientclick="this.disabled='disabled';" />
                        <input type="button" class="form-control" value="回上頁" onclick=" history.go(-1);" causesvalidation="false" />
                    </div>
                </div>
            </div>
</asp:Content>



