using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_TifmHis : System.Web.UI.Page
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
          
        }
    }



    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;

        String sql = @"            
             WITH Base AS (Select * from TafmHis)
            SELECT  ROW_NUMBER() OVER (ORDER BY base.身分證號) AS ROW_NO, * FROM Base
            where 1=1
        ";

        Dictionary<string, object> wDict = new Dictionary<string, object>();


        #region 權限篩選區塊
        //sql += Utility.setSQLAccess_ByRoleOrganType(wDict, userInfo);
        #endregion


        #region 查詢篩選區塊
        if (!string.IsNullOrEmpty(txt_Object.Text))
        {
            sql += " AND 主題名稱 Like '%' + @主題名稱 + '%' ";
            wDict.Add("主題名稱", txt_Object.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_Theme.Text))
        {
            sql += " AND 刊物名稱 Like '%' + @刊物名稱 + '%' ";
            wDict.Add("刊物名稱", txt_Theme.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " AND 身分證號= @身分證號 ";
            wDict.Add("身分證號", txt_PersonID.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_PName.Text))
        {
            sql += " AND 中文姓名= @中文姓名 ";
            wDict.Add("中文姓名", txt_PName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_DCNumber.Text))
        {
            sql += " AND 醫師證號= @醫師證號 ";
            wDict.Add("醫師證號", txt_DCNumber.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_CName.Text))
        {
            sql += " AND 專科暨訓練證照名稱= @專科暨訓練證照名稱 ";
            wDict.Add("專科暨訓練證照名稱", txt_CName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_CNumber.Text))
        {
            sql += " AND 專科暨訓練證照證號= @專科暨訓練證照證號 ";
            wDict.Add("專科暨訓練證照證號", txt_CNumber.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_SFinishedDate.Text))
        {
            sql += " AND 通訊課程日期起 >= @通訊課程日期起";
            wDict.Add("通訊課程日期起", txt_SFinishedDate.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_EFinishedDate.Text))
        {
            sql += " AND 通訊課程日期迄  <= @通訊課程日期迄";
            wDict.Add("通訊課程日期迄", txt_EFinishedDate.Text.Trim());
        }
        #endregion


        sql += " Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        if (objDT.Rows.Count < 1)
        {
            Response.Write("<script>alert('搜尋無資料')</script>");
            return;
        }
        ExportToExcel1(objDT, "歷史資料"+DateTime.Now.ToString("yyyyMMdd"));

        //設定匯出資料
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    }



    public void ExportToExcel1(DataTable dt, string fileName)
    {
        //將DataTable綁定到DataGird控件
        System.Web.UI.WebControls.DataGrid dg = new System.Web.UI.WebControls.DataGrid();
        dg.DataSource = dt.DefaultView;
        dg.AllowPaging = false;
        dg.HeaderStyle.BackColor = System.Drawing.Color.LightGray;
        dg.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        dg.HeaderStyle.Font.Bold = true;
        dg.DataBind();
        Response.Clear();
        Response.Buffer = true;

        //防止出現亂碼,加上這行可以防止在只有一行數據時出現亂碼
        Response.Write("<meta http-equiv=Content-Type content=text/html;charset=UTF-8>");
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8));
        Response.ContentType = "application/ms-excel";

        Response.Charset = "UTF-8";
        //指定編碼 防止中文亂碼現象
        Response.ContentEncoding = System.Text.Encoding.UTF8;

        //關閉控件的視圖狀態
        this.EnableViewState = false;

        //初始化HtmlWriter
        System.IO.StringWriter writer = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWriter = new System.Web.UI.HtmlTextWriter(writer);
        //將DataGird內容輸出到HtmlTextWriter對象中
        dg.RenderControl(htmlWriter);
        string outputStr = writer.ToString();
        //輸出
        Response.Write(outputStr);
        Response.Flush();
        Response.End();
    }
}