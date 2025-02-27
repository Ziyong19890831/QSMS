using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_NewHand : System.Web.UI.Page
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
            Utility.setClassRoleName(dd2_RoleName, "---適用人員全選---");
            bindData(1);
        }
    }

    protected void bindData(int page)
    {
        if (page < 1) page = 1;
        int pageRecord = 10;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        String sql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY N.CreateDT DESC) ROW_NO,
                NHName, NHPath, N.ISENABLE, N.CreateDT,
                " + Utility.setSQL_RoleBindName("Newtogo_AE", "N.NHSNO") + @"
            from NewHand N
            Where 1=1 
        ";

        if (!String.IsNullOrEmpty(txtSearch.Value))
        {
            sql += " And NHName Like '%' + @NHName + '%' ";
            aDict.Add("NHName", txtSearch.Value);
        }
        if (!String.IsNullOrEmpty(dd2_RoleName.SelectedValue))
        {
            sql += " and (select 1 from RoleBind RB where RB.CSNO=N.NHSNO and RB.TypeKey='Newtogo_AE' and RB.RoleSNO=@dd2_RoleName )=1";
            aDict.Add("dd2_RoleName", dd2_RoleName.SelectedValue);
        }

        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        rpt_NewHand.DataSource = objDT.DefaultView;
        rpt_NewHand.DataBind();
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

    public static void setClassSystem(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null, UserInfo userInfo = null)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable objDT = objDH.queryData(@"SELECT ROW_NUMBER() OVER(ORDER BY SYSTEMSNO) as ROW_NO, SYSTEMSNO, S.SYSTEM_ID, SYSTEM_NAME
                                            FROM SYSTEM S
                                            where ISEnable > 0 ", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
       
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }
}