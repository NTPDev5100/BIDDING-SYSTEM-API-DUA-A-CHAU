﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    //public class MemoryCacheHelper
    //{
    //    /// <summary>
    //    /// Get cache value by key
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns></returns>
    //    public static object GetValue(string key)
    //    {
    //        return MemoryCache.Default.Get(key);
    //    }

    //    /// <summary>
    //    /// Add a cache object with date expiration
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="value"></param>
    //    /// <param name="absExpiration"></param>
    //    /// <returns></returns>
    //    public static bool Add(string key, object value, DateTimeOffset absExpiration)
    //    {
    //        return MemoryCache.Default.Add(key, value, absExpiration);
    //    }

    //    /// <summary>
    //    /// Delete cache value from key
    //    /// </summary>
    //    /// <param name="key"></param>
    //    public static void Delete(string key)
    //    {
    //        MemoryCache memoryCache = MemoryCache.Default;
    //        if (memoryCache.Contains(key))
    //        {
    //            memoryCache.Remove(key);
    //        }
    //    }
    //    /// <summary>
    //    /// Xóa cache cũ nếu có và cập nhật lại mới
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="value"></param>
    //    /// <param name="absExpiration"></param>
    //    public static void Update(string key, object value, DateTimeOffset absExpiration)
    //    {
    //        MemoryCache memoryCache = MemoryCache.Default;
    //        if (memoryCache.Contains(key))
    //        {
    //            memoryCache.Remove(key);
    //        }
    //        MemoryCache.Default.Add(key, value, absExpiration);
    //    }
    //}
}
