using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_QS_EManager : System.Web.UI.Page
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

    /// <summary>
    /// 設定此報表可匯出名稱設定 ,  (注意 : BindData 欄位要包含設定可匯出資料)
    /// </summary>
    private void ReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("PersonID", "身分證號");
        _SetCol.Add("PName", "姓名");
        _SetCol.Add("CourseName", "課程名稱");
        _SetCol.Add("CTypeName", "證書名稱");
        _SetCol.Add("CDate", "日期");
        _SetCol.Add("Integral", "學分");
        _SetCol.Add("CType", "類別");
        _SetCol.Add("effective", "是否有效");
        _SetCol.Add("isUsedStatus", "是否使用");
        _SetCol.Add("CertEndDate", "證書到期日");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.QS_EManager.ToString()] = _ExcelInfo;
    }
    private void EReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("PersonID", "身分證號");
        _SetCol.Add("PName", "姓名");
        _SetCol.Add("CourseName", "課程名稱");
        _SetCol.Add("學分", "學分");
        _SetCol.Add("CreateDT", "學分取得時間");
        _SetCol.Add("IsUsed", "是否使用");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.Elearning.ToString()] = _ExcelInfo;
    }

    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;

        String sql = @"            
                 SELECT ROW_NUMBER() Over (order by [EISNO] ) as ROW_NO
	  ,[EISNO]
	  ,QECPC.PlanName
	  ,QECPC.CTypeSNO
	  ,QCT.CTypeName
      ,QCE.[PersonID]
      ,P.[PName]
      ,QCE.[EPClassSNO]
      ,CONVERT(varchar(100), CDate, 111) CDate
      ,[Integral]
        ,QCE.CourseName
	  , CType CTypeCode
      , C.MVal CType
      ,[IsUsed]
      ,STUFF ( P.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption'
	  ,CONVERT(varchar(100), QC.CertEndDate, 111) CertEndDate
	  ,Case when QC.CertEndDate >  [CDate] then '有效' Else '失效' End effective
	  ,Case  when  QC.CertEndDate <  [CDate] then '不可使用' When [IsUsed] =0 then '未使用' When [IsUsed]=1 then '已使用'   END isUsedStatus
  FROM [QS_EIntegral] QCE
  Left Join QS_ECoursePlanningClass QECPC ON QECPC.EPClassSNO=QCE.EPClassSNO
  Left Join QS_Certificate QC On QC.PersonID=QCE.PersonID  And QC.CTypeSNO=QECPC.CTypeSNO
  Left join QS_CertificateType QCT ON QCT.CTypeSNO=QC.CTypeSNO
  LEFT JOIN Person P on P.PersonID = QCE.PersonID
  LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
  LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
  Left JOIN Config C On C.PVal=QCE.Ctype and C.PGroup='CourseCType'
    Where  1=1
        ";

        Dictionary<string, object> wDict = new Dictionary<string, object>();


        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(wDict, userInfo);
        #endregion


        #region 查詢篩選區塊
        if (!string.IsNullOrEmpty(txt_Person.Text))
        {
            sql += " AND p.PName Like '%' + @PName + '%' ";
            wDict.Add("PName", txt_Person.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " AND p.PersonID= @PersonID  ";
            wDict.Add("PersonID", txt_PersonID.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_CourseName.Text))
        {
            sql += " AND QCE.CourseName Like '%' + @CourseName + '%' ";
            wDict.Add("CourseName", txt_CourseName.Text.Trim());
        }
        
        #endregion


        sql += " Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_EManager.DataSource = objDT.DefaultView;
        gv_EManager.DataBind();
        //設定匯出資料
        ReportInit(objDT);
        ltl_PageNumberss.Text = Utility.showPageNumber1(objDT.Rows.Count, page, pageRecord);
    }
    protected void EbindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;

        String sql = @"            
                    Select ROW_NUMBER() Over (order by [ISNO] ) as ROW_NO,STUFF ( P.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption'
,P.PName,P.PersonID,QC.CourseName,  case when QI.IsUsed=0 Then '未使用' Else '已使用' End IsUsed,QC.CHour 學分 ,QI.CreateDT
from QS_Integral QI
Left Join Person P On P.PersonSNO=QI.PersonSNO
LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
where QC.Class1=3 
        ";

        Dictionary<string, object> wDict = new Dictionary<string, object>();


        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(wDict, userInfo);
        #endregion


        #region 查詢篩選區塊
        if (!string.IsNullOrEmpty(txt_EPname.Text))
        {
            sql += " AND p.PName Like '%' + @PName + '%' ";
            wDict.Add("PName", txt_EPname.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_EPersonID.Text))
        {
            sql += " AND p.PersonID= @PersonID  ";
            wDict.Add("PersonID", txt_EPersonID.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_ECourseName.Text))
        {
            sql += " AND QC.CourseName Like '%' + @CourseName + '%' ";
            wDict.Add("CourseName", txt_ECourseName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_OnlineDateS.Text))
        {
            sql += " AND QI.CreateDT > @txt_OnlineDateS ";
            wDict.Add("txt_OnlineDateS", txt_OnlineDateS.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_OnlineDateE.Text))
        {
            sql += " AND QI.CreateDT < @txt_OnlineDateE ";
            wDict.Add("txt_OnlineDateE", txt_OnlineDateE.Text.Trim());
        }
        #endregion


        sql += " Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
       
        gv_Elearning.DataSource = objDT.DefaultView;
        gv_Elearning.DataBind();
        //設定匯出資料
        EReportInit(objDT);
        ltl_EPageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
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
        if (gv_EManager.Rows.Count == 0)
        {
            Response.Write("<script>alert('尚未查詢')</script>");
            return;
        }
        Utility.OpenExportWindows(this, ReportEnum.QS_EManager.ToString());
    }


    protected void btn_Update_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        WebControl webControlTxt = gvr.FindControl("txt_Integral") as WebControl;
        WebControl webControlBtn = gvr.FindControl("btn_Update_1") as WebControl;

        webControlTxt.Enabled = true;

        btn.Visible = false;
        webControlBtn.Visible = true;
        webControlTxt.BackColor = System.Drawing.Color.White;

    }

    protected void btn_Update_1_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        WebControl webControlTxt = gvr.FindControl("txt_Integral") as WebControl;
        WebControl webControlBtn = gvr.FindControl("btn_Update") as WebControl;
        WebControl webControlBtns = gvr.FindControl("btn_Update_1") as WebControl;
        webControlTxt.BackColor = Color.DarkGray;
        webControlTxt.Enabled = false;
        webControlBtn.Visible = true;
        webControlBtns.Visible = false;
        TextBox txt_Integral = gvr.FindControl("txt_Integral") as TextBox;
        string Integral = txt_Integral.Text;
        string EISNO = gvr.Cells[1].Text;
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("Integral", Integral);
        adict.Add("EISNO", EISNO);
        string SQL = @"Update QS_EIntegral Set [Integral]=@Integral where EISNO=@EISNO";
        DataHelper ObjDH = new DataHelper();
        ObjDH.executeNonQuery(SQL, adict);
    }

    protected void gv_LearningRecord_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = false;
    }

    protected void btn_ESearch_Click(object sender, EventArgs e)
    {
        EbindData(1);
    }

    protected void btn_EExport_Click(object sender, EventArgs e)
    {
        if (gv_Elearning.Rows.Count == 0)
        {
            Response.Write("<script>alert('尚未查詢')</script>");
            return;
        }
        Utility.OpenExportWindows(this, ReportEnum.Elearning.ToString());
    }

    protected void btn_EPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_EPage.Value, out page);
        EbindData(page);
    }

}