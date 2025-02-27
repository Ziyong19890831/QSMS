using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Config : System.Web.UI.Page
{
    public UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindData();
        }
    }

    //protected void btnDEL_Click(object sender, EventArgs e)
    //{
    //    LinkButton btn = (LinkButton)sender;
    //    String id = btn.CommandArgument;
    //    Dictionary<string, object> aDict = new Dictionary<string, object>();
    //    aDict.Add("id", id);
    //    DataHelper objDH = new DataHelper();
    //    objDH.executeNonQuery("Delete Notice Where NoticeSNO=@id", aDict);
    //    Response.Write("<script>alert('刪除成功!') </script>");
    //    return;
    //}

    protected void btnSave_Click(object sender, EventArgs e)
    {
        
    }

    protected void bindData()
    {

        string sql = @"
            select 
                ROW_NUMBER() OVER (ORDER BY PGroup, PID) as ROW_NO, PID, PGroup, PVal, MVal, Note
            from Config
        ";
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, aDict);
        gv_Config.DataSource = objDT.DefaultView;
        gv_Config.DataBind();

    }


}