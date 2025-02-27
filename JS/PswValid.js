
//CharMode函數 
//測試某個字元是屬於哪一類. 
function CharMode(iN) {
    if (iN >= 48 && iN <= 57) //數字 
        return 1;
    if (iN >= 65 && iN <= 90) //大寫字母
        return 1;
    if (iN >= 97 && iN >= 122) //小寫
        return 1;
    else
        return 8; //特殊字元
}
//bitTotal函數
//計算出當前密碼當中一共有多少種模式
function bitTotal(num) {
    modes = 0;
    for (i = 0; i < 4; i++) {
        if (num & 1) modes++;
        num >>>= 1;
    }
    return modes;
}
//checkStrong函數
//返回密碼的強度級別
function checkStrong(sPW) {
    var big = hasCapital(sPW);
    var small = hasLowercase(sPW);
    var num = hasNumber(sPW);
    //var ssign = hasSSign(sPW);
    var nper = hasOther(sPW);

    //if (nper == true) {
    //    alert("密碼不符合規定");
    //}
    if (sPW.length <= 7) {
        return 0; //密碼太短
    }
    Modes = 0;

    for (i = 0; i < sPW.length; i++) {
        //測試每一個字元的類別並統計一共有多少種模式. 
        Modes |= CharMode(sPW.charCodeAt(i));
    }

    if (big == true && small == true && num == true && nper == false) {
        if (sPW.length > 18) {
            return 3;
        }
        else {
            return bitTotal(Modes);
        }
    }
    else {
        return 0;
    }

}


// 判斷是否含有大寫字母
function hasCapital(str) {
    var result = str.match(/^.*[A-Z]+.*$/);
    if (result == null) return false;
    return true;
}

// 判斷是否含有小寫字母
function hasLowercase(str) {
    var result = str.match(/^.*[a-z]+.*$/);
    if (result == null) return false;
    return true;
}

// 判斷是否含有數字
function hasNumber(str) {
    var result = str.match(/^.*[0-9]+.*$/);
    if (result == null) return false;
    return true;
}

// 判斷是否含有規定字符
//function hasSSign(str) {
//    var result = str.match(/^.*[\~\!\#\^\*]+.*$/);
//    if (result == null) return false;
//    return true;
//}

// 判斷是否含有其他字符
function hasOther(str) {
    var result = str.match(/^.*[^0-9A-Za-z\~\!\#\^\*]+.*$/);
    if (result == null) return false;
    return true;
}



//密碼：驗證、強度

function pswChkStrength(psw1ID, psw2ID, msg1ID, msg2ID, strengthID) {

    O_color = "#eeeeee";
    L_color = "#fddc97";
    M_color = "#b7e3a5";
    H_color = "#87d068";
    if ($("#" + psw1ID) == null || $("#" + psw1ID).val() == '') {
        Lcolor = Mcolor = Hcolor = O_color;
    }
    else {
        S_level = checkStrong($("#" + psw1ID).val());
        switch (S_level) {
            case 0:
                Lcolor = Mcolor = Hcolor = O_color;
            case 1:
                Lcolor = L_color;
                Mcolor = Hcolor = O_color;
                break;
            case 2:
                Lcolor = Mcolor = M_color;
                Hcolor = O_color;
                break;
            default:
                Lcolor = Mcolor = Hcolor = H_color;
        }
    }
    
    document.getElementById(strengthID + "_L").style.background = Lcolor;
    document.getElementById(strengthID + "_M").style.background = Mcolor;
    document.getElementById(strengthID + "_H").style.background = Hcolor;


    if ($("#" + psw1ID).val() != "") {
        qucss(psw1ID, msg1ID, "H")
        pswChkInputIsSame(psw1ID, psw2ID, msg2ID);
    }
}


//確認兩次輸入的密碼是否相同：Web.master、Account.aspx
function pswChkInputIsSame(psw1ID, psw2ID, msgID) {
    if ($("#" + psw2ID).val() != "") {
        if ($("#" + psw1ID).val() != $("#" + psw2ID).val()) {
            $("#" + msgID).text("與密碼不相同");
            qucss(psw2ID, msgID, "S")
        }
        else {
            qucss(psw2ID, msgID, "H")
        }
    }
}



function qucss(tid, mid, pool) {
    if (pool == "H") {
        $('#' + tid).css("border", "1px solid #ccc");
        $('#' + mid).css("visibility", "hidden");
    } else {
        $('#' + tid).css("border", "1px dotted #f84b4b");
        $('#' + mid).css("visibility", "visible");
        $("#" + mid).css("color", "red");
    }
}


