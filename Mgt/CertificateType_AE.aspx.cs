using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_CertificateType_AE : System.Web.UI.Page
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
            GetRoleList();
            //Utility.setRoleNormal(ddl_Role);
            string work = "";
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


    protected void newData()
    {
        Work.Value = "NEW";       
    }

    protected void getData()
    {
        txt_ID.Value= Convert.ToString(Request.QueryString["sno"]);
        string id = txt_ID.Value;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("CTypeSNO", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
            SELECT 
                CTypeName,CTypeFile,CTypeString,CTypeSEQ,Note,ct.RoleSNO
            FROM QS_CertificateType ct
                Left Join Role R On R.RoleSNO=ct.RoleSNO
            WHERE CTypeSNO=@CTypeSNO
        ", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_CertificateType.Text = objDT.Rows[0]["CTypeName"].ToString();
            lb_CTypeFile.Text = objDT.Rows[0]["CTypeFile"].ToString();
            //ddl_Role.SelectedValue = objDT.Rows[0]["RoleSNO"].ToString();
            txt_CTypeString.Text = objDT.Rows[0]["CTypeString"].ToString();
            txt_CTypeSEQ.Text = objDT.Rows[0]["CTypeSEQ"].ToString();
        }

    }
 

    protected void btnOK_Click(object sender, EventArgs e)
    {

        String errorMessage = "";

        if (string.IsNullOrEmpty(txt_CertificateType.Text)) errorMessage += "請輸入證書名稱!\\n";
        if (string.IsNullOrEmpty(txt_CTypeString.Text)) errorMessage += "請輸入證書字號!\\n";
        if (string.IsNullOrEmpty(txt_CTypeSEQ.Text)) errorMessage += "請輸入流水編號!\\n";

        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        if (Work.Value.Equals("NEW"))
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("CTypeName", txt_CertificateType.Text);
            aDict.Add("CTypeString", txt_CTypeString.Text);
            aDict.Add("CTypeSEQ", txt_CTypeSEQ.Text);
            //aDict.Add("RoleSNO", ddl_Role.SelectedValue);
            //if (!string.IsNullOrEmpty(CTypeFile_New.FileName))
            //{
            //    aDict.Add("CTypeFile", txt_UnitName.Text);
            //}
            DataHelper objDH = new DataHelper();
            DataTable ObjDT= objDH.queryData(@"
            INSERT INTO QS_CertificateType( CTypeName, CTypeString, CTypeSEQ) VALUES (
                 @CTypeName, @CTypeString, @CTypeSEQ);
select @@Identity AS 'Identity' 
            ", aDict);
            if (ObjDT.Rows.Count > 0)
            {
                string CtypeSNO = ObjDT.Rows[0]["Identity"].ToString();
                //UpdateCertificateTypeRole(CtypeSNO);
                Utility.insertRoleBind(cb_Role, CtypeSNO, "CertificateType", userInfo.PersonSNO);
            }
            Response.Write("<script>alert('新增成功!');document.location.href='./CertificateType.aspx'; </script>");
        }
        else
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("id", txt_ID.Value);
            aDict.Add("CTypeName", txt_CertificateType.Text);
            aDict.Add("CTypeString", txt_CTypeString.Text);
            aDict.Add("CTypeSEQ", txt_CTypeSEQ.Text);
            //aDict.Add("RoleSNO", ddl_Role.SelectedValue);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery(@"
                UPDATE QS_CertificateType SET 
                         CTypeName = @CTypeName, 
                         CTypeString = @CTypeString, 
                         CTypeSEQ = @CTypeSEQ
                WHERE CTypeSNO=@id
            ", aDict);
            //UpdateCertificateTypeRole(txt_ID.Value);
            Utility.insertRoleBind(cb_Role, txt_ID.Value, "CertificateType", userInfo.PersonSNO);
            Response.Write("<script>alert('修改成功!');document.location.href='./CertificateType.aspx'; </script>");
        }

    }
    private void GetRoleList()
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT A.RoleSNO , A.RoleName FROM Role A WHERE A.IsAdmin = 0 and A.RoleSNO in ('15','16','17')", aDict);
        cb_Role.DataSource = objDT;
        cb_Role.DataBind();

        //修改預帶勾選
        String work = "";
        if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
        if (!work.Equals("N"))
        {
            String id = Convert.ToString(Request.QueryString["sno"]);
            aDict.Add("sno", id);
            objDT = objDH.queryData(@"SELECT A.RoleSNO FROM RoleBind A WHERE A.CSNO = @sno and A.TypeKey='CertificateType'", aDict);
            foreach (DataRow row in objDT.Rows)
            {

                foreach (ListItem item in cb_Role.Items)
                {
                    if (item.Value == row["RoleSNO"].ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }

            }

        }
    }

    //private void UpdateCertificateTypeRole(string CtypeSNO)
    //{
    //    Dictionary<string, object> aDict = new Dictionary<string, object>();
    //    aDict.Add("CtypeSNO", CtypeSNO);
    //    DataHelper objDH = new DataHelper();
    //    //清空選單
    //    string delSQL = "Delete QS_CertificateTypeRole Where CtypeSNO=@CtypeSNO";
    //    objDH.executeNonQuery(delSQL, aDict);
    //    aDict.Clear();

    //    aDict.Add("CtypeSNO", CtypeSNO);
    //    aDict.Add("CreateUserID", userInfo.PersonSNO);
    //    //寫入選單
    //    string insertSQL = "";
    //    int run = 1;
    //    foreach (ListItem item in cb_Role.Items)
    //    {
    //        if (item.Selected)
    //        {
    //            aDict.Add(string.Format("RoleSNO_{0}", run), item.Value);
    //            insertSQL += String.Format(@"INSERT INTO QS_CertificateTypeRole (CtypeSNO,RoleSNO,CreateUserID)
    //		           VALUES (@CtypeSNO , @RoleSNO_{0} , @CreateUserID);"
    //             , run);
    //            run += 1;
    //        }
    //    }
    //    if (!string.IsNullOrEmpty(insertSQL)) objDH.executeNonQuery(insertSQL, aDict);
    //}
}