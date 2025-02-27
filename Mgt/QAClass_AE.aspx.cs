using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_QAClass_AE : System.Web.UI.Page
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
        //註記
        if (txt_Note.Text.Length > 4000)
        {
            errorMessage += "註記字數過多!\\n";
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
            aDict.Add("Note", txt_Note.Text);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Insert Into QAClass(Name,Note,CreateUserID) Values(@Name,@Note,@CreateUserID)", aDict);
            Response.Write("<script>alert('新增成功!');document.location.href='./QAClass.aspx'; </script>");

        }
        else
        {
            String No = Convert.ToString(Request.QueryString["sno"]);
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("QAClass", No);
            aDict.Add("Name", txt_Name.Text);
            aDict.Add("Note", txt_Note.Text);
            aDict.Add("ModifyDT", Convert.ToDateTime(DateTime.Now));
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Update QAClass Set Name=@Name,Note=@Note,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID Where QACSNO=@QAClass", aDict);
            Response.Write("<script>alert('修改成功!');document.location.href='./QAClass.aspx'; </script>");

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
        DataTable objDT = objDH.queryData("select * from QAClass Where QACSNO=@sno", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_ID.Value = Convert.ToString(objDT.Rows[0]["QACSNO"]);
            txt_Name.Text = Convert.ToString(objDT.Rows[0]["Name"]);
            txt_Note.Text = Convert.ToString(objDT.Rows[0]["Note"]);

        }
    }
}