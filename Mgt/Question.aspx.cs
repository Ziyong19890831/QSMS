using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Question : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    int viewrole = 1;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindData(1);

            DataHelper odt = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("PaperID", Request.QueryString["sno"]);
            DataTable dt_P = odt.queryData("SELECT * FROM Paper WHERE PaperID =@PaperID", aDict);
            aDict.Clear();

            Label6.Text = dt_P.Rows[0]["isUse"].ToString();
            Label5.Text = Request.QueryString["sno"].ToString();
            Label2.Text = dt_P.Rows[0]["PaperName"].ToString();
            Label3.Text = dt_P.Rows[0]["PaperDetail"].ToString();

            foreach (RepeaterItem row in Repeater1.Items)
            {
                Label Label7 = (Label)row.FindControl("Label7");

                switch (Label7.Text)
                {
                    case "0":
                        Label7.Text = "問答題";
                        break;
                    case "1":
                        Label7.Text = "單選題";
                        break;
                    case "2":
                        Label7.Text = "多選題";
                        break;
                    case "3":
                        Label7.Text = "簡單輸入題";
                        break;
                    default:
                        break;
                }

            }


            if (dt_P.Rows[0]["isUse"].ToString() == "1")
            {
                Response.Write("<script>alert('此表單已啟用，更動修改任何題目及選項!'); </script>");

                foreach (RepeaterItem row in Repeater1.Items)
                {
                    LinkButton btnDEL = (LinkButton)row.FindControl("btnDEL");
                    
                    btnDEL.Visible = false;
                    
                }

            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    }

    protected void bindData(int page)
    {
        
        String sql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY Sort ASC )
                as ROW_NO,* FROM Question
            Where PaperID= @PaperID
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        wDict.Add("PaperID", Request.QueryString["sno"].ToString());



        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);

        if (objDT.Rows.Count == 0)
        {
            Label4.Visible = true;
            Panel1.Visible = false;
        }
        else
        {
            Label4.Visible = false;
            Panel1.Visible = true;
        }

        
        Repeater1.DataSource = objDT.DefaultView;
        Repeater1.DataBind();
        
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {
       
        LinkButton btn = (LinkButton)sender;
        String id = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("id", id);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete Question Where QuestionID=@id", aDict);
        
        return;
    }

 

    
}