using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_ICGet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Utility.setCtypeName(ddl_Certificate, "請選擇");
            Utility.setPlanName(ddl_CoursePlanningClass, "請選擇");
            Utility.setCtype(ddl_Type, "請選擇");
        }
    }

    protected void btn_InsertI_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string PersonSNO = Utility.ConvertPersonIDToPersonSNO(txt_PersonID_I.Text);
        string SQL = @"INSERT INTO [dbo].[QS_Integral]
           ([PersonSNO],[CourseSNO],[CreateDT],[CreateUserID],[IsUsed],[AuthType])
            Select @PersonSNO,CourseSNO,Getdate(),2,0,0 From QS_Course where Ctype=@Ctype and PClassSNO=@PClassSNO
           ";
        aDict.Add("PersonSNO",PersonSNO);
        aDict.Add("Ctype", ddl_Type.SelectedValue);
        aDict.Add("PClassSNO", ddl_CoursePlanningClass.SelectedValue);
        objDH.executeNonQuery(SQL, aDict);
        Utility.MessageBox.Show("匯入成功");

    }

    protected void btn_InsertC_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string SQL = @"INSERT INTO [dbo].[QS_Certificate]
           ([PersonID]
           ,[CertID]
           ,[CTypeSNO]
           ,[CUnitSNO]
           ,[CertPublicDate]
           ,[CertStartDate]
           ,[CertEndDate]
           ,[CertExt]
           ,[IsPrint]
           ,[CreateDT]
           ,[CreateUserID]
           ,[SysChange]
           ,[IsChange])
     VALUES
           (@PersonID
           ,@CertID
           ,@CTypeSNO
           ,@CUnitSNO
           ,@CertPublicDate
           ,@CertStartDate
           ,@CertEndDate
           ,0
           ,0
           ,getdate()
           ,2
           ,1
           ,0)";
        DateTime CertPublicDate = Convert.ToDateTime(CertEnddate.Text).AddYears(-6).AddDays(1);
        string CUnitSNO = ReturnCunit(ddl_Certificate.SelectedValue);
        aDict.Add("PersonID", txt_PersonID_C.Text);
        aDict.Add("CertID", txt_CertID.Text);
        aDict.Add("CTypeSNO", ddl_Certificate.SelectedValue);
        aDict.Add("CUnitSNO", CUnitSNO);
        aDict.Add("CertPublicDate", CertPublicDate);
        aDict.Add("CertStartDate", CertPublicDate);
        aDict.Add("CertEndDate", CertEnddate.Text);
        objDH.executeNonQuery(SQL, aDict);
        Utility.MessageBox.Show("匯入成功");
    }
    public static string ReturnCunit(string CtypeSNO)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string SQL = "Select CunitSNO from QS_CertificateType where CtypeSNO=@CtypeSNO";
        aDict.Add("CtypeSNO", CtypeSNO);
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT.Rows[0]["CUnitSNO"].ToString();
        }
        else
        {
            return "";
        }
    }

}

