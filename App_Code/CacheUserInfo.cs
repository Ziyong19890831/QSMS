using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// CacheUserInfo 的摘要描述
/// </summary>
public class CacheUserInfo
{
    public string SessionId { get; set; }
    public DateTime CreateTime { get; set; }
}

public class CountInfo
{
    public decimal HisCount { get; set; }
    public decimal TodayCount { get; set; }
    public DateTime Today { get; set; }
}