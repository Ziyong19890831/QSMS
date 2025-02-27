using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_SystemOrganRole : System.Web.UI.Page
{
    UserInfo userInfo = null;
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

            hidSystem.Value = Convert.ToString(Request.QueryString["st"]);
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
      
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    } 

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(hidPage.Value, out page);
        bindData(page);
    }

    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 20;
        String sql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY O.OrganCode) as ROW_NO, O.OrganSNO, O.OrganCode, O.OrganName, O.AreaCodeA, O.AreaCodeB, O.OrganAddr, O.OrganTel 
                ,ISNULL((Select Top 1 AREA_NAME From CD_AREA Where AREA_TYPE='A' AND AREA_CODE=AreaCodeA),'') + ISNULL((Select Top 1 AREA_NAME From CD_AREA Where AREA_TYPE='B' AND AREA_CODE=AreaCodeB),'') AS AreaName
                ,ISNULL(SOR.M_COUNT,0) AS M_COUNT
            FROM Organ O
            LEFT JOIN (
			SELECT SOR.OrganSNO,COUNT(0) AS M_COUNT
			FROM
				SYSOrganRole SOR
			INNER JOIN SYSRoleMenu SRM ON SOR.SYSTEM = SRM.SYSTEM AND SOR.SRID = SRM.SRID AND SRM.ISVIEW = '1'
			WHERE SOR.SYSTEM = 'S08' 
			AND SOR.ISVIEW = '1'
            GROUP BY SOR.OrganSNO
            ) SOR ON SOR.OrganSNO = O.OrganSNO
            WHERE 1=1 
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
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
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Role.DataSource = objDT.DefaultView;
        gv_Role.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

  
}