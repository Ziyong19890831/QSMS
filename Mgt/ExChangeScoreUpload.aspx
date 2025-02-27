<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="ExChangeScoreUpload.aspx.cs" Inherits="Mgt_ExChangeScoreUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../JS/zTree_v3/js/jquery.blockUI.js"></script>
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
      $(function () {
      $(".datepicker").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'yy-mm-dd'
            }).blur(function () {
                val = $(this).val();
                val1 = Date.parse(val);
                if (isNaN(val1) == true && val !== '') {
                    $(this).val('');
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
                $("#tabs").tabs();
            });
        </script>
        <script type="text/javascript">
            $(function () {
                $("#accordion-1").accordion();
                $("#accordion-2").accordion();
                $("#tabs").tabs();
            });
        </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="path txtS mb20">現在位置：<a href="#">繼續教育管理</a> <i class="fa fa-angle-right"></i><a href="ExChangeScoreUpload.aspx">繼續教育課程時數上傳</a></div>
       <div id="tabs" style="margin-bottom: 10px;">
        <ul>
            <li class="hide" id="tab1" runat="server"><a href="#tabs-1">繼續教育時數上傳</a></li>
            <li class="hide" visible="false" id="tab2" runat="server"><a href="#tabs-2">時數上傳查詢</a></li>
        </ul>
        <div id="tabs-1">
            
    <div class="both mb20">
        <fieldset>
            <legend>上傳</legend>
            <div class="left w8">

                <asp:UpdatePanel ID="upl_ddl" runat="server">
                    <ContentTemplate>
                        時數上傳類別：
                        <asp:DropDownList ID="ddl_UploadType" runat="server"  AutoPostBack="true">
                          <asp:ListItem Text="繼續教育時數上傳" Value="2"></asp:ListItem>
                        </asp:DropDownList>                      
                        <br />
                        繼續教育規劃類別：
                        <asp:DropDownList ID="ddl_ECoursePlanningClass" runat="server" AutoPostBack="true" 
                            DataValueField="EPClassSNO" DataTextField="PlanName" OnSelectedIndexChanged="ddl_CoursePlanningClass_SelectedIndexChanged"  ></asp:DropDownList>
                  
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="lb_Notice" ForeColor="Red" Font-Size="12px" runat="server" ></asp:Label>
                <br/>
                Excel檔案:       
                <asp:FileUpload ID="file_Upload" runat="server" />
            </div>
            <div class="right">
                <asp:Button ID="btnDownload" runat="server" Text="下載格式" OnClick="btnDownload_Click" />
                <br/>
                <asp:Button ID="btnUpload" runat="server" Text="上傳" OnClick="btnUpload_Click" />
            </div>
        </fieldset>

    </div>
    <fieldset>
        <legend>課程規劃資訊</legend>
         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
             <ContentTemplate>
         <asp:GridView ID="gv_CoursePlanningClass" runat="server" AutoGenerateColumns="False" >
              <Columns>
                  <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center">
                      <ItemStyle HorizontalAlign="Center"></ItemStyle>
                  </asp:BoundField>
                  <asp:BoundField HeaderText="繼續教育規劃名稱" DataField="PlanName" />
                  <%--<asp:BoundField HeaderText="適用起年度" DataField="CYear" />--%>
                  <asp:BoundField HeaderText="啟用" DataField="IsEnable" />
                  <asp:BoundField HeaderText="對應證書" DataField="CTypeName" />
<%--                  <asp:BoundField HeaderText="必修實體學分" DataField="Compulsory_Entity" />
                  <asp:BoundField HeaderText="必修實習學分" DataField="Compulsory_Practical" />
                  <asp:BoundField HeaderText="必修通訊學分" DataField="Compulsory_Communication" />
                   <asp:BoundField HeaderText="必修線上學分" DataField="Compulsory_Online" /> --%> 
                  <asp:BoundField HeaderText="適用對象" DataField="CRole" /> 
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
                <asp:BoundField DataField="Date" HeaderText="測驗日期" ItemStyle-HorizontalAlign="Center" />
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
                <asp:BoundField DataField="Date" HeaderText="測驗日期" ItemStyle-HorizontalAlign="Center" />
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
                 <asp:BoundField DataField="ID" HeaderText="身分證" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="MEMBER_NAME" HeaderText="學員名稱" ItemStyle-HorizontalAlign="Center" />              
                <asp:BoundField DataField="CourseName" HeaderText="課程名稱" ItemStyle-HorizontalAlign="Center" />
                 <asp:BoundField DataField="CDate" HeaderText="上課日期" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="EIntegral" HeaderText="學分" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="CType" HeaderText="類別" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="執行訊息">
                    <ItemTemplate>
                        <span style='<%#Eval("style")%>'><%#Eval("Status")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        
    </fieldset>
        </div>
           <div id="tabs-2" style="display:none">>
               <fieldset>
                             <div class="left w8"  style="display:none">>

                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        課程規劃類別
                        <asp:DropDownList ID="ddl_PlanName" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                            <asp:ListItem Text="繼續教育課程(證明展延)" Value="8"></asp:ListItem>
                            <asp:ListItem Text="繼續教育課程(醫師舊證書換新證明專屬課程)" Value="9"></asp:ListItem>


                        </asp:DropDownList>
                        課程類別
                        <asp:DropDownList ID="ddl_CourseClass" runat="server" AutoPostBack="true" DataValueField="PClassSNO" DataTextField="PlanName" OnSelectedIndexChanged="ddl_CoursePlanningClass_SelectedIndexChanged">
                            
                        </asp:DropDownList>
                        <br />
                        上傳時間
                        <asp:TextBox ID="txt_UploadTimeStart" runat="server" class="datepicker w2" placeholder="YYYY-MM-DD" ></asp:TextBox>~
                        <asp:TextBox ID="txt_UploadTimeEnd" runat="server" class="datepicker w2" placeholder="YYYY-MM-DD" ></asp:TextBox>
                        上傳帳號
                        <asp:TextBox ID="txt_UploadAccount" runat="server" class="w1"></asp:TextBox>
                        上傳者
                        <asp:TextBox ID="txt_UploadName" runat="server" class="w1"></asp:TextBox><br />
                        上傳單位
                         <asp:DropDownList ID="ddl_UploadUnit" runat="server" class="w2"  
                            DataValueField="PClassSNO" DataTextField="PlanName">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="Label1" ForeColor="Red" Font-Size="12px" runat="server" ></asp:Label>
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
           <asp:Label ID="lb_SearchNoAnswer" ForeColor="Red" Font-Size="Large"  runat="server" Visible="false">#查無結果</asp:Label>
           </div>
    </div>



    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>

