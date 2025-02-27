using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_UrlClass_AE : System.Web.UI.Page
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
            }
            else
            {
                getData();
            }
        }
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("select * from UrlClass", null);
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        string errorMessage = "";
        //名稱
        if(txt_Name.Text.Length>20)
        {
            errorMessage += "名稱字數過多\\n";
        }
        if (txt_Name.Text.Length == 0)
        {
            errorMessage += "請輸入名稱\\n";
        }
        //註記
        if (txt_Note.Text.Length>100)
        {
            errorMessage += "註記字數過多\\n";
        }
        //註記
        if (Work.Value.Equals("NEW"))
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("Name", txt_Name.Text);
            aDict.Add("Note", txt_Note.Text);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Insert Into UrlClass(Name,Note,CreateUserID) Values(@Name,@Note,@CreateUserID)", aDict);
            Response.Write("<script>alert('新增成功!');document.location.href='./UrlClass.aspx'; </script>");
        }
        else
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("Name", txt_Name.Text);
            aDict.Add("Note", txt_Note.Text);
            aDict.Add("URLCSNO", txt_No.Value);
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Update UrlClass Set Name=@Name,Note=@Note, ModifyUserID=@ModifyUserID,ModifyDT=getdate() Where URLCSNO=@URLCSNO", aDict);
            Response.Write("<script>alert('修改成功!');document.location.href='./UrlClass.aspx'; </script>");

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
        aDict.Add("URLCSNO", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
            SELECT ROW_NUMBER() OVER (ORDER BY URLCSNO) as ROW_NO,
            URLCSNO,Name,Note
            FROM UrlClass WHERE URLCSNO=@URLCSNO 
        ", aDict);
        if (objDT.Rows.Count > 0)
        {
            //txt_ID.Value = Convert.ToString(objDT.Rows[0]["id"]);
            txt_No.Value = objDT.Rows[0]["URLCSNO"].ToString();
            txt_Name.Text = Convert.ToString(objDT.Rows[0]["Name"]);
            txt_Note.Text = objDT.Rows[0]["Note"].ToString();

        }
        //Button1.Text = "修改";
    }

}