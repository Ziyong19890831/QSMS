using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class MasterPage_Web : System.Web.UI.MasterPage
{

    public UserInfo userInfo = null;
    public bool IsLogin;
    public string PACC = "";
    public string PPWD = "";
    public string PersonSNO = "";
    public string personid = "";
    public string BookURL = "";
    public string MarqueeIsEnable = "";

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) {
            userInfo = (UserInfo)Session["QSMS_UserInfo"];
            
            IsLogin = true;
        }
        else { IsLogin = false; }
        MarqueeIsEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["Marquee"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["QSMS_UserInfo"] != null)
        //{
        //    userInfo = (UserInfo)Session["QSMS_UserInfo"];
        //    AutoSign();
        //}

        if (!IsPostBack)
        {
            //lb_IISITechnical.Text = IISITel();
            if (userInfo != null) {
                lb_PersonSNO.Text = userInfo.PersonID;
                AutoSign();
            } 
            
        }


        if (userInfo == null)
        {
            Btn_Logout.Visible = false;
            Panel_User.Visible = false;
            Panel_right_unlogin.Visible = true;
        }
        
        else
        {
    
            getBookURL();
            
            //登入時顯示個人資料
            Btn_Logout.Visible = true;
            Panel_User.Visible = true;
            Panel_right_unlogin.Visible = false;
            lb_UserName.Text = "Hello, " + userInfo.UserName;

            Dictionary<string, object> aDictd = new Dictionary<string, object>();
            aDictd.Add("RoleSNO", userInfo.RoleSNO);
            DataHelper objDe = new DataHelper();
            DataTable obtDe = objDe.queryData(@"Select * from RoleMenu where RoleSNO=@RoleSNO", aDictd);
            if (obtDe.Rows.Count > 0)
                HL_SYS.Visible = true;
            else
                HL_SYS.Visible = false;

            bool NewIcon = Utility.CheckToDO(userInfo.PersonSNO);
            if (NewIcon)
            {
                btn_Message.Value = "站內訊息(New)";
            }
            else
            {
                btn_Message.Value = "站內訊息";
            }
            //Utility.CheckToDO(userInfo.PersonID) == true : lb_Count.Enabled == true ? lb_Count.Enabled == false;

        }

        Application.Set("OnlineUsers", Utility.GetOnlineCount(Session.SessionID));
        lbl_HisTotal.Text = Application["HisCount"].ToString();
        lbl_TodayTotal.Text = Application["TodayCount"].ToString();
        lbl_OnlineUser.Text = Application["OnlineUsers"].ToString();

      
    }
    [WebMethod]
    public static bool CheckCardLogin(string PersonID)
    {
        DataHelper odt = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = "Select * from Person where PersonID=@PersonID";
        aDict.Add("PersonID", PersonID);
        DataTable ObjDT = odt.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return true;
        }
        return false;
    }
    protected void lbtnLogin_Click(object sender, EventArgs e)
    {


        UserInfo userInfo = null;

        //ALERT錯誤訊息      
        if (txt_Account_Right.Value == "" || txt_PWD_Right.Value == "")
        {
            Utility.showMessage(Page, "ErrorMessage", "請輸入帳號、密碼");
            return;
        }


        //驗證碼不正確時會return，不會繼續往下執行。
        if (txt_Verification_Right.Value != Request.Cookies["CheckCode"].Value)
        {
            Utility.showMessage(Page, "ErrorMessage", "驗證碼錯誤");
            return;
        }



        //帳號登入，先讀取有無匹配的帳號或Mail
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PAccount", txt_Account_Right.Value);
        aDict.Add("PMail", txt_Account_Right.Value);
        aDict.Add("PPWD", txt_PWD_Right.Value);
        DataHelper objDH = new DataHelper();
        string sql = @"
            Select P.*, R.RoleName, R.RoleOrganType, R.RoleGroup, R.RoleLevel, 
                O.AreaCodeA, O.AreaCodeB, O.OrganName, O.OrganCode, O.OrganLevel, R.IsAdmin 
            From Person P
                Left JOIN Organ O ON O.OrganSNO = P.OrganSNO
                Left JOIN Role R ON R.RoleSNO = P.RoleSNO
            Where PAccount=@PAccount ";

        DataTable objDT = objDH.queryData(sql, aDict);


        //取得登入紀錄和IP
        string IP = Request.ServerVariables["REMOTE_ADDR"].ToString();
        Dictionary<string, object> DictIP = new Dictionary<string, object>();
        DictIP.Add("PAccount", txt_Account_Right.Value);
        DataHelper objDHIP = new DataHelper();
        DataTable objDTIP = objDH.queryData(@"SELECT TOP 1 * FROM LoginLog Where PAccount=@PAccount And LoginStatus='1' ORDER BY LoginTime DESC", DictIP);


        try
        {

            //成功讀取帳號資料
            if (objDT.Rows.Count > 0)
            {

                PersonSNO = objDT.Rows[0]["PersonSNO"].ToString();

                PACC = objDT.Rows[0]["PAccount"].ToString();
                PPWD = objDT.Rows[0]["PPWD"].ToString();
                personid = objDT.Rows[0]["PersonID"].ToString();
                Session["update_Psd"] = PersonSNO;
                //int Error = 0;
                if (objDT.Rows[0]["LoginErrorTime"].ToString() != "" && objDT.Rows[0]["LoginError"].ToString() != "")
                {
                    int LoginError = Convert.ToInt32(objDT.Rows[0]["LoginError"]);
                    DateTime LoginErrorTime = Convert.ToDateTime(objDT.Rows[0]["LoginErrorTime"]);

                    //驗證帳號是否鎖定，如果超過失敗次數則鎖定，並return。
                    if ((LoginError >= 3) && (DateTime.Now - LoginErrorTime).TotalMinutes < 5)
                    {
                        WriteLog(txt_Account_Right.Value, IP, "4004", "帳號鎖定", PersonSNO);
                        Response.Write("<script>alert('多次登入失敗，帳號已鎖定! 請聯繫管理者進行解鎖或五分鐘再嘗試登入。');</script>");
                        return;
                    }
                    //歸零鎖定次數
                    if ((DateTime.Now - LoginErrorTime).TotalMinutes >= 5)
                    {
                        aDict.Clear();
                        aDict.Add("PAccount", objDT.Rows[0]["PAccount"].ToString());
                        objDH.executeNonQuery("Update Person set LoginError='0' where PAccount=@PAccount", aDict);
                    }
                }

                

                //判斷帳號是否啟用或停用
                //僅先以IsEnable為判斷依據，起迄日不參考
                //啟用：startDate<today && endDate>today
                //停用：startDate=null || startDate>today || endDate<today
                //判斷密碼是否正確
                if (objDT.Rows[0]["IsEnable"].ToString() == "0")
                {
                    WriteLog(txt_Account_Right.Value, IP, "4005", "帳號停用中", PersonSNO);
                    Response.Write("<script>alert('很抱歉，您的帳號停用中，請詢問您的單位管理員。');</script>");
                    return;
                }
                if (PPWD != txt_PWD_Right.Value)
                {
                    int Error = Convert.ToInt16(objDT.Rows[0]["LoginError"]) + 1;
                    WriteLog(txt_Account_Right.Value, IP, "4003", "密碼錯誤", PersonSNO);
                    //Response.Write("<script>alert('密碼輸入錯誤，請重新再試。');</script>");
                    aDict.Clear();
                    aDict.Add("PAccount", objDT.Rows[0]["PAccount"].ToString());
                    aDict.Add("Error", Error);
                    aDict.Add("LoginErrorTime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    objDH.executeNonQuery("Update Person set LoginError=@Error,LoginErrorTime=@LoginErrorTime where PAccount=@PAccount", aDict);
                    Response.Write("<script>alert('密碼不正確，請重新再試。');</script>");
                    return;
                }
                //定期三個月修改密碼 

                if (objDT.Rows[0]["CreateDT"].ToString() != "")
                {

                    DateTime CreateDT = Convert.ToDateTime(objDT.Rows[0]["CreateDT"]);
                    if (objDT.Rows[0]["PasswordModilyDT"].ToString() != "")
                    {
                        DateTime PasswordModilyDT = Convert.ToDateTime(objDT.Rows[0]["PasswordModilyDT"]);
                        if ((DateTime.Now.Year * 12 + DateTime.Now.Month - PasswordModilyDT.Year * 12 - PasswordModilyDT.Month) >= 3)
                        {
                            if ((DateTime.Now.Year * 12 + DateTime.Now.Month - CreateDT.Year * 12 - CreateDT.Month) >= 3)
                            {
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "exampleModal", "$('#exampleModal').modal({ backdrop: 'static', keyboard: false });", true);
                                return;
                            }
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "exampleModal", "$('#exampleModal').modal({ backdrop: 'static', keyboard: false });", true);
                            return;
                        }
                    }
                    else
                    {
                        if ((DateTime.Now.Year * 12 + DateTime.Now.Month - CreateDT.Year * 12 - CreateDT.Month) >= 3)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "exampleModal", "$('#exampleModal').modal({ backdrop: 'static', keyboard: false });", true);
                            return;
                        }

                    }
                }
                //經過以上重重關卡後，終於可以登入囉~(如果上面關卡失敗會return，所以也不會跑到下面來)
                userInfo = new UserInfo();
                userInfo.PersonSNO = Convert.ToString(objDT.Rows[0]["PersonSNO"]);
                userInfo.RoleSNO = Convert.ToString(objDT.Rows[0]["RoleSNO"]);
                userInfo.RoleName = Convert.ToString(objDT.Rows[0]["RoleName"]);
                userInfo.RoleOrganType = Convert.ToString(objDT.Rows[0]["RoleOrganType"]);
                userInfo.RoleGroup = Convert.ToString(objDT.Rows[0]["RoleGroup"]);
                userInfo.RoleLevel = Convert.ToString(objDT.Rows[0]["RoleLevel"]);
                userInfo.IsAdmin = Convert.ToBoolean(objDT.Rows[0]["IsAdmin"]);
                userInfo.AreaCodeA = Convert.ToString(objDT.Rows[0]["AreaCodeA"]);
                userInfo.AreaCodeB = Convert.ToString(objDT.Rows[0]["AreaCodeB"]);
                userInfo.OrganSNO = Convert.ToString(objDT.Rows[0]["OrganSNO"]);
                userInfo.OrganCode = Convert.ToString(objDT.Rows[0]["OrganCode"]);
                userInfo.OrganName = Convert.ToString(objDT.Rows[0]["OrganName"]);
                userInfo.OrganLevel = Convert.ToString(objDT.Rows[0]["OrganLevel"]);
                userInfo.UserAccount = Convert.ToString(objDT.Rows[0]["PAccount"]);
                userInfo.UserPWD = Convert.ToString(objDT.Rows[0]["PPWD"]);
                userInfo.UserTel = Convert.ToString(objDT.Rows[0]["PTel"]);
                userInfo.UserPhone = Convert.ToString(objDT.Rows[0]["PPhone"]);
                userInfo.UserMail = Convert.ToString(objDT.Rows[0]["PMail"]);
                userInfo.UserName = Convert.ToString(objDT.Rows[0]["PName"]);
                userInfo.PersonID = Convert.ToString(objDT.Rows[0]["PersonID"]);
                userInfo.Birthday= Convert.ToString(objDT.Rows[0]["PBirthDate"]);
                userInfo.UserPhone = Convert.ToString(objDT.Rows[0]["PPhone"]);
                userInfo.Address = Convert.ToString(objDT.Rows[0]["PAddr"]);
                userInfo.TsType = Convert.ToString(objDT.Rows[0]["TsSNO"]);
                userInfo.TsTypeNote = Convert.ToString(objDT.Rows[0]["TSNote"]);

                WriteLog(txt_Account_Right.Value, IP, "0001", "登入成功", PersonSNO);

                //通過後建立Session
                Session["QSMS_UserInfo"] = userInfo;
                Session.Timeout = 30;

                if (CheckLogin(txt_Account_Right.Value))
                {

                    Response.Write("<script>alert('需定期修改基本資料，煩請重新確認。');location.href = 'Personnel_AE.aspx';</script>");
                }
                //FirstSignInELearning();
                //if ((bool)objDT.Rows[0]["IsAdmin"])
                //{
                //    Response.Write("<script>alert('登入成功!');location.href='Notice.aspx'; </script>");
                //}
                //else
                //{
                //    Response.Write("<script>alert('登入成功!');location.href='PersonnelSite.aspx'; </script>");
                //}
                Response.Write("<script>alert('登入成功!');location.href='Notice.aspx'; </script>");
                ///定期每年確認修改基本資料
            }

            //找不到匹配的帳號
            else
            {
                Dictionary<string, object> EictIP = new Dictionary<string, object>();
                EictIP.Add("PAccount", txt_Account_Right.Value);
                EictIP.Add("PMail", txt_Account_Right.Value);
                EictIP.Add("PPWD", txt_PWD_Right.Value);
                string Errorsql = @"
            Select P.*, R.RoleName, R.RoleOrganType, R.RoleGroup, R.RoleLevel, 
                O.AreaCodeA, O.AreaCodeB, O.OrganName, O.OrganCode, O.OrganLevel, R.IsAdmin 
            From Person P
                Left JOIN Organ O ON O.OrganSNO = P.OrganSNO
                Left JOIN Role R ON R.RoleSNO = P.RoleSNO
            Where PAccount=@PAccount ";

                DataTable ErrobjDT = objDH.queryData(Errorsql, EictIP);
                //判斷密碼是否正確
                if (ErrobjDT.Rows.Count > 0)
                {
                    if (txt_PWD_Right.Value != ErrobjDT.Rows[0]["PPWD"].ToString())
                    {
                        int Error = Convert.ToInt16(ErrobjDT.Rows[0]["LoginError"].ToString()) + 1;
                        WriteLog(txt_Account_Right.Value, IP, "4003", "密碼錯誤", PersonSNO);
                        //Response.Write("<script>alert('密碼輸入錯誤，請重新再試。');</script>");
                        EictIP.Clear();
                        EictIP.Add("PAccount", ErrobjDT.Rows[0]["PAccount"].ToString());
                        EictIP.Add("Error", Error);
                        EictIP.Add("LoginErrorTime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        objDH.executeNonQuery("Update Person set LoginError=@Error,LoginErrorTime=@LoginErrorTime where PAccount=@PAccount", EictIP);
                        Response.Write("<script>alert('帳號密碼不正確，請重新再試。');</script>");
                        return;
                    }

                    //確認登入錯誤次數
                    if (ErrobjDT.Rows[0]["LoginErrorTime"].ToString() != "" && ErrobjDT.Rows[0]["LoginError"].ToString() != "")
                    {
                        int LoginError = Convert.ToInt32(ErrobjDT.Rows[0]["LoginError"]);
                        DateTime LoginErrorTime = Convert.ToDateTime(ErrobjDT.Rows[0]["LoginErrorTime"]);

                        //驗證帳號是否鎖定，如果超過失敗次數則鎖定，並return。
                        if ((LoginError >= 3) && (DateTime.Now - LoginErrorTime).TotalMinutes < 5)
                        {
                            WriteLog(txt_Account_Right.Value, IP, "4004", "帳號鎖定", PersonSNO);
                            Response.Write("<script>alert('多次登入失敗，帳號已鎖定! 請聯繫管理者進行解鎖或五分鐘再嘗試登入。');</script>");
                            return;
                        }

                    }

                }
                else
                {
                    Response.Write("<script>alert('系統無此帳號，請先完成帳號申請。');</script>");
                }

                WriteLog(txt_Account_Right.Value, IP, "4002", "帳號不存在");
               
            }


        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('帳號或密碼不正確');location.href='Notice.aspx';</script>");
        }


    }
    protected void Btn_VisF_Click(object sender, EventArgs e)
    {
        try
        {
            Session["UserInfo"] = null;
            var Model = new CardLoginModel()
            {
                Page = Page,
                CardType = hidCardType.Value,
                IdNo = hidCardUserId.Value,
                Password = hidCardLoginKind.Value == "R" ? cardTypePassword.Value : cardTypePassword.Value,
                VerificationCode = hidCardLoginKind.Value == "R" ? txt_Verification_Right.Value : txt_Verification_Right.Value,
                CheckCode = Request.Cookies["CheckCode"].Value,
                IP = Request.ServerVariables["REMOTE_ADDR"].ToString(),
                Now = hidNow.Value,
                Sign = hidSign.Value,
            };

            var helper = CardLoginManager.CreateHelper(Model);
            if (!helper.VerifyInput()) return;
            if (!helper.VerifyCard()) return;
            var persons = helper.LoadPerson();
            var person = helper.VerifyPersons(persons);
            if (person == null) return;
            var lastLog = helper.LoadLastLogin(person);
            var userInfo = helper.ToUserInfo(person, lastLog);
            Session["QSMS_UserInfo"] = userInfo;
            WriteLog(txt_Account_Right.Value, Model.IP, "0001", "登入成功", userInfo.PersonSNO);
            Response.Write("<script>alert('登入成功!'); location.href='Notice.aspx';</script>");
        }
        catch (Exception)
        {
            Utility.showMessage(Page, "ErrorMessage", "伺服器內部錯誤");
        }
    }
    protected void lbtnCardLogin_Click(object sender, EventArgs e)
    {
        try
        {
            Session["UserInfo"] = null;
            var Model = new CardLoginModel()
            {
                Page = Page,
                CardType = hidCardType.Value,
                IdNo = hidCardUserId.Value,
                Password = hidCardLoginKind.Value == "R" ? cardTypePassword.Value : cardTypePassword.Value,
                VerificationCode = hidCardLoginKind.Value == "R" ? txt_Verification_Right.Value : txt_Verification_Right.Value,
                CheckCode = Request.Cookies["CheckCode"].Value,
                IP = Request.ServerVariables["REMOTE_ADDR"].ToString(),
                Now = hidNow.Value,
                Sign = hidSign.Value,
            };

            var helper = CardLoginManager.CreateHelper(Model);
            if (!helper.VerifyInput()) return;
            if (!helper.VerifyCard()) return;
            var persons = helper.LoadPerson();
            var person = helper.VerifyPersons(persons);
            if (person == null) return;
            var lastLog = helper.LoadLastLogin(person);
            var userInfo = helper.ToUserInfo(person, lastLog);
            Session["UserInfo"] = userInfo;
            WriteLog(txt_Account_Right.Value, Model.IP, "0001", "登入成功", userInfo.PersonSNO);
            Response.Write("<script>alert('登入成功!'); location.href='Notice.aspx';</script>");
        }
        catch (Exception)
        {
            Utility.showMessage(Page, "ErrorMessage", "伺服器內部錯誤");
        }
    }

    protected void getBookURL()
    {
        Dictionary<string, object> Dict = new Dictionary<string, object>();
        Dict.Add("PID", "QSResource");
        DataHelper objDH = new DataHelper();
        DataTable objTBL = objDH.queryData(@"Select PVal from Config where PID=@PID", Dict);
        if (objTBL.Rows.Count > 0)
            BookURL = objTBL.Rows[0][0].ToString();
    }
    protected void getInfo(Label label)
    {
        Dictionary<string, object> Dict = new Dictionary<string, object>();
        Dict.Add("PID", "Tel");
        DataHelper objDH = new DataHelper();
        DataTable objTBL = objDH.queryData(@"Select PVal from Config where PID=@PID", Dict);
        label.Text= objTBL.Rows[0][0].ToString();

    }

    protected void WriteLog(string acc, string ip, string status, string info, string psno = "")
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Clear();
        aDict.Add("PAccount", acc);
        aDict.Add("LoginIP", ip);
        aDict.Add("LoginStatus", status);
        aDict.Add("LoginInfo", info);
        if (psno == "")
        {
            objDH.executeNonQuery(@"Insert into LoginLog(PAccount,LoginIP,LoginStatus,LoginInfo) 
            Values(@PAccount,@LoginIP,@LoginStatus,@LoginInfo)", aDict);
        }
        else
        {
            aDict.Add("PersonSNO", psno);
            objDH.executeNonQuery(@"Insert into LoginLog(PAccount,LoginIP,LoginStatus,LoginInfo,PersonSNO) 
            Values(@PAccount,@LoginIP,@LoginStatus,@LoginInfo,@PersonSNO)", aDict);
        }
    }

    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        
        Response.Write("<script>alert('已登出!'); location.href='Notice.aspx';</script>");
    }

    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> Dict = new Dictionary<string, object>();
        string PSNO = Session["update_Psd"].ToString();
        //先撈出登入人員比對密碼
        string Strsql = "select * from Person where PersonSNO=@PersonSNO";
        Dict.Add("PersonSNO", PSNO);
        DataHelper dataHelper = new DataHelper();
        DataTable dt = dataHelper.queryData(Strsql, Dict);
        string a = dt.Rows[0]["PPWD"].ToString();
        if (txt_OldPswd.Value != dt.Rows[0]["PPWD"].ToString())
        {
            Response.Write("<script>alert('密碼不相符!')</script>");
            return;
        }

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonSNO", PSNO);
        aDict.Add("NewPWD", txt_Pswd.Value);
        aDict.Add("PasswordModilyDT", DateTime.Now);
        DataHelper objDH = new DataHelper();

        objDH.executeNonQuery("Update Person Set PPWD=@NewPWD,PasswordModilyDT=@PasswordModilyDT Where PersonSNO=@PersonSNO", aDict);
        Response.Write("<script>alert('修改成功!'); location.href='Notice.aspx';</script>");
    }
    public static bool IsExam(string PersonSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> Dict = new Dictionary<string, object>();
        string sql = "Select IsExam from Person where PersonSNO=@PersonSNO";
        Dict.Add("PersonSNO", PersonSNO);
        DataTable objDT = ObjDH.queryData(sql, Dict);
        if (objDT.Rows.Count > 0)
        {
            if (Convert.ToBoolean(objDT.Rows[0]["IsExam"]) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {

        }
        return true;
    }
    protected void AutoSign()
    {
        //string url_course = "https://e-quitsmoking.hpa.gov.tw/qsms-api/sso/generate-url?key=UoLgyT3cLMeM9jAu0smB";
        string url_course = "https://hpaqs.mydevhost.com/qsms-api/completions/query?key=UoLgyT3cLMeM9jAu0smB";
        string param = "";
        param += "firstName=" + userInfo.UserName.Substring(1);
        param += "&lastName=" + userInfo.UserName.Substring(0, 1);
        param += "&username=" + userInfo.PersonID;
        param += "&idNumber=" + userInfo.PersonID;
        param += "&email=" + userInfo.UserMail;

        //強制認為憑證都是通過的，特殊情況再使用
        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  //因應HTTPS調整

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url_course);
        request.Method = "POST";
        request.Timeout = 3000000;
        request.ContentType = "application/x-www-form-urlencoded";

        //要發送的字串轉為byte[] 
        byte[] byteArray = Encoding.UTF8.GetBytes(param);
        using (Stream reqStream = request.GetRequestStream())
        {
            reqStream.Write(byteArray, 0, byteArray.Length);

        }//end using

        //API回傳的字串
        string responseStr = "";
        using (WebResponse response = request.GetResponse())
        {
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseStr = sr.ReadToEnd();
            }//end using  
        }

        responseStr = responseStr.Replace("{\"loginUrl\":\"", "");
        responseStr = responseStr.Replace("\"}", "");
        responseStr = responseStr.Replace("\\", "");
        //lb_key.Text = responseStr;
        //hf_Link.Value = responseStr;
        //Response.Write(responseStr);
        //hf_Auto.Value = responseStr;
    }
    public static bool CheckLogin(string PAccount)
    {
        Dictionary<string, object> Dict = new Dictionary<string, object>();
        DataHelper dataHelper = new DataHelper();
        Dict.Add("PAccount", PAccount);
        string Strsql = "  Select Top(2) LoginTime from LoginLog where PAccount=@PAccount Order by LoginTime DESC";
        DataTable dt = dataHelper.queryData(Strsql, Dict);
        if (dt.Rows.Count > 0)
        {
            DateTime LoginTime = Convert.ToDateTime(dt.Rows[1]["LoginTime"]);
            if (LoginTime.Year < DateTime.Now.Year)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    protected static void CardLogin(string PersonID)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, Object> adict = new Dictionary<string, object>();
        string CardPersonID = PersonID;
        
        string SQL = "Select * from Person where PersonID=@PersonID";
        adict.Add("PersonID", CardPersonID);
        DataTable ObjDT = ObjDH.queryData(SQL, adict);
        if (ObjDT.Rows.Count > 0)
        {
           
            

            
        }
        else
        {
           
        }
    }
}


