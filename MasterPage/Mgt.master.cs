using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net.Security;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;

public partial class MasterPage_Mgt : System.Web.UI.MasterPage
{
    UserInfo userInfo = null;
    public string MarqueeIsEnable = "";

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
        if (userInfo == null) Response.Redirect("../Default.aspx");
        MarqueeIsEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["Marquee"];
        CheckAdmin();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (userInfo != null)
            {
                //AutoSignInELearning();
                GetMenu();

                Dictionary<string, object> DICC = new Dictionary<string, object>();
                DICC.Add("PersonSNO", userInfo.PersonSNO);
                DataHelper DHPER = new DataHelper();
                DataTable DTT = DHPER.queryData(@"SELECT COUNT (*) AS TOC FROM TODO WHERE STATE = 0 AND getPersonSNO = @PersonSNO", DICC);

                if (DTT.Rows[0]["TOC"].ToString() != "0")
                {
                    TOM.Text = DTT.Rows[0]["TOC"].ToString();
                    if (TOM.Text.Length == 1)
                    {
                        TOM.Text = "0" + TOM.Text;
                    }
                }
                else
                {
                    TOC.Visible = false;
                }

            }
        }


    }

    protected void CheckAdmin()
    {

        //砍掉所有管理頁面的Utility.getRole.....
        //將有無該功能的管理權限的判斷寫在此處
        string pageFileName = System.IO.Path.GetFileName(Request.PhysicalPath);
        string pageQuery = "?st=" + Request.QueryString["st"];
        pageFileName = pageFileName.Replace("_AE.aspx", ".aspx");
        //pageFileName = pageFileName.Replace("Detail.aspx", ".aspx");

        if (pageFileName == "TODOMGT.aspx" || pageFileName == "Default.aspx")
        { 
            //允許訪問
        }
        else
        {

            //判斷帳號擁有該功能的權限
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("pageFileName", pageFileName);
            aDict.Add("pageQuery", pageQuery);
            aDict.Add("RoleSNO", userInfo.RoleSNO);
            DataHelper objDH = new DataHelper();
            string sql = @"
            Select PLINKURL,PLINKNAME, ISUPDATE, ISINSERT, ISDELETE 
            From RoleMenu RM
	            Left Join PageLink PL ON PL.PLINKSNO=RM.PLINKSNO And ISVIEW=1
            where RoleSNO=@RoleSNO And (PL.PLINKURL like '%' + @pageFileName Or PL.PLINKURL like '%' + @pageFileName + @pageQuery)
            ";
            DataTable objDT = objDH.queryData(sql, aDict);

            //有權限
            if (objDT.Rows.Count > 0)
            {
                //判斷該權限的insert,delete,update，並寫到userinfo供其他功能讀取
                if ((int)objDT.Rows[0]["ISINSERT"] > 0) userInfo.AdminIsInsert = true; else userInfo.AdminIsInsert = false;
                if ((int)objDT.Rows[0]["ISDELETE"] > 0) userInfo.AdminIsDelete = true; else userInfo.AdminIsDelete = false;
                if ((int)objDT.Rows[0]["ISUPDATE"] > 0) userInfo.AdminIsUpdate = true; else userInfo.AdminIsUpdate = false;

                //判斷AE讀取SNO時是否有權限
                //if AE
            }

            //無權限
            else
            {
                Response.Write("<script>alert('沒有檢視該頁的權限。'); window.location.href='Default.aspx'; </script>");
            }


        }

    }
    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Response.Write("<script>alert('已登出!'); location.href='Notice.aspx';</script>");
    }
    protected void GetMenu()
    {
        //判斷帳號擁有哪些功能列表
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("RoleSNO", userInfo.RoleSNO);

        DataHelper objDH = new DataHelper();
        string sql = @"Select Distinct GROUPORDER,PLINKORDER,P.PPLINKSNO,P.PLINKSNO,PLINKURL,PLINKNAME FROM PageLink P
                        INNER JOIN ROLEMENU M ON M.PLINKSNO=P.PLINKSNO AND RoleSNO=@RoleSNO AND ISVIEW=1 
                       Where ISENABLE=1 ORDER BY GROUPORDER,PLINKORDER ";
        DataTable objDB = objDH.queryData(sql, aDict);
        objDB.DefaultView.RowFilter = "PPLINKSNO IS NULL";
        DataTable aDTable = objDB.DefaultView.ToTable();
        ltl_MenuBar.Text = "<ul id=\"MenuBar1\" class=\"MenuBarHorizontal nav L7 w10\">";
        for (int i = 0; i < aDTable.Rows.Count; i++)
        {
            ltl_MenuBar.Text += "<li>";
            ltl_MenuBar.Text += String.Format("<a class=\"MenuBarItemSubmenu\" href=\"#\" onclick=\"return false;\"><i class=\"fa fa-newspaper\" aria-hidden=\"true\"></i> {0}</a>", Convert.ToString(aDTable.Rows[i]["PLINKNAME"]));
            ltl_MenuBar.Text += GetSubMenu(objDB, Convert.ToString(aDTable.Rows[i]["PLINKSNO"]));
            ltl_MenuBar.Text += "</li>";
        }
        ltl_MenuBar.Text += "</ul>";
    }

    protected String GetSubMenu(DataTable pDTable, String pPPLinkSNO)
    {
        String SubMenu = "";
        pDTable.DefaultView.RowFilter = "PPLINKSNO = '" + pPPLinkSNO + "'";
        DataTable aDTable = pDTable.DefaultView.ToTable();
        if (aDTable.Rows.Count > 0)
        {
            SubMenu = "<ul>";
            for (int i = 0; i < aDTable.Rows.Count; i++)
            {
                SubMenu += "<li>";
                SubMenu += String.Format("<a href=\"{0}\">{1}</a>", Convert.ToString(aDTable.Rows[i]["PLINKURL"]), Convert.ToString(aDTable.Rows[i]["PLINKNAME"]));
                SubMenu += GetSubMenu(pDTable, Convert.ToString(aDTable.Rows[i]["PLINKSNO"]));
                SubMenu += "</li>";
            }
            SubMenu += "</ul>";
        }
        return SubMenu;
    }

    protected void rpt_Link_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var FoundRepeater = e.Item.FindControl("rpt_link") as Repeater;
            if (FoundRepeater != null)
            {
                SubLinkByCategory(FoundRepeater, DataBinder.Eval(e.Item.DataItem, "GROUPORDER").ToString());
            }
        }
    }

    protected void SubLinkByCategory(Repeater theRepeater, string param)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("GROUPORDER", param);
        string strSqlQry = @"Select * from PageLink Where GROUPORDER = @GROUPORDER And ISDIR=0 ORDER BY PLINKSNO";

        DataHelper objDH = new DataHelper();
        theRepeater.DataSource = objDH.queryData(strSqlQry, dic);
        theRepeater.DataBind();
    }
    protected void AutoSignInELearning()
    {
        //string url_course = "https://healthtraining.elearning.hpa.gov.tw/api/sso/generate-url?key=tAfx7FaLHGz6Vd3xFR0j";
        string url_course = "https://hpaqs.mydevhost.com/qsms-api/sso/generate-url?key=UoLgyT3cLMeM9jAu0smB"; 


        string param = "";
        param += "firstName=" + userInfo.UserName.Substring(1);
        param += "&lastName=" + userInfo.UserName.Substring(0, 1);
        //param += "&username=" + userInfo.UserAccount;
        param += "&username=" + userInfo.PersonID;
        param += "&idNumber=" + userInfo.PersonID;
        param += "&email=" + userInfo.UserMail;
        //param += "&courseId=" + ELCode;
        //firstName(必要) 名字
        //lastName(必要) 姓氏
        //username(必要) 帳號* 身分證字號*
        //email(必要) 電子郵件
        //courseId(選擇性) 課程Id，如果有填，登入後會自動導向課程頁面


        //強制認為憑證都是通過的，特殊情況再使用
        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  //因應HTTPS調整

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url_course);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";

        //要發送的字串轉為byte[] 
        byte[] byteArray = Encoding.UTF8.GetBytes(param);
        using (Stream reqStream = request.GetRequestStream())
        {
            reqStream.Write(byteArray, 0, byteArray.Length);

        }//end using

        //API回傳的字串
        string responseStr = "";
        using (WebResponse response = request.GetResponse())
        {
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseStr = sr.ReadToEnd();
            }//end using  
        }

        responseStr = responseStr.Replace("{\"loginUrl\":\"", "");
        responseStr = responseStr.Replace("\"}", "");
        responseStr = responseStr.Replace("\\", "");


        hyperlink1.Attributes.Add("href",responseStr);

        string js = "window.open('" + responseStr + "', '_blank')";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "openEL", js, true);

        //return "";
    }

    protected void hyperlink1_Click(object sender, EventArgs e)
    {
        AutoSignInELearning();
    }

    protected void btn_Back_Click(object sender, EventArgs e)
    {
        Response.Redirect("../web/Notice.aspx");
    }
}
