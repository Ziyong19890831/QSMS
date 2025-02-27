<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="TwentyScoreUpload.aspx.cs" Inherits="Mgt_TwentyScoreUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
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
    <script>

        $('#btnUploada').click(function (e) {
           
              

                $("<div><span><b>Are you sure you want to cancel this order?</b></span></div>").dialog({
                    modal: true,
                    draggable: false,
                    resizable: false,
                    width: 430,
                    height: 150,
                    buttons: {
                        "No": function () {
                            $(this).dialog("destroy");

                        },
                        "Yes": function () {
                            $("#btnCancel").unbind();
                            $(this).dialog("destroy");
                            document.getElementById('<%= btnUpload.ClientID %>').click();

                        }
                    }
                });
            });

        
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="path txtS mb20">現在位置：<a href="#">課程管理</a> <i class="fa fa-angle-right"></i><a href="TwentyScoreUpload.aspx">積分上傳-22縣市</a></div>

    <div class="both mb20">
        <fieldset>
            <legend>上傳</legend>
            <div class="left w8">

                <asp:UpdatePanel ID="upl_ddl" runat="server">
                    <ContentTemplate>
                        活動上傳：
                        <asp:DropDownList ID="ddl_EventName" runat="server" DataTextField="EventName" DataValueField="ERSNO">

                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                Excel檔案:       
                <asp:FileUpload ID="file_Upload" runat="server" />
            </div>
            <div class="right">
                <asp:Button ID="btnDownload" runat="server" Text="下載格式" OnClick="btnDownload_Click" />
                <br />
                <asp:Button ID="btnUpload" runat="server" Text="上傳" OnClick="btnUpload_Click" />
                <%--<input type="button" id="btnUploada" value="測試" />--%>
            </div>
        </fieldset>

    </div>


    <fieldset>
        <legend>上傳結果</legend>


        <asp:GridView ID="gv_ScoreUpload_Class" runat="server" AutoGenerateColumns="False" >

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

   

    </fieldset>


    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="txt_Page" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>

