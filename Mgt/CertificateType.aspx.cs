using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_CertificateType : System.Web.UI.Page
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
            bindData(1);
        }
    }

    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        String sql = @"
   SELECT m.CTypeSNO,m.ROW_NO,m.CTypeSNO,m.CTypeName,m.CTypeFile,m.CTypeString,m.CTypeSEQ,m.Note,m.RoleName,left(m.productIDs,len(m.productIDs)-1) as RoleBindName from  (SELECT ROW_NUMBER() OVER (ORDER BY CTypeSNO) as ROW_NO,
                CTypeSNO,CTypeName,CTypeFile,CTypeString,CTypeSEQ,Note,R.RoleName,(select cast (Role.RoleName AS NVARCHAR ) + ',' from [RoleBind] left join Role on RoleBind.RoleSNO=Role.RoleSNO
				where CSNO=ct.CTypeSNO and [RoleBind].TypeKey='certificatetype' FOR XML PATH('')) as productIDs
            FROM QS_CertificateType ct
                Left Join Role R On R.RoleSNO=ct.RoleSNO
            WHERE 1=1)M　
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();


        #region 權限篩選區塊
        switch (userInfo.RoleOrganType)
        {
            case "S":
                break;
            case "U":
                wDict.Add("RoleSNO", userInfo.RoleSNO);
                sql += " And ct.RoleSNO=@RoleSNO";
                break;
            default:
                sql += " And 1=2";
                break;
        }
        #endregion


        if (!string.IsNullOrEmpty(txt_CertificateType.Text))
        {
            sql += " AND CTypeName Like '%' + @CTypeName + '%' ";
            wDict.Add("CTypeName", txt_CertificateType.Text.Trim());
        }

        sql += " Order by ROW_NO";
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData(sql, wDict);
        int maxPageNumber = (objDT.Rows.Count - 1) / pageRecord + 1;
        if (page > maxPageNumber) page = maxPageNumber;
        objDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
        gv_Course.DataSource = objDT.DefaultView;
        gv_Course.DataBind();
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

    protected void btnDEL_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        String id = btn.CommandArgument;
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        aDict.Add("CTypeSNO", id);
        DataHelper objDH = new DataHelper();
        DataTable finddata = objDH.queryData("Select 1 From QS_CoursePlanningClass Where CTypeSNO=@CTypeSNO", aDict);
        if (finddata.Rows.Count > 0)
        {
            Utility.showMessage(Page, "注意！", "很抱歉，該[證書類別]已繫結[課程規劃類別]，請先至[課程規劃類別]取消繫結後再刪除。");
            return;
        }
        else
        {
            objDH.executeNonQuery("Delete QS_CertificateType Where CTypeSNO=@CTypeSNO", aDict);
            Utility.showMessage(Page, "訊息", "刪除成功。");
            btnPage_Click(sender, e);
            return;
        }

    }


}