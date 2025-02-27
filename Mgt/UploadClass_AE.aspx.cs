using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_UploadClass_AE : System.Web.UI.Page
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
    }

    protected void ButtonOK_Click(object sender, EventArgs e)
    {
        String errorMessage = "";
        //名稱
        if (txt_Name.Text.Length > 50)
        {
            errorMessage += "名稱字元過多！\\n";
        }
        if (txt_Name.Text.Length == 0)
        {
            errorMessage += "請輸入名稱！\\n";
        }
        if (ddl_SystemName.SelectedValue == "")
        {
            errorMessage += "請選擇所屬系統!\\n";
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
            DataHelper objDH = new DataHelper();
            aDict.Add("DLCNAME", txt_Name.Text);
            aDict.Add("ISENABLE", 1);
            aDict.Add("SYSTEM_ID", ddl_SystemName.SelectedValue);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            if (ddl_OrderSeq.SelectedValue != "0")
            {
                aDict.Add("OrderSeq", ddl_OrderSeq.SelectedValue);
                string sql = @"Insert Into DownloadClass(DLCNAME,ISENABLE,SYSTEM_ID,CreateUserID,OrderSeq) Values(@DLCNAME,@ISENABLE,@SYSTEM_ID,@CreateUserID,@OrderSeq)";
                objDH.executeNonQuery(sql, aDict);


            }
            else
            {
                string sql = @"Insert Into DownloadClass(DLCNAME,ISENABLE,SYSTEM_ID,CreateUserID) Values(@DLCNAME,@ISENABLE,@SYSTEM_ID,@CreateUserID)";
                objDH.executeNonQuery(sql, aDict);

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('成功')", true);
            Response.Write("<script>alert('新增成功!');document.location.href='./UploadClass.aspx'; </script>");


        }
        else
        {
            String No = Convert.ToString(Request.QueryString["sno"]);
            DataHelper objDH = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("DLCNAME", txt_Name.Text);
            aDict.Add("DLCSNO", No);
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            if (ddl_OrderSeq.SelectedValue != "0")
            {
                aDict.Add("OrderSeq", ddl_OrderSeq.SelectedValue);
                string sql = @"update DownloadClass set DLCNAME=@DLCNAME ,ModifyUserID=@ModifyUserID,ModifyDT=getdate(),OrderSeq=@OrderSeq  where DLCSNO=@DLCSNO ";
                objDH.executeNonQuery(sql, aDict);
            }

            else
            {
                string sql = @"update DownloadClass set DLCNAME=@DLCNAME ,ModifyUserID=@ModifyUserID,ModifyDT=getdate(),OrderSeq=null  where DLCSNO=@DLCSNO ";
                objDH.executeNonQuery(sql, aDict);
            }
            Response.Write("<script>alert('修改成功!');document.location.href='./UploadClass.aspx'; </script>");

        }
    }

    protected void newData()
    {
        Work.Value = "NEW";
        setClassSystem(ddl_SystemName, "---請選擇系統---");
    }

    protected void getData()
    {
        String No = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("DLCSNO", No);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
        select *
        from DownloadClass D
            LEFT JOIN SYSTEM S on D.SYSTEM_ID=S.SYSTEM_ID
        Where DLCSNO=@DLCSNO
        ", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_Name.Text = objDT.Rows[0]["DLCNAME"].ToString();
        }
        setClassSystem(ddl_SystemName, null);
        ddl_SystemName.SelectedValue = objDT.Rows[0]["SYSTEM_ID"].ToString();
        ddl_OrderSeq.SelectedValue = objDT.Rows[0]["OrderSeq"].ToString();
    }


    public static void setClassSystem(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER(ORDER BY SYSTEMSNO) as ROW_NO, SYSTEMSNO, SYSTEM_ID, SYSTEM_NAME FROM SYSTEM where ISEnable > 0 ", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }
}