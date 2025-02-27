<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/Dialog.master" CodeFile="ReportSetColumn.aspx.cs" Inherits="Mgt_SetExcelColumn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();

         });

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="path txtS mb20">
        現在位置：<a href="#">報表作業</a> <i class="fa fa-angle-right"></i>欄位設定                
    </div>
    <asp:Panel ID="p1" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:CheckBoxList ID="cbl_SetColumn" runat="server" AutoPostBack="True" RepeatColumns="5" DataTextField="Value" DataValueField="Key" RepeatDirection="Horizontal"></asp:CheckBoxList>
            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="p2" runat="server" Visible="false">
          <div id="tabs" style="margin-bottom: 10px;">
        <ul>
            <li><a href="#tabs-1">基本資料</a></li>
            <li><a href="#tabs-2">證書資料</a></li>
            <li><a href="#tabs-3">醫事人員資料庫類別</a></li>
             <li><a href="#tabs-4">課程</a></li>
             <li><a href="#tabs-5">活動報名</a></li>
            <li><a href="#tabs-6">機構合約狀態</a></li>
        </ul>
        <div id="tabs-1">
            <asp:UpdatePanel ID="Up1" runat="server">
                <ContentTemplate>   
                    <div>
                <asp:CheckBoxList ID="CheckBoxList1" runat="server"  RepeatColumns="5" DataTextField="Value" DataValueField="Key" RepeatDirection="Horizontal"></asp:CheckBoxList>
            </div>
                </ContentTemplate>
            </asp:UpdatePanel>
       </div>
        <div id="tabs-2">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div>
                <asp:CheckBoxList ID="CheckBoxList2" runat="server"  RepeatColumns="5" DataTextField="Value" DataValueField="Key" RepeatDirection="Horizontal"></asp:CheckBoxList>
            </div>
                </ContentTemplate>
            </asp:UpdatePanel>
       </div> 
        <div id="tabs-3">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                   <div>
                <asp:CheckBoxList ID="CheckBoxList3" runat="server"  RepeatColumns="5" DataTextField="Value" DataValueField="Key" RepeatDirection="Horizontal"></asp:CheckBoxList>
            </div>
              </ContentTemplate>
            </asp:UpdatePanel>
        </div>
                  <div id="tabs-4">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                   <div>
                <asp:CheckBoxList ID="CheckBoxList4" runat="server" RepeatColumns="5" DataTextField="Value" DataValueField="Key" RepeatDirection="Horizontal"></asp:CheckBoxList>
            </div>
              </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="CheckBoxList4" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
               <div id="tabs-5">
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                   <div>
                <asp:CheckBoxList ID="CheckBoxList5" runat="server" AutoPostBack="True" RepeatColumns="5" DataTextField="Value" DataValueField="Key" RepeatDirection="Horizontal"></asp:CheckBoxList>
            </div>
              </ContentTemplate>
            </asp:UpdatePanel>
        </div>
               <div id="tabs-6">
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                   <div>
                <asp:CheckBoxList ID="CheckBoxList6" runat="server" AutoPostBack="True" RepeatColumns="5" DataTextField="Value" DataValueField="Key" RepeatDirection="Horizontal"></asp:CheckBoxList>
            </div>
              </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </asp:Panel>
       
    

    <div class="center btns">
        <input name="CheckAll" id="CheckAll" type="button" value="全選" />
         <input name="CancelAll" id="CancelAll" type="button" value="全不選" />
        <asp:Button ID="btnOK" runat="server" Text="匯出Excel" OnClick="btnOK_Click" />
        <input name="btnCancel" type="button" value="關閉" onclick="window.close();" />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#CheckAll").click(function () {
                $("input[type='Checkbox']").each(function () {
                    $(this).prop("checked", true);

                })
                $("#ContentPlaceHolder1_CheckBoxList4_2").prop("checked", false);
                $("#ContentPlaceHolder1_CheckBoxList4_3").prop("checked", false);
                $("#ContentPlaceHolder1_CheckBoxList4_4").prop("checked", false);
                $("#ContentPlaceHolder1_CheckBoxList4_5").prop("checked", false);
                $("#ContentPlaceHolder1_CheckBoxList4_6").prop("checked", false);
            })
        })
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#CancelAll").click(function () {               
                $("input[type='Checkbox']").each(function () {
                    $(this).prop("checked", false);
                })
                $("#ContentPlaceHolder1_CheckBoxList4_2").attr("disabled", false);
                $("#ContentPlaceHolder1_CheckBoxList4_3").attr("disabled", false);
                $("#ContentPlaceHolder1_CheckBoxList4_4").attr("disabled", false);
                $("#ContentPlaceHolder1_CheckBoxList4_5").attr("disabled", false);
                $("#ContentPlaceHolder1_CheckBoxList4_6").attr("disabled", false);
            })
        })
    </script>
    <script type="text/javascript">
        $('#ContentPlaceHolder1_CheckBoxList4_2').change(function () {
            if ($(this).is(':checked')) {
                $("#ContentPlaceHolder1_CheckBoxList4_4").attr("disabled", true);
                $("#ContentPlaceHolder1_CheckBoxList4_5").attr("disabled", true);
            }
            else if ($('#ContentPlaceHolder1_CheckBoxList4_2').not(this).prop('checked', false) && $('#ContentPlaceHolder1_CheckBoxList4_3').not(this).prop('checked', false)) {
                $("#ContentPlaceHolder1_CheckBoxList4_4").attr("disabled", false);
                $("#ContentPlaceHolder1_CheckBoxList4_5").attr("disabled", false);
            }
        });
    </script>
    <script type="text/javascript">
        $('#ContentPlaceHolder1_CheckBoxList4_3').change(function () {
            if ($(this).is(':checked')) {
                $("#ContentPlaceHolder1_CheckBoxList4_4").attr("disabled", true);
                $("#ContentPlaceHolder1_CheckBoxList4_5").attr("disabled", true);
            }
            else if ($('#ContentPlaceHolder1_CheckBoxList4_2').not(this).prop('checked', false) && $('#ContentPlaceHolder1_CheckBoxList4_3').not(this).prop('checked', false)) {
                $("#ContentPlaceHolder1_CheckBoxList4_4").attr("disabled", false);
                $("#ContentPlaceHolder1_CheckBoxList4_5").attr("disabled", false);
            }
        });
    </script>
    <script type="text/javascript">
        $('#ContentPlaceHolder1_CheckBoxList4_4').change(function () {
            if ($(this).is(':checked')) {
                $("#ContentPlaceHolder1_CheckBoxList4_2").attr("disabled", true);
                $("#ContentPlaceHolder1_CheckBoxList4_3").attr("disabled", true);
                $("#ContentPlaceHolder1_CheckBoxList4_5").attr("disabled", true);
            } else {
                $("#ContentPlaceHolder1_CheckBoxList4_2").attr("disabled", false);
                $("#ContentPlaceHolder1_CheckBoxList4_3").attr("disabled", false);
                $("#ContentPlaceHolder1_CheckBoxList4_5").attr("disabled", false);
            }
        });
    </script>
    <script type="text/javascript">
        $('#ContentPlaceHolder1_CheckBoxList4_5').change(function () {
            if ($(this).is(':checked')) {
                $("#ContentPlaceHolder1_CheckBoxList4_2").attr("disabled", true);
                $("#ContentPlaceHolder1_CheckBoxList4_3").attr("disabled", true);
                $("#ContentPlaceHolder1_CheckBoxList4_4").attr("disabled", true);
            } else {
                $("#ContentPlaceHolder1_CheckBoxList4_2").attr("disabled", false);
                $("#ContentPlaceHolder1_CheckBoxList4_3").attr("disabled", false);
                $("#ContentPlaceHolder1_CheckBoxList4_5").attr("disabled", false);
            }
        });
    </script>
</asp:Content>
