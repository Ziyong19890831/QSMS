using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.events;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Mgt_ReportMemberDetail : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    DateTime NowTime = Convert.ToDateTime(DateTime.Now);
    string PersonSNO = "";
    string PersonID = "";
    string RoleSNO = "";

    public void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Utility.setMStatus(ddl_Status, "請選擇");
            PersonSNO = Convert.ToString(Request.QueryString["sno"]);
            getPersonInfo(PersonSNO);
            if (PersonID != "")
            {
                bindData_LearningRecord();
                bindData_LearningScore();
                bindSMKContract();
                bindData_Event();
                bindData_Certificate();
                bindData_CertificateN();
                bindData_CoursePlanningClass();
                bindData_ECoursePlanningClass();
                bindData_LearningReport();
                bindData_FeedBack();
                bindPharmacy();
            }
        }
    }

    //用PersonSNO撈出個人資料
    protected void getPersonInfo(string PersonSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonSNO", PersonSNO);
        DataTable objDT;
        string sql = @"
            Select 
		        P.PersonSNO , r.RoleName , R.RoleSNO , p.PAccount, p.MStatusSNO , p.PName , RE.REName
                , p.PersonID , p.IsEnable, P.CreateDT, P.ModifyDT
		        , p.PTel_O , p.PFax_O , p.PTel , p.PFax , p.PPhone  , p.PMail
		        , p.PZCode , p.PAddr , p.JMajor , p.JLicID  , P.country
                , p.Degree , p.QSExp, P.TJobType, P.TSType
		        , p.JobTitle , p.SchoolName , p.Major , p.Degree , p.QSExp 
                , CONVERT(char(10), p.PBirthDate,111) PBirthDate , O.[OrganName],O.[OrganAddr]
		        , case when p.PSex = 1 then '男' ELSE '女' END Sex ,P.IsEnable
		        , case when p.JLicStatus = 0 then '終止' ELSE '正常' END LicStatus
                , O.OrganSNO, O.OrganCode, O.OrganName, O.AreaCodeA, O.AreaCodeB, OC.ClassName,O.OrganTel
                , case when O.AbortDate is null then'營業'  ELSE '歇業' END AbortDate  
                , MP.OrganCode MPOrganCode, MPO.OrganName MPOrganName, MPO.OrganTel MPOrganTel,MPO.OrganAddr MPOOrganAddr,P.City , P.Area
                , MP.LStatus ,MP.LValid ,MP.VEDate, MP.LCN, MP.LStype, MP.LRtype, MP.LSCN, MP.JCN ,P.Note , MP.note PersonMPNote, MP.[OwnerUpdateDate] , MP.[GovUpdateDate]
            FROM Person P 
                LEFT JOIN Role r ON r.RoleSNO = p.RoleSNO										
                LEFT JOIN QS_MemberStatus m ON m.MStatusSNO = p.MStatusSNO
                LEFT JOIN Organ O ON O.OrganSNO=P.OrganSNO  
				LEFT JOIN RoleException RE On RE.RESNO =P.RoleException
				LEFT JOIN PersonMP MP on MP.PersonID=P.PersonID
                LEFT JOIN Organ MPO ON MPO.OrganCode=MP.OrganCode  
                LEFT Join OrganClass OC on OC.ClassSNO=MPO.OrganClass
                LEFT Join Config C On C.Pval=P.MStatusSNO and PGroup='Mstatus'
            Where P.PersonSno=@PersonSNO
        ";
        objDT = objDH.queryData(sql, aDict);
        lb_PName.Text = objDT.Rows[0]["PName"].ToString();
        lb_PersonID.Text = objDT.Rows[0]["PersonID"].ToString();
        PersonID = objDT.Rows[0]["PersonID"].ToString();
        RoleSNO = objDT.Rows[0]["RoleSNO"].ToString();

        //撈出學員詳細資料
        txt_Name.InnerText = objDT.Rows[0]["PName"].ToString();
        lb_Role.InnerText = objDT.Rows[0]["RoleName"].ToString();        
        txt_Account.InnerText = objDT.Rows[0]["PAccount"].ToString();
        txt_Personid.InnerText = objDT.Rows[0]["PersonID"].ToString();
        txt_Mail.InnerText = objDT.Rows[0]["PMail"].ToString();
        txt_degree.InnerText = objDT.Rows[0]["Degree"].ToString();
        txt_Country.InnerText = objDT.Rows[0]["Country"].ToString();
        txt_Birthday.InnerText = objDT.Rows[0]["PBirthDate"].ToString();
        txt_ZipCode.InnerText = objDT.Rows[0]["PZCode"].ToString();
        txt_Addr.InnerText = objDT.Rows[0]["City"].ToString() + objDT.Rows[0]["Area"].ToString() + objDT.Rows[0]["PAddr"].ToString();
        txt_Tel.InnerText = objDT.Rows[0]["PTel"].ToString();
        txt_Phone.InnerText = objDT.Rows[0]["PPhone"].ToString();
        txt_OrganCode.InnerText= objDT.Rows[0]["OrganCode"].ToString();
        txt_OrganName.InnerText = objDT.Rows[0]["OrganName"].ToString();
        lb_LCN.Text = objDT.Rows[0]["LCN"].ToString();
        ddl_IsEnable.SelectedValue = objDT.Rows[0]["IsEnable"].ToString();
        ddl_Status.SelectedValue= objDT.Rows[0]["MStatusSNO"].ToString();
        if (objDT.Rows[0]["VEDate"].ToString() == "")
        {
            lb_VEDate.Text = "";
        }
        else
        {
            lb_VEDate.Text = Convert.ToDateTime(objDT.Rows[0]["VEDate"]).ToShortDateString();
        }

        lb_LValid.Text = objDT.Rows[0]["LValid"].ToString();
        lb_LStatus.Text = objDT.Rows[0]["LStatus"].ToString();
        lb_AbortDate.Text = objDT.Rows[0]["AbortDate"].ToString();
        lb_organClassName.Text = objDT.Rows[0]["ClassName"].ToString();
        lb_OrganCode.Text = objDT.Rows[0]["MPOrganCode"].ToString();
        lb_OrganName.Text = objDT.Rows[0]["MPOrganName"].ToString();
        lb_OrganAddr.Text = objDT.Rows[0]["MPOOrganAddr"].ToString();
        lb_OrganTel.Text = objDT.Rows[0]["MPOrganTel"].ToString();
        lb_JCN.Text = objDT.Rows[0]["JCN"].ToString();
        lb_Note.InnerText = objDT.Rows[0]["Note"].ToString();
        lb_MPNote.Text = objDT.Rows[0]["PersonMPNote"].ToString();
        lb_OwnerUpdateDate.Text = objDT.Rows[0]["OwnerUpdateDate"].ToString();
        lb_GovUpdateDate.Text = objDT.Rows[0]["GovUpdateDate"].ToString();

        if (RoleSNO == "10" || RoleSNO == "11")
        {
            ForDoctor.Visible = true;
            lb_LRtype.Text = objDT.Rows[0]["LRtype"].ToString();
            lb_LSType.Text = objDT.Rows[0]["LSType"].ToString();
            lb_LSCN.Text = objDT.Rows[0]["LSCN"].ToString();
        }
        else
        {
            ForDoctor.Visible = false;
        }
        if (RoleSNO == "13")
        {
            ForOther.Visible = true;
            lb_TJobType.Text = objDT.Rows[0]["TJobType"].ToString();
            lb_TSType.Text = objDT.Rows[0]["TSType"].ToString();
        }
        else
        {
            ForOther.Visible = false;
        }
    }


    //撈已完成的E-Learning上課紀錄
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
        aDict.Add("PersonID", PersonID);
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
        aDict.Add("PersonID", PersonID);
        DataTable objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            LearningScore.DataSource = objDT.DefaultView;
            LearningScore.DataBind();
        }
        else
        {
            Tr9.Visible = false;
            lb_LearningScore.Visible = true;
        }

    }
    //撈已完成的E-Learning線上測驗
    protected void bindData_LearningReport()
    {
        PersonSNO = Convert.ToString(Request.QueryString["sno"]);
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = @"
          With 
                	getAllParts As (
                		Select ces.ELSCode, ces.ELSPart
                		From QS_CourseELearningSection ces 
						Left Join QS_Course QC On QC.ELSCode=ces.ELSCode
						
                	)
                
                	--step1-2 取得學員上課之統計節數
                	, getLearningParts As (
                		Select PersonID, ELSCode, Count(1) FinishedParts 
                		From (
                			Select Distinct lr.PersonID, lr.ELSCode, lr.ELSPart
                			From QS_LearningRecord lr 
                				Left Join QS_CourseELearningSection ces ON ces.ELSCode=lr.ELSCode
								Left Join QS_Course QC On QC.ELSCode=ces.ELSCode
								
                			--Where PersonID=@PersonID
                		) t
                		Group By PersonID, ELSCode
                	)
                
                	--step1-3 取得學員已完成課程之紀錄
                	, getFinishedRecord As (
                		Select 
                			ap.ELSCode, ap.ELSPart, lp.PersonID
                		From getAllParts ap
                			Left Join getLearningParts lp ON lp.ELSCode=ap.ELSCode 
                		Where ap.ELSPart<=lp.FinishedParts 
                	)
                
                	--step2 該學員是否已完成課程的測驗
                	, getFinishedExam As (
                		Select Distinct ls.PersonID, ls.ELSCode, ls.ExamDate, ls.Score, ls.IsPass 
                		From QS_LearningScore ls 
						Left Join QS_Course QC On QC.ELSCode=ls.ELSCode

                		Where 
                		--PersonID=@PersonID And 
                		ls.IsPass=1 
                	) 
                
                	--step3 該學員是否已完成課程的滿意度調查
                	, getFinishedFeedback As (
                		Select Distinct la.PersonID, lf.ELSCode, lf.FBID 
                		From QS_LearningAnswer la
                			Left Join QS_LearningFeedback lf ON lf.QID=la.QID
							Left Join QS_Course QC On QC.ELSCode=lf.ELSCode

                		
                	) 
                
                	--step4 取得ELSCode對應的CourseSNO
                	, getCourseSNO As (
                		Select c.CourseSNO, ces.ELSCode
                		From QS_CourseELearningSection ces
                			Left Join QS_Course c ON c.ELSCode=ces.ELSCode
						
                	) 
                
                	----step5 取得該學員所有紀錄
                	, getAllFinishedCourse As (
                		SELECT c.CourseSNO,QC.CourseName, QC.PClassSNO,
                		fe.PersonID ,P.PersonSNO,Qc.Class1
                		,case when fr.ELSCode is not null then '已觀看'　else '未觀看' END 'Record'
                		,case Convert(varchar(10),fe.IsPass) when 1 then '通過' Else '不通過' END 'Exam'
                		,case when ff.ELSCode is not null then '已填寫'　else '未填寫' END'Feedback'
                		From getCourseSNO c
							Left Join QS_Course Qc ON Qc.CourseSNO=C.CourseSNO
							Left Join getFinishedExam		fe ON fe.ELSCode=c.ELSCode 
                			Left Join getFinishedRecord		fr ON fr.ELSCode=c.ELSCode And fr.PersonID=fe.PersonID
                			Left Join getFinishedFeedback	ff ON ff.ELSCode=c.ELSCode And ff.PersonID=fe.PersonID
							LEFT JOIN Person P ON fr.PersonID=P.PersonID
                		--Where c.CourseSNO Is Not Null And 
                		--	  fr.ELSCode Is Not Null And 
                		--	  fe.ELSCode Is Not Null And 
                		--	  ff.ELSCode Is Not Null
                		where c.CourseSNO is not null
                	) ,getallData as(
                    --取得所有學員的E-Learning紀錄
                	    Select distinct GAFC.CourseSNO
						,p.PName
						,p.PersonSNO
						,GAFC.PersonID       
						,GAFC.CourseName
						,QCT.CTypeName
						,GAFC.Exam
						,GAFC.Feedback
						,GAFC.Record　	
						,QCT.CTypeSNO	
						,I.ISNO					
						,GAFC.Class1
						--,(Select 1 From QS_Integral QI Where QI.CourseSNO=GAFC.CourseSNO and QI.PersonSNO=GAFC.PersonSNO) Chk
						from getAllFinishedCourse GAFC
						Left JOIN Person P ON P.PersonID=GAFC.PersonID
						Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO=GAFC.PClassSNO
						Left Join QS_CertificateType QCT On QCT.CTypeSNO=QCPC.CTypeSNO
                       
						LEFT JOIN QS_Integral I ON I.CourseSNO=GAFC.CourseSNO and I.PersonSNO=P.PersonSNO				
						)
						Select gd.PersonSNO,gd.CourseName,gd.CTypeName,C1.MVal,gd.Exam,gd.Feedback,gd.Record
						,case when gd.ISNO is not null then '已取得' Else '未取得' End ISNO
						from getallData gd
						Left JOIN Person P ON P.PersonID=gd.PersonID
						 LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
						 Left Join Config C1 On C1.PVal=gd.Class1 and C1.PGroup='CourseClass1'
                        LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
                        where gd.PersonSNO=@PersonSNO
        ";
        aDict.Add("PersonSNO", PersonSNO);
        DataTable objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            LearningReport.DataSource = objDT.DefaultView;
            LearningReport.DataBind();
        }
        else
        {
            tbl_LearningScore.Visible = false;
            lb_LearningReport.Visible = true;
        }

    }

    protected void bindData_Event()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String sql = @"
            SELECT 
	            e.EventName, 
				convert(varchar(16), ed.CreateDT, 120) CreateDT,
                C.MVal Audit
            From EventD ed
                LEFT JOIN Event e on e.EventSNO=ed.EventSNO
				Left Join Config C On c.PVal=ed.Audit and C.PGroup='EventAudit'
            Where ed.PersonSNO=@PersonSNO 
        ";
        aDict.Add("PersonSNO", PersonSNO);
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

    //撈已取得的證書資料
    protected void bindData_Certificate()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;

        string sql = @"
              SELECT 
             CT.CTypeName,
                Cast(C.CertPublicDate As varchar(10)) CertPublicDate,
                Cast(C.CertStartDate As varchar(10)) CertStartDate,
                Cast(C.CertEndDate As varchar(10)) CertEndDate,
                (Case C.CertExt When 1 Then '有' Else '無' End) CertExt,
                C.Note,C.CertSNO,C.SysChange,C.CertID  
            FROM QS_Certificate C
                Left JOIN QS_CertificateType CT on CT.CTypeSNO=C.CTypeSNO
                Left JOIN QS_CertificateUnit CU on CU.CUnitSNO=C.CUnitSNO
            WHERE
                C.PersonID=@PersonID And CertEndDate >= getdate()
        ";
        aDict.Add("PersonID", PersonID);
        //if (userInfo.RoleOrganType != "S")
        //{
        //    sql += " And C.IsChange <> 1 ";
        //}
        sql += " Order by CT.Sort ";
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

    protected void bindData_CertificateN()
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
                (Case C.CertExt When 1 Then '有' Else '無' End) CertExt,C.CertID,
                C.Note       
            FROM QS_Certificate C
                Left JOIN QS_CertificateType CT on CT.CTypeSNO=C.CTypeSNO
                Left JOIN QS_CertificateUnit CU on CU.CUnitSNO=C.CUnitSNO
            WHERE
                C.PersonID=@PersonID And CertEndDate <getdate()
        ";
        aDict.Add("PersonID", PersonID);
        if (userInfo.RoleOrganType != "S")
        {
            sql += " And C.IsChange <> 1 ";
        }
        sql += " Order by CT.Sort ";
        objDT = objDH.queryData(sql, aDict);

        if (objDT.Rows.Count > 0)
        {
            gv_Certificate_N.DataSource = objDT;
            gv_Certificate_N.DataBind();
        }
        else
        {
            gv_Certificate_N.Visible = false;
            lb_CertificateN.Visible = true;
        }

    }

    //撈已完成或進行中的課程規劃表
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
				  Left Join QS_ECoursePlanningClass QECPC on QECPC.PClassSNO=QCPC.PClassSNO
                  Left Join QS_CertificateType QCT on QCT.CTypeSNO=QCPC.CTypeSNO
                  where 1=1 and I.isUsed <> 1 and I.PersonSNO=@PersonSNO and QC.Class1 <> 3 and QECPC.EPClassSNO is null
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
        aDict.Add("@PersonSNO", PersonSNO);
        objDT = objDH.queryData(sql, aDict);
        DataTable ConvertToDatarow = null;
        DataRow[] row = objDT.Select("PClassTotalHr <> 0");
        if (row.Count() > 0)
        {
            ConvertToDatarow = row.CopyToDataTable();
        }
        if (objDT.Rows.Count > 0 && row.Count() > 0)
        {
            for (int i = 0; i < objDT.Rows.Count; i++)
            {
                string PClassSNO= objDT.Rows[i]["PClassSNO"].ToString();
                string CtypeSNO = objDT.Rows[i]["CtypeSNO"].ToString();
                if (CtypeSNO == "10" || CtypeSNO == "11")
                {
                    string NecessaryC = objDT.Rows[i]["NecessaryC"].ToString();
                    string PersonID = Utility.ConvertPersonSNOToPersonID(objDT.Rows[i]["PersonSNO"].ToString());
                    bool CheckPersonGetCertificate = Utility.CheckPersonJuniorSenior(NecessaryC, PersonID);
                    int PClassTotalHr = Convert.ToInt16(objDT.Rows[i]["PClassTotalHr"]);
                    int sumHours = Convert.ToInt16(objDT.Rows[i]["sumHours"]);
                    if ((sumHours - PClassTotalHr <= 0 && CheckPersonGetCertificate) || (sumHours - PClassTotalHr <= 0 && PClassSNO=="29") || (sumHours - PClassTotalHr <= 0 && PClassSNO == "28"))
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

    //撈已完成或進行中的課程規劃表
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
                    ,QCPC.ElearnLimit
                	,Case when sum(CHour) >= QCPC.ElearnLimit then QCPC.ElearnLimit Else sum(CHour) End PClassTotalHr
                  FROM [New_QSMS].[dbo].[QS_Integral] I
                  Left Join Person P on P.PersonSNO=I.PersonSNO
                  Left Join QS_Course QC on QC.CourseSNO=I.CourseSNO
                  Left Join QS_ECoursePlanningClass QCPC on QCPC.PClassSNO=QC.PClassSNO
                  Left Join QS_CertificateType QCT on QCT.CTypeSNO=QCPC.CTypeSNO 
                    where 1=1 and P.PersonSNO=@PersonSNO and CTypeName <> '' and I.IsUsed <>1
                  Group by QCPC.PlanName,QCT.CTypeName,QCPC.CStartYear,QCPC.CEndYear,QC.PClassSNO,I.PersonSNO,QC.Class1,QCPC.EPClassSNO,QCPC.ElearnLimit
                  )
                  , getAllCourseHours As (
                				Select distinct  P.personID,QEPC.PlanName,QEPC.PClassSNO
								 ,QEPC.EPClassSNO,QEPC.TotalIntegral sumHours
								 ,QEPC.CStartYear,QEPC.CEndYear
								From QS_ECoursePlanningClass QEPC 
								Left Join QS_ECoursePlanningRole QEPR On QEPR.EPClassSNO=QEPC.EPClassSNO
								Left Join QS_CertificateType QCT On QCT.RoleSNO=QEPR.RoleSNO
								left join Role R on R.DocGroup=QEPR.RoleSNO
								Left Join Person P On P.RoleSNO=R.RoleGroup
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
                
                  select getAllCourseHours.*,isnull(getEintegral.TotalHr,0)+isnull(getsomething.PClassTotalHr,0) PClassTotalHr,getsomething.CTypeName,getsomething.ElearnLimit
                    from getAllCourseHours
                  left join getsomething on getsomething.EPClassSNO=getAllCourseHours.EPClassSNO
				  left join getEintegral on getEintegral.EPClassSNO=getAllCourseHours.EPClassSNO
                 
                 
     
            ";
        aDict.Add("@PersonSNO", PersonSNO);
        objDT = objDH.queryData(sql, aDict);
        DataTable ConvertToDatarow = null;
        DataRow[] row = objDT.Select("PClassTotalHr <> 0");
        if (row.Count() > 0)
        {
            ConvertToDatarow = row.CopyToDataTable();
        }

        //string PClassTotalHr = objDT.Rows[0]["PClassTotalHr"].ToString();


        if (objDT.Rows.Count > 0 && row.Count() > 0)
        {
            //lb_ElearnLimit.Text = ConvertToDatarow.Rows[0]["ElearnLimit"].ToString();
            rpt_ECoursePlanningClass.DataSource = ConvertToDatarow;
            rpt_ECoursePlanningClass.DataBind();
        }
        else
        {
            tbl_ECoursePlanningClass.Visible = false;
            lb_ECoursePlanningClass.Visible = true;
        }
    }

    protected void bindData_integral(string PclassSNO, string Class1, string Ctype)
    {

        PersonSNO = Convert.ToString(Request.QueryString["sno"]);
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
                      where I.PersonSNO=@PersonSNO and I.IsUsed=0
                      )
                      select Class1,CourseName,Ctype,CHour,CStartYear,CEndYear,X.Class,X.Type,
					  case O when  1 then'已取得' ELSE '未取得' END  積分,'-' as ExamDate,'-' as Unit,'-' as ClassLocation,'-' as CHour
					  from getX X
                      FULL outer Join getY Y ON Y.CourseSNO=X.CourseSNO
					  Left JOIN QS_CoursePlanningRole CPR ON CPR.RClassSNO=X.PClassSNO
					  Left Join Role R ON R.RoleSNO=CPR.RoleSNO
                       where X.PClassSNO=@PClassSNO and X.Class=@Class and X.Type=@Type and O is null
                    
            
            
                    
            
            ";
        aDict.Add("@PersonSNO", PersonSNO);
        aDict.Add("@PClassSNO", PclassSNO);
        aDict.Add("@Class", Class1);
        aDict.Add("@Type", Ctype);
        objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            Repeater1.DataSource = objDT.DefaultView;
            Repeater1.DataBind();
            Repeater2.DataSource = objDT.DefaultView;
            Repeater2.DataBind();
        }
        else
        {
            Repeater1.DataSource = null;
            Repeater1.DataBind();
            Repeater2.DataSource = null;
            Repeater2.DataBind();
        }

    }

    protected void bindData_Doneintegral(string PclassSNO, string Class1, string Ctype)
    {

        PersonSNO = Convert.ToString(Request.QueryString["sno"]);
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
                      select Class1,CourseName,X.Ctype,CHour,CStartYear,CEndYear,X.Class,X.Type,
					  case O when  1 then'已取得' ELSE '未取得' END  積分,convert(varchar, QL.ExamDate, 111)ExamDate,QL.Unit,QL.ClassLocation
					  from getX X
                      FULL outer Join getY Y ON Y.CourseSNO=X.CourseSNO
					  Left JOIN QS_CoursePlanningRole CPR ON CPR.RClassSNO=X.PClassSNO
					  Left Join Role R ON R.RoleSNO=CPR.RoleSNO
                      Left Join [QS_LearningUpload] QL On QL.PersonSNO=@PersonSNO and QL.CourseSNO=Y.CourseSNO
                       where X.PClassSNO=@PClassSNO and X.Class=@Class and X.Type=@Type and O=1
                            
            ";
        aDict.Add("@PersonSNO", PersonSNO);
        aDict.Add("@PClassSNO", PclassSNO);
        aDict.Add("@Class", Class1);
        aDict.Add("@Type", Ctype);
        objDT = objDH.queryData(sql, aDict);

        if (Ctype == "2")
        {
            Repeater2.DataSource = objDT.DefaultView;
            Repeater2.DataBind();
        }
        else
        {
            Repeater1.DataSource = objDT.DefaultView;
            Repeater1.DataBind();
        }




    }

    protected void bindData_Eintegral(string PclassSNO)
    {

        PersonSNO = Convert.ToString(Request.QueryString["sno"]);
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
                where QI.PersonSNO=@PersonSNO  and QC.PClassSNO=@PClassSNO and QI.isused<>1
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType
                
                ),getalreadyI as(
                Select gI.PClassSNO,gI.課程類別,gI.Class1,gI.授課方式,gI.CType,gI.應取得,isnull(gac.已取得,0)  已取得 
				from getalltypeCourse gI
                Left Join getIntegral  gac On gI.Class1=gac.Class1 and gI.CType=gac.CType
                )
                Select gaI.PClassSNO,Class1,QEPC.EPClassSNO
				,(Select 已取得 from getalreadyI where CType=1) 已取得
				,(Select 應取得-已取得 from getalreadyI where CType=1) 未取得
				,(Select Sum(Integral) from QS_EIntegral
					Left Join Person P On P.PersonID=QS_EIntegral.PersonID
                    left join QS_ECoursePlanningClass QEPC on QS_EIntegral.EPClassSNO=QEPC.EPClassSNO
				where CType in (1,2,3,4) and P.PersonSNO=@PersonSNO and QEPC.PClassSNO=@PClassSNO and IsUsed<>1) 實體積分上傳
				from getalltypeCourse gaI
                Left Join QS_ECoursePlanningClass QEPC On QEPC.PClassSNO=gaI.PClassSNO
				Group by gaI.PClassSNO,課程類別,Class1,QEPC.EPClassSNO
                Order by gaI.課程類別 DESC

            ";
        aDict.Add("@PersonSNO", PersonSNO);
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

        PersonSNO = Convert.ToString(Request.QueryString["sno"]);
        string PersonID = CovertPersonSNOtoID(PersonSNO);
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
                 where Ctype=1 and PersonID=@PersonID and IsUsed=0
                 Group by PersonID,EPClassSNO
                 ),
                   getEintergalE as (
                  Select PersonID,EPClassSNO,sum(Integral)Integral from QS_EIntegral
                 where Ctype=2 and PersonID=@PersonID and IsUsed=0
                 Group by PersonID,EPClassSNO
                 )
                 ,
                   getEintergalP as (
                  Select PersonID,EPClassSNO,sum(Integral)Integral from QS_EIntegral
                 where Ctype=3 and PersonID=@PersonID and IsUsed=0
                 Group by PersonID,EPClassSNO
                 )
                   ,
                   getEintergalC as (
                  Select PersonID,EPClassSNO,sum(Integral)Integral from QS_EIntegral
                 where Ctype=4 and PersonID=@PersonID and IsUsed=0
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
        aDict.Add("@PersonID", PersonID);
        aDict.Add("@EPClassSNO", EPclassSNO);

        objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            //rpt_EUploadintegralS.DataSource = objDT.DefaultView;
            //rpt_EUploadintegralS.DataBind();
        }

    }

    protected void EbindData_Upliadintegral(string EPclassSNO, string Ctype)
    {
        PersonSNO = Convert.ToString(Request.QueryString["sno"]);
        PersonID = Utility.ConvertPersonSNOToPersonID(PersonSNO);
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;

        string sql = @"            
                           
             Select * from QS_EIntegral where personID=@PersonID and EPClassSNO=@EPClassSNO
                     and Ctype In(1,2,3,4) and Isused=0
            
            ";
        aDict.Add("@PersonID", PersonID);

        aDict.Add("@EPclassSNO", EPclassSNO);
        objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            rpt_EUploadintegralDetail.DataSource = objDT.DefaultView;
            rpt_EUploadintegralDetail.DataBind();
        }


    }

    protected void EbindData_integral(string PclassSNO, string Class1, string Ctype)
    {

        PersonSNO = Convert.ToString(Request.QueryString["sno"]);
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
        aDict.Add("@PersonSNO", PersonSNO);
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

    protected void EbindData_Doneintegral(string PclassSNO, string Class1, string Ctype)
    {

        PersonSNO = Convert.ToString(Request.QueryString["sno"]);
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
                      where I.PersonSNO=@PersonSNO and I.isused=0
                      )
                      select Class1,CourseName,Ctype,CHour,CStartYear,CEndYear,X.Class,X.Type,
					  case O when  1 then'已取得' ELSE '未取得' END  積分
					  from getX X
                      FULL outer Join getY Y ON Y.CourseSNO=X.CourseSNO
					  Left JOIN QS_CoursePlanningRole CPR ON CPR.RClassSNO=X.PClassSNO
					  Left Join Role R ON R.RoleSNO=CPR.RoleSNO
                       where X.PClassSNO=@PClassSNO and X.Class=@Class and X.Type=@Type and O=1
                            
            ";
        aDict.Add("@PersonSNO", PersonSNO);
        aDict.Add("@PClassSNO", PclassSNO);
        aDict.Add("@Class", Class1);
        aDict.Add("@Type", Ctype);
        objDT = objDH.queryData(sql, aDict);


        Repeater4.DataSource = objDT.DefaultView;
        Repeater4.DataBind();



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
        aDict.Add("@PersonSNO", PersonSNO);
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



    protected void bindData_Detail(string PclassSNO)
    {

        PersonSNO = Convert.ToString(Request.QueryString["sno"]);
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
                where QI.PersonSNO=@PersonSNO and QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType
                
                ),getCoursePair as (
				Select Class1,Ctype,SUM(QC.Chour)CoursepairCount  from 
				QS_Course QC
				where PairCourseSNO <> 0 and PClassSNO=@PClassSNO
				Group by Class1,Ctype
				),AnalyticsPair as (
                    Select QC.Class1,QC.Ctype,QC.PairCourseSNO CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where PersonSNO=@PersonSNO and QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
                    Group by QC.Class1,QC.Ctype,QC.PairCourseSNO
                    INTERSECT 
                    Select QC.Class1,QC.Ctype,QI.CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where PersonSNO=@PersonSNO and QC.PClassSNO=@PClassSNO and QI.IsUsed = 0
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
        aDict.Add("@PersonSNO", PersonSNO);
        aDict.Add("@PClassSNO", PclassSNO);
        objDT = objDH.queryData(sql, aDict);

        if (objDT.Rows.Count > 0)
        {
            rpt_integralS.DataSource = objDT.DefaultView;
            rpt_integralS.DataBind();
        }

    }

    protected void bindSMKContract()
    {
        string PersonSNO = Convert.ToString(Request.QueryString["sno"]);
        string PrsnID = Utility.ConvertPersonSNOToPersonID(PersonSNO);
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = @"Select GC.SMKContractName,PC.HospID,O.OrganName,PC.PrsnID,PC.PrsnStartDate,PC.PrsnEndDate,PC.CouldTreat,
                        PC.CouldInstruct
                        from PrsnContract PC
                        Left Join GenSMKContract GC ON GC.SMKContractType=PC.SMKContractType
                        Left Join Organ O On O.OrganCode=PC.HospID
                        where PrsnID=@PrsnID";
        sql += @" order by PrsnStartDate desc,PrsnEndDate desc";
        aDict.Add("PrsnID", PrsnID);
        DataTable ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            Rpt_SMKContract.DataSource = ObjDT.DefaultView;
            Rpt_SMKContract.DataBind();
        }
        else
        {
            Tr11.Visible = false;
            lb_SMKContract.Visible = true;
        }
    }

    protected void bindPharmacy()
    {
        string PrsnID = Utility.ConvertPersonSNOToPersonID(PersonSNO);
        string PersonMPOrganCode = Utility.GetPersonMPOrganCode(PrsnID);
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = @"Select Distinct MP.OrganCode,MP.HospName,(Select Top(1) 1 from Pharmacy where Code=@PersonMPOrganCode) CheckOrg from PersonMP  MP
                        Left Join Pharmacy P ON MP.OrganCode=P.Code where MP.OrganCode=@PersonMPOrganCode and HospName <> '' ";
        aDict.Add("PersonMPOrganCode", PersonMPOrganCode);
        DataTable ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            Rpt_Pharmacy.DataSource = ObjDT.DefaultView;
            Rpt_Pharmacy.DataBind();
        }
        else
        {
            Tr6.Visible = false;
            lb_Pharmacy.Visible = true;
        }
    }

    protected void Btn_Print_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;
        string PersonSNO = Convert.ToString(Request.QueryString["sno"]);
        string CertSNO = (sender as LinkButton).CommandArgument;
        string sql = @"
            SELECT C.CertSNO,
                P.RoleSNO,
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
                QLU.ExamDate,QCPC.PClassSNO,
				CONVERT(varchar(100),C.CertPublicDate, 111) QCCertPublicDate,
				CONVERT(varchar(100), C.CertStartDate, 111) QCCertStartDate,
				CONVERT(varchar(100),C.CertEndDate, 111) QCCertEndDate
            FROM QS_Certificate C
				Left JOIN QS_CoursePlanningClass QCPC On QCPC.CTypeSNO=C.CTypeSNO
                Left JOIN QS_CertificateType CT on CT.CTypeSNO=C.CTypeSNO
                Left JOIN QS_CertificateUnit CU on CU.CUnitSNO=C.CUnitSNO
                Left Join Person P ON P.PersonID=C.PersonID
                Left Join [QS_LearningUpload] QLU On QLU.PersonSNO=P.PersonSNO
            WHERE
                 P.PersonSNO=@PersonSNO And C.CertSNO=@CertSNO
        ";
        aDict.Add("PersonSNO", PersonSNO);
        aDict.Add("CertSNO", CertSNO);
        objDT = objDH.queryData(sql, aDict);
        string CtypeSNO = objDT.Rows[0]["CtypeSNO"].ToString();
        string RoleSNO = objDT.Rows[0]["RoleSNO"].ToString();
        string PClassSNO = objDT.Rows[0]["PClassSNO"].ToString();
        string UserName = objDT.Rows[0]["PName"].ToString();
        string UserPersonID = objDT.Rows[0]["PersonID"].ToString();
        string UserPersonIDXXXX = UserPersonID.Substring(0, 6) + "****";
        if (CtypeSNO == "75")
        {
            Document doc = new Document(PageSize.A4, 10, 10, 10, 30); // 設定PageSize, Margin, left, right, top, bottom
            MemoryStream ms = new MemoryStream();
            PdfWriter pw = PdfWriter.GetInstance(doc, ms);

            ////    字型設定
            // 在PDF檔案內容中要顯示中文，最重要的是字型設定，如果沒有正確設定中文字型，會造成中文無法顯示的問題。
            // 首先設定基本字型：kaiu.ttf 是作業系統系統提供的標楷體字型，IDENTITY_H 是指編碼(The Unicode encoding with horizontal writing)，及是否要將字型嵌入PDF 檔中。
            // 再來針對基本字型做變化，例如Font Size、粗體斜體以及顏色等。當然你也可以採用其他中文字體字型。
            BaseFont bfChinese = BaseFont.CreateFont("C:\\Windows\\Fonts\\kaiu.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font ChFont_Header = new Font(bfChinese, 25);
            Font ChFont_CoreCertificate = new Font(bfChinese, 22, Font.NORMAL);
            Font ChFontTitle = new Font(bfChinese, 18);
            Font ChFont = new Font(bfChinese, 12);
            BaseColor myColorGreen = WebColors.GetRGBColor("#F2FFF2");
            BaseColor myColorOrange = WebColors.GetRGBColor("#FFE5B5");
            Font ChFont_Note = new Font(bfChinese, 8);
            Font ChFont_green = new Font(bfChinese, 40, Font.NORMAL, BaseColor.GREEN);
            Font ChFont_msg = new Font(bfChinese, 12, Font.ITALIC, BaseColor.RED);
            Font ChFont_Title = new Font(bfChinese, 20,Font.BOLD);
            Font ChFont_Date = new Font(bfChinese, 10);
            // 開啟檔案寫入內容後，將檔案關閉。
            doc.Open();
            // 產生表格 -- START

            // 塞入資料 -- START
            // 設定表頭
            //PdfPCell header = new PdfPCell(new Phrase("戒菸服務人員資格證明", ChFont));
            //header.Colspan = 4;
            //header.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
            //header.Padding = 5;
            //header.Border = 0;
            //header.PaddingBottom = 10;
            //pt.AddCell(header);
            Chunk c_51_PName = new Chunk("戒菸服務人員資格證明", ChFont_Title);
            Phrase p_51_PName = new Phrase(c_51_PName);
            Paragraph pg_51_PName = new Paragraph(p_51_PName);
            pg_51_PName.Alignment = Element.ALIGN_CENTER;
            ColumnText ct_51_PName = new ColumnText(pw.DirectContent);
            ct_51_PName.SetSimpleColumn(new Rectangle(600, 780));
            ct_51_PName.AddElement(pg_51_PName);
            ct_51_PName.Go();
            if(RoleSNO == "10" || RoleSNO == "11")
            {
                Chunk c_51_Doc = new Chunk("■醫師", ChFont);
                Phrase p_51_Doc = new Phrase(c_51_Doc);
                Paragraph pg_51_Doc = new Paragraph(p_51_Doc);
                pg_51_Doc.Alignment = Element.ALIGN_CENTER;
                ColumnText ct_51_Doc = new ColumnText(pw.DirectContent);
                ct_51_Doc.SetSimpleColumn(new Rectangle(900, 780));
                ct_51_Doc.AddElement(pg_51_Doc);
                ct_51_Doc.Go();
                Chunk c_51_NotDoc = new Chunk("□非醫師", ChFont);
                Phrase p_51_NotDoc = new Phrase(c_51_NotDoc);
                Paragraph pg_51_NotDoc = new Paragraph(p_51_NotDoc);
                pg_51_NotDoc.Alignment = Element.ALIGN_CENTER;
                ColumnText ct_51_NotDoc = new ColumnText(pw.DirectContent);
                ct_51_NotDoc.SetSimpleColumn(new Rectangle(913, 765));
                ct_51_NotDoc.AddElement(pg_51_NotDoc);
                ct_51_NotDoc.Go();
            }
            else
            {
                Chunk c_51_Doc = new Chunk("□醫師", ChFont);
                Phrase p_51_Doc = new Phrase(c_51_Doc);
                Paragraph pg_51_Doc = new Paragraph(p_51_Doc);
                pg_51_Doc.Alignment = Element.ALIGN_CENTER;
                ColumnText ct_51_Doc = new ColumnText(pw.DirectContent);
                ct_51_Doc.SetSimpleColumn(new Rectangle(900, 780));
                ct_51_Doc.AddElement(pg_51_Doc);
                ct_51_Doc.Go();
                Chunk c_51_NotDoc = new Chunk("■非醫師", ChFont);
                Phrase p_51_NotDoc = new Phrase(c_51_NotDoc);
                Paragraph pg_51_NotDoc = new Paragraph(p_51_NotDoc);
                pg_51_NotDoc.Alignment = Element.ALIGN_CENTER;
                ColumnText ct_51_NotDoc = new ColumnText(pw.DirectContent);
                ct_51_NotDoc.SetSimpleColumn(new Rectangle(913, 765));
                ct_51_NotDoc.AddElement(pg_51_NotDoc);
                ct_51_NotDoc.Go();
            }
            Chunk c_51_date = new Chunk("列印日期:"+DateTime.Today.ToString("yyyy/MM/dd"), ChFont_Date);
            Phrase p_51_date = new Phrase(c_51_date);
            Paragraph pg_51_date = new Paragraph(p_51_date);
            pg_51_date.Alignment = Element.ALIGN_CENTER;
            ColumnText ct_51_date = new ColumnText(pw.DirectContent);
            ct_51_PName.SetSimpleColumn(new Rectangle(1000, 800));
            ct_51_PName.AddElement(pg_51_date);
            ct_51_PName.Go();
            string imageFilePath = "";
            imageFilePath = Server.MapPath("../Images/PDDIlogo.png");
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);
            jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
            jpg.ScalePercent(60f);
            jpg.SetAbsolutePosition(50, 750);
            doc.Add(jpg);
            // 建立4個欄位表格之相對寬度
            PdfPTable pt = new PdfPTable(new float[] { 1, 2, 3, 2, 2, 2 });
            // 表格總寬
            pt.TotalWidth = 500f;
            pt.LockedWidth = true;
            PdfPCell PName = new PdfPCell(new Phrase("姓名：" + UserName+ "  身分證字號：" + UserPersonIDXXXX, ChFont));
            PName.BorderWidthTop = 0;
            PName.BorderWidthLeft = 0;
            PName.BorderWidthRight = 0;
            PName.PaddingBottom = 10;
            PName.Colspan = 6;
            pt.AddCell(PName);
            PdfPCell sno = new PdfPCell(new Phrase("序號", ChFont));
            sno.BorderWidthTop = 0;
            sno.BorderWidthLeft = 0;
            sno.BorderWidthRight = 0;
            sno.PaddingBottom = 10;
            sno.Colspan = 1;
            sno.HorizontalAlignment = Element.ALIGN_CENTER;
            pt.AddCell(sno);
            PdfPCell PClass = new PdfPCell(new Phrase("課程類別", ChFont));
            PClass.BorderWidthTop = 0;
            PClass.BorderWidthLeft = 0;
            PClass.BorderWidthRight = 0;
            PClass.PaddingBottom = 10;
            PClass.Colspan = 1;
            PClass.HorizontalAlignment = Element.ALIGN_CENTER;
            pt.AddCell(PClass);
            PdfPCell ClassName = new PdfPCell(new Phrase("課程名稱", ChFont));
            ClassName.BorderWidthTop = 0;
            ClassName.BorderWidthLeft = 0;
            ClassName.BorderWidthRight = 0;
            ClassName.PaddingBottom = 10;
            ClassName.Colspan = 2;
            pt.AddCell(ClassName);
            PdfPCell PassDate = new PdfPCell(new Phrase("通過日期", ChFont));
            PassDate.BorderWidthTop = 0;
            PassDate.BorderWidthLeft = 0;
            PassDate.BorderWidthRight = 0;
            PassDate.PaddingBottom = 10;
            PassDate.Colspan = 1;
            PassDate.HorizontalAlignment = Element.ALIGN_CENTER;
            pt.AddCell(PassDate);
            PdfPCell PassHour = new PdfPCell(new Phrase("通過認證時數", ChFont));
            PassHour.BorderWidthTop = 0;
            PassHour.BorderWidthLeft = 0;
            PassHour.BorderWidthRight = 0;
            PassHour.PaddingBottom = 10;
            PassHour.Colspan = 1;
            PassHour.HorizontalAlignment = Element.ALIGN_CENTER;
            pt.AddCell(PassHour);
            DataTable ScoreDetail = getCertificateDetail(PersonSNO, RoleSNO);
            DataTable ScoreEIDetail = getCertificateEIDetail(PersonSNO, RoleSNO);
            DataTable SumHours = getSumHours(PersonSNO, RoleSNO);
            int seq = 1;
            if (ScoreDetail != null)
            {
                for (int i = 0; i < ScoreDetail.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        #region 無顏色
                        PdfPCell Num = new PdfPCell(new Phrase((seq).ToString(), ChFont));
                        Num.Colspan = 1;
                        Num.HorizontalAlignment = Element.ALIGN_CENTER;
                        Num.BorderWidthTop = 0;
                        Num.BorderWidthLeft = 0;
                        Num.BorderWidthRight = 0;
                        Num.BorderWidthBottom = 0; 
                        pt.AddCell(Num);
                        PdfPCell Type = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["課程規劃"].ToString(), ChFont));
                        Type.Colspan = 1;
                        Type.HorizontalAlignment = Element.ALIGN_CENTER;
                        Type.BorderWidthTop = 0;
                        Type.BorderWidthLeft = 0;
                        Type.BorderWidthRight = 0;
                        Type.BorderWidthBottom = 0;
                        pt.AddCell(Type);
                        PdfPCell CourseName = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["CourseName"].ToString(), ChFont));
                        CourseName.Colspan = 2;
                        CourseName.HorizontalAlignment = Element.ALIGN_LEFT;
                        CourseName.BorderWidthTop = 0;
                        CourseName.BorderWidthLeft = 0;
                        CourseName.BorderWidthRight = 0;
                        CourseName.BorderWidthBottom = 0;
                        CourseName.PaddingBottom = 5;
                        pt.AddCell(CourseName);
                        PdfPCell Date = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["上傳時間"].ToString(), ChFont));
                        Date.Colspan = 1;
                        Date.HorizontalAlignment = Element.ALIGN_CENTER;
                        Date.BorderWidthTop = 0;
                        Date.BorderWidthLeft = 0;
                        Date.BorderWidthRight = 0;
                        Date.BorderWidthBottom = 0;
                        pt.AddCell(Date);
                        PdfPCell QI = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["積分"].ToString(), ChFont));
                        QI.Colspan = 1;
                        QI.HorizontalAlignment = Element.ALIGN_CENTER;
                        QI.BorderWidthTop = 0;
                        QI.BorderWidthLeft = 0;
                        QI.BorderWidthRight = 0;
                        QI.BorderWidthBottom = 0;
                        pt.AddCell(QI);
                        #endregion
                        seq += 1;
                    }
                    else
                    {
                        #region 有顏色
                        PdfPCell Num = new PdfPCell(new Phrase((seq).ToString(), ChFont));
                        Num.Colspan = 1;
                        Num.HorizontalAlignment = Element.ALIGN_CENTER;
                        Num.BorderWidthTop = 0;
                        Num.BorderWidthLeft = 0;
                        Num.BorderWidthRight = 0;
                        Num.BorderWidthBottom = 0;
                        Num.BackgroundColor = myColorGreen;
                        pt.AddCell(Num);
                        PdfPCell Type = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["課程規劃"].ToString(), ChFont));
                        Type.Colspan = 1;
                        Type.HorizontalAlignment = Element.ALIGN_CENTER;
                        Type.BorderWidthTop = 0;
                        Type.BorderWidthLeft = 0;
                        Type.BorderWidthRight = 0;
                        Type.BorderWidthBottom = 0;
                        Type.BackgroundColor = myColorGreen;
                        pt.AddCell(Type);
                        PdfPCell CourseName = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["CourseName"].ToString(), ChFont));
                        CourseName.Colspan = 2;
                        CourseName.HorizontalAlignment = Element.ALIGN_LEFT;
                        CourseName.BorderWidthTop = 0;
                        CourseName.BorderWidthLeft = 0;
                        CourseName.BorderWidthRight = 0;
                        CourseName.BorderWidthBottom = 0;
                        CourseName.PaddingBottom = 5;
                        CourseName.BackgroundColor = myColorGreen;
                        pt.AddCell(CourseName);
                        PdfPCell Date = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["上傳時間"].ToString(), ChFont));
                        Date.Colspan = 1;
                        Date.HorizontalAlignment = Element.ALIGN_CENTER;
                        Date.BorderWidthTop = 0;
                        Date.BorderWidthLeft = 0;
                        Date.BorderWidthRight = 0;
                        Date.BorderWidthBottom = 0;
                        Date.BackgroundColor = myColorGreen;
                        pt.AddCell(Date);
                        PdfPCell QI = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["積分"].ToString(), ChFont));
                        QI.Colspan = 1;
                        QI.HorizontalAlignment = Element.ALIGN_CENTER;
                        QI.BorderWidthTop = 0;
                        QI.BorderWidthLeft = 0;
                        QI.BorderWidthRight = 0;
                        QI.BorderWidthBottom = 0;
                        QI.BackgroundColor = myColorGreen;
                        pt.AddCell(QI);
                        #endregion
                        seq += 1;
                    }

                }
            }
            if (ScoreEIDetail != null)
            {
                for (int i = 0; i < ScoreEIDetail.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        #region 無顏色
                        PdfPCell Num = new PdfPCell(new Phrase((seq).ToString(), ChFont));
                        Num.Colspan = 1;
                        Num.HorizontalAlignment = Element.ALIGN_CENTER;
                        Num.BorderWidthTop = 0;
                        Num.BorderWidthLeft = 0;
                        Num.BorderWidthRight = 0;
                        Num.BorderWidthBottom = 0;
                        pt.AddCell(Num);
                        PdfPCell Type = new PdfPCell(new Phrase("繼續教育", ChFont));
                        Type.Colspan = 1;
                        Type.HorizontalAlignment = Element.ALIGN_CENTER;
                        Type.BorderWidthTop = 0;
                        Type.BorderWidthLeft = 0;
                        Type.BorderWidthRight = 0;
                        Type.BorderWidthBottom = 0;
                        pt.AddCell(Type);
                        PdfPCell CourseName = new PdfPCell(new Phrase(ScoreEIDetail.Rows[i]["CourseName"].ToString(), ChFont));
                        CourseName.Colspan = 2;
                        CourseName.HorizontalAlignment = Element.ALIGN_LEFT;
                        CourseName.BorderWidthTop = 0;
                        CourseName.BorderWidthLeft = 0;
                        CourseName.BorderWidthRight = 0;
                        CourseName.BorderWidthBottom = 0;
                        CourseName.PaddingBottom = 5;
                        pt.AddCell(CourseName);
                        PdfPCell Date = new PdfPCell(new Phrase(ScoreEIDetail.Rows[i]["CDate"].ToString(), ChFont));
                        Date.Colspan = 1;
                        Date.HorizontalAlignment = Element.ALIGN_CENTER;
                        Date.BorderWidthTop = 0;
                        Date.BorderWidthLeft = 0;
                        Date.BorderWidthRight = 0;
                        Date.BorderWidthBottom = 0;
                        pt.AddCell(Date);
                        PdfPCell QI = new PdfPCell(new Phrase(ScoreEIDetail.Rows[i]["Integral"].ToString(), ChFont));
                        QI.Colspan = 1;
                        QI.HorizontalAlignment = Element.ALIGN_CENTER;
                        QI.BorderWidthTop = 0;
                        QI.BorderWidthLeft = 0;
                        QI.BorderWidthRight = 0;
                        QI.BorderWidthBottom = 0;
                        pt.AddCell(QI);
                        #endregion
                        seq += 1;
                    }
                    else
                    {
                        #region 有顏色
                        PdfPCell Num = new PdfPCell(new Phrase((seq).ToString(), ChFont));
                        Num.Colspan = 1;
                        Num.HorizontalAlignment = Element.ALIGN_CENTER;
                        Num.BorderWidthTop = 0;
                        Num.BorderWidthLeft = 0;
                        Num.BorderWidthRight = 0;
                        Num.BorderWidthBottom = 0;
                        Num.BackgroundColor = myColorGreen;
                        pt.AddCell(Num);
                        PdfPCell Type = new PdfPCell(new Phrase("繼續教育", ChFont));
                        Type.Colspan = 1;
                        Type.HorizontalAlignment = Element.ALIGN_CENTER;
                        Type.BorderWidthTop = 0;
                        Type.BorderWidthLeft = 0;
                        Type.BorderWidthRight = 0;
                        Type.BorderWidthBottom = 0;
                        Type.BackgroundColor = myColorGreen;
                        pt.AddCell(Type);
                        PdfPCell CourseName = new PdfPCell(new Phrase(ScoreEIDetail.Rows[i]["CourseName"].ToString(), ChFont));
                        CourseName.Colspan = 2;
                        CourseName.HorizontalAlignment = Element.ALIGN_LEFT;
                        CourseName.BorderWidthTop = 0;
                        CourseName.BorderWidthLeft = 0;
                        CourseName.BorderWidthRight = 0;
                        CourseName.BorderWidthBottom = 0;
                        CourseName.PaddingBottom = 5;
                        CourseName.BackgroundColor = myColorGreen;
                        pt.AddCell(CourseName);
                        PdfPCell Date = new PdfPCell(new Phrase(ScoreEIDetail.Rows[i]["CDate"].ToString(), ChFont));
                        Date.Colspan = 1;
                        Date.HorizontalAlignment = Element.ALIGN_CENTER;
                        Date.BorderWidthTop = 0;
                        Date.BorderWidthLeft = 0;
                        Date.BorderWidthRight = 0;
                        Date.BorderWidthBottom = 0;
                        Date.BackgroundColor = myColorGreen;
                        pt.AddCell(Date);
                        PdfPCell QI = new PdfPCell(new Phrase(ScoreEIDetail.Rows[i]["Integral"].ToString(), ChFont));
                        QI.Colspan = 1;
                        QI.HorizontalAlignment = Element.ALIGN_CENTER;
                        QI.BorderWidthTop = 0;
                        QI.BorderWidthLeft = 0;
                        QI.BorderWidthRight = 0;
                        QI.BorderWidthBottom = 0;
                        QI.BackgroundColor = myColorGreen;
                        pt.AddCell(QI);
                        #endregion
                        seq += 1;
                    }

                }
            }
            PdfPCell TotolHour = new PdfPCell(new Phrase("總計時數", ChFont));
            //TotolHour.BorderWidthBottom = 0;
            TotolHour.BorderWidthLeft = 0;
            TotolHour.BorderWidthRight = 0;
            TotolHour.PaddingBottom = 10;
            TotolHour.Colspan = 5;
            TotolHour.BackgroundColor = myColorOrange;
            TotolHour.HorizontalAlignment = Element.ALIGN_LEFT;
            pt.AddCell(TotolHour);
            PdfPCell HourS = new PdfPCell(new Phrase(SumHours.Rows[0]["積分"].ToString(), ChFont));
            //HourS.BorderWidthBottom = 0;
            HourS.BorderWidthLeft = 0;
            HourS.BorderWidthRight = 0;
            HourS.PaddingBottom = 10;
            HourS.Colspan = 1;
            HourS.BackgroundColor = myColorOrange;
            HourS.HorizontalAlignment = Element.ALIGN_CENTER;
            pt.AddCell(HourS);
            PdfPCell QCStartDate = new PdfPCell(new Phrase("戒菸服務資格起日:"+ objDT.Rows[0]["QCCertPublicDate"].ToString(), ChFont));
            QCStartDate.BorderWidthBottom = 0;
            QCStartDate.BorderWidthTop = 0;
            QCStartDate.BorderWidthLeft = 0;
            QCStartDate.BorderWidthRight = 0;
            QCStartDate.PaddingBottom = 5;
            QCStartDate.Colspan = 6;
            QCStartDate.HorizontalAlignment = Element.ALIGN_LEFT;
            pt.AddCell(QCStartDate);
            PdfPCell QCEndDate = new PdfPCell(new Phrase("戒菸服務資格迄日:"+ objDT.Rows[0]["QCCertEndDate"].ToString(), ChFont));
            QCEndDate.BorderWidthTop = 0;
            QCEndDate.BorderWidthLeft = 0;
            QCEndDate.BorderWidthRight = 0;
            QCEndDate.PaddingBottom = 5;
            QCEndDate.Colspan = 6;
            QCEndDate.HorizontalAlignment = Element.ALIGN_LEFT;
            pt.AddCell(QCEndDate);
            PdfPCell NoteOne = new PdfPCell(new Phrase("備註1:", ChFont_Note));
            NoteOne.BorderWidthTop = 0;
            NoteOne.BorderWidthLeft = 0;
            NoteOne.BorderWidthRight = 0;
            NoteOne.BorderWidthBottom = 0;
            NoteOne.PaddingBottom = 2;
            NoteOne.Colspan = 1;
            NoteOne.HorizontalAlignment = Element.ALIGN_RIGHT;
            pt.AddCell(NoteOne);
            PdfPCell NoteOneContent = new PdfPCell(new Phrase("以上資料由醫事人員戒菸服務訓練系統下載列印，網址:https://quitsmoking.hpa.gov.tw 。", ChFont_Note));
            NoteOneContent.BorderWidthTop = 0;
            NoteOneContent.BorderWidthLeft = 0;
            NoteOneContent.BorderWidthRight = 0;
            NoteOneContent.BorderWidthBottom = 0;
            NoteOneContent.PaddingBottom = 2;
            NoteOneContent.Colspan = 5;
            NoteOneContent.HorizontalAlignment = Element.ALIGN_LEFT;
            pt.AddCell(NoteOneContent);
            PdfPCell NoteTwo = new PdfPCell(new Phrase("備註2:", ChFont_Note));
            NoteTwo.BorderWidthTop = 0;
            NoteTwo.BorderWidthLeft = 0;
            NoteTwo.BorderWidthRight = 0;
            NoteTwo.BorderWidthBottom = 0;
            NoteTwo.PaddingBottom = 2;
            NoteTwo.Colspan = 1;
            NoteTwo.HorizontalAlignment = Element.ALIGN_RIGHT;
            pt.AddCell(NoteTwo);
            PdfPCell NoteTwoConten = new PdfPCell(new Phrase("健保特約醫事機構，其執業登記之醫事人員或公共衛生師具效期內之戒菸服務人員資格者，得依該機構及服務人員之類別，申請為衛生福利部國民健康署之戒菸服務特約機構，並自核定生效日起，始得申請戒菸服務費用補助。", ChFont_Note));
            NoteTwoConten.BorderWidthTop = 0;
            NoteTwoConten.BorderWidthLeft = 0;
            NoteTwoConten.BorderWidthRight = 0;
            NoteTwoConten.BorderWidthBottom = 0;
            NoteTwoConten.PaddingBottom = 2;
            NoteTwoConten.Colspan = 5;
            NoteTwoConten.HorizontalAlignment = Element.ALIGN_LEFT;
            pt.AddCell(NoteTwoConten);
            PdfContentByte cb = pw.DirectContent;
            pt.WriteSelectedRows(0, -1, 50, 700, cb);
            //doc.Add(pt);
            // 塞入資料 -- END
            doc.Close();

            // 在Client端顯示PDF檔，讓USER可以下載
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + UserName + "-證書積分列印" + DateTime.Now.ToShortDateString() + ".pdf");
            Response.ContentType = "application/octet-stream";
            //Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);

            var stream = new MemoryStream();

            using (SyncfusionHelper.IHelper IH = new SyncfusionHelper.Helper())
            {     // 讀入方法一：byte    // 
                  //
                var byByte = IH.ByByte(ms.ToArray());
                //byte[] myByteArray = new byte[];
                stream.Write(byByte, 0, byByte.Length);

            }
            Response.OutputStream.Write(stream.GetBuffer(), 0, stream.GetBuffer().Length);
            stream.Dispose();

            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            Response.End();
        }

        if (CtypeSNO == "10" || CtypeSNO == "11")
        {
            Response.Redirect("./ScorePrintAjax.aspx?PClassSNO=" + PClassSNO + "&PersonSNO=" + PersonSNO + "&Word=0&CoreCertificate=1");
            return;
        }

        if (CtypeSNO == "51" || CtypeSNO == "52" || CtypeSNO == "53" || CtypeSNO == "54" || CtypeSNO == "55" || CtypeSNO == "8")
        {

            string PName = objDT.Rows[0]["PName"].ToString();
            string PersonID = objDT.Rows[0]["PersonID"].ToString();
            string CertID = objDT.Rows[0]["CertID"].ToString();
            DateTime CertPublicDate = Convert.ToDateTime(objDT.Rows[0]["CertPublicDate"]);
            DateTime CertStartDate = Convert.ToDateTime(objDT.Rows[0]["CertStartDate"]).AddYears(-1911);
            DateTime CertEndDate = Convert.ToDateTime(objDT.Rows[0]["CertEndDate"]).AddYears(-1911);
            DateTime NewRole11ChairMan = DateTime.Parse("2021/08/01").AddYears(-1911);
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

                                imageFilePath_1 = Server.MapPath("../Images/Role10Mark.jpg");
                                imageFilePath_2 = Server.MapPath("../Images/Role10NewChairman.jpg");
                                break;
                            }
                            if (CtypeSNO == "51")
                            {
                                imageFilePath_1 = Server.MapPath("../Images/Role13Mark.png");
                                imageFilePath_2 = Server.MapPath("../Images/老師印章.png");
                                break;
                            }
                            if (CtypeSNO == "55")
                            {
                                imageFilePath = Server.MapPath("../Images/Role11Certificate_2.jpg");
                                imageFilePath_1 = Server.MapPath("../Images/Role11Mark.png");
                                imageFilePath_2 = Server.MapPath("../Images/老師印章11.png");
                                break;
                            }
                            break;
                        case "11":
                            if (CertStartDate.AddYears(1911) >= NewRole11ChairMan.AddYears(1911))
                            {
                                imageFilePath_1 = Server.MapPath("../Images/Role11Mark.png");
                                imageFilePath_2 = Server.MapPath("../Images/NewChairman.png");
                            }
                            else
                            {
                                imageFilePath_1 = Server.MapPath("../Images/Role11Mark.png");
                                imageFilePath_2 = Server.MapPath("../Images/老師印章11.png");
                            }

                            break;
                        case "12":
                            imageFilePath_1 = Server.MapPath("../Images/Role12Mark.png");
                            imageFilePath_2 = Server.MapPath("../Images/004.png");

                            break;
                        case "13":
                            imageFilePath_1 = Server.MapPath("../Images/Role13Mark.png");
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
                                JPG2.SetAbsolutePosition(260, 160);
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
                            JPG2.SetAbsolutePosition(295, 150);
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
                            if (CtypeSNO == "51")
                            {
                                Chunk c_51_PName = new Chunk(PName, ChFont_Pname);
                                Phrase p_51_PName = new Phrase(c_51_PName);
                                Paragraph pg_51_PName = new Paragraph(p_51_PName);
                                pg_51_PName.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct_51_PName = new ColumnText(writer.DirectContent);
                                ct_51_PName.SetSimpleColumn(new Rectangle(300, 572));
                                ct_51_PName.AddElement(pg_51_PName);
                                ct_51_PName.Go();

                                Chunk c51 = new Chunk(PersonID, ChFont_Pname);
                                Phrase p51 = new Phrase(c51);
                                Paragraph pg51 = new Paragraph(p51);
                                pg51.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct51 = new ColumnText(writer.DirectContent);
                                ct51.SetSimpleColumn(new Rectangle(880, 572));
                                ct51.AddElement(pg51);
                                ct51.Go();

                                Chunk C51_CertID = new Chunk(CertID, ChFont_CertID);
                                Phrase p51CertID = new Phrase(C51_CertID);
                                Paragraph pg51CertID = new Paragraph(p51CertID);
                                pg51CertID.Alignment = Element.ALIGN_CENTER;
                                ColumnText c51t2 = new ColumnText(writer.DirectContent);
                                c51t2.SetSimpleColumn(new Rectangle(940, 613));
                                c51t2.AddElement(pg51CertID);
                                c51t2.Go();

                                Chunk c512 = new Chunk(PSex, ChFont_PSex);
                                Phrase p512 = new Phrase(c512);
                                Paragraph p51g2 = new Paragraph(p512);
                                p51g2.Alignment = Element.ALIGN_CENTER;
                                ColumnText c51t3 = new ColumnText(writer.DirectContent);
                                c51t3.SetSimpleColumn(new Rectangle(630, 572));
                                c51t3.AddElement(p51g2);
                                c51t3.Go();
                                break;
                            }
                            else
                            {
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
                            }

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
                        if (CtypeSNO == "51")
                        {
                            Chunk c3 = new Chunk(StartDateY, ChFont);
                            Phrase p3 = new Phrase(c3);
                            Paragraph pg3 = new Paragraph(p3);
                            pg3.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct4 = new ColumnText(writer.DirectContent);
                            ct4.SetSimpleColumn(new Rectangle(650, 405));
                            ct4.AddElement(pg3);
                            ct4.Go();

                            Chunk c4 = new Chunk(StartDateM, ChFont);
                            Phrase p4 = new Phrase(c4);
                            Paragraph pg4 = new Paragraph(p4);
                            pg4.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct5 = new ColumnText(writer.DirectContent);
                            ct5.SetSimpleColumn(new Rectangle(780, 405));
                            ct5.AddElement(pg4);
                            ct5.Go();

                            Chunk c5 = new Chunk(StartDateD, ChFont);
                            Phrase p5 = new Phrase(c5);
                            Paragraph pg5 = new Paragraph(p5);
                            pg5.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct6 = new ColumnText(writer.DirectContent);
                            ct6.SetSimpleColumn(new Rectangle(890, 405));
                            ct6.AddElement(pg5);
                            ct6.Go();

                            Chunk c6 = new Chunk(EndDateY, ChFont);
                            Phrase p6 = new Phrase(c6);
                            Paragraph pg6 = new Paragraph(p6);
                            pg6.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct7 = new ColumnText(writer.DirectContent);
                            ct7.SetSimpleColumn(new Rectangle(650, 365));
                            ct7.AddElement(pg6);
                            ct7.Go();

                            Chunk c7 = new Chunk(EndDateM, ChFont);
                            Phrase p7 = new Phrase(c7);
                            Paragraph pg7 = new Paragraph(p7);
                            pg7.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct8 = new ColumnText(writer.DirectContent);
                            ct8.SetSimpleColumn(new Rectangle(780, 365));
                            ct8.AddElement(pg7);
                            ct8.Go();

                            Chunk c8 = new Chunk(EndDateD, ChFont);
                            Phrase p8 = new Phrase(c8);
                            Paragraph pg8 = new Paragraph(p8);
                            pg8.Alignment = Element.ALIGN_CENTER;
                            ColumnText ct9 = new ColumnText(writer.DirectContent);
                            ct9.SetSimpleColumn(new Rectangle(890, 365));
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
                            if (CtypeSNO == "51")
                            {
                                Chunk c9_13_Year51 = new Chunk(StartDateY, ChFont);
                                Phrase p9_13_Year51 = new Phrase(c9_13_Year51);
                                Paragraph pg9_13_Year51 = new Paragraph(p9_13_Year51);
                                pg9_13_Year51.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct10_13_Year51 = new ColumnText(writer.DirectContent);
                                ct10_13_Year51.SetSimpleColumn(new Rectangle(535, 107));
                                ct10_13_Year51.AddElement(pg9_13_Year51);
                                ct10_13_Year51.Go();

                                Chunk c10_13_Year51 = new Chunk(StartDateM, ChFont);
                                Phrase p10_13_Year51 = new Phrase(c10_13_Year51);
                                Paragraph pg10_13_Year51 = new Paragraph(p10_13_Year51);
                                pg10_13_Year51.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct11_13_Year51 = new ColumnText(writer.DirectContent);
                                ct11_13_Year51.SetSimpleColumn(new Rectangle(680, 107));
                                ct11_13_Year51.AddElement(pg10_13_Year51);
                                ct11_13_Year51.Go();

                                Chunk c11_13_Year51 = new Chunk(StartDateD, ChFont);
                                Phrase p11_13_Year51 = new Phrase(c11_13_Year51);
                                Paragraph pg11_13_Year51 = new Paragraph(p11_13_Year51);
                                pg11_13_Year51.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct12_13_Year51 = new ColumnText(writer.DirectContent);
                                ct12_13_Year51.SetSimpleColumn(new Rectangle(805, 107));
                                ct12_13_Year51.AddElement(pg11_13_Year51);
                                ct12_13_Year51.Go();
                                break;
                            }
                            else
                            {
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
                            }

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
                    //Response.AddHeader("Transfer-Encoding", "identity");
                    Response.AddHeader("content-disposition", "attachment;filename=CertificatePring.pdf");
                    Response.ContentType = "application/octet-stream";
                    //Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);

                    var stream = new MemoryStream();

                    using (SyncfusionHelper.IHelper IH = new SyncfusionHelper.Helper())
                    {     // 讀入方法一：byte    // 
                          //
                        var byByte = IH.ByByte(ms.ToArray());
                        //byte[] myByteArray = new byte[];
                        stream.Write(byByte, 0, byByte.Length);

                    }
                    Response.OutputStream.Write(stream.GetBuffer(), 0, stream.GetBuffer().Length);
                    stream.Dispose();

                    Response.OutputStream.Flush();
                    Response.OutputStream.Close();
                    Response.Flush();
                    Response.End();
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

    private PdfPCell SetCell(string msg, Font font, FieldPositioningEvents events = null, bool border = false, int height = 20)
    {
        PdfPCell cell = new PdfPCell(new Phrase(msg, font));
        if (events != null) cell.CellEvent = events;
        if (!border) cell.Border = 0;
        cell.FixedHeight = height;

        return cell;
    }
    public static DataTable getCertificateDetail(string PersonSNO, string RoleSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable ObjDT;
        string sql = @"Select case when QECPC.EPClassSNO is null then '新訓' else '繼續教育' end 課程規劃,C.MVal+'課程'　'課程類別',QC.CourseName,C1.MVal '授課方式',CONVERT(DECIMAL(7,1),QC.CHour ) '積分',convert(varchar, QI.CreateDT, 111)　'上傳時間',case when C2.MVal  is null then '線上' else C2.MVal end '取得方式',case when QL.Unit  is null then '-' else QL.Unit end '開課單位',case when QL.ExamDate  is null then  convert(varchar, QC.CreateDT, 111) else convert(varchar, QL.ExamDate, 111) end '上課日期'　 from QS_Integral QI
                    Left Join QS_LearningUpload QL on QI.CourseSNO=QL.CourseSNO and QI.PersonSNO=QL.PersonSNO
                    Left Join QS_Course QC ON QC.CourseSNO=QI.CourseSNO
					left join QS_CoursePlanningClass QCPC on QC.PClassSNO=QCPC.PClassSNO
					left join QS_ECoursePlanningClass QECPC on QCPC.PClassSNO=QECPC.PClassSNO
                    Left Join Config C On QC.Class1=C.PVal and C.PGroup='CourseClass1'
                    Left Join Config C1 On QC.CType=C1.PVal and C1.PGroup='CourseCType'
					Left Join Config C2 On QL.Ctype=C2.PVal and C2.PGroup='CourseCType'
                    where QI.PersonSNO=@PersonSNO and QI.isused=1 order by 課程規劃";
        aDict.Add("PersonSNO", PersonSNO);
        ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT;
        }
        else
        {
            return null;
        }
    }
    public static DataTable getCertificateEIDetail(string PersonSNO, string RoleSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable ObjDT;
        string sql = @"SELECT convert(varchar, [CDate], 111)CDate,CONVERT(DECIMAL(7,1),[Integral] )Integral,[CType],[IsUsed],[CourseName]
  FROM [New_QSMS].[dbo].[QS_EIntegral] QEI
  left join Person P on qei.PersonID=P.PersonID
  where p.PersonSNO=@PersonSNO  and CDate>'2022-10-31' and QEI.isused=1 order by [CDate]";
        aDict.Add("PersonSNO", PersonSNO);
        ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT;
        }
        else
        {
            return null;
        }
    }
    public static DataTable getSumHours(string PersonSNO, string RoleSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable ObjDT;
        string sql = @"with QIhour as(Select  qi.PersonSNO,CONVERT(DECIMAL(7,1),sum(QC.CHour) )  '積分' from QS_Integral QI
                    Left Join QS_LearningUpload QL on QI.CourseSNO=QL.CourseSNO and QI.PersonSNO=QL.PersonSNO
                    Left Join QS_Course QC ON QC.CourseSNO=QI.CourseSNO
                    Left Join Config C On QC.Class1=C.PVal and C.PGroup='CourseClass1'
                    Left Join Config C1 On QC.CType=C1.PVal and C1.PGroup='CourseCType'
					Left Join Config C2 On QL.Ctype=C2.PVal and C2.PGroup='CourseCType'
                    where QI.PersonSNO=@PersonSNO and QI.isused=1";
        aDict.Add("PersonSNO", PersonSNO);
        if (RoleSNO == "10" || RoleSNO == "11")
        {
            sql += @" and QC.PClassSNO in('1','2','5','9','23')";
        }
        else if (RoleSNO == "12")
        {
            sql += @" and QC.PClassSNO in('1','3','9','23')";
        }
        else if (RoleSNO == "13")
        {
            sql += @" and QC.PClassSNO in('1','4','9','23')";
        }
        sql += @"group by qi.PersonSNO),
  EQIhour as(SELECT personsno,CONVERT(DECIMAL(7,1),sum(Integral) )Integral
  FROM [New_QSMS].[dbo].[QS_EIntegral] QEI
  left join Person P on qei.PersonID=P.PersonID
  where p.PersonSNO=@PersonSNO   and CDate>'2022-10-31' and QEI.isused=1
    group by PersonSNO)
  select sum(isnull(QIhour.積分,0))+sum(isnull(EQIhour.Integral,0)) 積分 from QIhour left join EQIhour on QIhour.PersonSNO=EQIhour.PersonSNO";
        ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT;
        }
        else
        {
            return null;
        }
    }
    protected void gv_Certificate_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //要隱藏的欄位，如果前台filed隱藏會抓不到值，所以在RowCreate時才隱藏
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {

            //e.Row.Cells[0].Visible = false;
            ////巡覽Gridview值
            //string ToolTipString = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "SysChange"));
            //if (ToolTipString == "True")
            //{
            //    //6碼
            //    e.Row.Cells[2].Visible = false;
            //}
            //else
            //{
            //    //原始
            //    e.Row.Cells[1].Visible = false;
            //}



        }
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
        if (Ctype == "2")
        {
            bindData_Doneintegral(PClassSNO, Class1, Ctype);
            Div1.Visible = false;
            Div3.Visible = true;
        }
        else
        {
            bindData_Doneintegral(PClassSNO, Class1, Ctype);
            Div1.Visible = true;
            Div3.Visible = false;
        }

    }

    protected void LK_NotDone_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);

        string[] AllArg = btn.CommandArgument.Split(',');
        string PClassSNO = AllArg[0].ToString();
        string Class1 = AllArg[1].ToString();
        string Ctype = AllArg[2].ToString();
        if (Ctype == "2")
        {
            bindData_integral(PClassSNO, Class1, Ctype);
            Div1.Visible = false;
            Div3.Visible = true;
        }
        else
        {
            bindData_integral(PClassSNO, Class1, Ctype);
            Div1.Visible = true;
            Div3.Visible = false;
        }
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
    protected static string CovertPersonSNOtoID(string PersonSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string SQL = "Select * from Person where PersonSNO=@PersonSNO";
        adict.Add("personSNO", PersonSNO);
        DataTable ObjDT = objDH.queryData(SQL, adict);
        string PersonID = ObjDT.Rows[0]["PersonID"].ToString();
        if (ObjDT.Rows.Count > 0)
        {
            return PersonID;
        }
        else
        {
            return "";
        }
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

    protected void btn_prove_Click(object sender, EventArgs e)
    {
        string PersonSNO = Request.QueryString["SNO"];
        LinkButton btn = (LinkButton)sender;
        RepeaterItem item = (RepeaterItem)btn.NamingContainer;
        Label lblName = (Label)item.FindControl("dd");
        string PClassSNO = lblName.Text;
        bool IsCore = Utility.ReturnCtypeSNO(PClassSNO);
        if (IsCore && hf_Core.Value == "2")
        {
            Response.Redirect("./ScorePrintAjax.aspx?PClassSNO=" + PClassSNO + "&PersonSNO=" + PersonSNO + "&Word=0&Core=2");
        }
        else if (IsCore && hf_Core.Value == "1")
        {
            Response.Redirect("./ScorePrintAjax.aspx?PClassSNO=" + PClassSNO + "&PersonSNO=" + PersonSNO + "&Word=0&Core=1");
        }
        else
        {
            Response.Redirect("./ScorePrintAjax.aspx?PClassSNO=" + PClassSNO + "&PersonSNO=" + PersonSNO + "&Word=0");
        }

    }

    protected void btn_prove_word_Click(object sender, EventArgs e)
    {
        string PersonSNO = Request.QueryString["SNO"];
        LinkButton btn = (LinkButton)sender;
        RepeaterItem item = (RepeaterItem)btn.NamingContainer;
        Label lblName = (Label)item.FindControl("dd");
        string PClassSNO = lblName.Text;
        bool IsCore = Utility.ReturnCtypeSNO(PClassSNO);
        if (IsCore && hf_Core.Value == "1")
        {
            Response.Redirect("./ScorePrintAjax.aspx?PClassSNO=" + PClassSNO + "&PersonSNO=" + PersonSNO + "&Word=1&Core=1");
        }
        else
        {
            Response.Redirect("./ScorePrintAjax.aspx?PClassSNO=" + PClassSNO + "&PersonSNO=" + PersonSNO + "&Word=1");
        }

    }
    protected void btn_Eprove_Click(object sender, EventArgs e)
    {
        string PersonSNO = Request.QueryString["SNO"];
        LinkButton btn = (LinkButton)sender;
        RepeaterItem item = (RepeaterItem)btn.NamingContainer;
        Label lblName = (Label)item.FindControl("cc");
        string PClassSNO = lblName.Text;
        Response.Redirect("./ScorePrintAjax.aspx?EPClassSNO=" + PClassSNO + "&PersonSNO=" + PersonSNO + "&Word=0");
    }

    protected void btn_Eprove_word_Click(object sender, EventArgs e)
    {
        string PersonSNO = Request.QueryString["SNO"];
        LinkButton btn = (LinkButton)sender;
        RepeaterItem item = (RepeaterItem)btn.NamingContainer;
        Label lblName = (Label)item.FindControl("cc");
        string PClassSNO = lblName.Text;
        Response.Redirect("./ScorePrintAjax.aspx?EPClassSNO=" + PClassSNO + "&PersonSNO=" + PersonSNO + "&Word=1");
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string PersonSNO = Request.QueryString["sno"].ToString();
        Response.Redirect("Personnel_AE.aspx??a=&n=NN&redirect=1&sno=" + PersonSNO);
    }

    protected void LK_Entity_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);
        string Ctype = "0,1,2,3,4";
        string EPClassSNO = btn.CommandArgument;
        EbindData_Upliadintegral(EPClassSNO, Ctype);
        EUploadDetail.Visible = true;
    }

    protected void gv_Certificate_N_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {

            
            //巡覽Gridview值
            string ToolTipString = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "SysChange"));
            if (ToolTipString == "True")
            {
                //6碼
                e.Row.Cells[2].Visible = false;
            }
            else
            {
                //原始
                e.Row.Cells[1].Visible = false;
            }



        }
    }
}
 