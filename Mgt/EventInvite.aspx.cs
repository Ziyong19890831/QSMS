using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_EventInvite : System.Web.UI.Page
{


    UserInfo userInfo = null;
    int viewrole = 1;
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void btn_Insert_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> adict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        string EventSNO = Request.QueryString["sno"];
        //判斷是否為繼續教育或證書
        string CheckEXorElse = @"Select * from Event where EventSNO=@EventSNO ";
        adict.Add("EventSNO", EventSNO);
        DataTable ObjDT = ObjDH.queryData(CheckEXorElse, adict);
        string CheckEX = ObjDT.Rows[0]["Class3"].ToString();//如果為1則為證書課程，2為繼續教育。
        if (CheckEX == "1")
        {
            //證書課程協助報名
            string PClassSNO = GetPClassSNO(EventSNO);
            string RoleCancel = Utility.RoleCancal(EventSNO);
            string PersonSNO = "";
            string RoleSNO = "";
            string PName = "";
            for (int i = 0; i < this.gv_Invite.Rows.Count; i++)
            {
                if (((CheckBox)gv_Invite.Rows[i].FindControl("Chk_Invite")).Checked)
                {

                    RoleSNO = gv_Invite.DataKeys[i].Values[0].ToString();
                    PersonSNO = gv_Invite.DataKeys[i].Values[1].ToString();
                    PName = gv_Invite.DataKeys[i].Values[2].ToString();
                    string ChkroleSNO = Utility.CheckUserInfo(EventSNO, RoleSNO);
                    if (ChkroleSNO == "1")
                    {
                        if (RoleCancel != "True")
                        {

                            Label myLabel = new Label();
                            myLabel.ID = "123" + i;
                            Label myLabe2 = new Label();
                            myLabe2.ID = "456" + i;
                            this.Controls.Add(myLabel);
                            this.Controls.Add(myLabe2);
                            Utility.ChkNotDone(PName, PersonSNO, PClassSNO, myLabel, myLabe2, EventSNO, userInfo.PersonSNO);


                        }
                        else
                        {
                            Dictionary<string, object> aDict = new Dictionary<string, object>();
                            aDict.Add("EventSNO", EventSNO);
                            aDict.Add("PersonSNO", PersonSNO);
                            aDict.Add("Audit", 0);
                            aDict.Add("NoticeType", 2);
                            aDict.Add("Notice", 0);
                            aDict.Add("CreateUserID", userInfo.PersonSNO);
                            DataHelper objDH = new DataHelper();
                            string SQL = @"  if not  exists(select 1  from EventD where [PersonSNO]=@PersonSNO and EventSNO=@EventSNO)  
                                        Insert Into EventD ([EventSNO],[PersonSNO],[Audit],[NoticeType],[Notice],[CreateUserID])Values(@EventSNO,@PersonSNO,@Audit,@NoticeType,@Notice,@CreateUserID )
                                       
                            ";
                            objDH.executeNonQuery(SQL, aDict);
                            Response.Write("<script>alert('" + PName + " 報名成功')</script>");


                        }

                    }
                    else
                    {

                        Response.Write("<script>alert('" + PName + "不是正確的適用人員')</script>");

                    }
                }
            }
        }
        else
        {
            //繼續教育協助報名
            string RoleCancel = Utility.RoleCancal(EventSNO);
            string CtypeSNO = Utility.EventCtypeSNO(EventSNO);
            string PersonSNO = "";
            string RoleSNO = "";
            string PName = "";
            string PersonID = "";
            for (int i = 0; i < this.gv_Invite.Rows.Count; i++)
            {

                if (((CheckBox)gv_Invite.Rows[i].FindControl("Chk_Invite")).Checked)
                {

                    RoleSNO = gv_Invite.DataKeys[i].Values[0].ToString();
                    PersonSNO = gv_Invite.DataKeys[i].Values[1].ToString();
                    PName = gv_Invite.DataKeys[i].Values[2].ToString();
                    PersonID = gv_Invite.DataKeys[i].Values[3].ToString();
                    string ChkroleSNO = Utility.CheckUserInfo(EventSNO, RoleSNO);
                    if (ChkroleSNO == "1")
                    {
                        string CheckCetificate = @"Select * from QS_Certificate where PersonID=@PersonID and CtypeSNO=@CtypeSNO";
                        adict.Add("PersonID", PersonID);
                        adict.Add("CtypeSNO", CtypeSNO);
                        DataTable ObjCCetificate = ObjDH.queryData(CheckCetificate, adict);
                        if (Convert.ToDateTime(ObjCCetificate.Rows[0]["CertStartDate"]) > Convert.ToDateTime(ObjCCetificate.Rows[0]["CertEndDate"]))
                        {
                            Response.Write("<script>alert('證書過期!')</script>");
                            return;
                        }
                        else
                        {
                            Dictionary<string, object> aDict = new Dictionary<string, object>();
                            aDict.Add("EventSNO", EventSNO);
                            aDict.Add("PersonSNO", PersonSNO);
                            aDict.Add("Audit", 0);
                            aDict.Add("NoticeType", 2);
                            aDict.Add("Notice", 0);
                            aDict.Add("CreateUserID", userInfo.PersonSNO);
                            DataHelper objDH = new DataHelper();
                            string SQL = @"  if not  exists(select 1  from EventD where [PersonSNO]=@PersonSNO and EventSNO=@EventSNO)  
                                        Insert Into EventD ([EventSNO],[PersonSNO],[Audit],[NoticeType],[Notice],[CreateUserID])Values(@EventSNO,@PersonSNO,@Audit,@NoticeType,@Notice,@CreateUserID )
                                       
                            ";
                            objDH.executeNonQuery(SQL, aDict);
                            Response.Write("<script>alert('" + PName + " 報名成功')</script>");
                        }

                    }
                    else
                    {
                        Response.Write("<script>alert('" + PName + "不是正確的適用人員')</script>");
                        return;
                    }
                }
            }

        }
        HttpContext.Current.Response.Write("<script>window.opener.location.reload()</script>");
    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        bindData(page);
    }

    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        string querystring = Request.QueryString["sno"];
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("EventSNO", querystring);
        string StrSql = @"Select ROW_NUMBER() OVER (ORDER BY P.PersonSNO) as ROW_NO,* from person P where 1=1 ";
        DataHelper DH = new DataHelper();
        if (!string.IsNullOrEmpty(txt_PName.Text))
        {
            StrSql += " And P.PName=@PName ";
            dict.Add("PName", txt_PName.Text);
        }
        if (!string.IsNullOrEmpty(txt_PersonID.Text))
        {
            StrSql += " And P.PersonID=@PersonID ";
            dict.Add("PersonID", txt_PersonID.Text);
        }
        DataTable objDT = DH.queryData(StrSql, dict);

        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Invite.DataSource = objDT;
        gv_Invite.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

    protected void btn_InviteSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    }


    protected static string GetPClassSNO(string EventSNO)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        DataHelper DH = new DataHelper();
        dict.Add("EventSNO", EventSNO);
        string sql = @"Select * from Event where EventSNO=@EventSNO";
        DataTable objDT = DH.queryData(sql, dict);
        string PClassSNO = objDT.Rows[0]["PClassSNO"].ToString();
        return PClassSNO;
    }

    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script language='javascript'>window.close();</script>");
    }

    protected void gv_Invite_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            //要隱藏的欄位，如果前台filed隱藏會抓不到值，所以在RowCreate時才隱藏
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
        }
    }

    protected void btn_Nocondition_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> adict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        string EventSNO = Request.QueryString["sno"];
        //判斷是否為繼續教育或證書
        string CheckEXorElse = @"Select * from Event where EventSNO=@EventSNO ";
        adict.Add("EventSNO", EventSNO);
        DataTable ObjDT = ObjDH.queryData(CheckEXorElse, adict);
        string CheckEX = ObjDT.Rows[0]["Class3"].ToString();//如果為1則為證書課程，2為繼續教育。
        if (CheckEX == "1")
        {
            //證書課程協助報名
            string PClassSNO = GetPClassSNO(EventSNO);
            string RoleCancel = Utility.RoleCancal(EventSNO);
            string PersonSNO = "";
            string RoleSNO = "";
            string PName = "";
            for (int i = 0; i < this.gv_Invite.Rows.Count; i++)
            {
                if (((CheckBox)gv_Invite.Rows[i].FindControl("Chk_Invite")).Checked)
                {

                    RoleSNO = gv_Invite.DataKeys[i].Values[0].ToString();
                    PersonSNO = gv_Invite.DataKeys[i].Values[1].ToString();
                    PName = gv_Invite.DataKeys[i].Values[2].ToString();
                    string ChkroleSNO = Utility.CheckUserInfo(EventSNO, RoleSNO);


                    Dictionary<string, object> aDict = new Dictionary<string, object>();
                    aDict.Add("EventSNO", EventSNO);
                    aDict.Add("PersonSNO", PersonSNO);
                    aDict.Add("Audit", 0);
                    aDict.Add("NoticeType", 2);
                    aDict.Add("Notice", 0);
                    aDict.Add("CreateUserID", userInfo.PersonSNO);
                    DataHelper objDH = new DataHelper();
                    string SQL = @"  if not  exists(select 1  from EventD where [PersonSNO]=@PersonSNO and EventSNO=@EventSNO)  
                                        Insert Into EventD ([EventSNO],[PersonSNO],[Audit],[NoticeType],[Notice],[CreateUserID])Values(@EventSNO,@PersonSNO,@Audit,@NoticeType,@Notice,@CreateUserID )
                                       
                            ";
                    objDH.executeNonQuery(SQL, aDict);
                    Response.Write("<script>alert('" + PName + " 報名成功')</script>");






                }
            }
        }
        else
        {
            //繼續教育協助報名
            string RoleCancel = Utility.RoleCancal(EventSNO);
            string PersonSNO = "";
            string RoleSNO = "";
            string PName = "";
            string PersonID = "";
            for (int i = 0; i < this.gv_Invite.Rows.Count; i++)
            {

                if (((CheckBox)gv_Invite.Rows[i].FindControl("Chk_Invite")).Checked)
                {
                    RoleSNO = gv_Invite.DataKeys[i].Values[0].ToString();
                    PersonSNO = gv_Invite.DataKeys[i].Values[1].ToString();
                    PName = gv_Invite.DataKeys[i].Values[2].ToString();
                    PersonID = gv_Invite.DataKeys[i].Values[3].ToString();
                    string ChkroleSNO = Utility.CheckUserInfo(EventSNO, RoleSNO);

                    string CheckCetificate = @"Select * from QS_Certificate where PersonID=@PersonID";
                    adict.Add("PersonID", PersonID);
                    DataTable ObjCCetificate = ObjDH.queryData(CheckCetificate, adict);


                    Dictionary<string, object> aDict = new Dictionary<string, object>();
                    aDict.Add("EventSNO", EventSNO);
                    aDict.Add("PersonSNO", PersonSNO);
                    aDict.Add("Audit", 0);
                    aDict.Add("NoticeType", 2);
                    aDict.Add("Notice", 0);
                    aDict.Add("CreateUserID", userInfo.PersonSNO);
                    DataHelper objDH = new DataHelper();
                    string SQL = @"  if not  exists(select 1  from EventD where [PersonSNO]=@PersonSNO and EventSNO=@EventSNO)  
                                        Insert Into EventD ([EventSNO],[PersonSNO],[Audit],[NoticeType],[Notice],[CreateUserID])Values(@EventSNO,@PersonSNO,@Audit,@NoticeType,@Notice,@CreateUserID )
                                       
                            ";
                    objDH.executeNonQuery(SQL, aDict);
                    Response.Write("<script>alert('" + PName + " 報名成功')</script>");
                }
            }

        }
        HttpContext.Current.Response.Write("<script>window.opener.location.reload()</script>");
    }
}