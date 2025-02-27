using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Mgt_CertificateUnit_Manager : System.Web.UI.Page
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

    }

    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        string sql = @"
			Select  ROW_NUMBER() OVER (ORDER BY [CUnitSNO]) as ROW_NO,* from QS_CertificateUnit QCU where IsAdmin=1
            
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(txt_CertUnit.Text))
        {
            sql += " AND QCU.CUnitName Like '%' + @CUnitName + '%' ";
            wDict.Add("CUnitName", txt_CertUnit.Text.Trim());
        }

        sql += " Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_CertUnit.DataSource = objDT.DefaultView;
        gv_CertUnit.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
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
    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String id = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PClassSNO", id);
        DataHelper objDH = new DataHelper();

        string delSql = @"Delete FROM QS_CoursePlanningClass Where PClassSNO=@PClassSNO ;
                              Delete FROM QS_CoursePlanningRole Where PClassSNO=@PClassSNO ; ";
        objDH.executeNonQuery(delSql, aDict);
        Utility.showMessage(Page, "訊息", "刪除成功。");
        btnPage_Click(sender, e);
        return;

    }
}