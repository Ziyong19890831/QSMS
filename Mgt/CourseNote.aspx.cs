using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_CourseNote : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    int viewrole = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int page = 1;
            int.TryParse(txt_Page.Value, out page);
            GetNoteContent();
            bindData(page);
        }
    }

    protected void bindData(int page)
    {

        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;

        String sql = @"
                        SELECT 
                        ROW_NUMBER() OVER (ORDER BY A.RoleSNO ASC) ROW_NO, RoleSNO ,
                        A.RoleName , A.CourseFile 
                        FROM Role A 
                        WHERE A.IsAdmin = 0  
        ";
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();


        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Course.DataSource = objDT.DefaultView;
        gv_Course.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

    private void GetNoteContent()
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT a.PVal from Config a where a.PID ='CourseNoteContent'", aDict);
        txt_CourseTitle.Text = "";
        if (objDT.Rows.Count > 0)
        {
            txt_CourseTitle.Text = objDT.Rows[0]["PVal"].ToString();
        }
    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
    }

    //上傳檔案
    protected void btnupload_Click(object sender, EventArgs e)
    {
        FileUpload fuCOA = (FileUpload)gv_Course.FindControl("FileUpload1");
        string fileName = fuCOA.FileName;
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        string errorMessage = "";
        if (string.IsNullOrEmpty(txt_CourseTitle.Text)) errorMessage += "標題不得為空\\n";
        if (txt_CourseTitle.Text.Length > 200) errorMessage += "標題字數過多\\n";

        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        aDict.Add("Pval", txt_CourseTitle.Text);
        objDH.executeNonQuery("UPDATE Config SET PVal =@Pval where PID ='CourseNoteContent'", aDict);
        Response.Write("<script>alert('修改成功!');document.location.href='./CourseNote.aspx'; </script>");

    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        FileUpload fuploadFile = (FileUpload)gv_Course.FooterRow.FindControl("FileUpload1");
    }
}