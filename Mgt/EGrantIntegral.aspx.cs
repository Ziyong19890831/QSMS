using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_EGrantIntegral : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    int viewrole = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //bindData(1);
            Utility.setCtypeName(ddl_CtypeName, "請選擇課程");
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        DataHelper objDH = new DataHelper();
        DataTable objDT;

        string sql = @"  
                      With 
                	getAllParts As (
                		Select ces.ELSCode, ces.ELSPart
                		From QS_CourseELearningSection ces 
						Left Join QS_Course QC On QC.ELSCode=ces.ELSCode
						where QC.Class1 in(3,5)
                	)
                
                	--step1-2 取得學員上課之統計節數
                	, getLearningParts As (
                		Select PersonID, ELSCode, Count(1) FinishedParts 
                		From (
                			Select Distinct lr.PersonID, lr.ELSCode, lr.ELSPart
                			From QS_LearningRecord lr 
                				Left Join QS_CourseELearningSection ces ON ces.ELSCode=lr.ELSCode
								Left Join QS_Course QC On QC.ELSCode=ces.ELSCode
								where QC.Class1 in(3,5)
                			--Where PersonID=@PersonID
                		) t
                		Group By PersonID, ELSCode
                	)
                
                	--step1-3 取得學員已完成課程之紀錄
                	, getFinishedRecord As (
                		Select 
                			ap.ELSCode, ap.ELSPart, lp.PersonID
                		From getAllParts ap
                			Left Join getLearningParts lp ON lp.ELSCode=ap.ELSCode 
                		Where ap.ELSPart<=lp.FinishedParts 
                	)
                
                	--step2 該學員是否已完成課程的測驗
                	, getFinishedExam As (
                		Select Distinct ls.PersonID, ls.ELSCode, ls.ExamDate, ls.Score, ls.IsPass 
                		From QS_LearningScore ls 
						Left Join QS_Course QC On QC.ELSCode=ls.ELSCode

                		Where 
                		--PersonID=@PersonID And 
                		ls.IsPass=1 and QC.Class1 in(3,5)
                	) 
                
                	--step3 該學員是否已完成課程的滿意度調查
                	, getFinishedFeedback As (
                		Select Distinct la.PersonID, lf.ELSCode, lf.FBID 
                		From QS_LearningAnswer la
                			Left Join QS_LearningFeedback lf ON lf.QID=la.QID
							Left Join QS_Course QC On QC.ELSCode=lf.ELSCode

                		Where QC.Class1 in(3,5)
                	) 
                
                	--step4 取得ELSCode對應的CourseSNO
                	, getCourseSNO As (
                		Select c.CourseSNO, ces.ELSCode
                		From QS_CourseELearningSection ces
                			Left Join QS_Course c ON c.ELSCode=ces.ELSCode
							Where c.Class1 in(3,5)
                	) 
                
                	----step5 取得該學員所有紀錄
                	, getAllFinishedCourse As (
                		SELECT c.CourseSNO,QC.CourseName, QC.PClassSNO,
                		fe.PersonID ,P.PersonSNO
                		,case when fr.ELSCode is not null then '已觀看'　else '未觀看' END 'Record'
                		,case Convert(varchar(10),fe.IsPass) when 1 then '通過' Else '不通過' END 'Exam'
                		,case when ff.ELSCode is not null then '已填寫'　else '未填寫' END'Feedback'
                		From getCourseSNO c
							Left Join QS_Course Qc ON Qc.CourseSNO=C.CourseSNO
							Left Join getFinishedExam		fe ON fe.ELSCode=c.ELSCode 
                			Left Join getFinishedRecord		fr ON fr.ELSCode=c.ELSCode And fr.PersonID=fe.PersonID
                			Left Join getFinishedFeedback	ff ON ff.ELSCode=c.ELSCode And ff.PersonID=fe.PersonID
							LEFT JOIN Person P ON fr.PersonID=P.PersonID
                		--Where c.CourseSNO Is Not Null And 
                		--	  fr.ELSCode Is Not Null And 
                		--	  fe.ELSCode Is Not Null And 
                		--	  ff.ELSCode Is Not Null
                		where c.CourseSNO is not null and Qc.Class1 in(3,5)
                	) ,getall as (
                    --取得所有學員的E-Learning紀錄
                	    Select distinct ROW_NUMBER() OVER (ORDER BY P.personSNO) AS ROW_NO, GAFC.CourseSNO
						,p.PName
						,p.PersonSNO
                        ,STUFF ( P.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption'
						,GAFC.PersonID       
						,GAFC.CourseName
						,QCT.CTypeName
						,GAFC.Exam
						,GAFC.Feedback
						,GAFC.Record　		
						,I.ISNO			
                        ,R.RoleLevel,O.AreaCodeA,O.AreaCodeB,R.RoleGroup,QCT.CtypeSNO
						--,(Select 1 From QS_Integral QI Where QI.CourseSNO=GAFC.CourseSNO and QI.PersonSNO=GAFC.PersonSNO) Chk
						from getAllFinishedCourse GAFC
						Left JOIN Person P ON P.PersonID=GAFC.PersonID
						Left Join QS_ECoursePlanningClass QCPC On QCPC.PClassSNO=GAFC.PClassSNO
						Left Join QS_CertificateType QCT On QCT.CTypeSNO=QCPC.CTypeSNO
                        LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
                        LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
						LEFT JOIN QS_Integral I ON I.CourseSNO=GAFC.CourseSNO and I.PersonSNO=P.PersonSNO					
                        Where 1 = 1 )
                    Select distinct ROW_NUMBER() OVER (ORDER BY getall.personSNO) AS ROW_NO,* from getall where 1=1
            ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();

        #region 權限篩選區塊
        sql += Utility.setSQLAccess_ByRoleOrganTypeForEintegral(wDict, userInfo);
        #endregion

        #region 查詢篩選區塊
        if (!String.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " AND getall.PersonID Like '%' + @PersonID + '%' ";
            wDict.Add("PersonID", txt_PersonID.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_Person.Text))
        {
            sql += " AND getall.PName =@PName ";
            wDict.Add("PName", txt_Person.Text.Trim());
        }
        if (!String.IsNullOrEmpty(txt_CourseName.Text))
        {
            sql += " AND getall.CourseName Like '%' + @txt_CourseName + '%' ";
            wDict.Add("txt_CourseName", txt_CourseName.Text.Trim());
        }
        if (!String.IsNullOrEmpty(ddl_CtypeName.SelectedValue))
        {
            sql += " AND getall.CTypeSNO=@CTypeSNO ";
            wDict.Add("CTypeSNO", ddl_CtypeName.SelectedValue);
        }
        //if (!String.IsNullOrEmpty(txt_PersonID.Text))
        //{
        //    sql += " AND p.PersonID Like '%' + @PersonID + '%' ";
        //    wDict.Add("PersonID", txt_PersonID.Text.Trim());
        //}
        #endregion
        objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_LearningRecord.DataSource = objDT;
        gv_LearningRecord.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }

    protected void Btn_Grant_Click(object sender,EventArgs e)
    {
        
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

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!IsPostBack)
        {
           
        }
    }
    protected void gv_LearningRecord_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Button myButton = (Button)e.CommandSource;
        GridViewRow myRow = (GridViewRow)myButton.NamingContainer;
        string GVCourseSNO = myRow.Cells[1].Text;
        string GVPersonSNO = myRow.Cells[3].Text;

        string sql = "Insert into QS_Integral (PersonSNO,CourseSNO,CreateUserID) values(@GVPersonSNO,@GVCourseSNO,@CreateUserID)";

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("GVPersonSNO", GVPersonSNO);
        aDict.Add("GVCourseSNO", GVCourseSNO);
        aDict.Add("CreateUserID",userInfo.PersonSNO);
        DataHelper DH = new DataHelper();
        DataTable DT = DH.queryData(sql, aDict);
        bindData(1);

    }

    protected void gv_LearningRecord_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        { 
            //要隱藏的欄位，如果前台filed隱藏會抓不到值，所以在RowCreate時才隱藏
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[3].Visible = false;
        }
    }

    protected void gv_LearningRecord_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem != null)
        {

            Button lblCustomerName = (Button)e.Row.FindControl("Btn_Grant");
            //Cast DataItem into your data object that you bound with Grid. I am assuming that you bound you grid with DataTable and CustomerName is a column in that table.
            string a = e.Row.Cells[8].Text;
            if (Convert.ToString(lblCustomerName.Text) == "")
            {
                lblCustomerName.Text = "授予";
            }
            else
            {
                lblCustomerName.Text = "已授予";
                lblCustomerName.Enabled = false;
            }
        }
    }
}