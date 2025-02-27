using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_SearchCode : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Code"] != null)
        {
            gv_Code.Visible = true;
            string Code = Request.QueryString["Code"].ToString();
            DataHelper ObjDH = new DataHelper();
            Dictionary<string, object> adict = new Dictionary<string, object>();
            string sql = @"Select * from QS_Course QC
                           Left Join Config C On C.PVal=QC.Ctype and C.PGroup='CourseCType'
                            where PClassSNO=@PClassSNO
                            ";
            adict.Add("PClassSNO", Code);
            DataTable ObjDT = ObjDH.queryData(sql, adict);
            gv_Code.DataSource = ObjDT;
            gv_Code.DataBind();
        }
        if (Request.QueryString["PClassSNO"] != null)
        {
            gv_PClassSNO.Visible = true;
            DataHelper ObjDH = new DataHelper();
            Dictionary<string, object> adict = new Dictionary<string, object>();
            string sql = @"Select * from [QS_CoursePlanningClass] ";
            DataTable ObjDT = ObjDH.queryData(sql, null);
            gv_PClassSNO.DataSource = ObjDT;
            gv_PClassSNO.DataBind();
        }
    }
}