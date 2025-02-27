using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_OKQuestion : System.Web.UI.Page
{
    UserInfo userInfo = null;
    
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
        if (userInfo == null) Response.Redirect("../Default.aspx");
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["QSMS_UserInfo"] != null)
            {

                String sql = @"
           SELECT *,CONVERT(varchar(100), CreateDT, 23) AS CDATE FROM Exam WHERE PaperID = @PaperID AND PersonSno = @PersonSno
            ";
                Dictionary<string, object> aDict = new Dictionary<string, object>();
                DataHelper objDH = new DataHelper();
                aDict.Add("PaperID", Request.QueryString["sno"]);
                aDict.Add("PersonSno", userInfo.PersonSNO);
                

                DataTable objDT = objDH.queryData(sql, aDict);

                if (objDT.Rows.Count < 1)
                {
                    Response.Redirect("../Web/Question.aspx");
                }

                Label5.Text = objDT.Rows[0]["CDATE"].ToString();

                bindData(1);
            }

        }
    }
    protected void bindData(int page)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();

        aDict.Add("PaperID", Request.QueryString["sno"]);
        aDict.Add("PersonSno", userInfo.PersonSNO);

        DataHelper objDH = new DataHelper();

        String sql = @"
                 SELECT
 QS.QuestionName,OP.OptionName
FROM
	Answer AW
LEFT JOIN Exam EM ON EM.ExamID = AW.ExamID
LEFT JOIN Question QS ON AW.QuestionID = QS.QuestionID
LEFT JOIN [Option] OP ON AW.OptionID = OP.OptionID
WHERE EM.PersonSno = @PersonSno AND EM.PaperID = @PaperID
ORDER BY QS.Sort ASC
            ";

        String sql1 = @"
           SELECT * FROM Paper WHERE PaperID = @PaperID
            ";

        

        DataTable objDT = objDH.queryData(sql, aDict);

        aDict.Clear();
        aDict.Add("PaperID", Request.QueryString["sno"]);

        DataTable objDTPAPER = objDH.queryData(sql1, aDict);

        if (objDT.Rows.Count != 0)
        {
            rpt_QA.DataSource = objDT.DefaultView;
            rpt_QA.DataBind();




            Label2.Text = objDTPAPER.Rows[0]["PaperName"].ToString();
            Label3.Text = objDTPAPER.Rows[0]["PaperDetail"].ToString();
        }


    }


}