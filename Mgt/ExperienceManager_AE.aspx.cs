using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_ExperienceManager_AE : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    int viewrole = 1;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null)
        {
            userInfo = (UserInfo)Session["QSMS_UserInfo"];

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Utility.setTrainTypeClass(ddl_TCName, "請選擇");
            Utility.setTrainPlanNumber(ddl_TrainPlanNumber, userInfo.RoleGroup,userInfo.RoleOrganType, "請選擇");
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonID", txt_PersonID.Text);
        aDict.Add("TrainType", ddl_TCName.SelectedValue);
        aDict.Add("TrainPlanNumber", ddl_TrainPlanNumber.SelectedValue);
        aDict.Add("TrainRoleType", ddl_TrainRoleType.SelectedValue);
        aDict.Add("CreateUserID", userInfo.PersonSNO);
        aDict.Add("EventName", "新增師資");
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("dbo.SP_WorkExperienceAdd @PersonID, @TrainType,@TrainPlanNumber,@TrainRoleType,@CreateUserID,@EventName", aDict);
        Response.Write("<script>alert('新增成功!');location.href='ExperienceManager.aspx' </script>");
        
        return;
    }

    protected void ddl_TCName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_TCName.SelectedValue == "0")
        {
            ddl_TrainPlanNumber.Enabled = true;
            ddl_TrainPlanNumber.BackColor = System.Drawing.Color.White;
        }
        else
        {
            ddl_TrainPlanNumber.Enabled = false;
            ddl_TrainPlanNumber.BackColor = System.Drawing.Color.LightGray;
        }
    }
}