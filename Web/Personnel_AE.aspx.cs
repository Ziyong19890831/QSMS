using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security.AntiXss;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Personnel_AE : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    public string RoleName;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
        //暫時關閉
        if (userInfo == null) Response.Redirect("../Default.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            #region 初始化設定

            //初始化:行政區
            Utility.setAreaCodeA_Access(ddl_AreaCodeA, userInfo.AreaCodeA, userInfo.AreaCodeB, userInfo.RoleOrganType, "請選擇");
            Utility.setTsTypeAccount(ddl_TsType, "請選擇");
            ddl_TsType.Enabled = false;
            txt_TSNote.Enabled = false;
            txt_TSNote.Visible = false;
            if (userInfo.RoleSNO == "10")
            {
                ddl_TsType.Visible = true;
            }
            else
            {
                ddl_TsType.Visible = false;
            }
            #endregion

            String work = "";
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
            if (work.Equals("N"))
            {
                Utility.setAreaCodeA(ddl_AddressC, "請選擇");
                newData();


            }
            else
            {

                Utility.setAreaCodeA(ddl_AddressC, "請選擇");

                //div_MP.Visible = true;
                getData();
                Utility.setAreaCodeB(ddl_AddressD, ddl_AddressC.SelectedValue, "請選擇");
                if (ddl_AddressD.SelectedItem.Text == "請選擇") ddl_AddressD.Enabled = false;

                if (Request.QueryString["Error"] == "1")
                {
                    Response.Write("<script>alert('標記紅色星號的必填欄位內容有缺漏，請確實補填後按下修改按鈕送出。')</script>");
                }

            }
        }

    }

    protected void newData()
    {
        Work.Value = "NEW";
    }

    protected void getData()
    {
        if (userInfo.RoleSNO == "10") ddl_TsType.Enabled = true;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        aDict.Add("PersonSNO", userInfo.PersonSNO);
        DataTable objDT = objDH.queryData(@"
            Select 
		        P.PersonSNO , R.RoleName , p.PAccount, p.MStatusSNO , p.PName , RE.REName , P.Note
                , p.PersonID , p.IsEnable, P.CreateDT, P.ModifyDT
		        , p.PTel_O , p.PFax_O , p.PTel , p.PFax , p.PPhone  , p.PMail
		        , p.PZCode , p.PAddr ,  p.JMajor , p.JLicID  , P.country
                , p.Degree , p.QSExp, P.TJobType, P.TSType
		        , p.JobTitle , p.SchoolName , p.Major , p.Degree , p.QSExp , p.City , p.Area ,O.OrganAddr
                , CONVERT(char(10), p.PBirthDate,111) PBirthDate
		        , case when p.PSex = 1 then '男' ELSE '女' END Sex 
		        , case when p.JLicStatus = 0 then '終止' ELSE '正常' END LicStatus
                , O.OrganSNO, O.OrganCode, O.OrganName, O.AreaCodeA, O.AreaCodeB, OC.ClassName
                , case when O.AbortDate is null then'營業'  ELSE '歇業' END AbortDate  
                , MP.OrganCode MPOrganCode, MPO.OrganName MPOrganName,MPO.OrganAddr MPOOrganAddr, MPO.OrganTel MPOrganTel
                , MP.LStatus ,MP.LValid ,MP.VEDate, MP.LCN, MP.LRType, MP.LStype, MP.LSCN ,MP.Note PersonMPNote
                ,CD1.AREA_CODE CityCode,CD2.AREA_CODE AreaCode,P.TSSNO,P.TSNote,CD2.ZipCode
            FROM Person P 
                LEFT JOIN Role r ON r.RoleSNO = p.RoleSNO						
                LEFT JOIN RoleException RE On RE.RESNO =P.RoleException
                LEFT JOIN QS_MemberStatus m ON m.MStatusSNO = p.MStatusSNO
                LEFT JOIN Organ O ON O.OrganSNO=P.OrganSNO  
				LEFT Join OrganClass OC on OC.ClassSNO=O.OrganClass
				LEFT JOIN PersonMP MP on MP.PersonID=P.PersonID
                LEFT JOIN Organ MPO ON MPO.OrganCode=MP.OrganCode  
                Left Join CD_AREA CD1 On CD1.AREA_NAME=P.City
				Left Join CD_AREA CD2 On CD2.AREA_NAME=P.Area And CD1.AREA_CODE=Left(CD2.AREA_CODE,2)
            Where P.PersonSno=@PersonSNO
        ", aDict);


        if (objDT.Rows.Count > 0)
        {
            txt_PID.Value = Convert.ToString(objDT.Rows[0]["PersonSNO"]);
            lb_OrganCodeName.Text = objDT.Rows[0]["OrganCode"].ToString() + "-" + objDT.Rows[0]["OrganName"].ToString();
            lb_Role.Text = Convert.ToString(objDT.Rows[0]["RoleName"]);
            lb_Role2.Text = Convert.ToString(objDT.Rows[0]["REName"]);
            lb_Account.Text = Convert.ToString(objDT.Rows[0]["PAccount"]);
            txt_Name.Text = Convert.ToString(objDT.Rows[0]["PName"]);
            txt_Personid.Text = Convert.ToString(objDT.Rows[0]["PersonID"]);
            txt_Birthday.Value = Convert.ToString(objDT.Rows[0]["PBirthDate"]);
            txt_Phone.Value = Convert.ToString(objDT.Rows[0]["PPhone"]);
            txt_Mail.Value = Convert.ToString(objDT.Rows[0]["PMail"]);
            txt_degree.Value = Convert.ToString(objDT.Rows[0]["degree"]);
            txt_Tel.Value = Convert.ToString(objDT.Rows[0]["PTel"]);
            txt_country.Value = Convert.ToString(objDT.Rows[0]["country"]);
            txt_contact_ZipCode.Value = Convert.ToString(objDT.Rows[0]["ZipCode"]);
            txt_contact_address.Value = HttpUtility.HtmlEncode(Convert.ToString(objDT.Rows[0]["PAddr"]));
            HF_OrganSNO.Value = Convert.ToString(objDT.Rows[0]["OrganSNO"]);
            ddl_TsType.SelectedValue = Convert.ToString(objDT.Rows[0]["TSSNO"]);
            txt_TSNote.Text = Convert.ToString(objDT.Rows[0]["TsNote"]);
            if (objDT.Rows[0]["CityCode"].ToString() == "" || objDT.Rows[0]["AreaCode"].ToString() == "")
            {
                ddl_AddressC.SelectedItem.Text = "請選擇";
            }
            else
            {
                ddl_AddressC.SelectedValue = Convert.ToString(objDT.Rows[0]["CityCode"]);
                ddl_AddressD.SelectedValue = Convert.ToString(objDT.Rows[0]["AreaCode"]);
            }
            if (ddl_TsType.SelectedItem.Text == "其他")
            {
                txt_TSNote.Visible = true;
                txt_TSNote.Enabled = true;
            }
            //衛教師
            txt_TJobType.Text = Convert.ToString(objDT.Rows[0]["TJobType"]);
            txt_TSType.Text = Convert.ToString(objDT.Rows[0]["TSType"]);


            //來自醫事機構表PersonMP 
            //lb_LCN.Text = Convert.ToString(objDT.Rows[0]["LCN"]);
            //lb_VEDate.Text = Convert.ToString(objDT.Rows[0]["VEDate"]);
            //lb_LValid.Text = Convert.ToString(objDT.Rows[0]["LValid"]);
            //lb_LStatus.Text = Convert.ToString(objDT.Rows[0]["LStatus"]);
            //lb_AbortDate.Text = Convert.ToString(objDT.Rows[0]["AbortDate"]);
            //lb_organClassName.Text = Convert.ToString(objDT.Rows[0]["ClassName"]);
            //lb_OrganName.Text = Convert.ToString(objDT.Rows[0]["MPOrganName"]);
            //lb_OrganAddr.Text = Convert.ToString(objDT.Rows[0]["MPOOrganAddr"]);
            //lb_OrganCode.Text = Convert.ToString(objDT.Rows[0]["MPOrganCode"]);
            //lb_OrganTel.Text = Convert.ToString(objDT.Rows[0]["MPOrganTel"]);
            //lb_Note.Text = Convert.ToString(objDT.Rows[0]["PersonMPNote"]);
            //lb_LSType.Text = Convert.ToString(objDT.Rows[0]["LSType"]);
            //lb_LRtype.Text = Convert.ToString(objDT.Rows[0]["LRType"]);
            //lb_LSCN.Text = Convert.ToString(objDT.Rows[0]["LSCN"]);

        }

    }

    protected void btn_submit_Click(object sender, EventArgs e)
    {

        string errorMessage = "";

        //if (string.IsNullOrEmpty(txt_Account.Value)) errorMessage += "請輸入使用者帳號！\\n";
        if (string.IsNullOrEmpty(txt_Name.Text)) errorMessage += "請輸入使用者姓名！\\n";
        if (string.IsNullOrEmpty(txt_Mail.Value)) errorMessage += "請輸入E-Mail信箱！\\n";
        if (string.IsNullOrEmpty(txt_Birthday.Value)) errorMessage += "請輸入E-Mail信箱！\\n";
        //if (txt_Account.Value.Length > 50) errorMessage += "帳號字數過多！\\n";
        if (txt_Name.Text.Length > 50) errorMessage += "姓名字數過多！\\n";
        if (txt_Birthday.Value.Length > 10) errorMessage += "生日字數過多！\\n";
        if (txt_Tel.Value.Length > 50) errorMessage += "電話字數過多！\\n";
        if (txt_Phone.Value.Length > 50) errorMessage += "手機字數過多！\\n";
        if (txt_Mail.Value.Length > 100) errorMessage += "E-Mail信箱字數過多！\\n";
        if (txt_contact_ZipCode.Value.Length > 5) errorMessage += "通訊地-區號字數過多！\\n";
        if (txt_contact_address.Value.Length > 50) errorMessage += "通訊地-地址字數過多！\\n";
        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }
        if (txt_Birthday.Value == "1900/01/01")
        {
            Response.Write("<script>alert('生日不得為1900/01/01!'); </script>");
            return;
        }
        if (ddl_TsType.SelectedValue == "" && txt_TSNote.Text == "" && userInfo.RoleSNO == "10")
        {
            Response.Write("<script>alert('請選填服務科別或服務科別備註'); </script>");
            return;
        }

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonSNO", userInfo.PersonSNO);
        aDict.Add("Tel", txt_Tel.Value);
        aDict.Add("Phone", txt_Phone.Value);
        aDict.Add("Mail", txt_Mail.Value);
        aDict.Add("Birthday", txt_Birthday.Value);
        aDict.Add("PZcode", txt_contact_ZipCode.Value);
        aDict.Add("PAddr", txt_contact_address.Value);
        aDict.Add("OrganSNO", HF_OrganSNO.Value);
        aDict.Add("City", ddl_AddressC.SelectedItem.Text);
        aDict.Add("Area", ddl_AddressD.SelectedItem.Text);
        aDict.Add("ModifyDT", DateTime.Now.ToShortDateString());
        aDict.Add("ModifyUserID", userInfo.PersonSNO);
        aDict.Add("TSSNO", ddl_TsType.SelectedValue);
        if (hf_OrganCode.Value != "000")
        {
            aDict.Add("PersonExp", "");
        }
        //else
        //{
        //    aDict.Add("PersonExp", txt_Experience.Text);
        //}
        if (ddl_TsType.SelectedItem.Text != "其他")
        {
            aDict.Add("TsNote", "");
        }
        else
        {
            aDict.Add("TsNote", txt_TSNote.Text);
        }
        PairData(userInfo.PersonSNO, HF_OrganSNO.Value, txt_Mail.Value, ddl_AddressC.SelectedItem.Text, ddl_AddressD.SelectedItem.Text, txt_contact_address.Value, txt_Birthday.Value, txt_Phone.Value);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery(@"
            Update Person Set   
                PTel=@Tel, PPhone=@Phone, PMail=@Mail, PBirthDate=@Birthday,TSSNO=@TSSNO,TSNote=@TsNote,
                PZcode=@PZcode, PAddr=@PAddr , City=@City , Area=@Area ,OrganSNO=@OrganSNO , ModifyDT=@ModifyDT , ModifyUserID=@ModifyUserID
            Where PersonSNO=@PersonSNO 
        ", aDict);
        Response.Write("<script>alert('修改成功!');</script>");
        Session.Abandon();
        Session.Clear();
        Response.Write("<script>alert('已登出!煩請重新登入!!'); location.href='./Notice.aspx';</script>");

    }
    protected void ddl_AddressB_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sql = "Select *  FROM [CD_AREA] where AREA_CODE=@AREA_CODE";
        DataHelper odt = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("AREA_CODE", ddl_AddressD.SelectedValue);
        DataTable ObjDT = odt.queryData(sql, aDict);
        txt_contact_ZipCode.Value = ObjDT.Rows[0]["ZipCode"].ToString();
    }
    protected void ddl_AddressC_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_AddressD.Items.Clear();

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String AreaCodeA = ddl_AddressC.SelectedValue;
        if (!String.IsNullOrEmpty(AreaCodeA))
        {
            Utility.setAreaCodeB(ddl_AddressD, AreaCodeA, "請選擇");
            ddl_AddressD.Enabled = true;
        }
        else
        {
            ddl_AddressD.Items.Add(new ListItem("請選擇", ""));
            ddl_AddressD.Enabled = false;

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
        HF_OrganSNO.Value = ddl_OrganCode.SelectedValue;
        lb_OrganCodeName.Text = ddl_OrganCode.SelectedItem.Text;
        //Person_Experience.Visible = false;
    }

    public void PairData(string PersonSNO, string OrganSNO, string Email, string City, string Area, string PAddr, string PBirthday, string PPhone)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> Dict = new Dictionary<string, object>();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = @"Select * from Person where PersonSNO=@PersonSNO";
        Dict.Add("PersonSNO", PersonSNO);
        DataTable ObjDT = objDH.queryData(sql, Dict);
        string Pair_OrganSNO = ObjDT.Rows[0]["OrganSNO"].ToString();
        string Pair_Email = ObjDT.Rows[0]["PMail"].ToString();
        string Pair_City = ObjDT.Rows[0]["City"].ToString();
        string Pair_Area = ObjDT.Rows[0]["Area"].ToString();
        string Pair_PAddr = ObjDT.Rows[0]["PAddr"].ToString();
        string Pair_PBirthday = ObjDT.Rows[0]["PBirthDate"].ToString();
        string Pair_PPhone = ObjDT.Rows[0]["PPhone"].ToString();
        string InsertLog = "Insert Into PersonDataLog(PersonSNO,ColumnName,CreateDT,CreateUserID) Values (@PersonSNO,@ColumnName,getdate(),@PersonSNO)";
        if (Pair_OrganSNO != OrganSNO)
        {
            aDict.Add("ColumnName", "修改執業醫事機構");
            aDict.Add("PersonSNO", PersonSNO);
            objDH.executeNonQuery(InsertLog, aDict);
            aDict.Clear();
        }
        if (Pair_Email.Trim() != Email.Trim())
        {
            aDict.Add("ColumnName", "修改信箱");
            aDict.Add("PersonSNO", PersonSNO);
            objDH.executeNonQuery(InsertLog, aDict);
            aDict.Clear();
        }
        if (Pair_City != City)
        {
            aDict.Add("ColumnName", "修改城市");
            aDict.Add("PersonSNO", PersonSNO);
            objDH.executeNonQuery(InsertLog, aDict);
            aDict.Clear();
        }
        if (Pair_Area != Area)
        {
            aDict.Add("ColumnName", "修改地區");
            aDict.Add("PersonSNO", PersonSNO);
            objDH.executeNonQuery(InsertLog, aDict);
            aDict.Clear();
        }
        if (Pair_PAddr.Trim() != PAddr.Trim())
        {
            aDict.Add("ColumnName", "修改地址");
            aDict.Add("PersonSNO", PersonSNO);
            objDH.executeNonQuery(InsertLog, aDict);
            aDict.Clear();
        }
        if (Pair_PBirthday != "")
        {
            if (Convert.ToDateTime(Pair_PBirthday) != Convert.ToDateTime(PBirthday))
            {

                aDict.Add("ColumnName", "修改出生年月日	");
                aDict.Add("PersonSNO", PersonSNO);
                objDH.executeNonQuery(InsertLog, aDict);
                aDict.Clear();
            }
        }

        if (Pair_PPhone.Trim() != PPhone.Trim())
        {
            aDict.Add("ColumnName", "修改手機");
            aDict.Add("PersonSNO", PersonSNO);
            objDH.executeNonQuery(InsertLog, aDict);
            aDict.Clear();
        }
    }


    protected void ddl_TsType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_TsType.SelectedItem.Text == "其他")
        {
            txt_TSNote.Enabled = true;
            txt_TSNote.Visible = true;
        }
        else
        {
            txt_TSNote.Enabled = false;
            txt_TSNote.Visible = false;
        }

    }
}