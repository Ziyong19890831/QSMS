using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_System : System.Web.UI.Page
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
            bindData(1);
        }
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String id = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("id", id);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete System Where SYSTEMSNO=@id", aDict);
        btnPage_Click(sender, e);
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
            
                 SELECT
                 	ROW_NUMBER () OVER (ORDER BY N.SYSTEM_ID DESC) AS ROW_NO,
                 	N.*,P.PName AS CUNAME,PP.PName AS MUNAME
                 FROM
                 	SYSTEM N
                 LEFT JOIN Person P ON P.PersonSNO = N.CreateUserID
                 LEFT JOIN Person PP ON PP.PersonSNO = N.ModifyUserID
                 WHERE
                 	1 = 1

        ";
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();

        if (!String.IsNullOrEmpty(txt_SysName.Text))
        {
            sql += " And SYSTEM_NAME  Like '%' + @SYSTEM_NAME + '%' ";
            aDict.Add("SYSTEM_NAME", txt_SysName.Text);
        }

        if (!String.IsNullOrEmpty(txt_SysID.Text))
        {
            sql += @" and SYSTEM_ID = @SYSTEM_ID";
            aDict.Add("SYSTEM_ID", txt_SysID.Text);
        }
        


        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Notice.DataSource = objDT.DefaultView;
        gv_Notice.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);

        if (objDT.Rows.Count <= 0)
        {
            lbl_msg.Visible = true;
        }
        else { lbl_msg.Visible = false; }
    }

   

    public static void setNoticeClass(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY NoticeCSNO) as ROW_NO,NoticeCSNO,Name  FROM NoticeClass", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }
}