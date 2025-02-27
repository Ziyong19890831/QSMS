using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Blank : System.Web.UI.Page
{
    UserInfo userInfo = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
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
        String sql = @"
        SELECT TOP 10  ROW_NUMBER() OVER (ORDER BY -OrderSeq DESC, SDate DESC ) ROW_NO, S.SYSTEM_NAME , NoticeSNO, Title, SDate, EDate, N.CreateDT, OrderSeq, C.Name as ClassName
        from Notice N
        LEFT JOIN NoticeClass C on N.NoticeCSNO=C.NoticeCSNO
        LEFT JOIN SYSTEM S on N.SYSTEM_ID=S.SYSTEM_ID ";
        
        DataTable objDT = objDH.queryData(sql, aDict);
        rpt_Notice.DataSource = objDT.DefaultView;
        rpt_Notice.DataBind();


    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData();
    }


}