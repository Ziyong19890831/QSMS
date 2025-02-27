using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Mgt_CertificateUnit_Manager_AE : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    int viewrole = 1;
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
            if (Request.QueryString["Work"] != null)
            {
                work = Request.QueryString["Work"];
                GetRoleList();

            }

            if (work.Equals("N"))
            {
                newData();
                btnOK.Text = "新增";
            }
            else
            {
                getData();
                GetRoleList();
            }
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
        DataTable objDT = objDH.queryData(@"
            SELECT 
	            *
            From [QS_CertificateUnit] A WHERE A.CUnitSNO = @sno", aDict);
        if (objDT.Rows.Count > 0)
        {
   
            txt_ID.Value = Convert.ToString(objDT.Rows[0]["CunitSNO"]);
            txt_UnitName.Text = Convert.ToString(objDT.Rows[0]["CUnitName"]);
            chk_admin.Checked= Convert.ToBoolean(objDT.Rows[0]["IsAdmin"]);
            if (chk_admin.Checked == true)
            {
                cb_Role.Enabled = true;
            }


        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (Work.Value.Equals("NEW"))
        {

            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("CunitName", txt_UnitName.Text);
            aDict.Add("IsAdmin", chk_admin.Checked);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            string pclassId = "";
            DataHelper objDH = new DataHelper();
            DataTable pClassResult = objDH.queryData(@"
                INSERT INTO QS_CertificateUnit (CunitName, IsAdmin, CreateUserID)
                VALUES(@CunitName,@IsAdmin,@CreateUserID) SELECT @@IDENTITY AS 'Identity'
            ", aDict);
            if (pClassResult.Rows.Count > 0)
            {
                pclassId = pClassResult.Rows[0]["Identity"].ToString();
                UpdateCourseRole(pclassId);
            }

            Response.Write("<script>alert('新增成功!');document.location.href='./CertificateUnit_Manager.aspx'; </script>");
        }
        else
        {
           
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("CunitSNO", txt_ID.Value);
            aDict.Add("CunitName", txt_UnitName.Text);
            aDict.Add("IsAdmin", chk_admin.Checked);
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery(@"
                                 UPDATE QS_CertificateUnit SET 
                                        CunitName = @CunitName, 
                                        IsAdmin = @IsAdmin,    
                                        ModifyUserID = @ModifyUserID
                                        WHERE CunitSNO = @CunitSNO", aDict);

            UpdateCourseRole(txt_ID.Value);
            Response.Write("<script>alert('修改成功!');document.location.href='./CertificateUnit_Manager.aspx'; </script>");
        }
    }
    private void GetRoleList()
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT A.CunitSNO , A.CunitName FROM [QS_CertificateUnit] A ", aDict);
        cb_Role.DataSource = objDT;
        cb_Role.DataBind();

        //修改預帶勾選
        String work = "";
        if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
        if (!work.Equals("N"))
        {
            String id = Convert.ToString(Request.QueryString["sno"]);
            aDict.Add("sno", id);
            objDT = objDH.queryData(@"SELECT A.CUnitPairSNO FROM [QS_CertificateUnitRole] A WHERE A.CunitSNO = @sno", aDict);
            foreach (DataRow row in objDT.Rows)
            {

                foreach (ListItem item in cb_Role.Items)
                {
                    if (item.Value == row["CUnitPairSNO"].ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }

            }

        }
    }
    private void UpdateCourseRole(string CunitSNO)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("CunitSNO", CunitSNO);
        DataHelper objDH = new DataHelper();
        //清空選單
        string delSQL = "Delete QS_CertificateUnitRole Where CunitSNO=@CunitSNO";
        objDH.executeNonQuery(delSQL, aDict);
        aDict.Clear();

        aDict.Add("CunitSNO", CunitSNO);
        aDict.Add("CreateUserID", userInfo.PersonSNO);
        //寫入選單
        string insertSQL = "";
        int run = 1;
        foreach (ListItem item in cb_Role.Items)
        {
            if (item.Selected)
            {
                aDict.Add(string.Format("CUnitPairSNO_{0}", run), item.Value);
                insertSQL += String.Format(@"INSERT INTO QS_CertificateUnitRole (CunitSNO,CUnitPairSNO,CreateUserID)
						           VALUES (@CunitSNO , @CUnitPairSNO_{0} , @CreateUserID);"
                 , run);
                run += 1;
            }
        }
        if (!string.IsNullOrEmpty(insertSQL)) objDH.executeNonQuery(insertSQL, aDict);
    }


    protected void chk_admin_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_admin.Checked == true)
        {
            cb_Role.Enabled = true;
        }
        else
        {
            cb_Role.Enabled = false;
        }
    }
}