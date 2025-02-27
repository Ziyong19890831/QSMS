using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Web_WriteQuestion : System.Web.UI.Page
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
           SELECT * FROM Exam WHERE PaperID = @PaperID AND PersonSNO = @PersonSNO
            ";
                Dictionary<string, object> aDict = new Dictionary<string, object>();
               DataHelper objDH = new DataHelper();
                aDict.Add("PaperID", Request.QueryString["sno"]);
                aDict.Add("PersonSNO", userInfo.PersonSNO);
                DataTable objDT = objDH.queryData(sql, aDict);

                if(objDT.Rows.Count >= 1)
                {
                    Response.Redirect("../Web/Question.aspx");
                }

                bindData(1);
                hf_PersonSNO.Value = userInfo.PersonSNO;
                hf_Query.Value = Request.QueryString["sno"].ToString();
            }

        }
    }
    protected void bindData(int page)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        
        String sql = @"
           SELECT * FROM QUESTION WHERE PaperID = @PaperID  ORDER BY Sort
            ";

        String sql1 = @"
           SELECT * FROM Paper WHERE PaperID = @PaperID
            ";

        aDict.Add("PaperID", Request.QueryString["sno"]);

        DataTable objDT = objDH.queryData(sql, aDict);
        DataTable objDTPAPER = objDH.queryData(sql1, aDict);

        if (objDT.Rows.Count != 0)
        {
            rpt_QA.DataSource = objDT.DefaultView;
            rpt_QA.DataBind();




            lb_PaperName.Text = objDTPAPER.Rows[0]["PaperName"].ToString();
            lb_PaperDetail.Text = objDTPAPER.Rows[0]["PaperDetail"].ToString();
        }
       
        
    }
    protected void rpt_QA_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
       
        Repeater rpt_option = e.Item.FindControl("rpt_option") as Repeater;
        Label lbl_QID = e.Item.FindControl("lbl_QID") as Label;
        Label Label5 = e.Item.FindControl("Label5") as Label;

        if (Label5.Text != "")
        {
            Label5.Text = "(" + Label5.Text+ ")";
        }
        

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
       
        String sql = @"
           SELECT * FROM [Option] WHERE QuestionID = @QuestionID
            ";
        

        aDict.Add("QuestionID", lbl_QID.Text);

        DataTable objDT = objDH.queryData(sql, aDict);

        if (objDT.Rows.Count != 0)
        {
            rpt_option.DataSource = objDT.DefaultView;
            rpt_option.DataBind();

          

        }

    }

    protected void saveQ_ServerClick(object sender, EventArgs e)
    {

    }
}