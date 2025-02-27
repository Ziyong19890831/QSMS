using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_EventManager : System.Web.UI.Page
{
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfo;
    public Dictionary<string, string> _SetCol;
    UserInfo userInfo = null;
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            binData();
        }
    }
    protected void binData()
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        String id = Convert.ToString(Request.QueryString["sno"]);
        string TwentySNO = (Request["twenty"]) != null ? Request["twenty"].ToString() : "";
        string CourseSNO = "";
        aDict.Add("EventSNO", id);
        string CtypeSNOSQL = @"SELECT 
                                	  isnull(QCPC.CTypeSNO,0) CTypeSNO,E.PClassSNO,E.EPClassSNO
                                  FROM [New_QSMS].[dbo].[Event] E
                                  Left Join QS_ECoursePlanningClass QEPC On QEPC.EPClassSNO=E.EPClassSNO
                                  Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO=QEPC.PClassSNO
                                  where E.EventSNO=@EventSNO";
        //DataTable ObjDTS = objDH.queryData(CtypeSNOSQL, aDict);
        string Class = "";
        //if (ObjDTS.Rows.Count > 0 && TwentySNO == "")
        //{
        //    if (ObjDTS.Rows[0]["PClassSNO"].ToString() != "")
        //    {
        //        Class = ObjDTS.Rows[0]["PClassSNO"].ToString();
        //        CourseSNO = GetCourseSNO(Class);
        //    }
        //    else
        //    {
        //        Class = ObjDTS.Rows[0]["EPClassSNO"].ToString();
        //        CourseSNO = GetCourseSNOForEPClass(Class);
        //    }
        //}
        //else//22縣市
        //{
        //    string sql = @"Select E.ERSNO,ERD.CourseSNO,ERD.RoleSNO from Event E
        //                    Left Join EventRoleDetail ERD On ERD.ERSNO=E.ERSNO
        //                    where E.EventSNO=@EventSNO";

        //    DataTable twentyObj = objDH.queryData(sql, aDict);
        //    CourseSNO = twentyObj.Rows[0]["CourseSNO"].ToString();
        //}
        //Crecord 1:有到 0:未到
        //舊的
 //       string gv_SQL = @"
 //               SELECT 
 //                   ROW_NUMBER() OVER (ORDER BY e.CreateDT ) as ROW_NO, e.EventDSNO ,
	//                p.PersonSNO,p.PName, p.PersonID,c3.MVal PSex, MP.JCN , p.PMail, p.PTel, p.PPhone, CONVERT(varchar(100), P.PBirthDate, 111) PBirthDate, e.Notice,P.PAddr,
	//				e.DLOADSNO,D.DLOADNAME,D.DLOADURL,
	//                r.RoleName, Convert(varchar(16), e.CreateDT, 120) ApplyDT,E.BGrade,E.AGrade,e.ScoreNote,
 //                    E.CRecord ,
 //                   c1.MVal EventAudit, c1.PVal EventAuditVal ,
	//                c2.MVal EventNotice,MP.LSType,MP.LSCN,O.OrganName,O.OrganAddr,O.OrganTel,c4.MVal MStatusSNO,
 //                  c1.MVal Audit,ED.ScoreNote EDNote,p.Area,p.City,e.Note,e.ScoreNote,(Select TOP(1) QC.CHour from QS_Integral QI Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO where QI.CourseSNO=@CourseSNO and PersonSNO=e.PersonSNO) as Integral
 //                   ,(Select TOP(1) Case  When QI.IsUsed=1 then '已使用' when QI.IsUsed=0 then '未使用' ELSE '未取得' End as isUseds from QS_Integral QI Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO where QI.CourseSNO=@CourseSNO and PersonSNO=e.PersonSNO) as IsUsed

 //               From EventD e
	//                Left Join Person p On p.PersonSNO = e.PersonSNO
	//                Left Join Role r On r.RoleSNO = p.RoleSNO
	//                Left Join Config c1 On c1.PVal = e.Audit And c1.PGroup='EventAudit'
	//                Left Join Config c2 On c2.PVal = e.NoticeType And c2.PGroup='EventNotice'
	//				Left Join Config c3 On c3.PVal = P.PSex And c3.PGroup='Sex'
	//				Left Join Config c4 On c4.PVal = P.MStatusSNO And c4.PGroup='Mstatus'
 //                   Left Join PersonMP MP ON MP.personID=P.PersonID
	//				Left Join Download D On e.DLOADSNO=D.DLOADSNO
	//				Left Join Organ O On O.OrganSNO=P.OrganSNO
	//				Left Join EventD ED ON ED.EventSNO=e.EventSNO and ED.PersonSNO=P.PersonSNO

 //               Where e.EventSNO = @EventSNO
 //           Order by e.CreateDT
 //";
        string gv_SQL = @"
                SELECT 
                    ROW_NUMBER() OVER (ORDER BY e.CreateDT ) as ROW_NO, e.EventDSNO ,
	                p.PersonSNO,p.PName, p.PersonID,c3.MVal PSex, MP.JCN , p.PMail, p.PTel, p.PPhone, CONVERT(varchar(100), P.PBirthDate, 111) PBirthDate, e.Notice,P.PAddr,
					e.DLOADSNO,D.DLOADNAME,D.DLOADURL,
	                r.RoleName, Convert(varchar(16), e.CreateDT, 120) ApplyDT,E.BGrade,E.AGrade,e.ScoreNote,
                     E.CRecord ,
                    c1.MVal EventAudit, c1.PVal EventAuditVal ,
	                c2.MVal EventNotice,MP.LSType,MP.LSCN,O.OrganName,O.OrganAddr,O.OrganTel,c4.MVal MStatusSNO,
                   c1.MVal Audit,ED.ScoreNote EDNote,p.Area,p.City,e.Note,e.ScoreNote,(Select TOP(1) QC.CHour from QS_Integral QI Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO where PersonSNO=e.PersonSNO) as Integral
                    ,(Select TOP(1) Case  When QI.IsUsed=1 then '已使用' when QI.IsUsed=0 then '未使用' ELSE '未取得' End as isUseds from QS_Integral QI Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO where PersonSNO=e.PersonSNO) as IsUsed

                From EventD e
	                Left Join Person p On p.PersonSNO = e.PersonSNO
	                Left Join Role r On r.RoleSNO = p.RoleSNO
	                Left Join Config c1 On c1.PVal = e.Audit And c1.PGroup='EventAudit'
	                Left Join Config c2 On c2.PVal = e.NoticeType And c2.PGroup='EventNotice'
					Left Join Config c3 On c3.PVal = P.PSex And c3.PGroup='Sex'
					Left Join Config c4 On c4.PVal = P.MStatusSNO And c4.PGroup='Mstatus'
                    Left Join PersonMP MP ON MP.personID=P.PersonID
					Left Join Download D On e.DLOADSNO=D.DLOADSNO
					Left Join Organ O On O.OrganSNO=P.OrganSNO
					Left Join EventD ED ON ED.EventSNO=e.EventSNO and ED.PersonSNO=P.PersonSNO

                Where e.EventSNO = @EventSNO
            Order by e.CreateDT
 ";


        //取報名資料
        aDict.Add("CourseSNO", CourseSNO);
        DataTable objDT = objDH.queryData(gv_SQL, aDict);
        
       
 
        gv_EventD.DataSource = objDT.DefaultView;

        Dictionary<string, object> CDict = new Dictionary<string, object>();
        string ReportSQL = @"        SELECT 
                    ROW_NUMBER() OVER (ORDER BY e.CreateDT ) as ROW_NO, e.EventDSNO ,
	                p.PersonSNO,p.PName, p.PersonID,c3.MVal PSex, MP.JCN , p.PMail, p.PTel, p.PPhone, CONVERT(varchar(100), P.PBirthDate, 111) PBirthDate, e.Notice,P.PAddr,
					e.DLOADSNO,D.DLOADNAME,D.DLOADURL,
	                r.RoleName, Convert(varchar(16), e.CreateDT, 120) ApplyDT,E.BGrade,E.AGrade,E.EIntegral,e.ScoreNote,
                     E.CRecord ,
                    c1.MVal EventAudit, c1.PVal EventAuditVal ,case (e.Meals) when '0' then '不需要' when '1' then '葷食' when '2' then '素食' ELSE '無填寫' END Meals ,
	                c2.MVal EventNotice,MP.LSType,MP.LSCN,O.OrganName,O.OrganAddr,O.OrganTel,c4.MVal MStatusSNO,
                   c1.MVal Audit,ED.ScoreNote EDNote,p.Area,p.City,e.Note,e.ScoreNote
                   ,STUFF((
            select (TEMP.CtypeString+','+TEMP.CertPublicDate+','+TEMP.CertStartDate+','+TEMP.CertEndDate+','+TEMP.CUnitName+';' )
			from
			(
                SELECT 
				Case when SysChange=1 then
                replace(CT.CTypeString,'@',RIGHT(REPLICATE('0', 6) + CAST(C.CertID as NVARCHAR), 6) )
				Else
				 replace(CT.CTypeString,'@',C.CertID)
				 END as CtypeString
				  ,
                Cast(C.CertPublicDate As varchar(10)) CertPublicDate,
                Cast(C.CertStartDate As varchar(10)) CertStartDate,
                Cast(C.CertEndDate As varchar(10)) CertEndDate,
                Case C.CertExt When 1 Then '有' Else '無' End CertExt,
				CU.CUnitName
                From  QS_Certificate C
				LEFT JOIN QS_CertificateType CT on CT.CTypeSNO=C.CTypeSNO
                LEFT JOIN QS_CertificateUnit CU on CU.CUnitSNO=C.CUnitSNO
                WHERE 1=1 And  PersonID=p.PersonID
                 ) as TEMP				 
		        For XML PATH('')),1,0,'') as 證書字號資料
                From EventD e
	                Left Join Person p On p.PersonSNO = e.PersonSNO
	                Left Join Role r On r.RoleSNO = p.RoleSNO
	                Left Join Config c1 On c1.PVal = e.Audit And c1.PGroup='EventAudit'
	                Left Join Config c2 On c2.PVal = e.NoticeType And c2.PGroup='EventNotice'
					Left Join Config c3 On c3.PVal = P.PSex And c3.PGroup='Sex'
					Left Join Config c4 On c4.PVal = P.MStatusSNO And c4.PGroup='Mstatus'
                    Left Join PersonMP MP ON MP.personID=P.PersonID
					Left Join Download D On e.DLOADSNO=D.DLOADSNO
					Left Join Organ O On O.OrganSNO=P.OrganSNO
					Left Join EventD ED ON ED.EventSNO=e.EventSNO and ED.PersonSNO=P.PersonSNO
                Where e.EventSNO = @EventSNO ";
        CDict.Add("EventSNO", id);
        DataTable objReport = objDH.queryData(ReportSQL, CDict);


        ReportInit(objReport);

        gv_EventD.DataBind();

        foreach (GridViewRow gridRow in gv_EventD.Rows)
        {
            var CRecord = (DropDownList)gridRow.FindControl("ddl_ClassRecordItem");
            var BGrade = (TextBox)gridRow.FindControl("txt_BGrade");
            var AGrade = (TextBox)gridRow.FindControl("txt_AGrade");
            //var EIntegral = (TextBox)gridRow.FindControl("txt_Integral");
            var Note = (TextBox)gridRow.FindControl("txt_Note");
            var AuditNote = (TextBox)gridRow.FindControl("txt_AuditNote");
            CRecord.SelectedValue = objDT.Rows[gridRow.RowIndex]["CRecord"].ToString();
            BGrade.Text = objDT.Rows[gridRow.RowIndex]["BGrade"].ToString();
            AGrade.Text = objDT.Rows[gridRow.RowIndex]["AGrade"].ToString();
            //EIntegral.Text = objDT.Rows[gridRow.RowIndex]["EIntegral"].ToString();
            //Note.Text = objDT.Rows[gridRow.RowIndex]["ScoreNote"].ToString();
            AuditNote.Text = objDT.Rows[gridRow.RowIndex]["Note"].ToString();
        }

        
    }
    private void ReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("ROW_NO", "序號");
        _SetCol.Add("PName", "姓名");
        _SetCol.Add("JCN", "醫事證號");
        _SetCol.Add("PersonID", "身分證號");
        _SetCol.Add("Psex", "性別");
        _SetCol.Add("PBirthDate", "出生日期");
        _SetCol.Add("PPhone", "行動電話");
        _SetCol.Add("PMail", "E-Mail");
        _SetCol.Add("PAddr", "通訊地址");
        _SetCol.Add("PTel", "通訊電話");
        _SetCol.Add("LSCN", "專科證號");
        _SetCol.Add("OrganName", "現職機構名稱");
        _SetCol.Add("OrganAddr", "現職機構地址");
        _SetCol.Add("OrganTel", "現職機構電話");
        _SetCol.Add("MstatusSNO", "學員狀態");
        _SetCol.Add("證書字號資料", "證書字號/首發日/公告日/到期日/發證單位");
        _SetCol.Add("Audit", "審核狀況");
        _SetCol.Add("Note", "審核備註");
        _SetCol.Add("ScoreNote", "學分備註");
        _SetCol.Add("CRecord", "上課紀錄");
        _SetCol.Add("BGrade", "前測成績");
        _SetCol.Add("AGrade", "後測成績");
        _SetCol.Add("EIntegral", "學分");
        _SetCol.Add("Area", "地區");
        _SetCol.Add("City", "城市");
        _SetCol.Add("Meals", "膳食");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.EventManager.ToString()] = _ExcelInfo;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {

        
        Dictionary<string, object> aDict = new Dictionary<string, object>();
     
        string sql = "";
        foreach (GridViewRow gridRow in gv_EventD.Rows)
        {
            string PersonSNO = gv_EventD.DataKeys[gridRow.RowIndex].Values[0].ToString();
            aDict.Clear();
            var CRecord = (DropDownList)gridRow.FindControl("ddl_ClassRecordItem");
            var hid_EventDid = (HiddenField)gridRow.FindControl("hid_EventDid"); 
            var BGrade = (TextBox)gridRow.FindControl("txt_BGrade");
            var AGrade = (TextBox)gridRow.FindControl("txt_AGrade");
            //var EIntegral = (TextBox)gridRow.FindControl("txt_Integral");
            var AuditNote = (TextBox)gridRow.FindControl("txt_AuditNote");
            var Note = (TextBox)gridRow.FindControl("txt_Note");
            aDict.Add("EventDSNO", hid_EventDid.Value);
            aDict.Add("CRecord", CRecord.SelectedValue);
            aDict.Add("BGrade", BGrade.Text);
            aDict.Add("AGrade", AGrade.Text);
            //aDict.Add("EIntegral", EIntegral.Text);
            aDict.Add("Note", AuditNote.Text);
            //aDict.Add("ScoreNote", Note.Text);
            aDict.Add("PersonSNO", PersonSNO);
            sql += " Update EventD Set CRecord = @CRecord,BGrade=@BGrade,AGrade=@AGrade,Note=@Note WHERE EventDSNO = @EventDSNO and PersonSNO=@PersonSNO ";
            DataHelper objDH = new DataHelper();
            DataTable objDTPAPER = objDH.queryData(sql, aDict);
        }



       
        Response.Write("<script>alert('送出成功!'); window.location=window.location;</script>");
    }

    protected void gv_EventD_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            //要隱藏的欄位，如果前台filed隱藏會抓不到值，所以在RowCreate時才隱藏
           
            e.Row.Cells[3].Visible = false;
        }
    }

    protected void Invite_download_Click(object sender, EventArgs e)
    {
        if (gv_EventD.Rows.Count == 0)
        {
            Response.Write("<script>alert('尚未查詢')</script>");
            return;
        }
        Utility.OpenExportWindows(this, ReportEnum.EventManager.ToString());
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        String filePath = Directory.GetCurrentDirectory() + @"/SysFile/名單管理上傳.xlsx";
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
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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
                        if (string.IsNullOrEmpty(workSheet.Cells[rowIterator, 1].Text)) item.Status += "身分證空白! ;";
                        item.PersonID = workSheet.Cells[rowIterator, 1].Text.Trim();
                        item.Record = workSheet.Cells[rowIterator, 2].Text;
                        int score;
                        var BeforeScore = Int32.TryParse(workSheet.Cells[rowIterator, 3].Text.Trim(), out score);
                        if (BeforeScore == true)
                        {
                            item.BeforeTest = workSheet.Cells[rowIterator, 3].Text.Trim();
                        }
                        else
                        {
                            item.Status += "分數格式不正確! ";
                        }
                        var AfterScore = Int32.TryParse(workSheet.Cells[rowIterator, 4].Text.Trim(), out score);
                        if (AfterScore == true)
                        {
                            item.AfterTest = workSheet.Cells[rowIterator, 4].Text.Trim();
                        }
                        else
                        {
                            item.Status += "分數格式不正確! ";
                        }        
                        item.ScoreNote = workSheet.Cells[rowIterator, 5].Text;
                        item.AuditNote = workSheet.Cells[rowIterator, 6].Text;




                    }
                    catch (Exception ex)
                    {
                        item.ROW_NO = rowid.ToString();
                        item.PersonID = workSheet.Cells[rowIterator, 1].Text;
                        item.Record = workSheet.Cells[rowIterator, 2].Text;
                        item.BeforeTest = workSheet.Cells[rowIterator, 3].Text;
                        item.AfterTest = workSheet.Cells[rowIterator, 4].Text;
                        item.ScoreNote = workSheet.Cells[rowIterator, 5].Text;
                        item.AuditNote = workSheet.Cells[rowIterator, 6].Text;
                        item.Status = ex.Message.ToString();
                    }
                    finally
                    {
                        if (!string.IsNullOrEmpty(item.PersonID))
                        {
                            rowid += 1;
                            orderItem.Add(item);
                        }
                    }

                }
                foreach (OrderItem item in orderItem)
                {
                    foreach (GridViewRow gridRow in gv_EventD.Rows)
                    {
                        string PersonID = gridRow.Cells[2].Text;
                        if (item.PersonID == PersonID)
                        {
                            var CRecord = (DropDownList)gridRow.FindControl("ddl_ClassRecordItem");
                            var BGrade = (TextBox)gridRow.FindControl("txt_BGrade");
                            var AGrade = (TextBox)gridRow.FindControl("txt_AGrade");
                            var AuditNote = (TextBox)gridRow.FindControl("txt_AuditNote");
                            var Note = (TextBox)gridRow.FindControl("txt_Note");
                            CRecord.SelectedValue = item.Record;
                            BGrade.Text = item.BeforeTest;
                            AGrade.Text = item.AfterTest;
                            AuditNote.Text= item.AuditNote;
                            //Note.Text = item.ScoreNote;
                        }
                    }
                }
            }
        }

    }

    public string GetCourseSNO(string PClassSNO)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string sql = "Select * from QS_Course where PClassSNO=@PClassSNO";
        aDict.Add("PClassSNO", PClassSNO);
        DataTable ObjDT = objDH.queryData(sql, aDict);
        return ObjDT.Rows[0]["CourseSNO"].ToString();
    }
    public string GetCourseSNOForEPClass(string EPClassSNO)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        aDict.Add("EPClassSNO",EPClassSNO);
        string sql = "Select PClassSNO from QS_ECoursePlanningClass where EPClassSNO=@EPClassSNO";
        DataTable ObjDT = objDH.queryData(sql, aDict);
        string PClassSNO= ObjDT.Rows[0]["PClassSNO"].ToString();
        string CourseSNO = GetCourseSNO(PClassSNO);
        return CourseSNO;
    }
    public class OrderItem
    {
        public string ROW_NO { set; get; }
        public string PersonID { set; get; }
        public string Record { set; get; }
        public string BeforeTest { set; get; }
        public string AfterTest { set; get; }
        public string ScoreNote { set; get; }
        public string AuditNote { get; set; }
        public string Status { set; get; }
        public string style { set; get; }
    }
}