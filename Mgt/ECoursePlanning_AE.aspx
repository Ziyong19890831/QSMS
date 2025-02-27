<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="ECoursePlanning_AE.aspx.cs" Inherits="Mgt_ECoursePlanning_AE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="Work" runat="server" />
    <asp:HiddenField ID="txt_ID" runat="server" />
    <br />

    <table>
        <tr>
            <th><i class="fa fa-star"></i>繼續教育規劃名稱</th>
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
            <th><i class="fa fa-star"></i>繼續教育總學分</th>
            <td>

                <asp:TextBox ID="txt_Total" class="required w10" MaxLength="50" runat="server" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>必修實體學分</th>
            <td>

                <asp:TextBox ID="txt_Compulsory_Entity" class="required w10" MaxLength="50" runat="server" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>必修實習學分</th>
            <td>

                <asp:TextBox ID="txt_Compulsory_Practical" class="required w10" MaxLength="50" runat="server" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>必修通訊學分</th>
            <td>

                <asp:TextBox ID="txt_Compulsory_Communication" class="required w10" MaxLength="50" runat="server" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>必修線上學分</th>
            <td>

                <asp:TextBox ID="txt_Compulsory_Online" class="required w10" MaxLength="50" runat="server" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th><i class="fa fa-star"></i>適用對象</th>
            <td>
                <asp:CheckBoxList ID="cb_Role" class="required" runat="server" RepeatColumns="3" DataTextField="RoleName" DataValueField="RoleSNO" RepeatLayout="Table" />
                <span id="rolemsg"></span>
            </td>
        </tr>
         <tr>
            <th>繼續教育線上課程</th>
            <td>
               <asp:CheckBox ID="cb_Elearning" runat="server" Text="是否綁定" Checked="false" OnCheckedChanged="cb_Elearning_CheckedChanged" AutoPostBack="true" />
               <asp:DropDownList ID="ddl_ElearingClass" runat="server" DataTextField="PlanName" DataValueField="PClassSNO" Visible="false"></asp:DropDownList>
            </td>
        </tr>
     <tr>
            <th>繼續教育線上課程上限學分</th>
             <td>

                <asp:TextBox ID="txt_ElearnLimit" class="required w10" MaxLength="50" runat="server" OnKeyPress="if(((event.keyCode>=48)&&(event.keyCode <=57))||(event.keyCode==46)) {event.returnValue=true;} else{event.returnValue=false;}"></asp:TextBox>
            </td>
        </tr>
    </table>

    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="修改" OnClientClick="checkinput();" OnClick="btnOK_Click" />
        <input name="btnCancel" type="button" value="取消" onclick="location.href = 'ECoursePlanning.aspx';" />
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
