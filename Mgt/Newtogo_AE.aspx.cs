using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Newtogo : System.Web.UI.Page
{
    UserInfo userInfo = null;
    DateTime NowTime = Convert.ToDateTime(DateTime.Now);

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String work = "";
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
            if (work.Equals("N"))
            {
                A.Visible = false;
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
        
        if (txt_Name.Text.Length >= 50) errorMessage += "檔案名稱字數過多\\n";
        if (txt_Name.Text.Length == 0) errorMessage += "檔案名稱字數錯誤\\n";
        int size = fileup_New.PostedFile.ContentLength;
        if (size > 30720000) errorMessage += "檔案不得大於30M\\n";

        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        if (Work.Value.Equals("NEW"))
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("NHName", txt_Name.Text);
            aDict.Add("ISENABLE", 1);
            aDict.Add("SYSTEM_ID", "S00");
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            aDict.Add("ModifyDT", NowTime);
            aDict.Add("ModifyUserID", userInfo.PersonSNO);

            if (fileup_New.HasFile)
            {

                fileup_New.SaveAs(Server.MapPath("../NewHand") + "/" + fileup_New.FileName);
                aDict.Add("NHPath", fileup_New.FileName);

                DataHelper objDH = new DataHelper();
                string Straql = "Insert Into NewHand(NHName,ISENABLE,NHPath,SYSTEM_ID,CreateUserID,ModifyDT,ModifyUserID) Values(@NHName,@ISENABLE,@NHPath,@SYSTEM_ID,@CreateUserID,@ModifyDT,@ModifyUserID) SELECT @@IDENTITY AS 'Identity'";
                DataTable dt = objDH.queryData(Straql, aDict);

                //寫入適用人員
                Utility.insertRoleBind(cb_Role, dt.Rows[0]["Identity"].ToString(), "Newtogo_AE", userInfo.PersonSNO);

                Response.Write("<script>alert('新增成功!');document.location.href='./Newtogo.aspx'; </script>");
            }
            else
            {
                //警告沒東西
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('沒有檔案')", true);
            }
        }
        else
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("NHSNO", txt_ID.Value);
            aDict.Add("NHName", txt_Name.Text);
            aDict.Add("SYSTEM_ID", "S00");
            aDict.Add("ModifyDT", NowTime);
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            if (fileup_New.HasFile)
            {
                Utility.deleteNewhandFile(Server.MapPath("../Newhand"), txt_ID.Value);
                fileup_New.SaveAs(Server.MapPath("../NewHand") + "/" + fileup_New.FileName);
                aDict.Add("NHPath", fileup_New.FileName);
            }
			
			
            DataHelper objDH = new DataHelper();
            if (fileup_New.HasFile == true)
            {
                objDH.executeNonQuery("Update NewHand Set NHName=@NHName,NHPath=@NHPath,SYSTEM_ID=@SYSTEM_ID,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID Where NHSNO=@NHSNO", aDict);
            }
            else
            {
                objDH.executeNonQuery("Update NewHand Set NHName=@NHName,SYSTEM_ID=@SYSTEM_ID,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID Where NHSNO=@NHSNO", aDict);
            }

            //寫入適用人員
            Utility.insertRoleBind(cb_Role, txt_ID.Value, "Newtogo_AE", userInfo.PersonSNO);

            Response.Write("<script>alert('修改成功!');document.location.href='./Newtogo.aspx'; </script>");
        }
    }

    protected void newData()
    {
        Work.Value = "NEW";
        Utility.setRoleBind(cb_Role, "0", "");
    }

    protected void getData()
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("sno", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"select N.*,S.SYSTEM_NAME,S.SYSTEM_ID
                                            from NewHand N
                                            LEFT JOIN SYSTEM S on N.SYSTEM_ID=S.SYSTEM_ID
                                            Where NHSNO=@sno"
                                        , aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_ID.Value = Convert.ToString(objDT.Rows[0]["NHSNO"]);
            txt_Name.Text = Convert.ToString(objDT.Rows[0]["NHName"]);
            Utility.setRoleBind(cb_Role, txt_ID.Value, "Newtogo_AE");
            getfiles();
        }
    }

    private void getfiles()
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("sno", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("select * from NewHand Where NHSNO=@sno", aDict);
        if (objDT.Rows.Count > 0)
        {
            lit_Newtogo.Text = "<a href='../NewHand/" + objDT.Rows[0]["NHPath"] + "' style='color:blue'>" + objDT.Rows[0]["NHPath"] + "</a >";
        }
    }

}