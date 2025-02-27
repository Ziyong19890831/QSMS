<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PDDI.Web.master" AutoEventWireup="true" CodeFile="Toolkits.aspx.cs" Inherits="Web_Toolkits" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
    #ContentPlaceHolder1_btn_Link{
        float: right;
    }
    </style>
    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=txt_Page.ClientID%>").value = pageNumber;
        document.getElementById("<%=btnPage.ClientID%>").click();
    }
    function _goPage1(pageNumber) {
        document.getElementById("<%=txt_PageType.ClientID%>").value = pageNumber;
        document.getElementById("<%=btnPageType.ClientID%>").click();
        }
        function Link() {
            window.open('https://forms.gle/ADrCd7gYXoiWdqWi8', '_blank');
        };
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="/">首頁</a></li>
            <li class="breadcrumb-item active" aria-current="page">戒菸套裝教材</li>
        </ol>
    </nav>

    <%--<asp:Button ID="btn_Link" class="btn btn-primary" runat="server" OnClientClick="Link()" Text="滿意度問卷填寫" />--%>
    <div style="clear: both;"></div>
 
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <ul class="nav nav-tabs" role="tablist">
        <li class="nav-item">
            <a class="nav-link active" id="tabs-a-1" data-toggle="tab" href="#tabs-d-1" role="tab" aria-controls="tabs-d-1" aria-selected="true">期程</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="tabs-a-2" data-toggle="tab" href="#tabs-d-2" role="tab" aria-controls="tabs-d-2" aria-selected="false">科別</a>
        </li>
    </ul>
    <div class="tab-content">
        <div class="tab-pane fade show active mt10 tb30" id="tabs-d-1" role="tabpanel" aria-labelledby="tabs-a-1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <div class="alert alert-primary" role="alert">
                        <div class="row">
                            <div class="col-12">
                                <div class="form-row">
                                    <div class="form-group col-md-3">
                                        教材類型：
                                        <asp:DropDownList ID="ddl_stageClass" runat="server" class="form-control" DataTextField="MVal" DataValueField="Pval"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-md-3">
                                        期程：
                                        <asp:DropDownList ID="ddl_stage" runat="server" class="form-control" DataTextField="MVal" DataValueField="Pval"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-md-3">
                                        檔案名稱
                                        <asp:TextBox ID="txt_fileName" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-md-3">
                                        <br>
                                        <asp:Button ID="btn_search" runat="server" class="btn btn-info" OnClick="btn_search_Click" Text="查詢" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                    <h5 class="mt20"><i class="fa fa-calendar-alt"></i>戒菸套裝教材</h5>
                    <div class="col-12 table-responsive">
                        <asp:GridView ID="gv_toolkies" class="table table-striped" runat="server" AutoGenerateColumns="false" OnRowCreated="gv_toolkies_RowCreated" OnRowCommand="gv_toolkies_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="圖片預覽">
                                    <ItemTemplate>
                                        <a href='<%# Eval("TkURL_pic") %>' target="_blank">
                                            <img alt="無預覽" src='<%# Eval("TkURL_pic") %>' width="80px" height="60px" /></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="TkURL" DataField="TkURL" />
                                <asp:BoundField HeaderText="適用性" DataField="stageClassName" />
                                <asp:BoundField HeaderText="期別" DataField="stageName" />
                                <asp:BoundField HeaderText="檔案名稱" DataField="TkName" />
                                <asp:BoundField HeaderText="副檔名" DataField="Extension" />
                                <asp:BoundField HeaderText="上傳日期" DataField="上傳日期" />
                                <asp:TemplateField HeaderText="下載">
                                    <ItemTemplate>
                                        <asp:Button ID="btn_download" class="btn btn-dark" Text="下載" runat="server" CommandName="getdata"></asp:Button>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="TKSNO" DataField="TKSNO" />

                            </Columns>
                        </asp:GridView>
                    </div>
                    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
                    <asp:HiddenField ID="txt_Page" runat="server" />
                    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="gv_toolkies" />
                </Triggers>
            </asp:UpdatePanel>
            
        </div>

        <div class="tab-pane fade mt10 tb30" id="tabs-d-2" role="tabpanel" aria-labelledby="tabs-a-2">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>

                    <div class="alert alert-primary" role="alert">
                        <div class="row">
                            <div class="col-12">
                                <div class="form-row">
                                    <div class="form-group col-md-3">
                                        教材類型：
                                        <asp:DropDownList ID="ddl_TkStageType" class="form-control" runat="server" DataTextField="MVal" DataValueField="Pval"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-md-3">
                                        科別：
                                        <asp:DropDownList ID="ddl_TkType" class="form-control" runat="server" DataTextField="MVal" DataValueField="Pval"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-md-3">
                                        檔案名稱
                                        <asp:TextBox ID="txt_fileName_Type" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-md-3">
                                        <br>
                                        <asp:Button ID="btn_SeachType" runat="server" class="btn btn-info"  OnClick="btn_SeachType_Click" Text="查詢" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <h5 class="mt20"><i class="fa fa-calendar-alt"></i>套裝教材</h5>
                    <asp:GridView ID="gv_toolkiesType" class="table table-striped" runat="server" AutoGenerateColumns="false" OnRowCreated="gv_toolkiesType_RowCreated" OnRowDataBound="gv_toolkiesType_RowDataBound" OnRowCommand="gv_toolkiesType_RowCommand">
                        <Columns>
                                <asp:TemplateField HeaderText="圖片預覽">
                                <ItemTemplate>
                                    <a href='<%# Eval("TkURL_pic") %>' target="_blank"><img alt="無預覽" src='<%# Eval("TkURL_pic") %>' width="80px" height="60px" /></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="TkURL" DataField="TkURL" />
                            <asp:BoundField HeaderText="適用性" DataField="stageClassName" />              
                                <asp:BoundField HeaderText="科別" DataField="TktypeName" />
                            <asp:BoundField HeaderText="檔案名稱" DataField="TkName" />
                            <asp:BoundField HeaderText="副檔名" DataField="Extension" />
                            <asp:BoundField HeaderText="上傳日期" DataField="上傳日期" />
                            <asp:TemplateField HeaderText="下載">
                                <ItemTemplate>
                                    <asp:Button ID="btn_download" Text="下載" class="btn btn-info" runat="server" CommandName="getdata"></asp:Button>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Literal ID="ltl_PageNumberType" runat="server"></asp:Literal>
                    <asp:HiddenField ID="txt_PageType" runat="server" />
                    <asp:Button ID="btnPageType" runat="server" Text="查詢" OnClick="btnPageType_Click" Style="display: none;" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="gv_toolkiesType" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

    </div>

</asp:Content>

