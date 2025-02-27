using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
//using System.Net.Http;
using System.IO;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Collections;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

/// <summary>
/// Utility 的摘要描述
/// </summary>
public class Utility
{
    public Utility()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    #region 下拉選單初始化

    /// <summary>
    /// 取得縣市行政區代碼
    /// </summary>
    /// <param name="ddl"></param>
    /// <param name="defaultString"></param>
    public static void setAreaCodeA(DropDownList ddl, String DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY AREA_CODE) as ROW_NO, AREA_CODE, AREA_NAME FROM CD_AREA WHERE AREA_TYPE='A'", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static void setRoleExpection(DropDownList ddl, String DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT [RESNO],[REName]  FROM [RoleException]", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static void setTrainPlanNumber(DropDownList ddl, String DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT Distinct [TrainPlanNumber] FROM [PDDI].[dbo].[PDDI_WorkExperience] ", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
    public static void setTrainPlanNumber(DropDownList ddl, string RoleGroup,string RoleOrganType, String DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        
        string RoleName = ReturnRoleName(RoleGroup);
        aDict.Add("RoleName", RoleName);
        DataHelper objDH = new DataHelper();
        string SQL = @"SELECT Distinct PW.[TrainPlanNumber]+'-'+S.TrainPlanName TrainPlan FROM [PDDI].[dbo].[PDDI_WorkExperience]  PW
                        Left Join Source266 S On S.TrainPlanNumber=PW.TrainPlanNumber
                        where 1=1 and PW.TrainPlanNumber is not null";
        if (RoleOrganType != "S")
        {
            SQL += " And PW.TrainPlanNumber=@RoleName";
        }
        
        DataTable objDT = objDH.queryData(SQL, aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
    public static void setTrainTypeClass(DropDownList ddl, String DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT  [TrainType],[TCName]  FROM [PDDI].[dbo].[TrainTypeClass]", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static void setAreaCodeACheckBoxList(CheckBoxList check)
    {

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY AREA_CODE) as ROW_NO, AREA_CODE, AREA_NAME FROM CD_AREA WHERE AREA_TYPE='A'", null);
        check.DataSource = objDT;
        check.DataBind();
   
    }

    public static void setAreaCodeACheckBoxList1(CheckBoxList check, String[] AreaCodeA)
    {
        string sql = "";
        for(int i = 0; i < AreaCodeA.Length; i++)
        {
            sql += " SELECT ROW_NUMBER() OVER (ORDER BY AREA_CODE) as ROW_NO, AREA_CODE, AREA_NAME FROM CD_AREA WHERE AREA_TYPE='B' AND AREA_CODE LIKE '"+ AreaCodeA[i] + "' + '%' ";
            sql += " Union";
        }
        sql = sql.Substring(0, sql.Length - 5);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, null);
        check.DataSource = objDT;
        check.DataBind();

    }
    /// <summary>
    /// 取得縣市行政區代碼_依管理權限區分顯示
    /// </summary>
    /// <param name="ddl"></param>
    /// <param name="AreaCodeA"></param>
    /// <param name="AreaCodeB"></param>
    /// <param name="RoleOrganType"></param>
    /// <param name="defaultString"></param>
    public static void setAreaCodeA_Access(DropDownList ddl, string AreaCodeA, string AreaCodeB, string RoleOrganType, String DefaultString = null)
    {

        string OfSQL = "";
        switch (RoleOrganType)
        {
            case "S":   //S皆可新增所有單位
                OfSQL = "";
                break;
            case "A":   //A只能新增同區域單位
                OfSQL = "And Substring(AREA_CODE,1,2)='" + AreaCodeA + "'";
                break;
            case "B":   //B只能新增同區域單位
                OfSQL = "And AREA_CODE='" + AreaCodeB + "'";
                break;
            case "U":   //協會皆可新增所有單位
                OfSQL = "";
                break;
            default:
                break;
        }

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"SELECT ROW_NUMBER() OVER (ORDER BY AREA_CODE) as ROW_NO, AREA_CODE, 
            AREA_NAME FROM CD_AREA WHERE AREA_TYPE='A' " + OfSQL, null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
    public static void setEventFor22(DropDownList ddl,string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();        
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY EventSNO) as ROW_NO, EventSNO, EventName FROM Event WHERE SYSTEM_ID='S22'", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static void setAreaCodeB(DropDownList ddl, String AreaCodeA, String DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("AreaCodeA", AreaCodeA);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY AREA_CODE) as ROW_NO, AREA_CODE, AREA_NAME FROM CD_AREA WHERE AREA_TYPE='B' AND AREA_CODE LIKE @AreaCodeA + '%'", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
    public static void setTsType(DropDownList ddl, String DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY TSSNO) as ROW_NO, TSSNO, TsTypeName FROM TsTypeClass ", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
            ddl.Items.Add(new ListItem("其他", "99"));
 
        }
    }

    public static void setTsTypeAccount(DropDownList ddl, String DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT TSSNO, TsTypeName FROM TsTypeClass where RoleSNO='10'", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));            

        }
    }

