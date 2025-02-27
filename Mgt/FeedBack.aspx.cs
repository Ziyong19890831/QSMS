using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_FeedBack : System.Web.UI.Page
{
    public UserInfo userInfo = null;

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

    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String id = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("FBSNO", id);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete FROM Feedback Where FBSNO=@FBSNO", aDict);
        Utility.showMessage(Page, "訊息", "刪除成功。");
        btnPage_Click(sender, e);
        return;
    }

    protected void bindData(int page)
    {
        if (page < 1) page = 1;
        int pageRecord = 10;

        String sql = @"
            select ROW_NUMBER() OVER (ORDER BY FBSNO DESC )
                as ROW_NO,FBSNO, FBTYPE, Name,Rank,Email,Tel,Explain,Response,FeedBackDate,CreateDT
            from Feedback F
            Where 1=1
        ";
        switch (userInfo.RoleSNO)
        {
            case "6":
                sql += " And FBTYPE='醫師課程'";
                break;
            case "7":
                sql += " And FBTYPE='牙醫師課程'";
                break;
            case "8":
                sql += " And FBTYPE='藥師課程'";
                break;
            case "9":
                sql += " And FBTYPE='衛教師課程'";
                break;
        }
           
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();

        if (!String.IsNullOrEmpty(txt_SearchTitle.Text))
        {
            sql += "And Explain  Like '%' + @Explain + '%' ";
            aDict.Add("Explain", txt_SearchTitle.Text);
        }
        if (!String.IsNullOrEmpty(txt_mail.Text))
        {
            sql += "And Email=@Email ";
            aDict.Add("Email", txt_mail.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_searchDateStart.Text))
        {
            sql += "And CreateDT >= @txt_searchDateStart ";
            aDict.Add("txt_searchDateStart", txt_searchDateStart.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_searchDateEnd.Text))
        {
            sql += "And CreateDT <= @txt_searchDateEnd ";
            aDict.Add("txt_searchDateEnd", txt_searchDateEnd.Text.Trim());
        }

        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Feedback.DataSource = objDT.DefaultView;
        gv_Feedback.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }


    protected void gv_Feedback_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem != null)
        {

    
            //Cast DataItem into your data object that you bound with Grid. I am assuming that you bound you grid with DataTable and CustomerName is a column in that table.
            string FB_Response = e.Row.Cells[9].Text;
            if (FB_Response == "已回覆")
            {
                //e.Row.Cells[11].Text = "";
            }

        }
    }
}