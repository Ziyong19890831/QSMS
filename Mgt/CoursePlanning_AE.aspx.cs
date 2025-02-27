using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_CoursePlanning_AE : System.Web.UI.Page
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
            GetCourse();

            if (work.Equals("N"))
            {
                newData();
                btnOK.Text = "新增";
            }
            else
            {
                getData();
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
	            A.PClassSNO, A.PlanName , A.CStartYear , A.CEndYear ,A.TargetIntegral
	            ,A.IsEnable , A.CTypeSNO ,A.SignLimit
            From QS_CoursePlanningClass A WHERE A.PClassSNO = @sno", aDict);
        if (objDT.Rows.Count > 0)
        {
            txt_ID.Value = Convert.ToString(objDT.Rows[0]["PClassSNO"]);
            txt_PlanName.Text = Convert.ToString(objDT.Rows[0]["PlanName"]);
            txt_TargetIntegral.Text= Convert.ToString(objDT.Rows[0]["TargetIntegral"]);
            txt_SignLimit.Text = Convert.ToString(objDT.Rows[0]["SignLimit"]);
            txt_CStartYear.Text = Convert.ToString(objDT.Rows[0]["CStartYear"]);
            txt_CEndYear.Text = Convert.ToString(objDT.Rows[0]["CEndYear"]);
            ddl_IsEnable.SelectedValue = Convert.ToBoolean(objDT.Rows[0]["IsEnable"]) == true ? "1" : "0";
            ddl_CType.SelectedValue = Convert.ToString(objDT.Rows[0]["CTypeSNO"]);
        }
    }

    private void GetRoleList()
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT A.RoleSNO , A.RoleName FROM Role A WHERE A.IsAdmin = 0 and A.RoleSNO in ('15','16','17')", aDict);
        cb_Role.DataSource = objDT;
        cb_Role.DataBind();

        //修改預帶勾選
        String work = "";
        if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
        if (!work.Equals("N"))
        {
            String id = Convert.ToString(Request.QueryString["sno"]);
            aDict.Add("sno", id);
            objDT = objDH.queryData(@"SELECT A.RoleSNO FROM QS_CoursePlanningRole A WHERE A.PClassSNO = @sno", aDict);
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

    protected void GetCourse()
    {
        string id = Convert.ToString(Request.QueryString["sno"]);
        string sql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY A.CourseSNO) as ROW_NO , A.CourseSNO,
                B.MVal + '課程' Class1  , C.MVal Class2 , A.UnitName , A.CourseName  
                , D.MVal Ctype , A.CHour , Case A.IsEnable When 1 then '是' ELSE '否' END IsEnable
            FROM QS_Course A
                Left JOIN Config B ON B.PGroup ='CourseClass1' AND A.Class1 = B.PVal
                Left JOIN Config C ON C.PGroup ='CourseClass2' AND A.Class2 = C.PVal
                Left JOIN Config D ON D.PGroup ='CourseCType' AND A.CType = D.PVal
            Where A.PClassSNO=@sno
            ORDER BY B.MVal , C.MVal , A.UnitName
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        if (string.IsNullOrEmpty(id)) id = "";
        wDict.Add("sno", id);

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        gv_Course.DataSource = objDT.DefaultView;
        gv_Course.DataBind();

    }
    protected void btnOK_Click(object sender, EventArgs e)
    {

        string errorMessage = "";
        if (txt_PlanName.Text.Length > 50) errorMessage += "課程規劃名稱字數過多\\n";
        if (txt_CStartYear.Text.Length > 4 || txt_CEndYear.Text.Length > 4) errorMessage += "起迄年度字數過多\\n";
        if (string.Compare(txt_CStartYear.Text, txt_CEndYear.Text) > 0) errorMessage += "結束需大於起始\\n";
        if (cb_Role.Items.Cast<ListItem>().Where(c => c.Selected).Count() == 0) errorMessage += "請輸入適用對象\\n";
        //errorMessage非空，傳送錯誤訊息至Client
        if (string.IsNullOrEmpty(txt_TargetIntegral.Text)) errorMessage += "請輸入目標積分\\n";
        if (string.IsNullOrEmpty(txt_PlanName.Text)) errorMessage += "請輸入課程規劃名稱\\n";
        if (string.IsNullOrEmpty(txt_CStartYear.Text) || string.IsNullOrEmpty(txt_CEndYear.Text)) errorMessage += "請輸入起迄年度\\n";
        if (string.IsNullOrEmpty(ddl_CType.SelectedValue)) errorMessage += "請輸入對應證書\\n";
        if (string.IsNullOrEmpty(ddl_IsEnable.SelectedValue)) errorMessage += "請輸入啟用\\n";
        if (string.IsNullOrEmpty(txt_SignLimit.Text)) errorMessage += "請輸入核心門檻積分\\n";

        if (!String.IsNullOrEmpty(errorMessage))
        {
            Utility.showMessage(Page, "ErrorMessage", errorMessage);
            return;
        }

        //if (Convert.ToInt16(txt_SignLimit.Text) > Convert.ToInt16( txt_TargetIntegral.Text))
        //{
        //    errorMessage += "門檻積分不得大於目標積分\\n";
        //    return;
        //}
       


      
       

        string id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        string sql = @"
            SELECT ROW_NUMBER() OVER (ORDER BY A.CourseSNO) as ROW_NO , A.CourseSNO,
                B.MVal + '課程' Class1  , C.MVal Class2 , A.UnitName , A.CourseName  
                , D.MVal Ctype , A.CHour
            FROM QS_Course A
                Left JOIN Config B ON B.PGroup ='CourseClass1' AND A.Class1 = B.PVal
                Left JOIN Config C ON C.PGroup ='CourseClass2' AND A.Class2 = C.PVal
                Left JOIN Config D ON D.PGroup ='CourseCType' AND A.CType = D.PVal
            Where A.PClassSNO=@sno
            ORDER BY B.MVal , C.MVal , A.UnitName
        ";
        int Chour = 0;
        DataHelper objCDH = new DataHelper();
        if (string.IsNullOrEmpty(id)) id = "";
        wDict.Add("sno", id);
        DataTable objCDT = objCDH.queryData(sql, wDict);
        int Total = 0;
        for(int i=0;i< objCDT.Rows.Count; i++)
        {
            Total += Convert.ToInt16(objCDT.Rows[i]["CHour"].ToString());
            if (objCDT.Rows[i]["Ctype"].ToString()== "線上")
            {
                Chour += Convert.ToInt16(objCDT.Rows[i]["CHour"].ToString());
            }

        }
        

        if (Work.Value.Equals("NEW"))
        {

            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("PlanName", txt_PlanName.Text);
            aDict.Add("CStartYear", txt_CStartYear.Text);
            aDict.Add("CEndYear", txt_CEndYear.Text);
            aDict.Add("IsEnable", ddl_IsEnable.SelectedValue);
            aDict.Add("CTypeSNO", ddl_CType.SelectedValue);
            aDict.Add("CreateUserID", userInfo.PersonSNO);
            aDict.Add("TargetIntegral", txt_TargetIntegral.Text);
            aDict.Add("SignLimit", txt_SignLimit.Text);
            string pclassId = "";
            DataHelper objDH = new DataHelper();
            DataTable pClassResult = objDH.queryData(@"
                INSERT INTO QS_CoursePlanningClass(PlanName, CStartYear, CEndYear, IsEnable, CTypeSNO, TargetIntegral,SignLimit, CreateUserID)
                VALUES(@PlanName,@CStartYear, @CEndYear, @IsEnable, @CTypeSNO, @TargetIntegral,@SignLimit, @CreateUserID) SELECT @@IDENTITY AS 'Identity'
            ", aDict);
            if (pClassResult.Rows.Count > 0)
            {
                pclassId = pClassResult.Rows[0]["Identity"].ToString();
                UpdateCourseRole(pclassId);
            }

            Response.Write("<script>alert('新增成功!');document.location.href='./CoursePlanning.aspx'; </script>");
        }
        else
        {
            if (Convert.ToInt16(txt_SignLimit.Text) > Chour)
            {
                Response.Write("<script>alert('實體報名門檻積分不得大於所有線上課程積分"+ Chour +"積分!'); </script>");
                return;
            }
            //if (Convert.ToInt16(txt_TargetIntegral.Text) > Total)
            //{
            //    Response.Write("<script>alert('目標積分不得大於課程總積分" + Total + "積分!!'); </script>");
            //    return;
            //}
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("PClassSNO", txt_ID.Value);
            aDict.Add("PlanName", txt_PlanName.Text);
            aDict.Add("CStartYear", txt_CStartYear.Text);
            aDict.Add("CEndYear", txt_CEndYear.Text);
            aDict.Add("IsEnable", ddl_IsEnable.SelectedValue);
            aDict.Add("CTypeSNO", ddl_CType.SelectedValue);
            aDict.Add("ModifyDT", Convert.ToDateTime(DateTime.Now));
            aDict.Add("ModifyUserID", userInfo.PersonSNO);
            aDict.Add("TargetIntegral", txt_TargetIntegral.Text);
            aDict.Add("SignLimit", txt_SignLimit.Text);
            DataHelper objDH = new DataHelper();
            objDH.executeNonQuery(@"
                                 UPDATE QS_CoursePlanningClass SET 
                                        PlanName = @PlanName, 
                                        CStartYear = @CStartYear, 
                                        CEndYear = @CEndYear, 
                                        IsEnable = @IsEnable, 
                                        CTypeSNO = @CTypeSNO, 
                                        ModifyDT = @ModifyDT, 
                                        TargetIntegral=@TargetIntegral,
                                        SignLimit=@SignLimit,
                                        ModifyUserID = @ModifyUserID
                                WHERE PClassSNO = @PClassSNO
                ", aDict);

            UpdateCourseRole(txt_ID.Value);
            Response.Write("<script>alert('修改成功!');document.location.href='./CoursePlanning.aspx'; </script>");
        }
    }

    private void UpdateCourseRole(string pClassId)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("PClassSNO", pClassId);
        DataHelper objDH = new DataHelper();
        //清空選單
        string delSQL = "Delete QS_CoursePlanningRole Where PClassSNO=@PClassSNO";
        objDH.executeNonQuery(delSQL, aDict);
        aDict.Clear();

        aDict.Add("PClassSNO", pClassId);
        aDict.Add("CreateUserID", userInfo.PersonSNO);
        //寫入選單
        string insertSQL = "";
        int run = 1;
        foreach (ListItem item in cb_Role.Items)
        {
            if (item.Selected)
            {
                aDict.Add(string.Format("RoleSNO_{0}", run), item.Value);
                insertSQL += String.Format(@"INSERT INTO QS_CoursePlanningRole (PClassSNO,RoleSNO,CreateUserID)
						           VALUES (@PClassSNO , @RoleSNO_{0} , @CreateUserID);"
                 , run);
                run += 1;
            }
        }
        if (!string.IsNullOrEmpty(insertSQL)) objDH.executeNonQuery(insertSQL, aDict);
    }

}
