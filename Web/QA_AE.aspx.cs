using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_QA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {          
            getData();
        }
    }
    protected void getData()
    {
        
        String QASNO = Convert.ToString(Request.QueryString["sno"]);
        bool CheckFor = EventRole.CheckIntegerType(QASNO);
        if (CheckFor == true)
        {
            Dictionary<string, object> aDict = new Dictionary<string, object>();
            aDict.Add("sno", QASNO);
            DataHelper objDH = new DataHelper();
            DataTable objDT = objDH.queryData(@"
            SELECT C.QACSNO, Name, Q.CreateDT, Q.Title, Q.Info ,S.SYSTEM_NAME
            from QAClass C 
                LEFT JOIN QA Q ON C.QACSNO = Q.QACSNO 
                LEFT JOIN SYSTEM S ON Q.SYSTEM_ID = S.SYSTEM_ID
            Where QASNO=@sno", aDict);
            rpt_QA.DataSource = objDT.DefaultView;
            rpt_QA.DataBind();
        }
        else
        {
            Response.Write("<Script>alert('錯誤參數');document.location.href='QA.aspx';</Script>");
            return;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        getData();
    }
}