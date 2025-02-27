using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_SystemRole : System.Web.UI.Page
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
            //取得單位類別
            //Utility.setOrganLevel(ddl_OrganLevel, "", "全部");

            hidSystem.Value = Convert.ToString(Request.QueryString["st"]);
            bindData(1); 
        }
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String SRID = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("SRID", SRID);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete SYSRole Where SRID=@SRID", aDict);
        btnPage_Click(sender, e);
        return;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        Response.Redirect("SystemRole_AE.aspx?st=" + Request.QueryString["st"]);
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
            SELECT ROW_NUMBER() OVER (ORDER BY R.SROrganLevel,R.SRNAME) as ROW_NO, R.SRID, R.SRNAME, O.LEVEL_NAME
            FROM SYSRole R
            LEFT JOIN CD_ORGAN O ON O.LEVEL_CODE=R.SROrganLevel
            WHERE 1=1 
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();

        sql += " AND R.SYSTEM = @SYSTEM ";
        wDict.Add("SYSTEM", hidSystem.Value);

        //角色名稱
        if (!String.IsNullOrEmpty(txt_SRNAME.Text))
        {
            sql += " AND R.SRNAME Like '%' + @SRNAME + '%' ";
            wDict.Add("SRNAME", txt_SRNAME.Text.Trim());
        }
        //角色類別
        if (!String.IsNullOrEmpty(ddl_OrganLevel.SelectedValue))
        {
            sql += " AND R.SROrganLevel = @SROrganLevel ";
            wDict.Add("SROrganLevel", ddl_OrganLevel.SelectedValue);
        }
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Role.DataSource = objDT.DefaultView;
        gv_Role.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

    protected void getRole()
    {
        //取得UserInfo資訊
        userInfo = (UserInfo)Session["QSMS_UserInfo"];
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("RoleSNO", userInfo.RoleSNO);
        aDict.Add("PLINKSNO", 49); 
        DataHelper objDH = new DataHelper();
        DataTable objDT;
        objDT = objDH.queryData("Select * from RoleMenu where RoleSNO=@RoleSNO and PLINKSNO=@PLINKSNO", aDict); 
 
        if (Convert.ToString(objDT.Rows[0]["ISINSERT"]) != "1")
        {
            btnInsert.Visible = false;
        }
        if (Convert.ToString(objDT.Rows[0]["ISVIEW"]) != "1")
        {
            Utility.showMessage(Page, "ErrorMessage", "沒有檢視本頁的權限!");
            viewrole = 0;
        }
        if (Convert.ToString(objDT.Rows[0]["ISUPDATE"]) != "1")
        {
            gv_Role.Columns[gv_Role.Columns.Count - 3].Visible = false;
            gv_Role.Columns[gv_Role.Columns.Count - 2].Visible = false;
        }
        if (Convert.ToString(objDT.Rows[0]["ISDELETE"]) != "1")
        {
            gv_Role.Columns[gv_Role.Columns.Count - 1].Visible = false;
        }
    }
}