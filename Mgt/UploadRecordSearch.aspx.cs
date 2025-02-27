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

public partial class Mgt_UploadRecordSearch : System.Web.UI.Page
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
            setCoursePlanningClass(ddl_CoursePlanningClass, "請選擇");
            SetCourse(ddl_CourseName, "請選擇");
            SetCourse(ddl_CourseClass, "請選擇");
            SetUploadUnit(ddl_UploadUnit, "請選擇");
            lb_Notice.Text = @"上傳方式:" + "<br>" + @"1.請先選好要上傳時數的「課程規劃類別」和「欲上傳的課程」，再點擊右側「下載格式」按鈕下載格式檔。" + "<br>" + @" 2.將資料依照格式檔欄位及格式輸入後存檔，點擊右側「上傳」按鈕選擇存好檔的檔案進行上傳，上傳後，請務必使用「時數上傳查詢」確認上傳結果。";
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        if (ddl_UploadType.SelectedValue == "0" && ddl_CourseName.SelectedValue == "999")
        {
            String filePath = Directory.GetCurrentDirectory() + @"/SysFile/新訓課程時數上傳.xlsx";
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
        else if (ddl_UploadType.SelectedValue == "0" && ddl_CourseName.SelectedValue == "998")
        {
            String filePath = Directory.GetCurrentDirectory() + @"/SysFile/新訓課程時數上傳.xlsx";
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
        else if (ddl_UploadType.SelectedValue == "0")
        {
            String filePath = Directory.GetCurrentDirectory() + @"/SysFile/新訓課程時數上傳.xlsx";
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


    #region 設定下拉選單

    protected void setCoursePlanningClass(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();

        string sql = "";

        sql = @"
             Select Distinct cpc.PClassSNO, cpc.PlanName
            From QS_CoursePlanningClass cpc
              Left Join QS_CoursePlanningRole cpr ON cpr.PClassSNO=cpc.PClassSNO
			   Left Join QS_ECoursePlanningClass cepc ON cepc.PClassSNO=cpc.PClassSNO
			  Left Join QS_Course QC On QC.PClassSNO=cpc.PClassSNO
              left join Role R on cpr.RoleSNO=R.DocGroup
            Where 1=1 and QC.Class1 <>3　 and　cepc.EPClassSNO is null
        ";

        if (userInfo.RoleOrganType == "U" && userInfo.RoleSNO != "18")
        {
            sql += " And R.RoleSNO=@RoleSNO";
            wDict.Add("RoleSNO", userInfo.RoleGroup);
        }
        else if (userInfo.RoleOrganType == "")
        {
            sql += " And 1=2";
        }
        else
        {
            //S、A、B
        }
        //else if (userInfo.RoleOrganType == "A")//衛生局對象為藥師和其他人員
        //{
        //    sql += " And cpr.RoleSNO in ('16','17')";
        //}



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
        SetCourse(ddl_CourseName, "請選擇");
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        string PClassSNO = ddl_CoursePlanningClass.SelectedValue;
        wDict.Add("PClassSNO", PClassSNO);
        string sql = @"
			With getAllCoursePlanningClass As (
				SELECT 
					cpc.PClassSNO,
					cpc.PlanName,
					Cast(CStartYear as varchar(4)) + '-' + Cast(CEndYear as varchar(4)) As 'CYear',
					(Case cpc.IsEnable When 1 Then '是' When 0 Then'否' End) IsEnable,
					ct.CTypeName,
                    ct.CTypeSNO,
					c.CHour,
					Substring(
					(
						Select ',' + RoleName 
						From 
							(Select cpr.PClassSNO, r.RoleName From QS_CoursePlanningRole cpr Left Join Role r ON r.RoleSNO=cpr.RoleSNO) t
						Where t.PClassSNO=cpc.PClassSNO For XML PATH ('')
					),2,100) as CRole,cpc.[TargetIntegral]
				From QS_CoursePlanningClass cpc
					Left Join QS_CertificateType ct ON ct.CTypeSNO=cpc.CTypeSNO
					Left Join QS_Course c ON c.PClassSNO=cpc.PClassSNO
			)

			--取得所有課程規劃類別之統計時數
			, getSumHours As (
				Select 
					PClassSNO, PlanName, CYear, IsEnable, CTypeName, CTypeSNO, CRole, Sum(CHour) sumHour, Count(CHour) countCourse
					,apc.[TargetIntegral]
				From getAllCoursePlanningClass apc
				Group By PClassSNO, PlanName, CYear, IsEnable, CTypeName, CTypeSNO, CRole,apc.[TargetIntegral]
			)

			Select ROW_NUMBER() OVER (ORDER BY PClassSNO) as ROW_NO, * 
			From getSumHours gs
			Where 1=1 and gs.PClassSNO=@PClassSNO
        ";
        DataTable objDT = objDH.queryData(sql, wDict);
        gv_CoursePlanningClass.DataSource = objDT.DefaultView;
        gv_CoursePlanningClass.DataBind();
    }

    protected void SetCourse(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        ddl.Items.Clear();
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        wDict.Add("PClassSNO", ddl_CoursePlanningClass.SelectedValue);
        DataTable objDT = objDH.queryData("SELECT CourseSNO, CourseName FROM QS_Course where PClassSNO=@PClassSNO And Ctype in (2,3,4)", wDict);
        ddl.DataSource = null;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
            ddl.Items.Insert(1, new System.Web.UI.WebControls.ListItem("基礎課程", "999"));
            ddl.Items.Insert(2, new System.Web.UI.WebControls.ListItem("專門課程", "998"));
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

    #endregion


    #region 上傳檔案

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string errorMessage = "";

        if (string.IsNullOrEmpty(ddl_CourseName.SelectedItem.Value))
        {
            errorMessage += "請選擇欲上傳分數的課程";
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        if ((file_Upload != null) && (file_Upload.PostedFile.ContentLength > 0) && !string.IsNullOrEmpty(file_Upload.FileName))
        {
            string extension = Path.GetExtension(file_Upload.FileName).ToLowerInvariant();
            List<string> allowedExtextsion = new List<string> { ".xlsx" };
            if (allowedExtextsion.IndexOf(extension) == -1) errorMessage += "請上傳 (xlsx) 類型檔案";
            if (!String.IsNullOrEmpty(errorMessage))
            {
                Utility.showMessage(Page, "ErrorMessage", errorMessage);
                return;
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<OrderItem> orderItem = new List<OrderItem>();
            string fileName = file_Upload.FileName;
            string fileSNO = GetFileSNO();
            using (var package = new ExcelPackage(file_Upload.PostedFile.InputStream))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                int rowid = 1;
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    if (ddl_UploadType.SelectedValue == "1")
                    {
                        DateTime dateTime;
                        OrderItem item = new OrderItem();
                        try
                        {
                            item.ROW_NO = rowid.ToString();
                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 1].Text)) item.Status += "姓名空白! ;";
                            item.MEMBER_NAME = workSheet.Cells[rowIterator, 1].Text;
                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 2].Text)) item.Status += "身分證空白! ;";
                            item.ID = workSheet.Cells[rowIterator, 2].Text.Trim();

                            int score;
                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 3].Text)) item.Status += "分數空白! ;";
                            var isScore = Int32.TryParse(workSheet.Cells[rowIterator, 3].Text, out score);
                            item.Score = score.ToString();
                            if (!isScore) item.Status += "分數格式不正確! ;";
                            string[] format = { "yyyy/M/dd", "yyyy/MM/dd" };

                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 4].Text)) item.Status += "日期空白! ;";
                            var isDate = DateTime.TryParseExact(workSheet.Cells[rowIterator, 4].Text, format, CultureInfo.InvariantCulture,
                                DateTimeStyles.None, out dateTime);
                            if (isDate)
                            {
                                item.Date = workSheet.Cells[rowIterator, 4].Text;

                            }
                            else
                            {
                                item.Status += "日期格式不正確! ;";
                            }
                            int PassScore;
                            var isPassScore = Int32.TryParse(workSheet.Cells[rowIterator, 5].Text, out PassScore);
                            item.PassScore = PassScore.ToString();
                            if (!isPassScore) item.Status += "分數格式不正確! ;";




                        }
                        catch (Exception ex)
                        {
                            item.ROW_NO = rowid.ToString();
                            item.MEMBER_NAME = workSheet.Cells[rowIterator, 1].Text;
                            item.ID = workSheet.Cells[rowIterator, 2].Text;
                            item.Score = workSheet.Cells[rowIterator, 3].Text;
                            item.Date = workSheet.Cells[rowIterator, 4].Text;
                            item.PassScore = workSheet.Cells[rowIterator, 5].Text;
                            item.Status = ex.Message.ToString();
                        }
                        finally
                        {
                            if (!string.IsNullOrEmpty(item.ID))
                            {
                                rowid += 1;
                                orderItem.Add(item);
                            }
                        }
                    }
                    if (ddl_UploadType.SelectedValue == "0")
                    {

                        OrderItem item = new OrderItem();
                        try
                        {
                            item.ROW_NO = rowid.ToString();
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
                            //int score;
                            //var isScore = Int32.TryParse(workSheet.Cells[rowIterator, 3].Text, out score);
                            //item.Score = score.ToString();
                            //if (!isScore) item.Status += "分數格式不正確! ;";
                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 3].Text)) item.Status += "上課地點空白! ;";
                            item.ClassLocation = workSheet.Cells[rowIterator, 3].Text;
                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 7].Text)) item.Status += "授課類別空白! ;";
                            item.Ctype = workSheet.Cells[rowIterator, 7].Text;
                            DateTime date;
                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 4].Text))
                            {
                                item.Status += "上課日期空白! ;";
                            }
                            else
                            {
                                string[] formats = { "yyyy/MM/dd", "yyyy/M/d", "yyyy/MM/d", "yyyy/M/dd" };
                                var isDate = DateTime.TryParseExact(workSheet.Cells[rowIterator, 4].Text, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date);
                                if (!isDate)
                                {
                                    item.Date = workSheet.Cells[rowIterator, 4].Text;
                                    item.Status += "日期格式不正確! ;";
                                }
                                else
                                {
                                    item.Date = date.ToString("yyyy-MM-dd");
                                }
                            }
                            if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 5].Text)) item.Status += "辦理單位空白! ;";
                            item.Unit = workSheet.Cells[rowIterator, 5].Text;

                            if (ddl_CourseName.SelectedValue == "998" || ddl_CourseName.SelectedValue == "999")
                            {
                                if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 6].Text)) { item.Status += "課程代號空白! ;"; }
                                else
                                {
                                    string[] CourseSNOArray = workSheet.Cells[rowIterator, 6].Text.Split(',');
                                    for (int i = 0; i < CourseSNOArray.Length; i++)
                                    {
                                        int CourseSNO_n;
                                        var CourseSNO = Int32.TryParse(CourseSNOArray[i], out CourseSNO_n);
                                        if (!CourseSNO)
                                        {
                                            item.Status += "課程代號不正確! ;";
                                        }
                                        else if (Utility.CheckCourseSNO(CourseSNOArray[i]) == false)
                                        {
                                            item.Status += "無此課程代號! ;";
                                        }
                                        else
                                        {
                                            item.CourseSNO = workSheet.Cells[rowIterator, 6].Text;
                                        }
                                    }
                                }
                            }


                        }
                        catch (Exception ex)
                        {
                            item.ROW_NO = rowid.ToString();
                            item.MEMBER_NAME = workSheet.Cells[rowIterator, 1].Text;
                            item.ID = workSheet.Cells[rowIterator, 2].Text;
                            item.Score = workSheet.Cells[rowIterator, 3].Text;
                            item.Date = workSheet.Cells[rowIterator, 4].Text;
                            item.PassScore = workSheet.Cells[rowIterator, 5].Text;
                            item.PassScore = workSheet.Cells[rowIterator, 6].Text;
                            item.Status = ex.Message.ToString();
                        }
                        finally
                        {
                            //if (string.IsNullOrEmpty(item.ID))
                            //{
                            rowid += 1;
                            orderItem.Add(item);
                            //}

                        }
                    }

                }
                foreach (OrderItem item in orderItem)
                {
                    if (!string.IsNullOrEmpty(item.Status)) continue;
                    string PersonSNO = QueryPersonSNO(item.ID);
                    string Ctype = StringToValue(item.Ctype);
                    if (PersonSNO.Equals("-1"))
                    {
                        item.Status = "上傳失敗，查無此帳號!";
                        item.style = "color:red; font-weight:bold;";
                        WriteUploadLog(item.ID, item.MEMBER_NAME, ddl_CoursePlanningClass.SelectedItem.Text, ddl_CourseName.SelectedItem.Text, "", item.Date, "1", item.ClassLocation, item.Unit, "", item.CourseSNO, item.Ctype, item.Status, fileName, fileSNO);
                        continue;
                    }
                    if (ddl_UploadType.SelectedValue == "1")
                    {
                        if (UpdateScore_Elearn(ddl_CourseName.SelectedItem.Value, PersonSNO, item.Score, item.Date, item.PassScore, item.IsPass, Ctype))
                        {
                            item.Status = "上傳成功!";
                            item.style = "color:blue;";
                        }
                        else
                        {
                            item.Status = "上傳失敗，查無記錄!";
                            item.style = "color:red; font-weight:bold;";
                        }
                        WriteUploadLog(item.ID, item.MEMBER_NAME, ddl_CoursePlanningClass.SelectedItem.Text, ddl_CourseName.SelectedItem.Text, "", item.Date, "1", item.ClassLocation, item.Unit, "", item.CourseSNO, item.Ctype, item.Status, fileName, fileSNO);
                    }
                    if (ddl_UploadType.SelectedValue == "0")
                    {
                        if (ddl_CourseName.SelectedValue == "999" || ddl_CourseName.SelectedValue == "998")
                        {
                            if (UpdateScore_LoacationForValue999(item.CourseSNO, PersonSNO, item.ClassLocation, item.Date, item.Unit, Ctype))
                            {
                                item.Status = "上傳成功!";
                                item.style = "color:blue;";
                            }
                            else
                            {
                                item.Status = "上傳失敗，查無記錄!";
                                item.style = "color:red; font-weight:bold;";
                            }
                            WriteUploadLog(item.ID, item.MEMBER_NAME, ddl_CoursePlanningClass.SelectedItem.Text, ddl_CourseName.SelectedItem.Text, "", item.Date, "1", item.ClassLocation, item.Unit, "", item.CourseSNO, item.Ctype, item.Status, fileName, fileSNO);
                        }
                        else
                        {
                            if (UpdateScore_Loacation(ddl_CourseName.SelectedItem.Value, PersonSNO, item.ClassLocation, item.Date, item.Unit))
                            {
                                item.Status = "上傳成功!";
                                item.style = "color:blue;";
                            }
                            else
                            {
                                item.Status = "上傳失敗，查無記錄!";
                                item.style = "color:red; font-weight:bold;";
                            }
                            WriteUploadLog(item.ID, item.MEMBER_NAME, ddl_CoursePlanningClass.SelectedItem.Text, ddl_CourseName.SelectedItem.Text, "", item.Date, "1", item.ClassLocation, item.Unit, "", item.CourseSNO, item.Ctype, item.Status, fileName, fileSNO);
                        }

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

    protected bool UpdateScore_Elearn(string CourseSNO, string PersonSNO, string Score, string date, string PassScore, string IsPass, string ClassLocation)
    {
        bool Insert_ispass;
        if (IsPass == "是") Insert_ispass = true;
        else Insert_ispass = false;

        Dictionary<string, object> dicpd = new Dictionary<string, object>();
        dicpd.Add("UploadType", ddl_UploadType.SelectedValue);
        dicpd.Add("CourseSNO", CourseSNO);
        dicpd.Add("PersonSNO", PersonSNO);
        dicpd.Add("Score", Score);
        dicpd.Add("ClassLocation", ClassLocation);
        dicpd.Add("date", date);
        dicpd.Add("PassScore", PassScore);
        dicpd.Add("IsPass", Insert_ispass);
        dicpd.Add("CreateDT", DateTime.Now);
        dicpd.Add("CreateUserID", userInfo.PersonSNO);
        dicpd.Add("ModifyDT", DateTime.Now);
        dicpd.Add("ModifyUserID", userInfo.PersonSNO);
        DataHelper objDH = new DataHelper();
        //需檢查person表中是否有這個人

        string sql = @"
                If not Exists(Select 1 From QS_integral Where CourseSNO=@CourseSNO and  PersonSNO=@PersonSNO and IsUSED=0)=
                Insert Into QS_LearningUpload(UploadType,PersonSNO,CourseSNO,Score,ExamDate,PassScore,IsPass,CreateDT,CreateUserID,ClassLocation) 
                VALUES (@UploadType,@PersonSNO,@CourseSNO,@Score,@date,@PassScore,@IsPass,@CreateDT,@CreateUserID,@ClassLocation)
        ";
        DataTable dt = objDH.queryData(sql, dicpd);
        if (dt.Rows.Count > 0)
        {

            if (Convert.ToInt16(dt.Rows[0]["rowcount"].ToString()) == 0) return false;
        }
        else
        {
            if (Insert_ispass == true)
            {
                Dictionary<string, object> wicpd = new Dictionary<string, object>();




                string InsertIntegral = @"

            If Exists(select 1 From QS_Integral Where PersonSNO=@PersonSNO And CourseSNO=@CourseSNO )
               
                  Insert Into QS_Integral(PersonSNO,CourseSNO,CreateUserID,Itype) 
                VALUES (@PersonSNO,@CourseSNO,@CreateUserID)
            Else
                Insert Into QS_Integral(PersonSNO,CourseSNO,CreateUserID,0) 
                VALUES (@PersonSNO,@CourseSNO,@CreateUserID)";

                wicpd.Add("PersonSNO", PersonSNO);
                wicpd.Add("CourseSNO", CourseSNO);
                wicpd.Add("AuthType", 1);
                wicpd.Add("CreateUserID", userInfo.PersonSNO);
                wicpd.Add("CreateDT", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                DataTable DS = objDH.queryData(InsertIntegral, wicpd);
            }

        }

        return true;
    }
    protected bool UpdateScore_Loacation(string CourseSNO, string PersonSNO, string ClassLocation, string date, string Unit)
    {
        //bool Insert_ispass;
        //if (IsPass == "是") Insert_ispass = true;
        //else Insert_ispass = false;

        Dictionary<string, object> dicpd = new Dictionary<string, object>();
        dicpd.Add("UploadType", ddl_UploadType.SelectedValue);
        dicpd.Add("CourseSNO", CourseSNO);
        dicpd.Add("PersonSNO", PersonSNO);
        dicpd.Add("ClassLocation", ClassLocation);
        dicpd.Add("date", date);
        dicpd.Add("Unit", Unit);
        dicpd.Add("CreateDT", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        dicpd.Add("CreateUserID", userInfo.PersonSNO);
        dicpd.Add("ModifyDT", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        dicpd.Add("ModifyUserID", userInfo.PersonSNO);
        DataHelper objDH = new DataHelper();
        //需檢查person表中是否有這個人
        string sql = @"
            If Exists(select 1 From QS_Integral Where PersonSNO=@PersonSNO And CourseSNO=@CourseSNO)
                update QS_LearningUpload set UploadType=@UploadType,PersonSNO=@PersonSNO,CourseSNO=@CourseSNO,ExamDate=@date,Unit=@unit,CreateDT=@CreateDT,CreateUserID=@CreateUserID,ClassLocation=@ClassLocation,Ctype=@Ctype
                Where PersonSNO=@PersonSNO And CourseSNO=@CourseSNO
            Else
                Insert Into QS_LearningUpload(UploadType,PersonSNO,CourseSNO,ExamDate,Unit,CreateDT,CreateUserID,ClassLocation) 
                VALUES (@UploadType,@PersonSNO,@CourseSNO,@date,@unit,@CreateDT,@CreateUserID,@ClassLocation)
        ";
        DataTable dt = objDH.queryData(sql, dicpd);
        //if (dt.Rows.Count > 0)
        //{

        //    if (Convert.ToInt16(dt.Rows[0]["rowcount"].ToString()) == 0) return false;
        //}
        //else
        //{

        Dictionary<string, object> wicpd = new Dictionary<string, object>();
        string InsertIntegral = @"
            If Exists(select 1 From QS_Integral Where PersonSNO=@PersonSNO And CourseSNO=@CourseSNO And isUsed=0)
                
                 Select 1
            Else
                Insert Into QS_Integral(PersonSNO,CourseSNO,CreateDT,CreateUserID,Itype) 
                VALUES (@PersonSNO,@CourseSNO,@CreateDT,@CreateUserID,0)";

        wicpd.Add("PersonSNO", PersonSNO);
        wicpd.Add("CourseSNO", CourseSNO);
        wicpd.Add("AuthType", 1);
        wicpd.Add("CreateDT", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        wicpd.Add("CreateUserID", userInfo.PersonSNO);
        DataTable DS = objDH.queryData(InsertIntegral, wicpd);


        //}

        return true;
    }
    protected bool UpdateScore_LoacationForValue999(string CourseSNO, string PersonSNO, string ClassLocation, string date, string Unit, string Ctype)
    {
        string[] CourseSNOArray = CourseSNO.Split(',');

        for (int i = 0; i < CourseSNOArray.Length; i++)
        {
            bool PairCourseSNO = CheckPairCourseSNO(CourseSNOArray[i]);
            //一筆一筆執行
            Dictionary<string, object> dicpd = new Dictionary<string, object>();
            dicpd.Add("UploadType", ddl_UploadType.SelectedValue);
            dicpd.Add("CourseSNO", CourseSNOArray[i]);
            dicpd.Add("PersonSNO", PersonSNO);
            dicpd.Add("ClassLocation", ClassLocation);
            dicpd.Add("date", date);
            dicpd.Add("Unit", Unit);
            dicpd.Add("Ctype", Ctype);
            dicpd.Add("CreateDT", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            dicpd.Add("CreateUserID", userInfo.PersonSNO);
            dicpd.Add("ModifyDT", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            dicpd.Add("ModifyUserID", userInfo.PersonSNO);
            DataHelper objDH = new DataHelper();
            //需檢查person表中是否有這個人
            string sql = @"
       If Exists(select 1 From QS_Integral Where PersonSNO=@PersonSNO And CourseSNO=@CourseSNO)
                update QS_LearningUpload set UploadType=@UploadType,PersonSNO=@PersonSNO,CourseSNO=@CourseSNO,ExamDate=@date,Unit=@unit,CreateDT=@CreateDT,CreateUserID=@CreateUserID,ClassLocation=@ClassLocation,Ctype=@Ctype
                Where PersonSNO=@PersonSNO And CourseSNO=@CourseSNO
            Else
                Insert Into QS_LearningUpload(UploadType,PersonSNO,CourseSNO,ExamDate,Unit,CreateDT,CreateUserID,ClassLocation,Ctype) 
                VALUES (@UploadType,@PersonSNO,@CourseSNO,@date,@unit,@CreateDT,@CreateUserID,@ClassLocation,@Ctype)
        ";
            DataTable dt = objDH.queryData(sql, dicpd);
            dicpd.Clear();
            //if (dt.Rows.Count > 0)
            //{

            //    if (Convert.ToInt16(dt.Rows[0]["rowcount"].ToString()) == 0) return false;
            //}
            //else
            //{

            Dictionary<string, object> wicpd = new Dictionary<string, object>();
            string InsertIntegral = @"
            If Exists(select 1 From QS_Integral Where PersonSNO=@PersonSNO And CourseSNO=@CourseSNO And isUsed=0)
                
                 Select 1
            Else
                Insert Into QS_Integral(PersonSNO,CourseSNO,CreateDT,CreateUserID,IsUsed) 
                VALUES (@PersonSNO,@CourseSNO,@CreateDT,@CreateUserID,0)";

            wicpd.Add("PersonSNO", PersonSNO);
            wicpd.Add("CourseSNO", CourseSNOArray[i]);
            wicpd.Add("AuthType", 1);
            wicpd.Add("CreateDT", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            wicpd.Add("CreateUserID", userInfo.PersonSNO);
            DataTable DS = objDH.queryData(InsertIntegral, wicpd);
            wicpd.Clear();

            //}
        }
        return true;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);

    }
    #endregion
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
  where 1=1 and LUR.EPClassSNO=0 
";
        #region 查詢篩選區塊
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        if (!String.IsNullOrEmpty(ddl_PlanName.SelectedValue))
        {
            sql += " AND CoursePlanning = @PlanName ";
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
            sql += "  AND lur.CreateDT <= @UploadTimeEnd";
            wDict.Add("UploadTimeEnd", txt_UploadTimeEnd.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_UploadAccount.Text))
        {
            sql += " AND  p.PAccount=@UploadAccount";
            wDict.Add("UploadAccount", txt_UploadAccount.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_UploadName.Text))
        {
            sql += " AND p.PName  Like '%' + @UploadName + '%'";
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

    #endregion

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

    protected void Btn_Print_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();

        string FileSNO = (sender as LinkButton).CommandArgument;
        string UploadLogFinal = @"Select [PName]'學員姓名',[PersonID]'身分證',[ClassLocation]'上課地點',convert(varchar,[CDate], 23)'上課日期',[Unit]'辦理單位',[CourseSNO]'課程代碼',[Ctype]'類別(實體、通訊、 線上、 視訊、 其他)',[Message]'上傳結果' from LearningUploadRecord Where FileSNO=@FileSNO";
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

    protected void ddl_UploadUnit_SelectedIndexChanged(object sender, EventArgs e) { }

    protected void ddl_CourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Dictionary<string, object> dicpd = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        string CourseSNO = ddl_CourseName.SelectedValue;
        string sql = "";
        if (CourseSNO == "998")
        {
            dicpd.Add("PClassSNO", ddl_CoursePlanningClass.SelectedValue);
            //專門實體
            sql = @"  Select QC.CourseSNO,QC.CourseName,QC.CHour,C.MVal 上課方式,C1.MVal 課程類別 from QS_Course QC
            Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO=QC.PClassSNO
            Left Join Config C On C.PVal=QC.Ctype and C.PGroup='CourseCType'
			Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
            where QCPC.PClassSNO=@PClassSNO and QC.Class1=2";
            lb_Notice.Visible = true;
        }
        else if (CourseSNO == "999")
        {
            dicpd.Add("PClassSNO", ddl_CoursePlanningClass.SelectedValue);
            //核心實體+核心線上
            sql = @"  Select QC.CourseSNO,QC.CourseName,QC.CHour,C.MVal 上課方式,C1.MVal 課程類別 from QS_Course QC
            Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO=QC.PClassSNO
            Left Join Config C On C.PVal=QC.Ctype and C.PGroup='CourseCType'
			Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
            where QCPC.PClassSNO=@PClassSNO and QC.Class1 <> 2";
            lb_Notice.Visible = true;
        }
        else
        {
            dicpd.Add("CourseSNO", CourseSNO);
            sql = @"SELECT QC.CourseSNO,QC.CourseName,QC.CHour,C.MVal 上課方式,C1.MVal 課程類別 FROM QS_Course QC 
            Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO = QC.PClassSNO
            Left Join Config C On C.PVal = QC.Ctype and C.PGroup = 'CourseCType'
            Left Join Config C1 On C1.PVal = QC.Class1 and C1.PGroup = 'CourseClass1'
            WHERE CourseSNO = @CourseSNO  ";
        }

        DataTable objDT = ObjDH.queryData(sql, dicpd);
        gv_CourseName.DataSource = objDT.DefaultView;
        gv_CourseName.DataBind();
    }
    protected void ddl_UploadType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Dictionary<string, object> dicpd = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        string CourseSNO = ddl_CourseName.SelectedValue;
        string sql = "";
        if (CourseSNO == "998")
        {
            dicpd.Add("PClassSNO", ddl_CoursePlanningClass.SelectedValue);
            //專門實體
            sql = @"  Select QC.CourseSNO,QC.CourseName,QC.CHour,C.MVal 上課方式,C1.MVal 課程類別 from QS_Course QC
            Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO=QC.PClassSNO
            Left Join Config C On C.PVal=QC.Ctype and C.PGroup='CourseCType'
			Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
            where QCPC.PClassSNO=@PClassSNO and QC.Class1=2";
            lb_Notice.Visible = true;
        }
        else if (CourseSNO == "999")
        {
            dicpd.Add("PClassSNO", ddl_CoursePlanningClass.SelectedValue);
            //核心實體+核心線上
            sql = @"  Select QC.CourseSNO,QC.CourseName,QC.CHour,C.MVal 上課方式,C1.MVal 課程類別 from QS_Course QC
            Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO=QC.PClassSNO
            Left Join Config C On C.PVal=QC.Ctype and C.PGroup='CourseCType'
			Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
            where QCPC.PClassSNO=@PClassSNO and QC.Class1 <> 2";
            lb_Notice.Visible = true;
        }
        else
        {
            dicpd.Add("CourseSNO", CourseSNO);
            sql = @"SELECT QC.CourseSNO,QC.CourseName,QC.CHour,C.MVal 上課方式,C1.MVal 課程類別 FROM QS_Course QC 
            Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO = QC.PClassSNO
            Left Join Config C On C.PVal = QC.Ctype and C.PGroup = 'CourseCType'
            Left Join Config C1 On C1.PVal = QC.Class1 and C1.PGroup = 'CourseClass1'
            WHERE CourseSNO = @CourseSNO  ";
        }

        DataTable objDT = ObjDH.queryData(sql, dicpd);
        gv_CourseName.DataSource = objDT.DefaultView;
        gv_CourseName.DataBind();
    }
    protected static bool CheckPairCourseSNO(string CourseSNO)
    {
        Dictionary<string, object> adict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        string sql = @"Select * from QS_Course where PairCourseSNO <> 0 and CourseSNO=@CourseSNO";
        adict.Add("CourseSNO", CourseSNO);
        DataTable objDT = ObjDH.queryData(sql, adict);
        if (objDT.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }


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
            case "視訊":
                return "0";
        }


        return "";
    }
}




public class OrderItem
{
    public string ROW_NO { set; get; }
    public string MEMBER_NAME { set; get; }
    public string ID { set; get; }
    public string Score { set; get; }
    public string Date { set; get; }
    public string PassScore { get; set; }
    public string IsPass { get; set; }
    public string Status { set; get; }
    public string style { set; get; }
    public string ClassLocation { get; set; }
    public string Unit { get; set; }
    public string CourseSNO { get; set; }
    public string Ctype { get; set; }
}