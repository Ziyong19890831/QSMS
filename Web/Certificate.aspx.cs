using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Certificate : System.Web.UI.Page
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
        if (!IsPostBack)
        {
            bindData_certificate();
            bindData_courseclass();
        }
    }

    protected void CheckLearningClass()
    {

        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = @"SELECT PVal From Config Where PID='CertNoteContent' ";
        aDict.Add("PersonSNO", userInfo.PersonSNO);
        DataTable objDT = objDH.queryData(sql, aDict);
    }

    //撈已取得的證書資料
    protected void bindData_certificate()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;


        string sql = @"SELECT PVal From Config Where PID='CertNoteContent' ";
        objDT = objDH.queryData(sql, null);
        if (objDT.Rows.Count > 0) NoteCertificate.Text = objDT.Rows[0]["PVal"].ToString();


        sql = @"
            SELECT 
                C.CertID,
                CT.CTypeName,
                CU.CUnitName,
                Cast(C.CertPublicDate As varchar(10)) CertPublicDate,
                Cast(C.CertStartDate As varchar(10)) CertStartDate,
                Cast(C.CertEndDate As varchar(10)) CertEndDate,
                --C.CertStartDate,
                --C.CertEndDate,
                (Case C.CertExt When 1 Then '有' Else '無' End) CertExt
            FROM QS_Certificate C
                Left JOIN QS_CertificateType CT on CT.CTypeSNO=C.CTypeSNO
                Left JOIN QS_CertificateUnit CU on CU.CUnitSNO=C.CUnitSNO
            WHERE
                C.PersonID=@PersonID
        ";
        aDict.Add("PersonID", userInfo.PersonID);
        objDT = objDH.queryData(sql, aDict);
        rpt_Notice.DataSource = objDT.DefaultView;
        rpt_Notice.DataBind();
    }
    
    //撈已完成或進行中的課程規劃表
    protected void bindData_courseclass()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;


        string sql = @"SELECT PVal From Config Where PID='ScoreNoteContent' ";
        objDT = objDH.queryData(sql, null);
        if (objDT.Rows.Count > 0) NoteScore.Text = objDT.Rows[0]["PVal"].ToString();


        sql = @"
			With getAllCertificateType As (
				SELECT 
					cpc.PClassSNO,
					cpc.PlanName,
					cpc.CStartYear,
					cpc.CEndYear,
					ct.CTypeName
				From QS_CoursePlanningClass cpc
					 LEFT JOIN QS_CoursePlanningRole cpr on cpr.PClassSNO=cpc.PClassSNO
					 LEFT JOIN QS_CertificateType ct on ct.CTypeSNO=cpc.CTypeSNO
				Where cpr.RoleSNO=@RoleSNO And IsEnable=1
			)

			--課程規劃類別之各個總時數
			, getAllCourseHours As (
				Select  c.PClassSNO, SUM(c.CHour) sumHours
				From QS_CoursePlanningClass cpc
					Left JOIN QS_Course c on c.PClassSNO=cpc.PClassSNO
				Group By c.PClassSNO
			)
	
			--取得所有E-learningPart數
			, getAllParts As (
				Select ELSCode, ces.ELSPart
				From QS_CourseELearningSection ces 
			)
			
			--取得學員上課之統計節數
			, getLearningParts As (
				Select ELSCode, Count(1) FinishedParts 
				From (
					Select Distinct lr.ELSCode, lr.ELSPart
					From QS_LearningRecord lr 
						Left Join QS_Course c ON c.ELSCode=lr.ELSCode
						Left Join QS_CourseELearningSection ces ON ces.ELSCode=lr.ELSCode
					Where lr.PersonID=@PersonID
				) t
				Group By ELSCode
			)
			
			--取得學員已完成課程之清單&統計學員已完成之課程類別總時數
			, getFinishedLearning As (
				Select PClassSNO, Sum(CHour) PClassTotalHr
				From (
					Select 
						c.PClassSNO, ap.ELSCode, ap.ELSPart, lp.FinishedParts, c.CHour
					From getAllParts ap
						Left Join getLearningParts lp ON lp.ELSCode=ap.ELSCode 
						Left Join QS_Course c ON c.ELSCode=ap.ELSCode
					Where ap.ELSPart=lp.FinishedParts And c.PClassSNO Is Not Null
				) t
				Group By PClassSNO
			)

			Select ct.*, fl.PClassTotalHr, ch.sumHours
			From getAllCertificateType ct
				Left Join getAllCourseHours ch On ch.PClassSNO=ct.PClassSNO
				Left Join getFinishedLearning fl ON fl.PClassSNO=ct.PClassSNO
        ";
        aDict.Add("RoleSNO", userInfo.RoleSNO);
        aDict.Add("PersonID", userInfo.PersonID);
        objDT = objDH.queryData(sql, aDict);
        rpt_CoursePlanningClass.DataSource = objDT.DefaultView;
        rpt_CoursePlanningClass.DataBind();
    }

    protected void getCert_Click(object sender, EventArgs e)
    {
        
    }

}