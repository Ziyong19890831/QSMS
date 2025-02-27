using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Organ : System.Web.UI.Page
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
            Utility.setAreaCodeA(ddl_AreaCodeA, "請選擇");
            //bindData(1);
        }
    }

    protected void ddl_AreaCodeA_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_AreaCodeB.Items.Clear();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String AreaCodeA = ddl_AreaCodeA.SelectedValue;
        if (!String.IsNullOrEmpty(AreaCodeA))
        {
            Utility.setAreaCodeB(ddl_AreaCodeB, AreaCodeA, "請選擇");
        }
        else
        {
            ddl_AreaCodeB.Items.Add(new ListItem("請先選擇縣市行政區", ""));
        }
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String id = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("id", id);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete Organ Where OrganSNO=@id", aDict);
        Response.Write("<script>alert('刪除成功!') </script>");
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
            SELECT ROW_NUMBER() OVER (ORDER BY OrganCode) as ROW_NO, OrganSNO, OrganCode, OrganName, AreaCodeA, AreaCodeB, OrganAddr, OrganTel 
                ,ISNULL((Select Top 1 AREA_NAME From CD_AREA Where AREA_TYPE='A' AND AREA_CODE=AreaCodeA),'') + ISNULL((Select Top 1 AREA_NAME From CD_AREA Where AREA_TYPE='B' AND AREA_CODE=AreaCodeB),'') AS AreaName
            FROM Organ O 
            WHERE 1=1 
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();


        //#region 權限篩選區塊
        //sql += Utility.setSQLAccess_ByRoleOrganType(wDict, userInfo, false);
        //#endregion


        #region 查詢篩選區塊

        if (!String.IsNullOrEmpty(txt_OrganCode.Text))
        {
            sql += " AND OrganCode Like '%' + @OrganCode + '%' ";
            wDict.Add("OrganCode", txt_OrganCode.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_OrganName.Text))
        {
            sql += " AND OrganName Like '%' + @OrganName + '%' ";
            wDict.Add("OrganName", txt_OrganName.Text.Trim());
        }
        if (!String.IsNullOrEmpty(ddl_AreaCodeA.SelectedValue))
        {
            sql += " AND AreaCodeA = @AreaCodeA ";
            wDict.Add("AreaCodeA", ddl_AreaCodeA.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_AreaCodeB.SelectedValue))
        {
            sql += " AND AreaCodeB = @AreaCodeB ";
            wDict.Add("AreaCodeB", ddl_AreaCodeB.SelectedValue);
        }
        #endregion


        sql += " Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Organ.DataSource = objDT.DefaultView;
        gv_Organ.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

}