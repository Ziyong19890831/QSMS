using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_AutoSendMail : System.Web.UI.Page
{
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfo;
    public Dictionary<string, string> _SetCol;
    public UserInfo userInfo = null;
    int viewrole = 1;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Ex = Request.QueryString["Ex"]==null?"": Request.QueryString["Ex"].ToString();
            if (Ex == "")
            {
                SetDdlEventTitle(ddl_EventTitle, "請選擇");
            }
            else
            {
                SetDdlEventTitleEX(ddl_EventTitle, "請選擇");
                chk_Search.Visible = false;
                lb_Search.Visible = false;
            }
            Utility.setddl_CertificateTypeForAutoSend(ddl_Certificate, "請選擇");
            setPlanNameNotEX(ddl_CoursePlanningClass, "請選擇");
            setRoleNormal(Chk_Role, "請選擇");
            txt_CertEndDate_S.Enabled = false;
            txt_CertEndDate_E.Enabled = false;
        }
    }
    private void ReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("PName", "姓名");
        _SetCol.Add("RoleName", "身份別");
        _SetCol.Add("PMail", "信箱");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.AutoSendMail.ToString()] = _ExcelInfo;
    }
    protected void btnSendMail_Click(object sender, EventArgs e)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string Role = getChk_Role();
        string Name = "";
        string EventName = ddl_EventTitle.SelectedItem.Text + DateTime.Now.ToShortDateString();
        string sn = Email.CreateGroup(EventName);
        DataTable ObjDT = ReturnTable(chk_Search.Checked, ddl_Certificate.SelectedValue, Role, chk_Certificate.SelectedValue, txt_CertEndDate_S.Text, txt_CertEndDate_E.Text, chk_NotGetIntegral.Checked, chk_Type.Checked, ddl_OnlineIntegral.SelectedValue, ddl_Entity.SelectedValue, ddl_CoursePlanningClass.SelectedValue);
        string Title = "Email,Name";
        for(int i=0;i< ObjDT.Rows.Count;i++)
        {
            Name += "\n" + ObjDT.Rows[i]["PMail"].ToString() + ",";
            Name += ObjDT.Rows[i]["PName"].ToString();
        }
        //JObject匿名物件
        JObject obj = new JObject(
             new JProperty("contacts", Title + Name)
            );
        //序列化為JSON字串並輸出結果
        var result = JsonConvert.SerializeObject(obj, Formatting.Indented);
        Email.InsertMemberString(sn, result);
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string SQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate()) SELECT @@IDENTITY AS 'Identity'";
        dict.Add("SN", sn);
        dict.Add("EventName", EventName);
        dict.Add("MailContent", editor_Mail.Value);
        DataTable dt = ObjDH.queryData(SQL, dict);
        Response.Write("<script>alert('寄送成功!');window.close();</script>");
    }

    protected void ddl_EventTitle_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = "Select Note from Event where EventSNO=@EventSNO";
        adict.Add("EventSNO", ddl_EventTitle.SelectedValue);
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        if (ObjDT.Rows.Count > 0)
        {

            editor_Mail.Value = HttpUtility.HtmlDecode(Convert.ToString(ObjDT.Rows[0]["Note"]));
            editor_Mail.Value += "<br/>實體課程報名連結：<a href=https://quitsmoking.hpa.gov.tw/Web/Event.aspx>請點此連結</a>";
        }
        else
        {
            editor_Mail.Value = "";
        }
    }

    protected void btn_Download_Click(object sender, EventArgs e)
    {
        string Role = getChk_Role();
        DataTable ObjDT = ReturnTable(chk_Search.Checked,ddl_Certificate.SelectedValue, Role, chk_Certificate.SelectedValue,txt_CertEndDate_S.Text, txt_CertEndDate_E.Text, chk_NotGetIntegral.Checked,chk_Type.Checked,ddl_OnlineIntegral.SelectedValue,ddl_Entity.SelectedValue,ddl_CoursePlanningClass.SelectedValue);
        ReportInit(ObjDT);
        Utility.OpenExportWindows(this, ReportEnum.AutoSendMail.ToString());

    }

    public static DataTable ReturnTable(bool Search, string CtypeSNO, string RoleSNO, string getCertificate, string CertEndDate_S, string CertEndDate_E, bool NotGetIntegral,bool SameType,string OnlineIntegral,string EntityIntegral,string PClassSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = "";
        #region 搜尋條件
        if (!Search)
        {
            if (SameType)
            {
                CtypeSNO = CertificateRoleType(CtypeSNO);
            }


            //如果沒有開啟進階搜尋
            sql += "Select P.PName,R.RoleName,P.PMail,P.PersonID from Person P";
            sql += " Left Join QS_Certificate QC On QC.PersonID=P.PersonID and QC.CtypeSNO IN (" + CtypeSNO + ")";
            sql += "  Left Join Role R On R.RoleSNO=P.RoleSNO ";
            sql += "  where P.RoleSNO IN ("+ RoleSNO + ") and CtypeSNO "; 
            if (getCertificate =="1") 
            { 
                sql += " is not null";
                if (CertEndDate_S != "")
                {
                    sql += " And QC.CertEndDate >= '" + CertEndDate_S + "'";
                }
                if (CertEndDate_E != "")
                {
                    sql += " And QC.CertEndDate <= '" + CertEndDate_E + "'";
                }
            }
            else
            {
                sql += " is null";
            }
            DataTable ObjDT = ObjDH.queryData(sql, null);
            if (ObjDT.Rows.Count > 0)
            {
                return ObjDT;
            }
            else
            {
                return null;
            }
        }
        else
        {
            //開啟進階搜索
            if (NotGetIntegral)
            {
                //勾選未取得積分
                 sql = @"with Temp as (
                        Select distinct P.PName,P.PMail,P.PersonID,R.RoleName,(select Count(ISNO) from QS_Integral where PersonSNO=P.PersonSNO) CountQI
                        ,(select Count(CertSNO) from QS_Certificate QC where QC.PersonID=P.PersonID) CountQC
                        from Person P 
                        Left Join QS_Integral Qi On QI.PersonSNO=P.PersonSNO
                        Left Join Role R On R.RoleSNO=P.RoleSNO
                        where P.RoleSNO In ('"+RoleSNO+"') )  Select * from Temp where CountQI=0 and CountQC=0";
                DataTable ObjDT = ObjDH.queryData(sql, adict);
                if (ObjDT.Rows.Count > 0)
                {
                    return ObjDT;
                }
                else
                {
                    return null;
                }
            }
            if (OnlineIntegral == "0" && EntityIntegral == "0")
            {
                string sqlOnline = @"with getalltypeCourse as(
                Select QC.PClassSNO,C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour)應取得 
                from QS_Course QC
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                Left Join QS_CoursePlanningClass QCPR On QCPR.PClassSNO=QC.PClassSNO
                where QC.PClassSNO=@PClassSNO and QC.IsEnable=1 
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType,QC.PClassSNO,QCPR.SignLimit
                )
                , getIntegral as (
                Select  QI.PersonSNO,C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour) 已取得
                from QS_Integral QI
                Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                Left Join Person P On P.PersonSNO=QI.personSNO
                where QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType,QI.PersonSNO
                
                ),getCoursePair as (
				Select Class1,Ctype,SUM(QC.Chour)CoursepairCount  from 
				QS_Course QC
				where PairCourseSNO <> 0 and PClassSNO=37
				Group by Class1,Ctype
				),AnalyticsPair as (
                    Select QC.Class1,QC.Ctype,QC.PairCourseSNO CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where  QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                    Group by QC.Class1,QC.Ctype,QC.PairCourseSNO
                    INTERSECT 
                    Select QC.Class1,QC.Ctype,QI.CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                    Group by QC.Class1,QC.Ctype,QI.CourseSNO)
                    ,getPairCourseSNO As(				  
					Select Class1,Ctype,SUM(Pair) pair from AnalyticsPair 
                    Group by Class1,Ctype
				  ),getalreadyI as(
                Select gI.PersonSNO,gac.PClassSNO,gac.課程類別,gac.Class1,gac.授課方式,gac.CType,gac.應取得-isnull(GCP.CoursepairCount,0) 應取得 ,isnull(gI.已取得,0)-isnull(GPC.Pair,0) 已取得 from getalltypeCourse gac
                Left Join getIntegral gI On gI.Class1=gac.Class1 and gI.CType=gac.CType
				 Left Join getPairCourseSNO GPC On GPC.Class1=gac.Class1 and GPC.CType=gac.CType
				 Left Join getCoursePair GCP On GCP.Class1=gac.Class1 and GCP.CType=gac.CType
                )
			
				,getResult as(
                Select P.PName,R.RoleName,P.PMail,P.PersonID,P.RoleSNO,gaI.*, case when gaI.應取得-gaI.已取得 < 0 then 0　Else gaI.應取得-gaI.已取得  END 未取得 from getalreadyI gaI
				Left Join Person P On P.personSNO=gaI.PersonSNO
				Left Join Role R ON R.RoleSNO=P.RoleSNO
               )
				Select * from getResult
				where getResult.未取得=0 And Class1=1 and CType=1 and RoleSNO In (" + RoleSNO + ")     Order by getResult.課程類別,getResult.授課方式 DESC ";
                adict.Add("PClassSNO", PClassSNO);

                DataTable ObjDT = ObjDH.queryData(sqlOnline, adict);
                string sqlEntity = @"with getalltypeCourse as(
                Select QC.PClassSNO,C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour)應取得 
                from QS_Course QC
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                where QC.PClassSNO=@PClassSNO and QC.IsEnable=1 
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType,QC.PClassSNO
                )
                , getIntegral as (
                Select  QI.PersonSNO,C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour) 已取得
                from QS_Integral QI
                Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                Left Join Person P On P.PersonSNO=QI.personSNO
                where QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType,QI.PersonSNO
                
                ),getCoursePair as (
				Select Class1,Ctype,SUM(QC.Chour)CoursepairCount  from 
				QS_Course QC
				where PairCourseSNO <> 0 and PClassSNO=37
				Group by Class1,Ctype
				),AnalyticsPair as (
                    Select QC.Class1,QC.Ctype,QC.PairCourseSNO CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where  QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                    Group by QC.Class1,QC.Ctype,QC.PairCourseSNO
                    INTERSECT 
                    Select QC.Class1,QC.Ctype,QI.CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                    Group by QC.Class1,QC.Ctype,QI.CourseSNO)
                    ,getPairCourseSNO As(				  
					Select Class1,Ctype,SUM(Pair) pair from AnalyticsPair 
                    Group by Class1,Ctype
				  ),getalreadyI as(
                Select gI.PersonSNO,gac.PClassSNO,gac.課程類別,gac.Class1,gac.授課方式,gac.CType,gac.應取得-isnull(GCP.CoursepairCount,0) 應取得 ,isnull(gI.已取得,0)-isnull(GPC.Pair,0) 已取得 from getalltypeCourse gac
                Left Join getIntegral gI On gI.Class1=gac.Class1 and gI.CType=gac.CType
				 Left Join getPairCourseSNO GPC On GPC.Class1=gac.Class1 and GPC.CType=gac.CType
				 Left Join getCoursePair GCP On GCP.Class1=gac.Class1 and GCP.CType=gac.CType
                )
			
				,getResult as(
                Select P.PName,R.RoleName,P.PMail,P.PersonID,gaI.*, case when gaI.應取得-gaI.已取得 < 0 then 0　Else gaI.應取得-gaI.已取得  END 未取得 from getalreadyI gaI
				Left Join Person P On P.personSNO=gaI.PersonSNO
				Left Join Role R ON R.RoleSNO=P.RoleSNO
               )
				Select * from getResult
				where  Class1=1 and 未取得=0 and CType=2
				Order by getResult.課程類別,getResult.授課方式 DESC";

                DataTable ObjDTList = ObjDH.queryData(sqlEntity, adict);
                DataTable AllData = new DataTable();
                AllData.Columns.Add("PName");
                AllData.Columns.Add("RoleName");
                AllData.Columns.Add("PMail");
                for (int i = 0; i < ObjDT.Rows.Count; i++)
                {
                    string PersonID = ObjDT.Rows[i]["PersoniD"].ToString();
                    for (int j = 0; j < ObjDTList.Rows.Count; j++)
                    {
                        string PersonID_List = ObjDTList.Rows[j]["PersonID"].ToString();
                        if(PersonID== PersonID_List)
                        {
                            DataRow row = AllData.NewRow();
                            row["PName"] = ObjDT.Rows[i]["PName"].ToString();
                            row["RoleName"] = ObjDT.Rows[i]["RoleName"].ToString();
                            row["PMail"] = ObjDT.Rows[i]["PMail"].ToString();
                            AllData.Rows.Add(row);
                        }
                    }
                }
                return AllData;
            }
            if (OnlineIntegral == "0")
            {
                sql = @"with getalltypeCourse as(
                Select QC.PClassSNO,C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour)應取得 
                from QS_Course QC
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                Left Join QS_CoursePlanningClass QCPR On QCPR.PClassSNO=QC.PClassSNO
                where QC.PClassSNO=@PClassSNO and QC.IsEnable=1 
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType,QC.PClassSNO,QCPR.SignLimit
                )
                , getIntegral as (
                Select  QI.PersonSNO,C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour) 已取得
                from QS_Integral QI
                Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                Left Join Person P On P.PersonSNO=QI.personSNO
                where QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType,QI.PersonSNO
                
                ),getCoursePair as (
				Select Class1,Ctype,SUM(QC.Chour)CoursepairCount  from 
				QS_Course QC
				where PairCourseSNO <> 0 and PClassSNO=37
				Group by Class1,Ctype
				),AnalyticsPair as (
                    Select QC.Class1,QC.Ctype,QC.PairCourseSNO CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where  QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                    Group by QC.Class1,QC.Ctype,QC.PairCourseSNO
                    INTERSECT 
                    Select QC.Class1,QC.Ctype,QI.CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                    Group by QC.Class1,QC.Ctype,QI.CourseSNO)
                    ,getPairCourseSNO As(				  
					Select Class1,Ctype,SUM(Pair) pair from AnalyticsPair 
                    Group by Class1,Ctype
				  ),getalreadyI as(
                Select gI.PersonSNO,gac.PClassSNO,gac.課程類別,gac.Class1,gac.授課方式,gac.CType,gac.應取得-isnull(GCP.CoursepairCount,0) 應取得 ,isnull(gI.已取得,0)-isnull(GPC.Pair,0) 已取得 from getalltypeCourse gac
                Left Join getIntegral gI On gI.Class1=gac.Class1 and gI.CType=gac.CType
				 Left Join getPairCourseSNO GPC On GPC.Class1=gac.Class1 and GPC.CType=gac.CType
				 Left Join getCoursePair GCP On GCP.Class1=gac.Class1 and GCP.CType=gac.CType
                )
			
				,getResult as(
                Select P.PName,R.RoleName,P.PMail,P.PersonID,P.RoleSNO,gaI.*, case when gaI.應取得-gaI.已取得 < 0 then 0　Else gaI.應取得-gaI.已取得  END 未取得 from getalreadyI gaI
				Left Join Person P On P.personSNO=gaI.PersonSNO
				Left Join Role R ON R.RoleSNO=P.RoleSNO
               )
				Select * from getResult
				where getResult.未取得=0 And Class1=1 and CType=1 and RoleSNO In (" + RoleSNO + ")     Order by getResult.課程類別,getResult.授課方式 DESC ";
                adict.Add("PClassSNO", PClassSNO);
               
                DataTable ObjDT = ObjDH.queryData(sql, adict);
                return ObjDT;
            }
            if (OnlineIntegral == "1")
            {
                sql = @"with getalltypeCourse as(
                Select QC.PClassSNO,C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour)應取得 
                from QS_Course QC
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                where QC.PClassSNO=@PClassSNO and QC.IsEnable=1 
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType,QC.PClassSNO
                )
                , getIntegral as (
                Select  QI.PersonSNO,C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour) 已取得
                from QS_Integral QI
                Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                Left Join Person P On P.PersonSNO=QI.personSNO
                where QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType,QI.PersonSNO
                
                ),getCoursePair as (
				Select Class1,Ctype,SUM(QC.Chour)CoursepairCount  from 
				QS_Course QC
				where PairCourseSNO <> 0 and PClassSNO=37
				Group by Class1,Ctype
				),AnalyticsPair as (
                    Select QC.Class1,QC.Ctype,QC.PairCourseSNO CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where  QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                    Group by QC.Class1,QC.Ctype,QC.PairCourseSNO
                    INTERSECT 
                    Select QC.Class1,QC.Ctype,QI.CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                    Group by QC.Class1,QC.Ctype,QI.CourseSNO)
                    ,getPairCourseSNO As(				  
					Select Class1,Ctype,SUM(Pair) pair from AnalyticsPair 
                    Group by Class1,Ctype
				  ),getalreadyI as(
                Select gI.PersonSNO,gac.PClassSNO,gac.課程類別,gac.Class1,gac.授課方式,gac.CType,gac.應取得-isnull(GCP.CoursepairCount,0) 應取得 ,isnull(gI.已取得,0)-isnull(GPC.Pair,0) 已取得 from getalltypeCourse gac
                Left Join getIntegral gI On gI.Class1=gac.Class1 and gI.CType=gac.CType
				 Left Join getPairCourseSNO GPC On GPC.Class1=gac.Class1 and GPC.CType=gac.CType
				 Left Join getCoursePair GCP On GCP.Class1=gac.Class1 and GCP.CType=gac.CType
                )
			
				,getResult as(
                Select P.PName,R.RoleName,P.PMail,P.PersonID,P.RoleSNO,gaI.*, case when gaI.應取得-gaI.已取得 < 0 then 0　Else gaI.應取得-gaI.已取得  END 未取得 from getalreadyI gaI
				Left Join Person P On P.personSNO=gaI.PersonSNO
				Left Join Role R ON R.RoleSNO=P.RoleSNO
               )
				Select * from getResult
				where getResult.未取得 > 0 And Class1=1 and CType=1 and RoleSNO In (" + RoleSNO + ")  Order by getResult.課程類別,getResult.授課方式 DESC";

                adict.Add("PClassSNO", PClassSNO);
                DataTable ObjDT = ObjDH.queryData(sql, adict);
                return ObjDT;
            }          
            if (EntityIntegral == "0")
            {
                sql = @"with getalltypeCourse as(
                Select QC.PClassSNO,C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour)應取得 
                from QS_Course QC
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                where QC.PClassSNO=@PClassSNO and QC.IsEnable=1 
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType,QC.PClassSNO
                )
                , getIntegral as (
                Select  QI.PersonSNO,C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour) 已取得
                from QS_Integral QI
                Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                Left Join Person P On P.PersonSNO=QI.personSNO
                where QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType,QI.PersonSNO
                
                ),getCoursePair as (
				Select Class1,Ctype,SUM(QC.Chour)CoursepairCount  from 
				QS_Course QC
				where PairCourseSNO <> 0 and PClassSNO=37
				Group by Class1,Ctype
				),AnalyticsPair as (
                    Select QC.Class1,QC.Ctype,QC.PairCourseSNO CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where  QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                    Group by QC.Class1,QC.Ctype,QC.PairCourseSNO
                    INTERSECT 
                    Select QC.Class1,QC.Ctype,QI.CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                    Group by QC.Class1,QC.Ctype,QI.CourseSNO)
                    ,getPairCourseSNO As(				  
					Select Class1,Ctype,SUM(Pair) pair from AnalyticsPair 
                    Group by Class1,Ctype
				  ),getalreadyI as(
                Select gI.PersonSNO,gac.PClassSNO,gac.課程類別,gac.Class1,gac.授課方式,gac.CType,gac.應取得-isnull(GCP.CoursepairCount,0) 應取得 ,isnull(gI.已取得,0)-isnull(GPC.Pair,0) 已取得 from getalltypeCourse gac
                Left Join getIntegral gI On gI.Class1=gac.Class1 and gI.CType=gac.CType
				 Left Join getPairCourseSNO GPC On GPC.Class1=gac.Class1 and GPC.CType=gac.CType
				 Left Join getCoursePair GCP On GCP.Class1=gac.Class1 and GCP.CType=gac.CType
                )
			
				,getResult as(
                Select P.PName,R.RoleName,P.PMail,P.PersonID,gaI.*, case when gaI.應取得-gaI.已取得 < 0 then 0　Else gaI.應取得-gaI.已取得  END 未取得 from getalreadyI gaI
				Left Join Person P On P.personSNO=gaI.PersonSNO
				Left Join Role R ON R.RoleSNO=P.RoleSNO
               )
				Select * from getResult
				where  Class1=1 and 未取得=0 and CType=2
				Order by getResult.課程類別,getResult.授課方式 DESC";
                adict.Add("PClassSNO", PClassSNO);
                DataTable ObjDT = ObjDH.queryData(sql, adict);
                return ObjDT;
            }
            if (EntityIntegral == "1")
            {
                sql = @"with getalltypeCourse as(
                Select QC.PClassSNO,C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour)應取得 
                from QS_Course QC
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                where QC.PClassSNO=@PClassSNO and QC.IsEnable=1 
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType,QC.PClassSNO
                )
                , getIntegral as (
                Select  QI.PersonSNO,C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour) 已取得
                from QS_Integral QI
                Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                Left Join Person P On P.PersonSNO=QI.personSNO
                where QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType,QI.PersonSNO
                
                ),getCoursePair as (
				Select Class1,Ctype,SUM(QC.Chour)CoursepairCount  from 
				QS_Course QC
				where PairCourseSNO <> 0 and PClassSNO=37
				Group by Class1,Ctype
				),AnalyticsPair as (
                    Select QC.Class1,QC.Ctype,QC.PairCourseSNO CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where  QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                    Group by QC.Class1,QC.Ctype,QC.PairCourseSNO
                    INTERSECT 
                    Select QC.Class1,QC.Ctype,QI.CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                    Group by QC.Class1,QC.Ctype,QI.CourseSNO)
                    ,getPairCourseSNO As(				  
					Select Class1,Ctype,SUM(Pair) pair from AnalyticsPair 
                    Group by Class1,Ctype
				  ),getalreadyI as(
                Select gI.PersonSNO,gac.PClassSNO,gac.課程類別,gac.Class1,gac.授課方式,gac.CType,gac.應取得-isnull(GCP.CoursepairCount,0) 應取得 ,isnull(gI.已取得,0)-isnull(GPC.Pair,0) 已取得 from getalltypeCourse gac
                Left Join getIntegral gI On gI.Class1=gac.Class1 and gI.CType=gac.CType
				 Left Join getPairCourseSNO GPC On GPC.Class1=gac.Class1 and GPC.CType=gac.CType
				 Left Join getCoursePair GCP On GCP.Class1=gac.Class1 and GCP.CType=gac.CType
                )
			
				,getResult as(
                Select P.PName,R.RoleName,P.PMail,P.PersonID,gaI.*, case when gaI.應取得-gaI.已取得 < 0 then 0　Else gaI.應取得-gaI.已取得  END 未取得 from getalreadyI gaI
				Left Join Person P On P.personSNO=gaI.PersonSNO
				Left Join Role R ON R.RoleSNO=P.RoleSNO
               )
				Select * from getResult
				where  Class1=1 and 未取得 > 0 and CType=2
				Order by getResult.課程類別,getResult.授課方式 DESC";
                adict.Add("PClassSNO", PClassSNO);
                DataTable ObjDT = ObjDH.queryData(sql, adict);
                return ObjDT;
            }

            return null;
        }
        #endregion
        
    }
    public Tuple<string,string>Class(string EventSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = "Select Class3,Class4 from Event where EventSNO=@EventSNO";
        adict.Add("EventSNO", ddl_EventTitle.SelectedValue);
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            string Class3 = ObjDT.Rows[0]["Class3"].ToString();
            string Class4 = ObjDT.Rows[0]["Class4"].ToString();
            return Tuple.Create(Class3, Class4);
        }
        else
        {
            return Tuple.Create("", "");
        }
    }
    public string[] Role(string EventSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql= @"Select QCPR.RoleSNO from Event E 
                        Left Join QS_CoursePlanningRole QCPR On QCPR.PClassSNO=E.PClassSNO
                        where E.EventSNO=@EventSNO";
        adict.Add("EventSNO", ddl_EventTitle.SelectedValue);
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            string Role = "";
            for(int i = 0; i < ObjDT.Rows.Count; i++)
            {
                Role = ObjDT.Rows[i]["RoleSNO"].ToString()+",";
            }
            Role = Role.Substring(0, Role.Length - 1);
            
            string[] RoleArray = Role.Split(',');
            return RoleArray;
        }
        else
        {
            return null;
        }
    }
    public void setRoleNormal(CheckBoxList chk, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT RoleSNO, RoleName FROM Role WHERE IsAdmin = 0", aDict);
        chk.DataSource = objDT;
        chk.DataBind();

        if (userInfo.RoleGroup == "10")
        {
            chk.Items[0].Selected = true;
            chk.Enabled = false;
        }
        if (userInfo.RoleGroup == "11")
        {
            chk.Items[1].Selected = true;
            chk.Items[2].Enabled = false;
            chk.Items[3].Enabled = false;
        }
        if (userInfo.RoleGroup == "12")
        {
            chk.Items[2].Selected = true;
            chk.Enabled = false;
        }
        if (userInfo.RoleGroup == "13")
        {
            chk.Items[3].Selected = true;
            chk.Enabled = false;
        }

    }

    protected void chk_Search_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_Search.Checked)
        {
            P1.Visible = true;
        }
        else
        {
            if(chk_NotGetIntegral.Checked == true)
            {
                chk_Certificate.Enabled = true;
                ddl_Certificate.Enabled = true;
                chk_Type.Enabled = true;
                txt_CertEndDate_S.Enabled = true;
                txt_CertEndDate_E.Enabled = true;
                ddl_CoursePlanningClass.Enabled = true;
                ddl_OnlineIntegral.Enabled = true;
                ddl_Entity.Enabled = true;
            }
            P1.Visible = false;
            ddl_OnlineIntegral.SelectedValue = "";
            chk_NotGetIntegral.Checked = false;
            ddl_CoursePlanningClass.SelectedValue = "";
            ddl_Entity.SelectedValue = "";
            chk_Certificate.Enabled = true;
            ddl_Certificate.Enabled = true;
            chk_Type.Enabled = true;
            txt_CertEndDate_S.Enabled = true;
            txt_CertEndDate_E.Enabled = true;
        }
    }

    protected void chk_Certificate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chk_Certificate.SelectedValue == "0")
        {
            txt_CertEndDate_S.Enabled = false;
            txt_CertEndDate_E.Enabled = false;
        }
        else
        {
            txt_CertEndDate_S.Enabled = true;
            txt_CertEndDate_E.Enabled = true;
        }
    }

    protected void chk_NotGetIntegral_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_NotGetIntegral.Checked)
        {
            ddl_CoursePlanningClass.Enabled = false;
            ddl_Entity.Enabled = false;
            ddl_OnlineIntegral.Enabled = false;
            chk_Certificate.Enabled = false;
            ddl_Certificate.Enabled = false;
            txt_CertEndDate_S.Enabled = false;
            txt_CertEndDate_E.Enabled = false;
            chk_Type.Enabled = false;
        }
        else
        {
            ddl_CoursePlanningClass.Enabled = true;
            chk_Type.Enabled = true;
            ddl_Entity.Enabled = true;
            ddl_OnlineIntegral.Enabled = true;
            chk_Certificate.Enabled = true;
            ddl_Certificate.Enabled = true;
            txt_CertEndDate_S.Enabled = true;
            txt_CertEndDate_E.Enabled = true;
        }
    }
    public static string CertificateRoleType(string CtypeSNO)
    {
        string CTypeSNO = "";
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        Dictionary<string, object> Dict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string sql = "Select CGroup from QS_CertificateType where CtypeSNO=@CtypeSNO";
        aDict.Add("CtypeSNO", CtypeSNO);
        DataTable ObjDT = objDH.queryData(sql, aDict);
        string CGroup = ObjDT.Rows[0]["CGroup"].ToString();
        string CtypeSQL = "Select CtypeSNO from QS_CertificateType where CGroup=@CGroup";
        Dict.Add("CGroup", CGroup);
        DataTable CtypeDT= objDH.queryData(CtypeSQL, Dict);
        if (CtypeDT.Rows.Count > 0)
        {
            for(int i = 0; i < CtypeDT.Rows.Count; i++)
            {
                CTypeSNO += CtypeDT.Rows[i]["CtypeSNO"].ToString() + ",";
            }
            CTypeSNO = CTypeSNO.Substring(0, CTypeSNO.Length - 1);
            return CTypeSNO;
        }
        else
        {
            return CTypeSNO;
        }
    }

    protected void ddl_OnlineIntegral_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_OnlineIntegral.SelectedValue=="" && ddl_Entity.SelectedValue == "")
        {
            chk_Type.Enabled = true;
            ddl_Entity.Enabled = true;
            ddl_OnlineIntegral.Enabled = true;
            chk_Certificate.Enabled = true;
            ddl_Certificate.Enabled = true;
            txt_CertEndDate_S.Enabled = false;
            txt_CertEndDate_E.Enabled = false;
        }
        else
        {
           
            
            chk_Certificate.Enabled = false;
            ddl_Certificate.Enabled = false;
            txt_CertEndDate_S.Enabled = true;
            txt_CertEndDate_E.Enabled = true;
            chk_Type.Enabled = false;            
        }
    }

    protected void ddl_Entity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_OnlineIntegral.SelectedValue == "" && ddl_Entity.SelectedValue == "")
        {
            chk_Type.Enabled = true;
            ddl_Entity.Enabled = true;
            ddl_OnlineIntegral.Enabled = true;
            chk_Certificate.Enabled = true;
            ddl_Certificate.Enabled = true;
            txt_CertEndDate_S.Enabled = true;
            txt_CertEndDate_E.Enabled = true;
        }
        else
        {
            
            chk_Certificate.Enabled = false;
            ddl_Certificate.Enabled = false;
            txt_CertEndDate_S.Enabled = false;
            txt_CertEndDate_E.Enabled = false;
            chk_Type.Enabled = false;
        }
    }

    public  void SetDdlEventTitle(DropDownList ddl, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string sql = @"Select EventName, EventSNO  FROM Event E
        LEFT JOIN Person P on P.PersonSNO = E.CreateUserID
        LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
        LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO where 1=1 And  [EndTime] > getdate() and E.Class3=1";
        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(aDict, userInfo);
        #endregion
        DataTable objDT = objDH.queryData(sql, aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
    public void SetDdlEventTitleEX(DropDownList ddl, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string sql = @"Select EventName, EventSNO  FROM Event E
        LEFT JOIN Person P on P.PersonSNO = E.CreateUserID
        LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
        LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO where 1=1 And  [EndTime] > getdate() and E.Class3=2";
        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(aDict, userInfo);
        #endregion
        DataTable objDT = objDH.queryData(sql, aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }


    protected void ddl_Certificate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_Certificate.SelectedValue == "8"|| ddl_Certificate.SelectedValue == "12" || ddl_Certificate.SelectedValue == "9")
        {
            chk_Type.Checked = false;
            chk_Type.Enabled = false;


        }
        else
        {
            chk_Type.Enabled = true;
        }
    }
    public string getChk_Role()
    {
        string RoleValue = "";
        for (int i = 0; i < Chk_Role.Items.Count; i++)
        {
            if (Chk_Role.Items[i].Selected == true)
            {
                RoleValue += Chk_Role.Items[i].Value + ",";
            }
        }
        RoleValue = RoleValue.Substring(0, RoleValue.Length - 1);
        return RoleValue;
    }

    protected void ddl_CoursePlanningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddl_CoursePlanningClass.SelectedValue=="1"|| ddl_CoursePlanningClass.SelectedValue == "2"
            ||ddl_CoursePlanningClass.SelectedValue == "6" || ddl_CoursePlanningClass.SelectedValue == "32" 
            || ddl_CoursePlanningClass.SelectedValue == "33" || ddl_CoursePlanningClass.SelectedValue == "34" 
            || ddl_CoursePlanningClass.SelectedValue == "35" || ddl_CoursePlanningClass.SelectedValue == "37"
            || ddl_CoursePlanningClass.SelectedValue == "41" || ddl_CoursePlanningClass.SelectedValue == "42")
        {
            ddl_Entity.SelectedValue = "";
            ddl_OnlineIntegral.Enabled = true;
            ddl_Entity.Enabled = false;
        }
       
        if (ddl_CoursePlanningClass.SelectedValue == "28" || ddl_CoursePlanningClass.SelectedValue == "29" || ddl_CoursePlanningClass.SelectedValue == "30" || ddl_CoursePlanningClass.SelectedValue == "31" )
        {
            ddl_Entity.Enabled = true;
            ddl_OnlineIntegral.Enabled = true;
        }
      
        if (ddl_CoursePlanningClass.SelectedValue == "3" || ddl_CoursePlanningClass.SelectedValue == "4"
            || ddl_CoursePlanningClass.SelectedValue == "21" || ddl_CoursePlanningClass.SelectedValue == "22"
            || ddl_CoursePlanningClass.SelectedValue == "26" || ddl_CoursePlanningClass.SelectedValue == "27"
            || ddl_CoursePlanningClass.SelectedValue == "36" )
        {
            ddl_OnlineIntegral.SelectedValue = "";
            ddl_Entity.SelectedValue = "";
            ddl_OnlineIntegral.Enabled = false;
            ddl_Entity.Enabled = false;
        }
        if (ddl_CoursePlanningClass.SelectedValue == "")
        {
            ddl_OnlineIntegral.Enabled = false;
            ddl_Entity.Enabled = false;
            ddl_OnlineIntegral.SelectedValue = "";
            ddl_Entity.SelectedValue = "";
        }
        
    }
    public static void setPlanNameNotEX(DropDownList dd1, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT [PClassSNO], [PlanName] FROM [QS_CoursePlanningClass] WHERE [IsEnable] = 1 and [PlanType] <> 1", aDict);
        dd1.DataSource = objDT;
        dd1.DataBind();
        if (DefaultString != null)
        {
            dd1.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
}