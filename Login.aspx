<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<head>

	<!-- Basics -->
	
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
	
	<title>登入</title>

	<!-- CSS -->
	
	<link rel="stylesheet" href="css/reset_1.css">
	<link rel="stylesheet" href="css/animate.css">
	<link rel="stylesheet" href="css/styles.css">
	<style type="text/css">

	</style>
</head>

	<!-- Main HTML -->
	
<body>
	
	<!-- Begin Page Content -->
	
	<div id="container">
		
		<form runat="server">
		
		<label for="name">帳號:</label>
		
		<input type="name" id="txt_Account_Right" runat="server" placeholder="請輸入帳號">
		
		<label for="username">密碼:</label>
		
		<p><a href="#">忘記密碼</a></p>

		<input type="password" id="txt_PWD_Right" runat="server" placeholder="請輸入密碼">
		<div>
             <input type="name" name="input" placeholder="驗證碼" id="txt_Verification_Right" runat="server" style="width:70px;" />   
             <button type="button" onclick="img_CheckCode.src='Web/CheckCode.aspx?' + Math.random();" class="vCode" style="background-color: gray; color:white; font-weight:normal;">更新</button>  
             <image border="0" src="Web/CheckCode.aspx" alt="驗證碼" id="img_CheckCode" runat="server" class="vCode" />
		 </div>
		<div id="lower">
		
		<input type="checkbox"><label class="check" for="checkbox">Keep me logged in</label>
		
		 <asp:Button ID="lbtn_Login_Right" runat="server"  OnClientClick="block_verification();" Text="登入" OnClick="lbtnLogin_Click" CausesValidation="False" />
		
		</div>
		 
		</form>
		
	</div>
	
	
	<!-- End Page Content -->
	
</body>

