using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text.pdf.events;

public partial class Mgt_CertificatePrint : System.Web.UI.Page
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
            bindData(1);
        }
    }

    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;

        string sql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY c.CertSNO) as ROW_NO 
	            , P.PName , c.CertID , ct.CTypeName , ctu.CUnitName
	            , CONVERT(char(10) ,  c.CertPublicDate ,121) CertPublicDate
	            , CONVERT(char(10) ,  c.CertStartDate ,121) CertStartDate
	            , CONVERT(char(10) ,  c.CertEndDate ,121) CertEndDate
	            , ct.CTypeSNO            
            FROM QS_Certificate c
                 JOIN QS_CertificateType ct ON ct.CTypeSNO = c.CTypeSNO
				 JOIN QS_CertificateUnit ctu ON ctu.CUnitSNO=c.CUnitSNO
                 JOIN Person P ON P.PersonID = c.PersonID
                 JOIN Organ O ON O.OrganSNO = P.OrganSNO
                 JOIN Role R ON R.RoleSNO = P.RoleSNO
        ";

        Dictionary<string, object> wDict = new Dictionary<string, object>();

        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(wDict, userInfo);
        #endregion


        #region 查詢篩選區塊
        if (!string.IsNullOrEmpty(ddl_CType.SelectedValue))
        {
            sql += " AND c.CTypeSNO = @CTypeSNO";
            wDict.Add("CTypeSNO", ddl_CType.SelectedValue);
        }
        #endregion

        sql += " Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Certificate.DataSource = objDT.DefaultView;
        gv_Certificate.DataBind();
        Session["CertificatePring"] = objDT;
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
    protected void btnPdf_Click(object sender, EventArgs e)
    {
        DataTable dataTable = null;
        dataTable = (DataTable)Session["CertificatePring"];
        try
        {

            MemoryStream ms = new MemoryStream();
            Document document = new Document(PageSize.A4.Rotate(), 50, 50, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();
            BaseFont bfChinese = BaseFont.CreateFont(@"C:\Windows\Fonts\kaiu.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font ChFont = new Font(bfChinese, 12);
            Font ChFont_blue = new Font(bfChinese, 22, Font.NORMAL, new BaseColor(51, 0, 153));
            Font ChFont_msg = new Font(bfChinese, 12, Font.NORMAL, BaseColor.RED);
            string imageFilePath = "C:/inetpub/wwwroot/Images/BG_A4.jpg";
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);
            //jpg.ScaleToFit(100%);
            jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
            jpg.SetAbsolutePosition(0, 0);


            foreach (DataRow row in dataTable.Rows)
            {
                document.NewPage();             
                //填寫表格
                //TextField tf = new TextField(writer, new Rectangle(67, 585, 140, 800), "cellTextBox");
                //events = new iTextSharp.text.pdf.events.FieldPositioningEvents(writer, tf.GetTextField());

                PdfPTable table = new PdfPTable(2);
                //欄位比列
                table.SetWidths(new int[] { 1, 3 });      
                //cell = new PdfPCell();           
                table.AddCell(SetCell("序號:", ChFont));
                table.AddCell(SetCell(row[0].ToString(), ChFont));
                table.AddCell(SetCell("學員名稱:", ChFont));
                table.AddCell(SetCell(row[1].ToString(), ChFont));
                table.AddCell(SetCell("證號:", ChFont));
                table.AddCell(SetCell(row[2].ToString(), ChFont));
                table.AddCell(SetCell("證書類型:", ChFont));
                table.AddCell(SetCell(row[3].ToString(), ChFont));
                table.AddCell(SetCell("發證單位:", ChFont));
                table.AddCell(SetCell(row[4].ToString(), ChFont));
                table.AddCell(SetCell("首發日期:", ChFont));
                table.AddCell(SetCell(row[5].ToString(), ChFont));
                table.AddCell(SetCell("公告日期:", ChFont));
                table.AddCell(SetCell(row[6].ToString(), ChFont));
                table.AddCell(SetCell("到期日期:", ChFont));
                table.AddCell(SetCell(row[7].ToString(), ChFont));
                
                document.Add(table);
                document.Add(jpg);
            }
            document.Close();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=CertificatePring.pdf");
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            Response.Close();
        }
        catch (Exception ex)
        {
            string script = "<script>alert('" + ex.Message + "');</script>";

        }
    }

    private PdfPCell SetCell(string msg, Font font, FieldPositioningEvents events = null, bool border = false, int height = 20)
    {
        PdfPCell cell = new PdfPCell(new Phrase(msg, font));
        if (events != null) cell.CellEvent = events;
        if (!border) cell.Border = 0;
        cell.FixedHeight = height;

        return cell;
    }

}