using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_SystemPageLink_AE : System.Web.UI.Page
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
            if (Request.QueryString["sno"] != null)
            {
                hidsno.Value = Convert.ToString(Request.QueryString["sno"]);
                getData();
                txt_PLinkAlias.Enabled = false;
            }
        }
    } 
    protected void getData()
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("SPLID", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select * From SYSPageLink Where SPLID=@SPLID", aDict);
        if (objDT.Rows.Count > 0)
        { 
            txt_PLinkName.Text = Convert.ToString(objDT.Rows[0]["SPLNAME"]);
            txt_PLinkAlias.Text = Convert.ToString(objDT.Rows[0]["SPLALIAS"]);
            txt_PLinkUrl.Text = Convert.ToString(objDT.Rows[0]["SPLURL"]);
            ddl_ISENABLE.SelectedValue = Convert.ToString(objDT.Rows[0]["ISENABLE"]);   
        } 
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        String errorMessage = "";

        //頁面名稱
        if (txt_PLinkName.Text.Length > 60)
        {
            errorMessage += "頁面名稱字元過多！\\n";
        }
        if (String.IsNullOrEmpty(txt_PLinkName.Text))
        {
            errorMessage += "請輸入頁面名稱！\\n";
        }
        //頁面別名
        if (txt_PLinkAlias.Text.Length > 60)
        {
            errorMessage += "頁面名稱字元過多！\\n";
        }
        if (String.IsNullOrEmpty(txt_PLinkAlias.Text))
        {
            errorMessage += "請輸入頁面名稱！\\n";
        }
        //頁面網址
        if(txt_PLinkUrl.Text.Length>200)
        {
            errorMessage += "頁面網址字元過多!\\n";
        }
        if (String.IsNullOrEmpty(txt_PLinkUrl.Text))
        {
            errorMessage += "請輸入頁面網址！\\n";
        }
        //狀態
        if (String.IsNullOrEmpty(ddl_ISENABLE.SelectedValue))
        {
            errorMessage += "請選擇狀態！\\n";
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
            aDict.Add("SPLNAME", txt_PLinkName.Text);
            aDict.Add("SPLALIAS", txt_PLinkAlias.Text);
            aDict.Add("SPLURL", txt_PLinkUrl.Text);
            aDict.Add("ISENABLE", ddl_ISENABLE.SelectedValue);
            aDict.Add("UPDATEUSER", userInfo.UserName.ToString()); 

            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Insert Into SYSPageLink(SYSTEM,SPLNAME,SPLALIAS,SPLURL,ISENABLE,UPDATEUSER) Values(@SYSTEM,@SPLNAME,@SPLALIAS,@SPLURL,@ISENABLE,@UPDATEUSER)", aDict);
            Response.Redirect("SystemPageLink.aspx?st=" + hidst.Value);
        }
        else
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("SPLID", hidsno.Value);
            aDict.Add("SYSTEM", hidst.Value);
            aDict.Add("SPLNAME", txt_PLinkName.Text);
            aDict.Add("SPLALIAS", txt_PLinkAlias.Text);
            aDict.Add("SPLURL", txt_PLinkUrl.Text);
            aDict.Add("ISENABLE", ddl_ISENABLE.SelectedValue);
            aDict.Add("UPDATEUSER", userInfo.UserName.ToString()); 

            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Update SYSPageLink Set SYSTEM=@SYSTEM,SPLNAME=@SPLNAME,SPLALIAS=@SPLALIAS,SPLURL=@SPLURL,ISENABLE=@ISENABLE,UPDATEUSER=@UPDATEUSER WHERE SPLID=@SPLID", aDict);
            Response.Redirect("SystemPageLink.aspx?st=" + hidst.Value);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("SystemPageLink.aspx?st=" + hidst.Value);
    }
}