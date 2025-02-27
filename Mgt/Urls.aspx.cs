using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Urls : System.Web.UI.Page
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
            setUrlClass(ddl_Class, "請選擇");
            bindData(1);
        }
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String id = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("URLSNO", id);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete Url Where URLSNO=@URLSNO", aDict);
        Response.Write("<script>alert('刪除成功!') </script>");
        btnPage_Click(sender, e);
        return;
    }

    protected void btnSearch_Click(object snder, EventArgs e)
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
            select ROW_NUMBER() OVER (ORDER BY URLSNO DESC) as ROW_No,U.URLSNO, U.URLCSNO, U.Name,Url,C.Name  as ClassName
            from Url U
                LEFT JOIN UrlClass C on U.URLCSNO=C.URLCSNO
            where 1=1
        ";
        Dictionary<string, object> aDict = new Dictionary<string, object>();


        #region 權限篩選區塊
        Utility.setSQLAccess_ByCreateUserID(aDict, userInfo, "U.");
        #endregion



        #region 查詢篩選區塊
        if (!String.IsNullOrEmpty(txt_Search.Text))
        {
            sql += "And U.Name  Like '%' + @Name + '%' ";
            aDict.Add("Name", txt_Search.Text);
        }
        if (!String.IsNullOrEmpty(ddl_Class.SelectedValue))
        {
            sql += "And U.URLCSNO =@URLCSNO";
            aDict.Add("URLCSNO", ddl_Class.SelectedValue);
        }
        #endregion


        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Urls.DataSource = objDT.DefaultView;
        gv_Urls.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

    public static void setUrlClass(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY URLCSNO) as ROW_NO,URLCSNO,Name  FROM UrlClass", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }

  
}