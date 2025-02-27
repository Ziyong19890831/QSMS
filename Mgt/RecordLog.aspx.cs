using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_RecordLog : System.Web.UI.Page
{
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfo;
    public Dictionary<string, string> _SetCol;
    public UserInfo userInfo = null;
    int viewrole = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindData(1);
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    private void ReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("PName", "學員名稱");
        _SetCol.Add("PersonID", "學員身分證");
        _SetCol.Add("PlanName", "課程規劃名稱");
        _SetCol.Add("ExportC", "已取得/總積分");
        _SetCol.Add("CtypeName", "可取得的證書");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.RecordLog.ToString()] = _ExcelInfo;
    }

    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        DataHelper objDH = new DataHelper();
        DataTable objDT;
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        string sql = @"  
  
            
               
                with getsomething as(
                SELECT 
                	I.PersonSNO
                    ,P.PName
                    ,P.PersonID
                	,QCPC.PlanName
                	,QCPC.CStartYear
                	,QCPC.CEndYear
                	,QCT.CTypeName
                	,QC.PClassSNO
                	,sum(CHour) PClassTotalHr
                  FROM [QS_Integral] I
                  Left Join Person P on P.PersonSNO=I.PersonSNO
                  Left Join QS_Course QC on QC.CourseSNO=I.CourseSNO
                  Left Join QS_CoursePlanningClass QCPC on QCPC.PClassSNO=QC.PClassSNO
                  Left Join QS_CertificateType QCT on QCT.CTypeSNO=QCPC.CTypeSNO
                    where 1=1
                  Group by QCPC.PlanName,QCT.CTypeName,QCPC.CStartYear,QCPC.CEndYear,QC.PClassSNO,I.PersonSNO,P.PName,P.PersonID
                  )
                  , getAllCourseHours As (
                				Select  c.PClassSNO, SUM(c.CHour) sumHours
                				From QS_CoursePlanningClass cpc
                					Left JOIN QS_Course c on c.PClassSNO=cpc.PClassSNO
                				Group By c.PClassSNO
                			)
                
                  select ROW_NUMBER() OVER (ORDER BY GS.PersonSNO) AS ROW_NO , GS.PersonSNO ,STUFF ( P.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption'
				  ,GS.PName ,GS.PersonID ,GS.PlanName ,GS.CTypeName ,GS.PClassTotalHr , gc.sumHours
				  ,Convert(varchar(20),GS.PClassTotalHr)+'/'+Convert(varchar(20),gc.sumHours)　ExportC
				  from getsomething GS
                  left join getAllCourseHours gc on gc.PClassSNO=GS.PClassSNO
				LEFT JOIN Person P on P.PersonSNO = gs.PersonSNO
				LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
				LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
                where 1 = 1 
            ";
        if (!String.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " AND p.PersonID Like '%' + @PersonID + '%' ";
            wDict.Add("PersonID", txt_PersonID.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_Person.Text))
        {
            sql += " AND p.PName =@PName ";
            wDict.Add("PName", txt_Person.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_PlanName.Text))
        {
            sql += " AND GS.PlanName Like '%' + @txt_PlanName + '%' ";
            wDict.Add("txt_PlanName", txt_PlanName.Text.Trim());
        }
       
        sql += Utility.setSQLAccess_ByRoleOrganType(wDict, userInfo);
        objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_LearningRecord.DataSource = objDT;
        gv_LearningRecord.DataBind();
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

        Utility.OpenExportWindows(this, ReportEnum.RecordLog.ToString());
    }
}