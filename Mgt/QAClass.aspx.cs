using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_QA_Class : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    int viewrole = 1;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"]; ;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindData(1);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String id = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("id", id);
        DataHelper objDH = new DataHelper();
        DataTable finddata = objDH.queryData("Select 1 From QA Where QACSNO=@id", aDict);
        if (finddata.Rows.Count > 0)
        {
            Utility.showMessage(Page, "注意！", "很抱歉，該[Q&A類別]已繫結[Q&A]，請先至[Q&A]取消繫結後再刪除。");
            return;
        }
        else
        {
            objDH.executeNonQuery("Delete QAClass Where QACSNO=@id", aDict);
            Utility.showMessage(Page, "訊息", "刪除成功。");
            btnPage_Click(sender, e);
            return;
        }
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
        String sql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY QACSNO) as ROW_NO, QACSNO,Name,Note
            FROM QAClass WHERE 1=1 
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();

        #region 權限篩選區塊
        Utility.setSQLAccess_ByCreateUserID(wDict, userInfo, "");
        #endregion


        #region 查詢篩選區塊
        if (!String.IsNullOrEmpty(txt_Search.Text))
        {
            sql += " AND Name Like '%' + @Name + '%' ";
            wDict.Add("Name", txt_Search.Text);
        }
        #endregion

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_NoticeClass.DataSource = objDT.DefaultView;
        gv_NoticeClass.DataBind();
        ltl_page.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

}