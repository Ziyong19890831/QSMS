using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Personnel_ElearningRecord : System.Web.UI.Page
{
    UserInfo userInfo = null;
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
        else Response.Redirect("../Default.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindData(1);
        }
        
    }
    protected void bindData(int page)
    {
        
        if (page < 1) page = 1;
        int pageRecord = 10;
        DataHelper objDH = new DataHelper();
        DataTable objDT;

        string sql = @"
        
          With
                    getAllParts As(
                        Select ces.ELSCode, ces.ELSPart

                        From QS_CourseELearningSection ces

                        Left Join QS_Course QC On QC.ELSCode = ces.ELSCode

                    )

                    --step1 - 2 取得學員上課之統計節數
                	, getLearningParts As(
                        Select PersonID, ELSCode, Count(1) FinishedParts

                        From(
                            Select Distinct lr.PersonID, lr.ELSCode, lr.ELSPart

                            From QS_LearningRecord lr

                                Left Join QS_CourseELearningSection ces ON ces.ELSCode = lr.ELSCode

                                Left Join QS_Course QC On QC.ELSCode = ces.ELSCode

                            --Where PersonID = @PersonID
                        ) t
                        Group By PersonID, ELSCode
                	)
                
                	--step1 - 3 取得學員已完成課程之紀錄
                	, getFinishedRecord As(
                        Select
                            ap.ELSCode, ap.ELSPart, lp.PersonID
                        From getAllParts ap

                            Left Join getLearningParts lp ON lp.ELSCode= ap.ELSCode

                        Where ap.ELSPart<= lp.FinishedParts
                	)
                
                	--step2 該學員是否已完成課程的測驗
                	, getFinishedExam As(
                        Select Distinct ls.PersonID, ls.ELSCode, ls.ExamDate, ls.Score, ls.IsPass
                        From QS_LearningScore ls

                        Left Join QS_Course QC On QC.ELSCode= ls.ELSCode


                        Where

                        --PersonID = @PersonID And

                        ls.IsPass = 1
                	) 
                
                	--step3 該學員是否已完成課程的滿意度調查
                	, getFinishedFeedback As(
                        Select Distinct la.PersonID, lf.ELSCode, lf.FBID
                        From QS_LearningAnswer la

                            Left Join QS_LearningFeedback lf ON lf.QID= la.QID

                            Left Join QS_Course QC On QC.ELSCode= lf.ELSCode


                    )


                    --step4 取得ELSCode對應的CourseSNO
                	, getCourseSNO As(
                        Select c.CourseSNO, ces.ELSCode
                        From QS_CourseELearningSection ces

                            Left Join QS_Course c ON c.ELSCode= ces.ELSCode

                    )


                    ----step5 取得該學員所有紀錄
                	, getAllFinishedCourse As(
                        SELECT c.CourseSNO, QC.CourseName, QC.PClassSNO,
                        fe.PersonID , P.PersonSNO, Qc.Class1,fe.ELSCode

                        ,case when fr.ELSCode is not null then '已觀看' else '未觀看' END 'Record'
                		,case Convert(varchar(10), fe.IsPass) when 1 then '通過' Else '不通過' END 'Exam'
                		,case when ff.ELSCode is not null then '已填寫' else '未填寫' END'Feedback'

                        From getCourseSNO c
                            Left Join QS_Course Qc ON Qc.CourseSNO = C.CourseSNO

                            Left Join getFinishedExam fe ON fe.ELSCode = c.ELSCode

                            Left Join getFinishedRecord fr ON fr.ELSCode = c.ELSCode And fr.PersonID = fe.PersonID

                            Left Join getFinishedFeedback ff ON ff.ELSCode = c.ELSCode And ff.PersonID = fe.PersonID

                            LEFT JOIN Person P ON fr.PersonID = P.PersonID
                        --Where c.CourseSNO Is Not Null And
                        --    fr.ELSCode Is Not Null And
                        --    fe.ELSCode Is Not Null And
                        --    ff.ELSCode Is Not Null

                        where c.CourseSNO is not null
                	) ,getallData as (
                    --取得所有學員的E - Learning紀錄

                        Select distinct GAFC.CourseSNO
						,p.PName
						,p.PersonSNO
						,GAFC.PersonID
						,GAFC.CourseName
						,QCT.CTypeName
						,GAFC.Exam
						,GAFC.Feedback
						,GAFC.Record
						,QCT.CTypeSNO
						,I.ISNO
						,GAFC.Class1
						,GAFC.ELSCode
                        --,(Select 1 From QS_Integral QI Where QI.CourseSNO = GAFC.CourseSNO and QI.PersonSNO = GAFC.PersonSNO) Chk
                            from getAllFinishedCourse GAFC

                        Left JOIN Person P ON P.PersonID = GAFC.PersonID

                        Left Join QS_CoursePlanningClass QCPC On QCPC.PClassSNO = GAFC.PClassSNO

                        Left Join QS_CertificateType QCT On QCT.CTypeSNO = QCPC.CTypeSNO


                        LEFT JOIN QS_Integral I ON I.CourseSNO = GAFC.CourseSNO and I.PersonSNO = P.PersonSNO
						)
						Select ROW_NUMBER() OVER (ORDER BY gd.PersonSNO DESC) ROW_NO,gd.PersonSNO,gd.CourseName,gd.CTypeName,C1.MVal,gd.Exam,gd.Feedback,gd.Record
						,case when gd.ISNO is not null then '已取得' Else '未取得' End ISNO,ELSCode

                        from getallData gd

                        Left JOIN Person P ON P.PersonID = gd.PersonID

                         LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO

                         Left Join Config C1 On C1.PVal = gd.Class1 and C1.PGroup = 'CourseClass1'
                        LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
                        where gd.PersonSNO = @PersonSNO








            ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        wDict.Add("PersonSNO", userInfo.PersonSNO);
        
        objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_LearningRecord.DataSource = objDT;
        gv_LearningRecord.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }
    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        bindData(page);
    }
    protected void gv_LearningRecord_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {
            //要隱藏的欄位，如果前台filed隱藏會抓不到值，所以在RowCreate時才隱藏
            //e.Row.Cells[1].Visible = false;
            //e.Row.Cells[3].Visible = false;
        }
    }

    protected void LB_Exam_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        LinkButton btn = (LinkButton)sender;
        String ELSCode = btn.CommandArgument;
        string sql = @"
            select 
                ce.ELName, Score, QuizName, PassScore,
                case IsPass when 0 then '不通過' when 1 then '通過' End Pass,
				convert(varchar(16), ExamDate, 120) ExamDate
            from QS_LearningScore ls
                Left Join QS_CourseELearning ce ON ce.ELCode=ls.ELCode
            where PersonID=@PersonID and ls.ELSCode=@ELSCode
        ";
        aDict.Add("PersonID", userInfo.PersonID);
        aDict.Add("ELSCode", ELSCode);
        DataTable objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            LearningScore.DataSource = objDT.DefaultView;
            LearningScore.DataBind();
            rpt_FeedBack.DataSource = null;
            rpt_FeedBack.DataBind();
            rpt_ECoursePlanningClass.DataSource = null;
            rpt_ECoursePlanningClass.DataBind();
            Tr1.Visible = true;
            Tr.Visible = false;
            Tr2.Visible = false;
            lb_Notice.Visible = false;
        }
        else
        {
            LearningScore.DataSource = null;
            LearningScore.DataBind();
            rpt_FeedBack.DataSource = null;
            rpt_FeedBack.DataBind();
            rpt_ECoursePlanningClass.DataSource = null;
            rpt_ECoursePlanningClass.DataBind();
            Tr.Visible = false;
            Tr1.Visible = false;
            Tr2.Visible = false;
            lb_Notice.Visible = true;
        }

    }
    protected void LB_Feedback_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        LinkButton btn = (LinkButton)sender;
        String ELSCode = btn.CommandArgument;
        string sql = @"    WITH Base AS (
         Select distinct P.PersonID,CES.ELSName,P.PName,LA.CompletedDate,LF.ELSCode,P.OrganSNO,P.RoleSNO,P.PersonSNO,QCE.ELName
	     from Person P
	     Join QS_LearningAnswer LA ON LA.PersonID=P.PersonID
	     Join QS_LearningFeedback LF ON LF.QID=LA.QID
	     Join QS_CourseELearningSection CES ON CES.ELSCode=LF.ELSCode
		 Join QS_CourseELearning QCE On QCE.ELCode=CES.ELCode
        )

        SELECT Base.ELSName,Base.ELName,Base.PersonID,Base.PersonSNO,Base.PName,Base.CompletedDate, ROW_NUMBER() OVER (ORDER BY PersonID) AS ROW_NO,ELSCode
        FROM Base
        LEFT JOIN Organ O ON O.OrganSNO = Base.OrganSNO
        LEFT JOIN Role R ON R.RoleSNO =Base.RoleSNO
        where 1=1 And PersonSNO=@PersonSNO and ELSCode=@ELSCode
            ";
        aDict.Add("PersonSNO", userInfo.PersonSNO);
        aDict.Add("ELSCode", ELSCode);
        DataTable objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            rpt_FeedBack.DataSource = objDT.DefaultView;
            rpt_FeedBack.DataBind();
            LearningScore.DataSource = null;
            LearningScore.DataBind();
            rpt_ECoursePlanningClass.DataSource = null;
            rpt_ECoursePlanningClass.DataBind();
            Tr2.Visible = true;
            Tr1.Visible = false;
            Tr.Visible = false;            
            lb_Notice.Visible = false;
        }
        else
        {
            LearningScore.DataSource = null;
            LearningScore.DataBind();
            rpt_FeedBack.DataSource = null;
            rpt_FeedBack.DataBind();
            rpt_ECoursePlanningClass.DataSource = null;
            rpt_ECoursePlanningClass.DataBind();
            Tr.Visible = false;
            Tr1.Visible = false;
            Tr2.Visible = false;
            lb_Notice.Visible = true;
        }

    }
    protected void LB_Record_Click(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        LinkButton btn = (LinkButton)sender;
        String ELSCode = btn.CommandArgument;
        String sql = @"
           		 SELECT 
				lr.ELSCode, (cast(lr.ELSPart as varchar)+'/'+cast(es.ELSPart as varchar)) ELSPart, es.ELSName, e.ELName,
				convert(varchar(16), lr.FinishedDate, 120) FinishedDate
            From QS_LearningRecord lr
                LEFT JOIN QS_CourseELearningSection es on es.ELSCode=lr.ELSCode
                LEFT JOIN QS_CourseELearning e on e.ELCode=es.ELCode
            Where lr.PersonID=@PersonID and lr.ELSCode=@ELSCode
			Order by ELSName
        ";
        aDict.Add("PersonID", userInfo.PersonID);
        aDict.Add("ELSCode", ELSCode);
        DataTable objDT = objDH.queryData(sql, aDict);
        if (objDT.Rows.Count > 0)
        {
            rpt_ECoursePlanningClass.DataSource = objDT.DefaultView;
            rpt_ECoursePlanningClass.DataBind();
            LearningScore.DataSource = null;
            LearningScore.DataBind();
            rpt_FeedBack.DataSource = null;
            rpt_FeedBack.DataBind();
            Tr.Visible = true;
            Tr1.Visible = false;
            Tr2.Visible = false;
            lb_Notice.Visible = false;
        }
        else
        {
            LearningScore.DataSource = null;
            LearningScore.DataBind();
            rpt_FeedBack.DataSource = null;
            rpt_FeedBack.DataBind();
            rpt_ECoursePlanningClass.DataSource = null;
            rpt_ECoursePlanningClass.DataBind();
            Tr1.Visible = false;
            Tr2.Visible = false;
            Tr.Visible = false;
            lb_Notice.Visible = true;
        }


    }
}