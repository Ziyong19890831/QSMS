using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_CustomerService : System.Web.UI.Page
{
    public UserInfo userInfo = null;
    int viewrole = 1;
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得Session資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
      
    }
    protected void bindData(int page)
    {
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        String sql = @"
             Select * from Person where 1=1
        ";
        if (!String.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " And  PersonID=@PersonID ";
            wDict.Add("PersonID", txt_PersonID.Text.Trim());
        }
      
       
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        gv_Account.DataSource = objDT.DefaultView;
        gv_Account.DataBind();

    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        bindData(1);
    }
}