<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Dialog.master" AutoEventWireup="true" CodeFile="Notice_Preview.aspx.cs" Inherits="Mgt_Notice_Preview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <h1><i class="fa fa-newspaper"></i>公告事項</h1>

      <div class="row">
        <div class="col-12">
            <div class="tab-content" id="myTableData">
                <table class="table table-striped">
                    <tr>
                        <th class="w7 txtL">
                            <asp:Label ID="lb_Name" class="control-label" runat="server"></asp:Label></th>
                        <th class="w3 txtL">
                            <asp:Label ID="lb_SDate" class="control-label" runat="server"></asp:Label></th>
                    </tr>
                    <tr>
                        <td colspan="2" class="padding20">
                            <asp:Label ID="lb_Title" class="control-label" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="padding20">
                            <asp:Label ID="lb_Info" class="control-label" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="center btns">  
        <input name="btnCancel" type="button" value="關閉視窗" onclick="window.close();" />
    </div> 
</asp:Content>

