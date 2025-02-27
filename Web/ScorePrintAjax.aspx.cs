using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_ScorePrintAjax : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {
        string PClassSNO = "";
        string PersonSNO = "";
        string EPClassSNO = "";
        string EPClassSNO_C = "";
        string PersonID_C = "";
        string CoreIsFull = "";
        if (Request.QueryString["PClassSNO"] != null)
        {
            PClassSNO = Request.QueryString["PClassSNO"].ToString();
        }

        if (Request.QueryString["PersonSNO"] != null)
        {
            PersonSNO = Request.QueryString["PersonSNO"].ToString();
            PersonID_C = Utility.ConvertPersonSNOToPersonID(PersonSNO);
        }
        if (Request.QueryString["EPClassSNO"] != null)
        {
            EPClassSNO = Request.QueryString["EPClassSNO"].ToString();//他其實是PClassSNO
            EPClassSNO_C = getEPClassSNO(EPClassSNO);//他才是EPClassSNO
        }
        if (Request.QueryString["Core"] != null)
        {
            CoreIsFull = Request.QueryString["Core"].ToString();
        }
        DataHelper ObjDH = new DataHelper();
        DataTable ObjDT = new DataTable();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = "Select * from Person where PersonSNO=@PersonSNO";
        adict.Add("PersonSNO", PersonSNO);
        ObjDT = ObjDH.queryData(sql, adict);
        string UserName = ObjDT.Rows[0]["PName"].ToString();
        string UserPersonID= ObjDT.Rows[0]["PersonID"].ToString();

        if (PClassSNO != null && PersonSNO!=null)
        {
            Document doc = new Document(PageSize.A4, 10, 10, 10, 30); // 設定PageSize, Margin, left, right, top, bottom
            MemoryStream ms = new MemoryStream();
            PdfWriter pw = PdfWriter.GetInstance(doc, ms);

            ////    字型設定
            // 在PDF檔案內容中要顯示中文，最重要的是字型設定，如果沒有正確設定中文字型，會造成中文無法顯示的問題。
            // 首先設定基本字型：kaiu.ttf 是作業系統系統提供的標楷體字型，IDENTITY_H 是指編碼(The Unicode encoding with horizontal writing)，及是否要將字型嵌入PDF 檔中。
            // 再來針對基本字型做變化，例如Font Size、粗體斜體以及顏色等。當然你也可以採用其他中文字體字型。
            BaseFont bfChinese = BaseFont.CreateFont("C:\\Windows\\Fonts\\kaiu.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font ChFont_Header = new Font(bfChinese, 25);
            Font ChFont_CoreCertificate = new Font(bfChinese, 22, Font.NORMAL);
            Font ChFontTitle = new Font(bfChinese, 18);
            Font ChFont = new Font(bfChinese, 12);
            Font ChFont_green = new Font(bfChinese, 40, Font.NORMAL, BaseColor.GREEN);
            Font ChFont_msg = new Font(bfChinese, 12, Font.ITALIC, BaseColor.RED);

            // 開啟檔案寫入內容後，將檔案關閉。
            doc.Open();

            // 產生表格 -- START
            // 建立4個欄位表格之相對寬度
            PdfPTable pt = new PdfPTable(new float[] { 2, 2, 2, 2 });
            // 表格總寬
            pt.TotalWidth = 500f;
            pt.LockedWidth = true;
            // 塞入資料 -- START
            // 設定表頭
            PdfPCell header = new PdfPCell(new Phrase("醫事人員服務訓練系統", ChFont_Header));
            header.Colspan = 4;
            header.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
            header.Padding = 5;
            header.Border = 0;
            header.PaddingBottom = 30;
            pt.AddCell(header);
            if (CoreIsFull == "2")
            {
                PdfPCell CoreCertificateaa = new PdfPCell(new Phrase("核心完訓證明", ChFont_CoreCertificate));
                CoreCertificateaa.Colspan = 4;
                CoreCertificateaa.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
                CoreCertificateaa.Padding = 5;
                CoreCertificateaa.Border = 0;
                pt.AddCell(CoreCertificateaa);
            }
            if (CoreIsFull == "1")
            {
                PdfPCell CoreCertificateaa = new PdfPCell(new Phrase("核心完訓證明(未領有證書)", ChFont_CoreCertificate));
                CoreCertificateaa.Colspan = 4;
                CoreCertificateaa.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
                CoreCertificateaa.Padding = 5;
                CoreCertificateaa.Border = 0;
                pt.AddCell(CoreCertificateaa);
            }
            PdfPCell PName = new PdfPCell(new Phrase("姓名："+UserName, ChFont));
            PName.BorderWidthBottom = 0;
            PName.BorderWidthRight = 0;
            PName.Colspan = 2;
            pt.AddCell(PName);
            PdfPCell PrintDate = new PdfPCell(new Phrase("列印日期：" + DateTime.Now.ToShortDateString(), ChFont));
            PrintDate.BorderWidthBottom = 0;
            PrintDate.BorderWidthLeft = 0;
            PrintDate.Colspan = 2;
            pt.AddCell(PrintDate);

            PdfPCell PersonID = new PdfPCell(new Phrase("身分證字號："+UserPersonID , ChFont));
            PersonID.Colspan = 4;
            PersonID.BorderWidthTop = 0;
            PersonID.PaddingBottom = 10;
            pt.AddCell(PersonID);
            //PdfPCell Certificate = new PdfPCell(new Phrase("訓練證書：" + userInfo.PersonID, ChFont));
            //Certificate.BorderWidthTop = 0;
            //Certificate.BorderWidthLeft = 0;
            //Certificate.Colspan = 2;
            //pt.AddCell(Certificate);

            PdfPCell Cerficate_Header = new PdfPCell(new Phrase("證書", ChFontTitle));
            Cerficate_Header.Colspan = 4;
            Cerficate_Header.HorizontalAlignment = Element.ALIGN_CENTER;// 內文靠右
            Cerficate_Header.Padding = 10;
            pt.AddCell(Cerficate_Header);

            //PdfPCell rows = new PdfPCell(new Phrase("ROW_4", ChFont_green));
            //rows.Rowspan = 3;
            //pt.AddCell(rows);
            DataTable Certificate = Utility.getCertificate(UserPersonID);
            if (Certificate != null)
            {
                PdfPCell CTypeNameTitle = new PdfPCell(new Phrase("證書名稱", ChFont));
                CTypeNameTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                pt.AddCell(CTypeNameTitle);
                PdfPCell CertPublicDateTitle = new PdfPCell(new Phrase("證書起始日", ChFont));
                CertPublicDateTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                pt.AddCell(CertPublicDateTitle);
                PdfPCell CertStartDatTitlee = new PdfPCell(new Phrase("證書公開日", ChFont));
                CertStartDatTitlee.HorizontalAlignment = Element.ALIGN_CENTER;
                pt.AddCell(CertStartDatTitlee);
                PdfPCell CertEndDateTitle = new PdfPCell(new Phrase("證書到期日", ChFont));
                CertEndDateTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                pt.AddCell(CertEndDateTitle);
                for (int i = 0; i < Certificate.Rows.Count; i++)
                {
                    PdfPCell CTypeName = new PdfPCell(new Phrase(Certificate.Rows[i]["CTypeName"].ToString(), ChFont));
                    CTypeName.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(CTypeName);
                    PdfPCell CertPublicDate = new PdfPCell(new Phrase(Convert.ToDateTime(Certificate.Rows[i]["CertPublicDate"].ToString()).ToShortDateString(), ChFont));
                    CertPublicDate.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(CertPublicDate);
                    PdfPCell CertStartDate = new PdfPCell(new Phrase(Convert.ToDateTime(Certificate.Rows[i]["CertStartDate"].ToString()).ToShortDateString(), ChFont));
                    CertStartDate.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(CertStartDate);
                    PdfPCell CertEndDate = new PdfPCell(new Phrase(Convert.ToDateTime(Certificate.Rows[i]["CertEndDate"].ToString()).ToShortDateString(), ChFont));
                    CertEndDate.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(CertEndDate);
                }
            }
            else
            {
                PdfPCell CertifiacetZero = new PdfPCell(new Phrase("無", ChFont));
                CertifiacetZero.Colspan = 4;
                CertifiacetZero.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
                CertifiacetZero.Padding = 5;
                pt.AddCell(CertifiacetZero);
            }
            if (PClassSNO != "")
            {
                PdfPCell CoursePlanning = new PdfPCell(new Phrase("課程規劃類別", ChFontTitle));
                CoursePlanning.Colspan = 4;
                CoursePlanning.HorizontalAlignment = Element.ALIGN_CENTER;// 內文靠右
                CoursePlanning.Padding = 10;
                pt.AddCell(CoursePlanning);
                DataTable CoursePlanName = Utility.getCoursePlan(PersonSNO, PClassSNO);
                if (CoursePlanName != null)
                {
                    PdfPCell ClassType = new PdfPCell(new Phrase("課程規劃", ChFont));
                    ClassType.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ClassType);
                    PdfPCell Ctype = new PdfPCell(new Phrase("參考年度", ChFont));
                    Ctype.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(Ctype);
                    PdfPCell ShouldGet = new PdfPCell(new Phrase("可認證/目標積分", ChFont));
                    ShouldGet.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ShouldGet);
                    PdfPCell AlreadyGet = new PdfPCell(new Phrase("可取得的證書", ChFont));
                    AlreadyGet.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(AlreadyGet);
                    for (int i = 0; i < CoursePlanName.Rows.Count; i++)
                    {
                        PdfPCell CTypeName = new PdfPCell(new Phrase(CoursePlanName.Rows[i]["PlanName"].ToString(), ChFont));
                        CTypeName.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CTypeName);
                        PdfPCell CertPublicDate = new PdfPCell(new Phrase(CoursePlanName.Rows[i]["CStartYear"].ToString() + "-" + CoursePlanName.Rows[i]["CEndYear"].ToString(), ChFont));
                        CertPublicDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertPublicDate);
                        PdfPCell CertStartDate = new PdfPCell(new Phrase(CoursePlanName.Rows[i]["PClassTotalHr"].ToString() + "/" + CoursePlanName.Rows[i]["sumHours"].ToString(), ChFont));
                        CertStartDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertStartDate);
                        PdfPCell CertEndDate = new PdfPCell(new Phrase(CoursePlanName.Rows[i]["CTypeName"].ToString(), ChFont));
                        CertEndDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertEndDate);
                    }

                }
                else
                {
                    PdfPCell CertifiacetZero = new PdfPCell(new Phrase("無", ChFont));
                    CertifiacetZero.Colspan = 4;
                    CertifiacetZero.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
                    CertifiacetZero.Padding = 5;
                    pt.AddCell(CertifiacetZero);
                }
                PdfPCell ScoreTitle = new PdfPCell(new Phrase("學分紀錄", ChFontTitle));
                ScoreTitle.Colspan = 4;
                ScoreTitle.HorizontalAlignment = Element.ALIGN_CENTER;// 內文靠右
                ScoreTitle.Padding = 10;
                pt.AddCell(ScoreTitle);
                DataTable Score = Utility.getScore(PersonSNO, PClassSNO);
                if (Score != null)
                {
                    PdfPCell ClassType = new PdfPCell(new Phrase("課程類別", ChFont));
                    ClassType.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ClassType);
                    PdfPCell Ctype = new PdfPCell(new Phrase("授課方式", ChFont));
                    Ctype.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(Ctype);
                    PdfPCell ShouldGet = new PdfPCell(new Phrase("應取得", ChFont));
                    ShouldGet.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ShouldGet);
                    PdfPCell AlreadyGet = new PdfPCell(new Phrase("已取得", ChFont));
                    AlreadyGet.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(AlreadyGet);
                    for (int i = 0; i < Score.Rows.Count; i++)
                    {
                        PdfPCell CTypeName = new PdfPCell(new Phrase(Score.Rows[i]["課程類別"].ToString(), ChFont));
                        CTypeName.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CTypeName);
                        PdfPCell CertPublicDate = new PdfPCell(new Phrase(Score.Rows[i]["授課方式"].ToString(), ChFont));
                        CertPublicDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertPublicDate);
                        PdfPCell CertStartDate = new PdfPCell(new Phrase(Score.Rows[i]["應取得"].ToString(), ChFont));
                        CertStartDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertStartDate);
                        PdfPCell CertEndDate = new PdfPCell(new Phrase(Score.Rows[i]["已取得"].ToString(), ChFont));
                        CertEndDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertEndDate);
                    }

                }
                else
                {
                    PdfPCell CertifiacetZero = new PdfPCell(new Phrase("無", ChFont));
                    CertifiacetZero.Colspan = 4;
                    CertifiacetZero.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
                    CertifiacetZero.Padding = 5;
                    pt.AddCell(CertifiacetZero);
                }
                PdfPCell ScoreDetailTitle = new PdfPCell(new Phrase("明細紀錄", ChFontTitle));
                ScoreDetailTitle.Colspan = 4;
                ScoreDetailTitle.HorizontalAlignment = Element.ALIGN_CENTER;// 內文靠右
                ScoreDetailTitle.Padding = 10;
                pt.AddCell(ScoreDetailTitle);
                DataTable ScoreDetail = Utility.getScoreDetail(PersonSNO,"");
                if (ScoreDetail != null)
                {
                    PdfPCell ClassType = new PdfPCell(new Phrase("課程類別", ChFont));
                    ClassType.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ClassType);
                    PdfPCell Ctype = new PdfPCell(new Phrase("課程名稱", ChFont));
                    Ctype.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(Ctype);
                    PdfPCell ShouldGet = new PdfPCell(new Phrase("授課方式", ChFont));
                    ShouldGet.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ShouldGet);
                    PdfPCell AlreadyGet = new PdfPCell(new Phrase("積分", ChFont));
                    AlreadyGet.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(AlreadyGet);
                    for (int i = 0; i < ScoreDetail.Rows.Count; i++)
                    {
                        PdfPCell CTypeName = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["課程類別"].ToString(), ChFont));
                        CTypeName.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CTypeName);
                        PdfPCell CertPublicDate = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["CourseName"].ToString(), ChFont));
                        CertPublicDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertPublicDate);
                        PdfPCell CertStartDate = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["授課方式"].ToString(), ChFont));
                        CertStartDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertStartDate);
                        PdfPCell CertEndDate = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["積分"].ToString(), ChFont));
                        CertEndDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertEndDate);
                    }

                }
                else
                {
                    PdfPCell CertifiacetZero = new PdfPCell(new Phrase("無", ChFont));
                    CertifiacetZero.Colspan = 4;
                    CertifiacetZero.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
                    CertifiacetZero.Padding = 5;
                    pt.AddCell(CertifiacetZero);
                }


                doc.Add(pt);
                // 塞入資料 -- END
                doc.Close();

                // 在Client端顯示PDF檔，讓USER可以下載
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + UserName + "-證書積分列印" + DateTime.Now.ToShortDateString() + ".pdf");
                Response.ContentType = "application/octet-stream";
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.Flush();
                Response.End();
            }
            else//繼續教育
            {
                PdfPCell CoursePlanning = new PdfPCell(new Phrase("繼續教育課程規劃類別", ChFontTitle));
                CoursePlanning.Colspan = 4;
                CoursePlanning.HorizontalAlignment = Element.ALIGN_CENTER;// 內文靠右
                CoursePlanning.Padding = 10;
                pt.AddCell(CoursePlanning);
                DataTable ECoursePlanName = Utility.getECoursePlan(PersonSNO, EPClassSNO);
                if (ECoursePlanName != null)
                {
                    PdfPCell ClassType = new PdfPCell(new Phrase("課程規劃", ChFont));
                    ClassType.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ClassType);
                    PdfPCell Ctype = new PdfPCell(new Phrase("參考年度", ChFont));
                    Ctype.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(Ctype);
                    PdfPCell ShouldGet = new PdfPCell(new Phrase("可認證/目標積分", ChFont));
                    ShouldGet.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ShouldGet);
                    PdfPCell AlreadyGet = new PdfPCell(new Phrase("可取得的證書", ChFont));
                    AlreadyGet.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(AlreadyGet);
                    for (int i = 0; i < ECoursePlanName.Rows.Count; i++)
                    {
                        PdfPCell CTypeName = new PdfPCell(new Phrase(ECoursePlanName.Rows[i]["PlanName"].ToString(), ChFont));
                        CTypeName.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CTypeName);
                        PdfPCell CertPublicDate = new PdfPCell(new Phrase(ECoursePlanName.Rows[i]["CStartYear"].ToString() + "-" + ECoursePlanName.Rows[i]["CEndYear"].ToString(), ChFont));
                        CertPublicDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertPublicDate);
                        PdfPCell CertStartDate = new PdfPCell(new Phrase(ECoursePlanName.Rows[i]["PClassTotalHr"].ToString() + "/" + ECoursePlanName.Rows[i]["sumHours"].ToString(), ChFont));
                        CertStartDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertStartDate);
                        PdfPCell CertEndDate = new PdfPCell(new Phrase(ECoursePlanName.Rows[i]["CTypeName"].ToString(), ChFont));
                        CertEndDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertEndDate);
                    }

                }
                else
                {
                    PdfPCell CertifiacetZero = new PdfPCell(new Phrase("無", ChFont));
                    CertifiacetZero.Colspan = 4;
                    CertifiacetZero.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
                    CertifiacetZero.Padding = 5;
                    pt.AddCell(CertifiacetZero);
                }
                PdfPCell ScoreTitle = new PdfPCell(new Phrase("繼續教育Elearning學分紀錄", ChFontTitle));
                ScoreTitle.Colspan = 4;
                ScoreTitle.HorizontalAlignment = Element.ALIGN_CENTER;// 內文靠右
                ScoreTitle.Padding = 10;
                pt.AddCell(ScoreTitle);
                DataTable Score = Utility.getScore(PersonSNO, EPClassSNO);
                if (Score != null)
                {
                    PdfPCell ClassType = new PdfPCell(new Phrase("課程類別", ChFont));
                    ClassType.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ClassType);
                    PdfPCell Ctype = new PdfPCell(new Phrase("授課方式", ChFont));
                    Ctype.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(Ctype);
                    //PdfPCell ShouldGet = new PdfPCell(new Phrase("應取得", ChFont));
                    //ShouldGet.HorizontalAlignment = Element.ALIGN_CENTER;
                    //pt.AddCell(ShouldGet);
                    PdfPCell AlreadyGet = new PdfPCell(new Phrase("已取得", ChFont));
                    AlreadyGet.Colspan = 2;
                    AlreadyGet.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(AlreadyGet);
                    for (int i = 0; i < Score.Rows.Count; i++)
                    {
                        PdfPCell CTypeName = new PdfPCell(new Phrase(Score.Rows[i]["課程類別"].ToString(), ChFont));
                        CTypeName.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CTypeName);
                        PdfPCell CertPublicDate = new PdfPCell(new Phrase(Score.Rows[i]["授課方式"].ToString(), ChFont));
                        CertPublicDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertPublicDate);
                        //PdfPCell CertStartDate = new PdfPCell(new Phrase(Score.Rows[i]["應取得"].ToString(), ChFont));
                        //pt.AddCell(CertStartDate);
                        PdfPCell CertEndDate = new PdfPCell(new Phrase(Score.Rows[i]["已取得"].ToString(), ChFont));
                        CertEndDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        CertEndDate.Colspan = 2;
                        pt.AddCell(CertEndDate);
                    }

                }
                else
                {
                    PdfPCell CertifiacetZero = new PdfPCell(new Phrase("無", ChFont));
                    CertifiacetZero.Colspan = 4;
                    CertifiacetZero.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
                    CertifiacetZero.Padding = 5;
                    pt.AddCell(CertifiacetZero);
                }
                PdfPCell ScoreDetailTitle = new PdfPCell(new Phrase("繼續教育明細紀錄", ChFontTitle));
                ScoreDetailTitle.Colspan = 4;
                ScoreDetailTitle.HorizontalAlignment = Element.ALIGN_CENTER;// 內文靠右
                ScoreDetailTitle.Padding = 10;
                pt.AddCell(ScoreDetailTitle);
                DataTable ScoreDetail = Utility.getScoreDetail(PersonSNO,"");
                if (ScoreDetail != null)
                {
                    PdfPCell ClassType = new PdfPCell(new Phrase("課程類別", ChFont));
                    ClassType.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ClassType);
                    PdfPCell Ctype = new PdfPCell(new Phrase("課程名稱", ChFont));
                    Ctype.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(Ctype);
                    PdfPCell ShouldGet = new PdfPCell(new Phrase("授課方式", ChFont));
                    ShouldGet.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ShouldGet);
                    PdfPCell AlreadyGet = new PdfPCell(new Phrase("積分", ChFont));
                    AlreadyGet.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(AlreadyGet);
                    for (int i = 0; i < ScoreDetail.Rows.Count; i++)
                    {
                        PdfPCell CTypeName = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["課程類別"].ToString(), ChFont));
                        CTypeName.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CTypeName);
                        PdfPCell CertPublicDate = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["CourseName"].ToString(), ChFont));
                        CertPublicDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertPublicDate);
                        PdfPCell CertStartDate = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["授課方式"].ToString(), ChFont));
                        CertStartDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertStartDate);
                        PdfPCell CertEndDate = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["積分"].ToString(), ChFont));
                        CertEndDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertEndDate);
                    }

                }
                else
                {
                    PdfPCell CertifiacetZero = new PdfPCell(new Phrase("無", ChFont));
                    CertifiacetZero.Colspan = 4;
                    CertifiacetZero.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
                    CertifiacetZero.Padding = 5;
                    pt.AddCell(CertifiacetZero);
                }
                PdfPCell ScoreUploadTitle = new PdfPCell(new Phrase("繼續教育積分上傳統計", ChFontTitle));
                ScoreUploadTitle.Colspan = 4;
                ScoreUploadTitle.HorizontalAlignment = Element.ALIGN_CENTER;// 內文靠右
                ScoreUploadTitle.Padding = 10;
                pt.AddCell(ScoreUploadTitle);
                DataTable Eintegral = Utility.getEIntegral(PersonID_C, EPClassSNO_C);
                if (Eintegral != null)
                {

                    PdfPCell ClassType = new PdfPCell(new Phrase("通訊", ChFont));
                    ClassType.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ClassType);
                    PdfPCell Ctype = new PdfPCell(new Phrase("線上", ChFont));
                    Ctype.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(Ctype);
                    PdfPCell ShouldGet = new PdfPCell(new Phrase("實體", ChFont));
                    ShouldGet.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ShouldGet);
                    PdfPCell AlreadyGet = new PdfPCell(new Phrase("實習", ChFont));
                    AlreadyGet.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(AlreadyGet);
                    for (int i = 0; i < Eintegral.Rows.Count; i++)
                    {
                        PdfPCell CTypeName = new PdfPCell(new Phrase(Eintegral.Rows[i]["通訊"].ToString(), ChFont));
                        CTypeName.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CTypeName);
                        PdfPCell CertPublicDate = new PdfPCell(new Phrase(Eintegral.Rows[i]["線上"].ToString(), ChFont));
                        CertPublicDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertPublicDate);
                        PdfPCell CertStartDate = new PdfPCell(new Phrase(Eintegral.Rows[i]["實體"].ToString(), ChFont));
                        CertStartDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertStartDate);
                        PdfPCell CertEndDate = new PdfPCell(new Phrase(Eintegral.Rows[i]["實習"].ToString(), ChFont));
                        CertEndDate.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(CertEndDate);
                    }
                }
                else
                {
                    PdfPCell CertifiacetZero = new PdfPCell(new Phrase("無", ChFont));
                    CertifiacetZero.Colspan = 4;
                    CertifiacetZero.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
                    CertifiacetZero.Padding = 5;
                    pt.AddCell(CertifiacetZero);
                }
                PdfPCell ScoreUploadDetailTitle = new PdfPCell(new Phrase("繼續教育積分上傳統計紀錄", ChFontTitle));
                ScoreUploadDetailTitle.Colspan = 4;
                ScoreUploadDetailTitle.HorizontalAlignment = Element.ALIGN_CENTER;// 內文靠右
                ScoreUploadDetailTitle.Padding = 10;
                pt.AddCell(ScoreUploadDetailTitle);
                DataTable EIntegralDetail = Utility.getEIntegralDetail(PersonID_C, EPClassSNO_C);
                if (EIntegralDetail != null)
                {

                    PdfPCell ClassType = new PdfPCell(new Phrase("課程名稱", ChFont));                  
                    ClassType.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ClassType);
                    PdfPCell ClassType1 = new PdfPCell(new Phrase("類型", ChFont));
                    ClassType1.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ClassType1);
                    PdfPCell Ctype = new PdfPCell(new Phrase("積分", ChFont));
                    Ctype.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(Ctype);
                    PdfPCell ShouldGet = new PdfPCell(new Phrase("上傳時間", ChFont));
                    ShouldGet.HorizontalAlignment = Element.ALIGN_CENTER;
                    pt.AddCell(ShouldGet);
    
                    for (int i = 0; i < EIntegralDetail.Rows.Count; i++)
                    {
                        PdfPCell aa = new PdfPCell(new Phrase(EIntegralDetail.Rows[i]["CourseName"].ToString(), ChFont));
                        aa.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(aa);
                        PdfPCell aaa = new PdfPCell(new Phrase(EIntegralDetail.Rows[i]["Mval"].ToString(), ChFont));
                        aaa.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(aaa);
                        PdfPCell bb = new PdfPCell(new Phrase(EIntegralDetail.Rows[i]["Integral"].ToString(), ChFont));
                        bb.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(bb);
                        PdfPCell cc = new PdfPCell(new Phrase(EIntegralDetail.Rows[i]["CreateDT"].ToString(), ChFont));
                        cc.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(cc);
                    }
                }
                else
                {
                    PdfPCell CertifiacetZero = new PdfPCell(new Phrase("無", ChFont));
                    CertifiacetZero.Colspan = 4;
                    CertifiacetZero.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
                    CertifiacetZero.Padding = 5;
                    pt.AddCell(CertifiacetZero);
                }
                doc.Add(pt);
                // 塞入資料 -- END
                doc.Close();

                // 在Client端顯示PDF檔，讓USER可以下載
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename="+ UserName + "-繼續教育積分列印"+DateTime.Now.ToShortDateString()+".pdf");
                Response.ContentType = "application/octet-stream";
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.Flush();
                Response.End();
            }
           
            
        }

    }

    public string getEPClassSNO(string PClassSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = @"Select * from QS_ECoursePlanningClass where PClassSNO=@PClassSNO";
        aDict.Add("PClassSNO", PClassSNO);
        DataTable ObjDT = objDH.queryData(sql, aDict);
        return ObjDT.Rows[0]["EPClassSNO"].ToString();
    }
}