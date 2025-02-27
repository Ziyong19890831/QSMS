using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_RexTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            getArea_A();
        }
    }
    protected void getArea_A()
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY AREA_CODE) as ROW_NO, AREA_CODE, AREA_NAME FROM CD_AREA WHERE AREA_TYPE='A'", null);
        DropDownList1.DataSource = objDT;
        DropDownList1.DataBind();
        DropDownList1.Items.Insert(0, new ListItem("請選擇", ""));
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList2.Items.Clear();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        String AREA_CODE_A = DropDownList1.SelectedValue;
        if (!String.IsNullOrEmpty(AREA_CODE_A))
        {
            aDict.Add("AREA_CODE", AREA_CODE_A);
            DataHelper objDH = new DataHelper();
            DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER (ORDER BY AREA_CODE) as ROW_NO, AREA_CODE, AREA_NAME FROM CD_AREA WHERE AREA_TYPE='B' AND AREA_CODE LIKE @AREA_CODE + '%'", aDict);
            DropDownList2.DataSource = objDT;
            DropDownList2.DataBind();
            DropDownList2.Items.Insert(0, new ListItem("請選擇", ""));
        }
        else
        {
            DropDownList2.Items.Add(new ListItem("請先選擇縣市行政區", ""));
        }
    }
}