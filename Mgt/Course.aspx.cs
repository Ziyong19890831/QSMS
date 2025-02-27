using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_Course : System.Web.UI.Page
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
            Utility.SetDdlConfig(ddl_Class1, "CourseClass1", "請選擇");
            //Utility.SetDdlConfig(ddl_Class2, "CourseClass2", "請選擇");
            Utility.SetDdlConfig(ddl_Ctype, "CourseCType", "請選擇");
            Utility.setRoleNormal(ddl_Rolename, "請選擇");
            bindData(1);
        }
    }

    protected void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        String sql = @"
SELECT ROW_NUMBER() OVER (ORDER BY M.CourseSNO) as ROW_NO , M.CourseSNO , M.BMVal + '課程' Class1 ,M.CourseName, M.DMVal Ctype,M.CHour,left(m.productIDs,len(m.productIDs)-1) as RoleName,M.BPVal,M.DMVal
from(select QC.CourseSNO ,B.MVal BMVal,B.PVal BPVal,D.MVal DMVal, D.PVal DPVal,QC.CourseName,QC.CHour,(SELECT  cast(RoleName AS NVARCHAR ) + ','
FROM QS_CoursePlanningRole A
Left Join Role R On R.RoleSNO=A.RoleSNO
where PClassSNO=Qc.PClassSNO
FOR XML PATH('')) as productIDs
from QS_Course QC
JOIN Config B ON B.PGroup ='CourseClass1' AND QC.Class1 = B.PVal
JOIN Config D ON D.PGroup ='CourseCType' AND QC.CType = D.PVal)M where 1=1 
        ";
        Dictionary<string, object> wDict = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(txt_CourseName.Text))
        {
            sql += " AND M.CourseName Like '%' + @CourseName + '%' ";
            wDict.Add("CourseName", txt_CourseName.Text.Trim());
        }
        //if (!string.IsNullOrEmpty(txt_UnitName.Text))
        //{
        //    sql += " AND A.UnitName Like '%' + @UnitName + '%' ";
        //    wDict.Add("UnitName", txt_UnitName.Text.Trim());
        //}
        //if (!String.IsNullOrEmpty(ddl_Class2.SelectedValue))
        //{
        //    sql += " AND C.PVal = @Class2 ";
        //    wDict.Add("Class2", ddl_Class2.SelectedValue);
        //}
        if (!String.IsNullOrEmpty(ddl_Class1.SelectedValue))
        {
            sql += " AND M.BPVal = @Class1 ";
            wDict.Add("Class1", ddl_Class1.SelectedValue);
        }

        if (!String.IsNullOrEmpty(ddl_Ctype.SelectedValue))
        {
            sql += " AND M.DPVal = @Ctype ";
            wDict.Add("Ctype", ddl_Ctype.SelectedValue);
        }
        //if (!String.IsNullOrEmpty(ddl_Rolename.SelectedValue))
        //{
        //    sql += " AND R.RoleSNO = @RoleSNO ";
        //    wDict.Add("RoleSNO", ddl_Rolename.SelectedValue);
        //}
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
        aDict.Add("id", id);
        DataHelper objDH = new DataHelper();
        objDH.executeNonQuery("Delete QS_Course Where CourseSNO=@id", aDict);
        Response.Write("<script>alert('刪除成功!') </script>");
        //objDH.executeNonQuery("Update Notice Set Title='abc' Where id=@id", aDict)
        btnPage_Click(sender, e);
        return;
    }
}