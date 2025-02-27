using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_QA : System.Web.UI.Page
{
    UserInfo userInfo = null;
    
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            setQAClassCode(ddl_QAClass, "---請選擇分類---");
            //setClassSystem(ddl_SystemName, "---請選擇系統---", userInfo);
            bindData(1);
        }
    }
    protected void bindData(int page)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        if (page < 1) page = 1;
        int pageRecord = 10;
        String sql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY SDate) as ROW_NO, QASNO, Q.CreateDT, Title, C.Name as ClassName, S.SYSTEM_NAME          
            FROM QA Q
                LEFT JOIN QAClass C on Q.QACSNO=C.QACSNO
                LEFT JOIN SYSTEM S on S.SYSTEM_ID=Q.SYSTEM_ID
            ";

        if (!String.IsNullOrEmpty(ddl_QAClass.SelectedValue))
        {
            sql += " And Q.QACSNO Like '%' + @QACSNO + '%' ";
            aDict.Add("QACSNO", ddl_QAClass.SelectedValue);
        }
        if (!String.IsNullOrEmpty(txt_Search.Text))
        {
            sql += " And Title Like '%' + @Title + '%' ";
            aDict.Add("Title", txt_Search.Text);
        }

        sql += @" Where EDate>=GETDATE() ";


        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        rpt_QA.DataSource = objDT.DefaultView;
        rpt_QA.DataBind();
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
    public static void setQAClassCode(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER(ORDER BY QACSNO) as ROW_NO, QACSNO, Name FROM QAClass", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }
    public static void setClassSystem(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null, UserInfo userInfo = null)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();

        DataTable objDT = objDH.queryData(@"SELECT ROW_NUMBER() OVER(ORDER BY SYSTEMSNO) as ROW_NO, SYSTEMSNO, S.SYSTEM_ID, SYSTEM_NAME
                                            FROM SYSTEM S
                                            where ISEnable > 0 AND SYSTEM_ID='S00'", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        ddl.Visible = false;
        //ddl.SelectedValue = "S00";

        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }

    }
}