using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// UserData 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
// [System.Web.Script.Services.ScriptService]
public class UserData : System.Web.Services.WebService
{

    public UserData()
    {

        //如果使用設計的元件，請取消註解下列一行
        //InitializeComponent(); 
    }

    /// <summary>
    /// SSO ex:A12345678_20180209110123_17383217
    /// SystemID 系統編號 ex: S01(癌症篩檢追蹤),S02(婦幼健康),S03(兒童健康)
    /// S04(成人預防保健),S05(服務),S06(孕產婦管理),S07(全國遺傳診斷)
    /// 
    /// SysPAccount 使用者帳號 ex:John123
    /// result 成功時回傳1，失敗回傳0，字串為Null或Empty回傳2
    /// </summary>
    [WebMethod]
    public string CheckSSO(string SSO, string SystemID)
    {
        string result = "";

        if (!String.IsNullOrEmpty(SSO) && !String.IsNullOrEmpty(SystemID))
        {
            //business logic
            if (true)
            {
                //sysBindSW = 1
                result = "1";
            }
            else
            {
                result = "0";
            }

        }
        else
        {
            result = "2";
        }
        return result;
    }


    /// <summary>
    /// PersonID 使用者身分證 ex:A123456789
    /// SystemID 系統編號 ex: S01(癌症篩檢追蹤),S02(婦幼健康),S03(兒童健康),S04(成人預防保健),S05(服務),S06(孕產婦管理),S07(全國遺傳診斷)
    /// SysPAccount 使用者帳號 ex:John123
    /// sysPName 使用者名稱 ex:王小明
    /// sysPMail 使用者信箱 ex:abc@aaa.com
    /// result 成功時回傳1，失敗回傳0，字串為Null或Empty回傳3
    /// </summary>
    [WebMethod]
    public string AddAcount(string PersonID, string SystemID, string SysPAccount, string sysPName, string sysPMail)
    {
        string result = "";

        if (!String.IsNullOrEmpty(PersonID) && !String.IsNullOrEmpty(SystemID) && !String.IsNullOrEmpty(SysPAccount) && !String.IsNullOrEmpty(sysPName) && !String.IsNullOrEmpty(sysPMail))
        {
            DataHelper objDH = new DataHelper();
            //確認UB系統是否存在子系統資訊
            Dictionary<string, object> dic = new Dictionary<string, object>();
            String sqlS = @"
           SELECT
	*
FROM
	PersonD PD
LEFT JOIN SYSTEM SM ON SM.SYSTEM_ID = PD.SYSTEM_ID
WHERE
	PD.PersonID =@PersonID
AND PD.SYSTEM_ID = @System_ID
";
            String sql = @"
           SELECT
	*
FROM
	Person 
WHERE
	PersonID =@PersonID
";

            dic.Add("PersonID", PersonID);
            dic.Add("System_ID", SystemID);
           
            DataTable objDT = objDH.queryData(sqlS, dic);
            dic.Clear();
            dic.Add("PersonID", PersonID);
            DataTable objDTD = objDH.queryData(sql, dic);

            dic.Clear();

            if (objDT.Rows.Count == 0) //如果UB系統無資訊及增加(PersonD)
            {
                dic.Add("SYSTEM_ID", SystemID);
                dic.Add("SysPAccount", SysPAccount);
                dic.Add("sysPName", sysPName);
                dic.Add("SysBindSW", "2");
                dic.Add("SysPAccountIsUser", "Y");
                dic.Add("sysPMail", sysPMail);
                dic.Add("PersonID", PersonID);

                string sqlpersond = @"
                    Insert Into PersonD
                   (SYSTEM_ID,SysPAccount,sysPName,SysPAccountIsUser,sysPMail,PersonID,CreateUserID,SysBindSW) 
            Values(@SYSTEM_ID,@SysPAccount,@sysPName,@SysPAccountIsUser,@sysPMail,@PersonID,2,@SysBindSW)";


                objDH.executeNonQuery(sqlpersond, dic);

                dic.Clear();
                result = "1";
            }
            if(objDTD.Rows.Count != 0 || objDT.Rows.Count != 0) //UB系統有資訊及更新(Person & PersonD)
            {
                dic.Add("PersonID", PersonID);
                dic.Add("SYSTEM_ID", SystemID);
                dic.Add("SysPAccountIsUser", "Y"); //啟用
                dic.Add("SysBindSW", "2"); //開啟第一次登入 


                string sqlpersond = @"
                      UPDATE PersonD
SET SysBindSW = @SysBindSW,
SysPAccountIsUser = @SysPAccountIsUser
WHERE
    PersonID = @PersonID
AND SYSTEM_ID = @SYSTEM_ID
";

                objDH.executeNonQuery(sqlpersond, dic);
                result = "0";
            }
            if (objDTD.Rows.Count == 0)//如果UB系統無資訊及增加(Person)
            {
                dic.Clear();

                dic.Add("RoleSNO", 1);
                dic.Add("OrganSNO", 1);
                dic.Add("PAccount", SysPAccount);
                dic.Add("PName", sysPName);
                dic.Add("PPWD", PersonID.Substring(4, 6));
                dic.Add("PMail", sysPMail);
                dic.Add("PersonID", PersonID);
                string sqlperson = @"Insert Into Person(RoleSNO,OrganSNO,PAccount,PName,PPWD,PMail,CreateUserID,PersonID) Values(@RoleSNO,@OrganSNO,@PAccount,@PName,@PPWD,@PMail,1,@PersonID)";
                objDH.executeNonQuery(sqlperson, dic);

            }
            if (objDT.Rows.Count!=0 && objDTD.Rows.Count!= 0) //UB系統有資訊(Person & PersonD) 發送代辦事項(提示信)給UB USER
            {
               
                    dic.Clear();
                    string sqltodo = @"Insert Into TODO(TODOTITLE,TODOTEXT,getPersonSNO,postPersonSNO,state) 
                                        Values(@TODOTITLE,@TODOTEXT,@getPersonSNO,@postPersonSNO,@state)";
                    dic.Add("TODOTITLE", objDT.Rows[0]["SYSTEM_NAME"]+"審核信件通知!");
                    dic.Add("TODOTEXT", "<a style='font-weight:bold;font-size:16pt;'>親愛的使用者您好!</a></br></br>您所申請的系統【" + objDT.Rows[0]["SYSTEM_NAME"] + "】結果為【審核通過(啟用)】</br> 詳細可於服務項目中申請系統狀態查看</br></br></br>醫療院所預防保健服務系統~感謝您!");
                    dic.Add("getPersonSNO", objDTD.Rows[0]["PersonSNO"].ToString());
                    dic.Add("postPersonSNO", 2);
                    dic.Add("state", 0);


                    objDH.executeNonQuery(sqltodo, dic);
                
            }

        }
        else
        {
            result = "2";//呼叫失敗
        }
        return result;
    }

