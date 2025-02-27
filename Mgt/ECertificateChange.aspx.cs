using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_ECertificateChange : System.Web.UI.Page
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
            string EPClassSNO = Request.QueryString["Esno"];
            Baseicbind(gv_BasicData, PersonSNO, EPClassSNO);
            bind(gv_CertificateStatus, PersonSNO, EPClassSNO);
            //bind(gv_SetValue, PersonSNO, EPClassSNO);
        }
    }
    protected void bind(GridView gv, string Psno, string Esno)
    {
        string[] SortValues = Psno.Split(',');
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        Dictionary<int, string> Sort = new Dictionary<int, string>();
        for (int i = 0; i < SortValues.Length; i++)
        {
            Sort.Add(i, SortValues[i]);
        }
        string GetCtypeSql = @"Select * from [QS_ECoursePlanningClass] QEPC
                                Left Join QS_CertificateType QCT On QCT.CTypeSNO=QEPC.CTypeSNO where EPClassSNO='" + Esno + "'";
        DataTable ObjCtype = ObjDH.queryData(GetCtypeSql, null);
        string RoleSNO = ObjCtype.Rows[0]["RoleSNO"].ToString();
        string CtypeSNO = ObjCtype.Rows[0]["CtypeSNO"].ToString();
        string SQL = @"
                         Select QC.CertSNO, P.PName,P.PersonSNO,P.PersonID,P.PTel,P.PMail,
                         convert(varchar, QC.CertStartDate, 111) CertStartDate  ,
                         convert(varchar, QC.CertPublicDate, 111) CertPublicDate,
                         convert(varchar, QC.CertEndDate, 111) CertEndDate,QCT.CTypeName,QCU.CUnitName ,
                         Case when QC.CTypeSNO ='1' then
                          (select replace(QCT1.CTypeString,'@',RIGHT(REPLICATE('0', 6) + CAST(QC.CertID as NVARCHAR), 6) ) from QS_CertificateType QCT1 where CTypeSNO=53)
                         else
                          replace(QCT.CTypeString,'@',RIGHT(REPLICATE('0', 6) + CAST(QC.CertID as NVARCHAR), 6) ) 
                          End CTypeString,
                         QC.CertID,QC.CTypeSNO,QC.CUnitSNO,QC.CertSNO,isnull(QC.IsChange,0) IsChange,QC.PrestoredNumber
                                from Person P
                              Left Join QS_Certificate QC On QC.PersonID=P.PersonID
                              Left JOin QS_CertificateType QCT ON QCT.CTypeSNO=QC.CTypeSNO
                              LEFT JOIN QS_CertificateUnit QCU ON QCU.CUnitSNO=QC.CUnitSNO ";

        SQL += " Where P.PersonSNO in (" + Psno + ") and QCT.RoleSNO='" + RoleSNO + "'";
        if(CtypeSNO=="4" || CtypeSNO == "5" || CtypeSNO=="51")
        {
            SQL += " and (QCT.CtypeSNO='4' or QCT.CtypeSNO='5' or QCT.CtypeSNO='51') and IsChange=0 ";
        }
        else if(CtypeSNO == "6" || CtypeSNO == "7" || CtypeSNO=="52")
        {
            SQL += " and (QCT.CtypeSNO='6' or QCT.CtypeSNO='7' or QCT.CtypeSNO='52') and IsChange=0 ";
        }
        else if (CtypeSNO == "2" || CtypeSNO=="54")
        {
            SQL += " and (QCT.CtypeSNO='2' or QCT.CtypeSNO='54') and IsChange=0 ";
        }
        else if (CtypeSNO == "3"|| CtypeSNO=="55")
        {
            SQL += " and (QCT.CtypeSNO='3' or QCT.CtypeSNO='55') and IsChange=0 ";
        }
        else if (CtypeSNO == "1" || CtypeSNO=="53")
        {
            SQL += " and (QCT.CtypeSNO='1' or QCT.CtypeSNO='53') and IsChange=0 ";
        }
        else
        {
            SQL += " and QCT.CtypeSNO='" + CtypeSNO + "' ";
        }
       
        DataTable ObjDT = ObjDH.queryData(SQL, null);
        System.Data.DataColumn newColumn = new System.Data.DataColumn("Sort", typeof(System.Int16));
        ObjDT.Columns.Add(newColumn);
        for (int i = 0; i < Sort.Count; i++)
        {
            for (int j = 0; j < ObjDT.Rows.Count; j++)
            {
                if (ObjDT.Rows[j]["PersonSNO"].ToString() == Sort[i].ToString())
                {
                    ObjDT.Rows[j][18] = i + 1;
                }
            }
        }
        ObjDT.DefaultView.Sort = "Sort";
        gv.DataSource = ObjDT;
        gv.DataBind();
        if (CtypeSNO == "1")
        {
            
        }
        if (ddl_Type.SelectedValue == "1")
        {
            foreach (GridViewRow gr in gv_SetValue.Rows)
            {
                TextBox CertStartcDate_1 = (TextBox)gr.FindControl("txt_CertStartDate");
                CertStartcDate_1.Text = DateTime.Now.ToShortDateString();
                TextBox CertEndDate_1 = (TextBox)gr.FindControl("txt_CertEndDate");
                CertEndDate_1.Text = DateTime.Now.AddYears(6).ToString("yyyy/12/31");
                TextBox tb1 = (TextBox)gr.FindControl("txt_CtypeString");
                tb1.Text = ddl_CtypeName.SelectedItem.Text.Replace("@", gr.Cells[11].Text);

            }
        }
        else
        {
            foreach (GridViewRow gr in gv_SetValue.Rows)
            {
                TextBox CertStartcDate_1 = (TextBox)gr.FindControl("txt_CertStartDate");
                CertStartcDate_1.Text = DateTime.Now.ToShortDateString();
                TextBox CertEndDate_1 = (TextBox)gr.FindControl("txt_CertEndDate");
                CertEndDate_1.Text = DateTime.Now.AddYears(6).ToString("yyyy/12/31");

            }
        }
    }
    protected void Baseicbind(GridView gv, string Psno, string Esno)
    {
        string CtSNO = Request.QueryString["CtSNO"];
        string[] SortValues = Psno.Split(',');
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        Dictionary<int, string> Sort = new Dictionary<int, string>();
        for (int i = 0; i < SortValues.Length; i++)
        {
            Sort.Add(i, SortValues[i]);
        }
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

        DataTable ObjDT = ObjDH.queryData(SQL, null);
       System.Data.DataColumn newColumn = new System.Data.DataColumn("Sort", typeof(System.Int16));
        ObjDT.Columns.Add(newColumn);
        for (int i = 0; i < Sort.Count; i++)
        {
           
            for (int j = 0; j < ObjDT.Rows.Count; j++)
            {
                
                if (ObjDT.Rows[j]["PersonSNO"].ToString() == Sort[i].ToString())
                {
                    ObjDT.Rows[j][10] = i + 1;
                }
            }
        }
        ObjDT.DefaultView.Sort = "Sort";
        gv.DataSource = ObjDT;
        gv.DataBind();
 
    }
    protected void ddl_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        string PersonSNO = Request.QueryString["Psno"];
        string EPClassSNO = Request.QueryString["Esno"];
        if (ddl_Type.SelectedValue == "1")
        {

            ddl_CtypeName.Visible = true;
            Utility.setddl_CertificateType(ddl_CtypeName, "請選擇");
            btn_OK.Enabled = false;
            btn_OK.Style["background-color"] = "gray";

            gv_SetValue.DataSource = null;
            gv_SetValue.DataBind();
        }
        else
        {
    
            btn_OK.Enabled = false;
            btn_OK.Style["background-color"] = "#gray";
            ddl_CtypeName.Visible = false;
            bind(gv_SetValue, PersonSNO, EPClassSNO);

        }
    }

    protected void ddl_CtypeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string PersonSNO = Request.QueryString["Psno"];
        string EPClassSNO = Request.QueryString["Esno"];

        bind(gv_SetValue, PersonSNO, EPClassSNO);
        gv_ScoreUpload.DataSource = null;
        gv_ScoreUpload.DataBind();
        if (ddl_CtypeName.SelectedValue == "")
        {
            gv_SetValue.Visible = false;

        }


    }

    protected void gv_SetValue_RowDataBound(object sender, GridViewRowEventArgs e)
    {


    }
    protected void gv_SetValue_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            //要隱藏的欄位，如果前台filed隱藏會抓不到值，所以在RowCreate時才隱藏
            if (ddl_Type.SelectedValue == "0")
            {
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
            }
            else
            {
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[12].Visible = false;
            }

        }
    }

    protected void btn_OK_Click(object sender, EventArgs e)
    {
        string PersonSNO = Request.QueryString["Psno"];
        string SEQ = lb_hidden.Text;
        
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        Dictionary<string, object> bdict = new Dictionary<string, object>();
        if (ddl_Type.SelectedValue == "0")
        {
            string EPClassSNO = Request.QueryString["Esno"];
            //string CTypeName = Utility.EReturnCtypeName(EPClassSNO);
            string Name = "";
            string MailContent = "學員您好，您已通過證書審核，電子證書請至醫事人員戒菸服務訓練系統(https://quitsmoking.hpa.gov.tw) 登入個人帳號後下載。";

            foreach (GridViewRow grb in gv_BasicData.Rows)
            {
                Name += "\n" + grb.Cells[3].Text + ",";
                Name += grb.Cells[0].Text;
            }

            foreach (GridViewRow gr in gv_SetValue.Rows)
            {
                if (gr.Cells[3].Text != "1")
                {
                    TextBox CertStartDate = (TextBox)gr.FindControl("txt_CertStartDate");
                    TextBox CertEndDate = (TextBox)gr.FindControl("txt_CertEndDate");
                    if (Convert.ToDateTime(gr.Cells[7].Text) >= Convert.ToDateTime(CertStartDate.Text))
                    {
                        Response.Write("<script>alert('公告日不得小於或等於首發日!')</script>");
                        return;
                    }


                    string UpdateSQL = @"Update QS_Certificate set IsChange=0 , SysChange=1, CertStartDate=@CertStartDate , CertEndDate=@CertEndDate,
                                    IsChangeTime=@IsChangeTime,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID,Note=@Note,CertExt=1
                                    where CertSNO=@CertSNO";

                    string CertSNO = gr.Cells[10].Text;                    
                    bdict.Add("CertEndDate", CertEndDate.Text);
                    bdict.Add("CertSNO", CertSNO);
                    bdict.Add("CertStartDate", CertStartDate.Text);
                    bdict.Add("ModifyDT", DateTime.Now.ToShortDateString());
                    bdict.Add("ModifyUserID", userInfo.PersonSNO);
                    bdict.Add("IsChangeTime", DateTime.Now.ToShortDateString());
                    bdict.Add("Note", "已於" + DateTime.Now.ToShortDateString() + "展延");
                    DataTable objdts = ObjDH.queryData(UpdateSQL, bdict);


                    string[] EPClassSNOURL = Request.QueryString["Esno"].Split(',');
                    string PClassSNO = EPClassSNOToPClassSNO(EPClassSNOURL[0]);
                    string[] PersonSNOURL = Request.QueryString["Psno"].Split(',');
                    string[] PersonURLArray = PersonSNOToPersonID(PersonSNOURL);
                    for (int i = 0; i < PersonSNOURL.Length; i++)
                    {
                        string UpdateIsUse = @"Update QS_EIntegral set isUsed=1 where PersonID='" + PersonURLArray[i] + "' and EPClassSNO='" + EPClassSNOURL[0] + "'";
                        DataTable objdta = ObjDH.queryData(UpdateIsUse, null);
                        string UpdateIntegral = @"With UpdateIntegral as (
                    Select QI.*,QC.Class1,QC.PClassSNO from QS_Integral QI
                        Left Join QS_Course QC ON QC.CourseSNO=QI.CourseSNO
                    )
                    Update UpdateIntegral Set IsUsed=1 where Class1=3 and PersonSNO='" + PersonSNOURL[i] + "' and PClassSNO='" + PClassSNO + "' ";
                        DataTable objdtas = ObjDH.queryData(UpdateIntegral, null);
                    }

                    bdict.Clear();
                    
                }
                else
                {
                    TextBox CertStartDate = (TextBox)gr.FindControl("txt_CertStartDate");
                    TextBox CertEndDate = (TextBox)gr.FindControl("txt_CertEndDate");
                    if (Convert.ToDateTime(gr.Cells[7].Text) >= Convert.ToDateTime(CertStartDate.Text))
                    {
                        Response.Write("<script>alert('公告日不得小於或等於首發日!')</script>");
                        return;
                    }


                    string UpdateSQL = @"Update QS_Certificate set IsChange=0 , SysChange=1, CertStartDate=@CertStartDate , CertEndDate=@CertEndDate,
                                    IsChangeTime=@IsChangeTime,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID,Note=@Note,CertExt=1,CTypeSNO=53
                                    where CertSNO=@CertSNO";

                    string CertSNO = gr.Cells[10].Text;
                   
                    bdict.Add("CertEndDate", CertEndDate.Text);
                    bdict.Add("CertSNO", CertSNO);
                    bdict.Add("CertStartDate", CertStartDate.Text);
                    bdict.Add("ModifyDT", DateTime.Now.ToShortDateString());
                    bdict.Add("ModifyUserID", userInfo.PersonSNO);
                    bdict.Add("IsChangeTime", DateTime.Now.ToShortDateString());
                    bdict.Add("Note", "已於" + DateTime.Now.ToShortDateString() + "展延");
                    DataTable objdts = ObjDH.queryData(UpdateSQL, bdict);


                    string[] EPClassSNOURL = Request.QueryString["Esno"].Split(',');
                    string PClassSNO = EPClassSNOToPClassSNO(EPClassSNOURL[0]);
                    string[] PersonSNOURL = Request.QueryString["Psno"].Split(',');
                    string[] PersonURLArray = PersonSNOToPersonID(PersonSNOURL);
                    for (int i = 0; i < PersonSNOURL.Length; i++)
                    {
                        string UpdateIsUse = @"Update QS_EIntegral set isUsed=1 where PersonID='" + PersonURLArray[i] + "' and EPClassSNO='" + EPClassSNOURL[0] + "'";
                        DataTable objdta = ObjDH.queryData(UpdateIsUse, null);
                        string UpdateIntegral = @"With UpdateIntegral as (
                    Select QI.*,QC.Class1,QC.PClassSNO from QS_Integral QI
                        Left Join QS_Course QC ON QC.CourseSNO=QI.CourseSNO
                    )
                    Update UpdateIntegral Set IsUsed=1 where Class1=3 and PersonSNO='" + PersonSNOURL[i] + "' and PClassSNO='" + PClassSNO + "' ";
                        DataTable objdtas = ObjDH.queryData(UpdateIntegral, null);
                    }

                    bdict.Clear();
                   
                }
                


            }

            string EventName = "證書換證-" + DateTime.Now.ToShortDateString();
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
        else
        {
            string Name = "";
            string CTypeName= Utility.ReturnCtypeName(ddl_CtypeName.SelectedValue);
            string MailContent = "學員您好，您已通過「" + CTypeName + "」證書審核，如欲下載電子證書，請登入醫事人員戒菸服務訓練系統(https://quitsmoking.hpa.gov.tw) ，在【個人首頁】內下載。";
            foreach (GridViewRow grb in gv_BasicData.Rows)
            {
                Name += "\n" + grb.Cells[3].Text + ",";
                Name += grb.Cells[0].Text;
            }
            foreach (GridViewRow gr in gv_SetValue.Rows)
            {
                string NCert = "";
                string CtypeSNO = ddl_CtypeName.SelectedValue;
                string PersonID = gr.Cells[1].Text;
                string CertID = gr.Cells[6].Text;
                string PrestoredNumber = gr.Cells[11].Text;
                string CUnitSNO = GetCUnitSNO(CtypeSNO);
                if (CUnitSNO == "0")
                {
                    CUnitSNO= gr.Cells[12].Text;
                }
                TextBox Txt_Cert = (TextBox)gr.FindControl("txt_CtypeString");
                
                NCert = Regex.Replace(Txt_Cert.Text, "[^0-9]", ""); //這邊一定要有預存號碼，我是抓頁面的textbox

        
                string CertPublicDate = gr.Cells[7].Text;

                TextBox CertEndDateT = (TextBox)gr.FindControl("txt_CertEndDate");
                TextBox CertStartDateT = (TextBox)gr.FindControl("txt_CertStartDate");
                
                //新增證書
                string InsertSQL = @"
                 Insert QS_Certificate ([PersonID],[CertID],[CTypeSNO],[CUnitSNO],[CertPublicDate],[CertStartDate],[CertEndDate],[CertExt],[IsPrint]
                ,[IsChange],[SysChange],[CreateDT],[CreateUserID]) 
                 Values (@PersonID,@CertID,@CTypeSNO,@CUnitSNO,@CertPublicDate,@CertStartDate,@CertEndDate,@CertExt,@IsPrint,@IsChange,@SysChange,@CreateDT,@CreateUserID) ";
                adict.Add("PersonID", PersonID);
                adict.Add("CertID", NCert);
                adict.Add("CTypeSNO", CtypeSNO);
                adict.Add("CUnitSNO", CUnitSNO);
                adict.Add("CertPublicDate", CertPublicDate);
                adict.Add("CertStartDate", CertStartDateT.Text);
                adict.Add("CertEndDate", CertEndDateT.Text);
                adict.Add("CertExt", 0);
                adict.Add("SysChange", 1);
                adict.Add("IsPrint", 0);
                adict.Add("IsChange", 0);
                adict.Add("CreateDT", DateTime.Now.ToShortDateString());
                adict.Add("CreateUserID", userInfo.PersonSNO);

                DataTable objdt = ObjDH.queryData(InsertSQL, adict);
                adict.Clear();
                if (PrestoredNumber == "")
                {
                    //修改證書的序號
                    string UpdateSqr = @"Update [QS_CertificateType] set [CTypeSEQ]='" + NCert + "' where CtypeSNO='" + CtypeSNO + "'";
                    DataTable objUpdateSqr = ObjDH.queryData(UpdateSqr, null);
                }             
                //修改積分狀態
                string[] EPClassSNOURL = Request.QueryString["Esno"].Split(',');
                string PClassSNO = EPClassSNOToPClassSNO(EPClassSNOURL[0]);
                string[] PersonSNOURL = Request.QueryString["Psno"].Split(',');
                string[] PersonURLArray = PersonSNOToPersonID(PersonSNOURL);
                for (int i = 0; i < PersonSNOURL.Length; i++)
                {
                    string UpdateIsUse = @"Update QS_EIntegral set isUsed=1 where PersonID='" + PersonURLArray[i] + "' and EPClassSNO='" + EPClassSNOURL[0] + "'";
                    DataTable objdta = ObjDH.queryData(UpdateIsUse, null);
                    string UpdateIntegral = @"With UpdateIntegral as (
                    Select QI.*,QC.Class1,QC.PClassSNO from QS_Integral QI
                        Left Join QS_Course QC ON QC.CourseSNO=QI.CourseSNO
                    )
                    Update UpdateIntegral Set IsUsed=1 where Class1=3 and PersonSNO='" + PersonSNOURL[i] + "' and PClassSNO='" + PClassSNO + "'";
                    DataTable objdtas = ObjDH.queryData(UpdateIntegral, null);
                }
                HttpContext.Current.Response.Write("<script>window.opener.location.href = window.opener.location.href;</script>");
                Response.Write("<script>alert('送出成功!');window.close();</script>");


            }
            foreach (GridViewRow grs in gv_CertificateStatus.Rows)
            {
                ////將舊的證書更改狀況
                string UpdateSQL = @"Update QS_Certificate set IsChange=1,SysChange=1,Note=@Note,CertExt=@CertExt,IsChangeTime=@IsChangeTime,ModifyDT=@ModifyDT,ModifyUserID=@ModifyUserID
                                        where CertSNO=@CertSNO";
                string CertSNO = grs.Cells[0].Text;
                bdict.Add("IsChange", 1);
                bdict.Add("CertExt", 1);
                bdict.Add("Note", "已於" + DateTime.Now.ToShortDateString() + "換證");
                bdict.Add("IsChangeTime", DateTime.Now.ToShortDateString());
                bdict.Add("CertSNO", CertSNO);
                bdict.Add("ModifyDT", DateTime.Now.ToShortDateString());
                bdict.Add("ModifyUserID", userInfo.PersonSNO);
                DataTable objdts = ObjDH.queryData(UpdateSQL, bdict);
                bdict.Clear();
            }
            #region 電子豹
            string EventName = "證書換證-" + DateTime.Now.ToShortDateString();
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
            #endregion
        }

    }
    public static string getStrInt(string msg)
    {
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

 
    protected string RequestCheckPersonSNO(string PersonID, string[] PersonSNO)
    {
        Dictionary<string, object> dicpd = new Dictionary<string, object>();
        dicpd.Add("PersonID", PersonID);

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT PersonSNO FROM Person WHERE PersonID=@PersonID", dicpd);

        if (objDT.Rows.Count == 0)
        {
            return "-1"; //系統內沒有
        }
        if (objDT.Rows.Count > 0)
        {

            string CPersonSNO = objDT.Rows[0]["PersonSNO"].ToString();
            int CheckPersonSNO = Array.IndexOf(PersonSNO, CPersonSNO);
            if (CheckPersonSNO != -1 )
            {
                return "1"; //完全符合
            }
            else
            {
                return "0";
            }
        }

        return "0";
    }

    protected string GetCUnitSNO(string CtypeSNO)
    {
        string sql = @"SELECT *
                        FROM [QS_CertificateType] where CTypeSNO=@CtypeSNO";
        Dictionary<string, object> dicpd = new Dictionary<string, object>();
        dicpd.Add("CtypeSNO", CtypeSNO);

        DataHelper objDH = new DataHelper();
        DataTable ObjDT = objDH.queryData(sql, dicpd);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT.Rows[0]["CunitSNO"].ToString();
        }
        return "0";

    }

    public string EPClassSNOToPClassSNO(string EPClassSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> dicpd = new Dictionary<string, object>();
        string sql = "Select * from [QS_ECoursePlanningClass] where EPClassSNO=@EPClassSNO";
        dicpd.Add("EPClassSNO", EPClassSNO);
        DataTable ObjDT = objDH.queryData(sql, dicpd);
        if(ObjDT.Rows.Count > 0)
        {
            return ObjDT.Rows[0]["PClassSNO"].ToString();
        }
        else
        {
            return "";
        }
       
    }

    protected void gv_CertificateStatus_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Visible = false;
        }
    }

    protected void txt_CertStartDate_TextChanged(object sender, EventArgs e)
    {
       
    }

    private void ReportInit(DataTable dt)
    {
        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _SetCol.Add("姓名", "姓名");
        _SetCol.Add("身分證", "身分證");
        _SetCol.Add("證書字號", "證書字號");
        _SetCol.Add("證書公開日", "證書公開日");
        _SetCol.Add("證書首發日", "證書起始日");
        _SetCol.Add("證書到期日", "證書到期日");
        _ExcelInfo.Add(_SetCol, dt);
        Session[ReportEnum.EcertificateChange.ToString()] = _ExcelInfo;
    }

    

    protected void btn_Export_Click(object sender, EventArgs e)
    {
        if (ddl_Type.SelectedValue == "0")
        {
            gv_SetValue.AllowPaging = false;    // here I'm disabling paging

            if (gv_SetValue.Rows.Count > 0)
            {
                var dtb = new DataTable();

                // Creating table headers
               
                dtb.Columns.Add("姓名", typeof(string));
                dtb.Columns.Add("身分證", typeof(string));
                dtb.Columns.Add("證書字號", typeof(string));
                dtb.Columns.Add("證書公開日", typeof(string));
                dtb.Columns.Add("證書首發日", typeof(string));
                dtb.Columns.Add("證書到期日", typeof(string));
       

                // Adding rows content
                foreach (GridViewRow row in gv_SetValue.Rows)
                {
                    TextBox txt_CertID = (TextBox)row.FindControl("txt_CtypeString");
                    TextBox txt_Public = (TextBox)row.FindControl("txt_CertStartDate");
                    TextBox txt_Start = (TextBox)row.FindControl("txt_CertStartDate");
                    TextBox txt_End = (TextBox)row.FindControl("txt_CertEndDate");
                    DropDownList ddl_Unit = (DropDownList)row.FindControl("ddl_AuditItem");
                    
                    var name = row.Cells[0].Text;
                    var personid = row.Cells[1].Text;
                    var certificate = row.Cells[6].Text;
                    var publicd = row.Cells[7].Text;
                    var startd = txt_Start.Text;
                    var endd = txt_End.Text;
                    //var unit = ddl_Unit.SelectedItem.Text;
                    dtb.Rows.Add(name, personid, certificate, publicd, startd, endd);
                }
                ReportInit(dtb);
                Utility.OpenExportWindows(this, ReportEnum.EcertificateChange.ToString());
                // Writing the excel file
                //using (OfficeOpenXml.ExcelPackage pck = new ExcelPackage())
                //{
                //    ExcelWorksheet wsDt = pck.Workbook.Worksheets.Add("Historique");

                //    wsDt.Cells["A1"].LoadFromDataTable(dtb, true, TableStyles.Light20);
                //    wsDt.Cells.AutoFitColumns();
                //    pck.Save("123456");
                //    Response.BinaryWrite(pck.GetAsByteArray("123456"));
                //}

                //Response.Flush();
                //Response.End();

            }
            else
            {
                Utility.MessageBox.Show("請先選擇");
            }
        }
        else
        {
            gv_SetValue.AllowPaging = false;    // here I'm disabling paging

            if (gv_SetValue.Rows.Count > 0)
            {
                var dtb = new DataTable();

                dtb.Columns.Add("姓名", typeof(string));
                dtb.Columns.Add("身分證", typeof(string));
                dtb.Columns.Add("證書字號", typeof(string));
                dtb.Columns.Add("證書公開日", typeof(string));
                dtb.Columns.Add("證書首發日", typeof(string));
                dtb.Columns.Add("證書到期日", typeof(string));

                // Adding rows content
                foreach (GridViewRow row in gv_SetValue.Rows)
                {
                    TextBox txt_CertID = (TextBox)row.FindControl("txt_CtypeString");
                    TextBox txt_Public = (TextBox)row.FindControl("txt_CertStartDate");
                    TextBox txt_Start = (TextBox)row.FindControl("txt_CertStartDate");
                    TextBox txt_End = (TextBox)row.FindControl("txt_CertEndDate");
                    DropDownList ddl_Unit = (DropDownList)row.FindControl("ddl_AuditItem");

                    var name = row.Cells[0].Text;
                    var personid = row.Cells[1].Text;
                    var certificate = txt_CertID.Text;
                    var publicd = row.Cells[7].Text;
                    var startd = txt_Start.Text;
                    var endd = txt_End.Text;
                    //var unit = ddl_Unit.SelectedItem.Text;
                    dtb.Rows.Add(name, personid, certificate, publicd, startd, endd);
                }
                ReportInit(dtb);
                Utility.OpenExportWindows(this, ReportEnum.EcertificateChange.ToString());
                // Writing the excel file
                //using (OfficeOpenXml.ExcelPackage pck = new ExcelPackage())
                //{
                //    ExcelWorksheet wsDt = pck.Workbook.Worksheets.Add("Historique");

                //    wsDt.Cells["A1"].LoadFromDataTable(dtb, true, TableStyles.Light20);
                //    wsDt.Cells.AutoFitColumns();
                //    pck.Save("123456");
                //    Response.BinaryWrite(pck.GetAsByteArray("123456"));
                //}

                //Response.Flush();
                //Response.End();

            }
            else
            {
                Utility.MessageBox.Show("請先選擇");
            }
        }
        
    }
}