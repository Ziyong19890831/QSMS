using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Web_Question : System.Web.UI.Page
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
                bindData(1);
            }

        }
    }
    protected void bindData(int page)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        if (page < 1) page = 1;
        int pageRecord = 10;
        String sql = @"
           SELECT
	ROW_NUMBER () OVER (ORDER BY PP.PaperID) AS ROW_NO ,PP.*,
CASE WHEN EM.ExamID IS NOT NULL THEN '已填' ELSE '未填寫' END uWrite
FROM
	PAPER PP
LEFT JOIN Exam EM ON EM.PersonSno = @PersonSno
WHERE PP.isUse = '1' AND PP.IsWrite = '1'
            ";

        aDict.Add("PersonSno", userInfo.PersonSNO);

        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);


        if (objDT.Rows.Count != 0)
        {
            rpt_QA.DataSource = objDT.DefaultView;
            rpt_QA.DataBind();
            Panel1.Visible = true;
            lbl_msg.Visible = false;

            foreach (RepeaterItem row in rpt_QA.Items)
            {

                HtmlTableCell teee = (HtmlTableCell)row.FindControl("teee");
                HtmlTableCell teee1 = (HtmlTableCell)row.FindControl("teee1");
                
                Label Label1 = (Label)row.FindControl("Label1");

                if (Label1.Text == "未填寫")
                {
                    tee2.Visible = true;
                    teee.Visible = true;
                    teee1.Visible = false;
                    tee3.Visible = false;
                }
                else
                {
                    tee2.Visible = false;
                    teee.Visible = false;
                    teee1.Visible = true;
                    tee3.Visible = true;
                }
            }

        }
        else
        {
            Panel1.Visible = false;
            lbl_msg.Visible = true;
        }

        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        bindData(page);
    }

}