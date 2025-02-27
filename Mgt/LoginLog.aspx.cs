using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_LoginLog : System.Web.UI.Page
{
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfo;
    public Dictionary<string, string> _SetCol;
    public UserInfo userInfo = null;
    int viewrole = 1;
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Utility.setAreaCodeA(ddl_AreaCodeA, "請選擇");
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
        int.TryParse(txt_Page.Value, out page);
        bindData(page);
    }
    private void ReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();

        _SetCol.Add("LoginTime", "登入時間");
        _SetCol.Add("LoginInfo", "登入資訊");
        _SetCol.Add("PAccount", "帳號");
        _SetCol.Add("PName", "人員名稱");
        _SetCol.Add("RoleName", "角色");
        _SetCol.Add("OrganName", "機構名稱");
        _SetCol.Add("AreaName", "區域");



        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.LoginLog.ToString()] = _ExcelInfo;
    }
        protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;

        string sql = @"
            Select ROW_NUMBER() OVER (ORDER BY LoginTime Desc) as ROW_NO, 
	            L.LoginTime, L.LoginInfo,
				P.PAccount,  P.PName, R.RoleName ,
	            O.OrganCode, O.OrganName, O.AreaCodeA, O.AreaCodeB, P.PersonSNO,
                ISNULL((Select Top 1 AREA_NAME From CD_AREA Where AREA_TYPE='A' AND AREA_CODE=O.AreaCodeA),'') + 
	            ISNULL((Select Top 1 AREA_NAME From CD_AREA Where AREA_TYPE='B' AND AREA_CODE=O.AreaCodeB),'') AS AreaName
            From LoginLog L
	            Left Join Person P ON P.PersonSNO=L.PersonSNO
                LEFT JOIN Organ O ON O.OrganSNO=P.OrganSNO
                LEFT JOIN Role R ON R.RoleSNO=P.RoleSNO
            WHERE L.PersonSNO Is Not Null And R.RoleLevel>=@RoleLevel 
        ";

        Dictionary<string, object> wDict = new Dictionary<string, object>();


        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(wDict, userInfo);
        #endregion


        #region 查詢區塊
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
        if (!String.IsNullOrEmpty(ddl_IsAdmin.SelectedValue))
        {
            if (ddl_IsAdmin.SelectedValue == "1")
            {
                sql += " AND R.RoleOrganType ='S' ";
            }  
            else if(ddl_IsAdmin.SelectedValue == "2")
            {
                sql += " AND R.RoleOrganType='U' ";
            }
            else
            {
                sql += " AND R.RoleOrganType <> '' ";
            }
           
        }
        if (!String.IsNullOrEmpty(ddl_LoginStatus.SelectedValue))
        {
            sql += " AND L.LoginStatus=@LoginStatus";
            wDict.Add("LoginStatus", ddl_LoginStatus.SelectedValue);
        }
        if (!String.IsNullOrEmpty(LoginStart.Text))
        {
            sql += " AND LoginTime > @LoginStart";
            wDict.Add("LoginStart", LoginStart.Text);
        }
        if (!String.IsNullOrEmpty(LoginEnd.Text))
        {
            sql += " AND LoginTime < @LoginEnd";
            wDict.Add("LoginEnd", LoginEnd.Text);
        }
        #endregion

        sql += " Order by L.LoginTime Desc";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Person.DataSource = objDT.DefaultView;
        gv_Person.DataBind();
        ReportInit(objDT);
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (gv_Person.Rows.Count == 0)
        {
            Response.Write("<script>alert('尚未查詢')</script>");
            return;
        }
        Utility.OpenExportWindows(this, ReportEnum.LoginLog.ToString());
    }

}