 
using RestSharp;
using System;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Nikan.Common.ApiCall
{
    public class ItsaazApi
    {
        private readonly IMemoryCache _cache;
        public ItsaazApi(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public string ErrorMessage { get; set; }



        // private static readonly string apiUrlOrigin = "https://api.itsaaz.ir";
        //  private static readonly string apiUrlOrigin = "https://gateway.itsaaz.ir";
        private static readonly string apiUrlOrigin = "https://gateway.itsaaz.ir";

        public async Task<string> AutenticationApi2()
        {
            string str;
            try
            {
                if (await this.HealthCheck())
                {
                    RestClient client = new RestClient(ItsaazApi.apiUrlOrigin + "/sts/connect/token");
                    RestRequest restRequest = new RestRequest(Method.POST);
                    restRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
                    restRequest.AddParameter("application/x-www-form-urlencoded", (object)"client_id=fava-esfahan&client_secret=92e4f058-a738-45bf-20d1-863d1315b03f&grant_type=password&password=f_86574213%40E&username=fava-esfahan", ParameterType.RequestBody);
                    RestRequest request = restRequest;
                    IRestResponse<LoginApiResult> restResponse = client.Post<LoginApiResult>((IRestRequest)request);
                    if (restResponse.IsSuccessful)
                    {
                        str = restResponse.Data.access_token;
                    }
                    else
                    {
                        this.ErrorMessage = "عدم دریافت توکن" + restResponse.ErrorMessage;
                        str = "";
                    }
                }
                else
                {
                    this.ErrorMessage = "عدم دریافت توکن|عدم ارتباط با سرور استعلام";
                    str = "";
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = "خطای دریافت توکن اعتبارسنجی" + ex.Message;
                str = "";
            }
            return str;
        }



        public async Task<string> AutenticationApi()
        {
            string str;
            try
            {
                if (await this.HealthCheck())
                {
                    RestClient client = new RestClient(ItsaazApi.apiUrlOrigin + "/sts/connect/token");
                    RestRequest restRequest = new RestRequest(Method.POST);
                    restRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
                    restRequest.AddParameter("application/x-www-form-urlencoded", (object)"client_id=fava-esfahan&client_secret=92e4f058-a738-45bf-20d1-863d1315b03f&grant_type=password&password=f_86574213%40E&username=fava-esfahan", ParameterType.RequestBody);
                    RestRequest request = restRequest;
                    IRestResponse<LoginApiResult> restResponse = client.Post<LoginApiResult>((IRestRequest)request);
                    if (restResponse.IsSuccessful)
                    {
                        str = restResponse.Data.access_token;
                    }
                    else
                    {
                        this.ErrorMessage = "عدم دریافت توکن" + restResponse.ErrorMessage;
                        str = "";
                    }
                }
                else
                {
                    this.ErrorMessage = "عدم دریافت توکن|عدم ارتباط با سرور استعلام";
                    str = "";
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = "خطای دریافت توکن اعتبارسنجی" + ex.Message;
                str = "";
            }
            return str;
        }
       
//        public async Task<ItsaazData> GetData2(GetDataParam model)
//        {

//            var res = new ItsaazData();
//            try
//            {
//                //https://gateway.itsaaz.ir/hub/api/v1/CivilRegistry/MixIdentityData
//                //https://gateway.itsaaz.ir/hub/api/v1/CivilRegistry/BIdentityInquiry' \
//                //string apiUrl = apiUrlOrigin + "/hub/api/v1/CivilRegistry/MixIdentityData"; 
//                string apiUrl = apiUrlOrigin + "/hub/api/v1/CivilRegistry/BIdentityInquiry";
//                var client = new RestClient(apiUrl);
//                var request = new RestRequest(Method.POST);

//                /*
                 
//                 "nationalCode": "0370362063",
//  "faBirthDate": "1369/07/11",
//  "platform": "web",
//  "userAgent": "1"
//}'
                 
//                 */
//                request.AddHeader("authorization", "Bearer " + model.Token);
//                request.AddHeader("content-type", "application/json");
//                // request.AddParameter("application/json", "{\n  \"nationalCode\": \""+ model.NationalCode + "\",\n  \"birthDate\": \"" + model.BirthDate + "\"\n}", ParameterType.RequestBody);

//                request.AddParameter("application/json", "{\n  \"nationalCode\": \"" + model.NationalCode + "\",\n  \"faBirthDate\": \"" + model.BirthDate + "\"\n,\r\n  \"platform\": \"web\",\r\n  \"userAgent\": \"1\"\r\n}", ParameterType.RequestBody);




//                var response = client.Post<GetDataResult>(request);
//                if (response.IsSuccessful)
//                {
//                    res = response.Data.data;
//                    if (res == null)
//                        res.isMatch = null;
//                    else if (res.match == true || res.isMatch == true)
//                        res.isMatch = true;

//                    if (res.isDead == true || res.alive == false)
//                        res.isDead = true;


//                    return res;
//                }
//                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
//                {
//                    res.isMatch = false;
//                    return res;
//                }
//                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
//                {
//                    res.isMatch = false;
//                    return res;
//                }
//                else
//                {
//                    ErrorMessage = "خطا سمت سرویس دهنده " + " [  " + response.StatusCode + " ] " + response.Content;
//                }
//            }
//            catch (Exception er)
//            {

//                ErrorMessage = "خطایی در استعلام رخ داده است" + "  " + er.Message;

//            }




//            return null;
//        }
//        public async Task<ItsaazData> GetData3(GetDataParam model)
//        {

//            var res = new ItsaazData();
//            try
//            {
//                if (await HealthCheck())
//                {
//                    //https://api-ithub.itsaaz.ir/api/v1/IdentityDataTypeB
//                    //https://gateway.itsaaz.ir/hub/api/v1/CivilRegistry/BIdentityInquiry' \
//                    //string apiUrl = apiUrlOrigin + "/hub/api/v1/CivilRegistry/MixIdentityData"; 
//                    // string apiUrl = "https://api-ithub.itsaaz.ir/api/v1/IdentityDataTypeB";
//                    string apiUrl = "https://gateway.itsaaz.ir/hub/api/v1/IdentityDataTypeA";
//                    var client = new RestClient(apiUrl);
//                    var request = new RestRequest(Method.POST);


//                    request.AddHeader("authorization", "Bearer " + model.Token);
//                    request.AddHeader("postman-token", "7845a7f2-a789-6c05-9683-15cebc171737");
//                    request.AddHeader("cache-control", "no-cache");
//                    request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
//                    request.AddHeader("accept", "application/json");
//                    request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"NationalCode\"\r\n\r\n" + model.NationalCode + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"BirthDate\"\r\n\r\n" + model.BirthDate + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"RequestStatus\"\r\n\r\n1\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);

//                    var response = client.Post<GetDataResult>(request);
//                    if (response.IsSuccessful)
//                    {
//                        res = response.Data.data;
//                        if (res == null)
//                            res.isMatch = null;
//                        else if (res.match == true || res.isMatch == true)
//                            res.isMatch = true;

//                        if (res.isDead == true || res.alive == false)
//                            res.isDead = true;


//                        return res;
//                    }
//                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
//                    {
//                        res.isMatch = false;
//                        return res;
//                    }
//                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
//                    {
//                        var error = response.Data.error;
//                        if (error != null)
//                        {
//                            if (error.errorCode == 400)
//                            {
//                                res.code = "400";
//                                res.isMatch = new bool?(false);
//                                return res;
//                            }
//                            if (error.errorCode == 1071)
//                            {
//                                res.code = "1071";
//                                res.isMatch = new bool?(false);
//                                return res;
//                            }
//                            res.code = error.errorCode.ToString();
//                            res.isMatch = new bool?();
//                            return res;
//                        }
//                        res.code = "BadRequest and error empty";
//                        res.isMatch = new bool?(false);
//                        return res;
//                    }
//                    else
//                    {
//                        ErrorMessage = "خطا سمت سرویس دهنده " + " [  " + response.StatusCode + " ] " + response.Content;
//                    }

//                }
//                else
//                {
//                    ErrorMessage = "عدم برقراری ارتباط با سرور";
//                    return  null;
//                }


//            }
//            catch (Exception er)
//            {

//                ErrorMessage = "خطایی در استعلام رخ داده است" + "  " + er.Message;

//            }




//            return null;
//        }

    






//        public async Task<ItsaazData> GetData4(GetDataParam model)
//        {
//            ItsaazApi itsaazApi1 = this;
//            ItsaazData res = new ItsaazData();
//            try
//            {
//                if (await HealthCheck())
//                {
//                    RestClient client = new RestClient("https://gateway.itsaaz.ir/hub/api/v1/IdentityDataTypeA");
//                    RestRequest request = new RestRequest(Method.POST);
//                    request.AddHeader("postman-token", "22453841-f946-aee0-bed3-7c8859ae5352");
//                    request.AddHeader("cache-control", "no-cache");
//                    request.AddHeader("authorization", "Bearer    " + model.Token);
//                    request.AddHeader("content-type", "application/json");
//                    request.AddParameter("application/json", (object)("{\r\n  \"nationalCode\": \"" + model.NationalCode + "\",\r\n  \"birthDate\": \"" + model.BirthDate + "\",\r\n  \"orderId\": \"\"\r\n}"), ParameterType.RequestBody);
//                    IRestResponse<GetDataResult> restResponse = client.Post<GetDataResult>((IRestRequest)request);
//                    if (restResponse.IsSuccessful)
//                    {
//                        res = restResponse.Data.data;
//                        if (restResponse.Data.error == null)
//                            res.isMatch = true;
//                        return res;
//                    }
//                    if (restResponse.StatusCode == HttpStatusCode.NotFound)
//                    {
//                        res.code = "NotFound";
//                        res.isMatch = false;
//                        return res;
//                    }
//                    if (restResponse.StatusCode == HttpStatusCode.BadRequest)
//                    {
//                        ItsaazError error = restResponse.Data.error;
//                        if (error != null)
//                        {
//                            if (error.errorCode == 400)
//                            {
//                                res.code = "400";
//                                res.isMatch = false;
//                                return res;
//                            }
//                            if (error.errorCode == 1071)
//                            {
//                                res.code = "1071";
//                                res.isMatch = false;
//                                return res;
//                            }
//                            res.code = error.errorCode.ToString();
//                            res.isMatch = new bool?();
//                            return res;
//                        }
//                        res.code = "BadRequest and error empty";
//                        res.isMatch = new bool?(false);
//                        return res;
//                    }
//                    if (restResponse.StatusCode == HttpStatusCode.Forbidden)
//                    {
//                        res.code = "Forbidden";
//                        res.isMatch = false;
//                        return res;
//                    }
//                    ItsaazData itsaazData = res;
//                    HttpStatusCode statusCode = restResponse.StatusCode;
//                    string str1 = statusCode.ToString();
//                    itsaazData.code = str1;
//                    ItsaazApi itsaazApi2 = itsaazApi1;
//                    statusCode = restResponse.StatusCode;
//                    string str2 = "خطا سمت سرویس دهنده  [  " + statusCode.ToString() + " ] " + restResponse.Content;
//                    itsaazApi2.ErrorMessage = str2;
//                }
//                else
//                {
//                    itsaazApi1.ErrorMessage = "عدم برقراری ارتباط با سرور";
//                    return (ItsaazData)null;
//                }
//            }
//            catch (Exception ex)
//            {
//                itsaazApi1.ErrorMessage = "خطایی در استعلام رخ داده است  " + ex.Message;
//            }
//            return  null;
//        }

         

        public async Task<ItsaazData> GetData(GetDataParam model)
        {

            
            ItsaazData res = new ItsaazData(); 

            try
            {
                if (await this.HealthCheck())
                {
                                                         //1401 12 02 13 36 14 59 6330
                    var requestId = DateTime.Now.ToString("yyyyMMddHHMMssff")+ "6330";
                    RestClient client = new RestClient("https://gateway.itsaaz.ir/hub/api/v1/IdentityDataTypeA");
                    RestRequest request = new RestRequest(Method.POST);
                    request.AddHeader("postman-token", "22453841-f946-aee0-bed3-7c8859ae5352");
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("authorization", "Bearer    " + model.Token);
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("requestId", requestId); 
                    request.AddParameter("application/json", (object)("{\r\n  \"nationalCode\": \"" + model.NationalCode + "\",\r\n  \"birthDate\": \"" + model.BirthDate + "\",\r\n  \"orderId\": \"\"\r\n}"), ParameterType.RequestBody);
                    IRestResponse<GetDataResult> restResponse = client.Post<GetDataResult>((IRestRequest)request);

                     

                    if (restResponse.IsSuccessful)
                    {
                        res = restResponse.Data.data;
                        if (restResponse.Data.error == null)
                            res.isMatch = true;

                        res.requestId = requestId;
                        return res;
                    }
                    if (restResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        res.code = "NotFound";
                        res.isMatch = false;
                        res.requestId = requestId;
                        return res;
                    }
                    if (restResponse.StatusCode == HttpStatusCode.BadRequest)
                    {
                        res.requestId = requestId;

                        ItsaazError error = restResponse.Data?.error;
                        if (error != null)
                        {
                            if (error.errorCode == 400)
                            {
                                res.code = "400";
                                res.isMatch = false;
                                return res;
                            }
                            if (error.errorCode == 1071)
                            {
                                res.code = "1071";
                                res.isMatch = false;
                                return res;
                            }
                            res.code = error.errorCode.ToString();
                            res.isMatch =null;
                            return res;
                        }
                        res.code = "BadRequest and error empty";
                        res.isMatch = false;
                        return res;
                    }
                    if (restResponse.StatusCode == HttpStatusCode.Forbidden)
                    {
                        res.requestId = requestId;
                        res.code = "Forbidden";
                        res.isMatch = false;
                        return res;
                    }
                    
                    res.requestId = requestId;
                    res.code = restResponse.StatusCode.ToString();
                    this.ErrorMessage = "خطا سمت سرویس دهنده  [  " + restResponse.StatusCode + " ] " + restResponse.Content;
                     
                    
                }
                else
                {
                    this.ErrorMessage = "عدم برقراری ارتباط با سرور";
                    return null;
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = "خطایی در استعلام رخ داده است  " + ex.Message;
            }
            return null;
        }







        public async Task<bool> HealthCheck()
        {
            bool flag = false;
            try
            {
                RestClient restClient = new RestClient("https://gateway.itsaaz.ir/hub/api/v1/Hc/Hub");
                RestRequest restRequest = new RestRequest(Method.GET);
                restRequest.AddHeader("postman-token", "f4a7fed9-fa18-9e28-8c63-92e9dcca8ea9");
                restRequest.AddHeader("cache-control", "no-cache");
                RestRequest request = restRequest;
                IRestResponse<HealthCheckModel> restResponse = restClient.Execute<HealthCheckModel>((IRestRequest)request);
                if (restResponse.IsSuccessful)
                    return restResponse.Data.data.Any<Datum>((Func<Datum, bool>)(w => w.hubRequestType == 101 && w.isConnect));
            }
            catch (Exception ex)
            {
                this.ErrorMessage = "خطایی در استعلام رخ داده است  " + ex.Message;
            }
            return flag;
        }













    }


    public class ItsaazError
    {
        public int errorCode { get; set; }

        public string customMessage { get; set; }

        public object exception { get; set; }
    }


    public class Datum
    {
        public int hubRequestType { get; set; }

        public string hubRequestTypeText { get; set; }

        public bool isConnect { get; set; }
    }
    public class HealthCheckModel
    {
        public List<Datum> data { get; set; }

        public object meta { get; set; }

        public object error { get; set; }
    }


    public class LoginParam
    { 
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; }
        public string password { get; set; }
        public string username { get; set; }
        

    }

    public class GetDataParam
    {
        public string Token { get; set; }
        public string NationalCode { get; set; }
        public string BirthDate { get; set; }
       

    }

    public class LoginApiResult
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
    }



    public class ItsaazData
    {
        public string birthDate { get; set; }
        public string lastName { get; set; }
        public string fatherName { get; set; }
        public int? gender { get; set; }
        public string firstName { get; set; }
        public string nationalCode { get; set; }
        public string identityId { get; set; }
        public bool isDead { get; set; }
        public bool? isMatch { get; set; }


        public bool? match { get; set; }
        public bool? alive { get; set; }

        public string requestId { get; set; }
        public string code { get; set; }


    }
    
    public class GetDataResult
    {
        public ItsaazData data { get; set; }
        public object meta { get; set; }
        public ItsaazError error { get; set; }
    }

    public class MyCacheService
    {
        private readonly IMemoryCache _cache;

        public MyCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetAndCache<T>(string cacheKey, Func<Task<T>> backup, int cachedMinutes = 20)
        {
            if (!_cache.TryGetValue(cacheKey, out T resultValue) || resultValue == null)
            {
                resultValue = await backup();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(cachedMinutes));

                _cache.Set(cacheKey, resultValue, cacheEntryOptions);
            }
            return resultValue;
        }
    }

}
