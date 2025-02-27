using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Role_AE : System.Web.UI.Page
{
    //使用者資訊
    protected UserInfo userInfo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];

        if (!IsPostBack)
        {
            //取得角色單位類別
            setRoleOrganType(ddl_RoleOrganType, "請選擇");

            String work = "";
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
            if (work.Equals("N"))
            {
                newData();
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
        //角色名稱
        if (String.IsNullOrEmpty(txt_RoleName.Text))
        {
            errorMessage += "請輸入角色名稱！\\n";
        }
        if (txt_RoleName.Text.Length > 50)
        {
            errorMessage += "角色名稱字數過多\\n";
        }
        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }


        if (Work.Value.Equals("NEW"))
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("RoleName", txt_RoleName.Text);
            aDict.Add("RoleLevel", txt_RoleLevel.Text);
            aDict.Add("RoleGroup", txt_RoleGroup.Text);
            aDict.Add("IsAdmin", ddl_Admin.SelectedValue);
            aDict.Add("CreateUserID", userInfo.PersonSNO);

            if (ddl_Admin.SelectedValue == "1")
                aDict.Add("RoleOrganType", ddl_RoleOrganType.SelectedValue);
            else
                aDict.Add("RoleOrganType", "");

            DataHelper objDH = new DataHelper();
            //判斷角色名稱唯一性
            DataTable objDT = objDH.queryData("Select RoleName From Role Where RoleName=@RoleName", aDict);
            if (objDT.Rows.Count > 0)
            {
                Utility.showMessage(Page, "ErrorMessage", String.Format("[{0}]角色名稱已存在！\\n", txt_RoleName.Text));
                return;
            }


            string sql = @"Insert Into Role(RoleName,RoleOrganType,RoleLevel,RoleGroup,IsAdmin,CreateUserID) 
                                Values(@RoleName,@RoleOrganType,@RoleLevel,@RoleGroup,@IsAdmin,@CreateUserID)";
            objDH.executeNonQuery(sql, aDict);
            Response.Write("<script>alert('新增成功!');document.location.href='./Role.aspx'; </script>");
        }
        else
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("RoleSNO", txt_RoleID.Value);
            aDict.Add("RoleName", txt_RoleName.Text);
            aDict.Add("RoleLevel", txt_RoleLevel.Text);
            aDict.Add("RoleGroup", txt_RoleGroup.Text);
            aDict.Add("IsAdmin", ddl_Admin.SelectedValue);
            aDict.Add("ModifyDT", Convert.ToDateTime(DateTime.Now));
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            if(ddl_Admin.SelectedValue=="1")
                aDict.Add("RoleOrganType", ddl_RoleOrganType.SelectedValue);
            else
                aDict.Add("RoleOrganType", "");

            DataHelper objDH = new DataHelper();
            //判斷角色名稱唯一性
            DataTable objDT = objDH.queryData("Select RoleName From Role Where RoleSNO<>@RoleSNO AND RoleName=@RoleName", aDict);
            if (objDT.Rows.Count > 0)
            {
                Utility.showMessage(Page, "ErrorMessage", String.Format("[{0}]角色名稱已存在！\\n", txt_RoleName.Text));
                return;
            }


            string sql = @"Update Role Set 
                    RoleName=@RoleName, RoleOrganType=@RoleOrganType, RoleLevel=@RoleLevel, RoleGroup=@RoleGroup,
                    IsAdmin=@IsAdmin, ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID 
                Where RoleSNO=@RoleSNO";
            objDH.executeNonQuery(sql, aDict);
            Response.Write("<script>alert('修改成功!');document.location.href='./Role.aspx'; </script>");
        }
    }

    protected void newData()
    {
        Work.Value = "NEW";
    }

    protected void getData()
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("sno", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"Select 
                RoleSNO, RoleName, RoleOrganType, RoleLevel, RoleGroup, (Case IsAdmin When 1 Then '1' Else '0' End) IsAdmin 
            From Role Where RoleSNO=@sno", aDict);
        if (objDT.Rows.Count > 0)
        {
            if (Convert.ToString(objDT.Rows[0]["IsAdmin"]) == "1")
                tr_RoleOrganType.Visible = true;
            else
                tr_RoleOrganType.Visible = false;

            txt_RoleID.Value = Convert.ToString(objDT.Rows[0]["RoleSNO"]);
            txt_RoleName.Text = Convert.ToString(objDT.Rows[0]["RoleName"]);
            txt_RoleLevel.Text = Convert.ToString(objDT.Rows[0]["RoleLevel"]);
            txt_RoleGroup.Text = Convert.ToString(objDT.Rows[0]["RoleGroup"]);
            ddl_Admin.SelectedValue = Convert.ToString(objDT.Rows[0]["IsAdmin"]);
            ddl_RoleOrganType.SelectedValue = Convert.ToString(objDT.Rows[0]["RoleOrganType"]);
        }
    }

    protected void setRoleOrganType(DropDownList ddl, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT PVal, PVal+':'+MVal as MVal FROM Config Where PGroup='OrganType' Order By PID ", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }



    protected void ddl_Admin_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_Admin.SelectedValue == "1")
            tr_RoleOrganType.Visible = true;
        else
            tr_RoleOrganType.Visible = false;
    }

}