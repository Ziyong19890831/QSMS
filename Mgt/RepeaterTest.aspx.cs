using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_RepeaterTest : System.Web.UI.Page
{
    public DataTable objDB = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        GetLink();
    }

    private void GetLink()
    {
        String Account = "AA11";
        DataHelper objDH = new DataHelper();
         objDB = objDH.queryData(@"SELECT * FROM PageLink P
                                            INNER JOIN ROLEMENU M ON M.PLINKSNO=P.PLINKSNO AND RoleID=2 AND ISVIEW=1
                                            Where ISENABLE=1
                                            ", null);

    objDB.DefaultView.RowFilter = "PPLINKSNO IS NULL";

        DataTable aDTable = objDB.DefaultView.ToTable();
        rpt_dir.DataSource = aDTable;
        rpt_dir.DataBind();
    }

    protected void rpt_Link_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var FoundRepeater = e.Item.FindControl("rpt_link") as Repeater;
            if (FoundRepeater != null)
            {
                SubLinkByCategory(FoundRepeater, DataBinder.Eval(e.Item.DataItem, "GROUPORDER").ToString());
            }
        }
    }

    protected void SubLinkByCategory(Repeater theRepeater, string param)
    {
        objDB.DefaultView.RowFilter =String.Format("GROUPORDER ='{0}'and PPLINKSNO IS NOT NULL ", param);
        DataTable aDTable = objDB.DefaultView.ToTable();
        theRepeater.DataSource = aDTable;
        theRepeater.DataBind();
    }
}