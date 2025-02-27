using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_EventLocal_AE : System.Web.UI.Page
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
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
            
            Utility.SetDdlConfig(ddl_Class1, "CourseClass3", "請選擇");
            Utility.SetDdlConfig(ddl_Class2, "CourseClass4", "請選擇");
            SetEventClass(ddl_EventClass, "請選擇");
            SetEventRole(ddl_EventRole, "請選擇");
            Utility.setAreaCodeA(ddl_AreaCodeA_F, "請選擇");
            Utility.setAreaCodeA(ddl_AreaCodeA_S, "請選擇");
            Utility.setAreaCodeA(ddl_AreaCodeA_T, "請選擇");
            Utility.setAreaCodeA(ddl_codeAreaA_Active_F, "請選擇");
            Utility.setAreaCodeA(ddl_codeAreaA_Active_S, "請選擇");
            Utility.setAreaCodeA(ddl_codeAreaA_Active_T, "請選擇");
            Utility.setAreaCodeA(ddl_codeAreaA_Address, "請選擇");           
            Utility.setAreaCodeB(ddl_codeAreaB_Address, ddl_codeAreaA_Address.SelectedValue, "請選擇");
            Utility.setAreaCodeB(ddl_AreaCodeB_F, ddl_AreaCodeA_F.SelectedValue, "請選擇");
            Utility.setAreaCodeB(ddl_AreaCodeB_S, ddl_AreaCodeA_S.SelectedValue, "請選擇");
            Utility.setAreaCodeB(ddl_AreaCodeB_T, ddl_AreaCodeA_T.SelectedValue, "請選擇");
            Utility.setAreaCodeB(ddl_codeAreaB_Active_F, ddl_codeAreaA_Active_F.SelectedValue, "請選擇");
            Utility.setAreaCodeB(ddl_codeAreaB_Active_S, ddl_codeAreaA_Active_S.SelectedValue, "請選擇");
            Utility.setAreaCodeB(ddl_codeAreaB_Active_T, ddl_codeAreaA_Active_T.SelectedValue, "請選擇");
            if (work.Equals("N"))
            {
   
                GetRoleList();
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
                }
            }
            else
            {

                getData();
                string Exchange = Request.QueryString["Exchange"];
                if (Exchange == "1")
                {
                    ddl_Class1.Items.RemoveAt(1);
                    ddl_Class2.Items.RemoveAt(0);
                    ddl_Class2.Items.RemoveAt(0);
                }
                else
                {
                    ddl_Class1.Items.RemoveAt(2);
                    ddl_Class2.Items.RemoveAt(3);
                }
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
        //if (editor1.Value.Length > 500) errorMessage += "內容字數過多\\n";
        if (editor1.Value.Length == 0) errorMessage += "內容字數錯誤\\n";
        if (ddl_Class1.SelectedValue == "") errorMessage += "請選擇分類\\n";
        if (ddl_EventClass.SelectedValue == "") errorMessage += "請選擇類別\\n";
        if (ddl_EventRole.SelectedValue == "") errorMessage += "請選擇規則\\n";
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
        if (!string.IsNullOrEmpty(txt_CStartDay_S.Text))
        {
            DateTime start_S = Convert.ToDateTime(txt_CStartDay_S.Text);
            DateTime end_S = Convert.ToDateTime(txt_CEndDay_S.Text);
            if (start_S > end_S) errorMessage += "結束日期小於開始日期!\\n";

        }

        if (!string.IsNullOrEmpty(txt_CStartDay_T.Text))
        {
            DateTime start_T = Convert.ToDateTime(txt_CStartDay_T.Text);
            DateTime end_T = Convert.ToDateTime(txt_CEndDay_T.Text);
            if (start_T > end_T) errorMessage += "結束日期小於開始日期!\\n";
        }

        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        if (Work.Value.Equals("NEW"))
        {
            //新增
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("EventSNO", txt_ID.Value);
            aDict.Add("SYSTEM_ID", "S22");
            aDict.Add("EventName", txt_Title.Text); //活動名稱
            aDict.Add("EventCSNO", ddl_EventClass.SelectedValue);
            aDict.Add("ERSNO", ddl_EventRole.SelectedValue);
            aDict.Add("Class3", ddl_Class1.SelectedValue); //活動分類	
            aDict.Add("Class4", ddl_Class2.SelectedValue); //活動分類	
            aDict.Add("Note", HttpUtility.HtmlEncode(editor1.Value)); //內容
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
            string Strsql = "";
            if (ddl_Class1.SelectedValue == "1")
            {
                Strsql = @"Insert Into Event(EventName,Class3,Class4,CountLimit,CountAdmit,TargetHour,QTypeName,ActiveCost,StartTime,EndTime,Host,Note,CPerosn,CPhone,CMail,isEnable,CLocationAreaA,CLocationAreaCodeA,CLocationAreaB,CLocationAreaCodeB,CLocation,CreateUserID,SYSTEM_ID,EventCSNO,ERSNO,SupportMeals,CancelRole) 
                Values(@EventName,@Class3,@Class4,@CountLimit,@CountAdmit,@TargetHour,@QTypeName,@ActiveCost,@StartTime,@EndTime,@Host,@Note,@CPerosn,@CPhone,@CMail,@isEnable,@CLocationAreaA,@CLocationAreaCodeA,@CLocationAreaB,@CLocationAreaCodeB,@CLocation,@CreateUserID,@SYSTEM_ID,@EventCSNO,@ERSNO,@SupportMeals,@CancelRole) SELECT @@IDENTITY AS 'Identity'
                ";
            }
            else
            {
                Strsql = @"Insert Into Event(EventName,Class3,Class4,CountLimit,CountAdmit,TargetHour,QTypeName,ActiveCost,StartTime,EndTime,Host,Note,CPerosn,CPhone,CMail,isEnable,CLocationAreaA,CLocationAreaCodeA,CLocationAreaB,CLocationAreaCodeB,CLocation,CreateUserID,SYSTEM_ID,EventCSNO,ERSNO,SupportMeals,CancelRole) 
                Values(@EventName,@Class3,@Class4,@CountLimit,@CountAdmit,@TargetHour,@QTypeName,@ActiveCost,@StartTime,@EndTime,@Host,@Note,@CPerosn,@CPhone,@CMail,@isEnable,@CLocationAreaA,@CLocationAreaCodeA,@CLocationAreaB,@CLocationAreaCodeB,@CLocation,@CreateUserID,@SYSTEM_ID,@EventCSNO,@ERSNO,@SupportMeals,@CancelRole) SELECT @@IDENTITY AS 'Identity'
                ";
            }
            DataHelper objDH = new DataHelper();
            DataTable dt = objDH.queryData(Strsql, aDict);
            //寫入適用人員
            Utility.insertRoleBind(cb_Role, dt.Rows[0]["Identity"].ToString(), "Event_AE", userInfo.PersonSNO);

            //Insert EventBatch
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
            bDict.Add("EventAreaCodeB", ddl_AreaCodeB_F.SelectedValue);
            bDict.Add("EventAreaCodeBText", ddl_AreaCodeB_F.SelectedItem.Text);
            bDict.Add("EventArea", txt_ActiveArea_F.Text);
            bDict.Add("EventLocationCodeA", ddl_codeAreaA_Active_F.SelectedValue);
            bDict.Add("EventLocationCodeAText", ddl_codeAreaA_Active_F.SelectedItem.Text);
            bDict.Add("EventLocationCodeB", ddl_codeAreaB_Active_F.SelectedValue);
            bDict.Add("EventLocationCodeBText", ddl_codeAreaB_Active_F.SelectedItem.Text);
            bDict.Add("EventLocation", txt_codeAreaA_Active_F.Text);
            //aDict.Add("SupportMeals", chk_Meals.Checked);
            InsertSQL(EventSNO, bDict);

            //判斷第二資訊以及第三資訊
            if (!string.IsNullOrEmpty(txt_CStartDay_S.Text) && !string.IsNullOrEmpty(txt_CEndDay_S.Text) && !string.IsNullOrEmpty(txt_CStratTime_S.Text) && !string.IsNullOrEmpty(txt_CEndTime_S.Text))
            {
                Dictionary<string, object> cDict = new Dictionary<string, object>();
                cDict.Add("EventSNO", EventSNO);
                cDict.Add("EventBSNO", 2);
                cDict.Add("CStartDay", txt_CStartDay_S.Text);
                cDict.Add("CEndDay", txt_CEndDay_S.Text);
                cDict.Add("CStartTime", txt_CStratTime_S.Text);
                cDict.Add("CEndtime", txt_CEndTime_S.Text);
                cDict.Add("Chour", txt_CHour_S.Text);
                cDict.Add("CCount", txt_CCount_S.Text);
                cDict.Add("EventAreaCodeA", ddl_AreaCodeA_S.SelectedValue);
                cDict.Add("EventAreaCodeAText", ddl_AreaCodeA_S.SelectedItem.Text);
                cDict.Add("EventAreaCodeB", ddl_AreaCodeB_S.SelectedValue);
                cDict.Add("EventAreaCodeBText", ddl_AreaCodeB_S.SelectedItem.Text);
                cDict.Add("EventArea", txt_ActiveArea_S.Text);
                cDict.Add("EventLocationCodeA", ddl_codeAreaA_Active_S.SelectedValue);
                cDict.Add("EventLocationCodeAText", ddl_codeAreaA_Active_S.SelectedItem.Text);
                cDict.Add("EventLocationCodeB", ddl_codeAreaB_Active_S.SelectedValue);
                cDict.Add("EventLocationCodeBText", ddl_codeAreaB_Active_S.SelectedItem.Text);
                cDict.Add("EventLocation", txt_codeAreaA_Active_S.Text);
                InsertSQL(EventSNO, cDict);
            }
            if (!string.IsNullOrEmpty(txt_CStartDay_T.Text) && !string.IsNullOrEmpty(txt_CEndDay_T.Text) && !string.IsNullOrEmpty(txt_CStartTime_T.Text) && !string.IsNullOrEmpty(txt_CEndTime_T.Text))
            {
                Dictionary<string, object> dDict = new Dictionary<string, object>();
                dDict.Add("EventSNO", EventSNO);
                dDict.Add("EventBSNO", 3);
                dDict.Add("CStartDay", txt_CStartDay_T.Text);
                dDict.Add("CEndDay", txt_CEndDay_T.Text);
                dDict.Add("CStartTime", txt_CStartTime_T.Text);
                dDict.Add("CEndtime", txt_CEndTime_T.Text);
                dDict.Add("Chour", txt_CHour_T.Text);
                dDict.Add("CCount", txt_CCount_T.Text);
                dDict.Add("EventAreaCodeA", ddl_AreaCodeA_T.SelectedValue);
                dDict.Add("EventAreaCodeAText", ddl_AreaCodeA_T.SelectedItem.Text);
                dDict.Add("EventAreaCodeB", ddl_AreaCodeB_T.SelectedValue);
                dDict.Add("EventAreaCodeBText", ddl_AreaCodeB_T.SelectedItem.Text);
                dDict.Add("EventArea", txt_ActiveArea_T.Text);
                dDict.Add("EventLocationCodeA", ddl_codeAreaA_Active_T.SelectedValue);
                dDict.Add("EventLocationCodeAText", ddl_codeAreaA_Active_T.SelectedItem.Text);
                dDict.Add("EventLocationCodeB", ddl_codeAreaB_Active_T.SelectedValue);
                dDict.Add("EventLocationCodeBText", ddl_codeAreaB_Active_T.SelectedItem.Text);
                dDict.Add("EventLocation", txt_codeAreaA_Active_T.Text);
                InsertSQL(EventSNO, dDict);
               
            }
            bool IsEnable;
            if (Chk_Enable.Checked)
            {
                IsEnable = true;
            }
            else
            {
                IsEnable = false;
            }
            //Utility.Insert22ExCanlendar(txt_SignS_F.Text, txt_SignE_F.Text, txt_Title.Text, EventSNO, DateTime.Now.ToShortDateString(), userInfo.PersonSNO, "", ddl_EventRole.SelectedValue, Chk_Enable.Checked);
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert('新增成功');window.location ='EventLocal.aspx';", true);
            CheckBack(ddl_Class1);
        }
        else
        {

            //更新
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("EventSNO", txt_ID.Value);
            aDict.Add("EventCSNO", ddl_EventClass.SelectedValue);           
            aDict.Add("EventName", txt_Title.Text); //活動名稱            
            aDict.Add("ERSNO", ddl_EventRole.SelectedValue);
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
            DataHelper objDH = new DataHelper();
            if (ddl_Class1.SelectedValue == "1") { 
            objDH.executeNonQuery(@"Update Event Set EventName=@EventName,Class3=@Class3,Class4=@Class4,SupportMeals=@SupportMeals,
                Note=@Note,CountLimit=@CountLimit,CountAdmit=@CountAdmit,TargetHour=@TargetHour,QTypeName=@QTypeName,
                ActiveCost=@ActiveCost,StartTime=@StartTime,EndTime=@EndTime,Host=@Host,
                CPerosn=@CPerosn,CPhone=@CPhone,CMail=@CMail,CLocationAreaA=@CLocationAreaA,CLocationAreaCodeA=@CLocationAreaCodeA,CLocationAreaB=@CLocationAreaB,
                CLocationAreaCodeB=@CLocationAreaCodeB,CLocation=@CLocation,isEnable=@isEnable,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID,EventCSNO=@EventCSNO,ERSNO=@ERSNO
                Where EventSNO=@EventSNO", aDict);
            }
            else
            {
                objDH.executeNonQuery(@"Update Event Set EventName=@EventName,Class3=@Class3,Class4=@Class4,SupportMeals=@SupportMeals,
                Note=@Note,CountLimit=@CountLimit,CountAdmit=@CountAdmit,TargetHour=@TargetHour,QTypeName=@QTypeName,
                ActiveCost=@ActiveCost,StartTime=@StartTime,EndTime=@EndTime,Host=@Host,
                CPerosn=@CPerosn,CPhone=@CPhone,CMail=@CMail,CLocationAreaA=@CLocationAreaA,CLocationAreaCodeA=@CLocationAreaCodeA,CLocationAreaB=@CLocationAreaB,
                CLocationAreaCodeB=@CLocationAreaCodeB,CLocation=@CLocation,isEnable=@isEnable,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID,EventCSNO=@EventCSNO,ERSNO=@ERSNO
                Where EventSNO=@EventSNO", aDict);
            }
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
            bDict.Add("EventAreaCodeB", ddl_AreaCodeB_F.SelectedValue);
            bDict.Add("EventAreaCodeBText", ddl_AreaCodeB_F.SelectedItem.Text);
            bDict.Add("EventArea", txt_ActiveArea_F.Text);
            bDict.Add("EventLocationCodeA", ddl_codeAreaA_Active_F.SelectedValue);
            bDict.Add("EventLocationCodeAText", ddl_codeAreaA_Active_F.SelectedItem.Text);
            bDict.Add("EventLocationCodeB", ddl_codeAreaB_Active_F.SelectedValue);
            bDict.Add("EventLocationCodeBText", ddl_codeAreaB_Active_F.SelectedItem.Text);
            bDict.Add("EventLocation", txt_codeAreaA_Active_F.Text);
            bDict.Add("ModifyDT", DateTime.Now);
            bDict.Add("ModifyUserID", userInfo.PersonSNO);
            UpdateSQL(txt_ID.Value, bDict,"1");

            if (!string.IsNullOrEmpty(txt_CStartDay_S.Text) && !string.IsNullOrEmpty(txt_CEndDay_S.Text) && !string.IsNullOrEmpty(txt_CStratTime_S.Text) && !string.IsNullOrEmpty(txt_CEndTime_S.Text))
            {
                Dictionary<string, object> fDict = new Dictionary<string, object>();
                fDict.Add("EventSNO", txt_ID.Value);
                fDict.Add("EventBSNO", 2);
                fDict.Add("CStartDay", txt_CStartDay_S.Text);
                fDict.Add("CEndDay", txt_CEndDay_S.Text);
                fDict.Add("CStartTime", txt_CStratTime_S.Text);
                fDict.Add("CEndtime", txt_CEndTime_S.Text);
                fDict.Add("Chour", txt_CHour_S.Text);
                fDict.Add("CCount", txt_CCount_S.Text);
                fDict.Add("EventAreaCodeA", ddl_AreaCodeA_S.SelectedValue);
                fDict.Add("EventAreaCodeAText", ddl_AreaCodeA_S.SelectedItem.Text);
                fDict.Add("EventAreaCodeB", ddl_AreaCodeB_S.SelectedValue);
                fDict.Add("EventAreaCodeBText", ddl_AreaCodeB_S.SelectedItem.Text);
                fDict.Add("EventArea", txt_ActiveArea_S.Text);
                fDict.Add("EventLocationCodeA", ddl_codeAreaA_Active_S.SelectedValue);
                fDict.Add("EventLocationCodeAText", ddl_codeAreaA_Active_S.SelectedItem.Text);
                fDict.Add("EventLocationCodeB", ddl_codeAreaB_Active_S.SelectedValue);
                fDict.Add("EventLocationCodeBText", ddl_codeAreaB_Active_S.SelectedItem.Text);
                fDict.Add("EventLocation", txt_codeAreaA_Active_S.Text);
                fDict.Add("ModifyDT", DateTime.Now);
                fDict.Add("ModifyUserID", userInfo.PersonSNO);
                UpdateSQL(txt_ID.Value, fDict,"2");
            }
            if (!string.IsNullOrEmpty(txt_CStartDay_T.Text) && !string.IsNullOrEmpty(txt_CEndDay_T.Text) && !string.IsNullOrEmpty(txt_CStartTime_T.Text) && !string.IsNullOrEmpty(txt_CEndTime_T.Text))
            {
                Dictionary<string, object> eDict = new Dictionary<string, object>();
                eDict.Add("EventSNO", txt_ID.Value);
                eDict.Add("EventBSNO", 3);
                eDict.Add("CStartDay", txt_CStartDay_T.Text);
                eDict.Add("CEndDay", txt_CEndDay_T.Text);
                eDict.Add("CStartTime", txt_CStartTime_T.Text);
                eDict.Add("CEndtime", txt_CEndTime_T.Text);
                eDict.Add("Chour", txt_CHour_T.Text);
                eDict.Add("CCount", txt_CCount_T.Text);
                eDict.Add("EventAreaCodeA", ddl_AreaCodeA_T.SelectedValue);
                eDict.Add("EventAreaCodeAText", ddl_AreaCodeA_T.SelectedItem.Text);
                eDict.Add("EventAreaCodeB", ddl_AreaCodeB_T.SelectedValue);
                eDict.Add("EventAreaCodeBText", ddl_AreaCodeB_T.SelectedItem.Text);
                eDict.Add("EventArea", txt_ActiveArea_T.Text);
                eDict.Add("EventLocationCodeA", ddl_codeAreaA_Active_T.SelectedValue);
                eDict.Add("EventLocationCodeAText", ddl_codeAreaA_Active_T.SelectedItem.Text);
                eDict.Add("EventLocationCodeB", ddl_codeAreaB_Active_T.SelectedValue);
                eDict.Add("EventLocationCodeBText", ddl_codeAreaB_Active_T.SelectedItem.Text);
                eDict.Add("EventLocation", txt_codeAreaA_Active_T.Text);
                eDict.Add("ModifyDT", DateTime.Now);
                eDict.Add("ModifyUserID", userInfo.PersonSNO);
                UpdateSQL(txt_ID.Value, eDict,"3");
            }
            //寫入適用人員
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
               

            CheckBack(ddl_Class1);

        }
    }

    protected void newData()
    {
        Work.Value = "NEW";
        Utility.setRoleBind(cb_Role, "0", "");
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
            E.Cmail,E.CancelRole,E.IsEnable,E.CLocationAreaA,E.CLocationAreaB,E.CLocation,E.CLocationAreaCodeA,E.CLocationAreaCodeB,E.Class3,E.Class4,ER.ERSNO
            FROM  Event E
            LEFT JOIN EventClass EC on E.EventCSNO=EC.EventCSNO
            LEFT JOIN SYSTEM S on E.SYSTEM_ID=S.SYSTEM_ID
            LEFT Join EventRole ER On ER.ERSNO=E.ERSNO  
            Where E.EventSNO=@sno", aDict);
        //string a = objDT.Rows[0]["PClassSNO"].ToString();

        if (objDT.Rows.Count > 0)
        {
            ddl_codeAreaB_Address.Enabled = true;
            ddl_EventClass.SelectedValue= Convert.ToString(objDT.Rows[0]["EventCSNO"]);
            ddl_EventRole.SelectedValue= Convert.ToString(objDT.Rows[0]["ERSNO"]);
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
            editor1.Value = HttpUtility.HtmlDecode(Convert.ToString(objDT.Rows[0]["Note"]));
            txt_CPerosn.Text = Convert.ToString(objDT.Rows[0]["CPerosn"]);
            txt_CountLimit.Text = Convert.ToString(objDT.Rows[0]["CountLimit"]);
            txt_CountAdmit.Text = Convert.ToString(objDT.Rows[0]["CountAdmit"]);
            Chk_Enable.Checked = Convert.ToBoolean(objDT.Rows[0]["IsEnable"]);
            chk_Meals.Checked = Convert.ToBoolean(objDT.Rows[0]["SupportMeals"])?Convert.ToBoolean(objDT.Rows[0]["SupportMeals"]):false;            
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
                for (int i = 0; i < objDT_batch.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        txt_CStartDay_F.Text = Convert.ToDateTime(objDT_batch.Rows[i]["CStartDay"]).ToString("yyyy-MM-dd");
                        txt_CEndDay_F.Text = Convert.ToDateTime(objDT_batch.Rows[i]["CEndDay"]).ToString("yyyy-MM-dd");
                        txt_CStratTime_F.Text = Convert.ToString(objDT_batch.Rows[i]["CStartTime"]);
                        txt_CEndTime_F.Text = Convert.ToString(objDT_batch.Rows[i]["CEndtime"]);
                        txt_CHour_F.Text = Convert.ToString(objDT_batch.Rows[i]["Chour"]);
                        txt_CCount_F.Text = Convert.ToString(objDT_batch.Rows[i]["CCount"]);
                        ddl_AreaCodeA_F.SelectedValue = Convert.ToString(objDT_batch.Rows[i]["EventAreaCodeA"]);
                        ddl_AreaCodeB_F.SelectedValue = Convert.ToString(objDT_batch.Rows[i]["EventAreaCodeB"]);
                        txt_ActiveArea_F.Text = Convert.ToString(objDT_batch.Rows[i]["EventArea"]);
                        ddl_codeAreaA_Active_F.SelectedValue = Convert.ToString(objDT_batch.Rows[i]["EventLocationCodeA"]);
                        ddl_codeAreaB_Active_F.SelectedValue = Convert.ToString(objDT_batch.Rows[i]["EventLocationCodeB"]);
                        txt_codeAreaA_Active_F.Text = Convert.ToString(objDT_batch.Rows[i]["EventLocation"]);
                    }
                    if (i == 1)
                    {
                        txt_CStartDay_S.Text = Convert.ToDateTime(objDT_batch.Rows[i]["CStartDay"]).ToString("yyyy-MM-dd");
                        txt_CEndDay_S.Text = Convert.ToDateTime(objDT_batch.Rows[i]["CEndDay"]).ToString("yyyy-MM-dd");
                        txt_CStratTime_S.Text = Convert.ToString(objDT_batch.Rows[i]["CStartTime"]);
                        txt_CEndTime_S.Text = Convert.ToString(objDT_batch.Rows[i]["CEndtime"]);
                        txt_CHour_S.Text = Convert.ToString(objDT_batch.Rows[i]["Chour"]);
                        txt_CCount_S.Text = Convert.ToString(objDT_batch.Rows[i]["CCount"]);
                        ddl_AreaCodeA_S.SelectedValue = Convert.ToString(objDT_batch.Rows[i]["EventAreaCodeA"]);
                        ddl_AreaCodeB_S.SelectedValue = Convert.ToString(objDT_batch.Rows[i]["EventAreaCodeB"]);
                        txt_ActiveArea_S.Text = Convert.ToString(objDT_batch.Rows[i]["EventArea"]);
                        ddl_codeAreaA_Active_S.SelectedValue = Convert.ToString(objDT_batch.Rows[i]["EventLocationCodeA"]);
                        ddl_codeAreaB_Active_S.SelectedValue = Convert.ToString(objDT_batch.Rows[i]["EventLocationCodeB"]);
                        txt_codeAreaA_Active_S.Text = Convert.ToString(objDT_batch.Rows[i]["EventLocation"]);
                    }
                    if (i == 2)
                    {
                        txt_CStartDay_T.Text = Convert.ToDateTime(objDT_batch.Rows[i]["CStartDay"]).ToString("yyyy-MM-dd");
                        txt_CEndDay_T.Text = Convert.ToDateTime(objDT_batch.Rows[i]["CEndDay"]).ToString("yyyy-MM-dd");
                        txt_CStartTime_T.Text = Convert.ToString(objDT_batch.Rows[i]["CStartTime"]);
                        txt_CEndTime_T.Text = Convert.ToString(objDT_batch.Rows[i]["CEndtime"]);
                        txt_CHour_T.Text = Convert.ToString(objDT_batch.Rows[i]["Chour"]);
                        txt_CCount_T.Text = Convert.ToString(objDT_batch.Rows[i]["CCount"]);
                        ddl_AreaCodeA_T.SelectedValue = Convert.ToString(objDT_batch.Rows[i]["EventAreaCodeA"]);
                        ddl_AreaCodeB_T.SelectedValue = Convert.ToString(objDT_batch.Rows[i]["EventAreaCodeB"]);
                        txt_ActiveArea_T.Text = Convert.ToString(objDT_batch.Rows[i]["EventArea"]);
                        ddl_codeAreaA_Active_T.SelectedValue = Convert.ToString(objDT_batch.Rows[i]["EventLocationCodeA"]);
                        ddl_codeAreaB_Active_T.SelectedValue = Convert.ToString(objDT_batch.Rows[i]["EventLocationCodeB"]);
                        txt_codeAreaA_Active_T.Text = Convert.ToString(objDT_batch.Rows[i]["EventLocation"]);
                    }
                }
            }
        }

    }
    #region
    public static void InsertSQL(string EventSNO, Dictionary<string, object> adict)
    {
        Dictionary<string, object> bDict = adict;
        DataHelper objDH = new DataHelper();
        string batchSQL = @"Insert Into EventBatch(EventSNO,EventBSNO,CStartDay,CEndDay,CStartTime,CEndtime,Chour,CCount,EventArea,[EventAreaCodeA],[EventAreaCodeAText],[EventAreaCodeB],[EventAreaCodeBText],EventLocationCodeA,EventLocationCodeAText,EventLocationCodeB,EventLocationCodeBText,EventLocation) 
                Values(@EventSNO,@EventBSNO,@CStartDay,@CEndDay,@CStartTime,@CEndtime,@Chour,@CCount,@EventArea,@EventAreaCodeA,@EventAreaCodeAText,@EventAreaCodeB,@EventAreaCodeBText,@EventLocationCodeA,@EventLocationCodeAText,@EventLocationCodeB,@EventLocationCodeBText,@EventLocation)";

        DataTable objDT = objDH.queryData(batchSQL, bDict);
        bDict.Clear();
    }

    public static void UpdateSQL(string EventSNO, Dictionary<string, object> adict,string EventBatch)
    {

        Dictionary<string, object> bDict = adict;
        DataHelper objDH = new DataHelper();
        string SQLBatch = "Select 1 from EventBatch where EventBSNO='"+ EventBatch + "' and EventSNO='"+ EventSNO + "'";
        DataTable ObjBatch = objDH.queryData(SQLBatch, null);
        if (ObjBatch.Rows.Count > 0)
        { 
        string batchSQL = @"Update EventBatch Set CStartDay=@CStartDay,CEndDay=@CEndDay,CStartTime=@CStartTime,
                CEndtime=@CEndtime,Chour=@Chour,CCount=@CCount,EventAreaCodeA=@EventAreaCodeA,EventAreaCodeAText=@EventAreaCodeAText,
                EventAreaCodeB=@EventAreaCodeB,EventAreaCodeBText=@EventAreaCodeBText,EventArea=@EventArea,
                EventLocationCodeA=@EventLocationCodeA,EventLocationCodeAText=@EventLocationCodeAText,EventLocationCodeB=@EventLocationCodeB,EventLocationCodeBText=@EventLocationCodeBText,EventLocation=@EventLocation,
                ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID
                Where EventSNO=@EventSNO and EventBSNO=@EventBSNO";
        
        DataTable objDT = objDH.queryData(batchSQL, bDict);

        }
        else
        {
            InsertSQL(EventSNO, adict);
        }
        bDict.Clear();
    }

    public static void setClassSystem(DropDownList ddl, String DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER(ORDER BY SYSTEMSNO) as ROW_NO, SYSTEMSNO, SYSTEM_ID, SYSTEM_NAME FROM SYSTEM where ISEnable > 0 ", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static void SetCourse(DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT CourseSNO, CourseName FROM QS_Course where Ctype in (2,3,4)", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static void SetDdlConfig1(DropDownList ddl, string ddlType, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PGroup", ddlType);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("select a.PVal , a.MVal FROM Config a where a.PGroup = @PGroup ", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static void SetDdlConfig2(DropDownList ddl, string ddlType, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PGroup", ddlType);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("select a.PVal , a.MVal FROM Config a where a.PGroup = @PGroup and a.PVAL <> 1 and  a.PVAL <> 2", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
    public static void SetEventClass(DropDownList ddl , string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();      
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select * from EventClass where EventCSNO <> 9", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static void SetEventRole(DropDownList ddl, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select * from EventRole where SystemID='S22'", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
    public static void Checkddl(DropDownList ddl1, DropDownList ddl2)
    {
        ddl2.Items.Clear();
        String AreaCodeAddress = ddl1.SelectedValue;
        if (!String.IsNullOrEmpty(AreaCodeAddress))
        {
            Utility.setAreaCodeB(ddl2, AreaCodeAddress, "請選擇");
            ddl2.Enabled = true;
        }
        else
        {
            ddl2.Items.Add(new ListItem("請選擇", ""));
        }
    }

    private void GetRoleList()
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT A.RoleSNO , A.RoleName FROM Role A WHERE A.IsAdmin = 0", aDict);
        cb_Role.DataSource = objDT;
        cb_Role.DataBind();

        //修改預帶勾選
        String work = "";
        if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
        if (!work.Equals("N"))
        {
            String id = Convert.ToString(Request.QueryString["sno"]);
            aDict.Add("sno", id);
            objDT = objDH.queryData(@"SELECT A.RoleSNO FROM QS_CoursePlanningRole A WHERE A.PClassSNO = @sno", aDict);
            foreach (DataRow row in objDT.Rows)
            {

                foreach (ListItem item in cb_Role.Items)
                {
                    if (item.Value == row["RoleSNO"].ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }

            }

        }
    }



    protected void cb_Role_SelectedIndexChanged(object sender, EventArgs e)
    {

        Utility.SetDdlConfig(ddl_Class1, "CourseClass3", "請選擇");
        Utility.SetDdlConfig(ddl_Class2, "CourseClass4", "請選擇");
    
        ddl_Class1.Enabled = true;
        ddl_Class2.Enabled = true;
        string Exchange = Request.QueryString["Exchange"];
        if (Exchange == "1")
        {
            //ddl_Class1.Items.RemoveAt(1);
            ddl_Class2.Items.RemoveAt(1);
            ddl_Class2.Items.RemoveAt(1);
        }
        else
        {
            //ddl_Class1.Items.RemoveAt(2);
            ddl_Class2.Items.RemoveAt(2);
        }
        ScriptManager.RegisterOnSubmitStatement(this, this.GetType(), "updatescript", "CKEDITOR.instances['ContentPlaceHolder1_editor1'].updateElement();");
    }

    protected void ddl_codeAreaA_Address_SelectedIndexChanged(object sender, EventArgs e)
    {
        Checkddl(ddl_codeAreaA_Address, ddl_codeAreaB_Address);
    }

    protected void ddl_AreaCodeA_F_SelectedIndexChanged(object sender, EventArgs e)
    {
        Checkddl(ddl_AreaCodeA_F, ddl_AreaCodeB_F);
    }

    protected void ddl_codeAreaA_Active_F_SelectedIndexChanged(object sender, EventArgs e)
    {
        Checkddl(ddl_codeAreaA_Active_F, ddl_codeAreaB_Active_F);
    }

    protected void ddl_AreaCodeA_S_SelectedIndexChanged(object sender, EventArgs e)
    {
        Checkddl(ddl_AreaCodeA_S, ddl_AreaCodeB_S);
    }

    protected void ddl_codeAreaA_Active_S_SelectedIndexChanged(object sender, EventArgs e)
    {
        Checkddl(ddl_codeAreaA_Active_S, ddl_codeAreaB_Active_S);
    }

    protected void ddl_AreaCodeA_T_SelectedIndexChanged(object sender, EventArgs e)
    {
        Checkddl(ddl_AreaCodeA_T, ddl_AreaCodeB_T);
    }

    protected void ddl_codeAreaA_Active_T_SelectedIndexChanged(object sender, EventArgs e)
    {
        Checkddl(ddl_codeAreaA_Active_T, ddl_codeAreaB_Active_T);
    }

    protected void CheckBack(DropDownList ddl)
    {
        string Exchange = Request.QueryString["Exchange"] !=null ? Request.QueryString["Exchange"].ToString() : "";
        if (ddl.SelectedValue == "2"&& Exchange=="1")
        {
            Response.Redirect("ExChangeEvent_Local.aspx");
        }
        else
        {
            Response.Redirect("EventLocal.aspx");
        }
    }
    #endregion

}