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
        string Word = Request.QueryString["Word"];
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
        if (Word == "0")
        {

            DataHelper ObjDH = new DataHelper();
            DataTable ObjDT = new DataTable();
            Dictionary<string, object> adict = new Dictionary<string, object>();
            string sql = "Select * from Person where PersonSNO=@PersonSNO";
            adict.Add("PersonSNO", PersonSNO);
            ObjDT = ObjDH.queryData(sql, adict);
            string UserName = ObjDT.Rows[0]["PName"].ToString();
            string UserPersonID = ObjDT.Rows[0]["PersonID"].ToString();
            string UserPersonIDXXXX = UserPersonID.Substring(0,6)+"****";
            if (PClassSNO != null && PersonSNO != null)
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
                PdfPTable pt = new PdfPTable(new float[] { 2, 2, 2, 2, 2, 2 ,2});
                // 表格總寬
                pt.TotalWidth = 500f;
                pt.LockedWidth = true;
                // 塞入資料 -- START
                // 設定表頭
                if (PClassSNO != "")//新訓
                {
                    PdfPCell header = new PdfPCell(new Phrase("戒菸服務人員訓練課程時數(新訓)", ChFont_Header));
                    header.Colspan = 7;
                    header.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
                    header.Padding = 5;
                    header.Border = 0;
                    header.PaddingBottom = 10;
                    pt.AddCell(header);
                }
                else
                {
                    PdfPCell header = new PdfPCell(new Phrase("戒菸服務人員訓練課程時數(繼續教育)", ChFont_Header));
                    header.Colspan = 7;
                    header.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
                    header.Padding = 5;
                    header.Border = 0;
                    header.PaddingBottom = 10;
                    pt.AddCell(header);
                }

                PdfPCell subTitle = new PdfPCell(new Phrase("醫事人員戒菸服務訓練系統", ChFontTitle));
                subTitle.Colspan = 7;
                subTitle.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
                subTitle.Padding = 5;
                subTitle.Border = 0;
                subTitle.PaddingBottom = 10;
                pt.AddCell(subTitle);
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
                PdfPCell PName = new PdfPCell(new Phrase("姓名：" + UserName, ChFont));
                PName.BorderWidthBottom = 0;
                PName.BorderWidthRight = 0;
                PName.Colspan = 4;
                pt.AddCell(PName);
                PdfPCell PrintDate = new PdfPCell(new Phrase("列印日期：" + DateTime.Now.ToShortDateString(), ChFont));
                PrintDate.BorderWidthBottom = 0;
                PrintDate.BorderWidthLeft = 0;
                PrintDate.Colspan = 3;
                PrintDate.HorizontalAlignment = Element.ALIGN_RIGHT;// 表頭內文置右
                pt.AddCell(PrintDate);

                PdfPCell PersonID = new PdfPCell(new Phrase("身分證字號：" + UserPersonIDXXXX, ChFont));
                PersonID.Colspan = 7;
                PersonID.BorderWidthTop = 0;
                PersonID.PaddingBottom = 10;
                pt.AddCell(PersonID);
                if (PClassSNO != "")
                {
                    DataTable CoursePlanName = Utility.getCoursePlan(PersonSNO, PClassSNO);
                    if (CoursePlanName != null)
                    {
                        PdfPCell ClassType = new PdfPCell(new Phrase("訓練類別", ChFont));
                        ClassType.HorizontalAlignment = Element.ALIGN_CENTER;
                        ClassType.Colspan = 3;
                        pt.AddCell(ClassType);
                        PdfPCell ShouldGet = new PdfPCell(new Phrase("已取得時數", ChFont));
                        ShouldGet.HorizontalAlignment = Element.ALIGN_CENTER;
                        ShouldGet.Colspan = 2;
                        pt.AddCell(ShouldGet);
                        PdfPCell AlreadyGet = new PdfPCell(new Phrase("目標時數", ChFont));
                        AlreadyGet.HorizontalAlignment = Element.ALIGN_CENTER;
                        AlreadyGet.Colspan = 2;
                        pt.AddCell(AlreadyGet);
                        for (int i = 0; i < CoursePlanName.Rows.Count; i++)
                        {
                            PdfPCell CTypeName = new PdfPCell(new Phrase("新訓課程", ChFont));//固定寫"新訓課程"
                            CTypeName.HorizontalAlignment = Element.ALIGN_CENTER;
                            CTypeName.Colspan = 3;
                            pt.AddCell(CTypeName);
                            PdfPCell CertStartDate = new PdfPCell(new Phrase(CoursePlanName.Rows[i]["PClassTotalHr"].ToString(), ChFont));
                            CertStartDate.HorizontalAlignment = Element.ALIGN_CENTER;
                            CertStartDate.Colspan = 2;
                            pt.AddCell(CertStartDate);
                            PdfPCell CertEndDate = new PdfPCell(new Phrase(CoursePlanName.Rows[i]["sumHours"].ToString(), ChFont));
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
                    PdfPCell ScoreDetailTitle = new PdfPCell(new Phrase("新訓課程時數明細", ChFontTitle));
                    ScoreDetailTitle.Colspan = 7;
                    ScoreDetailTitle.HorizontalAlignment = Element.ALIGN_CENTER;// 內文靠右
                    ScoreDetailTitle.Padding = 10;
                    pt.AddCell(ScoreDetailTitle);
                    DataTable ScoreDetail = Utility.getScoreDetail(PersonSNO, PClassSNO);
                    if (ScoreDetail != null)
                    {
                        PdfPCell Ctype = new PdfPCell(new Phrase("課程名稱", ChFont));
                        Ctype.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(Ctype);
                        PdfPCell ShouldGet = new PdfPCell(new Phrase("上課方式", ChFont));
                        ShouldGet.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(ShouldGet);
                        PdfPCell AlreadyGet = new PdfPCell(new Phrase("時數", ChFont));
                        AlreadyGet.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(AlreadyGet);
                        PdfPCell UploadTime = new PdfPCell(new Phrase("上傳時間", ChFont));
                        UploadTime.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(UploadTime);
                        PdfPCell Unit = new PdfPCell(new Phrase("開課單位", ChFont));
                        Unit.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(Unit);
                        PdfPCell ClassTime = new PdfPCell(new Phrase("上課日期", ChFont));
                        ClassTime.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(ClassTime);
                        PdfPCell NoteTitle = new PdfPCell(new Phrase("備註", ChFont));
                        NoteTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(NoteTitle);
                        for (int i = 0; i < ScoreDetail.Rows.Count; i++)
                        {
                            PdfPCell CTypeName = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["CourseName"].ToString(), ChFont));
                            CTypeName.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(CTypeName);
                            PdfPCell CertPublicDate = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["取得方式"].ToString(), ChFont));
                            CertPublicDate.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(CertPublicDate);
                            PdfPCell CertStartDate = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["積分"].ToString(), ChFont));
                            CertStartDate.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(CertStartDate);
                            PdfPCell CertEndDate = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["上傳時間"].ToString(), ChFont));
                            CertEndDate.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(CertEndDate);
                            PdfPCell UnitName = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["開課單位"].ToString(), ChFont));
                            UnitName.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(UnitName);
                            PdfPCell Class = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["上課日期"].ToString(), ChFont));
                            Class.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(Class);
                            PdfPCell Note = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["備註"].ToString(), ChFont));
                            Note.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(Note);
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
                    Response.AddHeader("content-disposition", "attachment;filename=" + UserName + "-新訓時數列印" + DateTime.Now.ToShortDateString() + ".pdf");
                    Response.ContentType = "application/octet-stream";
                    //Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);

                    var stream = new MemoryStream();

                    using (SyncfusionHelper.IHelper IH = new SyncfusionHelper.Helper())
                    {     // 讀入方法一：byte    // 
                          //
                        var byByte = IH.ByByte(ms.ToArray());
                        //byte[] myByteArray = new byte[];
                        stream.Write(byByte, 0, byByte.Length);

                    }
                    Response.OutputStream.Write(stream.GetBuffer(), 0, stream.GetBuffer().Length);
                    stream.Dispose();

                    Response.OutputStream.Flush();
                    Response.OutputStream.Close();
                    Response.Flush();
                    Response.End();
                }
                else//繼續教育
                {
                    DataTable ECoursePlanName = Utility.getECoursePlan(PersonSNO, EPClassSNO);
                    if (ECoursePlanName != null)
                    {
                        PdfPCell ClassType = new PdfPCell(new Phrase("訓練類別", ChFont));
                        ClassType.Colspan = 3;
                        ClassType.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(ClassType);
                        //PdfPCell Ctype = new PdfPCell(new Phrase("參考年度", ChFont));
                        //Ctype.HorizontalAlignment = Element.ALIGN_CENTER;
                        //pt.AddCell(Ctype);
                        PdfPCell ShouldGet = new PdfPCell(new Phrase("已取得時數", ChFont));
                        ShouldGet.Colspan = 2;
                        ShouldGet.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(ShouldGet);
                        PdfPCell AlreadyGet = new PdfPCell(new Phrase("目標時數", ChFont));
                        AlreadyGet.Colspan = 2;
                        AlreadyGet.HorizontalAlignment = Element.ALIGN_CENTER;
                        pt.AddCell(AlreadyGet);
                        for (int i = 0; i < ECoursePlanName.Rows.Count; i++)
                        {
                            PdfPCell CTypeName = new PdfPCell(new Phrase("繼續教育課程", ChFont));
                            CTypeName.HorizontalAlignment = Element.ALIGN_CENTER;
                            CTypeName.Colspan = 3;
                            pt.AddCell(CTypeName);
                            PdfPCell CertStartDate = new PdfPCell(new Phrase(ECoursePlanName.Rows[i]["PClassTotalHr"].ToString(), ChFont)); 
                            CertStartDate.HorizontalAlignment = Element.ALIGN_CENTER;
                            CertStartDate.Colspan = 2;
                            pt.AddCell(CertStartDate);
                            PdfPCell CertEndDate = new PdfPCell(new Phrase(ECoursePlanName.Rows[i]["sumHours"].ToString(), ChFont));
                            CertEndDate.HorizontalAlignment = Element.ALIGN_CENTER;
                            CertEndDate.Colspan = 2;
                            pt.AddCell(CertEndDate);
                        }

                    }
                    else
                    {
                        PdfPCell CertifiacetZero = new PdfPCell(new Phrase("無", ChFont));
                        CertifiacetZero.Colspan = 6;
                        CertifiacetZero.HorizontalAlignment = Element.ALIGN_CENTER;// 表頭內文置中
                        CertifiacetZero.Padding = 5;
                        pt.AddCell(CertifiacetZero);
                    }
                    PdfPCell ScoreDetailTitle = new PdfPCell(new Phrase("繼續教育課程時數明細", ChFontTitle));
                    ScoreDetailTitle.Colspan = 7;
                    ScoreDetailTitle.HorizontalAlignment = Element.ALIGN_CENTER;// 內文靠右
                    ScoreDetailTitle.Padding = 10;
                    pt.AddCell(ScoreDetailTitle);
                    DataTable ScoreDetail = Utility.getExScoreDetail(UserPersonID, EPClassSNO, EPClassSNO_C);
                    DataTable OnlineScoreDetail = Utility.getExOnlineScoreDetail(UserPersonID, PersonSNO,EPClassSNO, EPClassSNO_C);
                    if (ScoreDetail != null && OnlineScoreDetail!= null)
                    {
                        PdfPCell CourseName = new PdfPCell(new Phrase("課程名稱", ChFont));
                        CourseName.HorizontalAlignment = Element.ALIGN_CENTER;
                        CourseName.Colspan = 1;
                        pt.AddCell(CourseName);
                        PdfPCell Ctype = new PdfPCell(new Phrase("上課方式", ChFont));
                        Ctype.HorizontalAlignment = Element.ALIGN_CENTER;
                        Ctype.Colspan = 1;
                        pt.AddCell(Ctype);
                        PdfPCell Chour = new PdfPCell(new Phrase("時數", ChFont));
                        Chour.HorizontalAlignment = Element.ALIGN_CENTER;
                        Chour.Colspan = 1;
                        pt.AddCell(Chour);
                        PdfPCell UploadTime = new PdfPCell(new Phrase("上傳時間", ChFont));
                        UploadTime.HorizontalAlignment = Element.ALIGN_CENTER;
                        UploadTime.Colspan = 1;
                        pt.AddCell(UploadTime);
                        PdfPCell Unit = new PdfPCell(new Phrase("開課單位", ChFont));
                        Unit.HorizontalAlignment = Element.ALIGN_CENTER;
                        Unit.Colspan = 1;
                        pt.AddCell(Unit);
                        PdfPCell ClassTime = new PdfPCell(new Phrase("上課日期", ChFont));
                        ClassTime.HorizontalAlignment = Element.ALIGN_CENTER;
                        ClassTime.Colspan = 1;
                        pt.AddCell(ClassTime);
                        PdfPCell NoteTitle = new PdfPCell(new Phrase("備註", ChFont));
                        NoteTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                        NoteTitle.Colspan = 1;
                        pt.AddCell(NoteTitle);
                        for (int i = 0; i < OnlineScoreDetail.Rows.Count; i++)
                        {
                            PdfPCell CTypeName = new PdfPCell(new Phrase(OnlineScoreDetail.Rows[i]["CourseName"].ToString(), ChFont));
                            CTypeName.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(CTypeName);
                            PdfPCell GetType = new PdfPCell(new Phrase(OnlineScoreDetail.Rows[i]["Ctype"].ToString(), ChFont));
                            GetType.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(GetType);
                            PdfPCell Integral = new PdfPCell(new Phrase(OnlineScoreDetail.Rows[i]["CHour"].ToString(), ChFont));
                            Integral.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(Integral);
                            PdfPCell UpTime = new PdfPCell(new Phrase(OnlineScoreDetail.Rows[i]["上課日期"].ToString(), ChFont));
                            UpTime.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(UpTime);
                            PdfPCell Cunit = new PdfPCell(new Phrase("-", ChFont));
                            Cunit.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(Cunit);
                            PdfPCell ClassDate = new PdfPCell(new Phrase(OnlineScoreDetail.Rows[i]["上課日期"].ToString(), ChFont));
                            ClassDate.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(ClassDate);
                            PdfPCell Note = new PdfPCell(new Phrase(OnlineScoreDetail.Rows[i]["備註"].ToString(), ChFont));
                            Note.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(Note);
                        }
                        for (int i = 0; i < ScoreDetail.Rows.Count; i++)
                        {
                            PdfPCell CTypeName = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["CourseName"].ToString(), ChFont));
                            CTypeName.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(CTypeName);
                            PdfPCell GetType = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["授課方式"].ToString(), ChFont));
                            GetType.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(GetType);
                            PdfPCell Integral = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["Integral"].ToString(), ChFont));
                            Integral.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(Integral);
                            PdfPCell UpTime = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["CreateDT"].ToString(), ChFont));
                            UpTime.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(UpTime);
                            PdfPCell Cunit = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["Unit"].ToString(), ChFont));
                            Cunit.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(Cunit);
                            PdfPCell ClassDate = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["CDate"].ToString(), ChFont));
                            ClassDate.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(ClassDate);
                            PdfPCell Note = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["備註"].ToString(), ChFont));
                            Note.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(Note);
                        }

                    }
                    else if (ScoreDetail != null && OnlineScoreDetail == null)
                    {
                        PdfPCell CourseName = new PdfPCell(new Phrase("課程名稱", ChFont));
                        CourseName.HorizontalAlignment = Element.ALIGN_CENTER;
                        CourseName.Colspan = 1;
                        pt.AddCell(CourseName);
                        PdfPCell Ctype = new PdfPCell(new Phrase("上課方式", ChFont));
                        Ctype.HorizontalAlignment = Element.ALIGN_CENTER;
                        Ctype.Colspan = 1;
                        pt.AddCell(Ctype);
                        PdfPCell Chour = new PdfPCell(new Phrase("時數", ChFont));
                        Chour.HorizontalAlignment = Element.ALIGN_CENTER;
                        Chour.Colspan = 1;
                        pt.AddCell(Chour);
                        PdfPCell UploadTime = new PdfPCell(new Phrase("上傳時間", ChFont));
                        UploadTime.HorizontalAlignment = Element.ALIGN_CENTER;
                        UploadTime.Colspan = 1;
                        pt.AddCell(UploadTime);
                        PdfPCell Unit = new PdfPCell(new Phrase("開課單位", ChFont));
                        Unit.HorizontalAlignment = Element.ALIGN_CENTER;
                        Unit.Colspan = 1;
                        pt.AddCell(Unit);
                        PdfPCell ClassTime = new PdfPCell(new Phrase("上課日期", ChFont));
                        ClassTime.HorizontalAlignment = Element.ALIGN_CENTER;
                        ClassTime.Colspan = 1;
                        pt.AddCell(ClassTime);
                        PdfPCell NoteTitle = new PdfPCell(new Phrase("備註", ChFont));
                        NoteTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                        NoteTitle.Colspan = 1;
                        pt.AddCell(NoteTitle);
                        for (int i = 0; i < ScoreDetail.Rows.Count; i++)
                        {
                            PdfPCell CTypeName = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["CourseName"].ToString(), ChFont));
                            CTypeName.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(CTypeName);
                            PdfPCell GetType = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["授課方式"].ToString(), ChFont));
                            GetType.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(GetType);
                            PdfPCell Integral = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["Integral"].ToString(), ChFont));
                            Integral.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(Integral);
                            PdfPCell UpTime = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["CreateDT"].ToString(), ChFont));
                            UpTime.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(UpTime);
                            PdfPCell Cunit = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["Unit"].ToString(), ChFont));
                            Cunit.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(Cunit);
                            PdfPCell ClassDate = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["CDate"].ToString(), ChFont));
                            ClassDate.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(ClassDate);
                            PdfPCell Note = new PdfPCell(new Phrase(ScoreDetail.Rows[i]["備註"].ToString(), ChFont));
                            Note.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(Note);
                        }
                    }
                    else if (ScoreDetail == null && OnlineScoreDetail != null)
                    {
                        PdfPCell CourseName = new PdfPCell(new Phrase("課程名稱", ChFont));
                        CourseName.HorizontalAlignment = Element.ALIGN_CENTER;
                        CourseName.Colspan = 1;
                        pt.AddCell(CourseName);
                        PdfPCell Ctype = new PdfPCell(new Phrase("上課方式", ChFont));
                        Ctype.HorizontalAlignment = Element.ALIGN_CENTER;
                        Ctype.Colspan = 1;
                        pt.AddCell(Ctype);
                        PdfPCell Chour = new PdfPCell(new Phrase("時數", ChFont));
                        Chour.HorizontalAlignment = Element.ALIGN_CENTER;
                        Chour.Colspan = 1;
                        pt.AddCell(Chour);
                        PdfPCell UploadTime = new PdfPCell(new Phrase("上傳時間", ChFont));
                        UploadTime.HorizontalAlignment = Element.ALIGN_CENTER;
                        UploadTime.Colspan = 1;
                        pt.AddCell(UploadTime);
                        PdfPCell Unit = new PdfPCell(new Phrase("開課單位", ChFont));
                        Unit.HorizontalAlignment = Element.ALIGN_CENTER;
                        Unit.Colspan = 1;
                        pt.AddCell(Unit);
                        PdfPCell ClassTime = new PdfPCell(new Phrase("上課日期", ChFont));
                        ClassTime.HorizontalAlignment = Element.ALIGN_CENTER;
                        ClassTime.Colspan = 1;
                        pt.AddCell(ClassTime);
                        PdfPCell NoteTitle = new PdfPCell(new Phrase("備註", ChFont));
                        NoteTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                        NoteTitle.Colspan = 1;
                        pt.AddCell(NoteTitle);
                        for (int i = 0; i < OnlineScoreDetail.Rows.Count; i++)
                        {
                            PdfPCell CTypeName = new PdfPCell(new Phrase(OnlineScoreDetail.Rows[i]["CourseName"].ToString(), ChFont));
                            CTypeName.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(CTypeName);
                            PdfPCell GetType = new PdfPCell(new Phrase(OnlineScoreDetail.Rows[i]["Ctype"].ToString(), ChFont));
                            GetType.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(GetType);
                            PdfPCell Integral = new PdfPCell(new Phrase(OnlineScoreDetail.Rows[i]["CHour"].ToString(), ChFont));
                            Integral.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(Integral);
                            PdfPCell UpTime = new PdfPCell(new Phrase(OnlineScoreDetail.Rows[i]["上課日期"].ToString(), ChFont));
                            UpTime.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(UpTime);
                            PdfPCell Cunit = new PdfPCell(new Phrase("-", ChFont));
                            Cunit.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(Cunit);
                            PdfPCell ClassDate = new PdfPCell(new Phrase(OnlineScoreDetail.Rows[i]["上課日期"].ToString(), ChFont));
                            ClassDate.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(ClassDate);
                            PdfPCell Note = new PdfPCell(new Phrase(OnlineScoreDetail.Rows[i]["備註"].ToString(), ChFont));
                            Note.HorizontalAlignment = Element.ALIGN_CENTER;
                            pt.AddCell(Note);
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
                    Response.AddHeader("content-disposition", "attachment;filename=" + UserName + "-繼續教育時數列印" + DateTime.Now.ToShortDateString() + ".pdf");
                    Response.ContentType = "application/octet-stream";
                    //Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);

                    var stream = new MemoryStream();

                    using (SyncfusionHelper.IHelper IH = new SyncfusionHelper.Helper())
                    {     // 讀入方法一：byte    // 
                          //
                        var byByte = IH.ByByte(ms.ToArray());
                        //byte[] myByteArray = new byte[];
                        stream.Write(byByte, 0, byByte.Length);

                    }
                    Response.OutputStream.Write(stream.GetBuffer(), 0, stream.GetBuffer().Length);
                    stream.Dispose();

                    Response.OutputStream.Flush();
                    Response.OutputStream.Close();
                    Response.Flush();
                    Response.End();
                }


            }

        }
        else
        {
            if (Request.QueryString["PersonSNO"] != null)
            {
                PersonSNO = Request.QueryString["PersonSNO"].ToString();
            }
            if (Request.QueryString["PClassSNO"] != null)
            {
                PClassSNO = Request.QueryString["PClassSNO"].ToString();
            }
            if (Request.QueryString["EPClassSNO"] != null)
            {
                EPClassSNO = Request.QueryString["EPClassSNO"].ToString();
            }
            string PersonName = getPName(PersonSNO);
            Response.Clear();
            Response.BufferOutput = true;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("big5");
            Response.Charset = "big5";
            if (PClassSNO != "")
            {
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + PersonName + "時數" + DateTime.Now.ToShortDateString() + ".doc");
            }
            else
            {
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + PersonName + "繼續教育時數列印" + DateTime.Now.ToShortDateString() + ".doc");
            }
            Response.ContentType = "application/ms-word";
            Page.EnableViewState = false;

            if (PClassSNO != "")//Word 證書匯出
            {
                using (StringWriter sw = new StringWriter())
                {
                    DataHelper ObjDH = new DataHelper();
                    DataTable ObjDT = new DataTable();
                    Dictionary<string, object> adict = new Dictionary<string, object>();
                    string sql = "Select * from Person where PersonSNO=@PersonSNO";
                    adict.Add("PersonSNO", PersonSNO);
                    ObjDT = ObjDH.queryData(sql, adict);
                    string UserName = ObjDT.Rows[0]["PName"].ToString();
                    string UserPersonID = ObjDT.Rows[0]["PersonID"].ToString();
                    string UserPersonIDXXX = UserPersonID.Substring(0, 6)+"****";
                    if (PClassSNO != null && PersonSNO != null)
                    {
                        Table t = new Table();
                        t.Attributes.Add("Border", "1");
                        t.Width = 550;
                        t.Height = 800;
                        TableRow tr = new TableRow();
                        TableCell tc = new TableCell();
                        tc.ColumnSpan = 7;
                        tc.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        Label lbl = new Label();
                        lbl.Text = "戒菸服務人員訓練課程時數(新訓)";
                        tc.Controls.Add(lbl);
                        tr.Cells.Add(tc);
                        t.Rows.Add(tr);
                        TableRow ts = new TableRow();
                        TableCell st = new TableCell();
                        st.ColumnSpan = 7;
                        st.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        Label lblst = new Label();
                        lblst.Text = "醫事人員戒菸服務訓練系統";
                        st.Controls.Add(lblst);
                        ts.Cells.Add(st);
                        t.Rows.Add(ts);



                        TableRow tr1 = new TableRow();
                        TableCell tc1 = new TableCell();
                        Label lbl1 = new Label();
                        lbl1.Text = "姓名：" + UserName;
                        tc1.ColumnSpan = 4;
                        tc1.Controls.Add(lbl1);
                        TableCell tc2 = new TableCell();
                        Label lbl2 = new Label();
                        lbl2.Text = "列印日期：" + DateTime.Now.ToShortDateString();
                        tc2.ColumnSpan = 3;
                        tc2.Controls.Add(lbl2);
                        tr1.Cells.Add(tc1);
                        tr1.Cells.Add(tc2);
                        t.Rows.Add(tr1);
                        TableRow tr2 = new TableRow();
                        TableCell tc3 = new TableCell();
                        tc3.ColumnSpan = 7;
                        Label lbl3 = new Label();
                        lbl3.Text = "身分證字號：" + UserPersonIDXXX;
                        tc3.Controls.Add(lbl3);
                        tr2.Cells.Add(tc3);
                        t.Rows.Add(tr2);
                        
                        TableRow tr6 = new TableRow();
                        TableCell tc13 = new TableCell();
                        tc13.ColumnSpan = 7;
                        tc13.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        Label lbl13 = new Label();
                        lbl13.Text = "課程規劃類別";
                        tc13.Controls.Add(lbl13);
                        tr6.Cells.Add(tc13);
                        t.Rows.Add(tr6);

                        TableRow tr7 = new TableRow();
                        TableCell tc14 = new TableCell();
                        tc14.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        tc14.ColumnSpan = 3;
                        Label lbl14 = new Label();
                        lbl14.Text = "訓練類別";
                        tc14.Controls.Add(lbl14);
                        TableCell tc16 = new TableCell();
                        tc16.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        tc16.ColumnSpan = 2;
                        Label lbl16 = new Label();
                        lbl16.Text = "已取得時數";
                        tc16.Controls.Add(lbl16);
                        TableCell tc17 = new TableCell();
                        tc17.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        tc17.ColumnSpan = 2;
                        Label lbl17 = new Label();
                        lbl17.Text = "目標時數";
                        tc17.Controls.Add(lbl17);
                        tr7.Cells.Add(tc14); /*tr7.Cells.Add(tc15);*/ tr7.Cells.Add(tc16); tr7.Cells.Add(tc17);
                        t.Rows.Add(tr7);


                        DataTable CoursePlanName = Utility.getCoursePlan(PersonSNO, PClassSNO);
                        if (CoursePlanName != null)
                        {
                            for (int i = 0; i < CoursePlanName.Rows.Count; i++)
                            {
                                TableRow tr5 = new TableRow();
                                TableCell tc9 = new TableCell();
                                tc9.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                tc9.ColumnSpan = 3;
                                Label lbl9 = new Label();
                                lbl9.Text = "新訓課程";//固定寫"新訓課程"
                                tc9.Controls.Add(lbl9);
                                TableCell tc10 = new TableCell();
                                tc10.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                tc10.ColumnSpan = 2;
                                Label lbl10 = new Label();
                                lbl10.Text = CoursePlanName.Rows[i]["PClassTotalHr"].ToString();
                                tc10.Controls.Add(lbl10);
                                TableCell tc11 = new TableCell();
                                tc11.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                tc11.ColumnSpan = 2;
                                Label lbl11 = new Label();
                                lbl11.Text = CoursePlanName.Rows[i]["sumHours"].ToString();
                                tc11.Controls.Add(lbl11);
                               
                                tr5.Cells.Add(tc9); tr5.Cells.Add(tc10); tr5.Cells.Add(tc11);
                                t.Rows.Add(tr5);
                            }
                        }
                        else
                        {
                            TableRow tr_n = new TableRow();
                            TableCell tc_n = new TableCell();
                            tc_n.ColumnSpan = 4;
                            tc_n.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                            Label lbl_n = new Label();
                            lbl_n.Text = "無";
                            tc_n.Controls.Add(lbl_n);
                            tr_n.Cells.Add(tc_n);
                            t.Rows.Add(tr_n);
                        }
                       
                        TableRow tr10 = new TableRow();
                        TableCell tc23 = new TableCell();
                        tc23.ColumnSpan = 7;
                        tc23.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        Label lbl23 = new Label();
                        lbl23.Text = "明細紀錄";
                        tc23.Controls.Add(lbl23);
                        tr10.Cells.Add(tc23);
                        t.Rows.Add(tr10);

                        TableRow tr11 = new TableRow();
                        TableCell tc24 = new TableCell();
                        tc24.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        tc24.ColumnSpan = 1;
                        Label lbl24 = new Label();
                        lbl24.Text = "課程名稱";
                        tc24.Controls.Add(lbl24);
                        TableCell tc25 = new TableCell();
                        tc25.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        tc25.ColumnSpan = 1;
                        Label lbl25 = new Label();
                        lbl25.Text = "上課方式";
                        tc25.Controls.Add(lbl25);
                        TableCell tc26 = new TableCell();
                        tc26.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        tc26.ColumnSpan = 1;
                        Label lbl26 = new Label();
                        lbl26.Text = "時數";
                        tc26.Controls.Add(lbl26);
                        TableCell tc27 = new TableCell();
                        tc27.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        tc27.ColumnSpan = 1;
                        Label lbl27 = new Label();
                        lbl27.Text = "上傳時間";
                        tc27.Controls.Add(lbl27);
                        TableCell tc28 = new TableCell();
                        tc28.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        tc28.ColumnSpan = 1;
                        Label lbl28 = new Label();
                        lbl28.Text = "開課單位";
                        tc28.Controls.Add(lbl28);
                        TableCell tc29 = new TableCell();
                        tc29.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        tc29.ColumnSpan = 1;
                        Label lbl29 = new Label();
                        lbl29.Text = "上課日期";
                        tc29.Controls.Add(lbl29);
                        TableCell tc30 = new TableCell();
                        tc30.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        tc30.ColumnSpan = 1;
                        Label lbl30 = new Label();
                        lbl30.Text = "備註";
                        tc30.Controls.Add(lbl30);
                        tr11.Cells.Add(tc24); tr11.Cells.Add(tc25); tr11.Cells.Add(tc26); tr11.Cells.Add(tc27); tr11.Cells.Add(tc28); tr11.Cells.Add(tc29); tr11.Cells.Add(tc30);
                        t.Rows.Add(tr11);
                        DataTable ScoreDetail = Utility.getScoreDetail(PersonSNO, PClassSNO);
                        if (ScoreDetail != null)
                        {
                            for (int i = 0; i < ScoreDetail.Rows.Count; i++)
                            {
                                TableRow tr5 = new TableRow();
                                TableCell tc9 = new TableCell();
                                tc9.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl9 = new Label();
                                lbl9.Text = ScoreDetail.Rows[i]["CourseName"].ToString();
                                tc9.Controls.Add(lbl9);
                                TableCell tc10 = new TableCell();
                                tc10.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl10 = new Label();
                                lbl10.Text = ScoreDetail.Rows[i]["取得方式"].ToString();
                                tc10.Controls.Add(lbl10);
                                TableCell tc11 = new TableCell();
                                tc11.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl11 = new Label();
                                lbl11.Text = ScoreDetail.Rows[i]["積分"].ToString();
                                tc11.Controls.Add(lbl11);
                                TableCell tc12 = new TableCell();
                                tc12.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl12 = new Label();
                                lbl12.Text = ScoreDetail.Rows[i]["上傳時間"].ToString();
                                tc12.Controls.Add(lbl12);
                                TableCell tc15 = new TableCell();
                                tc15.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl15 = new Label();
                                lbl15.Text = ScoreDetail.Rows[i]["開課單位"].ToString();
                                tc15.Controls.Add(lbl15);
                                TableCell tc18 = new TableCell();
                                tc18.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl18 = new Label();
                                lbl18.Text = ScoreDetail.Rows[i]["上課日期"].ToString();
                                tc18.Controls.Add(lbl18);
                                TableCell tc19 = new TableCell();
                                tc19.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl19 = new Label();
                                lbl19.Text = ScoreDetail.Rows[i]["備註"].ToString();
                                tc19.Controls.Add(lbl19);
                                tr5.Cells.Add(tc9); tr5.Cells.Add(tc10); tr5.Cells.Add(tc11); tr5.Cells.Add(tc12); tr5.Cells.Add(tc15); tr5.Cells.Add(tc18); tr5.Cells.Add(tc19);
                                t.Rows.Add(tr5);
                            }
                        }
                        else
                        {
                            TableRow tr_n = new TableRow();
                            TableCell tc_n = new TableCell();
                            tc_n.ColumnSpan = 4;
                            tc_n.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                            Label lbl_n = new Label();
                            lbl_n.Text = "無";
                            tc_n.Controls.Add(lbl_n);
                            tr_n.Cells.Add(tc_n);
                            t.Rows.Add(tr_n);
                        }




                        t.RenderControl(new HtmlTextWriter(sw));

                        string html = sw.ToString();
                        Response.Write(sw.ToString());
                        Response.End();
                    }

                }
            }
            else
            {//Word 繼續教育匯出
                using (StringWriter sw = new StringWriter())
                {
                    DataHelper ObjDH = new DataHelper();
                    DataTable ObjDT = new DataTable();
                    Dictionary<string, object> adict = new Dictionary<string, object>();
                    string sql = "Select * from Person where PersonSNO=@PersonSNO";
                    adict.Add("PersonSNO", PersonSNO);
                    ObjDT = ObjDH.queryData(sql, adict);
                    string UserName = ObjDT.Rows[0]["PName"].ToString();
                    string UserPersonID = ObjDT.Rows[0]["PersonID"].ToString();

                    if (PClassSNO != null && PersonSNO != null)
                    {
                        Table t = new Table();
                        t.Attributes.Add("Border", "1");
                        t.Width = 550;
                        t.Height = 800;
                        TableRow tr = new TableRow();
                        TableCell tc = new TableCell();
                        tc.ColumnSpan = 7;
                        tc.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        Label lbl = new Label();
                        lbl.Text = "戒菸服務人員訓練課程時數(繼續教育)";
                        tc.Controls.Add(lbl);
                        tr.Cells.Add(tc);
                        t.Rows.Add(tr);
                        TableRow ts = new TableRow();
                        TableCell st = new TableCell();
                        st.ColumnSpan = 7;
                        st.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        Label lblst = new Label();
                        lblst.Text = "醫事人員戒菸服務訓練系統";
                        st.Controls.Add(lblst);
                        ts.Cells.Add(st);
                        t.Rows.Add(ts);
                        TableRow tr1 = new TableRow();
                        TableCell tc1 = new TableCell();
                        Label lbl1 = new Label();
                        lbl1.Text = "姓名：" + UserName;
                        tc1.ColumnSpan = 4;
                        tc1.Controls.Add(lbl1);
                        TableCell tc2 = new TableCell();
                        Label lbl2 = new Label();
                        lbl2.Text = "列印日期：" + DateTime.Now.ToShortDateString();
                        tc2.ColumnSpan = 3;
                        tc2.Controls.Add(lbl2);
                        tr1.Cells.Add(tc1);
                        tr1.Cells.Add(tc2);
                        t.Rows.Add(tr1);
                        TableRow tr2 = new TableRow();
                        TableCell tc3 = new TableCell();
                        tc3.ColumnSpan = 7;
                        Label lbl3 = new Label();
                        lbl3.Text = "身分證字號：" + UserPersonID;
                        tc3.Controls.Add(lbl3);
                        tr2.Cells.Add(tc3);
                        t.Rows.Add(tr2);

                        TableRow tr7 = new TableRow();
                        TableCell tc14 = new TableCell();
                        tc14.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        tc14.ColumnSpan = 3;
                        Label lbl14 = new Label();
                        lbl14.Text = "訓練類別";
                        tc14.Controls.Add(lbl14);
                        TableCell tc16 = new TableCell();
                        tc16.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        tc16.ColumnSpan = 2;
                        Label lbl16 = new Label();
                        lbl16.Text = "已取得時數";
                        tc16.Controls.Add(lbl16);
                        TableCell tc17 = new TableCell();
                        tc17.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        tc17.ColumnSpan = 2;
                        Label lbl17 = new Label();
                        lbl17.Text = "目標時數";
                        tc17.Controls.Add(lbl17);
                        tr7.Cells.Add(tc14); /*tr7.Cells.Add(tc15);*/ tr7.Cells.Add(tc16); tr7.Cells.Add(tc17);
                        t.Rows.Add(tr7);


                        DataTable CoursePlanName = Utility.getECoursePlan(PersonSNO, EPClassSNO);
                        if (CoursePlanName != null)
                        {
                            for (int i = 0; i < CoursePlanName.Rows.Count; i++)
                            {
                                TableRow tr5 = new TableRow();
                                TableCell tc9 = new TableCell();
                                tc9.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                tc9.ColumnSpan = 3;
                                Label lbl9 = new Label();
                                lbl9.Text = CoursePlanName.Rows[i]["PlanName"].ToString();
                                tc9.Controls.Add(lbl9);
                                TableCell tc11 = new TableCell();
                                tc11.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                tc11.ColumnSpan = 2;
                                Label lbl11 = new Label();
                                lbl11.Text = CoursePlanName.Rows[i]["PClassTotalHr"].ToString();
                                tc11.Controls.Add(lbl11);
                                TableCell tc12 = new TableCell();
                                tc12.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                tc12.ColumnSpan = 2;
                                Label lbl12 = new Label();
                                lbl12.Text = CoursePlanName.Rows[i]["sumHours"].ToString(); 
                                tc12.Controls.Add(lbl12);
                                tr5.Cells.Add(tc9); tr5.Cells.Add(tc11); tr5.Cells.Add(tc12);
                                t.Rows.Add(tr5);
                            }
                        }
                        else
                        {
                            TableRow tr_n = new TableRow();
                            TableCell tc_n = new TableCell();
                            tc_n.ColumnSpan = 4;
                            tc_n.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                            Label lbl_n = new Label();
                            lbl_n.Text = "無";
                            tc_n.Controls.Add(lbl_n);
                            tr_n.Cells.Add(tc_n);
                            t.Rows.Add(tr_n);
                        }

                        TableRow tr10 = new TableRow();
                        TableCell tc23 = new TableCell();
                        tc23.ColumnSpan = 7;
                        tc23.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        Label lbl23 = new Label();
                        lbl23.Text = "繼續教育課程時數明細";
                        tc23.Controls.Add(lbl23);
                        tr10.Cells.Add(tc23);
                        t.Rows.Add(tr10);

                        TableRow tr11 = new TableRow();
                        TableCell tc24 = new TableCell();
                        tc24.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        Label lbl24 = new Label();
                        lbl24.Text = "課程名稱";
                        tc24.Controls.Add(lbl24);
                        TableCell tc25 = new TableCell();
                        tc25.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        Label lbl25 = new Label();
                        lbl25.Text = "上課方式";
                        tc25.Controls.Add(lbl25);
                        TableCell tc26 = new TableCell();
                        tc26.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        Label lbl26 = new Label();
                        lbl26.Text = "時數";
                        tc26.Controls.Add(lbl26);
                        TableCell tc27 = new TableCell();
                        tc27.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        Label lbl27 = new Label();
                        lbl27.Text = "上傳時間";
                        tc27.Controls.Add(lbl27);
                        TableCell tc40 = new TableCell();
                        tc40.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        Label lbl40 = new Label();
                        lbl40.Text = "開課單位";
                        tc40.Controls.Add(lbl40);
                        TableCell tc41 = new TableCell();
                        tc41.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        Label lbl41 = new Label();
                        lbl41.Text = "上課日期";
                        tc41.Controls.Add(lbl41);
                        TableCell tc42 = new TableCell();
                        tc42.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                        Label lbl42 = new Label();
                        lbl42.Text = "備註";
                        tc42.Controls.Add(lbl42);
                        tr11.Cells.Add(tc24); tr11.Cells.Add(tc25); tr11.Cells.Add(tc26); tr11.Cells.Add(tc27); tr11.Cells.Add(tc40); tr11.Cells.Add(tc41); tr11.Cells.Add(tc42);
                        t.Rows.Add(tr11);
                        DataTable ScoreDetail = Utility.getExScoreDetail(UserPersonID, EPClassSNO, EPClassSNO_C);
                        DataTable OnlineScoreDetail = Utility.getExOnlineScoreDetail(UserPersonID, PersonSNO, EPClassSNO, EPClassSNO_C);
                        //DataTable ScoreDetail = Utility.getScoreDetail(PersonSNO, EPClassSNO);
                        if (ScoreDetail != null && OnlineScoreDetail != null)
                        {
                            for (int i = 0; i < OnlineScoreDetail.Rows.Count; i++)
                            {
                                TableRow tr5 = new TableRow();
                                TableCell tc9 = new TableCell();
                                tc9.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl9 = new Label();
                                lbl9.Text = OnlineScoreDetail.Rows[i]["CourseName"].ToString();
                                tc9.Controls.Add(lbl9);
                                TableCell tc10 = new TableCell();
                                tc10.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl10 = new Label();
                                lbl10.Text = OnlineScoreDetail.Rows[i]["Ctype"].ToString();
                                tc10.Controls.Add(lbl10);
                                TableCell tc11 = new TableCell();
                                tc11.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl11 = new Label();
                                lbl11.Text = OnlineScoreDetail.Rows[i]["CHour"].ToString();
                                tc11.Controls.Add(lbl11);
                                TableCell tc12 = new TableCell();
                                tc12.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl12 = new Label();
                                lbl12.Text = OnlineScoreDetail.Rows[i]["上課日期"].ToString();
                                tc12.Controls.Add(lbl12);
                                TableCell tc13 = new TableCell();
                                tc13.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl13 = new Label();
                                lbl13.Text = "-";
                                tc13.Controls.Add(lbl13);
                                TableCell tc15 = new TableCell();
                                tc15.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl15 = new Label();
                                lbl15.Text = OnlineScoreDetail.Rows[i]["上課日期"].ToString();
                                tc15.Controls.Add(lbl15);
                                TableCell tc18 = new TableCell();
                                tc18.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl18 = new Label();
                                lbl18.Text = OnlineScoreDetail.Rows[i]["備註"].ToString();
                                tc18.Controls.Add(lbl18);
                                tr5.Cells.Add(tc9); tr5.Cells.Add(tc10); tr5.Cells.Add(tc11); tr5.Cells.Add(tc12); tr5.Cells.Add(tc13); tr5.Cells.Add(tc15); tr5.Cells.Add(tc18);
                                t.Rows.Add(tr5);
                            }
                            for (int i = 0; i < ScoreDetail.Rows.Count; i++)
                            {
                                TableRow tr5 = new TableRow();
                                TableCell tc9 = new TableCell();
                                tc9.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl9 = new Label();
                                lbl9.Text = ScoreDetail.Rows[i]["CourseName"].ToString();
                                tc9.Controls.Add(lbl9);
                                TableCell tc10 = new TableCell();
                                tc10.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl10 = new Label();
                                lbl10.Text = ScoreDetail.Rows[i]["授課方式"].ToString();
                                tc10.Controls.Add(lbl10);
                                TableCell tc11 = new TableCell();
                                tc11.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl11 = new Label();
                                lbl11.Text = ScoreDetail.Rows[i]["Integral"].ToString();
                                tc11.Controls.Add(lbl11);
                                TableCell tc12 = new TableCell();
                                tc12.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl12 = new Label();
                                lbl12.Text = ScoreDetail.Rows[i]["CreateDT"].ToString();
                                tc12.Controls.Add(lbl12);
                                TableCell tc13 = new TableCell();
                                tc13.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl13 = new Label();
                                lbl13.Text = ScoreDetail.Rows[i]["Unit"].ToString();
                                tc13.Controls.Add(lbl13);
                                TableCell tc15 = new TableCell();
                                tc15.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl15 = new Label();
                                lbl15.Text = ScoreDetail.Rows[i]["CDate"].ToString();
                                tc15.Controls.Add(lbl15);
                                TableCell tc18 = new TableCell();
                                tc18.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl18 = new Label();
                                lbl18.Text = ScoreDetail.Rows[i]["備註"].ToString();
                                tc18.Controls.Add(lbl18);
                                tr5.Cells.Add(tc9); tr5.Cells.Add(tc10); tr5.Cells.Add(tc11); tr5.Cells.Add(tc12); tr5.Cells.Add(tc13); tr5.Cells.Add(tc15); tr5.Cells.Add(tc18);
                                t.Rows.Add(tr5);
                            }
                        }else if (ScoreDetail != null && OnlineScoreDetail == null)
                        {
                            for (int i = 0; i < ScoreDetail.Rows.Count; i++)
                            {
                                TableRow tr5 = new TableRow();
                                TableCell tc9 = new TableCell();
                                tc9.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl9 = new Label();
                                lbl9.Text = ScoreDetail.Rows[i]["CourseName"].ToString();
                                tc9.Controls.Add(lbl9);
                                TableCell tc10 = new TableCell();
                                tc10.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl10 = new Label();
                                lbl10.Text = ScoreDetail.Rows[i]["授課方式"].ToString();
                                tc10.Controls.Add(lbl10);
                                TableCell tc11 = new TableCell();
                                tc11.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl11 = new Label();
                                lbl11.Text = ScoreDetail.Rows[i]["Integral"].ToString();
                                tc11.Controls.Add(lbl11);
                                TableCell tc12 = new TableCell();
                                tc12.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl12 = new Label();
                                lbl12.Text = ScoreDetail.Rows[i]["CreateDT"].ToString();
                                tc12.Controls.Add(lbl12);
                                TableCell tc13 = new TableCell();
                                tc13.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl13 = new Label();
                                lbl13.Text = ScoreDetail.Rows[i]["Unit"].ToString();
                                tc13.Controls.Add(lbl13);
                                TableCell tc15 = new TableCell();
                                tc15.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl15 = new Label();
                                lbl15.Text = ScoreDetail.Rows[i]["CDate"].ToString();
                                tc15.Controls.Add(lbl15);
                                TableCell tc18 = new TableCell();
                                tc18.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl18 = new Label();
                                lbl18.Text = ScoreDetail.Rows[i]["備註"].ToString();
                                tc18.Controls.Add(lbl18);
                                tr5.Cells.Add(tc9); tr5.Cells.Add(tc10); tr5.Cells.Add(tc11); tr5.Cells.Add(tc12); tr5.Cells.Add(tc13); tr5.Cells.Add(tc15); tr5.Cells.Add(tc18);
                                t.Rows.Add(tr5);
                            }
                        }
                        else if (ScoreDetail == null && OnlineScoreDetail != null)
                        {
                            for (int i = 0; i < OnlineScoreDetail.Rows.Count; i++)
                            {
                                TableRow tr5 = new TableRow();
                                TableCell tc9 = new TableCell();
                                tc9.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl9 = new Label();
                                lbl9.Text = OnlineScoreDetail.Rows[i]["CourseName"].ToString();
                                tc9.Controls.Add(lbl9);
                                TableCell tc10 = new TableCell();
                                tc10.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl10 = new Label();
                                lbl10.Text = OnlineScoreDetail.Rows[i]["Ctype"].ToString();
                                tc10.Controls.Add(lbl10);
                                TableCell tc11 = new TableCell();
                                tc11.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl11 = new Label();
                                lbl11.Text = OnlineScoreDetail.Rows[i]["CHour"].ToString();
                                tc11.Controls.Add(lbl11);
                                TableCell tc12 = new TableCell();
                                tc12.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl12 = new Label();
                                lbl12.Text = OnlineScoreDetail.Rows[i]["上課日期"].ToString();
                                tc12.Controls.Add(lbl12);
                                TableCell tc13 = new TableCell();
                                tc13.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl13 = new Label();
                                lbl13.Text = "-";
                                tc13.Controls.Add(lbl13);
                                TableCell tc15 = new TableCell();
                                tc15.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl15 = new Label();
                                lbl15.Text = OnlineScoreDetail.Rows[i]["上課日期"].ToString();
                                tc15.Controls.Add(lbl15);
                                TableCell tc18 = new TableCell();
                                tc18.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                                Label lbl18 = new Label();
                                lbl18.Text = OnlineScoreDetail.Rows[i]["備註"].ToString();
                                tc18.Controls.Add(lbl18);
                                tr5.Cells.Add(tc9); tr5.Cells.Add(tc10); tr5.Cells.Add(tc11); tr5.Cells.Add(tc12); tr5.Cells.Add(tc13); tr5.Cells.Add(tc15); tr5.Cells.Add(tc18);
                                t.Rows.Add(tr5);
                            }
                        }
                        else
                        {
                            TableRow tr_n = new TableRow();
                            TableCell tc_n = new TableCell();
                            tc_n.ColumnSpan = 4;
                            tc_n.Style[HtmlTextWriterStyle.TextAlign] = "center"; //置中
                            Label lbl_n = new Label();
                            lbl_n.Text = "無";
                            tc_n.Controls.Add(lbl_n);
                            tr_n.Cells.Add(tc_n);
                            t.Rows.Add(tr_n);
                        }





                        t.RenderControl(new HtmlTextWriter(sw));

                        string html = sw.ToString();
                        Response.Write(sw.ToString());
                        Response.End();
                    }

                }
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
    public string getPName(string PersonSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = @"Select * from Person where PersonSNO=@PersonSNO";
        aDict.Add("PersonSNO", PersonSNO);
        DataTable ObjDT = objDH.queryData(sql, aDict);
        return ObjDT.Rows[0]["PName"].ToString();
    }
}