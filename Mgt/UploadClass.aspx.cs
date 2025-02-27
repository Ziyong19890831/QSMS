using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_UploadClass : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    int viewrole = 1;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
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
        DataTable finddata = objDH.queryData("Select 1 From Download Where DLCSNO=@id", aDict);
        if (finddata.Rows.Count > 0)
        {
            Utility.showMessage(Page, "注意！", "很抱歉，該[下載專區類別]已繫結[檔案]，請先至[下載專區作業]取消繫結後再刪除。");
            return;
        }
        else
        {
            objDH.executeNonQuery("Delete DownloadClass Where DLCSNO=@id", aDict);
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
        if (viewrole != 1) return;
        if (page < 1) page = 1;
        int pageRecord = 5;

        String sql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY -OrderSeq DESC, DLCSNO) as ROW_NO, DC.DLCSNO,DC.OrderSeq,DC.DLCNAME,S.SYSTEM_NAME
            FROM DownloadClass DC
			    LEFT JOIN SYSTEM S on DC.SYSTEM_ID=S.SYSTEM_ID
			WHERE DLCSNO>0  
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();


        #region 權限篩選區塊
        Utility.setSQLAccess_ByCreateUserID(wDict, userInfo, "DC.");
        #endregion



        #region 查詢篩選區塊
        if (!String.IsNullOrEmpty(txt_Search.Text))
        {
            sql += " AND DLCNAME Like '%' + @Name + '%' ";
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