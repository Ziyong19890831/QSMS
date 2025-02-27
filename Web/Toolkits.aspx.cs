using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Toolkits : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind(1);
            bindType(1);
            setStage(ddl_stage, "期程-請選擇");
            setStageClass(ddl_stageClass, "教材類型-請選擇");
            Utility.setTkType(ddl_TkType, "科別-請選擇");
            setStageClass(ddl_TkStageType, "教材類型-請選擇");
        }
    }
    protected void bind(int page)
    {
        if (page < 1) page = 1;
        int pageRecord = 10;
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String sql = @"
             Select ROW_NUMBER() OVER (ORDER BY tk.CreateDT DESC ) ROW_NO, C1.MVal '期別',C2.Mval '適用性',[TkSNO],
            tk.TkURL_Pic,tk.stage,tk.stageClass,tk.TkName,tk.Extension,tk.TKURL,convert(varchar,tk.CreateDT,111) '上傳日期' ,tk.stageClassName,tk.stageName,tk.TktypeName,tk.DCount
            from Toolkits tk
                Left Join Config C1 On C1.PVal=tk.Stage and C1.PGroup='Stage'
                Left Join Config C2 On C2.PVal=tk.StageClass and C2.PGroup='StageClass'
                where tk.isEnable=1 
        ";
        if (!String.IsNullOrEmpty(ddl_stage.SelectedValue))
        {
            sql += " And tk.stage Like '%' + @stage + '%' ";
            aDict.Add("stage", ddl_stage.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_stageClass.SelectedValue))
        {
            sql += " And tk.stageClass Like '%' + @stageClass + '%' ";
            aDict.Add("stageClass", ddl_stageClass.SelectedValue);
        }
        if (!String.IsNullOrEmpty(txt_fileName.Text))
        {
            sql += " And tk.TkName Like '%' + @TkName + '%' ";
            aDict.Add("TkName", txt_fileName.Text);
        }
        sql += " Order by tk.CreateDT DESC ";
        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);

        gv_toolkies.DataSource = objDT;
        gv_toolkies.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);

    }

    protected void bindType(int page)
    {
        if (page < 1) page = 1;
        int pageRecord = 10;
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String sql = @"
                Select ROW_NUMBER() OVER ( ORDER BY tk.CreateDT DESC ) ROW_NO,[TkSNO], C1.MVal '期別',C2.Mval '適用性',
                tk.TkURL_Pic,C3.MVal '科別',tk.stage,tk.stageClass,tk.TkName,tk.Extension,tk.TKURL,convert(varchar,tk.CreateDT,111) '上傳日期',tk.DCount ,tk.stageClassName,tk.stageName,tk.TktypeName,tk.tktype
                from Toolkits tk
                Left Join Config C1 On C1.PVal=tk.Stage and C1.PGroup='Stage'
                Left Join Config C2 On C2.PVal=tk.StageClass and C2.PGroup='StageClass'
                Left Join Config C3 ON C3.PVal=tk.tktype and C3.PGroup='TkType'
                where tk.isEnable=1 
        ";
        if (!String.IsNullOrEmpty(ddl_TkStageType.SelectedValue))
        {
            sql += " And tk.stageClass Like '%' + @stageClass + '%' ";
            aDict.Add("stageClass", ddl_TkStageType.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_TkType.SelectedValue))
        {
            sql += " And tk.tktype Like '%' + @TKtype + '%' ";
            aDict.Add("TKtype", ddl_TkType.SelectedValue);
        }
        if (!String.IsNullOrEmpty(txt_fileName_Type.Text))
        {
            sql += " And tk.TkName Like '%' + @TkName + '%' ";
            aDict.Add("TkName", txt_fileName_Type.Text);
        }
        sql += "  Order by tk.CreateDT DESC ";

        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);

        gv_toolkiesType.DataSource = objDT;
        gv_toolkiesType.DataBind();
        ltl_PageNumberType.Text = Utility.showPageNumber1(objDT.Rows.Count, page, pageRecord);

    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        bind(page);
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        bind(1);
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
 
    protected void downloadBook(string filename, Stream FileStream)
    {
        Byte[] Buf = new byte[FileStream.Length];
        FileStream.Read(Buf, 0, int.Parse(FileStream.Length.ToString()));
        FileStream.Close();

        //準備下載檔案 
        Response.ClearHeaders();
        Response.Clear();
        Response.Expires = 0;
        Response.Buffer = false;
        Response.ContentType = "Application/save-as";
        Response.Charset = "utf-8";
        //透過Header設定檔名 
        Response.AddHeader("Content-Disposition", "Attachment; filename=" + HttpUtility.UrlEncode(filename));
        Response.BinaryWrite(Buf);
        Response.End();
    }

    protected void gv_toolkies_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            //要隱藏的欄位，如果前台filed隱藏會抓不到值，所以在RowCreate時才隱藏
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[8].Visible = false;

        }
    }

    protected void gv_toolkies_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Button myButton = (Button)e.CommandSource;
        GridViewRow myRow = (GridViewRow)myButton.NamingContainer;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string URL = myRow.Cells[1].Text;
        string filename = myRow.Cells[4].Text;
        string TKSNO = myRow.Cells[8].Text;
        string extension;
        extension = Path.GetExtension(URL);
        aDict.Add("TkSNO", TKSNO);
        if (URL != "")
        {
            objDH.executeNonQuery("Update Toolkits set DCount=Dcount+1 where TkSNO=@TkSNO", aDict);
            Stream FileStream;
            FileStream = File.OpenRead(Server.MapPath(URL));
            downloadBook(filename + extension, FileStream);
           
        }
    }

    protected void btn_SeachType_Click(object sender, EventArgs e)
    {
        bindType(1);
    }

    protected void btnPageType_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_PageType.Value, out page);
        bindType(page);
    }

    protected void gv_toolkiesType_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            //要隱藏的欄位，如果前台filed隱藏會抓不到值，所以在RowCreate時才隱藏
            e.Row.Cells[1].Visible = false;

        }
    }

    protected void gv_toolkiesType_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void gv_toolkiesType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Button myButton = (Button)e.CommandSource;
        GridViewRow myRow = (GridViewRow)myButton.NamingContainer;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string URL = myRow.Cells[1].Text;
        string filename = myRow.Cells[4].Text;
        string TKSNO = myRow.Cells[8].Text;
        string extension;
        extension = Path.GetExtension(URL);
        aDict.Add("TkSNO", TKSNO);
        if (URL != "")
        {
            objDH.executeNonQuery("Update Toolkits set DCount=Dcount+1 where TkSNO=@TkSNO", aDict);
            Stream FileStream;
            FileStream = File.OpenRead(Server.MapPath(URL));
            downloadBook(filename + extension, FileStream);
          

        }
    }


    protected void btn_Link_Click(object sender, EventArgs e)
    {
        //Response.Redirect("https://forms.gle/ADrCd7gYXoiWdqWi8");
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        //bind(1);
    }
}