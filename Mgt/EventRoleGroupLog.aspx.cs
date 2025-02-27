using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_EventRoleGroupLog : System.Web.UI.Page
{
    UserInfo userInfo = null;
    int viewrole = 1;
    protected void Page_Init(object sender, EventArgs e)
    {
        //取得UserInfo資訊
        userInfo = (UserInfo)Session["QSMS_UserInfo"];
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindData(1);
        }
    }

    public void bindData(int page)
    {
        if (viewrole == 0) return;
        if (page < 1) page = 1;
        int pageRecord = 10;
        Dictionary<string, object> adict = new Dictionary<string, object>();
        DataHelper ObjDH = new DataHelper();
        string sql = @"with aa as (
                    Select distinct E.eventSNO,E.EventName,EG.EventGroup+'-'+Cast(EG.EventNum as nchar) 'EventGroupLog'  from RoleBind  RB
                    Left Join Event E On E.EventSNO=RB.CSNO
                    Left Join EventGroupNum EG On EG.EventSNO=E.EventSNO
                    Left Join Person P On P.PersonSNO=RB.CreateUserID
                    LEFT JOIN Organ O ON O.OrganSNO = P.OrganSNO
                    LEFT JOIN Role R ON R.RoleSNO = P.RoleSNO
                    where TypeKey='Event_AE' and R.RoleGroup=@RoleGroup and E.EventSNO is not null)
                    Select ROW_NUMBER() OVER (ORDER BY aa.eventSNO DESC)  as ROW_NO,* from aa where EventGroupLog is not null";

        adict.Add("RoleGroup", userInfo.RoleGroup);
        DataTable ObjDT = ObjDH.queryData(sql, adict);
        if (ObjDT.Rows.Count > 0)
        {
            int maxPageNumber = (ObjDT.Rows.Count - 1) / pageRecord + 1;
            if (page > maxPageNumber) page = maxPageNumber;
            ObjDT.DefaultView.RowFilter = String.Format("ROW_NO>={0} AND ROW_NO<={1}", (page - 1) * pageRecord + 1, page * pageRecord);
            gv_RoleGroupLog.DataSource = ObjDT;
            gv_RoleGroupLog.DataBind();
            ltl_PageNumber.Text = Utility.showPageNumber(ObjDT.Rows.Count, page, pageRecord);
        }
        else
        {
            lb_Hint.Text = "暫無編號紀錄";
        }

    }

    protected void gv_RoleGroupLog_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }

    protected void btnPage_Click(object sender, EventArgs e)
    {
        int page = 1;
        int.TryParse(txt_Page.Value, out page);
        bindData(page);
    }
}