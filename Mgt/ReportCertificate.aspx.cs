using DocumentFormat.OpenXml.Drawing.Charts;
using Ionic.Zip;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataTable = System.Data.DataTable;
using ListItem = System.Web.UI.WebControls.ListItem;
using Rectangle = iTextSharp.text.Rectangle;

public partial class Mgt_ReportCertificate : System.Web.UI.Page
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
            Utility.setRoleNormal(ddl_Role, "請選擇");
            SetDdlCtype(ddl_CType, "請選擇");
            Utility.setRoleNormal(ddl_NRole, "請選擇");
            SetDdlCtype(ddl_NCType, "請選擇");
            ddl_CertExt.Items.Insert(0, new System.Web.UI.WebControls.ListItem("請選擇", ""));
            ddl_CertExt.Items.Insert(1, new System.Web.UI.WebControls.ListItem("是", "1"));
            ddl_CertExt.Items.Insert(2, new System.Web.UI.WebControls.ListItem("否", "0"));
            ddl_NCertExt.Items.Insert(0, new System.Web.UI.WebControls.ListItem("請選擇", ""));
            ddl_NCertExt.Items.Insert(1, new System.Web.UI.WebControls.ListItem("是", "1"));
            ddl_NCertExt.Items.Insert(2, new System.Web.UI.WebControls.ListItem("否", "0"));
            CBL_Bind();
        }
    }

    /// <summary>
    /// 設定此報表可匯出名稱設定 ,  (注意 : BindData 欄位要包含設定可匯出資料)
    /// </summary>
    private void ReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("PName", "學員名稱");
        _SetCol.Add("PersonID", "身分證");
        _SetCol.Add("CertID", "證號");
        _SetCol.Add("CTypeName", "證書類型");
        _SetCol.Add("CUnitName", "發證單位");
        _SetCol.Add("CertPublicDate", "首發日期");
        _SetCol.Add("CertStartDate", "公告日期");
        _SetCol.Add("CertEndDate", "到期日期");
        _SetCol.Add("CertExt", "是否展延過");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.ReportCertificate.ToString()] = _ExcelInfo;
    }

    protected void bindData(int page, string PersonID = "")
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        String sql = @"
           SELECT 
                ROW_NUMBER() OVER (ORDER BY Qc.CertSNO) as ROW_NO,Qc.PersonID,
	           QC.CertID
				, ct.CTypeName , cu.CUnitName , P.PName 
	            , CONVERT(VARCHAR(10), Qc.CertPublicDate) CertPublicDate
	            , CONVERT(VARCHAR(10), Qc.CertStartDate ) CertStartDate
	            , CONVERT(VARCHAR(10), Qc.CertEndDate) CertEndDate
	            , CASE WHEN Qc.CertExt ='1' THEN '是' ELSE '否' END CertExt	
                ,STUFF ( P.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption'
            FROM QS_Certificate Qc
                LEFT JOIN QS_CertificateType ct ON ct.CTypeSNO = Qc.CTypeSNO
                LEFT JOIN QS_CertificateUnit cu ON cu.CUnitSNO = Qc.CUnitSNO
                LEFT JOIN Person P ON P.PersonID = Qc.PersonID
                LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
                LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
            WHERE 1=1 
          
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        #region 權限篩選區塊        
        sql += Utility.setSQLAccess_ByCertificate(wDict, userInfo);
        #endregion
        if (PersonID != "")
        {
            sql += " and Qc.PersonID In (" + PersonID + ")";
        }
        
        if (chk_his.Checked == true)
        {
            sql += " and Qc.IsChange = 0  ";
        }

        
        if (!String.IsNullOrEmpty(ddl_Role.SelectedValue))
        {
            sql += " AND P.RoleSNO = @RoleSNO ";
            wDict.Add("RoleSNO", ddl_Role.SelectedValue);
        }
        #region 查詢篩選區塊

        if (!String.IsNullOrEmpty(ddl_CType.SelectedValue))
        {
            sql += " AND Qc.CTypeSNO = @CTypeSNO ";
            wDict.Add("CTypeSNO", ddl_CType.SelectedValue);
        }
        if (!string.IsNullOrEmpty(txt_PName.Text))
        {
            sql += " AND P.PName Like '%' + @PName + '%' ";
            wDict.Add("PName", txt_PName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " AND Qc.PersonID = @PersonID  ";
            wDict.Add("PersonID", txt_PersonID.Text.Trim());
        }
        if (!String.IsNullOrEmpty(ddl_CertExt.SelectedValue))
        {
            sql += " AND Qc.CertExt = @CertExt ";
            wDict.Add("CertExt", ddl_CertExt.SelectedValue);
        }
        //首發日期
        if (!string.IsNullOrEmpty(txt_SPublicDate.Text))
        {
            sql += " AND Qc.CertPublicDate >= @SCertPublicDate";
            wDict.Add("SCertPublicDate", txt_SPublicDate.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_EPublicDate.Text))
        {
            sql += " AND Qc.CertPublicDate <= @ECertPublicDate";
            wDict.Add("ECertPublicDate", txt_EPublicDate.Text.Trim());
        }

        //到期日期
        if (!string.IsNullOrEmpty(txt_SEndDate.Text))
        {
            sql += " AND Qc.CertEndDate >= @SCertEndDate";
            wDict.Add("SCertEndDate", txt_SEndDate.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_EEndDate.Text))
        {
            sql += " AND Qc.CertEndDate <= @ECertEndDate";
            wDict.Add("ECertEndDate", txt_EEndDate.Text.Trim());
        }

        if (chk_Certificates.Checked)
        {
            string CtypeSNO = GetHeckList(CBL_Certificates);       
            sql += " AND Qc.CtypeSNO IN ("+CtypeSNO+")";    
        }
        #endregion

        sql += " Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        if (objDT.Rows.Count == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "alert('查無資料。')", true);
            return;
        }
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Course.DataSource = objDT.DefaultView;
        gv_Course.DataBind();
        //設定匯出資料
        ReportInit(objDT);
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

    protected void NbindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        String sql = @"
          With NonAccount as (
            Select P.PersonSNO,QC.* from QS_Certificate QC
            full Join Person P On QC.PersonID=P.PersonID
            where P.PersonSNO is null)
            Select ROW_NUMBER() OVER (ORDER BY NA.CertSNO) as ROW_NO, NA.PersonID,QCT.CTypeName,QCU.CUnitName,CertID,MP.PName
            , CONVERT(VARCHAR(10), NA.CertPublicDate) CertPublicDate
            , CONVERT(VARCHAR(10), NA.CertStartDate ) CertStartDate
            , CONVERT(VARCHAR(10), NA.CertEndDate) CertEndDate
             ,STUFF ( NA.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption',QCT.RoleSNO
            from NonAccount NA
            Left Join QS_CertificateType QCT On QCT.CTypeSNO=NA.CTypeSNO
            Left Join QS_CertificateUnit QCU ON QCU.CUnitSNO=NA.CUnitSNO
            Left Join PersonMP MP On MP.PersonID=NA.PersonID
            where 1=1  
          
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        if (chk_his.Checked == true)
        {
            sql += " and NA.IsChange = 0";
        }

        #region 查詢篩選區塊
        if (!String.IsNullOrEmpty(ddl_NRole.SelectedValue))
        {
            sql += " AND QCT.RoleSNO = @RoleSNO ";
            wDict.Add("RoleSNO", ddl_NRole.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_NCType.SelectedValue))
        {
            sql += " AND NA.CTypeSNO = @CTypeSNO ";
            wDict.Add("CTypeSNO", ddl_NCType.SelectedValue);
        }
        if (!string.IsNullOrEmpty(txt_NPName.Text))
        {
            sql += " AND MP.PName Like '%' + @PName + '%' ";
            wDict.Add("PName", txt_NPName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_NPersonID.Text))
        {
            sql += " AND NA.PersonID = @PersonID  ";
            wDict.Add("PersonID", txt_NPersonID.Text.Trim());
        }
        if (!String.IsNullOrEmpty(ddl_NCertExt.SelectedValue))
        {
            sql += " AND NA.CertExt = @CertExt ";
            wDict.Add("CertExt", ddl_NCertExt.SelectedValue);
        }
        //首發日期
        if (!string.IsNullOrEmpty(txt_NSPublicDate.Text))
        {
            sql += " AND NA.CertPublicDate >= @SCertPublicDate";
            wDict.Add("SCertPublicDate", txt_NSPublicDate.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_NEPublicDate.Text))
        {
            sql += " AND NA.CertPublicDate <= @ECertPublicDate";
            wDict.Add("ECertPublicDate", txt_NEPublicDate.Text.Trim());
        }

        //到期日期
        if (!string.IsNullOrEmpty(txt_NSEndDate.Text))
        {
            sql += " AND NA.CertEndDate >= @SCertEndDate";
            wDict.Add("SCertEndDate", txt_NSEndDate.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_NEEndDate.Text))
        {
            sql += " AND NA.CertEndDate <= @ECertEndDate";
            wDict.Add("ECertEndDate", txt_NEEndDate.Text.Trim());
        }

        #endregion
        if (String.IsNullOrEmpty(ddl_NRole.SelectedValue) && String.IsNullOrEmpty(ddl_NCType.SelectedValue))
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "alert('請選擇角色或是證書類型其中一項查詢!')", true);

            return;
        }

        sql += " Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_NCourse.DataSource = objDT.DefaultView;
        gv_NCourse.DataBind();
        //設定匯出資料
        ReportInit(objDT);
        ltl_NPageNumber.Text = Utility.showPageNumber1(objDT.Rows.Count, page, pageRecord);
    }

    private void SetDdlCtype(DropDownList ddl, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT A.CTypeSNO , A.CTypeName FROM QS_CertificateType A", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
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

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (gv_Course.Rows.Count == 0)
        {
            Response.Write("<script>alert('尚未查詢')</script>");
            return;
        }
        Utility.OpenExportWindows(this, ReportEnum.ReportCertificate.ToString());
    }    

    protected void btnECertificate_Click(object sender, EventArgs e)
    {
        string errorMessage = "";
        DataHelper objDH = new DataHelper();

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
                        item.PersonID = workSheet.Cells[rowIterator, 1].Text;

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
                    if (item.PersonID == "") continue;
                    Dictionary<string, object> dicpd = new Dictionary<string, object>();
                    string PersonID_item = Convert.ToString(item.PersonID);
                    dicpd.Add("PersonID", PersonID_item);
                    string sql = @"
                                 SELECT QC.CertSNO,
                                     P.RoleSNO,
                                     P.PName,
                                     case when PSex=0 then '女' ELSE '男' End  Psex,
                                     QC.PersonID,
                                     QC.CtypeSNO,
                                     QC.CertID,
                                     CT.CTypeName,
                                     CU.CUnitName,
                                     Cast(QC.CertPublicDate As varchar(10)) CertPublicDate,
                                     Cast(QC.CertStartDate As varchar(10)) CertStartDate,
                                     Cast(QC.CertEndDate As varchar(10)) CertEndDate,
                                     (Case QC.CertExt When 1 Then '有' Else '無' End) CertExt,
                                     QLU.ExamDate
                                 FROM QS_Certificate QC
                                     Left JOIN QS_CertificateType CT on CT.CTypeSNO=QC.CTypeSNO
                                     Left JOIN QS_CertificateUnit CU on CU.CUnitSNO=QC.CUnitSNO
                                     Left Join Person P ON P.PersonID=C.PersonID
                                     Left Join [QS_LearningUpload] QLU On QLU.PersonSNO=P.PersonSNO
                                 WHERE 1=1
                                     ";
                    #region 權限篩選區塊        
                    Utility.setSQLAccess_ByCertificate(dicpd, userInfo);
                    #endregion
                    DataTable objDT = objDH.queryData(sql, dicpd);
                    string CtypeSNO = objDT.Rows[0]["CtypeSNO"].ToString();
                    string RoleSNO = objDT.Rows[0]["RoleSNO"].ToString();
                    if (CtypeSNO == "51" || CtypeSNO == "52" || CtypeSNO == "53" || CtypeSNO == "54" || CtypeSNO == "55")
                    {

                        string PName = objDT.Rows[0]["PName"].ToString();
                        string PersonID = objDT.Rows[0]["PersonID"].ToString();
                        string CertID = objDT.Rows[0]["CertID"].ToString();
                        DateTime CertPublicDate = Convert.ToDateTime(objDT.Rows[0]["CertPublicDate"]);
                        DateTime CertStartDate = Convert.ToDateTime(objDT.Rows[0]["CertStartDate"]).AddYears(-1911);
                        DateTime CertEndDate = Convert.ToDateTime(objDT.Rows[0]["CertEndDate"]).AddYears(-1911);
                        string StartDateY = CertStartDate.Year.ToString(); string StartDateM = CertStartDate.Month.ToString(); ; string StartDateD = CertStartDate.Day.ToString();
                        string EndDateY = CertEndDate.Year.ToString(); string EndDateM = CertEndDate.Month.ToString(); ; string EndDateD = CertEndDate.Day.ToString();
                        string PSex = objDT.Rows[0]["PSex"].ToString();
                        string DatetimeY = DateTime.Now.AddYears(-1911).Year.ToString(); string DatetimeM = DateTime.Now.Month.ToString(); string DatetimeD = DateTime.Now.Day.ToString();

                        if (CertID.Length == 6)
                        {

                            string imageFilePath = "";
                            string imageFilePath_1 = "";
                            string imageFilePath_2 = "";
                            MemoryStream ms = new MemoryStream();
                            Document document = new Document(PageSize.A4, 50, 50, 50, 50);
                            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Context.Server.MapPath("../temppdf/") + CertID.ToString() + "-" + PName + ".pdf", FileMode.CreateNew));
                            document.Open();
                            document.NewPage();
                            BaseFont bfChinese = BaseFont.CreateFont(@"C:\Windows\Fonts\msjh.ttc,0", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                            BaseFont times = BaseFont.CreateFont(@"C:\Windows\Fonts\times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                            iTextSharp.text.Font ChFont = new iTextSharp.text.Font(times, 18);
                            iTextSharp.text.Font ChFont_Pname = new iTextSharp.text.Font(bfChinese, 18);
                            iTextSharp.text.Font ChFont_Role11Pname = new iTextSharp.text.Font(bfChinese, 24);
                            iTextSharp.text.Font ChFont_1 = new iTextSharp.text.Font(bfChinese, 22);
                            iTextSharp.text.Font ChFont_2 = new iTextSharp.text.Font(bfChinese, 18);
                            iTextSharp.text.Font ChFont_CertID = new iTextSharp.text.Font(times, 14);
                            iTextSharp.text.Font ChFont_blue = new iTextSharp.text.Font(bfChinese, 22, iTextSharp.text.Font.NORMAL, new BaseColor(51, 0, 153));
                            iTextSharp.text.Font ChFont_msg = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.NORMAL, BaseColor.RED);
                            switch (RoleSNO)
                            {
                                case "10":
                                    if (CtypeSNO == "53")
                                    {
                                        imageFilePath = Server.MapPath("../Images/Role10Certificate.png");
                                    }
                                    if (CtypeSNO == "51")
                                    {
                                        imageFilePath = Server.MapPath("../Images/Role13Certificate.jpg");
                                    }
                                    if (CtypeSNO == "55")
                                    {
                                        imageFilePath = Server.MapPath("../Images/Role11Certificate_2.jpg");
                                    }
                                    break;
                                case "11":
                                    if (CtypeSNO == "54")
                                    {
                                        imageFilePath = Server.MapPath("../Images/Role11Certificate_1.jpg");
                                    }
                                    if (CtypeSNO == "55")
                                    {
                                        imageFilePath = Server.MapPath("../Images/Role11Certificate_2.jpg");
                                    }
                                    break;
                                case "12":
                                    imageFilePath = Server.MapPath("../Images/Role12Certificate.jpg");
                                    break;
                                case "13":
                                    imageFilePath = Server.MapPath("../Images/Role13Certificate.jpg");
                                    break;
                            }

                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);


                            //jpg.ScaleToFit(100%);
                            jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
                            jpg.SetAbsolutePosition(0, 0);
                            jpg.ScaleAbsolute(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height);
                            document.Add(jpg);
                            switch (RoleSNO)
                            {
                                case "10":
                                    if (CtypeSNO == "53")
                                    {
                                        imageFilePath_1 = Server.MapPath("../Images/002.jpg");
                                        imageFilePath_2 = Server.MapPath("../Images/555.jpg");
                                        break;
                                    }
                                    if (CtypeSNO == "51")
                                    {
                                        imageFilePath_1 = Server.MapPath("../Images/001.png");
                                        imageFilePath_2 = Server.MapPath("../Images/老師印章.png");
                                        break;
                                    }
                                    if (CtypeSNO == "55")
                                    {
                                        imageFilePath = Server.MapPath("../Images/Role11Certificate_2.jpg");
                                        imageFilePath_1 = Server.MapPath("../Images/001.png");
                                        imageFilePath_2 = Server.MapPath("../Images/老師印章11.png");
                                        break;
                                    }
                                    break;
                                case "11":
                                    imageFilePath_1 = Server.MapPath("../Images/001.png");
                                    imageFilePath_2 = Server.MapPath("../Images/老師印章11.png");
                                    break;
                                case "12":
                                    imageFilePath_1 = Server.MapPath("../Images/005.png");
                                    imageFilePath_2 = Server.MapPath("../Images/004.png");

                                    break;
                                case "13":
                                    imageFilePath_1 = Server.MapPath("../Images/001.png");
                                    imageFilePath_2 = Server.MapPath("../Images/老師印章.png");
                                    break;
                            }

                            iTextSharp.text.Image JPG1 = iTextSharp.text.Image.GetInstance(imageFilePath_1);
                            JPG1.ScalePercent(60f);
                            JPG1.SetAbsolutePosition(80, 120);
                            document.Add(JPG1);
                            if (imageFilePath_2 != "")
                            {
                                iTextSharp.text.Image JPG2 = iTextSharp.text.Image.GetInstance(imageFilePath_2);
                                JPG2.ScalePercent(10f);
                                if (RoleSNO == "10")
                                {
                                    if (CtypeSNO == "53")
                                    {
                                        JPG2.SetAbsolutePosition(270, 140);
                                        document.Add(JPG2);
                                    }
                                    else
                                    {
                                        JPG2.SetAbsolutePosition(265, 150);
                                        document.Add(JPG2);
                                    }
                                }
                                else if (RoleSNO == "12")
                                {
                                    JPG2.SetAbsolutePosition(265, 220);
                                    document.Add(JPG2);
                                }
                                else
                                {
                                    JPG2.SetAbsolutePosition(265, 150);
                                    document.Add(JPG2);
                                }

                            }


                            switch (RoleSNO)
                            {
                                case "10":
                                    Chunk c_10_PName = new Chunk(PName, ChFont_Pname);
                                    Phrase p_10_PName = new Phrase(c_10_PName);
                                    Paragraph pg_10_PName = new Paragraph(p_10_PName);
                                    pg_10_PName.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct_10_PName = new ColumnText(writer.DirectContent);
                                    ct_10_PName.SetSimpleColumn(new Rectangle(300, 574));
                                    ct_10_PName.AddElement(pg_10_PName);
                                    ct_10_PName.Go();

                                    Chunk c1_10_PersonID = new Chunk(PersonID, ChFont_Pname);
                                    Phrase p1_10_PersonID = new Phrase(c1_10_PersonID);
                                    Paragraph pg1_10_PersonID = new Paragraph(p1_10_PersonID);
                                    pg1_10_PersonID.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct1_10_PersonID = new ColumnText(writer.DirectContent);
                                    ct1_10_PersonID.SetSimpleColumn(new Rectangle(880, 574));
                                    ct1_10_PersonID.AddElement(pg1_10_PersonID);
                                    ct1_10_PersonID.Go();
                                    Chunk C_10_CertID = new Chunk(CertID, ChFont_CertID);
                                    Phrase p_10_CertID = new Phrase(C_10_CertID);
                                    Paragraph pg_10_CertID = new Paragraph(p_10_CertID);
                                    pg_10_CertID.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct2_10_CertID = new ColumnText(writer.DirectContent);
                                    ct2_10_CertID.SetSimpleColumn(new Rectangle(940, 615));
                                    ct2_10_CertID.AddElement(pg_10_CertID);
                                    ct2_10_CertID.Go();

                                    Chunk c2_10_PersonID = new Chunk(PSex, ChFont_Pname);
                                    Phrase p2_10_PersonID = new Phrase(c2_10_PersonID);
                                    Paragraph pg2_10_PersonID = new Paragraph(p2_10_PersonID);
                                    pg2_10_PersonID.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct3_10_PersonID = new ColumnText(writer.DirectContent);
                                    ct3_10_PersonID.SetSimpleColumn(new Rectangle(650, 574));
                                    ct3_10_PersonID.AddElement(pg2_10_PersonID);
                                    ct3_10_PersonID.Go();
                                    break;
                                case "11":
                                    Chunk c_11_PName = new Chunk(PName, ChFont_Role11Pname);
                                    Phrase p_11_PName = new Phrase(c_11_PName);
                                    Paragraph pg_11_PName = new Paragraph(p_11_PName);
                                    pg_11_PName.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct_11_PName = new ColumnText(writer.DirectContent);
                                    ct_11_PName.SetSimpleColumn(new Rectangle(360, 580));
                                    ct_11_PName.AddElement(pg_11_PName);
                                    ct_11_PName.Go();

                                    Chunk c1_11_PersonID = new Chunk(PersonID, ChFont_Pname);
                                    Phrase p1_11_PersonID = new Phrase(c1_11_PersonID);
                                    Paragraph pg1_11_PersonID = new Paragraph(p1_11_PersonID);
                                    pg1_11_PersonID.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct1_11_PersonID = new ColumnText(writer.DirectContent);
                                    ct1_11_PersonID.SetSimpleColumn(new Rectangle(880, 572));
                                    ct1_11_PersonID.AddElement(pg1_11_PersonID);
                                    ct1_11_PersonID.Go();

                                    Chunk C_11_CertID = new Chunk(CertID, ChFont_CertID);
                                    Phrase p_11_CertID = new Phrase(C_11_CertID);
                                    Paragraph pg_11_CertID = new Paragraph(p_11_CertID);
                                    pg_11_CertID.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct2_11_CertID = new ColumnText(writer.DirectContent);
                                    ct2_11_CertID.SetSimpleColumn(new Rectangle(940, 611));
                                    ct2_11_CertID.AddElement(pg_11_CertID);
                                    ct2_11_CertID.Go();

                                    Chunk c2_11_PSex = new Chunk(PSex, ChFont_Pname);
                                    Phrase p2_11_PSex = new Phrase(c2_11_PSex);
                                    Paragraph pg2_11_PSex = new Paragraph(p2_11_PSex);
                                    pg2_11_PSex.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct3_11_PSex = new ColumnText(writer.DirectContent);
                                    ct3_11_PSex.SetSimpleColumn(new Rectangle(650, 572));
                                    ct3_11_PSex.AddElement(pg2_11_PSex);
                                    ct3_11_PSex.Go();
                                    break;
                                case "12":
                                    Chunk c_12_PName = new Chunk(PName, ChFont_Pname);
                                    Phrase p_12_PName = new Phrase(c_12_PName);
                                    Paragraph pg_12_PName = new Paragraph(p_12_PName);
                                    pg_12_PName.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct_12_PName = new ColumnText(writer.DirectContent);
                                    ct_12_PName.SetSimpleColumn(new Rectangle(300, 572));
                                    ct_12_PName.AddElement(pg_12_PName);
                                    ct_12_PName.Go();

                                    Chunk c1_12_PersonID = new Chunk(PersonID, ChFont_Pname);
                                    Phrase p1_12_PersonID = new Phrase(c1_12_PersonID);
                                    Paragraph pg1_12_PersonID = new Paragraph(p1_12_PersonID);
                                    pg1_12_PersonID.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct1_12_PersonID = new ColumnText(writer.DirectContent);
                                    ct1_12_PersonID.SetSimpleColumn(new Rectangle(880, 572));
                                    ct1_12_PersonID.AddElement(pg1_12_PersonID);
                                    ct1_12_PersonID.Go();

                                    Chunk C_12_CertID = new Chunk(CertID, ChFont_CertID);
                                    Phrase p_12_CertID = new Phrase(C_12_CertID);
                                    Paragraph pg_12_CertID = new Paragraph(p_12_CertID);
                                    pg_12_CertID.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct2_12_CertID = new ColumnText(writer.DirectContent);
                                    ct2_12_CertID.SetSimpleColumn(new Rectangle(940, 613));
                                    ct2_12_CertID.AddElement(pg_12_CertID);
                                    ct2_12_CertID.Go();

                                    Chunk c2_12_PSex = new Chunk(PSex, ChFont_Pname);
                                    Phrase p2_12_PSex = new Phrase(c2_12_PSex);
                                    Paragraph pg2_12_PSex = new Paragraph(p2_12_PSex);
                                    pg2_12_PSex.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct3_12_PSex = new ColumnText(writer.DirectContent);
                                    ct3_12_PSex.SetSimpleColumn(new Rectangle(630, 572));
                                    ct3_12_PSex.AddElement(pg2_12_PSex);
                                    ct3_12_PSex.Go();
                                    break;
                                case "13":
                                    Chunk c_13_PName = new Chunk(PName, ChFont_Pname);
                                    Phrase p_13_PName = new Phrase(c_13_PName);
                                    Paragraph pg_13_PName = new Paragraph(p_13_PName);
                                    pg_13_PName.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct_13_PName = new ColumnText(writer.DirectContent);
                                    ct_13_PName.SetSimpleColumn(new Rectangle(300, 572));
                                    ct_13_PName.AddElement(pg_13_PName);
                                    ct_13_PName.Go();

                                    Chunk c1 = new Chunk(PersonID, ChFont_Pname);
                                    Phrase p1 = new Phrase(c1);
                                    Paragraph pg1 = new Paragraph(p1);
                                    pg1.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct1 = new ColumnText(writer.DirectContent);
                                    ct1.SetSimpleColumn(new Rectangle(880, 572));
                                    ct1.AddElement(pg1);
                                    ct1.Go();

                                    Chunk C_CertID = new Chunk(CertID, ChFont_CertID);
                                    Phrase pCertID = new Phrase(C_CertID);
                                    Paragraph pgCertID = new Paragraph(pCertID);
                                    pgCertID.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct2 = new ColumnText(writer.DirectContent);
                                    ct2.SetSimpleColumn(new Rectangle(940, 613));
                                    ct2.AddElement(pgCertID);
                                    ct2.Go();

                                    Chunk c2 = new Chunk(PSex, ChFont_Pname);
                                    Phrase p2 = new Phrase(c2);
                                    Paragraph pg2 = new Paragraph(p2);
                                    pg2.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct3 = new ColumnText(writer.DirectContent);
                                    ct3.SetSimpleColumn(new Rectangle(630, 572));
                                    ct3.AddElement(pg2);
                                    ct3.Go();
                                    break;
                            }


                            if (RoleSNO == "10")
                            {
                                Chunk c3 = new Chunk(StartDateY, ChFont);
                                Phrase p3 = new Phrase(c3);
                                Paragraph pg3 = new Paragraph(p3);
                                pg3.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct4 = new ColumnText(writer.DirectContent);
                                ct4.SetSimpleColumn(new Rectangle(650, 415));
                                ct4.AddElement(pg3);
                                ct4.Go();

                                Chunk c4 = new Chunk(StartDateM, ChFont);
                                Phrase p4 = new Phrase(c4);
                                Paragraph pg4 = new Paragraph(p4);
                                pg4.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct5 = new ColumnText(writer.DirectContent);
                                ct5.SetSimpleColumn(new Rectangle(780, 415));
                                ct5.AddElement(pg4);
                                ct5.Go();

                                Chunk c5 = new Chunk(StartDateD, ChFont);
                                Phrase p5 = new Phrase(c5);
                                Paragraph pg5 = new Paragraph(p5);
                                pg5.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct6 = new ColumnText(writer.DirectContent);
                                ct6.SetSimpleColumn(new Rectangle(890, 415));
                                ct6.AddElement(pg5);
                                ct6.Go();

                                Chunk c6 = new Chunk(EndDateY, ChFont);
                                Phrase p6 = new Phrase(c6);
                                Paragraph pg6 = new Paragraph(p6);
                                pg6.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct7 = new ColumnText(writer.DirectContent);
                                ct7.SetSimpleColumn(new Rectangle(650, 375));
                                ct7.AddElement(pg6);
                                ct7.Go();

                                Chunk c7 = new Chunk(EndDateM, ChFont);
                                Phrase p7 = new Phrase(c7);
                                Paragraph pg7 = new Paragraph(p7);
                                pg7.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct8 = new ColumnText(writer.DirectContent);
                                ct8.SetSimpleColumn(new Rectangle(780, 375));
                                ct8.AddElement(pg7);
                                ct8.Go();

                                Chunk c8 = new Chunk(EndDateD, ChFont);
                                Phrase p8 = new Phrase(c8);
                                Paragraph pg8 = new Paragraph(p8);
                                pg8.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct9 = new ColumnText(writer.DirectContent);
                                ct9.SetSimpleColumn(new Rectangle(890, 375));
                                ct9.AddElement(pg8);
                                ct9.Go();

                            }
                            else
                            {
                                Chunk c3 = new Chunk(StartDateY, ChFont);
                                Phrase p3 = new Phrase(c3);
                                Paragraph pg3 = new Paragraph(p3);
                                pg3.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct4 = new ColumnText(writer.DirectContent);
                                ct4.SetSimpleColumn(new Rectangle(650, 408));
                                ct4.AddElement(pg3);
                                ct4.Go();

                                Chunk c4 = new Chunk(StartDateM, ChFont);
                                Phrase p4 = new Phrase(c4);
                                Paragraph pg4 = new Paragraph(p4);
                                pg4.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct5 = new ColumnText(writer.DirectContent);
                                ct5.SetSimpleColumn(new Rectangle(780, 408));
                                ct5.AddElement(pg4);
                                ct5.Go();

                                Chunk c5 = new Chunk(StartDateD, ChFont);
                                Phrase p5 = new Phrase(c5);
                                Paragraph pg5 = new Paragraph(p5);
                                pg5.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct6 = new ColumnText(writer.DirectContent);
                                ct6.SetSimpleColumn(new Rectangle(890, 408));
                                ct6.AddElement(pg5);
                                ct6.Go();

                                Chunk c6 = new Chunk(EndDateY, ChFont);
                                Phrase p6 = new Phrase(c6);
                                Paragraph pg6 = new Paragraph(p6);
                                pg6.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct7 = new ColumnText(writer.DirectContent);
                                ct7.SetSimpleColumn(new Rectangle(650, 363));
                                ct7.AddElement(pg6);
                                ct7.Go();

                                Chunk c7 = new Chunk(EndDateM, ChFont);
                                Phrase p7 = new Phrase(c7);
                                Paragraph pg7 = new Paragraph(p7);
                                pg7.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct8 = new ColumnText(writer.DirectContent);
                                ct8.SetSimpleColumn(new Rectangle(780, 363));
                                ct8.AddElement(pg7);
                                ct8.Go();

                                Chunk c8 = new Chunk(EndDateD, ChFont);
                                Phrase p8 = new Phrase(c8);
                                Paragraph pg8 = new Paragraph(p8);
                                pg8.Alignment = Element.ALIGN_CENTER;
                                ColumnText ct9 = new ColumnText(writer.DirectContent);
                                ct9.SetSimpleColumn(new Rectangle(890, 363));
                                ct9.AddElement(pg8);
                                ct9.Go();

                            }



                            switch (RoleSNO)
                            {
                                case "10":
                                    Chunk c9_10_Year = new Chunk(StartDateY, ChFont);
                                    Phrase p9_10_Year = new Phrase(c9_10_Year);
                                    Paragraph pg9_10_Year = new Paragraph(p9_10_Year);
                                    pg9_10_Year.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct10_10_Year = new ColumnText(writer.DirectContent);
                                    ct10_10_Year.SetSimpleColumn(new Rectangle(535, 109));
                                    ct10_10_Year.AddElement(pg9_10_Year);
                                    ct10_10_Year.Go();

                                    Chunk c10_10_Year = new Chunk(StartDateM, ChFont);
                                    Phrase p10_10_Year = new Phrase(c10_10_Year);
                                    Paragraph pg10_10_Year = new Paragraph(p10_10_Year);
                                    pg10_10_Year.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct11_10_Year = new ColumnText(writer.DirectContent);
                                    ct11_10_Year.SetSimpleColumn(new Rectangle(680, 109));
                                    ct11_10_Year.AddElement(pg10_10_Year);
                                    ct11_10_Year.Go();

                                    Chunk c11_10_Year = new Chunk(StartDateD, ChFont);
                                    Phrase p11_10_Year = new Phrase(c11_10_Year);
                                    Paragraph pg11_10_Year = new Paragraph(p11_10_Year);
                                    pg11_10_Year.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct12_10_Year = new ColumnText(writer.DirectContent);
                                    ct12_10_Year.SetSimpleColumn(new Rectangle(805, 109));
                                    ct12_10_Year.AddElement(pg11_10_Year);
                                    ct12_10_Year.Go();
                                    break;
                                case "11":
                                    Chunk c9_11_Year = new Chunk(StartDateY, ChFont);
                                    Phrase p9_11_Year = new Phrase(c9_11_Year);
                                    Paragraph pg9_11_Year = new Paragraph(p9_11_Year);
                                    pg9_11_Year.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct10_11_Year = new ColumnText(writer.DirectContent);
                                    ct10_11_Year.SetSimpleColumn(new Rectangle(535, 117));
                                    ct10_11_Year.AddElement(pg9_11_Year);
                                    ct10_11_Year.Go();

                                    Chunk c10_11_Year = new Chunk(StartDateM, ChFont);
                                    Phrase p10_11_Year = new Phrase(c10_11_Year);
                                    Paragraph pg10_11_Year = new Paragraph(p10_11_Year);
                                    pg10_11_Year.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct11_11_Year = new ColumnText(writer.DirectContent);
                                    ct11_11_Year.SetSimpleColumn(new Rectangle(680, 117));
                                    ct11_11_Year.AddElement(pg10_11_Year);
                                    ct11_11_Year.Go();

                                    Chunk c11_11_Year = new Chunk(StartDateD, ChFont);
                                    Phrase p11_11_Year = new Phrase(c11_11_Year);
                                    Paragraph pg11_11_Year = new Paragraph(p11_11_Year);
                                    pg11_11_Year.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct12_11_Year = new ColumnText(writer.DirectContent);
                                    ct12_11_Year.SetSimpleColumn(new Rectangle(805, 117));
                                    ct12_11_Year.AddElement(pg11_11_Year);
                                    ct12_11_Year.Go();
                                    break;
                                case "12":
                                    Chunk c9_12_Year = new Chunk(StartDateY, ChFont);
                                    Phrase p9_12_Year = new Phrase(c9_12_Year);
                                    Paragraph pg9_12_Year = new Paragraph(p9_12_Year);
                                    pg9_12_Year.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct10_12_Year = new ColumnText(writer.DirectContent);
                                    ct10_12_Year.SetSimpleColumn(new Rectangle(535, 125));
                                    ct10_12_Year.AddElement(pg9_12_Year);
                                    ct10_12_Year.Go();

                                    Chunk c10_12_Year = new Chunk(StartDateM, ChFont);
                                    Phrase p10_12_Year = new Phrase(c10_12_Year);
                                    Paragraph pg10_12_Year = new Paragraph(p10_12_Year);
                                    pg10_12_Year.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct11_12_Year = new ColumnText(writer.DirectContent);
                                    ct11_12_Year.SetSimpleColumn(new Rectangle(680, 125));
                                    ct11_12_Year.AddElement(pg10_12_Year);
                                    ct11_12_Year.Go();

                                    Chunk c11_12_Year = new Chunk(StartDateD, ChFont);
                                    Phrase p11_12_Year = new Phrase(c11_12_Year);
                                    Paragraph pg11_12_Year = new Paragraph(p11_12_Year);
                                    pg11_12_Year.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct12_12_Year = new ColumnText(writer.DirectContent);
                                    ct12_12_Year.SetSimpleColumn(new Rectangle(805, 125));
                                    ct12_12_Year.AddElement(pg11_12_Year);
                                    ct12_12_Year.Go();
                                    break;
                                case "13":
                                    Chunk c9_13_Year = new Chunk(StartDateY, ChFont);
                                    Phrase p9_13_Year = new Phrase(c9_13_Year);
                                    Paragraph pg9_13_Year = new Paragraph(p9_13_Year);
                                    pg9_13_Year.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct10_13_Year = new ColumnText(writer.DirectContent);
                                    ct10_13_Year.SetSimpleColumn(new Rectangle(535, 107));
                                    ct10_13_Year.AddElement(pg9_13_Year);
                                    ct10_13_Year.Go();

                                    Chunk c10_13_Year = new Chunk(StartDateM, ChFont);
                                    Phrase p10_13_Year = new Phrase(c10_13_Year);
                                    Paragraph pg10_13_Year = new Paragraph(p10_13_Year);
                                    pg10_13_Year.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct11_13_Year = new ColumnText(writer.DirectContent);
                                    ct11_13_Year.SetSimpleColumn(new Rectangle(680, 107));
                                    ct11_13_Year.AddElement(pg10_13_Year);
                                    ct11_13_Year.Go();

                                    Chunk c11_13_Year = new Chunk(StartDateD, ChFont);
                                    Phrase p11_13_Year = new Phrase(c11_13_Year);
                                    Paragraph pg11_13_Year = new Paragraph(p11_13_Year);
                                    pg11_13_Year.Alignment = Element.ALIGN_CENTER;
                                    ColumnText ct12_13_Year = new ColumnText(writer.DirectContent);
                                    ct12_13_Year.SetSimpleColumn(new Rectangle(805, 107));
                                    ct12_13_Year.AddElement(pg11_13_Year);
                                    ct12_13_Year.Go();
                                    break;
                            }


                            document.Close();


                        }
                        else
                        {
                            Response.Write("<script>alert('您的證書為舊證書字號，請先更新再下載。')</script>");
                        }

                    }
                    else
                    {
                        //Response.Write("<script>alert('尚未開放')</script>");
                    }

                }
            }

            Zip(Context.Server.MapPath("../temppdf/"), Context.Server.MapPath("../tempZip/Pdf.zip"));
            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename = z.zip");
            string filename = Server.MapPath("../tempZip/pdf.zip");
            Response.TransmitFile(filename);
        }
    }

    protected void chk_Open_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_Open.Checked == true)
        {
            p1.Visible = true;
        }
        else
        {
            p1.Visible = false;
        }

    }

    public void Zip(string path, string outputPath)
    {
        using (ZipFile zip = new ZipFile(Encoding.UTF8))
        {

            zip.Password = DateTime.Now.ToShortDateString();
            //...but this will be password protected
            zip.AddDirectory(path);
            zip.Save(outputPath);
        }
    }

    protected void btnNPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_NPage.Value, out page);
        NbindData(page);
    }

    protected void btnNSearch_Click(object sender, EventArgs e)
    {
        NbindData(1);
    }

    protected void btn_Upload_Click(object sender, EventArgs e)
    {
        string errorMessage = ""; string PersonID = "";
        string extension = Path.GetExtension(FU_load.FileName).ToLowerInvariant();
        List<string> allowedExtextsion = new List<string> { ".xlsx" };
        if (allowedExtextsion.IndexOf(extension) == -1) errorMessage += "請上傳 (xlsx) 類型檔案";
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        List<UploadItem> orderItem = new List<UploadItem>();
        string fileName = FU_load.FileName;
        using (var package = new ExcelPackage(FU_load.PostedFile.InputStream))
        {
            var currentSheet = package.Workbook.Worksheets;
            var workSheet = currentSheet.First();
            var noOfCol = workSheet.Dimension.End.Column;
            var noOfRow = workSheet.Dimension.End.Row;
            int rowid = 1;
            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
            {
                UploadItem item = new UploadItem();
                try
                {

                    item.PersonID = workSheet.Cells[rowIterator, 1].Text;
                }
                catch (Exception ex)
                {
                    item.PersonID = rowid.ToString();
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
            foreach (UploadItem item in orderItem)
            {
                if (item.PersonID == "") continue;
                PersonID += "'" + item.PersonID + "',";
            }
            PersonID = PersonID.Substring(0, PersonID.Length - 1);
            bindData(1, PersonID);
        }
    }

    protected void chk_ExportCTable_CheckedChanged(object sender, EventArgs e)
    {
       
    }

    protected void btn_ExportCTable_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT=null;
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = @"Select * from (
                        SELECT Case When CA.AREA_NAME  is null then '無縣市' Else CA.AREA_NAME End as '縣市',QCT.CTypeName,QC.CertSNO
                          FROM [QS_Certificate] QC
                          Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
                          Left Join Person P On P.PersonID=QC.PersonID
                          Left Join Organ O On P.OrganSNO=O.OrganSNO
                          Left Join CD_AREA CA On CA.AREA_CODE=O.AreaCodeA And CA.AREA_TYPE='A' 
                          Left Join Role R ON R.RoleSNO = P.RoleSNO where 1=1 ";

        #region 權限篩選區塊        
        sql+= Utility.setSQLAccess_ByCertificate(adict, userInfo);
        #endregion
        string sqlS = " and QC.CertStartDate > @txt_StartS";
        string sqlE = " and QC.CertStartDate < @txt_StartE ";
        string sqlPivot = @"          Group by CA.AREA_NAME,QCT.CTypeName,QC.CertSNO
                          ) t
                          PIVOT (
                        	Count(CertSNO) 
                        	FOR CTypeName IN 
                        	";
        switch (userInfo.RoleOrganType)
        {
            case "S":
                sqlPivot += @" ([證書-醫師],[證書-牙醫初階],[證書-牙醫進階],[證書-衛教師(106以前)],
                        	[證書-衛教師(107以後)],[證書-藥師(101-103)],[證書-藥師(104以後)],[服務資格證明書(衛教人員)],
                        	[服務資格證明書(藥事人員)],	[服務資格證明書(醫師)],[服務資格證明書(牙醫師-治療)],[服務資格證明書(牙醫師-衛教)],[初階證書],[進階證書])
                        ) p";

                break;

            case "U":
                switch (userInfo.RoleGroup)
                {
                    case "10":
                        sqlPivot += @" ([證書-醫師],	[服務資格證明書(醫師)])
                        ) p";
                        break;
                    case "11":
                        sqlPivot += @" ([證書-牙醫初階],[證書-牙醫進階],[服務資格證明書(牙醫師-治療)],[服務資格證明書(牙醫師-衛教)])
                        ) p";
                        break;
                    case "12":
                        sqlPivot += @" ([證書-藥師(101-103)],[證書-藥師(104以後)],[服務資格證明書(藥事人員)],[初階證書],[進階證書])
                        ) p";
                        break;
                    case "13":
                        sqlPivot += @" ([證書-衛教師(106以前)],[證書-衛教師(107以後)],[服務資格證明書(衛教人員)],[初階證書],[進階證書])
                        ) p";
                        break;
                }

                break;
            default:
                sql += " And 1=2";
                break;

        }
        if (!string.IsNullOrEmpty(txt_StartS.Text) && !string.IsNullOrEmpty(txt_StartE.Text))
        {
            adict.Add("txt_StartS", txt_StartS.Text);
            adict.Add("txt_StartE", txt_StartE.Text);
            string ExportSQL = sql +   sqlS +  sqlE + sqlPivot;
            objDT = objDH.queryData(ExportSQL, adict);
        }
        if (string.IsNullOrEmpty(txt_StartS.Text) && string.IsNullOrEmpty(txt_StartE.Text))
        {
            adict.Add("txt_StartS", txt_StartS.Text);
            adict.Add("txt_StartE", txt_StartE.Text);
            string ExportSQL = sql +   sqlPivot;
            objDT = objDH.queryData(ExportSQL, adict);
        }
        if (string.IsNullOrEmpty(txt_StartS.Text))
        {
            string ExportSQL = sql +   sqlS + sqlPivot;
            adict.Add("txt_StartS", txt_StartS.Text);
            objDT = objDH.queryData(ExportSQL, adict);
        }
        if (string.IsNullOrEmpty(txt_StartE.Text))
        {
            string ExportSQL = sql +   sqlE + sqlPivot;
            adict.Add("txt_StartE", txt_StartE.Text);
            objDT = objDH.queryData(ExportSQL, adict);
        }
        ReportInitGetExportCTable(objDT);
        Utility.OpenExportWindows(this, ReportEnum.ExportCTable.ToString());
        //byte[] file = null;
        //ExcelHelper.DatatableToExcelForWeb("各縣市各類醫事人員", objDT, ref file, false, "");

        //// 匯出 Excel --- 使用 Response 下載附件         
        //string filename = string.Format("{0}{1}.xlsx", "各縣市各類醫事人員培訓人數表", DateTime.Today.ToString("yyyyMMdd"));
        //Response.Clear();
        //Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}", filename));
        //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //Response.BinaryWrite(file);
        //Response.End();

    }

    public void CBL_Bind()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = @"Select CtypeSNO,CTypeName 
                    from QS_CertificateType QCT
                    Left Join Role R On R.RoleSNO=QCT.RoleSNO
                    ";
        if (userInfo.RoleGroup == "1" || userInfo.RoleGroup == "2")
        {

        }
        else
        {
            sql += " where R.RoleGroup=RoleGroup";
        }
        DataTable objDT = objDH.queryData(sql, adict);
        CBL_Certificates.DataSource = objDT.DefaultView;
        CBL_Certificates.DataBind();
    }

    public string GetHeckList(CheckBoxList ChkList)
    {
        string oValues = "";
        foreach (ListItem oItem in ChkList.Items)
        {
            if (oItem.Selected == true)
            {
                if (oValues.Length > 0)
                {
                    oValues += ",";
                }
                oValues += oItem.Value;
            }

        }

        return oValues;
    }

    protected void btn_ExportCTableODS_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = null;
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = @"Select * from (
                        SELECT Case When CA.AREA_NAME  is null then '無縣市' Else CA.AREA_NAME End as '縣市',QCT.CTypeName,QC.CertSNO
                          FROM [QS_Certificate] QC
                          Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
                          Left Join Person P On P.PersonID=QC.PersonID
                          Left Join Organ O On P.OrganSNO=O.OrganSNO
                          Left Join CD_AREA CA On CA.AREA_CODE=O.AreaCodeA And CA.AREA_TYPE='A' 
                          Left Join Role R ON R.RoleSNO = P.RoleSNO where 1=1 ";

        #region 權限篩選區塊        
        sql += Utility.setSQLAccess_ByCertificate(adict, userInfo);
        #endregion
        string sqlS = " and QC.CertStartDate > @txt_StartS";
        string sqlE = " and QC.CertStartDate < @txt_StartE ";
        string sqlPivot = @"          Group by CA.AREA_NAME,QCT.CTypeName,QC.CertSNO
                          ) t
                          PIVOT (
                        	Count(CertSNO) 
                        	FOR CTypeName IN 
                        	";
        switch (userInfo.RoleOrganType)
        {
            case "S":
                sqlPivot += @" ([證書-醫師],[證書-牙醫初階],[證書-牙醫進階],[證書-衛教師(106以前)],
                        	[證書-衛教師(107以後)],[證書-藥師(101-103)],[證書-藥師(104以後)],[服務資格證明書(衛教人員)],
                        	[服務資格證明書(藥事人員)],	[服務資格證明書(醫師)],[服務資格證明書(牙醫師-治療)],[服務資格證明書(牙醫師-衛教)])
                        ) p";

                break;

            case "U":
                switch (userInfo.RoleGroup)
                {
                    case "10":
                        sqlPivot += @" ([證書-醫師],	[服務資格證明書(醫師)])
                        ) p";
                        break;
                    case "11":
                        sqlPivot += @" ([證書-牙醫初階],[證書-牙醫進階],[服務資格證明書(牙醫師-治療)],[服務資格證明書(牙醫師-衛教)])
                        ) p";
                        break;
                    case "12":
                        sqlPivot += @" ([證書-藥師(101-103)],[證書-藥師(104以後)],[服務資格證明書(藥事人員)],[初階證書],[進階證書])
                        ) p";
                        break;
                    case "13":
                        sqlPivot += @" ([證書-衛教師(106以前)],[證書-衛教師(107以後)],[服務資格證明書(衛教人員)],[初階證書],[進階證書])
                        ) p";
                        break;
                }

                break;
            default:
                sql += " And 1=2";
                break;

        }
        if (!string.IsNullOrEmpty(txt_StartS.Text) && !string.IsNullOrEmpty(txt_StartE.Text))
        {
            adict.Add("txt_StartS", txt_StartS.Text);
            adict.Add("txt_StartE", txt_StartE.Text);
            string ExportSQL = sql +  sqlS + sqlE + sqlPivot;
            objDT = objDH.queryData(ExportSQL, adict);
        }
        if (string.IsNullOrEmpty(txt_StartS.Text) && string.IsNullOrEmpty(txt_StartE.Text))
        {
            adict.Add("txt_StartS", txt_StartS.Text);
            adict.Add("txt_StartE", txt_StartE.Text);
            string ExportSQL = sql +  sqlPivot;
            objDT = objDH.queryData(ExportSQL, adict);
        }
        if (string.IsNullOrEmpty(txt_StartS.Text))
        {
            string ExportSQL = sql +  sqlS + sqlPivot;
            adict.Add("txt_StartS", txt_StartS.Text);
            objDT = objDH.queryData(ExportSQL, adict);
        }
        if (string.IsNullOrEmpty(txt_StartE.Text))
        {
            string ExportSQL = sql +  sqlE + sqlPivot;
            adict.Add("txt_StartE", txt_StartE.Text);
            objDT = objDH.queryData(ExportSQL, adict);
        }
        ReportInitGetExportCTableODS(objDT);
        Utility.OpenExportWindows(this, ReportEnum.ExportCTableODS.ToString());
        //byte[] file = null;
        //ExcelHelper.DatatableToExcelForWeb("各縣市各類醫事人員", objDT, ref file, false, "");

        //// 匯出 Excel --- 使用 Response 下載附件         
        //string filename = string.Format("{0}{1}.ods", "各縣市各類醫事人員培訓人數表", DateTime.Today.ToString("yyyyMMdd"));

        //Response.Clear();
        //Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}", filename));
        //Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
        //Response.BinaryWrite(file);
        //Response.End();

    }

    protected void btn_Templte_Click(object sender, EventArgs e)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        String filePath = Directory.GetCurrentDirectory() + @"/SysFile/身份證查詢上傳格式.xlsx";
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

    public class OrderItem
    {
        public string PersonID { set; get; }

    }

    public class UploadItem
    {
        public string PersonID { get; set; }
        public string Status { get; set; }
    }

    protected void chk_Certificates_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_Certificates.Checked == true)
        {
            p2.Visible = true;
        }
        else
        {
            p2.Visible = false;
        }
    }

    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        //
    }

    private void ReportInitNewCertificate(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("PName", "姓名");
        _SetCol.Add("PersonID", "身分證");
        _SetCol.Add("RoleName", "身分別");
        _SetCol.Add("CertStartDate", "證書公開日");
        _SetCol.Add("CertPublicDate", "證書首發日");
        _SetCol.Add("CertEndDate", "證書到期日");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.NewCertificate.ToString()] = _ExcelInfo;
    }

    protected void btn_ExportGCTable_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        System.Data.DataTable objDT = null;
        Dictionary<string, object> adict = new Dictionary<string, object>();

        string sql = @"WITH RankedCertificates AS (
                            SELECT 
                                P.PName,
                                R.RoleName,
                                QC.PersonID,
                                CONVERT(varchar, QC.CertStartDate, 111) CertStartDate,
                                CONVERT(varchar, QC.CertPublicDate, 111) CertPublicDate,
                                CONVERT(varchar, QC.CertEndDate, 111) CertEndDate,                                
                                ROW_NUMBER() OVER (PARTITION BY P.PersonID ORDER BY QC.CreateDT DESC) as RowNum
                            FROM Person P
                            LEFT JOIN QS_Certificate QC ON QC.PersonID = P.PersonID
                            LEFT JOIN Role R On R.RoleSNO=P.RoleSNO
                            WHERE 
                                QC.CTypeSNO = 75 
                                AND QC.CertPublicDate BETWEEN '2023-02-20' AND '2025-02-20'
                        )
                        SELECT 
                            PName,
                            RoleName,
                            PersonID,
                            CertStartDate,
                            CertPublicDate,
                            CertEndDate
                        FROM RankedCertificates
                        WHERE RowNum = 1
                        ORDER BY CertPublicDate DESC ";
        adict.Add("CertPublicDateS", txt_GCStartS.Text);
        adict.Add("CertPublicDateE", txt_GCStartE.Text);
        objDT = objDH.queryData(sql, adict);
        ReportInitNewCertificate(objDT);
        Utility.OpenExportWindows(this, ReportEnum.NewCertificate.ToString());
    }

    protected void btn_ExportGCSearch_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = new DataTable();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("CertPublicDateS", txt_GCStartS.Text);
        adict.Add("CertPublicDateE", txt_GCStartE.Text);
        string sql = "Select Distinct PersonID from QS_Certificate where CTypeSNO=75 and CertPublicDate Between @CertPublicDateS and @CertPublicDateE";
        objDT = objDH.queryData(sql, adict);
        Lb_CerNum.Text = objDT.Rows.Count.ToString();
    }

    private void ReportInitGetExportCTable(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>(); 
        _SetCol.Add("縣市", "縣市");
        _SetCol.Add("證書-醫師", "證書-醫師");
        _SetCol.Add("證書-牙醫初階", "證書-牙醫初階");
        _SetCol.Add("證書-牙醫進階", "證書-牙醫進階");
        _SetCol.Add("證書-衛教師(106以前)", "證書-衛教師(106以前)");
        _SetCol.Add("證書-衛教師(107以後)", "證書-衛教師(107以後)");
        _SetCol.Add("證書-藥師(101-103)", "證書-藥師(101-103)");
        _SetCol.Add("證書-藥師(104以後)", "證書-藥師(104以後)");
        _SetCol.Add("服務資格證明書(衛教人員)", "服務資格證明書(衛教人員)");
        _SetCol.Add("服務資格證明書(藥事人員)", "服務資格證明書(藥事人員)");
        _SetCol.Add("服務資格證明書(醫師)", "服務資格證明書(醫師)");
        _SetCol.Add("服務資格證明書(牙醫師-治療)", "服務資格證明書(牙醫師-治療)");
        _SetCol.Add("服務資格證明書(牙醫師-衛教)", "服務資格證明書(牙醫師-衛教)");
        _SetCol.Add("初階證書", "初階證書");
        _SetCol.Add("進階證書", "進階證書");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.ExportCTable.ToString()] = _ExcelInfo;
    }

    private void ReportInitGetExportCTableODS(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("縣市", "縣市");
        _SetCol.Add("證書-醫師", "證書-醫師");
        _SetCol.Add("證書-牙醫初階", "證書-牙醫初階");
        _SetCol.Add("證書-牙醫進階", "證書-牙醫進階");
        _SetCol.Add("證書-衛教師(106以前)", "證書-衛教師(106以前)");
        _SetCol.Add("證書-衛教師(107以後)", "證書-衛教師(107以後)");
        _SetCol.Add("證書-藥師(101-103)", "證書-藥師(101-103)");
        _SetCol.Add("證書-藥師(104以後)", "證書-藥師(104以後)");
        _SetCol.Add("服務資格證明書(衛教人員)", "服務資格證明書(衛教人員)");
        _SetCol.Add("服務資格證明書(藥事人員)", "服務資格證明書(藥事人員)");
        _SetCol.Add("服務資格證明書(醫師)", "服務資格證明書(醫師)");
        _SetCol.Add("服務資格證明書(牙醫師-治療)", "服務資格證明書(牙醫師-治療)");
        _SetCol.Add("服務資格證明書(牙醫師-衛教)", "服務資格證明書(牙醫師-衛教)");
        _SetCol.Add("初階證書", "初階證書");
        _SetCol.Add("進階證書", "進階證書");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.ExportCTable.ToString()] = _ExcelInfo;
    }

    private void ReportInitGetQuictSmok(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("PName", "姓名");
        _SetCol.Add("PersonID", "身分證");
        _SetCol.Add("RoleName", "身分別");
        _SetCol.Add("CertStartDate", "證書公開日");
        _SetCol.Add("CertPublicDate", "證書首發日");
        _SetCol.Add("CertEndDate", "證書到期日");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.GetQuictSmok.ToString()] = _ExcelInfo;
    }

    protected void btn_ExportServiceTable_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = null;
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = @"WITH RankedCertificates AS (
                        SELECT 
                            P.PName,
                            P.PersonID,
                            CONVERT(varchar, QC.CertStartDate, 111) CertStartDate,
                            CONVERT(varchar, QC.CertPublicDate, 111) CertPublicDate,
                            CONVERT(varchar, QC.CertEndDate, 111) CertEndDate,                
                            R.RoleName,
                            QC.CTypeSNO,
                            CASE
                                WHEN QC.CTypeSNO = 75 THEN 1
                                WHEN QC.CTypeSNO IN (51, 52, 53, 54) THEN 2
                                WHEN QC.CTypeSNO IN (1, 2, 5, 7) THEN 3
                                WHEN QC.CTypeSNO IN (4, 6) THEN 4
                                ELSE 5
                            END AS PriorityRank,
                            ROW_NUMBER() OVER (
                                PARTITION BY P.PersonID 
                                ORDER BY 
                                    CASE
                                        WHEN QC.CTypeSNO = 75 THEN 1
                                        WHEN QC.CTypeSNO IN (51, 52, 53, 54) THEN 2
                                        WHEN QC.CTypeSNO IN (1, 2, 5, 7) THEN 3
                                        WHEN QC.CTypeSNO IN (4, 6) THEN 4
                                        ELSE 5
                                    END
                            ) AS RowNum
                        FROM Person P
                        LEFT JOIN QS_Certificate QC ON QC.PersonID = P.PersonID
                        LEFT JOIN ROLE R On R.RoleSNO=P.RoleSNO
                        WHERE QC.CTypeSNO IN (1, 2, 4, 5, 6, 7, 51, 52, 53, 54, 75) 
                        AND QC.CertEndDate >= GETDATE()
                        AND P.PName NOT LIKE N'%測試%'  -- 剔除名稱包含「測試」的帳號
                    )
                    SELECT 
                        PName,
                        PersonID,
                        RoleName,
                        CertStartDate,
                        CertPublicDate,
                        CertEndDate,
                        CTypeSNO
                    FROM RankedCertificates 
                    WHERE RowNum = 1 And CertEndDate >= @CertEndDate
                    ORDER BY PersonID";
        adict.Add("CertEndDate", txt_services.Text);
        objDT = objDH.queryData(sql, adict);
        ReportInitGetQuictSmok(objDT);
        Utility.OpenExportWindows(this, ReportEnum.GetQuictSmok.ToString());
        
    }

    protected void btn_ExportServiceSearch_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = null;
        Dictionary<string, object> adict = new Dictionary<string, object>();
       
        string sql = @"WITH RankedCertificates AS (
                        SELECT 
                            P.PName,
                            P.PersonID,
                            QC.CertStartDate,
                            QC.CertPublicDate,
                            QC.CertEndDate,
                            QC.CTypeSNO,
                            CASE
                                WHEN QC.CTypeSNO = 75 THEN 1
                                WHEN QC.CTypeSNO IN (51, 52, 53, 54) THEN 2
                                WHEN QC.CTypeSNO IN (1, 2, 5, 7) THEN 3
                                WHEN QC.CTypeSNO IN (4, 6) THEN 4
                                ELSE 5
                            END AS PriorityRank,
                            ROW_NUMBER() OVER (
                                PARTITION BY P.PersonID 
                                ORDER BY 
                                    CASE
                                        WHEN QC.CTypeSNO = 75 THEN 1
                                        WHEN QC.CTypeSNO IN (51, 52, 53, 54) THEN 2
                                        WHEN QC.CTypeSNO IN (1, 2, 5, 7) THEN 3
                                        WHEN QC.CTypeSNO IN (4, 6) THEN 4
                                        ELSE 5
                                    END
                            ) AS RowNum
                        FROM Person P
                        LEFT JOIN QS_Certificate QC ON QC.PersonID = P.PersonID
                        WHERE QC.CTypeSNO IN (1, 2, 4, 5, 6, 7, 51, 52, 53, 54, 75) 
                        AND QC.CertEndDate >= GETDATE()
                        AND P.PName NOT LIKE N'%測試%'  -- 剔除名稱包含「測試」的帳號
                    )
                    SELECT 
                        PName,
                        PersonID,
                        CertStartDate,
                        CertPublicDate,
                        CertEndDate,
                        CTypeSNO
                    FROM RankedCertificates
                    WHERE RowNum = 1 And CertEndDate >= @CertEndDate
                    ORDER BY PersonID";
        adict.Add("CertEndDate", txt_services.Text);
        objDT = objDH.queryData(sql, adict);
        lb_ServiceCount.Text = objDT.Rows.Count.ToString();
    }
}