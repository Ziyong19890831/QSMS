using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Link : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["IsLogin"] != null)
            {

            }
            setClassCode(ddl_Url_Class, "----請選擇----");
            GetLink();
        }
    }

    protected void GetLink()
    {
        String sql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY URLCSNO) as ROW_NO, URLCSNO ,Name        
            From UrlClass WHERE 1=1
            ";
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        if (!String.IsNullOrEmpty(ddl_Url_Class.SelectedValue))
        {
            sql += " And Name Like '%' + @Name + '%' ";
            aDict.Add("Name", ddl_Url_Class.SelectedValue);
        }
        DataTable objDT = objDH.queryData(sql, aDict);
        rpt_Link.DataSource = objDT.DefaultView;
        rpt_Link.DataBind();
    }

    protected void rpt_Link_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var FoundRepeater = e.Item.FindControl("rpt_SubLink") as Repeater;
            if (FoundRepeater != null)
            {
                SubLinkByCategory(FoundRepeater, DataBinder.Eval(e.Item.DataItem, "URLCSNO").ToString());
            }
        }
    }

    protected void SubLinkByCategory(Repeater theRepeater, string param)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("URLCSNO", param);
        string strSqlQry = @"Select Url, Name from Url Where URLCSNO = @URLCSNO ORDER BY URLCSNO";
        DataHelper objDH = new DataHelper();
        theRepeater.DataSource = objDH.queryData(strSqlQry, dic);
        theRepeater.DataBind();
    }
    public static void setClassCode(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT *  FROM UrlClass", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetLink();
    }
}