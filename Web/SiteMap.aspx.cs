using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_SiteMap : System.Web.UI.Page
{
    UserInfo userInfo = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["QSMS_UserInfo"] != null)
            {
                userInfo = (UserInfo)Session["QSMS_UserInfo"];
                bindG0();
                bindG1();
                bindG2();
            }
            else
            {
                bindG1();
                bindG2();

                //li_Login.Visible = false;
            }
        }
    }

    public void bindG0()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string SQL = @"Select * from SiteMap where MapGroup=0 ";
        if (txtSearch.Value != "")
        {
            SQL += " And LinkName Like '%' + @LinkName + '%' ";
            aDict.Add("LinkName", txtSearch.Value);
        }
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        rpt_SiteMapG0.DataSource = ObjDT;
        rpt_SiteMapG0.DataBind();
    }

    public void bindG1()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();

        string SQL = @"Select * from SiteMap where MapGroup=1 ";
        if (txtSearch.Value != "")
        {
            SQL += " And LinkName Like '%' + @LinkName + '%' ";
            aDict.Add("LinkName", txtSearch.Value);
        }
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        rpt_SiteMapG1.DataSource = ObjDT;
        rpt_SiteMapG1.DataBind();
    }

    public void bindG2()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();

        string SQL = @"Select * from SiteMap where MapGroup=2 ";
        if (txtSearch.Value != "")
        {
            SQL += " And LinkName Like '%' + @LinkName + '%' ";
            aDict.Add("LinkName", txtSearch.Value);
        }
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        rpt_SiteMapG2.DataSource = ObjDT;
        rpt_SiteMapG2.DataBind();
    }

    protected void btn_Search_ServerClick(object sender, EventArgs e)
    {
        bindG0();
        bindG1();
        bindG2();
    }
}