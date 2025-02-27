using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Mgt_Event : System.Web.UI.Page
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
            setClass(ddl_Class2, "請選擇");
            bindData(1);
            switch (userInfo.RoleGroup)
            {
                case "0":
                    txt_EventGroup.Text = "S";
                    break;
                case "1":
                    txt_EventGroup.Text = "S";
                    break;
                case "2":
                    txt_EventGroup.Text = "S";
                    break;
                case "10":
                    txt_EventGroup.Text = "A";
                    break;
                case "11":
                    txt_EventGroup.Text = "B";
                    break;
                case "12":
                    txt_EventGroup.Text = "C";
                    break;
                case "13":
                    txt_EventGroup.Text = "D";
                    break;
            }
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
        objDH.executeNonQuery("Delete Calendar Where EventSNO=@id", aDict);
        btnPage_Click(sender, e);
        Response.Write("<script>alert('刪除成功!'); location.href=location</script>");
        bindData(1);
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
        //S22為縣市的Record
        String sql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY E.EventSNO DESC) as ROW_NO,E.SYSTEM_ID,EGN.EventGroup,EGN.EventNum,
                E.EventSNO,EC.ClassName,E.EventName,E.StartTime,E.EndTime,
                E.CPerosn,E.CountLimit,G.Mval as Class1,F.Mval as Class2,EB.EventBSNO,
                (Select count(1) From EventD ed Where ed.EventSNO=E.EventSNO) pCount,C.id
				
            FROM Event E
                LEFT JOIN EventBatch EB On EB.EventSNO=E.EventSNO 
                LEFT JOIN EventClass EC on E.EventCSNO=EC.EventCSNO
                LEFT JOIN SYSTEM S on E.SYSTEM_ID=S.SYSTEM_ID
				Left Join EventGroupNum EGN On EGN.EventSNO=E.EventSNO
                Left Join Config G On E.class3=G.Pval and G.PGroup='CourseClass3'
                Left Join Config F On E.class4=F.Pval and F.PGroup='CourseClass4'
				LEFT JOIN Person P on P.PersonSNO = E.CreateUserID
				LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
				LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
                LEFT JOIN Calendar C On C.EventSNO=E.EventSNO
            WHERE 1=1 And G.PVal <> 2 and E.SYSTEM_ID <> 'S22'
        ";
        Dictionary<string, object> aDict = new Dictionary<string, object>();

        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(aDict, userInfo);
        if (userInfo.RoleSNO == "18")//開課單位只能檢視自己開的
        {
            sql += @" and E.CreateUserID=@CreateUserID";
            aDict.Add("CreateUserID", userInfo.PersonSNO);
        }
        #endregion

        #region 查詢篩選區塊

        if (!String.IsNullOrEmpty(txt_searchTitle.Text))
        {
            sql += " And EventName  Like '%' + @EventName + '%' ";
            aDict.Add("EventName", txt_searchTitle.Text);
        }
        if (!String.IsNullOrEmpty(txt_searchDate_star.Text))
        {
            sql += @" and StartTime>=@Search
                     and EndTime>=@Search";
            aDict.Add("Search", txt_searchDate_star.Text);
        }
        if (!String.IsNullOrEmpty(txt_searchDate_End.Text))
        {
            sql += @" and EndTime<=@Search_END";
            aDict.Add("Search_END", txt_searchDate_End.Text);
        }
        if (!String.IsNullOrEmpty(txt_EventGroup.Text) && userInfo.RoleOrganType != "S")
        {
            sql += @" and EGN.EventGroup=@EventGroup";
            aDict.Add("EventGroup", txt_EventGroup.Text);
        }
        if (!String.IsNullOrEmpty(txt_GroupNum.Text))
        {
            sql += @" and EGN.EventNum=@EventNum";
            aDict.Add("EventNum", txt_GroupNum.Text);
        }
        if (ddl_Class2.SelectedValue != "")
        {
            sql += @" and F.Mval=@Mval";
            aDict.Add("Mval", ddl_Class2.SelectedItem.Text);
        }
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
        if (e.Row.Cells[5].Text.Equals("0"))
        {
            e.Row.Cells[5].Text = "無上限";
        }
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string[] id = btn.CommandArgument.ToString().Split(new char[] { ',' });
        Dictionary<string, object> aDict = new Dictionary<string, object>();

        bool ChkERSNO = Event.ChkERSNO(id[0]);

        string EventSNOIdentity = Event.CopyEvent(id[0]);
        Event.CopyEventBatch(id[0], EventSNOIdentity);
        Event.CopyRoleBind(EventSNOIdentity, id[0], userInfo.PersonSNO);
        Event.CopyCalender(EventSNOIdentity);
        bindData(1);
        Response.Write("<script>alert('複製成功!'); location.href=location</script>");

        return;
        //if (ChkERSNO)
        //{
        //    string EventSNOIdentity = Event.CopyEvent(id[0]);
        //    Event.CopyEventBatch(id[0], EventSNOIdentity);
        //    Event.CopyRoleBind(EventSNOIdentity, id[0], userInfo.PersonSNO);
        //    Event.CopyCalender(EventSNOIdentity);
        //    bindData(1);
        //    Response.Write("<script>alert('複製成功!'); </script>");

        //    return;
        //}
        //else
        //{
        //    Response.Write("<script>alert('複製失敗，請設定好活動分類再複製!'); </script>");
        //    return;
        //}

    }
    public static void setClass(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("  Select PVal,MVal from Config where PGroup='CourseClass4' ", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        String filePath = Directory.GetCurrentDirectory() + @"/SysFile/戒菸服務人員訓練課程-講師資料庫.pdf";
        FileInfo file = new FileInfo(filePath);
        if (file.Exists)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "text/plain";
            Response.Flush();
            Response.TransmitFile(file.FullName);
            Response.End();
        }
    }
}