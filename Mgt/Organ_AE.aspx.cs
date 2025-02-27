using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Organ_AE : System.Web.UI.Page
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
            Utility.setAreaCodeA_Access(ddl_AreaCodeA, userInfo.AreaCodeA, userInfo.AreaCodeB, userInfo.RoleOrganType, "請選擇");

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

    protected void ddl_AreaCodeA_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_AreaCodeB.Items.Clear();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String AreaCodeA = ddl_AreaCodeA.SelectedValue;
        if (!String.IsNullOrEmpty(AreaCodeA))
        {
            Utility.setAreaCodeB(ddl_AreaCodeB, AreaCodeA, "請選擇");
        }
        else
        {
            ddl_AreaCodeB.Items.Add(new ListItem("請先選擇縣市行政區", ""));
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {

        String errorMessage = "";
        ////單位類別
        //if(ddl_Level.SelectedValue=="")
        //{
        //    errorMessage += "請選擇單位類別!\\n";
        //}
        //單位代碼
        if (txt_Code.Text.Length==0)
        {
            errorMessage += "請輸入代碼!\\n";
        }
        if(txt_Code.Text.Length>20)
        {
            errorMessage += "代碼字元過多!\\n";
        }
        //單位名稱
        if(txt_Name.Text.Length==0)
        {
            errorMessage += "請輸入名稱!\\n";
        }
        if(txt_Name.Text.Length>100)
        {
            errorMessage += "名稱字元過多!\\n";
        }
        //單位行政區
        if(ddl_AreaCodeA.SelectedValue=="" || ddl_AreaCodeB.SelectedValue=="")
        {
            errorMessage += "請選擇行政區!\\n";     
        }
        //聯絡地址
        if(txt_Addr.Text.Length==0)
        {
            errorMessage += "請輸入聯絡地址!\\n";
        }
        if(txt_Addr.Text.Length>60)
        {
            errorMessage += "聯絡地址字元過多!\\n";
        }
        //聯絡電話
        if (txt_Tel.Text.Length==0)
        {
            errorMessage += "請輸入聯絡電話!\\n";
        }
        if(txt_Tel.Text.Length>50)
        {
            errorMessage += "聯絡電話字元過多!\\n";
        }
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        if (Work.Value.Equals("NEW"))
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            //aDict.Add("OrganLevel", ddl_Level.SelectedValue);
            aDict.Add("OrganCode", txt_Code.Text);
            aDict.Add("OrganName", txt_Name.Text);
            aDict.Add("AreaCodeA", ddl_AreaCodeA.SelectedValue);
            aDict.Add("AreaCodeB", ddl_AreaCodeB.SelectedValue);
            aDict.Add("OrganAddr", txt_Addr.Text);
            aDict.Add("OrganTel", txt_Tel.Text);
            aDict.Add("CreateUserID", userInfo.PersonSNO);

            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Insert Into Organ(OrganCode,OrganName,AreaCodeA,AreaCodeB,OrganAddr,OrganTel,CreateUserID) Values(@OrganCode,@OrganName,@AreaCodeA,@AreaCodeB,@OrganAddr,@OrganTel,@CreateUserID)", aDict);
            Response.Write("<script>alert('新增成功!');document.location.href='./Organ.aspx'; </script>");
        }
        else
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("id", txt_ID.Value);
            //aDict.Add("OrganLevel", ddl_Level.SelectedValue);
            aDict.Add("OrganCode", txt_Code.Text);
            aDict.Add("OrganName", txt_Name.Text);
            aDict.Add("AreaCodeA", ddl_AreaCodeA.SelectedValue);
            aDict.Add("AreaCodeB", ddl_AreaCodeB.SelectedValue);
            aDict.Add("OrganAddr", txt_Addr.Text);
            aDict.Add("OrganTel", txt_Tel.Text);
            aDict.Add("ModifyDT", Convert.ToDateTime(DateTime.Now));
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery("Update Organ Set OrganCode=@OrganCode, OrganName=@OrganName, AreaCodeA=@AreaCodeA, AreaCodeB=@AreaCodeB, OrganAddr=@OrganAddr, OrganTel=@OrganTel,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID Where OrganSNO=@id", aDict);
            Response.Write("<script>alert('修改成功!');document.location.href='./Organ.aspx'; </script>");
        }
    }

    protected void newData()
    {
        Work.Value = "NEW";
        //Utility.setAreaCodeA(ddl_AreaCodeA, "請選擇");

        //Button1.Text = "新增";
    }

    protected void getData()
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("sno", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("select * from Organ Where OrganSNO=@sno", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_ID.Value = Convert.ToString(objDT.Rows[0]["OrganSNO"]);
            txt_Code.Text = Convert.ToString(objDT.Rows[0]["OrganCode"]);
            txt_Name.Text = Convert.ToString(objDT.Rows[0]["OrganName"]);
            //Utility.setAreaCodeA(ddl_AreaCodeA, "請選擇");
            ddl_AreaCodeA.SelectedValue = Convert.ToString(objDT.Rows[0]["AreaCodeA"]);
            Utility.setAreaCodeB(ddl_AreaCodeB, Convert.ToString(objDT.Rows[0]["AreaCodeA"]), "請選擇");
            ddl_AreaCodeB.SelectedValue = Convert.ToString(objDT.Rows[0]["AreaCodeB"]);
            txt_Addr.Text = Convert.ToString(objDT.Rows[0]["OrganAddr"]);
            txt_Tel.Text = Convert.ToString(objDT.Rows[0]["OrganTel"]);
        }
    }

    protected void txt_Code_TextChanged(object sender, EventArgs e)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();

        string sql = "Select * from Organ where OrganCode=@OrganCode";
        aDict.Add("OrganCode", txt_Code.Text);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            hf_Code.Value = "True";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", "alert('已有相同代號!')", true);
        }
        else
        {
            hf_Code.Value = "False";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", "alert('可使用!')", true);
        }
    }


}