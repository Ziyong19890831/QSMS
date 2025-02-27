using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_RoleMenu_AE : System.Web.UI.Page
{
    UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String work = "";
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
            getData();
        }
    }
    protected void getData()
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("RoleSNO", String.IsNullOrEmpty(id) ? "" : id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
            SELECT * FROM Role Where RoleSNO=@RoleSNO
        ", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_RoleID.Value = Convert.ToString(objDT.Rows[0]["RoleSNO"]);
            lbl_RoleName.Text = Convert.ToString(objDT.Rows[0]["RoleName"]);
        }
        objDT = objDH.queryData(@"
            WITH MenuA AS (
	            SELECT * FROM PageLink WHERE PPLINKSNO IS NULL --AND ISENABLE=1
	            UNION ALL
	            SELECT PageLink.* FROM PageLink
	            INNER JOIN MenuA ON MenuA.PLINKSNO=PageLink.PPLINKSNO
	            --WHERE PageLink.ISENABLE=1
            )
            SELECT ROW_NUMBER() OVER(ORDER BY GROUPORDER,PLINKORDER) AS ROW_NO, MenuA.*,ISVIEW,ISUPDATE,ISINSERT,ISDELETE  FROM MenuA 
            LEFT JOIN RoleMenu ON RoleMenu.RoleSNO=@RoleSNO AND RoleMenu.PLINKSNO = MenuA.PLINKSNO
        ", aDict);
        gv_RoleMenuAe.DataSource = objDT;
        gv_RoleMenuAe.DataBind();
        //Button1.Text = "修改";
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("RoleSNO", txt_RoleID.Value);
        //aDict.Add("RoleName", txt_RoleName.Text);
        DataHelper objDH = new DataHelper();
        //清空選單
        String delSQL = "Delete RoleMenu Where RoleSNO=@RoleSNO";
        objDH.executeNonQuery(delSQL, aDict);
        aDict.Add("CreateUserID", userInfo.PersonSNO);
        //寫入選單
        String insertSQL = "";
        for (int i = 0; i < gv_RoleMenuAe.Rows.Count; i++)
        {
            insertSQL += String.Format(@"Insert Into RoleMenu(RoleSNO,PPLINKSNO,PLINKSNO,ISVIEW,ISUPDATE,ISINSERT,ISDELETE,CreateUserID) Values(@RoleSNO,@PPLINKSNO_{0},@PLINKSNO_{0},@ISVIEW_{0},@ISUPDATE_{0},@ISINSERT_{0},@ISDELETE_{0},@CreateUserID);", i);
            aDict.Add(String.Format("PPLINKSNO_{0}", i), ((Label)gv_RoleMenuAe.Rows[i].FindControl("PPLINKSNO")).Text);
            aDict.Add(String.Format("PLINKSNO_{0}", i), ((Label)gv_RoleMenuAe.Rows[i].FindControl("PLINKSNO")).Text);
            aDict.Add(String.Format("ISVIEW_{0}", i), ((CheckBox)gv_RoleMenuAe.Rows[i].FindControl("chkISVIEW")).Checked ? 1 : 0);
            aDict.Add(String.Format("ISUPDATE_{0}", i), ((CheckBox)gv_RoleMenuAe.Rows[i].FindControl("chkISUPDATE")).Checked ? 1 : 0);
            aDict.Add(String.Format("ISINSERT_{0}", i), ((CheckBox)gv_RoleMenuAe.Rows[i].FindControl("chkISINSERT")).Checked ? 1 : 0);
            aDict.Add(String.Format("ISDELETE_{0}", i), ((CheckBox)gv_RoleMenuAe.Rows[i].FindControl("chkISDELETE")).Checked ? 1 : 0);
        }
        objDH.executeNonQuery(insertSQL, aDict);
        //Response.Write("<script>alert('修改成功!');document.location.href='./RoleMenu.aspx'; </script>");
        Response.Write("<script>alert('修改成功!');document.location.href='./Role.aspx'; </script>");
    }
}