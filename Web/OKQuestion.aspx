<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Web.master" AutoEventWireup="true" CodeFile="OKQuestion.aspx.cs" Inherits="Web_OKQuestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="path mb20">目前位置：<a href="Question.aspx">問卷填寫</a></div>
    <h1><i class="fa fa-edit"></i>問卷填寫<asp:Label ID="Label6" Font-Size="17pt" runat="server" Text="-填寫結果"></asp:Label>
        <asp:Label ID="Label1" Style="visibility: hidden" runat="server" Text=""></asp:Label>
        <asp:Label ID="Label4" Style="visibility: hidden" runat="server" Text=""></asp:Label>
    </h1>

    <div style="border: 1px solid #bcbcbc; padding: 15px;">
        問卷名稱:<br />
        <asp:Label ID="Label2" Font-Size="17pt" ForeColor="#0099cc" Font-Bold="true" runat="server" Text=""></asp:Label><br />
        <br />
        <a style="color: #bababa">填寫日期:</a><asp:Label ID="Label5" Font-Size="10pt" runat="server" Text=""></asp:Label>
        <br />
        <a style="color: #bababa">問卷說明:</a>
        <asp:Label ID="Label3" Font-Size="10pt" runat="server" Text=""></asp:Label>
        <br />
        <br />
        <asp:Repeater ID="rpt_QA" runat="server">
            <ItemTemplate>

                <div style="background-color: #d8d8d8; padding: 5px; border-left: 10px solid #808080">
                    <i style="color: white" class="fa fa-pencil-square"></i><a style="font-weight: bold; margin-left: 5px; font-size: 13pt"><%# Eval("QuestionName") %></a>

                </div>
                <div style="height: auto; margin-bottom: 5px; margin-top: 5px; padding-left: 30px;">
                    <div>
                        ➪ <%# Eval("OptionName") %>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>


</asp:Content>

