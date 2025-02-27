using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Event_NameList : System.Web.UI.Page
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
            bindData();
        }
    }


    protected void bindData()
    {

      
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        String id = Convert.ToString(Request.QueryString["sno"]);
        aDict.Add("EventSNO", id);
               

        //取報名資料
        DataTable objDT = objDH.queryData(@"
            
                SELECT 
                    ROW_NUMBER() OVER (ORDER BY e.CreateDT ) as ROW_NO, 
	                p.PName, p.PMail, p.PTel, p.PPhone, e.Notice,
	                r.RoleName, Convert(varchar(16), e.CreateDT, 120) ApplyDT,
                    c1.MVal 'EventAudit', 
	                c2.MVal 'EventNotice'
                From EventD e
	                Left Join Person p On p.PersonSNO = e.PersonSNO
	                Left Join Role r On r.RoleSNO = p.RoleSNO
	                Left Join Config c1 On c1.PVal = e.Audit And c1.PGroup='EventAudit'
	                Left Join Config c2 On c2.PVal = e.NoticeType And c2.PGroup='EventNotice'
                Where EventSNO = @EventSNO
            
        ", aDict);

        gv_EventD.DataSource = objDT.DefaultView;
        gv_EventD.DataBind();

    }
}