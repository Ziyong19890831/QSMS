using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_SystemOrganRole_AE : System.Web.UI.Page
{
    UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null)
        {
            userInfo = (UserInfo)Session["QSMS_UserInfo"];
        }
        if (!IsPostBack)
        {
            hidst.Value = Convert.ToString(Request.QueryString["st"]); 
            hidsno.Value = Convert.ToString(Request.QueryString["sno"]);
            getData();
        }
    }
    protected void getData()
    { 
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("OrganSNO", hidsno.Value);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
            SELECT * FROM Organ Where OrganSNO=@OrganSNO
        ", aDict);
        if (objDT.Rows.Count > 0)
        {
            lbl_RoleName.Text = Convert.ToString(objDT.Rows[0]["OrganName"]);
        }
        aDict.Clear();
        aDict.Add("OrganSNO", hidsno.Value);
        aDict.Add("SYSTEM", hidst.Value);
        objDT = objDH.queryData(@" 
               SELECT ROW_NUMBER() OVER (ORDER BY SR.SRNAME) as ROW_NO,SR.SRID,SR.SRNAME,ISNULL(SOR.ISVIEW,0) AS ISVIEW
               FROM SYSRole SR
               LEFT JOIN SYSOrganRole SOR ON SR.SRID = SOR.SRID AND SR.SYSTEM = SOR.SYSTEM AND SOR.OrganSNO = @OrganSNO
               WHERE SR.SYSTEM = @SYSTEM
        ", aDict);
        gv_RoleMenuAe.DataSource = objDT;
        gv_RoleMenuAe.DataBind();
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("SYSTEM", hidst.Value);
        aDict.Add("OrganSNO", hidsno.Value); 
        DataHelper objDH = new DataHelper();
        //清空選單
        String delSQL = "Delete SYSOrganRole Where OrganSNO=@OrganSNO And SYSTEM=@SYSTEM";
        objDH.executeNonQuery(delSQL, aDict);
        aDict.Clear();
        aDict.Add("SYSTEM", hidst.Value);
        aDict.Add("OrganSNO", hidsno.Value); 
        aDict.Add("UPDATEUSER", userInfo.PersonSNO);
        //寫入選單
        String insertSQL = "";
        for (int i = 0; i < gv_RoleMenuAe.Rows.Count; i++)
        {
            insertSQL += String.Format(@"Insert Into SYSOrganRole(SYSTEM,OrganSNO,SRID,ISVIEW,UPDATEUSER) Values(@SYSTEM,@OrganSNO,@SRID_{0},@ISVIEW_{0},@UPDATEUSER);", i);
            aDict.Add(String.Format("SRID_{0}", i), ((Label)gv_RoleMenuAe.Rows[i].FindControl("SRID")).Text); 
            aDict.Add(String.Format("ISVIEW_{0}", i), ((CheckBox)gv_RoleMenuAe.Rows[i].FindControl("chkISVIEW")).Checked ? 1 : 0); 
        }
        objDH.executeNonQuery(insertSQL, aDict);
        Response.Write("<script>alert('修改成功!');document.location.href='./SystemOrganRole.aspx?st=" + hidst.Value + "'; </script>");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("SystemOrganRole.aspx?st=" + hidst.Value);
    }
}