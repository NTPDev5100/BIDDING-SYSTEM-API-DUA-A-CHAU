using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class OneSignalPush
    {
        public static void OneSignalWebPushNotifications(string headings, string content, List<string> OneSignal_PlayerId, string urlOfOneSignal)
        {
            try
            {
                if (OneSignal_PlayerId.Any())
                {
                    string onesignalAppId = "72ba8155-836a-4013-abaa-a787681eb5eb";//cái này sửa lại
                    string onesignalRestId = "YTJjM2ZjZmUtN2ExNi00NDhlLTk4YWEtMjQ1MGZiZDMyMTFl";//cái này sửa lại

                    var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;
                    request.KeepAlive = true;
                    request.Method = "POST";
                    request.ContentType = "application/json; charset=utf-8";
                    request.Headers.Add("authorization", "Basic " + onesignalRestId);

                    var obj = new
                    {
                        app_id = onesignalAppId,
                        headings = new { en = headings },
                        contents = new { en = content },
                        channel_for_external_user_ids = "push",
                        //include_player_ids = new string[] { "4ecd269c-7356-11ec-9a39-2255d3251ce2" }//Gửi cho user đc chỉ định
                        include_player_ids =  OneSignal_PlayerId  ,//Gửi cho user đc chỉ định
                        url = urlOfOneSignal,
                        //included_segments = new string[] { "Subscribed Users" } //Gửi cho tất cả user nào đăng ký
                    };
                    var param = JsonConvert.SerializeObject(obj);
                    byte[] byteArray = Encoding.UTF8.GetBytes(param);

                    string responseContent = null;

                    try
                    {
                        using (var writer = request.GetRequestStream())
                        {
                            writer.Write(byteArray, 0, byteArray.Length);
                        }

                        using (var response = request.GetResponse() as HttpWebResponse)
                        {
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                responseContent = reader.ReadToEnd();
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                    }
                    System.Diagnostics.Debug.WriteLine(responseContent);
                }
            }
            catch { }
        }

        public static void OneSignalMobilePushNotifications(string headings, string content, List<string> OneSignal_PlayerId, string urlOfOneSignal)
        {
            try
            {
                if (OneSignal_PlayerId.Any())
                {
                    string onesignalAppId = "300d518d-1d40-4a06-8f46-eeb920a7bc52";//cái này sửa lại
                    string onesignalRestId = "MmVjYzQxZTUtMDI3Ni00MzZmLTgwMjItNDdjODVlM2M1MjUx";//cái này sửa lại

                    var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;
                    request.KeepAlive = true;
                    request.Method = "POST";
                    request.ContentType = "application/json; charset=utf-8";
                    request.Headers.Add("authorization", "Basic " + onesignalRestId);

                    var obj = new
                    {
                        app_id = onesignalAppId,
                        headings = new { en = headings },
                        contents = new { en = content },
                        channel_for_external_user_ids = "push",
                        //include_player_ids = new string[] { "532b38aa-66d7-4641-ba55-b5cb3588255b" },//Gửi cho user đc chỉ định
                        include_player_ids = OneSignal_PlayerId,//Gửi cho user đc chỉ định
                        url = urlOfOneSignal,
                        //included_segments = new string[] { "Subscribed Users" } //Gửi cho tất cả user nào đăng ký
                    };
                    var param = JsonConvert.SerializeObject(obj);
                    byte[] byteArray = Encoding.UTF8.GetBytes(param);

                    string responseContent = null;

                    try
                    {
                        using (var writer = request.GetRequestStream())
                        {
                            writer.Write(byteArray, 0, byteArray.Length);
                        }

                        using (var response = request.GetResponse() as HttpWebResponse)
                        {
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                responseContent = reader.ReadToEnd();
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                    }
                    System.Diagnostics.Debug.WriteLine(responseContent);
                }
            }
            catch { }
        }

    }
}
