using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Notice_Preview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Bindata();
    }
    protected void Bindata()
    {
        String id = Convert.ToString(Request.QueryString["sno"]);
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("sno", id);
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(@"
            SELECT C.Name, N.NoticeSNO, N.SDate, N.Title, N.Info, S.SYSTEM_NAME
            From Notice N 
                LEFT JOIN NoticeClass C ON C.NoticeCSNO = N.NoticeCSNO 
                LEFT JOIN SYSTEM S ON N.SYSTEM_ID = S.SYSTEM_ID
            Where NoticeSNO=@sno", aDict);
        lb_Name.Text = "分類：" + objDT.Rows[0]["Name"].ToString();
        lb_SDate.Text = "發布日期：" + Convert.ToDateTime(objDT.Rows[0]["SDate"]).ToString("yyyy-MM-dd");
        lb_Title.Text = "標題：" + objDT.Rows[0]["Title"].ToString();
        lb_Info.Text = getMark(HttpUtility.HtmlDecode(objDT.Rows[0]["Info"].ToString()));
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
}