using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_CalendarAjax : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<Calendar> eventsList = new List<Calendar>();
            DataHelper objDH = new DataHelper();
            string sql = @"Select Id,Title,StartTime,EndTime,Url from Calendar where IsEnable=1";
            DataTable ObjDT = objDH.queryData(sql, null);
            if (ObjDT.Rows.Count > 0)
            {
                for (int i = 0; i < ObjDT.Rows.Count; i++)
                {
                    
                    Calendar calEvent = new Calendar();  //new一個新的類別，將值一一塞進
                    calEvent.id = ObjDT.Rows[i]["id"].ToString();
                    DateTime SDatetime= Convert.ToDateTime(ObjDT.Rows[i]["StartTime"].ToString());
                    DateTime EDatetime = Convert.ToDateTime(ObjDT.Rows[i]["EndTime"].ToString());
                    calEvent.start = SDatetime.ToString("yyyy-MM-ddThh:mm:ss");
                    calEvent.end = EDatetime.ToString("yyyy-MM-ddThh:mm:ss");
                    calEvent.title = ObjDT.Rows[i]["Title"].ToString();
                    calEvent.url = ObjDT.Rows[i]["Url"].ToString();
                    eventsList.Add(calEvent);   //將此類別新增到eventsList 
                }
            }

            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string strEvents = js.Serialize(eventsList);
            Response.Clear();
            Response.Write(strEvents);
            Response.End();
        }
      


    }

    public class Calendar
    {
        public string id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string url { get; set; }
   

    }
}