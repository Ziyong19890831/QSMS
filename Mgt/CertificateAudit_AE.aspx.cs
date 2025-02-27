using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_CertificateAudit_AE : System.Web.UI.Page
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
        //string pclassid = Convert.ToString(Request.QueryString["pno"]);
        aDict.Add("PersonSNO", personid);
        //aDict.Add("PClassSNO", pclassid);

        //取報名資料
        DataTable objDT = objDH.queryData(@"            
                with getsomething as(
                SELECT 
                	I.PersonSNO
                    ,P.PName
                	,QCPC.PlanName
                	,QCPC.CStartYear
                	,QCPC.CEndYear
                	,QCT.CTypeName
                	,QC.PClassSNO
                    ,QCPC.TargetIntegral
                	,sum(CHour) PClassTotalHr
                  FROM [QS_Integral] I
                  Left Join Person P on P.PersonSNO=I.PersonSNO
                  Left Join QS_Course QC on QC.CourseSNO=I.CourseSNO
                  Left Join QS_CoursePlanningClass QCPC on QCPC.PClassSNO=QC.PClassSNO
                  Left Join QS_CertificateType QCT on QCT.CTypeSNO=QCPC.CTypeSNO
                    where 1=1
                  Group by QCPC.PlanName,QCT.CTypeName,QCPC.CStartYear,QCPC.CEndYear,QC.PClassSNO,I.PersonSNO,P.PName,QCPC.TargetIntegral
                  )
                  , getAllCourseHours As (
                				Select  c.PClassSNO, SUM(c.CHour) sumHours
                				From QS_CoursePlanningClass cpc
                					Left JOIN QS_Course c on c.PClassSNO=cpc.PClassSNO
                				Group By c.PClassSNO
                			)
                
                  select * from getsomething
                  left join getAllCourseHours gc on gc.PClassSNO=getsomething.PClassSNO
                  where PersonSNO=@PersonSNO          
        ", aDict);
        gv_Cerificate.DataSource = objDT.DefaultView;
        gv_Cerificate.DataBind();

        if (objDT.Rows.Count > 0)
        {
            lbl_Pname.Text = objDT.Rows[0]["PName"].ToString();
        }        

        //DataTable objDT1 = objDH.queryData(@"            
        //        SELECT C.CTypeName, B.PlanName  
        //        FROM  QS_CoursePlanningClass B  
        //            LEFT JOIN QS_CertificateType C ON C.CTypeSNO=B.CTypeSNO
        //        WHERE B.PClassSNO = @PClassSNO        
        //", aDict);

        //if (objDT1.Rows.Count > 0)
        //{
        //    lbl_CTypeName.Text = objDT1.Rows[0]["CTypeName"].ToString();
        //    lbl_PlanName.Text = objDT1.Rows[0]["PlanName"].ToString();
        //}


    }

}