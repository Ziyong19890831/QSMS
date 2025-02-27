using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_TsType : System.Web.UI.Page
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
        }
    }
    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String TsSNO = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("TsSNO", TsSNO);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete TsTypeClass Where TsSNO=@TsSNO", aDict);
        Response.Write("<script>alert('刪除成功!') </script>");
        btnPage_Click(sender, e);
        return;
    }

    protected void btnSearch_Click(object snder, EventArgs e)
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
            select ROW_NUMBER() OVER (ORDER BY TSSNO ) as ROW_No,TC.TsSNO, TC.TsTypeName ,R.RoleName,IsEnable
            from TsTypeClass TC
            Left Join Role R On R.RoleSNO=TC.RoleSNO
            where 1=1
        ";
        Dictionary<string, object> aDict = new Dictionary<string, object>();


        #region 權限篩選區塊
       
        #endregion



        #region 查詢篩選區塊
        if (!String.IsNullOrEmpty(txt_Search.Text))
        {
            sql += "And TC.TsTypeName  Like '%' + @TsTypeName + '%' ";
            aDict.Add("TsTypeName", txt_Search.Text);
        }

        #endregion


        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Urls.DataSource = objDT.DefaultView;
        gv_Urls.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

   
}