using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_ReportExamOnline : System.Web.UI.Page
{
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfo;
    public Dictionary<string, string> _SetCol;
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
            //Utility.setPlanName(dd1_ClassName, "請選擇課程類別");
            Utility.setAllELearn(ddl_ELName, "請選擇Elearn課程");
            //bindData(1);
        }
    }

    /// <summary>
    /// 設定此報表可匯出名稱設定 ,  (注意 : BindData 欄位要包含設定可匯出資料)
    /// </summary>
    private void ReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("ELName", "E-Learn");
        _SetCol.Add("PlanName", "課程規劃類別");
        _SetCol.Add("QuizName", "測驗名稱");
        _SetCol.Add("PName", "學員名稱");
        _SetCol.Add("PersonID", "學員身分證");
        _SetCol.Add("Score", "成績");
        _SetCol.Add("ExamDate", "測驗日期");
        _SetCol.Add("IsPass", "是否通過");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.ReportExamOnline.ToString()] = _ExcelInfo;
    }

    protected void bindData(int page)
    {

        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        String sql = @"
            SELECT ROW_NUMBER() over(order by ExamDate Desc) ROW_NO
				,QCPC.PlanName
			    ,ce.ELName
	            ,ls.QuizName
	            ,P.PName, P.PersonID,STUFF ( P.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption'
                ,ls.Score
                ,CONVERT(VARCHAR(19), ls.ExamDate , 121) ExamDate
                ,ls.PassScore
                ,case ls.IsPass when 1 then '是'　when 0 then '否'　end IsPass
            FROM QS_LearningScore ls
			    LEFT JOIN QS_CourseELearning ce on ce.ELcode=ls.ELcode
				Left JOIN QS_Course QC ON ls.ELSCode=QC.ELSCode
				Left JOIN QS_CoursePlanningClass QCPC ON QCPC.PClassSNO=QC.PClassSNO
                LEFT JOIN Person P on P.PersonID=ls.PersonID
                LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
                LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
            where 1=1
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();



        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(wDict, userInfo);
        #endregion


        #region 查詢篩選區塊
        if (!string.IsNullOrEmpty(txt_PName.Text))
        {
            sql += " AND P.Pname=@Pname ";
            wDict.Add("Pname", txt_PName.Text);
        }
        if (!string.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " AND p.PersonID Like '%' + @PersonID + '%' ";
            wDict.Add("PersonID", txt_PersonID.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_QuizName.Text))
        {
            sql += " AND ls.QuizName Like '%' + @QuizName  + '%' ";
            wDict.Add("QuizName", txt_QuizName.Text);
        }
        //if (!string.IsNullOrEmpty(dd1_ClassName.SelectedValue))
        //{
        //    sql += " AND cpc.PlanName  = @PlanName ";
        //    wDict.Add("PlanName", dd1_ClassName.SelectedItem.Text);
        //}
        if (!string.IsNullOrEmpty(Date_start.Text))
        {
            sql += " AND ExamDate > @Date_start ";
            wDict.Add("Date_start", Date_start.Text);
        }
        if (!string.IsNullOrEmpty(Date_End.Text))
        {
            sql += " AND ExamDate < @Date_End ";
            wDict.Add("Date_End", Date_End.Text);
        }
        if (!string.IsNullOrEmpty(ddl_ELName.SelectedValue))
        {
            sql += " AND ce.ELCode = @ELCode ";
            wDict.Add("ELCode", ddl_ELName.SelectedValue);
        }
        #endregion

        sql += " Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Course.DataSource = objDT.DefaultView;
        gv_Course.DataBind();
        //設定匯出資料
        ReportInit(objDT);
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
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (gv_Course.Rows.Count == 0)
        {
            Response.Write("<script>alert('尚未查詢')</script>");
            return;
        }
        Utility.OpenExportWindows(this, ReportEnum.ReportExamOnline.ToString());

    }

}