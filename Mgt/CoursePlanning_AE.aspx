<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="CoursePlanning_AE.aspx.cs" Inherits="Mgt_CoursePlanning_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />
    <br />

    <table>
        <tr>
            <th><i class="fa fa-star"></i>課程規劃名稱</th>
            <td>
                <asp:Label ID="lbl_PlanName" runat="server" Text="最多50字元" Font-Size="Medium"></asp:Label>
                <br />
                <asp:TextBox ID="txt_PlanName" class="required w10" MaxLength="50" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>起迄年度</th>
            <td>
                <asp:TextBox ID="txt_CStartYear" class="required number" MaxLength="4" runat="server" Width="80px"></asp:TextBox>
                -
                <asp:TextBox ID="txt_CEndYear" class="required number" MaxLength="4" runat="server" Width="80px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>啟用</th>
            <td>
                <asp:DropDownList ID="ddl_IsEnable" class="required" runat="server" /></td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>對應證書</th>
            <td>
                <asp:DropDownList ID="ddl_CType" class="required" runat="server" DataValueField="CTypeSNO" DataTextField="CTypeName" />
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>目標積分</th>
            <td>
               
                <asp:TextBox ID="txt_TargetIntegral" class="required w10" MaxLength="50" runat="server" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>線上報名門檻積分</th>
            <td>
               
                <asp:TextBox ID="txt_SignLimit" class="required w10" MaxLength="50" runat="server" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <th><i class="fa fa-star"></i>適用對象</th>
            <td>
                <asp:CheckBoxList ID="cb_Role" class="required" runat="server" RepeatColumns="2" DataTextField="RoleName" DataValueField="RoleSNO" RepeatLayout="Table" />
                <span id="rolemsg"></span>
            </td>
        </tr>

        <tr>
            <th>已繫結的課程<br />(可從課程維護功能變更)</th>
            <td>
                <asp:GridView ID="gv_Course" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="CourseSNO" runat="server" Text='<%# Eval("CourseSNO")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderTemplate>
                                CourseSNO
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="課程類別1" DataField="Class1" />
                        <asp:BoundField HeaderText="課程類别2" DataField="Class2" />
                        <asp:BoundField HeaderText="單元" DataField="UnitName" />
                        <asp:BoundField HeaderText="課程名稱" DataField="CourseName">
                            <ItemStyle Width="300px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="授課方式" DataField="Ctype" />
                        <asp:BoundField HeaderText="時數" DataField="CHour" />
                         <asp:BoundField HeaderText="是否啟用" DataField="IsEnable" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>

    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="修改" OnClientClick="checkinput();" OnClick="btnOK_Click"/>
        <input name="btnCancel" type="button" value="取消" onclick="location.href='CoursePlanning.aspx';"/>
    </div>



    <script type="text/javascript">

        function checkinput() {
            var valuelist = '';
            var isSelect;
            //共用檢核是否空值 
            $('#ContentPlaceHolder1_cb_Role input[type=checkbox]:checked').each(function () {
                isSelect = true;
                return false;
            });
            if (isSelect != true) {
                event.preventDefault();
                alert('請勾選適用對象');
            }

        }
    </script>
</asp:Content>
