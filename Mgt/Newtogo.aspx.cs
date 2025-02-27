using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Newtogo : System.Web.UI.Page
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
            setClassSystem(ddl_SystemName, "請選擇系統");
            bindData(1);
        }
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String id = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("id", id);
        DataHelper objDH = new DataHelper();
        Utility.deleteNewhandFile(Server.MapPath("../Newhand"), id);
        objDH.executeNonQuery("Delete NewHand Where NHSNO=@id", aDict);
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
            select ROW_NUMBER() OVER (ORDER BY NHSNO ASC) ROW_NO, NHSNO, S.SYSTEM_NAME , NHName, NHPath, N.ISENABLE, N.CreateDT, P.PName
            from NewHand N
                LEFT JOIN SYSTEM S on N.SYSTEM_ID=S.SYSTEM_ID
			    LEFT JOIN Person P on N.CreateUserID=P.PersonSNO
			WHERE 1=1
        ";
        Dictionary<string, object> aDict = new Dictionary<string, object>();


        #region 權限篩選區塊
        Utility.setSQLAccess_ByCreateUserID(aDict, userInfo, "N.");
        #endregion


        #region 查詢篩選區塊
        if (!String.IsNullOrEmpty(txt_searchTitle.Text))
        {
            sql += "And NHName  Like '%' + @NHName + '%' ";
            aDict.Add("NHName", txt_searchTitle.Text);
        }
        if (!String.IsNullOrEmpty(ddl_SystemName.SelectedValue))
        {
            sql += " And N.SYSTEM_ID=@SYSTEM_ID ";
            aDict.Add("SYSTEM_ID", ddl_SystemName.SelectedValue);
        }
        #endregion

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Notice.DataSource = objDT.DefaultView;
        gv_Notice.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
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