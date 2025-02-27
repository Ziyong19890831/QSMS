using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_SystemPageLink : System.Web.UI.Page
{
    UserInfo userInfo = null;
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
            hidSystem.Value = Convert.ToString(Request.QueryString["st"]);
            bindData(1); 
        }
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String SPLID = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("SPLID", SPLID);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete SYSPageLink Where SPLID=@SPLID", aDict);
        btnPage_Click(sender, e);
        return;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        Response.Redirect("SystemPageLink_AE.aspx?st=" + Request.QueryString["st"]);
    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(hidPage.Value, out page);
        bindData(page);
    }

    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        String sql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY SPLALIAS, SPLNAME) as ROW_NO, SPLID, SPLNAME, SPLALIAS, SPLURL 
            FROM SYSPageLink
            WHERE 1=1 
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();

        sql += " AND SYSTEM = @SYSTEM ";
        wDict.Add("SYSTEM", hidSystem.Value);

        //頁面名稱
        if (!String.IsNullOrEmpty(txt_PLinkName.Text))
        {
            sql += " AND SPLNAME Like '%' + @SPLNAME + '%' ";
            wDict.Add("SPLNAME", txt_PLinkName.Text.Trim());
        }
        //頁面別名
        if (!String.IsNullOrEmpty(txt_PLinkAlias.Text))
        {
            sql += " AND SPLALIAS Like '%' + @SPLALIAS + '%' ";
            wDict.Add("SPLALIAS", txt_PLinkAlias.Text.Trim());
        } 
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_PageLink.DataSource = objDT.DefaultView;
        gv_PageLink.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

  
}