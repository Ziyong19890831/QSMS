using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_SendMail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {            
            //Utility.SetDdlCertificateType(ddl_TargetCertificate, "請選擇");
            string ExChange = Request.QueryString["ExChange"] != null ? Request.QueryString["ExChange"].ToString() : "";
            if (ExChange == "")
            {
                Bind();
            }
            else
            {
                BindEX();
            }
            
        }
    }

    public void Bind()
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string EventSNO = Request.QueryString["sno"].ToString() == null ? "" : Request.QueryString["sno"].ToString();
        //bool CheckSend = Utility.CheckSend(EventSNO);
        //if (!CheckSend)
        //{
        //    Response.Write("<script>alert('不得重覆寄送!');window.close();</script>");
        //    return;
        //}
        if (!string.IsNullOrEmpty(EventSNO))
        {
            string sql = @"SELECT ROW_NUMBER() OVER (ORDER BY E.CreateDT DESC) as ROW_NO,E.SYSTEM_ID,
                E.EventSNO,EC.ClassName,E.EventName,E.StartTime,E.EndTime,
                E.CPerosn,E.CountLimit,G.Mval as Class1,F.Mval as Class2,
                (Select count(1) From EventD ed Where ed.EventSNO=E.EventSNO) pCount,C.id,E.PClassSNO
				
            FROM Event E
                LEFT JOIN EventClass EC on E.EventCSNO=EC.EventCSNO
                LEFT JOIN SYSTEM S on E.SYSTEM_ID=S.SYSTEM_ID
                Left Join Config G On E.class3=G.Pval and G.PGroup='CourseClass3'
                Left Join Config F On E.class4=F.Pval and F.PGroup='CourseClass4'
				LEFT JOIN Person P on P.PersonSNO = E.CreateUserID
				LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
				LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
                LEFT JOIN Calendar C On C.EventSNO=E.EventSNO
            WHERE 1=1 And G.PVal <> 2 and E.SYSTEM_ID <> 'S22' And E.EventSNO=@EventSNO";
            adict.Add("EventSNO", EventSNO);
            DataTable ObjDT = ObjDH.queryData(sql, adict);
            string NClass1 = ObjDT.Rows[0]["Class1"].ToString();
            string NClass2 = ObjDT.Rows[0]["Class2"].ToString();
            string NTitle = ObjDT.Rows[0]["EventName"].ToString();
            string NSign_F = ObjDT.Rows[0]["StartTime"].ToString();
            string NSign_E = ObjDT.Rows[0]["EndTime"].ToString();
            string Nsno = ObjDT.Rows[0]["EventSNO"].ToString();
            string Npsno = ObjDT.Rows[0]["PClassSNO"].ToString();
            editor_Mail.Value = "學員您好，目前開放報名課程有" + NClass1 + NClass2 + "「" + NTitle + "」報名時間為「" + NSign_F + "~" + NSign_E + "」，請視需求把握時間報名。活動連結：https://quitsmoking.hpa.gov.tw/Web/Event_AE.aspx?sno=" + Nsno + "&psno=" + Npsno + "";
            hf_Title.Value = NTitle;
            hf_EventSNO.Value = EventSNO;
        }

    }

    public void BindEX()
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        string EventSNO = Request.QueryString["sno"].ToString() == null ? "" : Request.QueryString["sno"].ToString();
        //bool CheckSend = Utility.CheckSend(EventSNO);
        //if (!CheckSend)
        //{
        //    Response.Write("<script>alert('不得重覆寄送!');window.close();</script>");
        //    return;
        //}
        if (!string.IsNullOrEmpty(EventSNO))
        {
            string sql = @"SELECT ROW_NUMBER() OVER (ORDER BY E.CreateDT DESC) as ROW_NO,E.SYSTEM_ID,
                E.EventSNO,EC.ClassName,E.EventName,E.StartTime,E.EndTime,
                E.CPerosn,E.CountLimit,G.Mval as Class1,F.Mval as Class2,
                (Select count(1) From EventD ed Where ed.EventSNO=E.EventSNO) pCount,C.id,E.PClassSNO
				
            FROM Event E
                LEFT JOIN EventClass EC on E.EventCSNO=EC.EventCSNO
                LEFT JOIN SYSTEM S on E.SYSTEM_ID=S.SYSTEM_ID
                Left Join Config G On E.class3=G.Pval and G.PGroup='CourseClass3'
                Left Join Config F On E.class4=F.Pval and F.PGroup='CourseClass4'
				LEFT JOIN Person P on P.PersonSNO = E.CreateUserID
				LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
				LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
                LEFT JOIN Calendar C On C.EventSNO=E.EventSNO
            WHERE 1=1 And G.PVal <> 1  And E.EventSNO=@EventSNO";
            adict.Add("EventSNO", EventSNO);
            DataTable ObjDT = ObjDH.queryData(sql, adict);
            string NClass1 = ObjDT.Rows[0]["Class1"].ToString();
            string NClass2 = ObjDT.Rows[0]["Class2"].ToString();
            string NTitle = ObjDT.Rows[0]["EventName"].ToString();
            string NSign_F = ObjDT.Rows[0]["StartTime"].ToString();
            string NSign_E = ObjDT.Rows[0]["EndTime"].ToString();
            string Nsno = ObjDT.Rows[0]["EventSNO"].ToString();
            string Npsno = ObjDT.Rows[0]["PClassSNO"].ToString();
            editor_Mail.Value = "學員您好，目前開放報名課程有" + NClass1 + NClass2 + "「" + NTitle + "」報名時間為「" + NSign_F + "~" + NSign_E + "」，請視需求把握時間報名。活動連結：https://quitsmoking.hpa.gov.tw/Web/Event_AE.aspx?sno=" + Nsno + "";
            hf_Title.Value = NTitle;
            hf_EventSNO.Value = EventSNO;
        }

    }

    protected async void btnSendMail_Click(object sender, EventArgs e)
    {
        string ExChange = Request.QueryString["ExChange"] != null ? Request.QueryString["ExChange"].ToString() : "";
        string EventSNO = Request.QueryString["sno"].ToString() == null ? "" : Request.QueryString["sno"].ToString();
        if (ExChange == "")
        {
            DataHelper ObjDH = new DataHelper();
            //string CtypeSNO = Utility.CGroupToCtypeSNO(ddl_TargetCertificate.SelectedValue);
            //string RoleSNO = Utility.RoleGroup(hf_EventSNO.Value);
            #region 電子豹-建立群組並取得sn
            //string EventSNO = Request.QueryString["sno"].ToString() == null ? "" : Request.QueryString["sno"].ToString();
            string EventName = hf_Title.Value + "-" + DateTime.Now.ToShortDateString();
            string sn = Email.CreateGroup(EventName);
            #endregion
            //如果選擇核心證書，則撈有初階證書之人員即可
            #region 取得活動人數並且匯入
            string Name = ""; string RoleSQL = "";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            //if (ddl_TargetCertificate.SelectedValue == "5" || ddl_TargetCertificate.SelectedValue == "6")
            //{
            //    RoleSQL = @"With CheckRole as (
            //            Select  P.PMail,P.PName,RoleSNO,P.PersonID,QC.CTypeSNO from Person P
            //                LEFT Join QS_Certificate QC On QC.PersonID=P.PersonID and CTypeSNO=56 	where P.RoleSNO In(" + RoleSNO + ") 	) 	Select * from CheckRole where CTypeSNO is null";
            //}
            //else
            //{
            //    RoleSQL = @"With CheckRole as (
            //            Select  P.PMail,P.PName,RoleSNO,P.PersonID,QC.CTypeSNO from Person P
            //                LEFT Join QS_Certificate QC On QC.PersonID=P.PersonID and CTypeSNO In (" + CtypeSNO + ") 	where P.RoleSNO In(" + RoleSNO + ") 	) 	Select * from CheckRole where CTypeSNO is null";
            //}             
            DataTable EmailDT = ObjDH.queryData(RoleSQL, dict);
            for (int i = 0; i < EmailDT.Rows.Count; i++)
            {
                Name += "\n" + EmailDT.Rows[i]["PMail"].ToString() + ",";
                Name += EmailDT.Rows[i]["PName"].ToString();
            }

            string Title = "Email,Name";
            //JObject匿名物件
            JObject obj = new JObject(
                 new JProperty("contacts", Title + Name)
                );
            //序列化為JSON字串並輸出結果
            var result = JsonConvert.SerializeObject(obj, Formatting.Indented);
            await Email.InsertMember(sn, result);
            Dictionary<string, object> adict = new Dictionary<string, object>();
            string SQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate()) SELECT @@IDENTITY AS 'Identity'";
            adict.Add("SN", sn);
            adict.Add("EventName", EventName);
            adict.Add("MailContent", editor_Mail.Value);
            DataTable dt = ObjDH.queryData(SQL, adict);
            string ESNO = dt.Rows[0]["Identity"].ToString();
            //Utility.UpdateEventSend(ESNO, EventSNO);
            Response.Write("<script>alert('寄送成功!');</script>");
            Response.Write("<script>window.close();</script>");
            #endregion
        }
        else
        {
            DataHelper ObjDH = new DataHelper();
            //string CtypeSNO = Utility.CGroupToCtypeSNO(ddl_TargetCertificate.SelectedValue);
            //string RoleSNO = Utility.RoleGroup(hf_EventSNO.Value);
            #region 電子豹-建立群組並取得sn
            //string EventSNO = Request.QueryString["sno"].ToString() == null ? "" : Request.QueryString["sno"].ToString();
            string EventName = hf_Title.Value + "-" + DateTime.Now.ToShortDateString();
            string sn = Email.CreateGroup(EventName);
            #endregion
            #region 取得活動人數並且匯入
            string Name = "";

            Dictionary<string, object> dict = new Dictionary<string, object>();
            string RoleSQL = "";
            //string RoleSQL = @"Select  P.PMail,P.PName,RoleSNO,P.PersonID,QC.CTypeSNO from Person P
            //        LEFT Join QS_Certificate QC On QC.PersonID=P.PersonID
            //  where P.RoleSNO  In(" + RoleSNO + ")  and QC.CTypeSNO In (" + CtypeSNO + ") and QC.CertEndDate < DATEADD(year,2,GETDATE()) and QC.CertEndDate > GETDATE()";
            DataTable EmailDT = ObjDH.queryData(RoleSQL, dict);
            for (int i = 0; i < EmailDT.Rows.Count; i++)
            {
                Name += "\n" + EmailDT.Rows[i]["PMail"].ToString() + ",";
                Name += EmailDT.Rows[i]["PName"].ToString();
            }

            string Title = "Email,Name";
            //JObject匿名物件
            JObject obj = new JObject(
                 new JProperty("contacts", Title + Name)
                );
            //序列化為JSON字串並輸出結果
            var result = JsonConvert.SerializeObject(obj, Formatting.Indented);
            await Email.InsertMember(sn, result);
            Dictionary<string, object> adict = new Dictionary<string, object>();
            string SQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate()) SELECT @@IDENTITY AS 'Identity'";
            adict.Add("SN", sn);
            adict.Add("EventName", EventName);
            adict.Add("MailContent", editor_Mail.Value);
            DataTable dt = ObjDH.queryData(SQL, adict);
            string ESNO = dt.Rows[0]["Identity"].ToString();
            //Utility.UpdateEventSend(ESNO, EventSNO);
            Response.Write("<script>alert('寄送成功!');</script>");
            Response.Write("<script>window.close();</script>");
            #endregion
        }

    }
}