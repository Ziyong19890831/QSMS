using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_EventRoleDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
            #region 規則Set
            Utility.setPlanName(ddl_CoursePlan10, "請選擇");
            Utility.setPlanName(ddl_CoursePlan10_1, "請選擇");
            //SetCourse(ddl_Course10, ddl_CoursePlan10.SelectedValue, "請選擇");
            Utility.setPlanName(ddl_CoursePlan11, "請選擇");
            Utility.setPlanName(ddl_CoursePlan11_1, "請選擇");
            //SetCourse(ddl_Course11, ddl_CoursePlan11.SelectedValue, "請選擇");
            Utility.setPlanName(ddl_CoursePlan12, "請選擇");
            Utility.setPlanName(ddl_CoursePlan12_1, "請選擇");
            //SetCourse(ddl_Course12, ddl_CoursePlan12.SelectedValue, "請選擇");
            Utility.setPlanName(ddl_CoursePlan13, "請選擇");
            Utility.setPlanName(ddl_CoursePlan13_1, "請選擇");
            //SetCourse(ddl_Course13, ddl_CoursePlan13.SelectedValue, "請選擇");
            Utility.setPlanName(ddl_None10CoursePlaningClass, "請選擇");
            Utility.setCtypeName(ddl_None10Certificate, "請選擇");
            Utility.setCtypeName(ddl_Junior10Certificate, "請選擇");
            Utility.setPlanName(ddl_Junior10CoursePlaningClass, "請選擇");
            Utility.setCtypeName(ddl_Senior10Certificate, "請選擇");
            Utility.setPlanName(ddl_Senior10CoursePlaningClass, "請選擇");
            Utility.setCtypeName(ddl_JuniorSenior10Certificate, "請選擇");
            Utility.setPlanName(ddl_JuniorSenior10CoursePlaningClass, "請選擇");
            Utility.setCtypeName(ddl_None11Certificate, "請選擇");
            Utility.setPlanName(ddl_None11CoursePlaningClass, "請選擇");
            Utility.setCtypeName(ddl_Junior11Certificate, "請選擇");
            Utility.setPlanName(ddl_Junior11CoursePlaningClass, "請選擇");
            Utility.setCtypeName(ddl_Senior11Certificate, "請選擇");
            Utility.setPlanName(ddl_Senior11CoursePlaningClass, "請選擇");
            Utility.setCtypeName(ddl_JuniorSenior11Certificate, "請選擇");
            Utility.setPlanName(ddl_JuniorSenior11CoursePlaningClass, "請選擇");
            Utility.setPlanName(ddl_None12CoursePlaningClass, "請選擇");
            Utility.setCtypeName(ddl_None12Certificate, "請選擇");
            Utility.setCtypeName(ddl_Junior12Certificate, "請選擇");
            Utility.setPlanName(ddl_Junior12CoursePlaningClass, "請選擇");
            Utility.setCtypeName(ddl_Senior12Certificate, "請選擇");
            Utility.setPlanName(ddl_Senior12CoursePlaningClass, "請選擇");
            Utility.setCtypeName(ddl_JuniorSenior12Certificate, "請選擇");
            Utility.setPlanName(ddl_JuniorSenior12CoursePlaningClass, "請選擇");
            Utility.setPlanName(ddl_None13CoursePlaningClass, "請選擇");
            Utility.setCtypeName(ddl_None13Certificate, "請選擇");
            Utility.setCtypeName(ddl_Junior13Certificate, "請選擇");
            Utility.setPlanName(ddl_Junior13CoursePlaningClass, "請選擇");
            Utility.setCtypeName(ddl_Senior13Certificate, "請選擇");
            Utility.setPlanName(ddl_Senior13CoursePlaningClass, "請選擇");
            Utility.setCtypeName(ddl_JuniorSenior13Certificate, "請選擇");
            Utility.setPlanName(ddl_JuniorSenior13CoursePlaningClass, "請選擇");
            #endregion
            string work = "";
            if (Request.QueryString["Work"] != null) work = Request.QueryString["Work"];
            if (work.Equals("N"))
            {
                newData();
            }
            else
            {
                getData();
                //CBL_None10Course.SelectedValue = "1";
            }
        }
    }

    protected void newData()
    {
        Work.Value = "NEW";
  
    }

    protected void getData()
    {
        for (int i = 10; i <= 13; i++)
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            DataHelper objDH = new DataHelper();
            string ERSNO = Request.QueryString["sno"].ToString();
            string SQL = @"Select * from [EventRoleDetail] where ERSNO=@ERSNO And RoleSNO=@RoleSNO";
            aDict.Add("ERSNO", ERSNO);
            aDict.Add("RoleSNO", i);
            DataTable ObjDT = objDH.queryData(SQL, aDict);
            if (ObjDT.Rows.Count > 0)
            {
                string PClassSNO= ObjDT.Rows[0]["PClassSNO"].ToString();
                string CourseSNO = ObjDT.Rows[0]["CourseSNO"].ToString();
                string CourseSNOForJunior = ObjDT.Rows[0]["CourseSNOForJunior"].ToString();
                string NoneRequireCertificate = ObjDT.Rows[0]["NoneRequireCertificate"].ToString();
                string NoneCoursePlanningSNO= ObjDT.Rows[0]["NoneCoursePlanningSNO"].ToString();
                string NoneRequireCourseSNO = ObjDT.Rows[0]["NoneRequireCourseSNO"].ToString();
                string NoneLogic = ObjDT.Rows[0]["NoneLogic"].ToString();
                string JuniorRequireCertificate = ObjDT.Rows[0]["JuniorRequireCertificate"].ToString();
                string JuniorCoursePlanningSNO = ObjDT.Rows[0]["JuniorCoursePlanningSNO"].ToString();
                string JuniorRequireCourseSNO = ObjDT.Rows[0]["JuniorRequireCourseSNO"].ToString();
                string JuniorLogic = ObjDT.Rows[0]["JuniorLogic"].ToString();
                string SeniorRequireCertificate = ObjDT.Rows[0]["SeniorRequireCertificate"].ToString();
                string SeniorCoursePlanningSNO = ObjDT.Rows[0]["SeniorCoursePlanningSNO"].ToString();
                string SeniorRequireCourseSNO = ObjDT.Rows[0]["SeniorRequireCourseSNO"].ToString();
                string SeniorLogic = ObjDT.Rows[0]["SeniorLogic"].ToString();
                string JuniorSeniorRequireCertificate = ObjDT.Rows[0]["JuniorSeniorRequireCertificate"].ToString();
                string JuniorSeniorCoursePlanningSNO = ObjDT.Rows[0]["JuniorSeniorCoursePlanningSNO"].ToString();
                string JuniorSeniorRequireCourseSNO = ObjDT.Rows[0]["JuniorSeniorRequireCourseSNO"].ToString();
                string JuniorSeniorLogic = ObjDT.Rows[0]["JuniorSeniorLogic"].ToString();

                string[] NoneRequireCertificateArray = NoneRequireCertificate.Split(';');
                string[] NoneCoursePlanningSNOArray = NoneCoursePlanningSNO.Split(';');
                string[] NoneRequireCourseSNOArray = NoneRequireCourseSNO.Split(';');
                string[] NoneLogicArray = NoneLogic.Split(';');
                string[] JuniorRequireCertificateArray = JuniorRequireCertificate.Split(';');
                string[] JuniorCoursePlanningSNOArray = JuniorCoursePlanningSNO.Split(';');
                string[] JuniorRequireCourseSNOArray = JuniorRequireCourseSNO.Split(';');
                string[] JuniorLogicArray = JuniorLogic.Split(';');
                string[] SeniorRequireCertificateArray = SeniorRequireCertificate.Split(';');
                string[] SeniorCoursePlanningSNOArray = SeniorCoursePlanningSNO.Split(';');
                string[] SeniorRequireCourseSNOArray = SeniorRequireCourseSNO.Split(';');
                string[] SeniorLogicArray = SeniorLogic.Split(';');
                string[] JuniorSeniorRequireCertificateArray = JuniorSeniorRequireCertificate.Split(';');
                string[] JuniorSeniorCoursePlanningSNOArray = JuniorSeniorCoursePlanningSNO.Split(';');
                string[] JuniorSeniorRequireCourseSNOArray = JuniorSeniorRequireCourseSNO.Split(';');
                string[] JuniorSeniorLogicArray = JuniorSeniorLogic.Split(';');

                switch (i)
                {
                    case 10:
                        ddl_CoursePlan10.SelectedValue = PClassSNO;
                        txt_CourseSNO_10.Text = CourseSNO;
                        txt_CourseSNO_10_1.Text = CourseSNOForJunior;
                        for (int j = 0; j < NoneRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {
                                
                                ddl_None10Certificate.SelectedValue = NoneRequireCertificateArray[j];
                                ddl_None10CoursePlaningClass.SelectedValue = NoneCoursePlanningSNOArray[j];
                                getCourse(CBL_None10Course,NoneCoursePlanningSNOArray[j], NoneRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Role1.Enabled = true;
                                chk_None10P1.Checked=true;
                                ddl_Logic1.SelectedValue = NoneLogicArray[j];
                                getCertificate(ddl_Certificate1, NoneRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_None10CoursePlaningClass1, NoneCoursePlanningSNOArray[j], "請選擇");                  
                                getCourse(CBL_Course1, NoneCoursePlanningSNOArray[j], NoneRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Role2.Enabled = true;
                                chk_None10P2.Checked = true;
                                ddl_Logic2.SelectedValue = NoneLogicArray[j];
                                getCertificate(ddl_Certificate2, NoneRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_None10CoursePlaningClass2, NoneCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Course2, NoneCoursePlanningSNOArray[j], NoneRequireCourseSNOArray[j]);
                            }
                        }
                        for (int j = 0; j < JuniorRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {

                                ddl_Junior10Certificate.SelectedValue = JuniorRequireCertificateArray[j];
                                ddl_Junior10CoursePlaningClass.SelectedValue = JuniorCoursePlanningSNOArray[j];
                                getCourse(CBL_Junior10, JuniorCoursePlanningSNOArray[j], JuniorRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Panel1.Enabled = true;
                                chk_JuniorP3.Checked = true;
                                DropDownList3.SelectedValue = JuniorLogicArray[j];
                                getCertificate(ddl_Junior10Certificate1, JuniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Junior10CoursePlaningClass1, JuniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Junior10_1, JuniorCoursePlanningSNOArray[j], JuniorRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Panel2.Enabled = true;
                                chk_JuniorP4.Checked = true;
                                DropDownList6.SelectedValue= JuniorLogicArray[j];
                                getCertificate(ddl_Junior10Certificate2, JuniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Junior10CoursePlaningClass2, JuniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Junior10_2, JuniorCoursePlanningSNOArray[j], JuniorRequireCourseSNOArray[j]);
                            }
                        }
                        for (int j = 0; j < SeniorRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {

                                ddl_Senior10Certificate.SelectedValue = SeniorRequireCertificateArray[j];
                                ddl_Senior10CoursePlaningClass.SelectedValue = SeniorCoursePlanningSNOArray[j];
                                getCourse(CBL_Sunior10, SeniorCoursePlanningSNOArray[j], SeniorRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Panel3.Enabled = true;
                                chk_P5.Checked = true;
                                DropDownList11.SelectedValue = SeniorLogicArray[j];
                                getCertificate(ddl_Senior10Certificate1, SeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Senior10CoursePlaningClass1, SeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Sunior10_1, SeniorCoursePlanningSNOArray[j], SeniorRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Panel4.Enabled = true;
                                chk_P6.Checked = true;
                                DropDownList14.SelectedValue= SeniorLogicArray[j];
                                getCertificate(ddl_Senior10Certificate2, SeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Senior10CoursePlaningClass2, SeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Sunior10_2, SeniorCoursePlanningSNOArray[j], SeniorRequireCourseSNOArray[j]);
                            }
                        }
                        for (int j = 0; j < JuniorSeniorRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {

                                ddl_JuniorSenior10Certificate.SelectedValue = JuniorSeniorRequireCertificateArray[j];
                                ddl_JuniorSenior10CoursePlaningClass.SelectedValue = JuniorSeniorCoursePlanningSNOArray[j];
                                getCourse(CBL_JuniorSenior10, JuniorSeniorCoursePlanningSNOArray[j], JuniorSeniorRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Panel5.Enabled = true;
                                chk_P7.Checked = true;
                                DropDownList19.SelectedValue = JuniorSeniorLogicArray[j];
                                getCertificate(ddl_JuniorSenior10Certificate1, JuniorSeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_JuniorSenior10CoursePlaningClass1, JuniorSeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_JuniorSenior10_1, JuniorSeniorCoursePlanningSNOArray[j], JuniorSeniorRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Panel6.Enabled = true;
                                chk_P8.Checked = true;
                                DropDownList22.SelectedValue = JuniorSeniorLogicArray[j];
                                getCertificate(ddl_JuniorSenior10Certificate2, JuniorSeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_JuniorSenior10CoursePlaningClass2, JuniorSeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_JuniorSenior10_2, JuniorSeniorCoursePlanningSNOArray[j], JuniorSeniorRequireCourseSNOArray[j]);
                            }
                        }
                        break;
                    case 11:
                        ddl_CoursePlan11.SelectedValue = PClassSNO;
                        txt_CourseSNO_11.Text = CourseSNO;
                        txt_CourseSNO_11_1.Text = CourseSNOForJunior;
                        for (int j = 0; j < NoneRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {

                                ddl_None11Certificate.SelectedValue = NoneRequireCertificateArray[j];
                                ddl_None11CoursePlaningClass.SelectedValue = NoneCoursePlanningSNOArray[j];
                                getCourse(CBL_None11Course, NoneCoursePlanningSNOArray[j], NoneRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Panel7.Enabled = true;
                                chk_P9.Checked = true;
                                DropDownList27.SelectedValue = NoneLogicArray[j];
                                getCertificate(ddl_None11Certificate1, NoneRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_None11CoursePlaningClass1, NoneCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_None11Course1, NoneCoursePlanningSNOArray[j], NoneRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Panel8.Enabled = true;
                                chk_P10.Checked = true;
                                DropDownList30.SelectedValue= NoneLogicArray[j];
                                getCertificate(ddl_None11Certificate2, NoneRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_None11CoursePlaningClass2, NoneCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_None11Course2, NoneCoursePlanningSNOArray[j], NoneRequireCourseSNOArray[j]);
                            }
                        }
                        for (int j = 0; j < JuniorRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {

                                ddl_Junior11Certificate.SelectedValue = JuniorRequireCertificateArray[j];
                                ddl_Junior11CoursePlaningClass.SelectedValue = JuniorCoursePlanningSNOArray[j];
                                getCourse(CBL_Junior11, JuniorCoursePlanningSNOArray[j], JuniorRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Panel9.Enabled = true;
                                chk_P11.Checked = true;
                                DropDownList35.SelectedValue = JuniorLogicArray[j];
                                getCertificate(ddl_Junior11Certificate1, JuniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Junior11CoursePlaningClass1, JuniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Junior11_1, JuniorCoursePlanningSNOArray[j], JuniorRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Panel10.Enabled = true;
                                chk_P12.Checked = true;
                                DropDownList38.SelectedValue = JuniorLogicArray[j];
                                getCertificate(ddl_Junior11Certificate2, JuniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Junior11CoursePlaningClass2, JuniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Junior11_2, JuniorCoursePlanningSNOArray[j], JuniorRequireCourseSNOArray[j]);
                            }
                        }
                        for (int j = 0; j < SeniorRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {

                                ddl_Senior11Certificate.SelectedValue = SeniorRequireCertificateArray[j];
                                ddl_Senior11CoursePlaningClass.SelectedValue = SeniorCoursePlanningSNOArray[j];
                                getCourse(CBL_Sunior11, SeniorCoursePlanningSNOArray[j], SeniorRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Panel11.Enabled = true;
                                chk_P13.Checked = true;
                                DropDownList43.SelectedValue = SeniorLogicArray[j];
                                getCertificate(ddl_Senior11Certificate1, SeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Senior11CoursePlaningClass1, SeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Sunior11_1, SeniorCoursePlanningSNOArray[j], SeniorRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Panel12.Enabled = true;
                                chk_P14.Checked = true;
                                DropDownList46.SelectedValue = SeniorLogicArray[j];
                                getCertificate(ddl_Senior11Certificate2, SeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Senior11CoursePlaningClass2, SeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Sunior11_2, SeniorCoursePlanningSNOArray[j], SeniorRequireCourseSNOArray[j]);
                            }
                        }
                        for (int j = 0; j < JuniorSeniorRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {

                                ddl_JuniorSenior11Certificate.SelectedValue = JuniorSeniorRequireCertificateArray[j];
                                ddl_JuniorSenior11CoursePlaningClass.SelectedValue = JuniorSeniorCoursePlanningSNOArray[j];
                                getCourse(CBL_JuniorSenior11, JuniorSeniorCoursePlanningSNOArray[j], JuniorSeniorRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Panel13.Enabled = true;
                                chk_P15.Checked = true;
                                DropDownList51.SelectedValue = JuniorSeniorLogicArray[j];
                                getCertificate(ddl_JuniorSenior11Certificate1, JuniorSeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_JuniorSenior11CoursePlaningClass1, JuniorSeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_JuniorSenior11_1, JuniorSeniorCoursePlanningSNOArray[j], JuniorSeniorRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Panel14.Enabled = true;
                                chk_P16.Checked = true;
                                DropDownList54.SelectedValue = JuniorSeniorLogicArray[j];
                                getCertificate(ddl_JuniorSenior11Certificate2, JuniorSeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_JuniorSenior11CoursePlaningClass2, JuniorSeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_JuniorSenior11_2, JuniorSeniorCoursePlanningSNOArray[j], JuniorSeniorRequireCourseSNOArray[j]);
                            }
                        }
                        break;
                    case 12:
                        ddl_CoursePlan12.SelectedValue = PClassSNO;
                        txt_CourseSNO_12.Text = CourseSNO;
                        txt_CourseSNO_12_1.Text = CourseSNOForJunior;
                        for (int j = 0; j < NoneRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {

                                ddl_None12Certificate.SelectedValue = NoneRequireCertificateArray[j];
                                ddl_None12CoursePlaningClass.SelectedValue = NoneCoursePlanningSNOArray[j];
                                getCourse(CBL_None12Course, NoneCoursePlanningSNOArray[j], NoneRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Panel15.Enabled = true;
                                chk_P17.Checked = true;
                                DropDownList59.SelectedValue = NoneLogicArray[j];
                                getCertificate(ddl_None12Certificate1, NoneRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_None12CoursePlaningClass1, NoneCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_None12Course1, NoneCoursePlanningSNOArray[j], NoneRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Panel16.Enabled = true;
                                chk_P18.Checked = true;
                                DropDownList62.SelectedValue = NoneLogicArray[j];
                                getCertificate(ddl_None12Certificate2, NoneRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_None12CoursePlaningClass2, NoneCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_None12Course2, NoneCoursePlanningSNOArray[j], NoneRequireCourseSNOArray[j]);
                            }
                        }
                        for (int j = 0; j < JuniorRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {

                                ddl_Junior12Certificate.SelectedValue = JuniorRequireCertificateArray[j];
                                ddl_Junior12CoursePlaningClass.SelectedValue = JuniorCoursePlanningSNOArray[j];
                                getCourse(CBL_Junior12, JuniorCoursePlanningSNOArray[j], JuniorRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Panel17.Enabled = true;
                                chk_P19.Checked = true;
                                DropDownList67.SelectedValue = JuniorLogicArray[j];
                                getCertificate(ddl_Junior12Certificate1, JuniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Junior12CoursePlaningClass1, JuniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Junior12_1, JuniorCoursePlanningSNOArray[j], JuniorRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Panel18.Enabled = true;
                                chk_P20.Checked = true;
                                DropDownList70.SelectedValue = JuniorLogicArray[j];
                                getCertificate(ddl_Junior12Certificate2, JuniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Junior12CoursePlaningClass2, JuniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Junior12_2, JuniorCoursePlanningSNOArray[j], JuniorRequireCourseSNOArray[j]);
                            }
                        }
                        for (int j = 0; j < SeniorRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {

                                ddl_Senior12Certificate.SelectedValue = SeniorRequireCertificateArray[j];
                                ddl_Senior12CoursePlaningClass.SelectedValue = SeniorCoursePlanningSNOArray[j];
                                getCourse(CBL_Sunior12, SeniorCoursePlanningSNOArray[j], SeniorRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Panel19.Enabled = true;
                                chk_P21.Checked = true;
                                DropDownList75.SelectedValue = SeniorLogicArray[j];
                                getCertificate(ddl_Senior12Certificate1, SeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Senior12CoursePlaningClass1, SeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Sunior12_1, SeniorCoursePlanningSNOArray[j], SeniorRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Panel20.Enabled = true;
                                chk_P22.Checked = true;
                                DropDownList78.SelectedValue = SeniorLogicArray[j];
                                getCertificate(ddl_Senior12Certificate2, SeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Senior12CoursePlaningClass2, SeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Sunior12_2, SeniorCoursePlanningSNOArray[j], SeniorRequireCourseSNOArray[j]);
                            }
                        }
                        for (int j = 0; j < JuniorSeniorRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {

                                ddl_JuniorSenior12Certificate.SelectedValue = JuniorSeniorRequireCertificateArray[j];
                                ddl_JuniorSenior12CoursePlaningClass.SelectedValue = JuniorSeniorCoursePlanningSNOArray[j];
                                getCourse(CBL_JuniorSenior12, JuniorSeniorCoursePlanningSNOArray[j], JuniorSeniorRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Panel21.Enabled = true;
                                chk_P23.Checked = true;
                                DropDownList83.SelectedValue = JuniorSeniorLogicArray[j];
                                getCertificate(ddl_JuniorSenior12Certificate1, JuniorSeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_JuniorSenior12CoursePlaningClass1, JuniorSeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_JuniorSenior12_1, JuniorSeniorCoursePlanningSNOArray[j], JuniorSeniorRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Panel22.Enabled = true;
                                chk_P24.Checked = true;
                                DropDownList86.SelectedValue = JuniorSeniorLogicArray[j];
                                getCertificate(ddl_JuniorSenior12Certificate2, JuniorSeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_JuniorSenior12CoursePlaningClass2, JuniorSeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_JuniorSenior12_2, JuniorSeniorCoursePlanningSNOArray[j], JuniorSeniorRequireCourseSNOArray[j]);
                            }
                        }
                        break;
                    case 13:
                        ddl_CoursePlan13.SelectedValue = PClassSNO;
                        txt_CourseSNO_13.Text = CourseSNO;
                        txt_CourseSNO_13_1.Text = CourseSNOForJunior;
                        for (int j = 0; j < NoneRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {

                                ddl_None13Certificate.SelectedValue = NoneRequireCertificateArray[j];
                                ddl_None13CoursePlaningClass.SelectedValue = NoneCoursePlanningSNOArray[j];
                                getCourse(CBL_None13Course, NoneCoursePlanningSNOArray[j], NoneRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Panel23.Enabled = true;
                                chk_P25.Checked = true;
                                DropDownList91.SelectedValue = NoneLogicArray[j];
                                getCertificate(ddl_None13Certificate1, NoneRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_None13CoursePlaningClass1, NoneCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_None13Course1, NoneCoursePlanningSNOArray[j], NoneRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Panel24.Enabled = true;
                                chk_P26.Checked = true;
                                DropDownList94.SelectedValue = NoneLogicArray[j];
                                getCertificate(ddl_None13Certificate2, NoneRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_None13CoursePlaningClass2, NoneCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_None13Course2, NoneCoursePlanningSNOArray[j], NoneRequireCourseSNOArray[j]);
                            }
                        }
                        for (int j = 0; j < JuniorRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {

                                ddl_Junior13Certificate.SelectedValue = JuniorRequireCertificateArray[j];
                                ddl_Junior13CoursePlaningClass.SelectedValue = JuniorCoursePlanningSNOArray[j];
                                getCourse(CBL_Junior13, JuniorCoursePlanningSNOArray[j], JuniorRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Panel25.Enabled = true;
                                chk_P27.Checked = true;
                                DropDownList99.SelectedValue = JuniorLogicArray[j];
                                getCertificate(ddl_Junior13Certificate1, JuniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Junior13CoursePlaningClass1, JuniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Junior13_1, JuniorCoursePlanningSNOArray[j], JuniorRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Panel26.Enabled = true;
                                chk_P28.Checked = true;
                                DropDownList102.SelectedValue = JuniorLogicArray[j];
                                getCertificate(ddl_Junior13Certificate2, JuniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Junior13CoursePlaningClass2, JuniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Junior13_2, JuniorCoursePlanningSNOArray[j], JuniorRequireCourseSNOArray[j]);
                            }
                        }
                        for (int j = 0; j < SeniorRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {

                                ddl_Senior13Certificate.SelectedValue = SeniorRequireCertificateArray[j];
                                ddl_Senior13CoursePlaningClass.SelectedValue = SeniorCoursePlanningSNOArray[j];
                                getCourse(CBL_Sunior13, SeniorCoursePlanningSNOArray[j], SeniorRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Panel27.Enabled = true;
                                chk_P29.Checked = true;
                                DropDownList107.SelectedValue = SeniorLogicArray[j];
                                getCertificate(ddl_Senior13Certificate1, SeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Senior13CoursePlaningClass1, SeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Sunior13_1, SeniorCoursePlanningSNOArray[j], SeniorRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Panel28.Enabled = true;
                                chk_P30.Checked = true;
                                DropDownList110.SelectedValue = SeniorLogicArray[j];
                                getCertificate(ddl_Senior13Certificate2, SeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_Senior13CoursePlaningClass2, SeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_Sunior13_2, SeniorCoursePlanningSNOArray[j], SeniorRequireCourseSNOArray[j]);
                            }
                        }
                        for (int j = 0; j < JuniorSeniorRequireCertificateArray.Length; j++)
                        {
                            if (j == 0)
                            {

                                ddl_JuniorSenior13Certificate.SelectedValue = JuniorSeniorRequireCertificateArray[j];
                                ddl_JuniorSenior13CoursePlaningClass.SelectedValue = JuniorSeniorCoursePlanningSNOArray[j];
                                getCourse(CBL_JuniorSenior13, JuniorSeniorCoursePlanningSNOArray[j], JuniorSeniorRequireCourseSNOArray[j]);

                            }
                            if (j == 1)
                            {
                                Panel29.Enabled = true;
                                chk_P31.Checked = true;
                                DropDownList115.SelectedValue = JuniorSeniorLogicArray[j];
                                getCertificate(ddl_JuniorSenior13Certificate1, JuniorSeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_JuniorSenior13CoursePlaningClass1, JuniorSeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_JuniorSenior13_1, JuniorSeniorCoursePlanningSNOArray[j], JuniorSeniorRequireCourseSNOArray[j]);
                            }
                            if (j == 2)
                            {
                                Panel30.Enabled = true;
                                chk_P32.Checked = true;
                                DropDownList118.SelectedValue = JuniorSeniorLogicArray[j];
                                getCertificate(ddl_JuniorSenior13Certificate2, JuniorSeniorRequireCertificateArray[j], "請選擇");
                                getCoursePlanning(ddl_JuniorSenior13CoursePlaningClass2, JuniorSeniorCoursePlanningSNOArray[j], "請選擇");
                                getCourse(CBL_JuniorSenior13_2, JuniorSeniorCoursePlanningSNOArray[j], JuniorSeniorRequireCourseSNOArray[j]);
                            }
                        }
                        break;
                }

            }
        }
       
    }
    protected void SetCourse(System.Web.UI.WebControls.DropDownList ddl,string PClassSNO, String DefaultString = null)
    {
        ddl.Items.Clear();
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        wDict.Add("PClassSNO", PClassSNO);
        DataTable objDT = objDH.queryData("SELECT CourseSNO, CourseName FROM QS_Course where PClassSNO=@PClassSNO and Ctype=2", wDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));

        }
    }

    #region 規則之UI建立
    protected void ddl_None10CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_None10CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_None10Course.DataSource = objDT;
        CBL_None10Course.DataBind();
    }
    public void getCoursePlanning(DropDownList ddl,string PClassSNO, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        aDict.Add("PClassSNO", PClassSNO);
        DataTable objDT = objDH.queryData("SELECT [PClassSNO], [PlanName] FROM [QS_CoursePlanningClass] WHERE [IsEnable] = 1 ", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
        ddl.SelectedValue = PClassSNO;

    }
    public void getCertificate(DropDownList ddl, string CtypeSNO, string DefaultString = null)
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        aDict.Add("CtypeSNO", CtypeSNO);
        DataTable objDT = objDH.queryData("SELECT [CTypeSNO],[CTypeName]  FROM [QS_CertificateType]", aDict);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new ListItem(DefaultString, ""));
        }
        ddl.SelectedValue = CtypeSNO;

    }
    public void getCourse(CheckBoxList check,string CoursePlaningSNO,string CourseSNO)
    {
        
        string[] CourseAttay = CourseSNO.Split(',');
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", CoursePlaningSNO);
        DataTable objDT = objDH.queryData(sql, wDict);
        check.DataSource = objDT;
        check.DataBind();
        foreach (ListItem item in check.Items)
        {
            for (int i = 0; i < CourseAttay.Length; i++)
            {
                if(item.Value== CourseAttay[i])
                {
                    item.Selected = true;
                }
            }
              
        }
    }
    protected void chk_P1_None10CheckedChanged(object sender, EventArgs e)
    {
        if (chk_None10P1.Checked)
        {
            Role1.Enabled = true;
            Utility.setPlanName(ddl_None10CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_Certificate1, "請選擇");
        }
        else
        {
            Role1.Enabled = false;
        }
    }

    protected void chk_P2_None10CheckedChanged(object sender, EventArgs e)
    {
        if (chk_None10P2.Checked)
        {
            Role2.Enabled = true;
            Utility.setPlanName(ddl_None10CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_Certificate2, "請選擇");
        }
        else
        {
            Role2.Enabled = false;
        }
    }

    protected void ddl_None10CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_None10CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Course1.DataSource = objDT;
        CBL_Course1.DataBind();
    }

    protected void ddl_None10CoursePlaningClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_None10CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Course2.DataSource = objDT;
        CBL_Course2.DataBind();
    }

    protected void chk_JuniorP4_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_JuniorP4.Checked)
        {
            Panel2.Enabled = true;
            Utility.setPlanName(ddl_Junior10CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_Junior10Certificate2, "請選擇");
        }
        else
        {
            Panel2.Enabled = false;
        }
    }

    protected void chk_JuniorP3_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_JuniorP3.Checked)
        {
            Panel1.Enabled = true;
            Utility.setPlanName(ddl_Junior10CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_Junior10Certificate1, "請選擇");
        }
        else
        {
            Panel1.Enabled = false;
        }
    }

    protected void ddl_Junior10CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Junior10CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Junior10.DataSource = objDT;
        CBL_Junior10.DataBind();
    }

    protected void ddl_Junior10CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Junior10CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Junior10_1.DataSource = objDT;
        CBL_Junior10_1.DataBind();
    }

    protected void ddl_Junior10Certificate2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Junior10CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Junior10_2.DataSource = objDT;
        CBL_Junior10_2.DataBind();
    }


    protected void chk_P5_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P5.Checked)
        {
            Panel3.Enabled = true;
            Utility.setPlanName(ddl_Senior10CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_Senior10Certificate1, "請選擇");
        }
        else
        {
            Panel3.Enabled = false;
        }
    }

    protected void chk_P6_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P6.Checked)
        {
            Panel4.Enabled = true;
            Utility.setPlanName(ddl_Senior10CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_Senior10Certificate2, "請選擇");
        }
        else
        {
            Panel4.Enabled = false;
        }
    }

    protected void ddl_Senior10CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Senior10CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Sunior10.DataSource = objDT;
        CBL_Sunior10.DataBind();
    }

    protected void ddl_Senior10CoursePlaningClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Senior10CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Sunior10_2.DataSource = objDT;
        CBL_Sunior10_2.DataBind();
    }

    protected void ddl_Senior10CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Senior10CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Sunior10_1.DataSource = objDT;
        CBL_Sunior10_1.DataBind();
    }

    protected void chk_P7_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P7.Checked)
        {
            Panel5.Enabled = true;
            Utility.setPlanName(ddl_JuniorSenior10CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_JuniorSenior10Certificate1, "請選擇");
        }
        else
        {
            Panel5.Enabled = false;
        }
    }

    protected void chk_P8_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P8.Checked)
        {
            Panel6.Enabled = true;
            Utility.setPlanName(ddl_JuniorSenior10CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_JuniorSenior10Certificate2, "請選擇");
        }
        else
        {
            Panel6.Enabled = false;
        }
    }

    protected void ddl_JuniorSenior10CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_JuniorSenior10CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_JuniorSenior10.DataSource = objDT;
        CBL_JuniorSenior10.DataBind();
    }

    protected void ddl_JuniorSenior10CoursePlaningClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_JuniorSenior10CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_JuniorSenior10_2.DataSource = objDT;
        CBL_JuniorSenior10_2.DataBind();
    }

    protected void ddl_JuniorSenior10CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_JuniorSenior10CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_JuniorSenior10_1.DataSource = objDT;
        CBL_JuniorSenior10_1.DataBind();
    }

    protected void chk_P10_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P10.Checked)
        {
            Panel8.Enabled = true;
            Utility.setPlanName(ddl_None11CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_None11Certificate2, "請選擇");
        }
        else
        {
            Panel8.Enabled = false;
        }
    }

    protected void chk_P9_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P9.Checked)
        {
            Panel7.Enabled = true;
            Utility.setPlanName(ddl_None11CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_None11Certificate1, "請選擇");
        }
        else
        {
            Panel7.Enabled = false;
        }
    }

    protected void ddl_None11CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_None11CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_None11Course.DataSource = objDT;
        CBL_None11Course.DataBind();
    }

    protected void ddl_None11CoursePlaningClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_None11CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_None11Course2.DataSource = objDT;
        CBL_None11Course2.DataBind();
    }

    protected void ddl_None11CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_None11CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_None11Course1.DataSource = objDT;
        CBL_None11Course1.DataBind();
    }

    protected void chk_P11_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P11.Checked)
        {
            Panel9.Enabled = true;
            Utility.setPlanName(ddl_Junior11CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_Junior11Certificate1, "請選擇");
        }
        else
        {
            Panel9.Enabled = false;
        }
    }

    protected void chk_P12_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P12.Checked)
        {
            Panel10.Enabled = true;
            Utility.setPlanName(ddl_Junior11CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_Junior11Certificate2, "請選擇");
        }
        else
        {
            Panel10.Enabled = false;
        }
    }

    protected void ddl_Junior11CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Junior11CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Junior11.DataSource = objDT;
        CBL_Junior11.DataBind();
    }

    protected void ddl_Junior11CoursePlaningClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Junior11CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Junior11_2.DataSource = objDT;
        CBL_Junior11_2.DataBind();
    }

    protected void ddl_Junior11CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Junior11CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Junior11_1.DataSource = objDT;
        CBL_Junior11_1.DataBind();
    }

    protected void chk_P13_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P13.Checked)
        {
            Panel11.Enabled = true;
            Utility.setPlanName(ddl_Senior11CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_Senior11Certificate1, "請選擇");
        }
        else
        {
            Panel11.Enabled = false;
        }
    }

    protected void chk_P14_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P14.Checked)
        {
            Panel12.Enabled = true;
            Utility.setPlanName(ddl_Senior11CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_Senior11Certificate2, "請選擇");
        }
        else
        {
            Panel12.Enabled = false;
        }
    }

    protected void ddl_Senior11CoursePlaningClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Senior11CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Sunior11_2.DataSource = objDT;
        CBL_Sunior11_2.DataBind();
    }

    protected void ddl_Senior11CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Senior11CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Sunior11_1.DataSource = objDT;
        CBL_Sunior11_1.DataBind();
    }

    protected void ddl_Senior11CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Senior11CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Sunior11.DataSource = objDT;
        CBL_Sunior11.DataBind();
    }

    protected void chk_P15_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P15.Checked)
        {
            Panel13.Enabled = true;
            Utility.setPlanName(ddl_JuniorSenior11CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_JuniorSenior11Certificate1, "請選擇");
        }
        else
        {
            Panel13.Enabled = false;
        }
    }

    protected void chk_P16_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P16.Checked)
        {
            Panel14.Enabled = true;
            Utility.setPlanName(ddl_JuniorSenior11CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_JuniorSenior11Certificate2, "請選擇");
        }
        else
        {
            Panel14.Enabled = false;
        }
    }

    protected void ddl_JuniorSenior11CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_JuniorSenior11CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_JuniorSenior11.DataSource = objDT;
        CBL_JuniorSenior11.DataBind();
    }

    protected void ddl_JuniorSenior11CoursePlaningClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_JuniorSenior11CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_JuniorSenior11_2.DataSource = objDT;
        CBL_JuniorSenior11_2.DataBind();
    }

    protected void ddl_JuniorSenior11CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_JuniorSenior11CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_JuniorSenior11_1.DataSource = objDT;
        CBL_JuniorSenior11_1.DataBind();
    }

    protected void chk_P18_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P18.Checked)
        {
            Panel16.Enabled = true;
            Utility.setPlanName(ddl_None12CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_None12Certificate2, "請選擇");
        }
        else
        {
            Panel16.Enabled = false;
        }
    }

    protected void chk_P17_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P17.Checked)
        {
            Panel15.Enabled = true;
            Utility.setPlanName(ddl_None12CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_None12Certificate1, "請選擇");
        }
        else
        {
            Panel15.Enabled = false;
        }
    }

    protected void ddl_None12CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_None12CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_None12Course.DataSource = objDT;
        CBL_None12Course.DataBind();
    }

    protected void ddl_None12CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_None12CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_None12Course1.DataSource = objDT;
        CBL_None12Course1.DataBind();
    }

    protected void ddl_None12CoursePlaningClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_None12CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_None12Course2.DataSource = objDT;
        CBL_None12Course2.DataBind();
    }

    protected void chk_P19_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P19.Checked)
        {
            Panel17.Enabled = true;
            Utility.setPlanName(ddl_Junior12CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_Junior12Certificate1, "請選擇");
        }
        else
        {
            Panel17.Enabled = false;
        }
    }

    protected void chk_P20_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P20.Checked)
        {
            Panel18.Enabled = true;
            Utility.setPlanName(ddl_Junior12CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_Junior12Certificate2, "請選擇");
        }
        else
        {
            Panel18.Enabled = false;
        }
    }

    protected void ddl_Junior12CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Junior12CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Junior12.DataSource = objDT;
        CBL_Junior12.DataBind();
    }

    protected void ddl_Junior12CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Junior12CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Junior12_1.DataSource = objDT;
        CBL_Junior12_1.DataBind();
    }

    protected void ddl_Junior12CoursePlaningClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Junior12CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Junior12_2.DataSource = objDT;
        CBL_Junior12_2.DataBind();
    }

    protected void chk_P21_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P21.Checked)
        {
            Panel19.Enabled = true;
            Utility.setPlanName(ddl_Senior12CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_Senior12Certificate1, "請選擇");
        }
        else
        {
            Panel19.Enabled = false;
        }
    }

    protected void chk_P22_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P22.Checked)
        {
            Panel20.Enabled = true;
            Utility.setPlanName(ddl_Senior12CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_Senior12Certificate2, "請選擇");
        }
        else
        {
            Panel20.Enabled = false;
        }

    }

    protected void ddl_Senior12CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Senior12CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Sunior12.DataSource = objDT;
        CBL_Sunior12.DataBind();
    }

    protected void ddl_Senior12CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Senior12CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Sunior12_1.DataSource = objDT;
        CBL_Sunior12_1.DataBind();
    }

    protected void ddl_Senior12CoursePlaningClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Senior12CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Sunior12_2.DataSource = objDT;
        CBL_Sunior12_2.DataBind();
    }

    protected void chk_P23_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P23.Checked)
        {
            Panel21.Enabled = true;
            Utility.setPlanName(ddl_JuniorSenior12CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_JuniorSenior12Certificate1, "請選擇");
        }
        else
        {
            Panel21.Enabled = false;
        }
    }

    protected void chk_P24_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P24.Checked)
        {
            Panel22.Enabled = true;
            Utility.setPlanName(ddl_JuniorSenior12CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_JuniorSenior12Certificate2, "請選擇");
        }
        else
        {
            Panel22.Enabled = false;
        }
    }

    protected void ddl_JuniorSenior12CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_JuniorSenior12CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_JuniorSenior12.DataSource = objDT;
        CBL_JuniorSenior12.DataBind();
    }

    protected void ddl_JuniorSenior12CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_JuniorSenior12CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_JuniorSenior12_1.DataSource = objDT;
        CBL_JuniorSenior12_1.DataBind();
    }

    protected void ddl_JuniorSenior12CoursePlaningClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_JuniorSenior12CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_JuniorSenior12_2.DataSource = objDT;
        CBL_JuniorSenior12_2.DataBind();
    }

    protected void chk_P25_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P25.Checked)
        {
            Panel23.Enabled = true;
            Utility.setPlanName(ddl_None13CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_None13Certificate1, "請選擇");
        }
        else
        {
            Panel23.Enabled = false;
        }
    }

    protected void chk_P26_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P26.Checked)
        {
            Panel24.Enabled = true;
            Utility.setPlanName(ddl_None13CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_None13Certificate2, "請選擇");
        }
        else
        {
            Panel24.Enabled = false;
        }
    }

    protected void ddl_None13CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_None13CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_None13Course.DataSource = objDT;
        CBL_None13Course.DataBind();
    }

    protected void ddl_None13CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_None13CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_None13Course1.DataSource = objDT;
        CBL_None13Course1.DataBind();
    }

    protected void ddl_None13CoursePlaningClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_None13CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_None13Course2.DataSource = objDT;
        CBL_None13Course2.DataBind();
    }

    protected void chk_P27_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P27.Checked)
        {
            Panel25.Enabled = true;
            Utility.setPlanName(ddl_Junior13CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_Junior13Certificate1, "請選擇");
        }
        else
        {
            Panel25.Enabled = false;
        }
    }

    protected void chk_P28_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P28.Checked)
        {
            Panel26.Enabled = true;
            Utility.setPlanName(ddl_Junior13CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_Junior13Certificate2, "請選擇");
        }
        else
        {
            Panel26.Enabled = false;
        }
    }


    protected void ddl_Junior13CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Junior13CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Junior13.DataSource = objDT;
        CBL_Junior13.DataBind();
    }

    protected void ddl_Junior13CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Junior13CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Junior13_1.DataSource = objDT;
        CBL_Junior13_1.DataBind();
    }

    protected void ddl_Junior13CoursePlaningClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Junior13CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Junior13_2.DataSource = objDT;
        CBL_Junior13_2.DataBind();
    }

    protected void chk_P29_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P29.Checked)
        {
            Panel27.Enabled = true;
            Utility.setPlanName(ddl_Senior13CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_Senior13Certificate1, "請選擇");
        }
        else
        {
            Panel27.Enabled = false;
        }
    }

    protected void chk_P30_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P30.Checked)
        {
            Panel28.Enabled = true;
            Utility.setPlanName(ddl_Senior13CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_Senior13Certificate2, "請選擇");
        }
        else
        {
            Panel28.Enabled = false;
        }
    }

    protected void ddl_Senior13CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Senior13CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Sunior13.DataSource = objDT;
        CBL_Sunior13.DataBind();
    }

    protected void ddl_Senior13CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Senior13CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Sunior13_1.DataSource = objDT;
        CBL_Sunior13_1.DataBind();
    }

    protected void ddl_Senior13CoursePlaningClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_Senior13CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_Sunior13_2.DataSource = objDT;
        CBL_Sunior13_2.DataBind();
    }

    protected void chk_P31_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P31.Checked)
        {
            Panel29.Enabled = true;
            Utility.setPlanName(ddl_JuniorSenior13CoursePlaningClass1, "請選擇");
            Utility.setCtypeName(ddl_JuniorSenior13Certificate1, "請選擇");
        }
        else
        {
            Panel29.Enabled = false;
        }
    }

    protected void chk_P32_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_P32.Checked)
        {
            Panel30.Enabled = true;
            Utility.setPlanName(ddl_JuniorSenior13CoursePlaningClass2, "請選擇");
            Utility.setCtypeName(ddl_JuniorSenior13Certificate2, "請選擇");
        }
        else
        {
            Panel30.Enabled = false;
        }
    }

    protected void ddl_JuniorSenior13CoursePlaningClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_JuniorSenior13CoursePlaningClass.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_JuniorSenior13.DataSource = objDT;
        CBL_JuniorSenior13.DataBind();
    }

    protected void ddl_JuniorSenior13CoursePlaningClass1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_JuniorSenior13CoursePlaningClass1.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_JuniorSenior13_1.DataSource = objDT;
        CBL_JuniorSenior13_1.DataBind();
    }

    protected void ddl_JuniorSenior13CoursePlaningClass2_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
           Select * from QS_Course where PClassSNO=@PClassSNO
        ";
        wDict.Add("PClassSNO", ddl_JuniorSenior13CoursePlaningClass2.SelectedValue);
        DataTable objDT = objDH.queryData(sql, wDict);
        CBL_JuniorSenior13_2.DataSource = objDT;
        CBL_JuniorSenior13_2.DataBind();
    }

    public string GetHeckList(CheckBoxList ChkList)
    {
        string oValues = "";
        foreach (ListItem oItem in ChkList.Items)
        {
            if (oItem.Selected == true)
            {
                if (oValues.Length > 0)
                {
                    oValues += ",";
                }
                oValues += oItem.Value;
            }

        }
        return oValues;
    }
    #endregion
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (Work.Value.Equals("NEW"))
        {
            string ERSNO = Convert.ToString(Request.QueryString["sno"]);
            Dictionary<string, object> wDict = new Dictionary<string, object>();
            #region 西醫師規則
            string None10Certificate = ddl_None10Certificate.SelectedValue;
            string None10CoursePlanning = ddl_None10CoursePlaningClass.SelectedValue;
            string None10CourseSNO = GetHeckList(CBL_None10Course);
            string None10Logic = ";";
            if (chk_None10P1.Checked)
            {
                None10Certificate += ";" + ddl_Certificate1.SelectedValue;
                None10CoursePlanning += ";" + ddl_None10CoursePlaningClass1.SelectedValue;
                None10CourseSNO += ";" + GetHeckList(CBL_Course1);
                None10Logic += ddl_Logic1.SelectedValue;
            }
            if (chk_None10P2.Checked)
            {
                None10Certificate += ";" + ddl_Certificate2.SelectedValue;
                None10CoursePlanning += ";" + ddl_None10CoursePlaningClass2.SelectedValue;
                None10CourseSNO += ";" + GetHeckList(CBL_Course2);
                None10Logic += ";" + ddl_Logic2.SelectedValue;
            }
            string Junior10Certificate = ddl_Junior10Certificate.SelectedValue;
            string Junior10CoursePlanning = ddl_Junior10CoursePlaningClass.SelectedValue;
            string Junior10CourseSNO = GetHeckList(CBL_Junior10);
            string Junior10Logic = ";";
            if (chk_JuniorP3.Checked)
            {
                Junior10Certificate += ";" + ddl_Junior10Certificate1.SelectedValue;
                Junior10CoursePlanning += ";" + ddl_Junior10CoursePlaningClass1.SelectedValue;
                Junior10CourseSNO += ";" + GetHeckList(CBL_Junior10_1);
                Junior10Logic += DropDownList3.SelectedValue;
            }
            if (chk_JuniorP4.Checked)
            {
                Junior10Certificate += ";" + ddl_Junior10Certificate2.SelectedValue;
                Junior10CoursePlanning += ";" + ddl_Junior10CoursePlaningClass2.SelectedValue;
                Junior10CourseSNO += ";" + GetHeckList(CBL_Junior10_2);
                Junior10Logic += ";" + DropDownList6.SelectedValue;
            }
            string Senior10Certificate = ddl_Senior10Certificate.SelectedValue;
            string Senior10CoursePlanning = ddl_Senior10CoursePlaningClass.SelectedValue;
            string Senior10CourseSNO = GetHeckList(CBL_Sunior10);
            string Senior10Logic = ";";
            if (chk_P5.Checked)
            {
                Senior10Certificate += ";" + ddl_Senior10Certificate1.SelectedValue;
                Senior10CoursePlanning+=";"+ ddl_Senior10CoursePlaningClass1.SelectedValue;
                Senior10CourseSNO += ";" + GetHeckList(CBL_Sunior10_1);
                Senior10Logic += DropDownList11.SelectedValue;
            }
            if (chk_P6.Checked)
            {
                Senior10Certificate += ";" + ddl_Senior10Certificate2.SelectedValue;
                Senior10CoursePlanning += ";" + ddl_Senior10CoursePlaningClass2.SelectedValue;
                Senior10CourseSNO += ";" + GetHeckList(CBL_Sunior10_2);
                Senior10Logic += ";" + DropDownList14.SelectedValue;
            }
            string JuniorSenior10Certificate = ddl_JuniorSenior10Certificate.SelectedValue;
            string JuniorSenior10CoursePlanning = ddl_JuniorSenior10CoursePlaningClass.SelectedValue;
            string JuniorSenior10CourseSNO = GetHeckList(CBL_JuniorSenior10);
            string JuniorSenior10Logic = ";";
            if (chk_P7.Checked)
            {
                JuniorSenior10Certificate += ";" + ddl_JuniorSenior10Certificate1.SelectedValue;
                JuniorSenior10CoursePlanning += ";" + ddl_JuniorSenior10CoursePlaningClass1.SelectedValue;
                JuniorSenior10CourseSNO += ";" + GetHeckList(CBL_JuniorSenior10_1);
                JuniorSenior10Logic += DropDownList19.SelectedValue;
            }
            if (chk_P8.Checked)
            {
                JuniorSenior10Certificate += ";" + ddl_JuniorSenior10Certificate2.SelectedValue;
                JuniorSenior10CoursePlanning += ";" + ddl_JuniorSenior10CoursePlaningClass2.SelectedValue;
                JuniorSenior10CourseSNO += ";" + GetHeckList(CBL_JuniorSenior10_2);
                JuniorSenior10Logic += ";" + DropDownList22.SelectedValue;
            }
            wDict.Add("ERSNO", ERSNO);
            wDict.Add("RoleSNO", "10");
            wDict.Add("CourseSNO", txt_CourseSNO_10.Text);
            wDict.Add("CourseSNOForJunior", txt_CourseSNO_10_1.Text);
            wDict.Add("PClassSNO", ddl_CoursePlan10.SelectedValue);
            wDict.Add("NoneRequireCertificate", None10Certificate);
            wDict.Add("NoneCoursePlanningSNO", None10CoursePlanning);
            wDict.Add("NoneRequireCourseSNO", None10CourseSNO);
            wDict.Add("NoneLogic", None10Logic);
            wDict.Add("JuniorRequireCertificate", Junior10Certificate);
            wDict.Add("JuniorCoursePlanningSNO", Junior10CoursePlanning);
            wDict.Add("JuniorRequireCourseSNO", Junior10CourseSNO);
            wDict.Add("JuniorLogic", Junior10Logic);
            wDict.Add("SeniorRequireCertificate", Senior10Certificate);
            wDict.Add("SeniorCoursePlanningSNO", Senior10CoursePlanning);
            wDict.Add("SeniorRequireCourseSNO", Senior10CourseSNO);
            wDict.Add("SeniorLogic", Senior10Logic);
            wDict.Add("JuniorSeniorRequireCertificate", JuniorSenior10Certificate);
            wDict.Add("JuniorSeniorCoursePlanningSNO", JuniorSenior10CoursePlanning);
            wDict.Add("JuniorSeniorRequireCourseSNO", JuniorSenior10CourseSNO);
            wDict.Add("JuniorSeniorLogic", JuniorSenior10Logic);
            wDict.Add("CreateDT", DateTime.Now);
            wDict.Add("CreateUserID", "0");
            if (txt_CourseSNO_10.Text != "")
            {
                UpdateNoneCondition(wDict);
            }
            wDict.Clear();
            #endregion
            #region 牙醫師規則
            string None11Certificate = ddl_None11Certificate.SelectedValue;
            string None11CoursePlanning = ddl_None11CoursePlaningClass.SelectedValue;
            string None11CourseSNO = GetHeckList(CBL_None11Course);
            string None11Logic = ";";
            if (chk_P9.Checked)
            {
                None11Certificate += ";" + ddl_None11Certificate1.SelectedValue;
                None11CoursePlanning+=";"+ ddl_None11CoursePlaningClass1.SelectedValue;
                None11CourseSNO += ";" + GetHeckList(CBL_None11Course1);
                None11Logic += DropDownList27.SelectedValue;
            }
            if (chk_P10.Checked)
            {
                None11Certificate += ";" + ddl_None11Certificate2.SelectedValue;
                None11CoursePlanning += ";" + ddl_None11CoursePlaningClass2.SelectedValue;
                None11CourseSNO += ";" + GetHeckList(CBL_None11Course2);
                None11Logic += ";" + DropDownList30.SelectedValue;
            }
            string Junior11Certificate = ddl_Junior11Certificate.SelectedValue;
            string Junior11CoursePlanning = ddl_Junior11CoursePlaningClass.SelectedValue;
            string Junior11CourseSNO = GetHeckList(CBL_Junior11);
            string Junior11Logic = ";";
            if (chk_P11.Checked)
            {
                Junior11Certificate += ";" + ddl_Junior11Certificate1.SelectedValue;
                Junior11CoursePlanning+=";"+ ddl_Junior11CoursePlaningClass1.SelectedValue;
                Junior11CourseSNO += ";" + GetHeckList(CBL_Junior11_1);
                Junior11Logic += DropDownList35.SelectedValue;
            }
            if (chk_P12.Checked)
            {
                Junior11Certificate += ";" + ddl_Junior11Certificate2.SelectedValue;
                Junior11CoursePlanning += ";" + ddl_Junior11CoursePlaningClass2.SelectedValue;
                Junior11CourseSNO += ";" + GetHeckList(CBL_Junior11_2);
                Junior11Logic += ";" + DropDownList38.SelectedValue;
            }
            string Senior11Certificate = ddl_Senior11Certificate.SelectedValue;
            string Senior11CoursePlanning = ddl_Senior11CoursePlaningClass.SelectedValue;
            string Senior11CourseSNO = GetHeckList(CBL_Sunior11);
            string Senior11Logic = ";";
            if (chk_P13.Checked)
            {
                Senior11Certificate += ";" + ddl_Senior11Certificate1.SelectedValue;
                Senior11CoursePlanning += ";" + ddl_Senior11CoursePlaningClass1.SelectedValue;
                Senior11CourseSNO += ";" + GetHeckList(CBL_Sunior11_1);
                Senior11Logic += DropDownList43.SelectedValue;
            }
            if (chk_P14.Checked)
            {
                Senior11Certificate += ";" + ddl_Senior11Certificate2.SelectedValue;
                Senior11CoursePlanning += ";" + ddl_Senior11CoursePlaningClass2.SelectedValue;
                Senior11CourseSNO += ";" + GetHeckList(CBL_Sunior11_2);
                Senior11Logic += ";" + DropDownList46.SelectedValue;
            }
            string JuniorSenior11Certificate = ddl_JuniorSenior11Certificate.SelectedValue;
            string JuniorSenior11CoursePlanning = ddl_JuniorSenior11CoursePlaningClass.SelectedValue;
            string JuniorSenior11CourseSNO = GetHeckList(CBL_JuniorSenior11);
            string JuniorSenior11Logic = ";";
            if (chk_P15.Checked)
            {
                JuniorSenior11Certificate += ";" + ddl_JuniorSenior11Certificate1.SelectedValue;
                JuniorSenior11CoursePlanning += ";" + ddl_JuniorSenior11CoursePlaningClass1.SelectedValue;
                JuniorSenior11CourseSNO += ";" + GetHeckList(CBL_JuniorSenior11_1);
                JuniorSenior11Logic += DropDownList51.SelectedValue;
            }
            if (chk_P16.Checked)
            {
                JuniorSenior11Certificate += ";" + ddl_JuniorSenior11Certificate2.SelectedValue;
                JuniorSenior11CoursePlanning += ";" + ddl_JuniorSenior11CoursePlaningClass2.SelectedValue;
                JuniorSenior11CourseSNO += ";" + GetHeckList(CBL_JuniorSenior11_2);
                JuniorSenior11Logic += ";" + DropDownList54.SelectedValue;
            }
            wDict.Add("ERSNO", ERSNO);
            wDict.Add("RoleSNO", "11");
            wDict.Add("CourseSNO", txt_CourseSNO_11.Text);
            wDict.Add("CourseSNOForJunior", txt_CourseSNO_11_1.Text);
            wDict.Add("PClassSNO", ddl_CoursePlan11.SelectedValue);
            wDict.Add("NoneRequireCertificate", None11Certificate);
            wDict.Add("NoneCoursePlanningSNO", None11CoursePlanning);
            wDict.Add("NoneRequireCourseSNO", None11CourseSNO);
            wDict.Add("NoneLogic", None11Logic);
            wDict.Add("JuniorRequireCertificate", Junior11Certificate);
            wDict.Add("JuniorCoursePlanningSNO", Junior11CoursePlanning);
            wDict.Add("JuniorRequireCourseSNO", Junior11CourseSNO);
            wDict.Add("JuniorLogic", Junior11Logic);
            wDict.Add("SeniorRequireCertificate", Senior11Certificate);
            wDict.Add("SeniorCoursePlanningSNO", Senior11CoursePlanning);
            wDict.Add("SeniorRequireCourseSNO", Senior11CourseSNO);
            wDict.Add("SeniorLogic", Senior11Logic);
            wDict.Add("JuniorSeniorRequireCertificate", JuniorSenior11Certificate);
            wDict.Add("JuniorSeniorCoursePlanningSNO", JuniorSenior11CoursePlanning);
            wDict.Add("JuniorSeniorRequireCourseSNO", JuniorSenior11CourseSNO);
            wDict.Add("JuniorSeniorLogic", JuniorSenior11Logic);
            wDict.Add("CreateDT", DateTime.Now);
            wDict.Add("CreateUserID", "0");
            if (txt_CourseSNO_11.Text != "")
            {
                UpdateNoneCondition(wDict);
            }

            wDict.Clear();
            #endregion
            #region 藥師規則
            string None12Certificate = ddl_None12Certificate.SelectedValue;
            string None12CoursePlanning = ddl_None12CoursePlaningClass.SelectedValue;
            string None12CourseSNO = GetHeckList(CBL_None12Course);
            string None12Logic = ";";
            if (chk_P17.Checked)
            {
                None12Certificate += ";" + ddl_None12Certificate1.SelectedValue;
                None12CoursePlanning += ";" + ddl_None12CoursePlaningClass1.SelectedValue;
                None12CourseSNO += ";" + GetHeckList(CBL_None12Course1);
                None12Logic += DropDownList59.SelectedValue;
            }
            if (chk_P18.Checked)
            {
                None12Certificate += ";" + ddl_None12Certificate2.SelectedValue;
                None12CoursePlanning += ";" + ddl_None12CoursePlaningClass2.SelectedValue;
                None12CourseSNO += ";" + GetHeckList(CBL_None12Course2);
                None12Logic += ";" + DropDownList62.SelectedValue;
            }
            string Junior12Certificate = ddl_Junior12Certificate.SelectedValue;
            string Junior12CoursePlanning = ddl_Junior12CoursePlaningClass.SelectedValue;
            string Junior12CourseSNO = GetHeckList(CBL_Junior12);
            string Junior12Logic = ";";
            if (chk_P19.Checked)
            {
                Junior12Certificate += ";" + ddl_Junior12Certificate1.SelectedValue;
                Junior12CoursePlanning += ";" + ddl_Junior12CoursePlaningClass1.SelectedValue;
                Junior12CourseSNO += ";" + GetHeckList(CBL_Junior12_1);
                Junior12Logic += DropDownList67.SelectedValue;
            }
            if (chk_P20.Checked)
            {
                Junior12Certificate += ";" + ddl_Junior12Certificate2.SelectedValue;
                Junior12CoursePlanning += ";" + ddl_Junior12CoursePlaningClass2.SelectedValue;
                Junior12CourseSNO += ";" + GetHeckList(CBL_Junior12_2);
                Junior12Logic += ";" + DropDownList70.SelectedValue;
            }
            string Senior12Certificate = ddl_Senior12Certificate.SelectedValue;
            string Senior12CoursePlanning = ddl_Senior12CoursePlaningClass.SelectedValue;
            string Senior12CourseSNO = GetHeckList(CBL_Sunior12);
            string Senior12Logic = ";";
            if (chk_P21.Checked)
            {
                Senior12Certificate += ";" + ddl_Senior12Certificate1.SelectedValue;
                Senior12CoursePlanning += ";" + ddl_Senior12CoursePlaningClass1.SelectedValue;
                Senior12CourseSNO += ";" + GetHeckList(CBL_Sunior12_1);
                Senior12Logic += DropDownList75.SelectedValue;
            }
            if (chk_P22.Checked)
            {
                Senior12Certificate += ";" + ddl_Senior12Certificate2.SelectedValue;
                Senior12CoursePlanning += ";" + ddl_Senior12CoursePlaningClass2.SelectedValue;
                Senior12CourseSNO += ";" + GetHeckList(CBL_Sunior12_2);
                Senior12Logic += ";" + DropDownList78.SelectedValue;
            }
            string JuniorSenior12Certificate = ddl_JuniorSenior12Certificate.SelectedValue;
            string JuniorSenior12CoursePlanning = ddl_JuniorSenior12CoursePlaningClass.SelectedValue;
            string JuniorSenior12CourseSNO = GetHeckList(CBL_JuniorSenior12);
            string JuniorSenior12Logic = ";";
            if (chk_P23.Checked)
            {
                JuniorSenior12Certificate += ";" + ddl_JuniorSenior12Certificate1.SelectedValue;
                JuniorSenior12CoursePlanning += ";" + ddl_JuniorSenior12CoursePlaningClass1.SelectedValue;
                JuniorSenior12CourseSNO += ";" + GetHeckList(CBL_JuniorSenior12_1);
                JuniorSenior12Logic += DropDownList83.SelectedValue;
            }
            if (chk_P24.Checked)
            {
                JuniorSenior12Certificate += ";" + ddl_JuniorSenior12Certificate2.SelectedValue;
                JuniorSenior12CoursePlanning += ";" + ddl_JuniorSenior12CoursePlaningClass2.SelectedValue;
                JuniorSenior12CourseSNO += ";" + GetHeckList(CBL_JuniorSenior12_2);
                JuniorSenior12Logic += ";" + DropDownList86.SelectedValue;
            }
            wDict.Add("ERSNO", ERSNO);
            wDict.Add("RoleSNO", "12");
            wDict.Add("CourseSNO", txt_CourseSNO_12.Text);
            wDict.Add("CourseSNOForJunior", txt_CourseSNO_12_1.Text);
            wDict.Add("PClassSNO", ddl_CoursePlan12.SelectedValue);
            wDict.Add("NoneRequireCertificate", None12Certificate);
            wDict.Add("NoneCoursePlanningSNO", None12CoursePlanning);
            wDict.Add("NoneRequireCourseSNO", None12CourseSNO);
            wDict.Add("NoneLogic", None12Logic);
            wDict.Add("JuniorRequireCertificate", Junior12Certificate);
            wDict.Add("JuniorCoursePlanningSNO", Junior12CoursePlanning);
            wDict.Add("JuniorRequireCourseSNO", Junior12CourseSNO);
            wDict.Add("JuniorLogic", Junior12Logic);
            wDict.Add("SeniorRequireCertificate", Senior12Certificate);
            wDict.Add("SeniorCoursePlanningSNO", Senior12CoursePlanning);
            wDict.Add("SeniorRequireCourseSNO", Senior12CourseSNO);
            wDict.Add("SeniorLogic", Senior12Logic);
            wDict.Add("JuniorSeniorRequireCertificate", JuniorSenior12Certificate);
            wDict.Add("JuniorSeniorCoursePlanningSNO", JuniorSenior12CoursePlanning);
            wDict.Add("JuniorSeniorRequireCourseSNO", JuniorSenior12CourseSNO);
            wDict.Add("JuniorSeniorLogic", JuniorSenior12Logic);
            wDict.Add("CreateDT", DateTime.Now);
            wDict.Add("CreateUserID", "0");
            if (txt_CourseSNO_12.Text != "")
            {
                UpdateNoneCondition(wDict);
            }

            wDict.Clear();
            #endregion
            #region 衛教師規則
            string None13Certificate = ddl_None13Certificate.SelectedValue;
            string None13CoursePlanning = ddl_None13CoursePlaningClass.SelectedValue;
            string None13CourseSNO = GetHeckList(CBL_None13Course);
            string None13Logic = ";";
            if (chk_P25.Checked)
            {
                None13Certificate += ";" + ddl_None13Certificate1.SelectedValue;
                None13CoursePlanning += ";" + ddl_None13CoursePlaningClass1.SelectedValue;
                None13CourseSNO += ";" + GetHeckList(CBL_None13Course1);
                None13Logic += DropDownList91.SelectedValue;
            }
            if (chk_P26.Checked)
            {
                None13Certificate += ";" + ddl_None13Certificate2.SelectedValue;
                None13CoursePlanning += ";" + ddl_None13CoursePlaningClass2.SelectedValue;
                None13CourseSNO += ";" + GetHeckList(CBL_None13Course2);
                None13Logic += ";" + DropDownList94.SelectedValue;
            }
            string Junior13Certificate = ddl_Junior13Certificate.SelectedValue;
            string Junior13CoursePlanning = ddl_Junior13CoursePlaningClass.SelectedValue;
            string Junior13CourseSNO = GetHeckList(CBL_Junior13);
            string Junior13Logic = ";";
            if (chk_P27.Checked)
            {
                Junior13Certificate += ";" + ddl_Junior13Certificate1.SelectedValue;
                Junior13CoursePlanning += ";" + ddl_Junior13CoursePlaningClass1.SelectedValue;
                Junior13CourseSNO += ";" + GetHeckList(CBL_Junior13_1);
                Junior13Logic += DropDownList99.SelectedValue;
            }
            if (chk_P28.Checked)
            {
                Junior13Certificate += ";" + ddl_Junior13Certificate2.SelectedValue;
                Junior13CoursePlanning += ";" + ddl_Junior13CoursePlaningClass2.SelectedValue;
                Junior13CourseSNO += ";" + GetHeckList(CBL_Junior13_2);
                Junior13Logic += ";" + DropDownList102.SelectedValue;
            }
            string Senior13Certificate = ddl_Senior13Certificate.SelectedValue;
            string Senior13CoursePlanning = ddl_Senior13CoursePlaningClass.SelectedValue;
            string Senior13CourseSNO = GetHeckList(CBL_Sunior13);
            string Senior13Logic = ";";
            if (chk_P29.Checked)
            {
                Senior13Certificate += ";" + ddl_Senior13Certificate1.SelectedValue;
                Senior13CoursePlanning += ";" + ddl_Senior13CoursePlaningClass1.SelectedValue;
                Senior13CourseSNO += ";" + GetHeckList(CBL_Sunior13_1);
                Senior13Logic += DropDownList107.SelectedValue;
            }
            if (chk_P30.Checked)
            {
                Senior13Certificate += ";" + ddl_Senior13Certificate2.SelectedValue;
                Senior13CoursePlanning += ";" + ddl_Senior13CoursePlaningClass2.SelectedValue;
                Senior13CourseSNO += ";" + GetHeckList(CBL_Sunior13_2);
                Senior13Logic += ";" + DropDownList110.SelectedValue;
            }
            string JuniorSenior13Certificate = ddl_JuniorSenior13Certificate.SelectedValue;
            string JuniorSenior13CoursePlanning = ddl_JuniorSenior13CoursePlaningClass.SelectedValue;
            string JuniorSenior13CourseSNO = GetHeckList(CBL_JuniorSenior13);
            string JuniorSenior13Logic = ";";
            if (chk_P31.Checked)
            {
                JuniorSenior13Certificate += ";" + ddl_JuniorSenior13Certificate1.SelectedValue;
                JuniorSenior13CoursePlanning += ";" + ddl_JuniorSenior13CoursePlaningClass1.SelectedValue;
                JuniorSenior13CourseSNO += ";" + GetHeckList(CBL_JuniorSenior13_1);
                JuniorSenior13Logic += DropDownList115.SelectedValue;
            }
            if (chk_P32.Checked)
            {
                JuniorSenior13Certificate += ";" + ddl_JuniorSenior13Certificate2.SelectedValue;
                JuniorSenior13CoursePlanning += ";" + ddl_JuniorSenior13CoursePlaningClass2.SelectedValue;
                JuniorSenior13CourseSNO += ";" + GetHeckList(CBL_JuniorSenior13_2);
                JuniorSenior13Logic += ";" + DropDownList118.SelectedValue;
            }
            wDict.Add("ERSNO", ERSNO);
            wDict.Add("RoleSNO", "13"); 
            wDict.Add("CourseSNO", txt_CourseSNO_13.Text);
            wDict.Add("CourseSNOForJunior", txt_CourseSNO_13_1.Text);
            wDict.Add("PClassSNO", ddl_CoursePlan13.SelectedValue);
            wDict.Add("NoneRequireCertificate", None13Certificate);
            wDict.Add("NoneCoursePlanningSNO", None13CoursePlanning);
            wDict.Add("NoneRequireCourseSNO", None13CourseSNO);
            wDict.Add("NoneLogic", None13Logic);
            wDict.Add("JuniorRequireCertificate", Junior13Certificate);
            wDict.Add("JuniorCoursePlanningSNO", Junior13CoursePlanning);
            wDict.Add("JuniorRequireCourseSNO", Junior13CourseSNO);
            wDict.Add("JuniorLogic", Junior13Logic);
            wDict.Add("SeniorRequireCertificate", Senior13Certificate);
            wDict.Add("SeniorCoursePlanningSNO", Senior13CoursePlanning);
            wDict.Add("SeniorRequireCourseSNO", Senior13CourseSNO);
            wDict.Add("SeniorLogic", Senior13Logic);
            wDict.Add("JuniorSeniorRequireCertificate", JuniorSenior13Certificate);
            wDict.Add("JuniorSeniorCoursePlanningSNO", JuniorSenior13CoursePlanning);
            wDict.Add("JuniorSeniorRequireCourseSNO", JuniorSenior13CourseSNO);
            wDict.Add("JuniorSeniorLogic", JuniorSenior13Logic);
            wDict.Add("CreateDT", DateTime.Now);
            wDict.Add("CreateUserID", "0");
            if (txt_CourseSNO_13.Text != "")
            {
                UpdateNoneCondition(wDict);
            }

            wDict.Clear();
            Response.Write("<script>alert('新增成功!');document.location.href='./EventRole.aspx'; </script>");
        }
        else
        {
            string ERSNO = Convert.ToString(Request.QueryString["sno"]);
            Dictionary<string, object> wDict = new Dictionary<string, object>();
            #region 西醫師規則
            string CourseSNO = txt_CourseSNO_10.Text;
            string None10Certificate = ddl_None10Certificate.SelectedValue;
            string None10CoursePlanning = ddl_None10CoursePlaningClass.SelectedValue;
            string None10CourseSNO = GetHeckList(CBL_None10Course);
            string None10Logic = ";";
            if (chk_None10P1.Checked)
            {
                None10Certificate += ";" + ddl_Certificate1.SelectedValue;
                None10CoursePlanning += ";" + ddl_None10CoursePlaningClass1.SelectedValue;
                None10CourseSNO += ";" + GetHeckList(CBL_Course1);
                None10Logic += ddl_Logic1.SelectedValue;
            }
            if (chk_None10P2.Checked)
            {
                None10Certificate += ";" + ddl_Certificate2.SelectedValue;
                None10CoursePlanning += ";" + ddl_None10CoursePlaningClass2.SelectedValue;
                None10CourseSNO += ";" + GetHeckList(CBL_Course2);
                None10Logic += ";" + ddl_Logic2.SelectedValue;
            }
            string Junior10Certificate = ddl_Junior10Certificate.SelectedValue;
            string Junior10CoursePlanning = ddl_Junior10CoursePlaningClass.SelectedValue;
            string Junior10CourseSNO = GetHeckList(CBL_Junior10);
            string Junior10Logic = ";";
            if (chk_JuniorP3.Checked)
            {
                Junior10Certificate += ";" + ddl_Junior10Certificate1.SelectedValue;
                Junior10CoursePlanning += ";" + ddl_Junior10CoursePlaningClass1.SelectedValue;
                Junior10CourseSNO += ";" + GetHeckList(CBL_Junior10_1);
                Junior10Logic += DropDownList3.SelectedValue;
            }
            if (chk_JuniorP4.Checked)
            {
                Junior10Certificate += ";" + ddl_Junior10Certificate2.SelectedValue;
                Junior10CoursePlanning += ";" + ddl_Junior10CoursePlaningClass2.SelectedValue;
                Junior10CourseSNO += ";" + GetHeckList(CBL_Junior10_2);
                Junior10Logic += ";" + DropDownList6.SelectedValue;
            }
            string Senior10Certificate = ddl_Senior10Certificate.SelectedValue;
            string Senior10CoursePlanning = ddl_Senior10CoursePlaningClass.SelectedValue;
            string Senior10CourseSNO = GetHeckList(CBL_Sunior10);
            string Senior10Logic = ";";
            if (chk_P5.Checked)
            {
                Senior10Certificate += ";" + ddl_Senior10Certificate1.SelectedValue;
                Senior10CoursePlanning += ";" + ddl_Senior10CoursePlaningClass1.SelectedValue;
                Senior10CourseSNO += ";" + GetHeckList(CBL_Sunior10_1);
                Senior10Logic += DropDownList11.SelectedValue;
            }
            if (chk_P6.Checked)
            {
                Senior10Certificate += ";" + ddl_Senior10Certificate2.SelectedValue;
                Senior10CoursePlanning += ";" + ddl_Senior10CoursePlaningClass2.SelectedValue;
                Senior10CourseSNO += ";" + GetHeckList(CBL_Sunior10_2);
                Senior10Logic += ";" + DropDownList14.SelectedValue;
            }
            string JuniorSenior10Certificate = ddl_JuniorSenior10Certificate.SelectedValue;
            string JuniorSenior10CoursePlanning = ddl_JuniorSenior10CoursePlaningClass.SelectedValue;
            string JuniorSenior10CourseSNO = GetHeckList(CBL_JuniorSenior10);
            string JuniorSenior10Logic = ";";
            if (chk_P7.Checked)
            {
                JuniorSenior10Certificate += ";" + ddl_JuniorSenior10Certificate1.SelectedValue;
                JuniorSenior10CoursePlanning += ";" + ddl_JuniorSenior10CoursePlaningClass1.SelectedValue;
                JuniorSenior10CourseSNO += ";" + GetHeckList(CBL_JuniorSenior10_1);
                JuniorSenior10Logic += DropDownList19.SelectedValue;
            }
            if (chk_P8.Checked)
            {
                JuniorSenior10Certificate += ";" + ddl_JuniorSenior10Certificate2.SelectedValue;
                JuniorSenior10CoursePlanning += ";" + ddl_JuniorSenior10CoursePlaningClass2.SelectedValue;
                JuniorSenior10CourseSNO += ";" + GetHeckList(CBL_JuniorSenior10_2);
                JuniorSenior10Logic += ";" + DropDownList22.SelectedValue;
            }
            wDict.Add("ERSNO", ERSNO);
            wDict.Add("RoleSNO", "10");
            wDict.Add("CourseSNO", txt_CourseSNO_10.Text);
            wDict.Add("CourseSNOForJunior", txt_CourseSNO_10_1.Text);
            wDict.Add("PClassSNO", ddl_CoursePlan10.SelectedValue);
            wDict.Add("NoneRequireCertificate", None10Certificate);
            wDict.Add("NoneCoursePlanningSNO", None10CoursePlanning);
            wDict.Add("NoneRequireCourseSNO", None10CourseSNO);
            wDict.Add("NoneLogic", None10Logic);
            wDict.Add("JuniorRequireCertificate", Junior10Certificate);
            wDict.Add("JuniorCoursePlanningSNO", Junior10CoursePlanning);
            wDict.Add("JuniorRequireCourseSNO", Junior10CourseSNO);
            wDict.Add("JuniorLogic", Junior10Logic);
            wDict.Add("SeniorRequireCertificate", Senior10Certificate);
            wDict.Add("SeniorCoursePlanningSNO", Senior10CoursePlanning);
            wDict.Add("SeniorRequireCourseSNO", Senior10CourseSNO);
            wDict.Add("SeniorLogic", Senior10Logic);
            wDict.Add("JuniorSeniorRequireCertificate", JuniorSenior10Certificate);
            wDict.Add("JuniorSeniorCoursePlanningSNO", JuniorSenior10CoursePlanning);
            wDict.Add("JuniorSeniorRequireCourseSNO", JuniorSenior10CourseSNO);
            wDict.Add("JuniorSeniorLogic", JuniorSenior10Logic);
            wDict.Add("CreateDT", DateTime.Now);
            wDict.Add("CreateUserID", "0");
            wDict.Add("ModifyUserID", "0");
            if (txt_CourseSNO_10.Text != "")
            {
                UpdateNoneCondition(wDict);
            }
            wDict.Clear();
            #endregion
            #region 牙醫師規則
            string None11Certificate = ddl_None11Certificate.SelectedValue;
            string None11CoursePlanning = ddl_None11CoursePlaningClass.SelectedValue;
            string None11CourseSNO = GetHeckList(CBL_None11Course);
            string None11Logic = ";";
            if (chk_P9.Checked)
            {
                None11Certificate += ";" + ddl_None11Certificate1.SelectedValue;
                None11CoursePlanning += ";" + ddl_None11CoursePlaningClass1.SelectedValue;
                None11CourseSNO += ";" + GetHeckList(CBL_None11Course1);
                None11Logic += DropDownList27.SelectedValue;
            }
            if (chk_P10.Checked)
            {
                None11Certificate += ";" + ddl_None11Certificate2.SelectedValue;
                None11CoursePlanning += ";" + ddl_None11CoursePlaningClass2.SelectedValue;
                None11CourseSNO += ";" + GetHeckList(CBL_None11Course2);
                None11Logic += ";" + DropDownList30.SelectedValue;
            }
            string Junior11Certificate = ddl_Junior11Certificate.SelectedValue;
            string Junior11CoursePlanning = ddl_Junior11CoursePlaningClass.SelectedValue;
            string Junior11CourseSNO = GetHeckList(CBL_Junior11);
            string Junior11Logic = ";";
            if (chk_P11.Checked)
            {
                Junior11Certificate += ";" + ddl_Junior11Certificate1.SelectedValue;
                Junior11CoursePlanning += ";" + ddl_Junior11CoursePlaningClass1.SelectedValue;
                Junior11CourseSNO += ";" + GetHeckList(CBL_Junior11_1);
                Junior11Logic += DropDownList35.SelectedValue;
            }
            if (chk_P12.Checked)
            {
                Junior11Certificate += ";" + ddl_Junior11Certificate2.SelectedValue;
                Junior11CoursePlanning += ";" + ddl_Junior11CoursePlaningClass2.SelectedValue;
                Junior11CourseSNO += ";" + GetHeckList(CBL_Junior11_2);
                Junior11Logic += ";" + DropDownList38.SelectedValue;
            }
            string Senior11Certificate = ddl_Senior11Certificate.SelectedValue;
            string Senior11CoursePlanning = ddl_Senior11CoursePlaningClass.SelectedValue;
            string Senior11CourseSNO = GetHeckList(CBL_Sunior11);
            string Senior11Logic = ";";
            if (chk_P13.Checked)
            {
                Senior11Certificate += ";" + ddl_Senior11Certificate1.SelectedValue;
                Senior11CoursePlanning += ";" + ddl_Senior11CoursePlaningClass1.SelectedValue;
                Senior11CourseSNO += ";" + GetHeckList(CBL_Sunior11_1);
                Senior11Logic += DropDownList43.SelectedValue;
            }
            if (chk_P14.Checked)
            {
                Senior11Certificate += ";" + ddl_Senior11Certificate2.SelectedValue;
                Senior11CoursePlanning += ";" + ddl_Senior11CoursePlaningClass2.SelectedValue;
                Senior11CourseSNO += ";" + GetHeckList(CBL_Sunior11_2);
                Senior11Logic += ";" + DropDownList46.SelectedValue;
            }
            string JuniorSenior11Certificate = ddl_JuniorSenior11Certificate.SelectedValue;
            string JuniorSenior11CoursePlanning = ddl_JuniorSenior11CoursePlaningClass.SelectedValue;
            string JuniorSenior11CourseSNO = GetHeckList(CBL_JuniorSenior11);
            string JuniorSenior11Logic = ";";
            if (chk_P15.Checked)
            {
                JuniorSenior11Certificate += ";" + ddl_JuniorSenior11Certificate1.SelectedValue;
                JuniorSenior11CoursePlanning += ";" + ddl_JuniorSenior11CoursePlaningClass1.SelectedValue;
                JuniorSenior11CourseSNO += ";" + GetHeckList(CBL_JuniorSenior11_1);
                JuniorSenior11Logic += DropDownList51.SelectedValue;
            }
            if (chk_P16.Checked)
            {
                JuniorSenior11Certificate += ";" + ddl_JuniorSenior11Certificate2.SelectedValue;
                JuniorSenior11CoursePlanning += ";" + ddl_JuniorSenior11CoursePlaningClass2.SelectedValue;
                JuniorSenior11CourseSNO += ";" + GetHeckList(CBL_JuniorSenior11_2);
                JuniorSenior11Logic += ";" + DropDownList54.SelectedValue;
            }
            wDict.Add("ERSNO", ERSNO);
            wDict.Add("RoleSNO", "11");
            wDict.Add("CourseSNO", txt_CourseSNO_11.Text);
            wDict.Add("CourseSNOForJunior", txt_CourseSNO_11_1.Text);
            wDict.Add("PClassSNO", ddl_CoursePlan11.SelectedValue);
            wDict.Add("NoneRequireCertificate", None11Certificate);
            wDict.Add("NoneCoursePlanningSNO", None11CoursePlanning);
            wDict.Add("NoneRequireCourseSNO", None11CourseSNO);
            wDict.Add("NoneLogic", None11Logic);
            wDict.Add("JuniorRequireCertificate", Junior11Certificate);
            wDict.Add("JuniorCoursePlanningSNO", Junior11CoursePlanning);
            wDict.Add("JuniorRequireCourseSNO", Junior11CourseSNO);
            wDict.Add("JuniorLogic", Junior11Logic);
            wDict.Add("SeniorRequireCertificate", Senior11Certificate);
            wDict.Add("SeniorCoursePlanningSNO", Senior11CoursePlanning);
            wDict.Add("SeniorRequireCourseSNO", Senior11CourseSNO);
            wDict.Add("SeniorLogic", Senior11Logic);
            wDict.Add("JuniorSeniorRequireCertificate", JuniorSenior11Certificate);
            wDict.Add("JuniorSeniorCoursePlanningSNO", JuniorSenior11CoursePlanning);
            wDict.Add("JuniorSeniorRequireCourseSNO", JuniorSenior11CourseSNO);
            wDict.Add("JuniorSeniorLogic", JuniorSenior11Logic);
            wDict.Add("CreateDT", DateTime.Now);
            wDict.Add("CreateUserID", "0");
            wDict.Add("ModifyUserID", "0");
            if (txt_CourseSNO_11.Text != "")
            {
                UpdateNoneCondition(wDict);
            }

            wDict.Clear();
            #endregion
            #region 藥師規則
            string None12Certificate = ddl_None12Certificate.SelectedValue;
            string None12CoursePlanning = ddl_None12CoursePlaningClass.SelectedValue;
            string None12CourseSNO = GetHeckList(CBL_None12Course);
            string None12Logic = ";";
            if (chk_P17.Checked)
            {
                None12Certificate += ";" + ddl_None12Certificate1.SelectedValue;
                None12CoursePlanning += ";" + ddl_None12CoursePlaningClass1.SelectedValue;
                None12CourseSNO += ";" + GetHeckList(CBL_None12Course1);
                None12Logic += DropDownList59.SelectedValue;
            }
            if (chk_P18.Checked)
            {
                None12Certificate += ";" + ddl_None12Certificate2.SelectedValue;
                None12CoursePlanning += ";" + ddl_None12CoursePlaningClass2.SelectedValue;
                None12CourseSNO += ";" + GetHeckList(CBL_None12Course2);
                None12Logic += ";" + DropDownList62.SelectedValue;
            }
            string Junior12Certificate = ddl_Junior12Certificate.SelectedValue;
            string Junior12CoursePlanning = ddl_Junior12CoursePlaningClass.SelectedValue;
            string Junior12CourseSNO = GetHeckList(CBL_Junior12);
            string Junior12Logic = ";";
            if (chk_P19.Checked)
            {
                Junior12Certificate += ";" + ddl_Junior12Certificate1.SelectedValue;
                Junior12CoursePlanning += ";" + ddl_Junior12CoursePlaningClass1.SelectedValue;
                Junior12CourseSNO += ";" + GetHeckList(CBL_Junior12_1);
                Junior12Logic += DropDownList67.SelectedValue;
            }
            if (chk_P20.Checked)
            {
                Junior12Certificate += ";" + ddl_Junior12Certificate2.SelectedValue;
                Junior12CoursePlanning += ";" + ddl_Junior12CoursePlaningClass2.SelectedValue;
                Junior12CourseSNO += ";" + GetHeckList(CBL_Junior12_2);
                Junior12Logic += ";" + DropDownList70.SelectedValue;
            }
            string Senior12Certificate = ddl_Senior12Certificate.SelectedValue;
            string Senior12CoursePlanning = ddl_Senior12CoursePlaningClass.SelectedValue;
            string Senior12CourseSNO = GetHeckList(CBL_Sunior12);
            string Senior12Logic = ";";
            if (chk_P21.Checked)
            {
                Senior12Certificate += ";" + ddl_Senior12Certificate1.SelectedValue;
                Senior12CoursePlanning += ";" + ddl_Senior12CoursePlaningClass1.SelectedValue;
                Senior12CourseSNO += ";" + GetHeckList(CBL_Sunior12_1);
                Senior12Logic += DropDownList75.SelectedValue;
            }
            if (chk_P22.Checked)
            {
                Senior12Certificate += ";" + ddl_Senior12Certificate2.SelectedValue;
                Senior12CoursePlanning += ";" + ddl_Senior12CoursePlaningClass2.SelectedValue;
                Senior12CourseSNO += ";" + GetHeckList(CBL_Sunior12_2);
                Senior12Logic += ";" + DropDownList78.SelectedValue;
            }
            string JuniorSenior12Certificate = ddl_JuniorSenior12Certificate.SelectedValue;
            string JuniorSenior12CoursePlanning = ddl_JuniorSenior12CoursePlaningClass.SelectedValue;
            string JuniorSenior12CourseSNO = GetHeckList(CBL_JuniorSenior12);
            string JuniorSenior12Logic = ";";
            if (chk_P23.Checked)
            {
                JuniorSenior12Certificate += ";" + ddl_JuniorSenior12Certificate1.SelectedValue;
                JuniorSenior12CoursePlanning += ";" + ddl_JuniorSenior12CoursePlaningClass1.SelectedValue;
                JuniorSenior12CourseSNO += ";" + GetHeckList(CBL_JuniorSenior12_1);
                JuniorSenior12Logic += DropDownList83.SelectedValue;
            }
            if (chk_P24.Checked)
            {
                JuniorSenior12Certificate += ";" + ddl_JuniorSenior12Certificate2.SelectedValue;
                JuniorSenior12CoursePlanning += ";" + ddl_JuniorSenior12CoursePlaningClass2.SelectedValue;
                JuniorSenior12CourseSNO += ";" + GetHeckList(CBL_JuniorSenior12_2);
                JuniorSenior12Logic += ";" + DropDownList86.SelectedValue;
            }
            wDict.Add("ERSNO", ERSNO);
            wDict.Add("RoleSNO", "12");
            wDict.Add("CourseSNO", txt_CourseSNO_12.Text);
            wDict.Add("CourseSNOForJunior", txt_CourseSNO_12_1.Text);
            wDict.Add("PClassSNO", ddl_CoursePlan12.SelectedValue);
            wDict.Add("NoneRequireCertificate", None12Certificate);
            wDict.Add("NoneCoursePlanningSNO", None12CoursePlanning);
            wDict.Add("NoneRequireCourseSNO", None12CourseSNO);
            wDict.Add("NoneLogic", None12Logic);
            wDict.Add("JuniorRequireCertificate", Junior12Certificate);
            wDict.Add("JuniorCoursePlanningSNO", Junior12CoursePlanning);
            wDict.Add("JuniorRequireCourseSNO", Junior12CourseSNO);
            wDict.Add("JuniorLogic", Junior12Logic);
            wDict.Add("SeniorRequireCertificate", Senior12Certificate);
            wDict.Add("SeniorCoursePlanningSNO", Senior12CoursePlanning);
            wDict.Add("SeniorRequireCourseSNO", Senior12CourseSNO);
            wDict.Add("SeniorLogic", Senior12Logic);
            wDict.Add("JuniorSeniorRequireCertificate", JuniorSenior12Certificate);
            wDict.Add("JuniorSeniorCoursePlanningSNO", JuniorSenior12CoursePlanning);
            wDict.Add("JuniorSeniorRequireCourseSNO", JuniorSenior12CourseSNO);
            wDict.Add("JuniorSeniorLogic", JuniorSenior12Logic);
            wDict.Add("CreateDT", DateTime.Now);
            wDict.Add("CreateUserID", "0");
            wDict.Add("ModifyUserID", "0");
            if (txt_CourseSNO_12.Text != "")
            {
                UpdateNoneCondition(wDict);
            }

            wDict.Clear();
            #endregion
            #region 衛教師規則
            string None13Certificate = ddl_None13Certificate.SelectedValue;
            string None13CoursePlanning = ddl_None13CoursePlaningClass.SelectedValue;
            string None13CourseSNO = GetHeckList(CBL_None13Course);
            string None13Logic = ";";
            if (chk_P25.Checked)
            {
                None13Certificate += ";" + ddl_None13Certificate1.SelectedValue;
                None13CoursePlanning += ";" + ddl_None13CoursePlaningClass1.SelectedValue;
                None13CourseSNO += ";" + GetHeckList(CBL_None13Course1);
                None13Logic += DropDownList91.SelectedValue;
            }
            if (chk_P26.Checked)
            {
                None13Certificate += ";" + ddl_None13Certificate2.SelectedValue;
                None13CoursePlanning += ";" + ddl_None13CoursePlaningClass2.SelectedValue;
                None13CourseSNO += ";" + GetHeckList(CBL_None13Course2);
                None13Logic += ";" + DropDownList94.SelectedValue;
            }
            string Junior13Certificate = ddl_Junior13Certificate.SelectedValue;
            string Junior13CoursePlanning = ddl_Junior13CoursePlaningClass.SelectedValue;
            string Junior13CourseSNO = GetHeckList(CBL_Junior13);
            string Junior13Logic = ";";
            if (chk_P27.Checked)
            {
                Junior13Certificate += ";" + ddl_Junior13Certificate1.SelectedValue;
                Junior13CoursePlanning += ";" + ddl_Junior13CoursePlaningClass1.SelectedValue;
                Junior13CourseSNO += ";" + GetHeckList(CBL_Junior13_1);
                Junior13Logic += DropDownList99.SelectedValue;
            }
            if (chk_P28.Checked)
            {
                Junior13Certificate += ";" + ddl_Junior13Certificate2.SelectedValue;
                Junior13CoursePlanning += ";" + ddl_Junior13CoursePlaningClass2.SelectedValue;
                Junior13CourseSNO += ";" + GetHeckList(CBL_Junior13_2);
                Junior13Logic += ";" + DropDownList102.SelectedValue;
            }
            string Senior13Certificate = ddl_Senior13Certificate.SelectedValue;
            string Senior13CoursePlanning = ddl_Senior13CoursePlaningClass.SelectedValue;
            string Senior13CourseSNO = GetHeckList(CBL_Sunior13);
            string Senior13Logic = ";";
            if (chk_P29.Checked)
            {
                Senior13Certificate += ";" + ddl_Senior13Certificate1.SelectedValue;
                Senior13CoursePlanning += ";" + ddl_Senior13CoursePlaningClass1.SelectedValue;
                Senior13CourseSNO += ";" + GetHeckList(CBL_Sunior13_1);
                Senior13Logic += DropDownList107.SelectedValue;
            }
            if (chk_P30.Checked)
            {
                Senior13Certificate += ";" + ddl_Senior13Certificate2.SelectedValue;
                Senior13CoursePlanning += ";" + ddl_Senior13CoursePlaningClass2.SelectedValue;
                Senior13CourseSNO += ";" + GetHeckList(CBL_Sunior13_2);
                Senior13Logic += ";" + DropDownList110.SelectedValue;
            }
            string JuniorSenior13Certificate = ddl_JuniorSenior13Certificate.SelectedValue;
            string JuniorSenior13CoursePlanning = ddl_JuniorSenior13CoursePlaningClass.SelectedValue;
            string JuniorSenior13CourseSNO = GetHeckList(CBL_JuniorSenior13);
            string JuniorSenior13Logic = ";";
            if (chk_P31.Checked)
            {
                JuniorSenior13Certificate += ";" + ddl_JuniorSenior13Certificate1.SelectedValue;
                JuniorSenior13CoursePlanning += ";" + ddl_JuniorSenior13CoursePlaningClass1.SelectedValue;
                JuniorSenior13CourseSNO += ";" + GetHeckList(CBL_JuniorSenior13_1);
                JuniorSenior13Logic += DropDownList115.SelectedValue;
            }
            if (chk_P32.Checked)
            {
                JuniorSenior13Certificate += ";" + ddl_JuniorSenior13Certificate2.SelectedValue;
                JuniorSenior13CoursePlanning += ";" + ddl_JuniorSenior13CoursePlaningClass2.SelectedValue;
                JuniorSenior13CourseSNO += ";" + GetHeckList(CBL_JuniorSenior13_2);
                JuniorSenior13Logic += ";" + DropDownList118.SelectedValue;
            }
            wDict.Add("ERSNO", ERSNO);
            wDict.Add("RoleSNO", "13");
            wDict.Add("CourseSNO", txt_CourseSNO_13.Text);
            wDict.Add("CourseSNOForJunior", txt_CourseSNO_13_1.Text);
            wDict.Add("PClassSNO", ddl_CoursePlan13.SelectedValue);
            wDict.Add("NoneRequireCertificate", None13Certificate);
            wDict.Add("NoneCoursePlanningSNO", None13CoursePlanning);
            wDict.Add("NoneRequireCourseSNO", None13CourseSNO);
            wDict.Add("NoneLogic", None13Logic);
            wDict.Add("JuniorRequireCertificate", Junior13Certificate);
            wDict.Add("JuniorCoursePlanningSNO", Junior13CoursePlanning);
            wDict.Add("JuniorRequireCourseSNO", Junior13CourseSNO);
            wDict.Add("JuniorLogic", Junior13Logic);
            wDict.Add("SeniorRequireCertificate", Senior13Certificate);
            wDict.Add("SeniorCoursePlanningSNO", Senior13CoursePlanning);
            wDict.Add("SeniorRequireCourseSNO", Senior13CourseSNO);
            wDict.Add("SeniorLogic", Senior13Logic);
            wDict.Add("JuniorSeniorRequireCertificate", JuniorSenior13Certificate);
            wDict.Add("JuniorSeniorCoursePlanningSNO", JuniorSenior13CoursePlanning);
            wDict.Add("JuniorSeniorRequireCourseSNO", JuniorSenior13CourseSNO);
            wDict.Add("JuniorSeniorLogic", JuniorSenior13Logic);
            wDict.Add("CreateDT", DateTime.Now);
            wDict.Add("CreateUserID", "0");
            wDict.Add("ModifyUserID", "0");
            if (txt_CourseSNO_13.Text!="")
            {
                UpdateNoneCondition(wDict);
            }

            wDict.Clear();
            Response.Write("<script>alert('更新成功!');document.location.href='./EventRole.aspx'; </script>");
        }
        #endregion
    }


    public void InsertNoneCondition(Dictionary<string, object> aDict)
    {
        DataHelper objDH = new DataHelper();
       
        string SQL = @"INSERT INTO [dbo].[EventRoleDetail]
                (ERSNO,[RoleSNO],CourseSNO,CourseSNOForJunior,PClassSNO
                ,[NoneRequireCertificate]
                ,NoneCoursePlanningSNO
                ,[NoneRequireCourseSNO]
                ,[NoneLogic]
                ,[JuniorRequireCertificate]
                ,JuniorCoursePlanningSNO
                ,[JuniorRequireCourseSNO]
                ,[JuniorLogic]
                ,[SeniorRequireCertificate]
                ,SeniorCoursePlanningSNO
                ,[SeniorRequireCourseSNO]
                ,[SeniorLogic]
                ,[JuniorSeniorRequireCertificate]
                ,JuniorSeniorCoursePlanningSNO
                ,[JuniorSeniorRequireCourseSNO]
                ,[JuniorSeniorLogic]
                ,[CreateDT]
                ,[CreateUserID])
          VALUES
                (@ERSNO,@RoleSNO,@CourseSNO,@CourseSNOForJunior,@PClassSNO
                ,@NoneRequireCertificate
                ,@NoneCoursePlanningSNO
                ,@NoneRequireCourseSNO
                ,@NoneLogic
                ,@JuniorRequireCertificate
                ,@JuniorCoursePlanningSNO
                ,@JuniorRequireCourseSNO
                ,@JuniorLogic
                ,@SeniorRequireCertificate
                ,@SeniorCoursePlanningSNO
                ,@SeniorRequireCourseSNO
                ,@SeniorLogic
                ,@JuniorSeniorRequireCertificate
                ,@JuniorSeniorCoursePlanningSNO
                ,@JuniorSeniorRequireCourseSNO
                ,@JuniorSeniorLogic
                ,@CreateDT
                ,@CreateUserID)";
        objDH.executeNonQuery(SQL, aDict);
    }
    public void UpdateNoneCondition(Dictionary<string, object> aDict)
    {
        DataHelper objDH = new DataHelper();
        
        string CheckInsertOrUpdateSQL = @"Select * from EventRoleDetail where RoleSNO=@RoleSNO and ERSNO=@ERSNO";
        DataTable ObjDT = objDH.queryData(CheckInsertOrUpdateSQL, aDict);
        //if (ObjDT.Rows.Count > 0)
        //{
            string SQL = @"
            If Exists(select 1 From EventRoleDetail Where RoleSNO=@RoleSNO and ERSNO=@ERSNO)
                           BEGIN
                    UPDATE [dbo].[EventRoleDetail]
                          SET CourseSNO=@CourseSNO,CourseSNOForJunior=@CourseSNOForJunior,PClassSNO=@PClassSNO
                             ,[NoneRequireCertificate] = @NoneRequireCertificate
                             ,[NoneCoursePlanningSNO] = @NoneCoursePlanningSNO
                             ,[NoneRequireCourseSNO] = @NoneRequireCourseSNO
                             ,[NoneLogic] = @NoneLogic
                             ,[JuniorRequireCertificate] = @JuniorRequireCertificate
                             ,[JuniorCoursePlanningSNO] = @JuniorCoursePlanningSNO
                             ,[JuniorRequireCourseSNO] = @JuniorRequireCourseSNO
                             ,[JuniorLogic] = @JuniorLogic
                             ,[SeniorRequireCertificate] = @SeniorRequireCertificate
                             ,[SeniorCoursePlanningSNO] = @SeniorCoursePlanningSNO
                             ,[SeniorRequireCourseSNO] = @SeniorRequireCourseSNO
                             ,[SeniorLogic] = @SeniorLogic
                             ,[JuniorSeniorRequireCertificate] = @JuniorSeniorRequireCertificate
                             ,[JuniorSeniorCoursePlanningSNO] = @JuniorSeniorCoursePlanningSNO
                             ,[JuniorSeniorRequireCourseSNO] = @JuniorSeniorRequireCourseSNO
                             ,[JuniorSeniorLogic] = @JuniorSeniorLogic
                             ,[ModifyDT] = getdate()
                             ,[ModifyUserID] = @ModifyUserID
                        WHERE [ERSNO] = @ERSNO And [RoleSNO] = @RoleSNO
                        End
                         Else
                            INSERT INTO [dbo].[EventRoleDetail]
                (ERSNO,[RoleSNO],CourseSNO
                ,[NoneRequireCertificate]
                ,NoneCoursePlanningSNO
                ,[NoneRequireCourseSNO]
                ,[NoneLogic]
                ,[JuniorRequireCertificate]
                ,JuniorCoursePlanningSNO
                ,[JuniorRequireCourseSNO]
                ,[JuniorLogic]
                ,[SeniorRequireCertificate]
                ,SeniorCoursePlanningSNO
                ,[SeniorRequireCourseSNO]
                ,[SeniorLogic]
                ,[JuniorSeniorRequireCertificate]
                ,JuniorSeniorCoursePlanningSNO
                ,[JuniorSeniorRequireCourseSNO]
                ,[JuniorSeniorLogic]
                ,[CreateDT]
                ,[CreateUserID])
          VALUES
                (@ERSNO,@RoleSNO,@CourseSNO
                ,@NoneRequireCertificate
                ,@NoneCoursePlanningSNO
                ,@NoneRequireCourseSNO
                ,@NoneLogic
                ,@JuniorRequireCertificate
                ,@JuniorCoursePlanningSNO
                ,@JuniorRequireCourseSNO
                ,@JuniorLogic
                ,@SeniorRequireCertificate
                ,@SeniorCoursePlanningSNO
                ,@SeniorRequireCourseSNO
                ,@SeniorLogic
                ,@JuniorSeniorRequireCertificate
                ,@JuniorSeniorCoursePlanningSNO
                ,@JuniorSeniorRequireCourseSNO
                ,@JuniorSeniorLogic
                ,@CreateDT
                ,@CreateUserID)

";
            objDH.executeNonQuery(SQL, aDict);
      
    }
    #endregion

    protected void ddl_CoursePlan_SelectedIndexChanged(object sender, EventArgs e)
    {
        //SetCourse(ddl_Course10, ddl_CoursePlan10.SelectedValue, "請選擇");
    }

    protected void ddl_CoursePlan11_SelectedIndexChanged(object sender, EventArgs e)
    {
        //SetCourse(ddl_Course11, ddl_CoursePlan11.SelectedValue, "請選擇");
    }

    protected void ddl_CoursePlan12_SelectedIndexChanged(object sender, EventArgs e)
    {
        //SetCourse(ddl_Course12, ddl_CoursePlan12.SelectedValue, "請選擇");
    }

    protected void ddl_CoursePlan13_SelectedIndexChanged(object sender, EventArgs e)
    {
        //SetCourse(ddl_Course13, ddl_CoursePlan13.SelectedValue, "請選擇");
    }

    protected void btn_LookFor10Code_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "buttonStartupBySM", "window.open('SearchCode.aspx?code="+ddl_CoursePlan10.SelectedValue+"', '新視窗的名稱', config='height=400,width=600');", true);
    }

    protected void btn_LookFor11Code_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "buttonStartupBySM", "window.open(' SearchCode.aspx?code=" + ddl_CoursePlan11.SelectedValue + " ', '新視窗的名稱', config='height=400,width=600');", true);
    }

    protected void btn_LookFor12Code_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "buttonStartupBySM", "window.open(' SearchCode.aspx?code=" + ddl_CoursePlan12.SelectedValue + " ', '新視窗的名稱', config='height=400,width=600');", true);
    }

    protected void btn_LookFor13Code_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "buttonStartupBySM", "window.open(' SearchCode.aspx?code=" + ddl_CoursePlan13.SelectedValue + " ', '新視窗的名稱', config='height=400,width=600');", true);
    }
}

