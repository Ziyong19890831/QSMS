using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class Web_TODOLIST : System.Web.UI.Page
{
    UserInfo userInfo = null;
    
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
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
        //查出該筆待辦事項
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        if (page < 1) page = 1;
        //int pageRecord = 10;
        String sql = @"
            SELECT
	ROW_NUMBER () OVER (ORDER BY PD.getPersonSno) AS ROW_NO ,PD.*,PS.PName,CONVERT(varchar(100),Createdate, 23) AS Createdate1
FROM
	TODO PD
LEFT JOIN Person PS ON PS.PersonSNO = PD.postPersonSNO
WHERE
	 TODOSNO = @TODOSNO
            ";

        aDict.Add("TODOSNO", Request.QueryString["sno"].ToString());

        DataTable objDT = objDH.queryData(sql, aDict);

        if (objDT.Rows.Count != 0)
        {
            if (objDT.Rows[0]["STATE"].ToString() == "0") //如果此篇為未讀則改變為已讀文章
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                DataHelper dher = new DataHelper();
                dic.Add("TODOSNO", Request.QueryString["sno"].ToString());
                string sqlu = @"update TODO set STATE=1 where TODOSNO=@TODOSNO ";
                dher.executeNonQuery(sqlu, dic);
            }
            lbl_ptitle.Text = objDT.Rows[0]["TODOTITLE"].ToString();
            lbl_postname.Text = objDT.Rows[0]["PName"].ToString();
            lbl_date.Text = objDT.Rows[0]["Createdate1"].ToString();
            lbl_content.Text = objDT.Rows[0]["TODOTEXT"].ToString();
        }
        
    }

   

}