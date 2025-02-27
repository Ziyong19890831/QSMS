using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ToolkitsBackStage_AE : System.Web.UI.Page
{
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
            string work = "";
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
            setStage(CBL_Stage, "請選擇");
            setTkType(CHB_TkType, "請選擇");
            setStageClass(CBL_StageClass, "請選擇");
            if (work.Equals("N"))
            {
                newData();
                //p1.Visible = true;
            }
            else
            {
                getData();
                //p1.Visible = false;
            }
        }
    }
    protected void newData()
    {
        Work.Value = "NEW";
    }
    protected void getData()
    {
        RequiredFieldValidator1.Enabled = false;
        string TkSNO = Convert.ToString(Request.QueryString["TkSNO"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("TkSNO", TkSNO);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
            SELECT * from [Toolkits]	           
            WHERE TkSNO=@TkSNO
        ", aDict);
        if (objDT.Rows.Count > 0)
        {

            string[] TkType = objDT.Rows[0]["TkType"].ToString().Split(',');
            if (TkType.Length > 0)
            {
                for (int j = 0; j < CHB_TkType.Items.Count; j++)
                {
                    for (int i = 0; i < TkType.Length; i++)
                    {
                        if (CHB_TkType.Items[j].Value == TkType[i])
                        {
                            CHB_TkType.Items[j].Selected = true;
                        }
                    }
                }
                    
            }
            string[] stage = objDT.Rows[0]["Stage"].ToString().Split(','); ;
            if (stage.Length > 0)
            {
                for(int j=0; j < CBL_Stage.Items.Count; j++)
                {
                    for (int i = 0; i < stage.Length; i++)
                    {
                        if(CBL_Stage.Items[j].Value== stage[i])
                        {
                            CBL_Stage.Items[j].Selected = true;
                        }
                       
                    }
                }
               
            }
            string[] stageClass = objDT.Rows[0]["StageClass"].ToString().Split(',');
            if (stageClass.Length > 0)
            {
                for (int j = 0; j < CBL_StageClass.Items.Count; j++)
                {
                    for (int i = 0; i < stageClass.Length; i++)
                    {
                        if (CBL_StageClass.Items[j].Value == stageClass[i])
                        {
                            CBL_StageClass.Items[j].Selected = true;
                        }
                    }
                }
            }
            txt_FileName.Text = objDT.Rows[0]["TkName"].ToString();
            chk_IsEnable.Checked = (Boolean)(objDT.Rows[0]["IsEnable"]);

        }
    }
    protected void btn_delfile_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        String id = btn.CommandArgument;
        ((Literal)Master.FindControl("ContentPlaceHolder1").FindControl("lt_file" + id)).Visible = false;
        ((FileUpload)Master.FindControl("ContentPlaceHolder1").FindControl("fileup_Document" + id)).Visible = true;
        ((Button)Master.FindControl("ContentPlaceHolder1").FindControl("btn_delfile" + id)).Visible = false;
    }
    public static void setStage(CheckBoxList CBL, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select PVal,MVal from Config C where C.PGroup='Stage'", null);
        CBL.DataSource = objDT;
        CBL.DataBind();

    }
    public static void setStageClass(CheckBoxList CBL, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select PVal,MVal from Config C where C.PGroup='StageClass'", null);
        CBL.DataSource = objDT;
        CBL.DataBind();

    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        String errorMessage = "";
        //if (string.IsNullOrEmpty(ddl_stageClass.SelectedValue)) errorMessage += "請選擇適用性!\\n";
        //if (ddl_stage.SelectedValue == "") errorMessage += "請選擇期別!\\n";
        if (string.IsNullOrEmpty(lbl_FileName.Text)) errorMessage += "請輸入教材名稱!\\n";
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }
        if (Work.Value.Equals("NEW"))
        {
            string stageClass = "";
            string stage = "";
            string TkType = "";
            string stageClassName = "";
            string stageName = "";
            string TkTypeName = "";
            if (CBL_Stage.Items.Count > 0)
            {
                for (int i = 0; i < CBL_Stage.Items.Count; i++)
                {
                    if (CBL_Stage.Items[i].Selected)
                    {
                        stage += CBL_Stage.Items[i].Value + ",";
                        stageName += CBL_Stage.Items[i].Text + ",";
                    }
                }
               
            }
            if (stageName.Length > 0)
            {
                stageName = stageName.Substring(0, stageName.Length - 1);
            }
            if (stage.Length > 0)
            {
                stage = stage.Substring(0, stage.Length - 1);
            }
            if (CHB_TkType.Items.Count > 0)
            {
                for (int i = 0; i < CHB_TkType.Items.Count; i++)
                {
                    if (CHB_TkType.Items[i].Selected)
                    {
                        TkType += CHB_TkType.Items[i].Value  + ",";
                        TkTypeName += CHB_TkType.Items[i].Text + ",";
                    }
                }
               
            }
            if (TkTypeName.Length > 0)
            {
                TkTypeName = TkTypeName.Substring(0, TkTypeName.Length - 1);
            }
            if (TkType.Length > 0)
            {
                TkType = TkType.Substring(0, TkType.Length - 1);
            }
            if (CBL_StageClass.Items.Count > 0)
            {
                for (int i = 0; i < CBL_StageClass.Items.Count; i++)
                {
                    if (CBL_StageClass.Items[i].Selected)
                    {
                        stageClass +=  CBL_StageClass.Items[i].Value  + ",";
                        stageClassName += CBL_StageClass.Items[i].Text + ",";
                    }
                }
            }
            if (stageClassName.Length > 0)
            {
                stageClassName = stageClassName.Substring(0, stageClassName.Length - 1);
            }
            if (stageClass.Length > 0)
            {
                stageClass = stageClass.Substring(0, stageClass.Length - 1);
            }
            System.IO.DriveInfo di = new System.IO.DriveInfo(@"F:\");

            string FileName = fileup_Document1.FileName;
            string FileName1 = fileup_Document2.FileName;
            string Extension = Path.GetExtension(FileName);
            string URL = Server.MapPath("../Toolkits") + "/";
            string TkURL = "../Toolkits" + "/" + FileName;
            string TkURL_Pic = "../Toolkits" + "/" + FileName1;
            uploadFiles(URL);

            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("Stage", stage);
            aDict.Add("StageClass", stageClass);
            aDict.Add("TKType", TkType);
            aDict.Add("StageName", stageName);
            aDict.Add("StageClassName", stageClassName);
            aDict.Add("TKTypeName", TkTypeName);
            aDict.Add("TkName", txt_FileName.Text);
            aDict.Add("Extension", Extension);
            aDict.Add("IsEnable", chk_IsEnable.Checked);
            aDict.Add("TkURL", TkURL);
            aDict.Add("TkURL_Pic", TkURL_Pic);
            aDict.Add("TkNote", txt_Note.Text);
            aDict.Add("Dcount", 0);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            aDict.Add("CreateDT", DateTime.Now.ToShortDateString());
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery(@"Insert Into Toolkits (Stage,StageClass,TKType,TkName,TkURL,TkURL_pic,TkNote,IsEnable,CreateUserID,[CreateDT],Extension,StageName,StageClassName,TkTypeName,Dcount)
            Values(@Stage,@StageClass,@TKType,@TkName,@TkURL,@TkURL_pic,@TkNote,@IsEnable,@CreateUserID,@CreateDT,@Extension,@StageName,@StageClassName,@TkTypeName,@Dcount)", aDict);
            Response.Write("<script>alert('新增成功!');document.location.href='./ToolkitsBackStage.aspx'; </script>");
        }
        else
        {
            
            string stageClass = "";
            string stage = "";
            string TkType = "";
            string stageClassName = "";
            string stageName = "";
            string TkTypeName = "";
            if (CBL_Stage.Items.Count > 0)
            {
                for (int i = 0; i < CBL_Stage.Items.Count; i++)
                {
                    if (CBL_Stage.Items[i].Selected)
                    {
                        stage += CBL_Stage.Items[i].Value + ",";
                        stageName += CBL_Stage.Items[i].Text + ",";
                    }
                }
               
            }
            if (stageName.Length > 0)
            {
                stageName = stageName.Substring(0, stageName.Length - 1);
            }
            if (stage.Length > 0)
            {
                stage = stage.Substring(0, stage.Length - 1);
            }
            if (CHB_TkType.Items.Count > 0)
            {
                for (int i = 0; i < CHB_TkType.Items.Count; i++)
                {
                    if (CHB_TkType.Items[i].Selected)
                    {
                        TkType +=  CHB_TkType.Items[i].Value  + ",";
                        TkTypeName+= CHB_TkType.Items[i].Text + ",";
                    }
                }
               
            }
            if (TkTypeName.Length > 0)
            {
                TkTypeName = TkTypeName.Substring(0, TkTypeName.Length - 1);
            }
            if (TkType.Length > 0)
            {
                TkType = TkType.Substring(0, TkType.Length - 1);
            }
            if (CBL_StageClass.Items.Count > 0)
            {
                for (int i = 0; i < CBL_StageClass.Items.Count; i++)
                {
                    if (CBL_StageClass.Items[i].Selected)
                    {
                        stageClass +=  CBL_StageClass.Items[i].Value + ",";
                        stageClassName+= CBL_StageClass.Items[i].Text + ",";
                    }
                }
               
            }
            if (stageClassName.Length > 0)
            {
                stageClassName = stageClassName.Substring(0, stageClassName.Length - 1);
            }
            if (stageClass.Length > 0)
            {
                stageClass = stageClass.Substring(0, stageClass.Length - 1);
            }
            string TkSNO = Convert.ToString(Request.QueryString["TkSNO"]);
            string FileName = fileup_Document1.FileName;
            string FileName1 = fileup_Document2.FileName;
            string Extension = Path.GetExtension(FileName);
            string URL = Server.MapPath("../Toolkits") + "/";
            string TkURL = "../Toolkits" + "/" + FileName;
            string TkURL_Pic = "../Toolkits" + "/" + FileName1;
            uploadFiles(URL);
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("TkSNO", TkSNO);
            aDict.Add("Stage", stage);
            aDict.Add("StageClass", stageClass);
            aDict.Add("TKType", TkType);
            aDict.Add("StageName", stageName);
            aDict.Add("StageClassName", stageClassName);
            aDict.Add("TKTypeName", TkTypeName);
            aDict.Add("TkName", txt_FileName.Text);
            aDict.Add("IsEnable", chk_IsEnable.Checked);
            aDict.Add("Extension", Extension);
            //aDict.Add("TkURL", TkURL);
            //aDict.Add("TkURL_Pic", TkURL_Pic);
            aDict.Add("TkNote", txt_Note.Text);
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            aDict.Add("ModifyDT", DateTime.Now.ToShortDateString());
            DataHelper objDH = new DataHelper();
            string sql = @"UPDATE Toolkits SET 
                        Stage = @Stage, 
                        StageName = @StageName, 
                        StageClass = @StageClass, 
                        StageClassName = @StageClassName, 
                        TKType=@TKType,
                        TKTypeName=@TKTypeName,
                        TkName=@TkName,                        
                        TkNote = @TkNote,
                        IsEnable=@IsEnable,
                        ModifyUserID = @ModifyUserID,
                        ModifyDT=@ModifyDT";
            if (Extension != "")
            {
                sql += " ,Extension=@Extension WHERE TkSNO=@TkSNO";
            }
            else
            {
                sql += " WHERE TkSNO=@TkSNO";
            }
            objDH.executeNonQuery(sql, aDict);
            Response.Write("<script>alert('修改成功!');document.location.href='./ToolkitsBackStage.aspx'; </script>");
        }
    }
    protected void uploadFiles(string folderPath)
    {

        Literal lt = ((Literal)Master.FindControl("ContentPlaceHolder1").FindControl("lt_file1"));
        FileUpload fu = ((FileUpload)Master.FindControl("ContentPlaceHolder1").FindControl("fileup_Document1"));
        Literal lt1 = ((Literal)Master.FindControl("ContentPlaceHolder1").FindControl("lt_file2"));
        FileUpload fu1 = ((FileUpload)Master.FindControl("ContentPlaceHolder1").FindControl("fileup_Document2"));
        if (fu.HasFile)
        {
            fu.SaveAs(folderPath + fu.FileName);
        }
        if (lt.Visible == false)
        {
            FileInfo fileInfo = new FileInfo(folderPath + lt.Text);
            if (fileInfo.Exists) fileInfo.Delete();
        }
        if (fu1.HasFile)
        {
            fu1.SaveAs(folderPath + fu1.FileName);
        }
        if (lt1.Visible == false)
        {
            FileInfo fileInfo1 = new FileInfo(folderPath + lt1.Text);
            if (fileInfo1.Exists) fileInfo1.Delete();
        }

    }
    public static void setTkType(CheckBoxList CBL, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("Select Pval,Mval from Config C where C.Pgroup='TKType'", null);
        CBL.DataSource = objDT;
        CBL.DataBind();

    }
    protected void btn_delfile2_Click(object sender, EventArgs e)
    {

    }
}