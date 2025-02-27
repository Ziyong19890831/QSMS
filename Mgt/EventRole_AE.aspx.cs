using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_EventRole_AE : System.Web.UI.Page
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
            Utility.SetDdlConfig(ddl_Class1, "CourseClass1", "請選擇");
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
        string id = Convert.ToString(Request.QueryString["sno"]);

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("ERSNO", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
           SELECT  [ERSNO]
                        
                         ,[ERName]
                         ,[Class1]
                         ,[IsEnable]
                         ,[CreateDT]
                         ,[CreateUserID]
                         ,[ModifyDT]
                         ,[ModifyUserID]
                     FROM [EventRole] where ERSNO=@ERSNO
        ", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_ERName.Text = objDT.Rows[0]["ERName"].ToString();
            ddl_Class1.SelectedValue= objDT.Rows[0]["Class1"].ToString();
            chk_IsEnable.Checked=(Boolean)objDT.Rows[0]["IsEnable"];
        }

    }
    protected void btnOK_Click(object sender, EventArgs e)
    {

        if (Work.Value.Equals("NEW"))
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("SystemID", "S00");
            aDict.Add("ERName", txt_ERName.Text);  
            aDict.Add("Class1", ddl_Class1.SelectedValue);
            aDict.Add("IsEnable", chk_IsEnable.Checked);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery(@"
                INSERT INTO EventRole (SystemID,ERName, Class1, IsEnable , CreateUserID )
			    VALUES (@SystemID,@ERName, @Class1, @IsEnable , @CreateUserID)
            ", aDict);
            Response.Write("<script>alert('新增成功!');document.location.href='./EventRole.aspx'; </script>");
        }
        else
        {
            string ERSNO = Request.QueryString["sno"];
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            
            aDict.Add("id", txt_ID.Value);           
            aDict.Add("ERName", txt_ERName.Text);
            aDict.Add("Class1", ddl_Class1.SelectedValue);
            aDict.Add("IsEnable", chk_IsEnable.Checked);
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            aDict.Add("ModifyDT", DateTime.Now);
            aDict.Add("ERSNO", ERSNO);

            DataHelper objDH = new DataHelper();
            string sql = @"UPDATE EventRole SET 
                        ERName = @ERName, 
                        Class1 = @Class1,                        
                        IsEnable=@IsEnable,                      
                        ModifyDT = @ModifyDT,
                        ModifyUserID = @ModifyUserID
                WHERE ERSNO=@ERSNO";
            objDH.executeNonQuery(sql, aDict);
            Response.Write("<script>alert('修改成功!');document.location.href='./EventRole.aspx'; </script>");
        }
    }
}