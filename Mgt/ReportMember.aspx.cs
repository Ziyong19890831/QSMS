using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_ReportMember : System.Web.UI.Page
{
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfo;
    public Dictionary<string, string> _SetCol;
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfoA;
    public Dictionary<string, string> _SetColA;
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfoB;
    public Dictionary<string, string> _SetColB;
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfoC;
    public Dictionary<string, string> _SetColC;
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfoD;
    public Dictionary<string, string> _SetColD;
    public Dictionary<Dictionary<string, string>, DataTable> _ExcelInfoE;
    public Dictionary<string, string> _SetColE;
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
            Utility.setMStatus(ddl_Status, "請選擇");
            if (userInfo.RoleSNO == "14")
            {
                btnExport.Visible = false;
            }
            if (userInfo.RoleLevel == "3" || userInfo.RoleLevel == "4")
            {
                P_userinfo.Visible = false;
            }
            if (userInfo.RoleOrganType == "A")
            {
                string CodeAreaA = GetAreaCodeA(userInfo.OrganSNO);
                Utility.setAreaCodeB(ddl_AreaCodeB, CodeAreaA, "請選擇");
                Utility.setRoleNormal(ddl_Role, "請選擇");
                Utility.setRoleExpection(ddl_Role2, "請選擇");
                Utility.setCtypeName(ddl_Certificate, "請選擇");
                Utility.setTsType(ddl_TSSNO, "請選擇");
            }
            else
            {
                Utility.setRoleExpection(ddl_Role2, "請選擇");
                Utility.setAreaCodeA(ddl_AreaCodeA, "請選擇");
                //Utility.setRoleFilter(ddl_Role, "IsAdmin=0", "請選擇");
                Utility.setRoleNormal(ddl_Role, "請選擇");
                Utility.setCtypeName(ddl_Certificate, "請選擇");
                Utility.setTsType(ddl_TSSNO, "請選擇");
                //bindData(1);
            }

        }
    }

    /// <summary>
    /// 設定此報表可匯出名稱設定 ,  (注意 : BindData 欄位要包含設定可匯出資料)
    /// </summary>
    private void ReportInit(DataTable dt)
    {

        _ExcelInfo = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetCol = new Dictionary<string, string>();
        _ExcelInfoA = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetColA = new Dictionary<string, string>();
        _ExcelInfoB = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetColB = new Dictionary<string, string>();
        _ExcelInfoC = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetColC = new Dictionary<string, string>();
        _ExcelInfoD = new Dictionary<Dictionary<string, string>, DataTable>();
        _SetColD = new Dictionary<string, string>();
        if (cb_Contract.Checked)
        {
            _ExcelInfoE = new Dictionary<Dictionary<string, string>, DataTable>();
            _SetColE = new Dictionary<string, string>();
        }
            
        
        _SetCol.Add("RoleName", "角色別");
        _SetCol.Add("RESNO", "角色別2");
        _SetCol.Add("Mstatus", "會員狀態");
        _SetCol.Add("PName", "姓名");
        _SetCol.Add("PBirthDate", "出生日期");
        _SetCol.Add("PersonID", "身分證字號");
        _SetCol.Add("PSex", "性別");
        _SetCol.Add("CreateDT", "學員帳號開始日期");
        _SetCol.Add("City", "城市");
        _SetCol.Add("Area", "地區");        
        _SetCol.Add("OrganCode", "自填單位代碼");
        _SetCol.Add("OrganName", "自填單位名稱");
        _SetCol.Add("PTel_O", "電話(公)");
        _SetCol.Add("PFax_O", "傳真(公)");
        _SetCol.Add("PTel", "電話(宅)");
        _SetCol.Add("PFax", "傳真(宅)");
        _SetCol.Add("PPhone", "手機");
        _SetCol.Add("PMail", "信箱");
        _SetCol.Add("PZCode", "區號");
        _SetCol.Add("PAddr", "通訊地址");
        //_SetCol.Add("MName", "學員狀態");
        _SetCol.Add("MNote", "學員狀態備註");
        _SetCol.Add("PAccount", "帳號");
        _SetCol.Add("IsEnable", "帳號啟用");
        _SetCol.Add("PasswordModilyDT", "需修改密碼日期");
        _SetCol.Add("SchoolName", "學校名稱");
        //_SetCol.Add("Major", "科系");
        _SetCol.Add("Degree", "學位");
        _SetCol.Add("JMajor", "現職專科");
        _SetCol.Add("OrganAddr", "機構地址");
        //_SetCol.Add("JobTitle", "職稱");
        //_SetCol.Add("JLicID", "執業證號");
        _SetCol.Add("MName", "學員狀態");
        _SetCol.Add("TsTypeName", "服務科別");
        _SetCol.Add("TsNote", "服務科別(其他)");
        _SetCol.Add("Note", "備註");
        _SetColA.Add("CTypeName", "證書名稱");
        _SetColA.Add("CertID", "證號");
        _SetColA.Add("CertPublicDate", "證書首發日");
        _SetColA.Add("CertStartDate", "證書公告日");
        _SetColA.Add("CertEndDate", "證書到期日");
        _SetColB.Add("MPDegree", "學位(醫)");
        _SetColB.Add("MPSchoolName", "畢業學校(醫)");
        _SetColB.Add("JType", "證書職稱(醫)");
        _SetColB.Add("JCN", "證書字號(醫)");
        _SetColB.Add("VSDate", "執業執照有效期間(起)(醫)");
        _SetColB.Add("VEDate", "執業執照有效期間(訖)(醫)");
        _SetColB.Add("LCN", "執業執照字號(醫)");
        _SetColB.Add("LValid", "執業執照是否有效(醫)");
        _SetColB.Add("LStatus", "執業狀態(醫)");
        _SetColB.Add("LRType", "執業登記科別(醫)");
        _SetColB.Add("LSType", "專科科別(醫)");
        _SetColB.Add("LSCN", "專科證書字號(醫)");
        _SetColB.Add("LSDate", "專科證書有效日期(醫)");
        _SetColB.Add("LSStatus", "專科證書是否有效(醫)");
        _SetColB.Add("MPOrganName", "醫事機構名稱(醫)");
        _SetColB.Add("MPOrganCode", "醫事機構代碼(醫)");
        _SetColB.Add("MPOrganAddr", "醫事機構地址(醫)");
        _SetColB.Add("MPOrganClassName", "醫事機構層級(醫)");
        _SetColC.Add("ELSName", "ELearning課程名稱");
        _SetColC.Add("ELName", "ELearning課程規劃名稱");
        _SetColC.Add("ExamDate", "測驗完成日期");
        _SetColC.Add("IsPass", "是否通過");
        _SetColC.Add("FinishedDate", "課程觀看完成日期");
        _SetColC.Add("CompletedDate", "滿意度完成日期");
        _SetColD.Add("EventName", "實體課程名稱");
        _SetColD.Add("BGrade", "前測成績");
        _SetColD.Add("AGrade", "後測成績");
        if (cb_Contract.Checked)
        {
            _SetColE.Add("HospID", "(合約)機構代碼");
            _SetColE.Add("ContractOrganName", "(合約)機構名稱");
            _SetColE.Add("SMKContractName", "(合約)合約名稱");
            _SetColE.Add("PrsnStartDate", "(合約)起始日");
            _SetColE.Add("PrsnEndDate", "(合約)結束日");
            _SetColE.Add("CouldTreat", "(合約)是否治療");
            _SetColE.Add("CouldInstruct", "(合約)是否衛教");
        }
            
        
        _ExcelInfo.Add(_SetCol, dt);
        _ExcelInfoA.Add(_SetColA, dt);
        _ExcelInfoB.Add(_SetColB, dt);
        _ExcelInfoC.Add(_SetColC, dt);
        _ExcelInfoD.Add(_SetColD, dt);
        if (cb_Contract.Checked)
        {
            _ExcelInfoE.Add(_SetColE, dt);
        }
            
        Session[ReportEnum.ReportMember.ToString()] = _ExcelInfo;
        Session[ReportEnum.ReportMember1.ToString()] = _ExcelInfoA;
        Session[ReportEnum.ReportMember2.ToString()] = _ExcelInfoB;
        Session[ReportEnum.ReportMember3.ToString()] = _ExcelInfoC;
        Session[ReportEnum.ReportMember4.ToString()] = _ExcelInfoD;
        if(cb_Contract.Checked)Session[ReportEnum.ReportMember5.ToString()] = _ExcelInfoE;
    }

    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        String sql = @"                       
              With SearchTable as (
            Select  distinct 
			 P.PersonSNO 
             ,STUFF ( P.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption'
			, p.RoleSNO 
			, R.RoleName 
            , p.PName 
            , p.PBirthDate
            , p.PersonID 
            , CASE WHEN p.PSex = 0 THEN '女' ELSE '男' END PSex
			, PTel_O
			, PFax_O
            , p.PTel
            , PFax
            , PPhone
			, PMail
            , PZCode 
			, p.PAddr
            , D.MName 
			, MNote
			, PAccount
			, CASE P.IsEnable WHEN 0 THEN '停用' WHEN 1 THEN '啟用' ELSE '從未登入' END IsEnable
			, StartDate 
            , EndDate 
			, PasswordModilyDT 
			, p.CreateDT 
            , p.SchoolName
            , p.Degree
            , MP.JCN
            , MP.JDate   
			, O.OrganCode 
			, O.OrganName
            , O.OrganAddr
            , JMajor 
			,MP.LStatus 		
            , P.Note
            ,P.Area,P.City	,P.TSSNO
		    From
		    Person p
		    LEFT JOIN Organ O ON O.OrganSNO = p.OrganSNO
            LEFT JOIN Role R ON R.RoleSNO = p.RoleSNO
            LEFT JOIN QS_MemberStatus D ON D.MStatusSNO = p.MStatusSNO
			LEFT JOIN PersonMP MP ON MP.PersonID=p.PersonID 
            LEFT JOIN Organ OMP ON OMP.OrganCode = MP.OrganCode
            LEFT JOIN Config C On C.Pval=P.MStatusSNO and C.PGroup='Mstatus'
            LEFT JOIN QS_Certificate QCF On QCF.PersonID=P.PersonID
            LEFT JOIN QS_CertificateType QCT ON QCT.CTypeSNO=QCF.CTypeSNO
			LEFT JOIN QS_LearningRecord QLR ON QLR.PersonID=P.PersonID
			LEFT JOIN QS_CourseELearningSection QCES On QCES.ELSCode=QLR.ELSCode
			LEFT JOIN QS_CourseELearning QCE On QCE.ELCode=QCES.ELCode
			LEFT JOIN QS_LearningScore QLS On QLS.PersonID=P.PersonID
			LEFT JOIN QS_LearningAnswer QLA On QLA.PersonID=P.PersonID
			LEFT jOIN QS_LearningFeedback QLF On QLF.QID=QLA.QID
            
            where 1=1
            
";
        Dictionary<string, object> wDict = new Dictionary<string, object>();

        if (userInfo.RoleGroup == "14")//服務窗口
        {
            sql += Utility.setSQLAccess_ByCertificate(wDict, userInfo);
        }
        else
        {
            sql += Utility.setSQLAccess_ByRoleOrganType(wDict, userInfo);
        }
        #region 權限篩選區塊        
       
        #endregion

        #region 查詢篩選區塊

        if (!String.IsNullOrEmpty(ddl_Role.SelectedValue))
        {
            sql += " AND p.RoleSNO = @RoleSNO ";
            wDict.Add("RoleSNO", ddl_Role.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_Role2.SelectedValue))
        {
            sql += " AND RE.RESNO = @RESNO ";
            wDict.Add("RESNO", ddl_Role2.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_Certificate.SelectedValue))
        {
            sql += " AND QCF.CTypeSNO = @CTypeSNO ";
            wDict.Add("CTypeSNO", ddl_Certificate.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_Stutas.SelectedValue))
        {
            sql += " AND P.MStatusSNO = @MStatusSNO ";
            wDict.Add("MStatusSNO", ddl_Stutas.SelectedValue);
        }
        if (!string.IsNullOrEmpty(txt_UnitCode.Text))
        {
            sql += " AND O.OrganCode Like '%' + @OrganCode + '%' ";
            wDict.Add("OrganCode", txt_UnitCode.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_PAccount.Text))
        {
            sql += " AND P.PAccount Like '%' + @PAccount + '%' ";
            wDict.Add("PAccount", txt_PAccount.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_UnitName.Text))
        {
            sql += " AND O.OrganName Like '%' + @OrganName + '%' ";
            wDict.Add("OrganName", txt_UnitName.Text.Trim());
        }

        if (!string.IsNullOrEmpty(ddl_AreaCodeA.SelectedValue))
        {
            sql += " AND O.AreaCodeA = @AreaCodeA ";
            wDict.Add("AreaCodeA", ddl_AreaCodeA.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddl_AreaCodeB.SelectedValue))
        {
            sql += " AND O.AreaCodeB = @AreaCodeB ";
            wDict.Add("AreaCodeB", ddl_AreaCodeB.SelectedValue);
        }
        if (!string.IsNullOrEmpty(txt_PName.Text))
        {
            sql += " AND p.PName Like '%' + @PName + '%' ";
            wDict.Add("PName", txt_PName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_mail.Text))
        {
            sql += " AND p.PMail = @PMail ";
            wDict.Add("PMail", txt_mail.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " AND p.PersonID=@PersonID ";
            wDict.Add("PersonID", txt_PersonID.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_Birthday.Text))
        {
            sql += " AND p.PBirthDate=@PBirthDate";
            wDict.Add("PBirthDate", txt_Birthday.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_Accountday.Text))
        {
            sql += " AND p.CreateDT=@CreateDT";
            wDict.Add("CreateDT", txt_Accountday.Text.Trim());
        }
        if (!String.IsNullOrEmpty(lb_chkl_City_Code.Text))
        {
            sql += " AND p.City In (" + lb_chkl_City.Text + ") ";

        }
        if (!String.IsNullOrEmpty(lb_chkl_City1.Text))
        {
            sql += " AND p.Area In (" + lb_chkl_City1.Text + ") ";

        }
        if (!String.IsNullOrEmpty(txt_CertS.Text))
        {
            sql += " AND QCF.CertID >= @CertID_S";
            wDict.Add("CertID_S", txt_CertS.Text.Trim().PadLeft(6, '0'));
        }
        if (!String.IsNullOrEmpty(txt_CertE.Text))
        {
            sql += " AND QCF.CertID <= @CertID_E";
            wDict.Add("CertID_E", txt_CertE.Text.Trim().PadLeft(6, '0'));
        }
        if (!String.IsNullOrEmpty(txt_CertPDateS.Text))
        {
            sql += " AND QCF.CertPublicDate >= @CertPublicDate_S";
            wDict.Add("CertPublicDate_S", txt_CertPDateS.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_CertPDateE.Text))
        {
            sql += " AND QCF.CertPublicDate <= @CertPublicDate_E";
            wDict.Add("CertPublicDate_E", txt_CertPDateE.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_CertSDateS.Text))
        {
            sql += " AND QCF.CertStartDate >= @CertStartDate_S";
            wDict.Add("CertStartDate_S", txt_CertSDateS.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_CertSDateE.Text))
        {
            sql += " AND QCF.CertStartDate <= @CertStartDate_E";
            wDict.Add("CertStartDate_E", txt_CertSDateE.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_CertEDateS.Text))
        {
            sql += " AND QCF.CertEndDate >= @CertEndDate_S";
            wDict.Add("CertEndDate_S", txt_CertEDateS.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_CertEDateE.Text))
        {
            sql += " AND QCF.CertEndDate <= @CertEndDate_E";
            wDict.Add("CertEndDate_E", txt_CertEDateE.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_GradSchool.Text))
        {
            sql += " AND MP.SchoolName = @MPSchoolName";
            wDict.Add("MPSchoolName", txt_GradSchool.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_Degree.Text))
        {
            sql += " AND MP.Degree = @MPDegree";
            wDict.Add("MPDegree", txt_Degree.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_Jtype.Text))
        {
            sql += " AND MP.Jtype = @Jtype";
            wDict.Add("Jtype", txt_Jtype.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_JCN.Text))
        {
            sql += " AND MP.JCN = @JCN";
            wDict.Add("JCN", txt_JCN.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_JDate.Text))
        {
            sql += " AND MP.JDate = @JDate";
            wDict.Add("JDate", txt_JDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_VSDate.Text))
        {
            sql += " AND MP.VSDate >= @VSDate";
            wDict.Add("VSDate", txt_VSDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_VEDate.Text))
        {
            sql += " AND MP.VEDate <= @VEDate";
            wDict.Add("VEDate", txt_VEDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_LCN.Text))
        {
            sql += " AND MP.LCN = @LCN";
            wDict.Add("LCN", txt_LCN.Text.Trim());
        }
        if (!string.IsNullOrEmpty(ddl_Lvalid.SelectedValue))
        {
            sql += " AND MP.LValid = @LValid ";
            wDict.Add("LValid", ddl_Lvalid.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddl_LStatus.SelectedValue))
        {
            sql += " AND MP.LStatus = @LStatus ";
            wDict.Add("LStatus", ddl_LStatus.SelectedValue);
        }
        if (!String.IsNullOrEmpty(txt_LRType.Text))
        {
            sql += " AND MP.LRType Like '%' + @LRType + '%'";
            wDict.Add("LRType", txt_LRType.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_ElearningCourseName.Text))
        {
            sql += " AND QCES.ELSName Like '%' + @ELSName + '%'";
            wDict.Add("ELSName", txt_ElearningCourseName.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_ElearningPLanName.Text))
        {
            sql += " AND QCE.ELName Like '%' + @ELName + '%'";
            wDict.Add("ELName", txt_ElearningPLanName.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_EViewSDate.Text))
        {
            sql += " AND QLR.FinishedDate >= @FinishedDateS";
            wDict.Add("FinishedDateS", txt_EViewSDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_EViewEDate.Text))
        {
            sql += " AND QLR.FinishedDate <= @FinishedDateE";
            wDict.Add("FinishedDateE", txt_EViewEDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_ETestSDate.Text))
        {
            sql += " AND QLS.ExamDate <= @ExamDateS";
            wDict.Add("ExamDateS", txt_ETestSDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_ETestEDate.Text))
        {
            sql += " AND QLS.ExamDate >= @ExamDateE";
            wDict.Add("ExamDateE", txt_ETestEDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_EFeedSDate.Text))
        {
            sql += " AND QLA.CompletedDate >= @CompletedDateS";
            wDict.Add("CompletedDateS", txt_EFeedSDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_EFeedEDate.Text))
        {
            sql += " AND QLA.CompletedDate <= @CompletedDateE";
            wDict.Add("CompletedDateE", txt_EFeedEDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_LSType.Text))
        {
            sql += " And MP.LSType Like '%' + @LSType + '%'";
            wDict.Add("LSType", txt_LSType.Text.Trim());
        }
        if(!string.IsNullOrEmpty(ddl_Status.SelectedValue))
        {
            sql += " And P.MStatusSNO=@Status";
            wDict.Add("Status", ddl_Status.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddl_TSSNO.SelectedValue))
        {
            sql += " AND P.TSSNO=@TSSNO";
            wDict.Add("TSSNO", ddl_TSSNO.SelectedValue);
        }
        #endregion


        sql += "   ) \n Select ROW_NUMBER() OVER (ORDER BY T.PersonSNO) as ROW_NO	,*from SearchTable T  Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        wDict.Clear();
        if (objDT.Rows.Count > 0)
        {
            int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
            if (page > maxPageNumber) page = maxPageNumber;
            objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
            ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
            gv_Course.DataSource = objDT.DefaultView;
            gv_Course.DataBind();
            //設定匯出資料
           
            if (cb_Enable.Checked && objDT.Rows.Count >1000)
            {
                Response.Write("<script>alert('查詢筆數過多，請縮小範圍。')</script>");
                return;
            }
            else
            {
                reportDataTable();
                //lb_CountP.Text = objDT.Rows.Count.ToString();
            }

            if (objDT.Rows.Count > 0)
            {
                gv_Course.DataSource = objDT.DefaultView;
                gv_Course.DataBind();
            }
            else
            {
                gv_Course.DataSource = null;
                gv_Course.DataBind();
            }

        }
//        else
//        {
//            if (viewrole == 0) return;
//            if (page < 1) page = 1;
//            String sqla = @"           
            
//           With SearchTable as (
//            Select  distinct 
//			 P.PersonSNO 
//             ,STUFF ( P.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption'
//			, p.RoleSNO 
//			, R.RoleName 
//            , p.PName 
//            , p.PBirthDate
//            , p.PersonID 
//            , CASE WHEN p.PSex = 0 THEN '女' ELSE '男' END PSex
//			, PTel_O
//			, PFax_O
//            , p.PTel
//            , PFax
//            , PPhone
//			, PMail
//            , PZCode 
//			, p.PAddr
//            , D.MName 
//			, MNote
//			, PAccount
//			, CASE P.IsEnable WHEN 0 THEN '停用' WHEN 1 THEN '啟用' ELSE '從未登入' END IsEnable
//			, StartDate 
//            , EndDate 
//			, PasswordModilyDT 
//			, p.CreateDT 
//            , p.SchoolName
//            , Major 
//            , p.Degree
//            , MP.JCN
//            , MP.JDate   
//			, O.OrganCode 
//			, O.OrganName
//            , O.OrganAddr
//            , JobTitle
//            , JMajor 
//			, JLicID
//			, CASE WHEN JLicStatus = 0 THEN '終止' ELSE '正常' END JLicStatus
//            , QSExp,MP.LStatus 		
//            , P.Note
//            ,P.Area,P.City	,P.TSSNO
//		    From
//		    Person p
//		    LEFT JOIN Organ O ON O.OrganSNO = p.OrganSNO
//            LEFT JOIN Role R ON R.RoleSNO = p.RoleSNO
//            LEFT JOIN QS_MemberStatus D ON D.MStatusSNO = p.MStatusSNO
//			LEFT JOIN PersonMP MP ON MP.PersonID=p.PersonID 
//            LEFT JOIN Organ OMP ON OMP.OrganCode = MP.OrganCode
//            LEFT JOIN RoleException RE On RE.RESNO=P.RoleException
//            LEFT JOIN Config C On C.Pval=P.MStatusSNO and C.PGroup='Mstatus'
//            LEFT JOIN QS_Certificate QCF On QCF.PersonID=P.PersonID
//            LEFT JOIN QS_CertificateType QCT ON QCT.CTypeSNO=QCF.CTypeSNO
//			LEFT JOIN QS_LearningRecord QLR ON QLR.PersonID=P.PersonID
//			LEFT JOIN QS_CourseELearningSection QCES On QCES.ELSCode=QLR.ELSCode
//			LEFT JOIN QS_CourseELearning QCE On QCE.ELCode=QCES.ELCode
//			LEFT JOIN QS_LearningScore QLS On QLS.PersonID=P.PersonID
//			LEFT JOIN QS_LearningAnswer QLA On QLA.PersonID=P.PersonID
//			LEFT jOIN QS_LearningFeedback QLF On QLF.QID=QLA.QID
            
//            where 1=1
            
//";


//            #region 權限篩選區塊
//            sqla += Utility.setSQLAccess_ByRoleOrganType(wDict, userInfo);

//            #endregion



//            #region 查詢篩選區塊
//            if (!string.IsNullOrEmpty(txt_PAccount.Text))
//            {
//                sql += " AND P.PAccount Like '%' + @PAccount + '%' ";
//                wDict.Add("PAccount", txt_PAccount.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(ddl_Role.SelectedValue))
//            {
//                sqla += " AND p.RoleSNO = @RoleSNO ";
//                wDict.Add("RoleSNO", ddl_Role.SelectedValue);
//            }
//            if (!String.IsNullOrEmpty(ddl_Role2.SelectedValue))
//            {
//                sqla += " AND RE.RESNO = @RESNO ";
//                wDict.Add("RESNO", ddl_Role2.SelectedValue);
//            }
//            if (!String.IsNullOrEmpty(ddl_Certificate.SelectedValue))
//            {
//                sqla += " AND QCF.CTypeSNO = @CTypeSNO ";
//                wDict.Add("CTypeSNO", ddl_Certificate.SelectedValue);
//            }
//            if (!String.IsNullOrEmpty(ddl_Stutas.SelectedValue))
//            {
//                sqla += " AND P.MStatusSNO = @MStatusSNO ";
//                wDict.Add("MStatusSNO", ddl_Stutas.SelectedValue);
//            }
//            if (!string.IsNullOrEmpty(txt_UnitCode.Text))
//            {
//                sqla += " AND O.OrganCode Like '%' + @OrganCode + '%' ";
//                wDict.Add("OrganCode", txt_UnitCode.Text.Trim());
//            }

//            if (!string.IsNullOrEmpty(txt_UnitName.Text))
//            {
//                sqla += " AND O.OrganName Like '%' + @OrganName + '%' ";
//                wDict.Add("OrganName", txt_UnitName.Text.Trim());
//            }

//            if (!string.IsNullOrEmpty(ddl_AreaCodeA.SelectedValue))
//            {
//                sqla += " AND O.AreaCodeA = @AreaCodeA ";
//                wDict.Add("AreaCodeA", ddl_AreaCodeA.SelectedValue);
//            }
//            if (!string.IsNullOrEmpty(ddl_AreaCodeB.SelectedValue))
//            {
//                sqla += " AND O.AreaCodeB = @AreaCodeB ";
//                wDict.Add("AreaCodeB", ddl_AreaCodeB.SelectedValue);
//            }
//            if (!string.IsNullOrEmpty(txt_PName.Text))
//            {
//                sqla += " AND p.PName Like '%' + @PName + '%' ";
//                wDict.Add("PName", txt_PName.Text.Trim());
//            }
//            if (!string.IsNullOrEmpty(txt_mail.Text))
//            {
//                sqla += " AND p.PMail = @PMail ";
//                wDict.Add("PMail", txt_mail.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_PersonID.Text))
//            {
//                sqla += " AND p.PersonID=@PersonID ";
//                wDict.Add("PersonID", txt_PersonID.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_Birthday.Text))
//            {
//                sqla += " AND p.PBirthDate=@PBirthDate";
//                wDict.Add("PBirthDate", txt_Birthday.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_Accountday.Text))
//            {
//                sqla += " AND p.CreateDT=@CreateDT";
//                wDict.Add("CreateDT", txt_Accountday.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(lb_chkl_City_Code.Text))
//            {
//                sqla += " AND p.City In (" + lb_chkl_City.Text + ") ";

//            }
//            if (!String.IsNullOrEmpty(lb_chkl_City1.Text))
//            {
//                sqla += " AND p.Area In (" + lb_chkl_City1.Text + ") ";

//            }
//            if (!String.IsNullOrEmpty(txt_CertS.Text))
//            {
//                sqla += " AND QCF.CertID >= @CertID_S";
//                wDict.Add("CertID_S", txt_CertS.Text.Trim().PadLeft(6, '0'));
//            }
//            if (!String.IsNullOrEmpty(txt_CertE.Text))
//            {
//                sqla += " AND QCF.CertID <= @CertID_E";
//                wDict.Add("CertID_E", txt_CertE.Text.Trim().PadLeft(6, '0'));
//            }
//            if (!String.IsNullOrEmpty(txt_CertPDateS.Text))
//            {
//                sqla += " AND QCF.CertPublicDate >= @CertPublicDate_S";
//                wDict.Add("CertPublicDate_S", txt_CertPDateS.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_CertPDateE.Text))
//            {
//                sqla += " AND QCF.CertPublicDate <= @CertPublicDate_E";
//                wDict.Add("CertPublicDate_E", txt_CertPDateE.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_CertSDateS.Text))
//            {
//                sqla += " AND QCF.CertStartDate >= @CertStartDate_S";
//                wDict.Add("CertStartDate_S", txt_CertSDateS.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_CertSDateE.Text))
//            {
//                sqla += " AND QCF.CertStartDate <= @CertStartDate_E";
//                wDict.Add("CertStartDate_E", txt_CertSDateE.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_CertEDateS.Text))
//            {
//                sqla += " AND QCF.CertEndDate >= @CertEndDate_S";
//                wDict.Add("CertEndDate_S", txt_CertEDateS.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_CertEDateE.Text))
//            {
//                sqla += " AND QCF.CertEndDate <= @CertEndDate_E";
//                wDict.Add("CertEndDate_E", txt_CertEDateE.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_GradSchool.Text))
//            {
//                sqla += " AND MP.SchoolName = @MPSchoolName";
//                wDict.Add("MPSchoolName", txt_GradSchool.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_Degree.Text))
//            {
//                sqla += " AND MP.Degree = @MPDegree";
//                wDict.Add("MPDegree", txt_Degree.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_Jtype.Text))
//            {
//                sqla += " AND MP.Jtype = @Jtype";
//                wDict.Add("Jtype", txt_Jtype.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_JCN.Text))
//            {
//                sqla += " AND MP.JCN = @JCN";
//                wDict.Add("JCN", txt_JCN.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_JDate.Text))
//            {
//                sqla += " AND MP.JDate = @JDate";
//                wDict.Add("JDate", txt_JDate.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_VSDate.Text))
//            {
//                sqla += " AND MP.VSDate >= @VSDate";
//                wDict.Add("VSDate", txt_VSDate.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_VEDate.Text))
//            {
//                sqla += " AND MP.VEDate <= @VEDate";
//                wDict.Add("VEDate", txt_VEDate.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_LCN.Text))
//            {
//                sqla += " AND MP.LCN = @LCN";
//                wDict.Add("LCN", txt_LCN.Text.Trim());
//            }
//            if (!string.IsNullOrEmpty(ddl_Lvalid.SelectedValue))
//            {
//                sqla += " AND MP.LValid = @LValid ";
//                wDict.Add("LValid", ddl_Lvalid.SelectedValue);
//            }
//            if (!string.IsNullOrEmpty(ddl_LStatus.SelectedValue))
//            {
//                sqla += " AND MP.LStatus = @LStatus ";
//                wDict.Add("LStatus", ddl_LStatus.SelectedValue);
//            }
//            if (!String.IsNullOrEmpty(txt_LRType.Text))
//            {
//                sqla += " AND MP.LRType Like '%' + @LRType + '%'";
//                wDict.Add("LRType", txt_LRType.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_ElearningCourseName.Text))
//            {
//                sqla += " AND QCES.ELSName Like '%' + @ELSName + '%'";
//                wDict.Add("ELSName", txt_ElearningCourseName.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_ElearningPLanName.Text))
//            {
//                sqla += " AND QCE.ELName Like '%' + @ELName + '%'";
//                wDict.Add("ELName", txt_ElearningPLanName.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_EViewSDate.Text))
//            {
//                sqla += " AND QLR.FinishedDate >= @FinishedDateS";
//                wDict.Add("FinishedDateS", txt_EViewSDate.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_EViewEDate.Text))
//            {
//                sqla += " AND QLR.FinishedDate <= @FinishedDateE";
//                wDict.Add("FinishedDateE", txt_EViewEDate.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_ETestSDate.Text))
//            {
//                sqla += " AND QLS.ExamDate <= @ExamDateS";
//                wDict.Add("ExamDateS", txt_ETestSDate.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_ETestEDate.Text))
//            {
//                sqla += " AND QLS.ExamDate >= @ExamDateE";
//                wDict.Add("ExamDateE", txt_ETestEDate.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_EFeedSDate.Text))
//            {
//                sqla += " AND QLA.CompletedDate >= @CompletedDateS";
//                wDict.Add("CompletedDateS", txt_EFeedSDate.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_EFeedEDate.Text))
//            {
//                sqla += " AND QLA.CompletedDate <= @CompletedDateE";
//                wDict.Add("CompletedDateE", txt_EFeedEDate.Text.Trim());
//            }
//            if (!String.IsNullOrEmpty(txt_LSType.Text))
//            {
//                sqla += " And MP.LSType Like '%' + @LSType + '%'";
//                wDict.Add("LSType", txt_LSType.Text.Trim());
//            }
//            if (!string.IsNullOrEmpty(ddl_Status.SelectedValue))
//            {
//                sqla += " And P.MStatusSNO=@Status";
//                wDict.Add("Status", ddl_Status.SelectedValue);
//            }
//            if (!string.IsNullOrEmpty(ddl_TSSNO.SelectedValue))
//            {
//                sqla += " AND P.TSSNO=@TSSNO";
//                wDict.Add("TSSNO", ddl_TSSNO.SelectedValue);
//            }
//            #endregion


//            sqla += "   ) \n Select ROW_NUMBER() OVER (ORDER BY T.PersonSNO) as ROW_NO	,*from SearchTable T  Order by ROW_NO";
   
//            DataTable objDTa = objDH.queryData(sqla, wDict);
//            wDict.Clear();
//            int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
//            if (page > maxPageNumber) page = maxPageNumber;
//            objDTa.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
//            if (objDTa.Rows.Count > 0)
//            {
//                gv_Course.DataSource = objDTa.DefaultView;
//                gv_Course.DataBind();
//            }
//            else
//            {
//                gv_Course.DataSource = null;
//                gv_Course.DataBind();
//            }
            
//            //設定匯出資料
//            ltl_PageNumber.Text = Utility.showPageNumber(objDTa.Rows.Count, page, pageRecord, objDT.Rows.Count);
//            reportDataTable();
            
//        }
       
    }

    protected void ddl_AreaCodeA_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_AreaCodeB.Items.Clear();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String AreaCodeA = ddl_AreaCodeA.SelectedValue;
        if (!String.IsNullOrEmpty(AreaCodeA))
        {
            Utility.setAreaCodeB(ddl_AreaCodeB, AreaCodeA, "請選擇");
        }
        else
        {
            ddl_AreaCodeB.Items.Add(new ListItem("請先選擇縣市行政區", ""));
        }
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        bindData(page);
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (gv_Course.Rows.Count == 0)
        {
            Response.Write("<script>alert('尚未查詢')</script>");
            return;
        }
        if (cb_Contract.Checked)
        {
            Utility.OpenExportWindowsForDetail(this, ReportEnum.ReportMember.ToString(), ReportEnum.ReportMember1.ToString(), ReportEnum.ReportMember2.ToString(), ReportEnum.ReportMember3.ToString(), ReportEnum.ReportMember4.ToString(), ReportEnum.ReportMember5.ToString());
        }
        else
        {
            Utility.OpenExportWindowsForDetail(this, ReportEnum.ReportMember.ToString(), ReportEnum.ReportMember1.ToString(), ReportEnum.ReportMember2.ToString(), ReportEnum.ReportMember3.ToString(), ReportEnum.ReportMember4.ToString());
        }
        
    }

    public static string GetAreaCodeA(string OrganSNO)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string SQL = "Select [AreaCodeA] from Organ where OrganSNO=@OrganSNO";
        aDict.Add("OrganSNO", OrganSNO);
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT.Rows[0]["AreaCodeA"].ToString();
        }
        else
        {
            return "1";//找不到所屬地區
        }

    }


    protected void chk_City_CheckedChanged(object sender, EventArgs e)
    {
        Utility.setAreaCodeACheckBoxList(chkl_City);
        if (chk_City.Checked)
        {
            chk_City1.Enabled = true;
        }
        else
        {
            chk_City1.Enabled = false;
        }


    }

    protected void chk_City1_CheckedChanged(object sender, EventArgs e)
    {
        string AreaCodeB = lb_chkl_City_Code.Text;
        //AreaCodeB = AreaCodeB.Substring(0, AreaCodeB.Length - 1);
        string[] AreaCodeBArray = AreaCodeB.Split(',');
        Utility.setAreaCodeACheckBoxList1(CheckBoxList1, AreaCodeBArray);
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (chkl_City.Items.Count > 0)
        {
            string chk_City_Value = "";
            string chk_City_Value_Code = "";
            for (int i = 0; i < chkl_City.Items.Count; i++)
            {
                if (chkl_City.Items[i].Selected)
                {
                    chk_City_Value_Code += chkl_City.Items[i].Value + ",";
                    chk_City_Value += "'" + chkl_City.Items[i].Text + "'" + ",";
                }
            }
            if (chk_City_Value.Length > 0)
            {
                chk_City_Value_Code = chk_City_Value_Code.Substring(0, chk_City_Value_Code.Length - 1);
                chk_City_Value = chk_City_Value.Substring(0, chk_City_Value.Length - 1);
                lb_chkl_City.Text = chk_City_Value;
                lb_chkl_City_Code.Text = chk_City_Value_Code;
            }
            else
            {
                chk_City.Checked = false;
                lb_chkl_City.Text = "";
            }

        }

    }

    protected void btnOK1_Click(object sender, EventArgs e)
    {
        if (CheckBoxList1.Items.Count > 0)
        {
            string chk_City_Value = "";
            for (int i = 0; i < CheckBoxList1.Items.Count; i++)
            {
                if (CheckBoxList1.Items[i].Selected)
                {
                    chk_City_Value += "'" + CheckBoxList1.Items[i].Text + "'" + ",";
                }
            }
            if (chk_City_Value.Length > 0)
            {
                chk_City_Value = chk_City_Value.Substring(0, chk_City_Value.Length - 1);
                lb_chkl_City1.Text = chk_City_Value;
            }
            else
            {
                chk_City1.Checked = false;
                lb_chkl_City1.Text = "";
            }

        }

    }

    public void reportDataTable()
    {
        string condition = "";
        string sql = @"           
            
            Select  distinct 
			 P.PersonSNO 
             ,STUFF ( P.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption'
			, p.RoleSNO 
			, R.RoleName 
            , p.PName 
            , p.PBirthDate
            , p.PersonID 
            , CASE WHEN p.PSex = 0 THEN '女' ELSE '男' END PSex
			, PTel_O
			, PFax_O
            , p.PTel
            , PFax
            , PPhone
			, PMail
            , PZCode 
			, p.PAddr
            , D.MName 
			, MNote
			, PAccount
			, CASE P.IsEnable WHEN 0 THEN '停用' WHEN 1 THEN '啟用' ELSE '從未登入' END IsEnable
			, StartDate 
            , EndDate 
			, PasswordModilyDT 
			, p.CreateDT 
            , p.SchoolName
            , p.Degree
            , OMP.OrganName MPOrganName
			, OMP.OrganCode MPOrganCode
            , OMP.[OrganAddr] MPOrganAddr
            , MP.JCN  
			, O.OrganCode 
			, O.OrganName
            , O.OrganAddr
            , JMajor 

		
            ,MP.LStatus 		
            , P.Note,TC.TsTypeName,P.TsNote,QS.MName
            ,P.Area,P.City,OC.ClassName MPOrganClassName
            ,C.MVal Mstatus,
            QCT.CTypeName,QCF.CertID,convert(varchar, QCF.[CertPublicDate], 111) CertPublicDate,convert(varchar, QCF.[CertStartDate], 111) CertStartDate,convert(varchar, QCF.[CertEndDate], 111) CertEndDate,
            MP.SchoolName MPSchoolName, MP.Degree MPDegree,MP.JType ,MP.JDate,MP.VSDate,MP.VEDate
            ,MP.LCN,MP.LValid,MP.LRType,MP.LSCN,MP.LSDate,MP.LSStatus,MP.LSType";
        if (cb_Contract.Checked)
        {
            sql += @"
            ,GC.SMKContractName,PC.HospID,OGN.OrganName ContractOrganName,PC.PrsnID,PC.PrsnStartDate
            ,PC.PrsnEndDate,GC.SMKContractName
            ,CASE  PC.CouldTreat When 0 THEN '否' When 1 THEN'是' ELSE NULL END CouldTreat
            ,CASE  PC.CouldInstruct When 0 THEN '否' When 1 THEN'是' ELSE NULL END CouldInstruct   ";
        }
            

        if (cb_Enable.Checked)
        {
            sql += @" ,QCE.ELName,QCES.ELSName,
			case QLS.IsPass when '1' then '通過' ELSE '不通過' end IsPass,
			CONVERT(VARCHAR(19), QLS.ExamDate , 111) ExamDate,Convert(varchar,QLA.[CompletedDate],111) CompletedDate,Convert(varchar,QLR.FinishedDate,111) FinishedDate
            ,E.EventName,ED.AGrade,ED.BGrade ";
        }

        sql += @" From
		    Person p
		    LEFT JOIN Organ O ON O.OrganSNO = p.OrganSNO
            LEFT JOIN Role R ON R.RoleSNO = p.RoleSNO
            LEFT JOIN QS_MemberStatus D ON D.MStatusSNO = p.MStatusSNO
			LEFT JOIN PersonMP MP ON MP.PersonID=p.PersonID 
            LEFT JOIN Organ OMP ON OMP.OrganCode = MP.OrganCode
            LEFT JOIN OrganClass OC ON OC.ClassSNO=OMP.OrganClass
            LEFT JOIN Config C On C.Pval=P.MStatusSNO and C.PGroup='Mstatus'            
			LEFT JOIN QS_LearningRecord QLR ON QLR.PersonID=P.PersonID 
			LEFT JOIN QS_CourseELearningSection QCES On QCES.ELSCode=QLR.ELSCode
			LEFT JOIN QS_CourseELearning QCE On QCE.ELCode=QCES.ELCode 
			LEFT JOIN QS_LearningScore QLS On QLS.PersonID=P.PersonID and QLS.ELSCode=QCES.ELSCode
			LEFT JOIN QS_LearningAnswer QLA On QLA.PersonID=P.PersonID 
			LEFT jOIN QS_LearningFeedback QLF On QLF.QID=QLA.QID
            LEFT JOIN QS_MemberStatus QS On QS.MstatusSNO=P.MstatusSNO
            LEFT JOIN EventD ED On ED.PersonSNO=P.PersonSNO
            LEFT JOIN TsTypeClass TC On TC.TSSNO=P.TSSNO
            LEFT JOIN Event E On E.EventSNO=ED.EventSNO
            LEFT JOIN QS_Certificate QCF On QCF.PersonID=P.PersonID
            LEFT JOIN QS_CertificateType QCT ON QCT.CTypeSNO=QCF.CTypeSNO ";
        if (cb_Contract.Checked)
        {
            sql += @"LEFT JOIN PrsnContract PC On PC.PrsnID=p.PersonID
            Left JOIN GenSMKContract GC ON GC.SMKContractType=PC.SMKContractType
            Left Join Organ OGN On OGN.OrganCode=PC.HospID";
        }
        sql += @"     where 1=1 ";         
      
        Dictionary<string, object> wDict = new Dictionary<string, object>();

        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganType(wDict, userInfo);
        #endregion

        #region 查詢篩選區塊

        if (!String.IsNullOrEmpty(ddl_Role.SelectedValue))
        {
            sql += " AND p.RoleSNO = @RoleSNO ";
            wDict.Add("RoleSNO", ddl_Role.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_Role2.SelectedValue))
        {
            sql += " AND RE.RESNO = @RESNO ";
            condition += " ,RE.RESNO ";
            wDict.Add("RESNO", ddl_Role2.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_Certificate.SelectedValue))
        {
            sql += " AND QCF.CTypeSNO = @CTypeSNO ";
            wDict.Add("CTypeSNO", ddl_Certificate.SelectedValue);
        }
        if (!String.IsNullOrEmpty(ddl_Stutas.SelectedValue))
        {
            sql += " AND P.MStatusSNO = @MStatusSNO ";
            wDict.Add("MStatusSNO", ddl_Stutas.SelectedValue);
        }
        if (!string.IsNullOrEmpty(txt_UnitCode.Text))
        {
            sql += " AND O.OrganCode Like '%' + @OrganCode + '%' ";
            wDict.Add("OrganCode", txt_UnitCode.Text.Trim());
        }

        if (!string.IsNullOrEmpty(txt_UnitName.Text))
        {
            sql += " AND O.OrganName Like '%' + @OrganName + '%' ";
            wDict.Add("OrganName", txt_UnitName.Text.Trim());
        }

        if (!string.IsNullOrEmpty(ddl_AreaCodeA.SelectedValue))
        {
            sql += " AND O.AreaCodeA = @AreaCodeA ";
            wDict.Add("AreaCodeA", ddl_AreaCodeA.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddl_AreaCodeB.SelectedValue))
        {
            sql += " AND O.AreaCodeB = @AreaCodeB ";
            wDict.Add("AreaCodeB", ddl_AreaCodeB.SelectedValue);
        }
        if (!string.IsNullOrEmpty(txt_PName.Text))
        {
            sql += " AND p.PName Like '%' + @PName + '%' ";
            wDict.Add("PName", txt_PName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txt_mail.Text))
        {
            sql += " AND p.PMail = @PMail ";
            wDict.Add("PMail", txt_mail.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " AND p.PersonID=@PersonID ";
            wDict.Add("PersonID", txt_PersonID.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_Birthday.Text))
        {
            sql += " AND p.PBirthDate=@PBirthDate";
            wDict.Add("PBirthDate", txt_Birthday.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_Accountday.Text))
        {
            sql += " AND p.CreateDT=@CreateDT";
            wDict.Add("CreateDT", txt_Accountday.Text.Trim());
        }
        if (chk_City.Checked)
        {
            sql += " AND p.City In (" + lb_chkl_City.Text + ")";
            wDict.Add("City", lb_chkl_City.Text);
        }
        if (chk_City1.Checked)
        {
            sql += " AND p.Area In (" + lb_chkl_City1.Text + ")";
            wDict.Add("Area", lb_chkl_City1.Text);
        }
        if (!String.IsNullOrEmpty(txt_CertS.Text))
        {
            sql += " AND QCF.CertID >= @CertID_S";
            wDict.Add("CertID_S", txt_CertS.Text.Trim().PadLeft(6, '0'));
        }
        if (!String.IsNullOrEmpty(txt_CertE.Text))
        {
            sql += " AND QCF.CertID <= @CertID_E";
            wDict.Add("CertID_E", txt_CertE.Text.Trim().PadLeft(6, '0'));
        }
        if (!String.IsNullOrEmpty(txt_CertPDateS.Text))
        {
            sql += " AND QCF.CertPublicDate >= @CertPublicDate_S";
            wDict.Add("CertPublicDate_S", txt_CertPDateS.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_CertPDateE.Text))
        {
            sql += " AND QCF.CertPublicDate <= @CertPublicDate_E";
            wDict.Add("CertPublicDate_E", txt_CertPDateE.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_CertSDateS.Text))
        {
            sql += " AND QCF.CertStartDate >= @CertStartDate_S";
            wDict.Add("CertStartDate_S", txt_CertSDateS.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_CertSDateE.Text))
        {
            sql += " AND QCF.CertStartDate <= @CertStartDate_E";
            wDict.Add("CertStartDate_E", txt_CertSDateE.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_CertEDateS.Text))
        {
            sql += " AND QCF.CertEndDate >= @CertEndDate_S";
            wDict.Add("CertEndDate_S", txt_CertEDateS.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_CertEDateE.Text))
        {
            sql += " AND QCF.CertEndDate <= @CertEndDate_E";
            wDict.Add("CertEndDate_E", txt_CertEDateE.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_GradSchool.Text))
        {
            sql += " AND MP.SchoolName = @MPSchoolName";
            wDict.Add("MPSchoolName", txt_GradSchool.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_Degree.Text))
        {
            sql += " AND MP.Degree = @MPDegree";
            wDict.Add("MPDegree", txt_Degree.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_Jtype.Text))
        {
            sql += " AND MP.Jtype = @Jtype";
            wDict.Add("Jtype", txt_Jtype.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_JCN.Text))
        {
            sql += " AND MP.JCN = @JCN";
            wDict.Add("JCN", txt_JCN.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_JDate.Text))
        {
            sql += " AND MP.JDate = @JDate";
            wDict.Add("JDate", txt_JDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_VSDate.Text))
        {
            sql += " AND MP.VSDate >= @VSDate";
            wDict.Add("VSDate", txt_VSDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_VEDate.Text))
        {
            sql += " AND MP.VEDate <= @VEDate";
            wDict.Add("VEDate", txt_VEDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_LCN.Text))
        {
            sql += " AND MP.LCN = @LCN";
            wDict.Add("LCN", txt_LCN.Text.Trim());
        }
        if (!string.IsNullOrEmpty(ddl_Lvalid.SelectedValue))
        {
            sql += " AND MP.LValid = @LValid ";
            wDict.Add("LValid", ddl_Lvalid.SelectedValue);
        }
        if (!string.IsNullOrEmpty(ddl_LStatus.SelectedValue))
        {
            sql += " AND MP.LStatus = @LStatus ";
            wDict.Add("LStatus", ddl_LStatus.SelectedValue);
        }
        if (!String.IsNullOrEmpty(txt_LRType.Text))
        {
            sql += " AND MP.LRType Like '%' + @LRType + '%'";
            wDict.Add("LRType", txt_LRType.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_ElearningCourseName.Text))
        {
        
            sql += " AND QCES.ELSName Like '%' + @ELSName + '%'";
            wDict.Add("ELSName", txt_ElearningCourseName.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_ElearningPLanName.Text))
        {
      
            sql += " AND QCE.ELName Like '%' + @ELName + '%'";
            wDict.Add("ELName", txt_ElearningPLanName.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_EViewSDate.Text))
        {
            sql += " AND QLR.FinishedDate >= @FinishedDateS";
            wDict.Add("FinishedDateS", txt_EViewSDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_EViewEDate.Text))
        {
            sql += " AND QLR.FinishedDate <= @FinishedDateE";
            wDict.Add("FinishedDateE", txt_EViewEDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_ETestSDate.Text))
        {
            sql += " AND QLS.ExamDate <= @ExamDateS";
            wDict.Add("ExamDateS", txt_ETestSDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_ETestEDate.Text))
        {
            sql += " AND QLS.ExamDate >= @ExamDateE";
            wDict.Add("ExamDateE", txt_ETestEDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_EFeedSDate.Text))
        {
            sql += " AND QLA.CompletedDate >= @CompletedDateS";
            wDict.Add("CompletedDateS", txt_EFeedSDate.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_EFeedSDate.Text))
        {
            sql += " AND QLA.CompletedDate <= @CompletedDateE";
            wDict.Add("CompletedDateE", txt_EFeedSDate.Text.Trim());
        }

        #endregion
        
        DataHelper objDH = new DataHelper();       
        
        DataTable ObjDT = objDH.queryData(sql, wDict);
        ReportInit(ObjDT);
    }

    protected void cb_Enable_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_Enable.Checked)
        {
            P_Learning.Enabled = true;
        }
        else
        {
            P_Learning.Enabled = false;
        }
    }

    protected void cb_Contract_CheckedChanged(object sender, EventArgs e)
    {

    }
}