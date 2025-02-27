using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security.AntiXss;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Event_AE : System.Web.UI.Page
{
    UserInfo userInfo = null;
    public string Role;
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];


    }

    protected void Page_Load(object sender, EventArgs e)
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
       
        if (!IsPostBack)
        {
            getData();
            if (userInfo != null) chkEventApply();

        }
    }


    protected void getData()
    {


        String id = Convert.ToString(Request.QueryString["sno"]);
        string CtypeSNO = Convert.ToString(Request.QueryString["CtypeSNO"]);
        bool CheckFor = EventRole.CheckIntegerType(id);
        if (CheckFor == true)
        {

            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("EventSNO", id);
            DataHelper objDH = new DataHelper();
            string SQL = @"
             SELECT 
                EventSNO, EventName, E.Note, CountLimit, E.TargetHour,CountLimit,
				CountAdmit,Convert(varchar, StartTime, 120) StartTime, Convert(varchar, EndTime, 120) EndTime,
                E.CLocation,E.CDate,E.CTime,E.CPerosn,E.CPhone,E.CMail,
                CF.MVal Class1,CF2.MVal Class2,E.Note,(E.CLocationAreaA+E.CLocationAreaB+E.CLocation)  L,E.QTypeName,
                E.ActiveCost,E.Host,
                " + Utility.setSQL_RoleBindName("Event_AE", "E.EventSNO") + @",E.TargetHour
            From Event E 			
				Left Join config CF on CF.Pval=E.class3 and CF.[PGroup]='CourseClass3' 
				Left Join config CF2 on CF2.Pval=E.class4 and CF2.[PGroup]='CourseClass4'
            Where E.EventSNO=@EventSNO";
            DataTable objDT = objDH.queryData(SQL, aDict);

            lb_Class1.Text = AntiXssEncoder.HtmlEncode(objDT.Rows[0]["Class1"].ToString(), true);
            lb_Class2.Text = AntiXssEncoder.HtmlEncode(objDT.Rows[0]["Class2"].ToString(), true);
            lb_EventName.Text = AntiXssEncoder.HtmlEncode(objDT.Rows[0]["EventName"].ToString(), true);
            lb_Time.Text = AntiXssEncoder.HtmlEncode(Convert.ToDateTime(objDT.Rows[0]["StartTime"]).ToString("yyyy/MM/dd HH:mm"), true) +"至"+ Convert.ToDateTime(objDT.Rows[0]["EndTime"]).ToString("yyyy/MM/dd HH:mm");
            lb_Note.Text = getMark(HttpUtility.HtmlDecode(objDT.Rows[0]["Note"].ToString()));
            lb_RoleBindName.Text= AntiXssEncoder.HtmlEncode(objDT.Rows[0]["RoleBindName"].ToString(), true);
            lb_CountAudit.Text = objDT.Rows[0]["CountAdmit"].ToString();
            lb_QTypeName.Text = objDT.Rows[0]["QTypeName"].ToString();
            lb_Host.Text = objDT.Rows[0]["Host"].ToString();
            lb_ActiveCost.Text = objDT.Rows[0]["ActiveCost"].ToString();
            lb_CPerson.Text = objDT.Rows[0]["CPerosn"].ToString();
            lb_CPhone.Text = objDT.Rows[0]["CPhone"].ToString();
            lb_Cmail.Text = objDT.Rows[0]["CMail"].ToString();
            lb_Address.Text = objDT.Rows[0]["L"].ToString();
            DataTable DT = objDH.queryData(@"
            SELECT p.PName 
            From EventD e
                Left Join Person p On p.PersonSNO=e.PersonSNO
            Where EventSNO=@EventSNO and Audit<>2 and Audit<>5", aDict);//未審 錄取 審核中 備取
            lbl_Count.Text = Convert.ToString(DT.Rows.Count);
            string CountLimit = objDT.Rows[0]["CountLimit"].ToString();
            if (CountLimit != "0")
            {
                lb_CountAdmit.Text = objDT.Rows[0]["CountAdmit"].ToString();
                if (Convert.ToInt16(objDT.Rows[0]["CountLimit"]) <= DT.Rows.Count && Convert.ToInt16(objDT.Rows[0]["CountLimit"]) != 0)
                {
                    lb_Msg.Visible = true;
                    btn_Apply.Visible = false;
                    btnDEL.Visible = false;
                }
            }
            else
            {
                tr_CountAdmit.Style.Add("display", "none");

            }
            Dictionary<string, object> bDict = new Dictionary<string, object>();
            bDict.Add("EventSNO", id);
            string SQLBatch = @"Select * from EventBatch where EventSNO=@EventSNO order by EventBSNO";
            DataTable DTBatch = objDH.queryData(SQLBatch, bDict);
            string CStartDay =Convert.ToDateTime(DTBatch.Rows[0]["CStartDay"]).ToString("yyyy/MM/dd");
            string CEndDay = Convert.ToDateTime(DTBatch.Rows[0]["CEndDay"]).ToString("yyyy/MM/dd");
            string CStartTime = DTBatch.Rows[0]["CStartTime"].ToString();
            string CEndTime = DTBatch.Rows[0]["CEndTime"].ToString();
            DateTime StartTime = Convert.ToDateTime(objDT.Rows[0]["StartTime"]);
            string DTS = objDT.Rows[0]["EndTime"].ToString();
            DateTime EndTime = Convert.ToDateTime(objDT.Rows[0]["EndTime"]);
            //lb_CEStart.Text = CStartDay +"  "+CStartTime + "至" + CEndDay + "  " + CEndTime;
            lb_CEdate.Text = CStartDay + "-" + CEndDay;
            lb_CEtime.Text = CStartTime + "-" + CEndTime;
            lb_Hour.Text = DTBatch.Rows[0]["Chour"].ToString();
            lb_Class.Text = DTBatch.Rows[0]["CCount"].ToString();
            lb_EventNote.Text = DTBatch.Rows[0]["EventAreaCodeAText"].ToString();
            if (DateTime.Now <= StartTime || DateTime.Now >= EndTime)
            {
                btn_Apply.Enabled = false;
                btn_Apply.Style["background-color"] = "#cccccc";
                btn_Apply.Style["color"] = "#666666";

            }
        }
        else
        {
            Response.Write("<Script>alert('錯誤參數');document.location.href='Event.aspx';</Script>");
            return;
        }
    }

    public string getMark(string Data)
    {
        string s = Data;
        int x = s.IndexOf("marker-yellow");
        if (x > 0)
        {
            s = s.Replace("class=\"marker-yellow\"", "class=\"marker-yellow\" style=\"background-color:#fdfd77\"");
        }

        x = s.IndexOf("marker-green");
        if (x > 0)
        {
            s = s.Replace("class=\"marker-green\"", "class=\"marker-green\" style=\"background-color:#63f963\"");
        }

        x = s.IndexOf("marker-pink");
        if (x > 0)
        {
            s = s.Replace("class=\"marker-pink\"", "class=\"marker-pink\" style=\"background-color:#fc7999\"");
        }

        x = s.IndexOf("marker-blue");
        if (x > 0)
        {
            s = s.Replace("class=\"marker-blue\"", "class=\"marker-blue\" style=\"background-color:#72cdfd\"");
        }
        x = s.IndexOf("pen-red");
        if (x > 0)
        {
            s = s.Replace("class=\"pen-red\"", "class=\"pen-red\" style=\"background-color:transparent;color:#e91313\"");
        }
        x = s.IndexOf("pen-green");
        if (x > 0)
        {
            s = s.Replace("class=\"pen-green\"", "class=\"pen-green\" style=\"background-color:transparent;color:#118800\"");
        }
        return s;
    }
    [WebMethod]
    public static Dictionary<string, object> SelectData(string s_Ipnut_Data1)
    {
        Dictionary<string, object> List_Data = new Dictionary<string, object>();
        List_Data.Add("DATA", "");
        return List_Data;
    }
    protected void chkEventApply()
    {

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String id = Convert.ToString(Request.QueryString["sno"]);
        bool CheckFor = EventRole.CheckIntegerType(id);
        if (CheckFor == true)
        {
            aDict.Add("PersonSNO", userInfo.PersonSNO);
            aDict.Add("EventSNO", id);
            DataHelper objDH = new DataHelper();
            DataTable objDT = objDH.queryData(@"SELECT 1 from EventD  Where EventSNO=@EventSNO And PersonSNO=@PersonSNO", aDict);

            //檢查自己是否已報名
            if (objDT.Rows.Count > 0)
            {
                btn_Apply.Visible = false;
                btnDEL.Visible = true;
            }

        }
        else
        {
            Response.Write("<Script>alert('錯誤參數');document.location.href='Event.aspx';</Script>");
            return;
        }
    }

    protected void btnDEL_Click(object sender, EventArgs e)
    {

        string id = Request.QueryString["sno"];
        bool CheckFor = EventRole.CheckIntegerType(id);
        if (CheckFor == true)
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            DataHelper objDH = new DataHelper();
            string SQL = @"SELECT ED.*,EB.CStartDay
                    FROM [EventD] ED
                    Left Join EventBatch EB On EB.EventSNO= ED.EventSNO 
                    where ED.PersonSNO=@PersonSNO And ED.EventSNO=@EventSNO";
            aDict.Add("EventSNO", id);
            aDict.Add("PersonSNO", userInfo.PersonSNO);
            DataTable CheckAudit = objDH.queryData(SQL, aDict);
            if (CheckAudit.Rows.Count>0)
            {
                DateTime Cstratday = Convert.ToDateTime(CheckAudit.Rows[0]["CStartDay"]);
                if (DateTime.Now > Cstratday)
                {
                    Response.Write("<script>alert('已經開始上課，無法取消報名，如有需要，請通知管理員')</script>");
                    return;
                }
                if (CheckAudit.Rows[0]["Audit"].ToString() != "0")
                {
                    Response.Write("<script>alert('已經通過審核!，如要取消，請通知管理員');</script>");
                    return;
                }
                else
                {


                    objDH.executeNonQuery("Delete EventD Where EventSNO=@EventSNO And PersonSNO=@PersonSNO", aDict);
                    Response.Write("<script>alert('取消報名成功!');  </script>");
                    Response.Write("<script>location.href='Event.aspx';</script>");
                    return;
                }
            }
            else
            {
                Response.Write("<script>alert('您尚未報名此課程');</script>");
                return;
            }
           
        }
        else
        {
            Response.Write("<Script>alert('錯誤參數');document.location.href='Event.aspx';</Script>");
            return;
        }
    }
    protected void btn_Apply_Click(object sender, EventArgs e)
    {

        if (Session["QSMS_UserInfo"] == null)
        {
            //判斷是否登入
            Utility.showMessage(Page, "ErrorMessage", "請先登入系統");
            return;
        }
        else
        {
            //判斷基本資料是否填妥
            bool checkS = Utility.CheckSession(userInfo);
            if (checkS == false)
            {
                Response.Redirect("Personnel_AE.aspx?Error=1");
            }
        }
        string Mstatus = Utility.CheckMstutas(userInfo.PersonSNO);
        if (Mstatus == "2")
        {
            //判斷醫事人員認證
            Utility.showMessage(Page, "ErrorMessage", "此學員狀態異常，請聯絡管理員。");
            return;
        }
        string id = Request.QueryString["sno"];
        string CtypeSNO = Request.QueryString["CtypeSNO"];
        if (!ChkEventCount(id))
        {
            Response.Write("<script>alert('已超過報名上限!'); document.location.href='./Event.aspx'</script>");
            return;
        }
        else
        {
            DataHelper ObjDH = new DataHelper();
            Dictionary<string, object> adict = new Dictionary<string, object>();
            string sql = @"  
                  SELECT ISNO,CourseSNO FROM [New_QSMS].[dbo].QS_Integral where IsUsed=0 and CourseSNO in ('1','2','3') and PersonSNO=@PersonSNO
            ";

            adict.Add("@PersonSNO", userInfo.PersonSNO);
            DataTable ObjDT = ObjDH.queryData(sql, adict);

            if (!CheckRole(userInfo.RoleSNO, id))
            {
                Utility.showMessage(Page, "ErrorMessage", "您不是正確的適用人員");
                return;
            }
            string Eventsql = @"  
                SELECT Class3,[Class4]  FROM [New_QSMS].[dbo].[Event] where EventSNO=@EventSNO 
            ";
            adict.Add("@EventSNO", id);
            DataTable EventDT = ObjDH.queryData(Eventsql, adict);
            DataTable objCourseSNODT;
            string Class3 = EventDT.Rows[0]["Class3"].ToString();
            string Class4 = EventDT.Rows[0]["Class4"].ToString();
            if (Class4 == "1")//共同
            {
                Response.Redirect("Event_Apply.aspx?sno=" + id);
            }
            else if (Class4 == "2")//專門
            {
                if (ObjDT.Rows.Count < 3)
                {
                    string SQL = "";
                    SQL += @"With getCourseSNO as (";
                    SQL += " 	Select * from QS_Course QC where QC.CourseSNO In(1,2,3)";
                    SQL += "         )";
                    SQL += " Select QCS.CourseName '課程名稱', Case When QI.ISNO is not null Then '已修' Else '未修' End as '是否已修' ";
                    SQL += " from getCourseSNO QCS";
                    SQL += " Left Join QS_Integral QI ON QCS.CourseSNO=QI.CourseSNO and QI.PersonSNO=@PersonSNO And QI.isused=0 ";
                    objCourseSNODT = ObjDH.queryData(SQL, adict);
                    GridView gv = new GridView();
                    Repeater1.DataSource = objCourseSNODT.DefaultView;
                    Repeater1.DataBind();
                    Label lb_Notice = new Label();
                    lb_Notice.Text = "您未滿足報名條件，須符合以下條件:";
                    lb_Notice.ForeColor = System.Drawing.Color.Red;
                    PlaceHolder1.Controls.Add(lb_Notice);
                    Tr4.Visible = true;
                    Utility.MessageBox.Show("尚未修完基礎課程");
                }
                else
                {
                    Response.Redirect("Event_Apply.aspx?sno=" + id);
                }

            }
            else if (Class4 == "3")//繼續教育
            {
                DataTable objCertificateSNODT;
                string Certificate = "";
                if (userInfo.RoleSNO == "10")//西醫師
                {
                    Certificate = @"('1','53','75')";
                }
                else if (userInfo.RoleSNO == "11")//牙醫師
                {
                    Certificate = @"('2','54','75')";
                }
                else if (userInfo.RoleSNO == "12")//藥師
                {
                    Certificate = @"('6','7','52','75')";
                }
                else if (userInfo.RoleSNO == "13")//衛教師
                {
                    Certificate = @"('4','5','51','75')";
                }
                string SQLs = @"With 
                    	getCertificateeSNO as ( 	
                    	Select * from QS_CertificateType where CTypeSNO in  ";
                SQLs += Certificate;
                SQLs += @"
                    
					),getEvent as (
					Select Top 1 EB.CStartDay from Event E
					Left Join EventBatch EB On E.EventSNO=EB.EventSNO
					where E.EventSNO=55
					),CheckDate as (
					 Select QCS.CTypeName '證書名稱', Case When QC.CTypeSNO is not null Then '已取得' Else '未取得' End as '是否取得證明' ,QC.CertEndDate
					 ,(Select *  from getEvent)EventSatrtDay
                    from getCertificateeSNO QCS 
                    Left Join QS_Certificate QC ON QCS.CTypeSNO=QC.CTypeSNO and QC.PersonID=@PersonID)
					Select CD.證書名稱,CD.是否取得證明,Case when CD.CertEndDate > getdate() then '有' ELSE '沒有' END '是否有在期限內' from CheckDate CD";
                adict.Add("PersonID", userInfo.PersonID);
                objCertificateSNODT = ObjDH.queryData(SQLs, adict);
                bool Certificatepass = true;

                if (objCertificateSNODT.Rows.Count > 0)
                {
                    for (int i = 0; i < objCertificateSNODT.Rows.Count; i++)
                    {
                        if (objCertificateSNODT.Rows[i]["是否取得證明"].ToString() == "已取得" && objCertificateSNODT.Rows[i]["是否有在期限內"].ToString() == "有")
                        {
                            Certificatepass = true;
                            break;
                        }
                        else if (objCertificateSNODT == null)
                        {
                            Certificatepass = true;
                        }
                        else
                        {
                            Certificatepass = false;
                        }
                    }
                }
                if (Certificatepass == true)
                {
                    Response.Redirect("Event_Apply.aspx?sno=" + id);
                }
                else
                {
                    GridView gv = new GridView();
                    Repeater2.DataSource = objCertificateSNODT.DefaultView;
                    Repeater2.DataBind();
                    Label lb_Notice = new Label();
                    lb_Notice.Text = "您未滿足報名條件，須符合以下條件:";
                    lb_Notice.ForeColor = System.Drawing.Color.Red;
                    PlaceHolder1.Controls.Add(lb_Notice);
                    Tr1.Visible = true;
                    Utility.MessageBox.Show("尚未取得證明");
                }

            }
        }
     
    }
    public void Check22CourseSNORole(string id, string PersonSNO, string Certificate, string CourseSNO, string Logic, bool Certificatepass = true, bool CoursePass = true, bool logic1 = true, bool logic2 = true, bool logic3 = true, string LogicSign1 = "", string LogicSign2 = "")
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        ArrayList l1 = new ArrayList();
        string[] CourseSNOArray = CourseSNO.Split(';');
        string[] CertificateArray = Certificate.Split(';');
        string[] LogicArray = Logic.Split(';');

        for (int j = 0; j < CourseSNOArray.Length; j++)
        {

            if (LogicArray[j] != "")
            {
                l1.Add(LogicArray[j]);
            }
            DataTable objCertificateSNODT;
            string SQLs = @"With 
                    	getCertificateeSNO as ( 	
                    	Select * from QS_CertificateType where CTypeSNO=@CTypeSNO
                    	)
                   
					,getEvent as (
					Select Top 1 EB.CStartDay from Event E
					Left Join EventBatch EB On E.EventSNO=EB.EventSNO
					where E.EventSNO=55
					),CheckDate as (
					 Select QCS.CTypeName '證書名稱', Case When QC.CTypeSNO is not null Then '已取得' Else '未取得' End as '是否取得證書' ,QC.CertEndDate
					 ,(Select *  from getEvent)EventSatrtDay
                    from getCertificateeSNO QCS 
                    Left Join QS_Certificate QC ON QCS.CTypeSNO=QC.CTypeSNO and QC.PersonID=@PersonID)
					Select CD.證書名稱,CD.是否取得證書,Case when CD.CertEndDate > getdate() then '有' ELSE '沒有' END '是否有在期限內' from CheckDate CD";
            aDict.Add("PersonID", userInfo.PersonID);
            aDict.Add("CTypeSNO", CertificateArray[j]);
            objCertificateSNODT = objDH.queryData(SQLs, aDict);
            GridView gvs = new GridView();
            gvs.ID = "gv_CertificateSNO";
            gvs.AutoGenerateColumns = false;
            if (objCertificateSNODT != null)
            {
                if (objCertificateSNODT.Rows.Count > 0)
                {
                    for (int y = 0; y < objCertificateSNODT.Columns.Count; y++)
                    {
                        BoundField boundField = new BoundField();
                        boundField.DataField = objCertificateSNODT.Columns[y].ColumnName.ToString();
                        boundField.HeaderText = objCertificateSNODT.Columns[y].ColumnName.ToString();
                        gvs.Columns.Add(boundField);
                    }
                }
            }
            if (objCertificateSNODT.Rows.Count > 0)
            {
                for (int i = 0; i < objCertificateSNODT.Rows.Count; i++)
                {
                    if (objCertificateSNODT.Rows[i]["是否取得證書"].ToString() == "已取得" && objCertificateSNODT.Rows[i]["是否有在期限內"].ToString() == "有")
                    {
                        Certificatepass = true;
                        break;
                    }
                    else if (objCertificateSNODT == null)
                    {
                        Certificatepass = true;
                    }
                    else
                    {
                        Certificatepass = false;
                    }
                }

            }

            for (int i = 0; i < CertificateArray.Length; i++)
            {
                if (CertificateArray[i] == "" && objCertificateSNODT.Rows.Count == 0)
                {
                    Certificatepass = true;
                }
            }


            Label lb_logic = new Label();
            switch (LogicArray[j])
            {
                case "Or":
                    lb_logic.Text = "或";
                    break;
                case "And":
                    lb_logic.Text = "與";
                    break;
            }
            lb_logic.ForeColor = System.Drawing.Color.Red;
            PlaceHolder2.Controls.Add(lb_logic);

            PlaceHolder2.Controls.Add(gvs);
            BindGridView(gvs, objCertificateSNODT);



            aDict.Clear();

            if (CourseSNOArray[j] != "")
            {
                string SQL = "";
                DataTable objCourseSNODT;
                SQL += @"With getCourseSNO as (";
                SQL += " 	Select * from QS_Course QC where QC.CourseSNO In(" + CourseSNOArray[j] + ")";
                SQL += "         )";
                SQL += " Select QCS.CourseName '課程名稱', Case When QI.ISNO is not null Then '已修' Else '未修' End as '是否已修' ";
                SQL += " from getCourseSNO QCS";
                SQL += " Left Join QS_Integral QI ON QCS.CourseSNO=QI.CourseSNO and QI.PersonSNO=@PersonSNO And QI.isused=0 ";

                aDict.Add("CourseSNO", CourseSNOArray[j]);
                aDict.Add("PersonSNO", PersonSNO);
                objCourseSNODT = objDH.queryData(SQL, aDict);
                GridView gv = new GridView();
                gv.ID = "gv_CourseSNO";
                gv.AutoGenerateColumns = false;
                if (objCourseSNODT != null)
                {
                    if (objCourseSNODT.Rows.Count > 0)
                    {
                        for (int i = 0; i < objCourseSNODT.Columns.Count; i++)
                        {
                            BoundField boundField = new BoundField();
                            boundField.DataField = objCourseSNODT.Columns[i].ColumnName.ToString();
                            boundField.HeaderText = objCourseSNODT.Columns[i].ColumnName.ToString();
                            gv.Columns.Add(boundField);
                        }
                    }
                }
                if (objCourseSNODT.Rows.Count > 0)
                {
                    for (int i = 0; i < objCourseSNODT.Rows.Count; i++)
                    {
                        if (objCourseSNODT.Rows[i]["是否已修"].ToString() != "已修")
                        {
                            CoursePass = false;
                            break;
                        }
                        else
                        {
                            CoursePass = true;
                        }
                    }
                }

                PlaceHolder2.Controls.Add(gv);
                BindGridView(gv, objCourseSNODT);
                aDict.Clear();



            }
            else
            {
                CoursePass = true;
            }





            if (CoursePass == true && Certificatepass == true)
            {
                l1.Add(true);
            }
            else
            {
                l1.Add(false);
            }

        }

        Label lb_Notice = new Label();
        lb_Notice.Text = "您未滿足報名條件，須符合以下條件:";
        lb_Notice.ForeColor = System.Drawing.Color.Red;
        PlaceHolder1.Controls.Add(lb_Notice);

        for (int i = 0; i < l1.Count; i++)
        {
            switch (i)
            {
                case 0:
                    logic1 = (Boolean)l1[0];
                    break;
                case 1:
                    if (l1[1].ToString() == "Or")
                    {
                        LogicSign1 = "||";
                    }
                    else if (l1[1].ToString() == "And")
                    {
                        LogicSign1 = "&&";
                    }
                    break;
                case 2:
                    logic2 = (Boolean)l1[2];
                    break;
                case 3:
                    if (l1[3].ToString() == "Or")
                    {
                        LogicSign2 = "||";
                    }
                    else if (l1[3].ToString() == "And")
                    {
                        LogicSign2 = "&&";
                    }
                    break;
                case 4:
                    logic3 = (Boolean)l1[4];
                    break;
            }
        }
        if (LogicSign1 == "" && LogicSign2 == "")
        {
            if (logic1)
            {
                Page.Response.Redirect("Event_Apply.aspx?sno=" + id + "");
            }
            else
            {
                //if (CBL_CertificateStutas.SelectedValue != "0")
                //{
                //    btn_Invite.Visible = true;
                //}


            }

        }
        else if (LogicSign2 == "")
        {
            if (LogicSign1 == "||")
            {
                if (logic1 || logic2)
                {
                    //Page.Response.Redirect("Event_Apply.aspx?sno=" + id + "&CBL_CertificateStutas=" + CBL_CertificateStutas.SelectedValue + "");
                }
                else
                {
                    //btn_Invite.Visible = true;
                }
            }
            else
            {
                if (logic1 && logic2)
                {
                    //Page.Response.Redirect("Event_Apply.aspx?sno=" + id + "&CBL_CertificateStutas=" + CBL_CertificateStutas.SelectedValue + "");
                }
                else
                {
                    //btn_Invite.Visible = true;
                }
            }
        }
        else
        {
            if (LogicSign1 == "||" && LogicSign2 == "&&")
            {
                if ((logic1 || logic2) && logic3)
                {
                    //Page.Response.Redirect("Event_Apply.aspx?sno=" + id + "&CBL_CertificateStutas=" + CBL_CertificateStutas.SelectedValue + "");
                }
                else
                {
                    //btn_Invite.Visible = true;
                    return;
                }
            }
            if (LogicSign1 == "&&" && LogicSign2 == "||")
            {
                if ((logic1 && logic2) || logic3)
                {
                    //Page.Response.Redirect("Event_Apply.aspx?sno=" + id + "&CBL_CertificateStutas=" + CBL_CertificateStutas.SelectedValue + "");
                }
                else
                {
                    //btn_Invite.Visible = true;
                    return;
                }
            }
            if (LogicSign1 == "||")
            {
                if (logic1 || logic2 || logic3)
                {
                    //Page.Response.Redirect("Event_Apply.aspx?sno=" + id + "&CBL_CertificateStutas=" + CBL_CertificateStutas.SelectedValue + "");
                }
                else
                {
                    //btn_Invite.Visible = true;
                    return;
                }
            }
            else
            {
                if (logic1 && logic2 && logic3)
                {
                    //Page.Response.Redirect("Event_Apply.aspx?sno=" + id + "&CBL_CertificateStutas=" + CBL_CertificateStutas.SelectedValue + "");
                }
                else
                {
                    //btn_Invite.Visible = true;
                    return;
                }
            }
        }
    }

    public static string getSystem_ID(string EventSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = "Select System_ID from Event where EventSNO=@EventSNO";
        aDict.Add("EventSNO", EventSNO);
        DataTable ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            return ObjDT.Rows[0]["System_ID"].ToString();
        }
        else
        {
            return ObjDT.Rows[0]["System_ID"].ToString();
        }
    }
    public void CheckCourseSNORole(string id, string PersonSNO, string Certificate, string CourseSNO, string Logic, bool Certificatepass = true, bool CoursePass = true, bool logic1 = true, bool logic2 = true, bool logic3 = true, string LogicSign1 = "", string LogicSign2 = "")
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        ArrayList l1 = new ArrayList();
        string[] CourseSNOArray = CourseSNO.Split(';');
        string[] CertificateArray = Certificate.Split(';');
        string[] LogicArray = Logic.Split(';');

        for (int j = 0; j < CourseSNOArray.Length; j++)
        {

            if (LogicArray[j] != "")
            {
                l1.Add(LogicArray[j]);
            }
            DataTable objCertificateSNODT;
            string SQLs = @"With 
                    	getCertificateeSNO as ( 	
                    	Select * from QS_CertificateType where CTypeSNO=@CTypeSNO
                    	)
                   
					,getEvent as (
					Select Top 1 EB.CStartDay from Event E
					Left Join EventBatch EB On E.EventSNO=EB.EventSNO
					where E.EventSNO=55
					),CheckDate as (
					 Select QCS.CTypeName '證書名稱', Case When QC.CTypeSNO is not null Then '已取得' Else '未取得' End as '是否取得證書' ,QC.CertEndDate
					 ,(Select *  from getEvent)EventSatrtDay
                    from getCertificateeSNO QCS 
                    Left Join QS_Certificate QC ON QCS.CTypeSNO=QC.CTypeSNO and QC.PersonID=@PersonID)
					Select CD.證書名稱,CD.是否取得證書,Case when CD.CertEndDate > getdate() then '有' ELSE '沒有' END '是否有在期限內' from CheckDate CD";
            aDict.Add("PersonID", userInfo.PersonID);
            aDict.Add("CTypeSNO", CertificateArray[j]);
            objCertificateSNODT = objDH.queryData(SQLs, aDict);
            GridView gvs = new GridView();
            gvs.ID = "gv_CertificateSNO";
            gvs.AutoGenerateColumns = false;
            if (objCertificateSNODT != null)
            {
                if (objCertificateSNODT.Rows.Count > 0)
                {
                    for (int y = 0; y < objCertificateSNODT.Columns.Count; y++)
                    {
                        BoundField boundField = new BoundField();
                        boundField.DataField = objCertificateSNODT.Columns[y].ColumnName.ToString();
                        boundField.HeaderText = objCertificateSNODT.Columns[y].ColumnName.ToString();
                        gvs.Columns.Add(boundField);
                    }
                }
            }
            if (objCertificateSNODT.Rows.Count > 0)
            {
                for (int i = 0; i < objCertificateSNODT.Rows.Count; i++)
                {
                    if (objCertificateSNODT.Rows[i]["是否取得證書"].ToString() == "已取得" && objCertificateSNODT.Rows[i]["是否有在期限內"].ToString() == "有")
                    {
                        Certificatepass = true;
                        break;
                    }
                    else if (objCertificateSNODT == null)
                    {
                        Certificatepass = true;
                    }
                    else
                    {
                        Certificatepass = false;
                    }
                }

            }

            for (int i = 0; i < CertificateArray.Length; i++)
            {
                if (CertificateArray[i] == "" && objCertificateSNODT.Rows.Count == 0)
                {
                    Certificatepass = true;
                }
            }


            Label lb_logic = new Label();
            switch (LogicArray[j])
            {
                case "Or":
                    lb_logic.Text = "或";
                    break;
                case "And":
                    lb_logic.Text = "與";
                    break;
            }
            lb_logic.ForeColor = System.Drawing.Color.Red;
            PlaceHolder2.Controls.Add(lb_logic);

            PlaceHolder2.Controls.Add(gvs);
            BindGridView(gvs, objCertificateSNODT);



            aDict.Clear();

            if (CourseSNOArray[j] != "")
            {
                string SQL = "";
                DataTable objCourseSNODT;
                SQL += @"With getCourseSNO as (";
                SQL += " 	Select * from QS_Course QC where QC.CourseSNO In(" + CourseSNOArray[j] + ")";
                SQL += "         )";
                SQL += " Select QCS.CourseName '課程名稱', Case When QI.ISNO is not null Then '已修' Else '未修' End as '是否已修' ";
                SQL += " from getCourseSNO QCS";
                SQL += " Left Join QS_Integral QI ON QCS.CourseSNO=QI.CourseSNO and QI.PersonSNO=@PersonSNO And QI.isused=0 ";

                aDict.Add("CourseSNO", CourseSNOArray[j]);
                aDict.Add("PersonSNO", PersonSNO);
                objCourseSNODT = objDH.queryData(SQL, aDict);
                GridView gv = new GridView();
                gv.ID = "gv_CourseSNO";
                gv.AutoGenerateColumns = false;
                if (objCourseSNODT != null)
                {
                    if (objCourseSNODT.Rows.Count > 0)
                    {
                        for (int i = 0; i < objCourseSNODT.Columns.Count; i++)
                        {
                            BoundField boundField = new BoundField();
                            boundField.DataField = objCourseSNODT.Columns[i].ColumnName.ToString();
                            boundField.HeaderText = objCourseSNODT.Columns[i].ColumnName.ToString();
                            gv.Columns.Add(boundField);
                        }
                    }
                }
                if (objCourseSNODT.Rows.Count > 0)
                {
                    for (int i = 0; i < objCourseSNODT.Rows.Count; i++)
                    {
                        if (objCourseSNODT.Rows[i]["是否已修"].ToString() != "已修")
                        {
                            CoursePass = false;
                            break;
                        }
                        else
                        {
                            CoursePass = true;
                        }
                    }
                }

                PlaceHolder2.Controls.Add(gv);
                BindGridView(gv, objCourseSNODT);
                aDict.Clear();



            }
            else
            {
                CoursePass = true;
            }





            if (CoursePass == true && Certificatepass == true)
            {
                l1.Add(true);
            }
            else
            {
                l1.Add(false);
            }

        }

        Label lb_Notice = new Label();
        lb_Notice.Text = "您未滿足報名條件，須符合以下條件:";
        lb_Notice.ForeColor = System.Drawing.Color.Red;
        PlaceHolder1.Controls.Add(lb_Notice);

        for (int i = 0; i < l1.Count; i++)
        {
            switch (i)
            {
                case 0:
                    logic1 = (Boolean)l1[0];
                    break;
                case 1:
                    if (l1[1].ToString() == "Or")
                    {
                        LogicSign1 = "||";
                    }
                    else if (l1[1].ToString() == "And")
                    {
                        LogicSign1 = "&&";
                    }
                    break;
                case 2:
                    logic2 = (Boolean)l1[2];
                    break;
                case 3:
                    if (l1[3].ToString() == "Or")
                    {
                        LogicSign2 = "||";
                    }
                    else if (l1[3].ToString() == "And")
                    {
                        LogicSign2 = "&&";
                    }
                    break;
                case 4:
                    logic3 = (Boolean)l1[4];
                    break;
            }
        }
        if (LogicSign1 == "" && LogicSign2 == "")
        {
            if (logic1)
            {
                Page.Response.Redirect("Event_Apply.aspx?sno=" + id + "");
            }
            else
            {
                //if (CBL_CertificateStutas.SelectedValue != "0")
                //{
                //    btn_Invite.Visible = true;
                //}


            }

        }
        else if (LogicSign2 == "")
        {
            if (LogicSign1 == "||")
            {
                if (logic1 || logic2)
                {
                    //Page.Response.Redirect("Event_Apply.aspx?sno=" + id + "&CBL_CertificateStutas=" + CBL_CertificateStutas.SelectedValue + "");
                }
                else
                {
                    //btn_Invite.Visible = true;
                }
            }
            else
            {
                if (logic1 && logic2)
                {
                    //Page.Response.Redirect("Event_Apply.aspx?sno=" + id + "&CBL_CertificateStutas=" + CBL_CertificateStutas.SelectedValue + "");
                }
                else
                {
                    //btn_Invite.Visible = true;
                }
            }
        }
        else
        {
            if (LogicSign1 == "||" && LogicSign2 == "&&")
            {
                if ((logic1 || logic2) && logic3)
                {
                    //Page.Response.Redirect("Event_Apply.aspx?sno=" + id + "&CBL_CertificateStutas=" + CBL_CertificateStutas.SelectedValue + "");
                }
                else
                {
                    //btn_Invite.Visible = true;
                    return;
                }
            }
            if (LogicSign1 == "&&" && LogicSign2 == "||")
            {
                if ((logic1 && logic2) || logic3)
                {
                    //Page.Response.Redirect("Event_Apply.aspx?sno=" + id + "&CBL_CertificateStutas=" + CBL_CertificateStutas.SelectedValue + "");
                }
                else
                {
                    //btn_Invite.Visible = true;
                    return;
                }
            }
            if (LogicSign1 == "||")
            {
                if (logic1 || logic2 || logic3)
                {
                    //Page.Response.Redirect("Event_Apply.aspx?sno=" + id + "&CBL_CertificateStutas=" + CBL_CertificateStutas.SelectedValue + "");
                }
                else
                {
                    //btn_Invite.Visible = true;
                    return;
                }
            }
            else
            {
                if (logic1 && logic2 && logic3)
                {
                    //Page.Response.Redirect("Event_Apply.aspx?sno=" + id + "&CBL_CertificateStutas=" + CBL_CertificateStutas.SelectedValue + "");
                }
                else
                {
                    //btn_Invite.Visible = true;
                    return;
                }
            }
        }
    }



    protected void BindGridView(GridView gv, DataTable dt)
    {
        gv.DataSource = dt;
        gv.DataBind();
    }
    public static bool CheckRole(string RoleSNO, string CSNO)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        string sql = @"SELECT R.RoleSNO, R.RoleName, 
	            (Select 1 From RoleBind RB Where RB.RoleSNO=R.RoleSNO And TypeKey='Event_AE' and CSNO=@CSNO) Chk
            FROM Role R 
            WHERE R.IsAdmin=0 and R.RoleSNO=@RoleSNO";
        aDict.Add("RoleSNO", RoleSNO);
        aDict.Add("CSNO", CSNO);
        DataTable ObjDT = objDH.queryData(sql, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            if (String.IsNullOrEmpty(ObjDT.Rows[0]["Chk"].ToString()))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }

    }
      public static bool ChkEventCount(string EventSNO)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, Object> adict = new Dictionary<string, object>();
        string SQL = @"
             SELECT 
                EventSNO, EventName, E.Note, CountLimit, E.TargetHour,
				CountAdmit,Convert(varchar, StartTime, 120) StartTime, Convert(varchar, EndTime, 120) EndTime,
                E.CLocation,E.CDate,E.CTime,E.CPerosn,E.CPhone,E.CMail,
                CF.MVal Class1,CF2.MVal Class2,E.Note,(E.CLocationAreaA+E.CLocationAreaB+E.CLocation)  L,E.QTypeName,
                E.ActiveCost,E.Host,
                " + Utility.setSQL_RoleBindName("Event_AE", "E.EventSNO") + @",E.TargetHour
            From Event E 			
				Left Join config CF on CF.Pval=E.class3 and CF.[PGroup]='CourseClass3' 
				Left Join config CF2 on CF2.Pval=E.class4 and CF2.[PGroup]='CourseClass4'
            Where E.EventSNO=@EventSNO";
        adict.Add("EventSNO", EventSNO);
        DataTable ObjDT = ObjDH.queryData(SQL, adict);
        string CountLimit = ObjDT.Rows[0]["CountLimit"].ToString();
        DataTable LimitDT = ObjDH.queryData(@"
            SELECT p.PName 
            From EventD e
                Left Join Person p On p.PersonSNO=e.PersonSNO
            Where EventSNO=@EventSNO", adict);
        if (LimitDT.Rows.Count < Convert.ToInt16(CountLimit))
        {
            DataTable DT = ObjDH.queryData(@"
            SELECT p.PName 
            From EventD e
                Left Join Person p On p.PersonSNO=e.PersonSNO
            Where EventSNO=@EventSNO and Audit<>2 and Audit<>5", adict);//未審 錄取 審核中 備取
            if (CountLimit != "0")
            {
                if (Convert.ToInt16(CountLimit) <= DT.Rows.Count && Convert.ToInt16(CountLimit) != 0)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return true;

            }
        }
        else
        {
            return false;
        }

    }

}