using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_RePerson : System.Web.UI.Page
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

        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string errorMessage = "";


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
            string fileName = file_Upload.FileName;
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
                        //item.PName = workSheet.Cells[rowIterator, 2].Text;                       
                        item.PersonID = workSheet.Cells[rowIterator, 1].Text;
                        item.CtypeSNO = workSheet.Cells[rowIterator, 2].Text;
                        item.CertID= workSheet.Cells[rowIterator, 3].Text;
                        item.CUnitSNO = workSheet.Cells[rowIterator, 4].Text;
                        //item.PrestoredNumber = workSheet.Cells[rowIterator, 4].Text;
                        item.CertPublic = workSheet.Cells[rowIterator, 5].Text;
                        item.CertStart = workSheet.Cells[rowIterator, 6].Text;
                        item.CertEnd = workSheet.Cells[rowIterator, 7].Text;
                        item.SysChange = workSheet.Cells[rowIterator, 8].Text;
                        item.IsChange = workSheet.Cells[rowIterator, 9].Text;
                        //item.Note = workSheet.Cells[rowIterator, 6].Text;
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
                    if (item.PersonID == "" && item.CertID == "" ) return;
                    Dictionary<string, object> dicpd = new Dictionary<string, object>();
                    //string PName= Convert.ToString(item.PName);
                    string PersonID = Convert.ToString(item.PersonID);
                    string CtypeSNO = Convert.ToString(item.CtypeSNO);
                    string CertID = Convert.ToString(item.CertID);
                    //string PrestoredNumber= Convert.ToString(item.PrestoredNumber);
                    string CunitSNO = Convert.ToString(item.CUnitSNO);
                    DateTime CertPublic = Convert.ToDateTime(item.CertPublic);
                    DateTime CertStart = Convert.ToDateTime(item.CertPublic);
                    DateTime CertEnd = Convert.ToDateTime(item.CertPublic).AddYears(3).AddDays(-1);
                    string Note = Convert.ToString(item.Note);
                    //dicpd.Add("PName", PName);
                    dicpd.Add("PersonID", PersonID);
                    dicpd.Add("CTypeSNO", CtypeSNO);
                    dicpd.Add("CertID", CertID);
                    dicpd.Add("CUnitSNO", CunitSNO);
                    dicpd.Add("CertPublicDate", CertPublic);
                    dicpd.Add("CertStartDate", CertStart);
                    dicpd.Add("CertEndDate", CertEnd);
                    dicpd.Add("SysChange", "0");
                    dicpd.Add("IsChange", "0");
                    DataHelper objDH = new DataHelper();
                    //需檢查person表中是否有這個人
                    string sql = @"
                         If not Exists(select 1 From QS_Certificate Where PersonID=@PersonID and CTypeSNO=@CTypeSNO)
                        BEGIN
                           INSERT INTO [dbo].[QS_Certificate] ([PersonID],[CertID],[CTypeSNO],[CUnitSNO],[CertPublicDate],[CertStartDate],[CertEndDate],[CertExt],[CreateDT],[CreateUserID],[SysChange],[IsChange])
                            VALUES
                        	(@PersonID,@CertID,@CTypeSNO,@CUnitSNO,@CertPublicDate,@CertStartDate,@CertEndDate,0,getdate(),2,@SysChange,@IsChange)
                        End            
                       
                    ";
                    DataTable dt = objDH.queryData(sql, dicpd);

                }
            }
        }

    }

    #region
    //原始版本
    //protected void btnUpload_Click(object sender, EventArgs e)
    //{
    //    string errorMessage = "";


    //    if ((file_Upload != null) && (file_Upload.PostedFile.ContentLength > 0) && !string.IsNullOrEmpty(file_Upload.FileName))
    //    {
    //        string extension = Path.GetExtension(file_Upload.FileName).ToLowerInvariant();
    //        List<string> allowedExtextsion = new List<string> { ".xlsx", ".xls" };
    //        if (allowedExtextsion.IndexOf(extension) == -1) errorMessage += "請上傳 (xlxs,xls) 類型檔案";
    //        if (!String.IsNullOrEmpty(errorMessage))
    //        {
    //            Utility.showMessage(Page, "ErrorMessage", errorMessage);
    //            return;
    //        }

    //        List<OrderItem> orderItem = new List<OrderItem>();
    //        string fileName = file_Upload.FileName;
    //        using (var package = new ExcelPackage(file_Upload.PostedFile.InputStream))
    //        {
    //            var currentSheet = package.Workbook.Worksheets;
    //            var workSheet = currentSheet.First();
    //            var noOfCol = workSheet.Dimension.End.Column;
    //            var noOfRow = workSheet.Dimension.End.Row;
    //            int rowid = 0;
    //            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
    //            {
    //                rowid += 1;
    //                OrderItem item = new OrderItem();
    //                try
    //                {
    //                    item.PersonID = workSheet.Cells[rowIterator, 1].Text;
    //                    item.CertID = workSheet.Cells[rowIterator, 2].Text;
    //                    item.CtypeSNO = workSheet.Cells[rowIterator, 3].Text;
    //                    item.CUnitSNO = workSheet.Cells[rowIterator, 4].Text;
    //                    item.CertPublicDate = workSheet.Cells[rowIterator, 5].Text;
    //                    item.CertStartDate = workSheet.Cells[rowIterator, 6].Text;
    //                    item.CertEndDate = workSheet.Cells[rowIterator, 7].Text;
    //                    item.CertExt = workSheet.Cells[rowIterator, 8].Text;
    //                    item.IsPrint = workSheet.Cells[rowIterator, 9].Text;
    //                    item.SysChange = workSheet.Cells[rowIterator, 10].Text;
    //                    item.IsChange = workSheet.Cells[rowIterator, 11].Text;
    //                    item.IsChangeTime = workSheet.Cells[rowIterator, 12].Text;
    //                    item.Note = workSheet.Cells[rowIterator, 13].Text;
    //                    item.CreateDT = workSheet.Cells[rowIterator, 14].Text;
    //                    item.CreateUserID = workSheet.Cells[rowIterator, 15].Text;
    //                    item.ModifyDT = workSheet.Cells[rowIterator, 16].Text;
    //                    item.ModifyUserID = workSheet.Cells[rowIterator, 17].Text;
    //                    item.CertSNO = workSheet.Cells[rowIterator, 18].Text;

    //                }
    //                catch (Exception ex)
    //                {

    //                }
    //                finally
    //                {
    //                    orderItem.Add(item);
    //                }
    //            }
    //            foreach (OrderItem item in orderItem)
    //            {
    //                Dictionary<string, object> dicpd = new Dictionary<string, object>();
    //                int CTypeSNO = Convert.ToInt16(item.CtypeSNO);
    //                int CUnitSNO = Convert.ToInt16(item.CUnitSNO);
    //                int CreateUserID = Convert.ToInt16(item.CreateUserID);
    //                //int ModifyUserID = Convert.ToInt16(item.ModifyUserID);

    //                DateTime CertStartDate = Convert.ToDateTime(item.CertStartDate);
    //                DateTime CertPublicDate = Convert.ToDateTime(item.CertPublicDate);
    //                DateTime CertEndDate = Convert.ToDateTime(item.CertEndDate);
    //                //DateTime CreateDT = Convert.ToDateTime(item.CreateDT);
    //                //DateTime ModifyDT = Convert.ToDateTime(item.ModifyDT);
    //                DateTime CertExtDate = CertPublicDate.AddYears(6).AddDays(-1);
    //                int CertExtCount = 0;
    //                if (CertEndDate > CertExtDate)
    //                {

    //                    CertExtCount = 1;

    //                }
    //                dicpd.Add("PersonID", item.PersonID);
    //                dicpd.Add("CertID", item.CertID);
    //                dicpd.Add("CTypeSNO", CTypeSNO);
    //                dicpd.Add("CUnitSNO", CUnitSNO);
    //                dicpd.Add("CertStartDate", CertStartDate);
    //                dicpd.Add("CertPublicDate", CertPublicDate);
    //                dicpd.Add("CertEndDate", CertEndDate);
    //                dicpd.Add("CertExt", item.CertExt);
    //                dicpd.Add("IsPrint", item.IsPrint);
    //                dicpd.Add("CreateUserID", CreateUserID);
    //                dicpd.Add("SysChange", item.SysChange);
    //                dicpd.Add("IsChange", item.IsChange);

    //                dicpd.Add("Note", item.Note);
    //                DataHelper objDH = new DataHelper();
    //需檢查person表中是否有這個人
    //string sql = @"
    //                       If not Exists(select 1 From QS_Certificate Where PersonID=@PersonID)
    //                        BEGIN
    //                        Insert Into Person([RoleSNO],[PAccount],[PPWD],[PCard],[LoginCount],[IsEnable],[MStatusSNO],[MNote],[StartDate],[EndDate],[PasswordModilyDT],[CreateDT],[CreateUserID],[ModifyDT],[ModifyUserID],[LoginError],[LoginErrorTime],[PName],[PersonID],[PBirthDate],[PSex],[Country],[PTel_O],[PFax_O],[PTel],[PFax],[PMail],[PPhone],[PZCode],[PAddr],[OrganSNO],[JMajor],[JLicID],[JLicStatus],[JobTitle],[SchoolName],[Major],[Degree],[QSExp],[TJobType],[TSType],[City],[Area]) 
    //                        VALUES (@RoleSNO,@PAccount,@PPWD,@PCard,@LoginCount,@IsEnable,@MStatusSNO,@MNote,@StartDate,@EndDate,@PasswordModilyDT,@CreateDT,@CreateUserID,@ModifyDT,@ModifyUserID,@LoginError,@LoginErrorTime,@PName,@PersonID,@PBirthDate,@PSex,@Country,@PTel_O,@PFax_O,@PTel,@PFax,@PMail,@PPhone,@PZCode,@PAddr,@OrganSNO,@JMajor,@JLicID,@JLicStatus,@JobTitle,@SchoolName,@Major,@Degree,@QSExp,@TJobType,@TSType,@City,@Area)
    //                        End
    //                        Else
    //                        Update Person set PName=@PName,PAddr=@PAddr,City=@City, Area=@Area ,MStatusSNO=@MStatusSNO ,PBirthDate=@PBirthDate , JLicStatus=@JLicStatus  where PersonID=@PersonID

    //";
    //需檢查person表中是否有這個人
    //                string sql = @"

    //               Insert Into QS_Certificate([PersonID],[CertID],[CTypeSNO],[CUnitSNO],[CertPublicDate],[CertStartDate],[CertEndDate],[CertExt],[IsPrint],[CreateUserID],[SysChange],[IsChange]) 
    //               VALUES (@PersonID,@CertID,@CTypeSNO,@CUnitSNO,@CertPublicDate,@CertStartDate,@CertEndDate,@CertExt,@IsPrint,@CreateUserID,@SysChange,@IsChange)

    //                ";

    //                DataTable dt = objDH.queryData(sql, dicpd);

    //            }
    //        }
    //    }

    //}
    #endregion
    #region
    //public class OrderItem
    //{
    //    public string PersonID { set; get; }
    //    public string CertID { set; get; }
    //    public string CtypeSNO { set; get; }
    //    public string CUnitSNO { set; get; }
    //    public string CertPublicDate { set; get; }
    //    public string CertStartDate { set; get; }
    //    public string CertEndDate { set; get; }
    //    public string CertExt { set; get; }
    //    public string IsPrint { set; get; }
    //    public string SysChange { set; get; }
    //    public string IsChange { set; get; }
    //    public string IsChangeTime { set; get; }
    //    public string Note { set; get; }
    //    public string CreateDT { set; get; }
    //    public string CreateUserID { set; get; }
    //    public string ModifyDT { set; get; }
    //    public string ModifyUserID { set; get; }
    //    public string CertSNO { set; get; }
    //}
    #endregion
    public class OrderItem
    {
        //public string PName { set; get; }
        public string PersonID { set; get; }
        public string CertID { set; get; }
        //public string PrestoredNumber { set; get; }
        public string CtypeSNO { set; get; }
        public string CUnitSNO { set; get; }
        public string CertPublic { set; get; }
        public string CertStart { set; get; }
        public string CertEnd { set; get; }
        public string SysChange { set; get; }
        public string IsChange { set; get; }        
        public string Note { set; get; }
    }
}