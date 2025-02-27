using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security.AntiXss;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_APSAuditCheck : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {

            DataHelper objDH = new DataHelper();
            string qid = Request.Form["qid"].ToString();

            String sqls = @"
                SELECT PD.*,PS.*,ST.SYSTEM_NAME,OG.OrganName
                FROM PersonD PD
                    LEFT JOIN Person PS ON PS.PersonID = PD.PersonID
                    LEFT JOIN SYSTEM ST ON ST.SYSTEM_ID = PD.SYSTEM_ID
                    LEFT JOIN Organ  OG ON OG.OrganSNO = PS.OrganSNO
                WHERE PersonDSNO = @PersonDSNO
            ";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("PersonDSNO", qid);

            DataTable dt = objDH.queryData(sqls, dic);
            lb_ApplyDate.Text = AntiXssEncoder.HtmlEncode(dt.Rows[0]["CreateDT"].ToString(),true);
            lb_ApplyName.Text = AntiXssEncoder.HtmlEncode(dt.Rows[0]["PName"].ToString(),true);
            lb_ApplyAccount.Text = AntiXssEncoder.HtmlEncode(dt.Rows[0]["PAccount"].ToString(),true);
            lb_ApplyMail.Text = AntiXssEncoder.HtmlEncode(dt.Rows[0]["PMail"].ToString(),true);
            lb_ApplyPhone.Text = AntiXssEncoder.HtmlEncode(dt.Rows[0]["PPhone"].ToString(),true);
            lb_ApplyTel.Text = AntiXssEncoder.HtmlEncode(dt.Rows[0]["PTel"].ToString(),true);
            lb_ApplySys.Text = AntiXssEncoder.HtmlEncode(dt.Rows[0]["SYSTEM_NAME"].ToString(),true);
            lb_ApplyOrgan.Text = AntiXssEncoder.HtmlEncode(dt.Rows[0]["OrganName"].ToString(),true);
            lb_ApplyStatus.Text = AntiXssEncoder.HtmlEncode(dt.Rows[0]["SysPAccountIsUser"].ToString(),true);
            AuditStatus.Value = AntiXssEncoder.HtmlEncode(dt.Rows[0]["SysPAccountIsUser"].ToString(),true);

            dic.Clear();



            String sqlOUR = @"
                    SELECT
                       SF.SFID AS ukey,
                       ROW_NUMBER() OVER (ORDER BY SF.SFINDEX) AS fun_index,
                       SF.FUN_FID AS fun_fid,
                       SF.FUN_ID AS fun_id,
                       SF.SFNAME AS fun_name,
                       SF.SFTYPE AS fun_type
                    FROM SYSFrame SF
                    WHERE
                        SF.SYSTEM = @SYSTEM AND 
                  ( SF.SFTYPE = 'A' AND SF.FUN_ID IN (
                   SELECT DISTINCT SRM.SPLALIAS FROM SYSOrganRole SOR 
                                INNER JOIN SYSRoleMenu SRM ON SOR.SYSTEM = SRM.SYSTEM AND SOR.SRID = SRM.SRID AND SRM.ISVIEW = '1'
                   WHERE SOR.SYSTEM = @SYSTEM AND SOR.OrganSNO = @OrganSNO AND SOR.ISVIEW = '1')
                        )
                    ORDER BY SF.SFINDEX
            ";

            dic.Add("SYSTEM", dt.Rows[0]["SYSTEM_ID"].ToString());
            dic.Add("OrganSNO", dt.Rows[0]["OrganSNO"].ToString());

            DataTable dtT = objDH.queryData(sqlOUR, dic);

            if (dtT.Rows.Count != 0)
            {
                CheckBoxList1.DataSource = dtT;
                CheckBoxList1.DataTextField = dtT.Columns["fun_name"].ToString();
                CheckBoxList1.DataValueField = dtT.Columns["fun_id"].ToString();
                CheckBoxList1.DataBind();
                Panel1.Visible = true;
                Check_chkcategory();

            }
            else
            {
                Panel1.Visible = false;
            }



        }
    }

    protected void Check_chkcategory()
    {
        DataHelper objDH = new DataHelper();
        string qid = Request.Form["qid"].ToString();
        String sqlOUR = @"SELECT SPLALIAS FROM SYSOUR WHERE PersonDSNO = @PersonDSNO";
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("PersonDSNO", qid);

        DataTable dtT = objDH.queryData(sqlOUR, dic);

        if (dtT.Rows.Count >= 1)
        {
            for (int i = 0; i <= dtT.Rows.Count - 1; i++)
            {

                string text = dtT.Rows[i]["SPLALIAS"].ToString();
                ListItem crItem = null;
                crItem = CheckBoxList1.Items.FindByValue(text);
                if (crItem != null)
                {
                    crItem.Selected = true;
                }

            }
        }




    }


}