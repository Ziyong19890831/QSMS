using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_CertificateAudit_AE_Review : System.Web.UI.Page
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
            bindData();
        }
    }
    protected void bindData()
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string personid = Convert.ToString(Request.QueryString["sno"]);
        aDict.Add("PersonSNO", personid);

        DataTable objDT = objDH.queryData( @"With AllPersonLearningRecord As (
	          SELECT 
                	I.PersonSNO
					
					,P.RoleSNO
					,P.PersonID
                    ,PName
                	,QCPC.PlanName
                	,QCPC.CStartYear
                	,QCPC.CEndYear
                	,QCT.CTypeName
					,QCT.CTypeSNO
                	,QC.PClassSNO
                	,sum(CHour) PClassTotalHr
                  FROM [QS_Integral] I
                  Left Join Person P on P.PersonSNO=I.PersonSNO				  
                  Left Join QS_Course QC on QC.CourseSNO=I.CourseSNO
                  Left Join QS_CoursePlanningClass QCPC on QCPC.PClassSNO=QC.PClassSNO
                  Left Join QS_CertificateType QCT on QCT.CTypeSNO=QCPC.CTypeSNO
                    where 1=1
                  Group by QCPC.PlanName,QCT.CTypeName,QCPC.CStartYear,QCPC.CEndYear,QC.PClassSNO,I.PersonSNO,P.PName,P.RoleSNO,P.PersonID,QCT.CTypeSNO
                  )
				  , CoursePlanningHours As (
	            SELECT 
		             cpc.PClassSNO,
		             cpc.PlanName,
		             ct.CTypeName,
		             Sum(c.CHour) PHours
	            From QS_CoursePlanningClass cpc
		             LEFT JOIN QS_CertificateType ct on ct.CTypeSNO=cpc.CTypeSNO
		             LEFT JOIN QS_Course c on c.PClassSNO=cpc.PClassSNO
	            Where Cpc.IsEnable=1
	            Group By cpc.PClassSNO, cpc.PlanName, ct.CTypeName
            )
            Select 
                ROW_NUMBER() OVER (ORDER BY PersonSNO DESC ) as ROW_NO,
                ap.PClassSNO,
	            ap.PersonSNO,
				ap.PersonID,
	            ap.PName,
	            ap.RoleSNO,
	            cph.PlanName,
	            cph.CTypeName,
				ap.CTypeSNO,
	            ap.PClassTotalHr,
	            cph.PHours,
				C.CUnitSNO,
				QC.CourseSNO,
				QC.CourseName
	
            From AllPersonLearningRecord ap
	            Left Join CoursePlanningHours cph On cph.PClassSNO=ap.PClassSNO
				Left Join QS_Certificate C ON C.PersonID=ap.PersonID
	            Left Join Role R On R.RoleSNO=ap.RoleSNO
				Left JOIN QS_Course QC ON QC.PClassSNO=ap.PClassSNO
            Where PClassTotalHr>=PHours 
            And ap.PersonSNO=@PersonSNO",aDict);
        gv_Cerificate_Review.DataSource = objDT.DefaultView;
        gv_Cerificate_Review.DataBind();

        if (objDT.Rows.Count > 0)
        {
            lbl_Pname.Text = objDT.Rows[0]["PName"].ToString();
            lbl_CTypeName.Text = objDT.Rows[0]["CTypeName"].ToString();
        }

    }
    protected void btnInsert_ServerClick(object sender, EventArgs e)
    {

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string personid = Convert.ToString(Request.QueryString["sno"]);
        aDict.Add("PersonSNO", personid);

        DataTable objDT = objDH.queryData(@"With AllPersonLearningRecord As (
	          SELECT 
                	I.PersonSNO
					
					,P.RoleSNO
					,P.PersonID
                    ,PName
                	,QCPC.PlanName
                	,QCPC.CStartYear
                	,QCPC.CEndYear
                	,QCT.CTypeName
					,QCT.CTypeSNO
                	,QC.PClassSNO
                	,sum(CHour) PClassTotalHr
                  FROM [QS_Integral] I
                  Left Join Person P on P.PersonSNO=I.PersonSNO				  
                  Left Join QS_Course QC on QC.CourseSNO=I.CourseSNO
                  Left Join QS_CoursePlanningClass QCPC on QCPC.PClassSNO=QC.PClassSNO
                  Left Join QS_CertificateType QCT on QCT.CTypeSNO=QCPC.CTypeSNO
                    where 1=1
                  Group by QCPC.PlanName,QCT.CTypeName,QCPC.CStartYear,QCPC.CEndYear,QC.PClassSNO,I.PersonSNO,P.PName,P.RoleSNO,P.PersonID,QCT.CTypeSNO
                  )
				  , CoursePlanningHours As (
	            SELECT 
		             cpc.PClassSNO,
		             cpc.PlanName,
		             ct.CTypeName,
		             Sum(c.CHour) PHours
	            From QS_CoursePlanningClass cpc
		             LEFT JOIN QS_CertificateType ct on ct.CTypeSNO=cpc.CTypeSNO
		             LEFT JOIN QS_Course c on c.PClassSNO=cpc.PClassSNO
	            Where IsEnable=1
	            Group By cpc.PClassSNO, cpc.PlanName, ct.CTypeName
            )
            Select 
                ROW_NUMBER() OVER (ORDER BY PersonSNO DESC ) as ROW_NO,
                ap.PClassSNO,
	            ap.PersonSNO,
				ap.PersonID,
	            ap.PName,
	            ap.RoleSNO,
	            cph.PlanName,
	            cph.CTypeName,
				ap.CTypeSNO,
	            ap.PClassTotalHr,
	            cph.PHours,
				C.CUnitSNO,
				QC.CourseSNO,
				QC.CourseName
	
            From AllPersonLearningRecord ap
	            Left Join CoursePlanningHours cph On cph.PClassSNO=ap.PClassSNO
				Left Join QS_Certificate C ON C.PersonID=ap.PersonID
	            Left Join Role R On R.RoleSNO=ap.RoleSNO
				Left JOIN QS_Course QC ON QC.PClassSNO=ap.PClassSNO
            Where PClassTotalHr>=PHours 
            And ap.PersonSNO=@PersonSNO", aDict);

        //Insert QS_Certificate
        string Ins_Pname = objDT.Rows[0]["Pname"].ToString();
        string Ins_PersonID = objDT.Rows[0]["PersonID"].ToString();
        string Ins_CtypeSNO = objDT.Rows[0]["CtypeSNO"].ToString();
        string Ins_CUnitSNO = objDT.Rows[0]["CUnitSNO"].ToString();
        string Ins_PersonSNO = objDT.Rows[0]["PersonSNO"].ToString();
        string Ins_CertPublicDate = DateTime.Now.ToString("yyyy-MM-dd");
        string Ins_CertStartDate = DateTime.Now.ToString("yyyy-MM-dd");
        DateTime Ins_TMP_CertEndDate = DateTime.Now.AddYears(6).AddDays(-1);
        string Ins_CertEndDate = Ins_TMP_CertEndDate.ToString("yyyy-MM-dd");
        DataHelper DH = new DataHelper();
        Dictionary<string, object> WDict = new Dictionary<string, object>();
        WDict.Add("PersonID", Ins_PersonID);
        WDict.Add("CertID", txt_CertID.Text);
        WDict.Add("CTypeSNO", Ins_CtypeSNO);
        WDict.Add("CUnitSNO", Ins_CUnitSNO);
        WDict.Add("CertPublicDate", Ins_CertPublicDate);
        WDict.Add("CertStartDate", Ins_CertStartDate);
        WDict.Add("CertEndDate", Ins_CertEndDate);
        WDict.Add("CertExt", 0);
        WDict.Add("IsPrint", 0);
        WDict.Add("CreateUserID", userInfo.PersonSNO);

        string Insert_SQL = @"Insert into QS_Certificate (
        [PersonID]
        ,[CertID]
        ,[CTypeSNO]
        ,[CUnitSNO]
        ,[CertPublicDate]
        ,[CertStartDate]
        ,[CertEndDate]
        ,[CertExt]
        ,[IsPrint]
        ,[CreateUserID]) values (
        @PersonID,@CertID,@CTypeSNO,@CUnitSNO,cast(@CertPublicDate as datetime),cast(@CertStartDate as datetime),cast(@CertEndDate as datetime),@CertExt,@IsPrint,@CreateUserID)";
        objDH.executeNonQuery(Insert_SQL, WDict);


        //Update QS_Integral isUsed欄位
        Dictionary<string, object> bDict = new Dictionary<string, object>();
        for (int i = 0; i < objDT.Rows.Count; i++)
        {

            string Update_SQL = @"Update [QS_Integral] set [IsUsed]=1 where [PersonSNO]=@PersonSNO" + i + " And [CourseSNO]=@CourseSNO" + i + "";
            bDict.Add("CourseSNO" + i + "", objDT.Rows[i]["CourseSNO"].ToString());
            bDict.Add("PersonSNO" + i + "", Ins_PersonSNO);
            objDH.executeNonQuery(Update_SQL, bDict);
        }
        Response.Write("<script>opener.location.reload()</script>");
        Response.Write("<script language='javascript'>window.close();</script>");
    }
}