<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EventDetail.aspx.cs" Inherits="Mgt_EventDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../css/reset.css" rel="stylesheet" type="text/css" />
    <link href="../css/style04.css" rel="stylesheet" type="text/css" />
    <script src="../SpryAssets/SpryMenuBar.js" type="text/javascript"></script>
    <link href="../SpryAssets/SpryMenuBarHorizontal.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="../JS/jquery-3.2.1.min.js"></script>
    <script src="../JS/jquery-ui.min.js"></script>
    <script src="../JS/datepicker-zh-TW.js"></script>
    <script src="../JS/validate.js"></script>
    <script src="../JS/jquery.js"></script>
    <script src="../JS/validate.js"></script>
    <link href="../CSS/cmxform.css" rel="stylesheet" />
    <script type="text/javascript">
        var $jq = jQuery.noConflict(true); //為jquery.min.js有衝突多寫這一行出來,把$符號全換成$jq
    </script>
    <title>報名名單</title>
</head>
<body>
    <form id="form1" runat="server">

        <div class="padding30 both" id="mainContent">
            <div class="block">
                <div class="center">
                    <asp:Label ID="lbl_EventName" runat="server" Text="Label"></asp:Label>
                </div>
                <asp:GridView ID="gv_EventD" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="ROW_NO" HeaderText="序號" ItemStyle-CssClass="center" />
                        <asp:BoundField DataField="PersonID" HeaderText="身分證" ItemStyle-CssClass="center" />
                        <asp:BoundField DataField="Name" HeaderText="姓名" ItemStyle-CssClass="center" />
                        <asp:BoundField DataField="Tel" HeaderText="電話" ItemStyle-CssClass="center" />
                        <asp:BoundField DataField="Email" HeaderText="信箱" ItemStyle-CssClass="center" />
                        <asp:BoundField DataField="Organ" HeaderText="所屬機構" ItemStyle-CssClass="center" />
                    </Columns>
                </asp:GridView>
                <asp:Literal ID="ltl_PageNumber" runat="server"></asp:Literal>
                <br />
                <div class="center">
                    <input name="btnCancel" type="button" value="關閉" onclick="window.close();" style="padding: 10px; font-size: 16px;" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
