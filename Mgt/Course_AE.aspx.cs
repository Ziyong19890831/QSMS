using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Course_AE : System.Web.UI.Page
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
            Utility.setPlanName(ddl_PClass, "請選擇");
            Utility.SetDdlConfig(ddl_Class1, "CourseClass1", "請選擇");
            //Utility.SetDdlConfig(ddl_Class2, "CourseClass2", "請選擇");
            Utility.SetDdlConfig(ddl_Ctype, "CourseCType", "請選擇");
            Utility.setCourse(ddl_Course, "","有需要請選擇");
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
            if (work.Equals("N"))
            {
                newData();
            }
            else
            {
                getData();
            }
        }
    }


    protected void newData()
    {
        Work.Value = "NEW";
        Utility.setELearn(ddl_Elearn, "請選擇");
        Utility.setELearnSectionWithNoBind(ddl_ElearnSection, ddl_Elearn.SelectedValue, "請選擇");
        lb_BindELS.Text = "尚未指定E-Learning對應";
        btn_reset.Visible = false;
    }

    protected void getData()

    {
        string id = Convert.ToString(Request.QueryString["sno"]);

        bool Checkpair = Utility.Checkpair(id);
        if (Checkpair == true)
        {
            lb_Pair.Visible = true;
            ddl_Course.Visible = false;
        }


        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("CourseSNO", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
            SELECT
	            A.CourseSNO, A.PClassSNO, A.Class1, A.Class2, A.UnitName,A.Compulsory, A.CourseName, A.Ctype, A.CHour, A.ELSCode, ces.ELSName, ce.ELName , A.IsEnable ,PairCourseSNO
            FROM QS_Course A
	            Left Join QS_CourseELearningSection ces On ces.ELSCode=A.ELSCode
	            Left Join QS_CourseELearning ce On ce.ELCode=ces.ELCode
            WHERE A.CourseSNO=@CourseSNO
        ", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_ID.Value = Convert.ToString(objDT.Rows[0]["CourseSNO"]);
            ddl_PClass.SelectedValue = Convert.ToString(objDT.Rows[0]["PClassSNO"]);
            ddl_Class1.SelectedValue = Convert.ToString(objDT.Rows[0]["Class1"]);
            chk_Compulsory.Checked = Convert.ToBoolean(objDT.Rows[0]["Compulsory"]);
            chk_IsEnable.Checked= Convert.ToBoolean(objDT.Rows[0]["IsEnable"]);
            //ddl_Class2.SelectedValue = Convert.ToString(objDT.Rows[0]["Class2"]);
            ddl_Ctype.SelectedValue = Convert.ToString(objDT.Rows[0]["Ctype"]);
            //txt_UnitName.Text = Convert.ToString(objDT.Rows[0]["UnitName"]);
            txt_CourseName.Text = Convert.ToString(objDT.Rows[0]["CourseName"]);
            txt_CHour.Text = Convert.ToString(objDT.Rows[0]["CHour"]);
            hf_ELSCode.Value = Convert.ToString(objDT.Rows[0]["ELSCode"]);
            ddl_Course.SelectedValue = Convert.ToString(objDT.Rows[0]["PairCourseSNO"]);
            if (objDT.Rows[0]["ELSName"].ToString() != "")
            {
                lb_BindELS.Text = "目前已指定的E-Learning對應：" + Convert.ToString(objDT.Rows[0]["ELName"]) + "-" + Convert.ToString(objDT.Rows[0]["ELSName"]);
            }
            else
            {
                lb_BindELS.Text = "尚未指定E-Learning對應";
                btn_reset.Visible = false;
            }
            Utility.setELearn(ddl_Elearn, "請選擇");
            Utility.setELearnSectionWithNoBind(ddl_ElearnSection, ddl_Elearn.SelectedValue, "請選擇");
            setElVisble();
        }

    }

    protected void btnOK_Click(object sender, EventArgs e)
    {

        String errorMessage = "";

        if (string.IsNullOrEmpty(ddl_PClass.SelectedValue)) errorMessage += "請選擇課程規劃類別對應\\n";
        if (ddl_Class1.SelectedValue == "") errorMessage += "請選擇課程類別1!\\n";
        //if (ddl_Class2.SelectedValue == "") errorMessage += "請選擇課程類別2!\\n";
        //if (string.IsNullOrEmpty(txt_UnitName.Text)) errorMessage += "請輸入單元!\\n";
        if (string.IsNullOrEmpty(txt_CourseName.Text)) errorMessage += "請輸入課程名稱!\\n";
        if (string.IsNullOrEmpty(txt_CHour.Text)) errorMessage += "請輸入時數!\\n";
        if (ddl_Ctype.SelectedValue == "") errorMessage += "請選擇授課方式!\\n";
        //if (txt_UnitName.Text.Length > 50)   errorMessage += "單元字元過多!\\n";
        if (txt_CourseName.Text.Length > 50) errorMessage += "課程名稱字元過多!\\n";

        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        if (Work.Value.Equals("NEW"))
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("PClassSNO", ddl_PClass.SelectedValue);
            aDict.Add("CourseName", txt_CourseName.Text);
            //aDict.Add("UnitName", txt_UnitName.Text);
            aDict.Add("CHour", txt_CHour.Text);
            aDict.Add("Class1", ddl_Class1.SelectedValue);
            aDict.Add("Compulsory", chk_Compulsory.Checked);
            //aDict.Add("Class2", ddl_Class2.SelectedValue);
            aDict.Add("Ctype", ddl_Ctype.SelectedValue);
            aDict.Add("IsEnable", chk_IsEnable.Checked);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            aDict.Add("ELSCode", hf_ELSCode.Value);
            aDict.Add("PairCourseSNO", ddl_Course.SelectedValue);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery(@"
                INSERT INTO QS_Course (PClassSNO, Class1,  CourseName, CType, CHour, ELSCode, Compulsory, IsEnable , CreateUserID , PairCourseSNO)
			    VALUES (@PClassSNO, @Class1,  @CourseName, @Ctype, @CHour, @ELSCode,@Compulsory , @IsEnable , @CreateUserID , @PairCourseSNO)
            ", aDict);
            Response.Write("<script>alert('新增成功!');document.location.href='./Course.aspx'; </script>");
        }
        else
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("id", txt_ID.Value);
            aDict.Add("PClassSNO", ddl_PClass.SelectedValue);
            aDict.Add("CourseName", txt_CourseName.Text);
            //aDict.Add("UnitName", txt_UnitName.Text);
            aDict.Add("IsEnable", chk_IsEnable.Checked);
            aDict.Add("CHour", txt_CHour.Text);
            aDict.Add("Class1", ddl_Class1.SelectedValue);
            aDict.Add("Compulsory", chk_Compulsory.Checked);
            //aDict.Add("Class2", ddl_Class2.SelectedValue);
            aDict.Add("Ctype", ddl_Ctype.SelectedValue);           
            aDict.Add("ModifyDT", Convert.ToDateTime(DateTime.Now));
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            aDict.Add("ELSCode", hf_ELSCode.Value);
            aDict.Add("PairCourseSNO", ddl_Course.SelectedValue);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery(@"
                UPDATE QS_Course SET 
                        PClassSNO = @PClassSNO, 
                        Class1 = @Class1, 
                        Compulsory=@Compulsory,
                        CourseName = @CourseName, 
                        CType = @Ctype,
                        CHour = @CHour, 
                        IsEnable=@IsEnable,
                        ELSCode = @ELSCode, 
                        ModifyDT = @ModifyDT,
                        ModifyUserID = @ModifyUserID,  
                        PairCourseSNO=@PairCourseSNO
                WHERE CourseSNO=@id
            ", aDict);
            Response.Write("<script>alert('修改成功!');document.location.href='./Course.aspx'; </script>");
        }
    }

    protected void ddl_Ctype_SelectedIndexChanged(object sender, EventArgs e)
    {
        setElVisble();
    }

    protected void setElVisble()
    {
        if (ddl_Ctype.SelectedValue == "1")
        {
            pl_el.Visible = true;
            pl_bind.Visible = true;
        }
        else
        {
            pl_el.Visible = false;
            pl_bind.Visible = false;
        }
    }

    protected void ddl_Elearn_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_ElearnSection.Items.Clear();
        Utility.setELearnSectionWithNoBind(ddl_ElearnSection, ddl_Elearn.SelectedValue, "請選擇");
    }

    protected void ddl_ElearnSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_ElearnSection.SelectedValue != "")
        {
            hf_ELSCode.Value = ddl_ElearnSection.SelectedValue;
            lb_BindELS.Text = "目前已指定的E-Learning對應：" + ddl_Elearn.SelectedItem.Text + "-" + ddl_ElearnSection.SelectedItem.Text;
            btn_reset.Visible = true;
        }
    }

}