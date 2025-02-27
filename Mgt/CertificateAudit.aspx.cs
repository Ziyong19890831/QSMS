using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_CertificateAudit : System.Web.UI.Page
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
            //bindData(1);
        }
    }


    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 100;

        string sql = @"
                    With AllPersonLearningRecord As (
	          SELECT 
                	I.PersonSNO
					,P.RoleSNO
					,P.PersonID
                    ,PName
                	,QCPC.PlanName
                	,QCPC.CStartYear
                	,QCPC.CEndYear
                	,QCT.CTypeName
					,QCT.CTypeSNO
                	,QC.PClassSNO
					,QCPC.TargetIntegral
                	,sum(CHour) PClassTotalHr
                  FROM [QS_Integral] I
                  Left Join Person P on P.PersonSNO=I.PersonSNO
				  
                  Left Join QS_Course QC on QC.CourseSNO=I.CourseSNO
                  Left Join QS_CoursePlanningClass QCPC on QCPC.PClassSNO=QC.PClassSNO
                  Left Join QS_CertificateType QCT on QCT.CTypeSNO=QCPC.CTypeSNO
                    where 1=1 and I.IsUsed <> 1 and QC.Class1 <> 3
                  Group by QCPC.PlanName,QCT.CTypeName,QCPC.CStartYear,QCPC.CEndYear,QC.PClassSNO,I.PersonSNO,P.PName,P.RoleSNO,P.PersonID,QCT.CTypeSNO,QCPC.TargetIntegral
                  )
				  , CoursePlanningHours As (
	            SELECT 
		             cpc.PClassSNO,
		             cpc.PlanName,
		             ct.CTypeName,
		             Sum(c.CHour) PHours
	            From QS_CoursePlanningClass cpc
		             LEFT JOIN QS_CertificateType ct on ct.CTypeSNO=cpc.CTypeSNO
		             LEFT JOIN QS_Course c on c.PClassSNO=cpc.PClassSNO
	            Where cpc.IsEnable=1 and C.Class1 <> 3
	            Group By cpc.PClassSNO, cpc.PlanName, ct.CTypeName
            ),getresult as(
            Select 
               distinct
                ap.PClassSNO,
	            ap.PersonSNO,
				ap.PersonID,
	            ap.PName,
	            ap.RoleSNO,
	            cph.PlanName,
	            cph.CTypeName,
				ap.CTypeSNO,
	            ap.PClassTotalHr,
	            cph.PHours,
				ap.TargetIntegral
		
            From AllPersonLearningRecord ap
	            Left Join CoursePlanningHours cph On cph.PClassSNO=ap.PClassSNO
				Left Join QS_Certificate C ON C.PersonID=ap.PersonID
	          
            Where PClassTotalHr>=TargetIntegral ),
			getresultEnd as(
			select distinct  getresult.CTypeSNO,getresult.PersonID,getresult.RoleSNO
			,getresult.PersonSNO,getresult.CTypeName,getresult.PName from getresult
			  
           )
		   Select ROW_NUMBER() OVER (ORDER BY getresultEnd.PersonID) as ROW_NO ,* from getresultEnd
             Left Join Role R On R.RoleSNO=getresultEnd.RoleSNO
                where 1=1  and CTypeSNO <> 60
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(txt_PName.Text))
        {

            sql += " AND getresultEnd.PName =@PName ";
            wDict.Add("PName", txt_PName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_PlanName.Text))
        {
            sql += " AND getresultEnd.PlanName Like '%' + @PlanName + '%' ";
            wDict.Add("PlanName", txt_PlanName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " AND getresultEnd.PersonID =@PersonID ";
            wDict.Add("PersonID", txt_PersonID.Text.Trim());
        }
        if (ddl_CType.SelectedValue != "")
        {
            sql += " AND getresultEnd.CTypeSNO =@CTypeSNO ";
            wDict.Add("CTypeSNO", ddl_CType.SelectedValue);
        }
        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(wDict, userInfo);
        #endregion
       
        sql += " Order by ROW_NO";

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Course.DataSource = objDT.DefaultView;
        gv_Course.DataBind();
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

    protected void gv_Course_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Button myButton = (Button)e.CommandSource;
        GridViewRow myRow = (GridViewRow)myButton.NamingContainer;
        string Pname = myRow.Cells[1].Text;
        string PersonID = myRow.Cells[2].Text;
        string CtypeSNO = myRow.Cells[3].Text;
        string CUnitSNO = myRow.Cells[4].Text;
        string PersonSNO = myRow.Cells[5].Text;
        string CertPublicDate = DateTime.Now.ToString("yyyy-MM-dd");
        string CertStartDate= DateTime.Now.ToString("yyyy-MM-dd");
        DateTime TMP_CertEndDate = DateTime.Now.AddYears(6).AddDays(-1);
        string CertEndDate = TMP_CertEndDate.ToString("yyyy-MM-dd");

        string Insert_SQL = @"Insert into [QS_Certificate] (
        [PersonID]
        ,[CertID]
        ,[CTypeSNO]
        ,[CUnitSNO]
        ,[CertPublicDate]
        ,[CertStartDate]
        ,[CertEndDate]
        ,[CertExt]
        ,[IsPrint]
        ,[CreateDT]
        ,[CreateUserID]) values (
        @PersonID,@CertID,@CTypeSNO,@CUnitSNO,@CertPublicDate,@CertStartDate,@CertEndDate,@CertExt,@IsPrint,@CreateUserID

)";
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonID", PersonID);
        //aDict.Add("CertID",);
        aDict.Add("CTypeSNO", CtypeSNO);
        aDict.Add("CUnitSNO", CUnitSNO);
        aDict.Add("CertPublicDate", CertPublicDate);
        aDict.Add("CertStartDate", CertStartDate);
        aDict.Add("CertEndDate", CertEndDate);
        aDict.Add("CertExt",0);
        aDict.Add("IsPrint",0);
        aDict.Add("CreateUserID",userInfo.PersonSNO);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery(Insert_SQL, aDict);
        //取得所選取的CourseSNO(所有的編號)
      
        //當這個人積分已經被使用掉,必須修改Integral裡面的isUsed
        string Update_SQL = @"Update [QS_Integral] set [IsUsed]=1 where [PersonSNO]=@PersonSNO And [CourseSNO]=@CourseSNO";
        Dictionary<string, object> bDict = new Dictionary<string, object>();
        bDict.Add("PersonSNO", PersonSNO);
        //bDict.Add("CourseSNO",);
        objDH.executeNonQuery(Update_SQL, bDict);
    }

    protected void gv_Course_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            //要隱藏的欄位，如果前台filed隱藏會抓不到值，所以在RowCreate時才隱藏


            e.Row.Cells[3].Visible = false;
            e.Row.Cells[4].Visible = false;

        }
    }

    protected void gv_Course_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void btn_Review_Click(object sender, EventArgs e)
    {

    }

    protected void btnGrant_Click(object sender, EventArgs e)
    {
        if (gv_Course.Rows.Count == 0)
        {
            Response.Write("<script>alert('尚未查詢')</script>");
            return;
        }

        int i;
        string PersonSNO = "";
        string PersonID = "";
        string CtypeSNO = "";
        string PClassSNO = "";
        string Sort = "";
        for (i = 0; i < this.gv_Course.Rows.Count; i++)
        {
            if (((CheckBox)gv_Course.Rows[i].FindControl("CheckBox")).Checked)
            {
                //PersonID = gv_Course.Rows[i].Cells[3].Text;
                CtypeSNO += gv_Course.Rows[i].Cells[3].Text+",";
                PersonSNO += gv_Course.Rows[i].Cells[4].Text + ",";
                //Sort += gv_Course.Rows[i].Cells[6].Text + ",";
                //PClassSNO+= gv_Course.Rows[i].Cells[10].Text + ",";
                //GridView1.DataKeys[i].Value.ToString() 可以抓到該列的DataKeys的值，我設定的是pk值
            }
 
        }
        if (PersonSNO.Length == 0)
        {
            Response.Write("<script>alert('您未勾選人員')</script>");
            return;
        }
        PersonSNO = PersonSNO.Substring(0, PersonSNO.Length - 1);
        //PersonID = PersonID.Substring(0, PersonID.Length - 1);
        CtypeSNO = CtypeSNO.Substring(0, CtypeSNO.Length - 1);
        //Sort = Sort.Substring(0, Sort.Length - 1);
        //string[] CheckPClassSNO = PClassSNO.Substring(0, PClassSNO.Length - 1).Split(',');
        string[] CheckCtypeSNO = CtypeSNO.Split(',');
      
        for (int j = 0; j < CheckCtypeSNO.Length; j++)
        {
            if (CheckCtypeSNO[0] != CheckCtypeSNO[j])
            {
                Response.Write("<script>alert('請勾選同一批證書與同一課程規劃進行更新')</script>");
                return;
            }
          
        }
        Response.Write("<script>var w = screen.width; window.open('CertificateChange.aspx?Psno=" + PersonSNO + "&CtSNO=" + CheckCtypeSNO[0] + "','','width=w,height=500');</script>");
    }

 

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        String filePath = Directory.GetCurrentDirectory() + @"/SysFile/領證上傳.xlsx";
        FileInfo file = new FileInfo(filePath);
        if (file.Exists)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "text/plain";
            Response.Flush();
            Response.TransmitFile(file.FullName);
            Response.End();
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        page_Panel.Visible = false;
        string errorMessage = "";

        string PersonID = "";
        if ((file_Upload != null) && (file_Upload.PostedFile.ContentLength > 0) && !string.IsNullOrEmpty(file_Upload.FileName))
        {
            string extension = Path.GetExtension(file_Upload.FileName).ToLowerInvariant();
            List<string> allowedExtextsion = new List<string> { ".xlsx", ".xls" };
            if (allowedExtextsion.IndexOf(extension) == -1) errorMessage += "請上傳 (xlxs,xls) 類型檔案";
            if (!String.IsNullOrEmpty(errorMessage))
            {
                Utility.showMessage(Page, "ErrorMessage", errorMessage);
                return;
            }

            List<OrderItem> orderItem = new List<OrderItem>();
            List<string> ExcelList = new List<string>();
            List<string> InExcelList = new List<string>();
            List<string> SQLList = new List<string>();
            Dictionary<int, string> Sort = new Dictionary<int, string>();
            string fileName = file_Upload.FileName;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 關閉新許可模式通知
            using (var package = new ExcelPackage(file_Upload.PostedFile.InputStream))
            {

                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                int rowid = 0;
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    rowid += 1;
                    OrderItem item = new OrderItem();
                    try
                    {
                       
                        item.PersonID = workSheet.Cells[rowIterator, 2].Text;
                        PersonID += "'" + item.PersonID + "'" + ",";
                        Sort.Add(rowid, item.PersonID);
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        orderItem.Add(item);
                    }
                }
                foreach (OrderItem item in orderItem)
                {
                    ExcelList.Add(item.PersonID);
                }
                lb_PerrsonID.Text = PersonID;
                if (lb_PerrsonID.Text != "")
                {
                    lb_PerrsonID.Text = PersonID.Substring(0, PersonID.Length - 1);
                }
                else
                {
                    Response.Write("<script>alert('EXCEL上傳內容，身分證不得為空。')</script>");
                    return;
                }


            }

            Dictionary<string, object> aDict = new Dictionary<string, object>();
            String sql = @" With AllPersonLearningRecord As (
	          SELECT 
                	I.PersonSNO
					,P.RoleSNO
					,P.PersonID
                    ,PName
                	,QCPC.PlanName
                	,QCPC.CStartYear
                	,QCPC.CEndYear
                	,QCT.CTypeName
					,QCT.CTypeSNO
                	,QC.PClassSNO
					,QCPC.TargetIntegral
                	,sum(CHour) PClassTotalHr
                  FROM [QS_Integral] I
                  Left Join Person P on P.PersonSNO=I.PersonSNO
				  
                  Left Join QS_Course QC on QC.CourseSNO=I.CourseSNO
                  Left Join QS_CoursePlanningClass QCPC on QCPC.PClassSNO=QC.PClassSNO
                  Left Join QS_CertificateType QCT on QCT.CTypeSNO=QCPC.CTypeSNO
                    where 1=1 and I.IsUsed <> 1 and QC.Class1 <> 3
                  Group by QCPC.PlanName,QCT.CTypeName,QCPC.CStartYear,QCPC.CEndYear,QC.PClassSNO,I.PersonSNO,P.PName,P.RoleSNO,P.PersonID,QCT.CTypeSNO,QCPC.TargetIntegral
                  )
				  , CoursePlanningHours As (
	            SELECT 
		             cpc.PClassSNO,
		             cpc.PlanName,
		             ct.CTypeName,
		             Sum(c.CHour) PHours
	            From QS_CoursePlanningClass cpc
		             LEFT JOIN QS_CertificateType ct on ct.CTypeSNO=cpc.CTypeSNO
		             LEFT JOIN QS_Course c on c.PClassSNO=cpc.PClassSNO
	            Where cpc.IsEnable=1 and C.Class1 <> 3
	            Group By cpc.PClassSNO, cpc.PlanName, ct.CTypeName
            ),getresult as(
            Select 
               distinct
                ap.PClassSNO,
	            ap.PersonSNO,
				ap.PersonID,
	            ap.PName,
	            ap.RoleSNO,
	            cph.PlanName,
	            cph.CTypeName,
				ap.CTypeSNO,
	            ap.PClassTotalHr,
	            cph.PHours,
				ap.TargetIntegral
		
            From AllPersonLearningRecord ap
	            Left Join CoursePlanningHours cph On cph.PClassSNO=ap.PClassSNO
				Left Join QS_Certificate C ON C.PersonID=ap.PersonID
	          
            Where PClassTotalHr>=TargetIntegral )
			select ROW_NUMBER() OVER (ORDER BY PersonID ) as ROW_NO,* from getresult
			  Left Join Role R On R.RoleSNO=getresult.RoleSNO
            Where getresult.PersonID In (" + lb_PerrsonID.Text + " ) And CtypeSNO not in (10,11,60) order by PersonID ";
            DataHelper objDH = new DataHelper();
            DataTable objDT = objDH.queryData(sql, aDict);
            System.Data.DataColumn newColumn = new System.Data.DataColumn("Sort", typeof(System.Int32));
            objDT.Columns.Add(newColumn);


            for (int i = 1; i <= Sort.Count; i++)
            {
                for (int j = 0; j < objDT.Rows.Count; j++)
                {
                    if (objDT.Rows[j]["PersonID"].ToString() == Sort[i].ToString())
                    {
                        objDT.Rows[j][23] = i;
                    }
                }
            }
            //objDT.Columns.Add(newColumn);
            objDT.DefaultView.Sort = "Sort";
            var dv = objDT.DefaultView;
            gv_Course.DataSource = dv.ToTable();

            gv_Course.DataBind();
            if (objDT.Rows.Count > 0)
            {
                lb_NoOne.Visible = false;
                for (int i = 0; i < objDT.Rows.Count; i++)
                {
                    SQLList.Add(objDT.Rows[i]["PersonID"].ToString());
                }
            }
            List<ForEXCEL> NotInEXCELListData = new List<ForEXCEL>();

            foreach (string aitem in ExcelList)
            {
                ForEXCEL notInEXCELListData = new ForEXCEL();
                if (!SQLList.Contains(aitem) && aitem != "")
                {
                    notInEXCELListData.PersonID = aitem;
                    notInEXCELListData.stutas = "換證所需積分名單內無此人";
                    NotInEXCELListData.Add(notInEXCELListData);

                }

            }
            if (NotInEXCELListData.Count > 0)
            {
                ProblemList.Visible = true;
                gv_NotInExcel.Visible = true;
                bindDataSource(NotInEXCELListData);
                btnGrant.Enabled = false;
                btnGrant.Style["background-color"] = "gray";

            }
            else
            {
                ProblemList.Visible = false;
                gv_NotInExcel.Visible = false;
                btnGrant.Enabled = true;
                btnGrant.Style["background-color"] = "#f9bf3b";
            }
        }
        else
        {
            Response.Write("<script>alert('請上傳EXCEL檔案')</script>");
            return;
        }




    
    }
    protected void bindDataSource(List<ForEXCEL> dataSource)
    {
        gv_NotInExcel.DataSource = dataSource;
        gv_NotInExcel.DataBind();

    }
    public class ForEXCEL
    {
        public string PersonID { set; get; }
        public string stutas { set; get; }
    }
    public class OrderItem
    {
        public string PersonID { set; get; }
        public string stutas { set; get; }
    }

}