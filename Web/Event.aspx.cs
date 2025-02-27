using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_Event : System.Web.UI.Page
{
    UserInfo userInfo = null;
    
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        if (Session["QSMS_UserInfo"] != null) userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Utility.setAreaCodeA(ddl_AddressA, "---請選擇城市---");
            Utility.setRoleNormal(dd2_RoleName, "---適用人員全選---");
            setClassCode(ddl_Event_Class, "---請選擇分類---");
            setClass4Code(ddl_CourseClass4, "---請選擇分類---");
            bindData();
        }
    }

    protected void bindData()
    {
        Dictionary<string, object> aDict = new Dictionary<string, object>();
        DataHelper objDH = new DataHelper();

        string sql = @"
          With T as(
			
                Select
                    E.EventSNO, EventName, StartTime, EndTime, E.IsEnable,[CLocationAreaA]+'/'+[CLocationAreaB] CLocationArea,
					Stuff(
					(Select','+ convert(varchar, CStartDay, 111) from EventBatch EB where EB.EventSNO=E. EventSNO
					 FOR XML PATH('')),1,1,''
					) CDay,
				    (Select count(1) From EventD ed Where ed.EventSNO=E.EventSNO) pCount , CountLimit,G.Mval as Class1,F.Mval as Class2,
                    [CLocationAreaA],[CLocationAreaB],
            Stuff( 
            (
                select ',' + CAST(R.RoleName as nvarchar) 
                from RoleBind RB
		            join Role R on R.RoleSNO=RB.RoleSNO
		        where RB.CSNO=E.EventSNO and RB.TypeKey='Event_AE' 
                FOR XML PATH('')
            )
            ,1,1,'') RoleBindName ,E.PClassSNO,E.ERSNO
		        from Event E 
		           
                    Left Join Config G On E.class3=G.Pval and G.PGroup='CourseClass3'
                    Left Join Config F On E.class4=F.Pval and F.PGroup='CourseClass4'					
		        Where E.isEnable=1
            )
		    Select  ROW_NUMBER() OVER (ORDER BY StartTime DESC) ROW_NO,* From T
            Left Join EventBatch EB On EB.EventSNO=T.EventSNO 
            where 1=1
            And getdate() > StartTime and GETDATE()<EndTime
        ";
        if (ddl_CourseClass4.SelectedValue != "")
        {
            sql += " And Class1=@Class1";
            aDict.Add("Class1", ddl_CourseClass4.SelectedItem.Text);
        }

        if (ddl_Event_Class.SelectedValue != "")
        {
            sql += " And Class2=@Class2";
            aDict.Add("Class2", ddl_Event_Class.SelectedItem.Text);
        }
        if (dd2_RoleName.SelectedValue != "")
        {
            sql += " And RoleBindName LIKE '%'+ @RoleBindName +'%'";
            aDict.Add("RoleBindName", dd2_RoleName.SelectedItem.Text);
        }
        if (txtSearch.Value != "")
        {
            sql += " And EventName LIKE '%'+ @EventName +'%'";
            aDict.Add("EventName", txtSearch.Value);
        }
        if (ddl_AddressA.SelectedValue != "")
        {
            sql += " And EventLocationCodeA=@EventLocationCodeA ";
            aDict.Add("EventLocationCodeA", ddl_AddressA.SelectedValue);
        }
        if (STime.Value != "")
        {
            sql += " And StartTime >=@StartTime ";
            aDict.Add("StartTime", STime.Value);
        }
        if (ETime.Value != "")
        {
            sql += " And EndTime <= @EndTime ";
            aDict.Add("EndTime", ETime.Value);
        }
        if (SCDay.Value != "")
        {
            sql += " And CDay >= @SCDay ";
            aDict.Add("SCDay", SCDay.Value.Replace("-", "/"));
        }
        if (ECDay.Value != "")
        {
            sql += " And CDay <= @ECDay ";
            aDict.Add("ECDay", ECDay.Value.Replace("-","/"));
        }
        DataTable objDT = objDH.queryData(sql, aDict);
        rpt_Notice.DataSource = objDT.DefaultView;
        rpt_Notice.DataBind();


    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindData();
    }

    public static void setClass4Code(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER(ORDER BY PVal) as ROW_NO, PVal, Mval FROM Config where PGroup='CourseClass3'", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }

    public static void setClassCode(System.Web.UI.WebControls.DropDownList ddl, String DefaultString = null)
    {
        DataHelper objDH = new DataHelper();
        DataTable objDT = objDH.queryData("SELECT ROW_NUMBER() OVER(ORDER BY PVal) as ROW_NO, PVal, Mval FROM Config where PGroup='CourseClass4'", null);
        ddl.DataSource = objDT;
        ddl.DataBind();
        if (DefaultString != null)
        {
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(DefaultString, ""));
        }
    }


    protected void ddl_AddressA_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddl_AddressB.Items.Clear();

        //Dictionary<string, object> aDict = new Dictionary<string, object>();
        //String AreaCodeA = ddl_AddressA.SelectedValue;
        //if (!String.IsNullOrEmpty(AreaCodeA))
        //{
        //    Utility.setAreaCodeB(ddl_AddressB, AreaCodeA, "---請選擇地區---");
        //    ddl_AddressB.Enabled = true;
        //}
        //else
        //{
        //    ddl_AddressB.Items.Add(new ListItem("請選擇", ""));
        //    ddl_AddressB.Enabled = false;

        //}
    }
}