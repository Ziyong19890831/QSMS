using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Mail;

public partial class Mgt_Event_AE : System.Web.UI.Page
{
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfo;
    public Dictionary<string, string> _SetCol;
    UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Utility.setAudit(ddl_Status, "請選擇");
            Utility.setEventInvite(ddl_EventInvite, "請選擇");
            binData();
            if (userInfo.RoleSNO == "2")
            {
                Role_view.Visible = true;
                Tabs_2.Visible = true;
            }
            else if (userInfo.RoleSNO == "3")
            {
                Role_view.Visible = true;
                Tabs_2.Visible = true;
            }
        }

    }

    protected void binData()
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();

        aDict.Add("EventSNO", id);
        string isExchange = Request.QueryString["Exchange"];
        if (isExchange == "1")
        {
            lb_Map1.Text = "繼續教育管理";
            lb_Map2.Text = "繼續教育報名管理";
        }

        string CtypeSNOSQL = @"SELECT 
                                	  isnull(QCPC.CTypeSNO,0) CTypeSNO
                                  FROM [Event] E
                                  Left Join QS_ECoursePlanningClass QEPC On QEPC.EPClassSNO=E.EPClassSNO
                                  Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO=QEPC.PClassSNO
                                  where E.EventSNO=@EventSNO";
        DataTable ObjDTS = objDH.queryData(CtypeSNOSQL, aDict);

        String sql = "";


        //取活動名稱
        sql = @"
                SELECT
                     a.EventName, a.CountLimit , a.CountAdmit
                    ,(Select count(1) From EventD ed Where ed.EventSNO = a.EventSNO) pCount
                    ,(Select count(1) From EventD ed Where ed.EventSNO=a.EventSNO AND ed.Audit=0 ) unAdmit
                    ,(Select count(1) From EventD ed Where ed.EventSNO=a.EventSNO AND ed.Audit=1 ) Admitted
                    ,(Select count(1) From EventD ed Where ed.EventSNO=a.EventSNO AND ed.Audit=2 ) Waiting
                    ,(Select count(1) From EventD ed Where ed.EventSNO=a.EventSNO AND ed.Audit=3 ) Chack
                    ,(Select count(1) From EventD ed Where ed.EventSNO=a.EventSNO AND ed.Audit=4 ) Ready
                    ,(Select count(1) From EventD ed Where ed.EventSNO=a.EventSNO AND ed.Audit=5 ) Cancel
                FROM Event A WHERE A.EventSNO = @EventSNO";
        DataTable EventName = objDH.queryData(sql, aDict);
        if (EventName.Rows.Count > 0)
        {
            lbl_EventName.Text = Convert.ToString(EventName.Rows[0]["EventName"]);
            lbl_Limit.Text = Convert.ToString(EventName.Rows[0]["CountLimit"]);
            lbl_Admit.Text = Convert.ToString(EventName.Rows[0]["CountAdmit"]);
            lbl_AdmitCtn.Text = Convert.ToString(EventName.Rows[0]["pCount"]);
            lbl_UnAdmit.Text = Convert.ToString(EventName.Rows[0]["unAdmit"]);
            lbl_Admitted.Text = Convert.ToString(EventName.Rows[0]["Admitted"]);
            lbl_Waiting.Text = Convert.ToString(EventName.Rows[0]["Waiting"]);
            lbl_Chack.Text = Convert.ToString(EventName.Rows[0]["Chack"]);
            lbl_Ready.Text = Convert.ToString(EventName.Rows[0]["Ready"]);
            lbl_cancel.Text = Convert.ToString(EventName.Rows[0]["Cancel"]);

        }

        aDict.Add("CountAdmit", EventName.Rows[0]["CountAdmit"]);

        string gv_SQL = @"
                 SELECT 
                    ROW_NUMBER() OVER (ORDER BY e.CreateDT ) as ROW_NO, e.EventDSNO ,
	                p.PersonSNO,p.PName, p.PersonID,c3.MVal PSex, MP.JCN , p.PMail, p.PTel, p.PPhone, CONVERT(varchar(100), P.PBirthDate, 111) PBirthDate, e.Notice,P.PAddr,
					e.DLOADSNO,D.DLOADNAME,D.DLOADURL,P.Area,P.City,
	                r.RoleName, Convert(varchar(16), e.CreateDT, 120) ApplyDT,
                    c1.MVal EventAudit, c1.PVal EventAuditVal ,
	                c2.MVal EventNotice,MP.LSType,MP.LSCN,O.OrganName,O.OrganAddr,O.OrganTel,c4.MVal MStatusSNO,Case EventInvite when 0 Then '自行報名' when 1 Then '協助報名' Else '協助報名' END   EventInvite
					,c1.PVal AuditCode,c1.MVal AuditText,e.ScoreNote,case (e.Meals) when '0' then '不需要' when '1' then '葷食' when '2' then '素食' ELSE '無填寫' END Meals 
                 ,STUFF((
            select (TEMP.CtypeString+','+TEMP.CertPublicDate+','+TEMP.CertStartDate+','+TEMP.CertEndDate+','+TEMP.CUnitName+';' )
			from
			(
                SELECT 
				Case when SysChange=1 then
                replace(CT.CTypeString,'@',RIGHT(REPLICATE('0', 6) + CAST(C.CertID as NVARCHAR), 6) )
				Else
				 replace(CT.CTypeString,'@',C.CertID)
				 END as CtypeString
				  ,
                Cast(C.CertPublicDate As varchar(10)) CertPublicDate,
                Cast(C.CertStartDate As varchar(10)) CertStartDate,
                Cast(C.CertEndDate As varchar(10)) CertEndDate,
                Case C.CertExt When 1 Then '有' Else '無' End CertExt,
				CU.CUnitName
                From  QS_Certificate C
				LEFT JOIN QS_CertificateType CT on CT.CTypeSNO=C.CTypeSNO
                LEFT JOIN QS_CertificateUnit CU on CU.CUnitSNO=C.CUnitSNO
                WHERE 1=1 And  PersonID=p.PersonID
                 ) as TEMP				 
		        For XML PATH('')),1,0,'') as 證書字號資料,e.CompletionClass1,e.CompletionCertificateType,e.Note,p.JMajor,MP.LRType,MP.LSType,OC.ClassName
                From EventD e
	                Left Join Person p On p.PersonSNO = e.PersonSNO
	                Left Join Role r On r.RoleSNO = p.RoleSNO
	                Left Join Config c1 On c1.PVal = e.Audit And c1.PGroup='EventAudit'
	                Left Join Config c2 On c2.PVal = e.NoticeType And c2.PGroup='EventNotice'
					Left Join Config c3 On c3.PVal = P.PSex And c3.PGroup='Sex'
					Left Join Config c4 On c4.PVal = P.MStatusSNO And c4.PGroup='Mstatus'
                    Left Join PersonMP MP ON MP.personID=P.PersonID
					Left Join Download D On e.DLOADSNO=D.DLOADSNO
					Left Join Organ O On O.OrganSNO=P.OrganSNO
                    Left Join Organ OMP On OMP.OrganCode=MP.OrganCode
                    Left Join [OrganClass] OC On OC.ClassSNO=OMP.[OrganClass]
                Where e.EventSNO = @EventSNO  ";
 //       if (isExchange == "1")
 //       {
 //           gv_SQL += "and QC.CertExt = 0 ";
 //           gv_SQL += "and QC.CtypeSNO = @CtypeSNO ";
 //           aDict.Add("CtypeSNO", ObjDTS.Rows[0]["CtypeSNO"].ToString());
 //       }
        //if (ObjDTS.Rows[0]["CtypeSNO"].ToString() != "0")
        //{
        //    string CtypeSNO = ObjDTS.Rows[0]["CTypeSNO"].ToString(); //取得證書
        //    gv_SQL += " And QC.CTypeSNO = @CTypeSNO ";
        //    aDict.Add("CTypeSNO", CtypeSNO);
        //}
        if (!String.IsNullOrEmpty(txt_Name.Text))
        {
            gv_SQL += " And P.PName  Like '%' + @PName + '%' ";
            aDict.Add("PName", txt_Name.Text);
        }
        if (!String.IsNullOrEmpty(txt_Phone.Text))
        {
            gv_SQL += " And P.PPhone = @PPhone ";
            aDict.Add("PPhone", txt_Phone.Text);
        }
        if (!String.IsNullOrEmpty(txt_Mail.Text))
        {
            gv_SQL += " And P.PMail = @PMail ";
            aDict.Add("PMail", txt_Mail.Text);
        }
        if (!String.IsNullOrEmpty(txt_personID.Text))
        {
            gv_SQL += " And P.PersonID = @PersonID ";
            aDict.Add("PersonID", txt_personID.Text);
        }
        if (!String.IsNullOrEmpty(txt_JSN.Text))
        {
            gv_SQL += " And MP.JCN  Like '%' + @JCN + '%' ";
            aDict.Add("JCN", txt_JSN.Text);
        }
        if (!String.IsNullOrEmpty(ddl_Status.SelectedValue))
        {
            gv_SQL += " And e.Audit=@Audit ";
            aDict.Add("Audit", ddl_Status.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_EventInvite.SelectedValue))
        {
            gv_SQL += " And EventInvite=@EventInvite ";
            aDict.Add("EventInvite", ddl_EventInvite.SelectedValue);
        }
        gv_SQL += " ORDER BY e.CreateDT";
        //取報名資料
        DataTable objDT = objDH.queryData(gv_SQL, aDict);
        gv_EventD.DataSource = objDT.DefaultView;
        gv_EventD.DataBind();
        //匯出資料
        ReportInit(objDT);

        DateTime Date = DateTime.Now;
        Dictionary<string, object> bDict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        string EventSNO = Request.QueryString["sno"];
        string SQL = @"select * from Event E
                     Left Join EventBatch EB on EB.EventSNO=E.EventSNO
                    where E.EventSNO=@EventSNO ";
        bDict.Add("EventSNO", EventSNO);
        DataTable ObjDT = ObjDH.queryData(SQL, bDict);
        DateTime CStartDay = Convert.ToDateTime(ObjDT.Rows[0]["CStartDay"]).AddDays(14);
        if (Date > CStartDay)
        {
            btn_invite.Disabled = true;
            btn_invite.Attributes.CssStyle.Add("background", " #dddddd;");
        }


    }

    protected async void btnSendMail_ClickAsync(object sender, EventArgs e)
    {
        string EventSNO = Request.QueryString["sno"].ToString() == null ? "" : Request.QueryString["sno"].ToString();
        string querystring = Request.QueryString["sno"];
        string Strsql = "";
        string Name = "";
        string EventName = lbl_EventName.Text + "-" + DateTime.Now.ToShortDateString();
        string sn = Email.CreateGroup(EventName);
        DataHelper objDH = new DataHelper();

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        if (Send_List1.SelectedValue == "0")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('請選擇寄送對象。');", true);
        }
        else
        {
            if (Send_List1.SelectedValue == "1")//錄取
            {
                string send_list = Send_List1.SelectedItem.Value;
                Strsql = @"select EventD.EventDSNO,Person.PersonSNO,Person.PName,EventD.EventSNO,EventD.Audit,EventD.PersonSNO,person.PPhone,person.PMail ";
                Strsql += " from EventD ";
                Strsql += " JOIN Person On EventD.PersonSNO=Person.PersonSNO";
                Strsql += " where EventD.EventSNO=@querystring and person.PMail <> '' ";
                Strsql += " and EventD.Audit=1";

                aDict.Add("querystring", querystring);
                DataTable SendEmailList = objDH.queryData(Strsql, aDict);

                if (SendEmailList.Rows.Count == 0)
                {
                    Response.Write("<script>alert('無錄取人員!');</script>");
                    return;
                }
                for (int i = 0; i < SendEmailList.Rows.Count; i++)
                {
                    //Name += "\n" + SendEmailList.Rows[i]["PMail"].ToString() + ",";
                    string MailContent = editor_Mail.Value;
                    SendMail(EventName, MailContent, SendEmailList.Rows[i]["PMail"].ToString());
                }
                //修改通知狀態
                for (int i = 0; i < SendEmailList.Rows.Count; i++)
                {
                    string PersonSNO = SendEmailList.Rows[i]["PersonSNO"].ToString();
                    Utility.ChangeNotic(querystring, PersonSNO);
                }
            }
            else if (Send_List1.SelectedValue == "2")
            {
                if (Join_list.SelectedValue == "")//未錄取全選
                {
                    string send_list = Send_List1.SelectedItem.Value;
                    Strsql = @"select EventD.EventDSNO,Person.PersonSNO,Person.PName,EventD.EventSNO,EventD.Audit,EventD.PersonSNO,person.PPhone,person.PMail ";
                    Strsql += " from EventD ";
                    Strsql += " JOIN Person On EventD.PersonSNO=Person.PersonSNO";
                    Strsql += " where EventD.EventSNO=@querystring and person.PMail <> '' ";
                    Strsql += " and EventD.Audit=2";

                    aDict.Add("querystring", querystring);
                    DataTable SendEmailList = objDH.queryData(Strsql, aDict);

                    if (SendEmailList.Rows.Count == 0)
                    {
                        Response.Write("<script>alert('無未錄取人員!');</script>");
                        return;
                    }
                    for (int i = 0; i < SendEmailList.Rows.Count; i++)
                    {
                        //Name += "\n" + SendEmailList.Rows[i]["PMail"].ToString() + ",";
                        string MailContent = editor_Mail.Value;
                        SendMail(EventName, MailContent, SendEmailList.Rows[i]["PMail"].ToString());
                    }
                    //修改通知狀態
                    for (int i = 0; i < SendEmailList.Rows.Count; i++)
                    {
                        string PersonSNO = SendEmailList.Rows[i]["PersonSNO"].ToString();
                        Utility.ChangeNotic(querystring, PersonSNO);
                    }
                }
                else//未錄取單筆
                {
                    string send_list = Send_List1.SelectedItem.Value;
                    Strsql = @"select EventD.EventDSNO,Person.PersonSNO,Person.PName,EventD.EventSNO,EventD.Audit,EventD.PersonSNO,person.PPhone,person.PMail ";
                    Strsql += " from EventD ";
                    Strsql += " JOIN Person On EventD.PersonSNO=Person.PersonSNO";
                    Strsql += " where EventD.EventSNO=@querystring and person.PMail <> '' ";
                    Strsql += " and EventDSNO=@Join_list";

                    aDict.Add("querystring", querystring);
                    aDict.Add("Join_list", Join_list.SelectedValue);
                    DataTable SendEmailList = objDH.queryData(Strsql, aDict);
                    if (SendEmailList.Rows.Count == 0)
                    {
                        Response.Write("<script>alert('無未錄取人員!');</script>");
                        return;
                    }
                    for (int i = 0; i < SendEmailList.Rows.Count; i++)
                    {
                        //Name += "\n" + SendEmailList.Rows[i]["PMail"].ToString() + ",";
                        string MailContent = editor_Mail.Value;
                        SendMail(EventName, MailContent, SendEmailList.Rows[i]["PMail"].ToString());
                    }
                    //修改通知狀態
                    for (int i = 0; i < SendEmailList.Rows.Count; i++)
                    {
                        string PersonSNO = SendEmailList.Rows[i]["PersonSNO"].ToString();
                        Utility.ChangeNotic(querystring, PersonSNO);
                    }
                }
            }
            else if (Send_List1.SelectedValue == "3")
            {
                string send_list = Send_List1.SelectedItem.Value;
                Strsql += @"select EventD.EventDSNO,Person.PersonSNO,Person.PName,EventD.EventSNO,EventD.Audit,EventD.PersonSNO,person.PPhone,person.PMail ";
                Strsql += " from EventD ";
                Strsql += " JOIN Person On EventD.PersonSNO=Person.PersonSNO";
                Strsql += " where EventD.EventSNO=@querystring and person.PMail <> '' ";
                Strsql += " and EventDSNO=@Join_list";

                aDict.Add("querystring", querystring);
                aDict.Add("Join_list", Join_list.SelectedValue);
                DataTable SendEmailList = objDH.queryData(Strsql, aDict);

                if (SendEmailList.Rows.Count == 0)
                {
                    Response.Write("<script>alert('無選取人員!');</script>");
                    return;
                }
                for (int i = 0; i < SendEmailList.Rows.Count; i++)
                {
                    //Name += "\n" + SendEmailList.Rows[i]["PMail"].ToString() + ",";
                    //string EventName = "您已取得戒菸服務人員資格證明";
                    string MailContent = editor_Mail.Value;
                    SendMail(EventName, MailContent, SendEmailList.Rows[i]["PMail"].ToString());
                }
                //修改通知狀態
                for (int i = 0; i < SendEmailList.Rows.Count; i++)
                {
                    string PersonSNO = SendEmailList.Rows[i]["PersonSNO"].ToString();
                    Utility.ChangeNotic(querystring, PersonSNO);
                }
            }
            else
            {
                #region 備取人員
                string send_list = Send_List1.SelectedItem.Value;
                Strsql = @"select EventD.EventDSNO,Person.PersonSNO,Person.PName,EventD.EventSNO,EventD.Audit,EventD.PersonSNO,person.PPhone,person.PMail ";
                Strsql += " from EventD ";
                Strsql += " JOIN Person On EventD.PersonSNO=Person.PersonSNO";
                Strsql += " where EventD.EventSNO=@querystring and person.PMail <> '' ";
                Strsql += " and EventD.Audit=4";

                aDict.Add("querystring", querystring);
                DataTable SendEmailList = objDH.queryData(Strsql, aDict);

                if (SendEmailList.Rows.Count == 0)
                {
                    Response.Write("<script>alert('無備取人員!');</script>");
                    return;
                }
                for (int i = 0; i < SendEmailList.Rows.Count; i++)
                {
                    //Name += "\n" + SendEmailList.Rows[i]["PMail"].ToString() + ",";
                    string MailContent = editor_Mail.Value;
                    SendMail(EventName, MailContent, SendEmailList.Rows[i]["PMail"].ToString());
                }
                #endregion
                //修改通知狀態
                for (int i = 0; i < SendEmailList.Rows.Count; i++)
                {
                    string PersonSNO = SendEmailList.Rows[i]["PersonSNO"].ToString();
                    Utility.ChangeNotic(querystring, PersonSNO);
                }
            }


            #region 建立活動並且寄送
            ////每四個小時寄送(Winform)
            //string Title = "Email,Name";
            ////JObject匿名物件
            //JObject obj = new JObject(
            //     new JProperty("contacts", Title + Name)
            //    );
            ////序列化為JSON字串並輸出結果
            //var result = JsonConvert.SerializeObject(obj, Formatting.Indented);
            //await Email.InsertMember(sn, result);

            //DataHelper ObjDH = new DataHelper();
            //Dictionary<string, object> adict = new Dictionary<string, object>();
            //string SQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate())";
            //adict.Add("SN", sn);
            //adict.Add("EventName", lbl_EventName.Text);
            //adict.Add("MailContent", editor_Mail.InnerHtml);
            //ObjDH.executeNonQuery(SQL, adict);

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('已為您排程寄送，每天09:00/13:00/17:00寄送');", true);
            #endregion
            binData();
        }
    }

    protected void btnSendSMS_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string SendSmsTo = "";
        string querystring = Request.QueryString["sno"];
        string send_list = Send_List1.SelectedItem.Value;
        string Strsql = @"select EventD.EventDSNO,Person.PersonSNO,Person.PName,EventD.EventSNO,EventD.Audit,EventD.PersonSNO,person.PPhone,person.PMail ";
        Strsql += " from EventD ";
        Strsql += " JOIN Person On EventD.PersonSNO=Person.PersonSNO";
        Strsql += " where EventD.EventSNO=@querystring and person.PPhone <> '' ";

        if (Send_List1.SelectedValue == "1" || Send_List1.SelectedValue == "2")
        {
            Strsql += " and EventD.Audit=@send_list";
        }
        if (Send_List1.SelectedValue == "3")
        {
            Strsql += " and EventDSNO=@Join_list";
        }
        aDict.Add("send_list", send_list);
        aDict.Add("querystring", querystring);
        aDict.Add("Join_list", Join_list.SelectedValue);
        DataTable SendSMSList = objDH.queryData(Strsql, aDict);
        if (SendSMSList.Rows.Count == 0)
        {
            Response.Write("<script>alert('無備取人員或無錄取人員!'); location.href='Event.aspx';</script>");
            return;
        }

        for (int i = 0; i < SendSMSList.Rows.Count; i++)
        {
            SendSmsTo += SendSMSList.Rows[i]["PPhone"].ToString() + ",";
        }
        SendSmsTo = SendSmsTo.Substring(0, SendSmsTo.Length - 1);

        
        string SMStempFile = Server.MapPath("\\SMSTemp\\SendSMSList.txt");
        if (!File.Exists(SMStempFile))
        {
            using (StreamWriter streamWriter = new StreamWriter(SMStempFile, true, Encoding.UTF8))
            {
                string[] Array_SendSms = SendSmsTo.Split(',');
                for (int i = 0; i < Array_SendSms.Length; i++)
                {
                    streamWriter.WriteLine("[" + 100 + i + "]");
                    streamWriter.WriteLine("dstaddr=" + Array_SendSms[i] + "");
                    streamWriter.WriteLine("smbody=" + txt_SMS.Text);
                }


            }
        }
        //修改通知狀態
        for (int i = 0; i < SendSMSList.Rows.Count; i++)
        {
            string PersonSNO = SendSMSList.Rows[i]["PersonSNO"].ToString();
            Utility.ChangeNotic(querystring, PersonSNO);
        }
        //Utility.sendSMS(SMStempFile);
        Response.Write("<script>alert('維護中!');</script>");
    }

    protected void btnSendOwnsite_click(object sender, EventArgs e)
    {

        string TODOTITLE = Send_List1.SelectedItem.Text + "通知";
        string TODOTEXT = editor_Sys.InnerText;
        string postPersonSNO = userInfo.PersonSNO;
        string a = userInfo.PersonSNO;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string querystring = Request.QueryString["sno"];
        string send_list = Send_List1.SelectedItem.Value;
        string Strsql = @"Insert Into TODO(TODOTITLE,TODOTEXT,getPersonSNO,postPersonSNO,CreateDate,STATE)";
        Strsql += " SELECT @TODOTITLE AS TODOTITLE,@TODOTEXT AS TODOTEXT,Person.PersonSNO,@postPersonSNO AS postPersonSNO,GETDATE() as CreateDate,@STATE AS STATE ";
        Strsql += " from EventD  JOIN Person On EventD.PersonSNO=Person.PersonSNO ";
        Strsql += "  where EventD.EventSNO=@querystring ";
        if (Send_List1.SelectedValue == "1" )
        {
            Strsql += " and EventD.Audit=@send_list";
        }
        if (Send_List1.SelectedValue == "2" || Send_List1.SelectedValue == "3")
        {
            Strsql += " and EventDSNO=@Join_list";
        }
        aDict.Add("@TODOTITLE", TODOTITLE);
        aDict.Add("@TODOTEXT", TODOTEXT);
        aDict.Add("@postPersonSNO", postPersonSNO);
        aDict.Add("@STATE", 0);
        aDict.Add("send_list", send_list);
        aDict.Add("querystring", querystring);
        aDict.Add("Join_list", Join_list.SelectedValue);
        objDH.executeNonQuery(Strsql, aDict);

        string UpdateSQL = @"SELECT @TODOTITLE AS TODOTITLE,@TODOTEXT AS TODOTEXT,Person.PersonSNO,@postPersonSNO AS postPersonSNO,GETDATE() as CreateDate,@STATE AS STATE ";
        UpdateSQL += " from EventD  JOIN Person On EventD.PersonSNO=Person.PersonSNO ";
        UpdateSQL += " where EventD.EventSNO=@querystring ";
        if (Send_List1.SelectedValue == "3")
        {
            UpdateSQL += " and EventDSNO=@Join_list";
        }
        DataTable UpdateOBJDT = objDH.queryData(UpdateSQL, aDict);

        //修改通知狀態
        for (int i = 0; i < UpdateOBJDT.Rows.Count; i++)
        {
            string PersonSNO = UpdateOBJDT.Rows[i]["PersonSNO"].ToString();
            Utility.ChangeNotic(querystring, PersonSNO);
        }



        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('寄送成功。');", true);
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        string errorMessage = "";
        String id = Convert.ToString(Request.QueryString["sno"]);
        string LimitSQL = " SELECT * FROM Event WHERE  EventSNO=" + id + "";
        DataTable ObjLimit = objDH.queryData(LimitSQL, null);

        int Limit = Convert.ToInt16(ObjLimit.Rows[0]["CountLimit"].ToString());
        int count = 0;
        string sql = "";
        if (gv_EventD.Rows.Count == 0)
        {
            Response.Write("<script>alert('目前無報名人員')</script>");
            return;
        }
        foreach (GridViewRow gridRow in gv_EventD.Rows)
        {

            var ddl_Audit = (DropDownList)gridRow.FindControl("ddl_AuditItem");
            var hid_EventDid = (HiddenField)gridRow.FindControl("hid_EventDid");
            if (ddl_Audit.SelectedValue == "1")
            {
                count += 1;
            }
            sql += " Update EventD Set Audit = '" + ddl_Audit.SelectedValue + "',ModifyDT='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + "',ModifyUserID=" + userInfo.PersonSNO + " WHERE EventDSNO = " + hid_EventDid.Value + " ; ";
        }
        if (Limit == 0 || count <= Limit)
        {
            DataTable objDTPAPER = objDH.queryData(sql, null);
            Response.Write("<script>alert('送出成功!'); </script>");
            Response.Redirect(Request.Url.ToString());
        }
        else
        {
            if (count > Limit) errorMessage = "超出錄取上限人數";
            if (!string.IsNullOrEmpty(errorMessage))
            {
                Utility.showMessage(Page, "ErrorMessage", errorMessage);
                return;
            }

        }
       
       
    }

    protected void ddl_AuditItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DropDownList list = (DropDownList)sender;
        int unAdmit = 0;
        int Admitted = 0;
        int Waiting = 0;
        int Chack = 0;
        int Ready = 0;
        int cancel = 0;
        foreach (GridViewRow gridRow in gv_EventD.Rows)
        {
            var ddl = (DropDownList)gridRow.FindControl("ddl_AuditItem");
            var selVal = Convert.ToInt16(ddl.SelectedValue);
            if (selVal == 0) unAdmit += 1;
            else if (selVal == 1) Admitted += 1;
            else if (selVal == 2) Waiting += 1;
            else if (selVal == 3) Chack += 1;
            else if (selVal == 4) Ready += 1;
            else if (selVal == 5) cancel += 1;
        }

        lbl_UnAdmit.Text = unAdmit.ToString();
        lbl_Admitted.Text = Admitted.ToString();
        lbl_Waiting.Text = Waiting.ToString();
        lbl_Chack.Text = Chack.ToString();
        lbl_Ready.Text = Ready.ToString();
        lbl_cancel.Text = cancel.ToString();
    }

    protected void Send_List1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string querystring = Request.QueryString["sno"];
        if (Send_List1.SelectedValue == "3")
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            DataHelper objDH = new DataHelper();
            Join_list.Enabled = true;
            string Strsql = @"select EventD.EventDSNO,Person.PName,EventD.EventSNO,EventD.Audit,EventD.PersonSNO,person.PPhone,person.PMail ";
            Strsql += " from EventD ";
            Strsql += " JOIN Person On EventD.PersonSNO=Person.PersonSNO where EventD.EventSNO=@querystring ORDER BY PName COLLATE Chinese_PRC_Stroke_ci_as";
            aDict.Add("querystring", querystring);
            DataTable Singleoption = objDH.queryData(Strsql, aDict);
            Join_list.Items.Clear();
            if (Singleoption.Rows.Count>0)
            {
                for (int i = 0; i < Singleoption.Rows.Count; i++)
                {
                    Join_list.Items.Add(new ListItem(Singleoption.Rows[i]["Pname"].ToString(), Singleoption.Rows[i]["EventDSNO"].ToString()));

                }
            }
            else
            {
                Join_list.Enabled = false;
            }


        }else if (Send_List1.SelectedValue == "2")//未錄取
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            DataHelper objDH = new DataHelper();
            Join_list.Enabled = true;
            string Strsql = @"select EventD.EventDSNO,Person.PName,EventD.EventSNO,EventD.Audit,EventD.PersonSNO,person.PPhone,person.PMail ";
            Strsql += " from EventD ";
            Strsql += " JOIN Person On EventD.PersonSNO=Person.PersonSNO where EventD.EventSNO=@querystring and EventD.Audit=2 ORDER BY PName COLLATE Chinese_PRC_Stroke_ci_as";
            aDict.Add("querystring", querystring);
            DataTable Singleoption = objDH.queryData(Strsql, aDict);
            Join_list.Items.Clear();
            if (Singleoption.Rows.Count>0)//有未錄取學員
            {
                Join_list.Items.Add(new ListItem("全選", ""));
                for (int i = 0; i < Singleoption.Rows.Count; i++)
                {
                    Join_list.Items.Add(new ListItem(Singleoption.Rows[i]["Pname"].ToString(), Singleoption.Rows[i]["EventDSNO"].ToString()));

                }
            }
            else
            {
                Join_list.Enabled = false;
            }
        }
        else if (Send_List1.SelectedValue == "1")//未錄取
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            DataHelper objDH = new DataHelper();
            Join_list.Enabled = true;
            string Strsql = @"select EventD.EventDSNO,Person.PName,EventD.EventSNO,EventD.Audit,EventD.PersonSNO,person.PPhone,person.PMail ";
            Strsql += " from EventD ";
            Strsql += " JOIN Person On EventD.PersonSNO=Person.PersonSNO where EventD.EventSNO=@querystring and EventD.Audit=1 ORDER BY PName COLLATE Chinese_PRC_Stroke_ci_as";
            aDict.Add("querystring", querystring);
            DataTable Singleoption = objDH.queryData(Strsql, aDict);
            Join_list.Items.Clear();
            if (Singleoption.Rows.Count>0)//有未錄取學員
            {
                Join_list.Items.Add(new ListItem("全選", ""));
                for (int i = 0; i < Singleoption.Rows.Count; i++)
                {
                    Join_list.Items.Add(new ListItem(Singleoption.Rows[i]["Pname"].ToString(), Singleoption.Rows[i]["EventDSNO"].ToString()));

                }
            }
            else
            {
                Join_list.Enabled = false;
            }
        }
        else
        {
            Join_list.Items.Clear();
            Join_list.Enabled = false;
        }
    }

    protected void Invite_download_Click(object sender, EventArgs e)
    {
        if (gv_EventD.Rows.Count == 0)
        {
            Response.Write("<script>alert('尚未查詢')</script>");
            return;
        }
        Utility.OpenExportWindows(this, ReportEnum.EventAudit.ToString());
    }

    protected void Btn_Search_Click(object sender, EventArgs e)
    {
        binData();

    }

    protected string getFiles(string dirID)
    {
        string fileList = "";
        if (Directory.Exists(Server.MapPath("../Download") + "/" + dirID))
        {
            string[] files = Directory.GetFiles(Server.MapPath("../Download") + "/" + dirID);
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fileInfo = new FileInfo(files[i]);
                fileList += "<a target='_blank' href='../Download/" + dirID + "/" + fileInfo.Name + "'><i class='fa fa-file'></i> " + fileInfo.Name + "</a><br/>";
            }
        }
        return fileList;
    }

    private void ReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("ROW_NO", "序號");
        _SetCol.Add("PName", "姓名");
        _SetCol.Add("PersonID", "身分證號");
        _SetCol.Add("Psex", "性別");
        _SetCol.Add("PBirthDate", "出生日期");
        _SetCol.Add("PPhone", "行動電話");
        _SetCol.Add("PMail", "E-Mail");
        _SetCol.Add("PAddr", "通訊地址");
        _SetCol.Add("PTel", "通訊電話");
        _SetCol.Add("ApplyDT", "報名時間");
        _SetCol.Add("OrganName", "現職機構名稱");
        _SetCol.Add("OrganAddr", "現職機構地址");
        _SetCol.Add("OrganTel", "現職機構電話");
        _SetCol.Add("MstatusSNO", "學員狀態");
        _SetCol.Add("證書字號資料", "證書字號/首發日/公告日/到期日/發證單位");
        _SetCol.Add("EventAudit", "審核狀況");
        _SetCol.Add("Area", "地區");
        _SetCol.Add("City", "城市");
        _SetCol.Add("Note", "審核備註");
        _SetCol.Add("Meals", "膳食");
        _SetCol.Add("JCN", "醫事證號(醫)");
        _SetCol.Add("LSCN", "專科證號(醫)");
        _SetCol.Add("LRType", "執業登記科別(醫)");
        _SetCol.Add("LSType", "專科科別(醫)");
        _SetCol.Add("ClassName", "醫事機構層級(醫)");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.EventAudit.ToString()] = _ExcelInfo;
    }

    protected void btn_Back_Click(object sender, EventArgs e)
    {

        string querystring = Request.QueryString["sno"];
        string Twenty = (Request["twenty"] != null) ? Request["twenty"].ToString() : "";
        string Exchange = (Request["Exchange"] != null) ? Request["Exchange"].ToString() : "";
        string sql = "Select * from Event where EventSNO=@EventSNO";
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("EventSNO", querystring);
        DataTable objDT = ObjDH.queryData(sql, adict);
        string URL = objDT.Rows[0]["Class3"].ToString();
        if (Twenty != "" && Exchange !="")
        {
            Response.Redirect("ExChangeEvent_Local.aspx");
        }
        else if (Twenty=="1"&& URL == "1")
        {
            Response.Redirect("EventLocal.aspx");
        }
        else if(URL=="1")
        {
            Response.Redirect("Event.aspx");
           
        }
        else if (Exchange == "1")
        {
            Response.Redirect("ExChangeEvent.aspx");
           
        }
        else
        {
           
        }

    }

    protected void gv_EventD_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "getdata")
        {
            DataHelper ObjDH = new DataHelper();
            Button myButton = (Button)e.CommandSource;
            GridViewRow myRow = (GridViewRow)myButton.NamingContainer;
            string PersonID = myRow.Cells[4].Text;
            string EventD = myRow.Cells[5].Text;
            TextBox txt_Note = myRow.FindControl("txt_Note") as TextBox;
            Label lb_msg = myRow.FindControl("lb_msg") as Label;
            Dictionary<string, object> adict = new Dictionary<string, object>();
            adict.Add("EventSNO", Convert.ToString(Request.QueryString["sno"]));
            string PersonSNO = Utility.ConvertPersonIDToPersonSNO(PersonID);
            adict.Add("PersonSNO", PersonSNO);
            adict.Add("note", txt_Note.Text);
            string sql = @"Update EventD set Note=@Note where EventSNO=@EventSNO and PersonSNO=@PersonSNO";
            ObjDH.executeNonQuery(sql, adict);
            lb_msg.Text = "✔";
            lb_msg.ForeColor =System.Drawing.Color.Green;
        }
        else
        {
            //Dictionary<string, object> adict = new Dictionary<string, object>();
            //DataHelper ObjDH = new DataHelper();
            //string EventSNO = Request.QueryString["sno"].ToString();
            //Button myButton = (Button)e.CommandSource;
            //GridViewRow myRow = (GridViewRow)myButton.NamingContainer;
            //string PersonID = myRow.Cells[4].Text;
            //string PersonSNO = Utility.ConvertPersonIDToPersonSNO(PersonID);
            //string EventDSNO = myRow.Cells[19].Text;
            //string sql = @"Select * from EventD  where EventSNO=@EventSNO and PersonSNO=@PersonSNO and EventDSNO=@EventDSNO ";
            //adict.Add("EventDSNO", EventDSNO);
            //adict.Add("EventSNO", EventSNO);
            //adict.Add("PersonSNO", PersonSNO);
            //DataTable ObjDT = ObjDH.queryData(sql, adict);
            //if (ObjDT.Rows.Count > 0)
            //{
            //    string Notice = ObjDT.Rows[0]["Notice"].ToString();
            //    if (Notice == "False")
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('尚未通知，不得簽退。');", true);
            //    }
            //    else
            //    {
            //        Dictionary<string, object> bdict = new Dictionary<string, object>();
            //        string Dsql = @"Delete EventD where PersonSNO=@PersonSNO and EventSNO=@EventSNO ";
            //        bdict.Add("EventSNO", EventSNO);
            //        bdict.Add("PersonSNO", PersonSNO);
            //        ObjDH.executeNonQuery(Dsql, bdict);
            //        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('簽退成功。');window.location.replace(location.href)", true);
                   
            //    }
            //}


        }

    }

    protected void gv_EventD_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem != null)
        {
            string twenty = (Request["twenty"] != null) ? Request["twenty"].ToString() : "N/A";
            string FC = e.Row.Cells[0].Text.Replace("&nbsp;","");
            string SC= e.Row.Cells[1].Text.Replace("&nbsp;", "");
            if (SC != "" && FC != "" && twenty == "1")
            {
                e.Row.BackColor = System.Drawing.Color.LightPink;

            }
        }
           
        
        
    }

    protected void gv_EventD_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[18].Visible = false;
            e.Row.Cells[19].Visible = false;
        }
  
    }

    protected async void Btn_Out_Click(object sender, EventArgs e)
    {
        string EventSNO = Request.QueryString["sno"].ToString() == null ? "" : Request.QueryString["sno"].ToString();
        string PersonSNO = hidpersonsno.Value;
        string EventDSNO = hideventDsno.Value;
        #region Mail
 
        string EventName = lbl_EventName.Text + "-" + DateTime.Now.ToShortDateString();
        //string sn = Email.CreateGroup(EventName);
        #endregion

        #region 取得活動人數並且匯入
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> Pdict = new Dictionary<string, object>();
        string Name = "";
        string Mailsql = @"Select Pmail from Person  where PersonSNO=@PersonSNO";
        Pdict.Add("PersonSNO", PersonSNO);
        DataTable MailDT = objDH.queryData(Mailsql, Pdict);
        if (MailDT.Rows.Count>0)
        {
            //Name = MailDT.Rows[0]["Pmail"].ToString();
            string MailContent = editor_Out.Value;
            SendMail(EventName, MailContent, MailDT.Rows[0]["Pmail"].ToString());
        }
        Utility.ChangeNotic(EventSNO, PersonSNO);

        //string Title = "Email,Name";
        ////JObject匿名物件
        //JObject obj = new JObject(
        //     new JProperty("contacts", Title + Name)
        //    );
        ////序列化為JSON字串並輸出結果
        //var result = JsonConvert.SerializeObject(obj, Formatting.Indented);
        //await Email.InsertMember(sn, result);

        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        //string SQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate())";
        //adict.Add("SN", sn);
        //adict.Add("EventName", lbl_EventName.Text);
        //adict.Add("MailContent", editor_Out.InnerHtml);
        //ObjDH.executeNonQuery(SQL, adict);
        #endregion

        string sql = @"Select * from EventD  where EventSNO=@EventSNO and PersonSNO=@PersonSNO and EventDSNO=@EventDSNO ";
        adict.Add("EventDSNO", EventDSNO);
        adict.Add("EventSNO", EventSNO);
        adict.Add("PersonSNO", PersonSNO);
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            string Notice = ObjDT.Rows[0]["Notice"].ToString();
            if (Notice == "False")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('尚未通知，不得簽退。');", true);
            }
            else
            {
                Dictionary<string, object> bdict = new Dictionary<string, object>();
                string Dsql = @"Delete EventD where PersonSNO=@PersonSNO and EventSNO=@EventSNO ";
                bdict.Add("EventSNO", EventSNO);
                bdict.Add("PersonSNO", PersonSNO);
                ObjDH.executeNonQuery(Dsql, bdict);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('簽退成功。');window.location.replace(location.href)", true);

            }
        }

    }

    public static void SendMail(string MailSub, string MailBody, string SendTo = "emma.chao@iisigroup.com")
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
                (Select PVal From Config Where PID='Account1') Account,
                (Select PVal From Config Where PID='Host1') Host,
                (Select PVal From Config Where PID='Port1') Port,
                (Select PVal From Config Where PID='Psw1') Psw,
                (Select PVal From Config Where PID='SSL1') SSL
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
        string[] SendToOver100 = SendTo.Split(',');

        for (int i = 0; i < SendToOver100.Length; i++)
        {
            MailMessage NewMail = new System.Net.Mail.MailMessage();
            NewMail.SubjectEncoding = System.Text.Encoding.UTF8; //主題編碼格式
            NewMail.Subject = MailSub; //主題
            NewMail.IsBodyHtml = isBodyHtml;  //HTML語法(true:開啟false:關閉)
            NewMail.BodyEncoding = System.Text.Encoding.UTF8; //內文編碼格式
            NewMail.Body = MailBody; //內文
            NewMail.From = new MailAddress(MailFrom, MailName); //發送者
            NewMail.To.Add(SendToOver100[i]);
            SmtpClient NewSmtp = new SmtpClient(); //建立SMTP連線
            NewSmtp.Credentials = new System.Net.NetworkCredential(MailAccount, MailPsw); //連線驗證
            NewSmtp.Port = smtpPort; //SMTP Port
            NewSmtp.Host = smtpServer; //SMTP主機名稱
            NewSmtp.EnableSsl = smtpSSL; //開啟SSL驗證
                                         //NewSmtp.UseDefaultCredentials = true;
            NewSmtp.ServicePoint.MaxIdleTime = 1;
            NewSmtp.Send(NewMail); //發送
            System.Threading.Thread.Sleep(500);
            NewMail.Dispose();



        }




    }

    protected void txt_Note_TextChanged(object sender, EventArgs e)
    {
        //Dictionary<string, object> adict = new Dictionary<string, object>();
        //DataHelper ObjDH = new DataHelper();
        //Button myButton = (Button)e.CommandSource;
        //GridViewRow myRow = (GridViewRow)myButton.NamingContainer;
        //string PersonID = myRow.Cells[4].Text;
        //string PersonSNO = Utility.ConvertPersonIDToPersonSNO(PersonID);
        //string EventDSNO = myRow.Cells[19].Text;
        //string sql = @"";

    }
}
