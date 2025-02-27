using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_CourseInfo : System.Web.UI.Page
{
    UserInfo userInfo = null;
    int viewrole = 1;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
        else Response.Redirect("../Default.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindData(1);
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

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        String sql = @"
            With MyCourse As (
                SELECT Distinct
                    (Case c.Class1 When 1 Then '核心課程' When 2 Then'專門課程' End) Class1,
                    (Case c.Class2 When 1 Then 'Knowledge' When 2 Then'Practice' End) Class2,
                    c.UnitName,
                    c.CourseName,
                    (Case c.CType When 1 Then '線上' When 2 Then'實體' When 3 Then '實習' End) CType,
                    c.CHour
                From QS_Course c
                     LEFT JOIN QS_CoursePlanningClass cpc on cpc.PClassSNO=c.PClassSNO
                     LEFT JOIN QS_CoursePlanningRole cpr on cpr.PClassSNO=c.PClassSNO
                     --LEFT JOIN Role R on R.RoleSNO=cpr.RoleSNO
                Where cpr.RoleSNO=@RoleSNO And IsEnable=1
            )
            Select ROW_NUMBER() OVER (ORDER BY Class1, Class2, UnitName, CourseName) as ROW_NO, * From MyCourse
            Order by Class1, Class2, UnitName, CourseName
        ";

        aDict.Add("RoleSNO", userInfo.RoleSNO);
        DataTable objDT = objDH.queryData(sql, aDict);



        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        rpt_CourseInfo.DataSource = objDT.DefaultView;
        rpt_CourseInfo.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);



    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    }


}