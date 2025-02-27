using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Event_Preview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    protected void Bind()
    {
        string EventSNO = Request.QueryString["Sno"] != null ? Request.QueryString["Sno"] : "";

        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        string SQL = "Select * from Event where EventSNO=@EventSNO";
        aDict.Add("EventSNO", EventSNO);
        DataTable ObjDT = objDH.queryData(SQL, aDict);
        if (ObjDT.Rows.Count > 0)
        {
            rpt_Event.DataSource = ObjDT.DefaultView;
            rpt_Event.DataBind();
        }
    }
}