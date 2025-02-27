using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Notice_AE : System.Web.UI.Page
{

    UserInfo userInfo = null;
    DateTime NowTime = Convert.ToDateTime(DateTime.Now);

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
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

        //if (txt_Title.Text.Length > 50) errorMessage += "標題字數過多\\n";
        if (txt_Title.Text.Length == 0) errorMessage += "標題字數錯誤\\n";
        if (ddl_Class.SelectedValue == "") errorMessage += "請選擇分類\\n";

        //比較結束日期和開始日期
        DateTime start = Convert.ToDateTime(txt_SDate.Text);
        DateTime end = Convert.ToDateTime(txt_EDate.Text);
        if (start > end) errorMessage += "結束日期小於開始日期!\\n";
        
        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        if (Work.Value.Equals("NEW"))
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("OrderSeq", ddl_OrderSeq.SelectedValue);
            aDict.Add("Title", txt_Title.Text);
            aDict.Add("Info", HttpUtility.HtmlEncode(editor1.Value));
            aDict.Add("SDate", txt_SDate.Text);
            aDict.Add("EDate", txt_EDate.Text + " 23:59:00");
            aDict.Add("SYSTEM_ID", "S00");
            aDict.Add("NoticeCSNO", ddl_Class.SelectedValue);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            aDict.Add("ModifyDT", NowTime);
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            aDict.Add("Show", chk_view.Checked);
            DataHelper objDH = new DataHelper();

            string OrderSeq = "";
            string OrderSeqV = "";
            if (ddl_OrderSeq.SelectedValue != "")
            {
                OrderSeq = ",OrderSeq";
                OrderSeqV = ",@OrderSeq";
            }
            string Strsql = @"
            Insert Into Notice(Title,Info,SDate,EDate,CreateUserID,SYSTEM_ID,NoticeCSNO,ModifyDT,ModifyUserID"+ OrderSeq + @",Show) 
            Values(@Title,@Info,@SDate,@EDate,@CreateUserID,@SYSTEM_ID,@NoticeCSNO,@ModifyDT,@ModifyUserID "+ OrderSeqV + @",@Show) 
            SELECT @@IDENTITY AS 'Identity'";

            DataTable dt = objDH.queryData(Strsql, aDict);

            //寫入適用人員
            Utility.insertRoleBind(cb_Role, dt.Rows[0]["Identity"].ToString(), "Notice_AE", userInfo.PersonSNO);

            Response.Write("<script>alert('新增成功!');document.location.href='./Notice.aspx'; </script>");
        }
        else
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("NoticeSNO", txt_ID.Value);
            aDict.Add("OrderSeq", ddl_OrderSeq.SelectedValue);
            aDict.Add("Title", txt_Title.Text);
            aDict.Add("SDate", txt_SDate.Text);
            aDict.Add("EDate", txt_EDate.Text + " 23:59:00");
            aDict.Add("Info", HttpUtility.HtmlEncode(editor1.Value));
            aDict.Add("SYSTEM_ID", "S00");
            aDict.Add("NoticeCSNO", ddl_Class.SelectedValue);
            aDict.Add("ModifyDT", NowTime);
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            aDict.Add("Show", chk_view.Checked);
            DataHelper objDH = new DataHelper();
            if (ddl_OrderSeq.SelectedValue == "")
            {
                objDH.executeNonQuery("Update Notice Set OrderSeq=null,Title=@Title,SDate=@SDate,EDate=@EDate,Info=@Info,SYSTEM_ID=@SYSTEM_ID,NoticeCSNO=@NoticeCSNO,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID,Show=@Show Where NoticeSNO=@NoticeSNO", aDict);
            }
            else
            {
                objDH.executeNonQuery("Update Notice Set OrderSeq=@OrderSeq,Title=@Title,SDate=@SDate,EDate=@EDate,Info=@Info,SYSTEM_ID=@SYSTEM_ID,NoticeCSNO=@NoticeCSNO,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID,Show=@Show Where NoticeSNO=@NoticeSNO", aDict);
            }

            //寫入適用人員
            Utility.insertRoleBind(cb_Role, txt_ID.Value, "Notice_AE", userInfo.PersonSNO);

            Response.Write("<script>alert('修改成功!');document.location.href='./Notice.aspx'; </script>");
        }
    }

    protected void newData()
    {
        Work.Value = "NEW";
        setNoticeClass(ddl_Class, "請選擇");
        Utility.setRoleBind(cb_Role, "0", "");

    }

    protected void getData()
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("sno", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"select NoticeSNO,OrderSeq,Title,SDate, Edate,N.CreateUserID,N.CreateDT,NoticeCSNO,info,S.SYSTEM_NAME,S.SYSTEM_ID,Show
                                            from Notice N
                                            LEFT JOIN SYSTEM S on N.SYSTEM_ID=S.SYSTEM_ID
                                            Where NoticeSNO=@sno"
                                        , aDict);
        if (objDT.Rows.Count > 0)
        {
            ddl_OrderSeq.SelectedValue = Convert.ToString(objDT.Rows[0]["OrderSeq"]);
            txt_ID.Value = Convert.ToString(objDT.Rows[0]["NoticeSNO"]);
            txt_Title.Text = Convert.ToString(objDT.Rows[0]["Title"]);
            editor1.Value = HttpUtility.HtmlDecode(Convert.ToString(objDT.Rows[0]["info"]));
            setNoticeClass(ddl_Class, null);
            ddl_Class.SelectedValue = Convert.ToString(objDT.Rows[0]["NoticeCSNO"]);
            txt_SDate.Text = Convert.ToDateTime(objDT.Rows[0]["SDate"]).ToString("yyyy-MM-dd");
            txt_EDate.Text = Convert.ToDateTime(objDT.Rows[0]["EDate"]).ToString("yyyy-MM-dd");
            if (!String.IsNullOrEmpty(objDT.Rows[0]["Show"].ToString()))
            {
                chk_view.Checked = Convert.ToBoolean(objDT.Rows[0]["Show"]);
            }
           
            Utility.setRoleBind(cb_Role, txt_ID.Value, "Notice_AE");
        }

    }

    public static void setNoticeClass(System.Web.UI.WebControls.DropDownList ddl, String ClassID, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY NoticeCSNO) as ROW_NO,NoticeCSNO,Name  FROM NoticeClass", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }

}