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
        if (Session["QSMS_UserInfo"] != null)
        {

            userInfo = (UserInfo)Session["QSMS_UserInfo"];
            //會員資料填補
            bool checkSession = Utility.CheckSession(userInfo);
            if (checkSession == false)
            {
                Response.Redirect("./Personnel_AE.aspx?Error=1");
            }
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            setClassCode(ddl_Notice_Class, "請選擇");
            Utility.setClassRoleName(dd2_RoleName, "---適用人員全選---");
            int PostBackPage = Request.QueryString["Page"] == null ? 1 : Convert.ToInt32(Request.QueryString["Page"]);
            int PostBackdata = Request.QueryString["data"] == null ? 1 : Convert.ToInt32(Request.QueryString["data"]);
            if (PostBackdata == 0)
            {
                bindData(PostBackPage);
            }
            else
            {
                bindData(1);
            }
            if (PostBackdata == 1)
            {
                bindDataWord(PostBackPage);
            }
            else
            {
                bindDataWord(1);
            }
            if (PostBackdata == 2)
            {
                bindData_his(PostBackPage);
            }
            else
            {
                bindData_his(1);
            }





        }
    }

    protected void bindData(int page)
    {
        if (page < 1) page = 1;
        int pageRecord = 10;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        String sql = "";
        sql += "SELECT Top 100";
        sql += "       ROW_NUMBER() OVER (ORDER BY OrderSeq DESC, N.CreateDT DESC ) ROW_NO, ";
        sql += "      Cast(NoticeSNO as varchar)+'&Data=0&Page=" + page + "' as NoticeSNO, Title, SDate, EDate, N.CreateDT, OrderSeq, C.Name as ClassName, ";
        sql += Utility.setSQL_RoleBindName("Notice_AE", "N.NoticeSNO");
        sql += "   from Notice N";
        sql += "       LEFT JOIN NoticeClass C on N.NoticeCSNO=C.NoticeCSNO";
        sql += "   where GETDATE() < EDate and Getdate() > SDate  and show=1 and C.[NoticeCSNO] <> 9";



        if (!String.IsNullOrEmpty(dd2_RoleName.SelectedValue))
        {
            sql += " and (select 1 from RoleBind RB where RB.CSNO=N.NoticeSNO and RB.TypeKey='Notice_AE' and RB.RoleSNO=@dd2_RoleName )=1";
            aDict.Add("dd2_RoleName", dd2_RoleName.SelectedValue);
        }
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

        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        rpt_NoticeMore.DataSource = objDT.DefaultView;
        rpt_NoticeMore.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);

    }
    protected void bindDataWord(int page)
    {
        if (page < 1) page = 1;
        int pageRecord = 10;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        String sql = @"
            SELECT Top 100 
                ROW_NUMBER() OVER (ORDER BY OrderSeq DESC, N.CreateDT DESC ) ROW_NO, 
                Cast(NoticeSNO as varchar)+'&Data=1&Page=" + page + "' as NoticeSNO, Title, SDate, EDate, N.CreateDT, OrderSeq, C.Name as ClassName,  " + Utility.setSQL_RoleBindName("Notice_AE", "N.NoticeSNO") + @"   from Notice N      LEFT JOIN NoticeClass C on N.NoticeCSNO=C.NoticeCSNO     where GETDATE() < EDate  and show=1 and C.[NoticeCSNO]=9
		";


        if (!String.IsNullOrEmpty(dd2_RoleName.SelectedValue))
        {
            sql += " and (select 1 from RoleBind RB where RB.CSNO=N.NoticeSNO and RB.TypeKey='Notice_AE' and RB.RoleSNO=@dd2_RoleName )=1";
            aDict.Add("dd2_RoleName", dd2_RoleName.SelectedValue);
        }
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

        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        rpt_Word.DataSource = objDT.DefaultView;
        rpt_Word.DataBind();
        ltl_PageNumber_word.Text = Utility.showPageNumber1(objDT.Rows.Count, page, pageRecord);

    }
    protected void bindData_his(int page)
    {
        if (page < 1) page = 1;
        int pageRecord = 10;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        String sql = @"
            SELECT Top 100 
                ROW_NUMBER() OVER (ORDER BY OrderSeq DESC, N.CreateDT DESC ) ROW_NO, 
                Cast(NoticeSNO as varchar)+'&Data=2&Page=" + page + "' as NoticeSNO, Title, SDate, EDate, N.CreateDT, OrderSeq, C.Name as ClassName,  " + Utility.setSQL_RoleBindName("Notice_AE", "N.NoticeSNO") + @"  from Notice N    LEFT JOIN NoticeClass C on N.NoticeCSNO=C.NoticeCSNO     where  GETDATE()  > EDate and show=1";


        if (!String.IsNullOrEmpty(dd2_RoleName.SelectedValue))
        {
            sql += " and (select 1 from RoleBind RB where RB.CSNO=N.NoticeSNO and RB.TypeKey='Notice_AE' and RB.RoleSNO=@dd2_RoleName )=1";
            aDict.Add("dd2_RoleName", dd2_RoleName.SelectedValue);
        }
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

        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        rpt_NoticeMore_his.DataSource = objDT.DefaultView;
        rpt_NoticeMore_his.DataBind();
        ltl_PageNumber_his.Text = Utility.showPageNumber2(objDT.Rows.Count, page, pageRecord);

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
    //protected void btnPagehis_Click(object sender, EventArgs e)
    //{
    //    int page = 1;
    //    int.TryParse(txt_Page.Value, out page);
    //    bindData_his(page);
    //}

    public static void setClassCode(DropDownList ddl, String DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER(ORDER BY NoticeCSNO) as ROW_NO, NoticeCSNO, Name  FROM NoticeClass where NoticeCSNO <> 9", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }


    protected void btnPage_his_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page_his.Value, out page);
        bindData_his(page);
    }

    protected void btnPage_word_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page_word.Value, out page);
        bindDataWord(page);
    }


    protected void btn_question_Click(object sender, EventArgs e)
    {

    }


}