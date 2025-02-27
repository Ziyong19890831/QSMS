using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

/// <summary>
/// Event 的摘要描述
/// </summary>
public class Event
{

    public string EventSNO { get; set; }
    public Event()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public static bool CheckIsEnable(string EvenSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = "Select IsEnable from Event where EventSNO=@EventSNO";
        adict.Add("EventSNO", EvenSNO);
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            bool IsEable = Convert.ToBoolean(ObjDT.Rows[0]["IsEnable"]);
            if (IsEable == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    public static string CopyEvent(string EventSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();

        string sqlEventSNO = @"INSERT INTO [dbo].[Event]([SYSTEM_ID],[EventName],[EventCSNO],[ERSNO],[CourseSNO]
                    ,[CLocation],[CDate],[CTime],[CPerosn],[CPhone],[CMail],[StartTime],[EndTime],[CountLimit],[CountAdmit],[Note]
                    ,[IsEnable],[CreateDT],[CreateUserID],[ModifyDT],[ModifyUserID],[PClassSNO],[Class3],[Class4],[TargetHour],[QTypeName]
                    ,[ActiveCost],[CLocationAreaA],[CLocationAreaCodeA],[CLocationAreaB],[CLocationAreaCodeB],[Host],[CancelRole],[EPClassSNO]
                    ,[SupportMeals],[ESNO])
                    SELECT Top(1) [SYSTEM_ID] ,[EventName]+'-複製' as EventName,[EventCSNO] ,[ERSNO] ,[CourseSNO] ,[CLocation] ,[CDate]
                    ,[CTime] ,[CPerosn] ,[CPhone] ,[CMail] ,[StartTime] ,[EndTime] ,[CountLimit] ,[CountAdmit] ,[Note] ,[IsEnable] ,[CreateDT]
                    ,[CreateUserID] ,[ModifyDT] ,[ModifyUserID] ,[PClassSNO] ,[Class3] ,[Class4] ,[TargetHour] ,[QTypeName] ,[ActiveCost]
                    ,[CLocationAreaA] ,[CLocationAreaCodeA] ,[CLocationAreaB] ,[CLocationAreaCodeB] ,[Host] ,[CancelRole] ,[EPClassSNO] ,[SupportMeals]
                    ,[ESNO]  FROM [Event] where EventSNO=@EventSNO SELECT @@IDENTITY AS 'Identity' ";
        adict.Add("EventSNO", EventSNO);
        DataTable dt = ObjDH.queryData(sqlEventSNO, adict);
        string EventSNOIdentity = dt.Rows[0]["Identity"].ToString();       
        return EventSNOIdentity;
    }

    public static void CopyEventBatch(string EventSNO, string EventSNOIdentity)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sqlEventBSNO = @"INSERT INTO [dbo].[EventBatch]([EventSNO],[EventBSNO],[CStartDay],[CEndDay],[CStartTime]
                                    ,[CEndtime],[Chour],[CCount],[EventAreaCodeA],[EventAreaCodeAText],[EventAreaCodeB],[EventAreaCodeBText],[EventArea]
                                    ,[EventLocationCodeA],[EventLocationCodeAText],[EventLocationCodeB],[EventLocationCodeBText],[EventLocation],[ModifyDT],[ModifyUserID])
                                    SELECT @CopyEventSNO,[EventBSNO],[CStartDay],[CEndDay],[CStartTime],[CEndtime],[Chour],[CCount]
                                    ,[EventAreaCodeA],[EventAreaCodeAText],[EventAreaCodeB],[EventAreaCodeBText]
                                    ,[EventArea],[EventLocationCodeA],[EventLocationCodeAText],[EventLocationCodeB]
                                    ,[EventLocationCodeBText],[EventLocation],[ModifyDT],[ModifyUserID]  FROM [EventBatch] where  EventSNO=@EventSNO";
        adict.Add("CopyEventSNO", EventSNOIdentity);
        adict.Add("EventSNO", EventSNO);
        ObjDH.executeNonQuery(sqlEventBSNO, adict);
    }

    public static void CopyRoleBind(string CopyEventSNO, string EventSNO, string CreateUserID, string TypeKey = "Event_AE")
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sqlCopyRoleBind = @"INSERT INTO [dbo].[RoleBind]([TypeKey],[CSNO],[RoleSNO],[CreateDT],[CreateUserID])
                                    Select @TypeKey,@CopyEventSNO,RoleSNO,getdate(),@CreateUserID From RoleBind where CSNO=@CSNO and TypeKey='Event_AE'
                ";
        adict.Add("TypeKey", TypeKey);
        adict.Add("CopyEventSNO", CopyEventSNO);
        adict.Add("CreateUserID", CreateUserID);
        adict.Add("CSNO", EventSNO);
        ObjDH.executeNonQuery(sqlCopyRoleBind, adict);
    }

    public static bool CheckEventNO(string PersonSNO, string EventSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = @"Select Count(*) from EventD ED
                                    Left Join Event E On E.EventSNO=ED.EventSNO
                                    where ED.PersonSNO=@PersonSNO and E.Event_NO=@EventNO";
        adict.Add("EventNO", GetEventNO(EventSNO));
        adict.Add("PersonSNO", PersonSNO);
        DataTable dt = ObjDH.queryData(sql, adict);
        int CountNO = Convert.ToInt32(dt.Rows[0][0]);
        if (CountNO >= 1)
        {
            return false;//同場次已報名過一次，不可再報名
        }
        else
        {
            return true;
        }

    }
    public static string GetEventNO(string EventSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = @"Select Event_NO from Event where EventSNO=@EventSNO";
        adict.Add("EventSNO", EventSNO);
        DataTable dt = ObjDH.queryData(sql, adict);

        string EventNO = dt.Rows[0][0].ToString();
        if (EventNO != "")
        {
            return EventNO;
        }
        else
        {
            return "";
        }

    }

    public static void GetRoleList(CheckBoxList cb_Role, string QueryString, string sno)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT A.RoleSNO , A.RoleName FROM Role A WHERE A.IsAdmin = 0 and A.DocGroup is not null", aDict);
        cb_Role.DataSource = objDT;
        cb_Role.DataBind();

        //修改預帶勾選
        String work = "";
        if (!string.IsNullOrEmpty(QueryString)) work = QueryString;
        if (!work.Equals("N"))
        {
            String id = Convert.ToString(sno);
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

    public static void InsertSQL(string EventSNO, Dictionary<string, object> adict)
    {
        Dictionary<string, object> bDict = adict;
        DataHelper objDH = new DataHelper();
        string batchSQL = @"Insert Into EventBatch(EventSNO,EventBSNO,CStartDay,CEndDay,CStartTime,CEndtime,Chour,CCount,[EventAreaCodeA],[EventAreaCodeAText],EventArea) 
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
                EventArea=@EventArea,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID
                Where EventSNO=@EventSNO and EventBSNO=@EventBSNO";

            DataTable objDT = objDH.queryData(batchSQL, bDict);

        }
        else
        {
            Event.InsertSQL(EventSNO, adict);
        }
        bDict.Clear();
    }

    public static void CheckBack(DropDownList ddl)
    {
        if (ddl.SelectedValue == "1")
        {
            HttpContext.Current.Response.Redirect("Event.aspx");
        }
        else
        {
            HttpContext.Current.Response.Redirect("ExchangeEvent.aspx");
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

    public static void CopyCalender(string EventSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, Object> adict = new Dictionary<string, object>();
        CopyCalender copyCalender = new CopyCalender();
        string sql = "Select * from Event where EventSNO=@EventSNO";
        adict.Add("EventSNO", EventSNO);
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        copyCalender.PClassSNO = ObjDT.Rows[0]["PClassSNO"].ToString();
        copyCalender.EventSNO= ObjDT.Rows[0]["EventSNO"].ToString();
        copyCalender.EventName = ObjDT.Rows[0]["EventName"].ToString();
        copyCalender.ERSNO = ObjDT.Rows[0]["ERSNO"].ToString();
        copyCalender.SDate = ObjDT.Rows[0]["StartTime"].ToString();
        copyCalender.EDate = ObjDT.Rows[0]["EndTime"].ToString();
        copyCalender.IsEnable =Convert.ToBoolean(ObjDT.Rows[0]["IsEnable"]);
        copyCalender.CreateUserID = ObjDT.Rows[0]["CreateUserID"].ToString();
        copyCalender.CreateDT = Convert.ToDateTime(ObjDT.Rows[0]["CreateDT"]).ToShortDateString();
        Utility.InsertCanlendar(copyCalender.SDate, copyCalender.EDate, copyCalender.EventName,copyCalender.EventSNO, copyCalender.CreateDT, copyCalender.CreateUserID, copyCalender.PClassSNO, copyCalender.ERSNO, copyCalender.EventSNO, copyCalender.IsEnable);
    }

    public static string EventGroupCode(string RoleGroup,string PersonSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, Object> adict = new Dictionary<string, object>();
        string sql = "Select * from EventGroup where PGroup='EventGroup' and Pval=@RoleGroup";
        adict.Add("RoleGroup", RoleGroup);
        adict.Add("PersonSNO", PersonSNO);
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        string Citysql = @"  select O.AreaCodeA from person P
  left join Organ O on O.OrganSNO = P.OrganSNO
  where P.PersonSNO =@PersonSNO ";
        
        DataTable CityObjDT = ObjDH.queryData(Citysql, adict);
        string city = CityObjDT.Rows[0]["AreaCodeA"].ToString();
        if (RoleGroup == "3")
        {
            string City = ObjDT.Rows[0]["City"].ToString();
            string Csql = "Select * from EventGroup where PGroup='EventGroup' and Pval=@RoleGroup and City=@City";
            adict.Add("City", city);
            DataTable CObjDT = ObjDH.queryData(Csql, adict);
            string CodeNum = CObjDT.Rows[0]["Mval"].ToString();
            return CodeNum;
        }
        else
        {
            for (int i = 0; i < ObjDT.Rows.Count; i++)
            {
                string GroupNum = ObjDT.Rows[i]["Pval"].ToString();
                string CodeNum = ObjDT.Rows[i]["Mval"].ToString();
                switch (GroupNum)
                {
                    case "10":
                        return CodeNum;
                    case "11":
                        return CodeNum;
                    case "12":
                        return CodeNum;
                    case "13":
                        return CodeNum;
                    case "1":
                        return CodeNum;
                    case "0":
                        return CodeNum;
                }
            }
        }
        return "Error";
    }

    public static void InsertEventGroupNum(string EventSNO,string Num,string EventGroup,UserInfo userInfo)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, Object> adict = new Dictionary<string, object>();
        string sql = @"INSERT INTO [dbo].[EventGroupNum]
           ([EventSNO]
           ,[EventGroup]
           ,[EventNum]
           ,[CreateDT]
           ,[CreateUserID])
     VALUES
           (@EventSNO
           ,@EventGroup
           ,@EventNum
           ,getdate()
           ,@CreateUserID)";
        
        adict.Add("EventGroup", EventGroup);
        adict.Add("EventSNO", EventSNO);
        adict.Add("EventNum", Num);
        adict.Add("CreateUserID", userInfo.PersonSNO);
        ObjDH.executeNonQuery(sql, adict);
    }

    public static void UpdateEventGroupNum(string EventSNO, string Num, string EventGroup, UserInfo userInfo)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, Object> dict = new Dictionary<string, object>();
        string ChkSQL = "Select 1 from EventGroupNum where EventSNO=@EventSNO";
        dict.Add("EventSNO", EventSNO);
        DataTable ObjDT = ObjDH.queryData(ChkSQL, dict);
        if (ObjDT.Rows.Count > 0)
        {
            Dictionary<string, Object> adict = new Dictionary<string, object>();
            string sql = @"UPDATE [dbo].[EventGroupNum]
                        SET [EventGroup] = @EventGroup
                        ,[EventNum] = @EventNum
                        ,[ModifyDT] = getdate()
                        ,[ModifyUserID] = @ModifyUserID where EventSNO=@EventSNO";
            if (EventGroup == "Error")
            {
                string EventGroup_ = "";
                switch (userInfo.RoleGroup)
                {
                    case "10":
                        EventGroup_ = "A";
                        break;
                    case "11":
                        EventGroup_ = "B";
                        break;
                    case "12":
                        EventGroup_ = "C";
                        break;
                    case "13":
                        EventGroup_ = "D";
                        break;
                    case "1":
                        EventGroup_ = "S";
                        break;
                }
                adict.Add("EventGroup", EventGroup_);
            }
            else
            {
                adict.Add("EventGroup", EventGroup);
            }

            adict.Add("EventSNO", EventSNO);
            adict.Add("EventNum", Num);
            adict.Add("ModifyUserID", userInfo.PersonSNO);
            ObjDH.executeNonQuery(sql, adict);
        }
        else
        {
            if (EventGroup == "Error")
            {
                string EventGroup_ = "";
                switch (userInfo.RoleGroup)
                {
                    case "10":
                        EventGroup_ = "A";
                        break;
                    case "11":
                        EventGroup_ = "B";
                        break;
                    case "12":
                        EventGroup_ = "C";
                        break;
                    case "13":
                        EventGroup_ = "D";
                        break;
                    case "1":
                        EventGroup_ = "S";
                        break;
                }
                InsertEventGroupNum(EventSNO, Num, EventGroup_, userInfo);
            }
            else
            {
                InsertEventGroupNum(EventSNO, Num, EventGroup, userInfo);
            }

           
        }
       
    }

    public static string GetEventGroupNum(string EventSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, Object> adict = new Dictionary<string, object>();
        string sql = "Select * from EventGroupNum where EventSNO=@EventSNO";
        adict.Add("EventSNO", EventSNO);
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        if (ObjDT.Rows.Count>0)
        {
            return ObjDT.Rows[0]["EventNum"].ToString();
        }
        else
        {
            return "無編號";
        }
    }

    public static string GetEventGroupCode(string EventSNO, UserInfo userInfo)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, Object> adict = new Dictionary<string, object>();
        string sql = "Select * from EventGroupNum where EventSNO=@EventSNO";
        adict.Add("EventSNO", EventSNO);
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT.Rows[0]["EventGroup"].ToString();
        }
        else
        {
            string EventGroup_ = "";
            switch (userInfo.RoleGroup)
            {
                case "10":
                    EventGroup_ = "A";
                    break;
                case "11":
                    EventGroup_ = "B";
                    break;
                case "12":
                    EventGroup_ = "C";
                    break;
                case "13":
                    EventGroup_ = "D";
                    break;
                case "1":
                    EventGroup_ = "S";
                    break;
            }
            return EventGroup_;
           
        }
    }

    public static string GetEventDGroupNum(string EventSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, Object> adict = new Dictionary<string, object>();
        string sql = "Select * from EventGroupNum where EventSNO=@EventSNO";
        adict.Add("EventSNO", EventSNO);
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT.Rows[0]["EventGroup"].ToString()+ ObjDT.Rows[0]["EventNum"].ToString();
        }
        else
        {
            return "Error";
        }
    }

    public static bool ChkEventDGroupNum(string PersonSNO,string EventGroupNum)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, Object> adict = new Dictionary<string, object>();
        string sql = "Select * from EventD where EventGroupNum=@EventGroupNum and PersonSNO=@PersonSNO";
        adict.Add("PersonSNO", PersonSNO);
        adict.Add("EventGroupNum", EventGroupNum);
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        if(ObjDT.Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
        
    }

    public static string ChkEventS22(string EventSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, Object> adict = new Dictionary<string, object>();
        string sql = "Select * from Event where EventSNO=@EventSNO";
        adict.Add("EventSNO", EventSNO);       
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        string EventType = ObjDT.Rows[0]["SYSTEM_ID"].ToString();
        if (ObjDT.Rows.Count > 0)
        {
            if (EventType == "S22") return "S22";
            if (EventType == "S00") return "S00";
        }
        return "";
    }

    public static bool ChkERSNO(string EventSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, Object> adict = new Dictionary<string, object>();
        string sql = "Select ERSNO from Event where EventSNO=@EventSNO";
        adict.Add("EventSNO", EventSNO);        
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        string ERSNO = ObjDT.Rows[0]["ERSNO"].ToString();
        if (ERSNO=="")
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    public static bool ChkEventCount(string EventSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, Object> adict = new Dictionary<string, object>();
        string SQL = @"
             SELECT 
                EventSNO, EventName, E.Note, CountLimit, E.TargetHour,
				CountAdmit,Convert(varchar, StartTime, 120) StartTime, Convert(varchar, EndTime, 120) EndTime,
                E.CLocation,E.CDate,E.CTime,E.CPerosn,E.CPhone,E.CMail,
                CF.MVal Class1,CF2.MVal Class2,E.Note,(E.CLocationAreaA+E.CLocationAreaB+E.CLocation)  L,E.QTypeName,
                E.ActiveCost,E.Host,
                " + Utility.setSQL_RoleBindName("Event_AE", "E.EventSNO") + @",E.TargetHour
            From Event E 			
				Left Join config CF on CF.Pval=E.class3 and CF.[PGroup]='CourseClass3' 
				Left Join config CF2 on CF2.Pval=E.class4 and CF2.[PGroup]='CourseClass4'
            Where E.EventSNO=@EventSNO";
        adict.Add("EventSNO", EventSNO);
        DataTable ObjDT = ObjDH.queryData(SQL, adict);
        string CountLimit = ObjDT.Rows[0]["CountLimit"].ToString();

        DataTable DT = ObjDH.queryData(@"
            SELECT p.PName 
            From EventD e
                Left Join Person p On p.PersonSNO=e.PersonSNO
            Where EventSNO=@EventSNO and Audit<>2 and Audit<>5", adict);//未審 錄取 審核中 備取
        if (CountLimit != "0")
        {
            if (Convert.ToInt16(CountLimit) <= DT.Rows.Count && Convert.ToInt16(CountLimit) != 0)
            {
                return false;
            }
            return true;
        }
        else
        {
            return true;

        }
    }
}


public class CopyCalender
{
    public string PClassSNO { get; set; }
    public string EventSNO { get; set; }
    public string EventName { get; set; }
    public string ERSNO { get; set; }
    public bool IsEnable { get; set; }
    public string SDate { get; set; }
    public string EDate { get; set; }
    public string CreateUserID { get; set; }
    public string CreateDT { get; set; }
}
