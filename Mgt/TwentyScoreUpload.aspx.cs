using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_TwentyScoreUpload : System.Web.UI.Page
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

            setEventName(ddl_EventName, "請選擇");

        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

        String filePath = Directory.GetCurrentDirectory() + @"/SysFile/證書實體課程學分.xlsx";
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


    #region 設定下拉選單

    protected void setEventName(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();

        string sql = "";

        sql = @"
            Select EventName,ERSNO from Event where ERSNO is not null and  DATEADD(MONTH,3,EndTime) > GETDATE()
        ";

        sql += Utility.setSQLAccess_ByCreateUserID(wDict, userInfo);

        DataTable objDT = objDH.queryData(sql, wDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }

    #endregion


    #region 上傳檔案

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string errorMessage = "";

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

            List<OrderItem> orderItem = new List<OrderItem>();
            string fileName = file_Upload.FileName;
            using (var package = new ExcelPackage(file_Upload.PostedFile.InputStream))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                int rowid = 1;
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {


                    OrderItem item = new OrderItem();
                    try
                    {

                        item.ROW_NO = rowid.ToString();
                        item.MEMBER_NAME = workSheet.Cells[rowIterator, 1].Text;
                        if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 2].Text)) item.Status += "身分證空白! ;";
                        item.ID = workSheet.Cells[rowIterator, 2].Text;
                        //int score;
                        //var isScore = Int32.TryParse(workSheet.Cells[rowIterator, 3].Text, out score);
                        //item.Score = score.ToString();
                        //if (!isScore) item.Status += "分數格式不正確! ;";
                        if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 3].Text)) item.Status += "上課地點空白! ;";
                        item.ClassLocation = workSheet.Cells[rowIterator, 3].Text;

                        DateTime date;
                        var isDate = DateTime.TryParse(workSheet.Cells[rowIterator, 4].Text, out date);
                        if (!isDate)
                        {
                            item.Date = workSheet.Cells[rowIterator, 4].Text;
                            item.Status += "日期格式不正確! ;";
                        }
                        else
                        {
                            item.Date = date.ToString("yyyy-MM-dd");
                        }
                        if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 4].Text)) item.Status += "上課日期空白! ;";
                        if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 5].Text)) item.Status += "辦理單位空白! ;";
                        item.Unit = workSheet.Cells[rowIterator, 5].Text;



                    }
                    catch (Exception ex)
                    {
                        item.ROW_NO = rowid.ToString();
                        item.MEMBER_NAME = workSheet.Cells[rowIterator, 1].Text;
                        item.ID = workSheet.Cells[rowIterator, 2].Text;
                        item.ClassLocation = workSheet.Cells[rowIterator, 3].Text;
                        item.Date = workSheet.Cells[rowIterator, 4].Text;
                        item.Unit = workSheet.Cells[rowIterator, 5].Text;
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
                foreach (OrderItem item in orderItem)
                {
                    if (!string.IsNullOrEmpty(item.Status)) continue;
                    string PersonSNO = QueryPersonSNO(item.ID);
                    string RoleSNO = QueryRoleSNO(item.ID);
                    string CourseSNO = QueryCourseSNO(RoleSNO, ddl_EventName.SelectedValue, item.ID);
                    if (CourseSNO == "-1")
                    {
                        item.Status += "上傳失敗，查無此繫結課程!";
                        item.style += "color:red; font-weight:bold;";
                        continue;
                    }
                    if (PersonSNO.Equals("-1"))
                    {
                        item.Status += "上傳失敗，查無此帳號!";
                        item.style += "color:red; font-weight:bold;";
                        continue;
                    }
                    //上傳Code
                    if (UpdateScore_Loacation(CourseSNO, PersonSNO, item.ClassLocation, item.Date, item.Unit))
                    {
                        item.Status = "上傳成功!";
                        item.style = "color:blue;";
                    }
                    else
                    {
                        item.Status = "上傳失敗，查無記錄!";
                        item.style = "color:red; font-weight:bold;";
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
    protected string QueryRoleSNO(string PersonID)
    {
        Dictionary<string, object> dicpd = new Dictionary<string, object>();
        dicpd.Add("PersonID", PersonID);

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT RoleSNO FROM Person WHERE PersonID=@PersonID ", dicpd);
        if (objDT.Rows.Count == 0)
        {
            return "-1";
        }
        return objDT.Rows[0]["RoleSNO"].ToString();
    }
    protected string GetPersonIntegral(string PersonSNO, string RoleSNO,string CourseSNO,string CourseSNOForJunior)
    {
        string PClassSNO = CourseToPClassSNO(CourseSNO);
        string PClassSNOForJunior = CourseToPClassSNO(CourseSNOForJunior);
        Dictionary<string, object> dicpd = new Dictionary<string, object>();
        dicpd.Add("PersonSNO", PersonSNO);
        string Sql = @"with aa as (
                Select isnull(count(PClassSNO),0) as CountCourseSNO,PersonSNO from QS_integral QI
                        Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO and PClassSNO=@PClassSNO
                        where PersonSNO=@PersonSNO  GROUP BY PersonSNO)
                        ,bb as (
                        Select isnull(count(PClassSNO),0) as CountCourseSNOForJunior,PersonSNO from QS_integral QI
                        Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO  and PClassSNO=@PClassSNOForJunior
                        where PersonSNO=@PersonSNO GROUP BY PersonSNO)
                    Select isnull(aa.CountCourseSNO,0)CountCourseSNO,isnull(bb.CountCourseSNOForJunior,0)CountCourseSNOForJunior from aa
                    Left Join bb On bb.PersonSNO=aa.PersonSNO";
        DataHelper objDH = new DataHelper();
        dicpd.Add("PClassSNO", PClassSNO);
        dicpd.Add("PClassSNOForJunior", PClassSNOForJunior);
        DataTable ObjDT = objDH.queryData(Sql, dicpd);
        
        switch (RoleSNO)
        {
            case "10":
                if (ObjDT.Rows.Count > 0)
                {
                    if (Convert.ToInt16(ObjDT.Rows[0]["CountCourseSNO"]) == 14)
                    {
                        return "0";//寫入報名規則的未取得CourseSNO
                    }
                    else if (Convert.ToInt16(ObjDT.Rows[0]["CountCourseSNO"]) >= 7 && Convert.ToInt16(ObjDT.Rows[0]["CountCourseSNO"]) < 14)
                    {
                        //CourseCount來自，先去找後台的皆未取得之上傳課程代號，利用課程代號去尋找課程規劃
                        //在使用課程規劃去查積分表裡面對於這個課程規劃之課程，他取得了多少積分
                        int CourseCount = Convert.ToInt16(ObjDT.Rows[0]["CountCourseSNO"]);
                        //CourseJCount來自，去找後台的取得初階之上船課程代號利用課程代號去尋找課程規劃
                        //在使用課程規劃去查積分表裡面對於這個課程規劃之課程，他取得了多少積分
                        int CourseJCount = Convert.ToInt16(ObjDT.Rows[0]["CountCourseSNOForJunior"]);
                        //如果他皆未取得的課程數較多，就拿皆未取得之課程代號上船給他積分，反之。
                        if (CourseCount > CourseJCount)
                        {
                            return "0";//寫入報名規則的未取得CourseSNO
                        }
                        else
                        {
                            return "1"; //寫入報名規則的初階CourseSNOForJunior
                        }

                    }
                }
                else
                {
                    return "0";//寫入報名規則的未取得CourseSNO
                }
                break;
            case "11":
                if (ObjDT.Rows.Count > 0)
                {
                    return "0";//寫入報名規則的未取得CourseSNO
                }
                break;
            case "12":
                if (ObjDT.Rows.Count > 0)
                {
                    if (Convert.ToInt16(ObjDT.Rows[0]["CountCourseSNO"]) == 14)
                    {
                        return "0";//寫入報名規則的未取得CourseSNO
                    }
                    else if (Convert.ToInt16(ObjDT.Rows[0]["CountCourseSNO"]) >= 7 && Convert.ToInt16(ObjDT.Rows[0]["CountCourseSNO"]) < 14)
                    {
                        //CourseCount來自，先去找後台的皆未取得之上傳課程代號，利用課程代號去尋找課程規劃
                        //在使用課程規劃去查積分表裡面對於這個課程規劃之課程，他取得了多少積分
                        int CourseCount = Convert.ToInt16(ObjDT.Rows[0]["CountCourseSNO"]);
                        //CourseJCount來自，去找後台的取得初階之上船課程代號利用課程代號去尋找課程規劃
                        //在使用課程規劃去查積分表裡面對於這個課程規劃之課程，他取得了多少積分
                        int CourseJCount = Convert.ToInt16(ObjDT.Rows[0]["CountCourseSNOForJunior"]);
                        //如果他皆未取得的課程數較多，就拿皆未取得之課程代號上船給他積分，反之。
                        if(CourseCount > CourseJCount)
                        {
                            return "0";//寫入報名規則的未取得CourseSNO
                        }
                        else
                        {
                            return "1"; //寫入報名規則的初階CourseSNOForJunior
                        }
                        
                    }

                }
                else
                {
                    return "0";//寫入報名規則的未取得CourseSNO
                }
                break;
            case "13":
                if (ObjDT.Rows.Count > 0)
                {
                    if (Convert.ToInt16(ObjDT.Rows[0]["CountCourseSNO"]) == 14)
                    {
                        return "0";//寫入報名規則的未取得CourseSNO
                    }
                    else if (Convert.ToInt16(ObjDT.Rows[0]["CountCourseSNO"]) >= 7 && Convert.ToInt16(ObjDT.Rows[0]["CountCourseSNO"]) < 14)
                    {

                        //CourseCount來自，先去找後台的皆未取得之上傳課程代號，利用課程代號去尋找課程規劃
                        //在使用課程規劃去查積分表裡面對於這個課程規劃之課程，他取得了多少積分
                        int CourseCount = Convert.ToInt16(ObjDT.Rows[0]["CountCourseSNO"]);
                        //CourseJCount來自，去找後台的取得初階之上船課程代號利用課程代號去尋找課程規劃
                        //在使用課程規劃去查積分表裡面對於這個課程規劃之課程，他取得了多少積分
                        int CourseJCount = Convert.ToInt16(ObjDT.Rows[0]["CountCourseSNOForJunior"]);
                        //如果他皆未取得的課程數較多，就拿皆未取得之課程代號上船給他積分，反之。
                        if (CourseCount > CourseJCount)
                        {
                            return "0";//寫入報名規則的未取得CourseSNO
                        }
                        else
                        {
                            return "1"; //寫入報名規則的初階CourseSNOForJunior
                        }
                    }

                }
                else
                {
                    return "0";//寫入報名規則的未取得CourseSNO
                }
                break;
        }


        return "";
    }

    protected string QueryCourseSNO(string RoleSNO, string EventSNO, string PersonID)
    {
        string CourseSNO = "";string CourseSNOForJunior = "";
        string PersonSNO = Utility.ConvertPersonIDToPersonSNO(PersonID);
        Dictionary<string, object> dicpd = new Dictionary<string, object>();
        dicpd.Add("RoleSNO", RoleSNO);
        dicpd.Add("EventSNO", EventSNO);
        DataHelper objDH = new DataHelper();
        string sql = @"Select ERD.RoleSNO,ERD.CourseSNO,ERD.CourseSNOForJunior from Event E
                    Left Join EventRoleDetail ERD On ERD.ERSNO=E.ERSNO
                     where ERD.ERSNO=@EventSNO and RoleSNO=@RoleSNO";
        DataTable objDT = objDH.queryData(sql, dicpd);

         
        if (objDT.Rows.Count == 0)
        {
            return "-1";
        }
        else
        {
            CourseSNO = objDT.Rows[0]["CourseSNO"].ToString();
            CourseSNOForJunior = objDT.Rows[0]["CourseSNOForJunior"].ToString();
        }
    
        string CheckInsertCourse = GetPersonIntegral(PersonSNO, RoleSNO, CourseSNO, CourseSNOForJunior);
        if (CheckInsertCourse == "0")
        {
            return objDT.Rows[0]["CourseSNO"].ToString();
        }
        else
        {
            return objDT.Rows[0]["CourseSNOForJunior"].ToString();
        }

    }
    public string CourseToPClassSNO(string CourseSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> dicpd = new Dictionary<string, object>();
        string[] CourseArray = CourseSNO.Split(',');
        string sql = "Select * from QS_Course where CourseSNO=@CourseSNO";
        dicpd.Add("CourseSNO", CourseArray[0]);
        DataTable objDT = objDH.queryData(sql, dicpd);
        if (objDT.Rows.Count > 0)
        {
            return objDT.Rows[0]["PClassSNO"].ToString();
        }
        return "";
    }

   
    protected bool UpdateScore_Loacation(string CourseSNO, string PersonSNO, string ClassLocation, string date, string Unit)
    {
        //bool Insert_ispass;
        //if (IsPass == "是") Insert_ispass = true;
        //else Insert_ispass = false;
        string[] CourseSNOArray = CourseSNO.Split(',');
        Dictionary<string, object> dicpd = new Dictionary<string, object>();
        Dictionary<string, object> wicpd = new Dictionary<string, object>();
        for (int i = 0; i < CourseSNOArray.Length; i++)
        {


            dicpd.Add("CourseSNO", CourseSNOArray[i]);
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
       
                Insert Into QS_LearningUpload(PersonSNO,CourseSNO,ExamDate,Unit,CreateDT,CreateUserID) 
                VALUES (@PersonSNO,@CourseSNO,@date,@unit,@CreateDT,@CreateUserID)
        ";
            DataTable dt = objDH.queryData(sql, dicpd);
            if (dt.Rows.Count > 0)
            {

                if (Convert.ToInt16(dt.Rows[0]["rowcount"].ToString()) == 0) return false;
            }
            else
            {


                string InsertIntegral = @"
            If Exists(select 1 From QS_Integral Where PersonSNO=@PersonSNO And CourseSNO=@CourseSNO)
                
                 Select 1
            Else
                Insert Into QS_Integral(PersonSNO,CourseSNO,CreateDT,CreateUserID,AuthType) 
                VALUES (@PersonSNO,@CourseSNO,@CreateDT,@CreateUserID,@AuthType)";

                wicpd.Add("PersonSNO", PersonSNO);
                wicpd.Add("CourseSNO", CourseSNOArray[i]);
                wicpd.Add("AuthType", 0);
                wicpd.Add("CreateDT", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                wicpd.Add("CreateUserID", userInfo.PersonSNO);
                DataTable DS = objDH.queryData(InsertIntegral, wicpd);
                dicpd.Clear();
                wicpd.Clear();

            }
        }


        return true;
    }
    
    #endregion


    #region 上傳檔案結果

    protected void bindData(List<OrderItem> dataSource)
    {

        this.gv_ScoreUpload_Class.DataSource = dataSource;
        this.gv_ScoreUpload_Class.DataBind();


    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
    }

    #endregion



}




public class OrderItem
{
    public string ROW_NO { set; get; }
    public string MEMBER_NAME { set; get; }
    public string ID { set; get; }
    public string Date { set; get; }
    public string Status { set; get; }
    public string style { set; get; }
    public string ClassLocation { get; set; }
    public string Unit { get; set; }
    public string CourseSNO { get; set; }
    public string CourseSNOForJunior { get; set; }
}



