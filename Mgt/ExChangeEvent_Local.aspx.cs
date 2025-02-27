using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_ExChangeEvent_Local : System.Web.UI.Page
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
            //setEventClass(ddl_Class, "請選擇分類");
            bindData(1);
        }
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {

        LinkButton btn = (LinkButton)sender;
        string[] id = btn.CommandArgument.ToString().Split(new char[] { ',' });
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        //Utility.DeleteCalendra(id[1]);
        aDict.Add("id", id[0]);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete EventD Where EventSNO=@id", aDict);
        objDH.executeNonQuery("Delete Event Where EventSNO=@id", aDict);
        objDH.executeNonQuery("Delete EventBatch Where EventSNO=@id", aDict);
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
            SELECT ROW_NUMBER() OVER (ORDER BY E.CreateDT DESC) as ROW_NO,
                E.EventSNO,EC.ClassName,E.EventName,E.StartTime,E.EndTime,
                E.CPerosn,E.CountLimit,G.Mval as Class1,G.PVal as Class1Code,F.Mval as Class2,F.PVal as Class2Code,
                (Select count(1) From EventD ed Where ed.EventSNO=E.EventSNO) pCount,C.id
            FROM Event E
                LEFT JOIN EventClass EC on E.EventCSNO=EC.EventCSNO
                LEFT JOIN SYSTEM S on E.SYSTEM_ID=S.SYSTEM_ID
                Left Join Config G On E.class3=G.Pval and G.PGroup='CourseClass3'
                Left Join Config F On E.class4=F.Pval and F.PGroup='CourseClass4'
                LEFT JOIN Person P on P.PersonSNO = E.CreateUserID
				LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
				LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
                LEFT JOIN Calendar C On C.EventSNO=E.EventSNO
            WHERE 1=1 And G.PVal=2 and E.SYSTEM_ID = 'S22'
        ";
        Dictionary<string, object> aDict = new Dictionary<string, object>();

        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(aDict, userInfo);
        #endregion

        #region 查詢篩選區塊

        if (!String.IsNullOrEmpty(txt_searchTitle.Text))
        {
            sql += "And EventName  Like '%' + @EventName + '%' ";
            aDict.Add("EventName", txt_searchTitle.Text);
        }
        if (!String.IsNullOrEmpty(txt_searchDate_star.Text))
        {
            sql += @"and StartTime>=@Search";
            aDict.Add("Search", txt_searchDate_star.Text);
        }
        if (!String.IsNullOrEmpty(txt_searchDate_End.Text))
        {
            sql += @" and EndTime<=@Search_END";
            aDict.Add("Search_END", txt_searchDate_End.Text);
        }

        //if (!string.IsNullOrEmpty(ddl_Class.SelectedValue))
        //{
        //    sql += " And E.EventCSNO=@EventCSNO ";
        //    aDict.Add("EventCSNO", ddl_Class.SelectedValue);
        //}
        #endregion

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Event.DataSource = objDT.DefaultView;
        gv_Event.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }


    public static void setEventClass(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY EventCSNO) as ROW_NO,EventCSNO,ClassName  FROM EventClass", null);
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
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER(ORDER BY SYSTEMSNO) as ROW_NO, SYSTEMSNO, SYSTEM_ID, SYSTEM_NAME FROM SYSTEM where ISEnable > 0 ", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }

    protected void gv_Event_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells[7].Text.Equals("0"))
        {
            e.Row.Cells[7].Text = "無上限";
        }
    }
}