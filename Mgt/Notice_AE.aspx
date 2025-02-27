<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Notice_AE.aspx.cs" Inherits="Mgt_Notice_AE" validateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#<%=txt_SDate.ClientID%>").datepicker({
              dateFormat: 'yy-mm-dd'
          });
      });
      $(function () {
          $("#<%=txt_EDate.ClientID%>").datepicker({
              dateFormat: 'yy-mm-dd'
          });
      });
      $.datepicker.setDefaults($.datepicker.regional["zh-TW"]);
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />

    <table>
        <tr>
            <th><i class="fa fa-star"></i>排序</th>
            <td colspan="3">
               <asp:DropDownList ID="ddl_OrderSeq" runat="server" DataTextField="OrderSeq">
                   <asp:ListItem Value="">無</asp:ListItem>
                   <asp:ListItem>1</asp:ListItem>
                   <asp:ListItem>2</asp:ListItem>
                   <asp:ListItem>3</asp:ListItem>
                   <asp:ListItem>4</asp:ListItem>
                   <asp:ListItem>5</asp:ListItem>
                   <asp:ListItem>6</asp:ListItem>
                   <asp:ListItem>7</asp:ListItem>
                   <asp:ListItem>8</asp:ListItem>
                   <asp:ListItem>9</asp:ListItem>
                   <asp:ListItem>10</asp:ListItem>
                </asp:DropDownList>
                <span>如未選取則按[發布日期倒序排序]，如有選擇[排序]，則依[大到小排序]，再依[發布日期倒序排序]。</span>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>標題</th>
            <td colspan="3">
                <%--<asp:Label ID="lbl_Title" runat="server" Text="最多50字元" Font-Size="Medium"></asp:Label>--%>
                <br />
                <asp:TextBox ID="txt_Title" runat="server" class="w10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_Title" runat="server" ErrorMessage="標題不得為空" ControlToValidate="txt_Title" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>適用人員</th>
            <td colspan="3">
                  <asp:CheckBoxList ID="cb_Role" class="required" runat="server" RepeatColumns="3" DataTextField="RoleName" DataValueField="RoleSNO" RepeatLayout="Table" /></td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>類別</th>
            <td colspan="1">
                <asp:DropDownList ID="ddl_Class" runat="server" DataValueField="NoticeCSNO" DataTextField="Name">
                </asp:DropDownList></td>
            <th><i class="fa fa-star"></i>是否顯示</th>
            <td><asp:CheckBox ID="chk_view" runat="server"  Checked="true" /></td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>開始日期</th>
            <td>
                <asp:Label ID="lbl_SDate" runat="server" Text="格式範例:2017-01-01" Font-Size="Smaller"></asp:Label>
                <asp:RequiredFieldValidator ID="rfv_Sdate" runat="server" ControlToValidate="txt_SDate" ErrorMessage="開始日期不得為空" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="rev_SDate" runat="server" ControlToValidate="txt_SDate" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}"></asp:RegularExpressionValidator>
                <br />
                <asp:TextBox ID="txt_SDate" runat="server"></asp:TextBox>
            </td>
            <th><i class="fa fa-star"></i>結束日期</th>
            <td>
                <asp:Label ID="lbl_EDate" runat="server" Text="格式範例:2017-01-01" Font-Size="Smaller"></asp:Label>
                <asp:RequiredFieldValidator ID="rfv_EDate" runat="server" ErrorMessage="結束日期不得為空" ControlToValidate="txt_EDate" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="rev_EDate" runat="server" ControlToValidate="txt_EDate" ErrorMessage="格式錯誤" ForeColor="Red" ValidationExpression="[0-9]{4}-[0-9]{2}-[0-9]{2}"></asp:RegularExpressionValidator>
                <br />
                <asp:TextBox ID="txt_EDate" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>內容</th>
            <td colspan="3">
                <textarea name="editor1" id="editor1" rows="50" cols="80" runat="server">
                    請在此輸入您的內文
                </textarea>
                <%--<asp:Label ID="lbl_Info" runat="server" Text="最多4000字元"></asp:Label><asp:TextBox ID="txt_Info" runat="server" TextMode="MultiLine" class="w10" Rows="5"></asp:TextBox>--%>
                <%--<asp:RequiredFieldValidator ID="rfv_Info" runat="server" ControlToValidate="txt_Info" ErrorMessage="內容不得為空" ForeColor="Red"></asp:RequiredFieldValidator>--%>
            </td>
        </tr>
    </table>

    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="修改" OnClick="btnOK_Click"/>
        <input name="btnCancel" type="button" value="取消" onclick="location.href='Notice.aspx'" />
    </div>
     

 <%--  <script type="text/javascript">
       CKEDITOR.replace('<%=editor1.ClientID %>', { filebrowserBrowseUrl: 'FileBrower.aspx?type=File'/*, filebrowserUploadUrl: 'FileBrower.aspx?type=Files'*/ });
   </script>--%>

    

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
</asp:Content>

