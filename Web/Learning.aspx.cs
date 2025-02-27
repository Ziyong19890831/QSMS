using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.events;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Web_Learning : System.Web.UI.Page
{
    UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
        else Response.Redirect("../Default.aspx");
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        //Utility.setPlanName(ddl_CoursePlanning, "請選擇");
        if (!IsPostBack)
        {
            //Utility.setPlanName(ddl_CoursePlanning, "請選擇");
            lb_PName.Text = userInfo.UserName;
            lb_PersonID.Text = userInfo.PersonID;
            bindData_LearningRecord();
            bindData_LearningScore();
            bindData_Certificate();
            bindData_CoursePlanningClass();
            //bindData_Certificate();
            bindData_Event();
            bindData_ECoursePlanningClass();
            bindData_FeedBack();
        }
    }

    protected void bindData_LearningRecord()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String sql = @"
           		 SELECT 
				lr.ELSCode, (cast(lr.ELSPart as varchar)+'/'+cast(es.ELSPart as varchar)) ELSPart, es.ELSName, e.ELName,
				convert(varchar(16), lr.FinishedDate, 120) FinishedDate
            From QS_LearningRecord lr
                LEFT JOIN QS_CourseELearningSection es on es.ELSCode=lr.ELSCode
                LEFT JOIN QS_CourseELearning e on e.ELCode=es.ELCode
            Where lr.PersonID=@PersonID
			Order by ELSName
        ";
        aDict.Add("PersonID", userInfo.PersonID);
        DataTable objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            LearningRecord.DataSource = objDT.DefaultView;
            LearningRecord.DataBind();
        }
        else
        {
            tbl_LearningRecord.Visible = false;
            lb_LearningRecord.Visible = true;
        }
    }
    protected void bindData_Detail(string PclassSNO)
    {


        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;

        string sql = @"            
                    with getalltypeCourse as(
                Select QC.PClassSNO,C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour)應取得 
                from QS_Course QC
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                where QC.PClassSNO=@PClassSNO and QC.IsEnable=1 
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType,QC.PClassSNO
                )
                , getIntegral as (
                Select  C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour) 已取得
                from QS_Integral QI
                Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                Left Join Person P On P.PersonSNO=QI.personSNO
                where QI.PersonSNO=@PersonSNO and QC.PClassSNO=@PClassSNO
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType
                
                ),getCoursePair as (
				Select Class1,Ctype,SUM(QC.Chour)CoursepairCount  from 
				QS_Course QC
				where PairCourseSNO <> 0 and PClassSNO=@PClassSNO
				Group by Class1,Ctype
				),AnalyticsPair as (
                   Select QC.Class1,QC.Ctype,QC.PairCourseSNO CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where PersonSNO=@PersonSNO and QC.PClassSNO=@PClassSNO
                    Group by QC.Class1,QC.Ctype,QC.PairCourseSNO
                    INTERSECT 
                    Select QC.Class1,QC.Ctype,QI.CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where PersonSNO=@PersonSNO and QC.PClassSNO=@PClassSNO
                    Group by QC.Class1,QC.Ctype,QI.CourseSNO)
               
                ,getPairCourseSNO As(				  
					Select Class1,Ctype,SUM(Pair) pair from AnalyticsPair 
                    Group by Class1,Ctype
				  ),getalreadyI as(
                Select gac.PClassSNO,gac.課程類別,gac.Class1,gac.授課方式,gac.CType,gac.應取得-isnull(GCP.CoursepairCount,0) 應取得 ,isnull(gI.已取得,0)-isnull(GPC.Pair,0) 已取得 from getalltypeCourse gac
                Left Join getIntegral gI On gI.Class1=gac.Class1 and gI.CType=gac.CType
				 Left Join getPairCourseSNO GPC On GPC.Class1=gac.Class1 and GPC.CType=gac.CType
				 Left Join getCoursePair GCP On GCP.Class1=gac.Class1 and GCP.CType=gac.CType
                )
				
                Select *, case when gaI.應取得-gaI.已取得 < 0 then 0　Else gaI.應取得-gaI.已取得  END 未取得 from getalreadyI gaI
                Order by gaI.課程類別,gaI.授課方式 DESC

            ";
        aDict.Add("@PersonSNO", userInfo.PersonSNO);
        aDict.Add("@PClassSNO", PclassSNO);
        objDT = objDH.queryData(sql, aDict);
        for (int i = 0; i < objDT.Rows.Count; i++)
        {
            if (objDT.Rows[i]["Class1"].ToString() == "2" && objDT.Rows[i]["CType"].ToString() == "3")
            {
                Button button = new Button();
                button.Text = "我要上傳";

            }
        }
        if (objDT.Rows.Count > 0)
        {
            rpt_integralS.DataSource = objDT.DefaultView;
            rpt_integralS.DataBind();
        }

    }
    protected void bindData_CoursePlanningClass()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;


        string sql = @"  
               
             with getsomething as(
                SELECT Distinct
                	I.PersonSNO
                	,QCPC.PlanName
                	,QCPC.CStartYear
                	,QCPC.CEndYear
                	,QCT.CTypeName
                	,QC.PClassSNO
                	,sum(CHour) PClassTotalHr
                  FROM [QS_Integral] I
                  Left Join Person P on P.PersonSNO=I.PersonSNO
                  Left Join QS_Course QC on QC.CourseSNO=I.CourseSNO
                  Left Join QS_CoursePlanningClass QCPC on QCPC.PClassSNO=QC.PClassSNO
                  Left Join QS_CertificateType QCT on QCT.CTypeSNO=QCPC.CTypeSNO
                    where 1=1 and I.isUsed <> 1 and I.PersonSNO=@PersonSNO and QC.Class1 <> 3
                  Group by QCPC.PlanName,QCT.CTypeName,QCPC.CStartYear,QCPC.CEndYear,QC.PClassSNO,I.PersonSNO
                  ),AnalyticsPair as (
                    Select QI.PersonSNO,QC.PClassSNO,QC.Ctype,QC.PairCourseSNO CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    Left Join getsomething gs On gs.PersonSNO=QI.PersonSNO
                    where QI.PersonSNO=@PersonSNO and QC.PClassSNO=gs.PClassSNO and QI.IsUsed=0
                    Group by QC.Ctype,QC.PairCourseSNO,QI.PersonSNO,QC.PClassSNO
                    INTERSECT 
                    Select QI.PersonSNO,QC.PClassSNO,QC.Ctype,QI.CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    Left Join getsomething gs On gs.PersonSNO=QI.PersonSNO
                    where QI.PersonSNO=@PersonSNO and QC.PClassSNO=gs.PClassSNO and QI.IsUsed=0
                    Group by QC.Ctype,QI.CourseSNO,QI.PersonSNO,QC.PClassSNO)
                    ,getPairCourseSNO As(				  
					Select PersonSNO,PClassSNO,Ctype,SUM(Pair) pair from AnalyticsPair 
                    Group by Ctype,PersonSNO,PClassSNO
				  )
                  , getAllCourseHours As (
                				Select  c.PClassSNO, cpc.[TargetIntegral] sumHours,CTypeSNO,NecessaryC
                				From QS_CoursePlanningClass cpc
                					Left JOIN QS_Course c on c.PClassSNO=cpc.PClassSNO
                				Group By c.PClassSNO, cpc.[TargetIntegral],CTypeSNO,NecessaryC
                			)
                
                ,sumAll as(
                  select getsomething.PersonSNO,PlanName,CStartYear,CEndYear,CTypeName,getsomething.PClassSNO,sum(PClassTotalHr)-isnull(GPC.Pair,0) PClassTotalHr ,sumHours,gc.CTypeSNO,gc.NecessaryC from getsomething
                  left join getAllCourseHours gc on gc.PClassSNO=getsomething.PClassSNO
				  left Join getPairCourseSNO GPC On GPC.PersonSNO= getsomething.PersonSNO And GPC.PClassSNO=getsomething.PClassSNO
                  where getsomething.PersonSNO=@PersonSNO 
				  Group by getsomething.PersonSNO,PlanName,CStartYear,CEndYear,CTypeName,getsomething.PClassSNO,sumHours,GPC.Pair,gc.CTypeSNO,NecessaryC
				  )
				  Select * from sumAll where PClassSNO is not null
            
            ";
        aDict.Add("@PersonSNO", userInfo.PersonSNO);
        objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            for (int i = 0; i < objDT.Rows.Count; i++)
            {
                string PClassSNO = objDT.Rows[i]["PClassSNO"].ToString();
                string CtypeSNO = objDT.Rows[i]["CtypeSNO"].ToString();
                if(CtypeSNO=="10" || CtypeSNO == "11")
                {
                    string NecessaryC = objDT.Rows[i]["NecessaryC"].ToString();
                    string PersonID = Utility.ConvertPersonSNOToPersonID(objDT.Rows[i]["PersonSNO"].ToString());
                    bool CheckPersonGetCertificate = Utility.CheckPersonJuniorSenior(NecessaryC, PersonID);
                    int PClassTotalHr = Convert.ToInt16(objDT.Rows[i]["PClassTotalHr"]);
                    int sumHours = Convert.ToInt16(objDT.Rows[i]["sumHours"]);
                    if ((sumHours - PClassTotalHr <= 0 && CheckPersonGetCertificate) || (sumHours - PClassTotalHr <= 0 && PClassSNO == "29") || (sumHours - PClassTotalHr <= 0 && PClassSNO == "28"))
                    {
                        hf_Core.Value = "2";
                    }
                    else if (sumHours - PClassTotalHr <= 0 && !CheckPersonGetCertificate)
                    {
                        hf_Core.Value = "1";
                    }
                    else
                    {
                        hf_Core.Value = "0";
                    }
                }                

            }
            rpt_CoursePlanningClass.DataSource = objDT.DefaultView;
            rpt_CoursePlanningClass.DataBind();
        }
        else
        {
            tbl_CoursePlanningClass.Visible = false;
            lb_CoursePlanningClass.Visible = true;
        }
    }
    protected void bindData_LearningScore()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = @"
            select 
                ce.ELName, Score, QuizName, PassScore,
                case IsPass when 0 then '不通過' when 1 then '通過' End Pass,
				convert(varchar(16), ExamDate, 120) ExamDate
            from QS_LearningScore ls
                Left Join QS_CourseELearning ce ON ce.ELCode=ls.ELCode
            where PersonID=@PersonID
        ";
        aDict.Add("PersonID", userInfo.PersonID);
        DataTable objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            LearningScore.DataSource = objDT.DefaultView;
            LearningScore.DataBind();
            
        }
        else
        {
            tbl_LearningScore.Visible = false;
            lb_LearningScore.Visible = true;
        }

    }
    protected void bindData_Certificate()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;

        string sql = @"
             SELECT 
               replace(CT.CTypeString,'@',RIGHT(REPLICATE('0', 6) + CAST(C.CertID as NVARCHAR), 6) ) NCTypeString,
                replace(CT.CTypeString,'@',C.CertID) OCTypeString,
                C.CertSNO,
                CT.CTypeName,
                CU.CUnitName,
                C.SysChange,
                Cast(C.CertPublicDate As varchar(10)) CertPublicDate,
                Cast(C.CertStartDate As varchar(10)) CertStartDate,
                Cast(C.CertEndDate As varchar(10)) CertEndDate,
                (Case C.CertExt When 1 Then '有' Else '無' End) CertExt,
                C.Note       
            FROM QS_Certificate C
                Left JOIN QS_CertificateType CT on CT.CTypeSNO=C.CTypeSNO
                Left JOIN QS_CertificateUnit CU on CU.CUnitSNO=C.CUnitSNO
            WHERE
                C.PersonID=@PersonID
        ";
        aDict.Add("PersonID", userInfo.PersonID);
        if (userInfo.RoleOrganType != "S")
        {
            sql += " And C.IsChange <> 1 ";
        }
        objDT = objDH.queryData(sql, aDict);

        if (objDT.Rows.Count > 0)
        {
            gv_Certificate.DataSource = objDT;
            gv_Certificate.DataBind();
        }
        else
        {
            gv_Certificate.Visible = false;
            lb_Certificate.Visible = true;
        }

    }


    protected void bindData_integral(string PclassSNO)
    {
        //實習 我要上傳按鈕

        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;

        string sql = @"            
               with getX as(
                    SELECT CPC.PClassSNO,CPC.PlanName,CPC.CStartYear,CPC.CEndYear,A.CourseSNO,A.CourseName,A.CHour
					,B.MVal + '課程' Class1 , C.MVal Class2 , A.UnitName , D.MVal Ctype
                      FROM [QS_CoursePlanningClass] CPC
                      Left Join QS_Course A ON A.PClassSNO=CPC.PClassSNO
					LEFT  JOIN Config B ON B.PGroup ='CourseClass1' AND A.Class1 = B.PVal
					LEFT  JOIN Config C ON C.PGroup ='CourseClass2' AND A.Class2 = C.PVal
                    LEFT  JOIN Config D ON D.PGroup ='CourseCType' AND A.CType = D.PVal
                      
					  
                      ),
                      getY as(
                      select 1 O,I.CourseSNO from QS_Integral I
                      where I.PersonSNO=@PersonSNO
                      )
                      select Class1,R.RoleName,CourseName,Ctype,CHour,CStartYear,CEndYear,
					  case O when  1 then'已取得' ELSE '未取得' END  積分
					  from getX X
                      FULL outer Join getY Y ON Y.CourseSNO=X.CourseSNO
					  Left JOIN QS_CoursePlanningRole CPR ON CPR.RClassSNO=X.PClassSNO
					  Left Join Role R ON R.RoleSNO=CPR.RoleSNO
                       where X.PClassSNO=@PClassSNO
            
            
            ";
        aDict.Add("@PersonSNO", userInfo.PersonSNO);
        aDict.Add("@PClassSNO", PclassSNO);
        objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            rpt_integralS.DataSource = objDT.DefaultView;
            rpt_integralS.DataBind();
        }

    }
    protected void bindData_Event()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String sql = @"
            SELECT e.EventSNO,
	            e.EventName, 
				convert(varchar(16), ed.CreateDT, 120) CreateDT,
                C.MVal Audit
            From EventD ed
                LEFT JOIN Event e on e.EventSNO=ed.EventSNO
				Left Join Config C On c.PVal=ed.Audit and C.PGroup='EventAudit'
            Where ed.PersonSNO=@PersonSNO 
        ";
        aDict.Add("PersonSNO", userInfo.PersonSNO);
        DataTable objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            Event.DataSource = objDT.DefaultView;
            Event.DataBind();
        }
        else
        {
            tbl_Event.Visible = false;
            lb_Event.Visible = true;
        }
    }
    protected void bindData_integral(string PclassSNO, string Class1, string Ctype)
    {


        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;

        string sql = @"            
                           
              with getX as(
                    SELECT CPC.PClassSNO,CPC.PlanName,CPC.CStartYear,CPC.CEndYear,A.CourseSNO,A.CourseName,A.CHour
					,B.MVal + '課程' Class1 , C.MVal Class2 , A.UnitName , D.MVal Ctype,A.Class1 Class ,A.CType Type
                      FROM [QS_CoursePlanningClass] CPC
                      Left Join QS_Course A ON A.PClassSNO=CPC.PClassSNO
					LEFT  JOIN Config B ON B.PGroup ='CourseClass1' AND A.Class1 = B.PVal
					LEFT  JOIN Config C ON C.PGroup ='CourseClass2' AND A.Class2 = C.PVal
                    LEFT  JOIN Config D ON D.PGroup ='CourseCType' AND A.CType = D.PVal
                      
					  
                      ),
                      getY as(
                      select 1 O,I.CourseSNO from QS_Integral I
                      where I.PersonSNO=@PersonSNO
                      )
                      select Class1,CourseName,Ctype,CHour,CStartYear,CEndYear,X.Class,X.Type,
					  case O when  1 then'已取得' ELSE '未取得' END  積分
					  from getX X
                      FULL outer Join getY Y ON Y.CourseSNO=X.CourseSNO
					  Left JOIN QS_CoursePlanningRole CPR ON CPR.RClassSNO=X.PClassSNO
					  Left Join Role R ON R.RoleSNO=CPR.RoleSNO
                       where X.PClassSNO=@PClassSNO and X.Class=@Class and X.Type=@Type and O is null
                    
            
            
                    
            
            ";
        aDict.Add("@PersonSNO", userInfo.PersonSNO);
        aDict.Add("@PClassSNO", PclassSNO);
        aDict.Add("@Class", Class1);
        aDict.Add("@Type", Ctype);
        objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            Repeater1.DataSource = objDT.DefaultView;
            Repeater1.DataBind();
        }

    }
    protected void bindData_ECoursePlanningClass()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;


        string sql = @"  
              
            with getsomething as(
                SELECT 
                	I.PersonSNO
                	,QCPC.PlanName
                	,QCPC.CStartYear
					,QC.Class1
                	,QCPC.CEndYear
                	,QCT.CTypeName
                	,QC.PClassSNO
					,QCPC.EPClassSNO
                    ,QCPC.[ElearnLimit]
                	,Case when sum(CHour) >= QCPC.ElearnLimit then QCPC.ElearnLimit Else sum(CHour) End PClassTotalHr
                  FROM [QS_Integral] I
                  Left Join Person P on P.PersonSNO=I.PersonSNO
                  Left Join QS_Course QC on QC.CourseSNO=I.CourseSNO
                  Left Join QS_ECoursePlanningClass QCPC on QCPC.PClassSNO=QC.PClassSNO
                  Left Join QS_CertificateType QCT on QCT.CTypeSNO=QCPC.CTypeSNO 
                    where 1=1 and P.PersonSNO=@PersonSNO  and QC.Class1 = 3 and CTypeName <> '' and I.IsUsed <>1
                  Group by QCPC.PlanName,QCT.CTypeName,QCPC.CStartYear,QCPC.CEndYear,QC.PClassSNO,I.PersonSNO,QC.Class1,QCPC.EPClassSNO,QCPC.ElearnLimit
                  )
                  , getAllCourseHours As (
                				Select distinct  P.personID,QEPC.PlanName,QEPC.PClassSNO
								 ,QEPC.EPClassSNO,QEPC.TotalIntegral sumHours
								 ,QEPC.CStartYear,QEPC.CEndYear
								From QS_ECoursePlanningClass QEPC 
								Left Join QS_ECoursePlanningRole QEPR On QEPR.EPClassSNO=QEPC.EPClassSNO
								Left Join QS_CertificateType QCT On QCT.RoleSNO=QEPR.RoleSNO
								Left Join Person P On P.RoleSNO=QEPR.RoleSNO
								where P.PersonSNO=@PersonSNO  and QEPC.PClassSNO <>''
                				
                			)
				  , getEintegral as (
				  
					Select  P.personID,QCPC.PlanName,QCPC.PClassSNO ,QCPC.EPClassSNO,sum(QE.Integral) TotalHr
                				From QS_ECoursePlanningClass QCPC
                				Left Join QS_EIntegral QE On QE.EPClassSNO=QCPC.EPClassSNO
								Left Join Person P on P.PersonID=QE.PersonID
								where P.PersonSNO=@PersonSNO and QE.IsUsed <> 1
                				Group by QCPC.TotalIntegral ,QCPC.EPClassSNO,P.PersonID,QCPC.PlanName ,QCPC.PClassSNO
				  )
                
                  select getAllCourseHours.*,isnull(getEintegral.TotalHr,0)+isnull(getsomething.PClassTotalHr,0) PClassTotalHr
                    ,getsomething.CTypeName,getsomething.ElearnLimit
                    from getAllCourseHours
                  left join getsomething on getsomething.EPClassSNO=getAllCourseHours.EPClassSNO
				  left join getEintegral on getEintegral.EPClassSNO=getAllCourseHours.EPClassSNO
                 
                 
     
            ";
        aDict.Add("@PersonSNO", userInfo.PersonSNO);
        objDT = objDH.queryData(sql, aDict);
        DataTable ConvertToDatarow = null;
        DataRow[] row = objDT.Select("PClassTotalHr <> 0");
        if (row.Count() > 0)
        {
            ConvertToDatarow = row.CopyToDataTable();
        }
        if (objDT.Rows.Count > 0 && row.Count() > 0)
        {
            lb_ElearnLimit.Text = ConvertToDatarow.Rows[0]["ElearnLimit"].ToString();
            rpt_ECoursePlanningClass.DataSource = ConvertToDatarow;
            rpt_ECoursePlanningClass.DataBind();
        }
        else
        {
            tbl_ECoursePlanningClass.Visible = false;
            lb_ECoursePlanningClass.Visible = true;
        }
    }
    protected void bindData_Eintegral(string PclassSNO)
    {


        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;

        string sql = @"            
                           
            with getalltypeCourse as(
                Select QC.PClassSNO,C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour)應取得 
                from QS_Course QC
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                where QC.PClassSNO=@PClassSNO 
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType,QC.PClassSNO
                )
                , getIntegral as (
                Select  C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour) 已取得
                from QS_Integral QI
                Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                Left Join Person P On P.PersonSNO=QI.personSNO
                where QI.PersonSNO=@PersonSNO and Class1=3 and QC.PClassSNO=@PClassSNO
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType
                
                ),getalreadyI as(
                Select gac.PClassSNO,gac.課程類別,gac.Class1,gac.授課方式,gac.CType,gac.應取得,isnull(gI.已取得,0)  已取得 
				from getIntegral gI
                Left Join getalltypeCourse gac On gI.Class1=gac.Class1 and gI.CType=gac.CType
                )
                Select *,gaI.應取得-gaI.已取得 未取得 from getalreadyI gaI
                Order by gaI.課程類別,gaI.授課方式 DESC



                    
            
            
                    
            
            ";
        aDict.Add("@PersonSNO", userInfo.PersonSNO);
        aDict.Add("@PClassSNO", PclassSNO);

        objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            rpt_EintegralS.DataSource = objDT.DefaultView;
            rpt_EintegralS.DataBind();
        }

    }
    protected void bindData_UploadEintegral(string EPclassSNO)
    {

        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;

        string sql = @"                 
                           
                             With getAllEcourse as (
                 Select EPClassSNO,PlanName,TotalIntegral,Compulsory_Communication
                 ,Compulsory_Entity,Compulsory_Online,Compulsory_Practical
                  from QS_ECoursePlanningClass
                
                 ),
                 getEintergalO as (
                 Select PersonID,EPClassSNO,sum(Integral)Integral from QS_EIntegral
                 where Ctype=1 and PersonID=@PersonID
                 Group by PersonID,EPClassSNO
                 ),
                   getEintergalE as (
                  Select PersonID,EPClassSNO,sum(Integral)Integral from QS_EIntegral
                 where Ctype=2 and PersonID=@PersonID
                 Group by PersonID,EPClassSNO
                 )
                 ,
                   getEintergalP as (
                  Select PersonID,EPClassSNO,sum(Integral)Integral from QS_EIntegral
                 where Ctype=3 and PersonID=@PersonID
                 Group by PersonID,EPClassSNO
                 )
                   ,
                   getEintergalC as (
                  Select PersonID,EPClassSNO,sum(Integral)Integral from QS_EIntegral
                 where Ctype=4 and PersonID=@PersonID
                 Group by PersonID,EPClassSNO
                 )
                 Select PlanName,gAE.EPClassSNO,
                 isnull(gEC.Integral,0) 通訊 ,
                 isnull(gEE.Integral,0) 實體 ,
                 isnull(gEO.Integral,0) 線上 ,
                 isnull(gEP.Integral,0) 實習
                 from getAllEcourse gAE
                 Left Join getEintergalO gEO On gEO.EPClassSNO=gAE.EPClassSNO
                 Left Join getEintergalE gEE On gEE.EPClassSNO=gAE.EPClassSNO
                 Left Join getEintergalP gEP On gEP.EPClassSNO=gAE.EPClassSNO
                 Left Join getEintergalC gEC On gEC.EPClassSNO=gAE.EPClassSNO
                 where gAE.EPClassSNO=@EPClassSNO
            
            
            ";
        aDict.Add("@PersonID", userInfo.PersonID);
        aDict.Add("@EPClassSNO", EPclassSNO);

        objDT = objDH.queryData(sql, aDict);

        if (objDT.Rows.Count > 0)
        {
            rpt_EUploadintegralS.DataSource = objDT.DefaultView;
            rpt_EUploadintegralS.DataBind();
        }

    }
    protected void EbindData_integral(string PclassSNO, string Class1, string Ctype)
    {


        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;

        string sql = @"            
                           
              with getX as(
                    SELECT CPC.PClassSNO,CPC.PlanName,CPC.CStartYear,CPC.CEndYear,A.CourseSNO,A.CourseName,A.CHour
					,B.MVal + '課程' Class1 , C.MVal Class2 , A.UnitName , D.MVal Ctype,A.Class1 Class ,A.CType Type
                      FROM [QS_CoursePlanningClass] CPC
                      Left Join QS_Course A ON A.PClassSNO=CPC.PClassSNO
					LEFT  JOIN Config B ON B.PGroup ='CourseClass1' AND A.Class1 = B.PVal
					LEFT  JOIN Config C ON C.PGroup ='CourseClass2' AND A.Class2 = C.PVal
                    LEFT  JOIN Config D ON D.PGroup ='CourseCType' AND A.CType = D.PVal
                      
					  
                      ),
                      getY as(
                      select 1 O,I.CourseSNO from QS_Integral I
                      where I.PersonSNO=@PersonSNO
                      )
                      select Class1,CourseName,Ctype,CHour,CStartYear,CEndYear,X.Class,X.Type,
					  case O when  1 then'已取得' ELSE '未取得' END  積分
					  from getX X
                      FULL outer Join getY Y ON Y.CourseSNO=X.CourseSNO
					  Left JOIN QS_CoursePlanningRole CPR ON CPR.RClassSNO=X.PClassSNO
					  Left Join Role R ON R.RoleSNO=CPR.RoleSNO
                       where X.PClassSNO=@PClassSNO and X.Class=@Class and X.Type=@Type and O is null
                    
            
            
                    
            
            ";
        aDict.Add("@PersonSNO", userInfo.PersonSNO);
        aDict.Add("@PClassSNO", PclassSNO);
        aDict.Add("@Class", Class1);
        aDict.Add("@Type", Ctype);
        objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            Repeater4.DataSource = objDT.DefaultView;
            Repeater4.DataBind();
        }

    }
    protected void EbindData_Upliadintegral(string EPclassSNO, string Ctype)
    {


        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;

        string sql = @"            
                           
             Select * from QS_EIntegral where personID=@PersonID and Ctype=@Ctype and EPClassSNO=@EPClassSNO
                    
            
            ";
        aDict.Add("@PersonID", userInfo.PersonID);
        aDict.Add("@Ctype", Ctype);
        aDict.Add("@EPclassSNO", EPclassSNO);
        objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            rpt_EUploadintegralDetail.DataSource = objDT.DefaultView;
            rpt_EUploadintegralDetail.DataBind();
        }

    }
    protected void EbindData_Doneintegral(string PclassSNO, string Class1, string Ctype)
    {

        //PersonSNO = Convert.ToString(Request.QueryString["sno"]);
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;

        string sql = @"            
                           
              with getX as(
                    SELECT CPC.PClassSNO,CPC.PlanName,CPC.CStartYear,CPC.CEndYear,A.CourseSNO,A.CourseName,A.CHour
					,B.MVal + '課程' Class1 , C.MVal Class2 , A.UnitName , D.MVal Ctype,A.Class1 Class ,A.CType Type
                      FROM [QS_CoursePlanningClass] CPC
                      Left Join QS_Course A ON A.PClassSNO=CPC.PClassSNO
					LEFT  JOIN Config B ON B.PGroup ='CourseClass1' AND A.Class1 = B.PVal
					LEFT  JOIN Config C ON C.PGroup ='CourseClass2' AND A.Class2 = C.PVal
                    LEFT  JOIN Config D ON D.PGroup ='CourseCType' AND A.CType = D.PVal
                      
					  
                      ),
                      getY as(
                      select 1 O,I.CourseSNO from QS_Integral I
                      where I.PersonSNO=@PersonSNO
                      )
                      select Class1,CourseName,Ctype,CHour,CStartYear,CEndYear,X.Class,X.Type,
					  case O when  1 then'已取得' ELSE '未取得' END  積分
					  from getX X
                      FULL outer Join getY Y ON Y.CourseSNO=X.CourseSNO
					  Left JOIN QS_CoursePlanningRole CPR ON CPR.RClassSNO=X.PClassSNO
					  Left Join Role R ON R.RoleSNO=CPR.RoleSNO
                       where X.PClassSNO=@PClassSNO and X.Class=@Class and X.Type=@Type and O=1
                            
            ";
        aDict.Add("@PersonSNO", userInfo.PersonSNO);
        aDict.Add("@PClassSNO", PclassSNO);
        aDict.Add("@Class", Class1);
        aDict.Add("@Type", Ctype);
        objDT = objDH.queryData(sql, aDict);


        Repeater4.DataSource = objDT.DefaultView;
        Repeater4.DataBind();



    }
    protected void bindData_Doneintegral(string PclassSNO, string Class1, string Ctype)
    {


        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;

        string sql = @"            
                           
              with getX as(
                    SELECT CPC.PClassSNO,CPC.PlanName,CPC.CStartYear,CPC.CEndYear,A.CourseSNO,A.CourseName,A.CHour
					,B.MVal + '課程' Class1 , C.MVal Class2 , A.UnitName , D.MVal Ctype,A.Class1 Class ,A.CType Type
                      FROM [QS_CoursePlanningClass] CPC
                      Left Join QS_Course A ON A.PClassSNO=CPC.PClassSNO
					LEFT  JOIN Config B ON B.PGroup ='CourseClass1' AND A.Class1 = B.PVal
					LEFT  JOIN Config C ON C.PGroup ='CourseClass2' AND A.Class2 = C.PVal
                    LEFT  JOIN Config D ON D.PGroup ='CourseCType' AND A.CType = D.PVal
                      
					  
                      ),
                      getY as(
                      select 1 O,I.CourseSNO from QS_Integral I
                      where I.PersonSNO=@PersonSNO
                      )
                      select Class1,CourseName,Ctype,CHour,CStartYear,CEndYear,X.Class,X.Type,
					  case O when  1 then'已取得' ELSE '未取得' END  積分
					  from getX X
                      FULL outer Join getY Y ON Y.CourseSNO=X.CourseSNO
					  Left JOIN QS_CoursePlanningRole CPR ON CPR.RClassSNO=X.PClassSNO
					  Left Join Role R ON R.RoleSNO=CPR.RoleSNO
                       where X.PClassSNO=@PClassSNO and X.Class=@Class and X.Type=@Type and O=1
                            
            ";
        aDict.Add("@PersonSNO", userInfo.PersonSNO);
        aDict.Add("@PClassSNO", PclassSNO);
        aDict.Add("@Class", Class1);
        aDict.Add("@Type", Ctype);
        objDT = objDH.queryData(sql, aDict);


        Repeater1.DataSource = objDT.DefaultView;
        Repeater1.DataBind();



    }

    protected void Btn_Print_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;

        string CertSNO = (sender as LinkButton).CommandArgument;
        string sql = @"
            SELECT C.CertSNO,P.RoleSNO,
                P.PName,
                case when PSex=0 then '女' ELSE '男' End  Psex,
                C.PersonID,
                C.CtypeSNO,
                C.CertID,
                CT.CTypeName,
                CU.CUnitName,
                Cast(C.CertPublicDate As varchar(10)) CertPublicDate,
                Cast(C.CertStartDate As varchar(10)) CertStartDate,
                Cast(C.CertEndDate As varchar(10)) CertEndDate,
                (Case C.CertExt When 1 Then '有' Else '無' End) CertExt,
                QLU.ExamDate
            FROM QS_Certificate C
                Left JOIN QS_CertificateType CT on CT.CTypeSNO=C.CTypeSNO
                Left JOIN QS_CertificateUnit CU on CU.CUnitSNO=C.CUnitSNO
                Left Join Person P ON P.PersonID=C.PersonID
                Left Join [QS_LearningUpload] QLU On QLU.PersonSNO=P.PersonSNO
            WHERE
                 P.PersonSNO=@PersonSNO And C.CertSNO=@CertSNO
        ";
        aDict.Add("PersonSNO", userInfo.PersonSNO);
        aDict.Add("CertSNO", CertSNO);
        objDT = objDH.queryData(sql, aDict);
        string CtypeSNO = objDT.Rows[0]["CtypeSNO"].ToString();
        string RoleSNO = objDT.Rows[0]["RoleSNO"].ToString();
        if (CtypeSNO == "51" || CtypeSNO == "52" || CtypeSNO == "53" || CtypeSNO == "54" || CtypeSNO == "55" || CtypeSNO == "8")
        {

            string PName = objDT.Rows[0]["PName"].ToString();
            string PersonID = objDT.Rows[0]["PersonID"].ToString();
            string CertID = objDT.Rows[0]["CertID"].ToString();
            DateTime CertPublicDate = Convert.ToDateTime(objDT.Rows[0]["CertPublicDate"]);
            DateTime CertStartDate = Convert.ToDateTime(objDT.Rows[0]["CertStartDate"]).AddYears(-1911);
            DateTime CertEndDate = Convert.ToDateTime(objDT.Rows[0]["CertEndDate"]).AddYears(-1911);
            string StartDateY = CertStartDate.Year.ToString(); string StartDateM = CertStartDate.Month.ToString(); ; string StartDateD = CertStartDate.Day.ToString();
            string EndDateY = CertEndDate.Year.ToString(); string EndDateM = CertEndDate.Month.ToString(); ; string EndDateD = CertEndDate.Day.ToString();
            string PSex = objDT.Rows[0]["PSex"].ToString();
            string DatetimeY = DateTime.Now.AddYears(-1911).Year.ToString(); string DatetimeM = DateTime.Now.Month.ToString(); string DatetimeD = DateTime.Now.Day.ToString();

            if (CertID.Length == 6)
            {
                try
                {
                    string imageFilePath = "";
                    string imageFilePath_1 = "";
                    string imageFilePath_2 = "";
                    MemoryStream ms = new MemoryStream();
                    Document document = new Document(PageSize.A4, 50, 50, 50, 50);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();
                    document.NewPage();
                    BaseFont bfChinese = BaseFont.CreateFont(@"C:\Windows\Fonts\msjh.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    BaseFont Chinese = BaseFont.CreateFont(@"C:\Windows\Fonts\kaiu.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    BaseFont times = BaseFont.CreateFont(@"C:\Windows\Fonts\times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    Font ChFont = new Font(times, 18);
                    Font ChFont_Pname = new Font(bfChinese, 18);
                    Font ChFont_PSex = new Font(Chinese, 18);
                    Font ChFont_Role11Pname = new Font(bfChinese, 22, 1);
                    Font ChFont_1 = new Font(bfChinese, 22);
                    Font ChFont_2 = new Font(bfChinese, 18);
                    Font ChFont_CertID = new Font(times, 14);
                    Font ChFont_blue = new Font(bfChinese, 22, Font.NORMAL, new BaseColor(51, 0, 153));
                    Font ChFont_msg = new Font(bfChinese, 12, Font.NORMAL, BaseColor.RED);
                    switch (RoleSNO)
                    {
                        case "10":
                            if (CtypeSNO == "53")
                            {
                                imageFilePath = Server.MapPath("../Images/Role10Certificate.png");
                            }
                            if (CtypeSNO == "51")
                            {
                                imageFilePath = Server.MapPath("../Images/Role13Certificate.jpg");
                            }
                            if (CtypeSNO == "55")
                            {
                                imageFilePath = Server.MapPath("../Images/Role11Certificate_2.jpg");
                            }
                            if (CtypeSNO == "8")
                            {
                                imageFilePath = Server.MapPath("../Images/Role10CertificateT.jpg");
                            }
                            break;
                        case "11":
                            if (CtypeSNO == "54")
                            {
                                imageFilePath = Server.MapPath("../Images/Role11Certificate_1.jpg");
                            }
                            if (CtypeSNO == "55")
                            {
                                imageFilePath = Server.MapPath("../Images/Role11Certificate_2.jpg");
                            }
                            break;
                        case "12":
                            imageFilePath = Server.MapPath("../Images/Role12Certificate.jpg");
                            break;
                        case "13":
                            imageFilePath = Server.MapPath("../Images/Role13Certificate.jpg");
                            break;
                    }

                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);


                    //jpg.ScaleToFit(100%);
                    jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
                    jpg.SetAbsolutePosition(0, 0);
                    jpg.ScaleAbsolute(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height);
                    document.Add(jpg);
                    switch (RoleSNO)
                    {
                        case "10":
                            if (CtypeSNO == "53" || CtypeSNO == "8")
                            {
                                imageFilePath_1 = Server.MapPath("../Images/002.jpg");
                                imageFilePath_2 = Server.MapPath("../Images/555.jpg");
                                break;
                            }
                            if (CtypeSNO == "51")
                            {
                                imageFilePath_1 = Server.MapPath("../Images/001.png");
                                imageFilePath_2 = Server.MapPath("../Images/老師印章.png");
                                break;
                            }
                            if (CtypeSNO == "55")
                            {
                                imageFilePath = Server.MapPath("../Images/Role11Certificate_2.jpg");
                                imageFilePath_1 = Server.MapPath("../Images/001.png");
                                imageFilePath_2 = Server.MapPath("../Images/老師印章11.png");
                                break;
                            }
                            break;
                        case "11":
                            imageFilePath_1 = Server.MapPath("../Images/001.png");
                            imageFilePath_2 = Server.MapPath("../Images/老師印章11.png");
                            break;
                        case "12":
                            imageFilePath_1 = Server.MapPath("../Images/005.png");
                            imageFilePath_2 = Server.MapPath("../Images/004.png");

                            break;
                        case "13":
                            imageFilePath_1 = Server.MapPath("../Images/001.png");
                            imageFilePath_2 = Server.MapPath("../Images/老師印章.png");
                            break;
                    }

                    iTextSharp.text.Image JPG1 = iTextSharp.text.Image.GetInstance(imageFilePath_1);
                    JPG1.ScalePercent(60f);
                    JPG1.SetAbsolutePosition(80, 120);
                    document.Add(JPG1);
                    if (imageFilePath_2 != "")
                    {
                        iTextSharp.text.Image JPG2 = iTextSharp.text.Image.GetInstance(imageFilePath_2);
                        JPG2.ScalePercent(10f);
                        if (RoleSNO == "10")
                        {
                            if (CtypeSNO == "53" || CtypeSNO == "8")
                            {
                                JPG2.SetAbsolutePosition(270, 140);
                                document.Add(JPG2);
                            }
                            else
                            {
                                JPG2.SetAbsolutePosition(265, 150);
                                document.Add(JPG2);
                            }
                        }
                        else if (RoleSNO == "11")
                        {
                            JPG2.SetAbsolutePosition(295, 225);
                            document.Add(JPG2);
                        }
                        else if (RoleSNO == "12")
                        {
                            JPG2.SetAbsolutePosition(265, 220);
                            document.Add(JPG2);
                        }
                        else
                        {
                            JPG2.SetAbsolutePosition(265, 150);
                            document.Add(JPG2);
                        }

                    }


                    switch (RoleSNO)
                    {
                        case "10":
                            Chunk c_10_PName = new Chunk(PName, ChFont_Pname);
                            Phrase p_10_PName = new Phrase(c_10_PName);
                            Paragraph pg_10_PName = new Paragraph(p_10_PName);
                            pg_10_PName.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct_10_PName = new ColumnText(writer.DirectContent);
                            ct_10_PName.SetSimpleColumn(new Rectangle(300, 574));
                            ct_10_PName.AddElement(pg_10_PName);
                            ct_10_PName.Go();

                            Chunk c1_10_PersonID = new Chunk(PersonID, ChFont_Pname);
                            Phrase p1_10_PersonID = new Phrase(c1_10_PersonID);
                            Paragraph pg1_10_PersonID = new Paragraph(p1_10_PersonID);
                            pg1_10_PersonID.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct1_10_PersonID = new ColumnText(writer.DirectContent);
                            ct1_10_PersonID.SetSimpleColumn(new Rectangle(880, 574));
                            ct1_10_PersonID.AddElement(pg1_10_PersonID);
                            ct1_10_PersonID.Go();

                            Chunk C_10_CertID = new Chunk(CertID, ChFont_CertID);
                            Phrase p_10_CertID = new Phrase(C_10_CertID);
                            Paragraph pg_10_CertID = new Paragraph(p_10_CertID);
                            pg_10_CertID.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct2_10_CertID = new ColumnText(writer.DirectContent);
                            ct2_10_CertID.SetSimpleColumn(new Rectangle(940, 615));
                            ct2_10_CertID.AddElement(pg_10_CertID);
                            ct2_10_CertID.Go();

                            Chunk c2_10_PersonID = new Chunk(PSex, ChFont_PSex);
                            Phrase p2_10_PersonID = new Phrase(c2_10_PersonID);
                            Paragraph pg2_10_PersonID = new Paragraph(p2_10_PersonID);
                            pg2_10_PersonID.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct3_10_PersonID = new ColumnText(writer.DirectContent);
                            ct3_10_PersonID.SetSimpleColumn(new Rectangle(650, 574));
                            ct3_10_PersonID.AddElement(pg2_10_PersonID);
                            ct3_10_PersonID.Go();
                            break;
                        case "11":
                            Chunk c_11_PName = new Chunk(PName, ChFont_Role11Pname);
                            Phrase p_11_PName = new Phrase(c_11_PName);
                            Paragraph pg_11_PName = new Paragraph(p_11_PName);
                            pg_11_PName.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct_11_PName = new ColumnText(writer.DirectContent);
                            ct_11_PName.SetSimpleColumn(new Rectangle(360, 577));
                            ct_11_PName.AddElement(pg_11_PName);
                            ct_11_PName.Go();

                            Chunk c1_11_PersonID = new Chunk(PersonID, ChFont_Pname);
                            Phrase p1_11_PersonID = new Phrase(c1_11_PersonID);
                            Paragraph pg1_11_PersonID = new Paragraph(p1_11_PersonID);
                            pg1_11_PersonID.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct1_11_PersonID = new ColumnText(writer.DirectContent);
                            ct1_11_PersonID.SetSimpleColumn(new Rectangle(880, 572));
                            ct1_11_PersonID.AddElement(pg1_11_PersonID);
                            ct1_11_PersonID.Go();

                            Chunk C_11_CertID = new Chunk(CertID, ChFont_CertID);
                            Phrase p_11_CertID = new Phrase(C_11_CertID);
                            Paragraph pg_11_CertID = new Paragraph(p_11_CertID);
                            pg_11_CertID.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct2_11_CertID = new ColumnText(writer.DirectContent);
                            ct2_11_CertID.SetSimpleColumn(new Rectangle(940, 611));
                            ct2_11_CertID.AddElement(pg_11_CertID);
                            ct2_11_CertID.Go();

                            Chunk c2_11_PSex = new Chunk(PSex, ChFont_PSex);
                            Phrase p2_11_PSex = new Phrase(c2_11_PSex);
                            Paragraph pg2_11_PSex = new Paragraph(p2_11_PSex);
                            pg2_11_PSex.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct3_11_PSex = new ColumnText(writer.DirectContent);
                            ct3_11_PSex.SetSimpleColumn(new Rectangle(650, 572));
                            ct3_11_PSex.AddElement(pg2_11_PSex);
                            ct3_11_PSex.Go();
                            break;
                        case "12":
                            Chunk c_12_PName = new Chunk(PName, ChFont_Pname);
                            Phrase p_12_PName = new Phrase(c_12_PName);
                            Paragraph pg_12_PName = new Paragraph(p_12_PName);
                            pg_12_PName.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct_12_PName = new ColumnText(writer.DirectContent);
                            ct_12_PName.SetSimpleColumn(new Rectangle(300, 572));
                            ct_12_PName.AddElement(pg_12_PName);
                            ct_12_PName.Go();

                            Chunk c1_12_PersonID = new Chunk(PersonID, ChFont_Pname);
                            Phrase p1_12_PersonID = new Phrase(c1_12_PersonID);
                            Paragraph pg1_12_PersonID = new Paragraph(p1_12_PersonID);
                            pg1_12_PersonID.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct1_12_PersonID = new ColumnText(writer.DirectContent);
                            ct1_12_PersonID.SetSimpleColumn(new Rectangle(880, 572));
                            ct1_12_PersonID.AddElement(pg1_12_PersonID);
                            ct1_12_PersonID.Go();

                            Chunk C_12_CertID = new Chunk(CertID, ChFont_CertID);
                            Phrase p_12_CertID = new Phrase(C_12_CertID);
                            Paragraph pg_12_CertID = new Paragraph(p_12_CertID);
                            pg_12_CertID.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct2_12_CertID = new ColumnText(writer.DirectContent);
                            ct2_12_CertID.SetSimpleColumn(new Rectangle(940, 613));
                            ct2_12_CertID.AddElement(pg_12_CertID);
                            ct2_12_CertID.Go();

                            Chunk c2_12_PSex = new Chunk(PSex, ChFont_PSex);
                            Phrase p2_12_PSex = new Phrase(c2_12_PSex);
                            Paragraph pg2_12_PSex = new Paragraph(p2_12_PSex);
                            pg2_12_PSex.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct3_12_PSex = new ColumnText(writer.DirectContent);
                            ct3_12_PSex.SetSimpleColumn(new Rectangle(630, 572));
                            ct3_12_PSex.AddElement(pg2_12_PSex);
                            ct3_12_PSex.Go();
                            break;
                        case "13":
                            Chunk c_13_PName = new Chunk(PName, ChFont_Pname);
                            Phrase p_13_PName = new Phrase(c_13_PName);
                            Paragraph pg_13_PName = new Paragraph(p_13_PName);
                            pg_13_PName.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct_13_PName = new ColumnText(writer.DirectContent);
                            ct_13_PName.SetSimpleColumn(new Rectangle(300, 572));
                            ct_13_PName.AddElement(pg_13_PName);
                            ct_13_PName.Go();

                            Chunk c1 = new Chunk(PersonID, ChFont_Pname);
                            Phrase p1 = new Phrase(c1);
                            Paragraph pg1 = new Paragraph(p1);
                            pg1.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct1 = new ColumnText(writer.DirectContent);
                            ct1.SetSimpleColumn(new Rectangle(880, 572));
                            ct1.AddElement(pg1);
                            ct1.Go();

                            Chunk C_CertID = new Chunk(CertID, ChFont_CertID);
                            Phrase pCertID = new Phrase(C_CertID);
                            Paragraph pgCertID = new Paragraph(pCertID);
                            pgCertID.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct2 = new ColumnText(writer.DirectContent);
                            ct2.SetSimpleColumn(new Rectangle(940, 613));
                            ct2.AddElement(pgCertID);
                            ct2.Go();

                            Chunk c2 = new Chunk(PSex, ChFont_PSex);
                            Phrase p2 = new Phrase(c2);
                            Paragraph pg2 = new Paragraph(p2);
                            pg2.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct3 = new ColumnText(writer.DirectContent);
                            ct3.SetSimpleColumn(new Rectangle(630, 572));
                            ct3.AddElement(pg2);
                            ct3.Go();
                            break;
                    }


                    if (RoleSNO == "10")
                    {
                        Chunk c3 = new Chunk(StartDateY, ChFont);
                        Phrase p3 = new Phrase(c3);
                        Paragraph pg3 = new Paragraph(p3);
                        pg3.Alignment = Element.ALIGN_CENTER;
                        ColumnText ct4 = new ColumnText(writer.DirectContent);
                        ct4.SetSimpleColumn(new Rectangle(650, 415));
                        ct4.AddElement(pg3);
                        ct4.Go();

                        Chunk c4 = new Chunk(StartDateM, ChFont);
                        Phrase p4 = new Phrase(c4);
                        Paragraph pg4 = new Paragraph(p4);
                        pg4.Alignment = Element.ALIGN_CENTER;
                        ColumnText ct5 = new ColumnText(writer.DirectContent);
                        ct5.SetSimpleColumn(new Rectangle(780, 415));
                        ct5.AddElement(pg4);
                        ct5.Go();

                        Chunk c5 = new Chunk(StartDateD, ChFont);
                        Phrase p5 = new Phrase(c5);
                        Paragraph pg5 = new Paragraph(p5);
                        pg5.Alignment = Element.ALIGN_CENTER;
                        ColumnText ct6 = new ColumnText(writer.DirectContent);
                        ct6.SetSimpleColumn(new Rectangle(890, 415));
                        ct6.AddElement(pg5);
                        ct6.Go();

                        Chunk c6 = new Chunk(EndDateY, ChFont);
                        Phrase p6 = new Phrase(c6);
                        Paragraph pg6 = new Paragraph(p6);
                        pg6.Alignment = Element.ALIGN_CENTER;
                        ColumnText ct7 = new ColumnText(writer.DirectContent);
                        ct7.SetSimpleColumn(new Rectangle(650, 375));
                        ct7.AddElement(pg6);
                        ct7.Go();

                        Chunk c7 = new Chunk(EndDateM, ChFont);
                        Phrase p7 = new Phrase(c7);
                        Paragraph pg7 = new Paragraph(p7);
                        pg7.Alignment = Element.ALIGN_CENTER;
                        ColumnText ct8 = new ColumnText(writer.DirectContent);
                        ct8.SetSimpleColumn(new Rectangle(780, 375));
                        ct8.AddElement(pg7);
                        ct8.Go();

                        Chunk c8 = new Chunk(EndDateD, ChFont);
                        Phrase p8 = new Phrase(c8);
                        Paragraph pg8 = new Paragraph(p8);
                        pg8.Alignment = Element.ALIGN_CENTER;
                        ColumnText ct9 = new ColumnText(writer.DirectContent);
                        ct9.SetSimpleColumn(new Rectangle(890, 375));
                        ct9.AddElement(pg8);
                        ct9.Go();

                    }
                    else
                    {
                        Chunk c3 = new Chunk(StartDateY, ChFont);
                        Phrase p3 = new Phrase(c3);
                        Paragraph pg3 = new Paragraph(p3);
                        pg3.Alignment = Element.ALIGN_CENTER;
                        ColumnText ct4 = new ColumnText(writer.DirectContent);
                        ct4.SetSimpleColumn(new Rectangle(650, 408));
                        ct4.AddElement(pg3);
                        ct4.Go();

                        Chunk c4 = new Chunk(StartDateM, ChFont);
                        Phrase p4 = new Phrase(c4);
                        Paragraph pg4 = new Paragraph(p4);
                        pg4.Alignment = Element.ALIGN_CENTER;
                        ColumnText ct5 = new ColumnText(writer.DirectContent);
                        ct5.SetSimpleColumn(new Rectangle(780, 408));
                        ct5.AddElement(pg4);
                        ct5.Go();

                        Chunk c5 = new Chunk(StartDateD, ChFont);
                        Phrase p5 = new Phrase(c5);
                        Paragraph pg5 = new Paragraph(p5);
                        pg5.Alignment = Element.ALIGN_CENTER;
                        ColumnText ct6 = new ColumnText(writer.DirectContent);
                        ct6.SetSimpleColumn(new Rectangle(890, 408));
                        ct6.AddElement(pg5);
                        ct6.Go();

                        Chunk c6 = new Chunk(EndDateY, ChFont);
                        Phrase p6 = new Phrase(c6);
                        Paragraph pg6 = new Paragraph(p6);
                        pg6.Alignment = Element.ALIGN_CENTER;
                        ColumnText ct7 = new ColumnText(writer.DirectContent);
                        ct7.SetSimpleColumn(new Rectangle(650, 363));
                        ct7.AddElement(pg6);
                        ct7.Go();

                        Chunk c7 = new Chunk(EndDateM, ChFont);
                        Phrase p7 = new Phrase(c7);
                        Paragraph pg7 = new Paragraph(p7);
                        pg7.Alignment = Element.ALIGN_CENTER;
                        ColumnText ct8 = new ColumnText(writer.DirectContent);
                        ct8.SetSimpleColumn(new Rectangle(780, 363));
                        ct8.AddElement(pg7);
                        ct8.Go();

                        Chunk c8 = new Chunk(EndDateD, ChFont);
                        Phrase p8 = new Phrase(c8);
                        Paragraph pg8 = new Paragraph(p8);
                        pg8.Alignment = Element.ALIGN_CENTER;
                        ColumnText ct9 = new ColumnText(writer.DirectContent);
                        ct9.SetSimpleColumn(new Rectangle(890, 363));
                        ct9.AddElement(pg8);
                        ct9.Go();

                    }

                    switch (RoleSNO)
                    {
                        case "10":
                            Chunk c9_10_Year = new Chunk(StartDateY, ChFont);
                            Phrase p9_10_Year = new Phrase(c9_10_Year);
                            Paragraph pg9_10_Year = new Paragraph(p9_10_Year);
                            pg9_10_Year.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct10_10_Year = new ColumnText(writer.DirectContent);
                            ct10_10_Year.SetSimpleColumn(new Rectangle(535, 109));
                            ct10_10_Year.AddElement(pg9_10_Year);
                            ct10_10_Year.Go();

                            Chunk c10_10_Year = new Chunk(StartDateM, ChFont);
                            Phrase p10_10_Year = new Phrase(c10_10_Year);
                            Paragraph pg10_10_Year = new Paragraph(p10_10_Year);
                            pg10_10_Year.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct11_10_Year = new ColumnText(writer.DirectContent);
                            ct11_10_Year.SetSimpleColumn(new Rectangle(680, 109));
                            ct11_10_Year.AddElement(pg10_10_Year);
                            ct11_10_Year.Go();

                            Chunk c11_10_Year = new Chunk(StartDateD, ChFont);
                            Phrase p11_10_Year = new Phrase(c11_10_Year);
                            Paragraph pg11_10_Year = new Paragraph(p11_10_Year);
                            pg11_10_Year.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct12_10_Year = new ColumnText(writer.DirectContent);
                            ct12_10_Year.SetSimpleColumn(new Rectangle(805, 109));
                            ct12_10_Year.AddElement(pg11_10_Year);
                            ct12_10_Year.Go();
                            break;
                        case "11":
                            Chunk c9_11_Year = new Chunk(StartDateY, ChFont);
                            Phrase p9_11_Year = new Phrase(c9_11_Year);
                            Paragraph pg9_11_Year = new Paragraph(p9_11_Year);
                            pg9_11_Year.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct10_11_Year = new ColumnText(writer.DirectContent);
                            ct10_11_Year.SetSimpleColumn(new Rectangle(535, 117));
                            ct10_11_Year.AddElement(pg9_11_Year);
                            ct10_11_Year.Go();

                            Chunk c10_11_Year = new Chunk(StartDateM, ChFont);
                            Phrase p10_11_Year = new Phrase(c10_11_Year);
                            Paragraph pg10_11_Year = new Paragraph(p10_11_Year);
                            pg10_11_Year.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct11_11_Year = new ColumnText(writer.DirectContent);
                            ct11_11_Year.SetSimpleColumn(new Rectangle(680, 117));
                            ct11_11_Year.AddElement(pg10_11_Year);
                            ct11_11_Year.Go();

                            Chunk c11_11_Year = new Chunk(StartDateD, ChFont);
                            Phrase p11_11_Year = new Phrase(c11_11_Year);
                            Paragraph pg11_11_Year = new Paragraph(p11_11_Year);
                            pg11_11_Year.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct12_11_Year = new ColumnText(writer.DirectContent);
                            ct12_11_Year.SetSimpleColumn(new Rectangle(805, 117));
                            ct12_11_Year.AddElement(pg11_11_Year);
                            ct12_11_Year.Go();
                            break;
                        case "12":
                            Chunk c9_12_Year = new Chunk(StartDateY, ChFont);
                            Phrase p9_12_Year = new Phrase(c9_12_Year);
                            Paragraph pg9_12_Year = new Paragraph(p9_12_Year);
                            pg9_12_Year.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct10_12_Year = new ColumnText(writer.DirectContent);
                            ct10_12_Year.SetSimpleColumn(new Rectangle(535, 125));
                            ct10_12_Year.AddElement(pg9_12_Year);
                            ct10_12_Year.Go();

                            Chunk c10_12_Year = new Chunk(StartDateM, ChFont);
                            Phrase p10_12_Year = new Phrase(c10_12_Year);
                            Paragraph pg10_12_Year = new Paragraph(p10_12_Year);
                            pg10_12_Year.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct11_12_Year = new ColumnText(writer.DirectContent);
                            ct11_12_Year.SetSimpleColumn(new Rectangle(680, 125));
                            ct11_12_Year.AddElement(pg10_12_Year);
                            ct11_12_Year.Go();

                            Chunk c11_12_Year = new Chunk(StartDateD, ChFont);
                            Phrase p11_12_Year = new Phrase(c11_12_Year);
                            Paragraph pg11_12_Year = new Paragraph(p11_12_Year);
                            pg11_12_Year.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct12_12_Year = new ColumnText(writer.DirectContent);
                            ct12_12_Year.SetSimpleColumn(new Rectangle(805, 125));
                            ct12_12_Year.AddElement(pg11_12_Year);
                            ct12_12_Year.Go();
                            break;
                        case "13":
                            Chunk c9_13_Year = new Chunk(StartDateY, ChFont);
                            Phrase p9_13_Year = new Phrase(c9_13_Year);
                            Paragraph pg9_13_Year = new Paragraph(p9_13_Year);
                            pg9_13_Year.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct10_13_Year = new ColumnText(writer.DirectContent);
                            ct10_13_Year.SetSimpleColumn(new Rectangle(535, 107));
                            ct10_13_Year.AddElement(pg9_13_Year);
                            ct10_13_Year.Go();

                            Chunk c10_13_Year = new Chunk(StartDateM, ChFont);
                            Phrase p10_13_Year = new Phrase(c10_13_Year);
                            Paragraph pg10_13_Year = new Paragraph(p10_13_Year);
                            pg10_13_Year.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct11_13_Year = new ColumnText(writer.DirectContent);
                            ct11_13_Year.SetSimpleColumn(new Rectangle(680, 107));
                            ct11_13_Year.AddElement(pg10_13_Year);
                            ct11_13_Year.Go();

                            Chunk c11_13_Year = new Chunk(StartDateD, ChFont);
                            Phrase p11_13_Year = new Phrase(c11_13_Year);
                            Paragraph pg11_13_Year = new Paragraph(p11_13_Year);
                            pg11_13_Year.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct12_13_Year = new ColumnText(writer.DirectContent);
                            ct12_13_Year.SetSimpleColumn(new Rectangle(805, 107));
                            ct12_13_Year.AddElement(pg11_13_Year);
                            ct12_13_Year.Go();
                            break;
                    }




                    document.Close();
                    Response.Clear();
                    Response.AddHeader("Transfer-Encoding", "identity");
                    Response.AddHeader("content-disposition", "attachment;filename=CertificatePring.pdf");
                    Response.ContentType = "application/octet-stream";
                    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    Response.OutputStream.Flush();
                    Response.OutputStream.Close();
                    Response.Flush();
                    Response.Close();
                }
                catch (Exception ex)
                {
                    string script = "<script>alert('" + ex.Message + "');</script>";

                }
            }
            else
            {
                Response.Write("<script>alert('您的證書為舊證書字號，請先更新再下載。')</script>");
            }

        }
        else
        {
            Response.Write("<script>alert('尚未開放')</script>");
        }
    }
    protected void bindData_FeedBack()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;


        string sql = @"    WITH Base AS (
         Select distinct P.PersonID,CES.ELSName,P.PName,LA.CompletedDate,LF.ELSCode,P.OrganSNO,P.RoleSNO,P.PersonSNO,QCE.ELName
	     from Person P
	     Join QS_LearningAnswer LA ON LA.PersonID=P.PersonID
	     Join QS_LearningFeedback LF ON LF.QID=LA.QID
	     Join QS_CourseELearningSection CES ON CES.ELSCode=LF.ELSCode
		 Join QS_CourseELearning QCE On QCE.ELCode=CES.ELCode
        )

        SELECT Base.ELSName,Base.ELName,Base.PersonID,Base.PersonSNO,Base.PName,Base.CompletedDate, ROW_NUMBER() OVER (ORDER BY PersonID) AS ROW_NO FROM Base
        LEFT JOIN Organ O ON O.OrganSNO = Base.OrganSNO
        LEFT JOIN Role R ON R.RoleSNO =Base.RoleSNO
        where 1=1 And PersonSNO=@PersonSNO
            ";
        aDict.Add("@PersonSNO", userInfo.PersonSNO);
        objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            rpt_FeedBack.DataSource = objDT.DefaultView;
            rpt_FeedBack.DataBind();
        }
        else
        {
            Tr10.Visible = false;
            lb_FeedBack.Visible = true;
        }
    }
    private PdfPCell SetCell(string msg, Font font, FieldPositioningEvents events = null, bool border = false, int height = 20)
    {
        PdfPCell cell = new PdfPCell(new Phrase(msg, font));
        if (events != null) cell.CellEvent = events;
        if (!border) cell.Border = 0;
        cell.FixedHeight = height;

        return cell;
    }

    protected void gv_Certificate_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //要隱藏的欄位，如果前台filed隱藏會抓不到值，所以在RowCreate時才隱藏
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Cells[0].Visible = false;
            //巡覽Gridview值
            string ToolTipString = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "SysChange"));
            if (ToolTipString == "True")
            {

                e.Row.Cells[2].Visible = false;
            }
            else
            {
                e.Row.Cells[1].Visible = false;
            }



        }
    }

    protected void Btn_Print_Click1(object sender, EventArgs e)
    {

    }
    protected void LK_integral_Click(object sender, EventArgs e)
    {

        LinkButton btn = (LinkButton)(sender);
        string PClassValue = btn.CommandArgument;
        bindData_Detail(PClassValue);
        c_content.Visible = true;
    }
    protected void LK_Done_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);
        string[] AllArg = btn.CommandArgument.Split(',');
        string PClassSNO = AllArg[0].ToString();
        string Class1 = AllArg[1].ToString();
        string Ctype = AllArg[2].ToString();
        bindData_Doneintegral(PClassSNO, Class1, Ctype);
        Div1.Visible = true;

    }

    protected void LK_NotDone_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);
        string[] AllArg = btn.CommandArgument.Split(',');
        string PClassSNO = AllArg[0].ToString();
        string Class1 = AllArg[1].ToString();
        string Ctype = AllArg[2].ToString();
        bindData_integral(PClassSNO, Class1, Ctype);
        Div1.Visible = true;
    }

    protected void LK_Eintegral_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);
        string[] AllArg = btn.CommandArgument.Split(',');

        string PClassValue = AllArg[0].ToString();
        string EPClassSNO = AllArg[1].ToString();
        bindData_Eintegral(PClassValue);
        bindData_UploadEintegral(EPClassSNO);
        EIntegralSUM.Visible = true;
        EUploadSUM.Visible = true;

    }

    protected void LK_EDone_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);

        string[] AllArg = btn.CommandArgument.Split(',');
        string PClassSNO = AllArg[0].ToString();
        string Class1 = AllArg[1].ToString();
        string Ctype = AllArg[2].ToString();
        EbindData_Doneintegral(PClassSNO, Class1, Ctype);
        EIntegralDetail.Visible = true;

    }

    protected void LK_ENotDone_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);

        string[] AllArg = btn.CommandArgument.Split(',');
        string PClassSNO = AllArg[0].ToString();
        string Class1 = AllArg[1].ToString();
        string Ctype = AllArg[2].ToString();
        EbindData_integral(PClassSNO, Class1, Ctype);
        EIntegralDetail.Visible = true;

    }

    protected void LK_C_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);
        string Ctype = "4";
        string EPClassSNO = btn.CommandArgument;
        EbindData_Upliadintegral(EPClassSNO, Ctype);
        EUploadDetail.Visible = true;

    }

    protected void LK_O_Click(object sender, EventArgs e)
    {

        LinkButton btn = (LinkButton)(sender);
        string Ctype = "1";
        string EPClassSNO = btn.CommandArgument;
        EbindData_Upliadintegral(EPClassSNO, Ctype);
        EUploadDetail.Visible = true;

    }

    protected void LK_E_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);
        string Ctype = "2";
        string EPClassSNO = btn.CommandArgument;
        EbindData_Upliadintegral(EPClassSNO, Ctype);
        EUploadDetail.Visible = true;

    }

    protected void LK_P_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);
        string Ctype = "3";
        string EPClassSNO = btn.CommandArgument;
        EbindData_Upliadintegral(EPClassSNO, Ctype);
        EUploadDetail.Visible = true;

    }


    protected void btn_Auto_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonID", userInfo.PersonID);
        aDict.Add("Note", "學員更新");
        objDH.queryData("dbo.SP_AutoAuditIntegral @PersonID, @Note", aDict);
    }

    protected void rpt_integralS_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName.Equals("Upload"))
        {
            string Class1 = ((Label)e.Item.FindControl("lb_Class1")).Text.Trim();
            string Ctype = ((Label)e.Item.FindControl("lb_CType")).Text.Trim();
            int CtypeNumber = 0;
            switch (Ctype)
            {
                case "線上":
                    CtypeNumber = 1;
                    break;
                case "實體":
                    CtypeNumber = 2;
                    break;
                case "實習":
                    CtypeNumber = 3;
                    break;
                case "通訊":
                    CtypeNumber = 4;
                    break;

            }
            if (Ctype != "實習")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('目前尚不需上傳此類別之檔案');", true);
            }
            else
            {
                //Response.Write("var w = screen.width; window.open('CertificateChange.aspx?Psno=" + PersonSNO + "&CtSNO=" + CheckCtypeSNO[0] + "&Sort=" + Sort + "&PClassSNO=" + CheckPClassSNO[0] + "','','width=w,height=500');</script>");
                string js = "window.open('./FileUpload.aspx?Ctype=" + CtypeNumber + "&userinfo=" + userInfo.PersonSNO + "', 'FileUpload', config='height=300,width=600,scrollbar=no');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "openEL", js, true);
            }

        }

    }


    protected void btn_IntegralPrint_Click(object sender, EventArgs e)
    {
        



    }


    protected void btn_prove_Click(object sender, EventArgs e)
    {        
        LinkButton btn = (LinkButton)sender;
        RepeaterItem item = (RepeaterItem)btn.NamingContainer;
        Label lblName = (Label)item.FindControl("dd");
        string PClassSNO = lblName.Text;
        bool IsCore = Utility.ReturnCtypeSNO(PClassSNO);
        if (IsCore && hf_Core.Value == "2")
        {
            Response.Redirect("./ScorePrintAjax.aspx?PClassSNO=" + PClassSNO + "&PersonSNO=" + userInfo.PersonSNO + "&Word=0&Core=2");
        }
        else if(IsCore && hf_Core.Value == "1")
        {
            Response.Redirect("./ScorePrintAjax.aspx?PClassSNO=" + PClassSNO + "&PersonSNO=" + userInfo.PersonSNO + "&Word=0&Core=1");
        }
        else
        {
            Response.Redirect("./ScorePrintAjax.aspx?PClassSNO=" + PClassSNO + "&PersonSNO=" + userInfo.PersonSNO + "&Word=0");
        }
       
        
    }

    protected void rpt_CoursePlanningClass_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lbldescriptionlink = (Label)e.Item.FindControl("dd");
        }
    }

    protected void btn_Eprove_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        RepeaterItem item = (RepeaterItem)btn.NamingContainer;
        Label lblName = (Label)item.FindControl("CC");
        string PClassSNO = lblName.Text;
        Response.Redirect("./ScorePrintAjax.aspx?EPClassSNO=" + PClassSNO + "&PersonSNO=" + userInfo.PersonSNO + "");
    }

    protected void rpt_integralS_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            LinkButton btn = (LinkButton)e.Item.FindControl("LK_Upload");
            Label lb = (Label)e.Item.FindControl("lb_CType");
            if (lb.Text == "實習")
            {
                btn.Enabled = true;
            }
            else
            {
                btn.ForeColor = System.Drawing.Color.Gray;
                btn.CssClass = "buttonD";
            }
        }
    }

    protected void btn_pdf_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String sql = @"
           		 SELECT 
				P.PName as '姓名',P.PersonID as '身分證', e.ELName as 'E-Learning主題名稱', es.ELSName as 'E-Learning課程名稱', (cast(lr.ELSPart as varchar)+'/'+cast(es.ELSPart as varchar)) '節數',
				convert(varchar(16), lr.FinishedDate, 120) as '課程完成日'
            From QS_LearningRecord lr
                LEFT JOIN QS_CourseELearningSection es on es.ELSCode=lr.ELSCode
                LEFT JOIN QS_CourseELearning e on e.ELCode=es.ELCode
                LEFT JOIN Person P On P.PersonID=lr.PersonID
            Where lr.PersonID=@PersonID
			Order by ELSName
        ";
        aDict.Add("PersonID", userInfo.PersonID);
        DataTable objDT = objDH.queryData(sql, aDict);
        ExportToPdf(objDT);
    }
    public void ExportToPdf(DataTable dt)
    {
        MemoryStream ms = new MemoryStream();
        Document document = new Document();
        PdfWriter writer = PdfWriter.GetInstance(document, ms);
        document.Open();
        BaseFont bfChinese = BaseFont.CreateFont(@"C:\Windows\Fonts\kaiu.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        Font font5 = new Font(bfChinese, 12);
        

        PdfPTable table = new PdfPTable(dt.Columns.Count);
        PdfPRow row = null;
        float[] widths = new float[dt.Columns.Count];
        for (int i = 0; i < dt.Columns.Count; i++)
            widths[i] = 4f;

        table.SetWidths(widths);

        table.WidthPercentage = 100;
        int iCol = 0;
        string colname = "";
        PdfPCell cell = new PdfPCell(new Phrase("Products"));

        cell.Colspan = dt.Columns.Count;

        foreach (DataColumn c in dt.Columns)
        {
            table.AddCell(new Phrase(c.ColumnName, font5));
        }

        foreach (DataRow r in dt.Rows)
        {
            if (dt.Rows.Count > 0)
            {
                for (int h = 0; h < dt.Columns.Count; h++)
                {
                    table.AddCell(new Phrase(r[h].ToString(), font5));
                }
            }
        }
        document.Add(table);
        document.Close();
        Response.Clear();
        Response.AddHeader("Transfer-Encoding", "identity");
        Response.AddHeader("content-disposition", "attachment;filename="+ userInfo.UserName +"Elearning上課紀錄.pdf");
        Response.ContentType = "application/octet-stream";
        Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
        Response.OutputStream.Flush();
        Response.OutputStream.Close();
        Response.Flush();
        Response.Close();
    }
}