using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Urls_AE : System.Web.UI.Page
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
                ButtonOK.Text = "新增";
            }
            else
            {
                getData();
            }
        }
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("select * from UrlClass", null);
        
    }

    protected void ButtonOK_Click(object sender, EventArgs e)
    {
        String errorMessage = "";
        //名稱
        if (txt_Name.Text.Length>50)
        {
            errorMessage += "名稱字數過多\\n";
            
        }
        if (txt_Name.Text.Length == 0)
        {
            errorMessage += "請輸入名稱\\n";
        }
        //超連結
        if (txt_Url.Text.Length>400)
        {
            errorMessage += "超連結字數過多!\\n";
        }
        if (txt_Url.Text.Length == 0)
        {
            errorMessage += "請輸入超連結!\\n";
        }
        //分類
        if (ddl_Class.SelectedValue=="")
        {
            errorMessage += "請選擇分類!\\n";
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
            aDict.Add("Url", txt_Url.Text);
            aDict.Add("Class", ddl_Class.SelectedValue);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Insert Into Url(Name, Url, URLCSNO,CreateUserID) Values(@Name, @Url, @Class,@CreateUserID)", aDict);
            Response.Write("<script>alert('新增成功!');document.location.href='./Urls.aspx'; </script>");
        }
        else
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            //aDict.Add("id", txt_ID.Value);
            aDict.Add("Name", txt_Name.Text);
            aDict.Add("Url", txt_Url.Text);
            aDict.Add("Class", ddl_Class.SelectedValue);
            aDict.Add("URLSNO", txt_No.Value);
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Update Url Set Name=@Name, Url=@Url,URLCSNO=@Class,ModifyUserID=@ModifyUserID,ModifyDT=getdate() Where URLSNO=@URLSNO", aDict);
            Response.Write("<script>alert('修改成功!');document.location.href='./Urls.aspx'; </script>");
        }
        
    }

    protected void newData()
    {
        Work.Value = "NEW";
        //Button1.Text = "新增";
        setUrlClass(ddl_Class, "請選擇");
    }

    protected void getData()
    {
        String No = Convert.ToString(Request.QueryString["No"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("URLSNO", No);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
        select URLSNO, Name, Url , URLCSNO
        from Url
        Where URLSNO=@URLSNO
        ", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_No.Value = Convert.ToString(objDT.Rows[0]["URLSNO"]);
            txt_Name.Text = Convert.ToString(objDT.Rows[0]["Name"]);
            txt_Url.Text = Convert.ToString(objDT.Rows[0]["Url"]);
            setUrlClass(ddl_Class, "請選擇");
            ddl_Class.SelectedValue = Convert.ToString(objDT.Rows[0]["URLCSNO"]);

        }
        //Button1.Text = "修改";
    }

    public static void setUrlClass(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY URLCSNO) as ROW_NO,URLCSNO,Name  FROM UrlClass", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }


}