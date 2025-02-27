using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Services;
using System.Web.Services.Protocols;



/// <summary>
/// ElearningConnection 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
// [System.Web.Script.Services.ScriptService]
public class ElearningConnection : System.Web.Services.WebService
{

    public ElearningConnection()
    {

        //如果使用設計的元件，請取消註解下列一行
        //InitializeComponent(); 
    }
    public class Authentication2 : SoapHeader
    {
        public string Token { get; set; }

    }
    public Authentication2 authentication=new Authentication2();

    [SoapHeader("authentication")]
    [WebMethod]
    public string InsertRecord(string PersonID, string ELSCode, string ELSPart, string FinishedDate)
    {
        try
        {

            if (IsValidUser())
            {
                Dictionary<string, object> adict = new Dictionary<string, object>();
                DataHelper ObjDH = new DataHelper();
                string CourseSNO = Utility.GetCourseSNOWithELSCode(ELSCode);
                string SQL = @"If not Exists(select 1 From QS_LearningRecord Where PersonID=@PersonID and CourseSNO=@CourseSNO and ELSCode=@ELSCode and ELSPart=@ELSPart)
                        BEGIN
                           INSERT INTO [dbo].[QS_LearningRecord] ([PersonID],[CourseSNO],[ELSCode],[ELSPart],[FinishedDate],[CreateDT],[CreateUserID])
                            VALUES
                        	(@PersonID,@CourseSNO,@ELSCode,@ELSPart,@FinishedDate,getdate(),2)
                        End  ";
                adict.Add("PersonID", PersonID);
                adict.Add("CourseSNO", CourseSNO);
                adict.Add("ELSCode", ELSCode);
                adict.Add("ELSPart", ELSPart);
                adict.Add("FinishedDate", FinishedDate);
                ObjDH.executeNonQuery(SQL, adict);
                //Call 預存程序
                Utility.AutoAuditIntegral(PersonID, "InsertRecord");

            }
            else
            {
                return "ERROR";
            }


        }
        catch (Exception ex)
        {
            string ErrorMessage = ex.Message.ToString();
            return ErrorMessage;
        }
        //可以使用ELSCode查到CourseSNO
        return "Success!";
    }

    [SoapHeader("authentication")]
    [WebMethod]
    public string InsertScore(string PersonID, string ELCode, string ELSCode, string Quizze_id, string Quizze_section, string Grade_grade, string Grade_finished, string Quizze_passGrade,string Grade_passed)
    {
        try
        {

            if (IsValidUser())
            {
                Dictionary<string, object> adict = new Dictionary<string, object>();
                DataHelper ObjDH = new DataHelper();
                string CourseSNO = Utility.GetCourseSNOWithELSCode(ELSCode);
                string sql = @"
                                            If Not Exists(Select 1 From QS_LearningScore
                                                    Where PersonID=@PersonID And ELCode=@ELCode And ELSCode=@ELSCode And QuizCode=@QuizCode And ExamDate=@ExamDate)
                                                Insert into QS_LearningScore(PersonID,ELCode,ELSCode,QuizCode,QuizName,Score,ExamDate,PassScore,IsPass,CreateUserID) 
                                                Values(@PersonID,@ELCode,@ELSCode,@QuizCode,@QuizName,@Score,@ExamDate,@PassScore,@IsPass,@CreateUserID)
                                            Else
                                                Update QS_LearningScore set
                                                    QuizName=@QuizName, Score=@Score, PassScore=@PassScore, IsPass=@IsPass,
                                                    ModifyDT=@ModifyDT, ModifyUserID=@ModifyUserID
                                                Where PersonID=@PersonID And ELCode=@ELCode And ELSCode=@ELSCode And QuizCode=@QuizCode And ExamDate=@ExamDate
                                        ";
                adict.Add("PersonID", PersonID);
                adict.Add("ELCode", ELCode);
                adict.Add("ELSCode", ELSCode);
                adict.Add("QuizCode", Quizze_id);
                adict.Add("QuizName", Quizze_section);
                adict.Add("Score", Grade_grade);
                adict.Add("ExamDate", Grade_finished);
                adict.Add("PassScore", Quizze_passGrade);
                adict.Add("IsPass", Grade_passed);
                adict.Add("CreateUserID", "0");
                adict.Add("ModifyDT", DateTime.Now);
                adict.Add("ModifyUserID", "0");
                ObjDH.executeNonQuery(sql, adict);
                //Call 預存程序
                Utility.AutoAuditIntegral(PersonID, "InsertRecord");

            }

        }
        catch (Exception ex)
        {
            string ErrorMessage = ex.Message.ToString();
            return ErrorMessage;
        }
        //可以使用ELSCode查到CourseSNO
        return "Success!";
    }

    [SoapHeader("authentication")]
    [WebMethod]
    public string InsertFeedback(string PersonID, string ELSCode, string fid, string qid,string questionName)
    {
        try
        {

            if (IsValidUser())
            {
                Dictionary<string, object> adict = new Dictionary<string, object>();
                DataHelper ObjDH = new DataHelper();
                string sql = @"
                       If Exists(Select 1 From QS_LearningFeedback Where ELSCode=@ELSCode And FBID=@FBID And QID=@QID)
                                        Update QS_LearningFeedback Set 
                                            QName=@QName, ModifyDT=GetDate(), ModifyUserID=@ModifyUserID
                                        Where ELSCode=@ELSCode And FBID=@FBID And QID=@QID
                                    Else
                                        Insert into QS_LearningFeedback(ELSCode,FBID,QID,QName,CreateUserID) 
                                        Values(@ELSCode,@FBID,@QID,@QName,@CreateUserID)
                        ";

                adict.Add("ELSCode", ELSCode);
                adict.Add("FBID", fid);
                adict.Add("QID", qid);
                adict.Add("QName", questionName);
                adict.Add("CreateUserID", 0);
                adict.Add("ModifyDT", 0);
                adict.Add("ModifyUserID", 0);
                ObjDH.executeNonQuery(sql, adict);
                //Call 預存程序
                Utility.AutoAuditIntegral(PersonID, "InsertFeedback");

            }
            else
            {
                return "ERROR";
            }


        }
        catch (Exception ex)
        {
            string ErrorMessage = ex.Message.ToString();
            return ErrorMessage;
        }
        //可以使用ELSCode查到CourseSNO
        return "Success!";
    }

    public bool IsValidUser()
    {

        //這段依使用狀況，可以改讀資料庫的帳號密碼或Web.Conifg 等....
        if (authentication.Token == "369b4a473a61a465e47d0a9bd4300cd8d6ff83b8f348eed23aad73c9a86fa2dc")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
