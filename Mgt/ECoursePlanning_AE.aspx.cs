using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_ECoursePlanning_AE : System.Web.UI.Page
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
            
            String work = "";
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];

            SetDdlCertificateType(ddl_CType, "請選擇");
            ddl_IsEnable.Items.Insert(0, new ListItem("請選擇", ""));
            ddl_IsEnable.Items.Insert(1, new ListItem("是", "1"));
            ddl_IsEnable.Items.Insert(2, new ListItem("否", "0"));
            GetRoleList();
            //GetCourse();

            if (work.Equals("N"))
            {
                newData();
                btnOK.Text = "新增";
            }
            else
            {
                getData();
                Utility.setPlanName(ddl_ElearingClass, "請選擇");
            }
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

    protected void newData()
    {
        Work.Value = "NEW";
    }

    protected void getData()
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("sno", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
            SELECT 
	            *
            From QS_ECoursePlanningClass A WHERE A.EPClassSNO = @sno", aDict);
        if (objDT.Rows.Count > 0)
        {
            if (Convert.ToString(objDT.Rows[0]["PClassSNO"]) != "" || Convert.ToString(objDT.Rows[0]["PClassSNO"])=="0")
            {
                cb_Elearning.Checked = true;
                ddl_ElearingClass.Visible = true;
                ddl_ElearingClass.SelectedValue = Convert.ToString(objDT.Rows[0]["PClassSNO"]);
            }
            else
            {
                cb_Elearning.Checked = false;
                ddl_ElearingClass.Visible = false;
            }
            txt_ID.Value = Convert.ToString(objDT.Rows[0]["EPClassSNO"]);
            txt_Total.Text = Convert.ToString(objDT.Rows[0]["TotalIntegral"]);
            txt_PlanName.Text = Convert.ToString(objDT.Rows[0]["PlanName"]);
            txt_Compulsory_Entity.Text= Convert.ToString(objDT.Rows[0]["Compulsory_Entity"]);
            txt_Compulsory_Practical.Text = Convert.ToString(objDT.Rows[0]["Compulsory_Practical"]);
            txt_Compulsory_Communication.Text = Convert.ToString(objDT.Rows[0]["Compulsory_Communication"]);
            txt_Compulsory_Online.Text = Convert.ToString(objDT.Rows[0]["Compulsory_Online"]);
            txt_CStartYear.Text = Convert.ToString(objDT.Rows[0]["CStartYear"]);
            txt_CEndYear.Text = Convert.ToString(objDT.Rows[0]["CEndYear"]);
            ddl_IsEnable.SelectedValue = Convert.ToBoolean(objDT.Rows[0]["IsEnable"]) == true ? "1" : "0";
            ddl_CType.SelectedValue = Convert.ToString(objDT.Rows[0]["CTypeSNO"]);
            txt_ElearnLimit.Text= Convert.ToString(objDT.Rows[0]["ElearnLimit"]);
        }
    }

    private void GetRoleList()
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT A.RoleSNO , A.RoleName FROM Role A WHERE A.IsAdmin = 0  and A.RoleSNO in ('15','16','17')", aDict);
        cb_Role.DataSource = objDT;
        cb_Role.DataBind();

        //修改預帶勾選
        String work = "";
        if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
        if (!work.Equals("N"))
        {
            String id = Convert.ToString(Request.QueryString["sno"]);
            aDict.Add("sno", id);
            objDT = objDH.queryData(@"SELECT A.RoleSNO FROM QS_ECoursePlanningRole A WHERE A.EPClassSNO = @sno", aDict);
            foreach (DataRow row in objDT.Rows)
            {

                foreach (ListItem item in cb_Role.Items)
                {
                    if (item.Value == row["RoleSNO"].ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }

            }

        }
    }

    //protected void GetCourse()
    //{
    //    string id = Convert.ToString(Request.QueryString["sno"]);
    //    string sql = @"
    //        SELECT ROW_NUMBER() OVER (ORDER BY A.CourseSNO) as ROW_NO , A.CourseSNO,
    //            B.MVal + '課程' Class1  , C.MVal Class2 , A.UnitName , A.CourseName  
    //            , D.MVal Ctype , A.CHour
    //        FROM QS_Course A
    //            Left JOIN Config B ON B.PGroup ='CourseClass1' AND A.Class1 = B.PVal
    //            Left JOIN Config C ON C.PGroup ='CourseClass2' AND A.Class2 = C.PVal
    //            Left JOIN Config D ON D.PGroup ='CourseCType' AND A.CType = D.PVal
    //        Where A.PClassSNO=@sno
    //        ORDER BY B.MVal , C.MVal , A.UnitName
    //    ";
    //    Dictionary<string, object> wDict = new Dictionary<string, object>();
    //    if (string.IsNullOrEmpty(id)) id = "";
    //    wDict.Add("sno", id);

    //    DataHelper objDH = new DataHelper();
    //    DataTable objDT = objDH.queryData(sql, wDict);
    //    gv_Course.DataSource = objDT.DefaultView;
    //    gv_Course.DataBind();

    //}
    protected void btnOK_Click(object sender, EventArgs e)
    {

        string errorMessage = "";
        if (string.IsNullOrEmpty(txt_Total.Text)) errorMessage += "請輸入總學分\\n";
        if (string.IsNullOrEmpty(txt_Compulsory_Entity.Text)) errorMessage += "請輸入必修實體學分\\n";
        if (string.IsNullOrEmpty(txt_Compulsory_Practical.Text)) errorMessage += "請輸入必修實習學分\\n";
        if (string.IsNullOrEmpty(txt_Compulsory_Communication.Text)) errorMessage += "請輸入必修通訊學分\\n";
        if (string.IsNullOrEmpty(txt_Compulsory_Online.Text)) errorMessage += "請輸入必修線上學分\\n";
        if (string.IsNullOrEmpty(txt_PlanName.Text)) errorMessage += "請輸入課程規劃名稱\\n";
        if (string.IsNullOrEmpty(txt_CStartYear.Text) || string.IsNullOrEmpty(txt_CEndYear.Text)) errorMessage += "請輸入起迄年度\\n";
        if (string.IsNullOrEmpty(ddl_CType.SelectedValue)) errorMessage += "請輸入對應證書\\n";
        if (string.IsNullOrEmpty(ddl_IsEnable.SelectedValue)) errorMessage += "請輸入啟用\\n";
        if (cb_Role.Items.Cast<ListItem>().Where(c => c.Selected).Count() == 0) errorMessage += "請輸入適用對象\\n";


        if (txt_PlanName.Text.Length > 50) errorMessage += "課程規劃名稱字數過多\\n";
        if (txt_CStartYear.Text.Length > 4 || txt_CEndYear.Text.Length > 4) errorMessage += "起迄年度字數過多\\n";
        if (string.Compare(txt_CStartYear.Text, txt_CEndYear.Text) > 0) errorMessage += "結束需大於起始\\n";

        //errorMessage非空，傳送錯誤訊息至Client
        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }



        if (Work.Value.Equals("NEW"))
        {
            //判斷總學分不得低於必修課程
            DataHelper objDH = new DataHelper();
            int ElearningSnoreCheck = 0;
            if (cb_Elearning.Checked == true)
            {

                string CompulsorySQL = @"	Select  sum(Compulsory) 課程必修學分
		                                    from QS_Course 
		                                    where PClassSNO='" + ddl_ElearingClass.SelectedValue + "' and Compulsory=1  GROUP BY Compulsory";
                DataTable ObjDT = objDH.queryData(CompulsorySQL, null);
                if (ObjDT.Rows.Count > 0)
                {
                    ElearningSnoreCheck = Convert.ToInt16(ObjDT.Rows[0]["課程必修學分"].ToString());
                }
                
            }
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("PClassSNO", ddl_ElearingClass.SelectedValue);
            aDict.Add("PlanName", txt_PlanName.Text);
            aDict.Add("CStartYear", txt_CStartYear.Text);
            aDict.Add("CEndYear", txt_CEndYear.Text);
            aDict.Add("IsEnable", ddl_IsEnable.SelectedValue);
            aDict.Add("CTypeSNO", ddl_CType.SelectedValue);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            aDict.Add("TotalIntegral", txt_Total.Text);
            aDict.Add("Compulsory_Entity", txt_Compulsory_Entity.Text);
            aDict.Add("Compulsory_Practical", txt_Compulsory_Practical.Text);
            aDict.Add("Compulsory_Communication", txt_Compulsory_Communication.Text);
            aDict.Add("Compulsory_Online", txt_Compulsory_Online.Text);
            aDict.Add("ElearnLimit", txt_ElearnLimit.Text);
            string pclassId = "";
            
            if (Convert.ToInt16(txt_Compulsory_Entity.Text) + Convert.ToInt16(txt_Compulsory_Practical.Text) + Convert.ToInt16(txt_Compulsory_Communication.Text) + Convert.ToInt16(txt_Compulsory_Online.Text) + ElearningSnoreCheck > Convert.ToInt16(txt_Total.Text))
            {
                Response.Write("<script>alert('目前繫結之課程必修學分為" + ElearningSnoreCheck + "，各學分加總不得大於總學分'); </script>");
                return;
            }
           
            DataTable pClassResult = objDH.queryData(@"
                INSERT INTO QS_ECoursePlanningClass(PClassSNO, PlanName, CStartYear, CEndYear, IsEnable, CTypeSNO, TotalIntegral ,Compulsory_Entity,Compulsory_Practical,Compulsory_Communication,Compulsory_Online, CreateUserID)
                VALUES(@PClassSNO , @PlanName,@CStartYear, @CEndYear, @IsEnable, @CTypeSNO, @TotalIntegral, @Compulsory_Entity,@Compulsory_Practical,@Compulsory_Communication,@Compulsory_Online, @CreateUserID) SELECT @@IDENTITY AS 'Identity'
            ", aDict);

            if (pClassResult.Rows.Count > 0)
            {
                pclassId = pClassResult.Rows[0]["Identity"].ToString();
                UpdateCourseRole(pclassId);
            }

            Response.Write("<script>alert('新增成功!');document.location.href='./ECoursePlanning.aspx'; </script>");
        }
        else
        {
            int ElearningSnoreCheck=0;
            DataHelper objDH = new DataHelper();
            if (cb_Elearning.Checked == true)
            {
               
                string CompulsorySQL = @"	SELECT COALESCE((	Select  SUM(CHour) 課程必修學分
		                                    from QS_Course 
		                                    where PClassSNO='" + ddl_ElearingClass.SelectedValue + "' and Compulsory=1  GROUP BY CHour),0) as 課程必修學分";
                DataTable ObjDT = objDH.queryData(CompulsorySQL, null);
                string Compulsory = ObjDT.Rows[0]["課程必修學分"].ToString();
                if (Compulsory != "")
                {
                    ElearningSnoreCheck = Convert.ToInt16(ObjDT.Rows[0]["課程必修學分"].ToString());
                }
                
            }
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("EPClassSNO", txt_ID.Value);
            aDict.Add("PClassSNO", ddl_ElearingClass.SelectedValue);
            aDict.Add("TotalIntegral", txt_Total.Text);
            aDict.Add("PlanName", txt_PlanName.Text);
            aDict.Add("CStartYear", txt_CStartYear.Text);
            aDict.Add("CEndYear", txt_CEndYear.Text);
            aDict.Add("IsEnable", ddl_IsEnable.SelectedValue);
            aDict.Add("CTypeSNO", ddl_CType.SelectedValue);
            aDict.Add("ModifyDT", Convert.ToDateTime(DateTime.Now));
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            aDict.Add("Compulsory_Entity", txt_Compulsory_Entity.Text);
            aDict.Add("Compulsory_Practical", txt_Compulsory_Practical.Text);
            aDict.Add("Compulsory_Communication", txt_Compulsory_Communication.Text);
            aDict.Add("Compulsory_Online", txt_Compulsory_Online.Text);
            aDict.Add("ElearnLimit", txt_ElearnLimit.Text);
            if (Convert.ToInt16(txt_Compulsory_Entity.Text)+ Convert.ToInt16(txt_Compulsory_Practical.Text)+ Convert.ToInt16(txt_Compulsory_Communication.Text)+ Convert.ToInt16(txt_Compulsory_Online.Text)+ElearningSnoreCheck > Convert.ToInt16(txt_Total.Text))
            {
                Response.Write("<script>alert('目前繫結之課程必修學分為"+ ElearningSnoreCheck + "，各學分加總不得大於總學分'); </script>");
                return;
            }
            objDH.executeNonQuery(@"
                                 UPDATE QS_ECoursePlanningClass SET 
                                        PClassSNO=@PClassSNO,
                                        PlanName = @PlanName, 
                                        TotalIntegral=@TotalIntegral,
                                        CStartYear = @CStartYear, 
                                        CEndYear = @CEndYear, 
                                        IsEnable = @IsEnable, 
                                        CTypeSNO = @CTypeSNO, 
                                        ElearnLimit=@ElearnLimit,
                                        ModifyDT = @ModifyDT, 
                                        Compulsory_Entity=@Compulsory_Entity,
                                        Compulsory_Practical=@Compulsory_Practical,
                                        Compulsory_Communication=@Compulsory_Communication,
                                        Compulsory_Online=@Compulsory_Online,
                                        ModifyUserID = @ModifyUserID
                                WHERE EPClassSNO = @EPClassSNO
                ", aDict);

            UpdateCourseRole(txt_ID.Value);
            Response.Write("<script>alert('修改成功!');document.location.href='./ECoursePlanning.aspx'; </script>");
        }
    }

    private void UpdateCourseRole(string pClassId)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("EPClassSNO", pClassId);
        DataHelper objDH = new DataHelper();
        //清空選單
        string delSQL = "Delete QS_ECoursePlanningRole Where EPClassSNO=@EPClassSNO";
        objDH.executeNonQuery(delSQL, aDict);
        aDict.Clear();

        aDict.Add("EPClassSNO", pClassId);
        aDict.Add("CreateUserID", userInfo.PersonSNO);
        //寫入選單
        string insertSQL = "";
        int run = 1;
        foreach (ListItem item in cb_Role.Items)
        {
            if (item.Selected)
            {
                aDict.Add(string.Format("RoleSNO_{0}", run), item.Value);
                insertSQL += String.Format(@"INSERT INTO QS_ECoursePlanningRole (EPClassSNO,RoleSNO,CreateUserID)
						           VALUES (@EPClassSNO , @RoleSNO_{0} , @CreateUserID);"
                 , run);
                run += 1;
            }
        }
        if (!string.IsNullOrEmpty(insertSQL)) objDH.executeNonQuery(insertSQL, aDict);
    }


    protected void cb_Elearning_CheckedChanged(object sender, EventArgs e)
    {
        ddl_ElearingClass.Visible = true;
        Utility.setPlanName(ddl_ElearingClass,"請選擇");
    }
}
