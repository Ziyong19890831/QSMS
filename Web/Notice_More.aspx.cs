using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Notice : System.Web.UI.Page
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
            setClassCode(ddl_Notice_Class, "----請選擇----");
            setClassSystem(ddl_SystemName, "---請選擇系統---", userInfo);
            bindData(1);
        }
    }

    protected void bindData(int page)
    {
        if (page < 1) page = 1;
        int pageRecord = 10;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        String sql = @"
        SELECT ROW_NUMBER() OVER (ORDER BY -OrderSeq DESC, SDate DESC ) ROW_NO, S.SYSTEM_NAME , NoticeSNO, Title, SDate, EDate, N.CreateDT, OrderSeq, C.Name as ClassName
        from Notice N
        INNER JOIN NoticeClass C on N.NoticeCSNO=C.NoticeCSNO
        INNER JOIN SYSTEM S on N.SYSTEM_ID=S.SYSTEM_ID ";

        if (!String.IsNullOrEmpty(txtSearch.Value))
        {
            sql += " And Title Like '%' + @Title + '%' ";
            aDict.Add("Title", txtSearch.Value);
        }
        if (!String.IsNullOrEmpty(ddl_Notice_Class.SelectedValue))
        {
            sql += " And N.NoticeCSNO=@NoticeCSNO ";
            aDict.Add("NoticeCSNO", ddl_Notice_Class.SelectedValue);
        }
        if (userInfo != null)
        {
            if (!String.IsNullOrEmpty(ddl_SystemName.SelectedValue))
            {
                if (ddl_SystemName.SelectedValue == "S00")
                {
                    sql += @" LEFT JOIN PersonD PD on PD.SYSTEM_ID=S.SYSTEM_ID 
                      Where EDate>=GETDATE() AND  S.SYSTEM_ID = 'S00' ";
                    aDict.Add("SYSTEM_ID", ddl_SystemName.SelectedValue);
                    aDict.Add("PersonID", userInfo.PersonID);
                }
                else
                {
                    sql += @" LEFT JOIN PersonD PD on PD.SYSTEM_ID=S.SYSTEM_ID 
                      Where EDate>=GETDATE() AND PD.PersonID=@PersonID And S.SYSTEM_ID=@SYSTEM_ID ";
                    aDict.Add("SYSTEM_ID", ddl_SystemName.SelectedValue);
                    aDict.Add("PersonID", userInfo.PersonID);
                }
            }
            else
            {
                sql += @" LEFT JOIN PersonD PD on PD.SYSTEM_ID=S.SYSTEM_ID 
                      Where EDate>=GETDATE() AND PD.PersonID=@PersonID or S.SYSTEM_ID = 'S00' ";
                aDict.Add("PersonID", userInfo.PersonID);
            }
        }
        else
        {
            sql += @" Where EDate>=GETDATE() AND S.SYSTEM_ID='S00' ";
        }
        if (userInfo == null)
        {
            sql += " or S.SYSTEM_ID = 'S00' ";
        }

        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        rpt_NoticeMore.DataSource = objDT.DefaultView;
        rpt_NoticeMore.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);

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

    public static void setClassCode(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER(ORDER BY NoticeCSNO) as ROW_NO, NoticeCSNO, Name  FROM NoticeClass", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }
    public static void setClassSystem(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null, UserInfo userInfo = null)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        //沒登入只能看S00(預保)
        if (userInfo == null)
        {
            DataTable objDT = objDH.queryData(@"SELECT ROW_NUMBER() OVER(ORDER BY SYSTEMSNO) as ROW_NO, SYSTEMSNO, S.SYSTEM_ID, SYSTEM_NAME
                                            FROM SYSTEM S
                                            where ISEnable > 0 AND SYSTEM_ID='S00'", null);
            ddl.DataSource = objDT;
            ddl.DataBind();
            ddl.Visible = false;
        }
        //根據使用者能觀看的權限給予能選的系統
        else
        {
            aDict.Add("PersonID", userInfo.PersonID);
            DataTable objDT = objDH.queryData(@"SELECT ROW_NUMBER() OVER(ORDER BY SYSTEMSNO) as ROW_NO, SYSTEMSNO, S.SYSTEM_ID, SYSTEM_NAME, PD.PersonID
                                            FROM SYSTEM S
                                            LEFT JOIN PersonD PD on PD.SYSTEM_ID=S.SYSTEM_ID
                                            where (PersonID=@PersonID AND ISEnable > 0) or (S.SYSTEM_ID='S00')", aDict);
            ddl.Visible = true;
            ddl.DataSource = objDT;
            ddl.DataBind();
        }

        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }

    }
}