    /// <summary>
    /// PersonID 使用者身分證 ex:A123456789
    /// SystemID 系統編號 ex: S01(癌症篩檢追蹤),S02(婦幼健康),S03(兒童健康),S04(成人預防保健),S05(服務),S06(孕產婦管理),S07(全國遺傳診斷)
    /// SysPAccount 使用者帳號 ex:John123
    /// result 成功時回傳1，失敗回傳0，字串為Null或Empty回傳3
    /// </summary>
    [WebMethod]
    public string DelAccount(string PersonID, string SystemID, string SysPAccount)
    {
        string result = "";

        if (!String.IsNullOrEmpty(PersonID) && !String.IsNullOrEmpty(SystemID) && !String.IsNullOrEmpty(SysPAccount))
        {
            DataHelper objDH = new DataHelper();

            Dictionary<string, object> dic = new Dictionary<string, object>();  //查詢是否此使用者存在UB
            String sqlS = @"
            SELECT * FROM PersonD
WHERE
	PersonID =@PersonID
AND SYSTEM_ID = @SYSTEM_ID
";
          
            dic.Add("SYSTEM_ID", SystemID);
            dic.Add("PersonID", PersonID);

            DataTable objDT = objDH.queryData(sqlS, dic);

            dic.Clear();

            String sqlE = @"
           SELECT
	*
FROM
	Person 
WHERE
	PersonID =@PersonID
";
            dic.Add("PersonID", PersonID);

            DataTable objDTD = objDH.queryData(sqlE, dic);
            dic.Clear();
            if (objDT.Rows.Count!=0 && objDTD.Rows.Count != 0) //如果此使用者存在UB(Person & PersonD)
            {
 
                dic.Add("PersonID", PersonID);
                dic.Add("SYSTEM_ID", SystemID);
                dic.Add("SysPAccountIsUser", "N");  //改為停權
                

                string sql = @"
UPDATE PersonD
SET SysPAccountIsUser =@SysPAccountIsUser
WHERE
	PersonID =@PersonID
AND SYSTEM_ID = @SYSTEM_ID
";

                objDH.executeNonQuery(sql, dic);

                dic.Clear();

                //並發送代辦事項(提示信)給UB USER

                string sqltodo = @"Insert Into TODO(TODOTITLE,TODOTEXT,getPersonSNO,postPersonSNO,state) 
                                        Values(@TODOTITLE,@TODOTEXT,@getPersonSNO,@postPersonSNO,@state)";
                dic.Add("TODOTITLE", objDT.Rows[0]["SYSTEM_NAME"] + "審核信件通知!");
                dic.Add("TODOTEXT", "<a style='font-weight:bold;font-size:16pt;'>親愛的使用者您好!</a></br></br>您所申請的系統【" + objDT.Rows[0]["SYSTEM_NAME"] + "】結果為【審核通過(啟用)】</br> 詳細可於服務項目中申請系統狀態查看</br></br></br>醫療院所預防保健服務系統~感謝您!");
                dic.Add("getPersonSNO", objDTD.Rows[0]["PersonSNO"].ToString());
                dic.Add("postPersonSNO", 2);
                dic.Add("state", 0);


                objDH.executeNonQuery(sqltodo, dic);

                result = "1";
            }
            else
            {
                result = "0";
            }

        }
        else
        {
            result = "2";
        }
        return result;
    }

