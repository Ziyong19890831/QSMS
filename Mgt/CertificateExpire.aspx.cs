using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_CertificateExpire : System.Web.UI.Page
{
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfo;
    public Dictionary<string, string> _SetCol;
    public UserInfo userInfo = null;
    int viewrole = 1;
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetDdlCertificateType(ddl_CType, "請選擇");
      
            if (userInfo.RoleSNO == "2")
            {
                Role_view.Visible = true;
                Tabs_2.Visible = true;
            }
            else if (userInfo.RoleSNO == "3")
            {
                Role_view.Visible = true;
                Tabs_2.Visible = true;
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    }

    private void ReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("ROW_NO", "序號");
        _SetCol.Add("PName", "姓名");
        _SetCol.Add("PersonID", "身分證字號");
        _SetCol.Add("CertStartDate", "發照日期");
        _SetCol.Add("CertEndDate", "到期日期");
        _SetCol.Add("CUnitName", "發證單位");
        _SetCol.Add("CTypeName", "證書類型名稱");
        _SetCol.Add("CTypeString", "證書字號");
        _SetCol.Add("PMail", "EMIAL");
        _SetCol.Add("PAddr", "通訊地址");
        _SetCol.Add("PAddr2", "戶籍地址");
        _SetCol.Add("PTel", "通訊電話");
        _SetCol.Add("PPhone", "手機");
        _SetCol.Add("OrganName", "機構名稱");
        _SetCol.Add("OrganTel", "機構聯絡電話");

        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.CertificateExpire.ToString()] = _ExcelInfo;
    }


    protected void bindData(int page)
    {
        page_Panel.Visible = true;
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 100;

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String sql = @"with ExchangeScore as (
   Select * from QS_EIntegral where isUsed=0
   ),GetEPCLassSNO as (
        Select EPClassSNO,PersonID,PersonSNO,PName,OrganSNO,P.RoleSNO from QS_ECoursePlanningClass QEPC
	Left JOin QS_CoursePlanningRole QCPR On QCPR.PClassSNO=QEPC.PClassSNO
	Left Join Role R On R.DocGroup=QCPR.RoleSNO
	Left Join Person P On P.RoleSNO=R.RoleSNO
	
   ),
      getC1 as(
     SELECT PersonID,EPClassSNO,sum(Integral)C1 
     FROM [QS_EIntegral]
     where   Ctype=1 and IsUsed=0
     Group by PersonID,EPClassSNO,ctype
     ),
      getC2 as(
     SELECT PersonID,EPClassSNO,sum(Integral)C2 
     FROM [QS_EIntegral]
     where Ctype=2 and IsUsed=0
     Group by PersonID,EPClassSNO,ctype
     ),
      getC3 as(
     SELECT PersonID,EPClassSNO,sum(Integral)C3 
     FROM [QS_EIntegral]
     where    Ctype=3 and IsUsed=0
     Group by PersonID,EPClassSNO,ctype
     ),
       getC4 as(
     SELECT PersonID,EPClassSNO,sum(Integral)C4 
     FROM [QS_EIntegral]
     where   Ctype=4 and IsUsed=0
     Group by PersonID,EPClassSNO,ctype
     ),
	 getElearningScore as (
	Select P.PName,QI.PersonSNO,P.PersonID,QEPC.EPClassSNO,QI.IsUsed,Case when Sum(QC.CHour)>Cast(QEPC.ElearnLimit as int) then QEPC.ElearnLimit Else Sum(QC.CHour)  END 學分,Sum(QC.Compulsory)必修堂數
		from QS_Integral QI
                    Left Join Person P On P.PersonSNO=QI.PersonSNO
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
					Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO=QC.PClassSNO
					Left Join QS_ECoursePlanningClass QEPC On QEPC.PClassSNO=QCPC.PClassSNO 
                    where QC.Class1=3 and IsUsed=0
			GROUP BY  P.PName,QI.PersonSNO,QC.CHour,QEPC.EPClassSNO,QI.IsUsed,P.PersonID,QEPC.ElearnLimit
		
	 ),
	 Checkcompulsory as (
			Select QEPC.EPClassSNO,QI.PersonSNO from QS_Integral QI
			Left Join QS_Course QC ON QC.CourseSNO=QI.CourseSNO
			Left Join QS_ECoursePlanningClass QEPC On QEPC.PClassSNO=QC.PClassSNO
			where  QI.IsUsed=0 
			Group by PersonSNO,QEPC.EPClassSNO
	 ),
	 getallEpclasscompulsory as (
	Select QEPC.EPClassSNO,Count(QC.Compulsory) countI
	 from QS_ECoursePlanningClass QEPC
    Left Join QS_Course QC On QC.PClassSNO=QEPC.PClassSNO
            Group by QEPC.EPClassSNO
	 )
	 ,Alldonecompulsory as(
	 Select Cc.EPClassSNO,Cc.PersonSNO,gec.countI from Checkcompulsory Cc
	 Left Join getallEpclasscompulsory gec On gec.EPClassSNO=Cc.EPClassSNO
	 
	 )
	 ,
	 countcompulsory as (
	 
	 Select distinct gEs.* from getElearningScore gEs
	 Left Join Alldonecompulsory Cc On Cc.PersonSNO=gEs.PersonSNO
	 where Cc.countI>=gEs.必修堂數 

	 )
	 ,
     getResult1 as(
     select distinct isnull(P.EPClassSNO,ES.EPClassSNO) EPClassSNO,
	   P.PersonSNO,P.PersonID,P.PName,isnull(gES.學分,0) ElearningScore,isnull(c1.C1,0) 線上學分,isnull(c2.C2,0) 實體學分,isnull(c3.C3,0) 實習學分,isnull(c4.C4,0) 通訊學分,p.OrganSNO,p.RoleSNO
	  from GetEPCLassSNO P     
	 left join countcompulsory gES On gES.PersonID=P.PersonID and gES.EPClassSNO=P.EPClassSNO
	 left join  ExchangeScore ES On ES.PersonID=P.PersonID  and ES.EPClassSNO=P.EPClassSNO  
     left join getC1 c1 On c1.PersonID=ES.PersonID and C1.EPClassSNO=P.EPClassSNO
     left Join getC2 c2 On c2.PersonID=ES.PersonID and C2.EPClassSNO=P.EPClassSNO
     left Join getc3 c3 On c3.PersonID=ES.PersonID and C3.EPClassSNO=P.EPClassSNO
     left Join getC4 c4 on c4.PersonID=ES.PersonID and C4.EPClassSNO=P.EPClassSNO 
	 
     ) 
	,
	 
	 getResult2 as(
     Select  QECP.PlanName,TotalIntegral,
	  gR.EPClassSNO,PersonSNO,PersonID,PName,
        Cast(sum(gR.實體學分+gR.實習學分+gR.通訊學分+gR.線上學分+gR.ElearningScore) as nvarchar )+'/'+ Cast(TotalIntegral as nvarchar) TotalIntegrals,
      gR.ElearningScore ElearningIntegral,
	  isnull(Cast(gR.實體學分 as nvarchar),0)+'/'+isnull(Cast(QECP.Compulsory_Entity as nvarchar),0) 實體學分,
	  isnull(Cast(gR.實習學分 as nvarchar),0)+'/'+ isnull(Cast(QECP.Compulsory_Practical as nvarchar),0) 實習學分,
	  isnull(Cast(gR.通訊學分 as nvarchar),0)+'/'+ isnull(Cast(QECP.Compulsory_Communication as nvarchar),0) 通訊學分,
	  isnull(Cast(gR.線上學分 as nvarchar),0)+'/'+ isnull(Cast(QECP.Compulsory_Online as nvarchar),0) 線上學分  ,
	  sum(gR.實體學分+gR.實習學分+gR.通訊學分+gR.線上學分+gR.ElearningScore) StudentScore
	  from getResult1 gR
     LEFT JOIN QS_ECoursePlanningClass QECP On QECP.EPClassSNO=gR.EPClassSNO
     LEFT JOIN Organ O ON O.OrganSNO = gR.OrganSNO
     LEFT JOIN Role R ON R.RoleSNO = gR.RoleSNO ";
        sql += Utility.setSQLAccess_ByRoleOrganType(aDict, userInfo);
        sql += @"
       where 1 = 1 And isnull(gR.線上學分,0) >= isnull(QECP.Compulsory_Online,0) and 
	  isnull(gR.實體學分,0) >= isnull(QECP.Compulsory_Entity,0) and 
	  isnull(gR.實習學分,0) >= isnull(QECP.Compulsory_Practical,0) and 
	  isnull(gR.通訊學分,0) >= isnull(QECP.Compulsory_Communication,0) 
	 
	 Group by gr.PersonSNO,QECP.PlanName, gR.EPClassSNO,PersonSNO,PersonID,PName,TotalIntegral,QECP.Compulsory_Entity,QECP.Compulsory_Practical,QECP.Compulsory_Communication,QECP.Compulsory_Online,gR.實體學分,gR.實習學分,gR.通訊學分,gR.線上學分,gR.ElearningScore
	  )
	  select ROW_NUMBER() over(order by getResult2.PersonSNO) Row_NO,ROW_NUMBER() over(order by getResult2.PersonSNO) as Sort,getResult2.*,STUFF ( getResult2.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption',getResult2.PersonID,QCT.CTypeName,QCT.CtypeSNO from getResult2
      Left Join QS_ECoursePlanningClass  QEPC On QEPC.EPClassSNO=getResult2.EPClassSNO
	  Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO=QEPC.PClassSNO
	  Left Join QS_CertificateType QCT On QCT.CTypeSNO=QCPC.CTypeSNO
	  where StudentScore >= getResult2.TotalIntegral 
                  ";


        #region 權限篩選區塊

        #endregion

        #region 查詢篩選區塊
        if (!String.IsNullOrEmpty(txt_PName.Text))
        {
            sql += " And getResult2.Pname like '%' + @txt_PName + '%' ";
            aDict.Add("txt_PName", txt_PName.Text);
        }
        if (!String.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " And getResult2.PersonID =@txt_PersonID ";
            aDict.Add("txt_PersonID", txt_PersonID.Text);
        }
        if (!String.IsNullOrEmpty(txt_EventName.Text))
        {
            sql += " And getResult2.PlanName Like '%' + @PlanName + '%'  ";
            aDict.Add("PlanName", txt_EventName.Text);
        }
        if (!String.IsNullOrEmpty(ddl_CType.SelectedValue))
        {
            sql += " And QCT.CTypeSNO = @CTypeSNO ";
            aDict.Add("CTypeSNO", ddl_CType.SelectedValue);
        }
        //if (!String.IsNullOrEmpty(txt_EndTime.Text))
        //{
        //    sql += " And gED.EndTime = @EndTime ";
        //    aDict.Add("EndTime", txt_EndTime.Text);
        //}
        //if (!String.IsNullOrEmpty(txt_CtypeName.Text))
        //{
        //    sql += " And QCT.CtypeName like '%' + @txt_CtypeName + '%' ";
        //    aDict.Add("txt_CtypeName", txt_CtypeName.Text);
        //}
        //if (!String.IsNullOrEmpty(ddl_datetime.SelectedValue))
        //{
        //    sql += " And CertEndDate between convert(datetime, getdate(), 120)-convert(datetime,@ddl_datetime) and convert(datetime, getdate(), 120) ";
        //    aDict.Add("ddl_datetime", Convert.ToInt16(ddl_datetime.SelectedValue));
        //}
        //sql += " Group by gr.PersonSNO,QECP.PlanName, gR.EPClassSNO,PersonSNO,PersonID,PName,TotalIntegral,QECP.Compulsory_Entity,QECP.Compulsory_Practical,QECP.Compulsory_Communication,QECP.Compulsory_Online,gR.實體學分,gR.實習學分,gR.通訊學分,gR.線上學分 ";

        //sql += " order by P.PersonSNO";
        #endregion
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Event.DataSource = objDT.DefaultView;
        gv_Event.DataBind();
        gv_NotInExcel.Visible = false;
        btnGrant.Enabled = true;
        btnGrant.Style["background-color"] = "#f9bf3b";
        //count.Text = objDT.Rows.Count.ToString();
        ReportInit(objDT);
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }


    protected void btnSendOwnsite_click(object sender, EventArgs e)
    {

        string TODOTITLE = "換證通知";
        string TODOTEXT = editor_Sys.InnerText;
        string postPersonSNO = userInfo.PersonSNO;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();

        string sql = @"Insert Into TODO(TODOTITLE,TODOTEXT,getPersonSNO,postPersonSNO,CreateDate,STATE)";
        sql += " SELECT @TODOTITLE AS TODOTITLE,@TODOTEXT AS TODOTEXT,P.PersonSNO,@postPersonSNO AS postPersonSNO,GETDATE() as CreateDate,@STATE AS STATE ";
        sql += " from Person P ";
        sql += " Left Join QS_Certificate QC On QC.PersonID=P.PersonID";
        sql += " Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO";
        sql += " LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO";
        sql += " LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO";
        sql += " where 1 = 1 And QC.CertStartDate is not null and QC.CertEndDate is not null ";
        sql += "";
        sql += "";
        aDict.Add("@TODOTITLE", TODOTITLE);
        aDict.Add("@TODOTEXT", TODOTEXT);
        aDict.Add("@postPersonSNO", postPersonSNO);
        aDict.Add("@STATE", 1);


        #region 查詢篩選區塊
        //if (!String.IsNullOrEmpty(txt_PName.Text))
        //{
        //    sql += " And getResult2.Pname like '%' + @txt_PName + '%' ";
        //    aDict.Add("txt_PName", txt_PName.Text);
        //}
        //if (!String.IsNullOrEmpty(txt_PersonID.Text))
        //{
        //    sql += " And getResult2.PersonID =@txt_PersonID ";
        //    aDict.Add("txt_PersonID", txt_PersonID.Text);
        //}
        //if (!String.IsNullOrEmpty(txt_EventName.Text))
        //{
        //    sql += " And E.EventName >= @EventName ";
        //    aDict.Add("EventName", txt_EventName.Text);
        //}
        //if (!String.IsNullOrEmpty(ddl_CType.SelectedValue))
        //{
        //    sql += " And QCT.CTypeSNO = @CTypeSNO ";
        //    aDict.Add("CTypeSNO", ddl_CType.SelectedValue);
        //}
        //if (!String.IsNullOrEmpty(txt_EndTime.Text))
        //{
        //    sql += " And E.EndTime = @EndTime ";
        //    aDict.Add("EndTime", txt_EndTime.Text);
        //}
        //if (!String.IsNullOrEmpty(txt_CtypeName.Text))
        //{
        //    sql += " And QCT.CtypeName like '%' + @txt_CtypeName + '%' ";
        //    aDict.Add("txt_CtypeName", txt_CtypeName.Text);
        //}
        //if (!String.IsNullOrEmpty(ddl_datetime.SelectedValue))
        //{
        //    sql += " And CertEndDate between convert(datetime, getdate(), 120)-convert(datetime,@ddl_datetime) and convert(datetime, getdate(), 120) ";
        //    aDict.Add("ddl_datetime", Convert.ToInt16(ddl_datetime.SelectedValue));
        //}
        //sql += " Group by gr.PersonSNO,QECP.PlanName, gR.EPClassSNO,PersonSNO,PersonID,PName,TotalIntegral,QECP.Compulsory_Entity,QECP.Compulsory_Practical,QECP.Compulsory_Communication,QECP.Compulsory_Online,gR.實體學分,gR.實習學分,gR.通訊學分,gR.線上學分 ";
        #endregion

        objDH.executeNonQuery(sql, aDict);
        Response.Write("<script>alert('送出成功!'); location.href='CertificateExpire.aspx';</script>");
    }

    protected void btnSendMail_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string SendmailTo = "";
        String sql = @"
           Select ROW_NUMBER() OVER (ORDER BY PersonSNO) as ROW_NO	
           ,P.PersonSNO
           ,P.PName
           ,P.PMail
           ,P.PersonID
           ,P.PAccount
           ,QC.CertStartDate
           ,QC.CertEndDate
           ,QCT.CTypeName
           ,QCT.CTypeString 
           from Person P
           Left Join QS_Certificate QC On QC.PersonID=P.PersonID
           Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
           LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
           LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
           where 1 = 1 And QC.CertStartDate is not null and QC.CertEndDate is not null 
        ";
        #region 權限篩選區塊
        Utility.setSQLAccess_ByRoleOrganType(aDict, userInfo);
        #endregion

        #region 查詢篩選區塊
        //if (!String.IsNullOrEmpty(txt_PName.Text))
        //{
        //    sql += " And getResult2.Pname like '%' + @txt_PName + '%' ";
        //    aDict.Add("txt_PName", txt_PName.Text);
        //}
        //if (!String.IsNullOrEmpty(txt_PersonID.Text))
        //{
        //    sql += " And getResult2.PersonID =@txt_PersonID ";
        //    aDict.Add("txt_PersonID", txt_PersonID.Text);
        //}
        //if (!String.IsNullOrEmpty(txt_EventName.Text))
        //{
        //    sql += " And E.EventName >= @EventName ";
        //    aDict.Add("EventName", txt_EventName.Text);
        //}
        //if (!String.IsNullOrEmpty(ddl_CType.SelectedValue))
        //{
        //    sql += " And QCT.CTypeSNO = @CTypeSNO ";
        //    aDict.Add("CTypeSNO", ddl_CType.SelectedValue);
        //}
        //if (!String.IsNullOrEmpty(txt_EndTime.Text))
        //{
        //    sql += " And E.EndTime = @EndTime ";
        //    aDict.Add("EndTime", txt_EndTime.Text);
        //}
        //if (!String.IsNullOrEmpty(txt_CtypeName.Text))
        //{
        //    sql += " And QCT.CtypeName like '%' + @txt_CtypeName + '%' ";
        //    aDict.Add("txt_CtypeName", txt_CtypeName.Text);
        //}
        //if (!String.IsNullOrEmpty(ddl_datetime.SelectedValue))
        //{
        //    sql += " And CertEndDate between convert(datetime, getdate(), 120)-convert(datetime,@ddl_datetime) and convert(datetime, getdate(), 120) ";
        //    aDict.Add("ddl_datetime", Convert.ToInt16(ddl_datetime.SelectedValue));
        //}
        //sql += " Group by gr.PersonSNO,QECP.PlanName, gR.EPClassSNO,PersonSNO,PersonID,PName,TotalIntegral,QECP.Compulsory_Entity,QECP.Compulsory_Practical,QECP.Compulsory_Communication,QECP.Compulsory_Online,gR.實體學分,gR.實習學分,gR.通訊學分,gR.線上學分 ";
        #endregion

        DataTable SendEmailList = objDH.queryData(sql, aDict);
        if (SendEmailList.Rows.Count == 0)
        {
            Response.Write("<script>alert('無換證人員'); location.href='CertificateExpire.aspx';</script>");
            return;
        }
        for (int i = 0; i < SendEmailList.Rows.Count; i++)
        {
            SendmailTo += SendEmailList.Rows[i]["PMail"].ToString() + ",";
        }
        SendmailTo = SendmailTo.Substring(0, SendmailTo.Length - 1);
        Utility.SendMail("換證通知", editor_Mail.InnerText, SendmailTo);
        Response.Write("<script>alert('送出成功!'); location.href='CertificateExpire.aspx';</script>");
    }

    protected void btnSendSMS_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string SendSmsTo = "";
        string querystring = Request.QueryString["sno"];

        String sql = @"
           Select ROW_NUMBER() OVER (ORDER BY PersonSNO) as ROW_NO	
           ,P.PersonSNO
           ,P.PName
           ,P.PMail
           ,P.PPhone
           ,P.PersonID
           ,P.PAccount
           ,QC.CertStartDate
           ,QC.CertEndDate
           ,QCT.CTypeName
           ,QCT.CTypeString 
           from Person P
           Left Join QS_Certificate QC On QC.PersonID=P.PersonID
           Left Join QS_CertificateType QCT On QCT.CTypeSNO=QC.CTypeSNO
           LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
           LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
           where 1 = 1 And QC.CertStartDate is not null and QC.CertEndDate is not null 
        ";
        #region 查詢篩選區塊
        //if (!String.IsNullOrEmpty(txt_PName.Text))
        //{
        //    sql += " And getResult2.Pname like '%' + @txt_PName + '%' ";
        //    aDict.Add("txt_PName", txt_PName.Text);
        //}
        //if (!String.IsNullOrEmpty(txt_PersonID.Text))
        //{
        //    sql += " And getResult2.PersonID =@txt_PersonID ";
        //    aDict.Add("txt_PersonID", txt_PersonID.Text);
        //}
        //if (!String.IsNullOrEmpty(txt_EventName.Text))
        //{
        //    sql += " And E.EventName >= @EventName ";
        //    aDict.Add("EventName", txt_EventName.Text);
        //}
        //if (!String.IsNullOrEmpty(ddl_CType.SelectedValue))
        //{
        //    sql += " And QCT.CTypeSNO = @CTypeSNO ";
        //    aDict.Add("CTypeSNO", ddl_CType.SelectedValue);
        //}
        //if (!String.IsNullOrEmpty(txt_CtypeName.Text))
        //{
        //    sql += " And QCT.CtypeName like '%' + @txt_CtypeName + '%' ";
        //    aDict.Add("txt_CtypeName", txt_CtypeName.Text);
        //}
        //if (!String.IsNullOrEmpty(ddl_datetime.SelectedValue))
        //{
        //    sql += " And CertEndDate between convert(datetime, getdate(), 120)-convert(datetime,@ddl_datetime) and convert(datetime, getdate(), 120) ";
        //    aDict.Add("ddl_datetime", Convert.ToInt16(ddl_datetime.SelectedValue));
        //}
        //sql += " Group by gr.PersonSNO,QECP.PlanName, gR.EPClassSNO,PersonSNO,PersonID,PName,TotalIntegral,QECP.Compulsory_Entity,QECP.Compulsory_Practical,QECP.Compulsory_Communication,QECP.Compulsory_Online,gR.實體學分,gR.實習學分,gR.通訊學分,gR.線上學分 ";
        #endregion
        DataTable SendSMSList = objDH.queryData(sql, aDict);
        if (SendSMSList.Rows.Count == 0)
        {
            Response.Write("<script>alert('無換證人員!'); location.href='Event.aspx';</script>");
            return;
        }

        for (int i = 0; i < SendSMSList.Rows.Count; i++)
        {
            SendSmsTo += SendSMSList.Rows[i]["PPhone"].ToString() + ",";
        }
        SendSmsTo = SendSmsTo.Substring(0, SendSmsTo.Length - 1);

        
        string SMStempFile = Server.MapPath("\\SMSTemp\\SendSMSList.txt");
        if (!File.Exists(SMStempFile))
        {
            using (StreamWriter streamWriter = new StreamWriter(SMStempFile, true, Encoding.UTF8))
            {
                string[] Array_SendSms = SendSmsTo.Split(',');
                for (int i = 0; i < Array_SendSms.Length; i++)
                {
                    streamWriter.WriteLine("[" + 100 + i + "]");
                    streamWriter.WriteLine("dstaddr=" + Array_SendSms[i] + "");
                    streamWriter.WriteLine("smbody=" + txt_SMS.Text);
                }


            }
        }

        //Utility.sendSMS(SMStempFile);
        Response.Write("<script>alert('維護中!'); location.href='CertificateExpire.aspx';</script>");
    }

    protected void gv_Event_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        var custid = e.CommandArgument;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (gv_Event.Rows.Count == 0)
        {
            Response.Write("<script>alert('尚未查詢')</script>");
            return;
        }
        Utility.OpenExportWindows(this, ReportEnum.CertificateExpire.ToString());
    }

    protected void btnGrant_Click(object sender, EventArgs e)
    {

        int i;
        string PersonSNO = "";
        string EPclassSNO = "";
        string Sort = "";
        string CtypeSNO = "";
        for (i = 0; i < this.gv_Event.Rows.Count; i++)
        {
            if (((CheckBox)gv_Event.Rows[i].FindControl("CheckBox")).Checked)
            {

                PersonSNO += gv_Event.Rows[i].Cells[2].Text + ",";
                EPclassSNO += gv_Event.Rows[i].Cells[3].Text + ",";
                CtypeSNO+= gv_Event.Rows[i].Cells[4].Text + ",";
                Sort += gv_Event.Rows[i].Cells[1].Text + ",";
            }
        }
        if (PersonSNO.Length == 0)
        {
            Response.Write("<script>alert('您未勾選人員')</script>");
            return;
        }
        PersonSNO = PersonSNO.Substring(0, PersonSNO.Length - 1);
        EPclassSNO = EPclassSNO.Substring(0, EPclassSNO.Length - 1);
        string[] CheckEPClass = EPclassSNO.Split(',');
        for (int j = 0; j < CheckEPClass.Length; j++)
        {

            if (CheckEPClass[0] != CheckEPClass[j])
            {
                Response.Write("<script>alert('請勾選同一批證書進行更新')</script>");
                return;
            }
            else
            {

            }
        }

        Response.Write("<script>var w = screen.width; window.open('ECertificateChange.aspx?Psno="+ PersonSNO + "&CtSNO="+ CtypeSNO [0]+ "&Esno=" + EPclassSNO[0] +"&Sort="+ Sort + "','','width=w,height=500');</script>");
        //GetProduct();
    }

    protected void gv_Event_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            //要隱藏的欄位，如果前台filed隱藏會抓不到值，所以在RowCreate時才隱藏
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[4].Visible = false;
        }
    }
    public static void SetDdlCertificateType(DropDownList ddl, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select A.CTypeSNO , A.CTypeName  FROM QS_CertificateType A", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        page_Panel.Visible = false;
        string errorMessage = "";

        string PersonID = "";
        if ((file_Upload != null) && (file_Upload.PostedFile.ContentLength > 0) && !string.IsNullOrEmpty(file_Upload.FileName))
        {
            string extension = Path.GetExtension(file_Upload.FileName).ToLowerInvariant();
            List<string> allowedExtextsion = new List<string> { ".xlsx", ".xls" };
            if (allowedExtextsion.IndexOf(extension) == -1) errorMessage += "請上傳 (xlxs,xls) 類型檔案";
            if (!String.IsNullOrEmpty(errorMessage))
            {
                Utility.showMessage(Page, "ErrorMessage", errorMessage);
                return;
            }

            List<OrderItem> orderItem = new List<OrderItem>();
            List<string> ExcelList = new List<string>();
            List<string> InExcelList = new List<string>();
            List<string> SQLList = new List<string>();
            Dictionary<int, string> Sort = new Dictionary<int, string>();
            string fileName = file_Upload.FileName;

            using (var package = new ExcelPackage(file_Upload.PostedFile.InputStream))
            {

                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                int rowid = 0;
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    rowid += 1;
                    OrderItem item = new OrderItem();
                    try
                    {
                        item.PersonID = workSheet.Cells[rowIterator, 2].Text;
                        PersonID += "'" + item.PersonID + "'" + ",";
                        Sort.Add(rowid, item.PersonID);
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        orderItem.Add(item);
                    }
                }
                foreach (OrderItem item in orderItem)
                {
                    ExcelList.Add(item.PersonID);
                }
                lb_PerrsonID.Text = PersonID;
                if (lb_PerrsonID.Text != "")
                {
                    lb_PerrsonID.Text = PersonID.Substring(0, PersonID.Length - 1);
                }
                else
                {
                    Response.Write("<script>alert('EXCEL上傳內容，身分證不得為空。')</script>");
                    return;
                }
         
             
            }

            Dictionary<string, object> aDict = new Dictionary<string, object>();
            String sql = @"
                    with ExchangeScore as (
   Select * from QS_EIntegral where isUsed=0
   ),GetEPCLassSNO as (
     Select EPClassSNO,PersonID,PersonSNO,PName,OrganSNO,P.RoleSNO from QS_ECoursePlanningClass QEPC
	Left JOin QS_CoursePlanningRole QCPR On QCPR.PClassSNO=QEPC.PClassSNO
	Left Join Person P On P.RoleSNO=QCPR.RoleSNO
	
   ),
      getC1 as(
     SELECT PersonID,EPClassSNO,sum(Integral)C1 
     FROM [QS_EIntegral]
     where   Ctype=1 and IsUsed=0
     Group by PersonID,EPClassSNO,ctype
     ),
      getC2 as(
     SELECT PersonID,EPClassSNO,sum(Integral)C2 
     FROM [QS_EIntegral]
     where Ctype=2 and IsUsed=0
     Group by PersonID,EPClassSNO,ctype
     ),
      getC3 as(
     SELECT PersonID,EPClassSNO,sum(Integral)C3 
     FROM [QS_EIntegral]
     where    Ctype=3 and IsUsed=0
     Group by PersonID,EPClassSNO,ctype
     ),
       getC4 as(
     SELECT PersonID,EPClassSNO,sum(Integral)C4 
     FROM [QS_EIntegral]
     where   Ctype=4 and IsUsed=0
     Group by PersonID,EPClassSNO,ctype
     ),
	 getElearningScore as (
	Select P.PName,QI.PersonSNO,P.PersonID,QEPC.EPClassSNO,QI.IsUsed,Case when Sum(QC.CHour)>Cast(QEPC.ElearnLimit as int) then QEPC.ElearnLimit Else Sum(QC.CHour)  END 學分,Sum(QC.Compulsory)必修堂數
		from QS_Integral QI
                    Left Join Person P On P.PersonSNO=QI.PersonSNO
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
					Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO=QC.PClassSNO
					Left Join QS_ECoursePlanningClass QEPC On QEPC.PClassSNO=QCPC.PClassSNO 
                    where QC.Class1=3 and IsUsed=0
			GROUP BY  P.PName,QI.PersonSNO,QC.CHour,QEPC.EPClassSNO,QI.IsUsed,P.PersonID,QEPC.ElearnLimit
		
	 ),
	 Checkcompulsory as (
			Select QEPC.EPClassSNO,QI.PersonSNO from QS_Integral QI
			Left Join QS_Course QC ON QC.CourseSNO=QI.CourseSNO
			Left Join QS_ECoursePlanningClass QEPC On QEPC.PClassSNO=QC.PClassSNO
			where  QI.IsUsed=0 
			Group by PersonSNO,QEPC.EPClassSNO
	 ),
	 getallEpclasscompulsory as (
	Select QEPC.EPClassSNO,Count(QC.Compulsory) countI
	 from QS_ECoursePlanningClass QEPC
    Left Join QS_Course QC On QC.PClassSNO=QEPC.PClassSNO
            Group by QEPC.EPClassSNO
	 )
	 ,Alldonecompulsory as(
	 Select Cc.EPClassSNO,Cc.PersonSNO,gec.countI from Checkcompulsory Cc
	 Left Join getallEpclasscompulsory gec On gec.EPClassSNO=Cc.EPClassSNO
	 
	 )
	 ,
	 countcompulsory as (
	 
	 Select distinct gEs.* from getElearningScore gEs
	 Left Join Alldonecompulsory Cc On Cc.PersonSNO=gEs.PersonSNO
	 where Cc.countI>=gEs.必修堂數 

	 )
	 ,
     getResult1 as(
     select distinct isnull(P.EPClassSNO,ES.EPClassSNO) EPClassSNO,
	   P.PersonSNO,P.PersonID,P.PName,isnull(gES.學分,0) ElearningScore,isnull(c1.C1,0) 線上學分,isnull(c2.C2,0) 實體學分,isnull(c3.C3,0) 實習學分,isnull(c4.C4,0) 通訊學分,p.OrganSNO,p.RoleSNO
	  from GetEPCLassSNO P     
	 left join countcompulsory gES On gES.PersonID=P.PersonID and gES.EPClassSNO=P.EPClassSNO
	 left join  ExchangeScore ES On ES.PersonID=P.PersonID  and ES.EPClassSNO=P.EPClassSNO  
     left join getC1 c1 On c1.PersonID=ES.PersonID and C1.EPClassSNO=P.EPClassSNO
     left Join getC2 c2 On c2.PersonID=ES.PersonID and C2.EPClassSNO=P.EPClassSNO
     left Join getc3 c3 On c3.PersonID=ES.PersonID and C3.EPClassSNO=P.EPClassSNO
     left Join getC4 c4 on c4.PersonID=ES.PersonID and C4.EPClassSNO=P.EPClassSNO 
	 
     ) 
	,
	 
	 getResult2 as(
     Select  QECP.PlanName,TotalIntegral,
	  gR.EPClassSNO,PersonSNO,PersonID,PName,
        Cast(sum(gR.實體學分+gR.實習學分+gR.通訊學分+gR.線上學分+gR.ElearningScore) as nvarchar )+'/'+ Cast(TotalIntegral as nvarchar) TotalIntegrals,
      gR.ElearningScore ElearningIntegral,
	  isnull(Cast(gR.實體學分 as nvarchar),0)+'/'+isnull(Cast(QECP.Compulsory_Entity as nvarchar),0) 實體學分,
	  isnull(Cast(gR.實習學分 as nvarchar),0)+'/'+ isnull(Cast(QECP.Compulsory_Practical as nvarchar),0) 實習學分,
	  isnull(Cast(gR.通訊學分 as nvarchar),0)+'/'+ isnull(Cast(QECP.Compulsory_Communication as nvarchar),0) 通訊學分,
	  isnull(Cast(gR.線上學分 as nvarchar),0)+'/'+ isnull(Cast(QECP.Compulsory_Online as nvarchar),0) 線上學分  ,
	  sum(gR.實體學分+gR.實習學分+gR.通訊學分+gR.線上學分+gR.ElearningScore) StudentScore
	  from getResult1 gR
     LEFT JOIN QS_ECoursePlanningClass QECP On QECP.EPClassSNO=gR.EPClassSNO
     LEFT JOIN Organ O ON O.OrganSNO = gR.OrganSNO
     LEFT JOIN Role R ON R.RoleSNO = gR.RoleSNO  ";
            sql += Utility.setSQLAccess_ByRoleOrganType(aDict, userInfo);
            sql += @"
                    where 1 = 1 And isnull(gR.線上學分,0) >= isnull(QECP.Compulsory_Online,0) and 
	                  isnull(gR.實體學分,0) >= isnull(QECP.Compulsory_Entity,0) and 
	                  isnull(gR.實習學分,0) >= isnull(QECP.Compulsory_Practical,0) and 
	                  isnull(gR.通訊學分,0) >= isnull(QECP.Compulsory_Communication,0) 
	                 
	                 Group by gr.PersonSNO,QECP.PlanName, gR.EPClassSNO,PersonSNO,PersonID,PName,TotalIntegral,QECP.Compulsory_Entity,QECP.Compulsory_Practical,QECP.Compulsory_Communication,QECP.Compulsory_Online,gR.實體學分,gR.實習學分,gR.通訊學分,gR.線上學分,gR.ElearningScore
	                  )
	                  select ROW_NUMBER() over(order by getResult2.PersonSNO) Row_NO,getResult2.*,STUFF ( getResult2.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption',getResult2.PersonID,QCT.CTypeName,QCT.CtypeSNO from getResult2
                   Left Join QS_ECoursePlanningClass  QEPC On QEPC.EPClassSNO=getResult2.EPClassSNO
	                  Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO=QEPC.PClassSNO
	                  Left Join QS_CertificateType QCT On QCT.CTypeSNO=QCPC.CTypeSNO
	                  where StudentScore >= getResult2.TotalIntegral and getResult2.PersonID In (" + lb_PerrsonID.Text+ " )         ";
            DataHelper objDH = new DataHelper();
            DataTable objDT = objDH.queryData(sql, aDict);
            System.Data.DataColumn newColumn = new System.Data.DataColumn("Sort", typeof(System.Int32));
            objDT.Columns.Add(newColumn);


            for (int i = 1; i <= Sort.Count; i++)
            {
                for (int j = 0; j < objDT.Rows.Count; j++)
                {
                    if (objDT.Rows[j]["PersonID"].ToString() == Sort[i].ToString())
                    {
                        objDT.Rows[j][18] = i;
                    }
                }
            }
            //objDT.Columns.Add(newColumn);
            objDT.DefaultView.Sort = "Sort";
            var dv = objDT.DefaultView;
            gv_Event.DataSource = objDT.DefaultView;
            gv_Event.DataBind();
            if (objDT.Rows.Count > 0)
            {
                lb_NoOne.Visible = false;
                for (int i = 0; i < objDT.Rows.Count; i++)
                {
                    SQLList.Add(objDT.Rows[i]["PersonID"].ToString());
                }
            }

   

            List<ForEXCEL> NotInEXCELListData = new List<ForEXCEL>();
           
            foreach (string aitem in ExcelList)
            {
                ForEXCEL notInEXCELListData = new ForEXCEL();
                if (!SQLList.Contains(aitem) && aitem !="")
                {
                    notInEXCELListData.PersonID = aitem;
                    notInEXCELListData.stutas = "換證所需積分名單內無此人";
                    NotInEXCELListData.Add(notInEXCELListData);

                }

            }
            if (NotInEXCELListData.Count > 0)
            {
                ProblemList.Visible = true;
                gv_NotInExcel.Visible = true;
                bindDataSource(NotInEXCELListData);
                btnGrant.Enabled = false;
                btnGrant.Style["background-color"] = "gray";

            }
            else
            {
                ProblemList.Visible = false;
                gv_NotInExcel.Visible = false;
                btnGrant.Enabled = true;
                btnGrant.Style["background-color"] = "#f9bf3b";
            }
        }
        else
        {
            Response.Write("<script>alert('請上傳EXCEL檔案')</script>");
            return;
        }
    }
    protected void bindDataSource(List<ForEXCEL> dataSource)
    {
        gv_NotInExcel.DataSource = dataSource;
        gv_NotInExcel.DataBind();

    }
    public class ForEXCEL
    {
        public string PersonID { set; get; }
        public string stutas { set; get; }
    }
    public class OrderItem
    {
        public string PersonID { set; get; }
        public string stutas { set; get; }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        String filePath = Directory.GetCurrentDirectory() + @"/SysFile/換證上傳.xlsx";
        FileInfo file = new FileInfo(filePath);
        if (file.Exists)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "text/plain";
            Response.Flush();
            Response.TransmitFile(file.FullName);
            Response.End();
        }
    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        bindData(page);
    }
}