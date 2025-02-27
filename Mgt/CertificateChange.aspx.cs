using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_CertificateChange : System.Web.UI.Page
{
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfo;
    public Dictionary<string, string> _SetCol;
    public UserInfo userInfo = null;
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            string PersonSNO = Request.QueryString["Psno"];
            string CtSNO = Request.QueryString["CtSNO"];
            string PClassSNO = Request.QueryString["PClassSNO"];
            string CtypeSNO= Utility.CtypeBinding(CtSNO);
            Baseicbind(gv_BasicData, PersonSNO);
            Scorebind(gv_CertificateStatus, PersonSNO, PClassSNO);
            bind(gv_SetValue, PersonSNO, PClassSNO, CtypeSNO);
            setC(CtSNO);
        }
    }
    protected void Scorebind(GridView gv, string Psno, string PClassSNO)
    {
        //string[] SortKey = Request.QueryString["Sort"].Split(',');
        string[] SortValues = Psno.Split(',');
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        Dictionary<int, string> Sort = new Dictionary<int, string>();
        for (int i = 0; i < SortValues.Length; i++)
        {
            Sort.Add(i, SortValues[i]);
        }
        string SQL = @"
                         with getsomething as(
                SELECT Distinct
                	I.PersonSNO
					,P.PersonID
					,P.PName
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
                    where 1=1 and I.isUsed <> 1
                  Group by QCPC.PlanName,QCT.CTypeName,QCPC.CStartYear,QCPC.CEndYear,QC.PClassSNO,I.PersonSNO,P.PersonID,P.PName
                  ),AnalyticsPair as (
                    Select QI.PersonSNO,QC.PClassSNO,QC.Ctype,QC.PairCourseSNO CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    Left Join getsomething gs On gs.PersonSNO=QI.PersonSNO
                    where  QC.PClassSNO=gs.PClassSNO
                    Group by QC.Ctype,QC.PairCourseSNO,QI.PersonSNO,QC.PClassSNO
                    INTERSECT 
                    Select QI.PersonSNO,QC.PClassSNO,QC.Ctype,QI.CourseSNO,SUM(QC.Chour)Pair from QS_Integral QI
                    Left Join QS_Course QC On QC.CourseSNO=QI.CourseSNO
                    Left Join getsomething gs On gs.PersonSNO=QI.PersonSNO
                    where  QC.PClassSNO=gs.PClassSNO
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
                  select getsomething.PName,getsomething.PersonSNO,getsomething.PersonID,PlanName,CStartYear,CEndYear,CTypeName,getsomething.PClassSNO,sum(PClassTotalHr)-isnull(GPC.Pair,0) PClassTotalHr ,sumHours from getsomething
                  left join getAllCourseHours gc on gc.PClassSNO=getsomething.PClassSNO
				  left Join getPairCourseSNO GPC On GPC.PersonSNO= getsomething.PersonSNO And GPC.PClassSNO=getsomething.PClassSNO                 
				  Group by getsomething.PersonSNO,PlanName,CStartYear,CEndYear,CTypeName,getsomething.PClassSNO,sumHours,GPC.Pair,getsomething.PersonID,getsomething.PName
				  )
				  Select
				   *,Cast(PClassTotalHr as nvarchar)+'/'+cast(sumHours as nvarchar) AllHour
				  ,cast(CStartYear as nvarchar)+'-'+Cast(CEndYear as nvarchar) CYear
				   from sumAll
				    where PersonSNO in (" + Psno + ") ";

        //SQL += " Where P.PersonSNO in (" + Psno + ")  ";
        aDict.Add("PersonSNO", Psno);
        //aDict.Add("PClassSNO", PClassSNO);
        DataTable ObjDT = ObjDH.queryData(SQL, aDict);
        System.Data.DataColumn newColumn = new System.Data.DataColumn("Sort", typeof(System.Int16));
        ObjDT.Columns.Add(newColumn);


        for (int i = 0; i < Sort.Count; i++)
        {
            for (int j = 0; j < ObjDT.Rows.Count; j++)
            {
                if (ObjDT.Rows[j]["PersonSNO"].ToString() == Sort[i].ToString())
                {
                    ObjDT.Rows[j][12] = i + 1;
                }
            }
        }
        //objDT.Columns.Add(newColumn);
        ObjDT.DefaultView.Sort = "Sort";
        gv.DataSource = ObjDT;
        gv.DataBind();
    }
    protected void bind(GridView gv, string Psno, string PClassSNO,string BindCtypeSNO)
    {
 
        //string[] SortKey = Request.QueryString["Sort"].Split(',');
        string[] SortValues = Psno.Split(',');
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        Dictionary<int, string> Sort = new Dictionary<int, string>();
        for (int i = 0; i < SortValues.Length; i++)
        {
            Sort.Add(i, SortValues[i]);
        }
        string SQL = @"
                          with getsomething as(
                SELECT 
                	I.PersonSNO
                	,QCPC.CStartYear
                    ,QC.Class1
                	,QCPC.CEndYear
                	,QCT.CTypeName
                	,QC.PClassSNO
                    ,P.PName
                    ,P.personID
					,QCU.CUnitName
					,QCT.CTypeSEQ
                    ,QCU.CUnitSNO
                	,sum(CHour) PClassTotalHr
                    ,QCT.CTypeSNO
                  FROM [QS_Integral] I
                  Left Join Person P on P.PersonSNO=I.PersonSNO
                  Left Join QS_Course QC on QC.CourseSNO=I.CourseSNO
                  Left Join QS_CoursePlanningClass QCPC on QCPC.PClassSNO=QC.PClassSNO
                  Left Join QS_CertificateType QCT on QCT.CTypeSNO=QCPC.CTypeSNO
                  Left Join QS_CertificateUnit QCU On QCU.CUnitSNO=QCT.CUnitSNO
                    where 1=1
                  Group by QCT.CTypeName,QCPC.CStartYear,QCPC.CEndYear,QC.PClassSNO,I.PersonSNO,QC.Class1,P.PName,P.personID,QCT.CTypeSEQ,QCU.CUnitName,QCU.CUnitSNO,QCT.CTypeSNO
                  )
                  , getAllCourseHours As (
                				Select  c.PClassSNO, cpc.[TargetIntegral] sumHours
                				From QS_CoursePlanningClass cpc
                					Left JOIN QS_Course c on c.PClassSNO=cpc.PClassSNO
                				Group By c.PClassSNO, cpc.[TargetIntegral]
                			)
                ,sumAll as(
                  select PersonSNO,CStartYear,CEndYear,CTypeName,getsomething.PClassSNO,sum(PClassTotalHr)PClassTotalHr,sumHours,PName,personID,CTypeSEQ,CUnitName,CUnitSNO,CTypeSNO 
				  from getsomething
                  left join getAllCourseHours gc on gc.PClassSNO=getsomething.PClassSNO
                  where PersonSNO in (" + Psno + ") and getsomething.Class1 <> 3    Group by PersonSNO,CStartYear,CEndYear,CTypeName,getsomething.PClassSNO,sumHours,PName,personID,CTypeSEQ,CUnitName,CUnitSNO,CTypeSNO  ) " +
                  "   Select distinct sA.PersonSNO,CTypeName,PName,personID,CTypeSEQ+1 CTypeSEQ ,CUnitName,CUnitSNO,CTypeSNO from sumAll sA    where PClassTotalHr >= sumHours    " +
                  " Group by CTypeName,PName,personID,CStartYear,CEndYear,PClassTotalHr,sumHours,CTypeSEQ,CUnitName,CUnitSNO,CTypeSNO,sA.PersonSNO  ";

        //SQL += " Where P.PersonSNO in (" + Psno + ")  ";
        aDict.Add("PersonSNO", Psno);
        //aDict.Add("PClassSNO", PClassSNO);
        DataTable ObjDT = ObjDH.queryData(SQL, aDict);
        System.Data.DataColumn newColumn = new System.Data.DataColumn("Sort", typeof(System.Int16));
        ObjDT.Columns.Add(newColumn);


        for (int i = 0; i < Sort.Count; i++)
        {
            for (int j = 0; j < ObjDT.Rows.Count; j++)
            {
                if (ObjDT.Rows[j]["PersonSNO"].ToString() == Sort[i].ToString())
                {
                    ObjDT.Rows[j][8] = i + 1;
                }
            }
        }
        //objDT.Columns.Add(newColumn);
        ObjDT.DefaultView.Sort = "Sort";
        gv.DataSource = ObjDT;
        gv.DataBind();
    }
    protected void Baseicbind(GridView gv, string Psno)
    {
        string CtSNO = Request.QueryString["CtSNO"];
        string[] SortValues = Psno.Split(',');
        Dictionary<int, string> Sort = new Dictionary<int, string>();
        for (int i = 0; i < SortValues.Length; i++)
        {
            Sort.Add(i, SortValues[i]);
        }
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        string SQL = @"with aa as(
                     Select P.PName,P.PersonID,QC.CertID,P.PMail,P.PTel,R.RoleName,P.PersonSNO, Case When QC.CertSNO is null then 'N' Else 'Y' END CertSNO,QC.CtypeSNO,QCT.CTypeName
                      from Person P
                        Left Join Role R On R.RoleSNO=P.RoleSNO
                        Left Join QS_Certificate QC On QC.PersonID=P.PersonID
                        Left Join QS_CertificateType QCT On QCT.CtypeSNO=QC.CtypeSNO
                    	  Where PersonSNO in (" + Psno + ")  )	  Select * from aa where 1=1 or CTypeSNO is null and CTypeSNO <> 8 and CTypeSNO <> 9 and CTypeSNO <> 12 ";

        switch (CtSNO)
        {
            case "51":
                SQL += " And aa.CtypeSNO=4 or aa.CtypeSNO=5";

                break;
            case "52":
                SQL += " And aa.CtypeSNO=6 or aa.CtypeSNO=7";
                break;
            case "53":
                SQL += " And aa.CtypeSNO=1";
                break;
            case "54":
                SQL += " And aa.CtypeSNO=2";
                break;
            case "55":
                SQL += " And aa.CtypeSNO=3";
                break;
            default:
                break;
        }
        DataTable ObjDT = ObjDH.queryData(SQL, aDict);





        System.Data.DataColumn newColumn = new System.Data.DataColumn("Sort", typeof(System.Int16));
        ObjDT.Columns.Add(newColumn);


        for (int i = 0; i < Sort.Count; i++)
        {
            string SortPersonSNO = Sort[i].ToString();
            for (int j = 0; j < ObjDT.Rows.Count; j++)
            {
                if (ObjDT.Rows[j]["PersonSNO"].ToString() == Sort[i].ToString())
                {
                    ObjDT.Rows[j][10] = i + 1;
                }
            }
        }
        //objDT.Columns.Add(newColumn);
        ObjDT.DefaultView.Sort = "Sort";
        gv.DataSource = ObjDT;
        gv.DataBind();
    }




    protected void gv_SetValue_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DropDownList ddl_AuditH;
        DropDownList ddl_AuditItem;
        if (e.Row.RowType == DataControlRowType.Header)
        {

            ddl_AuditH = (DropDownList)e.Row.FindControl("ddl_AuditH");
            Utility.setCertificateUnit(ddl_AuditH, userInfo.RoleGroup, "請選擇");
        }



        //要特別注意一下這邊，如果不用這個if包起來的話，RowDataBound會跑Header，Footer，Pager
        //我們的DropDownList是放在DataRow裡，所以只有在這邊才會找到DropDownList控制項
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ddl_AuditItem = (DropDownList)e.Row.FindControl("ddl_AuditItem");
            Utility.setCertificateUnit(ddl_AuditItem, userInfo.RoleGroup, "請選擇");
            //ddl_AuditH.Items.Add("2");
            //Utility.setCertificateUnit(ddl_AuditH, "請選擇");
        }


    }
    protected void gv_SetValue_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            //要隱藏的欄位，如果前台filed隱藏會抓不到值，所以在RowCreate時才隱藏

            //e.Row.Cells[2].Visible = false;
            //e.Row.Cells[3].Visible = false;
            //e.Row.Cells[4].Visible = false;
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;
            e.Row.Cells[10].Visible = false;
            e.Row.Cells[11].Visible = false;
            e.Row.Cells[13].Visible = false;
            //e.Row.Cells[14].Visible = false;
        }
    }

    protected void btn_OK_Click(object sender, EventArgs e)
    {
        string CertID = "";
        string PersonSNO = Request.QueryString["Psno"];
        string SEQ = lb_hidden.Text;
        string CtSNO = Request.QueryString["CtSNO"].ToString();
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        Dictionary<string, object> bdict = new Dictionary<string, object>();
        string OldCtypeSNO = ""; string Name = "";
        string CTypeName = Utility.ReturnCtypeName(CtSNO);
        string MailContent = "學員您好，您已通過「" + CTypeName + "」證書審核，如欲下載電子證書，請登入醫事人員戒菸服務訓練系統(https://quitsmoking.hpa.gov.tw) ，在【個人首頁】內下載。";
        //int CountNoneOldCertificate = 0;
        //foreach (GridViewRow grb in gv_BasicData.Rows)
        //{
        //    Name += "\n" + grb.Cells[5].Text + ",";
        //    Name += grb.Cells[1].Text;

        //    OldCtypeSNO += grb.Cells[7].Text.Replace("&nbsp;", "") + ",";
        //    if (grb.Cells[7].Text == "&nbsp;")
        //    {

        //        CountNoneOldCertificate++;
        //    }            
        //}
        //OldCtypeSNO = OldCtypeSNO.Substring(0, OldCtypeSNO.Length - 1);
        string[] OldCtypeSNOArray = OldCtypeSNO.Split(',');
        foreach (GridViewRow gr in gv_SetValue.Rows)
        {

            string InsertSQL = @"INSERT INTO [dbo].[QS_Certificate] ([PersonID],[CertID],
                        [CTypeSNO],[CUnitSNO],[CertPublicDate],[CertStartDate],[CertEndDate],[CertExt],[IsPrint] ,[CreateUserID],isChange,Syschange,Note
                        )
                        VALUES(@PersonID,@CertID,@CTypeSNO,@CUnitSNO,@CertPublicDate,@CertStartDate,@CertEndDate,@CertExt,@IsPrint,@CreateUserID,@isChange,@Syschange,@Note
                        )";
            string PersonID = gr.Cells[2].Text;
            string CTypeSNO = gr.Cells[11].Text;
            //string OldC = gr.Cells[13].Text.Replace("&nbsp;", "");
            string gr_PersonSNO = gr.Cells[13].Text;
            TextBox txt_CertID = (TextBox)gr.FindControl("txt_CertID");
            TextBox CertStartDate_1 = (TextBox)gr.FindControl("CertStartDate");
            TextBox CertPublicDate_1 = (TextBox)gr.FindControl("CertPublicDate");
            TextBox CertEndDate_1 = (TextBox)gr.FindControl("CertEndDate");
            DropDownList Unit = (DropDownList)gr.FindControl("ddl_AuditItem");
            string Cunit = Unit.SelectedValue;
            string CertStartDate = CertStartDate_1.Text;
            string CertPublicDate = CertPublicDate_1.Text;
            string CertEndDate = CertEndDate_1.Text;
            
            CertID = getStrInt(txt_CertID.Text);
            bdict.Add("PersonID", PersonID);
            bdict.Add("CertID", CertID);
            bdict.Add("CTypeSNO", CTypeSNO);
            bdict.Add("CUnitSNO", Cunit);
            bdict.Add("CertPublicDate", CertPublicDate);
            bdict.Add("CertStartDate", CertStartDate);
            bdict.Add("CertEndDate", CertEndDate);
            bdict.Add("CertExt", 0);
            bdict.Add("IsPrint", 0);
            bdict.Add("CreateUserID", userInfo.PersonSNO);
            bdict.Add("isChange", 0);
            bdict.Add("Syschange", 1);
            bdict.Add("Note", "已於" + DateTime.Now.ToShortDateString() + "取得證書");

            string[] PersonSNOURL = Request.QueryString["Psno"].Split(',');
            string[] PersonURLArray = PersonSNOToPersonID(PersonSNOURL);
            string CourseSNO = "";

            string UpdateSQL = @"Select * from QS_Integral QI
                                        Left Join QS_Course QC On QC.CourseSNO = QI.CourseSNO
                                        Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO = QC.PClassSNO
                                        where PersonSNO = '" + gr_PersonSNO + "' and QCPC.CTypeSNO = '" + CTypeSNO + "'";

            DataTable OBJUpdate = ObjDH.queryData(UpdateSQL, null);
            for (int j = 0; j < OBJUpdate.Rows.Count; j++)
            {
                CourseSNO += OBJUpdate.Rows[j]["CourseSNO"].ToString() + ",";

            }

            switch (CTypeSNO)
            {
                case "51":
                    string SQLCoreRole12 = @"Select * from QS_Integral QI
                                                Left Join QS_Course QC On QC.CourseSNO = QI.CourseSNO
                                                    where PClassSNO In (29,31,32,34)";
                    DataTable ObjCoreRole12 = ObjDH.queryData(SQLCoreRole12, null);
                    for (int j = 0; j < ObjCoreRole12.Rows.Count; j++)
                    {
                        CourseSNO += ObjCoreRole12.Rows[j]["CourseSNO"].ToString() + ",";

                    }
                    break;
                case "52":
                    string SQLCoreRole13 = @"Select * from QS_Integral QI
                                                Left Join QS_Course QC On QC.CourseSNO = QI.CourseSNO
                                                    where PClassSNO In (28,30,33,35)";
                    DataTable ObjCoreRole13 = ObjDH.queryData(SQLCoreRole13, null);
                    for (int j = 0; j < ObjCoreRole13.Rows.Count; j++)
                    {
                        CourseSNO += ObjCoreRole13.Rows[j]["CourseSNO"].ToString() + ",";

                    }
                    break;
            }
            CourseSNO = CourseSNO.Substring(0, CourseSNO.Length - 1);
            string UpdateIsUse = @"Update QS_Integral set isUsed=1 where PersonSNO='" + gr_PersonSNO + "' and CourseSNO in (" + CourseSNO + ")";
            DataTable objdta = ObjDH.queryData(UpdateIsUse, null);
            for (int x = 0; x < OldCtypeSNOArray.Length; x++)
            {
                if (OldCtypeSNOArray[x] == CtSNO || OldCtypeSNOArray[x]=="1")//暫時先這樣2021/5/10
                {
                    string CheckOldC = "Update QS_Certificate set IsChange=1,Note='已於" + DateTime.Now.ToShortDateString() + "取得新證書' where  CtypeSNO='" + OldCtypeSNOArray[x] + "' and PersonID='" + PersonURLArray[x] + "'";
                DataTable objCheckkOldC = ObjDH.queryData(CheckOldC, null);
            }
        }
            DataTable objdts = ObjDH.queryData(InsertSQL, bdict);
            bdict.Clear();

            //更新CtypeSEQ

            string UpdateSqr = @"Update [QS_CertificateType] set [CTypeSEQ]=CTypeSEQ+" + 1 + " where CtypeSNO='" + CtSNO + "'";
            DataTable objUpdateSqr = ObjDH.queryData(UpdateSqr, null);



        }
        string EventName = "證書領證-" + DateTime.Now.ToShortDateString();
        string sn = Email.CreateGroup(EventName);
        string Title = "Email,Name";
        //JObject匿名物件
        JObject obj = new JObject(
             new JProperty("contacts", Title + Name)
            );
        //序列化為JSON字串並輸出結果
        var result = JsonConvert.SerializeObject(obj, Formatting.Indented);
        Email.InsertMemberString(sn, result);
        Dictionary<string, object> dict = new Dictionary<string, object>();
        string SQL = "INSERT INTO [dbo].[EmailSeriesNumber]([SN],[EventName],[IsSend],MailContent,[CreateDT]) VALUES (@SN,@EventName,0,@MailContent,getdate()) SELECT @@IDENTITY AS 'Identity'";
        dict.Add("SN", sn);
        dict.Add("EventName", EventName);
        dict.Add("MailContent", MailContent);
        DataTable dt = ObjDH.queryData(SQL, dict);

        Response.Write("<script>alert('更新成功!');window.close();</script>");
    }
    public static string getStrInt(string msg)
    {
        msg = msg.Remove(0, 3);
        msg = Regex.Replace(msg, "[^0-9]", "");
        return msg;
    }

    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        //Response.Write("<script>window.close();</script>");
    }

    protected static string[] PersonSNOToPersonID(string[] PersonSNO)
    {
        DataHelper ObjDH = new DataHelper();
        string PersonID = "";
        for (int i = 0; i < PersonSNO.Length; i++)
        {

            string sql = "Select * from Person where PersonSNO='" + PersonSNO[i] + "'";
            DataTable ObjDT = ObjDH.queryData(sql, null);
            PersonID += ObjDT.Rows[0]["PersonID"].ToString() + ",";
        }
        PersonID = PersonID.Substring(0, PersonID.Length - 1);
        string[] PersonArray = PersonID.Split(',');
        return PersonArray;
    }
    protected void setC(string CtypeSNO)
    {

        DataHelper ObjDH = new DataHelper();
        string sql = @"SELECT CTypeString,CTypeSEQ
                       FROM [QS_CertificateType] where CtypeSNO='" + CtypeSNO + "'";
        DataTable ObjDT = ObjDH.queryData(sql, null);
        string CtypeString = ObjDT.Rows[0]["CTypeString"].ToString();
        int CtypeSEQ = Convert.ToInt32(ObjDT.Rows[0]["CTypeSEQ"]);
        Dictionary<string, object> adict = new Dictionary<string, object>();



        foreach (GridViewRow gr in gv_SetValue.Rows)
        {
            if (CtypeSNO == "")
            {
                return;
            }
            string PersonID = gr.Cells[2].Text;
            string OldCertIDSql = @"with aa as(
                                        Select * from QS_Certificate where PersonID=@PersonID  
                                        )
                                        select * from aa where 1=1 ";
            switch (CtypeSNO)
            {
                case "51":
                    OldCertIDSql += " And aa.CtypeSNO=4 or aa.CtypeSNO=5";
                    break;
                case "52":
                    OldCertIDSql += " And aa.CtypeSNO=6 or aa.CtypeSNO=7";
                    break;
                case "53":
                    OldCertIDSql += " And aa.CtypeSNO=1";
                    break;
                case "54":
                    OldCertIDSql += " And aa.CtypeSNO=2";
                    break;
                case "55":
                    OldCertIDSql += " And aa.CtypeSNO=3";
                    break;
                default:
                    OldCertIDSql += " And aa.CtypeSNO=@CtypeSNO";
                    break;
            }

            adict.Add("PersonID", PersonID);
            adict.Add("CtypeSNO", CtypeSNO);
            DataTable OnjDT = ObjDH.queryData(OldCertIDSql, adict);

            adict.Clear();
            TextBox tb1 = (TextBox)gr.FindControl("txt_CertID");
            TextBox CertPublicDate_1 = (TextBox)gr.FindControl("CertPublicDate");
            TextBox CertStartcDate_1 = (TextBox)gr.FindControl("CertStartDate");
            TextBox CertEndDate_1 = (TextBox)gr.FindControl("CertEndDate");
            if (OnjDT.Rows.Count > 0)
            {
                //有舊證書，證書過期               
                string OldCertID = OnjDT.Rows[0]["CertID"].ToString();
                string CertPublicDate = OnjDT.Rows[0]["CertPublicDate"].ToString();
                CertPublicDate_1.Text = CertPublicDate!=""? Convert.ToDateTime(CertPublicDate).ToShortDateString():"";
                CertStartcDate_1.Text = DateTime.Now.ToShortDateString();
                CertEndDate_1.Text = DateTime.Now.AddYears(6).ToString("yyyy/12/31");
                CertPublicDate_1.Enabled = false;
                tb1.Text = CtypeString.Replace("@", OldCertID);
                if (OldCertID.Length != 6)
                {
                    //有舊證書且證書字號大於六碼
                    string PrestoredNumber = OnjDT.Rows[0]["PrestoredNumber"].ToString();
                    PrestoredNumber = PrestoredNumber.PadLeft(6, '0');
                    tb1.Text = CtypeString.Replace("@", PrestoredNumber);
                }


            }
            else
            {
                //沒有舊證書
                CtypeSEQ += 1;

                string txt_CertID = CtypeSEQ.ToString();
                string txt_CertIDFor0 = txt_CertID.PadLeft(6, '0');
                CertStartcDate_1.Text = DateTime.Now.ToShortDateString();
                CertEndDate_1.Text = DateTime.Now.AddYears(6).ToString("yyyy/12/31");
                string StringCtypeSEQ = CtypeSEQ.ToString();
                string StringCtypeSEQFor0 = StringCtypeSEQ.PadLeft(6, '0');
                tb1.Text = CtypeString.Replace("@", StringCtypeSEQFor0);
                lb_hidden.Text = StringCtypeSEQFor0;
            }



        }
    }


    protected void gv_BasicData_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            //要隱藏的欄位，如果前台filed隱藏會抓不到值，所以在RowCreate時才隱藏

            e.Row.Cells[7].Visible = false;

        }
    }
    private void ReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("排序", "排序");
        _SetCol.Add("姓名", "姓名");
        _SetCol.Add("身分證", "身分證");
        _SetCol.Add("證書字號", "證書字號");
        _SetCol.Add("證書首發日", "證書首發日");
        _SetCol.Add("證書公告日", "證書公告日");
        _SetCol.Add("證書到期日", "證書到期日");
        _SetCol.Add("發證單位", "發證單位");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.CertificateChange.ToString()] = _ExcelInfo;
    }

    protected void btn_Export_Click(object sender, EventArgs e)
    {

        gv_SetValue.AllowPaging = false;    // here I'm disabling paging

        if (gv_SetValue.Rows.Count > 0)
        {
            var dtb = new DataTable();

            // Creating table headers
            dtb.Columns.Add("排序", typeof(string));
            dtb.Columns.Add("姓名", typeof(string));
            dtb.Columns.Add("身分證", typeof(string));            
            dtb.Columns.Add("證書字號", typeof(string));
            dtb.Columns.Add("證書首發日", typeof(string));
            dtb.Columns.Add("證書公告日", typeof(string));
            dtb.Columns.Add("證書到期日", typeof(string));
            dtb.Columns.Add("發證單位", typeof(string));

            // Adding rows content
            foreach (GridViewRow row in gv_SetValue.Rows)
            {
                TextBox txt_CertID = (TextBox)row.FindControl("txt_CertID");
                TextBox txt_Public = (TextBox)row.FindControl("CertPublicDate");
                TextBox txt_Start = (TextBox)row.FindControl("CertStartDate");
                TextBox txt_End = (TextBox)row.FindControl("CertEndDate");
                DropDownList ddl_Unit = (DropDownList)row.FindControl("ddl_AuditItem");
                var sort = row.Cells[0].Text;
                var name = row.Cells[1].Text;
                var personid = row.Cells[2].Text;                
                var certificate = txt_CertID.Text;
                var publicd = txt_Public.Text;
                var startd = txt_Start.Text;
                var endd = txt_End.Text;
                var unit = ddl_Unit.SelectedItem.Text;
                dtb.Rows.Add(sort, name, personid, certificate,publicd,startd,endd,unit);
            }
            ReportInit(dtb);
            Utility.OpenExportWindows(this, ReportEnum.CertificateChange.ToString());


        }
    }
}