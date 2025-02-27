using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Notice : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    int viewrole = 1;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null)
        {
            userInfo = (UserInfo)Session["QSMS_UserInfo"];
     
        } 
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            setNoticeClass(ddl_Class, "請選擇類型");
            //setClassSystem(ddl_SystemName, "請選擇系統");
            bindData(1);
            bindData_his(1);
            //bindDataWord(1);

        }
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String id = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("id", id);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete Notice Where NoticeSNO=@id", aDict);
        Response.Write("<script>alert('刪除成功!') </script>");
        btnPage_Click(sender, e);
        bindData_his(1);
        return;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
        bindData_his(1);
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
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String sql = @"
     
            select ROW_NUMBER() OVER (ORDER BY OrderSeq DESC, N.CreateDT DESC ) as ROW_NO,
                NoticeSNO, Title, SDate, Edate, P.PName, N.CreateDT, C.Name as ClassName, S.SYSTEM_NAME, OrderSeq,N.NoticeCSNO
            from Notice N
                LEFT JOIN NoticeClass C on N.NoticeCSNO=C.NoticeCSNO
			    LEFT JOIN Person P on N.CreateUserID=P.PersonSNO
                LEFT JOIN Organ O ON O.OrganSNO = p.OrganSNO
                LEFT JOIN Role R ON R.RoleSNO = p.RoleSNO
            LEFT JOIN SYSTEM S on N.SYSTEM_ID=S.SYSTEM_ID
			WHERE 1=1 and EDate > GETDATE()  and N.NoticeCSNO <> 9

        ";
        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(aDict, userInfo);
        if (userInfo.RoleSNO == "18")//開課單位只能檢視自己開的
        {
            sql += @" and N.CreateUserID=@CreateUserID";
            aDict.Add("CreateUserID", userInfo.PersonSNO);
        }
        #endregion


        #region 查詢篩選區塊
        if (!String.IsNullOrEmpty(txt_searchTitle.Text))
        {
            sql += " And Title  Like '%' + @Title + '%' ";
            aDict.Add("Title", txt_searchTitle.Text);
        }

        if (!String.IsNullOrEmpty(txt_searchDateStart.Text))
        {
            sql += @" and SDate>=@StartSearch";
            aDict.Add("StartSearch", txt_searchDateStart.Text);
        }

        if (!String.IsNullOrEmpty(txt_searchDateEnd.Text))
        {
            sql += @" and  Edate<=@EndSearch";
            aDict.Add("EndSearch", txt_searchDateEnd.Text);
        }
        if (!string.IsNullOrEmpty(ddl_Class.SelectedValue))
        {
            sql += " And C.NoticeCSNO=@NoticeCSNO ";
            aDict.Add("NoticeCSNO", ddl_Class.SelectedValue);
        }
        //if (!String.IsNullOrEmpty(ddl_SystemName.SelectedValue))
        //{
        //    sql += " And N.SYSTEM_ID=@SYSTEM_ID ";
        //    aDict.Add("SYSTEM_ID", ddl_SystemName.SelectedValue);
        //}
        #endregion

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Notice.DataSource = objDT.DefaultView;
        gv_Notice.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }
    protected void bindDataWord(int page)
    {
        if (page < 1) page = 1;
        int pageRecord = 10;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        String sql = @"
           With NOX as(
            select ROW_NUMBER() OVER (ORDER BY OrderSeq DESC, N.CreateDT DESC ) as ROW_NO,
                NoticeSNO, Title, SDate, Edate, P.PName, N.CreateDT, C.Name as ClassName, S.SYSTEM_NAME, OrderSeq,N.NoticeCSNO
            from Notice N
                LEFT JOIN NoticeClass C on N.NoticeCSNO=C.NoticeCSNO
			    LEFT JOIN Person P on N.CreateUserID=P.PersonSNO
                LEFT JOIN Organ O ON O.OrganSNO = p.OrganSNO
                LEFT JOIN Role R ON R.RoleSNO = p.RoleSNO
                LEFT JOIN SYSTEM S on N.SYSTEM_ID=S.SYSTEM_ID";
        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(aDict, userInfo);
        if (userInfo.RoleSNO == "18")//開課單位只能檢視自己開的
        {
            sql += @" and N.CreateUserID=@CreateUserID";
            aDict.Add("CreateUserID", userInfo.PersonSNO);
        }
        #endregion
        sql += @"
			WHERE 1=1 and EDate > GETDATE()
			)
			Select ROW_NUMBER() OVER (ORDER BY OrderSeq DESC, CreateDT DESC ) as ROW_NO, * From NOX where 1=1 
        ";

        #region 查詢篩選區塊
        if (!String.IsNullOrEmpty(txt_searchTitle.Text))
        {
            sql += " And Title  Like '%' + @Title + '%' ";
            aDict.Add("Title", txt_searchTitle.Text);
        }

        if (!String.IsNullOrEmpty(txt_searchDateStart.Text))
        {
            sql += @" and SDate>=@StartSearch";
            aDict.Add("StartSearch", txt_searchDateStart.Text);
        }

        if (!String.IsNullOrEmpty(txt_searchDateEnd.Text))
        {
            sql += @" and  Edate<=@EndSearch";
            aDict.Add("EndSearch", txt_searchDateEnd.Text);
        }
        if (!string.IsNullOrEmpty(ddl_Class.SelectedValue))
        {
            sql += " And NOX.NoticeCSNO=@NoticeCSNO ";
            aDict.Add("NoticeCSNO", ddl_Class.SelectedValue);
        }
        //if (!String.IsNullOrEmpty(ddl_SystemName.SelectedValue))
        //{
        //    sql += " And N.SYSTEM_ID=@SYSTEM_ID ";
        //    aDict.Add("SYSTEM_ID", ddl_SystemName.SelectedValue);
        //}
        #endregion


        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_word.DataSource = objDT.DefaultView;
        gv_word.DataBind();
        ltl_PageNumber_word.Text = Utility.showPageNumber1(objDT.Rows.Count, page, pageRecord);

    }
    protected void bindData_his(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;

        String sql = @"
            select ROW_NUMBER() OVER (ORDER BY OrderSeq DESC, N.CreateDT DESC ) as ROW_NO,
                NoticeSNO, Title, SDate, Edate, P.PName, N.CreateDT, C.Name as ClassName, S.SYSTEM_NAME, OrderSeq,N.NoticeCSNO
            from Notice N
                LEFT JOIN NoticeClass C on N.NoticeCSNO=C.NoticeCSNO
			    LEFT JOIN Person P on N.CreateUserID=P.PersonSNO
                LEFT JOIN Organ O ON O.OrganSNO = p.OrganSNO
                LEFT JOIN Role R ON R.RoleSNO = p.RoleSNO
                LEFT JOIN SYSTEM S on N.SYSTEM_ID=S.SYSTEM_ID
			WHERE 1=1 And  EDate < GETDATE()
        ";
        Dictionary<string, object> aDict = new Dictionary<string, object>();


        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(aDict, userInfo);
        if (userInfo.RoleSNO == "18")//開課單位只能檢視自己開的
        {
            sql += @" and N.CreateUserID=@CreateUserID";
            aDict.Add("CreateUserID", userInfo.PersonSNO);
        }
        #endregion


        #region 查詢篩選區塊
        if (!String.IsNullOrEmpty(txt_searchTitle.Text))
        {
            sql += " And Title  Like '%' + @Title + '%' ";
            aDict.Add("Title", txt_searchTitle.Text);
        }

        if (!String.IsNullOrEmpty(txt_searchDateStart.Text))
        {
            sql += @" and SDate>=@StartSearch";
            aDict.Add("StartSearch", txt_searchDateStart.Text);
        }

        if (!String.IsNullOrEmpty(txt_searchDateEnd.Text))
        {
            sql += @" and  Edate<=@EndSearch";
            aDict.Add("EndSearch", txt_searchDateEnd.Text);
        }

        if (!string.IsNullOrEmpty(ddl_Class.SelectedValue))
        {
            sql += " And N.NoticeCSNO=@NoticeCSNO ";
            aDict.Add("NoticeCSNO", ddl_Class.SelectedValue);
        }
        //if (!String.IsNullOrEmpty(ddl_SystemName.SelectedValue))
        //{
        //    sql += " And N.SYSTEM_ID=@SYSTEM_ID ";
        //    aDict.Add("SYSTEM_ID", ddl_SystemName.SelectedValue);
        //}
        #endregion

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Notice_his.DataSource = objDT.DefaultView;
        gv_Notice_his.DataBind();
        ltl_PageNumber_his.Text = Utility.showPageNumber2(objDT.Rows.Count, page, pageRecord);
    }
    public static void setNoticeClass(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY NoticeCSNO) as ROW_NO,NoticeCSNO,Name  FROM NoticeClass where NoticeCSNO <> 9", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }
    public static void setClassSystem(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER(ORDER BY SYSTEMSNO) as ROW_NO, SYSTEMSNO, SYSTEM_ID, SYSTEM_NAME FROM SYSTEM where ISEnable>0 ", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }
    protected void btnPage_word_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page_word.Value, out page);
        bindDataWord(page);
    }
    protected void btnPage_his_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page_his.Value, out page);
        bindData_his(page);
    }

}