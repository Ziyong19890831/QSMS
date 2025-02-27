<%@ Application Language="C#" %>
<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
       

    }

    void Application_End(object sender, EventArgs e)
    {
        //  在應用程式關閉時執行的程式碼

    }

    void Application_Error(object sender, EventArgs e)
    {
        // 在發生未處理的錯誤時執行的程式碼
        //只會徵測到程式錯誤才會導向已下網址
        //Response.Redirect("Error404.aspx");


    }
    void Session_Start(object sender, EventArgs e)
    {
        //WebApiConfig.Register(System.Web.Http.GlobalConfiguration.Configuration);
        // 在新的工作階段啟動時執行的程式碼
        var isNewUser = Utility.CheckNewUserCache(Session.SessionID);
        var historyResult = Utility.GetHistoryTotal();
        bool isToday = true;

        if (historyResult.Today == null || (historyResult.Today - DateTime.Today).Days != 0)
        {
            historyResult.TodayCount = 1;
            isToday = false;
        }
        Application.Lock();
        if (isNewUser)
        {
            historyResult.HisCount += 1;
            if (isToday) historyResult.TodayCount += 1;
        }

        historyResult.Today = DateTime.Today;
        Utility.UpdateUserCount(historyResult);
        Application.Set("HisCount", historyResult.HisCount);
        Application.Set("TodayCount", historyResult.TodayCount);
        Application.Set("OnlineUsers", Utility.GetOnlineCount(Session.SessionID));
        Application.UnLock();
    }

    void Session_End(object sender, EventArgs e)
    {
        // 在工作階段結束時執行的程式碼
        // 注意: 只有在  Web.config 檔案中將 sessionstate 模式設定為 InProc 時，
        // 才會引起 Session_End 事件。如果將 session 模式設定為 StateServer 
        // 或 SQLServer，則不會引起該事件。
        Application.Lock();
        Application.Set("OnlineUsers", Utility.GetOnlineCount(Session.SessionID));
        Application.UnLock();
    }

</script>
