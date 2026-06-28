using System;
using Newtonsoft.Json; 
using Newtonsoft.Json.Linq; 
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using StackExchange.Redis;
using Nikan.Common.GlobalEnum;

namespace Nikan.Common
{
    public static class Redis
    {

        public static string Configuration { get; set; }


        /// <summary>
        /// فراخوانی کلید از بانک اطلاعاتی ردیس
        /// </summary>
        /// <param name="keyValue">مقدار کلید</param>
        /// <returns></returns>
        public static string GetValue(string keyValue)
        {
            string strResult = null;

            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration))
            {
                IDatabase db = redis.GetDatabase();
                try
                {
                    strResult = db.StringGet(keyValue);
                }
                catch (Exception ex)
                {

                }
            }
            return strResult;
        }


        /// <summary>
        /// فراخوانی کلید از بانک اطلاعاتی ردیس و بروزرسانی زمان زنده بودن آن
        /// </summary>
        /// <param name="keyValue">مقدار کلید</param>
        /// <param name="time">مدت زمان</param>
        /// <returns></returns>
        public static string GetValueAndKeepAlive(string keyValue, TimeSpan time)
        {
            string strResult = null;

            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration))
            {
                IDatabase db = redis.GetDatabase();
                strResult = db.StringGet(keyValue);

                if (!string.IsNullOrEmpty(strResult))
                {
                    db.KeyExpire(keyValue, time);
                }
            }
            return strResult;
        }

        /// <summary>
        /// فراخوانی کلید از بانک اطلاعاتی ردیس و به دست آوردن مقدار پراپرتی خاص در مقدار جی سان برگشتی
        /// </summary>
        /// <param name="keyValue">مقدار کلید</param>
        /// <param name="jsonProperty">مقدار json</param>
        /// <returns></returns>
        public static string GetValue(string keyValue, string jsonProperty)
        {
            string strResult = GetValue(keyValue);

            if (!string.IsNullOrEmpty(strResult))
            {
                var data = (JObject)JsonConvert.DeserializeObject(strResult);
                strResult = data[jsonProperty].Value<string>(); //"currency"
            }

            return strResult;
        }

        /// <summary>
        /// ثبت کلید در بانک اطلاعاتی ردیس
        /// </summary>
        /// <param name="key">نام کلید</param>
        /// <param name="value">مقدار</param>
        /// <returns></returns>
        public static string  SetValue(string key, string value, TimeSpan? time = null)
        {
            try
            {
                using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration))
                {
                    IDatabase db = redis.GetDatabase();
                    db.StringSet(key, value, time);
                }
            }
            catch (Exception er)
            {
                return er.Message;

            }
            return "ok";
        }

        public static void SetObject(string key, object value, TimeSpan? time = null)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration))
            {
                IDatabase db = redis.GetDatabase();

                var serializedUser = JsonConvert.SerializeObject(value);
                db.StringSet(key, serializedUser);
            }
        }


        public static TimeSpan? GetKeyTimeToLive(string keyValue)
        {
            TimeSpan? strResult = null;

            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration))
            {
                IDatabase db = redis.GetDatabase();
                strResult = db.KeyTimeToLive(keyValue);

            }
            return strResult;
        }

        /// <summary>
        /// بررسی اینکه کاربر معتبر است و البته نقش کاربر مهمان جداگانه بررسی می شود
        /// </summary>
        /// <param name="userSessionkey"></param>
        /// <param name="userRole"></param>
        /// <param name="isWebService">آیا نوع کاربر وب سرویس هست</param>
        /// <returns></returns>
        public static bool IsTokenExpired(string userSessionkey, string userRole, bool isWebService)
        {
            var blnResult = true;//توکن غیر معتبر هست
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration))
            {
                IDatabase db = redis.GetDatabase();
                TimeSpan? strResult = null;
                try
                {
                    //برای کلیدهایی که اعتبار ندارند ، نال بر می گردد
                    strResult = db.KeyTimeToLive(userSessionkey);
                }
                catch (Exception ex)
                {
                }
                blnResult = (strResult == null);

                if (blnResult == false || userRole ==  RoleEnum.GUEST.ToString()) //اگر معتبر بود یا کاربر مهمان بود
                {
                    blnResult = false;//کاربر مهمان را مطمین بشیم که معتبر است

                    if (strResult != null)
                    {
                        //برای کاربر مهمان با اینکه ثبت می کند که معتبر شود، چون کلید وجود ندارد، اطلاعات ندارد

                        //به مدت 30 دقیقه برای حالت پیش فرش
                        TimeSpan time;
                        if (isWebService)
                        {
                            //کاربر وب سرویس به مدت 24 ساعت
                            time = new TimeSpan(24, 0, 0);
                        }
                        else if (userRole ==  RoleEnum.GUEST.ToString())
                        {
                            //کاربر مهمان به مدت 10 ساعت
                            time = new TimeSpan(10, 0, 0);
                        }
                        else
                        {
                            //به مدت 120 دقیقه برای حالت پیش فرش
                            //1398/09/05 - درخواست آقای حق شناس از مدت 30 دقیقه به مدت 120 دقیقه
                            time = new TimeSpan(0, 120, 0);
                        }

                        db.KeyExpire(userSessionkey, time);
                    }
                }
            }
            return blnResult;
        }

        public static void SetKeyTimeToLive(string key, TimeSpan? time = null)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration))
            {
                IDatabase db = redis.GetDatabase();
                db.KeyExpire(key, time);
            }
        }

        public static void DeleteKey(string key)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration))
            {
                IDatabase db = redis.GetDatabase();
                db.KeyDelete(key);
            }
        }

        public static void DeleteKeyByPattern(string keyPattern)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration))
            {

                IDatabase db = redis.GetDatabase();
                var endpoints = redis.GetEndPoints();
                if (!keyPattern.EndsWith("*"))
                {
                    keyPattern += "*";
                }
                foreach (var item in endpoints)
                {
                    var server = redis.GetServer(item);
                    if (server != null)
                    {

                        var keys = server.Keys(pattern: keyPattern, pageSize: 10, flags: CommandFlags.DemandMaster);
                        var targetKeys = keys.Where(k => Regex.IsMatch(k, keyPattern)).ToList();
                        foreach (var key in keys)
                        {

                            redis.GetDatabase().KeyDelete(key);
                        }
                    }

                }
            }
        }


        /// <summary>
        /// Store a list of objects to redis. then the keys to all objects are stored in another array with the key defined by SetKey parameter.
        /// </summary>
        /// <param name="SetKey">The key to the list of keys</param>
        /// <param name="pairs">the list of keys and objects related to them</param>
        public static void StoreSets(string SetKey, Dictionary<string, string> pairs)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration))
            {
                IDatabase db = redis.GetDatabase();
                List<RedisValue> keys = new List<RedisValue>();
                List<KeyValuePair<RedisKey, RedisValue>> KeyValues = new List<KeyValuePair<RedisKey, RedisValue>>();

                int index = 1;
                foreach (var item in pairs)
                {
                    keys.Add(item.Key);
                    KeyValues.Add(new KeyValuePair<RedisKey, RedisValue>(item.Key, item.Value));
                    if (index % 100 == 0 || index == pairs.Count)
                    {
                        db.SetAdd(SetKey, keys.ToArray());
                        db.StringSet(KeyValues.ToArray());
                        keys.Clear();
                        KeyValues.Clear();
                    }

                    index++;
                }

            }
        }

        /// <summary>
        /// Retrieves all data stored by multiple keys. keys should have the format specified by ItemKeyPattern parameter.
        /// The list of keys are stored in a separate array with the key specified by SetKey.
        /// </summary>
        /// <param name="SetKey">The key to list of all item keys</param>
        /// <param name="ItemKeyPattern">The pattern of item keys. it's recommended to use  so  mething like aName_aNumber .
        /// Important: The number part should be zero padded to a fixed digit count (9 digits are recommended). it's crutial for string sort operation used in the function.</param>
        /// <returns>A list of json objects serialized to strings.</returns>
        public static string[] RetrieveSets(string SetKey, string ItemKeyPattern = "HotelSearchPack_*")
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(Configuration))
            {
                IDatabase db = redis.GetDatabase();


                //    var keys = db.SetMembers(SetKey);

                var length = db.SetLength(SetKey);

                List<string> results = new List<string>();
                int round = 0;

                for (int i = 0; i < length; i += 100)
                {
                    var vals = db.Sort(SetKey, by: ItemKeyPattern, skip: round * 100, take: 100, get: new RedisValue[] { "*" });
                    results.AddRange(vals.Select(x => x.ToString()));
                    round++;
                }



                return results.ToArray();
            }
        }






    }
}
