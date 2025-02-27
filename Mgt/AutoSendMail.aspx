<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="AutoSendMail.aspx.cs" Inherits="Mgt_AutoSendMail" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type="text/javascript" src="../ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        function processData() {
            // getting data
            var data = CKEDITOR.instances.<%=editor_Mail.ClientID%>.getData()
            alert(data);
        }
        $(function () {
            $("#<%=txt_CertEndDate_S.ClientID%>").datepicker({
                  dateFormat: 'yy-mm-dd'
              });
        });
        $(function () {
            $("#<%=txt_CertEndDate_E.ClientID%>").datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });
        function ValidateModuleList(source, args) {
            var chkListModules = document.getElementById('<%= Chk_Role.ClientID %>');
            var chkListinputs = chkListModules.getElementsByTagName("input");
            for (var i = 0; i < chkListinputs.length; i++) {
                if (chkListinputs[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }
    </script>
        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="Note_content" class="ContentDiv">
        <asp:HiddenField ID="hf_Title" runat="server" />
        <asp:HiddenField ID="hf_EventSNO" runat="server" />
        <h1><i class="fa fa-at" aria-hidden="true"></i>課程資訊通知</h1>
        <fieldset>
            <legend>搜尋條件一</legend>
            標題：<asp:DropDownList ID="ddl_EventTitle" runat="server" DataTextField="EventName" DataValueField="EventSNO" OnSelectedIndexChanged="ddl_EventTitle_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="ddl_EventTitle" InitialValue="" ErrorMessage="請選填標題" ForeColor="Red" />
        </fieldset>
        <fieldset>
            <legend>搜尋條件二</legend>
            <fieldset>
                <legend>身份類型</legend>
                 身份別：<asp:CheckBoxList ID="Chk_Role" runat="server" RepeatColumns="4" DataTextField="RoleName" DataValueField="RoleSNO"></asp:CheckBoxList>
                <asp:CustomValidator runat="server" ID="cvmodulelist"  ClientValidationFunction="ValidateModuleList"  ErrorMessage="請選填身份別" ForeColor="Red" ></asp:CustomValidator>
            </fieldset>
           <fieldset>
               <legend>證書類型</legend>
                證書：<asp:RadioButtonList ID="chk_Certificate" runat="server"  RepeatColumns="2" AutoPostBack="true" OnSelectedIndexChanged="chk_Certificate_SelectedIndexChanged">

                <asp:ListItem Text="未取得" Selected="True" Value="0"></asp:ListItem>
                <asp:ListItem Text="已取得" Value="1"></asp:ListItem>
            </asp:RadioButtonList>        
            證書名稱：<asp:DropDownList ID="ddl_Certificate" runat="server" DataTextField="CtypeName" DataValueField="CtypeSNO" AutoPostBack="true" OnSelectedIndexChanged="ddl_Certificate_SelectedIndexChanged"></asp:DropDownList>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddl_Certificate" InitialValue="" ErrorMessage="請選填證書名稱" ForeColor="Red" />
               包含同類型證書：<asp:CheckBox ID="chk_Type" runat="server" /><br />
            到期日(起)：<asp:Textbox ID="txt_CertEndDate_S" runat="server"></asp:Textbox>～到期日(迄)：<asp:Textbox ID="txt_CertEndDate_E" runat="server"></asp:Textbox>
           </fieldset>
           
            <asp:Label ID="lb_Search" runat="server" Text="進階搜尋："></asp:Label> <asp:CheckBox ID="chk_Search" runat="server" AutoPostBack="true" OnCheckedChanged="chk_Search_CheckedChanged" />
            <asp:Panel ID="P1" runat="server" Visible="false">
               無證書也無積分：<asp:CheckBox ID="chk_NotGetIntegral" runat="server" AutoPostBack="true"  OnCheckedChanged="chk_NotGetIntegral_CheckedChanged"/><br />
                課程規劃：<asp:DropDownList ID="ddl_CoursePlanningClass" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_CoursePlanningClass_SelectedIndexChanged"></asp:DropDownList><br />
                核心線上積分：<asp:DropDownList ID="ddl_OnlineIntegral" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_OnlineIntegral_SelectedIndexChanged">
                    <asp:ListItem Text="請選擇" Value="" Selected="True" ></asp:ListItem>
                    <asp:ListItem Text="已滿" Value="0"></asp:ListItem>
                     <asp:ListItem Text="未滿" Value="1"></asp:ListItem>
                       </asp:DropDownList><br />
                 核心實體積分：<asp:DropDownList ID="ddl_Entity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_Entity_SelectedIndexChanged">
                     <asp:ListItem Text="請選擇" Value="" Selected="True" ></asp:ListItem>
                    <asp:ListItem Text="已滿" Value="0"></asp:ListItem>
                     <asp:ListItem Text="未滿" Value="1"></asp:ListItem>
                       </asp:DropDownList>
            </asp:Panel>
        </fieldset>
        <textarea id="editor_Mail" name="editor_Mail" runat="server"></textarea>
        <div class="center" style="margin-top: 10px">
            <asp:Button ID="btnSendMail" runat="server" Text="寄送信件" OnClick="btnSendMail_Click" />
            <asp:Button ID="btn_Download" runat="server" Text="名單下載" OnClick="btn_Download_Click" />
        </div>

    </div>
        <script>
            // This sample still does not showcase all CKEditor 5 features (!)
            // Visit https://ckeditor.com/docs/ckeditor5/latest/features/index.html to browse all the features.
            CKEDITOR.ClassicEditor.create(document.getElementById('<%=editor_Mail.ClientID %>'), {
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
</asp:Content>

