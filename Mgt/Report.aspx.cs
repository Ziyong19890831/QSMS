using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Utility.setClassRoleName(ddl_RoleName, "請選擇適用人員");
        }

    }

    protected void btn_report_Click(object sender, EventArgs e)
    {
        if (ddl_RoleName.SelectedValue == "")
        {
            Response.Write("<script language=javascript>alert('請選擇適用人員');</" + "script>");
            return;
        }
      
        bind();
        Response.ClearContent();
        string excelFileName = "Report"+ DateTime.Today.ToString("yyyyMMdd") + ".xls";
        Response.AddHeader("content-disposition", "attachment;filename=" + Server.UrlEncode(excelFileName));
        Response.ContentType = "application/excel";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        Export_Panel.RenderControl(htmlWrite);
        Response.Write(stringWrite.ToString());
        Response.End();
    }
    protected void bind()
    {
        string Table = @"
        <table>
        <tr bgcolor='#B7DEE8' align='center'>
            <th rowspan='2'>角色</th>
            <th rowspan='2'>姓名</th>
            <th rowspan='2'>id</th>
            <th rowspan='2'>email</th>
            <th rowspan='2'>手機</th>
            <th rowspan='2'>電話</th>
            <th rowspan='2'>積分</th>
       ";
       

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String sql = @"select C.CourseName,R.RoleName,C.CourseSNO from QS_Course C
                        LEFT Join  QS_CoursePlanningRole CPR ON CPR.PClassSNO=C.PClassSNO
                        LEFT Join Role R ON R.RoleSNO=CPR.RoleSNO
                        where 1=1 ";
        #region 查詢篩選區塊
        if (!String.IsNullOrEmpty(ddl_RoleName.SelectedValue))
        {
            sql += " AND R.RoleSNO = @RoleSNO ";
            aDict.Add("RoleSNO", ddl_RoleName.SelectedValue);
        }
        #endregion
        DataHelper ObjDH = new DataHelper();
        DataTable objDT = ObjDH.queryData(sql, aDict);

        for(int i = 0; i < objDT.Rows.Count; i++)
        {
            Table += "<th colspan='4'>" + objDT.Rows[i]["CourseName"].ToString()+ "</th>";
        }
        Table += "<tr bgcolor='#B7DEE8' align='center'>";
        for (int j = 0; j < objDT.Rows.Count; j++)
        {
            Table += "<td>觀看</td><td>測驗</td><td>滿意度</td><td>積分授予</td>";
        }
        Table += "</tr>";
        Table += " </tr >";

        string SqlString = @" 
                	--step1-1 取得所有E-learningPart數
                	With 
                	getAllParts As (
                		Select ELSCode, ces.ELSPart
                		From QS_CourseELearningSection ces 
                	)
                
                	--step1-2 取得學員上課之統計節數
                	, getLearningParts As (
                		Select PersonID, ELSCode, Count(1) FinishedParts 
                		From (
                			Select Distinct lr.PersonID, lr.ELSCode, lr.ELSPart
                			From QS_LearningRecord lr 
                				Left Join QS_CourseELearningSection ces ON ces.ELSCode=lr.ELSCode
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
                		Where 
                		--PersonID=@PersonID And 
                		ls.IsPass=1
                	) 
                
                	--step3 該學員是否已完成課程的滿意度調查
                	, getFinishedFeedback As (
                		Select Distinct la.PersonID, lf.ELSCode, lf.FBID 
                		From QS_LearningAnswer la
                			Left Join QS_LearningFeedback lf ON lf.QID=la.QID
                		--Where PersonID=@PersonID
                	) 
                
                	--step4 取得ELSCode對應的CourseSNO
                	, getCourseSNO As (
                		Select c.CourseSNO, ces.ELSCode
                		From QS_CourseELearningSection ces
                			Left Join QS_Course c ON c.ELSCode=ces.ELSCode
                	) 
                
                	----step5 取得該學員所有紀錄
                	, getAllFinishedCourse As (
                		SELECT c.CourseSNO 
                		,fr.PersonID                
                        ,case when I.AuthType is not null then '*' END 'AuthType'
                		,case when fr.ELSCode is not null then '已觀看' END 'Record'
                		,case Convert(varchar(10),fe.IsPass) when 1 then '通過' Else '不通過' END 'Exam'
                		,case when ff.ELSCode is not null then '已填寫' END'Feedback'
                		From getCourseSNO c
                			Left Join getFinishedRecord		fr ON fr.ELSCode=c.ELSCode
                			Left Join getFinishedExam		fe ON fe.ELSCode=c.ELSCode And fe.PersonID=fr.PersonID
                			Left Join getFinishedFeedback	ff ON ff.ELSCode=c.ELSCode And ff.PersonID=fr.PersonID
                            Left Join Person P ON P.PersonID=fr.PersonID
							Left Join QS_Integral I ON I.PersonSNO=P.PersonSNO And I.CourseSNO=c.CourseSNO
                		--Where c.CourseSNO Is Not Null And 
                		--	  fr.ELSCode Is Not Null And 
                		--	  fe.ELSCode Is Not Null And 
                		--	  ff.ELSCode Is Not Null
                		where C.CourseSNO is not null
                	) 
                    --取得所有學員的E-Learning紀錄
                	    Select distinct R.RoleName, P.PName,P.PersonID,P.PMail,P.PPhone,P.PTel  ";
        for (int i = 0; i < objDT.Rows.Count; i++)
        {
            SqlString += " ,ISNULL(Convert(varchar(10), gAFC_" + i + ".Record),'未觀看') 'Record" + i + "',ISNULL(Convert(varchar(10),gAFC_" + i + ".Exam),'未測驗') 'Exam" + i + "' , ISNULL(Convert(varchar(10),gAFC_" + i + ".Feedback),'未填寫') 'Feedback" + i + "', ISNULL(Convert(varchar(10),gAFC_" + i + ".AuthType),'') 'AuthType" + i + "'";
        }
        SqlString += @" From Person P ";
        for (int i = 0; i < objDT.Rows.Count; i++)
        {
            SqlString += " Left Join getAllFinishedCourse gAFC_" + i + " ON gAFC_" + i + ".PersonID=P.PersonID And gAFC_" + i + ".CourseSNO="+ objDT.Rows[i]["CourseSNO"] +"	";
        }
        SqlString += "Left JOIN Role R ON R.RoleSNO=P.RoleSNO";
        SqlString += " Where 1=1 ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        if (!String.IsNullOrEmpty(ddl_RoleName.SelectedValue))
        {
            SqlString += " AND P.RoleSNO = @RoleSNO ";
            wDict.Add("RoleSNO", ddl_RoleName.SelectedValue);
        }
       
        DataTable objDT_1 = ObjDH.queryData(SqlString, wDict);

        Table += " </table> ";
        Label1.Text = Table;
        gv_Report.DataSource = objDT_1.DefaultView;
        gv_Report.DataBind();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //'XX'型別 必須置於有 runat=server 的表單標記之中
    }

    //protected void export_init()
    //{
    //    gv_Report.Columns.Add()
    //}

    protected void gv_Report_RowCreated(object sender, GridViewRowEventArgs e)
    {


        //if (e.Row.RowType == DataControlRowType.Header) // If header created
        //{
        //    GridView ProductGrid = (GridView)sender;

        //    // Creating a Row
        //    GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
        //    BoundField boundField = new BoundField();
        //    //Adding Year Column
        //    TableCell HeaderCell = new TableCell();
        //    HeaderCell.Text = "角色";
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderCell.RowSpan = 3; // For merging first, second row cells to one
        //    HeaderCell.CssClass = "HeaderStyle";
        //    HeaderRow.Cells.Add(HeaderCell);
           

        //    //Adding Period Column
        //    HeaderCell = new TableCell();
        //    HeaderCell.Text = "姓名";
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderCell.RowSpan =3;
        //    HeaderCell.CssClass = "HeaderStyle";
        //    HeaderRow.Cells.Add(HeaderCell);

        //    //Adding Audited By Column
        //    HeaderCell = new TableCell();
        //    HeaderCell.Text = "ID";
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderCell.RowSpan = 3;
        //    HeaderCell.CssClass = "HeaderStyle";
        //    HeaderRow.Cells.Add(HeaderCell);

        //    //Adding Audited By Column
        //    HeaderCell = new TableCell();
        //    HeaderCell.Text = "Email";
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderCell.RowSpan = 3;
        //    HeaderCell.CssClass = "HeaderStyle";
        //    HeaderRow.Cells.Add(HeaderCell);

        //    //Adding Audited By Column
        //    HeaderCell = new TableCell();
        //    HeaderCell.Text = "手機";
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderCell.RowSpan = 3;
        //    HeaderCell.CssClass = "HeaderStyle";
        //    HeaderRow.Cells.Add(HeaderCell);

        //    //Adding Audited By Column
        //    HeaderCell = new TableCell();
        //    HeaderCell.Text = "電話";
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderCell.RowSpan = 3;
        //    HeaderCell.CssClass = "HeaderStyle";
        //    HeaderRow.Cells.Add(HeaderCell);

        //    HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

        //    for (int i = 0; i < 5; i++) { 
        //    //Adding Revenue Column
        //    HeaderCell = new TableCell();
        //    HeaderCell.Text = "課程";
        //    HeaderCell.Width = 300;
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderCell.ColumnSpan = 3; // For merging three columns (Direct, Referral, Total)
        //    HeaderCell.CssClass = "HeaderStyle";
        //    HeaderRow.Cells.Add(HeaderCell);

        //    ProductGrid.Controls[0].Controls.AddAt(0, HeaderRow);

        //    HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);


        //    //Adding Head Office Debit Column
        //    HeaderCell = new TableCell();
        //    HeaderCell.Text = "觀看";
        //    HeaderCell.Width = 100;
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderCell.RowSpan = 2;
        //    HeaderCell.CssClass = "HeaderStyle";
        //    HeaderRow.Cells.Add(HeaderCell);

        //    //Adding Head Office Debit Column
        //    HeaderCell = new TableCell();
        //    HeaderCell.Text = "測驗";
        //    HeaderCell.Width = 100;
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderCell.RowSpan = 2;
        //    HeaderCell.CssClass = "HeaderStyle";
        //    HeaderRow.Cells.Add(HeaderCell);

        //    //Adding Head Office Debit Column
        //    HeaderCell = new TableCell();
        //    HeaderCell.Text = "滿意度";
        //    HeaderCell.Width = 100;
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderCell.RowSpan = 2;
        //    HeaderCell.CssClass = "HeaderStyle";
        //    HeaderRow.Cells.Add(HeaderCell);

        //    ProductGrid.Controls[0].Controls.AddAt(1, HeaderRow);

        //        //Adding the Row at the 0th position (first row) in the Grid

        //    }
        //}
    }
    protected void gv_Report_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // Invisibling the first three columns of second row header (normally created on binding)
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    e.Row.Cells[0].Visible = false; // Invisibiling Year Header Cell
        //    e.Row.Cells[1].Visible = false; // Invisibiling Period Header Cell
        //    e.Row.Cells[2].Visible = false; // Invisibiling Audited By Header Cell
        //    e.Row.Cells[3].Visible = false;
        //    e.Row.Cells[4].Visible = false;
        //    e.Row.Cells[5].Visible = false;
            
        //}
    }
}





 


