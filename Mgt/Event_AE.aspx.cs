using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Event_AE : System.Web.UI.Page
{
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

            String work = "";
            Utility.SetDdlConfig(ddl_Class1, "CourseClass3", "請選擇");
            Utility.SetDdlConfig(ddl_Class2, "CourseClass4", "請選擇");
            SetEventClass(ddl_EventClass, "請選擇");
            //SetEventRole(ddl_EventRole,userInfo, "請選擇");
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
            Utility.SetDdlConfig(ddl_Class1, "CourseClass3", "請選擇");
            Utility.SetDdlConfig(ddl_Class2, "CourseClass4", "請選擇");
            Utility.setAreaCodeA(ddl_AreaCodeA_F, "請選擇");
            Utility.setAreaCodeA(ddl_codeAreaA_Address, "請選擇");
            Utility.setAreaCodeB(ddl_codeAreaB_Address, ddl_codeAreaA_Address.SelectedValue, "請選擇");
            //Utility.setAreaCodeB(ddl_AreaCodeB_F, ddl_AreaCodeA_F.SelectedValue, "請選擇");
            if (work.Equals("N"))
            {
                string Csno = Request.QueryString["csno"] == null ? "" : Request.QueryString["csno"].ToString();
                //ddl_Class3.Visible = false;
                Event.GetRoleList(cb_Role, work, Csno);
                newData();
                btnOK.Text = "新增";
                string Exchange = Request.QueryString["Exchange"];
                if (Exchange == "1")
                {
                    ddl_Class1.Items.RemoveAt(1);
                    ddl_Class2.Items.RemoveAt(1);
                    ddl_Class2.Items.RemoveAt(1);
                }
                else
                {
                    ddl_Class1.Items.RemoveAt(2);
                    ddl_Class2.Items.RemoveAt(3);
                    //ddl_Class2.Items.RemoveAt(3);
                }
                //測試
                lb_Code.Text = Event.EventGroupCode(userInfo.RoleGroup,userInfo.PersonSNO);
            }
            else
            {
                lb_Mark.Text = "修改";
                //Utility.setEPClassSNO(ddl_Class3, "請選擇");
                getData();
                string Exchange = Request.QueryString["Exchange"];
                if (Exchange == "1")
                {
                    ddl_Class1.Items.RemoveAt(1);
                    //ddl_Class2.Items.RemoveAt(4);
                    ddl_Class2.Items.RemoveAt(1);
                    ddl_Class2.Items.RemoveAt(1);
                }
                else
                {
                    ddl_Class1.Items.RemoveAt(2);
                    ddl_Class2.Items.RemoveAt(3);
                    //ddl_Class2.Items.RemoveAt(3);
                }
                lb_Code.Text = Event.EventGroupCode(userInfo.RoleGroup, userInfo.PersonSNO);
            }
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        var page = HttpContext.Current.CurrentHandler as Page;
        String errorMessage = "";
        //標題
        if (txt_Title.Text.Length > 50) errorMessage += "標題字數過多\\n";
        if (txt_Title.Text.Length == 0) errorMessage += "標題字數錯誤\\n";
        if (editor1.Value.Length == 0) errorMessage += "內容字數錯誤\\n";
        if (ddl_Class1.SelectedValue == "") errorMessage += "請選擇分類\\n";
        if (txt_CPerosn.Text.Length == 0) errorMessage += "請輸入主辦人\\n";
        if (txt_CPerosn.Text.Length > 50) errorMessage += "主辦人字數過多\\n";
        if (txt_CountLimit.Text != "0")
        {
            if (Convert.ToInt16(txt_CountLimit.Text) < Convert.ToInt16(txt_CountAdmit.Text)) errorMessage += "可報名人數不可小於錄取人數\\n";
        }

        //比較結束日期和開始日期
        if (!string.IsNullOrEmpty(txt_CStartDay_F.Text))
        {
            DateTime start_F = Convert.ToDateTime(txt_CStartDay_F.Text);
            DateTime end_F = Convert.ToDateTime(txt_CEndDay_F.Text);
            if (start_F > end_F) errorMessage += "結束日期小於開始日期!\\n";
        }

        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        if (Work.Value.Equals("NEW"))
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("EventSNO", txt_ID.Value);
            aDict.Add("SYSTEM_ID", "S00");
            aDict.Add("EventName", txt_Title.Text); //活動名稱
            aDict.Add("EventCSNO", ddl_EventClass.SelectedValue);
            //aDict.Add("ERSNO", ddl_EventRole.SelectedValue);
            aDict.Add("Class3", ddl_Class1.SelectedValue); //活動分類	
            aDict.Add("Class4", ddl_Class2.SelectedValue); //活動分類	
            aDict.Add("Note", HttpUtility.HtmlDecode(editor1.Value)); //內容
            aDict.Add("CountLimit", txt_CountLimit.Text); //可報名人數	
            aDict.Add("CountAdmit", txt_CountAdmit.Text); //錄取人數
            aDict.Add("TargetHour", txt_TargetHour.Text);//認定時數
            aDict.Add("QTypeName", txt_QTypeName.Text); //證書類別	
            aDict.Add("ActiveCost", txt_ActiveCost.Text); //活動費用
            aDict.Add("StartTime", txt_SignS_F.Text); //報名期間(起)
            aDict.Add("EndTime", txt_SignE_F.Text + " 23:59:00"); //報名期間(迄)
            aDict.Add("Host", txt_Host.Text); //主辦人           
            aDict.Add("CPerosn", txt_CPerosn.Text); //聯絡人
            aDict.Add("CPhone", txt_contact_C.Text); //連絡電話
            aDict.Add("CMail", txt_contact_mail.Text); //聯絡Email	  
            aDict.Add("CLocationAreaA", ddl_codeAreaA_Address.SelectedItem.Text);//聯絡城市
            aDict.Add("CLocationAreaCodeA", ddl_codeAreaA_Address.SelectedValue);//聯絡城市
            aDict.Add("CLocationAreaB", ddl_codeAreaB_Address.SelectedItem.Text);//聯絡地區
            aDict.Add("CLocationAreaCodeB", ddl_codeAreaB_Address.SelectedValue);//聯絡城市
            aDict.Add("CLocation", txt_CAddress.Text); //聯絡地址
            aDict.Add("isEnable", Chk_Enable.Checked); //開放前台查詢
            aDict.Add("SupportMeals", chk_Meals.Checked);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            aDict.Add("CancelRole", 0);
            //aDict.Add("Webbex", txt_Webbex.Text);
            string Strsql = "";
            if (ddl_Class1.SelectedValue == "1")
            {
                Strsql = @"Insert Into Event(EventName,Class3,Class4,CountLimit,CountAdmit,TargetHour,QTypeName,ActiveCost,StartTime,EndTime,Host,Note,CPerosn,CPhone,CMail,isEnable,CLocationAreaA,CLocationAreaCodeA,CLocationAreaB,CLocationAreaCodeB,CLocation,CreateUserID,SYSTEM_ID,EventCSNO,SupportMeals,CancelRole) 
                Values(@EventName,@Class3,@Class4,@CountLimit,@CountAdmit,@TargetHour,@QTypeName,@ActiveCost,@StartTime,@EndTime,@Host,@Note,@CPerosn,@CPhone,@CMail,@isEnable,@CLocationAreaA,@CLocationAreaCodeA,@CLocationAreaB,@CLocationAreaCodeB,@CLocation,@CreateUserID,@SYSTEM_ID,@EventCSNO,@SupportMeals,@CancelRole) SELECT @@IDENTITY AS 'Identity'
                ";
            }
            else
            {
                Strsql = @"Insert Into Event(EventName,Class3,Class4,CountLimit,CountAdmit,TargetHour,QTypeName,ActiveCost,StartTime,EndTime,Host,Note,CPerosn,CPhone,CMail,isEnable,CLocationAreaA,CLocationAreaCodeA,CLocationAreaB,CLocationAreaCodeB,CLocation,CreateUserID,SYSTEM_ID,EventCSNO,SupportMeals,CancelRole) 
                Values(@EventName,@Class3,@Class4,@CountLimit,@CountAdmit,@TargetHour,@QTypeName,@ActiveCost,@StartTime,@EndTime,@Host,@Note,@CPerosn,@CPhone,@CMail,@isEnable,@CLocationAreaA,@CLocationAreaCodeA,@CLocationAreaB,@CLocationAreaCodeB,@CLocation,@CreateUserID,@SYSTEM_ID,@EventCSNO,@SupportMeals,@CancelRole) SELECT @@IDENTITY AS 'Identity'
                ";
            }
            DataHelper objDH = new DataHelper();
            DataTable dt = objDH.queryData(Strsql, aDict);
            Event.InsertEventGroupNum(lb_EventIdentity.Value, txt_EventGNO.Text, lb_Code.Text, userInfo);
            string EventSNO = dt.Rows[0]["Identity"].ToString();
            Dictionary<string, object> bDict = new Dictionary<string, object>();
            bDict.Add("EventSNO", EventSNO);
            bDict.Add("EventBSNO", 1);
            bDict.Add("CStartDay", txt_CStartDay_F.Text);
            bDict.Add("CEndDay", txt_CEndDay_F.Text);
            bDict.Add("CStartTime", txt_CStratTime_F.Text);
            bDict.Add("CEndtime", txt_CEndTime_F.Text);
            bDict.Add("Chour", txt_CHour_F.Text);
            bDict.Add("CCount", txt_CCount_F.Text);
            bDict.Add("EventAreaCodeA", ddl_AreaCodeA_F.SelectedValue);
            bDict.Add("EventAreaCodeAText", ddl_AreaCodeA_F.SelectedItem.Text);
            bDict.Add("EventArea", txt_ActiveArea_F.Text);
            InsertSQL(EventSNO, bDict);
            //寫入適用人員
            Utility.insertRoleBind(cb_Role, dt.Rows[0]["Identity"].ToString(), "Event_AE", userInfo.PersonSNO);

            bool IsEnable;
            if (Chk_Enable.Checked)
            {
                IsEnable = true;
            }
            else
            {
                IsEnable = false;
            }
            //Insert22ExCanlendar(txt_SignS_F.Text, txt_SignE_F.Text, txt_Title.Text, EventSNO, 
            //    DateTime.Now.ToShortDateString(), userInfo.PersonSNO, "", ddl_EventRole.SelectedValue,Chk_Enable.Checked);

            if (ddl_Class1.SelectedValue == "1")
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert('新增成功');window.location ='Event.aspx';", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert('新增成功');window.location ='ExchangeEvent.aspx';", true);

            }
        }
        else
        {
            //更新
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("EventSNO", txt_ID.Value);
            aDict.Add("EventCSNO", ddl_EventClass.SelectedValue);
            aDict.Add("EventName", txt_Title.Text); //活動名稱            
            //aDict.Add("ERSNO", ddl_EventRole.SelectedValue);
            aDict.Add("Class3", ddl_Class1.SelectedValue); //活動分類	
            aDict.Add("Class4", ddl_Class2.SelectedValue); //活動分類	
            aDict.Add("Note", HttpUtility.HtmlDecode(editor1.Value)); //內容
            aDict.Add("CountLimit", txt_CountLimit.Text); //可報名人數	
            aDict.Add("CountAdmit", txt_CountAdmit.Text); //錄取人數
            aDict.Add("TargetHour", txt_TargetHour.Text);//認定時數
            aDict.Add("QTypeName", txt_QTypeName.Text); //證書類別	
            aDict.Add("ActiveCost", txt_ActiveCost.Text); //活動費用
            aDict.Add("StartTime", txt_SignS_F.Text); //報名期間(起)
            aDict.Add("EndTime", txt_SignE_F.Text + " 23:59:00"); //報名期間(迄)
            aDict.Add("Host", txt_Host.Text); //主辦人           
            aDict.Add("CPerosn", txt_CPerosn.Text); //聯絡人
            aDict.Add("CPhone", txt_contact_C.Text); //連絡電話
            aDict.Add("CMail", txt_contact_mail.Text); //聯絡Email	  
            aDict.Add("CLocationAreaA", ddl_codeAreaA_Address.SelectedItem.Text);//聯絡城市
            aDict.Add("CLocationAreaCodeA", ddl_codeAreaA_Address.SelectedValue);//聯絡城市
            aDict.Add("CLocationAreaB", ddl_codeAreaB_Address.SelectedItem.Text);//聯絡地區
            aDict.Add("CLocationAreaCodeB", ddl_codeAreaB_Address.SelectedValue);//聯絡城市
            aDict.Add("CLocation", txt_CAddress.Text); //聯絡地址
            aDict.Add("isEnable", Chk_Enable.Checked); //開放前台查詢
            aDict.Add("ModifyDT", DateTime.Now);
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            aDict.Add("SupportMeals", chk_Meals.Checked);
            //aDict.Add("Webbex", txt_Webbex.Text);
            DataHelper objDH = new DataHelper();
            if (ddl_Class1.SelectedValue == "1")
            {
                objDH.executeNonQuery(@"Update Event Set EventName=@EventName,Class3=@Class3,Class4=@Class4,SupportMeals=@SupportMeals,
                Note=@Note,CountLimit=@CountLimit,CountAdmit=@CountAdmit,TargetHour=@TargetHour,QTypeName=@QTypeName,
                ActiveCost=@ActiveCost,StartTime=@StartTime,EndTime=@EndTime,Host=@Host,
                CPerosn=@CPerosn,CPhone=@CPhone,CMail=@CMail,CLocationAreaA=@CLocationAreaA,CLocationAreaCodeA=@CLocationAreaCodeA,CLocationAreaB=@CLocationAreaB,
                CLocationAreaCodeB=@CLocationAreaCodeB,CLocation=@CLocation,isEnable=@isEnable,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID,EventCSNO=@EventCSNO
                Where EventSNO=@EventSNO", aDict);
            }
            else
            {
                objDH.executeNonQuery(@"Update Event Set EventName=@EventName,Class3=@Class3,Class4=@Class4,SupportMeals=@SupportMeals,
                Note=@Note,CountLimit=@CountLimit,CountAdmit=@CountAdmit,TargetHour=@TargetHour,QTypeName=@QTypeName,
                ActiveCost=@ActiveCost,StartTime=@StartTime,EndTime=@EndTime,Host=@Host,
                CPerosn=@CPerosn,CPhone=@CPhone,CMail=@CMail,CLocationAreaA=@CLocationAreaA,CLocationAreaCodeA=@CLocationAreaCodeA,CLocationAreaB=@CLocationAreaB,
                CLocationAreaCodeB=@CLocationAreaCodeB,CLocation=@CLocation,isEnable=@isEnable,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID,EventCSNO=@EventCSNO
                Where EventSNO=@EventSNO", aDict);
            }
            Event.UpdateEventGroupNum(txt_ID.Value, txt_EventGNO.Text, lb_Code.Text, userInfo);
            Dictionary<string, object> bDict = new Dictionary<string, object>();
            bDict.Add("EventSNO", txt_ID.Value);
            bDict.Add("EventBSNO", 1);
            bDict.Add("CStartDay", txt_CStartDay_F.Text);
            bDict.Add("CEndDay", txt_CEndDay_F.Text);
            bDict.Add("CStartTime", txt_CStratTime_F.Text);
            bDict.Add("CEndtime", txt_CEndTime_F.Text);
            bDict.Add("Chour", txt_CHour_F.Text);
            bDict.Add("CCount", txt_CCount_F.Text);
            bDict.Add("EventAreaCodeA", ddl_AreaCodeA_F.SelectedValue);
            bDict.Add("EventAreaCodeAText", ddl_AreaCodeA_F.SelectedItem.Text);
            bDict.Add("ModifyDT", DateTime.Now);
            bDict.Add("ModifyUserID", userInfo.PersonSNO);
            bDict.Add("EventArea", txt_ActiveArea_F.Text);
            UpdateSQL(txt_ID.Value, bDict, "1");
            Utility.insertRoleBind(cb_Role, txt_ID.Value, "Event_AE", userInfo.PersonSNO);
            bool IsEnable;
            if (Chk_Enable.Checked)
            {
                IsEnable = true;
            }
            else
            {
                IsEnable = false;
            }
            string Exchange = Request.QueryString["Exchange"] == null ? "" : Request.QueryString["Exchange"].ToString();
            string Csno = Request.QueryString["csno"] == null ? "" : Request.QueryString["csno"].ToString();

            //if (Exchange == "")
            //{
            //    Utility.Update22Canlendar(txt_SignS_F.Text, txt_SignE_F.Text, txt_Title.Text, txt_ID.Value, DateTime.Now.ToShortDateString(), userInfo.PersonSNO, ddl_EventRole.SelectedValue, Csno, IsEnable);
            //}
            //else
            //{
            //    Utility.Update22Canlendar(txt_SignS_F.Text, txt_SignE_F.Text, txt_Title.Text, txt_ID.Value, DateTime.Now.ToShortDateString(), userInfo.PersonSNO, ddl_EventRole.SelectedValue, Csno, IsEnable);
            //}

            if (ddl_Class1.SelectedValue == "1")
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert('修改成功');window.location ='Event.aspx';", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert('修改成功');window.location ='ExchangeEvent.aspx';", true);

            }

        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        lb_Note.Text = HttpUtility.HtmlDecode(editor1.Value);
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", " showMSG();", true);
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static string GetData(string User)
    {
        return User;
    }
    protected void newData()
    {
        Work.Value = "NEW";
        Utility.setRoleBind(cb_Role, "0", "");
        lb_EventIdentity.Value = Utility.getEventSNOIdentity();
    }

    protected void getData()
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("sno", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
            SELECT 
            ROW_NUMBER() OVER (ORDER BY E.CreateDT DESC ) as ROW_NO,
            E.EventSNO,E.EventCSNO,EC.ClassName,S.SYSTEM_NAME,E.SYSTEM_ID,E.EventName,E.PClassSNO,EPClassSNO,SupportMeals,
            E.Note,E.StartTime,E.EndTime,E.CPerosn,E.CountLimit,E.CountAdmit,E.TargetHour,E.QTypeName,E.ActiveCost,E.Host,E.CPhone,
            E.Cmail,E.CancelRole,E.IsEnable,E.CLocationAreaA,E.CLocationAreaB,E.CLocation,E.CLocationAreaCodeA,E.CLocationAreaCodeB,E.Class3,E.Class4
            FROM  Event E
            LEFT JOIN EventClass EC on E.EventCSNO=EC.EventCSNO
            LEFT JOIN SYSTEM S on E.SYSTEM_ID=S.SYSTEM_ID     
            LEFT Join EventRole ER On ER.ERSNO=E.ERSNO  
            Where E.EventSNO=@sno", aDict);
        //string a = objDT.Rows[0]["PClassSNO"].ToString();

        if (objDT.Rows.Count > 0)
        {
            ddl_codeAreaB_Address.Enabled = true;
            ddl_EventClass.SelectedValue = Convert.ToString(objDT.Rows[0]["EventCSNO"]);
            //ddl_EventRole.SelectedValue = Convert.ToString(objDT.Rows[0]["ERSNO"]);
            txt_ID.Value = Convert.ToString(objDT.Rows[0]["EventSNO"]);
            txt_Title.Text = Convert.ToString(objDT.Rows[0]["EventName"]);
            txt_TargetHour.Text = Convert.ToString(objDT.Rows[0]["TargetHour"]);
            txt_QTypeName.Text = Convert.ToString(objDT.Rows[0]["QTypeName"]);
            txt_ActiveCost.Text = Convert.ToString(objDT.Rows[0]["ActiveCost"]);
            txt_SignS_F.Text = Convert.ToDateTime(objDT.Rows[0]["StartTime"]).ToString("yyyy-MM-dd");
            txt_SignE_F.Text = Convert.ToDateTime(objDT.Rows[0]["EndTime"]).ToString("yyyy-MM-dd");
            txt_Host.Text = Convert.ToString(objDT.Rows[0]["Host"]);
            txt_contact_C.Text = Convert.ToString(objDT.Rows[0]["CPhone"]);
            txt_contact_mail.Text = Convert.ToString(objDT.Rows[0]["Cmail"]);
            ddl_codeAreaA_Address.SelectedValue = Convert.ToString(objDT.Rows[0]["CLocationAreaCodeA"]);
            ddl_codeAreaB_Address.SelectedValue = Convert.ToString(objDT.Rows[0]["CLocationAreaCodeB"]);
            ddl_Class1.SelectedValue = Convert.ToString(objDT.Rows[0]["Class3"]);
            ddl_Class2.SelectedValue = Convert.ToString(objDT.Rows[0]["Class4"]);
            txt_CAddress.Text = Convert.ToString(objDT.Rows[0]["CLocation"]);
            //editor1.Value = HttpUtility.HtmlDecode(Convert.ToString(objDT.Rows[0]["Note"]));
            editor1.Value = Convert.ToString(objDT.Rows[0]["Note"]).Replace("font-family:\"新細明體\",serif;", "");
            editor1.Value = Convert.ToString(editor1.Value.Replace("font-family:\"微軟正黑體\",sans-serif;", ""));
            editor1.Value = Convert.ToString(editor1.Value.Replace("roman\"; mso-bidi-font-size:12.0pt; mso-font-kerning:0pt; \"", ""));
            editor1.Value = Convert.ToString(editor1.Value.Replace("mso-bidi-font-family:", ""));
            editor1.Value = Convert.ToString(editor1.Value.Replace("Times New Roman", ""));
            editor1.Value = Convert.ToString(editor1.Value.Replace(";mso-bidi-font-size:12.0pt;mso-font-kerning:0pt;", ""));
            editor1.Value = Convert.ToString(editor1.Value.Replace("\"\"", ""));
            txt_CPerosn.Text = Convert.ToString(objDT.Rows[0]["CPerosn"]);
            txt_CountLimit.Text = Convert.ToString(objDT.Rows[0]["CountLimit"]);
            txt_CountAdmit.Text = Convert.ToString(objDT.Rows[0]["CountAdmit"]);
            Chk_Enable.Checked = Convert.ToBoolean(objDT.Rows[0]["IsEnable"]);
            chk_Meals.Checked = Convert.ToBoolean(objDT.Rows[0]["SupportMeals"]) ? Convert.ToBoolean(objDT.Rows[0]["SupportMeals"]) : false;
            //txt_Webbex.Text= Convert.ToString(objDT.Rows[0]["Webbex"]);
            Utility.setRoleBind(cb_Role, txt_ID.Value, "Event_AE");
            //帶下半部分
            Dictionary<string, object> eDict = new Dictionary<string, object>();
            eDict.Add("EventSNO", id);
            DataTable objDT_batch = objDH.queryData(@"
            SELECT [EventSNO],[EventBSNO],[CStartDay],[CEndDay],[CStartTime],[CEndtime]
            ,[Chour],[CCount],[EventAreaCodeA],[EventAreaCodeAText],[EventAreaCodeB]
            ,[EventAreaCodeBText],[EventArea],[EventLocationCodeA],[EventLocationCodeAText]
	        ,[EventLocationCodeB],[EventLocationCodeBText],[EventLocation]
            FROM [EventBatch]
            where EventSNO=@EventSNO order by EventBSNO ", eDict);
            if (objDT_batch.Rows.Count > 0)
            {
                txt_CStartDay_F.Text = Convert.ToDateTime(objDT_batch.Rows[0]["CStartDay"]).ToString("yyyy-MM-dd");
                txt_CEndDay_F.Text = Convert.ToDateTime(objDT_batch.Rows[0]["CEndDay"]).ToString("yyyy-MM-dd");
                txt_CStratTime_F.Text = Convert.ToString(objDT_batch.Rows[0]["CStartTime"]);
                txt_CEndTime_F.Text = Convert.ToString(objDT_batch.Rows[0]["CEndtime"]);
                txt_CHour_F.Text = Convert.ToString(objDT_batch.Rows[0]["Chour"]);
                txt_CCount_F.Text = Convert.ToString(objDT_batch.Rows[0]["CCount"]);
                ddl_AreaCodeA_F.SelectedValue = Convert.ToString(objDT_batch.Rows[0]["EventAreaCodeA"]);
                txt_ActiveArea_F.Text = Convert.ToString(objDT_batch.Rows[0]["EventArea"]);

            }

            txt_EventGNO.Text =Event.GetEventGroupNum(id).Trim();
            lb_Code.Text= Event.GetEventGroupCode(id, userInfo).Trim();
        }

    }
    #region

    public void getddlvalue()
    {
        string RoleValue = "";
        Event.SetDdlConfig1(ddl_Class2, "CourseClass4");
        //tr_Role.Visible = true;
        for (int i = 0; i < cb_Role.Items.Count; i++)
        {
            if (cb_Role.Items[i].Selected == true)
            {
                RoleValue += cb_Role.Items[i].Value + ",";
            }
        }
        RoleValue = RoleValue.Substring(0, RoleValue.Length - 1);
        string[] RoleArray = RoleValue.Split(',');
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string ddl_SQL = @"SELECT CPC.[PClassSNO]
                        		,CPR.RoleSNO
                              ,[PlanName]
                              ,[CStartYear]
                              ,[CEndYear]
                              ,[IsEnable]
                              ,[CTypeSNO]
                              ,[TargetIntegral]
                          FROM [QS_CoursePlanningClass] CPC
                          Left join QS_CoursePlanningRole CPR ON CPR.PClassSNO=CPC.PClassSNO where 1=1 ";
        string Exchange = Request.QueryString["Exchange"];
        if (Exchange == "1")
        {
            ddl_SQL += "and PlanType=1";
        }
        else
        {
            ddl_SQL += "and PlanType In (0,2)";
        }
        for (int j = 0; j < RoleArray.Length; j++)
        {
            ddl_SQL += " or CPR.RoleSNO=" + RoleArray[j] + "";
        }
        DataTable objDT = objDH.queryData(ddl_SQL, aDict);
        //ddl_CoursePlanning.DataSource = objDT;
        //ddl_CoursePlanning.DataBind();
    }

  

    protected void cb_Role_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Utility.SetDdlConfig(ddl_Class1, "CourseClass3", "請選擇");
        //Utility.SetDdlConfig(ddl_Class2, "CourseClass4", "請選擇");

        //ddl_Class1.Enabled = true;
        //ddl_Class2.Enabled = true;
        //string Exchange = Request.QueryString["Exchange"];
        //if (Exchange == "1")
        //{
        //    //ddl_Class1.Items.RemoveAt(1);
        //    ddl_Class2.Items.RemoveAt(1);
        //    ddl_Class2.Items.RemoveAt(1);
        //}
        //else
        //{
        //    //ddl_Class1.Items.RemoveAt(2);
        //    ddl_Class2.Items.RemoveAt(2);
        //}
        ScriptManager.RegisterOnSubmitStatement(this, this.GetType(), "updatescript", "CKEDITOR.instances['ContentPlaceHolder1_editor1'].updateElement();");
    }

    protected void ddl_codeAreaA_Address_SelectedIndexChanged(object sender, EventArgs e)
    {
        Event.Checkddl(ddl_codeAreaA_Address, ddl_codeAreaB_Address);
    }

    protected void ddl_AreaCodeA_F_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Event.Checkddl(ddl_AreaCodeA_F, ddl_AreaCodeB_F);
    }

    public static void SetEventClass(DropDownList ddl, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select * from EventClass where EventCSNO <> 2", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static void SetEventRole(DropDownList ddl,UserInfo userInfo, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string sql = "Select * from EventRole where SystemID='S00'";       
        DataTable objDT = objDH.queryData(sql, aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
    protected void CheckBack(DropDownList ddl)
    {
        string Exchange = Request.QueryString["Exchange"] != null ? Request.QueryString["Exchange"].ToString() : "";
        if (ddl.SelectedValue == "2" && Exchange == "1")
        {
            Response.Redirect("ExChangeEvent.aspx");
        }
        else
        {
            Response.Redirect("Event.aspx");
        }
    }

    public static void InsertSQL(string EventSNO, Dictionary<string, object> adict)
    {
        Dictionary<string, object> bDict = adict;
        DataHelper objDH = new DataHelper();
        string batchSQL = @"Insert Into EventBatch(EventSNO,EventBSNO,CStartDay,CEndDay,CStartTime,CEndtime,Chour,CCount,[EventAreaCodeA],[EventAreaCodeAText],[EventArea]) 
                Values(@EventSNO,@EventBSNO,@CStartDay,@CEndDay,@CStartTime,@CEndtime,@Chour,@CCount,@EventAreaCodeA,@EventAreaCodeAText,@EventArea)";

        DataTable objDT = objDH.queryData(batchSQL, bDict);
        bDict.Clear();
    }

    public static void UpdateSQL(string EventSNO, Dictionary<string, object> adict, string EventBatch)
    {

        Dictionary<string, object> bDict = adict;
        DataHelper objDH = new DataHelper();
        string SQLBatch = "Select 1 from EventBatch where EventBSNO='" + EventBatch + "' and EventSNO='" + EventSNO + "'";
        DataTable ObjBatch = objDH.queryData(SQLBatch, null);
        if (ObjBatch.Rows.Count > 0)
        {
            string batchSQL = @"Update EventBatch Set CStartDay=@CStartDay,CEndDay=@CEndDay,CStartTime=@CStartTime,
                CEndtime=@CEndtime,Chour=@Chour,CCount=@CCount,EventAreaCodeA=@EventAreaCodeA,EventAreaCodeAText=@EventAreaCodeAText,       
                ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID,EventArea=@EventArea
                Where EventSNO=@EventSNO and EventBSNO=@EventBSNO";

            DataTable objDT = objDH.queryData(batchSQL, bDict);

        }
        else
        {
            InsertSQL(EventSNO, adict);
        }
        bDict.Clear();
    }
    #endregion
    //寫入行事曆(22縣市繼續教育)
    public static void Insert22ExCanlendar(string StartDate, string EndDate, string Title, string Url, string CreateDT, string CreateUserID, string PClassSNO, string ERSNO, bool IsEnable)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = @"Insert Into Calendar([EventSNO],[Title],[StartTime],[EndTime],[Url],[IsEnable],[CreateDT],[CreatUserID]) Values(@EventSNo,@Title,@StartTime,@EndTime,@Url,@IsEnable,@CreateDT,@CreateUserID)";
        adict.Add("EventSNO", Url);
        adict.Add("StartTime", StartDate);
        adict.Add("EndTime", EndDate);
        adict.Add("Title", Title);
        adict.Add("Url", "Event_AE.aspx?sno=" + Url + "&ersno=" + ERSNO);
        adict.Add("IsEnable", IsEnable);
        adict.Add("CreateDT", CreateDT);
        adict.Add("CreateUserID", CreateUserID);
        ObjDH.executeNonQuery(sql, adict);
    }
    protected void LK_NumLog_Click(object sender, EventArgs e)
    {

    }
    [System.Web.Services.WebMethod]
    public static Dictionary<string, object> SelectData(string s_Ipnut_Data1)
    {
        Dictionary<string, object> List_Data = new Dictionary<string, object>();
        List_Data.Add("DATA", "");
        return List_Data;
    }
}