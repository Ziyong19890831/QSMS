<%@ Page Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="Course_AE.aspx.cs" Inherits="Mgt_Course_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


    <table>
        <tr class="w3">
            <th><i class="fa fa-star"></i>課程規劃類別對應</th>
            <td>
                <asp:DropDownList ID="ddl_PClass" runat="server" DataValueField="PClassSNO" DataTextField="PlanName"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>課程類別1</th>
            <td>
                <asp:DropDownList ID="ddl_Class1" class="required" runat="server" DataValueField="PVal" DataTextField="MVal" />
            </td>
        </tr>
<%--        <tr>
            <th><i class="fa fa-star"></i>課程類別2</th>
            <td>
                <asp:DropDownList ID="ddl_Class2" class="required"  runat="server" DataValueField="PVal" DataTextField="MVal" />
            </td>
        </tr>--%>
<%--        <tr>
            <th ><i class="fa fa-star"></i>單元</th>
            <td>
                <asp:Label ID="lbl_UnitName" runat="server" Text="最多50字元"></asp:Label>
                <br />
                <asp:TextBox ID="txt_UnitName" class="required w10" runat="server" MaxLength="50"></asp:TextBox><br />
            </td>
        </tr>--%>
        <tr>
            <th ><i class="fa fa-star"></i>課程名稱</th>
            <td>
                <asp:Label ID="lbl_CourseName" runat="server" Text="最多50字元"></asp:Label>
                <br />
                <asp:TextBox ID="txt_CourseName" class="required w10" runat="server" MaxLength="50"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>時數</th>
            <td>
                <asp:Label ID="lbl_CHour" runat="server" Text="數字"></asp:Label>
                <br />
                <asp:TextBox ID="txt_CHour" class="required number" runat="server" MaxLength="4"></asp:TextBox><br />
            </td>
        </tr>
        <tr>
            <th>必修</th>
            <td>
                <asp:CheckBox ID="chk_Compulsory" runat="server" Text="是" />
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>是否啟用</th>
            <td>
                <asp:CheckBox ID="chk_IsEnable" runat="server" Text="是" />
            </td>
        </tr>
                <tr>
            <th>課程新舊版對應</th>
            <td>
                <asp:DropDownList ID="ddl_Course" runat="server" DataTextField="CourseName" DataValueField="CourseSNO"></asp:DropDownList>
               <asp:Label ID="lb_Pair" runat="server" Visible="false">已對應課程</asp:Label>
            </td>
        </tr>
        
        <tr>
            <th><i class="fa fa-star"></i>授課方式</th>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddl_Ctype" class="required" runat="server" AutoPostBack="true" DataValueField="PVal" DataTextField="MVal" OnSelectedIndexChanged="ddl_Ctype_SelectedIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>

        <tr id="tr_el" class="w2" runat="server">
            <th>E-Learning對應<br />(選擇授課方式為"線上"才可設定)</th>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pl_bind" runat="server" Visible="false">
                            <asp:Label ID="lb_BindELS" runat="server"></asp:Label>
                            <asp:HiddenField ID="hf_ELSCode" runat="server" />
                            <input id="btn_reset" runat="server" name="btnCancel" type="button" value="取消對應" onclick="restELSCode()"/>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddl_Ctype" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddl_ElearnSection" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pl_el" runat="server" Visible="false">
                            <asp:DropDownList ID="ddl_Elearn" runat="server" DataValueField="ELCode" DataTextField="ELName" AutoPostBack="true" OnSelectedIndexChanged="ddl_Elearn_SelectedIndexChanged"></asp:DropDownList>
                            <asp:DropDownList ID="ddl_ElearnSection" runat="server" DataValueField="ELSCode" DataTextField="ELSName" AutoPostBack="true" OnSelectedIndexChanged="ddl_ElearnSection_SelectedIndexChanged"></asp:DropDownList>
                        </asp:Panel>
                        </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddl_Ctype" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="確定" OnClick="btnOK_Click"/>
        <input name="btnCancel" type="button" value="取消" onclick="location.href='Course.aspx';"/>
    </div>


    <script>

        function restELSCode() {
            $("#<%=lb_BindELS.ClientID%>").text("尚未指定E-Learning對應");
            $("#<%=hf_ELSCode.ClientID%>").val("");
            $("#<%=btn_reset.ClientID%>").hide();
        }

    </script>


</asp:Content>

