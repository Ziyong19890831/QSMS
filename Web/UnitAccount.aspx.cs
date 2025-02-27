using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Web_UnitAccount : System.Web.UI.Page
{

    public string PersonSno = "";
    Random random = new Random();
    //public UserInfo userInfo = null;
    public static int timeLeft = 30;
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            Response.Write("<script>alert('本功能僅提供開課單位申請管理者帳號使用，若您的身分為學員，請勿使用此功能申請帳號。');</script>");
        }
    }
    protected int GetRandomInt(int min, int max)
    {
        return random.Next(min, max);
    }

    protected void ddl_AreaCodeA_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddl_AreaCodeB.Items.Clear();
        //ddl_OrganCode.Items.Clear();
        //Dictionary<string, object> aDict = new Dictionary<string, object>();
        //String AreaCodeA = ddl_AreaCodeA.SelectedValue;
        //if (!String.IsNullOrEmpty(AreaCodeA))
        //{
        //    Utility.setAreaCodeB(ddl_AreaCodeB, AreaCodeA, "請選擇");
        //    ddl_AreaCodeB.Enabled = true;
        //}
        //else
        //{
        //    ddl_AreaCodeB.Items.Add(new ListItem("請選擇", ""));
        //    ddl_AreaCodeB.Enabled = false;
        //    ddl_OrganCode.Enabled = false;
        //}
        //ddl_OrganCode.Items.Add(new ListItem("請選擇", ""));
    }

    protected void ddl_AreaCodeB_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddl_OrganCode.Items.Clear();
        //Dictionary<string, object> aDict = new Dictionary<string, object>();
        //String AreaCodeB = ddl_AreaCodeB.SelectedValue;
        //if (!String.IsNullOrEmpty(AreaCodeB))
        //{
        //    Utility.setOrganID(ddl_OrganCode, AreaCodeB, "請選擇");
        //    ddl_OrganCode.Enabled = true;
        //}
        //else
        //{
        //    ddl_OrganCode.Items.Add(new ListItem("請選擇", ""));
        //    ddl_OrganCode.Enabled = false;
        //}
    }

    protected void ddl_OrganCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //HF_OrganSNO.Value = ddl_OrganCode.SelectedValue; ;
        //lb_OrganCodeName.Text = ddl_OrganCode.SelectedItem.Text + "(可使用)";
    }

    protected void btn_submit_ServerClick(object sender, EventArgs e) //送出按鈕
    {

        DataHelper odt = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PAccount", txt_Account.Value);
        aDict.Add("PMail", txt_Mail.Value);
        aDict.Add("OrganCode", txt_Organ.Value);
        DataTable dt_org = odt.queryData("SELECT * FROM Organ WHERE OrganCode=@OrganCode", aDict);
        aDict.Add("OrganSNO", HF_OrganSNO.Value);

        // init 錯誤訊息
        String errorMessage = "";

        // 開始註冊, 複寫資料
        // 檢查資料
        DataTable dt_acc = odt.queryData("SELECT * FROM Person WHERE PAccount=@PAccount", aDict);
        DataTable dt_mail = odt.queryData("SELECT * FROM Person WHERE PMail=@PMail", aDict);
        DataTable dt_PAccount = odt.queryData("SELECT * FROM Person WHERE OrganSNO=@OrganSNO and RoleSNO=18 and IsEnable=1", aDict);
        //if (string.IsNullOrEmpty(ddl_Role.SelectedValue)) errorMessage += "請選擇您的角色！\\n";
        if (string.IsNullOrEmpty(txt_Account.Value)) errorMessage += "請輸入您的帳號！\\n";
        if (string.IsNullOrEmpty(txt_cPswd.Value)) errorMessage += "請輸入您的密碼！\\n";
        if (string.IsNullOrEmpty(txt_Name.Value)) errorMessage += "請輸入您的姓名！\\n";
        //if (string.IsNullOrEmpty(txt_Personid.Value)) errorMessage += "請輸入您的身分證！\\n";
        if (string.IsNullOrEmpty(txt_Mail.Value)) errorMessage += "請輸入您的E-Mail信箱！\\n";
        //if (string.IsNullOrEmpty(txt_Birthday.Value)) errorMessage += "請輸入您的生日！\\n";
        if (txt_Pswd.Value != txt_cPswd.Value) errorMessage += "密碼與密碼確認不相符！\\n";
        if (dt_acc.Rows.Count > 0) errorMessage += "此帳號已申請過！\\n";
        if (dt_mail.Rows.Count > 0) errorMessage += "此信箱已申請過！\\n";
        if (dt_PAccount.Rows.Count > 1) errorMessage += "同一開課單位最多申請2組帳號！\\n";

        // 申請過的才可以
        // if (dt_PersonID.Rows.Count > 0) errorMessage += "此身分證已申請過！\\n";

        if (txt_Account.Value.Length > 50) errorMessage += "帳號字數過多(超過50字)！\\n";
        if (txt_Account.Value.Length < 5) errorMessage += "帳號字數過少(少於5字)！\\n";
        if (txt_cPswd.Value.Length > 50) errorMessage += "密碼字數過多(超過50字)！\\n";
        if (txt_Name.Value.Length > 50) errorMessage += "姓名字數過多(超過50字)！\\n";
        //if (txt_Personid.Value.Length > 10) errorMessage += "身分證字數過多(超過10字)！\\n";
       // if (txt_Birthday.Value.Length > 10) errorMessage += "生日字數過多(超過10字)！\\n";
        if (txt_Tel.Value.Length > 50) errorMessage += "電話字數過多(超過50字)！\\n";
        if (txt_Phone.Value.Length != 10) errorMessage += "手機字數必須10碼)！\\n";
        if (txt_Mail.Value.Length > 50) errorMessage += "E-Mail信箱字數過多(超過50字)！\\n";
       // if (txt_ZipCode.Value.Length > 5) errorMessage += "通訊區號字數過多(超過5字)！\\n";
        //if (txt_Addr.Value.Length > 50) errorMessage += "通訊地址字數過多(超過50字)！\\n";

        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }


        //經上方驗證通過後，可新增至資料庫中
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> dic = new Dictionary<string, object>();

        string sql = "Select *  FROM [CD_AREA] where AREA_CODE=@AREA_CODE";
        aDict.Add("AREA_CODE", dt_org.Rows[0]["AreaCodeB"].ToString());
        DataTable ObjDT = odt.queryData(sql, aDict);
        dic.Add("RoleSNO", "18");
        dic.Add("PAccount", txt_Account.Value);
        dic.Add("IsEnable", 0);
        dic.Add("MStatusSNO", 4);
        dic.Add("CreateDT", DateTime.Now) ;
        dic.Add("CreateUserID", 0);
        dic.Add("LoginError", 0);
        dic.Add("PName", txt_Name.Value);
        dic.Add("PersonID", "");
        dic.Add("PBirthDate", DateTime.Now.ToString("yyyy-MM-dd"));
        dic.Add("PMail", txt_Mail.Value);
        dic.Add("PPhone", txt_Phone.Value);
        dic.Add("PZCode", ObjDT.Rows[0]["ZipCode"].ToString());
        dic.Add("PAddr", dt_org.Rows[0]["OrganAddr"].ToString());
        dic.Add("OrganSNO", HF_OrganSNO.Value);
        //dic.Add("City", txt_cPswd.Value);
        //dic.Add("Area", txt_cPswd.Value);
        dic.Add("PPWD", txt_cPswd.Value);
        
        string sqlperson = @"
            Insert Into Person(
                RoleSNO , PAccount, PName, PersonID, MStatusSNO ,
                PMail, PPhone,PBirthDate,IsEnable,
                PZCode, PAddr , PPWD,OrganSNO,
                LoginError, CreateUserID 
            ) Values(
                @RoleSNO, @PAccount, @PName, UPPER(@PersonID), @MStatusSNO ,
                 @PMail, @PPhone,@PBirthDate,@IsEnable,
                 @PZCode, @PAddr ,@PPWD,@OrganSNO,
                 @LoginError ,@CreateUserID
            )
        ";
        objDH.executeNonQuery(sqlperson, dic);
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Message", "alert('帳號申請成功，請填寫管理者帳號申請單寄送至國民健康署審核!');window.location.href = 'Notice.aspx';", true);



    }

    protected int getSex()
    {
        //if (txt_Personid.Value.Substring(1, 1) == "1" || txt_Personid.Value.Substring(1, 1) == "A" || txt_Personid.Value.Substring(1, 1) == "C") return 1;
        //else if (txt_Personid.Value.Substring(1, 1) == "2" || txt_Personid.Value.Substring(1, 1) == "B" || txt_Personid.Value.Substring(1, 1) == "D") return 0;
        return 0;
    }
    protected void setRole_13_Pannel()
    {
        //if (ddl_Role.SelectedValue == "13") Guardian.Visible = true;
        //else Guardian.Visible = false;
    }
    protected void setRole_11_Pannel()
    {
        //if (ddl_Role.SelectedValue == "11")
        //{
        //    RoleException.Visible = true;
        //    Utility.setRoleException(ddl_RoleExceprion, "請選擇");
        //}
        //else
        //{
        //    RoleException.Visible = false;
        //}
    }
    protected void ddl_Role_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddl_Role.SelectedValue == "10")
        //{
        //    ddl_TSType.Enabled = true;
        //    ddl_TSType.Visible = true;
        //    txt_TSNote.Enabled = true;

        //}
        //else
        //{
        //    ddl_TSType.Visible = false;
        //    txt_TSNote.Visible = false;
        //}
        setRole_13_Pannel();
        setRole_11_Pannel();
    }
    protected static string MP(string PersonID, string ddl_RoleException)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonID", PersonID);
        string sql = @"
        Select 1 from PersonMP where PersonID=@PersonID
            ";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            return "0";
        }
        else if (ddl_RoleException != "")
        {
            return "0";
        }
        else
        {
            return "4";
        }
    }
    protected void ddl_AddressA_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddl_AddressB.Items.Clear();

        //Dictionary<string, object> aDict = new Dictionary<string, object>();
        //String AreaCodeA = ddl_AddressA.SelectedValue;
        //if (!String.IsNullOrEmpty(AreaCodeA))
        //{
        //    Utility.setAreaCodeB(ddl_AddressB, AreaCodeA, "請選擇");
        //    ddl_AddressB.Enabled = true;
        //}
        //else
        //{
        //    ddl_AddressB.Items.Add(new ListItem("請選擇", ""));
        //    ddl_AddressB.Enabled = false;

        //}
    }
    //#region Email驗證
    //protected void btn_Send_Click(object sender, EventArgs e)
    //{
    //    Timer1.Interval = 1000;//設定每秒執行一次
    //    //System.Threading.Thread.Sleep(4000);
    //    lb_Count.Text = Convert.ToString((Convert.ToInt32(lb_Count.Text)) + 1);
    //    string VS = GetRandomInt(0, 9999).ToString();
    //    //btn_Send.Enabled = false;
    //    if (VS != "")
    //    {
    //        Session.Remove("VS");
    //        Session.Add("VS", VS);

    //        if (String.IsNullOrEmpty(txt_Mail.Value))
    //        {
    //            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Message", "alert('請填寫Email');", true);
    //            return;
    //        }
    //        else
    //        {
    //            bool CEmail = checkEmail(txt_Mail.Value);
    //            if (CEmail == false)
    //            {
    //                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Message", "alert('您輸入了重複Email，請使用新的Email進行註冊。');", true);
    //            }
    //            else
    //            {

    //                //btn_Send.Style.Add("background-color", "Gray");
    //                //Timer1.Enabled = true;
    //                string getTemplate = System.IO.File.ReadAllText(Server.MapPath("../SysFile/TemplateMailForRegister.html"));
    //                getTemplate = getTemplate.Replace("@RestDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
    //                getTemplate = getTemplate.Replace("@RestNewPassword", VS);
    //                SendMail("註冊驗證信件", getTemplate, txt_Mail.Value);
    //                hid_vs.Value = VS;
    //                timeLeft = 30;
    //                Timer1.Enabled = true;
    //                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Message", "alert('已送出Email，請至信箱查收。');", true);
    //                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Message", "<script>time(o);</script>", false);
    //                //ScriptManager.RegisterStartupScript(Page, GetType(), "TimeDelay", ScriptDelay, true);
    //                //ScriptManager.RegisterStartupScript(Up_Send, Up_Send.GetType(), "", "YourMailToFunction()", true);

    //            }

    //        }

    //    }
    //}
    //protected void Timer1_Tick(object sender, EventArgs e)
    //{



    //    if (timeLeft > 0)
    //    {
    //        timeLeft = timeLeft - 1;
    //        btn_Send.Text = "請等待" + timeLeft + "秒";
    //        btn_Send.Enabled = false;
    //        btn_Send.BackColor = System.Drawing.Color.DarkGray;
    //        btn_Send.ForeColor = System.Drawing.Color.White;
    //    }
    //    else
    //    {
    //        Timer1.Enabled = false;
    //        btn_Send.Enabled = true;
    //        btn_Send.BackColor = System.Drawing.Color.FromArgb(23, 162, 184);
    //        btn_Send.Text = "重新獲得驗證碼";
    //    }
    //}
    //#endregion



    public void SendMail(string MailSub, string MailBody, string SendTo)
    {

        DataHelper objDH = new DataHelper();
        Dictionary<string, object> Dict = new Dictionary<string, object>();
        Dict.Add("Account", "Account");
        Dict.Add("Host", "Host");
        Dict.Add("Port", "Port");
        Dict.Add("Psw", "Psw");
        Dict.Add("SSL", "SSL");
        DataTable objDT = objDH.queryData(@"
            Select 
                (Select PVal From Config Where PID=@Account) Account,
                (Select PVal From Config Where PID=@Host) Host,
                (Select PVal From Config Where PID=@Port) Port,
                (Select PVal From Config Where PID=@Psw) Psw,
                (Select PVal From Config Where PID=@SSL) SSL
        ", Dict);

        string Account = objDT.Rows[0]["Account"].ToString();
        string Host = objDT.Rows[0]["Host"].ToString();
        int Port = Convert.ToInt16(objDT.Rows[0]["Port"]);
        string Psw = objDT.Rows[0]["Psw"].ToString();
        bool SSL = Convert.ToBoolean(objDT.Rows[0]["SSL"]);

        string smtpServer = Host;
        int smtpPort = Port;
        bool smtpSSL = SSL;
        string MailAccount = Account;
        string MailName = "醫事人員戒菸服務訓練系統";
        string MailPsw = Psw;
        string MailFrom = Account;
        bool isBodyHtml = true;
        MailMessage NewMail = new System.Net.Mail.MailMessage();
        try
        {
            NewMail.SubjectEncoding = System.Text.Encoding.UTF8; //主題編碼格式
            NewMail.Subject = MailSub; //主題
            NewMail.IsBodyHtml = isBodyHtml;  //HTML語法(true:開啟false:關閉)
            NewMail.BodyEncoding = System.Text.Encoding.UTF8; //內文編碼格式
            NewMail.Body = MailBody; //內文
            NewMail.From = new MailAddress(MailFrom, MailName); //發送者

            NewMail.To.Add(SendTo);
            SmtpClient NewSmtp = new SmtpClient(); //建立SMTP連線
            NewSmtp.Credentials = new System.Net.NetworkCredential(MailAccount, MailPsw); //連線驗證
            NewSmtp.Port = smtpPort; //SMTP Port
            NewSmtp.Host = smtpServer; //SMTP主機名稱
            NewSmtp.EnableSsl = smtpSSL; //開啟SSL驗證
            //NewSmtp.UseDefaultCredentials = true;

            NewSmtp.Send(NewMail); //發送
            NewMail.Dispose();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message + "');", true);

        }

    }
    public static bool checkEmail(string Email)
    {
        DataHelper odt = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = "Select 1 from Person where PMail=@Email";
        aDict.Add("Email", Email.Trim());
        DataTable ObjDT = odt.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0) //大於0代表有重複Email
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    protected void ddl_AddressB_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string sql = "Select *  FROM [CD_AREA] where AREA_CODE=@AREA_CODE";
        //DataHelper odt = new DataHelper();
        //Dictionary<string, object> aDict = new Dictionary<string, object>();
        //aDict.Add("AREA_CODE", ddl_AddressB.SelectedValue);
        //DataTable ObjDT = odt.queryData(sql, aDict);
        //txt_ZipCode.Value = ObjDT.Rows[0]["ZipCode"].ToString();
    }

    protected void ddl_TSType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddl_TSType.SelectedItem.Text == "其他")
        //{
        //    txt_TSNote.Visible = true;
        //}
        //else
        //{
        //    txt_TSNote.Visible = false;
        //}

    }

    //protected void ddl_Job_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddl_Job.SelectedItem.Text == "其他")
    //    {
    //        txt_PJNote.Visible = true;
    //    }
    //    else
    //    {
    //        txt_PJNote.Visible = false;
    //    }
    //}


}

