using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_EventClass_AE : System.Web.UI.Page
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
                Utility.SetDdlConfig(ddl_Class1, "CourseClass3");
                Utility.SetDdlConfig(ddl_Class2, "CourseClass4");
                newData();
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
        //分類名稱
        if (txt_Name.Text.Length > 50)
        {
            errorMessage += "名稱字數過多!\\n";
        }
        if (txt_Name.Text.Length == 0)
        {
            errorMessage += "名稱不得為空!\\n";
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
            aDict.Add("Name", txt_Name.Text);
            aDict.Add("Class1", ddl_Class1.SelectedValue);
            aDict.Add("Class2", ddl_Class2.SelectedValue);
            aDict.Add("RoleCheckCoreOnline", Rd_1.SelectedValue);
            aDict.Add("RoleCheckCorePhyAndOnline", Rd_1.SelectedValue);
            aDict.Add("RoleCheckProOnline", txt_Name.Text);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            aDict.Add("CreateDT", DateTime.Now);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Insert Into EventClass(ClassName,Class1,Class2,RoleCheckCoreOnline,RoleCheckCorePhyAndOnline,RoleCheckProOnline,CreateUserID,CreateDT) " +
                "Values(@Name,@Class1,@Class2,@RoleCheckCoreOnline,@RoleCheckCorePhyAndOnline,@RoleCheckProOnline,@CreateUserID)", aDict);
            Response.Write("<script>alert('新增成功!');document.location.href='./EventClass.aspx'; </script>");

        }
        else
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("EventCSNO", txt_ID.Value);
            aDict.Add("Name", txt_Name.Text);
            aDict.Add("ModifyDT", Convert.ToDateTime(DateTime.Now));
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Update EventClass Set ClassName=@Name,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID Where EventCSNO=@EventCSNO", aDict);
            Response.Write("<script>alert('修改成功!');document.location.href='./EventClass.aspx'; </script>");

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
        DataTable objDT = objDH.queryData("select * from EventClass Where EventCSNO=@sno", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_ID.Value = Convert.ToString(objDT.Rows[0]["EventCSNO"]);
            txt_Name.Text = Convert.ToString(objDT.Rows[0]["ClassName"]);

        }
    }
    protected void ddl_Class1_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddl_Class1.SelectedValue == "1")
        {
            ddl_Class1.Items.Remove(new ListItem("實體課程","2"));
        }
        else if (ddl_Class1.SelectedValue == "2")
        {

        }
    }

    protected void ddl_Class2_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddl_Class2.SelectedValue == "1")
        {
            lb_ddl.Visible = true;
            lb_ddl.Text = "是否檢核-核心線上課程";
            Rd_1.Visible = true;
            Rd_2.Visible = false;
            lb_ddl_1.Visible = false;
        }
        else if (ddl_Class2.SelectedValue == "2")
        {
            lb_ddl.Visible = true;
            lb_ddl.Text = "是否檢核-核心線上課程(實體+線上)";
            Rd_1.Visible = true;
            lb_ddl_1.Text = "是否檢核-專門線上課程";
            Rd_2.Visible = true;
            lb_ddl_1.Visible = true;
        }

    }
}