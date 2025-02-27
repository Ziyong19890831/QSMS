using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_ReportCourseOnline : System.Web.UI.Page
{
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfo ;
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
            bindData(1);
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
        _SetCol.Add("CourseName", "課程名稱");
        _SetCol.Add("LearnCount", "完成人數");
        _SetCol.Add("FinishedDate", "課程完成日");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.ReportCourseOnline.ToString()] = _ExcelInfo;
    }

    protected void bindData(int page)
    {
       
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        String sql = @"

			--取得所有E-learningPart數
			With getAllParts As (
				Select ELSCode, ces.ELSPart
				From QS_CourseELearningSection ces 
				where ces.IsEnable=1
			)

			--取得學員上課之統計節數
			, getLearningParts As (
				Select PersonID, ELSCode, Count(1) FinishedParts 
				From (
					Select Distinct lr.PersonID, lr.ELSCode, lr.ELSPart
					From QS_LearningRecord lr 
						Left Join QS_CourseELearningSection ces ON ces.ELSCode=lr.ELSCode
						where ces.IsEnable=1
				) t
				Group By PersonID, ELSCode
			)
			
			--取得學員已完成課程之清單
			, getFinishedLearning As (
				Select 
					ap.ELSCode, ap.ELSPart, lp.PersonID
				From getAllParts ap
					Left Join getLearningParts lp ON lp.ELSCode=ap.ELSCode 
				Where ap.ELSPart=lp.FinishedParts 
			)
			
			--取得課程之總上課紀錄
			, getTotalLearningCount As (
				SELECT 
					ces.ELSCode, c.CourseName, ces.ELSName, Count(1) LearnCount
				FROM QS_CourseELearningSection ces
					Left JOIN getFinishedLearning fl ON fl.ELSCode=ces.ELSCode
					Left JOIN QS_Course c ON c.ELSCode=ces.ELSCode
				Where c.CourseName Is Not Null and ces.IsEnable=1
				Group By ces.ELSCode, c.CourseName, ces.ELSName
			)

			SELECT ROW_NUMBER() OVER (ORDER BY tlc.ELSCode) as ROW_NO, *
            From getTotalLearningCount tlc
            WHERE 1=1
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();


        #region 查詢篩選區塊
        if (!string.IsNullOrEmpty(txt_CourseName.Text))
        {
            sql += " AND tlc.CourseName Like '%' + @CourseName + '%' ";
            wDict.Add("CourseName", txt_CourseName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_UnitName.Text))
        {
            sql += " AND tlc.UnitName Like '%' + @UnitName + '%' ";
            wDict.Add("UnitName", txt_UnitName.Text.Trim());
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
        Utility.OpenExportWindows(this , ReportEnum.ReportCourseOnline.ToString());
       
    }

}