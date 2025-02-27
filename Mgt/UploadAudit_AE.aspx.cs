using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.IO;
using System.Text;

public partial class Mgt_UploadAudit_AE : System.Web.UI.Page
{
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfo;
    public Dictionary<string, string> _SetCol;
    UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //Utility.setAudit(ddl_Status, "請選擇");
            binData();
            //if (userInfo.RoleSNO == "2")
            //{
            //    Role_view.Visible = true;
            //    Tabs_2.Visible = true;
            //}
            //else if (userInfo.RoleSNO == "3")
            //{
            //    Role_view.Visible = true;
            //    Tabs_2.Visible = true;
            //}
        }

    }

    protected void binData()
    {
        String PersonSNO = Convert.ToString(Request.QueryString["Psno"]);
        string CourseSNO= Convert.ToString(Request.QueryString["Csno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();      

        string gv_SQL = @" Select ROW_NUMBER() OVER (ORDER BY P.PersonID Desc) as ROW_NO,UU.ULSNO,UU.Note,QC.CourseSNO,P.PersonSNO
                                ,P.PName,P.PersonID,QC.CourseName,UU.Url,UU.CreateDT,UU.Audit,UU.Notice,P.PMail
                            from UserUpload UU
                            Left Join Person P On P.PersonSNO= UU.PersonSNO
                            Left Join QS_Course QC On QC.CourseSNO=UU.CourseSNO
                             where UU.PersonSNO=@PersonSNO and UU.CourseSNO=@CourseSNO
                 ";
        aDict.Add("PersonSNO", PersonSNO);
        aDict.Add("CourseSNO", CourseSNO);

        DataTable objDT = objDH.queryData(gv_SQL, aDict);
        gv_EventD.DataSource = objDT.DefaultView;
        gv_EventD.DataBind();

        lb_Mail.Text = objDT.Rows[0]["Pmail"].ToString();
        foreach (GridViewRow gvr in gv_EventD.Rows)
        {
            var type = ((DropDownList)gvr.FindControl("ddl_AuditItem"));
            var Btn = ((Button)gvr.FindControl("Btn_Send"));
            if (type.SelectedValue != "0")
            {
                type.Enabled = false;
                type.CssClass = "dis";
                Btn.Enabled = false;
                Btn.CssClass = "dis";
            }
           
        }

    }

    protected void btnSendMail_Click(object sender, EventArgs e)
    {

        Utility.SendMail("實習上傳審核通知", editor_Mail.InnerText,lb_Mail.Text);
        DataHelper objDH = new DataHelper();
        string sql = @"Update UserUpload set Notice=1,MailContent=@MailContent where ULSNO=@ULSNO";
        Dictionary<string, object> adict = new Dictionary<string, object>();
        adict.Add("MailContent", HttpUtility.HtmlEncode(editor_Mail.InnerText));
        adict.Add("ULSNO", lb_ULSNO.Text);
        objDH.executeNonQuery(sql, adict);
        Response.Write("<scipt>alert('寄件成功')</script>");
        Response.Redirect(Request.Url.ToString());
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        string errorMessage = "";
        String id = Convert.ToString(Request.QueryString["sno"]);
        string LimitSQL = " SELECT * FROM Event WHERE  EventSNO=" + id + "";
        DataTable ObjLimit = objDH.queryData(LimitSQL, null);

        int Limit = Convert.ToInt16(ObjLimit.Rows[0]["CountLimit"].ToString());
        int count = 0;
        string sql = "";
        if (gv_EventD.Rows.Count == 0)
        {
            Response.Write("<script>alert('目前無報名人員')</script>");
            return;
        }
        foreach (GridViewRow gridRow in gv_EventD.Rows)
        {

            var ddl_Audit = (DropDownList)gridRow.FindControl("ddl_AuditItem");
            var hid_EventDid = (HiddenField)gridRow.FindControl("hid_EventDid");
            if (ddl_Audit.SelectedValue == "1")
            {
                count += 1;
            }
            sql += " Update EventD Set Audit = '" + ddl_Audit.SelectedValue + "',ModifyDT='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + "',ModifyUserID=" + userInfo.PersonSNO + " WHERE EventDSNO = " + hid_EventDid.Value + " ; ";
        }
        if (Limit == 0 || count <= Limit)
        {
            DataTable objDTPAPER = objDH.queryData(sql, null);
            Response.Write("<script>alert('送出成功!'); </script>");
            Response.Redirect(Request.Url.ToString());
        }
        else
        {
            if (count > Limit) errorMessage = "超出錄取上限人數";
            if (!string.IsNullOrEmpty(errorMessage))
            {
                Utility.showMessage(Page, "ErrorMessage", errorMessage);
                return;
            }

        }
       
       
    }

    protected void ddl_AuditItem_SelectedIndexChanged(object sender, EventArgs e)
    {

        int unAdmit = 0;
        int Admitted = 0;
        int Waiting = 0;

        foreach (GridViewRow gridRow in gv_EventD.Rows)
        {
            var ddl = (DropDownList)gridRow.FindControl("ddl_AuditItem");
            var selVal = Convert.ToInt16(ddl.SelectedValue);
            if (selVal == 0) unAdmit += 1;
            else if (selVal == 1) Admitted += 1;
            else if (selVal == 2) Waiting += 1;

        }

        lbl_UnAdmit.Text = unAdmit.ToString();
        lbl_Admitted.Text = Admitted.ToString();
        lbl_Waiting.Text = Waiting.ToString();

    }

   

    protected void Invite_download_Click(object sender, EventArgs e)
    {
        if (gv_EventD.Rows.Count == 0)
        {
            Response.Write("<script>alert('尚未查詢')</script>");
            return;
        }
        Utility.OpenExportWindows(this, ReportEnum.EventAudit.ToString());
    }

    protected void Btn_Search_Click(object sender, EventArgs e)
    {
        binData();

    }

    protected string getFiles(string dirID)
    {
        string fileList = "";
        if (Directory.Exists(Server.MapPath(dirID)))
        {
            string[] files = Directory.GetFiles(Server.MapPath("../Upload") + "/" + dirID);
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fileInfo = new FileInfo(files[i]);
                fileList += "<a target='_blank' href='../Upload/" + dirID + "/" + fileInfo.Name + "'><i class='fa fa-file'></i> " + fileInfo.Name + "</a><br/>";
            }
        }
        return fileList;
    }

  

    protected void btn_Back_Click(object sender, EventArgs e)
    {

     
            Response.Redirect("UploadAudit.aspx");
        
    }


    protected void gv_EventD_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        if (e.CommandName == "result")
        {

           
            Button myButton = (Button)e.CommandSource;
            GridViewRow myRow = (GridViewRow)myButton.NamingContainer;
            var ddl_Audit = (DropDownList)myRow.FindControl("ddl_AuditItem");
            string Audit = ddl_Audit.SelectedValue;
            string ULSNO = myRow.Cells[1].Text;
            lb_ULSNO.Text = ULSNO;
            
            adict.Add("ULSNO", ULSNO);
            adict.Add("Audit", Audit);
            string sql = @"Update UserUpload set Audit=@Audit where ULSNO=@ULSNO";
            DataTable objDT = ObjDH.queryData(sql, adict);
            string PersonSNO = myRow.Cells[14].Text;
            string CourseSNO = myRow.Cells[5].Text;
            if (Audit == "1")
            {
                InsertIntegral(PersonSNO, CourseSNO);
            }
            adict.Clear();

        }
        if(e.CommandName == "Downloads")
        {
            Button myButton = (Button)e.CommandSource;
            GridViewRow myRow = (GridViewRow)myButton.NamingContainer;
            string URL = myRow.Cells[6].Text;
            System.Net.WebClient wc = new System.Net.WebClient(); //呼叫 webclient 方式做檔案下載
            byte[] xfile = null;
            string docupath = Request.PhysicalApplicationPath;
            string aa = docupath + URL;
            xfile = wc.DownloadData(docupath + URL);
            string xfileName = System.IO.Path.GetFileName(docupath + URL);
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpContext.Current.Server.UrlEncode(xfileName));
            HttpContext.Current.Response.ContentType = "application/octet-stream"; //二進位方式
            HttpContext.Current.Response.BinaryWrite(xfile); //內容轉出作檔案下載
            HttpContext.Current.Response.End();
        }
        if(e.CommandName == "ReadMail")
        {
            Button myButton = (Button)e.CommandSource;
            GridViewRow myRow = (GridViewRow)myButton.NamingContainer;
            string ULSNO = myRow.Cells[1].Text;
            string js = "window.open('ReadMail.aspx?ULSNO="+ ULSNO + "', 'ReadMail', config='height=300,width=600,scrollbar=no');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "openEL", js, true);
            adict.Clear();
        }
          
    }

    protected void Btn_Note_Click(object sender, EventArgs e)
    {

    }



    protected void gv_EventD_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem != null)
        {
            string twenty = (Request["twenty"] != null) ? Request["twenty"].ToString() : "N/A";
            string FC = e.Row.Cells[0].Text.Replace("&nbsp;","");
            string SC= e.Row.Cells[1].Text.Replace("&nbsp;", "");
            if (SC != "" && FC != "" && twenty == "1")
            {
                e.Row.BackColor = System.Drawing.Color.LightPink;

            }
        }
      


    }

    protected void gv_EventD_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[5].Visible = false;
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[14].Visible = false;
        }

    }

    protected void Btn_Out_Click(object sender, EventArgs e)
    {
        //btnSendMail.Text = "寄送且簽退";

    }

    protected void Lk_Downloads_Click(object sender, EventArgs e)
    {

    }




    protected void Btn_Send_Click(object sender, EventArgs e)
    {
        //Button btn = (Button)sender;
        //GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        //string a=gvr.Cells[0].Text;
        //var ddl_Audit = (DropDownList)gvr.FindControl("ddl_AuditItem");
        //string XX = ddl_Audit.SelectedValue;
    }
    public void InsertIntegral(string PersonSNO,string CourseSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wicpd = new Dictionary<string, object>();
        string sql = @" If Exists(select 1 From QS_Integral Where PersonSNO=@PersonSNO And CourseSNO=@CourseSNO)
                
                 Select 1
            Else
                Insert Into QS_Integral(PersonSNO,CourseSNO,CreateDT,CreateUserID) 
                VALUES (@PersonSNO,@CourseSNO,@CreateDT,@CreateUserID)";
        wicpd.Add("PersonSNO", PersonSNO);
        wicpd.Add("CourseSNO", CourseSNO);
        wicpd.Add("AuthType", 1);
        wicpd.Add("CreateDT", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        wicpd.Add("CreateUserID", userInfo.PersonSNO);
        DataTable DS = objDH.queryData(sql, wicpd);
    }
}
