using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Option : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        string qid = Request.Form["qid"].ToString();
        string isUse = Request.Form["isUse"].ToString();
        Label2.Text = qid;
        Label3.Text = isUse;

        String sqls = @"
            SELECT * FROM Question Where QuestionID= @QuestionID
        ";
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("QuestionID", qid);
        
        DataTable dt = objDH.queryData(sqls, dic);
        Label1.Text = dt.Rows[0]["QuestionName"].ToString();



        String sql = @"
            SELECT * FROM [Option] Where QuestionID= @QuestionID
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        wDict.Add("QuestionID", qid);

        DataTable objDT = objDH.queryData(sql, wDict);

        if (objDT.Rows.Count != 0)
        {
            Repeater1.DataSource = objDT;
            Repeater1.DataBind();
        }



    }
}