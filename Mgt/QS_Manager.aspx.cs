using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_QS_Manager : System.Web.UI.Page
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

    private void EReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("PName", "姓名");
        _SetCol.Add("PersonID", "身分證號");       
        _SetCol.Add("CourseName", "課程名稱");
        _SetCol.Add("Integral", "學分");
        _SetCol.Add("CreateDT", "學分取得時間");
        _SetCol.Add("IsUsed", "是否使用");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.QS_Manager.ToString()] = _ExcelInfo;
    }

    protected void bindData(int page)
    {
        //第二分頁
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;

        String sql = @"            
                 SELECT ROW_NUMBER() Over (order by [ISNO] ) as ROW_NO
	  ,[ISNO],P.[PName],P.[PersonID],C.CourseName,C.Chour 'Integral'
	  ,QECPC.CTypeSNO
      
	  ,QCT.CTypeName
      
      
      ,C.[PClassSNO]
        
	  , CType CTypeCode
      ,[IsUsed]
      ,STUFF ( P.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption'
        ,QCE.CreateDT
        ,QC.CertEndDate
  FROM [QS_Integral] QCE
  Left Join QS_Course C On C.CourseSNO=QCE.CourseSNO
  Left Join QS_CoursePlanningClass QECPC ON QECPC.PClassSNO=C.PClassSNO
   LEFT JOIN Person P on P.PersonSNO = QCE.PersonSNO
  Left Join QS_Certificate QC On QC.PersonID=P.PersonID  And QC.CTypeSNO=QECPC.CTypeSNO
  Left join QS_CertificateType QCT ON QCT.CTypeSNO=QC.CTypeSNO
 
  LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
  LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
    Where  1=1 and C.CType =1 And C.Class1 <> 3 and QCE.IsUsed=0 
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
            sql += " AND C.CourseName Like '%' + @CourseName + '%' ";
            wDict.Add("CourseName", txt_CourseName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_IntegralS.Text))
        {
            sql += " AND QCE.CreateDT > @CreateDTS ";
            wDict.Add("CreateDTS", txt_IntegralS.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_IntegralE.Text))
        {
            sql += " AND QCE.CreateDT < @CreateDTE ";
            wDict.Add("CreateDTE", txt_IntegralE.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_CertificateS.Text))
        {
            sql += " AND QC.CertEndDate > @CertificateCreateDTS ";
            wDict.Add("CertificateCreateDTS", txt_CertificateS.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_CertificateE.Text))
        {
            sql += " AND QC.CertEndDate < @CertificateCreateDTE ";
            wDict.Add("CertificateCreateDTE", txt_CertificateE.Text.Trim());
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
        EReportInit(objDT);
        ltl_PageNumberss.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

    protected void EbindData(int page)
    {
        //第二分頁
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;

        String sql = @"            
                 SELECT ROW_NUMBER() Over (order by [ISNO] ) as ROW_NO
	  ,[ISNO],P.[PName],P.[PersonID],C.CourseName,C.Chour 'Integral'
	  ,QECPC.CTypeSNO
      
	  ,QCT.CTypeName
      
      
      ,C.[PClassSNO]
        
	  , CType CTypeCode
      ,case when QCE.IsUsed=0 Then '未使用' Else '已使用' End IsUsed
      ,STUFF ( P.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption'
        ,QCE.CreateDT
        ,QC.CertEndDate
  FROM [QS_Integral] QCE
  Left Join QS_Course C On C.CourseSNO=QCE.CourseSNO
  Left Join QS_CoursePlanningClass QECPC ON QECPC.PClassSNO=C.PClassSNO
   LEFT JOIN Person P on P.PersonSNO = QCE.PersonSNO
  Left Join QS_Certificate QC On QC.PersonID=P.PersonID  And QC.CTypeSNO=QECPC.CTypeSNO
  Left join QS_CertificateType QCT ON QCT.CTypeSNO=QC.CTypeSNO
 
  LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
  LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
    Where  1=1 and C.CType In (2,3)
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
            sql += " AND C.CourseName Like '%' + @CourseName + '%' ";
            wDict.Add("CourseName", txt_ECourseName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_OnlineDateS.Text))
        {
            sql += " AND QCE.CreateDT > @CreateDTS ";
            wDict.Add("CreateDTS", txt_OnlineDateS.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_OnlineDateE.Text))
        {
            sql += " AND QCE.CreateDT < @CreateDTE ";
            wDict.Add("CreateDTE", txt_OnlineDateE.Text.Trim());
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
        ltl_EPageNumber.Text = Utility.showPageNumber1(objDT.Rows.Count, page, pageRecord);
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
        Utility.OpenExportWindows(this, ReportEnum.QS_Manager.ToString());
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


    protected void btnEDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String ISNO = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("ISNO", ISNO);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete QS_Integral Where ISNO=@ISNO", aDict);
        Response.Write("<script>alert('刪除成功!') </script>");
        btnPage_Click(sender, e);
        bindData(1);
        return;
    }

    protected void btn_EPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_EPage.Value, out page);
        EbindData(page);
    }

    protected void btn_ESearch_Click(object sender, EventArgs e)
    {
        EbindData(1);

    }

    protected void btn_EExport_Click(object sender, EventArgs e)
    {
        if (gv_EManager.Rows.Count == 0)
        {
            Response.Write("<script>alert('尚未查詢')</script>");
            return;
        }
        Utility.OpenExportWindows(this, ReportEnum.QS_Manager.ToString());
    }

    protected void gv_Elearning_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }

    protected void btn_EUpdate_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        WebControl webControlTxt = gvr.FindControl("txt_EIntegral") as WebControl;
        WebControl webControlBtn = gvr.FindControl("btn_EUpdate_1") as WebControl;

        webControlTxt.Enabled = true;

        btn.Visible = false;
        webControlBtn.Visible = true;
        webControlTxt.BackColor = System.Drawing.Color.White;
    }

    protected void btn_EUpdate_1_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        WebControl webControlTxt = gvr.FindControl("txt_EIntegral") as WebControl;
        WebControl webControlBtn = gvr.FindControl("btn_EUpdate") as WebControl;
        WebControl webControlBtns = gvr.FindControl("btn_EUpdate_1") as WebControl;
        webControlTxt.BackColor = Color.DarkGray;
        webControlTxt.Enabled = false;
        webControlBtn.Visible = true;
        webControlBtns.Visible = false;
        //TextBox txt_Integral = gvr.FindControl("txt_Integral") as TextBox;
        //string Integral = txt_Integral.Text;
        //string EISNO = gvr.Cells[1].Text;
        //Dictionary<string, object> adict = new Dictionary<string, object>();
        //adict.Add("Integral", Integral);
        //adict.Add("EISNO", EISNO);
        //string SQL = @"Update QS_EIntegral Set [Integral]=@Integral where EISNO=@EISNO";
        //DataHelper ObjDH = new DataHelper();
        //ObjDH.executeNonQuery(SQL, adict);
    }

    protected void btnEDEL_1_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String ISNO = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("ISNO", ISNO);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete QS_Integral Where ISNO=@ISNO", aDict);
        Response.Write("<script>alert('刪除成功!') </script>");
        btn_EPage_Click(sender, e);

        
    }
}