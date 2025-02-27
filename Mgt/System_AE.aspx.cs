using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Mgt_System_AE : System.Web.UI.Page
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
            if (work.Equals("N"))
            {
                btnOK.Text = "新增";
            }
            else
            {
                getData();
            }
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        String errorMessage = "";
        //系統名稱
        if (txt_sysname.Text.Length > 50)
        {
            errorMessage += "系統名稱字數過多\\n";
        }
        if (txt_sysname.Text.Length == 0)
        {
            errorMessage += "系統名稱字數錯誤\\n";
        }
        
        if (txt_Info.Text.Length > 800)
        {
            errorMessage += "系統簡述字數過多\\n";
        }
        //系統代碼
        if (txt_sysid.Text.Length > 3)
        {
            errorMessage += "系統代碼字數過多\\n";
        }
        if (txt_sysid.Text.Length == 0)
        {
            errorMessage += "系統代碼字數錯誤\\n";
        }
     

        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        if (Request.QueryString["sno"]== null)
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("SYSTEM_ID", txt_sysid.Text);
            aDict.Add("SYSTEM_NAME", txt_sysname.Text);
            aDict.Add("SYSTEM_INFO", txt_Info.Text);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            aDict.Add("ISEnable", DropDownList1.SelectedValue);

            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Insert Into System(SYSTEM_ID,SYSTEM_NAME,SYSTEM_INFO,CreateUserID,ISEnable) Values(@SYSTEM_ID,@SYSTEM_NAME,@SYSTEM_INFO,@CreateUserID,@ISEnable)", aDict);
            Response.Write("<script>alert('新增成功!');document.location.href='./System.aspx'; </script>");
        }
        else
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("SYSTEM_ID", txt_sysid.Text);
            aDict.Add("SYSTEM_NAME", txt_sysname.Text);
            aDict.Add("SYSTEM_INFO", txt_Info.Text);
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            aDict.Add("ModifyDT", Convert.ToDateTime(DateTime.Now));
            aDict.Add("SYSTEMSNO", Request.QueryString["sno"].ToString());
            aDict.Add("ISEnable", DropDownList1.SelectedValue);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Update System Set SYSTEM_ID=@SYSTEM_ID,SYSTEM_NAME=@SYSTEM_NAME,SYSTEM_INFO=@SYSTEM_INFO,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID,ISEnable=@ISEnable Where SYSTEMSNO=@SYSTEMSNO", aDict);
            Response.Write("<script>alert('修改成功!');document.location.href='./System.aspx'; </script>");
        }
    }

   

    protected void getData()
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("sno", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("select * from System Where SYSTEMSNO=@sno", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_ID.Value = Convert.ToString(objDT.Rows[0]["SYSTEMSNO"]);
            txt_sysid.Text = Convert.ToString(objDT.Rows[0]["SYSTEM_ID"]);
            txt_sysname.Text = Convert.ToString(objDT.Rows[0]["SYSTEM_NAME"]);
            txt_Info.Text = Convert.ToString(objDT.Rows[0]["SYSTEM_INFO"]);
            DropDownList1.SelectedValue = Convert.ToString(objDT.Rows[0]["ISEnable"]);
        }
    }


    public static void setNoticeClass(System.Web.UI.WebControls.DropDownList ddl, String ClassID, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY NoticeCSNO) as ROW_NO,NoticeCSNO,Name  FROM NoticeClass", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }
}