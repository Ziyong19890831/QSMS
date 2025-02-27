using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_PersonDataLog : System.Web.UI.Page
{
    UserInfo userInfo = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {            
           
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void bindData(int page)
    {
        if (page < 1) page = 1;
        int pageRecord = 10;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();
        String sql = @"
            Select ROW_NUMBER() OVER (ORDER BY PDL.CreateDT DESC) ROW_NO,PDL.*,P.PName,
				 STUFF ( P.PersonID , 4 , 3 , 'OOO' ) as 'PersonID_encryption',PL.PName SName
                from PersonDataLog PDL
                Left Join Person P On P.PersonSNO=PDL.PersonSNO
                Left Join Person PL On PL.PersonSNO=PDL.CreateUserID
                where 1=1              
        ";
        if (!string.IsNullOrEmpty(txt_PersonID.Text))
        {
            sql += " And P.PersonID=@PersonID ";
            aDict.Add("PersonID", txt_PersonID.Text);
        }
        if (!string.IsNullOrEmpty(txt_Person.Text))
        {
            sql += " And P.PName like '% @PName %'";
            aDict.Add("PName", txt_Person.Text);
        }
        if (!string.IsNullOrEmpty(txt_SDate.Text))
        {
            sql += " And PDL.CreateDT >= @SDate";
            aDict.Add("SDate", txt_SDate.Text);
        }
        if (!string.IsNullOrEmpty(txt_EDate.Text))
        {
            sql += " And PDL.CreateDT <= @SDate";
            aDict.Add("SDate", txt_EDate.Text);
        }
        aDict.Add("PersonSNO", userInfo.PersonSNO);
        DataTable objDT = objDH.queryData(sql, aDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_PersonDataLog.DataSource = objDT.DefaultView;
        gv_PersonDataLog.DataBind();
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

}