    /// <summary>
    /// PersonID 使用者身分證 ex:A123456789
    /// SystemID 系統編號 ex: S01(癌症篩檢追蹤),S02(婦幼健康),S03(兒童健康),S04(成人預防保健),S05(服務),S06(孕產婦管理),S07(全國遺傳診斷)
    /// SysPAccount 使用者帳號 ex:John123
    ///  state 狀態 ex: '1'(第一次登入), 'no/+核退原因'(核退+原因) PS.可不填原因,'StopDC'(帳號停用),'StartDC'(帳號啟用)
    /// result 成功時回傳1，失敗回傳0，字串為Null或Empty回傳3

    /// </summary>
    [WebMethod]
    public string ModAccount(string PersonID, string SystemID, string SysPAccount, string state)
    {
        string result = "";

        if (!String.IsNullOrEmpty(PersonID) && !String.IsNullOrEmpty(SystemID) && !String.IsNullOrEmpty(SysPAccount) && !String.IsNullOrEmpty(state))
        {
            DataHelper objDH = new DataHelper();
            //查看ub是否有此使用者
            Dictionary<string, object> dic = new Dictionary<string, object>();
            String sqlS = @"
             SELECT
	*
FROM
	PersonD PD
LEFT JOIN SYSTEM SM ON SM.SYSTEM_ID = PD.SYSTEM_ID
WHERE
	PD.PersonID =@PersonID
AND PD.SYSTEM_ID = @SYSTEM_ID
";

            dic.Add("PersonID", PersonID);
            dic.Add("SYSTEM_ID", SystemID);
            DataTable objDT = objDH.queryData(sqlS, dic);
            dic.Clear();

            String sqla = @"
           SELECT
	*
FROM
	Person 
WHERE
	PersonID =@PersonID
";

            dic.Add("PersonID", PersonID);
            
           
            DataTable objDTD = objDH.queryData(sqla, dic);
            
            
            dic.Clear();

            if (objDT.Rows.Count != 0) //UB確認有資料 (PersonD)
            {
                if (state == "1") //第一次登入
                {
                    
                    Dictionary<string, object> dicpd = new Dictionary<string, object>();
                    dicpd.Add("SYSTEM_ID", SystemID);
                    dicpd.Add("PersonID", PersonID);
                    dicpd.Add("SysBindSW", 1);

                    string sql = @"
                UPDATE PersonD
                SET SysBindSW =@SysBindSW
                WHERE
                	PersonID =@PersonID
                AND SYSTEM_ID = @SYSTEM_ID
                ";
                    objDH.executeNonQuery(sql, dicpd);
                }

                if (state.Substring(0, 2).ToUpper()+"/" == "NO/")//如果接受是核退狀態(給子系統的state為no/ 確保子系統 誤傳no為大寫或大小寫 故先轉為Upper)
                {
                    
                    Dictionary<string, object> dicpd = new Dictionary<string, object>();
                    dicpd.Add("SYSTEM_ID", SystemID);
                    dicpd.Add("PersonID", PersonID);
                    dicpd.Add("SysPAccountIsUser", "S"); //改為核退狀態

                    string sql = @"
                UPDATE PersonD
                SET SysPAccountIsUser =@SysPAccountIsUser
                WHERE
                	PersonID =@PersonID
                AND SYSTEM_ID = @SYSTEM_ID
                ";
                    objDH.executeNonQuery(sql, dicpd);


                    //發送代辦事項(提示信)給UB USER

                    dicpd.Clear();
                    string sqltodo = @"Insert Into TODO(TODOTITLE,TODOTEXT,getPersonSNO,postPersonSNO,state) 
                                        Values(@TODOTITLE,@TODOTEXT,@getPersonSNO,@postPersonSNO,@state)";
                    dicpd.Add("TODOTITLE", objDT.Rows[0]["SYSTEM_NAME"] + "審核信件通知!");

                    string no = "";
                    if(state.Length >3)
                    {
                        no = "</br>核退原因:" + state.Substring(4);
                    }

                    dicpd.Add("TODOTEXT", "<a style='font-weight:bold;font-size:16pt;'>親愛的使用者您好!</a></br></br>您所申請的系統【" + objDT.Rows[0]["SYSTEM_NAME"] + "】結果為【核退(未通過審核)】  " + no + "  </br></br></br></br>醫療院所預防保健服務系統~感謝您!");
                    dicpd.Add("getPersonSNO", objDTD.Rows[0]["PersonSNO"].ToString());
                    dicpd.Add("postPersonSNO", 2);
                    dicpd.Add("state", 0);


                    objDH.executeNonQuery(sqltodo, dicpd);


                }

                if (state == "StopDC" || state == "StartDC")//該使用者系統為停用或啟用
                {

                    Dictionary<string, object> dicpd = new Dictionary<string, object>();
                    dicpd.Add("SYSTEM_ID", SystemID);
                    dicpd.Add("PersonID", PersonID);
                    if (state == "StopDC")
                    {
                        dicpd.Add("SysPAccountIsUser", "N"); //為停權
                    }
                    else if(state == "StartDC")
                    {
                        dicpd.Add("SysPAccountIsUser", "Y"); //為啟用
                    }

                    string sql = @"
                UPDATE PersonD
                SET SysPAccountIsUser =@SysPAccountIsUser
                WHERE
                	PersonID =@PersonID
                AND SYSTEM_ID = @SYSTEM_ID
                ";
                    objDH.executeNonQuery(sql, dicpd);

                    dicpd.Clear();

                    //發送代辦事項(提示信)給UB USER

                    string msg = "";
                    if (state == "StopDC") msg = "停用";
                    if (state == "StartDC") msg = "啟用";


                    SendTODO ST = new SendTODO();

                    string title = objDT.Rows[0]["SYSTEM_NAME"] + "審核信件通知!";
                    string ct = "親愛的使用者您好!";
                    string cont = "您的系統【" + objDT.Rows[0]["SYSTEM_NAME"] + "】目前已被 " + msg + " </br>詳細可於服務項目中申請系統狀態查看";
                    ST.SendTODOHelper(title,ct,cont,objDTD.Rows[0]["PersonSNO"].ToString(),"2");
                    

                }
                result = "1";
            }
            else
            {
                result = "0";
            }

        }
        else
        {
            result = "2";
        }
        return result;
    }

}
