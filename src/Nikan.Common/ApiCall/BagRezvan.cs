using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
namespace Nikan.Common.ApiCall
{
    public class BagRezvanService
    {

        public string ErrorMessage { get; set; }
        private static readonly string apiUrlOrigin = "http://192.168.104.10:727/Exist";
        public List<CallCheckingCitizensDead> CheckNationCodes(List<string> NationCodes)
        {
            var model = new CheckingCitizensDeadModel();
            try
            {
                
                var httpRequest = (HttpWebRequest)WebRequest.Create(apiUrlOrigin);
                httpRequest.Method = "POST";
                httpRequest.Accept = "application/json";
                httpRequest.Headers["Authorization"] = "Basic RmF2YTpGQHY0RGF0YQ==";
                httpRequest.ContentType = "application/json";
                var l = "";
                for (int i = 0; i < NationCodes.Count; i++)
                {
                    l += " '" + NationCodes[i].Trim() + "'";
                    if (i != NationCodes.Count - 1)
                    {
                        l += ",";
                    }
                }

                var data = @"[" + l + " ]";
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(data);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var result = streamReader.ReadToEnd();
                        if (!string.IsNullOrEmpty(result))
                        {
                            model = JsonConvert.DeserializeObject<CheckingCitizensDeadModel>(result);
                            if (model.Status == 2)
                            {
                                if (model.Value.Any())
                                {
                                    return model.Value;
                                }
                            }
                        }


                    }

                }

            }
            catch (Exception er)
            {


            }
            return null;
        }
        public bool? IsDead(string nationCode )
        {
            var model = new CheckingCitizensDeadModel();
            try
            {

                var httpRequest = (HttpWebRequest)WebRequest.Create(apiUrlOrigin);
                httpRequest.Method = "POST";
                httpRequest.Accept = "application/json";
                httpRequest.Headers["Authorization"] = "Basic RmF2YTpGQHY0RGF0YQ==";
                httpRequest.ContentType = "application/json";
                var l = " '" + nationCode + "'"; 
                    

                var data = @"[" + l + " ]";
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(data);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var result = streamReader.ReadToEnd();
                        if (!string.IsNullOrEmpty(result))
                        {
                            model = JsonConvert.DeserializeObject<CheckingCitizensDeadModel>(result);
                            if (model.Status == 2)
                            {
                                if (model.Value.Any())
                                {
                                    var f = model.Value.FirstOrDefault();
                                    return f.Exist; 
                                }
                            }
                        } 
                    } 
                }

            }
            catch (Exception er)
            {
                ErrorMessage = " خطایی در استعلام رخ داده است " +er.Message;

            }
            return null;
        }

    }


    public class CallCheckingCitizensDead
    {
        public string NationalCode { get; set; }
        public bool Exist { get; set; }
    }


    public class CheckingCitizensDeadModel
    {
        public List<CallCheckingCitizensDead> Value { get; set; }
        public int OperationType { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public List<FeedBackError> ExceptionMessage { get; set; }
    }

    public class FeedBackError
    {

        public string Code { get; set; }
        public string Description { get; set; }
    }






}
