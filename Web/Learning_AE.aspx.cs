using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Learning_AE : System.Web.UI.Page
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
            bindData_CoursePlanningClass();
        }
    }
    protected void bindData_CoursePlanningClass()
    {
       
        string request_PclassSNO = Request.QueryString["PClassSNO"];
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT;

        string sql = @"            
               with getX as(
                    SELECT CPC.PClassSNO,CPC.PlanName,CPC.CStartYear,CPC.CEndYear,A.CourseSNO,A.CourseName,A.CHour
					,B.MVal + '課程' Class1 , C.MVal Class2 , A.UnitName , D.MVal Ctype
                      FROM [QS_CoursePlanningClass] CPC
                      Left Join QS_Course A ON A.PClassSNO=CPC.PClassSNO
					  JOIN Config B ON B.PGroup ='CourseClass1' AND A.Class1 = B.PVal
					  JOIN Config C ON C.PGroup ='CourseClass2' AND A.Class2 = C.PVal
                      JOIN Config D ON D.PGroup ='CourseCType' AND A.CType = D.PVal
                      
					  
                      ),
                      getY as(
                      select 1 O,I.CourseSNO from QS_Integral I
                      where I.PersonSNO=@PersonSNO
                      )
                      select * from getX X
                      FULL outer Join getY Y ON Y.CourseSNO=X.CourseSNO
					  Left JOIN QS_CoursePlanningRole CPR ON CPR.RClassSNO=X.PClassSNO
					  Left Join Role R ON R.RoleSNO=CPR.RoleSNO
                       where X.PClassSNO=@PClassSNO
                    
            
            ";
        aDict.Add("@PersonSNO", userInfo.PersonSNO);
        aDict.Add("@PClassSNO", request_PclassSNO);
        objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            rpt_CoursePlanningClass.DataSource = objDT.DefaultView;
            rpt_CoursePlanningClass.DataBind();
        }
        else
        {
            tbl_CoursePlanningClass.Visible = false;
            //lb_CoursePlanningClass.Visible = true;
        }
    }
}