using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_AccountRe : System.Web.UI.Page
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
           
            //bindData(1);
        }
    }
    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;

        String sql = @"
              SELECT ROW_NUMBER() OVER (ORDER BY P.PersonSNO ) as ROW_NO,STUFF ( P.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption'
            ,P.PName,P.LoginError,P.PersonID,R.RoleName,P.[LoginErrorTime],DATEDIFF ( MINUTE , [LoginErrorTime] , Getdate() ) Retime
            from Person P
            Left Join Role R ON R.RoleSNO=P.RoleSNO
            LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO 
			where LoginError >= 3 and DATEDIFF ( MINUTE , [LoginErrorTime] , Getdate() ) < 30
        ";
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        #region 權限篩選區塊
        Utility.setSQLAccess_ByRoleOrganType(aDict, userInfo);
        #endregion


        #region 查詢篩選區塊

        if (!String.IsNullOrEmpty(txt_searchID.Text))
        {
            sql += "And P.PersonID  =  @PersonID ";
            aDict.Add("PersonID", txt_searchID.Text);
        }

        #endregion

        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, aDict);
        
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_AccountRe.DataSource = objDT.DefaultView;
        gv_AccountRe.DataBind();
        ltl_PageNumber.Text = Utility.showPageNumber(objDT.Rows.Count, page, pageRecord);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData(1);
    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        bindData(page);
    }


    protected void gv_AccountRe_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataHelper objDH = new DataHelper();
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        Button myButton = (Button)e.CommandSource;
        GridViewRow myRow = (GridViewRow)myButton.NamingContainer;
        string PersonID = myRow.Cells[7].Text;
        aDict.Add("PersonID", PersonID);
        objDH.executeNonQuery("Update Person set LoginError=0 where PersonID=@PersonID", aDict);
        Response.Redirect(Request.Url.ToString());
    }

    protected void gv_AccountRe_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }

    protected void gv_AccountRe_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}