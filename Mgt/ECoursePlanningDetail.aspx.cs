using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_ECoursePlanningDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    protected void bind()
    {
        string EPClassSNO = Request.QueryString["Esno"];
        Dictionary<string, object> adict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        string SQL = @"Select  ROW_NUMBER() OVER (ORDER BY QECPC.EPClassSNO) as ROW_NO, QECPC.[EPClassSNO]
                            ,[PlanName]
	                        ,Cast(CStartYear as varchar(4)) + '-' + Cast(CEndYear as varchar(4)) As 'CYear'
                            ,QECPC.[IsEnable]
	                        ,[QECPC].CTypeSNO
                            ,[Compulsory_Entity]
                            ,[Compulsory_Practical]
                            ,[Compulsory_Communication]
                            ,[Compulsory_Online]
	                        ,ct.CTypeName
                            ,QECPC.[CreateDT]
                            ,QECPC.[CreateUserID]
                            ,QECPC.[ModifyDT]
                            ,QECPC.[ModifyUserID]
							,E.StartTime
							,E.EndTime
                            ,Substring(
							 (
                                        	Select ',' + RoleName 
                                        	From 
                                        		(Select cpr.EPClassSNO, r.RoleName From QS_ECoursePlanningRole cpr
                                                Left Join Role r ON r.RoleSNO=cpr.RoleSNO) t
                                        	Where t.EPClassSNO=QECPC.EPClassSNO For XML PATH ('')
                                        ),2,100) as CRole
                            from [QS_ECoursePlanningClass] QECPC
							Left Join Event E On E.EPClassSNO=QECPC.EPClassSNO
                            Left Join QS_CertificateType ct ON ct.CTypeSNO=[QECPC].CTypeSNO Where 1=1 and QECPC.EPClassSNO=@EPClassSNO";
        adict.Add("EPClassSNO", EPClassSNO);
        DataTable ObjDT = ObjDH.queryData(SQL, adict);
        gv_EcourseDetail.DataSource = ObjDT;
        gv_EcourseDetail.DataBind();

    }
}