    public static void setJob(DropDownList ddl, String DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT JSNO, JobName FROM QS_Job", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));

        }
    }

    public static void setOrgan(DropDownList ddl, String AreaCodeB, String DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("AreaCodeB", AreaCodeB);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY OrganCode) as ROW_NO, OrganCode, OrganName FROM ORGAN WHERE AreaCodeB=@AreaCodeB", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
    public static void setRoleException(DropDownList ddl, String DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select RESNO,REName from RoleException", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
    public static void setOrganID(DropDownList ddl, String AreaCodeB, String DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("AreaCodeB", AreaCodeB);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY OrganCode) as ROW_NO, OrganSNO, OrganCode + '-' + OrganName AS ORGAN_NAME FROM ORGAN WHERE AreaCodeB=@AreaCodeB", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static void setAudit(DropDownList ddl,  String DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select * from config where PGroup='EventAudit'", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
    public static void setEventInvite(DropDownList ddl, String DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select * from config where PGroup='EventInvite'", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    /// <summary>
    /// 取得角色別
    /// </summary>
    /// <param name="ddl"></param>
    /// <param name="RoleOrganType"></param>
    /// <param name="defaultString"></param>
    public static void setRole_Access(DropDownList ddl, bool IsAdmin, string RoleOrganType, string RoleLevel, string RoleGroup, string DefaultString = null)
    {
        string fSQL = IsAdmin == true ? "IsAdmin=1" : "IsAdmin=0";

        switch (RoleOrganType)
        {
            //case "S":   //S皆可新增所有角色
            //    fSQL += " And RoleLevel>=" + RoleLevel; break;
            //case "A":
            //    fSQL += " And RoleLevel>=" + RoleLevel; break;
            //case "B":
            //    fSQL += " And RoleLevel>=" + RoleLevel; break;
            case "S":   //SAB皆可新增大於其RoleLevel的角色，
            case "A":
            case "B":
                fSQL += " And RoleLevel>=" + RoleLevel; break;
            case "U":   //協會皆可新增相同RoleGroup的角色
                fSQL += " And RoleGroup='" + RoleGroup + "'"; break;
            default: break;
        }
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT RoleSNO , RoleName FROM Role WHERE " + fSQL, aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    /// <summary>
    /// 取得Config資料表的參數值
    /// </summary>
    /// <param name="ddl"></param>
    /// <param name="sql"></param>
    /// <param name="defaultString"></param>
    public static void SetDdlConfig(DropDownList ddl, string ddlType, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PGroup", ddlType);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("select a.PVal , a.MVal FROM Config a where a.PGroup = @PGroup", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
    public static void SetCountry(DropDownList ddl, string ddlType, string DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("  Select PVal,MVal from Config where PGroup='Country' ", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }

    public static void SetDdlConfigFor22Event(DropDownList ddl, string ddlType, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PGroup", ddlType);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("select a.PVal , a.MVal FROM Config a where a.PGroup = @PGroup", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    //取得roleName
    public static void setRoleNormal(DropDownList ddl, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT RoleSNO, RoleName FROM Role WHERE IsAdmin = 0 and DocGroup is not null", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static void setRoleAdmin(DropDownList ddl, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT RoleSNO, RoleName FROM Role WHERE IsAdmin = 1", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static void setUnitRoleAdmin(DropDownList ddl, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT RoleSNO, RoleName ,RoleGroup FROM Role WHERE IsAdmin = 1 and [RoleOrganType]='U'", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
    /// <summary>
    /// 取得學員狀態
    /// </summary>
    /// <param name="ddl"></param>
    /// <param name="defaultString"></param>
    public static void setMemberStatus(DropDownList ddl, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT MStatusSNO, MName FROM QS_MemberStatus", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    /// <summary>
    /// 取得Course
    /// </summary>
    /// <param name="ddl"></param>
    /// <param name="defaultString"></param>
    public static void setCourse(DropDownList ddl, string filter, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT CourseSNO, CourseName FROM QS_Course Where 1=1 " + filter, aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    //取得所有測驗名稱
    public static void setPlanName(DropDownList dd1, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT [PClassSNO], [PlanName] FROM [QS_CoursePlanningClass] WHERE [IsEnable] = 1", aDict);
        dd1.DataSource = objDT;
        dd1.DataBind();
        if (DefaultString != null)
        {
            dd1.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static void setCtype(DropDownList dd1, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT [Mval], [Pval] FROM [Config] WHERE [PGroup] = 'CourseCType' ", aDict);
        dd1.DataSource = objDT;
        dd1.DataBind();
        if (DefaultString != null)
        {
            dd1.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }



    public static void setEPClassSNO(DropDownList dd1, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT [EPClassSNO], [PlanName] FROM [QS_ECoursePlanningClass]", aDict);
        dd1.DataSource = objDT;
        dd1.DataBind();
        if (DefaultString != null)
        {
            dd1.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    //取得所有E-Learn名稱
    public static void setELearn(DropDownList dd1, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ELCode, ELName FROM QS_CourseELearning WHERE IsEnable = 1", aDict);
        dd1.DataSource = objDT;
        dd1.DataBind();
        if (DefaultString != null)
        {
            dd1.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static void setMStatus(DropDownList dd1, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT[MStatusSNO],[MName] FROM [QS_MemberStatus]", aDict);
        dd1.DataSource = objDT;
        dd1.DataBind();
        if (DefaultString != null)
        {
            dd1.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static void setAllELearn(DropDownList dd1, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ELCode, ELName FROM QS_CourseELearning ", aDict);
        dd1.DataSource = objDT;
        dd1.DataBind();
        if (DefaultString != null)
        {
            dd1.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }
    //取得所有證書名稱
    //取得所有E-Learn名稱
    public static void setCtypeName(DropDownList dd1, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT [CTypeSNO],[CTypeName]  FROM [QS_CertificateType]", aDict);
        dd1.DataSource = objDT;
        dd1.DataBind();
        if (DefaultString != null)
        {
            dd1.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    //取得ELearnSection未綁定
    public static void setELearnSectionWithNoBind(DropDownList ddl, string ELCode, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        aDict.Add("ELCode", ELCode);
        DataTable objDT = objDH.queryData(@"
            SELECT ces.ELSCode, ces.ELSName 
            FROM QS_CourseELearningSection ces
                Left Join QS_Course c On c.ELSCode= ces.ELSCode
            Where ELCode=@ELCode And c.ELSCode is null
        ", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    #endregion

    #region 權限判斷

    /// <summary>
    /// 取得頁面為資料夾之清單
    /// </summary>
    /// <param name="ddl"></param>
    /// <param name="DefaultString"></param>
    public static void setPPLink(DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY GROUPORDER, PLINKNAME) as ROW_NO, PLINKSNO, PLINKNAME FROM PageLink WHERE ISENABLE=1 AND ISDIR=1", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    /// <summary>
    /// 依RoleOrganType設定權限篩選
    /// </summary>
    /// <param name="Dict">Parameter</param>
    /// <param name="userInfo">UserInfo</param>
    /// <param name="CompareRoleLevel">比較RoleLevel</param>
    /// <returns>依RoleOrganType設定權限篩選，呼叫本段Code，所回傳的sql必須接上前段，且必須有[Organ O]&[Role R]表存在</returns>
    public static string setSQLAccess_ByRoleOrganType(Dictionary<string, object> Dict, UserInfo userInfo, bool CompareRoleLevel = true)
    {
        //很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！
        //依RoleOrganType設定權限篩選，呼叫本段Code，所回傳的sql必須接上前段，且必須有[Organ O]&[Role R]表存在
        //依RoleOrganType設定權限篩選，呼叫本段Code，所回傳的sql必須接上前段，且必須有[Organ O]&[Role R]表存在
        //依RoleOrganType設定權限篩選，呼叫本段Code，所回傳的sql必須接上前段，且必須有[Organ O]&[Role R]表存在
        //LEFT JOIN Person P on P.PersonSNO = XXXXXX
        //LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
        //LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
        //很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！


        //判斷順序：
        //1.RoleOrganType為S時，代表該管理者可以[向下管理]帳號
        //2.RoleOrganType為A時，代表該管理者可以[管理與其相同地理區域]的角色，管理的依據為Organ.AreaCodeA
        //3.RoleOrganType為B時，代表該管理者可以[管理與其相同地理區域]的角色，管理的依據為Organ.AreaCodeB
        //4.RoleOrganType為U時，代表該管理者僅能管與自己相同RoleGroup的角色
        //5.其它無權限

        string sql = "";

        //判斷RoleOrganType
        switch (userInfo.RoleOrganType)
        {
            case "S":
                if (CompareRoleLevel)
                {
                    Dict.Add("RoleLevel", userInfo.RoleLevel);
                    sql += " And R.RoleLevel>=@RoleLevel";
                }
                break;
            case "A":
                Dict.Add("AreaCodeA", userInfo.AreaCodeA);
                sql += " And O.AreaCodeA=@AreaCodeA";
                if (CompareRoleLevel)
                {
                    sql += " And R.RoleLevel>=@RoleLevel";
                    Dict.Add("RoleLevel", userInfo.RoleLevel);
                }
                break;
            case "B":
                Dict.Add(" AreaCodeB", userInfo.AreaCodeB);
                sql += " And O.AreaCodeB=@AreaCodeB";
                if (CompareRoleLevel)
                {
                    sql += " And R.RoleLevel>=@RoleLevel";
                    Dict.Add("RoleLevel", userInfo.RoleLevel);
                }
                break;
            case "U":
                Dict.Add("RoleGroup", userInfo.RoleGroup);
                sql += " And R.RoleGroup=@RoleGroup";
                break;
            default:
                sql += " And 1=2";
                break;
        }
        return sql;
    }

    public static string setSQLAccess_ByCertificate(Dictionary<string, object> dicpd, UserInfo userInfo, bool CompareRoleLevel = true)
    {
        //很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！
        //依RoleOrganType設定權限篩選，呼叫本段Code，所回傳的sql必須接上前段，且必須有[Organ O]&[Role R]表存在
        //依RoleOrganType設定權限篩選，呼叫本段Code，所回傳的sql必須接上前段，且必須有[Organ O]&[Role R]表存在
        //依RoleOrganType設定權限篩選，呼叫本段Code，所回傳的sql必須接上前段，且必須有[Organ O]&[Role R]表存在
        //LEFT JOIN Person P on P.PersonSNO = XXXXXX
        //LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
        //LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
        //很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！


        //判斷順序：
        //1.RoleOrganType為S時，代表該管理者可以[向下管理]帳號
        //2.RoleOrganType為A時，代表該管理者可以[管理與其相同地理區域]的角色，管理的依據為Organ.AreaCodeA
        //3.RoleOrganType為B時，代表該管理者可以[管理與其相同地理區域]的角色，管理的依據為Organ.AreaCodeB
        //4.RoleOrganType為U時，代表該管理者僅能管與自己相同RoleGroup的角色
        //5.其它無權限

        string sql = "";

        //判斷RoleOrganType
        switch (userInfo.RoleOrganType)
        {
            case "S":
                if (CompareRoleLevel)
                {
                    dicpd.Add("RoleLevel", userInfo.RoleLevel);
                    sql += " And R.RoleLevel>=@RoleLevel";
                }
                break;
            case "A":
                dicpd.Add("AreaCodeA", userInfo.AreaCodeA);
                sql += " And O.AreaCodeA=@AreaCodeA";
                if (CompareRoleLevel)
                {
                    sql += " And R.RoleLevel>=@RoleLevel";
                    dicpd.Add("RoleLevel", userInfo.RoleLevel);
                }
                break;
            case "B":
                dicpd.Add(" AreaCodeB", userInfo.AreaCodeB);
                sql += " And O.AreaCodeB=@AreaCodeB";
                if (CompareRoleLevel)
                {
                    sql += " And R.RoleLevel>=@RoleLevel";
                    dicpd.Add("RoleLevel", userInfo.RoleLevel);
                }
                break;
            case "U":
                switch (userInfo.RoleGroup)
                {
                    case "10":
                        sql += " And (QC.CTypeSNO In (1,8,53) or QC.CTypeSNO is null)";
                        break;
                    case "11":
                        sql += " And (QC.CTypeSNO In (2,3,54,55) or QC.CTypeSNO is null)";
                        break;
                    case "12":
                        sql += " And P.RoleSNO=12 And (QC.CTypeSNO In (6,7,11,12,52,56,57) or QC.CTypeSNO is null)";
                        break;
                    case "13":
                        sql += " And P.RoleSNO=13 And (QC.CTypeSNO In (4,5,9,10,51,56,57) or QC.CTypeSNO is null)";
                        break;
                }

                break;
            default:
                sql += " And 1=2";
                break;
        }
        return sql;
    }

    public static string setSQLAccess_ByRoleOrganTypeForEintegral(Dictionary<string, object> Dict, UserInfo userInfo, bool CompareRoleLevel = true)
    {
        //很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！
        //依RoleOrganType設定權限篩選，呼叫本段Code，所回傳的sql必須接上前段，且必須有[Organ O]&[Role R]表存在
        //依RoleOrganType設定權限篩選，呼叫本段Code，所回傳的sql必須接上前段，且必須有[Organ O]&[Role R]表存在
        //依RoleOrganType設定權限篩選，呼叫本段Code，所回傳的sql必須接上前段，且必須有[Organ O]&[Role R]表存在
        //LEFT JOIN Person P on P.PersonSNO = XXXXXX
        //LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
        //LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
        //很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！很重要說N遍！


        //判斷順序：
        //1.RoleOrganType為S時，代表該管理者可以[向下管理]帳號
        //2.RoleOrganType為A時，代表該管理者可以[管理與其相同地理區域]的角色，管理的依據為Organ.AreaCodeA
        //3.RoleOrganType為B時，代表該管理者可以[管理與其相同地理區域]的角色，管理的依據為Organ.AreaCodeB
        //4.RoleOrganType為U時，代表該管理者僅能管與自己相同RoleGroup的角色
        //5.其它無權限

        string sql = "";

        //判斷RoleOrganType
        switch (userInfo.RoleOrganType)
        {
            case "S":
                if (CompareRoleLevel)
                {
                    Dict.Add("RoleLevel", userInfo.RoleLevel);
                    sql += " And getall.RoleLevel>=@RoleLevel";
                }
                break;
            case "A":
                Dict.Add("AreaCodeA", userInfo.AreaCodeA);
                sql += " And getall.AreaCodeA=@AreaCodeA";
                if (CompareRoleLevel)
                {
                    sql += " And getall.RoleLevel>=@RoleLevel";
                    Dict.Add("RoleLevel", userInfo.RoleLevel);
                }
                break;
            case "B":
                Dict.Add(" AreaCodeB", userInfo.AreaCodeB);
                sql += " And getall.AreaCodeB=@AreaCodeB";
                if (CompareRoleLevel)
                {
                    sql += " And getall.RoleLevel>=@RoleLevel";
                    Dict.Add("RoleLevel", userInfo.RoleLevel);
                }
                break;
            case "U":
                Dict.Add("RoleGroup", userInfo.RoleGroup);
                sql += " And getall.RoleGroup=@RoleGroup";
                break;
            default:
                sql += " And 1=2";
                break;
        }
        return sql;
    }
    //public static string setSQLAccess(Dictionary<string, object> Dict, UserInfo userInfo)
    //{

    //    string sql = "";

    //    //判斷順序：
    //    //1.RoleOrganType為S時，代表該管理者可以[向下管理]帳號，
    //    //      CompareRoleLevel=true：比自己RoleLevel還要大的所有角色
    //    //      CompareRoleLevel=false：全部角色
    //    //2.RoleOrganType為A時，代表該管理者可以[管理與其相同地理區域]的角色，管理的依據為Organ.AreaCodeA
    //    //3.RoleOrganType為B時，代表該管理者可以[管理與其相同地理區域]的角色，管理的依據為Organ.AreaCodeB
    //    //4.RoleOrganType為U時，代表該管理者僅能管與自己相同RoleGroup的角色
    //    //5.其它無權限

    //    //判斷RoleOrganType
    //    switch (userInfo.RoleOrganType)
    //    {
    //        case "S":
    //            break;
    //        case "A":
    //            Dict.Add("AreaCodeA", userInfo.AreaCodeA);
    //            sql += " And O.AreaCodeA=@AreaCodeA";
    //            break;
    //        case "B":
    //            Dict.Add("AreaCodeB", userInfo.AreaCodeB);
    //            sql += " And O.AreaCodeB=@AreaCodeB";
    //            break;
    //        case "U":
    //            Dict.Add("RoleGroup", userInfo.RoleGroup);
    //            sql += " And R.RoleGroup=@RoleGroup";
    //            break;
    //        default:
    //            break;
    //    }
    //    return sql;
    //}

    public static string setSQLAccess_ByCreateUserID(Dictionary<string, object> Dict, UserInfo userInfo, string AliasName = "")
    {
        string sql = "";
        if (userInfo.RoleOrganType != "S")
        {
            sql += "And " + AliasName + "CreateUserID=@CreateUserID ";
            Dict.Add("CreateUserID", userInfo.PersonSNO);
        }
        return sql;
    }

    ///// <summary>
    ///// 取得是否有修改該帳號的權限
    ///// </summary>
    //public static Boolean chkPersonnelAUTH(string aOrganSNO, string aRoleLevel, string uPersonSNO)
    //{

    //    //管理者為"系統管理員"
    //    if (aRoleLevel == "S") return true;

    //    //管理者為"機構管理員"
    //    else if (aRoleLevel == "A")
    //    {
    //        Dictionary<string, object> aDict = new Dictionary<string, object>();
    //        aDict.Add("PersonSNO", uPersonSNO);
    //        DataHelper objDH = new DataHelper();
    //        DataTable objDT = objDH.queryData("Select * from Person Where PersonSNO=@PersonSNO", aDict);

    //        if (objDT.Rows.Count > 0)
    //        {
    //            string uRoleLevel = objDT.Rows[0]["RoleLevel"].ToString();
    //            string uOrganSNO = objDT.Rows[0]["OrganSNO"].ToString();

    //            if (uRoleLevel == "B" && aOrganSNO == uOrganSNO) return true;
    //            else return false;
    //        }
    //        else return false;
    //    }
    //    else return false;

    //    //return true;
    //}


    #endregion



    #region 發送簡訊&Mail

    /// <summary>
    /// 發送Mail
    /// </summary>
    /// <param name="MailSub">主旨</param>
    /// <param name="MailBody">信件內容</param>
    /// <param name="SendTo">收件人</param>
    /// <returns>是否成功</returns>
    /// 
    public static void SendMail(string MailSub, string MailBody, string SendTo = "emma.chao@iisigroup.com")
    {

        DataHelper objDH = new DataHelper();
        Dictionary<string, object> Dict = new Dictionary<string, object>();
        Dict.Add("Account", "Account");
        Dict.Add("Host", "Host");
        Dict.Add("Port", "Port");
        Dict.Add("Psw", "Psw");
        Dict.Add("SSL", "SSL");
        DataTable objDT = objDH.queryData(@"
            Select 
                (Select PVal From Config Where PID=@Account) Account,
                (Select PVal From Config Where PID=@Host) Host,
                (Select PVal From Config Where PID=@Port) Port,
                (Select PVal From Config Where PID=@Psw) Psw,
                (Select PVal From Config Where PID=@SSL) SSL
        ", Dict);

        string Account = objDT.Rows[0]["Account"].ToString();
        string Host = objDT.Rows[0]["Host"].ToString();
        int Port = Convert.ToInt16(objDT.Rows[0]["Port"]);
        string Psw = objDT.Rows[0]["Psw"].ToString();
        bool SSL = Convert.ToBoolean(objDT.Rows[0]["SSL"]);

        string smtpServer = Host;
        int smtpPort = Port;
        bool smtpSSL = SSL;
        string MailAccount = Account;
        string MailName = "醫事人員戒菸服務訓練系統";
        string MailPsw = Psw;
        string MailFrom = Account;
        bool isBodyHtml = true;
        string[] SendToOver100 = SendTo.Split(',');

        for (int i = 0; i < SendToOver100.Length; i++)
        {
            MailMessage NewMail = new System.Net.Mail.MailMessage();
            NewMail.SubjectEncoding = System.Text.Encoding.UTF8; //主題編碼格式
            NewMail.Subject = MailSub; //主題
            NewMail.IsBodyHtml = isBodyHtml;  //HTML語法(true:開啟false:關閉)
            NewMail.BodyEncoding = System.Text.Encoding.UTF8; //內文編碼格式
            NewMail.Body = MailBody; //內文
            NewMail.From = new MailAddress(MailFrom, MailName); //發送者
            NewMail.To.Add(SendToOver100[i]);
            SmtpClient NewSmtp = new SmtpClient(); //建立SMTP連線
            NewSmtp.Credentials = new System.Net.NetworkCredential(MailAccount, MailPsw); //連線驗證
            NewSmtp.Port = smtpPort; //SMTP Port
            NewSmtp.Host = smtpServer; //SMTP主機名稱
            NewSmtp.EnableSsl = smtpSSL; //開啟SSL驗證
            //NewSmtp.UseDefaultCredentials = true;
            NewSmtp.ServicePoint.MaxIdleTime = 1;
            NewSmtp.Send(NewMail); //發送
            System.Threading.Thread.Sleep(500);
            NewMail.Dispose();



        }




    }
    public static string sendSMS(string SMStempFile)
    {

        DataHelper objDH = new DataHelper();
        Dictionary<string, object> Dict = new Dictionary<string, object>();
        Dict.Add("SMSUsername", "SMSUsername");
        Dict.Add("SMSPsw", "SMSPsw");
        Dict.Add("SMSAPI", "SMSAPI");
        DataTable objDT = objDH.queryData(@"
            Select 
                (Select PVal From Config Where PID=@SMSUsername) SMSUsername,
                (Select PVal From Config Where PID=@SMSPsw) SMSPsw,
                (Select PVal From Config Where PID=@SMSAPI) SMSAPI
        ", Dict);

        string api = objDT.Rows[0]["SMSAPI"].ToString();
        string username = objDT.Rows[0]["SMSUsername"].ToString();
        string password = objDT.Rows[0]["SMSPsw"].ToString();

        HttpClient client = new HttpClient();
        StringBuilder url = new StringBuilder(api);
        url.Append("username=").Append(username);
        url.Append("&password=").Append(password);
        //string tempPath = "C:\\IISI\\服務醫事人員管理系統\\01_Code\\QSMS_dev\\SMSTemp\\SendSMSList.txt";
        var stream = new FileStream(SMStempFile, FileMode.Open, FileAccess.Read);
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        var response = client.PostAsync(url.ToString(), new ByteArrayContent(buffer)).Result;
        string responseBody = response.Content.ReadAsStringAsync().Result;
        stream.Close();
        stream.Dispose();
        File.Delete(SMStempFile);
        return responseBody;

    }
    /// <summary>
    /// 發送SMS
    /// </summary>


    #endregion

    #region 報表匯出

    /// <summary>
    /// Dictionary , Key = 設定匯出欄位與名稱  , Value = 來源Source
    /// </summary> 
    public static DataTable DataProcess(Dictionary<List<ListItem>, DataTable> dic)
    {

        DataTable resultInfo = new DataTable();
        var showItem1 = dic.Select(x => x.Key).ToList();
        Dictionary<string, string> adict = new Dictionary<string, string>();
        foreach (var item in showItem1)
        {
            foreach (var item1 in item)
            {
                adict.Add(item1.Value, item1.Text);

            }

        }
        resultInfo = dic.FirstOrDefault().Value.Copy();

        foreach (DataColumn dc in dic.FirstOrDefault().Value.Columns)
        {
            var dicItem = adict.Where(o => o.Key.ToUpper() == dc.ColumnName.ToUpper()).FirstOrDefault();
            if (dicItem.Key == null || string.IsNullOrEmpty(dicItem.Value))
            {
                //刪除不匯出的欄位
                resultInfo.Columns.Remove(dc.ColumnName);
            }
            else
            {
                //變更Excel ColumnName
                resultInfo.Columns[dicItem.Key].ColumnName = dicItem.Value.ToString();
            }
        }
        return resultInfo;
    }

    public static void OpenExportWindows(Page _this, string reportType)
    {
        string url = "ReportSetColumn.aspx?ReporType=" + reportType;
        string fullURL = "window.open('" + url + "', '_blank', 'height=500,width=800,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(_this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }
    public static void OpenExportWindowsForDetail(Page _this, string reportType, string reportType1, string reportType2, string reportType3, string reportType4)
    {
        string url = "ReportSetColumn.aspx?ReporType=" + reportType + "&ReporType1=" + reportType1 + "&ReporType2=" + reportType2 + "&ReporType3=" + reportType3 + "&ReporType4=" + reportType4;
        string fullURL = "window.open('" + url + "', '_blank', 'height=500,width=800,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(_this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }
    public static void OpenExportWindowsForDetail(Page _this, string reportType, string reportType1, string reportType2, string reportType3, string reportType4, string reportType5)
    {
        string url = "ReportSetColumn.aspx?ReporType=" + reportType+ "&ReporType1="+ reportType1 + "&ReporType2=" + reportType2 + "&ReporType3=" + reportType3 + "&ReporType4=" + reportType4 + "&ReporType5="+ reportType5+ "";
        string fullURL = "window.open('" + url + "', '_blank', 'height=500,width=800,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
        ScriptManager.RegisterStartupScript(_this, typeof(string), "OPEN_WINDOW", fullURL, true);
    }
    #endregion

    #region 系統輔助功能

    /// <summary>
    /// 顯示頁碼
    /// </summary>
    /// <param name="rowCount">總筆數</param>
    /// <param name="pageNumber">目前頁碼</param>
    /// <param name="pageRecord">每頁顯示筆數</param>
    /// <param name="pageShow">頁碼顯示頁數</param>
    /// <returns></returns>
    public static String showPageNumber(int rowCount, int pageNumber, int pageRecord = 10, int pageShow = 10)
    {
        String returnString = "";
        if ((rowCount <= 0) || (pageNumber <= 0) || (pageRecord <= 0) || (pageShow <= 0)) return "";
        int maxPageNumber = (rowCount - 1) / pageRecord + 1;
        if (maxPageNumber <= 1) return "";
        returnString += "<div class=\"center pages both w10\">";

        //第一頁
        if (pageNumber > 1) returnString += String.Format("<a href=\"#\" style=\"margin-right:0px;\" onclick=\"_goPage({0});return false;\"><i class=\"fa fa-angle-double-left\" aria-hidden=\"true\"></i></a>", 1);
        //上一頁
        if (pageNumber > 1) returnString += String.Format("<a href=\"#\" style=\"margin-left:0px;\" onclick=\"_goPage({0});return false;\">上一頁</a>", pageNumber - 1);
        //顯示頁碼
        int pageShowStart = pageNumber - pageShow / 2;
        if (pageShowStart < 1) pageShowStart = 1;
        int pageShowEnd = pageShowStart + pageShow - 1;
        if (pageShowEnd > maxPageNumber) pageShowEnd = maxPageNumber;
        for (int i = pageShowStart; i <= pageShowEnd; i++)
        {
            if (i == pageNumber)
            {
                returnString += String.Format("<a href=\"#\" style=\"color:red;cursor:default;\" onclick=\"return false;\">{0}</a>{1}", i, (i == pageShowEnd) ? "" : " ");
            }
            else
            {
                returnString += String.Format("<a href=\"#\" onclick=\"_goPage({0});return false;\">{0}</a>{1}", i, (i == pageShowEnd) ? "" : " ");
            }
        }
        //下一頁
        if (pageNumber < maxPageNumber) returnString += String.Format("<a href=\"#\" style=\"margin-right:0px;\" onclick=\"_goPage({0});return false;\">下一頁</a>", pageNumber + 1);
        //最後一頁
        if (pageNumber < maxPageNumber) returnString += String.Format("<a href=\"#\" style=\"margin-left:0px;\" onclick=\"_goPage({0});return false;\"><i class=\"fa fa-angle-double-right\" aria-hidden=\"true\"></i></a>", maxPageNumber);
        //計算總筆數
        if (rowCount != 0) returnString += string.Format("共<span style='color:red'>{0}</span>筆", rowCount);
        returnString += "</div>";
        return returnString;
    }
   

    public static String showPageNumber1(int rowCount, int pageNumber, int pageRecord = 10, int pageShow = 10)
    {
        String returnString = "";
        if ((rowCount <= 0) || (pageNumber <= 0) || (pageRecord <= 0) || (pageShow <= 0)) return "";
        int maxPageNumber = (rowCount - 1) / pageRecord + 1;
        if (maxPageNumber <= 1) return "";
        returnString += "<div class=\"center pages both w10\">";

        //第一頁
        if (pageNumber > 1) returnString += String.Format("<a href=\"#\" style=\"margin-right:0px;\" onclick=\"_goPage1({0});return false;\"><i class=\"fa fa-angle-double-left\" aria-hidden=\"true\"></i></a>", 1);
        //上一頁
        if (pageNumber > 1) returnString += String.Format("<a href=\"#\" style=\"margin-left:0px;\" onclick=\"_goPage1({0});return false;\">上一頁</a>", pageNumber - 1);
        //顯示頁碼
        int pageShowStart = pageNumber - pageShow / 2;
        if (pageShowStart < 1) pageShowStart = 1;
        int pageShowEnd = pageShowStart + pageShow - 1;
        if (pageShowEnd > maxPageNumber) pageShowEnd = maxPageNumber;
        for (int i = pageShowStart; i <= pageShowEnd; i++)
        {
            if (i == pageNumber)
            {
                returnString += String.Format("<a href=\"#\" style=\"color:red;cursor:default;\" onclick=\"return false;\">{0}</a>{1}", i, (i == pageShowEnd) ? "" : " ");
            }
            else
            {
                returnString += String.Format("<a href=\"#\" onclick=\"_goPage1({0});return false;\">{0}</a>{1}", i, (i == pageShowEnd) ? "" : " ");
            }
        }
        //下一頁
        if (pageNumber < maxPageNumber) returnString += String.Format("<a href=\"#\" style=\"margin-right:0px;\" onclick=\"_goPage1({0});return false;\">下一頁</a>", pageNumber + 1);
        //最後一頁
        if (pageNumber < maxPageNumber) returnString += String.Format("<a href=\"#\" style=\"margin-left:0px;\" onclick=\"_goPage1({0});return false;\"><i class=\"fa fa-angle-double-right\" aria-hidden=\"true\"></i></a>", maxPageNumber);
        if (rowCount != 0) returnString += string.Format("共<span style='color:red'>{0}</span>筆", rowCount);
        returnString += "</div>";
        return returnString;
    }
    public static String showPageNumber2(int rowCount, int pageNumber, int pageRecord = 10, int pageShow = 10)
    {
        String returnString = "";
        if ((rowCount <= 0) || (pageNumber <= 0) || (pageRecord <= 0) || (pageShow <= 0)) return "";
        int maxPageNumber = (rowCount - 1) / pageRecord + 1;
        if (maxPageNumber <= 1) return "";
        returnString += "<div class=\"center pages both w10\">";

        //第一頁
        if (pageNumber > 1) returnString += String.Format("<a href=\"#\" style=\"margin-right:0px;\" onclick=\"_goPage2({0});return false;\"><i class=\"fa fa-angle-double-left\" aria-hidden=\"true\"></i></a>", 1);
        //上一頁
        if (pageNumber > 1) returnString += String.Format("<a href=\"#\" style=\"margin-left:0px;\" onclick=\"_goPage2({0});return false;\">上一頁</a>", pageNumber - 1);
        //顯示頁碼
        int pageShowStart = pageNumber - pageShow / 2;
        if (pageShowStart < 1) pageShowStart = 1;
        int pageShowEnd = pageShowStart + pageShow - 1;
        if (pageShowEnd > maxPageNumber) pageShowEnd = maxPageNumber;
        for (int i = pageShowStart; i <= pageShowEnd; i++)
        {
            if (i == pageNumber)
            {
                returnString += String.Format("<a href=\"#\" style=\"color:red;cursor:default;\" onclick=\"return false;\">{0}</a>{1}", i, (i == pageShowEnd) ? "" : " ");
            }
            else
            {
                returnString += String.Format("<a href=\"#\" onclick=\"_goPage2({0});return false;\">{0}</a>{1}", i, (i == pageShowEnd) ? "" : " ");
            }
        }
        //下一頁
        if (pageNumber < maxPageNumber) returnString += String.Format("<a href=\"#\" style=\"margin-right:0px;\" onclick=\"_goPage2({0});return false;\">下一頁</a>", pageNumber + 1);
        //最後一頁
        if (pageNumber < maxPageNumber) returnString += String.Format("<a href=\"#\" style=\"margin-left:0px;\" onclick=\"_goPage2({0});return false;\"><i class=\"fa fa-angle-double-right\" aria-hidden=\"true\"></i></a>", maxPageNumber);
        if (rowCount != 0) returnString += string.Format("共<span style='color:red'>{0}</span>筆", rowCount);
        returnString += "</div>";
        return returnString;
    }
    public static void showMessage(System.Web.UI.Page pPage, String pKey, String pMessage)
    {
        pPage.ClientScript.RegisterStartupScript(pPage.GetType(), pKey, String.Format("alert('{0}');", pMessage), true);
    }

    public static void InsertToDO(string TODOTitle,string ToDOText,string getPersonSNO,string PostPersonSNO,bool State)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("TODOTitle", TODOTitle);
        aDict.Add("ToDOText", ToDOText);
        aDict.Add("getPersonSNO", getPersonSNO);
        aDict.Add("postPersonSNO", PostPersonSNO);
        aDict.Add("State", State);
        string sql = @"INSERT INTO [dbo].[TODO]
           ([TODOTITLE]
           ,[TODOTEXT]
           ,[getPersonSNO]
           ,[postPersonSNO]
           ,[CreateDate]
           ,[STATE])
     VALUES
           (@TODOTITLE
           ,@TODOTEXT
           ,@getPersonSNO
           ,@postPersonSNO
           ,getdate()
           ,@STATE)";
        objDH.executeNonQuery(sql, aDict);
    }
    public static DataTable getCoursePlanningClassAdminRoleSNO(string PClassSNO,string RoleSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = @"With getRoleAdmin as (
                    Select R.RoleSNO from Role R
                    where R.IsAdmin=1 and R.RoleGroup=(Select QCPR.RoleSNO from QS_CoursePlanningClass QCPC
                    Left Join QS_CoursePlanningRole QCPR On QCPR.PClassSNO=QCPC.PClassSNO
                    where QCPC.PClassSNO=@PClassSNO and QCPR.RoleSNO=@RoleSNO )
                    )
                    Select PersonSNO from Person P
                    Join getRoleAdmin gRA On gRA.RoleSNO=P.RoleSNO";
        aDict.Add("PClassSNO", PClassSNO);
        aDict.Add("RoleSNO", RoleSNO);
        DataTable ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT;
        }
        else
        {
            return null;
        }
    }
    public static bool CheckToDO(string PersonSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonSNO", PersonSNO);
        string sql = @"SELECT 1
            FROM [TODO]
            where getPersonSNO=@PersonSNO and STATE=0";
        DataTable ObjDT = objDH.queryData(sql, aDict);
        if(ObjDT.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    public static void deleteNewhandFile(string path, string id)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("id", id);
        DataTable objDT = objDH.queryData("Select NHPath From NewHand Where NHSNO=@id", aDict);
        string fPath = objDT.Rows[0]["NHPath"].ToString();
        if (File.Exists(path)) File.Delete(path + "/" + fPath);
    }
    public static string ReturnRoleName(string RoleGroup)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("RoleGroup", RoleGroup);
        DataTable ObjDT = objDH.queryData("Select RoleName from Role where RoleGroup=@RoleGroup", aDict);
        if (ObjDT.Rows.Count > 0)
        {
            string RoleName = ObjDT.Rows[0]["RoleName"].ToString();
            return RoleName;
        }
        else
        {
            return null;
        }

    }
    public static void deleteDownloadDFolder(string path, string id)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("id", id);
        DataTable objDT = objDH.queryData("Select DLOADURL From Download Where DLOADSNO=@id", aDict);
        string fPath = objDT.Rows[0]["DLOADURL"].ToString();
        //刪除前一個檔案
        if (Directory.Exists(path + "/" + fPath))
            Directory.Delete(path + "/" + fPath, true);
    }
    public static string RoleCancal(string EventSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> bDict = new Dictionary<string, object>();
        bDict.Add("EventSNO", EventSNO);
        string ChkEventRole = "select * from Event where EventSNO=@EventSNO";
        DataTable ObjChk = ObjDH.queryData(ChkEventRole, bDict);
        string RoleCancal = ObjChk.Rows[0]["CancelRole"].ToString();
        return RoleCancal;
    }

    public static void ChkNotDone(string PName, string PersonSNO, string PClassSNO, Label lb_NotDone, Label lb_Table, string EventSNO, string uPersonSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        //Compulsory 欄位為必修課程判斷
        string ChkSQL = @"with GetAll as(SELECT 
                              ROW_NUMBER() OVER(ORDER BY  QCPC.[PClassSNO]) AS R,
                              QCPC.[PClassSNO]
                              ,[PlanName]
                        	  ,QC.CourseName
                        	  ,QC.CourseSNO
                              ,[CStartYear]
                              ,[CEndYear]
                              ,QCPC.[IsEnable]
                              ,[CTypeSNO]
                              ,[TargetIntegral]	 
                                ,SignLimit
                              ,QC.Compulsory 
                          FROM [QS_CoursePlanningClass] QCPC
                          Left Join QS_Course QC ON QC.PClassSNO=QCPC.PClassSNO
                          where QC.CType=1
                          ),
                          getPerson as(
                          select * from QS_Integral where PersonSNO=@PersonSNO
                          )
                          select GA.*,ISNO from GetAll GA
                          Left outer Join getPerson gP ON gP.CourseSNO= GA.CourseSNO
                          where GA.PClassSNO=@PClassSNO";
        aDict.Add("PersonSNO", PersonSNO);
        aDict.Add("PClassSNO", PClassSNO);
        DataTable ObjDT = ObjDH.queryData(ChkSQL, aDict);
        string NotDone = "";
        int CompulsoryCount = 0;
        int SignLimit = Convert.ToInt16(ObjDT.Rows[0]["SignLimit"]);
        int TargetIntegral = Convert.ToInt16(ObjDT.Rows[0]["TargetIntegral"]);
        int TotalCount = ObjDT.Rows.Count;
        int alerdydone = 0;
        string TableS = "<Table>";
        TableS += "<tr><th>課程名稱</th><th>課程狀況</th><th>修課狀況</th></tr>";
        for (int i = 0; i < ObjDT.Rows.Count; i++)
        {
            TableS += "<tr>";
            string ISNO = ObjDT.Rows[i]["ISNO"].ToString();
            string Compulsory = ObjDT.Rows[i]["Compulsory"].ToString();
            TableS += "<td>" + ObjDT.Rows[i]["CourseName"].ToString() + "</td>";
            if (Compulsory == "True")
            {
                TableS += "<td>必修</td>";
            }
            else
            {
                TableS += "<td>選修</td>";
            }
            if (ISNO == "")
            {
                TableS += "<td>未修</td></tr>";
            }
            else
            {
                TableS += "<td>已修</td></tr>";
                alerdydone += 1;
            }
            if (Compulsory == "True" && ISNO == "")
            {
                CompulsoryCount += 1;
            }
        }
        SignLimit = SignLimit - CompulsoryCount - alerdydone;
        if (SignLimit <= 0)
            SignLimit = 0;
        if (CompulsoryCount == 0 && SignLimit == 0)
        {
            Dictionary<string, object> vDict = new Dictionary<string, object>();
            vDict.Add("EventSNO", EventSNO);
            vDict.Add("PersonSNO", PersonSNO);
            vDict.Add("Audit", 0);
            vDict.Add("NoticeType", 2);
            vDict.Add("Notice", 0);
            vDict.Add("CreateUserID", uPersonSNO);
            DataHelper objDH = new DataHelper();
            string SQL = @"if Exists (select 1 from EventD  WHERE EventSNO = @EventSNO and PersonSNO=@PersonSNO)
	                        select 1 
	                        else
                            	Insert Into EventD ([EventSNO],[PersonSNO],[Audit],[NoticeType],[Notice],[CreateUserID])
                                        Values( @EventSNO,@PersonSNO,@Audit,@NoticeType,@Notice,@CreateUserID)
";
            objDH.executeNonQuery(SQL, vDict);
            HttpContext.Current.Response.Write("<script>alert('" + PName + " 報名成功')</script>");
            HttpContext.Current.Response.Write("<script>window.opener.location.reload()</script>");

        }
        else
        {
            NotDone += "★" + PName + "未完成報名，您尚需完成線上課程必修" + CompulsoryCount + "堂、選修" + TargetIntegral + "堂";
            lb_NotDone.Text = NotDone;
            lb_Table.Text = TableS + "</Table>";
        }
    }

    public static string CheckMstutas(string PersonSNO)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        aDict.Add("PersonSNO", PersonSNO);
        string SQL = @"Select * from Person P where P.PersonSNO=@PersonSNO";
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            string Mstutas = ObjDT.Rows[0]["MStatusSNO"].ToString();
            if (Mstutas == "0")
            {
                return "0";
            }
            else if (Mstutas == "1")
            {
                return "1";
            }
            else
            {
                return "2";
            }
        }
        else
        {
            return "";
        }
        
    }

    public static string CheckEventType(string EventSNO)
    {
        //1 為證書課程
        //2 為繼續教育
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        aDict.Add("EventSNO", EventSNO);
        string SQL = @"Select * from Event E where E.EventSNO=@EventSNO";
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            string EventType = ObjDT.Rows[0]["Class3"].ToString();
            if (EventType == "1")
            {
                return "1";
            }
            else
            {
                return "2";
            }
        }
        return "";
    }

    public static string CheckCoreOrProfession(string EventSNO)
    {
        //1 為核心課程
        //2 為專門課程
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        aDict.Add("EventSNO", EventSNO);
        string SQL = @"Select * from Event E where E.EventSNO=@EventSNO";
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            string EventType = ObjDT.Rows[0]["Class4"].ToString();
            if (EventType == "1")
            {
                return "1";
            }
            else
            {
                return "2";
            }
        }
        return "";
    }

    public static string[] CertificateRoleSNO(string PersonID)
    {

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        aDict.Add("PersonID", PersonID);
        string SQL = @" Select P.RoleSNO,QC.* from QS_Certificate QC
			Left Join Person P ON P.PersonID=QC.PersonID			
			where QC.PersonID=@PersonID";
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        string RoleSNO = "";


        for (int i = 0; i < ObjDT.Rows.Count; i++)
        {
            RoleSNO += ObjDT.Rows[i]["RoleSNO"].ToString() + ",";
        }
        string[] RoleSNOArray = RoleSNO.Split(',');

        return RoleSNOArray;

    }

    public static void ChangeNotic(string EventSNO, string PersonSNO)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        aDict.Add("EventSNO", EventSNO);
        aDict.Add("PersonSNO", PersonSNO);
        string SQL = @"
        Update EventD set Notice=1 where EventSNO=@EventSNO and PersonSNO=@PersonSNO
";
        objDH.executeNonQuery(SQL, aDict);
    }

    public static bool CheckSession(UserInfo userInfo)
    {
        if (userInfo.UserPhone == "" || userInfo.UserPhone == null || userInfo.UserPhone.Length != 10)
        {
            return false;
        }
        if (userInfo.Address == "" || userInfo.Address == null)
        {
            return false;
        }
        if (userInfo.Birthday == "" || userInfo.Birthday == null || userInfo.Birthday == "1900/1/1 上午 12:00:00")
        {
            return false;
        }
        if (userInfo.UserMail == "" || userInfo.UserMail == null || CheckEmailFormat(userInfo.UserMail) == false)
        {
            return false;
        }
        if (userInfo.TsType == "" && userInfo.TsTypeNote == "" && userInfo.RoleSNO == "10")
        {
            return false;
        }
        if (userInfo.TsType == "53" && userInfo.TsTypeNote == "" && userInfo.RoleSNO == "10")
        {
            return false;
        }
        if (userInfo.TsType == "53" && userInfo.TsTypeNote == null && userInfo.RoleSNO == "10")
        {
            return false;
        }
        return true;
    }

    public static bool CheckCoreClass(string PClassSNO)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string SQL = @"
       with GetAll as(SELECT 
                              ROW_NUMBER() OVER(ORDER BY  QCPC.[PClassSNO]) AS R,
                              QCPC.[PClassSNO]
                              ,[PlanName]
                        	  ,QC.CourseName
                        	  ,QC.CourseSNO
                              ,[CStartYear]
                              ,[CEndYear]
                              ,QCPC.[IsEnable]
                              ,[CTypeSNO]
                              ,[TargetIntegral]	  
                              ,SignLimit
                              ,QC.Compulsory 
							  ,QC.CType
                              ,C1.MVal CtypeName
							  ,QC.Class1
							  ,C2.MVal Class1Name
                          FROM [QS_CoursePlanningClass] QCPC
                          Left Join QS_Course QC ON QC.PClassSNO=QCPC.PClassSNO
						  Left Join Config C1 On C1.PVal=QC.CType and C1.PGroup='CourseCType'
						  Left Join Config C2 On C2.PVal=QC.Class1 and C2.PGroup='CourseClass1'
                          
                          )
                          select * from GetAll GA
          
                          where GA.PClassSNO=@PClassSNO 
";
        aDict.Add("PClassSNO", PClassSNO);
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        int CCount = 0;
        for (int i = 0; i < ObjDT.Rows.Count; i++)
        {
            string CType = ObjDT.Rows[i]["CType"].ToString();
            if (CType == "1" || CType == "2")
            {
                CCount += 1;
            }
        }
        if (CCount == 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public static void setddl_CertificateType(DropDownList dd2, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select CTypeSNO,CtypeString from QS_CertificateType where CtypeString <> '' and CtypeString is not null", null);
        dd2.DataSource = objDT;
        dd2.DataBind();
        if (DefaultString != null)
        {
            dd2.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }

    public static void setddl_CertificateTypeForAutoSend(DropDownList dd2, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select CTypeSNO,[CTypeName]+'('+CtypeString+')'CTypeName  from QS_CertificateType where CtypeString <> '' and CtypeString is not null and CtypeSNO not in(56,57)", null);
        dd2.DataSource = objDT;
        dd2.DataBind();
        if (DefaultString != null)
        {
            dd2.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }

    public static bool CheckEmailFormat(string email)
    {
        //Email檢查格式
        Regex EmailExpression = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.Compiled | RegexOptions.Singleline);
        try
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            else
            {
                return EmailExpression.IsMatch(email);
            }
        }
        catch (Exception ex)
        {
            //log.Error(ex.Message);
            return false;
        }
    }


    public static bool CheckCourseSNO(string CourseSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("CourseSNO", CourseSNO);
        string CCourseSNO = "Select * from QS_Course where CourseSNO=@CourseSNO";
        DataTable ObjDT = ObjDH.queryData(CCourseSNO, adict);
        if (ObjDT.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public static string ConvertPersonIDToPersonSNO(string PersonID)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("PersonID", PersonID);
        string sql = @"Select PersonSNO,PersonID from Person where PersonID=@PersonID";
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            string PersonSNO = ObjDT.Rows[0]["PersonSNO"].ToString();
            return PersonSNO;
        }
        else
        {
            return "False";
        }

    }

    public static string ConvertPersonSNOToPersonID(string PersonSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("PersonSNO", PersonSNO);
        string sql = @"Select PersonSNO,PersonID from Person where PersonSNO=@PersonSNO";
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            string PersonID = ObjDT.Rows[0]["PersonID"].ToString();
            return PersonID;
        }
        else
        {
            return "False";
        }

    }

    public static bool Checkpair(string CourseSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("PairCourseSNO", CourseSNO);
        string sql = "Select * from QS_Course where PairCourseSNO=@PairCourseSNO";
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public static string EventCtypeSNO(string EventSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("EventSNO", EventSNO);
        string SQL = @"Select distinct QCPC.CTypeSNO from  Event E
        Left Join QS_ECoursePlanningClass QEPC On QEPC.EPClassSNO=E.EPClassSNO
        Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO=QEPC.PClassSNO
        where E.EventSNO=49";
        DataTable ObjDT = ObjDH.queryData(SQL, adict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT.Rows[0]["CtypeSNO"].ToString();
        }
        else
        {
            return "";
        }
    }

    public static bool ReturnCtypeSNO(string PClassSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("PClassSNO", PClassSNO);
        string SQL = @"Select CtypeSNO from QS_CoursePlanningClass QCPC where QCPC.PClassSNO=@PClassSNO";
        DataTable ObjDT = ObjDH.queryData(SQL, adict);
        string CtypeSNO = ObjDT.Rows[0]["CtypeSNO"].ToString();
        if(CtypeSNO=="10" || CtypeSNO == "11")
        {
            return true;
        }
        else
        {
            return false;
        }   
    }

    public static bool CheckPersonJuniorSenior(string CtypeSNO,string PersonID)
    {
        if (CtypeSNO == "")
        {
            return false;
        }
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("CtypeSNO", CtypeSNO);
        adict.Add("PersonID", PersonID);
        string SQL = @"Select 1 from QS_Certificate QC where QC.CtypeSNO In ("+ CtypeSNO+") and PersonID=@PersonID and getdate() < QC.CertEndDate";
        DataTable ObjDT = ObjDH.queryData(SQL, adict);
        if(ObjDT.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void setCertificateUnit(DropDownList ddl, string RoleGroups, string DefaultString = null)
    {

        DataHelper objDH = new DataHelper();
        string SQL = "SELECT QCUR.*,QCU.CUnitName  FROM [QS_CertificateUnitRole] QCUR  left Join QS_CertificateUnit QCU On QCU.CUnitSNO = QCUR.CUnitPairSNO ";
        switch (RoleGroups)
        {
            case "10":
                SQL += "where QCUR.[CUnitSNO] = 30";
                break;
            case "11":
                SQL += "where QCUR.[CUnitSNO] = 31";
                break;
            case "12":
                SQL += "where QCUR.[CUnitSNO] = 32";
                break;
            case "13":
                SQL += "where QCUR.[CUnitSNO] = 33";
                break;
            default:
                break;
        }


        DataTable objDT = objDH.queryData(SQL, null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    public static bool CheckPeronMPOrganCode(string PersonID,string UserMPOrganCode)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = "Select OrganCode from PersonMP where PersonID=@PersonID";
        adict.Add("PersonID", PersonID);
        DataTable ObjDT = objDH.queryData(sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            string OrganCode = ObjDT.Rows[0]["OrganCode"].ToString();
            if (OrganCode.Equals(UserMPOrganCode))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public class MessageBox
    {
        private static Hashtable m_executingPages = new Hashtable();

        private MessageBox() { }
        /// <summary>
        /// MessageBox訊息窗
        /// </summary>
        /// <param name="sMessage">要顯示的訊息</param>
        public static void Show(string sMessage)
        {
            // If this is the first time a page has called this method then
            if (!m_executingPages.Contains(HttpContext.Current.Handler))
            {
                // Attempt to cast HttpHandler as a Page.
                Page executingPage = HttpContext.Current.Handler as Page;

                if (executingPage != null)
                {
                    // Create a Queue to hold one or more messages.
                    Queue messageQueue = new Queue();

                    // Add our message to the Queue
                    messageQueue.Enqueue(sMessage);

                    // Add our message queue to the hash table. Use our page reference
                    // (IHttpHandler) as the key.
                    m_executingPages.Add(HttpContext.Current.Handler, messageQueue);

                    // Wire up Unload event so that we can inject some JavaScript for the alerts.
                    executingPage.Unload += new EventHandler(ExecutingPage_Unload);
                }
            }
            else
            {
                // If were here then the method has allready been called from the executing Page.
                // We have allready created a message queue and stored a reference to it in our hastable. 
                Queue queue = (Queue)m_executingPages[HttpContext.Current.Handler];

                // Add our message to the Queue
                queue.Enqueue(sMessage);
            }
        }


        // Our page has finished rendering so lets output the JavaScript to produce the alert's
        private static void ExecutingPage_Unload(object sender, EventArgs e)
        {
            // Get our message queue from the hashtable
            Queue queue = (Queue)m_executingPages[HttpContext.Current.Handler];

            if (queue != null)
            {
                StringBuilder sb = new StringBuilder();

                // How many messages have been registered?
                int iMsgCount = queue.Count;

                // Use StringBuilder to build up our client slide JavaScript.
                sb.Append("<script language='javascript'>");

                // Loop round registered messages
                string sMsg;
                while (iMsgCount-- > 0)
                {
                    sMsg = (string)queue.Dequeue();
                    //sMsg = sMsg.Replace( "\n", "\\n" ); //這部分是我mark掉的
                    sMsg = sMsg.Replace("\"", "'");

                    //W3c建議要避開的危險字元
                    //&;`'\"|*?~<>^()[]{}$\n\r
                    sMsg = sMsg.Replace("\n", "_");
                    sMsg = sMsg.Replace("\r", "_");

                    sb.Append(@"alert( """ + sMsg + @""" );");
                }

                // Close our JS
                sb.Append(@"</script>");

                // Were done, so remove our page reference from the hashtable
                m_executingPages.Remove(HttpContext.Current.Handler);

                // Write the JavaScript to the end of the response stream.
                HttpContext.Current.Response.Write(sb.ToString());
            }
        }
    }

    #endregion
    public static string getEventSNOIdentity()
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string SQL = "SELECT IDENT_CURRENT ('Event') +1 AS Current_Identity";
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        return ObjDT.Rows[0]["Current_Identity"].ToString();
    }

    #region 適用人員RoleBind

    public static void insertRoleBind(CheckBoxList cb_Role, string MappingSNO, string TypeKey, string CreateUserID)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> bDict = new Dictionary<string, object>();
        bDict.Add("TypeKey", TypeKey);
        bDict.Add("CSNO", MappingSNO);
        //先清除RoleBind
        objDH.executeNonQuery(@"Delete From RoleBind Where TypeKey=@TypeKey And CSNO=@CSNO", bDict);

        //新增RoleBind
        for (int i = 0; i < cb_Role.Items.Count; i++)
        {
            if (cb_Role.Items[i].Selected == true)
            {
                bDict.Clear();
                bDict.Add("TypeKey", TypeKey);
                bDict.Add("CSNO", MappingSNO);
                bDict.Add("RoleSNO", cb_Role.Items[i].Value);
                bDict.Add("CreateDT", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                bDict.Add("CreateUserID", CreateUserID);
                objDH.executeNonQuery(@"Insert Into RoleBind(TypeKey,CSNO,RoleSNO,CreateDT,CreateUserID)
                Values(@TypeKey,@CSNO,@RoleSNO,@CreateDT,@CreateUserID)", bDict);
            }
        }
    }

    public static void setRoleBind(CheckBoxList cb_Role, string MappingSNO, string TypeKey)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> bDict = new Dictionary<string, object>();
        bDict.Add("TypeKey", TypeKey);
        bDict.Add("CSNO", MappingSNO);

        string sql = @"
            SELECT R.RoleSNO, R.RoleName, 
	            (Select 1 From RoleBind RB Where RB.RoleSNO=R.RoleSNO And TypeKey=@TypeKey and CSNO=@CSNO) Chk
            FROM Role R 
            WHERE R.IsAdmin=0 and R.DocGroup is not null
        ";
        DataTable objDT = objDH.queryData(sql, bDict);
        cb_Role.DataSource = objDT;
        cb_Role.DataBind();

        for (int i = 0; i < cb_Role.Items.Count; i++)
        {
            if (objDT.Rows[i]["Chk"].ToString() == "1")
            //if (cb_Role.Items[i].Value == objDT.Rows[i]["RoleSNO"].ToString())
            {
                cb_Role.Items[i].Selected = true;
            }
        }

    }

    public static string setSQL_RoleBindName(string TypeKey, string CSNO)
    {
        string sql = @"
            Stuff( 
            (
                select ',' + CAST(R.RoleName as nvarchar) 
                from RoleBind RB
		            join Role R on R.RoleSNO=RB.RoleSNO
		        where RB.CSNO=" + CSNO + @" and RB.TypeKey='" + TypeKey + @"' 
                FOR XML PATH('')
            )
            ,1,1,'') RoleBindName ";
        return sql;
    }

    public static DataTable getCertificate(string PersonID)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable ObjDT;
        string sql = @"Select QCT.CTypeName,QC.CertPublicDate,QC.CertStartDate,QC.CertEndDate from QS_Certificate  QC
                        Left Join QS_CertificateType QCT On QCT.CTypeSNO = QC.CTypeSNO
                        where PersonID=@PersonID";
        aDict.Add("PersonID", PersonID);
        ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT;
        }
        else
        {
            return null;
        }
    }

    public static DataTable getScore(string PersonSNO,string PClassSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable ObjDT;
        string sql = @"    with getalltypeCourse as(
                Select QC.PClassSNO,C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour)應取得 
                from QS_Course QC
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                where QC.PClassSNO=@PClassSNO and QC.IsEnable=1 
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType,QC.PClassSNO
                )
                , getIntegral as (
                Select  C1.MVal+'課程' 課程類別,QC.Class1,C2.MVal 授課方式,QC.CType,Sum(QC.CHour) 已取得
                from QS_Integral QI
                Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                Left Join Config C1 On C1.PVal=QC.Class1 and C1.PGroup='CourseClass1'
                Left Join Config C2 On C2.PVal=QC.CType and C2.PGroup='CourseCType'
                Left Join Person P On P.PersonSNO=QI.personSNO
                where QI.PersonSNO=@PersonSNO and QC.PClassSNO=@PClassSNO
                GROUP BY C1.MVal,C2.MVal,QC.Class1,QC.CType
                
                ),getCoursePair as (
				Select Class1,Ctype,SUM(QC.Chour)CoursepairCount  from 
				QS_Course QC
				where PairCourseSNO <> 0 and PClassSNO=@PClassSNO
				Group by Class1,Ctype
				),AnalyticsPair as (
                   Select QC.Class1,QC.Ctype,QC.PairCourseSNO CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where PersonSNO=@PersonSNO and QC.PClassSNO=@PClassSNO
                    Group by QC.Class1,QC.Ctype,QC.PairCourseSNO
                    INTERSECT 
                    Select QC.Class1,QC.Ctype,QI.CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    where PersonSNO=@PersonSNO and QC.PClassSNO=@PClassSNO
                    Group by QC.Class1,QC.Ctype,QI.CourseSNO)
               
                ,getPairCourseSNO As(				  
					Select Class1,Ctype,SUM(Pair) pair from AnalyticsPair 
                    Group by Class1,Ctype
				  ),getalreadyI as(
                Select gac.PClassSNO,gac.課程類別,gac.Class1,gac.授課方式,gac.CType,gac.應取得-isnull(GCP.CoursepairCount,0) 應取得 ,isnull(gI.已取得,0)-isnull(GPC.Pair,0) 已取得 from getalltypeCourse gac
                Left Join getIntegral gI On gI.Class1=gac.Class1 and gI.CType=gac.CType
				 Left Join getPairCourseSNO GPC On GPC.Class1=gac.Class1 and GPC.CType=gac.CType
				 Left Join getCoursePair GCP On GCP.Class1=gac.Class1 and GCP.CType=gac.CType
                )
				
                Select *, case when gaI.應取得-gaI.已取得 < 0 then 0　Else gaI.應取得-gaI.已取得  END 未取得 from getalreadyI gaI
                Order by gaI.課程類別,gaI.授課方式 DESC";
        aDict.Add("PersonSNO", PersonSNO);
        aDict.Add("PClassSNO", PClassSNO);
        ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT;
        }
        else
        {
            return null;
        }
    }

    public static DataTable getEIntegral(string PersonID, string EPClassSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable ObjDT;
        string sql = @" 
                             With getAllEcourse as (
                 Select EPClassSNO,PlanName,TotalIntegral,Compulsory_Communication
                 ,Compulsory_Entity,Compulsory_Online,Compulsory_Practical
                  from QS_ECoursePlanningClass
                
                 ),
                 getEintergalO as (
                 Select PersonID,EPClassSNO,sum(Integral)Integral from QS_EIntegral
                 where Ctype=1 and PersonID=@PersonID
                 Group by PersonID,EPClassSNO
                 ),
                   getEintergalE as (
                  Select PersonID,EPClassSNO,sum(Integral)Integral from QS_EIntegral
                 where Ctype=2 and PersonID=@PersonID
                 Group by PersonID,EPClassSNO
                 )
                 ,
                   getEintergalP as (
                  Select PersonID,EPClassSNO,sum(Integral)Integral from QS_EIntegral
                 where Ctype=3 and PersonID=@PersonID
                 Group by PersonID,EPClassSNO
                 )
                   ,
                   getEintergalC as (
                  Select PersonID,EPClassSNO,sum(Integral)Integral from QS_EIntegral
                 where Ctype=4 and PersonID=@PersonID
                 Group by PersonID,EPClassSNO
                 )
                 Select PlanName,gAE.EPClassSNO,
                 isnull(gEC.Integral,0) 通訊 ,
                 isnull(gEE.Integral,0) 實體 ,
                 isnull(gEO.Integral,0) 線上 ,
                 isnull(gEP.Integral,0) 實習
                 from getAllEcourse gAE
                 Left Join getEintergalO gEO On gEO.EPClassSNO=gAE.EPClassSNO
                 Left Join getEintergalE gEE On gEE.EPClassSNO=gAE.EPClassSNO
                 Left Join getEintergalP gEP On gEP.EPClassSNO=gAE.EPClassSNO
                 Left Join getEintergalC gEC On gEC.EPClassSNO=gAE.EPClassSNO
                 where gAE.EPClassSNO=@EPClassSNO

";
        aDict.Add("PersonID", PersonID);
        aDict.Add("EPClassSNO", EPClassSNO);
        ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT;
        }
        else
        {
            return null;
        }
    }

    public static DataTable getEIntegralDetail(string PersonID, string EPClassSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable ObjDT;
        string sql = @" 
                               Select *,C.MVal from QS_EIntegral QE
                                Left Join Config C On C.PVal=QE.CType and C.PGroup='CourseCType'
                            where personID=@PersonID and EPClassSNO=@EPClassSNO

";
        aDict.Add("PersonID", PersonID);
        aDict.Add("EPClassSNO", EPClassSNO);
        ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT;
        }
        else
        {
            return null;
        }
    }

    public static DataTable getScoreDetail(string PersonSNO,string PClassSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable ObjDT;
        string sql = @"with getDetail as(Select C.MVal+'課程'　'課程類別',QC.CourseName,C1.MVal '授課方式',QC.CHour '積分',convert(varchar,  QI.CreateDT, 111)　'上傳時間',case when C2.MVal  is null then '線上' else C2.MVal end '取得方式',case when QL.Unit  is null then '-' else QL.Unit end '開課單位',case when QL.ExamDate  is null then  convert(varchar, QI.CreateDT, 111) else convert(varchar, QL.ExamDate, 111) end '上課日期',LR.LearnSNO　 from QS_Integral QI
                    left join person p on QI.PersonSNO=p.PersonSNO
					Left Join QS_LearningUpload QL on QI.CourseSNO=QL.CourseSNO and QI.PersonSNO=QL.PersonSNO
                    Left Join QS_Course QC ON QC.CourseSNO=QI.CourseSNO
					left join QS_LearningRecord LR on p.PersonID=LR.PersonID and QC.ELSCode=LR.ELSCode
                    Left Join Config C On QC.Class1=C.PVal and C.PGroup='CourseClass1'
                    Left Join Config C1 On QC.CType=C1.PVal and C1.PGroup='CourseCType'
					Left Join Config C2 On QL.Ctype=C2.PVal and C2.PGroup='CourseCType'
                    where QI.PersonSNO=@PersonSNO and QC.PClassSNO=@PClassSNO)
					select distinct getDetail.課程類別,getDetail.CourseName,getDetail.授課方式,getDetail.積分,case when getDetail.上傳時間 is null then '-' else getDetail.上傳時間 end as '上傳時間' ,getDetail.取得方式,getDetail.開課單位,case when getDetail.上課日期 is null then '-' else getDetail.上課日期 end as '上課日期',case when getDetail.LearnSNO is null and getDetail.上傳時間 is null  then '舊換新' else '-' end as '備註' from getDetail";
        aDict.Add("PersonSNO", PersonSNO);
        aDict.Add("PClassSNO", PClassSNO);
        ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT;
        }
        else
        {
            return null;
        }
    }
    public static DataTable getExScoreDetail(string PersonID, string PClassSNO, string EPClassSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable ObjDT;
        string sql = @" Select QE.CourseName,QE.Integral,case when QE.CDate<'2022-11-01' then '-' else  convert(varchar, QE.CreateDT, 111) end as CreateDT,QELU.Unit,convert(varchar, QE.CDate, 111)  as CDate,C.MVal 授課方式,case when QE.CDate<'2022-11-01' then '舊換新' else '-' end as '備註' from QS_EIntegral QE
  left join QS_ELearningUpload QELU
  on QE.personID=QELU.personID and QE.EISNO=QELU.EISNO
left join config C on C.PGroup='CourseCType' and QELU.Ctype = C.PVal
  where QE.personID=@PersonID and QE.EPClassSNO=@EPClassSNO
                     and  QE.Ctype In(0,1,2,3,4)      ";

        aDict.Add("PersonID", PersonID);

        aDict.Add("EPclassSNO", EPClassSNO);
        ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT;
        }
        else
        {
            return null;
        }
    }
    public static DataTable getExOnlineScoreDetail(string PersonID, string PersonSNO, string PClassSNO, string EPClassSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable ObjDT;
        string sql = @"   with getX as(
                    SELECT CPC.PClassSNO,CPC.PlanName,CPC.CStartYear,CPC.CEndYear,A.CourseSNO,A.CourseName,A.CHour
					,B.MVal + '課程' Class1 , C.MVal Class2 , A.UnitName , D.MVal Ctype,A.Class1 Class ,A.CType Type
                      FROM [New_QSMS].[dbo].[QS_ECoursePlanningClass] CPC
                      Left Join QS_Course A ON A.PClassSNO=CPC.PClassSNO
					LEFT  JOIN Config B ON B.PGroup ='CourseClass1' AND A.Class1 = B.PVal
					LEFT  JOIN Config C ON C.PGroup ='CourseClass2' AND A.Class2 = C.PVal
                    LEFT  JOIN Config D ON D.PGroup ='CourseCType' AND A.CType = D.PVal
                      
					  
                      ),
                      getY as(
                      select 1 O,I.CourseSNO,i.CreateDT,LR.LearnSNO from QS_Integral I
					  left join person p on I.PersonSNO=p.PersonSNO
					Left Join QS_LearningUpload QL on I.CourseSNO=QL.CourseSNO and I.PersonSNO=QL.PersonSNO
                    Left Join QS_Course QC ON QC.CourseSNO=I.CourseSNO
					left join QS_LearningRecord LR on p.PersonID=LR.PersonID and QC.ELSCode=LR.ELSCode
                      where I.PersonSNO=@PersonSNO
                      )
                      select distinct Class1,CourseName,Ctype,CHour,CStartYear,CEndYear,X.Class,X.Type,
					 case O when  1 then'已取得' ELSE '未取得' END  積分,case when Y.LearnSNO is null then '-' else convert(varchar, Y.CreateDT, 111) end as '上課日期' ,case when Y.LearnSNO is null then '舊換新' else '-' end as '備註' 
					 from getX X
                      FULL outer Join getY Y ON Y.CourseSNO=X.CourseSNO
					  Left JOIN QS_CoursePlanningRole CPR ON CPR.RClassSNO=X.PClassSNO
					  Left Join Role R ON R.RoleSNO=CPR.RoleSNO
                       where X.PClassSNO=@PClassSNO  and O=1 ";

        aDict.Add("PersonID", PersonID);
        aDict.Add("PersonSNO", PersonSNO);
        aDict.Add("@PClassSNO", PClassSNO);
        aDict.Add("@Class", 3);
        aDict.Add("@Type", 1);
        aDict.Add("EPclassSNO", EPClassSNO);
        ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT;
        }
        else
        {
            return null;
        }
    }
    public static string GetLastExamDate(string PersonID,string CtypeSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonID", PersonID);
        aDict.Add("CtypeSNO", CtypeSNO);
        DataTable ObjDT = objDH.queryData("Select Top(1) CertPublicDate from QS_Certificate  where PersonID=@PersonID And CtypeSNO=@CtypeSNO", aDict);
        if (ObjDT.Rows.Count > 0)
        {
            string LastExam = Convert.ToDateTime(ObjDT.Rows[0]["CertPublicDate"]).AddYears(-1911).ToString("yyy");
            return LastExam;
        }
        return "";
    }

    public static DataTable getCoursePlan(string PersonSNO, string PClassSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable ObjDT;
        string sql = @"with getsomething as(
                SELECT Distinct
                	I.PersonSNO
                	,QCPC.PlanName
                	,QCPC.CStartYear
                	,QCPC.CEndYear
                	,QCT.CTypeName
                	,QC.PClassSNO
                	,sum(CHour) PClassTotalHr
                  FROM [QS_Integral] I
                  Left Join Person P on P.PersonSNO=I.PersonSNO
                  Left Join QS_Course QC on QC.CourseSNO=I.CourseSNO
                  Left Join QS_CoursePlanningClass QCPC on QCPC.PClassSNO=QC.PClassSNO
                  Left Join QS_CertificateType QCT on QCT.CTypeSNO=QCPC.CTypeSNO
                    where 1=1 and I.isUsed <> 1 and I.PersonSNO=@PersonSNO 
                  Group by QCPC.PlanName,QCT.CTypeName,QCPC.CStartYear,QCPC.CEndYear,QC.PClassSNO,I.PersonSNO
                  ),AnalyticsPair as (
                    Select QI.PersonSNO,QC.PClassSNO,QC.Ctype,QC.PairCourseSNO CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    Left Join getsomething gs On gs.PersonSNO=QI.PersonSNO
                    where QI.PersonSNO=@PersonSNO and QC.PClassSNO=gs.PClassSNO
                    Group by QC.Ctype,QC.PairCourseSNO,QI.PersonSNO,QC.PClassSNO
                    INTERSECT 
                    Select QI.PersonSNO,QC.PClassSNO,QC.Ctype,QI.CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    Left Join getsomething gs On gs.PersonSNO=QI.PersonSNO
                    where QI.PersonSNO=@PersonSNO and QC.PClassSNO=gs.PClassSNO
                    Group by QC.Ctype,QI.CourseSNO,QI.PersonSNO,QC.PClassSNO)
                    ,getPairCourseSNO As(				  
					Select PersonSNO,PClassSNO,Ctype,SUM(Pair) pair from AnalyticsPair 
                    Group by Ctype,PersonSNO,PClassSNO
				  )
                  , getAllCourseHours As (
                				Select  c.PClassSNO, cpc.[TargetIntegral] sumHours
                				From QS_CoursePlanningClass cpc
                					Left JOIN QS_Course c on c.PClassSNO=cpc.PClassSNO
                				Group By c.PClassSNO, cpc.[TargetIntegral]
                			)
                
                ,sumAll as(
                  select getsomething.PersonSNO,PlanName,CStartYear,CEndYear,CTypeName,getsomething.PClassSNO,sum(PClassTotalHr)-isnull(GPC.Pair,0) PClassTotalHr ,sumHours from getsomething
                  left join getAllCourseHours gc on gc.PClassSNO=getsomething.PClassSNO
				  left Join getPairCourseSNO GPC On GPC.PersonSNO= getsomething.PersonSNO And GPC.PClassSNO=getsomething.PClassSNO
                  where getsomething.PersonSNO=@PersonSNO 
				  Group by getsomething.PersonSNO,PlanName,CStartYear,CEndYear,CTypeName,getsomething.PClassSNO,sumHours,GPC.Pair
				  )
				  Select * from sumAll where PClassSNO is not null and PClassSNO=@PClassSNO ";
        aDict.Add("PersonSNO", PersonSNO);
        aDict.Add("PClassSNO", PClassSNO);
        ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT;
        }
        else
        {
            return null;
        }
    }

    public static DataTable getECoursePlan(string PersonSNO, string PClassSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataTable ObjDT;
        string sql = @"  with getsomething as(
                SELECT 
                	I.PersonSNO
                	,QCPC.PlanName
                	,QCPC.CStartYear
					,QC.Class1
                	,QCPC.CEndYear
                	,QCT.CTypeName
                	,QC.PClassSNO
					,QCPC.EPClassSNO
                    ,QCPC.[ElearnLimit]
                	,Case when sum(CHour) >= QCPC.ElearnLimit then QCPC.ElearnLimit Else sum(CHour) End PClassTotalHr
                  FROM [New_QSMS].[dbo].[QS_Integral] I
                  Left Join Person P on P.PersonSNO=I.PersonSNO
                  Left Join QS_Course QC on QC.CourseSNO=I.CourseSNO
                  Left Join QS_ECoursePlanningClass QCPC on QCPC.PClassSNO=QC.PClassSNO
                  Left Join QS_CertificateType QCT on QCT.CTypeSNO=QCPC.CTypeSNO 
                    where 1=1 and P.PersonSNO=@PersonSNO and CTypeName <> '' and I.IsUsed <>1
                  Group by QCPC.PlanName,QCT.CTypeName,QCPC.CStartYear,QCPC.CEndYear,QC.PClassSNO,I.PersonSNO,QC.Class1,QCPC.EPClassSNO,QCPC.ElearnLimit
                  )
                  , getAllCourseHours As (
                				Select distinct  P.personID,QEPC.PlanName,QEPC.PClassSNO
								 ,QEPC.EPClassSNO,QEPC.TotalIntegral sumHours
								 ,QEPC.CStartYear,QEPC.CEndYear
								From QS_ECoursePlanningClass QEPC 
								Left Join QS_ECoursePlanningRole QEPR On QEPR.EPClassSNO=QEPC.EPClassSNO
								Left Join QS_CertificateType QCT On QCT.RoleSNO=QEPR.RoleSNO
								Left Join Role R on R.DocGroup=QEPR.RoleSNO
								Left Join Person P On P.RoleSNO=R.RoleSNO
								where P.PersonSNO=@PersonSNO  and QEPC.PClassSNO <>''
                				
                			)
				  , getEintegral as (
				  
					Select  P.personID,QCPC.PlanName,QCPC.PClassSNO ,QCPC.EPClassSNO,sum(QE.Integral) TotalHr
                				From QS_ECoursePlanningClass QCPC
                				Left Join QS_EIntegral QE On QE.EPClassSNO=QCPC.EPClassSNO
								Left Join Person P on P.PersonID=QE.PersonID
								where P.PersonSNO=@PersonSNO and QE.IsUsed <> 1
                				Group by QCPC.TotalIntegral ,QCPC.EPClassSNO,P.PersonID,QCPC.PlanName ,QCPC.PClassSNO
				  )
                
                  select getAllCourseHours.*,isnull(getEintegral.TotalHr,0)+isnull(getsomething.PClassTotalHr,0) PClassTotalHr
                    ,getsomething.CTypeName,getsomething.ElearnLimit
                    from getAllCourseHours
                  left join getsomething on getsomething.EPClassSNO=getAllCourseHours.EPClassSNO
				  left join getEintegral on getEintegral.EPClassSNO=getAllCourseHours.EPClassSNO
                where getAllCourseHours.PClassSNO=@PClassSNO
";
        aDict.Add("PersonSNO", PersonSNO);
        aDict.Add("PClassSNO", PClassSNO);
        ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT;
        }
        else
        {
            return null;
        }
    }
    //使用ELSCode取得CourseSNO
    public static string GetCourseSNOWithELSCode(string ELSCode)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        string SQL = @"Select c.CourseSNO, ces.ELSCode
			From QS_CourseELearningSection ces
				Left Join QS_Course c ON c.ELSCode=ces.ELSCode where c.ELSCode=@ELSCode";
        aDict.Add("ELSCode", ELSCode);
        DataTable ObjDT = ObjDH.queryData(SQL, aDict);
        string CourseSNO = ObjDT.Rows[0]["CourseSNO"].ToString();
        return CourseSNO;
    }

    //Call 預存程序
    public static void AutoAuditIntegral(string PersonID, string Note)
    {
        //EXEC dbo.SP_AutoAuditIntegral @PersonID=PersonID;
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonID", PersonID);
        aDict.Add("Note", Note);
        objDH.queryData("dbo.SP_AutoAuditIntegral @PersonID, @Note", aDict);
    }

    //寫入行事曆(協會證書課程)
    public static void InsertCanlendar(string StartDate,string EndDate,string Title,string Url,string CreateDT,string CreateUserID,string PClassSNO,string ERSNO,string EventSNO,bool IsEnable)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string sql = @"Insert Into Calendar([Title],[StartTime],[EndTime],[Url],[EventSNO],[IsEnable],[CreateDT],[CreatUserID]) Values(@Title,@StartTime,@EndTime,@Url,@EventSNO,@IsEnable,@CreateDT,@CreateUserID)";
        adict.Add("StartTime",Convert.ToDateTime(StartDate));
        adict.Add("EndTime",Convert.ToDateTime(EndDate));
        adict.Add("Title", Title);
        adict.Add("Url", "Event_AE.aspx?sno="+Url+"&psno="+PClassSNO);
        adict.Add("PClassSNO", PClassSNO);
        adict.Add("ERSNO", ERSNO);
        adict.Add("EventSNO", EventSNO);
        adict.Add("IsEnable", IsEnable);
        adict.Add("CreateDT", Convert.ToDateTime(CreateDT));
        adict.Add("CreateUserID", CreateUserID);
        ObjDH.executeNonQuery(sql, adict);
    }

    public static string ReturnCtypeName(string CtypeSNO)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        aDict.Add("CtypeSNO", CtypeSNO);
        DataTable objDT = objDH.queryData("Select CTypeName  FROM QS_CertificateType where CtypeSNO=@CtypeSNO", aDict);
       
        string CtypeName = objDT.Rows[0]["CTypeName"].ToString();
        return CtypeName;
    }

    public static string GetPersonMPOrganCode(string PersonID)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string Sql = "Select OrganCode from PersonMP where PersonID=@PersonID";
        aDict.Add("PersonID", PersonID);
        DataTable ObjDT = objDH.queryData(Sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT.Rows[0]["OrganCode"].ToString();
        }
        else
        {
            return "";
        }

    }

    public static string CtypeBinding(string CtypeSNO)
    {
        if(CtypeSNO=="1" || CtypeSNO == "53")
        {
            return "1,53";
        }
        else if(CtypeSNO == "2" || CtypeSNO == "54")
        {
            return "2,54";
        }
        else if (CtypeSNO == "3" || CtypeSNO == "55")
        {
            return "3,55";
        }
        else if (CtypeSNO == "4" || CtypeSNO == "5" || CtypeSNO == "51")
        {
            return "4,5,51";
        }
        else if (CtypeSNO == "6" || CtypeSNO == "7" || CtypeSNO == "52")
        {
            return "6,7,52";
        }
        else if (CtypeSNO == "8")
        {
            return "8";
        }
            return CtypeSNO;
    }
  
    public static string ReturnCoreCreateDT(string PersonSNO)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PersonSNO", PersonSNO);
        DataHelper objDH = new DataHelper();
        string sql = "Select TOP(1)Convert(varchar,CreateDT,111)CreateDT from QS_Integral where PersonSNO=@PersonSNO and IType=1 order by CreateDT DESC";
        DataTable objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            return objDT.Rows[0]["CreateDT"].ToString();
        }
        else
        {
            return "";
        }
    }
    #endregion


    #region 線上人數
    public static CountInfo GetHistoryTotal()
    {
        CountInfo countInfo = new CountInfo();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"SELECT HistoryCount , TodayCount , Today FROM UserCount", null);
        //decimal historyTotal = 0;
        if (objDT.Rows.Count > 0)
        {
            countInfo.HisCount = objDT.Rows[0]["HistoryCount"] == DBNull.Value ? 0 : Convert.ToDecimal(objDT.Rows[0]["HistoryCount"]);
            countInfo.TodayCount = objDT.Rows[0]["TodayCount"] == DBNull.Value ? 0 : Convert.ToDecimal(objDT.Rows[0]["TodayCount"]);
            countInfo.Today = objDT.Rows[0]["Today"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(objDT.Rows[0]["Today"]);
        }
        return countInfo;
    }
    public static void UpdateUserCount(CountInfo countInfo)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("HistoryCount", countInfo.HisCount);
        aDict.Add("TodayCount", countInfo.TodayCount);
        aDict.Add("Today", countInfo.Today);

        DataHelper objDH = new DataHelper();
        string sql = @" UPDATE UserCount SET HistoryCount = @HistoryCount
                                            , TodayCount = @TodayCount
                                            , Today = @Today";
        objDH.executeNonQuery(sql, aDict);

    }

    /// <summary>
    /// 設定 Online Cache , 並回傳是否線上新使用者
    /// </summary>
    /// 
    public static void setClassRoleName(System.Web.UI.WebControls.DropDownList dd2, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select RoleName,RoleGroup from Role where IsAdmin<>1 and DocGroup is not null", null);
        dd2.DataSource = objDT;
        dd2.DataBind();
        if (DefaultString != null)
        {
            dd2.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }

    public static void setTkType(System.Web.UI.WebControls.DropDownList dd2, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select Pval,Mval from Config C where C.Pgroup='TKType'", null);
        dd2.DataSource = objDT;
        dd2.DataBind();
        if (DefaultString != null)
        {
            dd2.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }
    public static bool CheckNewUserCache(string sessionId)
    {
        bool isNewUser = false;
        string cacheKey = "OnlineList";
        int checkMin = 20;

        var result = DataCacheHelper.Get<List<CacheUserInfo>>(cacheKey);
        if (result == null)
        {
            result = new List<CacheUserInfo> { new CacheUserInfo {
                     CreateTime = DateTime.Now,
                     SessionId = sessionId
            }};
            isNewUser = true;
        }
        else
        {
            var sessionIdResult = result.Where(o => o.SessionId == sessionId).FirstOrDefault();
            if (sessionIdResult != null)
            {
                //超過預設時間將重置, 並算為新使用者.
                if ((DateTime.Now - sessionIdResult.CreateTime).TotalMinutes > checkMin)
                {
                    sessionIdResult.CreateTime = DateTime.Now;
                    isNewUser = true;
                }
            }
            else
            {
                result.Add(new CacheUserInfo
                {
                    CreateTime = DateTime.Now,
                    SessionId = sessionId
                });
                isNewUser = true;
            }
        }

        DataCacheHelper.SetCache<List<CacheUserInfo>>(cacheKey, result, 60, true);
        result = DataCacheHelper.Get<List<CacheUserInfo>>(cacheKey);

        return isNewUser;
    }

    public static int GetOnlineCount(string sessionId)
    {
        //20分 未更新排除
        int checkMin = 5;
        string cacheKey = "OnlineList";
        var result = DataCacheHelper.Get<List<CacheUserInfo>>(cacheKey);
        if (result == null) return 1;

        result.RemoveAll(o => (DateTime.Now - o.CreateTime).TotalMinutes > checkMin);
        if (result.Where(o => o.SessionId == sessionId).FirstOrDefault() == null)
        {
            CheckNewUserCache(sessionId);
        }

        if (result.Count() == 0) return 1;

        return result.Count();

    }

    public static bool CheckUserLastLogin(string PersonSNO, string PAccount)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        Dictionary<string, object> wdict = new Dictionary<string, object>();
        adict.Add("PersonSNO", PersonSNO);
        string SQL = "Select * from LoginLog where PersonSNO=@PersonSNO And LoginStatus='0001' and LoginInfo='建立Session' order by LoginTime DESC ";
        DataTable ObjDT = ObjDH.queryData(SQL, adict);

        if (ObjDT.Rows.Count > 0)
        {           

            DateTime LastLoginTime = Convert.ToDateTime(ObjDT.Rows[0]["LoginTime"]).AddHours(2);//上一次成功登入
            DateTime time = DateTime.Now;
            if (time > LastLoginTime)
            {
                string WriteSQL = "Insert Into LoginLog([PAccount],[LoginTime],[LoginStatus],[LoginInfo],[PersonSNO]) values (@PAccount,@LoginTime,@LoginStatus,@LoginInfo,@PersonSNO)";
                wdict.Add("PAccount", PAccount);
                wdict.Add("LoginTime", DateTime.Now);
                wdict.Add("LoginStatus", "0001");
                wdict.Add("LoginInfo", "建立Session");
                wdict.Add("PersonSNO", PersonSNO);
                ObjDH.executeNonQuery(WriteSQL, wdict);
                return false;
            }
            else
            {
                return true;
            }

        }
        else
        {
            string WriteSQL = "Insert Into LoginLog([PAccount],[LoginTime],[LoginStatus],[LoginInfo],[PersonSNO]) values (@PAccount,@LoginTime,@LoginStatus,@LoginInfo,@PersonSNO)";
            wdict.Add("PAccount", PAccount);
            wdict.Add("LoginTime", DateTime.Now);
            wdict.Add("LoginStatus", "0001");
            wdict.Add("LoginInfo", "建立Session");
            wdict.Add("PersonSNO", PersonSNO);
            ObjDH.executeNonQuery(WriteSQL, wdict);
            return false;
            
        }
    }

    public static string CheckUserInfo(string EventSNO, string URoleSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("EventSNO", EventSNO);
        string RoleSNO = "";
        string result = "";
        string sql = @"
             SELECT  
                    Stuff( 
            (
               select ',' + CAST(RB.RoleSNO as nvarchar) 
                from RoleBind RB
		            join Role R on R.RoleSNO=RB.RoleSNO
		        where RB.CSNO=E.EventSNO and RB.TypeKey='Event_AE' 
                FOR XML PATH('')
            )
            ,1,1,'') RoleSNO
	        ,
            Stuff( 
            (
                select ',' + CAST(R.RoleName as nvarchar) 
                from RoleBind RB
		            join Role R on R.RoleSNO=RB.RoleSNO
		        where RB.CSNO=E.EventSNO and RB.TypeKey='Event_AE' 
                FOR XML PATH('')
            )
            ,1,1,'') RoleBindName 
            FROM [Event] E where 1=1 and E.EventSNO=@EventSNO
        ";

        DataTable objDT = ObjDH.queryData(sql, adict);
        for (int i = 0; i < objDT.Rows.Count; i++)
        {
            RoleSNO += objDT.Rows[i]["RoleSNO"].ToString() + ",";
        }
        RoleSNO = RoleSNO.Substring(0, RoleSNO.Length - 1);
        string[] RoleArray = RoleSNO.Split(',');
        int ChkRoleName = Array.IndexOf(RoleArray, URoleSNO);
        if (ChkRoleName == -1)
        {
            result = "0";
        }
        else
        {
            result = "1";
        }
        return result;
    }


    #endregion
}