using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_ToolkitsBackStage : System.Web.UI.Page
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
            setStage(ddl_stage, "請選擇");
            setStageClass(ddl_stageClass, "請選擇");
            Utility.setTkType(ddl_TkType, "請選擇");
            bindData(1);
        }
    }

    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        String sql = @"
             Select ROW_NUMBER() OVER (ORDER BY TkSNO DESC ) ROW_NO, TkSNO , C1.MVal '期別',C2.Mval '適用性',C3.MVal '科別'
            ,tk.tkURL_Pic,tk.stage
            ,tk.stageClass,tk.Extension,tk.TkName,tk.TKURL,convert(varchar,tk.CreateDT,111) '上傳日期',tk.stageClassName,tk.stageName,tk.TktypeName,Dcount
                from Toolkits tk
                Left Join Config C1 On C1.PVal=tk.Stage and C1.PGroup='Stage'
                Left Join Config C2 On C2.PVal=tk.StageClass and C2.PGroup='StageClass'
               Left Join Config C3 On C3.PVal=tk.TKType and C3.PGroup='TKType'
                where 1 = 1 
        ";

        Dictionary<string, object> wDict = new Dictionary<string, object>();
        if (!String.IsNullOrEmpty(ddl_stage.SelectedValue))
        {
            sql += " And tk.stage Like '%' + @stage + '%' ";
            wDict.Add("stage", ddl_stage.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_stageClass.SelectedValue))
        {
            sql += " And tk.stageClass Like '%' + @stageClass + '%' ";
            wDict.Add("stageClass", ddl_stageClass.SelectedValue);
        }
        if (!String.IsNullOrEmpty(txt_fileName.Text))
        {
            sql += " And tk.TkName  Like '%' + @TkName + '%' ";
            wDict.Add("TkName", txt_fileName.Text);
        }
        if (!String.IsNullOrEmpty(ddl_TkType.SelectedValue))
        {
            sql += " And C3.PVal Like '%' + @TKtype + '%' ";
            wDict.Add("TKtype", ddl_TkType.SelectedValue);
        }
        sql += " Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Course.DataSource = objDT.DefaultView;
        gv_Course.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
        ReportInit(objDT);
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

    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String TkSNO = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("TkSNO", TkSNO);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete [Toolkits] Where TkSNO=@TkSNO", aDict);
        Response.Write("<script>alert('刪除成功!') </script>");
        //objDH.executeNonQuery("Update Notice Set Title='abc' Where id=@id", aDict)
        btnPage_Click(sender, e);
        return;
    }
    public static void setStage(DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select PVal,MVal from Config C where C.PGroup='Stage'", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
    public static void setStageClass(DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select PVal,MVal from Config C where C.PGroup='StageClass'", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        bindData(1);
    }

    protected void gv_Course_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            //要隱藏的欄位，如果前台filed隱藏會抓不到值，所以在RowCreate時才隱藏
            e.Row.Cells[0].Visible = false;

        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (gv_Course.Rows.Count == 0)
        {
            Response.Write("<script>alert('尚未查詢')</script>");
            return;
        }
        Utility.OpenExportWindows(this, ReportEnum.Toolkits.ToString());
    }

    private void ReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("TkName", "檔案名稱");
        _SetCol.Add("上傳日期", "上傳日期");
        _SetCol.Add("Dcount", "下載次數");
        _SetCol.Add("適用性", "適用性");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.Toolkits.ToString()] = _ExcelInfo;
    }
}