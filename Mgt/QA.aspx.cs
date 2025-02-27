using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_QA : System.Web.UI.Page
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
            setQAClass(ddl_Class, "請選擇類型");
            setClassSystem(ddl_SystemName, "請選擇系統");
            bindData(1);
        }
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String id = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("QASNO", id);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete QA Where QASNO=@QASNO", aDict);
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
        String sql = @"
            select ROW_NUMBER() OVER (ORDER BY Q.CreateDT DESC )
                as ROW_NO,Q.QASNO, Q.QACSNO,Q.Title,Q.CreateUserID,Q.CreateDT,Q.Info,C.Name as ClassName,P.PName,S.SYSTEM_NAME
            from QA Q
                LEFT JOIN QAClass C ON Q.QACSNO=C.QACSNO
			    LEFT JOIN Person P on Q.CreateUserID=P.PersonSNO
                LEFT JOIN SYSTEM S on Q.SYSTEM_ID=S.SYSTEM_ID
            Where 1=1
        ";
        Dictionary<string, object> aDict = new Dictionary<string, object>();

        #region 權限篩選區塊
        Utility.setSQLAccess_ByCreateUserID(aDict, userInfo, "Q.");
        #endregion
        

        #region 查詢篩選區塊
        if (!String.IsNullOrEmpty(txt_searchTitle.Text))
        {
            sql += " And Title  Like '%' + @Title + '%' ";
            aDict.Add("Title", txt_searchTitle.Text);
        }
        if(!String.IsNullOrEmpty(ddl_Class.SelectedValue))
        {
            sql += " And Q.QACSNO=@QACSNO ";
            aDict.Add("QACSNO", ddl_Class.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_SystemName.SelectedValue))
        {
            sql += " And Q.SYSTEM_ID=@SYSTEM_ID ";
            aDict.Add("SYSTEM_ID", ddl_SystemName.SelectedValue);
        }
        #endregion


        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_QA.DataSource = objDT.DefaultView;
        gv_QA.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

    public static void setQAClass(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY QACSNO) as ROW_NO,QACSNO,Name  FROM QAClass", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }

    public static void setClassSystem(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER(ORDER BY SYSTEMSNO) as ROW_NO, SYSTEMSNO, SYSTEM_ID, SYSTEM_NAME FROM SYSTEM where ISEnable > 0 ", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }
}