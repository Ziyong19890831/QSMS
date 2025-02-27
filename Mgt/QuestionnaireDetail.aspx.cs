using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Mgt_QuestionnaireDetail : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    int viewrole = 1;
    public void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {          
            BindData(1);

        }
 
    }

    public void BindData(int page)
    {
        string PersonSNO = Convert.ToString(Request.QueryString["sno"] == "" ? "" : Request.QueryString["sno"]);
        string ELSCode = Convert.ToString(Request.QueryString["esno"] == "" ? "" : Request.QueryString["esno"]);
        if (PersonSNO != "")
        {
            if (viewrole == 0) return;
            if (page < 1) page = 1;
            int pageRecord = 10;

            DataHelper ObjDH = new DataHelper();
            Dictionary<string, Object> adict = new Dictionary<string, object>();
            string sql = @"Select  ROW_NUMBER() OVER (ORDER BY Ano) AS ROW_NO,P.PName,LA.PersonID,R.RoleName,C.Mval Class,CES.ELSName,LF.QName,ANO,
			Case when C2.Mval is null Then LA.Ans  ELSE C2.Mval END Ans,LA.CompletedDate
            from QS_LearningFeedback LF           
            LEFT Join QS_LearningAnswer LA ON LA.QID=LF.QID
			Left JOin Person P On P.PersonID=LA.PersonID
            LEFT  Join Role R ON R.RoleSNO=P.RoleSNO            
            LEFT  Join QS_CourseELearningSection CES ON CES.ELSCode=LF.ELSCode
            LEFT Join QS_Course QC On QC.ELSCode=CES.ELSCode
            LEFT Join Config C On C.PVal=QC.Class1 and C.PGroup='CourseClass1'
            LEFT Join Config C2 On C2.Pval=LA.ANS and C2.PGroup='Questionnaire'          
            where P.PersonSNO=@PersonSNO and CES.ELScode=@ELSCode order by LA.ANO";
            adict.Add("PersonSNO", PersonSNO);
            adict.Add("ELScode", ELSCode);
            DataTable ObjDT = ObjDH.queryData(sql, adict);
            int maxPageNumber = (ObjDT.Rows.Count - 1) / pageRecord + 1;
            if (page > maxPageNumber) page = maxPageNumber;
            ObjDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
            rpt_QuestionnaireDetail.DataSource = ObjDT.DefaultView;
            rpt_QuestionnaireDetail.DataBind();
            ltl_PageNumber.Text = Utility.showPageNumber(ObjDT.Rows.Count, page, pageRecord);
        }
           
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData(1);
    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        BindData(page);
    }
}