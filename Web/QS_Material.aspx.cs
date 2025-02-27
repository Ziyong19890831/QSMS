using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_QS_Material : System.Web.UI.Page
{
    UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Utility.setClassRoleName(dd2_RoleName, "---適用人員全選---");
            setClassCode(ddl_Download_Class, "---請選擇分類---");
            GetDownlaod(1);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetDownlaod(1);
    }

    protected void GetDownlaod(int page)
    {
        if (page < 1) page = 1;
        int pageRecord = 10;

        Dictionary<string, object> dic = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string strSqlQry = @"
            select ROW_NUMBER() OVER (ORDER BY D.OrderSeq DESC, D.CreateDT Desc ) as ROW_NO,
                D.DLOADURL, D.DLOADNAME, DC.DLCNAME, D.ISENABLE ,D.CreateDT,D.SYSTEM_ID,S.SYSTEM_NAME,
                " + Utility.setSQL_RoleBindName("Upload_AE", "D.DLOADSNO") + @",D.DLOADNote
            from Download D 
                  LEFT JOIN DownloadClass DC on DC.DLCSNO=D.DLCSNO
                  LEFT JOIN SYSTEM S on D.SYSTEM_ID=S.SYSTEM_ID 
            Where D.ISENABLE=1 and DC.DLCSNO <> 4
        ";

        if (!String.IsNullOrEmpty(ddl_Download_Class.SelectedValue))
        {
            strSqlQry += " And D.DLCSNO =@DLCSNO";
            dic.Add("DLCSNO", ddl_Download_Class.SelectedValue);
        }
        //if (!String.IsNullOrEmpty(dd2_RoleName.SelectedValue))
        //{
        //    strSqlQry += " And (select 1 from RoleBind RB where RB.CSNO=D.DLOADSNO and RB.TypeKey='Upload_AE' and RB.RoleSNO=@dd2_RoleName )=1";
        //    dic.Add("dd2_RoleName", dd2_RoleName.SelectedValue);
        //}
        if (txtSearch.Value != "")
        {
            strSqlQry += " And DLOADNAME Like '%' + @DLOADNAME + '%' ";
            dic.Add("DLOADNAME", txtSearch.Value);
        }

        strSqlQry += " ORDER BY D.OrderSeq DESC, D.CreateDT Desc";

        DataTable objDT = objDH.queryData(strSqlQry, dic);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        rpt_DLOAD.DataSource = objDT.DefaultView;
        rpt_DLOAD.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

    protected string getFiles(string dirID)
    {
        string fileList = "";
        if (Directory.Exists(Server.MapPath("../Download") + "/" + dirID))
        {
            string[] files = Directory.GetFiles(Server.MapPath("../Download") + "/" + dirID);
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fileInfo = new FileInfo(files[i]);
                fileList += "<a target='_blank' href='../Download/" + dirID + "/" + fileInfo.Name + "'><i class='fa fa-file'></i> " + fileInfo.Name + "</a><br/>";
            }
        }
        return fileList;
    }

    protected void setClassCode(DropDownList ddl, String DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER(ORDER BY DLCSNO) as ROW_NO, DLCSNO, DLCNAME  FROM DownloadClass Where ISENABLE>0 and DLCSNO<>4", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        GetDownlaod(page);
    }

}