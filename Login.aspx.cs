using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
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
        if (Session["QSMS_UserInfo"] != null)
        {
            userInfo = (UserInfo)Session["QSMS_UserInfo"];
            IsLogin = true;
        }
        else { IsLogin = false; }
        MarqueeIsEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["Marquee"];
    }
    protected void Page_Load(object sender, EventArgs e)
    {

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
        //if (txt_Verification_Right.Value != Request.Cookies["CheckCode"].Value)
        //{
        //    Utility.showMessage(Page, "ErrorMessage", "驗證碼錯誤");
        //    return;
        //}



        //帳號登入，先讀取有無匹配的帳號或Mail
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PAccount", txt_Account_Right.Value);
        aDict.Add("PMail", txt_Account_Right.Value);
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
        //Dictionary<string, object> DictIP = new Dictionary<string, object>();
        //DictIP.Add("PAccount", txt_Account_Right.Value);
        //DataHelper objDHIP = new DataHelper();
        //DataTable objDTIP = objDH.queryData(@"SELECT TOP 1 * FROM LoginLog Where PAccount=@PAccount And LoginStatus='1' ORDER BY LoginTime DESC", DictIP);


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
                //確認登入錯誤次數
                if (objDT.Rows[0]["LoginErrorTime"].ToString() != "" && objDT.Rows[0]["LoginError"].ToString() != "")
                {
                    int LoginError = Convert.ToInt32(objDT.Rows[0]["LoginError"]);
                    DateTime LoginErrorTime = Convert.ToDateTime(objDT.Rows[0]["LoginErrorTime"]);
                    //驗證帳號是否鎖定，如果超過失敗次數則鎖定，並return。
                    if ((LoginError >= 3) && (DateTime.Now - LoginErrorTime).TotalMinutes < 30)
                    {
                        WriteLog(txt_Account_Right.Value, IP, "4004", "帳號鎖定", PersonSNO);
                        Response.Write("<script>alert('多次登入失敗，帳號已鎖定! 請聯繫管理者進行解鎖或三十分鐘再嘗試登入。');</script>");
                        return;
                    }
                    //歸零鎖定次數
                    if (LoginError < 3 || (DateTime.Now - LoginErrorTime).TotalMinutes >= 30)
                    {
                        aDict.Clear();
                        aDict.Add("PAccount", objDT.Rows[0]["PAccount"].ToString());
                        objDH.executeNonQuery("Update Person set LoginError='0' where PAccount=@PAccount", aDict);
                    }
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

                //判斷密碼是否正確
                if (txt_PWD_Right.Value != objDT.Rows[0]["PPWD"].ToString())
                {
                    WriteLog(txt_Account_Right.Value, IP, "4003", "密碼錯誤", PersonSNO);
                    Response.Write("<script>alert('密碼輸入錯誤，請重新再試。');</script>");
                    return;
                }

                //判斷帳號是否啟用或停用
                //僅先以IsEnable為判斷依據，起迄日不參考
                //啟用：startDate<today && endDate>today
                //停用：startDate=null || startDate>today || endDate<todayW
                //判斷密碼是否正確
                if (objDT.Rows[0]["IsEnable"].ToString() == "0")
                {
                    WriteLog(txt_Account_Right.Value, IP, "4005", "帳號停用中", PersonSNO);
                    Response.Write("<script>alert('很抱歉，您的帳號停用中，請詢問您的單位管理員。');</script>");
                    return;
                }


                //經過以上重重關卡後，終於可以登入囉~(如果上面關卡失敗會return，所以也不會跑到下面來)
                userInfo = new UserInfo();
                userInfo.PersonSNO = Convert.ToString(objDT.Rows[0]["PersonSNO"]);
                userInfo.RoleSNO = Convert.ToString(objDT.Rows[0]["RoleSNO"]);
                userInfo.RoleName = Convert.ToString(objDT.Rows[0]["RoleName"]);
                userInfo.RoleOrganType = Convert.ToString(objDT.Rows[0]["RoleOrganType"]);
                userInfo.RoleGroup = Convert.ToString(objDT.Rows[0]["RoleGroup"]);
                userInfo.RoleLevel = Convert.ToString(objDT.Rows[0]["RoleLevel"]);
                userInfo.IsAdmin = (bool)objDT.Rows[0]["IsAdmin"];
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



                WriteLog(txt_Account_Right.Value, IP, "0001", "登入成功", PersonSNO);

                //通過後建立Session
                Session["QSMS_UserInfo"] = userInfo;
                Session.Timeout = 60;
                Response.Write("<script>alert('登入成功!'); location.href='Notice.aspx';</script>");

            }

            //找不到匹配的帳號
            else
            {
                WriteLog(txt_Account_Right.Value, IP, "4002", "帳號不存在");
                Response.Write("<script>alert('查無此帳號，請重新再試。');</script>");
            }


        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('" + ex.Message + "'); location.href='Notice.aspx';</script>");
        }
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
}