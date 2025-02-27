using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_GetPAccount : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {

            //初始化:角色別
           

            //行政區初始化
            Utility.setAreaCodeA(ddl_AreaCodeA, "請選擇");
            if (ddl_AreaCodeA.SelectedValue == "")
            {
                ddl_AreaCodeB.Enabled = false;
                ddl_OrganCode.Enabled = false;
            }

        }
    }

    protected void btn_Reset_Click(object sender, EventArgs e)
    {
        GetPassword();
    }

    protected void GetPassword()
    {

        String errorMessage = "";
        //身分證
        if (String.IsNullOrEmpty(txt_PersonID.Value)) errorMessage += "身分證不可為空白！\\n";

        //醫事機構代碼
        if (String.IsNullOrEmpty(HF_OrganSNO.Value)) errorMessage += "醫事機構代碼不可為空白！\\n";

        //信箱
        if (String.IsNullOrEmpty(txt_Mail.Value)) errorMessage += "信箱不可為空白！\\n";

        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }
       
        bool ExistsPAccount = GetPasswordAndSendMail(txt_PersonID.Value, txt_Mail.Value, HF_OrganSNO.Value , txt_Organ.Value);
        if (ExistsPAccount)
            Response.Write("<script>alert('您的帳號已寄送，請至您登記的信箱收取帳號！'); location.href='Notice.aspx';</script>");
        else
            Response.Write("<script>alert('您醫事機構代碼或信箱或身分證不正確！'); </script>");

    }

    public bool GetPasswordAndSendMail(string PersonID, string PMail, string OrganSNO ,string txt_Organ)
    {

        //確認是否有PAccount&PMail
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonID", PersonID);
        aDict.Add("PMail", PMail);
        aDict.Add("OrganSNO", OrganSNO);
        string sql = @"Select PAccount,OrganCode From Person P 
                        Left Join Organ O On O.OrganSNO=P.OrganSNO
                        Where PMail=@PMail And PersonID=@PersonID And O.OrganSNO=@OrganSNO";
        DataTable objDT = objDH.queryData(sql, aDict);
        bool CheckOrganCode = Utility.CheckPeronMPOrganCode(PersonID, txt_Organ);
        if (CheckOrganCode)
        {
            if (objDT.Rows.Count > 0)
            {
                string PAccount = objDT.Rows[0]["PAccount"].ToString();
                //取得mail寄送樣板路徑，以及讀取內容
                string getTemplate = System.IO.File.ReadAllText(Server.MapPath("../SysFile/TemplateMailForGetPAccount.html"));
                getTemplate = getTemplate.Replace("@RestDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                getTemplate = getTemplate.Replace("@RestNewPassword", PAccount);
                //發送取得帳號信件，依照各系統的發送mail配置自行調整
                Utility.SendMail("帳號取得通知信件", getTemplate, PMail);
                return true;
            }
            else
            {
                Response.Write("<script>alert('尚未在本系統建立帳號！'); </script>");
                return false;
            }
               
        }
        else
        {
          
            if (objDT.Rows.Count > 0)
            {


                string PAccount = objDT.Rows[0]["PAccount"].ToString();
                //取得mail寄送樣板路徑，以及讀取內容
                string getTemplate = System.IO.File.ReadAllText(Server.MapPath("../SysFile/TemplateMailForGetPAccount.html"));
                getTemplate = getTemplate.Replace("@RestDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                getTemplate = getTemplate.Replace("@RestNewPassword", PAccount);
                //發送取得帳號信件，依照各系統的發送mail配置自行調整
                Utility.SendMail("帳號取得通知信件", getTemplate, PMail);

                return true;
            }
            else
            {
                return false;
            }
        }
       

    }
    protected void ddl_AreaCodeA_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_AreaCodeB.Items.Clear();
        ddl_OrganCode.Items.Clear();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String AreaCodeA = ddl_AreaCodeA.SelectedValue;
        if (!String.IsNullOrEmpty(AreaCodeA))
        {
            Utility.setAreaCodeB(ddl_AreaCodeB, AreaCodeA, "請選擇");
            ddl_AreaCodeB.Enabled = true;
        }
        else
        {
            ddl_AreaCodeB.Items.Add(new ListItem("請選擇", ""));
            ddl_AreaCodeB.Enabled = false;
            ddl_OrganCode.Enabled = false;
        }
        ddl_OrganCode.Items.Add(new ListItem("請選擇", ""));
    }

    protected void ddl_AreaCodeB_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_OrganCode.Items.Clear();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String AreaCodeB = ddl_AreaCodeB.SelectedValue;
        if (!String.IsNullOrEmpty(AreaCodeB))
        {
            Utility.setOrganID(ddl_OrganCode, AreaCodeB, "請選擇");
            ddl_OrganCode.Enabled = true;
        }
        else
        {
            ddl_OrganCode.Items.Add(new ListItem("請選擇", ""));
            ddl_OrganCode.Enabled = false;
        }
    }

    protected void ddl_OrganCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        HF_OrganSNO.Value = ddl_OrganCode.SelectedValue; ;
        lb_OrganCodeName.Text = ddl_OrganCode.SelectedItem.Text + "(可使用)";
    }

}