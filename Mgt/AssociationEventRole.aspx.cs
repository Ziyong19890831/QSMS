﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_AssociationEventRole : System.Web.UI.Page
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

    protected void bindData(int page)
    {

        if (page < 1) page = 1;
        int pageRecord = 10;
        String sql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY AERSNO) as ROW_NO ,*
            FROM AssociationEventRole 
            WHERE 1=1
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(txt_RoleName.Text))
        {
            sql += " AND AERName Like '%' + @AERName + '%' ";
            wDict.Add("AERName", txt_RoleName.Text.Trim());
        }

        sql += " Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_AssociationEventRole.DataSource = objDT.DefaultView;
        gv_AssociationEventRole.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
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
        aDict.Add("id", id);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete EventRole Where AERSNO=@id", aDict);
        Response.Write("<script>alert('刪除成功!') </script>");
        //objDH.executeNonQuery("Update Notice Set Title='abc' Where id=@id", aDict)
        btnPage_Click(sender, e);
        return;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    }


    protected void lk_Link_Click(object sender, EventArgs e)
    {
        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        string ERSNO = grdrow.Cells[0].Text;
        if (checkESNO(ERSNO))
        {
            Response.Write("<script>alert('此規則已加入規則，請使用修改規則')</script>");
        }
        else
        {
            Response.Redirect("./EventRoleDetail.aspx?sno=" + ERSNO + "");
        }
    }
    public static bool checkESNO(string AERSNO)
    {
        DataHelper objDH = new DataHelper();
        string sql = "Select * from [AssociationEventRoleDetail] where AERSNO=@AERSNO";
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("AERSNO", AERSNO);
        DataTable ObjDT = objDH.queryData(sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    protected void gv_AssociationEventRole_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;
        }
    }

}