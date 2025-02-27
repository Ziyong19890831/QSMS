using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dialog_Mgt : System.Web.UI.MasterPage
{
    UserInfo userInfo = null;
    public String errorMessage = "";

    protected void Page_Init(object sender, EventArgs e)
    {

        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
        if (userInfo == null) Response.Redirect("../Default.aspx");


    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (userInfo != null)
            {
                

            }
        }


    }

    
    
}
