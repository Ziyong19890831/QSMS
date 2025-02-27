using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_ExUploadRecordSearch : System.Web.UI.Page
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

            setCoursePlanningClass(ddl_ECoursePlanningClass, "請選擇");
            SetCourse(ddl_CourseClass, "請選擇");
            SetUploadUnit(ddl_UploadUnit, "請選擇");
            lb_Notice.Text = @"上傳方式:" + "<br>" + @"1.請先選好要上傳時數的「繼續教育規劃類別」，再點擊右側「下載格式」按鈕下載格式檔。" + "<br>" + @" 2.將資料依照格式檔欄位及格式輸入後存檔，點擊右側「上傳」按鈕選擇存好檔的檔案進行上傳，上傳後，請務必使用「時數上傳查詢」確認上傳結果。";
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        if (ddl_UploadType.SelectedValue == "0")
        {
            String filePath = Directory.GetCurrentDirectory() + @"/SysFile/實體課程積分上傳格式.xlsx";
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
        else if (ddl_UploadType.SelectedValue == "1")
        {
            String filePath = Directory.GetCurrentDirectory() + @"/SysFile/線上課程實地測驗.xlsx";
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
        else
        {
            String filePath = Directory.GetCurrentDirectory() + @"/SysFile/繼續教育課程時數上傳.xlsx";
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
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    }
    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        String sql = @"                       
        with final as(SELECT distinct CoursePlanning,Class,convert(varchar, LUR.CDate, 23)CDate,convert(varchar, LUR.CreateDT, 23)CreateDT,p.PAccount,p.PName,o.OrganName,FileSNO
  FROM [dbo].[LearningUploadRecord] LUR
  left join Person P on lur.CreateUserID=p.PersonSNO
  left join Organ o on p.OrganSNO=o.OrganSNO
  where 1=1 and LUR.EPClassSNO <> 0 
";
        #region 查詢篩選區塊
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        if (!String.IsNullOrEmpty(ddl_PlanName.SelectedValue))
        {
            sql += " AND  CoursePlanning = @PlanName ";
            wDict.Add("PlanName", ddl_PlanName.SelectedItem.Text);
        }
        if (!String.IsNullOrEmpty(ddl_CourseClass.SelectedValue))
        {
            sql += " AND Class = @Class ";
            wDict.Add("Class", ddl_CourseClass.SelectedItem.Text);
        }
        if (!string.IsNullOrEmpty(txt_UploadTimeStart.Text))
        {
            sql += "  AND lur.CreateDT >= @UploadTimeStart";
            wDict.Add("UploadTimeStart", txt_UploadTimeStart.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_UploadTimeEnd.Text))
        {
            sql += "  AND lur.CreateDT <= @UploadTimeEnd ";
            wDict.Add("UploadTimeEnd", txt_UploadTimeEnd.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_UploadAccount.Text))
        {
            sql += " AND  p.PAccount=@UploadAccount";
            wDict.Add("UploadAccount", txt_UploadAccount.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_UploadName.Text))
        {
            sql += " AND p.PName Like '%' + @UploadName + '%' ";
            wDict.Add("UploadName", txt_UploadName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_CDate.Text))
        {
            sql += "  AND lur.CDate = @CDate";
            wDict.Add("CDate", txt_CDate.Text.Trim());
        }
        if (!string.IsNullOrEmpty(ddl_UploadUnit.SelectedValue))
        {
            sql += " AND o.OrganCode = @OrganSNO ";
            wDict.Add("OrganSNO", ddl_UploadUnit.SelectedValue.Trim());
        }
        #endregion
        sql += ") SELECT  ROW_NUMBER() OVER (ORDER BY FileSNO) as ROW_NO,* from final";


        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        wDict.Clear();
        if (objDT.Rows.Count > 0)
        {
            lb_SearchNoAnswer.Visible = false;
            int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
            if (page > maxPageNumber) page = maxPageNumber;
            objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
            ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
            gv_Course.DataSource = objDT.DefaultView;
            gv_Course.DataBind();
            //設定匯出資料


            if (objDT.Rows.Count > 0)
            {
                gv_Course.DataSource = objDT.DefaultView;
                gv_Course.DataBind();
            }
            else
            {
                gv_Course.DataSource = null;
                gv_Course.DataBind();
            }

        }
        else
        {
            ltl_PageNumber.Visible = false;
            txt_Page.Visible = false;
            lb_SearchNoAnswer.Visible = true;
            gv_Course.DataSource = null;
            gv_Course.DataBind();
        }

    }
    protected void Btn_Print_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();

        string FileSNO = (sender as LinkButton).CommandArgument;
        string UploadLogFinal = @"Select  [PName]'姓名',[PersonID]'身分證',[CourseName]'課程名稱',convert(varchar,[CDate], 23)'上課日期',[Integral]'時數',[ClassLocation]'上課地點',[Unit]'辦理單位',[Ctype]'類別(實體、通訊、 線上、 視訊、 其他)',[Message]'上傳結果' from LearningUploadRecord Where FileSNO=@FileSNO";
        string sqlFileName = @"Select [FileName] from LearningUploadRecord Where FileSNO=@FileSNO";

        aDict.Add("FileSNO", FileSNO);
        DataTable objDT = objDH.queryData(sqlFileName, aDict);
        DataTable DS = objDH.queryData(UploadLogFinal, aDict);
        string FileName = objDT.Rows[0]["FileName"].ToString(); ;
        byte[] file = null;
        ExcelHelper.DatatableToExcelForWeb(FileName, DS, ref file, false, "");

        // 匯出 Excel --- 使用 Response 下載附件         
        string filename = string.Format("{0}.xlsx", FileName);
        Response.Clear();
        Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}", filename));
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.BinaryWrite(file);
        Response.End();

    }
    protected void SetCourse(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        ddl.Items.Clear();
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        wDict.Add("PClassSNO", ddl_CourseClass.SelectedValue);
        DataTable objDT = objDH.queryData("SELECT CourseSNO, CourseName FROM QS_Course where PClassSNO=@PClassSNO And Ctype in (2,3,4)", wDict);
        ddl.DataSource = null;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
            ddl.Items.Insert(1, new System.Web.UI.WebControls.ListItem("繼續教育", "999"));
        }
    }
    protected void SetUploadUnit(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        ddl.Items.Clear();
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        if (userInfo.RoleOrganType == "S")
        {
            DataTable objDT = objDH.queryData("SELECT distinct OrganCode,OrganName FROM Person P LEFT JOIN Organ O ON O.OrganSNO=P.OrganSNO LEFT JOIN Role R ON R.RoleSNO=P.RoleSNO WHERE 1=1 And IsAdmin=1 and IsEnable=1 and O.OrganName<>'無現職單位'", wDict);
            ddl.DataSource = null;
            ddl.DataBind();
            if (DefaultString != null)
            {
                ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
                if (objDT.Rows.Count > 0)
                {
                    for (int i = 0; i < objDT.Rows.Count; i++)
                    {
                        objDT.Rows[i]["OrganName"].ToString();
                        ddl.Items.Insert(i + 1, new System.Web.UI.WebControls.ListItem(objDT.Rows[i]["OrganName"].ToString(), objDT.Rows[i]["OrganCode"].ToString()));
                    }
                }
            }
        }
        else
        {
            wDict.Add("PersonSNO", userInfo.PersonSNO);
            DataTable objDT = objDH.queryData("SELECT  OrganCode,OrganName FROM Person P LEFT JOIN Organ O ON O.OrganSNO=P.OrganSNO where PersonSNO=@PersonSNO", wDict);
            ddl.DataSource = null;
            ddl.DataBind();
            objDT.Rows[0]["OrganName"].ToString();
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(objDT.Rows[0]["OrganName"].ToString(), objDT.Rows[0]["OrganCode"].ToString()));
            ddl_UploadUnit.Enabled = false;
        }

    }

    #region 設定下拉選單

    protected void setCoursePlanningClass(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();

        string sql = "";

        sql = @"
            Select Distinct cpc.EPClassSNO, cpc.PlanName
            From QS_ECoursePlanningClass cpc
              Left Join QS_ECoursePlanningRole cpr ON cpr.EPClassSNO=cpc.EPClassSNO
            left join Role R on cpr.RoleSNO=R.DocGroup
            Where 1=1
        ";

        if (userInfo.RoleOrganType == "U" && userInfo.RoleSNO != "18")
        {
            sql += " And R.RoleSNO=@RoleSNO";
            wDict.Add("RoleSNO", userInfo.RoleGroup);
        }
        else if (userInfo.RoleOrganType == "U" && userInfo.RoleSNO == "18")
        {
            //開課單位
        }
        else if (userInfo.RoleOrganType == "")
        {
            sql += " And 1=2";
        }
        else if (userInfo.RoleOrganType == "A")//衛生局對象為藥師和其他人員
        {
            sql += " And cpr.RoleSNO in ('16','17')";
        }
        else
        {
            //S、A、B
        }


        DataTable objDT = objDH.queryData(sql, wDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }

    protected void ddl_CoursePlanningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        //SetCourse(ddl_CourseName, "請選擇");
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        string EPClassSNO = ddl_ECoursePlanningClass.SelectedValue;
        wDict.Add("EPClassSNO", EPClassSNO);
        string sql = @"
			Select  ROW_NUMBER() OVER (ORDER BY EPClassSNO) as ROW_NO, [EPClassSNO]
                            ,[PlanName]
	                        ,Cast(CStartYear as varchar(4)) + '-' + Cast(CEndYear as varchar(4)) As 'CYear'
                            ,[IsEnable]
	                        ,[QECPC].CTypeSNO
                            ,[Compulsory_Entity]
                            ,[Compulsory_Practical]
                            ,[Compulsory_Communication]
                            ,[Compulsory_Online]
	                        ,ct.CTypeName
                            ,QECPC.[CreateDT]
                            ,QECPC.[CreateUserID]
                            ,QECPC.[ModifyDT]
                            ,QECPC.[ModifyUserID]
                            ,Substring(
							 (
                                        	Select ',' + RoleName 
                                        	From 
                                        		(Select cpr.EPClassSNO, r.RoleName From QS_ECoursePlanningRole cpr
                                                Left Join Role r ON r.RoleSNO=cpr.RoleSNO) t
                                        	Where t.EPClassSNO=QECPC.EPClassSNO For XML PATH ('')
                                        ),2,100) as CRole
                            from [QS_ECoursePlanningClass] QECPC
                            Left Join QS_CertificateType ct ON ct.CTypeSNO=[QECPC].CTypeSNO
                        where QECPC.EPClassSNO=@EPClassSNO
        ";
        DataTable objDT = objDH.queryData(sql, wDict);
        gv_CoursePlanningClass.DataSource = objDT.DefaultView;
        gv_CoursePlanningClass.DataBind();
    }


    #endregion


    #region 上傳檔案

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (ddl_ECoursePlanningClass.SelectedValue == "")
        {
            Response.Write("<script>alert('請選擇課程規劃')</script>");
            return;
        }
        string errorMessage = "";
        string sql = "";
        string LearningUploadsql = "";
        Dictionary<string, object> dicpd = new Dictionary<string, object>();
        DataHelper CjDH = new DataHelper();
        if ((file_Upload != null) && (file_Upload.PostedFile.ContentLength > 0) && !string.IsNullOrEmpty(file_Upload.FileName))
        {
            string extension = Path.GetExtension(file_Upload.FileName).ToLowerInvariant();
            List<string> allowedExtextsion = new List<string> { ".xlsx" };
            //ConvertXls(file_Upload.FileName);
            if (allowedExtextsion.IndexOf(extension) == -1) errorMessage += "請上傳 (xlsx) 類型檔案";
            if (!String.IsNullOrEmpty(errorMessage))
            {
                Utility.showMessage(Page, "ErrorMessage", errorMessage);
                return;
            }

            List<OrderItem> orderItem = new List<OrderItem>();
            string fileName = file_Upload.FileName;
            string fileSNO = GetFileSNO();
            using (var package = new ExcelPackage(file_Upload.PostedFile.InputStream))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                int rowid = 1;

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {

                    if (ddl_UploadType.SelectedValue == "2")
                    {

                        DateTime newDate = System.Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
                        OrderItem item = new OrderItem();
                        var isDate = DateTime.TryParse(workSheet.Cells[rowIterator, 4].Text, out newDate);
                        try
                        {

                            int Integer;
                            item.ROW_NO = rowid.ToString();
                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 1].Text)) item.Status += "姓名空白! ;";
                            item.MEMBER_NAME = workSheet.Cells[rowIterator, 1].Text;
                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 2].Text))
                            {
                                item.Status += "身分證空白! ;";
                            }
                            else if (!ValidIsID(workSheet.Cells[rowIterator, 2].Text))
                            {
                                if (!ValidIsID2(workSheet.Cells[rowIterator, 2].Text) && !ValidIsNID3(workSheet.Cells[rowIterator, 2].Text.Trim().Substring(0, 1), workSheet.Cells[rowIterator, 2].Text.Trim().Substring(1, workSheet.Cells[rowIterator, 2].Text.Trim().Length - 1)))
                                {
                                    item.Status += "身分證/居留證格式有誤！;";
                                }
                            }
                            item.ID = workSheet.Cells[rowIterator, 2].Text;
                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 3].Text)) item.Status += "課程名稱空白! ;";
                            item.style = "color:red; font-weight:bold;";
                            item.CourseName = workSheet.Cells[rowIterator, 3].Text;
                            string[] format = { "yyyy/MM/dd" };

                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 4].Text)) item.Status += "日期空白! ;";

                            if (isDate)
                            {
                                item.Cdate = workSheet.Cells[rowIterator, 4].Text;


                            }
                            else
                            {
                                item.Status += "日期格式不正確! ;";
                            }

                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 5].Text)) item.Status += "學分空白! ;";

                            var isInteger = int.TryParse(workSheet.Cells[rowIterator, 5].Text, out Integer);
                            if (!isInteger)
                            {
                                item.EIntegral = workSheet.Cells[rowIterator, 5].Text;
                                item.Status += "學分格式不正確!";
                                item.style = "color:red; font-weight:bold;";
                            }
                            else
                            {
                                item.EIntegral = workSheet.Cells[rowIterator, 5].Text;
                            }
                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 6].Text)) item.Status += "上課地點! ;";
                            item.CLocation = workSheet.Cells[rowIterator, 6].Text;
                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 7].Text)) item.Status += "辦理單位空白! ;";
                            item.Uni = workSheet.Cells[rowIterator, 7].Text;
                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 8].Text)) item.Status += "類別空白! ;";
                            item.CType = workSheet.Cells[rowIterator, 8].Text;
                            switch (item.CType)
                            {
                                case "實體":
                                    item.CTypeCode = "2"; break;
                                case "實習":
                                    item.CTypeCode = "3"; break;
                                case "通訊":
                                    item.CTypeCode = "4"; break;
                                case "線上":
                                    item.CTypeCode = "1"; break;
                            }

                        }

                        catch (Exception ex)
                        {

                            item.ROW_NO = rowid.ToString();
                            item.MEMBER_NAME = workSheet.Cells[rowIterator, 1].Text;
                            item.ID = workSheet.Cells[rowIterator, 2].Text;
                            item.CourseName = workSheet.Cells[rowIterator, 3].Text;
                            item.Cdate = workSheet.Cells[rowIterator, 4].Text;
                            item.EIntegral = workSheet.Cells[rowIterator, 5].Text;
                            item.CType = workSheet.Cells[rowIterator, 6].Text;
                            item.Status = ex.Message.ToString();
                        }
                        finally
                        {
                            if (!string.IsNullOrEmpty(item.ID) || isDate)
                            {
                                rowid += 1;
                                orderItem.Add(item);
                            }



                        }
                    }
                }
                foreach (OrderItem item in orderItem)
                {
                    if (!string.IsNullOrEmpty(item.Status))
                    {
                        continue;
                    }
                    string PersonSNO = QueryPersonSNO(item.ID);
                    if (PersonSNO.Equals("-1"))
                    {
                        item.Status = "上傳失敗，查無此帳號!";
                        item.style = "color:red; font-weight:bold;";
                        WriteUploadLog(item.ID, item.MEMBER_NAME, ddl_ECoursePlanningClass.SelectedItem.Text, "繼續教育", item.CourseName, item.Cdate, item.EIntegral, item.CLocation, item.Uni, ddl_ECoursePlanningClass.SelectedValue, "", item.CType, item.Status, fileName, fileSNO);
                        continue;
                    }
                    else
                    {
                        item.Status = "上傳成功!";
                        item.style = "color:Green; font-weight:bold;";
                        WriteUploadLog(item.ID, item.MEMBER_NAME, ddl_ECoursePlanningClass.SelectedItem.Text, "繼續教育", item.CourseName, item.Cdate, item.EIntegral, item.CLocation, item.Uni, ddl_ECoursePlanningClass.SelectedValue, "", item.CType, item.Status, fileName, fileSNO);
                    }
                    string CheckC = @"Select * from QS_Certificate where PersonID='" + item.ID + "'";
                    DataTable CheckCdt = CjDH.queryData(CheckC, null);
                    if (CheckCdt.Rows.Count == 0)
                    {
                        item.Status = "上傳失敗，查無此帳號的證書!";
                        item.style = "color:red; font-weight:bold;";
                        WriteUploadLog(item.ID, item.MEMBER_NAME, ddl_ECoursePlanningClass.SelectedItem.Text, "繼續教育", item.CourseName, item.Cdate, item.EIntegral, item.CLocation, item.Uni, ddl_ECoursePlanningClass.SelectedValue, "", item.CType, item.Status, fileName, fileSNO);
                        continue;
                    }
                    //繼續教育，由於無法做細項課程規劃，所以名稱他們自己上傳，想傳什麼就傳什麼…
                    sql = "";
                    sql += " If Exists(select 1 From QS_EIntegral Where PersonID='" + item.ID + "' And CDate='" + item.Cdate + "' And EPClassSNO='" + ddl_ECoursePlanningClass.SelectedValue + "' And Integral='" + item.EIntegral + "' and Ctype='" + StringToValue(item.CType) + "' and CourseName='" + item.CourseName + "') ";
                    sql += " Select 1 AS 'Identity'";
                    sql += " Else ";
                    sql += " Insert Into [QS_EIntegral] (PersonID,PName,EPClassSNO,CDate,Integral,CType,IsUsed,CourseName,CreateDT,CreateUserID) ";
                    sql += " VALUES ('" + item.ID + "','" + item.MEMBER_NAME + "','" + ddl_ECoursePlanningClass.SelectedValue + "','" + item.Cdate + "','" + item.EIntegral + "','" + StringToValue(item.CType) + "','0','" + item.CourseName + "','" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','" + userInfo.PersonSNO + "')";
                    string sqlS = "BEGIN TRY   BEGIN TRANSACTION \n";
                    string sqlE = "COMMIT TRANSACTION        END TRY        BEGIN CATCH        ROLLBACK TRANSACTION        END CATCH \n";
                    string sqlI = "SELECT @@IDENTITY AS 'Identity'";
                    string fullSQL = sqlS + sql + sqlI + sqlE;
                    DataHelper objDH = new DataHelper();
                    DataTable dt = objDH.queryData(fullSQL, dicpd);
                    if (Convert.ToString(dt.Rows[0]["Identity"]) != "1")
                    {
                        string EISNOIdentity = dt.Rows[0]["Identity"].ToString();
                        int EISNO = Convert.ToInt32(EISNOIdentity);
                        LearningUploadsql = "";
                        LearningUploadsql += "Insert Into QS_ELearningUpload(PersonID,EISNO,CreateDT,CreateUserID,ClassLocation,Unit,Ctype) ";
                        LearningUploadsql += "VALUES ('" + item.ID + "','" + EISNO + "','" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','" + userInfo.PersonSNO + "','" + item.CLocation + "','" + item.Uni + "','" + StringToValue(item.CType) + "')";

                        dt = objDH.queryData(LearningUploadsql, dicpd);
                    }
                }

                if (rowid > 0)
                {
                    bindData(orderItem);
                }
            }
        }
        else
        {
            Response.Write("<script>alert('請上傳EXCEL檔案')</script>");
            return;
        }
    }
    protected String GetFileSNO()
    {
        Dictionary<string, object> dicpd = new Dictionary<string, object>();
        dicpd.Add("PID", "FileSNO");


        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("select PVal from Config where PID=@PID ", dicpd);
        string FileSNO = objDT.Rows[0]["PVal"].ToString();
        int NewFileSNO = Convert.ToInt32(FileSNO) + 1;
        dicpd.Add("PVal", NewFileSNO.ToString());
        objDT = objDH.queryData("Update Config set PVal=@PVal where PID=@PID ", dicpd);
        return FileSNO;
    }
    protected void WriteUploadLog(string PersonID, string PName, string CoursePlanning, string Class, string CourseName, string date, string Score, string ClassLocation, string Unit, string EPClassSNO, string CourseSNO, string Ctype, string Message, string FileName, string FileSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Clear();
        aDict.Add("PersonID", PersonID);
        aDict.Add("PName", PName);
        aDict.Add("CoursePlanning", CoursePlanning);
        aDict.Add("Class", Class);
        aDict.Add("CourseName", CourseName);
        aDict.Add("CDate", date);
        aDict.Add("Integral", Score);
        aDict.Add("ClassLocation", ClassLocation);
        aDict.Add("Unit", Unit);
        aDict.Add("EPClassSNO", EPClassSNO);
        aDict.Add("CourseSNO", CourseSNO);
        aDict.Add("Ctype", Ctype);
        aDict.Add("Message", Message);
        aDict.Add("CreateDT", DateTime.Now.ToString("yyyy-MM-dd"));
        aDict.Add("CreateUserID", userInfo.PersonSNO);
        aDict.Add("FileName", FileName);
        aDict.Add("FileSNO", FileSNO);

        try
        {
            objDH.executeNonQuery(@"  Insert into LearningUploadRecord([PersonID],[PName],[CoursePlanning],[Class],[CourseName],[CDate],[Integral],[ClassLocation],[Unit],[EPClassSNO],[CourseSNO],[Ctype],[Message],[CreateDT],[CreateUserID],[FileName],[FileSNO]) 
            Values(@PersonID,@PName,@CoursePlanning,@Class,@CourseName,@CDate,@Integral,@ClassLocation,@Unit,@EPClassSNO,@CourseSNO,@Ctype,@Message,@CreateDT,@CreateUserID,@FileName,@FileSNO)", aDict);
        }
        catch (Exception ex)
        {

        }

    }

    protected string QueryPersonSNO(string PersonID)
    {
        Dictionary<string, object> dicpd = new Dictionary<string, object>();
        dicpd.Add("PersonID", PersonID);

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT PersonSNO FROM Person WHERE PersonID=@PersonID ", dicpd);
        if (objDT.Rows.Count == 0)
        {
            return "-1";
        }
        return objDT.Rows[0]["PersonSNO"].ToString();
    }
    public static bool ValidIsID(string ID)
    {
        #region 長度為10
        if (string.IsNullOrEmpty(ID) || ID.Length != 10)
        {
            return false;
        }
        #endregion

        #region 檢查基本字串格式 1位英文字+9位數字
        string regextext = @"^[a-z]{1}(1|2){1}\d{8}$";
        if (!Regex.IsMatch(ID, regextext, RegexOptions.IgnoreCase))
        {
            return false;
        }
        #endregion

        #region 英文字轉碼
        string _strID = "";
        switch (ID.Substring(0, 1).ToUpper())
        {
            case "A":
                _strID = "10";
                break;
            case "B":
                _strID = "11";
                break;
            case "C":
                _strID = "12";
                break;
            case "D":
                _strID = "13";
                break;
            case "E":
                _strID = "14";
                break;
            case "F":
                _strID = "15";
                break;
            case "G":
                _strID = "16";
                break;
            case "H":
                _strID = "17";
                break;
            case "I":
                _strID = "34";
                break;
            case "J":
                _strID = "18";
                break;
            case "K":
                _strID = "19";
                break;
            case "L":
                _strID = "20";
                break;
            case "M":
                _strID = "21";
                break;
            case "N":
                _strID = "22";
                break;
            case "O":
                _strID = "35";
                break;
            case "P":
                _strID = "23";
                break;
            case "Q":
                _strID = "24";
                break;
            case "R":
                _strID = "25";
                break;
            case "S":
                _strID = "26";
                break;
            case "T":
                _strID = "27";
                break;
            case "U":
                _strID = "28";
                break;
            case "V":
                _strID = "29";
                break;
            case "W":
                _strID = "32";
                break;
            case "X":
                _strID = "30";
                break;
            case "Y":
                _strID = "31";
                break;
            case "Z":
                _strID = "33";
                break;
        }


        if (string.IsNullOrEmpty(_strID))
        {
            return false;
        }
        #endregion

        #region 檢查第2碼須為1或2
        if (ID.Substring(1, 1) != "1" && ID.Substring(1, 1) != "2")
        {
            return false;
        }
        #endregion

        #region 邏輯檢核
        int total = Convert.ToInt32(_strID.Substring(0, 1)) * 1 + Convert.ToInt32(_strID.Substring(1, 1)) * 9 +
                Convert.ToInt32(ID.Substring(1, 1)) * 8 + Convert.ToInt32(ID.Substring(2, 1)) * 7 +
                Convert.ToInt32(ID.Substring(3, 1)) * 6 + Convert.ToInt32(ID.Substring(4, 1)) * 5 +
                Convert.ToInt32(ID.Substring(5, 1)) * 4 + Convert.ToInt32(ID.Substring(6, 1)) * 3 +
                Convert.ToInt32(ID.Substring(7, 1)) * 2 + Convert.ToInt32(ID.Substring(8, 1)) * 1;

        int tmod = total % 10;
        if (tmod == 0 && tmod == Convert.ToInt32(ID.Substring(9, 1)))
        {
            return true;
        }
        else if ((10 - tmod) == Convert.ToInt32(ID.Substring(9, 1)))
        {
            return true;
        }

        return false;
        #endregion
    }
    public static bool ValidIsID2(string ID)
    {
        #region 長度為10
        if (string.IsNullOrEmpty(ID) || ID.Length != 10)
        {
            return false;
        }
        #endregion

        #region 檢查基本字串格式 1位英文字+abcdb任一碼+8位數字
        string regextext = @"^[a-zA-Z]{1}(a|b|c|d|A|B|C|D){1}\d{8}$";
        if (!Regex.IsMatch(ID, regextext, RegexOptions.IgnoreCase))
        {
            return false;
        }
        #endregion

        #region 英文字轉碼
        string[] _strID = new string[10];
        for (int i = 0; i < 10; i++)
        {
            _strID[i] = ID.Substring(i, 1);
        }

        int[] _intID = new int[10];
        string FirstLetter = GetLetterNum(_strID[0]);
        string SecondLetter = GetLetterNum(_strID[1]);
        if (string.IsNullOrEmpty(FirstLetter) || string.IsNullOrEmpty(SecondLetter))
        {
            return false;
        }
        #endregion

        #region 邏輯檢核
        _intID[0] = int.Parse(FirstLetter.Substring(0, 1));
        _intID[1] = int.Parse(FirstLetter.Substring(1, 1));
        _intID[2] = int.Parse(SecondLetter.Substring(1, 1));
        _intID[3] = int.Parse(_strID[2]);
        _intID[4] = int.Parse(_strID[3]);
        _intID[5] = int.Parse(_strID[4]);
        _intID[6] = int.Parse(_strID[5]);
        _intID[7] = int.Parse(_strID[6]);
        _intID[8] = int.Parse(_strID[7]);
        _intID[9] = int.Parse(_strID[8]);

        int total = GetSpecialMultiplicationValue(_intID[0], 1) + GetSpecialMultiplicationValue(_intID[1], 9) +
            GetSpecialMultiplicationValue(_intID[2], 8) + GetSpecialMultiplicationValue(_intID[3], 7) +
            GetSpecialMultiplicationValue(_intID[4], 6) + GetSpecialMultiplicationValue(_intID[5], 5) +
            GetSpecialMultiplicationValue(_intID[6], 4) + GetSpecialMultiplicationValue(_intID[7], 3) +
            GetSpecialMultiplicationValue(_intID[8], 2) + GetSpecialMultiplicationValue(_intID[9], 1);

        int tmod = int.Parse(total.ToString().Substring(total.ToString().Length - 1, 1));
        if (tmod == 0 && tmod == Convert.ToInt32(_strID[9]))
        {
            return true;
        }
        else if ((10 - tmod) == Convert.ToInt32(_strID[9]))
        {
            return true;
        }
        return false;
        #endregion
    }
    public static string GetLetterNum(string Letter)
    {
        if (string.IsNullOrEmpty(Letter) || Letter.Length != 1)
        {
            return "";
        }
        string _strID = "";
        switch (Letter.ToUpper())
        {
            case "A":
                _strID = "10";
                break;
            case "B":
                _strID = "11";
                break;
            case "C":
                _strID = "12";
                break;
            case "D":
                _strID = "13";
                break;
            case "E":
                _strID = "14";
                break;
            case "F":
                _strID = "15";
                break;
            case "G":
                _strID = "16";
                break;
            case "H":
                _strID = "17";
                break;
            case "J":
                _strID = "18";
                break;
            case "K":
                _strID = "19";
                break;
            case "L":
                _strID = "20";
                break;
            case "M":
                _strID = "21";
                break;
            case "N":
                _strID = "22";
                break;
            case "P":
                _strID = "23";
                break;
            case "Q":
                _strID = "24";
                break;
            case "R":
                _strID = "25";
                break;
            case "S":
                _strID = "26";
                break;
            case "T":
                _strID = "27";
                break;
            case "U":
                _strID = "28";
                break;
            case "V":
                _strID = "29";
                break;
            case "X":
                _strID = "30";
                break;
            case "Y":
                _strID = "31";
                break;
            case "W":
                _strID = "32";
                break;
            case "Z":
                _strID = "33";
                break;
            case "I":
                _strID = "34";
                break;
            case "O":
                _strID = "35";
                break;

        }
        return _strID;
    }
    public static bool ValidIsNID3(string firstLetter, string num)
    {
        char[] myArr = num.ToCharArray();
        Array.Reverse(myArr);
        Regex eng = new Regex("^[A-Z]+$");
        Regex number = new Regex("^[0-9]+$");
        if (num.Length < 9 || (!eng.IsMatch(firstLetter)) || (!number.IsMatch(new string(myArr))))
        {
            return false;
        }

        ///建立字母對應表(A~Z)
        ///A=10 B=11 C=12 D=13 E=14 F=15 G=16 H=17 J=18 K=19 L=20 M=21 N=22
        ///P=23 Q=24 R=25 S=26 T=27 U=28 V=29 X=30 Y=31 W=32  Z=33 I=34 O=35 
        string alphabet = "ABCDEFGHJKLMNPQRSTUVXYWZIO";
        string transferIdNo = "" + (alphabet.IndexOf(firstLetter) + 10) + "" + num + "";

        int[] idNoArray = transferIdNo.ToCharArray()
                                      .Select(c => Convert.ToInt32(c.ToString()))
                                      .ToArray();

        int sum = idNoArray[0];
        int checkSum = 0;
        int[] weight = new int[] { 1, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        for (int i = 0; i < weight.Length; i++)
        {
            checkSum += (weight[i] * idNoArray[i]) % 10;
        }
        int checkNum = 10 - (checkSum % 10) == 10 ? 0 : 10 - (checkSum % 10);

        return checkNum == Convert.ToInt32(new string(myArr).Substring(0, 1));

    }
    public static int GetSpecialMultiplicationValue(int value, int compareValue)
    {
        string strValue = (value * compareValue).ToString();
        int reValue = int.Parse(strValue.Substring(strValue.Length - 1, 1));
        return reValue;
    }

    //public void ConvertXls(string files)
    //{
    //    Workbook workbook = new Workbook();
    //    workbook.LoadFromFile(files);
    //    workbook.SaveToFile(files+".xlsx", ExcelVersion.Version2013);
    //}

    #endregion


    #region 上傳檔案結果

    protected void bindData(List<OrderItem> dataSource)
    {
        if (ddl_UploadType.SelectedValue == "1")
        {
            this.gv_ScoreUpload.DataSource = dataSource;
            this.gv_ScoreUpload.DataBind();
            this.gv_ScoreUpload.Visible = true;
            this.gv_ScoreUpload_Class.Visible = false;
        }
        else if (ddl_UploadType.SelectedValue == "0")
        {
            this.gv_ScoreUpload_Class.DataSource = dataSource;
            this.gv_ScoreUpload_Class.DataBind();
            this.gv_ScoreUpload.Visible = false;
            this.gv_ScoreUpload_Class.Visible = true;
        }
        else
        {
            this.gv_UpdateScore_Change.DataSource = dataSource;
            this.gv_UpdateScore_Change.DataBind();
            this.gv_UpdateScore_Change.Visible = false;
            this.gv_UpdateScore_Change.Visible = true;
        }
    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        bindData(page);
    }

    protected static string StringToValue(string Type)
    {

        switch (Type)
        {
            case "線上":
                return "1";
            case "實體":
                return "2";
            case "實習":
                return "3";
            case "通訊":
                return "4";
        }


        return "";
    }
    #endregion


}



public class OrderItem
{
    public string ROW_NO { set; get; }
    public string ID { set; get; }
    public string MEMBER_NAME { set; get; }
    public string CourseName { set; get; }
    public string Cdate { set; get; }
    public string EIntegral { set; get; }
    public string CType { set; get; }
    public string CTypeCode { set; get; }
    public string Status { set; get; }
    public string style { set; get; }
    public string CLocation { get; set; }
    public string Uni { get; set; }
}
