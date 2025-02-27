using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_SystemRole_AE : System.Web.UI.Page
{
    //使用者資訊
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

            //取得單位類別
            //Utility.setOrganLevel(ddl_OrganLevel, "", "全部");

            if (Request.QueryString["sno"] != null)
            {
                hidsno.Value = Convert.ToString(Request.QueryString["sno"]);
                getData(); 
            }
        }
    } 
    protected void getData()
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("SRID", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select * From SYSRole Where SRID=@SRID", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_SRNAME.Text = Convert.ToString(objDT.Rows[0]["SRNAME"]);
            ddl_OrganLevel.SelectedValue = Convert.ToString(objDT.Rows[0]["SROrganLevel"]);   
        } 
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        String errorMessage = "";

        //頁面名稱
        if (txt_SRNAME.Text.Length > 50)
        {
            errorMessage += "角色名稱字元過多！\\n";
        }
        if (String.IsNullOrEmpty(txt_SRNAME.Text))
        {
            errorMessage += "請輸入角色名稱！\\n";
        } 
        //狀態
        if (String.IsNullOrEmpty(ddl_OrganLevel.SelectedValue))
        {
            errorMessage += "請選擇角色類別！\\n";
        }
        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }
        if (hidsno.Value == "")
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("SYSTEM", hidst.Value);
            aDict.Add("SRNAME", txt_SRNAME.Text);
            aDict.Add("SROrganLevel", ddl_OrganLevel.SelectedValue);
            aDict.Add("UPDATEUSER", userInfo.UserName.ToString()); 

            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Insert Into SYSRole(SYSTEM,SRNAME,SROrganLevel,UPDATEUSER) Values(@SYSTEM,@SRNAME,@SROrganLevel,@UPDATEUSER)", aDict);
            Response.Redirect("SystemRole.aspx?st=" + hidst.Value);
        }
        else
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("SRID", hidsno.Value);
            aDict.Add("SYSTEM", hidst.Value);
            aDict.Add("SRNAME", txt_SRNAME.Text);
            aDict.Add("SROrganLevel", ddl_OrganLevel.SelectedValue);
            aDict.Add("UPDATEUSER", userInfo.UserName.ToString()); 

            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Update SYSRole Set SYSTEM=@SYSTEM,SRNAME=@SRNAME,SROrganLevel=@SROrganLevel,UPDATEUSER=@UPDATEUSER WHERE SRID=@SRID", aDict);
            Response.Redirect("SystemRole.aspx?st=" + hidst.Value);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("SystemRole.aspx?st=" + hidst.Value);
    }
}