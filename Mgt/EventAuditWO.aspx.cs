using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_EventAuditWO : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
    protected void bindData_learning()
    {
        string personSNO = Request.QueryString["PersonSNO"];
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String sql = @"
            SELECT 
	            (Case c.Class1 When 1 Then '核心課程' When 2 Then'專門課程' End) Class1,
	            (Case c.Class2 When 1 Then 'Knowledge' When 2 Then'Practice' End) Class2,
	            c.UnitName,
	            c.CourseName,
	            (Case c.CType When 1 Then '線上' When 2 Then'實體' When 3 Then '實習' End) CType,
	            c.CHour,
                convert(varchar(16), lr.FinishedDate, 120) FinishedDate
            From QS_LearningRecord lr
                LEFT JOIN QS_Course c on c.CourseSNO=lr.CourseSNO
                LEFT JOIN Person P on P.PersonID=lr.PersonID
            Where P.PersonSNO=@PersonSNO 
        ";
        aDict.Add("PersonSNO", personSNO);
        DataTable objDT = objDH.queryData(sql, aDict);
        rpt_Learning.DataSource = objDT.DefaultView;
        rpt_Learning.DataBind();
    }



    protected void NewLearning()
    {
        string PersonSNO = Request.QueryString["PersonSNO"];
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = @"
            select Score, QuizName, ExamDate, PassScore,
                Case IsPass when 0 then '不通過' when 1 then '通過' End Pass 
            from QS_LearningScore ls
                LEFT JOIN Person P on P.PersonID=ls.PersonID
            where P.PersonSNO=@PersonSNO
        ";
        aDict.Add("PersonSNO", PersonSNO);
        DataTable objDT = objDH.queryData(sql, aDict);
        Learning.DataSource = objDT.DefaultView;
        Learning.DataBind();

    }

}