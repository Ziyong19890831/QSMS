using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_PageLink_AE : System.Web.UI.Page
{
    //使用者資訊
    UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null)
        {
            userInfo = (UserInfo)Session["QSMS_UserInfo"];
        }
        if (!IsPostBack)
        {
            //頁面類型
            ddl_ISDIR.Items.Add(new ListItem("頁面", "0"));
            ddl_ISDIR.Items.Add(new ListItem("資料夾", "1"));
            //狀態
            ddl_ISENABLE.Items.Add(new ListItem("啟用", "1"));
            ddl_ISENABLE.Items.Add(new ListItem("停用", "0"));
            //取得頁面父節點
            Utility.setPPLink(ddl_PPLinkSNO, "請選擇");

            String work = "";
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
            if (work.Equals("N"))
            {
                newData();
            }
            else
            {
                try
                {
                    getData();
                }
                catch (Exception ex)
                {
                    Utility.showMessage(this, "ErrorMessage", "取值失敗!");
                    return;
                }
                
            }
        }
    }
    protected void newData()
    {
        Work.Value = "NEW";
        //Button1.Text = "新增";
    }
    protected void getData()
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("sno", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select * From PageLink Where PLINKSNO=@sno", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_PLinkSNO.Value = Convert.ToString(objDT.Rows[0]["PLINKSNO"]);
            txt_PLinkName.Text = Convert.ToString(objDT.Rows[0]["PLINKNAME"]);
            txt_PLinkUrl.Text = Convert.ToString(objDT.Rows[0]["PLINKURL"]);
            ddl_ISDIR.SelectedValue = Convert.ToString(objDT.Rows[0]["ISDIR"]);
            txt_ISDIR_S.Value = Convert.ToString(objDT.Rows[0]["ISDIR"]);
            ddl_ISENABLE.SelectedValue = Convert.ToString(objDT.Rows[0]["ISENABLE"]);
            ddl_PPLinkSNO.SelectedValue = Convert.ToString(objDT.Rows[0]["PPLINKSNO"]);
            ddl_ISDIR.Attributes.Add("disabled", "disable");
            if (Convert.ToString(objDT.Rows[0]["ISDIR"]).Equals("1"))
            {//資料夾
                txt_Order.Text = Convert.ToString(objDT.Rows[0]["GROUPORDER"]);
                ddl_PPLinkSNO.Attributes.Add("disabled", "disable");
            }
            else
            {//頁面
                txt_Order.Text = Convert.ToString(objDT.Rows[0]["PLINKORDER"]);
            }
        }
        //Button1.Text = "修改";
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        String errorMessage = "";

        //頁面名稱
        if (txt_PLinkName.Text.Length > 60)
        {
            errorMessage += "頁面名稱字元過多！\\n";
        }
        if (String.IsNullOrEmpty(txt_PLinkName.Text))
        {
            errorMessage += "請輸入頁面名稱！\\n";
        }
        //頁面網址
        if(txt_PLinkUrl.Text.Length>200)
        {
            errorMessage += "頁面網址字元過多!\\n";
        }
        if (String.IsNullOrEmpty(txt_PLinkUrl.Text))
        {
            errorMessage += "請輸入頁面網址！\\n";
        }
        //頁面類型
        if (String.IsNullOrEmpty(ddl_ISDIR.SelectedValue))
        {
            errorMessage += "請選擇頁面類型！\\n";
        }
        //狀態
        if (String.IsNullOrEmpty(ddl_ISENABLE.SelectedValue))
        {
            errorMessage += "請選擇狀態！\\n";
        }
        //頁面父節點
        if (Work.Value.Equals("NEW"))
        {
            if (!ddl_ISDIR.SelectedValue.Equals("1"))
            {//當頁面類型為頁面時需選擇[頁面父節點]，為資料來時頁面父節點不使用
                if (String.IsNullOrEmpty(ddl_PPLinkSNO.SelectedValue))
                {
                    errorMessage += "請選擇頁面父節點！\\n";
                }
            }
        }
        else
        {
            ddl_ISDIR.SelectedValue = txt_ISDIR_S.Value;
            if (!txt_ISDIR_S.Value.Equals("1"))
            {//當頁面類型為頁面時需選擇[頁面父節點]，為資料來時頁面父節點不使用
                if (String.IsNullOrEmpty(ddl_PPLinkSNO.SelectedValue))
                {
                    errorMessage += "請選擇頁面父節點！\\n";
                }
            }
        }
        //順序
        int aOrder = 0;
        if (String.IsNullOrEmpty(txt_Order.Text))
        {
            errorMessage += "請輸入順序！\\n";
        }
        else
        {
            if (!int.TryParse(txt_Order.Text, out aOrder))
            {
                errorMessage += "順序請輸入數字！\\n";
            }
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
            aDict.Add("PLINKNAME", txt_PLinkName.Text);
            aDict.Add("PLINKURL", txt_PLinkUrl.Text);
            aDict.Add("ISDIR", ddl_ISDIR.SelectedValue);
            aDict.Add("ISENABLE", ddl_ISENABLE.SelectedValue);
            aDict.Add("ORDER", aOrder);
            aDict.Add("CreateUserID", userInfo.PersonSNO);

            DataHelper objDH = new DataHelper();
            ////判斷角色名稱唯一性
            //DataTable objDT = objDH.queryData("Select RoleName From Role Where RoleName=@RoleName", aDict);
            //if (objDT.Rows.Count > 0)
            //{
            //    Utility.showMessage(Page, "ErrorMessage", String.Format("[{0}]角色名稱已存在！\\n", txt_RoleName.Text));
            //    return;
            //}
            if (ddl_ISDIR.SelectedValue.Equals("1"))
            {//資料夾
                objDH.executeNonQuery("Insert Into PageLink(PLINKNAME,PLINKURL,ISDIR,ISENABLE,PPLINKSNO,PLINKORDER,GROUPORDER,CreateUserID) Values(@PLINKNAME,@PLINKURL,1,@ISENABLE,NULL,0,@ORDER,@CreateUserID)", aDict);
            }
            else
            {//頁面
                aDict.Add("PPLINKSNO", ddl_PPLinkSNO.SelectedValue);
                objDH.executeNonQuery("Insert Into PageLink(PLINKNAME,PLINKURL,ISDIR,ISENABLE,PPLINKSNO,PLINKORDER,GROUPORDER,CreateUserID) Values(@PLINKNAME,@PLINKURL,0,@ISENABLE,@PPLINKSNO,@ORDER,(SELECT TOP 1 GROUPORDER FROM PageLink WHERE PLINKSNO=@PPLINKSNO),@CreateUserID)", aDict);
            }
            Response.Write("<script>alert('新增成功!');document.location.href='./PageLink.aspx'; </script>");
        }
        else
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("PLINKSNO", txt_PLinkSNO.Value);
            aDict.Add("PLINKNAME", txt_PLinkName.Text);
            aDict.Add("PLINKURL", txt_PLinkUrl.Text);
            aDict.Add("ISENABLE", ddl_ISENABLE.SelectedValue);
            aDict.Add("ORDER", aOrder);
            aDict.Add("ModifyDT", Convert.ToDateTime(DateTime.Now));
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            DataHelper objDH = new DataHelper();
            ////判斷角色名稱唯一性
            //DataTable objDT = objDH.queryData("Select RoleName From Role Where RoleID<>@RoleID AND RoleName=@RoleName", aDict);
            //if (objDT.Rows.Count > 0)
            //{
            //    Utility.showMessage(Page, "ErrorMessage", String.Format("[{0}]角色名稱已存在！\\n", txt_RoleName.Text));
            //    return;
            //}
            if (txt_ISDIR_S.Value.Equals("1"))
            {//資料夾
                objDH.executeNonQuery("Update PageLink Set PLINKNAME=@PLINKNAME,PLINKURL=@PLINKURL,ISENABLE=@ISENABLE,PPLINKSNO=NULL,PLINKORDER=0,GROUPORDER=@ORDER,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID WHERE PLINKSNO=@PLINKSNO", aDict);
            }
            else
            {//頁面
                aDict.Add("PPLINKSNO", ddl_PPLinkSNO.SelectedValue);
                objDH.executeNonQuery("Update PageLink Set PLINKNAME=@PLINKNAME,PLINKURL=@PLINKURL,ISENABLE=@ISENABLE,PPLINKSNO=@PPLINKSNO,PLINKORDER=@ORDER,GROUPORDER=(SELECT TOP 1 GROUPORDER FROM PageLink WHERE PLINKSNO=@PPLINKSNO),ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID WHERE PLINKSNO=@PLINKSNO", aDict);
            }
            Response.Write("<script>alert('修改成功!');document.location.href='./PageLink.aspx'; </script>");
        }
    }
}