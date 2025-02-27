using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_FormPaperAjax : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Controls.Clear();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string insertPaper(string formname, string isuse, string content)
    {


        string result = "";

        DataHelper odt = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PaperName", formname);
        aDict.Add("isUse", isuse);
        aDict.Add("PaperDetail", content);
        aDict.Add("CreateUserID", 2);
        string sql = @"
            INSERT INTO Paper (
            	PaperName,
            	isUse,
            CreateUserID,
            PaperDetail,
            isWrite
            )
            VALUES
	    (
		@PaperName ,@isUse ,@CreateUserID ,@PaperDetail,'0'
	    )
            ";

        odt.executeNonQuery(sql, aDict);
        aDict.Clear();

        return result;
    }


    [WebMethod]
    public static string updatePaper(string formname, string isuse, string personid, string content, string view)
    {
        string result = "";

        DataHelper odt = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PaperName", formname);
        aDict.Add("isUse", isuse);
        aDict.Add("PaperDetail", content);
        aDict.Add("ModifyUserID", 2);
        aDict.Add("PaperID", personid);
        aDict.Add("isWrite", view);

        string sql = @"
            UPDATE Paper
            SET PaperName =@PaperName,
             isUse =@isUse,
             PaperDetail =@PaperDetail,
             ModifyDT = SYSDATETIME(),
             ModifyUserID =@ModifyUserID,
            isWrite = @isWrite
            WHERE
            	PaperID =@PaperID
            ";

        odt.executeNonQuery(sql, aDict);
        aDict.Clear();

        return result;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string insertQuestion(string formname, string content, string paperID, string skin, string qcount)
    {
        int sort;
        String sql1 = @"
            SELECT Max(Sort) as Sort FROM Question Where PaperID=@PaperID
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        wDict.Add("PaperID", paperID);

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql1, wDict);

        if (objDT.Rows[0]["sort"].ToString() != "")
        {
            sort = Int32.Parse(objDT.Rows[0]["sort"].ToString());
            sort = sort + 1;
        }
        else
        {
            sort = 1;
        }
        string result = "";

        DataHelper odt = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("QuestionName", formname);
        aDict.Add("PaperID", paperID);
        aDict.Add("Sort", sort);
        aDict.Add("QuestionDetail", content);
        aDict.Add("CreateUserID", 2);
        aDict.Add("skin", skin);
        aDict.Add("qcount", qcount);

        string sql = @"
INSERT INTO Question (
	QuestionName,
PaperID,
CreateUserID,
QuestionDetail,
Sort,
skin,
qcount
)
VALUES
	(
		@QuestionName,@PaperID,@CreateUserID ,@QuestionDetail,@Sort,@skin,@qcount
	)
";

        odt.executeNonQuery(sql, aDict);
        aDict.Clear();

        return result;
    }


    [WebMethod]
    public static string updateQuestion(string formname, string personid, string content, string paperID, string skin, string qcount)
    {
        string result = "";

        DataHelper odt = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("QuestionName", formname);
        aDict.Add("PaperID", paperID);
        aDict.Add("QuestionDetail", content);
        aDict.Add("ModifyUserID", 2);
        aDict.Add("QuestionID", personid);
        aDict.Add("skin", skin);

        if (skin != "2")
        {
            aDict.Add("qcount", "");
        }
        else
        {
            aDict.Add("qcount", qcount);
        }


        string sql = @"
UPDATE Question
SET QuestionName =@QuestionName,
 QuestionDetail =@QuestionDetail,
 ModifyDT = SYSDATETIME (),
 ModifyUserID =@ModifyUserID,
 qcount = @qcount,
skin=@skin
WHERE
	PaperID =@PaperID
AND QuestionID = @QuestionID
";

        odt.executeNonQuery(sql, aDict);
        aDict.Clear();

        return result;
    }
    [WebMethod]
    public static bool CheckCardLogin(string PrsnID)
    {
        string PersonID= PrsnID.Replace(@"""","");
        DataHelper odt = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = "Select * from Person where PersonID=@PersonID";
        aDict.Add("PersonID", PersonID);
        DataTable ObjDT = odt.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return true;
        }
        return false;
    }

    [WebMethod]
    public static string updatesort(string sortGroup, string paperID)
    {
        string result = "";
        String[] sGroup = sortGroup.Split(',');

        for (int i = 1; i <= sGroup.Length; i++)
        {
            DataHelper odt = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("Sort", i);
            aDict.Add("PaperID", paperID);
            aDict.Add("QuestionID", sGroup[i - 1]);

            string sql = @"
UPDATE Question
SET Sort =@Sort
WHERE
	PaperID =@PaperID
AND QuestionID = @QuestionID
";

            odt.executeNonQuery(sql, aDict);
            aDict.Clear();
        }



        return result;
    }


    [WebMethod]
    public static string insertOption(string optionGroup, string QuestionID)
    {
        DataHelper objDH = new DataHelper();
        string result = "";
        String[] sGroup = optionGroup.Split(',');

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("QuestionID", QuestionID);

        objDH.executeNonQuery("Delete [Option] Where QuestionID=@QuestionID", dic);

        for (int i = 0; i <= sGroup.Length - 1; i++)
        {
            DataHelper odt = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();

            aDict.Add("QuestionID", QuestionID);
            aDict.Add("OptionName", sGroup[i]);
            aDict.Add("CreateUserID", 2);
            string sql = @"
            INSERT INTO [Option] (
            OptionName,
            QuestionID,
            CreateUserID
            )
            VALUES
	        (
	        	@OptionName,@QuestionID,@CreateUserID
	        )
            ";

            odt.executeNonQuery(sql, aDict);
            aDict.Clear();
        }



        return result;
    }

    [WebMethod]
    public static string UserAddAnswer(string totle, string PersonSno, string PaperID)
    {
        DataHelper objDH = new DataHelper();
        string result = "";
        String[] stotle = totle.Split(',');
        Dictionary<string, object> dic = new Dictionary<string, object>();

        dic.Add("PersonSno", PersonSno);
        dic.Add("PaperID", PaperID);
        string sql1 = @"
                INSERT INTO Exam (PersonSno,PaperID)
                VALUES(@PersonSno,@PaperID) 
                SELECT @@IDENTITY AS 'Identity'
";
        DataTable objDTPAPER = objDH.queryData(sql1, dic);
        for (int i = 0; i <= stotle.Length - 1; i++)
        {
            String[] PT = stotle[i].Split('/');
            DataHelper odt = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            string caseSwitch = PT[1];
            //skin =>>>> 0:問答題 1:單選題 2:多選題 3:簡單輸入題
            switch ("1")
            {
                case "0":
                    aDict.Add("OptionID", "");
                    aDict.Add("f_content", PT[1]);

                    break;
                case "1":
                    aDict.Add("OptionID", PT[1]);
                    aDict.Add("f_content", "");
                    break;
                case "2":
                    aDict.Add("OptionID", PT[1]);
                    aDict.Add("f_content", "");
                    break;
                case "3":
                    aDict.Add("OptionID", "");
                    aDict.Add("f_content", PT[1]);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            aDict.Add("QuestionID", PT[0]);
            aDict.Add("skin", "1");
            aDict.Add("ExamID", objDTPAPER.Rows[0]["Identity"].ToString());
            string sql = @"
            INSERT INTO Answer (OptionID,QuestionID,ExamID,skin,f_content)
            VALUES( @OptionID,@QuestionID,@ExamID,@skin,@f_content) ";
            odt.executeNonQuery(sql, aDict);
            aDict.Clear();

        }
        Dictionary<string, object> adic = new Dictionary<string, object>();
        string sqlUpdate = "Update Person set IsExam=1 where PersonSNO=@PersonSNO";
        adic.Add("PersonSNO", PersonSno);
        objDH.executeNonQuery(sqlUpdate, adic);
        return result;
    }


    [WebMethod]
    public static List<string> SearchOption(string Questionid)
    {
        DataHelper objDH = new DataHelper();


        string result = "";
        //String[] stotle = totle.Split(',');


        Dictionary<string, object> dic = new Dictionary<string, object>();

        dic.Add("QuestionID", Questionid);
        string sql1 = @"
SELECT OptionID,OptionName FROM [Option] WHERE QuestionID = @QuestionID
";
        DataTable odt = objDH.queryData(sql1, dic);
        List<string> d_list = new List<string>();
        if (odt.Rows.Count != 0)
        {

            for (int i = 0; i <= odt.Rows.Count - 1; i++)
            {

                d_list.Add(odt.Rows[i]["OptionID"].ToString());
                d_list.Add(odt.Rows[i]["OptionName"].ToString());
                //result = result + odt.Rows[i]["OptionID"].ToString() + "/" + odt.Rows[i]["OptionName"].ToString() + ",";
            }
            //result = result.Substring(0, result.Length - 1);


        }
        else
        {
            result = "null";
        }

        return d_list;
    }



    [WebMethod]
    public static string CanserBind(string systemID, string personSNO)
    {
        DataHelper objDH = new DataHelper();

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("SYSTEM_ID", systemID);
        dic.Add("PersonSNO", personSNO);

        string sql1 = @"
SELECT * FROM PersonD Where SYSTEM_ID=@SYSTEM_ID AND PersonSNO = @PersonSNO
";
        DataTable odt = objDH.queryData(sql1, dic);

        objDH.executeNonQuery("Delete PersonD Where SYSTEM_ID=@SYSTEM_ID AND PersonSNO = @PersonSNO", dic);

        if (odt.Rows.Count == 0)
        {
            return "NO";
        }
        else
        {
            return "OK";
        }

    }


    [WebMethod]
    public static string insertTODO(string person, string content, string system, string name)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper dhpp = new DataHelper();
        aDict.Add("system", system);
        aDict.Add("TODOTITLE", name);
        aDict.Add("TODOTEXT", content);
        aDict.Add("postPersonSNO", person);
        aDict.Add("STATE", 0);

        string InsertTODOSql = " Insert Into TODO(TODOTITLE,TODOTEXT,getPersonSNO,postPersonSNO,CreateDate,STATE)";
        InsertTODOSql += " SELECT @TODOTITLE AS TODOTITLE,@TODOTEXT AS TODOTEXT,Person.PersonSNO,@postPersonSNO AS postPersonSNO,GETDATE() as CreateDate,@STATE AS STATE";
        InsertTODOSql += " from Role inner join Person";
        InsertTODOSql += " on Role.RoleGroup=Person.RoleSNO";
        InsertTODOSql += " where role.IsAdmin=0 and Role.RoleSNO=@system";
        InsertTODOSql += "  order by person.PersonSNO";



        dhpp.executeNonQuery(InsertTODOSql, aDict);

        string result = "success";
        return result;
    }



    //[WebMethod]
    //public static string UpdateAudit(string state, string id, string optionGroup, string addU, string note)
    //{
    //    string result = "";
    //    string[] stotle = optionGroup.Split(',');

    //    //查詢使用者申請系統名稱及person編號
    //    Dictionary<string, object> aDict = new Dictionary<string, object>();
    //    DataHelper dhp = new DataHelper();
    //    aDict.Add("PersonDSNO", id);
    //    DataTable objDT;

    //    string searchsql = @"
    //        SELECT PD.*, SM.SYSTEM_NAME,PS.PersonSNO
    //        FROM PersonD PD
    //        LEFT JOIN SYSTEM SM ON SM.SYSTEM_ID = PD.SYSTEM_ID
    //        INNER JOIN Person PS ON PS.PersonID = PD.PersonID  
    //        WHERE PD.PersonDSNO=@PersonDSNO
    //    ";
    //    objDT = dhp.queryData(searchsql, aDict);


    //    aDict.Clear();
    //    if (objDT.Rows[0]["SysPAccountIsUser"].ToString() != state)
    //    {

    //        //讀取帳號資料
    //        string PersonSNO = objDT.Rows[0]["PersonSNO"].ToString();
    //        string sysAcc = objDT.Rows[0]["SysPAccount"].ToString();
    //        string SYSTEM_ID = objDT.Rows[0]["SYSTEM_ID"].ToString();


    //        //SSO驗證
    //        string DATETIME = DateTime.Now.ToString("yyyyMMddHHmmss");
    //        RSAsign.RSASignWSSoapClient RSAsign = new RSAsign.RSASignWSSoapClient();
    //        string sign = RSAsign.GetSignStr(sysAcc, DATETIME);
    //        byte[] vsign = RSAsign.GetSign(sysAcc, DATETIME, sign, SYSTEM_ID);
    //        string vsign_64 = Convert.ToBase64String(vsign);


    //        bool bAudit = false;
    //        //呼叫子系統審核API，並更新子系統帳號狀態
    //        switch (SYSTEM_ID)
    //        {
    //            case "S01": break;
    //            case "S02": break;
    //            case "S03": break;
    //            case "S04": break;
    //            case "S05": break;
    //            case "S06": break;
    //            case "S07": break;
    //            case "S08":
    //                //MCHSWS.AccountAudit MCHS_Audit = new MCHSWS.AccountAudit();
    //                //bAudit = MCHS_Audit.AuditAccount(vsign_64, sign, "organ", sysAcc, state);
    //                break;
    //            default: break;
    //        }



    //        //呼叫子系統審核API成功
    //        if (bAudit == true)
    //        {

    //            //更新PersonD帳號狀態
    //            DataHelper odt = new DataHelper();
    //            aDict.Add("SysPAccountIsUser", state);
    //            aDict.Add("PersonDSNO", id);
    //            string sql = @"UPDATE PersonD SET SysPAccountIsUser=@SysPAccountIsUser WHERE PersonDSNO=@PersonDSNO";
    //            odt.executeNonQuery(sql, aDict);


    //            //更新主帳號Person IsEnable狀態
    //            //switch (state)
    //            //{
    //            //    default:
    //            //        break;
    //            //}
    //            //string searchsql = @"
    //            //    SELECT * FROM PersonD PD WHERE PD.PersonDSNO=@PersonDSNO
    //            //";

    //            string IsEnable = "";
    //            switch (state)
    //            {
    //                case "D": IsEnable = ""; break;    //系統審核中
    //                case "N": IsEnable = ""; break;    //停權
    //                case "Y": IsEnable = "1"; break;    //審核通過
    //                case "S": IsEnable = "0"; break;    //審核退回
    //                default: break;
    //            }

    //            aDict.Clear();
    //            aDict.Add("IsEnable", IsEnable);
    //            aDict.Add("PersonSNO", PersonSNO);
    //            string usql = @"UPDATE Person SET IsEnable=@IsEnable WHERE PersonSNO=@PersonSNO";
    //            odt.executeNonQuery(usql, aDict);


    //            //將帳號審核狀態寫入記錄檔
    //            aDict.Clear();
    //            aDict.Add("PersonSNO", PersonSNO);
    //            aDict.Add("SYSTEM_ID", SYSTEM_ID);
    //            aDict.Add("AuditStatus", state);
    //            aDict.Add("Note", note);
    //            sql = @"Insert Into RegAuditLog(PersonSNO, SYSTEM_ID, AuditStatus, Note) Values(@PersonSNO, @SYSTEM_ID, @AuditStatus, @Note)";
    //            odt.executeNonQuery(sql, aDict);


    //            aDict.Clear();
    //            //發送站內系統通知
    //            string sqltodo = @"Insert Into TODO(TODOTITLE,TODOTEXT,getPersonSNO,postPersonSNO,state) 
    //                           Values(@TODOTITLE,@TODOTEXT,@getPersonSNO,@postPersonSNO,@state)";
    //            aDict.Add("TODOTITLE", "系統審核信件通知!");

    //            if (state == "N") state = "停權";
    //            else if (state == "Y") state = "已核准啟用";
    //            else if (state == "D") state = "系統審核中";
    //            else if (state == "S") state = "核退";
    //            aDict.Add("TODOTEXT", "<a style='font-weight:bold;font-size:16pt;'>親愛的使用者您好!</a></br></br>您申請的:【" + objDT.Rows[0]["SYSTEM_NAME"] + "】系統狀態已變更為:【" + state + "】</br> 詳細可於服務項目中申請系統狀態查看</br></br></br>醫療院所預防保健服務系統~感謝您!");
    //            aDict.Add("getPersonSNO", objDT.Rows[0]["PersonSNO"]);
    //            aDict.Add("postPersonSNO", 2);
    //            aDict.Add("state", 0);

    //            DataHelper dhpp = new DataHelper();
    //            dhpp.executeNonQuery(sqltodo, aDict);

    //            result = "success";
    //        }

    //        //呼叫子系統審核API失敗
    //        else
    //        {
    //            result = "failure";
    //        }

    //    }



    //    aDict.Clear();
    //    aDict.Add("PersonDSNO", id);
    //    string sqldel = @"Delete SYSOUR Where PersonDSNO=@PersonDSNO";
    //    dhp.executeNonQuery(sqldel, aDict);


    //    for (int i = 0; i <= stotle.Length - 1; i++)
    //    {

    //        DataHelper dhp1 = new DataHelper();
    //        Dictionary<string, object> aDict1 = new Dictionary<string, object>();

    //        aDict1.Add("PersonDSNO", id);
    //        aDict1.Add("SPLALIAS", stotle[i]);
    //        aDict1.Add("CreateUserID", addU);

    //        string sql1 = @"INSERT INTO SYSOUR ( PersonDSNO, SPLALIAS, CreateUserID ) VALUES( @PersonDSNO,@SPLALIAS,@CreateUserID )";
    //        dhp1.executeNonQuery(sql1, aDict1);
    //        aDict.Clear();

    //    }

    //    return result;
    //}


    [WebMethod]
    public static string UpdateOften(string ckval, string personsno)
    {
        string result = "";

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper odt = new DataHelper();
        aDict.Add("PersonSNO", personsno);
        aDict.Add("often", 0);

        string sql = @"
UPDATE PersonD
SET often =@often
WHERE
	 PersonSNO = @PersonSNO
";
        odt.executeNonQuery(sql, aDict);

        aDict.Clear();

        if (ckval != "")
        {
            String[] stotle = ckval.Split(',');

            for (int a = 0; a <= stotle.Length - 1; a++)
            {
                aDict.Add("PersonDSNO", stotle[a]);
                aDict.Add("often", 1);

                string sqlo = @"
UPDATE PersonD
SET often =@often
WHERE
	 PersonDSNO = @PersonDSNO
";
                odt.executeNonQuery(sqlo, aDict);
            }

        }
        result = "設定成功";

        return result;
    }



}