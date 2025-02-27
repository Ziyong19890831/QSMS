using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_QA_AE : System.Web.UI.Page
{
    UserInfo userInfo = null;
    DateTime NowTime = Convert.ToDateTime(DateTime.Now);

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            setQAClass(ddl_Class, "請選擇");
            setClassSystem(ddl_SystemName, "請選擇");
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
    }

    protected void ButtonOK_Click(object sender, EventArgs e)
    {
        String errorMessage = "";
        //選項
        if (ddl_Class.SelectedValue == "")
        {
            errorMessage += "請選擇分類!\\n";
        }
        //問題
        if (txt_Title.Text.Length > 50)
        {
            errorMessage += "問題字數過多!\\n";
        }
        if (txt_Title.Text.Length == 0)
        {
            errorMessage += "請輸入問題!\\n";
        }
        if (txt_Info.Text.Length > 4000)
        {
            errorMessage += "回答字數過多!\\n";
        }
        if (txt_Info.Text.Length == 0)
        {
            errorMessage += "請輸入回答!\\n";
        }
        if (ddl_SystemName.SelectedValue == "")
        {
            errorMessage += "請選擇系統\\n";
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
            aDict.Add("Title", txt_Title.Text);
            aDict.Add("Info", txt_Info.Text);
            aDict.Add("SYSTEM_ID", ddl_SystemName.SelectedValue);
            aDict.Add("QACSNO", ddl_Class.SelectedValue);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            aDict.Add("SDate", "2000-01-01");
            aDict.Add("EDate", "2500-01-01");
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Insert Into QA(Title,Info,CreateUserID,SYSTEM_ID,QACSNO,SDate,EDate) Values(@Title,@Info,@CreateUserID,@SYSTEM_ID,@QACSNO,@SDate,@EDate)", aDict);
            Response.Write("<script>alert('新增成功!');document.location.href='./QA.aspx'; </script>");
        }
        else
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("Title", txt_Title.Text);
            aDict.Add("Info", txt_Info.Text);
            aDict.Add("SYSTEM_ID", ddl_SystemName.SelectedValue);
            aDict.Add("QACSNO", ddl_Class.SelectedValue);
            aDict.Add("QASNO", txt_ID.Value);
            aDict.Add("ModifyDT", NowTime);
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Update QA Set Title=@Title,Info=@Info,QACSNO=@QACSNO,SYSTEM_ID=@SYSTEM_ID,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID Where QASNO=@QASNO", aDict);
            Response.Write("<script>alert('修改成功!');document.location.href='./QA.aspx'; </script>");
        }

    }

    protected void newData()
    {
        Work.Value = "NEW";
        ButtonOK.Text = "新增";
    }

    protected void getData()
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("sno", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"select QASNO,Q.Title,Q.CreateUserID,Q.CreateDT,Q.QACSNO,Q.Info,C.Name as ClassName,S.SYSTEM_NAME,S.SYSTEM_ID
                                            from QA Q 
                                            LEFT JOIN QAClass C ON Q.QACSNO=C.QACSNO
                                            LEFT JOIN SYSTEM S on Q.SYSTEM_ID=S.SYSTEM_ID
                                            Where QASNO=@sno                                         
                                            ", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_ID.Value = Convert.ToString(objDT.Rows[0]["QASNO"]);
            txt_Title.Text = Convert.ToString(objDT.Rows[0]["Title"]);
            txt_Info.Text = Convert.ToString(objDT.Rows[0]["Info"]);
            ddl_SystemName.SelectedValue = Convert.ToString(objDT.Rows[0]["SYSTEM_ID"]);
            ddl_Class.SelectedValue = Convert.ToString(objDT.Rows[0]["QACSNO"]);

        }
    }

    public static void setQAClass(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY QACSNO) as ROW_NO,QACSNO,Name  FROM QAClass", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }
    public static void setClassSystem(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER(ORDER BY SYSTEMSNO) as ROW_NO, SYSTEMSNO, SYSTEM_ID, SYSTEM_NAME FROM SYSTEM where ISEnable > 0 ", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        //if (DefaultString != null)
        //{
        //    ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        //}
    }
}