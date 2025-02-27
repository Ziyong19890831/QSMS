using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Personnel_AE : System.Web.UI.Page
{
    UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            #region 初始化設定

            //string RfSQL = "1=1";
            //string OfSQL = "";
            //switch (userInfo.RoleOrganType)
            //{
            //    case "S":
            //        RfSQL = "IsAdmin=1";     //S可新增全部的管理角色別
            //        OfSQL = "";     //S皆可新增所有單位
            //        break;
            //    case "A":
            //        RfSQL = "IsAdmin=1";     //A可新增全部的管理角色別
            //        OfSQL = "And Substring(AREA_CODE,1,2)='" + userInfo.AreaCodeA + "'";     //A只能新增同區域單位
            //        break;
            //    case "B":
            //        RfSQL = "IsAdmin=1";     //B可新增全部的管理角色別
            //        OfSQL = "And AREA_CODE='" + userInfo.AreaCodeB + "'";     //B只能新增同區域單位
            //        break;
            //    case "U":   //協會單位
            //        RfSQL = "RoleOrganType='" + userInfo.RoleOrganType + "'";   //僅可新增同單位類別的管理角色
            //        OfSQL = "IsAdmin=1";     //協會皆可新增所有單位
            //        break;
            //    default:
            //        break;
            //}

            //初始化:角色別
            Utility.setRole_Access(ddl_Role, true, userInfo.RoleOrganType, userInfo.RoleLevel, userInfo.RoleGroup, "請選擇");

            //初始化:行政區
            //Utility.setAreaCodeA(ddl_AreaCodeA, OfSQL, "請選擇");
            Utility.setAreaCodeA_Access(ddl_AreaCodeA, userInfo.AreaCodeA, userInfo.AreaCodeB, userInfo.RoleOrganType, "請選擇");
            //if (userInfo.RoleOrganType == "U")
            //    Panel_Organ.Visible = false;
            //else 
            //    Utility.setAreaCodeA_Access(ddl_AreaCodeA, userInfo.AreaCodeA, userInfo.AreaCodeB, userInfo.RoleOrganType, "請選擇");

            #endregion


            String work = "";
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
            if (work.Equals("N"))
            {
                newData();
                PasswordPanel.Visible = false;
            }

            else
            {
                getData();
                txt_PWD.Style.Add("display", "none");
                PasswordPanel.Visible = true;
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
        }
        else
        {
            ddl_AreaCodeB.Items.Add(new ListItem("請選擇", ""));
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
        }
        else
        {
            ddl_OrganCode.Items.Add(new ListItem("請選擇", ""));
        }
    }

    protected void ddl_OrganCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        HF_OrganSNO.Value = ddl_OrganCode.SelectedValue; ;
        lb_OrganCodeName.Text = ddl_OrganCode.SelectedItem.Text;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();

        string errorMessage = "";
        if (string.IsNullOrEmpty(ddl_Role.SelectedValue)) errorMessage += "請選擇角色別！\\n";
        if (string.IsNullOrEmpty(HF_OrganSNO.Value)) errorMessage += "請選擇現職單位！\\n";
        if (string.IsNullOrEmpty(ddl_Role.SelectedValue)) errorMessage += "請選擇角色別！\\n";
        if (string.IsNullOrEmpty(txt_Account.Value)) errorMessage += "請輸入使用者帳號！\\n";
        if (string.IsNullOrEmpty(txt_Name.Value)) errorMessage += "請輸入使用者姓名！\\n";
        if (string.IsNullOrEmpty(txt_Mail.Value)) errorMessage += "請輸入E-Mail信箱！\\n";
        if (string.IsNullOrEmpty(txt_PWD.Text) && Work.Value.Equals("NEW")) errorMessage += "請輸入密碼！\\n";

        if (txt_Account.Value.Length > 50) errorMessage += "帳號字數過多！\\n";
        if (txt_Name.Value.Length > 50) errorMessage += "姓名字數過多！\\n";
        if (txt_Tel.Value.Length > 50) errorMessage += "電話字數過多！\\n";
        if (txt_Phone.Value.Length > 50) errorMessage += "手機字數過多！\\n";
        if (txt_Mail.Value.Length > 100) errorMessage += "E-Mail信箱字數過多！\\n";
        if (txt_PWD.Text.Length > 50) errorMessage += "密碼字數過多！\\n";


        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("RoleSNO", ddl_Role.SelectedValue);
        aDict.Add("OrganSNO", HF_OrganSNO.Value);
        aDict.Add("PAccount", txt_Account.Value);
        aDict.Add("PName", txt_Name.Value);   
        aDict.Add("PersonID", txt_Personid.Value);    
        aDict.Add("PTel", txt_Tel.Value);   
        aDict.Add("PMail", txt_Mail.Value);
        aDict.Add("PPhone", txt_Phone.Value);
        aDict.Add("IsEnable", ddl_IsEnable.SelectedValue);

        if (Work.Value.Equals("NEW"))
        {
            aDict.Add("PPWD", txt_PWD.Text);
            aDict.Add("CreateUserID", userInfo.PersonSNO);

            //判斷帳號唯一性
            DataTable objDT = objDH.queryData("Select PAccount From Person Where PAccount=@PAccount", aDict);
            if (objDT.Rows.Count > 0)
            {
                Utility.showMessage(Page, "ErrorMessage", String.Format("[{0}]帳號已存在！\\n", txt_Account.Value));
                return;
            }

            //判斷身分證唯一性
            if (txt_Personid.Value != "")
            {
                objDT = objDH.queryData("Select PersonID From Person Where PersonID=@PersonID", aDict);
                if (objDT.Rows.Count > 0)
                {
                    Utility.showMessage(Page, "ErrorMessage", String.Format("[{0}]身分證已存在！\\n", txt_Personid.Value));
                    return;
                }
            }

            objDH.executeNonQuery(@"
                    Insert Into Person(RoleSNO,OrganSNO,PAccount,PName,PersonID,PPWD,PTel,PMail,PPhone,CreateUserID) 
                                Values(@RoleSNO,@OrganSNO,@PAccount,@PName,@PersonID,@PPWD,@PTel,@PMail,@PPhone,@CreateUserID)
                ", aDict);
            Response.Write("<script>alert('新增成功!');document.location.href='./PersonnelAdmin.aspx'; </script>");
        }
        else
        {     
            string updatePSW = "";
            aDict.Add("PersonSNO", txt_PID.Value);           
            aDict.Add("ModifyDT", Convert.ToDateTime(DateTime.Now));
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            if (txt_PWD.Text.Length > 0)
            {
                aDict.Add("PPWD", txt_PWD.Text);
                updatePSW = ",PPWD=@PPWD";
            }
            //判斷帳號唯一性
            DataTable objDT = objDH.queryData(@"
                    Select PAccount From Person Where PersonSNO<>@PersonSNO AND PAccount=@PAccount
                ", aDict);
            if (objDT.Rows.Count > 0)
            {
                Utility.showMessage(Page, "ErrorMessage", String.Format("[{0}]帳號已存在！\\n", txt_Account.Value));
                return;
            }


                objDH.executeNonQuery(@"Update Person Set 
                                    RoleSNO=@RoleSNO
                                    ,OrganSNO=@OrganSNO
                                    ,PAccount=@PAccount
                                    ,PName=@PName
                                    ,PersonID=@PersonID
                                    ,PTel=@PTel  
                                    ,PMail=@PMail
                                    ,PPhone=@PPhone
                                    ,IsEnable=@IsEnable
                                    ,ModifyDT=@ModifyDT
                                    ,ModifyUserID=@ModifyUserID "
                + updatePSW + " Where PersonSNO = @PersonSNO", aDict);
            Response.Write("<script>alert('修改成功!');document.location.href='./PersonnelAdmin.aspx'; </script>");
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
        DataTable objDT = objDH.queryData(@"
            Select 
		            P.PersonSNO , r.RoleName , R.RoleSNO , p.PAccount
                    , p.PName , p.PersonID , p.IsEnable
		            , p.PTel , p.PFax , p.PPhone  , p.PMail, P.CreateDT, P.ModifyDT
                    , O.OrganSNO, O.OrganCode, O.OrganName, O.AreaCodeA, O.AreaCodeB
            FROM Person P 
                LEFT JOIN Role r ON r.RoleSNO = p.RoleSNO     
                LEFT JOIN Organ O ON O.OrganSNO=P.OrganSNO  
            Where P.PersonSno=@sno
        ", aDict);

        if (objDT.Rows.Count > 0)
        {
            txt_PID.Value = Convert.ToString(objDT.Rows[0]["PersonSNO"]);


            lb_DataInfo.Text = "建立日期：" + Convert.ToString(objDT.Rows[0]["CreateDT"]) + "　　上一次修改日期：" + Convert.ToString(objDT.Rows[0]["ModifyDT"]);
            div_DataInfo.Visible = true;


            HF_OrganSNO.Value = objDT.Rows[0]["OrganSNO"].ToString();
            lb_OrganCodeName.Text = objDT.Rows[0]["OrganCode"].ToString() + "-" + objDT.Rows[0]["OrganName"].ToString();


            ddl_Role.SelectedValue = Convert.ToString(objDT.Rows[0]["RoleSNO"]);
            ddl_IsEnable.SelectedValue = Convert.ToString(objDT.Rows[0]["IsEnable"]);
            txt_Account.Value = Convert.ToString(objDT.Rows[0]["PAccount"]);
            txt_Name.Value = Convert.ToString(objDT.Rows[0]["PName"]);
            txt_Personid.Value = Convert.ToString(objDT.Rows[0]["PersonID"]);
            txt_Tel.Value = Convert.ToString(objDT.Rows[0]["PTel"]);
            txt_Phone.Value = Convert.ToString(objDT.Rows[0]["PPhone"]);
            txt_Mail.Value = Convert.ToString(objDT.Rows[0]["PMail"]);

        }

    }

}