using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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

            //初始化:角色別
            Utility.setRole_Access(ddl_Role, false, userInfo.RoleOrganType, userInfo.RoleLevel, userInfo.RoleGroup, "請選擇");
            Utility.setTsTypeAccount(ddl_TSType, "請選擇");
            ddl_TSType.Enabled = false;
            txt_TSNote.Enabled = false;
            txt_TSNote.Visible = false;
            //初始化:行政區
            Utility.setAreaCodeA_Access(ddl_AreaCodeA, userInfo.AreaCodeA, userInfo.AreaCodeB, userInfo.RoleOrganType, "請選擇");

            #endregion



            String work = "";
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
            if (work.Equals("N"))
            {
                Utility.setAreaCodeA(ddl_AreaCodeA, "請選擇");
                Utility.setAreaCodeA(ddl_AddressA, "請選擇");
                Utility.setMStatus(ddl_Status, "請選擇");
                newData();
                PasswordPanel.Visible = false;
                Psex.Visible = false;
                btn_Prview.Visible = false;
            }

            else
            {
                Utility.setAreaCodeA(ddl_AddressA, "請選擇");
                Utility.setMStatus(ddl_Status, "請選擇");
                div_MP.Visible = true;
                txt_PWD.Style.Add("display", "none");
                PasswordPanel.Visible = true;
                getData();
                Utility.setAreaCodeB(ddl_AddressB, ddl_AddressA.SelectedValue, "請選擇");
                if (ddl_AddressB.SelectedItem.Text == "請選擇") ddl_AddressB.Enabled = false;
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
        if (txt_Account.Value.Length < 5) errorMessage += "帳號字數過少(少於5字)！\\n";
        if (txt_Name.Value.Length > 50) errorMessage += "姓名字數過多！\\n";
        if (txt_Personid.Value.Length > 10) errorMessage += "身分證字數過多！\\n";
        if (txt_PWD.Text.Length > 50) errorMessage += "密碼字數過多！\\n";
        if (txt_Birthday.Value.Length > 10) errorMessage += "生日字數過多！\\n";
        if (txt_Tel.Value.Length > 50) errorMessage += "電話字數過多！\\n";
        if (txt_Phone.Value.Length > 50) errorMessage += "手機字數過多！\\n";
        if (txt_Mail.Value.Length > 100) errorMessage += "E-Mail信箱字數過多！\\n";
        if (txt_ZipCode.Value.Length > 5) errorMessage += "通訊地-區號字數過多！\\n";
        if (txt_Addr.Value.Length > 50) errorMessage += "通訊地-地址字數過多！\\n";

        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        DropDownList ddl_Country = (DropDownList)WUC_Country.FindControl("ddl_Country");
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("RoleSNO", ddl_Role.SelectedValue);
        aDict.Add("MStatusSNO", ddl_Status.SelectedValue);
        aDict.Add("OrganSNO", HF_OrganSNO.Value);
        aDict.Add("PAccount", txt_Account.Value);
        aDict.Add("PName", txt_Name.Value);
        aDict.Add("PBirthDate", txt_Birthday.Value);
        aDict.Add("PTel", txt_Tel.Value);
        aDict.Add("PMail", txt_Mail.Value);
        aDict.Add("PPhone", txt_Phone.Value);
        aDict.Add("PZCode", txt_ZipCode.Value);
        aDict.Add("PAddr", txt_Addr.Value);
        aDict.Add("country", ddl_Country.SelectedValue);
        aDict.Add("Degree", txt_degree.Value);
        aDict.Add("PersonId", txt_Personid.Value);
        aDict.Add("PSex", getSex());
        aDict.Add("IsEnable", ddl_IsEnable.SelectedValue);
        aDict.Add("TJobType", txt_TJobType.Value);
        aDict.Add("TSType", txt_TSType.Value);
        aDict.Add("Note", txt_Note.Value);
        aDict.Add("City", ddl_AddressA.SelectedItem.Text);
        aDict.Add("Area", ddl_AddressB.SelectedItem.Text);
        aDict.Add("TSSNO", ddl_TSType.SelectedValue);
        aDict.Add("TSNote", txt_TSNote.Text);

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

            objDH.executeNonQuery(@"
                    Insert Into Person(RoleSNO,OrganSNO,MStatusSNO,PAccount,PName,PersonId,PSex
                                        ,PPWD,PBirthDate,PTel,PMail,PPhone,country,TJobType,TSType,Degree
                                        ,PZCode,PAddr,IsEnable,Note,CreateUserID,Area,City,TSSNO ,TSNote) 
                                Values(@RoleSNO,@OrganSNO,@MStatusSNO,@PAccount,@PName,@PersonId,@PSex
                                        ,@PPWD,@PBirthDate,@PTel,@PMail,@PPhone,@country,@TJobType,@TSType,@Degree
                                        ,@PZCode,@PAddr,@IsEnable,@Note,@CreateUserID,@Area,@City,@TSSNO,@TSNote)
                ", aDict);
            Response.Write("<script>alert('新增成功!');document.location.href='./Personnel.aspx'; </script>");
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
            DataTable objDT = objDH.queryData("Select PAccount From Person Where PersonSNO<>@PersonSNO AND PAccount=@PAccount", aDict);
            if (objDT.Rows.Count > 0)
            {
                Utility.showMessage(Page, "ErrorMessage", String.Format("[{0}]帳號已存在！\\n", txt_Account.Value));
                return;
            }

            if (txt_Birthday.Value == "1900/01/01")
            {
                Response.Write("<script>alert('生日不得為1900/01/01!'); </script>");
                return;
            }
            //修改歷程
            PairData(txt_PID.Value, HF_OrganSNO.Value, txt_Mail.Value, ddl_AddressA.SelectedItem.Text, ddl_AddressB.SelectedItem.Text, txt_Addr.Value, txt_Birthday.Value, txt_Phone.Value);

            objDH.executeNonQuery(@"Update Person Set 
                                    RoleSNO=@RoleSNO
                                    ,OrganSNO=@OrganSNO
                                    ,PAccount=@PAccount
                                    ,PName=@PName
                                    ,PersonId=@PersonId
                                    ,PBirthDate=@PBirthDate
                                    ,PTel=@PTel
                                    ,Degree=@Degree                                   
                                    ,PMail=@PMail
                                    ,PPhone=@PPhone
                                    ,PZCode=@PZCode
                                    ,PAddr=@PAddr
                                    ,country=@country
                                    ,Note=@Note
                                    ,MStatusSNO=@MStatusSNO
                                    ,IsEnable=@IsEnable
                                    ,ModifyDT=@ModifyDT                                  
                                    ,ModifyUserID=@ModifyUserID
                                    ,Area=@Area
                                    ,City=@City
                                    ,TSSNO=@TSSNO
                                    ,TSNote=@TsNote
                                    "
                + updatePSW + " ,PSex=" + rbl_Sex.SelectedValue + " Where PersonSNO = @PersonSNO", aDict);

            string Link = Request.QueryString["Link"] != null ? Request.QueryString["Link"].ToString() : "";
            string Ridrect = Request.QueryString["redirect"] != null ? Request.QueryString["redirect"].ToString() : "";
            if (Ridrect == "1")
            {
                Response.Write("<script>alert('修改成功!');document.location.href='./ReportMemberDetail.aspx?sno=" + txt_PID.Value + "'; </script>");
                return;
            }
            if (Link == "")
            {
                Response.Write("<script>alert('修改成功!');document.location.href='./Personnel.aspx'; </script>");

            }
            else
            {
                Response.Write("<script>alert('修改成功!');document.location.href='./Personnel.aspx';window.close(); </script>");
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
        //修改時不得對身分證欄位動手腳
        txt_Personid.Disabled = true;
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("sno", id);
        DropDownList ddl_Country = (DropDownList)WUC_Country.FindControl("ddl_Country");
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
            Select 
		        P.PersonSNO , r.RoleName , R.RoleSNO , p.PAccount, p.MStatusSNO , p.PName 
                , p.PersonID , p.IsEnable, P.CreateDT, P.ModifyDT
		        , p.PTel_O , p.PFax_O , p.PTel , p.PFax , p.PPhone  , p.PMail
		        , p.PZCode , p.PAddr ,  p.JMajor , p.JLicID  , P.country
                , p.Degree , p.QSExp, P.TJobType, P.TSType,P.Psex
		        , p.JobTitle , p.SchoolName , p.Major  , p.QSExp  ,O.[OrganAddr]
                , CONVERT(char(10), p.PBirthDate,111) PBirthDate
		        , case when p.PSex = 1 then '男' ELSE '女' END Sex 
		        , case when p.JLicStatus = 0 then '終止' ELSE '正常' END LicStatus
                , O.OrganSNO, O.OrganCode, O.OrganName, O.AreaCodeA, O.AreaCodeB, OC.ClassName
                , case when O.AbortDate is null then'營業'  ELSE '歇業' END AbortDate  
                , MP.OrganCode MPOrganCode, MPO.OrganName MPOrganName, MPO.OrganTel MPOrganTel,P.City,P.Area,MPO.OrganAddr MPOOrganAddr
                , MP.LStatus ,MP.LValid ,MP.VSDate, MP.LCN, MP.LStype, MP.LRtype, MP.LSCN  , P.Note , MP.Note PersonMPNote,MP.VEDate,MP.JCN
                ,CD1.AREA_CODE CityCode,CD2.AREA_CODE AreaCode,(Select PName from Person where P.ModifyUserID=Person.PersonSNO) ModifyUserName
                ,P.TsSNO,P.TsNote
            FROM Person P 
                LEFT JOIN Role r ON r.RoleSNO = p.RoleSNO										
                LEFT JOIN QS_MemberStatus m ON m.MStatusSNO = p.MStatusSNO
                LEFT JOIN Organ O ON O.OrganSNO=P.OrganSNO 			
				LEFT JOIN PersonMP MP on MP.PersonID=P.PersonID
                LEFT JOIN Organ MPO ON MPO.OrganCode=MP.OrganCode  
                LEFT Join OrganClass OC on OC.ClassSNO=MPO.OrganClass
                Left Join CD_AREA CD1 On CD1.AREA_NAME=P.City
				Left Join CD_AREA CD2 On CD2.AREA_NAME=P.Area And CD1.AREA_CODE=Left(CD2.AREA_CODE,2)
                Left Join TsTypeClass TC On TC.TSSNO=P.TSSNO
            Where P.PersonSno=@sno
        ", aDict);

        if (objDT.Rows.Count > 0)
        {
            txt_PID.Value = Convert.ToString(objDT.Rows[0]["PersonSNO"]);

            lb_DataInfo.Text = "建立日期：" + Convert.ToString(objDT.Rows[0]["CreateDT"]) + "　　上一次修改日期：" + Convert.ToString(objDT.Rows[0]["ModifyDT"]);
            lb_UserInfo.Text = "上一次修改人員：" + Convert.ToString(objDT.Rows[0]["ModifyUserName"]);
            div_DataInfo.Visible = true;
            HF_OrganSNO.Value = objDT.Rows[0]["OrganSNO"].ToString();
            lb_OrganCodeName.Text = objDT.Rows[0]["OrganCode"].ToString() + "-" + objDT.Rows[0]["OrganName"].ToString();
            ddl_Role.SelectedValue = Convert.ToString(objDT.Rows[0]["RoleSNO"]);
            ddl_IsEnable.SelectedValue = Convert.ToString(objDT.Rows[0]["IsEnable"]);
            txt_Account.Value = Convert.ToString(objDT.Rows[0]["PAccount"]);
            txt_Name.Value = Convert.ToString(objDT.Rows[0]["PName"]);
            txt_Personid.Value = Convert.ToString(objDT.Rows[0]["PersonID"]);
            txt_Birthday.Value = Convert.ToString(objDT.Rows[0]["PBirthDate"]);
            txt_Phone.Value = Convert.ToString(objDT.Rows[0]["PPhone"]);
            txt_Mail.Value = Convert.ToString(objDT.Rows[0]["PMail"]);
            txt_degree.Value = Convert.ToString(objDT.Rows[0]["degree"]);
            txt_Tel.Value = Convert.ToString(objDT.Rows[0]["PTel"]);
            rbl_Sex.SelectedValue = Convert.ToString(objDT.Rows[0]["PSex"]);
            ddl_Status.SelectedValue = Convert.ToString(objDT.Rows[0]["MStatusSNO"]);
            //txt_country.Value = Convert.ToString(objDT.Rows[0]["country"]);
            ddl_Country.SelectedValue = Convert.ToString(objDT.Rows[0]["country"]);
            txt_ZipCode.Value = Convert.ToString(objDT.Rows[0]["PZCode"]);
            txt_Addr.Value = Convert.ToString(objDT.Rows[0]["PAddr"]);
            txt_Note.Value = Convert.ToString(objDT.Rows[0]["Note"]);
            ddl_TSType.SelectedValue = Convert.ToString(objDT.Rows[0]["TSSNO"]);
            txt_TSNote.Text = Convert.ToString(objDT.Rows[0]["TSNote"]);
            if (rbl_Sex.SelectedValue == "")
            {
                rbl_Sex.SelectedValue = "1";
            }
            if (objDT.Rows[0]["CityCode"].ToString() == "" || objDT.Rows[0]["AreaCode"].ToString() == "")
            {
                ddl_AddressA.SelectedItem.Text = "請選擇";
            }
            else
            {
                ddl_AddressA.SelectedValue = Convert.ToString(objDT.Rows[0]["CityCode"]);
                ddl_AddressB.SelectedValue = Convert.ToString(objDT.Rows[0]["AreaCode"]);
            }
            if (ddl_TSType.SelectedItem.Text == "其他")
            {
                txt_TSNote.Visible = true;
                txt_TSNote.Enabled = true;
            }
            //衛教師
            txt_TJobType.Value = Convert.ToString(objDT.Rows[0]["TJobType"]);
            txt_TSType.Value = Convert.ToString(objDT.Rows[0]["TSType"]);
            //來自醫事機構表PersonMP 
            lb_LCN.Text = Convert.ToString(objDT.Rows[0]["LCN"]);
            if ((objDT.Rows[0]["VEDate"].ToString()) == "")
            {
                lb_VEDate.Text = "";
            }
            else
            {
                lb_VEDate.Text = Convert.ToDateTime(objDT.Rows[0]["VEDate"]).ToShortDateString();
            }
            lb_LValid.Text = Convert.ToString(objDT.Rows[0]["LValid"]);
            lb_LStatus.Text = Convert.ToString(objDT.Rows[0]["LStatus"]);
            lb_AbortDate.Text = Convert.ToString(objDT.Rows[0]["AbortDate"]);
            lb_organClassName.Text = Convert.ToString(objDT.Rows[0]["ClassName"]);
            lb_OrganName.Text = Convert.ToString(objDT.Rows[0]["MPOrganName"]);
            lb_OrganCode.Text = Convert.ToString(objDT.Rows[0]["MPOrganCode"]);
            lb_OrganTel.Text = Convert.ToString(objDT.Rows[0]["MPOrganTel"]);
            lb_OrganAddr.Text = Convert.ToString(objDT.Rows[0]["MPOOrganAddr"]);
            lb_LSType.Text = Convert.ToString(objDT.Rows[0]["LStype"]);
            lb_LRtype.Text = Convert.ToString(objDT.Rows[0]["LRtype"]);
            lb_LSCN.Text = Convert.ToString(objDT.Rows[0]["LSCN"]);
            lb_MPNote.Text = Convert.ToString(objDT.Rows[0]["PersonMPNote"]);
            lb_JCN.Text = Convert.ToString(objDT.Rows[0]["JCN"]);
            setRole_13_Pannel();
            if (ddl_Role.SelectedValue == "10")
            {
                Doctor1.Visible = true;
                Doctor2.Visible = true;
                Doctor3.Visible = true;
                ddl_TSType.Enabled = true;
                txt_TSNote.Enabled = true;
            }
            else
            {
                ddl_TSType.Enabled = false;
                txt_TSNote.Enabled = false;
            }

        }

    }

    protected int getSex()
    {
        if (txt_Personid.Value.Substring(1, 1) == "1") return 1;
        else if (txt_Personid.Value.Substring(1, 1) == "2") return 0;
        return 0;
    }

    protected void setRole_13_Pannel()
    {
        if (ddl_Role.SelectedValue == "13") Guardian.Visible = true;
        else Guardian.Visible = false;
    }

    protected void ddl_Role_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_Role.SelectedValue == "10")
        {
            ddl_TSType.Enabled = true;
            ddl_TSType.Visible = true;
            txt_TSNote.Enabled = true;

        }
        else
        {
            ddl_TSType.Visible = false;
            txt_TSNote.Visible = false;
        }
        setRole_13_Pannel();
    }
    protected void ddl_AddressA_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_AddressB.Items.Clear();

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String AreaCodeA = ddl_AddressA.SelectedValue;
        if (!String.IsNullOrEmpty(AreaCodeA))
        {
            Utility.setAreaCodeB(ddl_AddressB, AreaCodeA, "請選擇");
            ddl_AddressB.Enabled = true;
        }
        else
        {
            ddl_AddressB.Items.Add(new ListItem("請選擇", ""));
            ddl_AddressB.Enabled = false;

        }
    }

    protected void ddl_AddressB_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sql = "Select *  FROM [New_QSMS].[dbo].[CD_AREA] where AREA_CODE=@AREA_CODE";
        DataHelper odt = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("AREA_CODE", ddl_AddressB.SelectedValue);
        DataTable ObjDT = odt.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            txt_ZipCode.Value = ObjDT.Rows[0]["ZipCode"].ToString();
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string Link = Request.QueryString["Link"] != null ? Request.QueryString["Link"].ToString() : "";
        string Ridrect = Request.QueryString["redirect"] != null ? Request.QueryString["redirect"].ToString() : "";
        if (Ridrect == "1")
        {
            Response.Write("<script>document.location.href='./ReportMemberDetail.aspx?sno=" + txt_PID.Value + "'; </script>");
            return;
        }
        if (Link == "")
        {
            Response.Write("<script>document.location.href='./Personnel.aspx'; </script>");

        }
        else
        {
            Response.Write("<script>document.location.href='./Personnel.aspx';window.close(); </script>");
        }
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
        string InsertLog = "Insert Into PersonDataLog(PersonSNO,ColumnName,CreateDT,CreateUserID) Values (@PersonSNO,@ColumnName,getdate(),@CreateUserID)";
        if (Pair_OrganSNO != OrganSNO)
        {
            aDict.Add("ColumnName", "修改執業醫事機構");
            aDict.Add("PersonSNO", PersonSNO);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            objDH.executeNonQuery(InsertLog, aDict);
            aDict.Clear();
        }
        if (Pair_Email.Trim() != Email.Trim())
        {
            aDict.Add("ColumnName", "修改信箱");
            aDict.Add("PersonSNO", PersonSNO);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            objDH.executeNonQuery(InsertLog, aDict);
            aDict.Clear();
        }
        if (Pair_City != City)
        {
            aDict.Add("ColumnName", "修改城市");
            aDict.Add("PersonSNO", PersonSNO);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            objDH.executeNonQuery(InsertLog, aDict);
            aDict.Clear();
        }
        if (Pair_Area != Area)
        {
            aDict.Add("ColumnName", "修改地區");
            aDict.Add("PersonSNO", PersonSNO);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            objDH.executeNonQuery(InsertLog, aDict);
            aDict.Clear();
        }
        if (Pair_PAddr.Trim() != PAddr.Trim())
        {
            aDict.Add("ColumnName", "修改地址");
            aDict.Add("PersonSNO", PersonSNO);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            objDH.executeNonQuery(InsertLog, aDict);
            aDict.Clear();
        }
        if (Pair_PBirthday != "")
        {
            if (Convert.ToDateTime(Pair_PBirthday) != Convert.ToDateTime(PBirthday))
            {
                aDict.Add("ColumnName", "修改出生年月日	");
                aDict.Add("PersonSNO", PersonSNO);
                aDict.Add("CreateUserID", userInfo.PersonSNO);
                objDH.executeNonQuery(InsertLog, aDict);
                aDict.Clear();
            }
        }

        if (Pair_PPhone.Trim() != PPhone.Trim())
        {
            aDict.Add("ColumnName", "修改手機");
            aDict.Add("PersonSNO", PersonSNO);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            objDH.executeNonQuery(InsertLog, aDict);
            aDict.Clear();
        }
    }


    protected void ddl_TSType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_TSType.SelectedItem.Text == "其他")
        {
            txt_TSNote.Visible = true;
        }
        else
        {
            txt_TSNote.Visible = false;
        }
        if (ddl_Role.SelectedValue != "10") txt_TSNote.Visible = false;
    }

    protected void btn_Prview_Click(object sender, EventArgs e)
    {
        DropDownList ddl_Country = (DropDownList)WUC_Country.FindControl("ddl_Country");
        DataHelper objDH = new DataHelper();
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("sno", id);
        DataTable objDT = objDH.queryData(@"
            Select 
		        P.PersonSNO , r.RoleName , R.RoleSNO , p.PAccount, Case When p.MStatusSNO=0 Then  '正常' When p.MStatusSNO=1 Then '已退休' When p.MStatusSNO=2 Then '歿' Else '請選擇' End 'MStatusSNO', p.PName 
                , p.PersonID , Case When p.IsEnable =1 then'啟用'Else '停用' END 'IsEnable', P.CreateDT, P.ModifyDT
		        , p.PTel_O , p.PFax_O , p.PTel , p.PFax , p.PPhone  , p.PMail
		        , p.PZCode , p.PAddr ,  p.JMajor , p.JLicID  , P.country
                , p.Degree , p.QSExp, P.TJobType, P.TSType,P.Psex
		        , p.JobTitle , p.SchoolName , p.Major  , p.QSExp  ,O.[OrganAddr]
                , CONVERT(char(10), p.PBirthDate,111) PBirthDate
		        , case when p.PSex = 1 then '男'  when p.PSex = 0 then'女' else '' END Sex 
		        , case when p.JLicStatus = 0 then '終止' ELSE '正常' END LicStatus
                , O.OrganSNO, O.OrganCode, O.OrganName, O.AreaCodeA, O.AreaCodeB, OC.ClassName
                , case when O.AbortDate is null then'營業'  ELSE '歇業' END AbortDate  
                , MP.OrganCode MPOrganCode, MPO.OrganName MPOrganName, MPO.OrganTel MPOrganTel,P.City,P.Area,MPO.OrganAddr MPOOrganAddr
                , MP.LStatus ,MP.LValid ,MP.VSDate, MP.LCN, MP.LStype, MP.LRtype, MP.LSCN  , P.Note , MP.Note PersonMPNote,MP.VEDate,MP.JCN
                ,CD1.AREA_CODE CityCode,CD2.AREA_CODE AreaCode,(Select PName from Person where P.ModifyUserID=Person.PersonSNO) ModifyUserName
                ,P.TsSNO,P.TsNote
            FROM Person P 
                LEFT JOIN Role r ON r.RoleSNO = p.RoleSNO										
                LEFT JOIN QS_MemberStatus m ON m.MStatusSNO = p.MStatusSNO
                LEFT JOIN Organ O ON O.OrganSNO=P.OrganSNO 			
				LEFT JOIN PersonMP MP on MP.PersonID=P.PersonID
                LEFT JOIN Organ MPO ON MPO.OrganCode=MP.OrganCode  
                LEFT Join OrganClass OC on OC.ClassSNO=MPO.OrganClass
                Left Join CD_AREA CD1 On CD1.AREA_NAME=P.City
				Left Join CD_AREA CD2 On CD2.AREA_NAME=P.Area And CD1.AREA_CODE=Left(CD2.AREA_CODE,2)
                Left Join TsTypeClass TC On TC.TSSNO=P.TSSNO
            Where P.PersonSno=@sno
        ", aDict);

        string Change = " 已修改為：";
        if (objDT.Rows.Count > 0)
        {
            if (String.IsNullOrEmpty(txt_Organ.Value))
            {

                if (objDT.Rows[0]["OrganCode"].ToString() == ddl_OrganCode.SelectedItem.Text || ddl_OrganCode.SelectedItem.Text == "請選擇")
                {
                    lb_HiddenOrganName.Visible = false;
                }
                else
                {

                    lb_HiddenOrganName.Visible = true;
                    lb_HiddenOrganName.Text = objDT.Rows[0]["OrganCode"].ToString() == ddl_OrganCode.SelectedItem.Text ? "" : objDT.Rows[0]["OrganCode"].ToString() + "-" + objDT.Rows[0]["OrganName"].ToString() + Change + ddl_OrganCode.SelectedItem.Text;
                }

            }
            else
            {
                string NewOrgan = ReturnORganCodeAndName(txt_Organ.Value);
                if (objDT.Rows[0]["OrganCode"].ToString() == txt_Organ.Value)
                {
                    lb_HiddenOrganName.Visible = false;
                }
                else
                {
                    lb_HiddenOrganName.Visible = true;
                    lb_HiddenOrganName.Text = objDT.Rows[0]["OrganCode"].ToString() == txt_Organ.Value ? "" : objDT.Rows[0]["OrganCode"].ToString() + "-" + objDT.Rows[0]["OrganName"].ToString() + Change + NewOrgan.ToString();
                }

            }
            string OrginAddress = txt_ZipCode.Value + ddl_AddressA.SelectedItem.Text + ddl_AddressB.SelectedItem.Text + txt_Addr.Value;
            string Address = objDT.Rows[0]["PZCode"].ToString() + objDT.Rows[0]["City"].ToString() + objDT.Rows[0]["Area"].ToString() + objDT.Rows[0]["PAddr"].ToString();
            if (ddl_Role.SelectedItem.Text == "請選擇")
            {
                lb_HiddenRole.Text = "不得使用請選擇";
            }
            else
            {
                lb_HiddenRole.Text = objDT.Rows[0]["RoleName"].ToString() == ddl_Role.SelectedItem.Text ? "" : objDT.Rows[0]["RoleName"].ToString() + Change + ddl_Role.SelectedItem.Text;
            }
            if (ddl_Role.SelectedItem.Value == "10")
            {
                if (!CheckChange(ddl_TSType.SelectedItem.Value, objDT.Rows[0]["TsSNO"].ToString(), lb_HiddenTSType)) lb_HiddenTSType.Text = objDT.Rows[0]["TSType"].ToString() == ddl_TSType.SelectedItem.Text ? "" : objDT.Rows[0]["TSType"].ToString() + Change + ddl_TSType.SelectedItem.Text;
            }
            else
            {
                if (ddl_TSType.SelectedItem.Text == "請選擇")
                {
                    lb_HiddenTSType.Text = "不得使用請選擇";
                }
                lb_HiddenTSType.Visible = false;
            }

            if (ddl_Role.SelectedItem.Value == "13")
            {
                if (!CheckChange(txt_TJobType.Value, objDT.Rows[0]["TJobType"].ToString(), lb_HiddenTJobType)) lb_HiddenTJobType.Text = objDT.Rows[0]["TJobType"].ToString() == txt_TJobType.Value ? "" : objDT.Rows[0]["TJobType"].ToString() + Change + txt_TJobType.Value;
                if (!CheckChange(txt_TSType.Value, objDT.Rows[0]["TSType"].ToString(), lb_HiddenTSTypeRole13)) lb_HiddenTSTypeRole13.Text = objDT.Rows[0]["TSType"].ToString() == txt_TSType.Value ? "" : objDT.Rows[0]["TSType"].ToString() + Change + txt_TSType.Value;
            }
            else
            {
                lb_HiddenTSTypeRole13.Visible = false;
            }
            if (ddl_TSType.SelectedItem.Text == "請選擇")
            {
                lb_HiddenTSType.Text = "不得使用請選擇";
                lb_HiddenTSType.Visible = true;
            }
            if (!CheckChange(txt_Account.Value, objDT.Rows[0]["PAccount"].ToString(), lb_HiddenAccount)) lb_HiddenAccount.Text = objDT.Rows[0]["PAccount"].ToString() == txt_Account.Value ? "" : objDT.Rows[0]["PAccount"].ToString() + Change + txt_Account.Value;
            if (!CheckChange(txt_Name.Value, objDT.Rows[0]["PName"].ToString(), lb_HiddenName)) lb_HiddenName.Text = objDT.Rows[0]["PName"].ToString() == txt_Name.Value ? "" : objDT.Rows[0]["PName"].ToString() + Change + txt_Name.Value;
            if (!CheckChange(txt_Mail.Value, objDT.Rows[0]["PMail"].ToString(), lb_HiddenMail)) lb_HiddenMail.Text = objDT.Rows[0]["PMail"].ToString() == txt_Mail.Value ? "" : objDT.Rows[0]["PMail"].ToString() + Change + txt_Mail.Value;
            if (!CheckChange(txt_degree.Value, objDT.Rows[0]["Degree"].ToString(), lb_Hiddendegree)) lb_Hiddendegree.Text = objDT.Rows[0]["Degree"].ToString() == txt_degree.Value ? "" : objDT.Rows[0]["Degree"].ToString() + Change + txt_degree.Value;
            if (!CheckChange(ddl_Country.SelectedItem.Text, objDT.Rows[0]["country"].ToString(), lb_HiddenCountry)) lb_HiddenCountry.Text = objDT.Rows[0]["country"].ToString() == ddl_Country.SelectedItem.Text ? "" : objDT.Rows[0]["country"].ToString() + Change + ddl_Country.SelectedItem.Text;
            if (!CheckChange(txt_Birthday.Value, objDT.Rows[0]["PBirthDate"].ToString(), lb_HiddenBirthday)) lb_HiddenBirthday.Text = objDT.Rows[0]["PBirthDate"].ToString() == txt_Birthday.Value ? "" : objDT.Rows[0]["PBirthDate"].ToString() + Change + txt_Birthday.Value;
            if (!CheckChange(OrginAddress, Address)) lb_HiddenAddr.Text = Address == OrginAddress ? "" : Address + Change + OrginAddress;
            if (!CheckChange(rbl_Sex.SelectedItem.Text, objDT.Rows[0]["Sex"].ToString(), lb_Hiddensex)) lb_Hiddensex.Text = objDT.Rows[0]["Sex"].ToString() == rbl_Sex.SelectedItem.Text ? "" : objDT.Rows[0]["Sex"].ToString() + Change + rbl_Sex.SelectedItem.Text;
            if (!CheckChange(txt_Tel.Value, objDT.Rows[0]["PTel"].ToString(), lb_HiddenTel)) lb_HiddenTel.Text = objDT.Rows[0]["PTel"].ToString() == txt_Tel.Value ? "" : objDT.Rows[0]["PTel"].ToString() + Change + txt_Tel.Value;
            if (!CheckChange(txt_Phone.Value, objDT.Rows[0]["PPhone"].ToString(), lb_HiddenPhone)) lb_HiddenPhone.Text = objDT.Rows[0]["PPhone"].ToString() == txt_Phone.Value ? "" : objDT.Rows[0]["PPhone"].ToString() + Change + txt_Phone.Value;
            if (!CheckChange(txt_Note.Value, objDT.Rows[0]["Note"].ToString(), lb_HiddenNote)) lb_HiddenNote.Text = objDT.Rows[0]["Note"].ToString() == txt_Note.Value ? "" : objDT.Rows[0]["Note"].ToString() + Change + txt_Note.Value;
            if (!CheckChange(ddl_IsEnable.SelectedItem.Text, objDT.Rows[0]["IsEnable"].ToString(), lb_HiddenIsEnable)) lb_HiddenIsEnable.Text = objDT.Rows[0]["IsEnable"].ToString() == ddl_IsEnable.SelectedItem.Text ? "" : objDT.Rows[0]["IsEnable"].ToString() + Change + ddl_IsEnable.SelectedItem.Text;
            if (!CheckChange(ddl_Status.SelectedItem.Text, objDT.Rows[0]["MStatusSNO"].ToString(), lb_HiddenStatus)) lb_HiddenStatus.Text = objDT.Rows[0]["MStatusSNO"].ToString() == ddl_Status.SelectedItem.Text ? "" : objDT.Rows[0]["MStatusSNO"].ToString() + Change + ddl_Status.SelectedItem.Text;
        }
    }

    public static string ReturnORganCodeAndName(string OrganCode)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("OrganCode", OrganCode);
        DataTable ObjDT = objDH.queryData("Select [OrganCode]+'-'+[OrganName] Organ from Organ where OrganCode=@OrganCode", aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT.Rows[0][0].ToString();
        }
        else
        {
            return "";
        }
    }
    public static bool CheckChange(string Pairstring, string originalString)
    {
        if (Pairstring == "請選擇請選擇") Pairstring = "";
        if (Pairstring.Trim() == originalString.Trim())
        {

            return true;

        }
        else
        {
            return false;
        }
    }
    public static bool CheckChange(string Pairstring, string originalString, WebControl control)
    {
        if (Pairstring == "請選擇") Pairstring = "";
        if (Pairstring.Trim() == originalString.Trim())
        {
            control.Visible = false;
            return true;

        }
        else
        {
            control.Visible = true;
            return false;
        }
    }

}