using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Web_Account : System.Web.UI.Page
{

    public string PersonSno = "";
    Random random = new Random();
    //public UserInfo userInfo = null;
    public static int timeLeft = 30;
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            //初始化:角色別
            Utility.setRoleNormal(ddl_Role, "請選擇");
            Utility.setTsTypeAccount(ddl_TSType, "請選擇");
            ddl_TSType.Enabled = false;
            txt_TSNote.Enabled = false;
            txt_TSNote.Visible = false;
            //行政區初始化
            Utility.setAreaCodeA(ddl_AreaCodeA, "請選擇");
            Utility.setAreaCodeA(ddl_AddressA, "請選擇");
            //Utility.setAreaCodeA(ddl_AddressC, "請選擇");
            if (ddl_AreaCodeA.SelectedValue == "")
            {
                ddl_AreaCodeB.Enabled = false;
                ddl_OrganCode.Enabled = false;
            }
            SetCountry(ddl_Country, "Country");
        }
    }
    protected int GetRandomInt(int min, int max)
    {
        return random.Next(min, max);
    }

    protected void ddl_AreaCodeA_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_AreaCodeB.Items.Clear();
        ddl_OrganCode.Items.Clear();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String AreaCodeA = ddl_AreaCodeA.SelectedValue;
        if (!String.IsNullOrEmpty(AreaCodeA))
        {
            Utility.setAreaCodeB(ddl_AreaCodeB, AreaCodeA, "請選擇");
            ddl_AreaCodeB.Enabled = true;
        }
        else
        {
            ddl_AreaCodeB.Items.Add(new ListItem("請選擇", ""));
            ddl_AreaCodeB.Enabled = false;
            ddl_OrganCode.Enabled = false;
        }
        ddl_OrganCode.Items.Add(new ListItem("請選擇", ""));
    }

    protected void ddl_AreaCodeB_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_OrganCode.Items.Clear();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String AreaCodeB = ddl_AreaCodeB.SelectedValue;
        if (!String.IsNullOrEmpty(AreaCodeB))
        {
            Utility.setOrganID(ddl_OrganCode, AreaCodeB, "請選擇");
            ddl_OrganCode.Enabled = true;
        }
        else
        {
            ddl_OrganCode.Items.Add(new ListItem("請選擇", ""));
            ddl_OrganCode.Enabled = false;
        }
    }

    protected void ddl_OrganCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        HF_OrganSNO.Value = ddl_OrganCode.SelectedValue; ;
        lb_OrganCodeName.Text = ddl_OrganCode.SelectedItem.Text + "(可使用)";
    }

    protected void btn_submit_ServerClick(object sender, EventArgs e) //送出按鈕
    {
        string test = ddl_Country.SelectedItem.Text;
        DataHelper odt = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PAccount", txt_Account.Value);
        aDict.Add("PMail", txt_Mail.Value);
        aDict.Add("PersonID", txt_Personid.Value);

        // init 錯誤訊息
        String errorMessage = "";

        // 開始註冊, 複寫資料
        // 檢查資料
        DataTable dt_acc = odt.queryData("SELECT * FROM Person WHERE PAccount=@PAccount", aDict);
        DataTable dt_mail = odt.queryData("SELECT * FROM Person WHERE PMail=@PMail", aDict);
        DataTable dt_personMP = odt.queryData("SELECT * FROM PersonMP WHERE PersonID=@PersonID", aDict);
        if (string.IsNullOrEmpty(ddl_Role.SelectedValue)) errorMessage += "請選擇您的角色！\\n";
        if (string.IsNullOrEmpty(txt_Account.Value)) errorMessage += "請輸入您的帳號！\\n";
        if (string.IsNullOrEmpty(txt_cPswd.Value)) errorMessage += "請輸入您的密碼！\\n";
        if (string.IsNullOrEmpty(txt_Name.Value)) errorMessage += "請輸入您的姓名！\\n";
        if (string.IsNullOrEmpty(txt_Personid.Value)) errorMessage += "請輸入您的身分證！\\n"; 
        if (string.IsNullOrEmpty(txt_Mail.Value)) errorMessage += "請輸入您的E-Mail信箱！\\n";
        if (string.IsNullOrEmpty(txt_Birthday.Value)) errorMessage += "請輸入您的生日！\\n";
        if (txt_Pswd.Value != txt_cPswd.Value) errorMessage += "密碼與密碼確認不相符！\\n";
        if (dt_acc.Rows.Count > 0) errorMessage += "此帳號已申請過！\\n";
        if (dt_mail.Rows.Count > 0) errorMessage += "此信箱已申請過！\\n";

        // 申請過的才可以
        // if (dt_PersonID.Rows.Count > 0) errorMessage += "此身分證已申請過！\\n";

        if (txt_Account.Value.Length > 50) errorMessage += "帳號字數過多(超過50字)！\\n";
        if (txt_Account.Value.Length < 5) errorMessage += "帳號字數過少(少於5字)！\\n";
        if (txt_cPswd.Value.Length > 50) errorMessage += "密碼字數過多(超過50字)！\\n";
        if (txt_Name.Value.Length > 50) errorMessage += "姓名字數過多(超過50字)！\\n";
        if (txt_Personid.Value.Length > 10) errorMessage += "身分證字數過多(超過10字)！\\n";
        if (txt_Personid.Value != "")
        {
            //檢查身分證
            if (!ValidIsID(txt_Personid.Value))
            {
                if (!ValidIsID2(txt_Personid.Value) && !ValidIsNID3(txt_Personid.Value.Trim().Substring(0, 1), txt_Personid.Value.Trim().Substring(1, txt_Personid.Value.Trim().Length - 1)))
                {
                    errorMessage += "身分證/居留證格式有誤！\\n";
                }
            }
        }
        if (txt_Birthday.Value.Length > 10) errorMessage += "生日字數過多(超過10字)！\\n";
        if (txt_Tel.Value.Length > 50) errorMessage += "電話字數過多(超過50字)！\\n";
        if (txt_Phone.Value.Length != 10) errorMessage += "手機字數必須10碼)！\\n";
        if (txt_Mail.Value.Length > 50) errorMessage += "E-Mail信箱字數過多(超過50字)！\\n";
        if (txt_ZipCode.Value.Length > 5) errorMessage += "通訊區號字數過多(超過5字)！\\n";
        if (txt_Addr.Value.Length > 50) errorMessage += "通訊地址字數過多(超過50字)！\\n";
        if (dt_personMP.Rows.Count == 0) errorMessage += "帳號申請僅限醫事人員！\\n 「因人員身分辨識資訊係來自醫事管理系統，轉檔會有時間落差，如您本身具醫事人員或公共衛生師資格，但無法申請帳密，請洽客服協助，謝謝」";

        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }


        //經上方驗證通過後，可新增至資料庫中
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> dic = new Dictionary<string, object>();

        //dic.Add("Note", txt_RoleException.Text);
        dic.Add("RoleLevel", ddl_Role.SelectedValue);
        dic.Add("RoleException", ddl_RoleExceprion.SelectedValue);
        dic.Add("Note", txt_RoleException.Text);
        dic.Add("OrganSNO", HF_OrganSNO.Value);
        dic.Add("PAccount", txt_Account.Value);
        dic.Add("PPWD", txt_cPswd.Value);
        dic.Add("PName", txt_Name.Value);
        dic.Add("PersonID", txt_Personid.Value.ToUpper());
        dic.Add("PAddr", txt_Addr.Value);
        dic.Add("PZCode", txt_ZipCode.Value);
        dic.Add("PBirthDate", txt_Birthday.Value);
        dic.Add("PSex", getSex());
        dic.Add("PTel", txt_Tel.Value);
        dic.Add("Country", ddl_Country.SelectedItem.Text);
        dic.Add("Degree", txt_degree.Value);
        dic.Add("PMail", txt_Mail.Value);
        dic.Add("PPhone", txt_Phone.Value);
        dic.Add("City", ddl_AddressA.SelectedItem.Text);
        dic.Add("Area", ddl_AddressB.SelectedItem.Text);
        dic.Add("LoginError", 0);
        dic.Add("CreateUserID", 0);
        dic.Add("TJobType", txt_TJobType.Value);
        dic.Add("TSType", txt_TSType.Value);
        dic.Add("TSSNO", ddl_TSType.SelectedValue);
        dic.Add("TSNote", txt_TSNote.Text);
        int MStatusSNO = Convert.ToInt16(MP(txt_Personid.Value, ddl_RoleExceprion.SelectedValue));
        dic.Add("MStatusSNO", MStatusSNO);
        string sqlperson = @"
            Insert Into Person(
                RoleSNO , RoleException , OrganSNO, PAccount, PPWD, PName, PersonID, MStatusSNO ,
                PBirthDate, PSex, PTel, PMail, PPhone,
                PZCode, PAddr ,  Country, Degree, 
                TJobType, TSType, LoginError, CreateUserID , City ,  Area , Note ,TSSNO,TSNote
            ) Values(
                @RoleLevel , @RoleException , @OrganSNO, @PAccount, @PPWD, @PName, UPPER(@PersonID), @MStatusSNO ,
                @PBirthDate, @PSex, @PTel ,@PMail, @PPhone,
                 @PZCode, @PAddr ,@Country, @Degree,
                @TJobType, @TSType, @LoginError ,@CreateUserID,@City,@Area , @Note ,@TSSNO,@TSNote
            )
        ";
        objDH.executeNonQuery(sqlperson, dic);
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Message", "alert('送出成功!');window.location.href = 'Notice.aspx';", true);



    }

    protected int getSex()
    {
        if (txt_Personid.Value.Substring(1, 1) == "1" || txt_Personid.Value.Substring(1, 1) == "A" || txt_Personid.Value.Substring(1, 1) == "C") return 1;
        else if (txt_Personid.Value.Substring(1, 1) == "2" || txt_Personid.Value.Substring(1, 1) == "B" || txt_Personid.Value.Substring(1, 1) == "D") return 0;
        return 0;
    }
    protected void setRole_13_Pannel()
    {
        if (ddl_Role.SelectedValue == "13") Guardian.Visible = true;
        else Guardian.Visible = false;
    }
    protected void setRole_11_Pannel()
    {
        if (ddl_Role.SelectedValue == "11")
        {
            RoleException.Visible = true;
            Utility.setRoleException(ddl_RoleExceprion, "請選擇");
        }
        else
        {
            RoleException.Visible = false;
        }
    }
    public static void SetCountry(DropDownList ddl, string ddlType, string DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("  Select PVal,MVal from Config where PGroup='Country' ", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }
    protected void ddl_Role_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_Role.SelectedValue == "10")
        {
            ddl_TSType.Enabled = true;
            ddl_TSType.Visible = true;
            txt_TSNote.Enabled = true;

        }
        else
        {
            ddl_TSType.Visible = false;
            txt_TSNote.Visible = false;
        }
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
        ddl_AddressB.Items.Clear();

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String AreaCodeA = ddl_AddressA.SelectedValue;
        if (!String.IsNullOrEmpty(AreaCodeA))
        {
            Utility.setAreaCodeB(ddl_AddressB, AreaCodeA, "請選擇");
            ddl_AddressB.Enabled = true;
        }
        else
        {
            ddl_AddressB.Items.Add(new ListItem("請選擇", ""));
            ddl_AddressB.Enabled = false;

        }
    }
    #region Email驗證
    protected void btn_Send_Click(object sender, EventArgs e)
    {
        Timer1.Interval = 1000;//設定每秒執行一次
        //System.Threading.Thread.Sleep(4000);
        lb_Count.Text = Convert.ToString((Convert.ToInt32(lb_Count.Text)) + 1);
        string VS = GetRandomInt(0, 9999).ToString();
        //btn_Send.Enabled = false;
        if (VS != "")
        {
            Session.Remove("VS");
            Session.Add("VS", VS);

            if (String.IsNullOrEmpty(txt_Mail.Value))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Message", "alert('請填寫Email');", true);
                return;
            }
            else
            {
                bool CEmail = checkEmail(txt_Mail.Value);
                if (CEmail == false)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Message", "alert('您輸入了重複Email，請使用新的Email進行註冊。');", true);
                }
                else
                {

                    //btn_Send.Style.Add("background-color", "Gray");
                    //Timer1.Enabled = true;
                    string getTemplate = System.IO.File.ReadAllText(Server.MapPath("../SysFile/TemplateMailForRegister.html"));
                    getTemplate = getTemplate.Replace("@RestDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    getTemplate = getTemplate.Replace("@RestNewPassword", VS);
                    SendMail("註冊驗證信件", getTemplate, txt_Mail.Value);
                    hid_vs.Value = VS;
                    timeLeft = 30;
                    Timer1.Enabled = true;
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Message", "alert('已送出Email，請至信箱查收。');", true);
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Message", "<script>time(o);</script>", false);
                    //ScriptManager.RegisterStartupScript(Page, GetType(), "TimeDelay", ScriptDelay, true);
                    //ScriptManager.RegisterStartupScript(Up_Send, Up_Send.GetType(), "", "YourMailToFunction()", true);

                }

            }

        }
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {



        if (timeLeft > 0)
        {
            timeLeft = timeLeft - 1;
            btn_Send.Text = "請等待" + timeLeft + "秒";
            btn_Send.Enabled = false;
            btn_Send.BackColor = System.Drawing.Color.DarkGray;
            btn_Send.ForeColor = System.Drawing.Color.White;
            btn_Send.ForeColor = System.Drawing.Color.White;
        }
        else
        {
            Timer1.Enabled = false;
            btn_Send.Enabled = true;
            btn_Send.BackColor = System.Drawing.Color.FromArgb(23, 162, 184);
            btn_Send.Text = "重新獲得驗證碼";
        }
    }
    #endregion



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
    protected void btn_SMSSend_Click(object sender, EventArgs e)
    {
        timer.Enabled = true;
        lb_SCount.Text = Convert.ToString((Convert.ToInt32(lb_SCount.Text)) + 1);
        string VSS = GetRandomInt(0, 9999).ToString();
        if (VSS != "")
        {
            Session.Remove("VSS");
            Session.Add("VSS", VSS);

            if (String.IsNullOrEmpty(txt_Phone.Value))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Message", "alert('請填寫手機號碼');", true);
                return;
            }
            else
            {
                string SendSmsTo = txt_Phone.Value;
                //string SMStempPath = "C:\\IISI\\戒菸服務醫事人員管理系統\\01_Code\\QSMS_dev\\SMSTemp\\SendSMSList.txt";
                string SMStempFile = Server.MapPath("../SMSTemp/SendSMSList.txt");
                if (!File.Exists(SMStempFile))
                {
                    using (StreamWriter streamWriter = new StreamWriter(SMStempFile, true, Encoding.UTF8))
                    {
                        string[] Array_SendSms = SendSmsTo.Split(',');
                        for (int i = 0; i < Array_SendSms.Length; i++)
                        {
                            streamWriter.WriteLine("[" + 100 + i + "]");
                            streamWriter.WriteLine("dstaddr=" + Array_SendSms[i] + "");
                            streamWriter.WriteLine("smbody=" + "醫事人員戒菸服務訓練系統註冊簡訊為：" + VSS);
                        }


                    }
                }

                Utility.sendSMS(SMStempFile);
                hid_vsS.Value = VSS;
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Message", "$.unblockUI();", true);



            }



        }
        btn_SMSSend.BackColor = System.Drawing.Color.Gray;
        btn_SMSSend.ForeColor = System.Drawing.Color.Black;
        btn_SMSSend.Enabled = false;

    }
    protected void timer_Tick(object sender, EventArgs e)
    {
        btn_SMSSend.Enabled = true;
        btn_SMSSend.BackColor = System.Drawing.Color.Orange;
        btn_SMSSend.ForeColor = System.Drawing.Color.Black;
        //timer.Dispose();
        timer.Enabled = false;
    }
    protected void ddl_AddressB_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sql = "Select *  FROM [CD_AREA] where AREA_CODE=@AREA_CODE";
        DataHelper odt = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("AREA_CODE", ddl_AddressB.SelectedValue);
        DataTable ObjDT = odt.queryData(sql, aDict);
        txt_ZipCode.Value = ObjDT.Rows[0]["ZipCode"].ToString();
    }

    protected void ddl_TSType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_TSType.SelectedItem.Text == "其他")
        {
            txt_TSNote.Visible = true;
        }
        else
        {
            txt_TSNote.Visible = false;
        }

    }
    /// <summary>
    /// 身份證格式驗證
    /// </summary>
    /// <param name="ID">指定的字符串</param>
    /// <returns>是否符合身份證號格式</returns>
    public static bool ValidIsID(string ID)
    {
        #region 長度為10
        if (string.IsNullOrEmpty(ID) || ID.Length != 10)
        {
            return false;
        }
        #endregion

        #region 檢查基本字串格式 1位英文字+9位數字
        string regextext = @"^[a-z]{1}(1|2){1}\d{8}$";
        if (!Regex.IsMatch(ID, regextext, RegexOptions.IgnoreCase))
        {
            return false;
        }
        #endregion

        #region 英文字轉碼
        string _strID = "";
        switch (ID.Substring(0, 1).ToUpper())
        {
            case "A":
                _strID = "10";
                break;
            case "B":
                _strID = "11";
                break;
            case "C":
                _strID = "12";
                break;
            case "D":
                _strID = "13";
                break;
            case "E":
                _strID = "14";
                break;
            case "F":
                _strID = "15";
                break;
            case "G":
                _strID = "16";
                break;
            case "H":
                _strID = "17";
                break;
            case "I":
                _strID = "34";
                break;
            case "J":
                _strID = "18";
                break;
            case "K":
                _strID = "19";
                break;
            case "L":
                _strID = "20";
                break;
            case "M":
                _strID = "21";
                break;
            case "N":
                _strID = "22";
                break;
            case "O":
                _strID = "35";
                break;
            case "P":
                _strID = "23";
                break;
            case "Q":
                _strID = "24";
                break;
            case "R":
                _strID = "25";
                break;
            case "S":
                _strID = "26";
                break;
            case "T":
                _strID = "27";
                break;
            case "U":
                _strID = "28";
                break;
            case "V":
                _strID = "29";
                break;
            case "W":
                _strID = "32";
                break;
            case "X":
                _strID = "30";
                break;
            case "Y":
                _strID = "31";
                break;
            case "Z":
                _strID = "33";
                break;
        }


        if (string.IsNullOrEmpty(_strID))
        {
            return false;
        }
        #endregion

        #region 檢查第2碼須為1或2
        if (ID.Substring(1, 1) != "1" && ID.Substring(1, 1) != "2")
        {
            return false;
        }
        #endregion

        #region 邏輯檢核
        int total = Convert.ToInt32(_strID.Substring(0, 1)) * 1 + Convert.ToInt32(_strID.Substring(1, 1)) * 9 +
                Convert.ToInt32(ID.Substring(1, 1)) * 8 + Convert.ToInt32(ID.Substring(2, 1)) * 7 +
                Convert.ToInt32(ID.Substring(3, 1)) * 6 + Convert.ToInt32(ID.Substring(4, 1)) * 5 +
                Convert.ToInt32(ID.Substring(5, 1)) * 4 + Convert.ToInt32(ID.Substring(6, 1)) * 3 +
                Convert.ToInt32(ID.Substring(7, 1)) * 2 + Convert.ToInt32(ID.Substring(8, 1)) * 1;

        int tmod = total % 10;
        if (tmod == 0 && tmod == Convert.ToInt32(ID.Substring(9, 1)))
        {
            return true;
        }
        else if ((10 - tmod) == Convert.ToInt32(ID.Substring(9, 1)))
        {
            return true;
        }

        return false;
        #endregion
    }

    /// <summary>
    /// 舊式統號驗證
    /// </summary>
    /// <param name="ID">指定的字符串</param>
    /// <returns>是否符合舊式統號格式</returns>
    public static bool ValidIsID2(string ID)
    {
        #region 長度為10
        if (string.IsNullOrEmpty(ID) || ID.Length != 10)
        {
            return false;
        }
        #endregion

        #region 檢查基本字串格式 1位英文字+abcdb任一碼+8位數字
        string regextext = @"^[a-zA-Z]{1}(a|b|c|d|A|B|C|D){1}\d{8}$";
        if (!Regex.IsMatch(ID, regextext, RegexOptions.IgnoreCase))
        {
            return false;
        }
        #endregion

        #region 英文字轉碼
        string[] _strID = new string[10];
        for (int i = 0; i < 10; i++)
        {
            _strID[i] = ID.Substring(i, 1);
        }

        int[] _intID = new int[10];
        string FirstLetter = GetLetterNum(_strID[0]);
        string SecondLetter = GetLetterNum(_strID[1]);
        if (string.IsNullOrEmpty(FirstLetter) || string.IsNullOrEmpty(SecondLetter))
        {
            return false;
        }
        #endregion

        #region 邏輯檢核
        _intID[0] = int.Parse(FirstLetter.Substring(0, 1));
        _intID[1] = int.Parse(FirstLetter.Substring(1, 1));
        _intID[2] = int.Parse(SecondLetter.Substring(1, 1));
        _intID[3] = int.Parse(_strID[2]);
        _intID[4] = int.Parse(_strID[3]);
        _intID[5] = int.Parse(_strID[4]);
        _intID[6] = int.Parse(_strID[5]);
        _intID[7] = int.Parse(_strID[6]);
        _intID[8] = int.Parse(_strID[7]);
        _intID[9] = int.Parse(_strID[8]);

        int total = GetSpecialMultiplicationValue(_intID[0], 1) + GetSpecialMultiplicationValue(_intID[1], 9) +
            GetSpecialMultiplicationValue(_intID[2], 8) + GetSpecialMultiplicationValue(_intID[3], 7) +
            GetSpecialMultiplicationValue(_intID[4], 6) + GetSpecialMultiplicationValue(_intID[5], 5) +
            GetSpecialMultiplicationValue(_intID[6], 4) + GetSpecialMultiplicationValue(_intID[7], 3) +
            GetSpecialMultiplicationValue(_intID[8], 2) + GetSpecialMultiplicationValue(_intID[9], 1);

        int tmod = int.Parse(total.ToString().Substring(total.ToString().Length - 1, 1));
        if (tmod == 0 && tmod == Convert.ToInt32(_strID[9]))
        {
            return true;
        }
        else if ((10 - tmod) == Convert.ToInt32(_strID[9]))
        {
            return true;
        }
        return false;
        #endregion
    }
    public static string GetLetterNum(string Letter)
    {
        if (string.IsNullOrEmpty(Letter) || Letter.Length != 1)
        {
            return "";
        }
        string _strID = "";
        switch (Letter.ToUpper())
        {
            case "A":
                _strID = "10";
                break;
            case "B":
                _strID = "11";
                break;
            case "C":
                _strID = "12";
                break;
            case "D":
                _strID = "13";
                break;
            case "E":
                _strID = "14";
                break;
            case "F":
                _strID = "15";
                break;
            case "G":
                _strID = "16";
                break;
            case "H":
                _strID = "17";
                break;
            case "J":
                _strID = "18";
                break;
            case "K":
                _strID = "19";
                break;
            case "L":
                _strID = "20";
                break;
            case "M":
                _strID = "21";
                break;
            case "N":
                _strID = "22";
                break;
            case "P":
                _strID = "23";
                break;
            case "Q":
                _strID = "24";
                break;
            case "R":
                _strID = "25";
                break;
            case "S":
                _strID = "26";
                break;
            case "T":
                _strID = "27";
                break;
            case "U":
                _strID = "28";
                break;
            case "V":
                _strID = "29";
                break;
            case "X":
                _strID = "30";
                break;
            case "Y":
                _strID = "31";
                break;
            case "W":
                _strID = "32";
                break;
            case "Z":
                _strID = "33";
                break;
            case "I":
                _strID = "34";
                break;
            case "O":
                _strID = "35";
                break;

        }
        return _strID;
    }
    public static int GetSpecialMultiplicationValue(int value, int compareValue)
    {
        string strValue = (value * compareValue).ToString();
        int reValue = int.Parse(strValue.Substring(strValue.Length - 1, 1));
        return reValue;
    }

    /// <summary>
    /// 新式統號驗證
    /// </summary>
    /// <param name="firstLetter">第1碼英文字母(區域碼)</param>
    /// <param name="num">第2碼(性別碼) + 第3~9流水號 + 第10碼檢查碼</param>
    /// <returns>是否符合新式統號格式</returns>
    public static bool ValidIsNID3(string firstLetter, string num)
    {
        char[] myArr = num.ToCharArray();
        Array.Reverse(myArr);
        Regex eng = new Regex("^[A-Z]+$");
        Regex number = new Regex("^[0-9]+$");
        if (num.Length < 9 || (!eng.IsMatch(firstLetter)) || (!number.IsMatch(new string(myArr))))
        {
            return false;
        }

        ///建立字母對應表(A~Z)
        ///A=10 B=11 C=12 D=13 E=14 F=15 G=16 H=17 J=18 K=19 L=20 M=21 N=22
        ///P=23 Q=24 R=25 S=26 T=27 U=28 V=29 X=30 Y=31 W=32  Z=33 I=34 O=35 
        string alphabet = "ABCDEFGHJKLMNPQRSTUVXYWZIO";
        string transferIdNo = "" + (alphabet.IndexOf(firstLetter) + 10) + "" + num + "";

        int[] idNoArray = transferIdNo.ToCharArray()
                                      .Select(c => Convert.ToInt32(c.ToString()))
                                      .ToArray();

        int sum = idNoArray[0];
        int checkSum = 0;
        int[] weight = new int[] { 1, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        for (int i = 0; i < weight.Length; i++)
        {
            checkSum += (weight[i] * idNoArray[i]) % 10;
        }
        int checkNum = 10 - (checkSum % 10) == 10 ? 0 : 10 - (checkSum % 10);

        return checkNum == Convert.ToInt32(new string(myArr).Substring(0, 1));

    }
    /// <summary>
    /// 護照格式驗證
    /// </summary>
    /// <param name="ID">指定的字符串</param>
    /// <returns>是否符合護照格式</returns>
    public static bool ValidIsID3(string ID)
    {
        bool Result = false;
        if (string.IsNullOrEmpty(ID) || ID.Length != 9)//長度為9
            Result = false;
        else
        {
            //檢查基本字串格式 X或數字+8位數字
            string regextext = @"^(x|X|\d){1}\d{8}$";
            if (Regex.IsMatch(ID, regextext, RegexOptions.IgnoreCase))
                Result = true;
            else
                Result = false;
        }

        return Result;
    }
    private bool checkId(string user_id, string state) //檢查身分證字號
    {
        int[] uid = new int[10]; //數字陣列存放身分證字號用
        int chkTotal; //計算總和用

        if (user_id.Length == 10) //檢查長度
        {
            user_id = user_id.ToUpper(); //將身分證字號英文改為大寫

            //將輸入的值存入陣列中
            for (int i = 1; i < user_id.Length; i++)
            {
                uid[i] = Convert.ToInt32(user_id.Substring(i, 1));
            }
            //將開頭字母轉換為對應的數值
            switch (user_id.Substring(0, 1).ToUpper())
            {
                case "A": uid[0] = 10; break;
                case "B": uid[0] = 11; break;
                case "C": uid[0] = 12; break;
                case "D": uid[0] = 13; break;
                case "E": uid[0] = 14; break;
                case "F": uid[0] = 15; break;
                case "G": uid[0] = 16; break;
                case "H": uid[0] = 17; break;
                case "I": uid[0] = 34; break;
                case "J": uid[0] = 18; break;
                case "K": uid[0] = 19; break;
                case "L": uid[0] = 20; break;
                case "M": uid[0] = 21; break;
                case "N": uid[0] = 22; break;
                case "O": uid[0] = 35; break;
                case "P": uid[0] = 23; break;
                case "Q": uid[0] = 24; break;
                case "R": uid[0] = 25; break;
                case "S": uid[0] = 26; break;
                case "T": uid[0] = 27; break;
                case "U": uid[0] = 28; break;
                case "V": uid[0] = 29; break;
                case "W": uid[0] = 32; break;
                case "X": uid[0] = 30; break;
                case "Y": uid[0] = 31; break;
                case "Z": uid[0] = 33; break;
            }
            //檢查第一個數值是否為1.2(判斷性別)
            if (uid[1] == 1 || uid[1] == 2)
            {
                chkTotal = (uid[0] / 10 * 1) + (uid[0] % 10 * 9);

                int k = 8;
                for (int j = 1; j < 9; j++)
                {
                    chkTotal += uid[j] * k;
                    k--;
                }

                chkTotal += uid[9];

                if (chkTotal % 10 != 0)
                {
                    return false;
                }
                else { return true; }
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

