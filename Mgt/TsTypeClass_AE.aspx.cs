using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_TsTypeClass_AE : System.Web.UI.Page
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
            setddlRole(ddl_Role);
            String work = "";
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
            if (work.Equals("N"))
            {
                newData();
                btnOK.Text = "確認";
            }
            else
            {
               
                getData();
                btnOK.Text = "修改";
            }
        }
       
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        string errorMessage = "";
        //名稱
        if (txt_Name.Text.Length > 20)
        {
            errorMessage += "名稱字數過多\\n";
        }
        if (txt_Name.Text.Length == 0)
        {
            errorMessage += "請輸入名稱\\n";
        }
 
        if (Work.Value.Equals("NEW"))
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("TsTypeName", txt_Name.Text);
            aDict.Add("RoleSNO", ddl_Role.SelectedValue);
            aDict.Add("IsEnable", chk_IsEnable.Checked);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Insert Into TsTypeClass(TsTypeName,RoleSNO,IsEnable,CreateUserID) Values(@TsTypeName,@RoleSNO,@IsEnable,@CreateUserID)", aDict);
            Response.Write("<script>alert('新增成功!');document.location.href='./TsTypeClass.aspx'; </script>");
        }
        else
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("TsSNO", txt_No.Value);
            aDict.Add("TsTypeName", txt_Name.Text);
            aDict.Add("RoleSNO", ddl_Role.SelectedValue);
            aDict.Add("IsEnable", chk_IsEnable.Checked);
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Update TsTypeClass Set TsTypeName=@TsTypeName,RoleSNO=@RoleSNO,IsEnable=@IsEnable,ModifyUserID=@ModifyUserID,ModifyDT=getdate() Where TsSNO=@TsSNO", aDict);
            Response.Write("<script>alert('修改成功!');document.location.href='./TsTypeClass.aspx'; </script>");

        }
    }

    protected void newData()
    {
        Work.Value = "NEW";
        //Button1.Text = "新增";
    }

    protected void getData()
    {
        String id = Convert.ToString(Request.QueryString["No"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("TsSNO", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
            SELECT ROW_NUMBER() OVER (ORDER BY TsSNO) as ROW_NO,TsSNO,
            TsTypeName,IsEnable,RoleSNO
            FROM TsTypeClass WHERE TsSNO=@TsSNO 
        ", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_No.Value = objDT.Rows[0]["TsSNO"].ToString();
            txt_Name.Text = Convert.ToString(objDT.Rows[0]["TsTypeName"]);
            chk_IsEnable.Checked = Convert.ToBoolean(objDT.Rows[0]["IsEnable"].ToString());
            ddl_Role.SelectedValue= objDT.Rows[0]["RoleSNO"].ToString();
        }
    }
    
    public void setddlRole(DropDownList ddl)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
            Select RoleSNO,RoleName from Role where RoleLevel='50'
        ", aDict);
        ddl_Role.DataSource = objDT.DefaultView;
        ddl_Role.DataBind();
    }
}