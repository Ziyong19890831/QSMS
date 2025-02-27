using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Role : System.Web.UI.Page
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
        }
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String RoleID = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("RoleSNO", RoleID);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete Role Where RoleSNO=@RoleSNO", aDict);
        Response.Write("<script>alert('刪除成功!') </script>");
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
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        string sql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY RoleLevel) as ROW_NO, 
                R.RoleSNO, R.RoleName, R.RoleOrganType, R.RoleLevel, R.RoleGroup, IsAdmin,
                (Case IsAdmin When 1 Then '是' Else '否' End) IsAdminN
            FROM Role R
            WHERE 1=1 
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        //角色名稱
        if (!String.IsNullOrEmpty(txt_RoleName.Text))
        {
            sql += " AND RoleName Like '%' + @RoleName + '%' ";
            wDict.Add("RoleName", txt_RoleName.Text.Trim());
        }
        sql += " Order by RoleLevel, RoleGroup ";


        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Role.DataSource = objDT.DefaultView;
        gv_Role.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }
    
}