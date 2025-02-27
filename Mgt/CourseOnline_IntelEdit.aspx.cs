using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_CourseOnline_IntelEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            binddate();
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = "Update Config set Mval=@Mval where PID='CourseIntel'";
        adict.Add("Mval", editor1.Value);
        ObjDH.executeNonQuery(sql, adict);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "alert", "alert('修改成功')", true);
    }
    
    public void binddate()
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string Sql = "Select * from Config where [PID]='CourseIntel'";
        DataTable ObjDT = ObjDH.queryData(Sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            editor1.Value = HttpUtility.HtmlDecode(Convert.ToString(ObjDT.Rows[0]["Mval"]));
           
        }
        else
        {
            editor1.Value = "系統錯誤，請聯繫工程師。";
            editor1.Disabled = true;
        }
        
    }
}