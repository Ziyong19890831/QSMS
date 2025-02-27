using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_CoursePlanning : System.Web.UI.Page
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
            SetDdlCertificateType(ddl_CType, "請選擇");
            ddl_IsEnable.Items.Insert(0, new ListItem("請選擇", ""));
            ddl_IsEnable.Items.Insert(1, new ListItem("是", "1"));
            ddl_IsEnable.Items.Insert(2, new ListItem("否", "0"));
            bindData(1);
        }
    }

    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        string sql = @"
			With getAllCoursePlanningClass As (
				SELECT 
					cpc.PClassSNO,
					cpc.PlanName,
					Cast(CStartYear as varchar(4)) + '-' + Cast(CEndYear as varchar(4)) As 'CYear',
					(Case cpc.IsEnable When 1 Then '是' When 0 Then'否' End) IsEnables,
					ct.CTypeName,
                    ct.CTypeSNO,
					c.CHour,
					Substring(
					(
						Select ',' + RoleName 
						From 
							(Select cpr.PClassSNO, r.RoleName From QS_CoursePlanningRole cpr Left Join Role r ON r.RoleSNO=cpr.RoleSNO) t
						Where t.PClassSNO=cpc.PClassSNO For XML PATH ('')
					),2,100) as CRole,cpc.[TargetIntegral]
				From QS_CoursePlanningClass cpc
					Left Join QS_CertificateType ct ON ct.CTypeSNO=cpc.CTypeSNO
					Left Join QS_Course c ON c.PClassSNO=cpc.PClassSNO
			)

			--取得所有課程規劃類別之統計時數
			, getSumHours As (
				Select 
					PClassSNO, PlanName, CYear, IsEnables, CTypeName, CTypeSNO, CRole, Sum(CHour) sumHour, Count(CHour) countCourse
					,apc.[TargetIntegral]
				From getAllCoursePlanningClass apc
				Group By PClassSNO, PlanName, CYear, IsEnables, CTypeName, CTypeSNO, CRole,apc.[TargetIntegral]
			)

			Select ROW_NUMBER() OVER (ORDER BY PClassSNO) as ROW_NO, * 
			From getSumHours gs
			Where 1=1
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(txt_PlanName.Text))
        {
            sql += " AND gs.PlanName Like '%' + @PlanName + '%' ";
            wDict.Add("PlanName", txt_PlanName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(ddl_CType.SelectedValue))
        {
            sql += " AND gs.CTypeSNO = @CTypeSNO ";
            wDict.Add("CTypeSNO", ddl_CType.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddl_IsEnable.SelectedValue))
        {
            sql += " AND gs.IsEnable = @IsEnable ";
            wDict.Add("IsEnable", ddl_IsEnable.SelectedValue);
        }

        sql += " Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_CourseClass.DataSource = objDT.DefaultView;
        gv_CourseClass.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

    public static void SetDdlCertificateType(DropDownList ddl, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select A.CTypeSNO , A.CTypeName  FROM QS_CertificateType A", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
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
        DataTable finddata = objDH.queryData("Select 1 From QS_Course Where PClassSNO=@PClassSNO", aDict);
        if (finddata.Rows.Count > 0)
        {
            Utility.showMessage(Page, "注意！", "很抱歉，該[證書規劃類別]已繫結[課程]，請先至[課程維護]取消繫結後再刪除。");
            return;
        }
        else
        {
            string delSql = @"Delete FROM QS_CoursePlanningClass Where PClassSNO=@PClassSNO ;
                              Delete FROM QS_CoursePlanningRole Where PClassSNO=@PClassSNO ; ";
            objDH.executeNonQuery(delSql, aDict);
            Utility.showMessage(Page, "訊息", "刪除成功。");
            btnPage_Click(sender, e);
            return;
        }
    }
}