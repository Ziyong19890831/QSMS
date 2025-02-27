using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class Web_UToDo : System.Web.UI.Page
{
    public string Name = "";
    public string PersonID = "";
    public string Email = "";
    UserInfo userInfo = null;
    object Debug = System.Web.Configuration.WebConfigurationManager.AppSettings["Debug"];
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
        if (Session["QSMS_UserInfo"] == null) { Response.Redirect("Notice.aspx"); }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["QSMS_UserInfo"] != null)
            {
                bindData(1);

                //查詢使用者資料，準備post用
                DataHelper objDH = new DataHelper();
                Dictionary<string, object> aDict = new Dictionary<string, object>();
                String sql = @"
            SELECT * FROM Person WHERE PersonSNO = @PersonSNO
            ";

                aDict.Add("PersonSNO", userInfo.PersonSNO);

                DataTable objDT = objDH.queryData(sql, aDict);

                Name = objDT.Rows[0]["PNAME"].ToString();
                PersonID = objDT.Rows[0]["PersonID"].ToString();
                Email = objDT.Rows[0]["PMail"].ToString();


            }

        }
    }
    protected void bindData(int page)
    {
        //查詢該使用者有的系統審核
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        if (page < 1) page = 1;
        int pageRecord = 10;
        String sql = @"
            SELECT
	ROW_NUMBER () OVER (ORDER BY PD.PersonDSNO) AS ROW_NO ,SM.SYSTEM_NAME,PD.SysPAccountIsUser,PD.CreateDT
FROM
	PersonD PD
LEFT JOIN SYSTEM SM ON PD.SYSTEM_ID = SM.SYSTEM_ID
WHERE
	PersonID = @PersonID
            ";

        aDict.Add("PersonID", userInfo.PersonID);

        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);


        rpt_QA.DataSource = objDT.DefaultView;
        rpt_QA.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);

        //變更審核狀態名稱
        foreach (RepeaterItem item in rpt_QA.Items)
        {
            Label lbl_apState = (Label)item.FindControl("Label2");
            if(lbl_apState.Text== "N")
            {
                lbl_apState.Text = "停權";
            }
            else if (lbl_apState.Text == "Y")
            {
                lbl_apState.Text = "已核准啟用";
            }
            else if (lbl_apState.Text == "D")
            {
                lbl_apState.Text = "系統審核中";
            }
            else if (lbl_apState.Text == "S")
            {
                lbl_apState.Text = "核退";
            }
        }

        //查詢該使用者是否有核退的系統
        string psysSQL = @"
SELECT SYSTEM_ID FROM PersonD WHERE PersonID= @PersonID AND SysPAccountIsUser <> 'S'
";
        DataTable objDTs = objDH.queryData(psysSQL, aDict);


        aDict.Clear();

        string sqls = @"
SELECT
	SM.SYSTEM_NAME,
	SM.SYSTEM_ID,
  SD.AP_ApplyURL,
SD.unIcon
FROM
	SYSTEM SM
LEFT JOIN SYSTEMD SD ON SM.SYSTEM_ID = SD.SYSTEM_ID
WHERE
	SM.ISEnable > 0
AND SM.SYSTEM_ID <> 'S00'
";
        //濾掉該使用者為核退系統，不得重複申請
        if (objDTs.Rows.Count != 0)
        {
            for (int i = 0; i <= objDTs.Rows.Count - 1; i++)
            {
                sqls += " AND SM.SYSTEM_ID <> '" + objDTs.Rows[i]["SYSTEM_ID"].ToString() + "'";
            }

        }

        DataTable objDTe = objDH.queryData(sqls, aDict);

        if (objDT.Rows.Count == 0)
        {
            //此人無任何申請系統
            Panel1.Visible = false;
        }
        else
        {
            //繫結可申請系統
            rptsystemm.DataSource = objDTe;
            rptsystemm.DataBind();
        }
        


    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        bindData(page);
    }

    protected void btn_submit_ServerClick(object sender, EventArgs e) //送出按鈕
    {
        int aa=0; //判斷是否勾選系統申請
        foreach (RepeaterItem item in rptsystemm.Items)
        {
            HtmlInputCheckBox chkDisplayTitle = (HtmlInputCheckBox)item.FindControl("selectit");
            if (chkDisplayTitle.Checked) //有勾選系統申請寫入資料庫
            {
                aa++;
                DataHelper objDH = new DataHelper();
                Dictionary<string, object> dicpd = new Dictionary<string, object>();
                dicpd.Add("SYSTEM_ID", chkDisplayTitle.Attributes["sid"]);
                dicpd.Add("SysPAccount", userInfo.UserAccount);
                dicpd.Add("sysPName", userInfo.UserName);
                dicpd.Add("SysPAccountIsUser", "D");
                dicpd.Add("sysPMail", userInfo.UserMail);
                dicpd.Add("PersonID", userInfo.PersonID);
                string sqlpersond = @"
                    Insert Into PersonD
                   (SYSTEM_ID,SysPAccount,sysPName,SysPAccountIsUser,sysPMail,PersonID,CreateUserID) 
            Values(@SYSTEM_ID,@SysPAccount,@sysPName,@SysPAccountIsUser,@sysPMail,@PersonID,1)";
                objDH.executeNonQuery(sqlpersond, dicpd);
            }
        }
        if (aa != 0)
        {
            
        }
        else
        {
            Response.Write("<script>alert('申請完成!'); location.href='UToDo.aspx';</script>");
        }
    }

}