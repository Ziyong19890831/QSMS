using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_ReportLearning : System.Web.UI.Page
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
            Utility.setAllELearn(ddl_Elearning, "請選擇");
        }
    }

    /// <summary>
    /// 設定此報表可匯出名稱設定 ,  (注意 : BindData 欄位要包含設定可匯出資料)
    /// </summary>
    private void ReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("ELSName", "E-Learning課程名稱");
        _SetCol.Add("ELSPart", "完成節數");
        _SetCol.Add("PName", "學員名稱");
        _SetCol.Add("PersonID", "學員身分證");
        _SetCol.Add("FinishedDate", "課程完成日");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.ReportLearning.ToString()] = _ExcelInfo;
    }


    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;

        String sql = @"            
            SELECT ROW_NUMBER() OVER (ORDER BY lr.LearnSNO Desc) as ROW_NO,e.ELName ,
                ces.ELSName, cast(lr.ELSPart as nvarchar)+'/'+cast(ces.ELSPart as nvarchar) ELSPart, p.PName, P.PersonID, CONVERT(VARCHAR(19), lr.FinishedDate , 121) FinishedDate
				,
				 STUFF ( P.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption'
            FROM QS_LearningRecord lr
                LEFT JOIN QS_CourseELearningSection ces ON ces.ELSCode = lr.ELSCode
                LEFT JOIN QS_CourseELearning e on e.ELCode=ces.ELCode
                LEFT JOIN Person P on P.PersonID = lr.PersonID
                LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
                LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
            WHERE 1=1
        ";

        Dictionary<string, object> wDict = new Dictionary<string, object>();


        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(wDict, userInfo);
        #endregion


        #region 查詢篩選區塊
        if (!string.IsNullOrEmpty(txt_CourseName.Text))
        {
            sql += " AND ces.ELSName Like '%' + @ELSName + '%' ";
            wDict.Add("ELSName", txt_CourseName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_Person.Text))
        {
            sql += " AND p.PName Like '%' + @PName + '%' ";
            wDict.Add("PName", txt_Person.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " AND p.PersonID Like '%' + @PersonID + '%' ";
            wDict.Add("PersonID", txt_PersonID.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_SFinishedDate.Text))
        {
            sql += " AND DATEDIFF(D, @SFinishedDate, lr.FinishedDate) >= 0";
            wDict.Add("SFinishedDate", txt_SFinishedDate.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_EFinishedDate.Text))
        {
            sql += " AND DATEDIFF(D, @EFinishedDate, lr.FinishedDate) <= 0";
            wDict.Add("EFinishedDate", txt_EFinishedDate.Text.Trim());
        }
        if(!string.IsNullOrEmpty(ddl_Elearning.SelectedValue))
        {
            sql += " AND e.ELCode=@ELCode ";
            wDict.Add("ELCode", ddl_Elearning.SelectedValue);
        }
        #endregion


        sql += " Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        //gv_Course.DataSource = objDT.DefaultView;
        //gv_Course.DataBind();
        gv_LearningRecord.DataSource = objDT.DefaultView;
        gv_LearningRecord.DataBind();
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
        if (gv_LearningRecord.Rows.Count == 0)
        {
            Response.Write("<script>alert('尚未查詢')</script>");
            return;
        }
        Utility.OpenExportWindows(this, ReportEnum.ReportLearning.ToString());
    }

}