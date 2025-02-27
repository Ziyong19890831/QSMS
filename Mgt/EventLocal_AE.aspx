<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="EventLocal_AE.aspx.cs" Inherits="Mgt_EventLocal_AE"  validateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../ckeditor/ckeditor.js"></script>

    <script type="text/javascript">
        $(function () {
            $("#<%=txt_CStartDay_F.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
            $("#<%=txt_CEndDay_F.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });

            $("#<%=txt_SignS_F.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
            $("#<%=txt_SignE_F.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });

        });
        $(function () {
            $("#<%=txt_CStartDay_S.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
            $("#<%=txt_CEndDay_S.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });



        });
        $(function () {
            $("#<%=txt_CStartDay_T.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
            $("#<%=txt_CEndDay_T.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });


        });

        $.datepicker.setDefaults($.datepicker.regional["zh-TW"]);

        $(function () {
            $("#tabs").tabs();

        });

        $(function () {
            var select = document.getElementById('ContentPlaceHolder1_ddl_Class2');

            for (i = 0; i < select.length; i++) {
                if (select.options[i].value == '0') {
                    select.remove(i)
                }
            }
        });

        

          
    </script>

         <script>
             function pageLoad() {
                 // This sample still does not showcase all CKEditor 5 features (!)
                 // Visit https://ckeditor.com/docs/ckeditor5/latest/features/index.html to browse all the features.
                 CKEDITOR.ClassicEditor.create(document.getElementById('<%=editor1.ClientID %>'), {
                     // https://ckeditor.com/docs/ckeditor5/latest/features/toolbar/toolbar.html#extended-toolbar-configuration-format
                     toolbar: {
                         items: ['ckfinder', '|',
                             'exportPDF', 'exportWord', '|',
                             'findAndReplace', 'selectAll', '|',
                             'heading', '|',
                             'bold', 'italic', 'strikethrough', 'underline', 'code', 'subscript', 'superscript', 'removeFormat', '|',
                             'bulletedList', 'numberedList', 'todoList', '|',
                             'outdent', 'indent', '|',
                             'undo', 'redo',
                             '-',
                             'fontSize', 'fontFamily', 'fontColor', 'fontBackgroundColor', 'highlight', '|',
                             'alignment', '|',
                             'link', 'insertImage', 'blockQuote', 'insertTable', 'mediaEmbed', 'codeBlock', 'htmlEmbed', '|',
                             'specialCharacters', 'horizontalLine', 'pageBreak', '|',
                             'textPartLanguage', '|',
                             'sourceEditing'
                         ],
                         shouldNotGroupWhenFull: true
                     },
                     // Changing the language of the interface requires loading the language file using the <script> tag.
                     language: 'tw',
                     list: {
                         properties: {
                             styles: true,
                             startIndex: true,
                             reversed: true
                         }
                     },
                     // https://ckeditor.com/docs/ckeditor5/latest/features/headings.html#configuration
                     heading: {
                         options: [
                             { model: 'paragraph', title: 'Paragraph', class: 'ck-heading_paragraph' },
                             { model: 'heading1', view: 'h1', title: 'Heading 1', class: 'ck-heading_heading1' },
                             { model: 'heading2', view: 'h2', title: 'Heading 2', class: 'ck-heading_heading2' },
                             { model: 'heading3', view: 'h3', title: 'Heading 3', class: 'ck-heading_heading3' },
                             { model: 'heading4', view: 'h4', title: 'Heading 4', class: 'ck-heading_heading4' },
                             { model: 'heading5', view: 'h5', title: 'Heading 5', class: 'ck-heading_heading5' },
                             { model: 'heading6', view: 'h6', title: 'Heading 6', class: 'ck-heading_heading6' }
                         ]
                     },
                     // https://ckeditor.com/docs/ckeditor5/latest/features/editor-placeholder.html#using-the-editor-configuration
                     placeholder: 'Welcome to CKEditor 5!',
                     // https://ckeditor.com/docs/ckeditor5/latest/features/font.html#configuring-the-font-family-feature
                     fontFamily: {
                         options: [
                             'default',
                             'Arial, Helvetica, sans-serif',
                             'Courier New, Courier, monospace',
                             'Georgia, serif',
                             'Lucida Sans Unicode, Lucida Grande, sans-serif',
                             'Tahoma, Geneva, sans-serif',
                             'Times New Roman, Times, serif',
                             'Trebuchet MS, Helvetica, sans-serif',
                             'Verdana, Geneva, sans-serif'
                         ],
                         supportAllValues: true
                     },
                     // https://ckeditor.com/docs/ckeditor5/latest/features/font.html#configuring-the-font-size-feature
                     fontSize: {
                         options: [10, 12, 14, 'default', 18, 20, 22],
                         supportAllValues: true
                     },
                     // Be careful with the setting below. It instructs CKEditor to accept ALL HTML markup.
                     // https://ckeditor.com/docs/ckeditor5/latest/features/general-html-support.html#enabling-all-html-features
                     htmlSupport: {
                         allow: [
                             {
                                 name: /.*/,
                                 attributes: true,
                                 classes: true,
                                 styles: true
                             }
                         ]
                     },
                     // Be careful with enabling previews
                     // https://ckeditor.com/docs/ckeditor5/latest/features/html-embed.html#content-previews
                     htmlEmbed: {
                         showPreviews: true
                     },
                     // https://ckeditor.com/docs/ckeditor5/latest/features/link.html#custom-link-attributes-decorators
                     link: {
                         decorators: {
                             addTargetToExternalLinks: true,
                             defaultProtocol: 'https://',
                             toggleDownloadable: {
                                 mode: 'manual',
                                 label: 'Downloadable',
                                 attributes: {
                                     download: 'file'
                                 }
                             }
                         }
                     },
                     // https://ckeditor.com/docs/ckeditor5/latest/features/mentions.html#configuration
                     mention: {
                         feeds: [
                             {
                                 marker: '@',
                                 feed: [
                                     '@apple', '@bears', '@brownie', '@cake', '@cake', '@candy', '@canes', '@chocolate', '@cookie', '@cotton', '@cream',
                                     '@cupcake', '@danish', '@donut', '@dragée', '@fruitcake', '@gingerbread', '@gummi', '@ice', '@jelly-o',
                                     '@liquorice', '@macaroon', '@marzipan', '@oat', '@pie', '@plum', '@pudding', '@sesame', '@snaps', '@soufflé',
                                     '@sugar', '@sweet', '@topping', '@wafer'
                                 ],
                                 minimumCharacters: 1
                             }
                         ]
                     },
                     // The "super-build" contains more premium features that require additional configuration, disable them below.
                     // Do not turn them on unless you read the documentation and know how to configure them and setup the editor.
                     removePlugins: [
                         // These two are commercial, but you can try them out without registering to a trial.
                         // 'ExportPdf',
                         // 'ExportWord',



                         // This sample uses the Base64UploadAdapter to handle image uploads as it requires no configuration.
                         // https://ckeditor.com/docs/ckeditor5/latest/features/images/image-upload/base64-upload-adapter.html
                         // Storing images as Base64 is usually a very bad idea.
                         // Replace it on production website with other solutions:
                         // https://ckeditor.com/docs/ckeditor5/latest/features/images/image-upload/image-upload.html
                         // 'Base64UploadAdapter',
                         'RealTimeCollaborativeComments',
                         'RealTimeCollaborativeTrackChanges',
                         'RealTimeCollaborativeRevisionHistory',
                         'PresenceList',
                         'Comments',
                         'TrackChanges',
                         'TrackChangesData',
                         'RevisionHistory',
                         'Pagination',
                         'WProofreader',
                         // Careful, with the Mathtype plugin CKEditor will not load when loading this sample
                         // from a local file system (file://) - load this site via HTTP server if you enable MathType
                         'MathType'
                     ]
                 })
             };
         </script>
    <style type="text/css">
        .mydropdownlist {
            float: left;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />
    <br />

            <table>
                <tr>
                    <th><i class="fa fa-star"></i>活動名稱</th>
                    <td colspan="3">
                        <%--<asp:Label ID="lbl_Title" runat="server" Text="最多50字元" Font-Size="Medium"></asp:Label>--%>
                        <br />
                        <asp:TextBox ID="txt_Title" runat="server" placeholder="最多50字元" Height="35px" class="w10"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv_Title" runat="server" ErrorMessage="活動名稱不得為空" ControlToValidate="txt_Title" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>適用人員</th>
                    <td colspan="3">
                        <asp:CheckBoxList ID="cb_Role" class="required" runat="server"  RepeatColumns="4" DataTextField="RoleName" DataValueField="RoleSNO" RepeatLayout="Table" OnSelectedIndexChanged="cb_Role_SelectedIndexChanged" />
                    </td>
                </tr>

                <tr>
                    <th><i class="fa fa-star"></i>活動分類</th>
                    <td colspan="3">
                          <asp:DropDownList ID="ddl_EventClass" runat="server"  DataTextField="ClassName" DataValueField="EventCSNO">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddl_EventRole" runat="server"  DataTextField="ERName" DataValueField="ERSNO">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddl_Class1" runat="server"   CssClass="mydropdownlist" DataTextField="MVal" DataValueField="PVal">
                        </asp:DropDownList>
                       

                        <asp:DropDownList ID="ddl_Class2" runat="server"  Enable="false" CssClass="mydropdownlist" DataTextField="MVal" DataValueField="PVal" >
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddl_Class2" ErrorMessage="內容不得為空" ForeColor="Red"></asp:RequiredFieldValidator>

                    </td>
                </tr>

                <tr>
                    <th><i class="fa fa-star"></i>內容</th>
                    <td colspan="3">
                       <textarea name="editor1" id="editor1" rows="50" cols="80" runat="server"></textarea>
                        <asp:RequiredFieldValidator ID="rfv_Info" runat="server" ControlToValidate="editor1" ErrorMessage="內容不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                       
                <tr>
                    <th><i class="fa fa-star"></i>可報名人數</th>
                    <td>
                        <asp:Label ID="lb_CountLimit" runat="server" Text="如有填寫，則限制，如填寫0，則無限制人數。" Font-Size="Smaller"></asp:Label>
                        <br />
                        <asp:TextBox ID="txt_CountLimit" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="rev_CL" runat="server" ControlToValidate="txt_CountLimit" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txt_CountLimit" ErrorMessage="內容不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    <th><i class="fa fa-star"></i>錄取人數</th>
                    <td>
                        <asp:TextBox ID="txt_CountAdmit" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv_CA" runat="server" ControlToValidate="txt_CountAdmit" ErrorMessage="錄取人數不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="rev_CA" runat="server" ControlToValidate="txt_CountAdmit" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>認定時數</th>
                    <td>
                        <asp:TextBox ID="txt_TargetHour" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txt_TargetHour" ErrorMessage="認定時數不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ControlToValidate="txt_TargetHour" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>

                    </td>
                    <th><i class="fa fa-star"></i>預防及延緩證書類別</th>
                    <td>
                        <asp:TextBox ID="txt_QTypeName" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_QTypeName" ErrorMessage="預防及延緩證證書類別不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>活動費用</th>
                    <td>
                        <asp:TextBox ID="txt_ActiveCost" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator20" runat="server" ControlToValidate="txt_ActiveCost" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>
                    </td>
                    <th>提供膳食</th>
                    <td>
                         <asp:CheckBox ID="chk_Meals" runat="server" Text="是" />
                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txt_ActiveCost" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>--%>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>報名期間(起)</th>
                    <td>
                        <asp:Label ID="Label12" runat="server" Text="格式範例:2017-01-01" Font-Size="Smaller"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txt_SignS_F" ErrorMessage="開始日期不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server" ControlToValidate="txt_SignS_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}"></asp:RegularExpressionValidator>
                        <br />
                        <asp:TextBox ID="txt_SignS_F" runat="server"></asp:TextBox>
                    </td>
                    <th><i class="fa fa-star"></i>報名期間(迄)</th>
                    <td>
                        <asp:Label ID="Label13" runat="server" Text="格式範例:2017-12-31" Font-Size="Smaller"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="結束日期不得為空" ControlToValidate="txt_SignE_F" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator17" runat="server" ControlToValidate="txt_SignE_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}"></asp:RegularExpressionValidator>


                        <br />
                        <asp:TextBox ID="txt_SignE_F" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>主辦單位</th>
                    <td>
                        <asp:TextBox ID="txt_Host" runat="server" placeholder="最多50字元"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ControlToValidate="txt_Host" ErrorMessage="主辦單位不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    <th><i class="fa fa-star"></i>聯絡人</th>
                    <td>
                        <asp:TextBox ID="txt_CPerosn" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_CPerosn" ErrorMessage="聯絡人不得為空" ForeColor="Red"></asp:RequiredFieldValidator>

                    </td>

                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>連絡電話</th>
                    <td>
                        <asp:TextBox ID="txt_contact_C" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_contact_C" ErrorMessage="連絡電話不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                       
                    </td>
                    <th><i class="fa fa-star"></i>聯絡Email</th>
                    <td>
                        <asp:TextBox ID="txt_contact_mail" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_contact_mail" ErrorMessage="聯絡Email不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txt_contact_mail" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    </td>
                </tr>


                <tr>
                    <th><i class="fa fa-star"></i>聯絡地址</th>
                    <td>
                        <asp:DropDownList ID="ddl_codeAreaA_Address" runat="server" CssClass="mydropdownlist" OnSelectedIndexChanged="ddl_codeAreaA_Address_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                        </asp:DropDownList>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_codeAreaB_Address" runat="server" CssClass="mydropdownlist" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                                    <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_codeAreaA_Address" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:TextBox ID="txt_CAddress" runat="server" CssClass="w5"></asp:TextBox>
                        <div style="display:-webkit-inline-box">
                            <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="ddl_codeAreaA_Address" InitialValue="" ErrorMessage="縣市不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddl_codeAreaB_Address" InitialValue="" ErrorMessage="市區不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txt_CAddress" ErrorMessage="聯絡地址不得為空" ForeColor="Red"></asp:RequiredFieldValidator>

                        </div>
                    </td>
                    <th>開放前台查詢</th>
                    <td>
                        <asp:CheckBox ID="Chk_Enable" runat="server" Text="是" />
                    </td>
                </tr>


            </table>
 
    <div id="tabs" style="margin-bottom: 10px;">
        <ul>
            <li><a href="#tabs-1">課程資訊一</a></li>
            <li><a href="#tabs-2">課程資訊二</a></li>
            <li><a href="#tabs-3">課程資訊三</a></li>
        </ul>
        <div id="tabs-1">
            <table>
                <tr>
                    <th><i class="fa fa-star"></i>上課日期起</th>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="格式範例:2017-01-01" Font-Size="Smaller"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_CStartDay_F" ErrorMessage="上課日期不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt_CStartDay_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}"></asp:RegularExpressionValidator>
                        <br />
                        <asp:TextBox ID="txt_CStartDay_F" runat="server"></asp:TextBox>
                    </td>
                    <th><i class="fa fa-star"></i>上課日期迄</th>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="格式範例:2017-01-01" Font-Size="Smaller"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="結束日期不得為空" ControlToValidate="txt_CEndDay_F" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txt_CEndDay_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}"></asp:RegularExpressionValidator>
                        <br />
                        <asp:TextBox ID="txt_CEndDay_F" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>上課時間起</th>
                    <td>
                        <asp:Label ID="Label10" runat="server" Text="格式範例:01:01" Font-Size="Smaller"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txt_CStratTime_F" ErrorMessage="開始日期不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server" ControlToValidate="txt_CStratTime_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$"></asp:RegularExpressionValidator>
                        <br />
                        <asp:TextBox ID="txt_CStratTime_F" runat="server"></asp:TextBox>
                    </td>
                    <th><i class="fa fa-star"></i>上課時間迄</th>
                    <td>
                        <asp:Label ID="Label11" runat="server" Text="格式範例:23:59" Font-Size="Smaller"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="結束日期不得為空" ControlToValidate="txt_CEndTime_F" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server" ControlToValidate="txt_CEndTime_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$"></asp:RegularExpressionValidator>


                        <br />
                        <asp:TextBox ID="txt_CEndTime_F" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <th><i class="fa fa-star"></i>上課時數</th>
                    <td>
                        <asp:TextBox ID="txt_CHour_F" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txt_CHour_F" ErrorMessage="上課時數不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator18" runat="server" ControlToValidate="txt_CHour_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>


                    </td>
                    <th><i class="fa fa-star"></i>上課節數</th>
                    <td>
                        <asp:TextBox ID="txt_CCount_F" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="上課節數不得為空" ControlToValidate="txt_CCount_F" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator19" runat="server" ControlToValidate="txt_CCount_F" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>



                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>活動地區</th>
                    <td colspan="3">

                        <asp:DropDownList ID="ddl_AreaCodeA_F" runat="server" CssClass="mydropdownlist" OnSelectedIndexChanged="ddl_AreaCodeA_F_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                        </asp:DropDownList>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_AreaCodeB_F" runat="server" CssClass="mydropdownlist" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                                    <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_AreaCodeA_F" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:TextBox ID="txt_ActiveArea_F" runat="server" CssClass="w5"></asp:TextBox>
                       <div style="display:-webkit-inline-box">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddl_AreaCodeA_F" InitialValue="" ErrorMessage="縣市不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddl_AreaCodeB_F" InitialValue="" ErrorMessage="市區不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txt_ActiveArea_F" ErrorMessage="聯絡地址不得為空" ForeColor="Red"></asp:RequiredFieldValidator>

                        </div>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>活動地點</th>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_codeAreaA_Active_F" runat="server" CssClass="mydropdownlist" OnSelectedIndexChanged="ddl_codeAreaA_Active_F_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                        </asp:DropDownList>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_codeAreaB_Active_F" runat="server" CssClass="mydropdownlist" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                                    <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_codeAreaA_Active_F" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:TextBox ID="txt_codeAreaA_Active_F" runat="server" CssClass="w5"></asp:TextBox>
                       <div style="display:-webkit-inline-box">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddl_codeAreaA_Active_F" InitialValue="" ErrorMessage="縣市不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="ddl_codeAreaB_Active_F" InitialValue="" ErrorMessage="市區不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="txt_codeAreaA_Active_F" ErrorMessage="聯絡地址不得為空" ForeColor="Red"></asp:RequiredFieldValidator>

                        </div>
                    </td>
                </tr>
            </table>

        </div>
        <div id="tabs-2">
            <table>
                <tr>
                    <th>上課日期起</th>
                    <td>
                        <asp:Label ID="lbl_SDate" runat="server" Text="格式範例:2017-01-01" Font-Size="Smaller"></asp:Label><br />
                        <asp:TextBox ID="txt_CStartDay_S" runat="server"></asp:TextBox>
                    </td>
                    <th>上課日期迄</th>
                    <td>
                        <asp:Label ID="lbl_EDate" runat="server" Text="格式範例:2017-01-01" Font-Size="Smaller"></asp:Label><br />
                        <asp:TextBox ID="txt_CEndDay_S" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>上課時間起</th>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="格式範例:01:01" Font-Size="Smaller"></asp:Label><br />
                        <asp:TextBox ID="txt_CStratTime_S" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txt_CStratTime_S" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$"></asp:RegularExpressionValidator>
                    </td>
                    <th>上課時間迄</th>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="格式範例:23:59" Font-Size="Smaller"></asp:Label><br />
                        <asp:TextBox ID="txt_CEndTime_S" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ControlToValidate="txt_CEndTime_S" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <th>上課時數</th>
                    <td>
                        <asp:TextBox ID="txt_CHour_S" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_CHour_S" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>
                    </td>
                    <th>上課節數</th>
                    <td>
                        <asp:TextBox ID="txt_CCount_S" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txt_CCount_S" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>

                    </td>
                </tr>
                <tr>
                    <th>活動地區</th>
                    <td colspan="3">

                        <asp:DropDownList ID="ddl_AreaCodeA_S" runat="server" CssClass="mydropdownlist" OnSelectedIndexChanged="ddl_AreaCodeA_S_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                        </asp:DropDownList>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_AreaCodeB_S" runat="server" CssClass="mydropdownlist" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                                    <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_AreaCodeA_S" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:TextBox ID="txt_ActiveArea_S" runat="server" CssClass="w5"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>活動地點</th>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_codeAreaA_Active_S" runat="server" CssClass="mydropdownlist" OnSelectedIndexChanged="ddl_codeAreaA_Active_S_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                        </asp:DropDownList>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_codeAreaB_Active_S" runat="server" CssClass="mydropdownlist" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                                    <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_codeAreaA_Active_S" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:TextBox ID="txt_codeAreaA_Active_S" runat="server" CssClass="w5"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div id="tabs-3">
            <table>
                <tr>
                    <th>上課日期起</th>
                    <td>
                        <asp:Label ID="Label16" runat="server" Text="格式範例:2017-01-01" Font-Size="Smaller"></asp:Label><br />
                        <asp:TextBox ID="txt_CStartDay_T" runat="server"></asp:TextBox>
                    </td>
                    <th>上課日期迄</th>
                    <td>
                        <asp:Label ID="Label17" runat="server" Text="格式範例:2017-01-01" Font-Size="Smaller"></asp:Label><br />
                        <asp:TextBox ID="txt_CEndDay_T" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>上課時間起</th>
                    <td>
                        <asp:Label ID="Label18" runat="server" Text="格式範例:01:01" Font-Size="Smaller"></asp:Label><br />
                        <asp:TextBox ID="txt_CStartTime_T" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ControlToValidate="txt_CStartTime_T" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$"></asp:RegularExpressionValidator>

                    </td>
                    <th>上課時間迄</th>
                    <td>
                        <asp:Label ID="Label19" runat="server" Text="格式範例:23:59" Font-Size="Smaller"></asp:Label><br />
                        <asp:TextBox ID="txt_CEndTime_T" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="txt_CEndTime_T" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$"></asp:RegularExpressionValidator>

                    </td>
                </tr>
                <tr>
                    <th>上課時數</th>
                    <td>
                        <asp:TextBox ID="txt_CHour_T" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txt_CHour_T" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>
                    </td>
                    <th>上課節數</th>
                    <td>
                        <asp:TextBox ID="txt_CCount_T" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txt_CCount_T" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="\d*"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <th>活動地區</th>
                    <td colspan="3">

                        <asp:DropDownList ID="ddl_AreaCodeA_T" runat="server" CssClass="mydropdownlist" OnSelectedIndexChanged="ddl_AreaCodeA_T_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                        </asp:DropDownList>
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_AreaCodeB_T" runat="server" CssClass="mydropdownlist" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                                    <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_AreaCodeA_T" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:TextBox ID="txt_ActiveArea_T" runat="server" CssClass="w5"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <th>活動地點</th>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_codeAreaA_Active_T" runat="server" CssClass="mydropdownlist" OnSelectedIndexChanged="ddl_codeAreaA_Active_T_SelectedIndexChanged" AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                        </asp:DropDownList>
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_codeAreaB_Active_T" runat="server" CssClass="mydropdownlist" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                                    <asp:ListItem Text="請選擇" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_codeAreaA_Active_T" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:TextBox ID="txt_codeAreaA_Active_T" runat="server" CssClass="w5"></asp:TextBox>

                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="修改" OnClick="btnOK_Click" />
        <input name="btnCancel" type="button" value="取消" onclick="Back();" />
    </div>
          <script>
              // This sample still does not showcase all CKEditor 5 features (!)
              // Visit https://ckeditor.com/docs/ckeditor5/latest/features/index.html to browse all the features.
              CKEDITOR.ClassicEditor.create(document.getElementById('<%=editor1.ClientID %>'), {
                  // https://ckeditor.com/docs/ckeditor5/latest/features/toolbar/toolbar.html#extended-toolbar-configuration-format
                  toolbar: {
                      items: ['ckfinder', '|',
                          'exportPDF', 'exportWord', '|',
                          'findAndReplace', 'selectAll', '|',
                          'heading', '|',
                          'bold', 'italic', 'strikethrough', 'underline', 'code', 'subscript', 'superscript', 'removeFormat', '|',
                          'bulletedList', 'numberedList', 'todoList', '|',
                          'outdent', 'indent', '|',
                          'undo', 'redo',
                          '-',
                          'fontSize', 'fontFamily', 'fontColor', 'fontBackgroundColor', 'highlight', '|',
                          'alignment', '|',
                          'link', 'insertImage', 'blockQuote', 'insertTable', 'mediaEmbed', 'codeBlock', 'htmlEmbed', '|',
                          'specialCharacters', 'horizontalLine', 'pageBreak', '|',
                          'textPartLanguage', '|',
                          'sourceEditing'
                      ],
                      shouldNotGroupWhenFull: true
                  },
                  // Changing the language of the interface requires loading the language file using the <script> tag.
                  language: 'tw',
                  list: {
                      properties: {
                          styles: true,
                          startIndex: true,
                          reversed: true
                      }
                  },
                  // https://ckeditor.com/docs/ckeditor5/latest/features/headings.html#configuration
                  heading: {
                      options: [
                          { model: 'paragraph', title: 'Paragraph', class: 'ck-heading_paragraph' },
                          { model: 'heading1', view: 'h1', title: 'Heading 1', class: 'ck-heading_heading1' },
                          { model: 'heading2', view: 'h2', title: 'Heading 2', class: 'ck-heading_heading2' },
                          { model: 'heading3', view: 'h3', title: 'Heading 3', class: 'ck-heading_heading3' },
                          { model: 'heading4', view: 'h4', title: 'Heading 4', class: 'ck-heading_heading4' },
                          { model: 'heading5', view: 'h5', title: 'Heading 5', class: 'ck-heading_heading5' },
                          { model: 'heading6', view: 'h6', title: 'Heading 6', class: 'ck-heading_heading6' }
                      ]
                  },
                  // https://ckeditor.com/docs/ckeditor5/latest/features/editor-placeholder.html#using-the-editor-configuration
                  placeholder: 'Welcome to CKEditor 5!',
                  // https://ckeditor.com/docs/ckeditor5/latest/features/font.html#configuring-the-font-family-feature
                  fontFamily: {
                      options: [
                          'default',
                          'Arial, Helvetica, sans-serif',
                          'Courier New, Courier, monospace',
                          'Georgia, serif',
                          'Lucida Sans Unicode, Lucida Grande, sans-serif',
                          'Tahoma, Geneva, sans-serif',
                          'Times New Roman, Times, serif',
                          'Trebuchet MS, Helvetica, sans-serif',
                          'Verdana, Geneva, sans-serif'
                      ],
                      supportAllValues: true
                  },
                  // https://ckeditor.com/docs/ckeditor5/latest/features/font.html#configuring-the-font-size-feature
                  fontSize: {
                      options: [10, 12, 14, 'default', 18, 20, 22],
                      supportAllValues: true
                  },
                  // Be careful with the setting below. It instructs CKEditor to accept ALL HTML markup.
                  // https://ckeditor.com/docs/ckeditor5/latest/features/general-html-support.html#enabling-all-html-features
                  htmlSupport: {
                      allow: [
                          {
                              name: /.*/,
                              attributes: true,
                              classes: true,
                              styles: true
                          }
                      ]
                  },
                  // Be careful with enabling previews
                  // https://ckeditor.com/docs/ckeditor5/latest/features/html-embed.html#content-previews
                  htmlEmbed: {
                      showPreviews: true
                  },
                  // https://ckeditor.com/docs/ckeditor5/latest/features/link.html#custom-link-attributes-decorators
                  link: {
                      decorators: {
                          addTargetToExternalLinks: true,
                          defaultProtocol: 'https://',
                          toggleDownloadable: {
                              mode: 'manual',
                              label: 'Downloadable',
                              attributes: {
                                  download: 'file'
                              }
                          }
                      }
                  },
                  // https://ckeditor.com/docs/ckeditor5/latest/features/mentions.html#configuration
                  mention: {
                      feeds: [
                          {
                              marker: '@',
                              feed: [
                                  '@apple', '@bears', '@brownie', '@cake', '@cake', '@candy', '@canes', '@chocolate', '@cookie', '@cotton', '@cream',
                                  '@cupcake', '@danish', '@donut', '@dragée', '@fruitcake', '@gingerbread', '@gummi', '@ice', '@jelly-o',
                                  '@liquorice', '@macaroon', '@marzipan', '@oat', '@pie', '@plum', '@pudding', '@sesame', '@snaps', '@soufflé',
                                  '@sugar', '@sweet', '@topping', '@wafer'
                              ],
                              minimumCharacters: 1
                          }
                      ]
                  },
                  // The "super-build" contains more premium features that require additional configuration, disable them below.
                  // Do not turn them on unless you read the documentation and know how to configure them and setup the editor.
                  removePlugins: [
                      // These two are commercial, but you can try them out without registering to a trial.
                      // 'ExportPdf',
                      // 'ExportWord',



                      // This sample uses the Base64UploadAdapter to handle image uploads as it requires no configuration.
                      // https://ckeditor.com/docs/ckeditor5/latest/features/images/image-upload/base64-upload-adapter.html
                      // Storing images as Base64 is usually a very bad idea.
                      // Replace it on production website with other solutions:
                      // https://ckeditor.com/docs/ckeditor5/latest/features/images/image-upload/image-upload.html
                      // 'Base64UploadAdapter',
                      'RealTimeCollaborativeComments',
                      'RealTimeCollaborativeTrackChanges',
                      'RealTimeCollaborativeRevisionHistory',
                      'PresenceList',
                      'Comments',
                      'TrackChanges',
                      'TrackChangesData',
                      'RevisionHistory',
                      'Pagination',
                      'WProofreader',
                      // Careful, with the Mathtype plugin CKEditor will not load when loading this sample
                      // from a local file system (file://) - load this site via HTTP server if you enable MathType
                      'MathType'
                  ]
              });
          </script>
    <script type="text/javascript">
        function Back() {
            history.back()
        }
    </script>
</asp:Content>

