using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_EventDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bindData(1);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    }
    protected void bindData(int page)
    {
        if (page < 1) page = 1;
        int pageRecord = 20;
        if (page < 1) page = 1;
        String sql = "";
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        String id = Convert.ToString(Request.QueryString["sno"]);
        aDict.Add("sno", id);

        //取活動名稱
        sql = "select * from Event where EventSNO=@sno";
        DataTable EventName = objDH.queryData(sql, aDict);
        lbl_EventName.Text = "目前活動:"+Convert.ToString(EventName.Rows[0]["EventName"]);

        //取報名資料
        sql =@"
            select ROW_NUMBER() OVER (ORDER BY ED.CreateDT DESC ) as ROW_NO, ED.* from EventD ED Where EventSNO=@sno
        ";
        DataTable objDT = objDH.queryData(sql, aDict);

        if (objDT.Rows.Count>0)
        {
            int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
            if (page > maxPageNumber) page = maxPageNumber;
            objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
            gv_EventD.DataSource = objDT.DefaultView;
            gv_EventD.DataBind();
            ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
        }
        else
        {
            ltl_PageNumber.Text = "<div class='center'>尚未有人報名</div>";
        }

    }
}