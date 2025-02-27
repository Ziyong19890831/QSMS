using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_AccountAjax : System.Web.UI.Page
{




    protected void Page_Load(object sender, EventArgs e)
    {
        string acc = "";
        string org = "";
        string pid = "";
        string pwd = "";
        string result = "";

        if (Request.Form["personid"] != null)
        {
            pid = Request.Form["personid"].ToString();
        }

        if (Request.Form["account"] != null)
        {
            acc = Request.Form["account"].ToString();
        }

        if (Request.Form["orgid"] != null)
        {
            org = Request.Form["orgid"].ToString();
        }

        if (Request.Form["pwd"] != null)
        {
            pwd = Request.Form["pwd"].ToString();
        }

        if (pid == "0")
        {
            DataHelper odt = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("PAccount", acc);
            DataTable dt_acc = odt.queryData("SELECT * FROM Person WHERE PAccount=@PAccount ", aDict);
            aDict.Clear();


            if (dt_acc.Rows.Count != 0)
            {
                result = "您輸入的帳號已存在";

            }
            else
            {
                result = "可使用";
            }
            Response.Write(result);
            Response.End();
        }
        if (acc == "0")
        {
            DataHelper odt = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("PersonID", pid);
            DataTable dt_pid = odt.queryData("SELECT * FROM Person WHERE PersonID =@PersonID", aDict);
            if (dt_pid.Rows.Count != 0)
            {
                result = "您輸入的身分證已存在";

            }
            else
            {
                result = "可使用";
            }
            Response.Write(result);
            Response.End();
        }

        if (pid == "#")
        {
            DataHelper odt = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("PMail", acc);
            DataTable dt_pid = odt.queryData("SELECT * FROM Person WHERE PMail=@PMail", aDict);
            if (dt_pid.Rows.Count != 0)
            {
                result = "您輸入的信箱已存在";
            }
            else
            {
                result = "可使用";
            }
            Response.Write(result);
            Response.End();
        }
        if (pwd != "")
        {
            DataHelper odt = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("PAccount", acc);
            aDict.Add("PPWD", pwd);
            DataTable dt_pwd = odt.queryData("SELECT * FROM Person WHERE PPWD collate Chinese_Taiwan_Stroke_CS_AS =@PPWD AND PAccount=@PAccount ", aDict);
            aDict.Clear();
            if (dt_pwd.Rows.Count != 0)
            {
                result = "密碼正確";
            }
            else
            {
                result = "您輸入的密碼錯誤";
            }
            Response.Write(result);
            Response.End();
        }
        if (org != "") {

            DataHelper odt = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("OrganCode", org);
            DataTable dt_org = odt.queryData("SELECT * FROM Organ WHERE OrganCode=@OrganCode", aDict);
            if (dt_org.Rows.Count != 0)
            {
                result = "可使用," + dt_org.Rows[0]["OrganSNO"] + "," + dt_org.Rows[0]["OrganCode"] + "-" + dt_org.Rows[0]["OrganName"];
            }
            else
            {
                result = "查無單位代碼";
            }
            Response.Write(result);
            Response.End();
        }

    }
}