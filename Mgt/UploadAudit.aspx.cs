using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_UploadAudit : System.Web.UI.Page
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

        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bind(1);
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

    protected void bind(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        Dictionary<string, object> adict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
    
        string sql = @"with getlist as (
                                SELECT distinct
                                      P.PName,P.PersonID,QC.CourseName,UU.PersonSNO,QC.CourseSNO,max(UU.CreateDT)CreateDT
									  ,QC.PClassSNO
                                 FROM [UserUpload] UU
                                  Left Join QS_Course QC On QC.CourseSNO=UU.CourseSNO
                                  Left Join Person P On P.personSNO=UU.PersonSNO
								  Group by P.PName,P.PersonID,QC.CourseName,UU.PersonSNO,QC.CourseSNO,QC.PClassSNO
								   ),getlastdata as (
                                  Select distinct gl.PName
                                    ,gl.PersonID,gl.CourseName,gl.CourseSNO ,gl.PersonSNO,gl.CreateDT,
								Case when UU.Audit=0 Then '未審核' ELSE '已審核' End 'Audit',gl.PClassSNO
                                    from getlist　gl
								  Join UserUpload UU On UU.CreateDT=gl.CreateDT
                                  
								  )Select ROW_NUMBER() OVER (ORDER BY gld.PersonID Desc) as ROW_NO,gld.*,QCPR.RoleSNO
								  from getlastdata gld
								  Left Join QS_CoursePlanningRole QCPR On QCPR.PClassSNO=gld.PClassSNO
                                    Where 1=1 


 ";

        #region 權限篩選區塊
        switch (userInfo.RoleGroup)
        {
            case "10":
                sql += " And RoleSNO=10";
                break;
                
            case "11":
                sql += " And RoleSNO=11";
                break;
               
            case "12":
                sql += " And RoleSNO=12";
                break;
                
            case "13":
                sql += " And RoleSNO=13";
                break;
            default:
                break;
                
        }
        #endregion
        #region 查詢篩選區塊
        if (!string.IsNullOrEmpty(txt_Name.Text))
        {
            sql += " AND gld.PName Like '%' + @PName + '%' ";
            adict.Add("PName", txt_Name.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " AND gld.PersonID=@PersonID ";
            adict.Add("PersonID", txt_PersonID.Text.Trim());
        }
        #endregion

        DataTable objDT = ObjDH.queryData(sql, adict);
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

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        bind(page);
    }


    protected void lk_Link_Click(object sender, EventArgs e)
    {
        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        string PersonSNO = grdrow.Cells[3].Text;
        string CourseSNO = grdrow.Cells[4].Text;
        Response.Redirect("./UploadAudit_AE.aspx?Psno=" + PersonSNO + "&Csno="+CourseSNO+"");
    }

    protected void gv_LearningRecord_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[4].Visible = false;
           
        }
    }
}