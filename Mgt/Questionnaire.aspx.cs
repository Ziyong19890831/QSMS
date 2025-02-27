using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Questionnaire : System.Web.UI.Page
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
            //bindData(1);
        }
    }
    private void ReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("ELSName", "E-Learning課程名稱");
        _SetCol.Add("PName", "學員名稱");
        _SetCol.Add("PersonID", "學員身分證");
        _SetCol.Add("RoleName", "身份別");
        _SetCol.Add("CompletedDate", "課程完成日");
        _SetCol.Add("Class", "類別");
        _SetCol.Add("QName", "問卷題目");
        _SetCol.Add("Ans", "學員答案");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.Questionnaire.ToString()] = _ExcelInfo;
    }

    private void ReportInitDetail(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("ELSName", "E-Learning課程名稱");
        _SetCol.Add("PName", "學員名稱");
        _SetCol.Add("PersonID", "學員身分證");
        _SetCol.Add("CompletedDate", "課程完成日");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.Questionnaire.ToString()] = _ExcelInfo;
    }


    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;

        String sql = @"            
                         WITH Base AS (
         Select distinct P.PersonID,CES.ELSName,P.PName,LA.CompletedDate,LF.ELSCode,P.OrganSNO,P.RoleSNO,STUFF ( P.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption'
	     from Person P
	     Join QS_LearningAnswer LA ON LA.PersonID=P.PersonID
	     Join QS_LearningFeedback LF ON LF.QID=LA.QID
	     Join QS_CourseELearningSection CES ON CES.ELSCode=LF.ELSCode
        )

        SELECT PP.PersonSNO,Base.ELSName,Base.PersonID,Base.PName,Base.PersonID_encryption,Base.ELSCode,Base.CompletedDate, ROW_NUMBER() OVER (ORDER BY Base.PersonID) AS ROW_NO FROM Base
        Left Join Person PP On PP.PersonID=Base.PersonID
        LEFT JOIN Organ O ON O.OrganSNO = Base.OrganSNO
        LEFT JOIN Role R ON R.RoleSNO =Base.RoleSNO
        where 1=1 and Base.PersonID <>'' and Base.PersonID is not null and ELSName is not null 
        ";

        Dictionary<string, object> wDict = new Dictionary<string, object>();
        Dictionary<string, object> Dict = new Dictionary<string, object>();

        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(wDict, userInfo);
        #endregion


        #region 查詢篩選區塊
        if (!string.IsNullOrEmpty(txt_CourseName.Text))
        {
            sql += " AND Base.ELSName Like '%' + @ELSName + '%' ";
            wDict.Add("ELSName", txt_CourseName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_Person.Text))
        {
            sql += " AND Base.PName Like '%' + @PName + '%' ";
            wDict.Add("PName", txt_Person.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " AND Base.PersonID Like '%' + @PersonID + '%' ";
            wDict.Add("PersonID", txt_PersonID.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_SFinishedDate.Text))
        {
            sql += " AND Base.CompletedDate >= @SFinishedDate";
            wDict.Add("SFinishedDate", txt_SFinishedDate.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_EFinishedDate.Text))
        {
            sql += " AND Base.CompletedDate  <= @EFinishedDate";
            wDict.Add("EFinishedDate", txt_EFinishedDate.Text.Trim());
        }
        #endregion


        sql += " Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_LearningRecord.DataSource = objDT.DefaultView;
        gv_LearningRecord.DataBind();

        string reportSQL = @"Select  ROW_NUMBER() OVER (ORDER BY Ano) AS ROW_NO,P.PName,LA.PersonID,R.RoleName,C.Mval Class,CES.ELSName,LF.QName,ANO,
			Case when C2.Mval is null Then LA.Ans  ELSE C2.Mval END Ans,LA.CompletedDate
            from QS_LearningFeedback LF           
            LEFT Join QS_LearningAnswer LA ON LA.QID=LF.QID
			Left JOin Person P On P.PersonID=LA.PersonID
            LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
            LEFT  Join Role R ON R.RoleSNO=P.RoleSNO            
            LEFT  Join QS_CourseELearningSection CES ON CES.ELSCode=LF.ELSCode
            LEFT Join QS_Course QC On QC.ELSCode=CES.ELSCode
            LEFT Join Config C On C.PVal=QC.Class1 and C.PGroup='CourseClass1'
            LEFT Join Config C2 On C2.Pval=LA.ANS and C2.PGroup='Questionnaire'
            where 1=1  ";

        #region 查詢篩選區塊

        #region 權限篩選區塊
        reportSQL += Utility.setSQLAccess_ByRoleOrganType(Dict, userInfo);
        #endregion
        if (!string.IsNullOrEmpty(txt_CourseName.Text))
        {
            reportSQL += " AND CES.ELSName Like '%' + @ELSName + '%' ";
            Dict.Add("ELSName", txt_CourseName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_Person.Text))
        {
            reportSQL += " AND P.PName Like '%' + @PName + '%' ";
            Dict.Add("PName", txt_Person.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_PersonID.Text))
        {
            reportSQL += " AND P.PersonID Like '%' + @PersonID + '%' ";
            Dict.Add("PersonID", txt_PersonID.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_SFinishedDate.Text))
        {
            reportSQL += " AND LA.CompletedDate >= @SFinishedDate";
            Dict.Add("SFinishedDate", txt_SFinishedDate.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_EFinishedDate.Text))
        {
            reportSQL += " AND LA.CompletedDate  <= @EFinishedDate";
            Dict.Add("EFinishedDate", txt_EFinishedDate.Text.Trim());
        }
        #endregion
        reportSQL += " order by ANO";
        DataTable ReportobjDT = objDH.queryData(reportSQL, Dict);
        //設定匯出資料
        ReportInit(ReportobjDT);
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
        Utility.OpenExportWindows(this, ReportEnum.Questionnaire.ToString());
    }

    protected void btn_ExportDetail_Click(object sender, EventArgs e)
    {
        if (gv_LearningRecord.Rows.Count == 0)
        {
            Response.Write("<script>alert('請先查詢')</script>");
            return;
        }
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, Object> adict = new Dictionary<string, object>();
        string sql = @"Select   ROW_NUMBER() OVER (ORDER BY FBSNO) AS ROW_NO,P.PersonSNO,P.PName,LA.PersonID,R.RoleName,C.Mval Class,CES.ELSName,LF.QName,C2.Mval Ans
            from Person P
            Join QS_LearningAnswer LA ON LA.PersonID=P.PersonID
            Join Role R ON R.RoleSNO=P.RoleSNO
            LEFT Join QS_LearningFeedback LF ON LF.QID=LA.QID
            Join QS_CourseELearningSection CES ON CES.ELSCode=LF.ELSCode
            Join QS_Course QC On QC.ELSCode=CES.ELSCode
            Join Config C On C.PVal=QC.Class1 and C.PGroup='CourseClass1'
            Join Config C2 On C2.Pval=LA.ANS and C2.PGroup='Questionnaire'
            where 1=1 ";
        #region 查詢篩選區塊
        if (!string.IsNullOrEmpty(txt_CourseName.Text))
        {
            sql += " AND Base.ELSName Like '%' + @ELSName + '%' ";
            adict.Add("ELSName", txt_CourseName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_Person.Text))
        {
            sql += " AND Base.PName Like '%' + @PName + '%' ";
            adict.Add("PName", txt_Person.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " AND Base.PersonID Like '%' + @PersonID + '%' ";
            adict.Add("PersonID", txt_PersonID.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_SFinishedDate.Text))
        {
            sql += " AND Base.CompletedDate >= @SFinishedDate";
            adict.Add("SFinishedDate", txt_SFinishedDate.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_EFinishedDate.Text))
        {
            sql += " AND Base.CompletedDate  <= @EFinishedDate";
            adict.Add("EFinishedDate", txt_EFinishedDate.Text.Trim());
        }
        #endregion
        sql += " Order by FBSNO";
        DataTable ObjDT = ObjDH.queryData(sql, adict);
    }
}