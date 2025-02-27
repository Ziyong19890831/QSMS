using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_ExperienceManager : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    int viewrole = 1;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null)
        {
            userInfo = (UserInfo)Session["QSMS_UserInfo"];

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Utility.setTrainPlanNumber(ddl_TrainPlanNumber, "請選擇");
            Bind(1);
        }
    }

    protected void btnSearch_ServerClick(object sender, EventArgs e)
    {
        Bind(1);
    }
    private void Bind(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 20;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String sql = @"
                Select distinct ROW_NUMBER() OVER (ORDER BY PW.WESNO ) as ROW_NO,PW.WESNO,P.PName,P.PMail,PW.PersonID,PW.TrainType,TrainRoleType,
                PW.TrainPlanNumber,TC.TCName ,Case When P.PAccount is null Then'未註冊'Else'已註冊' End Register,
				Case When QC.CertSNO is null then '未回訓' Else '已回訓' End as 'IsTrain',
                STUFF ( PW.PersonID , 4 , 3 , '***' ) as 'PersonID_encryption'
                    from PDDI_WorkExperience PW
                    Left Join TrainTypeClass TC on TC.TrainType=PW.TrainType
                    Left Join Person P on P.PersonID=PW.PersonID
					Left Join QS_Certificate QC On QC.PersonID=PW.PersonID
                    LEFT JOIN Organ O ON O.OrganSNO = p.OrganSNO
                    LEFT JOIN Role R ON R.RoleSNO = p.RoleSNO
                    where  1=1
                    
        ";
        #region 權限篩選區塊
        if(userInfo.RoleOrganType != "S")
        {
            string RoleName = Utility.ReturnRoleName(userInfo.RoleGroup);
            sql += " And TrainPlanNumber=@RoleName";
            aDict.Add("RoleName", RoleName);
        }   
        #endregion


        #region 查詢篩選區塊
        if (!String.IsNullOrEmpty(txt_searchPName.Text))
        {
            sql += " And P.PName  Like '%' + @PName + '%' ";
            aDict.Add("PName", txt_searchPName.Text);
        }
        if (!String.IsNullOrEmpty(txt_searchTrainPlanNumber.Text))
        {
            sql += " And PW.TrainPlanNumber Like '%' + @TrainPlanNumber + '%' ";
            aDict.Add("TrainPlanNumber", txt_searchTrainPlanNumber.Text);
        }
        if (!String.IsNullOrEmpty(txt_searchPersonID.Text))
        {
            sql += @" and PW.PersonID=@PersonID";
            aDict.Add("PersonID", txt_searchPersonID.Text);
        }
        if (ddl_TrainPlanNumber.SelectedValue != "")
        {
            sql += @" and PW.TrainPlanNumber=@TrainPlanNumber";
            aDict.Add("TrainPlanNumber", ddl_TrainPlanNumber.SelectedValue);
        }

        #endregion
        sql += " order by PW.WESNO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Experience.DataSource = objDT.DefaultView;
        gv_Experience.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        Bind(page);
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {
       
        //0   衛生福利部長期照顧司「預防及延緩失能照護方案研發與人才培訓計畫」指導員
        //1   衛生福利部國民健康署106年「運動保健師資培訓計畫」(含地方政府交互認證)
        //2   教育部體育署中級國民體適能指導員
        LinkButton btn = (LinkButton)sender;
        String[] Parm = btn.CommandArgument.Split(',');
        string PersonID = Parm[0];
        string TrainType = Parm[1];
        string TrainPlanNumber = Parm[2];
        string TrainRoleType = Parm[3];
        string PName = Parm[4];
        string PMail = Parm[5];
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonID", PersonID);
        aDict.Add("TrainType", TrainType);
        aDict.Add("TrainPlanNumber", TrainPlanNumber);
        aDict.Add("TrainRoleType", TrainRoleType);
        aDict.Add("EventName", "刪除資料");
        aDict.Add("CreateUserID", userInfo.PersonSNO);
        DataHelper objDH = new DataHelper();
       
        if (CheckRegister(PersonID))//如果有註冊，需要寄信
        {
            if (CheckTrainPlanNumber(PersonID))
            {   //仍保有師資經歷
                objDH.executeNonQuery("dbo.SP_WorkExperienceDelete @PersonID, @TrainType,@TrainPlanNumber,@TrainRoleType,@CreateUserID,@EventName", aDict);
                string getTemplate = System.IO.File.ReadAllText(Server.MapPath("../SysFile/TemplateMailForTrainTypeFirst.html"));
                getTemplate = getTemplate.Replace("@User", PName);
                getTemplate = getTemplate.Replace("@Exp", TrainPlanNumber);
                getTemplate = getTemplate.Replace("@TrainPlanNumber", ReturnTrainPlanNumber(PersonID));
                Utility.SendMail("醫事人員戒菸服務訓練系統", getTemplate, PMail);
            }
            else
            {   //格已被註銷(系統帳號已停用)
                objDH.executeNonQuery("dbo.SP_WorkExperienceDelete @PersonID, @TrainType,@TrainPlanNumber,@TrainRoleType,@CreateUserID,@EventName", aDict);
                string getTemplate = System.IO.File.ReadAllText(Server.MapPath("../SysFile/TemplateMailForTrainTypeSecond.html"));
                getTemplate = getTemplate.Replace("@User", PName);
                getTemplate = getTemplate.Replace("@Exp", TrainPlanNumber);
                Utility.SendMail("醫事人員戒菸服務訓練系統", getTemplate, PMail);
                
            }
        }
        else//沒有註冊直接刪除，不用寄信
        {
            objDH.executeNonQuery("dbo.SP_WorkExperienceDelete @PersonID, @TrainType,@TrainPlanNumber,@TrainRoleType,@CreateUserID,@EventName", aDict);
        }
        
        //string getTemplate;
        //switch (TrainType)
        //{
        //    case "0":
        //         getTemplate = System.IO.File.ReadAllText(Server.MapPath("../SysFile/TemplateMailForTrainTypeFirst.html"));
        //        getTemplate = getTemplate.Replace("@User", PName);
        //        getTemplate = getTemplate.Replace("@Exp", TrainPlanNumber);
        //        break;
        //    case "1":
        //         getTemplate = System.IO.File.ReadAllText(Server.MapPath("../SysFile/TemplateMailForTrainTypeSecond.html"));
        //        break;
        //    case "2":
        //         getTemplate = System.IO.File.ReadAllText(Server.MapPath("../SysFile/TemplateMailForTrainTypeThird.html"));
        //        break;
        //}
        Response.Write("<script>alert('刪除成功!') </script>");
        btnPage_Click(sender, e);
        return;
    }

    public static string ReturnTrainPlanNumber(string PersonID)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonID", PersonID);
        string SQL = "SELECT  [TrainPlanNumber]  FROM [PDDI].[dbo].[PDDI_WorkExperience] where PersonID=@PersonID ";
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            string TrainPlanNumber = "";
            for (int i = 0; i < ObjDT.Rows.Count; i++)
            {
                 TrainPlanNumber += "衛生福利部長期照顧司「預防及延緩失能照護方案研發與人才培訓計畫」指導員" + ObjDT.Rows[i]["TrainPlanNumber"].ToString()+"<br />";
            }
            
            return TrainPlanNumber;
        }
        return "";
    }
    public static bool CheckTrainPlanNumber(string PersonID)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonID", PersonID);
        string SQL = "SELECT  [TrainPlanNumber]  FROM [PDDI].[dbo].[PDDI_WorkExperience] where PersonID=@PersonID ";
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        if (ObjDT.Rows.Count > 1)
        {
            return true;
        }
        if(ObjDT.Rows.Count == 1)
        {
            return false;
        }
        else
        {
            return false;
        }
    }

    public static bool CheckRegister(string PersonID)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonID", PersonID);
        string SQL = "SELECT  PAccount  FROM [PDDI].[dbo].[Person] where PersonID=@PersonID ";
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            if (ObjDT.Rows[0][0] != DBNull.Value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;

    }

    protected void btn_Del_Click(object sender, EventArgs e)
    {

    }

    protected void btnSendMail_Click(object sender, EventArgs e)
    {

    }
}