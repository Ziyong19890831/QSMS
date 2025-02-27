<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master"  AutoEventWireup="true" CodeFile="UploadRecordSearch.aspx.cs" Inherits="Mgt_UploadRecordSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
        $(document).ready(function () {
            $('body').on('focusin', '.datepicker', function () {
                if (!$(this).data('datepicker')) {
                    $(this).datepicker({
                        dateFormat: 'yy-mm-dd'
                    });
                }
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $("#ContentPlaceHolder1_btnUpload").click(function () {
                $.blockUI({
                    message: '<h1>讀取中...</h1>'
                });
            });//end click

        });
    </script>
    <script type="text/javascript">
        $(function () {
            $("#accordion-1").accordion();
            $("#accordion-2").accordion();
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".datepicker").datepicker({
                dateFormat: 'yy-mm-dd'
            });

        });
        $(function () {
            $('#tabs').tabs({ cookie: { expires: 30 } });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="path txtS mb20">現在位置：新訓課程時數上傳紀錄查詢<i class="fa fa-angle-right"></i><a href="UploadRecordSearch.aspx">新訓課程時數上傳紀錄查詢</a></div>
    <div id="tabs" style="margin-bottom: 10px;" >
        <ul>
            <li class="hide" visible="false" id="tab1" runat="server"><a href="#tabs-1">新訓課程時數上傳</a></li>
            <li class="hide" id="tab2" runat="server"><a href="#tabs-2">新訓課程時數上傳紀錄查詢</a></li>
        </ul>

        <div id="tabs-1" style="display:none">
            <div class="both mb20" style="display:none">
                <fieldset>
                    <legend>上傳</legend>
                    <div class="left w8">

                        <asp:UpdatePanel ID="upl_ddl" runat="server">
                            <ContentTemplate>
                                時數上傳類別：
                        <asp:DropDownList ID="ddl_UploadType" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="新訓課程時數" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                                <br />
                                課程規劃類別：
                        <asp:DropDownList ID="ddl_CoursePlanningClass" runat="server" AutoPostBack="true"
                            DataValueField="PClassSNO" DataTextField="PlanName" OnSelectedIndexChanged="ddl_UploadType_SelectedIndexChanged">
                        </asp:DropDownList>
                                <br />
                                欲上傳的課程:
                        <asp:DropDownList ID="ddl_CourseName" runat="server" DataValueField="CourseSNO" DataTextField="CourseName" OnSelectedIndexChanged="ddl_CourseName_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:Label ID="lb_Notice" ForeColor="Red" Font-Size="12px" runat="server"></asp:Label>
                        <br />
                        Excel檔案:       
                <asp:FileUpload ID="file_Upload" runat="server" />
                    </div>
                    <div class="right">
                        <asp:Button ID="btnDownload" runat="server" Text="下載格式" OnClick="btnDownload_Click" />
                        <br />
                        <asp:Button ID="btnUpload" runat="server" Text="上傳" OnClick="btnUpload_Click" />
                    </div>
                </fieldset>

            </div>
            <fieldset>
                <legend>課程規劃資訊</legend>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gv_CoursePlanningClass" runat="server" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="課程規劃名稱" DataField="PlanName" />
                                <%--                        <asp:BoundField HeaderText="適用起迄年度" DataField="CYear" />--%>
                                <asp:BoundField HeaderText="啟用" DataField="IsEnable" />
                                <asp:BoundField HeaderText="對應證明" DataField="CTypeName" />
                                <asp:BoundField HeaderText="總時數" DataField="sumHour" />
                                <asp:BoundField HeaderText="課程數" DataField="countCourse" />
                                <asp:BoundField HeaderText="目標時數" DataField="TargetIntegral" />
                                <asp:BoundField HeaderText="適用對象" DataField="CRole" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
            <fieldset>
                <legend>課程資訊</legend>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gv_CourseName" runat="server" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField HeaderText="課程名稱" DataField="CourseName" />
                                <asp:BoundField HeaderText="課程時數" DataField="CHour" />
                                <asp:BoundField HeaderText="課程類型" DataField="課程類別" />
                                <%--                        <asp:BoundField HeaderText="上課方式" DataField="上課方式" />   --%>
                                <asp:BoundField HeaderText="課程代碼" DataField="CourseSNO" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>

            <fieldset>
                <legend>上傳結果</legend>

                <asp:GridView ID="gv_ScoreUpload" runat="server" AutoGenerateColumns="False" Visible="false">

                    <Columns>
                        <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="MEMBER_NAME" HeaderText="學員名稱" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ID" HeaderText="身分證" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Score" HeaderText="測驗分數" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Date" HeaderText="上課日期" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="PassScore" HeaderText="通過分數" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="IsPass" HeaderText="是否通過" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="執行訊息">
                            <ItemTemplate>
                                <span style='<%#Eval("style")%>'><%#Eval("Status")%></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:GridView ID="gv_ScoreUpload_Class" runat="server" AutoGenerateColumns="False" Visible="false">

                    <Columns>
                        <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="MEMBER_NAME" HeaderText="學員名稱" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ID" HeaderText="身分證" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ClassLocation" HeaderText="上課地點" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Date" HeaderText="上課日期" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Unit" HeaderText="辦理單位" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="執行訊息">
                            <ItemTemplate>
                                <span style='<%#Eval("style")%>'><%#Eval("Status")%></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:GridView ID="gv_UpdateScore_Change" runat="server" AutoGenerateColumns="False" Visible="false">

                    <Columns>
                        <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="MEMBER_NAME" HeaderText="學員名稱" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ID" HeaderText="身分證" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ClassLocation" HeaderText="上課地點" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Date" HeaderText="上課日期" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Unit" HeaderText="辦理單位" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Score" HeaderText="分數" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="執行訊息">
                            <ItemTemplate>
                                <span style='<%#Eval("style")%>'><%#Eval("Status")%></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </fieldset>

        </div>
        <div id="tabs-2" style="display:none">
            <fieldset>
                <legend>功能列</legend>
                <div class="left w8">

                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            課程規劃類別
                        <asp:DropDownList ID="ddl_PlanName" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                            <asp:ListItem Text="基礎課程(新訓)" Value="1"></asp:ListItem>
                            <asp:ListItem Text="專門課程(醫師新訓)" Value="2"></asp:ListItem>
                            <asp:ListItem Text="專門課程(藥師新訓)" Value="3"></asp:ListItem>
                            <asp:ListItem Text="專門課程(其他人員新訓)" Value="4"></asp:ListItem>
                            <asp:ListItem Text="基礎課程(醫師衛教資格補課)" Value="5"></asp:ListItem>

                        </asp:DropDownList>
                            課程類別
                        <asp:DropDownList ID="ddl_CourseClass" runat="server" AutoPostBack="true"
                            DataValueField="PClassSNO" DataTextField="PlanName" OnSelectedIndexChanged="ddl_CoursePlanningClass_SelectedIndexChanged">
                        </asp:DropDownList>
                            <br />
                            上傳時間
                            <asp:TextBox ID="txt_UploadTimeStart" runat="server" class="datepicker w2" placeholder="YYYY-MM-DD"></asp:TextBox>~<asp:TextBox ID="txt_UploadTimeEnd" runat="server" class="datepicker w2" placeholder="YYYY-MM-DD"></asp:TextBox>
                            上傳帳號
                        <asp:TextBox ID="txt_UploadAccount" runat="server" class="w1"></asp:TextBox>
                            上傳者
                        <asp:TextBox ID="txt_UploadName" runat="server" class="w1"></asp:TextBox><br />
                            上課日期
                            <asp:TextBox ID="txt_CDate" runat="server" class="datepicker w2" placeholder="YYYY-MM-DD"></asp:TextBox>
                            上傳單位
                        <asp:DropDownList ID="ddl_UploadUnit" runat="server" class="w2" DataValueField="PClassSNO" DataTextField="PlanName">
                        </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Label ID="Label1" ForeColor="Red" Font-Size="12px" runat="server"></asp:Label>
                </div>
                <div class="right">
                    <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                </div>
            </fieldset>
            <asp:GridView ID="gv_Course" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField HeaderText="序號" DataField="ROW_NO">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="課程規劃類別名" DataField="CoursePlanning" />
                    <asp:BoundField HeaderText="課程類別" DataField="Class" />
                    <asp:BoundField HeaderText="上課日期" DataField="CDate" />
                    <asp:BoundField HeaderText="上傳時間" DataField="CreateDT" />
                    <asp:BoundField HeaderText="上傳帳號" DataField="PAccount" />
                    <asp:BoundField HeaderText="上傳者" DataField="PName" />
                    <asp:BoundField HeaderText="上傳單位" DataField="OrganName" />
                    <asp:TemplateField HeaderText="下載">
                        <ItemTemplate>
                            <asp:LinkButton ID="Btn_Print" runat="server" ForeColor="Blue" CommandArgument='<%# Eval("FileSNO") %>' OnClick="Btn_Print_Click">下載</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Label ID="lb_SearchNoAnswer" ForeColor="Red" Font-Size="Large" runat="server" Visible="false">#查無結果</asp:Label>
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:Button ID="Button1" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
                <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />

        </div>
    </div>




    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>
