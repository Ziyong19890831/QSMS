using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

/// <summary>
/// DataCacheHelper 的摘要描述
/// </summary>
public static class DataCacheHelper
{
    public static Cache _cache { get { return HttpRuntime.Cache; } }

    public static T Get<T>(string cacheId) where T : class
    {
        return _cache[cacheId] as T;
    }

    public static void Clear<T>(string cacheId) where T : class
    {
        _cache.Remove(cacheId);
    }

    /// <summary>
    /// 設定Cache 資料
    /// </summary>
    /// <param name="cacheId">Key</param>
    /// <param name="cacheObj">來源資料</param>
    /// <param name="cacheMinutes">Cache 效期 , 單位:分 (預設60分)</param>
    public static void SetCache<T>(string cacheId, T cachData, double? cacheTimes, bool isMin = true)
    {

        if (!cacheTimes.HasValue || cacheTimes == 0) cacheTimes = 60;
        //設定資料
        if (cachData != null)
        {
            //區分分或秒 , 預設為分
            if (isMin)  cacheTimes = cacheTimes * 60;            

            _cache.Insert(cacheId, cachData, null, DateTime.Now.AddSeconds(cacheTimes.Value)
                        , Cache.NoSlidingExpiration, CacheItemPriority.High, null);


        }
    }
}