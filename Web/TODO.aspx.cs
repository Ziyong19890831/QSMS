using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class Web_TODO : System.Web.UI.Page
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
	            ROW_NUMBER () OVER (ORDER BY PD.getPersonSno,State ASC, CreateDate DESC) AS ROW_NO ,PD.*,PS.PName
            FROM TODO PD
                LEFT JOIN Person PS ON PS.PersonSNO = PD.postPersonSNO
            WHERE getPersonSno = @getPersonSno 
            ";

        if (ddl_Choice.SelectedValue == "1")
        {
            sql += " And State=0";
        }
        if (ddl_Choice.SelectedValue == "2")
        {
            sql += " And State=1";
        }
        if (!String.IsNullOrEmpty(txt_Search.Text))
        {
            sql += " And TodoTitle Like '%' + @TodoTitle + '%' ";
            aDict.Add("TodoTitle", txt_Search.Text);
        }
        aDict.Add("getPersonSno", userInfo.PersonSNO);
        sql += " order by PD.CreateDate DESC";
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
                HtmlTableRow teee = (HtmlTableRow)row.FindControl("teee");
                Label Label1 = (Label)row.FindControl("Label1");
                Label Label2 = (Label)row.FindControl("Label2");
                Label Label3 = (Label)row.FindControl("Label3");
                Label Label4 = (Label)row.FindControl("Label4");



                if (Label2.Text == "1")
                {
                    Label3.Visible = true;
                    teee.Style["background-color"] = "#f5f5f5";
                    Label4.ForeColor = ColorTranslator.FromHtml("#666666");
                }
                else
                {
                    Label1.Visible = true;
                    Label4.Font.Bold = true;
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


    protected void Unnamed_ServerClick(object sender, EventArgs e)
    {
        bindData(1);
    }
}