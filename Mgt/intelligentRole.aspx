<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Mgt.master" AutoEventWireup="true" CodeFile="intelligentRole.aspx.cs" Inherits="Mgt_intelligentRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        input{
            width:100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="path mb20">目前位置：<a href="intelligentRole.aspx">智慧選課規則</a></div>
    <fieldset>
        <legend>功能列</legend>
        <asp:Button ID="btn_SearchCode" runat="server" Text="查詢代碼"  OnClick="btn_SearchCode_Click" /><br />
        <asp:Button ID="btn_FirstPart" runat="server" Text="編輯文字" OnClick="btn_FirstPart_Click" /><br />
        綠底：當我沒有這張證書時，要使用課程規劃代號去取得<br />
        籃底：當我有了這張證書時，要使用課程規劃代號去做繼續教育
    </fieldset>
    <table>
        <tr>
            <th>身分別</th>
            <th>證書</th>
            <th>無證書</th>
            <th>初階</th>
            <th>進階</th>
            <th>初進階</th>
            <th style="background-color:blue">治療</th>
            <th style="background-color:blue">衛教</th>
            <th style="background-color:blue">種籽師資</th>
            <th style="background-color:blue">取得牙衛</th>
            <th style="background-color:blue">取得衛衛教</th>
        </tr>
        <tr>
            <td rowspan="3">西醫師</td>
            <td>治療</td>
            <td>
                <asp:TextBox ID="txt_ICT0_10_N" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_10_J" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_10_S" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_10_SJ" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_10_T" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_10_G" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_10_SD" runat="server"></asp:TextBox></td>
                 <td>
                </td>
            <td>
                </td>
        </tr>
        <tr>
            <td>衛教</td>
            <td>
                <asp:TextBox ID="txt_ICT1_10_N"  runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_10_J"  runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_10_S"  runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_10_SJ" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_10_T"  runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_10_G"  runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_10_SD" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_10_11"  runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_10_13" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>種籽師資</td>
            <td>
                <asp:TextBox ID="txt_ICT2_10_N" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_10_J" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_10_S" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_10_SJ" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_10_T" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_10_G" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_10_SD" runat="server"></asp:TextBox></td>
            <td>
                </td>
            <td>
              </td>
        </tr>
        <tr>
            <td rowspan="3">牙醫師</td>
            <td>治療</td>
            <td>
                <asp:TextBox ID="txt_ICT0_11_N" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_11_J" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_11_S" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_11_SJ" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_11_T" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_11_G" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_11_SD" runat="server"></asp:TextBox></td>
            <td>
               </td>
            <td>
               </td>
        </tr>
        <tr>
            <td>衛教</td>
           <td>
                <asp:TextBox ID="txt_ICT1_11_N" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_11_J" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_11_S" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_11_SJ" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_11_T" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_11_G" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_11_SD" runat="server"></asp:TextBox></td>
            <td>
               </td>
            <td>
                </td>
        </tr>
        <tr>
            <td>種籽師資</td>
            <td>
                <asp:TextBox ID="txt_ICT2_11_N" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_11_J" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_11_S" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_11_SJ" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_11_T" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_11_G" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_11_SD" runat="server"></asp:TextBox></td>
            <td>
                </td>
            <td>
               </td>
        </tr>
        <tr>
            <td rowspan="3">藥師</td>
            <td>治療</td>
          <td>
                <asp:TextBox ID="txt_ICT0_12_N" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_12_J" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_12_S" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_12_SJ" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_12_T" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_12_G" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_12_SD" runat="server"></asp:TextBox></td>
            <td>
               </td>
            <td>
                </td>
        </tr>
        <tr>
            <td>衛教</td>
            <td>
                <asp:TextBox ID="txt_ICT1_12_N" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_12_J" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_12_S" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_12_SJ" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_12_T" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_12_G" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_12_SD" runat="server"></asp:TextBox></td>
            <td>
                </td>
            <td>
                </td>
        </tr>
        <tr>
            <td>種籽師資</td>
            <td>
                <asp:TextBox ID="txt_ICT2_12_N" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_12_J" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_12_S" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_12_SJ" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_12_T" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_12_G" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_12_SD" runat="server"></asp:TextBox></td>
            <td>
               </td>
            <td>
                </td>
        </tr>
        <tr>
            <td rowspan="3">衛教師</td>
            <td>治療</td>
            <td>
                <asp:TextBox ID="txt_ICT0_13_N" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_13_J" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_13_S" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_13_SJ" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_13_T" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_13_G" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT0_13_SD" runat="server"></asp:TextBox></td>
            <td>
                </td>
            <td>
               </td>
        </tr>
        <tr>
            <td>衛教</td>
            <td>
                <asp:TextBox ID="txt_ICT1_13_N" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_13_J" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_13_S" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_13_SJ" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_13_T" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_13_G" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT1_13_SD" runat="server"></asp:TextBox></td>
            <td>
               </td>
            <td>
                </td>
        </tr>
        <tr>
            <td>種籽師資</td>
            <td>
                <asp:TextBox ID="txt_ICT2_13_N" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_13_J" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_13_S" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_13_SJ" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_13_T" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_13_G" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txt_ICT2_13_SD" runat="server"></asp:TextBox></td>
            <td>
                </td>
            <td>
               </td>
        </tr>

    </table>
    <div class="center btns">
        <asp:Button ID="btnOK" runat="server" Text="確認" OnClick="btnOK_Click" />
    </div>
</asp:Content>

