using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_RoleMenu : System.Web.UI.Page
{
    public UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindData(1);
        }
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String RoleID = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("RoleSNO", RoleID);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete RoleMenu Where RoleSNO=@RoleSNO", aDict);
        btnPage_Click(sender, e);
        return;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        bindData(page);
    }

    protected void bindData(int page)
    {
        if (page < 1) page = 1;
        int pageRecord = 20;
        String sql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY RoleName) as ROW_NO, RoleSNO, RoleName
            FROM Role WHERE 1=1 
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        if (!String.IsNullOrEmpty(txt_RoleName.Text))
        {
            sql += " AND RoleName Like '%' + @RoleName + '%' ";
            wDict.Add("RoleName", txt_RoleName.Text.Trim());
        }
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_RoleMenu.DataSource = objDT.DefaultView;
        gv_RoleMenu.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }
}