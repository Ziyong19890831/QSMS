using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mgt_SetExcelColumn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (string.IsNullOrEmpty(Request.QueryString["ReporType"])) NoDataError();
            var item = ((Dictionary<Dictionary<string, string>, DataTable>)Session[Request.QueryString["ReporType"]]).FirstOrDefault();
            if (Request.QueryString["ReporType"].ToString() != "ReportMember")
            {
                if (item.Key == null) NoDataError();

                cbl_SetColumn.DataSource = item.Key;
                cbl_SetColumn.DataBind();

            }
            else
            {
                var itemA = ((Dictionary<Dictionary<string, string>, DataTable>)Session[Request.QueryString["ReporType1"]]).FirstOrDefault();
                var itemB = ((Dictionary<Dictionary<string, string>, DataTable>)Session[Request.QueryString["ReporType2"]]).FirstOrDefault();
                var itemC = ((Dictionary<Dictionary<string, string>, DataTable>)Session[Request.QueryString["ReporType3"]]).FirstOrDefault();
                var itemD = ((Dictionary<Dictionary<string, string>, DataTable>)Session[Request.QueryString["ReporType4"]]).FirstOrDefault();
               
                p1.Visible = false;
                p2.Visible = true;
                CheckBoxList1.DataSource = item.Key;
                CheckBoxList1.DataBind();
                CheckBoxList2.DataSource = itemA.Key;
                CheckBoxList2.DataBind();
                CheckBoxList3.DataSource = itemB.Key;
                CheckBoxList3.DataBind();
                CheckBoxList4.DataSource = itemC.Key;
                CheckBoxList4.DataBind();
                CheckBoxList5.DataSource = itemD.Key;
                CheckBoxList5.DataBind();
                if (Session[Request.QueryString["ReporType5"]] != null)
                {
                    var itemE = ((Dictionary<Dictionary<string, string>, DataTable>)Session[Request.QueryString["ReporType5"]]).FirstOrDefault();
                    CheckBoxList6.DataSource = itemE.Key;
                    CheckBoxList6.DataBind();
                }
                    
            }
            //Key = 匯出設定 , Value = QueryData 

        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        string reportType = Request.QueryString["ReporType"];
        string reportType1 = Request.QueryString["ReporType1"];
        string reportType2 = Request.QueryString["ReporType2"];
        string reportType3 = Request.QueryString["ReporType3"];
        string reportType4 = Request.QueryString["ReporType4"];
        string reportType5 = Request.QueryString["ReporType5"];
        if (string.IsNullOrEmpty(reportType)) NoDataError();

        var item = ((Dictionary<Dictionary<string, string>, DataTable>)Session[reportType]).FirstOrDefault();
        DataTable dt = item.Value;
        if (dt == null || dt.Rows.Count == 0) NoDataError();
        if (reportType4 == null)
        {
            var selected = cbl_SetColumn.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
            Dictionary<List<ListItem>, DataTable> dicData = new Dictionary<List<ListItem>, DataTable>();
            dicData.Add(selected, dt);
            var dataResult = Utility.DataProcess(dicData);

            byte[] file = null;
            ExcelHelper.DatatableToExcelForWeb(reportType, dataResult, ref file, false, "");

            // 匯出 Excel --- 使用 Response 下載附件         
            string filename = string.Format("{0}{1}.xlsx", reportType, DateTime.Today.ToString("yyyyMMdd"));
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}", filename));
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.BinaryWrite(file);
            Response.End();
        }
        else
        {
            Dictionary<List<ListItem>, DataTable> dicData = new Dictionary<List<ListItem>, DataTable>();
            var selected = CheckBoxList1.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
            var selected1 = CheckBoxList2.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
            var selected2 = CheckBoxList3.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
            var selected3 = CheckBoxList4.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
            var selected4 = CheckBoxList5.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
            var selected5 = CheckBoxList6.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
            dicData.Add(selected, dt);
            dicData.Add(selected1, dt);
            dicData.Add(selected2, dt);
            dicData.Add(selected3, dt);
            dicData.Add(selected4, dt);
            dicData.Add(selected5, dt);
            var dataResult = Utility.DataProcess(dicData);
            byte[] file = null;
            ExcelHelper.DatatableToExcelForWeb(reportType, dataResult, ref file, false, "");

            // 匯出 Excel --- 使用 Response 下載附件         
            string filename = string.Format("{0}{1}.xlsx", reportType, DateTime.Today.ToString("yyyyMMdd"));
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}", filename));
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.BinaryWrite(file);
            Response.End();
        }

    }

    private void NoDataError()
    {
        Response.Write("<script>alert('請重新查詢點選匯出!');window.close(); </script>");
        Response.End();
    }
}