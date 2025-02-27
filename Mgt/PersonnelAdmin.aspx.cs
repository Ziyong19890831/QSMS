using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Personnel : System.Web.UI.Page
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
            Utility.setAreaCodeA(ddl_AreaCodeA, "請選擇");
            Utility.setRoleAdmin(ddl_Role, "請選擇");
            bindData(1);
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
        objDH.executeNonQuery("Delete Person Where PersonSNO=@id", aDict);
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
            SELECT ROW_NUMBER() OVER (ORDER BY PersonSNO Desc) as ROW_NO, O.OrganCode, O.OrganName, O.AreaCodeA, O.AreaCodeB, P.PersonSNO, P.PAccount, P.PName, P.PTel, P.PMail 
	            ,ISNULL((Select Top 1 AREA_NAME From CD_AREA Where AREA_TYPE='A' AND AREA_CODE=O.AreaCodeA),'') + ISNULL((Select Top 1 AREA_NAME From CD_AREA Where AREA_TYPE='B' AND AREA_CODE=O.AreaCodeB),'') AS AreaName
                ,R.RoleName, R.RoleLevel
                ,(Case IsEnable When '0'  Then '停用' When '1' Then '啟用' When '2' Then '未開通' Else '-' End) PStatus
            FROM Person P 
                LEFT JOIN Organ O ON O.OrganSNO=P.OrganSNO
                LEFT JOIN Role R ON R.RoleSNO=P.RoleSNO
            WHERE 1=1 And IsAdmin=1 
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();


        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(wDict, userInfo);
        #endregion


        #region 查詢篩選區塊

        if (!String.IsNullOrEmpty(txt_OrganCode.Text))
        {
            sql += " AND O.OrganCode Like '%' + @OrganCode + '%' ";
            wDict.Add("OrganCode", txt_OrganCode.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_OrganName.Text))
        {
            sql += " AND O.OrganName Like '%' + @OrganName + '%' ";
            wDict.Add("OrganName", txt_OrganName.Text.Trim());
        }
        if (!String.IsNullOrEmpty(ddl_AreaCodeA.SelectedValue))
        {
            sql += " AND O.AreaCodeA = @AreaCodeA ";
            wDict.Add("AreaCodeA", ddl_AreaCodeA.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_AreaCodeB.SelectedValue))
        {
            sql += " AND O.AreaCodeB = @AreaCodeB ";
            wDict.Add("AreaCodeB", ddl_AreaCodeB.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_Role.SelectedValue))
        {
            sql += " AND R.RoleSNO = @RoleSNO ";
            wDict.Add("RoleSNO", ddl_Role.SelectedValue);
        }
        if (!String.IsNullOrEmpty(txt_PAccount.Text))
        {
            sql += " AND P.PAccount Like '%' + @PAccount + '%' ";
            wDict.Add("PAccount", txt_PAccount.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_PName.Text))
        {
            sql += " AND P.PName Like '%' + @PName + '%' ";
            wDict.Add("PName", txt_PName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(ddl_IsEnable.SelectedItem.Value))
        {
            sql += " AND P.IsEnable=@IsEnable";
            wDict.Add("IsEnable", ddl_IsEnable.SelectedValue);
        }
        #endregion

        sql += " Order by PersonSNO Desc";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Person.DataSource = objDT.DefaultView;
        gv_Person.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);

    }

}