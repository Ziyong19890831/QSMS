using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_UserAPSAudit_AE : System.Web.UI.Page
{
    UserInfo userInfo = null;
    DateTime NowTime = Convert.ToDateTime(DateTime.Now);
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String work = "";
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
            Utility.setMemberStatus(ddl_Status, "請選擇");
            GetPersonData();
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        string errorMessage = "";

        if (string.IsNullOrEmpty(ddl_Status.SelectedValue)) errorMessage += "請選擇學員狀態";


        if (!string.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }
        

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        aDict.Add("PersonSNO", Request.QueryString["sno"]);
        aDict.Add("MStatusSNO", ddl_Status.SelectedValue);
        objDH.executeNonQuery(@"UPDATE Person SET MStatusSNO=@MStatusSNO
                                WHERE PersonSNO = @PersonSNO", aDict);

        Response.Write("<script>alert('修改成功!');window.opener.location.reload();;window.close(); </script>");

    }


    protected void GetPersonData()
    {
        string id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("sno", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
            SELECT
	            O.OrganCode, O.OrganName, P.PersonSNO, P.PAccount, P.PName,
	            R.RoleName , P.CreateDT,
	            MP.OrganCode mOrganCode, MP.PName mPName, OM.OrganName mOrganName, MP.JType, 
                MP.JCN, MP.JDate, MP.VSDate, MP.VEDate, MP.LCN, MP.LValid, MP.ModifyDT mModifyDT
            FROM Person P 
                LEFT JOIN PersonMP MP ON MP.PersonID=P.PersonID
                LEFT JOIN Organ O ON O.OrganSNO=P.OrganSNO
                LEFT JOIN Organ OM ON OM.OrganCode=MP.OrganCode
                LEFT JOIN Role R ON R.RoleSNO=P.RoleSNO
            WHERE P.PersonSNO=@sno
        ", aDict);

        if (objDT.Rows.Count > 0)
        {
            lbl_PAccount.Text = objDT.Rows[0]["PAccount"].ToString();
            lbl_CreateDate.Text = objDT.Rows[0]["CreateDT"].ToString() != "" ? ((DateTime)objDT.Rows[0]["CreateDT"]).ToString("yyyy/MM/dd HH:mm:ss") : "";
            lbl_OrganCode.Text = objDT.Rows[0]["OrganCode"].ToString();
            lbl_OrganName.Text = objDT.Rows[0]["OrganName"].ToString();
            lbl_PName.Text = objDT.Rows[0]["PName"].ToString();
            lbl_RoleName.Text = objDT.Rows[0]["RoleName"].ToString();
            lbr_OrganCode.Text = objDT.Rows[0]["mOrganCode"].ToString();
            lbr_OrganName.Text = objDT.Rows[0]["mOrganName"].ToString();
            lbr_PName.Text = objDT.Rows[0]["mPName"].ToString();
            lbr_JType.Text = objDT.Rows[0]["JType"].ToString();
            lbr_JCN.Text = objDT.Rows[0]["JCN"].ToString();
            lbr_JDate.Text = objDT.Rows[0]["JDate"].ToString();
            lbr_VDate.Text = objDT.Rows[0]["VSDate"].ToString() + "~" + objDT.Rows[0]["VEDate"].ToString();
            lbr_ModifyDate.Text = objDT.Rows[0]["mModifyDT"].ToString();
        }
    }

   

}