<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="EventRoleDetail.aspx.cs" Inherits="Mgt_EventRoleDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            $("#accordion").accordion();
        });
        $(function () {
            $("#tabsRole10").tabs();
            $("#tabsRole11").tabs();
            $("#tabsRole12").tabs();
            $("#tabsRole13").tabs();
        });
    </script>
    <style>
        th {
            width: 30%;
        }

        .ui-accordion-content {
            height: 540px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="sc" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />
    <div id="accordion">
        <h3>西醫師</h3>
        <div>
            <div id="tabsRole10" style="margin-bottom: 10px;">
                <ul>
                    <li><a href="#tabs-1">皆未取得</a></li>
                    <li><a href="#tabs-2">取得初階</a></li>
                    <li><a href="#tabs-3">取得進階</a></li>
                    <li><a href="#tabs-4">取得初進階</a></li>
                </ul>

                <div id="tabs-1">
                    <asp:UpdatePanel ID="UpdatePanel96" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <th>積分上傳之課程選擇</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_CoursePlan10" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_CoursePlan_SelectedIndexChanged"></asp:DropDownList>
                                         <asp:Button ID="btn_LookFor10Code" runat="server" Text="查看代號" OnClick="btn_LookFor10Code_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>上傳課程代號</th>
                                    <td>
                                        <asp:TextBox ID="txt_CourseSNO_10" runat="server"></asp:TextBox>
                                </tr>
                                <tr>
                                    <th>增加規則</th>
                                    <td>

                                        <asp:CheckBox ID="chk_None10P1" runat="server" OnCheckedChanged="chk_P1_None10CheckedChanged" Text="規則一" AutoPostBack="true" />
                                        <asp:CheckBox ID="chk_None10P2" runat="server" OnCheckedChanged="chk_P2_None10CheckedChanged" Text="規則二" AutoPostBack="true" />

                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_None10Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UP1" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_None10CoursePlaningClass" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_None10CoursePlaningClass_SelectedIndexChanged"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_None10Course" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_None10CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>

                                <asp:Panel ID="Role1" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Logic1" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_None10CoursePlaningClass1" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_None10CoursePlaningClass1_SelectedIndexChanged"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Course1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_None10CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Role2" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Logic2" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_None10CoursePlaningClass2" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_None10CoursePlaningClass2_SelectedIndexChanged"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Course2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_None10CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>


                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tabs-2">
                    <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                        <ContentTemplate>
                            <table>
                                 <tr>
                                    <th>積分上傳之課程選擇</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_CoursePlan10_1" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_CoursePlan_SelectedIndexChanged"></asp:DropDownList>
                                         <asp:Button ID="btn_LookFor10Code_1" runat="server" Text="查看代號" OnClick="btn_LookFor10Code_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>上傳課程代號</th>
                                    <td>
                                        <asp:TextBox ID="txt_CourseSNO_10_1" runat="server"></asp:TextBox>
                                </tr>
                                <tr>
                                    <th>增加規則</th>
                                    <td>
                                        <asp:CheckBox ID="chk_JuniorP3" runat="server" OnCheckedChanged="chk_JuniorP3_CheckedChanged" Text="規則一" AutoPostBack="true" />
                                        <asp:CheckBox ID="chk_JuniorP4" runat="server" OnCheckedChanged="chk_JuniorP4_CheckedChanged" Text="規則二" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_Junior10Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_Junior10CoursePlaningClass" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_Junior10CoursePlaningClass_SelectedIndexChanged"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_Junior10" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_Junior10CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <asp:Panel ID="Panel1" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList3" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Junior10Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Junior10CoursePlaningClass1" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_Junior10CoursePlaningClass1_SelectedIndexChanged"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Junior10_1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Junior10CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Panel2" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList6" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Junior10Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Junior10CoursePlaningClass2" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_Junior10Certificate2_SelectedIndexChanged"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Junior10_2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Junior10CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tabs-3">
                    <asp:UpdatePanel ID="UpdatePanel101" runat="server">
                        <ContentTemplate>
                            <table>

                                <tr>
                                    <th>增加規則</th>
                                    <td>
                                        <asp:CheckBox ID="chk_P5" runat="server" Text="規則一" AutoPostBack="true" OnCheckedChanged="chk_P5_CheckedChanged" />
                                        <asp:CheckBox ID="chk_P6" runat="server" Text="規則二" AutoPostBack="true" OnCheckedChanged="chk_P6_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_Senior10Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_Senior10CoursePlaningClass" OnSelectedIndexChanged="ddl_Senior10CoursePlaningClass_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_Sunior10" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_Senior10CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <asp:Panel ID="Panel3" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList11" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Senior10Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Senior10CoursePlaningClass1" OnSelectedIndexChanged="ddl_Senior10CoursePlaningClass1_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Sunior10_1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Senior10CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Panel4" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList14" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Senior10Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Senior10CoursePlaningClass2" OnSelectedIndexChanged="ddl_Senior10CoursePlaningClass2_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Sunior10_2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Senior10CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tabs-4">
                    <asp:UpdatePanel ID="UpdatePanel102" runat="server">
                        <ContentTemplate>
                            <table>

                                <tr>
                                    <th>增加規則</th>
                                    <td>
                                        <asp:CheckBox ID="chk_P7" runat="server" OnCheckedChanged="chk_P7_CheckedChanged" Text="規則一" AutoPostBack="true" />
                                        <asp:CheckBox ID="chk_P8" runat="server" OnCheckedChanged="chk_P8_CheckedChanged" Text="規則二" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_JuniorSenior10Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_JuniorSenior10CoursePlaningClass" OnSelectedIndexChanged="ddl_JuniorSenior10CoursePlaningClass_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_JuniorSenior10" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_JuniorSenior10CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <asp:Panel ID="Panel5" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList19" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_JuniorSenior10Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_JuniorSenior10CoursePlaningClass1" OnSelectedIndexChanged="ddl_JuniorSenior10CoursePlaningClass1_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_JuniorSenior10_1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_JuniorSenior10CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Panel6" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList22" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_JuniorSenior10Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_JuniorSenior10CoursePlaningClass2" OnSelectedIndexChanged="ddl_JuniorSenior10CoursePlaningClass2_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_JuniorSenior10_2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_JuniorSenior10CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <h3>牙醫師</h3>
        <div>
            <div id="tabsRole11" style="margin-bottom: 10px;">
                <ul>
                    <li><a href="#tabs-5">皆未取得</a></li>
                    <li><a href="#tabs-6">取得初階</a></li>
                    <li><a href="#tabs-7">取得進階</a></li>
                    <li><a href="#tabs-8">取得初進階</a></li>
                </ul>

                <div id="tabs-5">
                    <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <th>積分上傳之課程選擇</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_CoursePlan11" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_CoursePlan11_SelectedIndexChanged"></asp:DropDownList>
                                         <asp:Button ID="btn_LookFor11Code" OnClick="btn_LookFor11Code_Click" runat="server" Text="查看代號" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>上傳課程代號</th>
                                    <td>
                                        <asp:TextBox ID="txt_CourseSNO_11" runat="server"></asp:TextBox>
                                </tr>
                                <tr>
                                    <th>增加規則</th>
                                    <td>

                                        <asp:CheckBox ID="chk_P9" runat="server" OnCheckedChanged="chk_P9_CheckedChanged" Text="規則一" AutoPostBack="true" />
                                        <asp:CheckBox ID="chk_P10" runat="server" OnCheckedChanged="chk_P10_CheckedChanged" Text="規則二" AutoPostBack="true" />

                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_None11Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_None11CoursePlaningClass" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_None11CoursePlaningClass_SelectedIndexChanged"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_None11Course" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_None11CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>

                                <asp:Panel ID="Panel7" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList27" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_None11Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel27" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_None11CoursePlaningClass1" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_None11CoursePlaningClass1_SelectedIndexChanged"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel28" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_None11Course1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_None11CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Panel8" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList30" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_None11Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel29" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_None11CoursePlaningClass2" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_None11CoursePlaningClass2_SelectedIndexChanged"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel30" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_None11Course2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_None11CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>


                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tabs-6">
                    <asp:UpdatePanel ID="UpdatePanel103" runat="server">
                        <ContentTemplate>
                            <table>
                                 <tr>
                                    <th>積分上傳之課程選擇</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_CoursePlan11_1" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_CoursePlan_SelectedIndexChanged"></asp:DropDownList>
                                         <asp:Button ID="btn_LookFor11Code_1" runat="server" Text="查看代號" OnClick="btn_LookFor10Code_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>上傳課程代號</th>
                                    <td>
                                        <asp:TextBox ID="txt_CourseSNO_11_1" runat="server"></asp:TextBox>
                                </tr>
                                <tr>
                                    <th>增加規則</th>
                                    <td>
                                        <asp:CheckBox ID="chk_P11" runat="server" OnCheckedChanged="chk_P11_CheckedChanged" Text="規則一" AutoPostBack="true" />
                                        <asp:CheckBox ID="chk_P12" runat="server" OnCheckedChanged="chk_P12_CheckedChanged" Text="規則二" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_Junior11Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel31" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_Junior11CoursePlaningClass" OnSelectedIndexChanged="ddl_Junior11CoursePlaningClass_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel32" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_Junior11" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_Junior11CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <asp:Panel ID="Panel9" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList35" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Junior11Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel33" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Junior11CoursePlaningClass1" OnSelectedIndexChanged="ddl_Junior11CoursePlaningClass1_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel34" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Junior11_1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Junior11CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Panel10" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList38" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Junior11Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel35" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Junior11CoursePlaningClass2" OnSelectedIndexChanged="ddl_Junior11CoursePlaningClass2_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel36" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Junior11_2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Junior11CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tabs-7">
                    <asp:UpdatePanel ID="UpdatePanel104" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <th>增加規則</th>
                                    <td>
                                        <asp:CheckBox ID="chk_P13" OnCheckedChanged="chk_P13_CheckedChanged" runat="server" Text="規則一" AutoPostBack="true" />
                                        <asp:CheckBox ID="chk_P14" OnCheckedChanged="chk_P14_CheckedChanged" runat="server" Text="規則二" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_Senior11Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel37" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_Senior11CoursePlaningClass" OnSelectedIndexChanged="ddl_Senior11CoursePlaningClass_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel38" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_Sunior11" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_Senior11CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <asp:Panel ID="Panel11" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList43" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Senior11Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel39" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Senior11CoursePlaningClass1" OnSelectedIndexChanged="ddl_Senior11CoursePlaningClass1_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel40" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Sunior11_1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Senior11CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Panel12" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList46" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Senior11Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel41" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Senior11CoursePlaningClass2" OnSelectedIndexChanged="ddl_Senior11CoursePlaningClass2_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel42" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Sunior11_2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Senior11CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tabs-8">
                    <asp:UpdatePanel ID="UpdatePanel105" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <th>增加規則</th>
                                    <td>
                                        <asp:CheckBox ID="chk_P15" runat="server" OnCheckedChanged="chk_P15_CheckedChanged" Text="規則一" AutoPostBack="true" />
                                        <asp:CheckBox ID="chk_P16" runat="server" OnCheckedChanged="chk_P16_CheckedChanged" Text="規則二" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_JuniorSenior11Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel43" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_JuniorSenior11CoursePlaningClass" OnSelectedIndexChanged="ddl_JuniorSenior11CoursePlaningClass_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel44" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_JuniorSenior11" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_JuniorSenior11CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <asp:Panel ID="Panel13" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList51" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_JuniorSenior11Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel45" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_JuniorSenior11CoursePlaningClass1" OnSelectedIndexChanged="ddl_JuniorSenior11CoursePlaningClass1_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel46" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_JuniorSenior11_1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_None10CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Panel14" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList54" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_JuniorSenior11Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel47" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_JuniorSenior11CoursePlaningClass2" OnSelectedIndexChanged="ddl_JuniorSenior11CoursePlaningClass2_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel97" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_JuniorSenior11_2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_JuniorSenior11CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <h3>藥師</h3>
        <div>
            <div id="tabsRole12" style="margin-bottom: 10px;">
                <ul>
                    <li><a href="#tabs-9">皆未取得</a></li>
                    <li><a href="#tabs-10">取得初階</a></li>
                    <li><a href="#tabs-11">取得進階</a></li>
                    <li><a href="#tabs-12">取得初進階</a></li>
                </ul>
                <div id="tabs-9">
                    <asp:UpdatePanel ID="UpdatePanel48" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <th>積分上傳之課程選擇</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_CoursePlan12" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_CoursePlan12_SelectedIndexChanged"></asp:DropDownList>
                                         <asp:Button ID="btn_LookFor12Code" OnClick="btn_LookFor12Code_Click" runat="server" Text="查看代號" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>上傳課程代號</th>
                                    <td>
                                        <asp:TextBox ID="txt_CourseSNO_12" runat="server"></asp:TextBox>
                                </tr>
                                <tr>
                                    <th>增加規則</th>
                                    <td>

                                        <asp:CheckBox ID="chk_P17" runat="server" OnCheckedChanged="chk_P17_CheckedChanged" Text="規則一" AutoPostBack="true" />
                                        <asp:CheckBox ID="chk_P18" runat="server" OnCheckedChanged="chk_P18_CheckedChanged" Text="規則二" AutoPostBack="true" />

                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_None12Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel49" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_None12CoursePlaningClass" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_None12CoursePlaningClass_SelectedIndexChanged"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel50" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_None12Course" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_None12CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>

                                <asp:Panel ID="Panel15" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList59" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_None12Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel51" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_None12CoursePlaningClass1" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_None12CoursePlaningClass1_SelectedIndexChanged"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel52" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_None12Course1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_None12CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Panel16" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList62" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_None12Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel53" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_None12CoursePlaningClass2" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_None12CoursePlaningClass2_SelectedIndexChanged"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel54" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_None12Course2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_None12CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>


                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tabs-10">
                    <asp:UpdatePanel ID="UpdatePanel106" runat="server">
                        <ContentTemplate>
                            <table>
                                 <tr>
                                    <th>積分上傳之課程選擇</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_CoursePlan12_1" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_CoursePlan_SelectedIndexChanged"></asp:DropDownList>
                                         <asp:Button ID="btn_LookFor12Code_1" runat="server" Text="查看代號" OnClick="btn_LookFor10Code_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>上傳課程代號</th>
                                    <td>
                                        <asp:TextBox ID="txt_CourseSNO_12_1" runat="server"></asp:TextBox>
                                </tr>
                                <tr>
                                    <th>增加規則</th>
                                    <td>
                                        <asp:CheckBox ID="chk_P19" runat="server" OnCheckedChanged="chk_P19_CheckedChanged" Text="規則一" AutoPostBack="true" />
                                        <asp:CheckBox ID="chk_P20" runat="server" OnCheckedChanged="chk_P20_CheckedChanged" Text="規則二" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_Junior12Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel55" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_Junior12CoursePlaningClass" OnSelectedIndexChanged="ddl_Junior12CoursePlaningClass_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel56" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_Junior12" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_Junior12CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <asp:Panel ID="Panel17" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList67" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Junior12Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel57" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Junior12CoursePlaningClass1" OnSelectedIndexChanged="ddl_Junior12CoursePlaningClass1_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel58" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Junior12_1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Junior12CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Panel18" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList70" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Junior12Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel59" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Junior12CoursePlaningClass2" OnSelectedIndexChanged="ddl_Junior12CoursePlaningClass2_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel60" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Junior12_2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Junior12CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tabs-11">
                    <asp:UpdatePanel ID="UpdatePanel107" runat="server">
                        <ContentTemplate>
                            <table>

                                <tr>
                                    <th>增加規則</th>
                                    <td>
                                        <asp:CheckBox ID="chk_P21" OnCheckedChanged="chk_P21_CheckedChanged" runat="server" Text="規則一" AutoPostBack="true" />
                                        <asp:CheckBox ID="chk_P22" OnCheckedChanged="chk_P22_CheckedChanged" runat="server" Text="規則二" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_Senior12Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel61" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_Senior12CoursePlaningClass" OnSelectedIndexChanged="ddl_Senior12CoursePlaningClass_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel62" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_Sunior12" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_Senior12CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <asp:Panel ID="Panel19" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList75" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Senior12Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel63" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Senior12CoursePlaningClass1" OnSelectedIndexChanged="ddl_Senior12CoursePlaningClass1_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel64" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Sunior12_1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Senior12CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Panel20" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList78" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Senior12Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel65" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Senior12CoursePlaningClass2" OnSelectedIndexChanged="ddl_Senior12CoursePlaningClass2_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel66" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Sunior12_2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Senior12CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tabs-12">
                    <asp:UpdatePanel ID="UpdatePanel108" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <th>增加規則</th>
                                    <td>
                                        <asp:CheckBox ID="chk_P23" OnCheckedChanged="chk_P23_CheckedChanged" runat="server" Text="規則一" AutoPostBack="true" />
                                        <asp:CheckBox ID="chk_P24" OnCheckedChanged="chk_P24_CheckedChanged" runat="server" Text="規則二" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_JuniorSenior12Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel67" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_JuniorSenior12CoursePlaningClass" OnSelectedIndexChanged="ddl_JuniorSenior12CoursePlaningClass_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel68" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_JuniorSenior12" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_JuniorSenior12CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <asp:Panel ID="Panel21" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList83" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_JuniorSenior12Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel69" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_JuniorSenior12CoursePlaningClass1" OnSelectedIndexChanged="ddl_JuniorSenior12CoursePlaningClass1_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel70" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_JuniorSenior12_1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_JuniorSenior12CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Panel22" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList86" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_JuniorSenior12Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel71" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_JuniorSenior12CoursePlaningClass2" OnSelectedIndexChanged="ddl_JuniorSenior12CoursePlaningClass2_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel98" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_JuniorSenior12_2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_JuniorSenior12CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <h3>衛教師</h3>
        <div>
            <div id="tabsRole13" style="margin-bottom: 10px;">
                <ul>
                    <li><a href="#tabs-13">皆未取得</a></li>
                    <li><a href="#tabs-14">取得初階</a></li>
                    <li><a href="#tabs-15">取得進階</a></li>
                    <li><a href="#tabs-16">取得初進階</a></li>
                </ul>

                <div id="tabs-13">
                    <asp:UpdatePanel ID="UpdatePanel72" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <th>積分上傳之課程選擇</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_CoursePlan13" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_CoursePlan13_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:Button ID="btn_LookFor13Code" OnClick="btn_LookFor13Code_Click" runat="server" Text="查看代號" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>上傳課程代號</th>
                                    <td>
                                        <asp:TextBox ID="txt_CourseSNO_13" runat="server"></asp:TextBox>
                                </tr>
                                <tr>
                                    <th>增加規則</th>
                                    <td>

                                        <asp:CheckBox ID="chk_P25" runat="server" OnCheckedChanged="chk_P25_CheckedChanged" Text="規則一" AutoPostBack="true" />
                                        <asp:CheckBox ID="chk_P26" runat="server" OnCheckedChanged="chk_P26_CheckedChanged" Text="規則二" AutoPostBack="true" />

                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_None13Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel73" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_None13CoursePlaningClass" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_None13CoursePlaningClass_SelectedIndexChanged"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel74" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_None13Course" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_None13CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>

                                <asp:Panel ID="Panel23" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList91" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_None13Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel75" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_None13CoursePlaningClass1" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_None13CoursePlaningClass1_SelectedIndexChanged"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel76" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_None13Course1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_None13CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Panel24" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList94" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_None13Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel77" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_None13CoursePlaningClass2" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_None13CoursePlaningClass2_SelectedIndexChanged"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel78" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_None13Course2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_None13CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>


                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tabs-14">
                    <asp:UpdatePanel ID="UpdatePanel109" runat="server">
                        <ContentTemplate>
                            <table>
                                 <tr>
                                    <th>積分上傳之課程選擇</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_CoursePlan13_1" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" OnSelectedIndexChanged="ddl_CoursePlan_SelectedIndexChanged"></asp:DropDownList>
                                         <asp:Button ID="btn_LookFor13Code_1" runat="server" Text="查看代號" OnClick="btn_LookFor10Code_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>上傳課程代號</th>
                                    <td>
                                        <asp:TextBox ID="txt_CourseSNO_13_1" runat="server"></asp:TextBox>
                                </tr>
                                <tr>
                                    <th>增加規則</th>
                                    <td>
                                        <asp:CheckBox ID="chk_P27" OnCheckedChanged="chk_P27_CheckedChanged" runat="server" Text="規則一" AutoPostBack="true" />
                                        <asp:CheckBox ID="chk_P28" OnCheckedChanged="chk_P28_CheckedChanged" runat="server" Text="規則二" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_Junior13Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel79" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_Junior13CoursePlaningClass" OnSelectedIndexChanged="ddl_Junior13CoursePlaningClass_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel80" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_Junior13" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_Junior13CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <asp:Panel ID="Panel25" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList99" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Junior13Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel81" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Junior13CoursePlaningClass1" OnSelectedIndexChanged="ddl_Junior13CoursePlaningClass1_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel82" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Junior13_1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Junior13CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Panel26" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList102" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Junior13Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel83" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Junior13CoursePlaningClass2" OnSelectedIndexChanged="ddl_Junior13CoursePlaningClass2_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel84" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Junior13_2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Junior13CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tabs-15">
                    <asp:UpdatePanel ID="UpdatePanel110" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <th>增加規則</th>
                                    <td>
                                        <asp:CheckBox ID="chk_P29" OnCheckedChanged="chk_P29_CheckedChanged" runat="server" Text="規則一" AutoPostBack="true" />
                                        <asp:CheckBox ID="chk_P30" OnCheckedChanged="chk_P30_CheckedChanged" runat="server" Text="規則二" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_Senior13Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel85" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_Senior13CoursePlaningClass" OnSelectedIndexChanged="ddl_Senior13CoursePlaningClass_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel86" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_Sunior13" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_Senior13CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <asp:Panel ID="Panel27" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList107" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Senior13Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel87" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Senior13CoursePlaningClass1" OnSelectedIndexChanged="ddl_Senior13CoursePlaningClass1_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel88" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Sunior13_1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Senior13CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Panel28" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList110" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Senior13Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel89" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Senior13CoursePlaningClass2" OnSelectedIndexChanged="ddl_Senior13CoursePlaningClass2_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel90" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_Sunior13_2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_Senior13CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="tabs-16">
                    <asp:UpdatePanel ID="UpdatePanel111" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <th>增加規則</th>
                                    <td>
                                        <asp:CheckBox ID="chk_P31" OnCheckedChanged="chk_P31_CheckedChanged" runat="server" Text="規則一" AutoPostBack="true" />
                                        <asp:CheckBox ID="chk_P32" OnCheckedChanged="chk_P32_CheckedChanged" runat="server" Text="規則二" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>報名所需證書</th>
                                    <td>
                                        <asp:DropDownList ID="ddl_JuniorSenior13Certificate" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th>報名所需課程</th>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel91" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddl_JuniorSenior13CoursePlaningClass" OnSelectedIndexChanged="ddl_JuniorSenior13CoursePlaningClass_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel92" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBoxList ID="CBL_JuniorSenior13" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddl_JuniorSenior13CoursePlaningClass" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <asp:Panel ID="Panel29" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList115" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書一</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_JuniorSenior13Certificate1" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程一</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel93" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_JuniorSenior13CoursePlaningClass1" OnSelectedIndexChanged="ddl_JuniorSenior13CoursePlaningClass1_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel94" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_JuniorSenior13_1" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_JuniorSenior13CoursePlaningClass1" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="Panel30" runat="server" Enabled="false">
                                    <tr>
                                        <th>邏輯</th>
                                        <td>
                                            <asp:DropDownList ID="DropDownList118" runat="server">
                                                <asp:ListItem Value="Or" Text="Or"></asp:ListItem>
                                                <asp:ListItem Value="And" Text="And"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需證書二</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_JuniorSenior13Certificate2" runat="server" DataTextField="CTypeName" DataValueField="CtypeSNO"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <th>報名所需課程二</th>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel95" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_JuniorSenior13CoursePlaningClass2" OnSelectedIndexChanged="ddl_JuniorSenior13CoursePlaningClass2_SelectedIndexChanged" AutoPostBack="true" runat="server" DataTextField="PlanName" DataValueField="PClassSNO"></asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel99" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBoxList ID="CBL_JuniorSenior13_2" runat="server" RepeatColumns="5" DataTextField="CourseName" DataValueField="CourseSNO" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_JuniorSenior13CoursePlaningClass2" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click" />
        <input name="btnCancel" type="button" value="取消" onclick="location.href = 'EventRole.aspx';" />
    </div>

</asp:Content>

