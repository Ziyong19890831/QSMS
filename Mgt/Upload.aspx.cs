using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Upload : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    int viewrole = 1;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindData(1);
            setClassCode(ddl_Download_Class, "請選擇分類");
            Utility.setClassRoleName(ddl_setRoleName, "---適用人員全選---");
        }
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String id = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("id", id);
        DataHelper objDH = new DataHelper();
        Utility.deleteDownloadDFolder(Server.MapPath("../Download"), id);
        objDH.executeNonQuery("Delete Download Where DLOADSNO=@id", aDict);
        btnPage_Click(sender, e);
        Response.Write("<script>alert('刪除成功!') </script>");
        return;
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

    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;

        String sql = @"
            select ROW_NUMBER() OVER (ORDER BY D.OrderSeq DESC, D.CreateDT Desc )as ROW_NO,
                D.OrderSeq,D.DLOADSNO,D.DLOADNAME,D.DLOADURL,D.ISENABLE,D.SYSTEM_ID,S.SYSTEM_NAME,
                DC.DLCNAME,D.CreateDT, " + Utility.setSQL_RoleBindName("Upload_AE", "D.DLOADSNO") + @"
            from Download D
                LEFT JOIN SYSTEM S on D.SYSTEM_ID=S.SYSTEM_ID
                LEFT JOIN DownloadClass DC on DC.DLCSNO=D.DLCSNO
                Left Join Person P ON P.PersonSNO=D.CreateUserID
				LEFT JOIN Organ O ON O.OrganSNO = p.OrganSNO
				LEFT JOIN Role R ON R.RoleSNO = p.RoleSNO
            Where D.ISENABLE=1 And R.IsAdmin=1
        ";
        Dictionary<string, object> aDict = new Dictionary<string, object>();


        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(aDict, userInfo);
        #endregion


        #region 查詢篩選區塊
        if (!String.IsNullOrEmpty(txt_searchTitle.Text))
        {
            sql += " And D.DLOADNAME Like '%' + @DLOADNAME + '%'";
            aDict.Add("DLOADNAME", txt_searchTitle.Text);
        }
        if (!String.IsNullOrEmpty(ddl_Download_Class.SelectedValue))
        {
            sql += " And D.DLCSNO =@DLCSNO";
            aDict.Add("DLCSNO", ddl_Download_Class.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_setRoleName.SelectedValue))
        {
            sql += " and (select 1 from RoleBind RB where RB.CSNO=D.DLOADSNO and RB.TypeKey='Upload_AE' and RB.RoleSNO=@ddl_setRoleName )=1";
            aDict.Add("ddl_setRoleName", ddl_setRoleName.SelectedValue);
        }
        #endregion

        sql += " ORDER BY D.OrderSeq DESC, D.CreateDT Desc";

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Upload.DataSource = objDT.DefaultView;
        gv_Upload.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

    protected void setClassCode(DropDownList ddl, String DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER(ORDER BY DLCSNO) as ROW_NO, DLCSNO, DLCNAME  FROM DownloadClass Where ISENABLE>0", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    protected int getFilesCount(string dirID)
    {
        int fc = 0;
        if (Directory.Exists(Server.MapPath("../Download") + "/" + dirID))
        {
            string[] files = Directory.GetFiles(Server.MapPath("../Download") + "/" + dirID);
            fc = files.Length;
        }
        return fc;
    }

}