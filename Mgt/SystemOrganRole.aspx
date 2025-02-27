<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="SystemOrganRole.aspx.cs" Inherits="Mgt_SystemOrganRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function _goPage(pageNumber) {
            document.getElementById("<%=hidPage.ClientID%>").value = pageNumber;
            document.getElementById("<%=btnPage.ClientID%>").click();
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="path txtS mb20">
        現在位置：<a href="#">系統功能管理</a> <i class="fa fa-angle-right" aria-hidden="true"></i>
        <a href="#">單位角色管理</a>
    </div>
    <div class="both mb20">
        <div class="left mb20">
            單位代碼<asp:TextBox ID="txt_OrganCode" runat="server"></asp:TextBox>
            單位名稱<asp:TextBox ID="txt_OrganName" runat="server"></asp:TextBox>
            <asp:UpdatePanel ID="upl_ddl" runat="server" UpdateMode="Conditional" style="display: inline;">
                <ContentTemplate>
                    行政區：
                        <asp:DropDownList ID="ddl_AreaCodeA" runat="server" OnSelectedIndexChanged="ddl_AreaCodeA_SelectedIndexChanged"
                            AutoPostBack="true" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                        </asp:DropDownList>
                    <asp:DropDownList ID="ddl_AreaCodeB" runat="server" DataValueField="AREA_CODE" DataTextField="AREA_NAME">
                        <asp:ListItem Value="" Text="請先選擇縣市行政區"></asp:ListItem>
                    </asp:DropDownList>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddl_AreaCodeA" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="right">
            <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" Style="padding: 10px; font-size: 16px;" />
        </div>
    </div>
    <asp:GridView ID="gv_Role" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="OrganCode" HeaderText="單位代碼" />
            <asp:BoundField DataField="AreaName" HeaderText="行政區" />
            <asp:BoundField DataField="OrganName" HeaderText="單位名稱" />
            <asp:TemplateField HeaderText="修改" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <a href='./SystemOrganRole_AE.aspx?sno=<%#Eval("OrganSNO").ToString() %>&st=<%=Request.QueryString["st"] %>'>
                        <i class="fa fa-pen-square" aria-hidden="true"></i></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="單位選單預覽" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <div <%# Eval("M_COUNT").ToString() =="0" ? "style='display:none;'":""%>>
                        <a href='./SystemTree.aspx?sno=<%#Eval("OrganSNO").ToString() %>&st=<%=Request.QueryString["st"] %>'>
                            <i class="fa fa-pen-square" aria-hidden="true"></i></a>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
    <asp:HiddenField ID="hidPage" runat="server" />
    <asp:HiddenField ID="hidSystem" runat="server" />
    <asp:Button ID="btnPage" runat="server" Text="查詢" OnClick="btnPage_Click" Style="display: none;" />

</asp:Content>
