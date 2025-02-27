using CertificateExpireAuto.App_Code;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CertificateExpireAuto
{
    class Program
    {
        static void Main(string[] args)
        {
            ExChangeCertificate();
            DocChangeNewCertificate();
            GetNewCertificate();
        }
        public static void GetNewCertificate()//新領證
        {
            DateTime LogTime = DateTime.Now;
            int LogID = 0;
            string LogFunction = "GetNewCertificate_start";
            int ExcuteType = 0;    //0:其他, 1:查詢, 2:新增, 3:修改, 4:刪除
            string ExcuteMsg = "開始GetNewCertificate處理";
            int ResultType = 1;    //0:失敗, 1:成功
            string SQLCmd = "";
            int RunTime = 0;
            string ErrorMsg = "";

            WriteLog(LogTime, LogID, LogFunction, ExcuteType, ExcuteMsg, ResultType, SQLCmd, RunTime, ErrorMsg);
            DataHelper objDH = new DataHelper();
            string sql = @"with BasicIntegral as(
                                                SELECT P.PersonID,P.PName,P.PMail,P.RoleSNO,QI.PersonSNO,QCPC.PClassSNO,sum(QC.CHour
                                                )　基礎課程
                                                FROM [New_QSMS].[dbo].[QS_Integral] QI
                                                left join [New_QSMS].[dbo].QS_Course QC on QI.CourseSNO=QC.CourseSNO
                                                left join [New_QSMS].[dbo].QS_CoursePlanningClass QCPC on QC.PClassSNO=QCPC.PClassSNO
                                                left join [New_QSMS].[dbo].Person P on QI.PersonSNO=P.PersonSNO
                                                where QCPC.PClassSNO=1 and QI.IsUsed=0
                                                group by P.PersonID,P.PName,P.PMail,P.RoleSNO,QI.PersonSNO,QCPC.PClassSNO),
                                TargetIntegral as(
                                                select PClassSNO,TargetIntegral from QS_CoursePlanningClass QCPC where QCPC.PClassSNO=1),
                                BasicFinal as(
                                                select BasicIntegral.* from BasicIntegral 
                                                left join TargetIntegral on BasicIntegral.PClassSNO=TargetIntegral.PClassSNO
                                                where BasicIntegral.基礎課程>=TargetIntegral.TargetIntegral ),
                                SpecialIntegral as(
                                                SELECT P.PersonID,P.PName,P.PMail,P.RoleSNO,QI.PersonSNO,QCPC.PClassSNO,sum(QC.CHour)　專門課程
                                                FROM [New_QSMS].[dbo].[QS_Integral] QI
                                                left join [New_QSMS].[dbo].QS_Course QC on QI.CourseSNO=QC.CourseSNO
                                                left join [New_QSMS].[dbo].QS_CoursePlanningClass QCPC on QC.PClassSNO=QCPC.PClassSNO
                                                left join [New_QSMS].[dbo].Person P on QI.PersonSNO=P.PersonSNO
                                                where QCPC.PClassSNO in(2,3,4) and QI.IsUsed=0
                                                group by P.PersonID,P.PName,P.PMail,P.RoleSNO,QI.PersonSNO,QCPC.PClassSNO),
                                SpecialTargetIntegral as(
                                                select PClassSNO,TargetIntegral from QS_CoursePlanningClass QCPC where QCPC.PClassSNO in(2,3,4)),
                                SpecialFinal as(
                                                select SpecialIntegral.* from SpecialIntegral 
                                                left join SpecialTargetIntegral on SpecialIntegral.PClassSNO=SpecialTargetIntegral.PClassSNO
                                                where SpecialIntegral.專門課程>=SpecialTargetIntegral.TargetIntegral )
                                select SpecialFinal.*,BasicFinal.基礎課程 from SpecialFinal left join BasicFinal 
                                on BasicFinal.PersonSNO=SpecialFinal.PersonSNO where 基礎課程 is not null"; //撈出基礎課程3積分且專門滿積分名單
            DataTable objDT = objDH.queryData(sql, null);
            if (objDT.Rows.Count > 0)//如果基礎課程完成名單有人的話
            {
                try
                {
                    for (int i = 0; i < objDT.Rows.Count; i++)
                    {
                        if (!CheckNewCertificate(objDT.Rows[i]["PersonID"].ToString()) && !CheckEfficientCertificate(objDT.Rows[i]["PersonID"].ToString()))//檢查無新證明且無有效證明
                        {
                            try
                            {
                                Dictionary<string, object> aDict = new Dictionary<string, object>();
                                string PersonID = objDT.Rows[i]["PersonID"].ToString();
                                string PName = objDT.Rows[i]["PName"].ToString();
                                string RoleSNO = objDT.Rows[i]["RoleSNO"].ToString();
                                string PersonSNO = objDT.Rows[i]["PersonSNO"].ToString();
                                string PClassSNO = "";
                                switch (RoleSNO)
                                {
                                    case "10":
                                        PClassSNO = "2"; break;
                                    case "11":
                                        PClassSNO = "2"; break;
                                    case "12":
                                        PClassSNO = "3"; break;
                                    case "13":
                                        PClassSNO = "4"; break;
                                }
                                aDict.Add("PersonID", PersonID);
                                aDict.Add("PName", PName);
                                aDict.Add("RoleSNO", RoleSNO);
                                aDict.Add("PersonSNO", PersonSNO);
                                aDict.Add("PClassSNO", PClassSNO);
                                if (CheckSpecIntegral(PersonSNO, PClassSNO))//檢查專門是否滿積分
                                {
                                    #region 證書寫入
                                    string LastDatesql = @"select top 1 qi.CreateDT from QS_Integral QI
                                                           left join qs_course qc on qi.CourseSNO=qc.CourseSNO
                                                           where personSNO=@personSNO and qc.PClassSNO=@PClassSNO
                                                           order by qi.CreateDT　desc";
                                    DataTable LastDateobjDT = objDH.queryData(LastDatesql, aDict);
                                    string CertPublicDate = "2022/11/01";
                                    if (string.IsNullOrEmpty(LastDateobjDT.Rows[index: 0][columnName: "CreateDT"].ToString()))
                                    {

                                    }
                                    else
                                    {
                                        CertPublicDate = LastDateobjDT.Rows[index: 0][columnName: "CreateDT"].ToString();
                                    }
                                    DateTime LastDate = Convert.ToDateTime(CertPublicDate);//起始日.公開日用最後一個積分取得時間
                                    DateTime EndDate = LastDate.AddYears(6);//迄日用最後一個積分取得時間+6年
                                    CertPublicDate = LastDate.ToString("yyyy/MM/dd");
                                    string CertStartDate = LastDate.ToString("yyyy/MM/dd");
                                    string CertEndDate = EndDate.ToString("yyyy/12/31");
                                    string InsertSQL = @"insert into [New_QSMS].[dbo].QS_Certificate
                                                        ([PersonID],[CertID],[CTypeSNO],[CUnitSNO],[CertPublicDate],[CertStartDate],[CertEndDate],[CertExt],[IsPrint],[CreateDT],[CreateUserID],[SysChange],[IsChange],[Note] )
                                                        values
                                                        (@PersonID,@CertID,'75',@CUnitSNO,@CertPublicDate,@CertStartDate,@CertEndDate,@CertExt,@IsPrint,@CreateDT,@CreateUserID,@SysChange,@IsChange,@Note )";
                                    Dictionary<string, object> qcDict = new Dictionary<string, object>();
                                    qcDict.Add("PersonID", PersonID);
                                    qcDict.Add("CertID", "000000");
                                    qcDict.Add("CUnitSNO", 2);
                                    qcDict.Add("CertPublicDate", CertPublicDate);
                                    qcDict.Add("CertStartDate", CertStartDate);
                                    qcDict.Add("CertEndDate", CertEndDate);
                                    qcDict.Add("CertExt", 0);
                                    qcDict.Add("IsPrint", 0);
                                    qcDict.Add("CreateDT", DateTime.Now.ToShortDateString());
                                    qcDict.Add("CreateUserID", 0);
                                    qcDict.Add("SysChange", 1);
                                    qcDict.Add("IsChange", 0);
                                    qcDict.Add("Note", "已於" + DateTime.Now.ToString("yyyy/MM/dd") + "排程取得證明");
                                    objDH.executeNonQuery(InsertSQL, qcDict);
                                    #endregion
                                    #region 積分IsUsed
                                    string selectCourseSNO = @"select coursesno from qs_course where PClassSNO in";
                                    switch (RoleSNO)
                                    {
                                        case "10":
                                            selectCourseSNO += "(1,2)"; break;
                                        case "11":
                                            selectCourseSNO += "(1,2)"; break;
                                        case "12":
                                            selectCourseSNO += "(1,3)"; break;
                                        case "13":
                                            selectCourseSNO += "(1,4)"; break;
                                    }
                                    DataTable ObjDT;
                                    ObjDT = objDH.queryData(selectCourseSNO, null);

                                    for (int j = 0; j < ObjDT.Rows.Count; j++)
                                    {
                                        string CourseSNO = ObjDT.Rows[j]["coursesno"].ToString();
                                        Dictionary<string, object> IsUsedDict = new Dictionary<string, object>();
                                        string IsUsed = @"Update qs_integral set isused=1 where PersonSNO=@PersonSNO and CourseSNO=@CourseSNO";
                                        IsUsedDict.Add("PersonSNO", PersonSNO);
                                        IsUsedDict.Add("CourseSNO", CourseSNO);
                                        objDH.executeNonQuery(IsUsed, IsUsedDict);
                                    }
                                    #endregion
                                    #region 電子豹
                                    //string Name = "\n" + objDT.Rows[i]["PMail"].ToString() + ",";
                                    //string EventName = "證書換證-" + DateTime.Now.ToShortDateString();
                                    //string sn = Email.CreateGroup(EventName);
                                    //string Title = "Email,Name";
                                    //string MailContent = "您好:<br/>您已取得「戒菸服務人員資格證明」，如欲下載電子證明，請登入醫事人員戒菸服務訓練系統(https://quitsmoking.hpa.gov.tw)  ，在【個人首頁】內下載。";
                                    ////JObject匿名物件
                                    //JObject obj = new JObject(
                                    //     new JProperty("contacts", Title + Name)
                                    //    );
                                    ////序列化為JSON字串並輸出結果
                                    //var result = JsonConvert.SerializeObject(obj, Formatting.Indented);
                                    //Email.InsertMemberString(sn, result);
                                    //Dictionary<string, object> dict = new Dictionary<string, object>();
                                    //string mailSQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate()) SELECT @@IDENTITY AS 'Identity'";
                                    //dict.Add("SN", sn);
                                    //dict.Add("EventName", EventName);
                                    //dict.Add("MailContent", MailContent);
                                    //DataTable dt = objDH.queryData(mailSQL, dict);
                                    #endregion
                                    string EventName = "您已取得戒菸服務人員資格證明";
                                    string MailContent = "您好:<br/>您已取得「戒菸服務人員資格證明」，如欲下載電子證明，請登入醫事人員戒菸服務訓練系統(https://quitsmoking.hpa.gov.tw)  ，在【個人首頁】內下載。";
                                    SendMail(EventName, MailContent, objDT.Rows[i]["PMail"].ToString());
                                    #region 寄信給VPN
                                    //string VPNName = "\n" + "ttchpa@gmail.com" + ",";
                                    //string VPNEventName = "訓練系統領證通知-" + DateTime.Now.ToShortDateString();
                                    //string VPNsn = Email.CreateGroup(VPNEventName);
                                    //string VPNTitle = "Email,Name";
                                    //string VPNMailContent = "身分證:"+ PersonID+ "; 公告日:"+ CertStartDate;
                                    ////JObject匿名物件
                                    //JObject VPNobj = new JObject(
                                    //     new JProperty("contacts", VPNTitle + VPNName)
                                    //    );
                                    ////序列化為JSON字串並輸出結果
                                    //var VPNresult = JsonConvert.SerializeObject(VPNobj, Formatting.Indented);
                                    //Email.InsertMemberString(VPNsn, VPNresult);
                                    //Dictionary<string, object> VPNdict = new Dictionary<string, object>();
                                    //string VPNmailSQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate()) SELECT @@IDENTITY AS 'Identity'";
                                    //VPNdict.Add("SN", VPNsn);
                                    //VPNdict.Add("EventName", VPNEventName);
                                    //VPNdict.Add("MailContent", VPNMailContent);
                                    //DataTable VPNdt = objDH.queryData(VPNmailSQL, VPNdict);
                                    #endregion
                                    string VPNEventName = "訓練系統領證通知-" + DateTime.Now.ToShortDateString();
                                    string VPNMailContent = "身分證:" + PersonID + "; 公告日:" + CertStartDate;
                                    SendMail(VPNEventName, VPNMailContent, "ttchpa@gmail.com");
                                }
                            }
                            catch (Exception ex)
                            {
                                LogTime = DateTime.Now;
                                ExcuteMsg = objDT.Rows[i]["PersonID"].ToString() + "GetNewCertificate執行失敗";
                                ResultType = 0;    //0:失敗, 1:成功
                                ErrorMsg = ex.Message;
                                WriteLog(LogTime, LogID, LogFunction, ExcuteType, ExcuteMsg, ResultType, SQLCmd, RunTime, ErrorMsg);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogTime = DateTime.Now;
                    ExcuteMsg = "GetNewCertificate執行失敗";
                    ResultType = 0;    //0:失敗, 1:成功
                    ErrorMsg = ex.Message;
                    WriteLog(LogTime, LogID, LogFunction, ExcuteType, ExcuteMsg, ResultType, SQLCmd, RunTime, ErrorMsg);
                }

            }

            LogTime = DateTime.Now;
            LogID = 0;
            LogFunction = "GetNewCertificateend";
            ExcuteType = 0;    //0:其他, 1:查詢, 2:新增, 3:修改, 4:刪除
            ExcuteMsg = "結束GetNewCertificate處理";
            ResultType = 1;    //0:失敗, 1:成功
            SQLCmd = "";
            RunTime = 0;
            ErrorMsg = "";
            WriteLog(LogTime, LogID, LogFunction, ExcuteType, ExcuteMsg, ResultType, SQLCmd, RunTime, ErrorMsg);
        }
        public static void DocChangeNewCertificate()//醫師一積分換新證明
        {
            DateTime LogTime = DateTime.Now;
            int LogID = 0;
            string LogFunction = "DocChangeNewCertificate_start";
            int ExcuteType = 0;    //0:其他, 1:查詢, 2:新增, 3:修改, 4:刪除
            string ExcuteMsg = "開始DocChangeNewCertificate處理";
            int ResultType = 1;    //0:失敗, 1:成功
            string SQLCmd = "";
            int RunTime = 0;
            string ErrorMsg = "";

            WriteLog(LogTime, LogID, LogFunction, ExcuteType, ExcuteMsg, ResultType, SQLCmd, RunTime, ErrorMsg);

            DataHelper objDH = new DataHelper();
            string sql = @"select p.PersonSNO,p.PersonID,p.RoleSNO,p.PMail,qi.ISNO,qi.CreateDT from [New_QSMS].[dbo].[qs_integral]　qi
left join [New_QSMS].[dbo].[QS_Course] qc on qi.CourseSNO=qc.CourseSNO
left join [New_QSMS].[dbo].[Person] P on qi.PersonSNO=P.PersonSNO
where qi.coursesno=17 and qi.IsUsed=0"; //撈出取得補課積分名單
            DataTable objDT = objDH.queryData(sql, null);
            if (objDT.Rows.Count > 0)
            {
                try
                {
                    for (int i = 0; i < objDT.Rows.Count; i++)
                    {
                        try {
                            if (!CheckNewCertificate(objDT.Rows[i]["PersonID"].ToString()))//檢查有無新證明
                            {
                                string QCsql = @"SELECT top 1 [PersonID],[CertPublicDate],[CertStartDate],[CertEndDate]
                                FROM [New_QSMS].[dbo].[QS_Certificate]
                                where PersonID=@PersonID and CertEndDate>getdate() and CTypeSNO IN(1,53,2,54) order by CertEndDate desc"; //撈出舊證書
                                Dictionary<string, object> oldDict = new Dictionary<string, object>();
                                oldDict.Add("PersonID", objDT.Rows[i]["PersonID"]);
                                DataTable QCobjDT = objDH.queryData(QCsql, oldDict);
                                if (QCobjDT.Rows.Count > 0)//有找到證書的話
                                {
                                    #region 給新證明
                                    string PersonID = objDT.Rows[i]["PersonID"].ToString();
                                    DateTime QIDate = DateTime.Parse(objDT.Rows[i]["CreateDT"].ToString());
                                    DateTime OldQCPublic = DateTime.Parse(QCobjDT.Rows[0]["CertPublicDate"].ToString());
                                    string CertPublicDate = OldQCPublic.ToShortDateString();
                                    string CertStartDate = QIDate.ToShortDateString();
                                    DateTime QCEndDate = DateTime.Parse(QCobjDT.Rows[0]["CertEndDate"].ToString());
                                    string CertEndDate = QCEndDate.ToShortDateString();
                                    string InsertSQL = @"  insert into [New_QSMS].[dbo].QS_Certificate  ([PersonID],[CertID],[CTypeSNO],[CUnitSNO],[CertPublicDate],[CertStartDate],[CertEndDate],[CertExt],[IsPrint],[CreateDT],[CreateUserID],[SysChange],[IsChange],[Note] )
                                        values(@PersonID,@CertID,'75',@CUnitSNO,@CertPublicDate,@CertStartDate,@CertEndDate,@CertExt,@IsPrint,@CreateDT,@CreateUserID,@SysChange,@IsChange,@Note )";
                                    Dictionary<string, object> qcDict = new Dictionary<string, object>();
                                    qcDict.Add("PersonID", PersonID);
                                    qcDict.Add("CertID", "000000");
                                    qcDict.Add("CUnitSNO", 2);
                                    qcDict.Add("CertPublicDate", CertPublicDate);
                                    qcDict.Add("CertStartDate", CertStartDate);
                                    qcDict.Add("CertEndDate", CertEndDate);
                                    qcDict.Add("CertExt", 0);
                                    qcDict.Add("IsPrint", 0);
                                    qcDict.Add("CreateDT", DateTime.Now.ToShortDateString());
                                    qcDict.Add("CreateUserID", 0);
                                    qcDict.Add("SysChange", 1);
                                    qcDict.Add("IsChange", 0);
                                    qcDict.Add("Note", "已於" + DateTime.Now.ToString("yyyy/MM/dd") + "排程取得新證明");
                                    objDH.executeNonQuery(InsertSQL, qcDict);
                                    #endregion
                                    #region 積分IsUsed
                                    string updateIntegral = @"update [New_QSMS].[dbo].QS_Integral set IsUsed=1 where PersonSNO=@PersonSNO and CourseSNO=17 ";
                                    Dictionary<string, object> QIDict = new Dictionary<string, object>();
                                    QIDict.Add("PersonSNO", objDT.Rows[i]["PersonSNO"]);
                                    objDH.executeNonQuery(updateIntegral, QIDict);
                                    #endregion
                                    #region 電子豹
                                    //string Name = "\n" + objDT.Rows[i]["PMail"].ToString() + ",";
                                    //string EventName = "證書換證-" + DateTime.Now.ToShortDateString();
                                    //string sn = Email.CreateGroup(EventName);
                                    //string Title = "Email,Name";
                                    //string MailContent = "您好:<br/>您已取得「戒菸服務人員資格證明」，如欲下載電子證明，請登入醫事人員戒菸服務訓練系統(https://quitsmoking.hpa.gov.tw)  ，在【個人首頁】內下載。";
                                    ////JObject匿名物件
                                    //JObject obj = new JObject(
                                    //     new JProperty("contacts", Title + Name)
                                    //    );
                                    ////序列化為JSON字串並輸出結果
                                    //var result = JsonConvert.SerializeObject(obj, Formatting.Indented);
                                    //Email.InsertMemberString(sn, result);
                                    //Dictionary<string, object> dict = new Dictionary<string, object>();
                                    //string mailSQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate()) SELECT @@IDENTITY AS 'Identity'";
                                    //dict.Add("SN", sn);
                                    //dict.Add("EventName", EventName);
                                    //dict.Add("MailContent", MailContent);
                                    //DataTable dt = objDH.queryData(mailSQL, dict);
                                    string EventName = "您已取得戒菸服務人員資格證明";
                                    string MailContent = "您好:<br/>您已取得「戒菸服務人員資格證明」，如欲下載電子證明，請登入醫事人員戒菸服務訓練系統(https://quitsmoking.hpa.gov.tw)  ，在【個人首頁】內下載。";
                                    SendMail(EventName, MailContent, objDT.Rows[i]["PMail"].ToString());
                                    #endregion
                                    #region 寄信給VPN
                                    //string VPNName = "\n" + "ttchpa@gmail.com" + ",";
                                    //string VPNEventName = "訓練系統領證通知-" + DateTime.Now.ToShortDateString();
                                    //string VPNsn = Email.CreateGroup(VPNEventName);
                                    //string VPNTitle = "Email,Name";
                                    //string VPNMailContent = "身分證:" + PersonID + "; 公告日:" + CertStartDate;
                                    ////JObject匿名物件
                                    //JObject VPNobj = new JObject(
                                    //     new JProperty("contacts", VPNTitle + VPNName)
                                    //    );
                                    ////序列化為JSON字串並輸出結果
                                    //var VPNresult = JsonConvert.SerializeObject(VPNobj, Formatting.Indented);
                                    //Email.InsertMemberString(VPNsn, VPNresult);
                                    //Dictionary<string, object> VPNdict = new Dictionary<string, object>();
                                    //string VPNmailSQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate()) SELECT @@IDENTITY AS 'Identity'";
                                    //VPNdict.Add("SN", VPNsn);
                                    //VPNdict.Add("EventName", VPNEventName);
                                    //VPNdict.Add("MailContent", VPNMailContent);
                                    //DataTable VPNdt = objDH.queryData(VPNmailSQL, VPNdict);
                                    string VPNEventName = "訓練系統領證通知-" + DateTime.Now.ToShortDateString();
                                    string VPNMailContent = "身分證:" + PersonID + "; 公告日:" + CertStartDate;
                                    SendMail(VPNEventName, VPNMailContent, "ttchpa@gmail.com");
                                    #endregion
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            LogTime = DateTime.Now;
                            ExcuteMsg = objDT.Rows[i]["PersonID"].ToString() + "DocChangeNewCertificate執行失敗";
                            ResultType = 0;    //0:失敗, 1:成功
                            ErrorMsg = ex.Message;
                            WriteLog(LogTime, LogID, LogFunction, ExcuteType, ExcuteMsg, ResultType, SQLCmd, RunTime, ErrorMsg);
                        }
                    }
                }
                catch(Exception ex)
                {
                    LogTime = DateTime.Now;
                    ExcuteMsg = "DocChangeNewCertificate執行失敗";
                    ResultType = 0;    //0:失敗, 1:成功
                    ErrorMsg = ex.Message;
                    WriteLog(LogTime, LogID, LogFunction, ExcuteType, ExcuteMsg, ResultType, SQLCmd, RunTime, ErrorMsg);
                }
            }
            LogTime = DateTime.Now;
            LogID = 0;
            LogFunction = "DocChangeNewCertificate";
            ExcuteType = 0;    //0:其他, 1:查詢, 2:新增, 3:修改, 4:刪除
            ExcuteMsg = "結束DocChangeNewCertificate處理";
            ResultType = 1;    //0:失敗, 1:成功
            SQLCmd = "";
            RunTime = 0;
            ErrorMsg = "";
            WriteLog(LogTime, LogID, LogFunction, ExcuteType, ExcuteMsg, ResultType, SQLCmd, RunTime, ErrorMsg);
        }
        public static void ExChangeCertificate()
        {
            DateTime LogTime = DateTime.Now;
            int LogID = 0;
            string LogFunction = "ExChangeCertificate_start";
            int ExcuteType = 0;    //0:其他, 1:查詢, 2:新增, 3:修改, 4:刪除
            string ExcuteMsg = "開始ExChangeCertificate處理";
            int ResultType = 1;    //0:失敗, 1:成功
            string SQLCmd = "";
            int RunTime = 0;
            string ErrorMsg = "";

            WriteLog(LogTime, LogID, LogFunction, ExcuteType, ExcuteMsg, ResultType, SQLCmd, RunTime, ErrorMsg);
            DataHelper objDH = new DataHelper();
            string sql = @"with allPerson as(select PersonSNO,PersonID,PName,PMail,RoleSNO from Person where IsEnable=1),
EcoursePlan as(select ECPC.EPClassSNO,PlanName,CTypeSNO,TotalIntegral,PClassSNO,ECPR.RoleSNO from QS_ECoursePlanningClass ECPC
left join QS_ECoursePlanningRole ECPR on ECPC.EPClassSNO=ECPR.EPClassSNO),
allIntegralList as(select ISNO,PersonSNO,QI.CourseSNO,QC.PClassSNO,CHour,QECPC.EPClassSNO,QECPC.PlanName,QECPC.TotalIntegral from QS_Integral QI
left join QS_Course QC on  QI.CourseSNO=QC.CourseSNO
left join QS_CoursePlanningClass QCPC on QC.PClassSNO=QCPC.PClassSNO
left join QS_ECoursePlanningClass QECPC on QCPC.PClassSNO=QECPC.PClassSNO where QI.IsUsed<>1),
getAllSum as (select PersonSNO,EPClassSNO,SUM(CHour)線上總積分 from allIntegralList group by PersonSNO,EPClassSNO),
allEIntegralList as(select PersonID,PName,EPClassSNO,CDate,Integral from QS_EIntegral where IsUsed<>1),
getAllESum as(select PersonID,EPClassSNO,SUM(Integral)實體總積分 from allEIntegralList group by PersonID,EPClassSNO),
getAllPersonInt as (select Ap.*,EP.EPClassSNO,EP.PClassSNO,EP.PlanName,EP.TotalIntegral,isNull(GAS.線上總積分,0)線上總積分,isNull(GAES.實體總積分,0)實體總積分,isNull(GAS.線上總積分,0)+isNull(GAES.實體總積分,0)總積分 from allPerson AP 
left join Role R on AP.RoleSNO=R.RoleSNO
left join EcoursePlan EP on R.DocGroup=EP.RoleSNO
left join getAllSum GAS on EP.EPClassSNO=GAS.EPClassSNO and AP.PersonSNO=GAS.PersonSNO
left join getAllESum GAES on EP.EPClassSNO=GAES.EPClassSNO and AP.PersonID=GAES.PersonID)
select * from getAllPersonInt where 總積分>=TotalIntegral";//找出所有總積分大於目標積分
            DataTable objDT = objDH.queryData(sql, null);
            if (objDT.Rows.Count > 0)//如果有總積分大於目標積分的話
            {
                try
                {
                    for (int i = 0; i < objDT.Rows.Count; i++)
                    {
                        try
                        {
                            string PersonSNO = objDT.Rows[i]["PersonSNO"].ToString();
                            string PersonID = objDT.Rows[i]["PersonID"].ToString();
                            string RoleSNO = objDT.Rows[i]["RoleSNO"].ToString();
                            string EPClassSNO = objDT.Rows[i]["EPClassSNO"].ToString();
                            string PClassSNO = objDT.Rows[i]["PClassSNO"].ToString();
                            string TotalIntegral = objDT.Rows[i]["TotalIntegral"].ToString();
                            string PName = objDT.Rows[i]["PName"].ToString();
                            if (EPClassSNO == "8")//繼續教育課程(證明展延)
                            {

                                if (CheckNewCertificate(objDT.Rows[i]["PersonID"].ToString()))//已經有新證明
                                {
                                    string selectNewCertificate = @"select CertSNO,PersonID,CertPublicDate,CertStartDate,CertEndDate from qs_certificate where  PersonID=@PersonID and CTypeSNO=75 and CertEndDate>getdate()";
                                    Dictionary<string, object> oldDict = new Dictionary<string, object>();
                                    oldDict.Add("PersonID", PersonID);
                                    DataTable OldobjDT = objDH.queryData(selectNewCertificate, oldDict);
                                    string CertSNO = OldobjDT.Rows[0]["CertSNO"].ToString();
                                    DateTime CertPublicDateDT = Convert.ToDateTime(OldobjDT.Rows[0]["CertPublicDate"].ToString());
                                    string CertPublicDate = CertPublicDateDT.ToShortDateString();
                                    DateTime CertStartDateDT = Convert.ToDateTime(OldobjDT.Rows[0]["CertStartDate"].ToString());
                                    string CertStartDate = CertStartDateDT.ToShortDateString();
                                    string CertEndDate = OldobjDT.Rows[0]["CertEndDate"].ToString();
                                    int EndYear = DateTime.Parse(CertEndDate).Year;//取得原本證書到期年
                                    int NowYear = DateTime.Now.Year;
                                    if (EndYear == NowYear)//到期年=今年才換
                                    {
                                        string QIdate = @"select top 1 QI.CreateDT from QS_Integral QI
left join QS_Course QC on QI.CourseSNO=QC.CourseSNO
where personSNO=@personSNO and PClassSNO=@PClassSNO and isused<>1 order by QI.CreateDT desc";
                                        Dictionary<string, object> QIDict = new Dictionary<string, object>();
                                        QIDict.Add("PersonID", PersonID);
                                        QIDict.Add("PersonSNO", PersonSNO);
                                        QIDict.Add("PClassSNO", PClassSNO);
                                        QIDict.Add("EPClassSNO", EPClassSNO);
                                        DataTable QIobjDT = objDH.queryData(QIdate, QIDict);//線上最新日期
                                        string EQIdate = @"select top 1 QI.CDate from QS_EIntegral QI
where PersonID=@PersonID and EPClassSNO=@EPClassSNO and isused<>1  order by QI.CreateDT desc";
                                        DataTable EQIobjDT = objDH.queryData(EQIdate, QIDict);//實體最新日期
                                        string IntegralDate = "";
                                        if (QIobjDT.Rows.Count > 0 && EQIobjDT.Rows.Count == 0)//線上沒有就用實體
                                        {
                                            IntegralDate = QIobjDT.Rows[0]["CreateDT"].ToString();
                                        }
                                        else if (QIobjDT.Rows.Count == 0 && EQIobjDT.Rows.Count > 0)//實體沒有就用線上
                                        {
                                            IntegralDate = EQIobjDT.Rows[0]["CDate"].ToString();
                                        }
                                        else
                                        {
                                            DateTime QIDate = DateTime.Parse(QIobjDT.Rows[0]["CreateDT"].ToString());
                                            DateTime EQIDate = DateTime.Parse(EQIobjDT.Rows[0]["CDate"].ToString());
                                            if (QIDate > EQIDate)//線上大於實體用線上
                                            {
                                                IntegralDate = QIobjDT.Rows[0]["CreateDT"].ToString();
                                            }
                                            else
                                            {
                                                IntegralDate = EQIobjDT.Rows[0]["CDate"].ToString();
                                            }
                                        }

                                        {
                                            int IntegralDateTime = DateTime.Parse(IntegralDate).Year;
                                            if (IntegralDateTime < EndYear)
                                            {
                                                CertStartDate = NowYear.ToString() + "/01/01";

                                            }
                                            else
                                            {
                                                CertStartDateDT = Convert.ToDateTime(IntegralDate);
                                                CertStartDate = CertStartDateDT.ToShortDateString();
                                            }
                                        }
                                        CertEndDate = DateTime.Now.AddYears(6).ToString("yyyy/12/31");
                                        string InsertSQL = @"   update [New_QSMS].[dbo].QS_Certificate set CertStartDate=@CertStartDate,CertEndDate=@CertEndDate,ModifyDT=@ModifyDT,SysChange=@SysChange,Note=@Note where CertSNO=@CertSNO";
                                        Dictionary<string, object> qcDict = new Dictionary<string, object>();
                                        qcDict.Add("CertSNO", CertSNO);
                                        qcDict.Add("CertStartDate", CertStartDate);
                                        qcDict.Add("CertEndDate", CertEndDate);
                                        qcDict.Add("ModifyDT", DateTime.Now.ToShortDateString());
                                        qcDict.Add("SysChange", 1);
                                        qcDict.Add("Note", "已於" + DateTime.Now.ToString("yyyy/MM/dd") + "排程延長證明");
                                        objDH.executeNonQuery(InsertSQL, qcDict);

                                        #region 積分IsUsed
                                        string selectCourseSNO = @"select coursesno from qs_course where PClassSNO=9";
                                        DataTable ObjDT;
                                        ObjDT = objDH.queryData(selectCourseSNO, null);

                                        for (int j = 0; j < ObjDT.Rows.Count; j++)
                                        {
                                            string CourseSNOisUsed = ObjDT.Rows[j]["coursesno"].ToString();
                                            Dictionary<string, object> IsUsedDict = new Dictionary<string, object>();
                                            string IsUsed = @"Update qs_integral set isused=1 where PersonSNO=@PersonSNO and CourseSNO=@CourseSNO";
                                            IsUsedDict.Add("PersonSNO", PersonSNO);
                                            IsUsedDict.Add("CourseSNO", CourseSNOisUsed);
                                            objDH.executeNonQuery(IsUsed, IsUsedDict);
                                        }
                                        string EQIIsUsed = @"Update qs_Eintegral set isused=1 where PersonID=@PersonID and EPCLASSSNO=8";
                                        Dictionary<string, object> EQIIsUsedDict = new Dictionary<string, object>();
                                        EQIIsUsedDict.Add("PersonID", PersonID);
                                        objDH.executeNonQuery(EQIIsUsed, EQIIsUsedDict);
                                        #endregion
                                        #region 電子豹
                                        //string Name = "\n" + objDT.Rows[i]["PMail"].ToString() + ",";
                                        //string EventName = "證書換證-" + DateTime.Now.ToShortDateString();
                                        //string sn = Email.CreateGroup(EventName);
                                        //string Title = "Email,Name";
                                        //string MailContent = "您好:<br/>您已完成「戒菸服務人員資格證明」效期更新，如欲下載電子證明，請登入醫事人員戒菸服務訓練系統(https://quitsmoking.hpa.gov.tw)，在【個人首頁】內下載。";
                                        ////JObject匿名物件
                                        //JObject obj = new JObject(
                                        //     new JProperty("contacts", Title + Name)
                                        //    );
                                        ////序列化為JSON字串並輸出結果
                                        //var result = JsonConvert.SerializeObject(obj, Formatting.Indented);
                                        //Email.InsertMemberString(sn, result);
                                        //Dictionary<string, object> dict = new Dictionary<string, object>();
                                        //string mailSQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate()) SELECT @@IDENTITY AS 'Identity'";
                                        //dict.Add("SN", sn);
                                        //dict.Add("EventName", EventName);
                                        //dict.Add("MailContent", MailContent);
                                        //DataTable dt = objDH.queryData(mailSQL, dict);
                                        #endregion
                                        string EventName = "您已完成戒菸服務人員資格證明效期更新";
                                        string MailContent = "您好:<br/>您已完成「戒菸服務人員資格證明」效期更新，如欲下載電子證明，請登入醫事人員戒菸服務訓練系統(https://quitsmoking.hpa.gov.tw)  ，在【個人首頁】內下載。";
                                        SendMail(EventName, MailContent, objDT.Rows[i]["PMail"].ToString());

                                        #region 寄信給VPN
                                        //string VPNName = "\n" + "ttchpa@gmail.com" + ",";
                                        //string VPNEventName = "訓練系統領證通知-" + DateTime.Now.ToShortDateString();
                                        //string VPNsn = Email.CreateGroup(VPNEventName);
                                        //string VPNTitle = "Email,Name";
                                        //string VPNMailContent = "身分證:" + PersonID + "; 公告日:" + CertStartDate;
                                        ////JObject匿名物件
                                        //JObject VPNobj = new JObject(
                                        //     new JProperty("contacts", VPNTitle + VPNName)
                                        //    );
                                        ////序列化為JSON字串並輸出結果
                                        //var VPNresult = JsonConvert.SerializeObject(VPNobj, Formatting.Indented);
                                        //Email.InsertMemberString(VPNsn, VPNresult);
                                        //Dictionary<string, object> VPNdict = new Dictionary<string, object>();
                                        //string VPNmailSQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate()) SELECT @@IDENTITY AS 'Identity'";
                                        //VPNdict.Add("SN", VPNsn);
                                        //VPNdict.Add("EventName", VPNEventName);
                                        //VPNdict.Add("MailContent", VPNMailContent);
                                        //DataTable VPNdt = objDH.queryData(VPNmailSQL, VPNdict);
                                        string VPNEventName = "訓練系統領證通知-" + DateTime.Now.ToShortDateString();
                                        string VPNMailContent = "身分證:" + PersonID + "; 公告日:" + CertStartDate;
                                        SendMail(VPNEventName, VPNMailContent, "ttchpa@gmail.com");
                                        #endregion
                                    }
                                }
                                else//尚無新證明
                                {
                                    if (RoleSNO == "10" || RoleSNO=="11")
                                    {

                                    }
                                    else
                                    {
                                        string selectOldCertificate = @"select CertSNO,qc.CUnitSNO,PersonID,CertPublicDate,CertStartDate,CertEndDate from qs_certificate qc left join qs_certificatetype qct on qc.ctypesno=qct.ctypesno  where  PersonID=@PersonID and CtypeClass<>0  and CertEndDate>getdate() order BY CertEndDate DESC";
                                        Dictionary<string, object> oldDict = new Dictionary<string, object>();
                                        oldDict.Add("PersonID", PersonID);
                                        DataTable OldobjDT = objDH.queryData(selectOldCertificate, oldDict);
                                        string CertSNO = OldobjDT.Rows[0]["CertSNO"].ToString();
                                        string CUnitSNO = OldobjDT.Rows[0]["CUnitSNO"].ToString();
                                        DateTime CertPublicDateTime = DateTime.Parse(OldobjDT.Rows[0]["CertPublicDate"].ToString());
                                        string CertPublicDate = CertPublicDateTime.ToString("yyyy/MM/dd");
                                        string CertStartDate = OldobjDT.Rows[0]["CertStartDate"].ToString();
                                        string CertEndDate = OldobjDT.Rows[0]["CertEndDate"].ToString();
                                        int EndYear = DateTime.Parse(CertEndDate).Year;//取得原本證書到期年
                                        int NowYear = DateTime.Now.Year;
                                        if (EndYear == NowYear)
                                        {
                                            string QIdate = @"select top 1 QI.CreateDT from QS_Integral QI
left join QS_Course QC on QI.CourseSNO=QC.CourseSNO
where personSNO=@personSNO and PClassSNO=@PClassSNO and isused<>1 order by QI.CreateDT desc";
                                            Dictionary<string, object> QIDict = new Dictionary<string, object>();
                                            QIDict.Add("PersonID", PersonID);
                                            QIDict.Add("PersonSNO", PersonSNO);
                                            QIDict.Add("PClassSNO", PClassSNO);
                                            QIDict.Add("EPClassSNO", EPClassSNO);
                                            DataTable QIobjDT = objDH.queryData(QIdate, QIDict);//線上最新日期
                                            string EQIdate = @"select top 1 QI.CDate from QS_EIntegral QI
where PersonID=@PersonID and EPClassSNO=@EPClassSNO and isused<>1  order by QI.CreateDT desc";
                                            DataTable EQIobjDT = objDH.queryData(EQIdate, QIDict);//實體最新日期
                                            string IntegralDate = "";
                                            if (QIobjDT.Rows.Count > 0 && EQIobjDT.Rows.Count == 0)//線上沒有就用實體
                                            {
                                                IntegralDate = QIobjDT.Rows[0]["CreateDT"].ToString();
                                            }
                                            else if (QIobjDT.Rows.Count == 0 && EQIobjDT.Rows.Count > 0)//實體沒有就用線上
                                            {
                                                IntegralDate = EQIobjDT.Rows[0]["CDate"].ToString();
                                            }
                                            else
                                            {
                                                DateTime QIDate = DateTime.Parse(QIobjDT.Rows[0]["CreateDT"].ToString());
                                                DateTime EQIDate = DateTime.Parse(EQIobjDT.Rows[0]["CDate"].ToString());
                                                if (QIDate > EQIDate)//線上大於實體用線上
                                                {
                                                    IntegralDate = QIobjDT.Rows[0]["CreateDT"].ToString();
                                                }
                                                else
                                                {
                                                    IntegralDate = EQIobjDT.Rows[0]["CDate"].ToString();
                                                }
                                            }

                                            int IntegralYear = DateTime.Parse(IntegralDate).Year;
                                            DateTime IntegralDateTime = DateTime.Parse(IntegralDate);
                                            if (IntegralYear < EndYear)
                                            {
                                                CertStartDate = NowYear.ToString() + "/01/01";

                                            }
                                            else
                                            {
                                                CertStartDate = IntegralDateTime.ToShortDateString();
                                            }
                                            CertEndDate = DateTime.Now.AddYears(6).ToString("yyyy/12/31");

                                            string InsertSQL = @"  insert into [New_QSMS].[dbo].QS_Certificate  ([PersonID],[CertID],[CTypeSNO],[CUnitSNO],[CertPublicDate],[CertStartDate],[CertEndDate],[CertExt],[IsPrint],[CreateDT],[CreateUserID],[SysChange],[IsChange],[Note] )
                                        values(@PersonID,@CertID,'75',@CUnitSNO,@CertPublicDate,@CertStartDate,@CertEndDate,@CertExt,@IsPrint,@CreateDT,@CreateUserID,@SysChange,@IsChange,@Note )";
                                            Dictionary<string, object> qcDict = new Dictionary<string, object>();
                                            qcDict.Add("PersonID", PersonID);
                                            qcDict.Add("CertID", "000000");
                                            qcDict.Add("CUnitSNO", CUnitSNO);
                                            qcDict.Add("CertPublicDate", CertPublicDate);
                                            qcDict.Add("CertStartDate", CertStartDate);
                                            qcDict.Add("CertEndDate", CertEndDate);
                                            qcDict.Add("CertExt", 0);
                                            qcDict.Add("IsPrint", 0);
                                            qcDict.Add("CreateDT", DateTime.Now.ToShortDateString());
                                            qcDict.Add("CreateUserID", 0);
                                            qcDict.Add("SysChange", 1);
                                            qcDict.Add("IsChange", 0);
                                            qcDict.Add("Note", "已於" + DateTime.Now.ToString("yyyy/MM/dd") + "排程取得新證明");
                                            objDH.executeNonQuery(InsertSQL, qcDict);
                                            #region 積分IsUsed
                                            string selectCourseSNO = @"select coursesno from qs_course where PClassSNO=9";
                                            DataTable ObjDT;
                                            ObjDT = objDH.queryData(selectCourseSNO, null);

                                            for (int j = 0; j < ObjDT.Rows.Count; j++)
                                            {
                                                string CourseSNOisUsed = ObjDT.Rows[j]["coursesno"].ToString();
                                                Dictionary<string, object> IsUsedDict = new Dictionary<string, object>();
                                                string IsUsed = @"Update qs_integral set isused=1 where PersonSNO=@PersonSNO and CourseSNO=@CourseSNO";
                                                IsUsedDict.Add("PersonSNO", PersonSNO);
                                                IsUsedDict.Add("CourseSNO", CourseSNOisUsed);
                                                objDH.executeNonQuery(IsUsed, IsUsedDict);
                                            }
                                            string EQIIsUsed = @"Update qs_Eintegral set isused=1 where PersonID=@PersonID and EPCLASSSNO=8";
                                            Dictionary<string, object> EQIIsUsedDict = new Dictionary<string, object>();
                                            EQIIsUsedDict.Add("PersonID", PersonID);
                                            objDH.executeNonQuery(EQIIsUsed, EQIIsUsedDict);
                                            #endregion
                                            #region 電子豹
                                            //string Name = "\n" + objDT.Rows[i]["PMail"].ToString() + ",";
                                            //string EventName = "證書換證-" + DateTime.Now.ToShortDateString();
                                            //string sn = Email.CreateGroup(EventName);
                                            //string Title = "Email,Name";
                                            //string MailContent = "您好:<br/>您已取得「戒菸服務人員資格證明」，如欲下載電子證明，請登入醫事人員戒菸服務訓練系統(https://quitsmoking.hpa.gov.tw)  ，在【個人首頁】內下載。";
                                            ////JObject匿名物件
                                            //JObject obj = new JObject(
                                            //     new JProperty("contacts", Title + Name)
                                            //    );
                                            ////序列化為JSON字串並輸出結果
                                            //var result = JsonConvert.SerializeObject(obj, Formatting.Indented);
                                            //Email.InsertMemberString(sn, result);
                                            //Dictionary<string, object> dict = new Dictionary<string, object>();
                                            //string mailSQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate()) SELECT @@IDENTITY AS 'Identity'";
                                            //dict.Add("SN", sn);
                                            //dict.Add("EventName", EventName);
                                            //dict.Add("MailContent", MailContent);
                                            //DataTable dt = objDH.queryData(mailSQL, dict);
                                            #endregion
                                            string EventName = "您已取得戒菸服務人員資格證明";
                                            string MailContent = "您好:<br/>您已取得「戒菸服務人員資格證明」，如欲下載電子證明，請登入醫事人員戒菸服務訓練系統(https://quitsmoking.hpa.gov.tw)  ，在【個人首頁】內下載。";
                                            SendMail(EventName, MailContent, objDT.Rows[i]["PMail"].ToString());
                                            #region 寄信給VPN
                                            //string VPNName = "\n" + "ttchpa@gmail.com" + ",";
                                            //string VPNEventName = "訓練系統領證通知-" + DateTime.Now.ToShortDateString();
                                            //string VPNsn = Email.CreateGroup(VPNEventName);
                                            //string VPNTitle = "Email,Name";
                                            //string VPNMailContent = "身分證:" + PersonID + "; 公告日:" + CertStartDate;
                                            ////JObject匿名物件
                                            //JObject VPNobj = new JObject(
                                            //     new JProperty("contacts", VPNTitle + VPNName)
                                            //    );
                                            ////序列化為JSON字串並輸出結果
                                            //var VPNresult = JsonConvert.SerializeObject(VPNobj, Formatting.Indented);
                                            //Email.InsertMemberString(VPNsn, VPNresult);
                                            //Dictionary<string, object> VPNdict = new Dictionary<string, object>();
                                            //string VPNmailSQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate()) SELECT @@IDENTITY AS 'Identity'";
                                            //VPNdict.Add("SN", VPNsn);
                                            //VPNdict.Add("EventName", VPNEventName);
                                            //VPNdict.Add("MailContent", VPNMailContent);
                                            //DataTable VPNdt = objDH.queryData(VPNmailSQL, VPNdict);
                                            #endregion
                                            string VPNEventName = "訓練系統領證通知-" + DateTime.Now.ToShortDateString();
                                            string VPNMailContent = "身分證:" + PersonID + "; 公告日:" + CertStartDate;
                                            SendMail(VPNEventName, VPNMailContent, "ttchpa@gmail.com");
                                        }
                                    }
                                }
                            }
                            else if (EPClassSNO == "9")
                            {
                                if (CheckDocCourse(PersonSNO))
                                {
                                    string CompulsorySNO = @"select CourseSNO from QS_Course QC left join QS_ECoursePlanningClass ECPC on QC.PClassSNO=ECPC.PClassSNO where Compulsory=1 and EPClassSNO=@EPClassSNO";
                                    Dictionary<string, object> cccDict = new Dictionary<string, object>();
                                    cccDict.Add("EPclassSNO", EPClassSNO);
                                    DataTable CCobjDT = objDH.queryData(CompulsorySNO, cccDict);
                                    string CourseSNO = CCobjDT.Rows[0]["CourseSNO"].ToString();
                                    string checkCompulsory = @"select 1 from qs_Integral where  PersonSNO=@PersonSNO and CourseSNO=@CourseSNO and IsUsed<>1";
                                    cccDict.Add("CourseSNO", CourseSNO);
                                    cccDict.Add("PersonSNO", PersonSNO);
                                    DataTable CompulsorobjDT = objDH.queryData(checkCompulsory, cccDict);
                                    if (CompulsorobjDT.Rows.Count > 0)
                                    {
                                        string OldCertificate = @"select  PersonID,CertPublicDate,CertStartDate,CertEndDate from qs_certificate where CTypeSNO in (1,2,53,54) and CertEndDate>getdate() and IsChange=0 and PersonID=@PersonID";
                                        Dictionary<string, object> oldDict = new Dictionary<string, object>();
                                        oldDict.Add("PersonID", PersonID);
                                        DataTable OldobjDT = objDH.queryData(OldCertificate, oldDict);
                                        DateTime CertPublicDateDT = Convert.ToDateTime(OldobjDT.Rows[0]["CertPublicDate"].ToString());
                                        string CertPublicDate = CertPublicDateDT.ToShortDateString();
                                        DateTime CertStartDateDT = Convert.ToDateTime(OldobjDT.Rows[0]["CertStartDate"].ToString());
                                        string CertStartDate = CertStartDateDT.ToShortDateString();
                                        string CertEndDate = OldobjDT.Rows[0]["CertEndDate"].ToString();
                                        int EndYear = DateTime.Parse(CertEndDate).Year;//取得原本證書到期年
                                        int NowYear = DateTime.Now.Year;
                                        string QIdate = @"select top 1 QI.CreateDT from QS_Integral QI
left join QS_Course QC on QI.CourseSNO=QC.CourseSNO
where personSNO=@personSNO and PClassSNO=@PClassSNO and isused<>1 order by QI.CreateDT desc";
                                        Dictionary<string, object> QIDict = new Dictionary<string, object>();
                                        QIDict.Add("PersonID", PersonID);
                                        QIDict.Add("PersonSNO", PersonSNO);
                                        QIDict.Add("PClassSNO", PClassSNO);
                                        QIDict.Add("EPClassSNO", EPClassSNO);
                                        DataTable QIobjDT = objDH.queryData(QIdate, QIDict);//線上最新日期
                                        string EQIdate = @"select top 1 QI.CDate from QS_EIntegral QI
where PersonID=@PersonID and EPClassSNO=@EPClassSNO and isused<>1  order by QI.CreateDT desc";
                                        DataTable EQIobjDT = objDH.queryData(EQIdate, QIDict);//實體最新日期
                                        string IntegralDate = "";
                                        if (QIobjDT.Rows.Count==0 && EQIobjDT.Rows.Count > 0)//線上沒有就用實體
                                        {
                                            IntegralDate = EQIobjDT.Rows[0]["CDate"].ToString();
                                        }
                                        else if (EQIobjDT.Rows.Count==0 && QIobjDT.Rows.Count > 0)//實體沒有就用線上
                                        {
                                            IntegralDate = QIobjDT.Rows[0]["CreateDT"].ToString();
                                        }
                                        else
                                        {
                                            DateTime QIDate = DateTime.Parse(QIobjDT.Rows[0]["CreateDT"].ToString());
                                            DateTime EQIDate = DateTime.Parse(EQIobjDT.Rows[0]["CDate"].ToString());
                                            if (QIDate > EQIDate)//線上大於實體用線上
                                            {
                                                IntegralDate = QIobjDT.Rows[0]["CreateDT"].ToString();
                                            }
                                            else
                                            {
                                                IntegralDate = EQIobjDT.Rows[0]["CDate"].ToString();
                                            }
                                        }
                                        if (EndYear == NowYear)//到期年=今年才換
                                        {
                                            int IntegralDateTime = DateTime.Parse(IntegralDate).Year;
                                            if (IntegralDateTime < EndYear)
                                            {
                                                CertStartDate = NowYear.ToString() + "/01/01";

                                            }
                                            else
                                            {
                                                CertStartDateDT = Convert.ToDateTime(IntegralDate);
                                                CertStartDate = CertStartDateDT.ToShortDateString();
                                            }
                                            CertEndDate = DateTime.Now.AddYears(6).ToString("yyyy/12/31");
                                            string InsertSQL = @"  insert into [New_QSMS].[dbo].QS_Certificate  ([PersonID],[CertID],[CTypeSNO],[CUnitSNO],[CertPublicDate],[CertStartDate],[CertEndDate],[CertExt],[IsPrint],[CreateDT],[CreateUserID],[SysChange],[IsChange],[Note] )
                values(@PersonID,@CertID,'75',@CUnitSNO,@CertPublicDate,@CertStartDate,@CertEndDate,@CertExt,@IsPrint,@CreateDT,@CreateUserID,@SysChange,@IsChange,@Note )";
                                            Dictionary<string, object> qcDict = new Dictionary<string, object>();
                                            qcDict.Add("PersonID", PersonID);
                                            qcDict.Add("CertID", "000000");
                                            qcDict.Add("CUnitSNO", 2);
                                            qcDict.Add("CertPublicDate", CertPublicDate);
                                            qcDict.Add("CertStartDate", CertStartDate);
                                            qcDict.Add("CertEndDate", CertEndDate);
                                            qcDict.Add("CertExt", 0);
                                            qcDict.Add("IsPrint", 0);
                                            qcDict.Add("CreateDT", DateTime.Now.ToShortDateString());
                                            qcDict.Add("CreateUserID", 0);
                                            qcDict.Add("SysChange", 1);
                                            qcDict.Add("IsChange", 0);
                                            qcDict.Add("Note", "已於" + DateTime.Now.ToString("yyyy/MM/dd") + "排程取得證明");
                                            objDH.executeNonQuery(InsertSQL, qcDict);

                                            #region 電子豹
                                            //string Name = "\n" + objDT.Rows[i]["PMail"].ToString() + ",";
                                            //string EventName = "證書換證-" + DateTime.Now.ToShortDateString();
                                            //string sn = Email.CreateGroup(EventName);
                                            //string Title = "Email,Name";
                                            //string MailContent = "您好:<br/>您已取得「戒菸服務人員資格證明」，如欲下載電子證明，請登入醫事人員戒菸服務訓練系統(https://quitsmoking.hpa.gov.tw)  ，在【個人首頁】內下載。";
                                            ////JObject匿名物件
                                            //JObject obj = new JObject(
                                            //     new JProperty("contacts", Title + Name)
                                            //    );
                                            ////序列化為JSON字串並輸出結果
                                            //var result = JsonConvert.SerializeObject(obj, Formatting.Indented);
                                            //Email.InsertMemberString(sn, result);
                                            //Dictionary<string, object> dict = new Dictionary<string, object>();
                                            //string mailSQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate()) SELECT @@IDENTITY AS 'Identity'";
                                            //dict.Add("SN", sn);
                                            //dict.Add("EventName", EventName);
                                            //dict.Add("MailContent", MailContent);
                                            //DataTable dt = objDH.queryData(mailSQL, dict);
                                            #endregion
                                            string EventName = "您已取得戒菸服務人員資格證明";
                                            string MailContent = "您好:<br/>您已取得「戒菸服務人員資格證明」，如欲下載電子證明，請登入醫事人員戒菸服務訓練系統(https://quitsmoking.hpa.gov.tw)  ，在【個人首頁】內下載。";
                                            SendMail(EventName, MailContent, objDT.Rows[i]["PMail"].ToString());
                                            #region 寄信給VPN
                                            //string VPNName = "\n" + "ttchpa@gmail.com" + ",";
                                            //string VPNEventName = "訓練系統領證通知-" + DateTime.Now.ToShortDateString();
                                            //string VPNsn = Email.CreateGroup(VPNEventName);
                                            //string VPNTitle = "Email,Name";
                                            //string VPNMailContent = "身分證:" + PersonID + "; 公告日:" + CertStartDate;
                                            ////JObject匿名物件
                                            //JObject VPNobj = new JObject(
                                            //     new JProperty("contacts", VPNTitle + VPNName)
                                            //    );
                                            ////序列化為JSON字串並輸出結果
                                            //var VPNresult = JsonConvert.SerializeObject(VPNobj, Formatting.Indented);
                                            //Email.InsertMemberString(VPNsn, VPNresult);
                                            //Dictionary<string, object> VPNdict = new Dictionary<string, object>();
                                            //string VPNmailSQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate()) SELECT @@IDENTITY AS 'Identity'";
                                            //VPNdict.Add("SN", VPNsn);
                                            //VPNdict.Add("EventName", VPNEventName);
                                            //VPNdict.Add("MailContent", VPNMailContent);
                                            //DataTable VPNdt = objDH.queryData(VPNmailSQL, VPNdict);
                                            #endregion
                                            string VPNEventName = "訓練系統領證通知-" + DateTime.Now.ToShortDateString();
                                            string VPNMailContent = "身分證:" + PersonID + "; 公告日:" + CertStartDate;
                                            SendMail(VPNEventName, VPNMailContent, "ttchpa@gmail.com");
                                        }
                                    }
                                }
                                else
                                {
                                    LogTime = DateTime.Now;
                                    ExcuteMsg = objDT.Rows[i]["PersonID"].ToString() + "ExChangeCertificate尚未取得必修";
                                    ResultType = 0;    //0:失敗, 1:成功
                                    ErrorMsg = "";
                                    WriteLog(LogTime, LogID, LogFunction, ExcuteType, ExcuteMsg, ResultType, SQLCmd, RunTime, ErrorMsg);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            LogTime = DateTime.Now;
                            ExcuteMsg = objDT.Rows[i]["PersonID"].ToString() + "ExChangeCertificate執行失敗";
                            ResultType = 0;    //0:失敗, 1:成功
                            ErrorMsg = ex.Message;
                            WriteLog(LogTime, LogID, LogFunction, ExcuteType, ExcuteMsg, ResultType, SQLCmd, RunTime, ErrorMsg);
                        }
                    }
                    LogTime = DateTime.Now;
                    ExcuteMsg = "ExChangeCertificate執行結束";
                    ResultType = 0;    //0:失敗, 1:成功
                    ErrorMsg = "";
                    WriteLog(LogTime, LogID, LogFunction, ExcuteType, ExcuteMsg, ResultType, SQLCmd, RunTime, ErrorMsg);
                }
                catch (Exception ex)
                {
                    LogTime = DateTime.Now;
                    ExcuteMsg = "ExChangeCertificate執行失敗";
                    ResultType = 0;    //0:失敗, 1:成功
                    ErrorMsg = ex.Message;
                    WriteLog(LogTime, LogID, LogFunction, ExcuteType, ExcuteMsg, ResultType, SQLCmd, RunTime, ErrorMsg);
                }
            }
        }
        public static bool CheckNewCertificate(string PersonID)
        {
            DataHelper ObjDH = new DataHelper();
            Dictionary<string, object> adict = new Dictionary<string, object>();
            string CheckCompilsorySql = @"select 1 from QS_Certificate QC 
where PersonID=@PersonID and QC.CTypeSNO=75 and QC.CertEndDate>GETDATE() and QC.IsChange<>1";
            adict.Add("PersonID", PersonID);
            DataTable ObjDT = ObjDH.queryData(CheckCompilsorySql, adict);
            if (ObjDT.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool CheckEfficientCertificate(string PersonID)
        {
            DataHelper ObjDH = new DataHelper();
            Dictionary<string, object> adict = new Dictionary<string, object>();
            string CheckCompilsorySql = @"select 1 from QS_Certificate QC 
where PersonID=@PersonID and QC.CTypeSNO not in(8,9,10,11,12,13,56,57,60) and QC.CertEndDate>GETDATE() and QC.IsChange<>1";
            adict.Add("PersonID", PersonID);
            DataTable ObjDT = ObjDH.queryData(CheckCompilsorySql, adict);
            if (ObjDT.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool CheckDocCourse(string PersonSNO)
        {
            DataHelper ObjDH = new DataHelper();
            Dictionary<string, object> adict = new Dictionary<string, object>();
            string CheckSpecIntegralSql = @"Select 1 from qs_integral where coursesno in(17,42) and isused=0 and personsno=@PersonSNO";
            adict.Add("PersonSNO", PersonSNO);
            DataTable ObjDT = ObjDH.queryData(CheckSpecIntegralSql, adict);
            if (ObjDT.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool CheckSpecIntegral(string PersonSNO, string PClassSNO)
        {
            DataHelper ObjDH = new DataHelper();
            Dictionary<string, object> adict = new Dictionary<string, object>();
            string CheckSpecIntegralSql = @"  with SpecIntegral as(SELECT P.PersonID,P.PName,P.RoleSNO,QI.PersonSNO,QCPC.PClassSNO,sum(QC.CHour)　專門課程
  FROM [New_QSMS].[dbo].[QS_Integral] QI
  left join [New_QSMS].[dbo].QS_Course QC on QI.CourseSNO=QC.CourseSNO
  left join [New_QSMS].[dbo].QS_CoursePlanningClass QCPC on QC.PClassSNO=QCPC.PClassSNO
  left join [New_QSMS].[dbo].Person P on QI.PersonSNO=P.PersonSNO
  where QCPC.PClassSNO=@PClassSNO　and QI.PersonSNO=@PersonSNO and QI.Isused<>1
  group by P.PersonID,P.PName,P.RoleSNO,QI.PersonSNO,QCPC.PClassSNO),
  TargetIntegral as(select PClassSNO,TargetIntegral from QS_CoursePlanningClass QCPC where QCPC.PClassSNO=@PClassSNO)
  select * from SpecIntegral 
  left join TargetIntegral on SpecIntegral.PClassSNO=TargetIntegral.PClassSNO
  where SpecIntegral.專門課程>=TargetIntegral.TargetIntegral";
            adict.Add("PersonSNO", PersonSNO);
            adict.Add("PClassSNO", PClassSNO);
            DataTable ObjDT = ObjDH.queryData(CheckSpecIntegralSql, adict);
            if (ObjDT.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void WriteLog(DateTime LogTime, int LogID, string LogFunction, int ExcuteType, string ExcuteMsg,
            int ResultType, string SQLCmd, int RunTime, string ErrorMsg)
        {
            DataHelper objDH = new DataHelper();
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Clear();
            aDict.Add("LogTime", LogTime);
            aDict.Add("LogID", LogID);
            aDict.Add("LogFunction", LogFunction);
            aDict.Add("ExcuteType", ExcuteType);
            aDict.Add("ExcuteMsg", ExcuteMsg);
            aDict.Add("ResultType", ResultType);
            aDict.Add("SQLCmd", SQLCmd);
            aDict.Add("RunTime", RunTime);
            aDict.Add("ErrorMsg", ErrorMsg.Replace("'", ""));
            string sql = @"Insert into AccessLog(LogTime,LogID,LogFunction,ExcuteType,ExcuteMsg,ResultType,SQLCmd,RunTime,ErrorMsg) 
            Values(@LogTime,@LogID,@LogFunction,@ExcuteType,@ExcuteMsg,@ResultType,@SQLCmd,@RunTime,@ErrorMsg)";
            objDH.executeNonQuery(sql, aDict);
        }
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
                (Select PVal From Config Where PID='Account1') Account,
                (Select PVal From Config Where PID='Host1') Host,
                (Select PVal From Config Where PID='Port1') Port,
                (Select PVal From Config Where PID='Psw1') Psw,
                (Select PVal From Config Where PID='SSL1') SSL
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
    }
}
