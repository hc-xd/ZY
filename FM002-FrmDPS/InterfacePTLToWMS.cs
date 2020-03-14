using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
namespace NFrmDPS
{
    class InterfacePTLToWMS
    {
        public static resultMsg Send_updatePickingInfo(PickModel pickModel,string pick_info_url)
        {
            resultMsg msg = new resultMsg();
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
              //  string json = JsonConvert.SerializeObject(pickModel);
                string json = js.Serialize(pickModel);
                string result= SendApi(pick_info_url,json,"post");
                msg = js.Deserialize<resultMsg>(result);// JsonConvert.DeserializeObject<resultMsg>(result);
            }
            catch (Exception ex)
            {
                msg.code = "-1";
                msg.msg =string.Format("发送请求出现异常，异常原因:{0}",ex.Message);
                msg.data = "";
            }
            return msg;
        }

        public static resultMsg Send_updateReviewInfo(ReviewModel review,string review_info_url)
        {
            resultMsg msg = new resultMsg();
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(review);   // JsonConvert.SerializeObject(review);
                string result = SendApi(review_info_url, json, "post");
                msg = js.Deserialize<resultMsg>(result);   //JsonConvert.DeserializeObject<resultMsg>(result);
            }
            catch (Exception ex)
            {
                msg.code = "-1";
                msg.msg = string.Format("发送请求出现异常，异常原因:{0}", ex.Message);
                msg.data = "";
            }
            return msg;
        }

        public static string SendApi(string url, string jsonstr, string type)
        {
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);//webrequest请求api地址  
            request.Accept = "text/html,application/xhtml+xml,*/*";
            request.ContentType = "application/json";
            request.Method = type.ToUpper().ToString();//get或者post  
            byte[] buffer = encoding.GetBytes(jsonstr);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }


    

    public class resultMsg
    {
        public string code { get; set; }

        public string msg { get; set; }

        public string data { get; set; }
    }

    public class PickModel
    {
        public int pickID { get; set; }
        public int PickQty { get; set; }
        public int pickFlag { get; set; }
    }

    public class ReviewModel
    {
        public int ReviewID { get; set; }
        public int ReviewQty { get; set; }
        public int ReviewFlag { get; set; }
    }
}
