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
                        item.ROW_NO = rowid.ToString();
                        item.RoleSNO = workSheet.Cells[rowIterator, 1].Text;
                        item.PAccount = workSheet.Cells[rowIterator, 2].Text;
                        item.PPWD = workSheet.Cells[rowIterator, 3].Text;
                        item.PCard = workSheet.Cells[rowIterator, 4].Text;
                        item.LoginCount = workSheet.Cells[rowIterator, 5].Text;
                        item.IsEnable = workSheet.Cells[rowIterator, 6].Text;
                        item.MStatusSNO = workSheet.Cells[rowIterator, 7].Text;
                        item.MNote = workSheet.Cells[rowIterator, 8].Text;
                        item.StartDate = workSheet.Cells[rowIterator, 9].Text;
                        item.EndDate = workSheet.Cells[rowIterator, 10].Text;
                        item.PasswordModilyDT = workSheet.Cells[rowIterator, 11].Text;
                        item.CreateDT = workSheet.Cells[rowIterator, 12].Text;
                        item.CreateUserID = workSheet.Cells[rowIterator, 13].Text;
                        item.ModifyDT = workSheet.Cells[rowIterator, 14].Text;
                        item.ModifyUserID = workSheet.Cells[rowIterator, 15].Text;
                        item.LoginError = workSheet.Cells[rowIterator, 16].Text;
                        item.LoginErrorTime = workSheet.Cells[rowIterator, 17].Text;
                        item.PName = workSheet.Cells[rowIterator, 18].Text;
                        item.PSex = workSheet.Cells[rowIterator, 19].Text;
                        item.PersonID = workSheet.Cells[rowIterator, 20].Text;
                        item.PBirthDate = workSheet.Cells[rowIterator, 21].Text;                      
                        item.Country = workSheet.Cells[rowIterator, 22].Text;
                        item.PTel = workSheet.Cells[rowIterator, 23].Text;
                        item.PFax = workSheet.Cells[rowIterator, 24].Text;
                        item.PTel_O = workSheet.Cells[rowIterator, 25].Text;
                        item.PFax_O = workSheet.Cells[rowIterator, 26].Text;
                        item.PMail = workSheet.Cells[rowIterator, 27].Text;
                        item.PPhone = workSheet.Cells[rowIterator, 28].Text;
                        item.PZCode = workSheet.Cells[rowIterator, 29].Text;
                        item.PAddr = workSheet.Cells[rowIterator, 31].Text;
                        item.PAddr2 = workSheet.Cells[rowIterator, 30].Text;
                        item.OrganSNO = workSheet.Cells[rowIterator, 32].Text;
                        item.JMajor = workSheet.Cells[rowIterator, 33].Text;
                        item.JLicID = workSheet.Cells[rowIterator, 34].Text;
                        item.JLicStatus = workSheet.Cells[rowIterator, 35].Text;
                        item.JobTitle = workSheet.Cells[rowIterator, 36].Text;
                        item.SchoolName = workSheet.Cells[rowIterator,37].Text;
                        item.Major = workSheet.Cells[rowIterator, 38].Text;
                        item.Degree = workSheet.Cells[rowIterator, 39].Text;
                        item.QSExp = workSheet.Cells[rowIterator, 40].Text;
                        item.TJobType = workSheet.Cells[rowIterator, 41].Text;
                        item.TSType = workSheet.Cells[rowIterator, 42].Text;
                        item.PZCode2 = workSheet.Cells[rowIterator, 43].Text;
                        item.City = workSheet.Cells[rowIterator, 44].Text;
                        item.Area = workSheet.Cells[rowIterator, 45].Text;
                        item.Note= workSheet.Cells[rowIterator, 46].Text;
                    }
                    catch (Exception ex)
                    {
                        
                        item.Status = ex.Message.ToString();
                    }
                    finally
                    {
                        orderItem.Add(item);
                    }                    
                }
                foreach (OrderItem item in orderItem)
                {
                    string PAddr = item.PAddr.ToString();
                    if (item.PAddr != "")
                    {

                        bool CheckAddrC = PAddr.Contains(item.City);
                        bool CheckAddrA = PAddr.Contains(item.Area);
                        if (item.Area != "" && item.City != "")
                        {
                            if (CheckAddrC == true)
                            {
                                PAddr = PAddr.Replace(item.City, "");
                            }
                            if (CheckAddrA == true)
                            {
                                PAddr = PAddr.Replace(item.Area, "");
                            }
                        }
                        
                    }
                    Dictionary<string, object> dicpd = new Dictionary<string, object>();
                    dicpd.Add("ROW_NO",item.ROW_NO);
                    dicpd.Add("RoleSNO",item.RoleSNO);
                    dicpd.Add("PAccount",item.PAccount);
                    dicpd.Add("PPWD",item.PPWD);
                    dicpd.Add("PCard",item.PCard);
                    dicpd.Add("LoginCount",item.LoginCount);
                    dicpd.Add("IsEnable",item.IsEnable);
                    dicpd.Add("MStatusSNO",item.MStatusSNO);
                    dicpd.Add("MNote",item.MNote);
                    dicpd.Add("StartDate",item.StartDate);
                    dicpd.Add("EndDate",item.EndDate);
                    dicpd.Add("PasswordModilyDT",item.PasswordModilyDT);
                    dicpd.Add("CreateDT",item.CreateDT);
                    dicpd.Add("CreateUserID",item.CreateUserID);
                    dicpd.Add("ModifyDT",item.ModifyDT);
                    dicpd.Add("ModifyUserID",item.ModifyUserID);
                    dicpd.Add("LoginError",item.LoginError);
                    dicpd.Add("LoginErrorTime",item.LoginErrorTime);
                    dicpd.Add("PName",item.PName);
                    dicpd.Add("PersonID",item.PersonID);
                    dicpd.Add("PBirthDate", item.PBirthDate);
                    dicpd.Add("PSex",item.PSex);
                    dicpd.Add("Country",item.Country);
                    dicpd.Add("PTel_O", item.PTel_O);
                    dicpd.Add("PFax_O", item.PFax_O);
                    dicpd.Add("PTel",item.PTel);
                    dicpd.Add("PFax",item.PFax);
                    dicpd.Add("PMail",item.PMail);
                    dicpd.Add("PPhone",item.PPhone);
                    dicpd.Add("PZCode",item.PZCode);
                    dicpd.Add("PAddr", PAddr);
                    dicpd.Add("OrganSNO",item.OrganSNO);
                    dicpd.Add("JMajor",item.JMajor);
                    dicpd.Add("JLicID",item.JLicID);
                    dicpd.Add("JLicStatus",item.JLicStatus);
                    dicpd.Add("JobTitle",item.JobTitle);
                    dicpd.Add("SchoolName",item.SchoolName);
                    dicpd.Add("Major",item.Major);
                    dicpd.Add("Degree",item.Degree);
                    dicpd.Add("QSExp",item.QSExp);
                    dicpd.Add("TJobType",item.TJobType);
                    dicpd.Add("TSType",item.TSType);
        
                    dicpd.Add("City", item.City);
                    dicpd.Add("Area", item.Area);
                    dicpd.Add("Note", item.Note);
                    DataHelper objDH = new DataHelper();
                    //需檢查person表中是否有這個人
                    string sql = @"
                                           If not Exists(select 1 From Person Where PersonID=@PersonID)
                                            BEGIN
                                            Insert Into Person([RoleSNO],[PAccount],[PPWD],[PCard],[LoginCount],[IsEnable],[MStatusSNO],[MNote],[StartDate],[EndDate],[PasswordModilyDT],[CreateDT],[CreateUserID],[ModifyDT],[ModifyUserID],[LoginError],[LoginErrorTime],[PName],[PersonID],[PBirthDate],[PSex],[Country],[PTel_O],[PFax_O],[PTel],[PFax],[PMail],[PPhone],[PZCode],[PAddr],[OrganSNO],[JMajor],[JLicID],[JLicStatus],[JobTitle],[SchoolName],[Major],[Degree],[QSExp],[TJobType],[TSType],[City],[Area]) 
                                            VALUES (@RoleSNO,@PAccount,@PPWD,@PCard,@LoginCount,@IsEnable,@MStatusSNO,@MNote,@StartDate,@EndDate,@PasswordModilyDT,@CreateDT,@CreateUserID,@ModifyDT,@ModifyUserID,@LoginError,@LoginErrorTime,@PName,@PersonID,@PBirthDate,@PSex,@Country,@PTel_O,@PFax_O,@PTel,@PFax,@PMail,@PPhone,@PZCode,@PAddr,@OrganSNO,@JMajor,@JLicID,@JLicStatus,@JobTitle,@SchoolName,@Major,@Degree,@QSExp,@TJobType,@TSType,@City,@Area)
                                            End
                                            Else
                                            Update Person set PName=@PName,PAddr=@PAddr,City=@City, Area=@Area ,MStatusSNO=@MStatusSNO ,PBirthDate=@PBirthDate , JLicStatus=@JLicStatus  where PersonID=@PersonID

                    ";
                    //string sql = @"

                    //    Update Person set RoleSNO=@RoleSNO where personID=@PersonID";
                    DataTable dt = objDH.queryData(sql, dicpd);

                }
            }
        }

    }
    public class OrderItem
    {
        public string ROW_NO            { set; get; }
        public string RoleSNO           { set; get; }
        public string PAccount          { set; get; }
        public string PPWD              { set; get; }
        public string PCard             { set; get; }
        public string LoginCount        { set; get; }
        public string IsEnable          { get; set; }
        public string MStatusSNO        { get; set; }
        public string MNote             { set; get; }
        public string StartDate         { set; get; }
        public string EndDate           { get; set; }
        public string PasswordModilyDT  { get; set; }
        public string CreateDT          { set; get; }
        public string CreateUserID      { set; get; }
        public string ModifyDT          { set; get; }
        public string ModifyUserID      { set; get; }
        public string LoginError        { set; get; }
        public string LoginErrorTime    { get; set; }
        public string PName             { get; set; }
        public string PersonID          { set; get; }
        public string PBirthDate        { set; get; }
        public string PSex              { get; set; }
        public string Country           { get; set; }
        public string PTel_O            { set; get; }
        public string PFax_O            { set; get; }
        public string PTel              { set; get; }
        public string PFax              { set; get; }
        public string PMail             { set; get; }
        public string PPhone            { get; set; }
        public string PZCode            { get; set; }
        public string City              { set; get; }
        public string Area              { set; get; }
        public string PAddr             { set; get; }
        public string PAddr2            { set; get; }
        public string OrganSNO          { get; set; }
        public string JMajor            { get; set; }
        public string JLicID            { set; get; }
        public string JLicStatus        { set; get; }
        public string JobTitle          { set; get; }
        public string SchoolName        { set; get; }
        public string Major             { set; get; }
        public string Degree            { get; set; }
        public string QSExp             { get; set; }
        public string TJobType          { set; get; }
        public string TSType            { set; get; }
        public string PZCode2           { get; set; }
        public string Note              { get; set; }
        public string Status            { get; set; }
        
    }
}