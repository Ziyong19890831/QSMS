using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_intelligentRole : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            getdate();
        }
    }
    public void getdate()
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        for (int i = 10; i <= 13; i++)
        {
            for (int j = 0; j <= 2; j++)
            {
                adict.Add("ICertificateType", j);
                adict.Add("RoleSNO", i);
                string sql = @"Select * from [IntelligentRole] where ICertificateType=@ICertificateType and RoleSNO=@RoleSNO";
                DataTable ObJDT = ObjDH.queryData(sql, adict);
                adict.Clear();
                if(i==10 && j == 0)
                {
                    txt_ICT0_10_N.Text = ObJDT.Rows[0]["N_PClassSNO"].ToString();
                    txt_ICT0_10_J.Text = ObJDT.Rows[0]["J_PClassSNO"].ToString();
                    txt_ICT0_10_S.Text = ObJDT.Rows[0]["S_PClassSNO"].ToString();
                    txt_ICT0_10_SJ.Text= ObJDT.Rows[0]["JS_PClassSNO"].ToString();
                    txt_ICT0_10_T.Text = ObJDT.Rows[0]["Q_TreadPClassSNO"].ToString();
                    txt_ICT0_10_G.Text = ObJDT.Rows[0]["Q_GuardianPClassSNO"].ToString();
                    txt_ICT0_10_SD.Text= ObJDT.Rows[0]["SeedPClassSNO"].ToString();

                }
                else if(i==10 && j == 1)
                {
                    txt_ICT1_10_N.Text = ObJDT.Rows[0]["N_PClassSNO"].ToString();
                    txt_ICT1_10_J.Text = ObJDT.Rows[0]["J_PClassSNO"].ToString();
                    txt_ICT1_10_S.Text = ObJDT.Rows[0]["S_PClassSNO"].ToString();
                    txt_ICT1_10_SJ.Text= ObJDT.Rows[0]["JS_PClassSNO"].ToString();
                    txt_ICT1_10_T.Text = ObJDT.Rows[0]["Q_TreadPClassSNO"].ToString();
                    txt_ICT1_10_G.Text = ObJDT.Rows[0]["Q_GuardianPClassSNO"].ToString();
                    txt_ICT1_10_SD.Text= ObJDT.Rows[0]["SeedPClassSNO"].ToString();
                    txt_ICT1_10_11.Text= ObJDT.Rows[0]["Role10ToGet11Guardian"].ToString();
                    txt_ICT1_10_13.Text= ObJDT.Rows[0]["Role10ToGet13Guardian"].ToString();
                }
                else if(i==10 && j == 2)
                {
                    txt_ICT2_10_N.Text = ObJDT.Rows[0]["N_PClassSNO"].ToString();
                    txt_ICT2_10_J.Text = ObJDT.Rows[0]["J_PClassSNO"].ToString();
                    txt_ICT2_10_S.Text = ObJDT.Rows[0]["S_PClassSNO"].ToString();
                    txt_ICT2_10_SJ.Text = ObJDT.Rows[0]["JS_PClassSNO"].ToString();
                    txt_ICT2_10_T.Text = ObJDT.Rows[0]["Q_TreadPClassSNO"].ToString();
                    txt_ICT2_10_G.Text = ObJDT.Rows[0]["Q_GuardianPClassSNO"].ToString();
                    txt_ICT2_10_SD.Text = ObJDT.Rows[0]["SeedPClassSNO"].ToString();
                }
                else if(i == 11 && j == 0)
                {
                    txt_ICT0_11_N.Text = ObJDT.Rows[0]["N_PClassSNO"].ToString();
                    txt_ICT0_11_J.Text = ObJDT.Rows[0]["J_PClassSNO"].ToString();
                    txt_ICT0_11_S.Text = ObJDT.Rows[0]["S_PClassSNO"].ToString();
                    txt_ICT0_11_SJ.Text= ObJDT.Rows[0]["JS_PClassSNO"].ToString();
                    txt_ICT0_11_T.Text = ObJDT.Rows[0]["Q_TreadPClassSNO"].ToString();
                    txt_ICT0_11_G.Text = ObJDT.Rows[0]["Q_GuardianPClassSNO"].ToString();
                    txt_ICT0_11_SD.Text= ObJDT.Rows[0]["SeedPClassSNO"].ToString();
                }
                else if (i == 11 && j == 1)
                {
                    txt_ICT1_11_N.Text = ObJDT.Rows[0]["N_PClassSNO"].ToString();
                    txt_ICT1_11_J.Text = ObJDT.Rows[0]["J_PClassSNO"].ToString();
                    txt_ICT1_11_S.Text = ObJDT.Rows[0]["S_PClassSNO"].ToString();
                    txt_ICT1_11_SJ.Text= ObJDT.Rows[0]["JS_PClassSNO"].ToString();
                    txt_ICT1_11_T.Text = ObJDT.Rows[0]["Q_TreadPClassSNO"].ToString();
                    txt_ICT1_11_G.Text = ObJDT.Rows[0]["Q_GuardianPClassSNO"].ToString();
                    txt_ICT1_11_SD.Text= ObJDT.Rows[0]["SeedPClassSNO"].ToString();
                }
                else if (i == 11 && j == 2)
                {
                    txt_ICT2_11_N.Text = ObJDT.Rows[0]["N_PClassSNO"].ToString();
                    txt_ICT2_11_J.Text = ObJDT.Rows[0]["J_PClassSNO"].ToString();
                    txt_ICT2_11_S.Text = ObJDT.Rows[0]["S_PClassSNO"].ToString();
                    txt_ICT2_11_SJ.Text= ObJDT.Rows[0]["JS_PClassSNO"].ToString();
                    txt_ICT2_11_T.Text = ObJDT.Rows[0]["Q_TreadPClassSNO"].ToString();
                    txt_ICT2_11_G.Text = ObJDT.Rows[0]["Q_GuardianPClassSNO"].ToString();
                    txt_ICT2_11_SD.Text= ObJDT.Rows[0]["SeedPClassSNO"].ToString();
                }
                else if (i == 12 && j == 0)
                {
                    txt_ICT0_12_N.Text = ObJDT.Rows[0]["N_PClassSNO"].ToString();
                    txt_ICT0_12_J.Text = ObJDT.Rows[0]["J_PClassSNO"].ToString();
                    txt_ICT0_12_S.Text = ObJDT.Rows[0]["S_PClassSNO"].ToString();
                    txt_ICT0_12_SJ.Text= ObJDT.Rows[0]["JS_PClassSNO"].ToString();
                    txt_ICT0_12_T.Text = ObJDT.Rows[0]["Q_TreadPClassSNO"].ToString();
                    txt_ICT0_12_G.Text = ObJDT.Rows[0]["Q_GuardianPClassSNO"].ToString();
                    txt_ICT0_12_SD.Text= ObJDT.Rows[0]["SeedPClassSNO"].ToString();
                }
                else if (i == 12 && j == 1)
                {
                    txt_ICT1_12_N.Text = ObJDT.Rows[0]["N_PClassSNO"].ToString();
                    txt_ICT1_12_J.Text = ObJDT.Rows[0]["J_PClassSNO"].ToString();
                    txt_ICT1_12_S.Text = ObJDT.Rows[0]["S_PClassSNO"].ToString();
                    txt_ICT1_12_SJ.Text =ObJDT.Rows[0]["JS_PClassSNO"].ToString();
                    txt_ICT1_12_T.Text = ObJDT.Rows[0]["Q_TreadPClassSNO"].ToString();
                    txt_ICT1_12_G.Text = ObJDT.Rows[0]["Q_GuardianPClassSNO"].ToString();
                    txt_ICT1_12_SD.Text = ObJDT.Rows[0]["SeedPClassSNO"].ToString();
                }
                else if (i == 12 && j == 2)
                {
                    txt_ICT2_12_N.Text = ObJDT.Rows[0]["N_PClassSNO"].ToString();
                    txt_ICT2_12_J.Text = ObJDT.Rows[0]["J_PClassSNO"].ToString();
                    txt_ICT2_12_S.Text = ObJDT.Rows[0]["S_PClassSNO"].ToString();
                    txt_ICT2_12_SJ.Text =ObJDT.Rows[0]["JS_PClassSNO"].ToString();
                    txt_ICT2_12_T.Text = ObJDT.Rows[0]["Q_TreadPClassSNO"].ToString();
                    txt_ICT2_12_G.Text = ObJDT.Rows[0]["Q_GuardianPClassSNO"].ToString();
                    txt_ICT2_12_SD.Text = ObJDT.Rows[0]["SeedPClassSNO"].ToString();
                }
                else if (i == 13 && j == 0)
                {
                    txt_ICT0_13_N.Text = ObJDT.Rows[0]["N_PClassSNO"].ToString();
                    txt_ICT0_13_J.Text = ObJDT.Rows[0]["J_PClassSNO"].ToString();
                    txt_ICT0_13_S.Text = ObJDT.Rows[0]["S_PClassSNO"].ToString();
                    txt_ICT0_13_SJ.Text =ObJDT.Rows[0]["JS_PClassSNO"].ToString();
                    txt_ICT0_13_T.Text = ObJDT.Rows[0]["Q_TreadPClassSNO"].ToString();
                    txt_ICT0_13_G.Text = ObJDT.Rows[0]["Q_GuardianPClassSNO"].ToString();
                    txt_ICT0_13_SD.Text = ObJDT.Rows[0]["SeedPClassSNO"].ToString();
                }
                else if (i == 13 && j == 1)
                {
                    txt_ICT1_13_N.Text = ObJDT.Rows[0]["N_PClassSNO"].ToString();
                    txt_ICT1_13_J.Text = ObJDT.Rows[0]["J_PClassSNO"].ToString();
                    txt_ICT1_13_S.Text = ObJDT.Rows[0]["S_PClassSNO"].ToString();
                    txt_ICT1_13_SJ.Text =ObJDT.Rows[0]["JS_PClassSNO"].ToString();
                    txt_ICT1_13_T.Text = ObJDT.Rows[0]["Q_TreadPClassSNO"].ToString();
                    txt_ICT1_13_G.Text = ObJDT.Rows[0]["Q_GuardianPClassSNO"].ToString();
                    txt_ICT1_13_SD.Text = ObJDT.Rows[0]["SeedPClassSNO"].ToString();
                }
                else if (i == 13 && j == 2)
                {
                    txt_ICT2_13_N.Text = ObJDT.Rows[0]["N_PClassSNO"].ToString();
                    txt_ICT2_13_J.Text = ObJDT.Rows[0]["J_PClassSNO"].ToString();
                    txt_ICT2_13_S.Text = ObJDT.Rows[0]["S_PClassSNO"].ToString();
                    txt_ICT2_13_SJ.Text =ObJDT.Rows[0]["JS_PClassSNO"].ToString();
                    txt_ICT2_13_T.Text = ObJDT.Rows[0]["Q_TreadPClassSNO"].ToString();
                    txt_ICT2_13_G.Text = ObJDT.Rows[0]["Q_GuardianPClassSNO"].ToString();
                    txt_ICT2_13_SD.Text = ObJDT.Rows[0]["SeedPClassSNO"].ToString();
                }
            }
        }


    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        DataHelper ObjDH = new DataHelper();
        Dictionary<string, object> adict = new Dictionary<string, object>();
        for (int i = 10; i <= 13; i++)
        {
            adict.Clear();
            for (int j = 0; j <= 2; j++)
            {
                adict.Add("ICertificateType", j);
                adict.Add("RoleSNO", i);
                string SQL = @"Update IntelligentRole set N_PClassSNO=@N_PClassSNO,J_PClassSNO=@J_PClassSNO,
                              S_PClassSNO=@S_PClassSNO,JS_PClassSNO=@JS_PClassSNO,Q_TreadPClassSNO=@Q_TreadPClassSNO,
                              Q_GuardianPClassSNO=@Q_GuardianPClassSNO,SeedPClassSNO=@SeedPClassSNO,Role10ToGet11Guardian=@Role10ToGet11Guardian,
                                Role10ToGet13Guardian=@Role10ToGet13Guardian 
                                where ICertificateType=@ICertificateType and RoleSNO=@RoleSNO";
               
                if (i == 10 && j == 0)
                {
                    adict.Add("N_PClassSNO",        txt_ICT0_10_N.Text );
                    adict.Add("J_PClassSNO",        txt_ICT0_10_J.Text );
                    adict.Add("S_PClassSNO",        txt_ICT0_10_S.Text );
                    adict.Add("JS_PClassSNO",       txt_ICT0_10_SJ.Text);
                    adict.Add("Q_TreadPClassSNO",   txt_ICT0_10_T.Text );
                    adict.Add("Q_GuardianPClassSNO",txt_ICT0_10_G.Text );
                    adict.Add("SeedPClassSNO",      txt_ICT0_10_SD.Text);
                    adict.Add("Role10ToGet11Guardian", "");
                    adict.Add("Role10ToGet13Guardian", "");
                    ObjDH.executeNonQuery(SQL, adict);
                    adict.Clear();
                }
                else if (i == 10 && j == 1)
                {
                    adict.Add("N_PClassSNO", txt_ICT1_10_N.Text);
                    adict.Add("J_PClassSNO", txt_ICT1_10_J.Text);
                    adict.Add("S_PClassSNO", txt_ICT1_10_S.Text);
                    adict.Add("JS_PClassSNO", txt_ICT1_10_SJ.Text);
                    adict.Add("Q_TreadPClassSNO", txt_ICT1_10_T.Text);
                    adict.Add("Q_GuardianPClassSNO", txt_ICT1_10_G.Text);
                    adict.Add("SeedPClassSNO", txt_ICT1_10_SD.Text);
                    adict.Add("Role10ToGet11Guardian", txt_ICT1_10_11.Text);
                    adict.Add("Role10ToGet13Guardian", txt_ICT1_10_13.Text);
                    ObjDH.executeNonQuery(SQL, adict);
                    adict.Clear();
                }
                else if (i == 10 && j == 2)
                {
                    adict.Add("N_PClassSNO", txt_ICT2_10_N.Text);
                    adict.Add("J_PClassSNO", txt_ICT2_10_J.Text);
                    adict.Add("S_PClassSNO", txt_ICT2_10_S.Text);
                    adict.Add("JS_PClassSNO", txt_ICT2_10_SJ.Text);
                    adict.Add("Q_TreadPClassSNO", txt_ICT2_10_T.Text);
                    adict.Add("Q_GuardianPClassSNO", txt_ICT2_10_G.Text);
                    adict.Add("SeedPClassSNO", txt_ICT2_10_SD.Text);
                    adict.Add("Role10ToGet11Guardian", "");
                    adict.Add("Role10ToGet13Guardian", "");
                    ObjDH.executeNonQuery(SQL, adict);
                    adict.Clear();
                }
                else if (i == 11 && j == 0)
                {
                    adict.Add("N_PClassSNO", txt_ICT0_11_N.Text);
                    adict.Add("J_PClassSNO", txt_ICT0_11_J.Text);
                    adict.Add("S_PClassSNO", txt_ICT0_11_S.Text);
                    adict.Add("JS_PClassSNO", txt_ICT0_11_SJ.Text);
                    adict.Add("Q_TreadPClassSNO", txt_ICT0_11_T.Text);
                    adict.Add("Q_GuardianPClassSNO", txt_ICT0_11_G.Text);
                    adict.Add("SeedPClassSNO", txt_ICT0_11_SD.Text);
                    adict.Add("Role10ToGet11Guardian", "");
                    adict.Add("Role10ToGet13Guardian", "");
                    ObjDH.executeNonQuery(SQL, adict);
                    adict.Clear();
                }
                else if (i == 11 && j == 1)
                {
                    adict.Add("N_PClassSNO", txt_ICT1_11_N.Text);
                    adict.Add("J_PClassSNO", txt_ICT1_11_J.Text);
                    adict.Add("S_PClassSNO", txt_ICT1_11_S.Text);
                    adict.Add("JS_PClassSNO", txt_ICT1_11_SJ.Text);
                    adict.Add("Q_TreadPClassSNO", txt_ICT1_11_T.Text);
                    adict.Add("Q_GuardianPClassSNO", txt_ICT1_11_G.Text);
                    adict.Add("SeedPClassSNO", txt_ICT1_11_SD.Text);
                    adict.Add("Role10ToGet11Guardian", "");
                    adict.Add("Role10ToGet13Guardian", "");
                    ObjDH.executeNonQuery(SQL, adict);
                    adict.Clear();
                }
                else if (i == 11 && j == 2)
                {
                    adict.Add("N_PClassSNO", txt_ICT2_11_N.Text);
                    adict.Add("J_PClassSNO", txt_ICT2_11_J.Text);
                    adict.Add("S_PClassSNO", txt_ICT2_11_S.Text);
                    adict.Add("JS_PClassSNO", txt_ICT2_11_SJ.Text);
                    adict.Add("Q_TreadPClassSNO", txt_ICT2_11_T.Text);
                    adict.Add("Q_GuardianPClassSNO", txt_ICT2_11_G.Text);
                    adict.Add("SeedPClassSNO", txt_ICT2_11_SD.Text);
                    adict.Add("Role10ToGet11Guardian", "");
                    adict.Add("Role10ToGet13Guardian", "");
                    ObjDH.executeNonQuery(SQL, adict);
                    adict.Clear();
                }
                else if (i == 12 && j == 0)
                {
                    adict.Add("N_PClassSNO", txt_ICT0_12_N.Text);
                    adict.Add("J_PClassSNO", txt_ICT0_12_J.Text);
                    adict.Add("S_PClassSNO", txt_ICT0_12_S.Text);
                    adict.Add("JS_PClassSNO", txt_ICT0_12_SJ.Text);
                    adict.Add("Q_TreadPClassSNO", txt_ICT0_12_T.Text);
                    adict.Add("Q_GuardianPClassSNO", txt_ICT0_12_G.Text);
                    adict.Add("SeedPClassSNO", txt_ICT0_12_SD.Text);
                    adict.Add("Role10ToGet11Guardian", "");
                    adict.Add("Role10ToGet13Guardian", "");
                    ObjDH.executeNonQuery(SQL, adict);
                    adict.Clear();
                }
                else if (i == 12 && j == 1)
                {
                    adict.Add("N_PClassSNO", txt_ICT1_12_N.Text);
                    adict.Add("J_PClassSNO", txt_ICT1_12_J.Text);
                    adict.Add("S_PClassSNO", txt_ICT1_12_S.Text);
                    adict.Add("JS_PClassSNO", txt_ICT1_12_SJ.Text);
                    adict.Add("Q_TreadPClassSNO", txt_ICT1_12_T.Text);
                    adict.Add("Q_GuardianPClassSNO", txt_ICT1_12_G.Text);
                    adict.Add("SeedPClassSNO", txt_ICT1_12_SD.Text);
                    adict.Add("Role10ToGet11Guardian", "");
                    adict.Add("Role10ToGet13Guardian", "");
                    ObjDH.executeNonQuery(SQL, adict);
                    adict.Clear();
                }
                else if (i == 12 && j == 2)
                {
                    adict.Add("N_PClassSNO", txt_ICT2_12_N.Text);
                    adict.Add("J_PClassSNO", txt_ICT2_12_J.Text);
                    adict.Add("S_PClassSNO", txt_ICT2_12_S.Text);
                    adict.Add("JS_PClassSNO", txt_ICT2_12_SJ.Text);
                    adict.Add("Q_TreadPClassSNO", txt_ICT2_12_T.Text);
                    adict.Add("Q_GuardianPClassSNO", txt_ICT2_12_G.Text);
                    adict.Add("SeedPClassSNO", txt_ICT2_12_SD.Text);
                    adict.Add("Role10ToGet11Guardian", "");
                    adict.Add("Role10ToGet13Guardian", "");
                    ObjDH.executeNonQuery(SQL, adict);
                    adict.Clear();
                }
                else if (i == 13 && j == 0)
                {
                    adict.Add("N_PClassSNO", txt_ICT0_13_N.Text);
                    adict.Add("J_PClassSNO", txt_ICT0_13_J.Text);
                    adict.Add("S_PClassSNO", txt_ICT0_13_S.Text);
                    adict.Add("JS_PClassSNO", txt_ICT0_13_SJ.Text);
                    adict.Add("Q_TreadPClassSNO", txt_ICT0_13_T.Text);
                    adict.Add("Q_GuardianPClassSNO", txt_ICT0_13_G.Text);
                    adict.Add("SeedPClassSNO", txt_ICT0_13_SD.Text);
                    adict.Add("Role10ToGet11Guardian", "");
                    adict.Add("Role10ToGet13Guardian", "");
                    ObjDH.executeNonQuery(SQL, adict);
                    adict.Clear();
                }
                else if (i == 13 && j == 1)
                {
                    adict.Add("N_PClassSNO", txt_ICT1_13_N.Text);
                    adict.Add("J_PClassSNO", txt_ICT1_13_J.Text);
                    adict.Add("S_PClassSNO", txt_ICT1_13_S.Text);
                    adict.Add("JS_PClassSNO", txt_ICT1_13_SJ.Text);
                    adict.Add("Q_TreadPClassSNO", txt_ICT1_13_T.Text);
                    adict.Add("Q_GuardianPClassSNO", txt_ICT1_13_G.Text);
                    adict.Add("SeedPClassSNO", txt_ICT1_13_SD.Text);
                    adict.Add("Role10ToGet11Guardian", "");
                    adict.Add("Role10ToGet13Guardian", "");
                    ObjDH.executeNonQuery(SQL, adict);
                    adict.Clear();
                }
                else if (i == 13 && j == 2)
                {
                    adict.Add("N_PClassSNO", txt_ICT2_13_N.Text);
                    adict.Add("J_PClassSNO", txt_ICT2_13_J.Text);
                    adict.Add("S_PClassSNO", txt_ICT2_13_S.Text);
                    adict.Add("JS_PClassSNO", txt_ICT2_13_SJ.Text);
                    adict.Add("Q_TreadPClassSNO", txt_ICT2_13_T.Text);
                    adict.Add("Q_GuardianPClassSNO", txt_ICT2_13_G.Text);
                    adict.Add("SeedPClassSNO", txt_ICT2_13_SD.Text);
                    adict.Add("Role10ToGet11Guardian", "");
                    adict.Add("Role10ToGet13Guardian", "");
                    ObjDH.executeNonQuery(SQL, adict);
                    adict.Clear();
                }
            }
        }
    }

    protected void btn_SearchCode_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "buttonStartupBySM", "window.open(' SearchCode.aspx?PClassSNO=999', '新視窗的名稱', config='height=400,width=600');", true);
    }

    protected void btn_FirstPart_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "buttonStartupBySM", "window.open('CourseOnline_IntelEdit.aspx', '新視窗的名稱', config='height=400,width=1000px');", true);
    }
}