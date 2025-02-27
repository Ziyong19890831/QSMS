<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="FeedBack_AE.aspx.cs" Inherits="Mgt_FeedBack_AE" ValidateRequest="False" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="path txtS mb20">現在位置：<a href="#">常用服務管理</a> <i class="fa fa-angle-right"></i><a href="FeedBack_AE.aspx">意見回饋</a></div>
     <fieldset>
         <legend>資訊列</legend> 
         <div class="left w8">
             填寫人姓名：<asp:Label ID="lb_QName" runat="server"></asp:Label>
             <br />
             填寫人Email：<asp:Label ID="lb_QEmail" runat="server"></asp:Label>
             <br />
             填寫內容：<asp:TextBox ID="txt_Qcontent" runat="server" Enabled="false" ReadOnly="true" TextMode="MultiLine" Height="70px" Width="1100"></asp:TextBox>
         </div>
         </fieldset>
    <hr />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>    
            <table >
                <tr class="w3">
                    <th><i class="fa fa-star"></i>回覆選項</th>
                    <td>
                        <asp:RadioButton ID="RB_FeedBack" Text="回覆該問題" runat="server" AutoPostBack="true"   Checked="true" GroupName="FeedBack"/>
                        <asp:RadioButton ID="RB_SendOther" Text="轉寄協會" runat="server" AutoPostBack="true"   GroupName="FeedBack"/>

                    </td>
                </tr>      
                <tr id="forward" runat="server" visible="false">
                    <th><i class="fa fa-star"></i>轉寄mail</th>
                    <td>
                        <asp:TextBox ID="txt_Forward" runat="server" Width="600px"></asp:TextBox>
                    </td>
                </tr>
                <tr >
                    <th><i class="fa fa-star"></i>回覆主題</th>
                    <td>
                        <asp:TextBox ID="txt_ReplyTheme" runat="server" Width="600px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><i class="fa fa-star"></i>回覆內容</th>
                    <td>
                        <textarea name="editor1" id="editor1" rows="50" cols="80" runat="server"></textarea>
                        <%--<asp:TextBox ID="txt_ReplyContent" runat="server" TextMode="MultiLine" Height="70px" Width="1000px" ></asp:TextBox>--%>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="center btns">
        <asp:Button ID="btn_Send" runat="server" Text="寄出" OnClick="btn_Send_Click"/>
        <input name="btnCancel" type="button" value="取消" onclick="window.close();"/>
    </div>
     <script type="text/jscript" src="https://cdn.ckeditor.com/ckeditor5/34.1.0/super-build/ckeditor.js"></script>
         <script type="text/javascript">
             // This sample still does not showcase all CKEditor 5 features (!)
             // Visit https://ckeditor.com/docs/ckeditor5/latest/features/index.html to browse all the features.
             CKEDITOR.ClassicEditor.create(document.getElementById("ContentPlaceHolder1_editor1"), {
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

