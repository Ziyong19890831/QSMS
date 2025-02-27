<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="CertificateAudit.aspx.cs" Inherits="Mgt_CertificateAudit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="path txtS mb20">現在位置：<a href="#">證書課程管理</a> <i class="fa fa-angle-right"></i><a href="CertificateAudit.aspx">證書審核</a></div>

    <div class="both mb20">
        <fieldset>
            <legend>功能列</legend>
            <fieldset>
                <legend>單一查詢</legend>
                姓名<asp:TextBox ID="txt_PName" runat="server"></asp:TextBox>
                <br />
                課程規劃名稱<asp:TextBox ID="txt_PlanName" runat="server" class="w2"></asp:TextBox>
                <br />
                身分證<asp:TextBox ID="txt_PersonID" runat="server" class="w2"></asp:TextBox>
                <br />
                證書名稱<asp:DropDownList ID="ddl_CType" runat="server" DataValueField="CTypeSNO" DataTextField="CTypeName"></asp:DropDownList>
                <div class="right">
                    <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
                </div>
            </fieldset>
            <fieldset>
                <legend>批次上傳查詢</legend>
                <div class="left w8">
                    Excel檔案:       
                <asp:FileUpload ID="file_Upload" runat="server" />
                </div>
                <div class="right">
                    <asp:Button ID="btnDownload" runat="server" Text="下載格式" OnClick="btnDownload_Click" />
                    <asp:Button ID="btnUpload" runat="server" Text="上傳&查詢" OnClick="btnUpload_Click" />
                </div>
            </fieldset>
            <div class="right">

                <asp:Button ID="btnGrant" runat="server" Text="批次授予" OnClick="btnGrant_Click" />
            </div>
        </fieldset>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lb_PerrsonID" runat="server" Visible="false"></asp:Label>
            <fieldset id="ProblemList" runat="server">
                <legend>問題名單</legend>
                <asp:GridView ID="gv_NotInExcel" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="PersonID" HeaderText="身分證" />
                        <asp:BoundField DataField="stutas" HeaderText="狀態" />
                    </Columns>
                </asp:GridView>
            </fieldset>
            <fieldset id="NoproblenList" runat="server">
                <legend>正確名單</legend>
                <asp:GridView ID="gv_Course" runat="server" AutoGenerateColumns="False"  OnRowCommand="gv_Course_RowCommand" OnRowCreated="gv_Course_RowCreated" OnRowDataBound="gv_Course_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="*" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Wrap="False" />
                            <ItemStyle Wrap="False" />
                            <HeaderTemplate>
                                <asp:CheckBox ID="CheckAllItem" runat="server" onclick="javascript: CheckAllItem(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox" AutoCallBack="true" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="Sort" HeaderText="排序" ItemStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>--%>
                        <asp:BoundField HeaderText="學員名稱" DataField="PName" />
                        <asp:BoundField HeaderText="身分證" DataField="PersonID" />
                        <asp:BoundField HeaderText="CtypeSNO" DataField="CtypeSNO" />
                        <%--<asp:BoundField HeaderText="CunitSNO" DataField="CunitSNO" />--%>
                        <asp:BoundField HeaderText="PersonSNO" DataField="PersonSNO" />
                        <%--<asp:BoundField HeaderText="完成的課程規劃" DataField="PlanName" />--%>
                        <%--<asp:BoundField HeaderText="適用起迄年度" DataField="CYear" />--%>
                        <asp:BoundField HeaderText="對應證書" DataField="CTypeName" />

                        <%--<asp:BoundField HeaderText="Sort" DataField="Sort" />--%>

                        <%--<asp:TemplateField HeaderText="積分紀錄" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <% if (userInfo.AdminIsUpdate == true)
                                    { %>
                                <a href="#" onclick="var winvar = window.open('./CertificateAudit_AE.aspx?a=<%=Request.QueryString["a"] %>&n=NN&sno=<%#Eval("PersonSNO").ToString() %>&pno=<%#Eval("PClassSNO").ToString() %>','winname','width=1200 height=550 location=no,menubar=no status=no,toolbar=no');"><i class="fa fa-pen-square"></i></a>
                                <% }
                                    else
                                    { %>
                       無權限
                    <% } %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="PClassSNO" DataField="PClassSNO" />--%>
                    </Columns>
                </asp:GridView>
            </fieldset>
                <asp:Panel ID="page_Panel" runat="server">
        <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
        <asp:HiddenField ID="txt_Page" runat="server" />
        <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
    </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
 
    <asp:Label ID="lb_NoOne" runat="server" ForeColor="Red"></asp:Label>
    <script type="text/javascript">	
        function CheckAllItem(Check) {
            elm = document.forms[0];  //取得form表單
            for (i = 0; i < elm.length; i++) {
                if (elm[i].type == "checkbox" && elm[i].id != Check.id) //若為checkbox，並且ID與表頭CheckBox不同。表示為明細的CheckBox
                {
                    if (elm.elements[i].checked != Check.checked)  //若明細的CheckBox的checked狀態與表頭CheckBox不同
                    {
                        elm.elements[i].click();  //明細的CheckBox執行click        
                    }
                }
            }
        }
    </script>
</asp:Content>

