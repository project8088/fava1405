using Microsoft.AspNetCore.Http;
using Nikan.Common;
using ServiceReference1;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Xml;

namespace Nikan.MellatApp
{
    public class BankMellatImplement
    {
        //https://bpm.shaparak.ir/pgwchannel/services/pgw?wsdl

        #region Base Variable Definition

        public string PgwSite = "https://bpm.shaparak.ir/pgwchannel/startpay.mellat";
        public string  url = "https://bpm.shaparak.ir/pgwchannel/services/pgw?wsdl";
        static readonly string _callBackUrl = "http://isfahangit.com";
        private static readonly string _action = "/Pay/Index";
        //static readonly string userName = "bozorg1399";
        //static readonly string password = "64555119";
        //static readonly long terminalId = 5627530;
        //static readonly long payerId = 555531;//شناسه پرداخت
       

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        //private readonly IOptions<MessagesOptions> _messagesOptions;
        public BankMellatImplement(  IHttpContextAccessor httpContextAccessor ,
            IHttpClientFactory httpClientFactory ) 
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpContextAccessor = httpContextAccessor;
            
        }

        string localDate = string.Empty;
        string localTime = string.Empty;
        #endregion

        private string GetDate()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                   DateTime.Now.Day.ToString().PadLeft(2, '0');
        }
        private string GetTime()
        {
            return DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                   DateTime.Now.Second.ToString().PadLeft(2, '0');
        }

        /// <summary>
        /// شناسه مشتری
        /// شناسه بانکی مشتری
        /// </summary>
        public long BankCustomerId { get; set; }

        /// <summary>
        /// نام کاربری
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// کلمه عبور
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        /// شماره ترمینال بانکی
        /// </summary>
        public long TerminalId { get; set; }



        public string  ErrorMessage { get; set; }



        public int BankPaymentMethod { get; set; }
        

        public BankMellatImplement(long terminalId, string userName,
            string password, long payerId, int bankPaymentMethod)
        {
            try
            {
                BankCustomerId = payerId;
                UserName = userName;
                Password = password;
                TerminalId = terminalId;
                BankPaymentMethod = bankPaymentMethod;
                localDate = "20161218";// DateTime.Now.ToString("yyyyMMdd");
                localTime = DateTime.Now.ToString("HHMMSS");
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
        public   PaymentRequestResult  bpPayRequest(long orderId, long priceAmount,
            string additionalText,  string callBackUrl =null  )
        {
             
            var res = new PaymentRequestResult();
             
            try
            {
                if (string.IsNullOrWhiteSpace(callBackUrl))
                    callBackUrl = _callBackUrl + _action;

                var webServiceResponse = CallbpPayRequest(orderId, priceAmount, callBackUrl, additionalText, TerminalId
                    , UserName, Password, BankCustomerId); 
                var result = XmlHelper.GetNodeValueFromXml(webServiceResponse, "return"); 
                var arrayResult = result.Split(','); 
                res.ResultCode = arrayResult[0];
                res.RefId = arrayResult.Length > 1 ? arrayResult[1] : string.Empty;
                res.Result=  DesribtionStatusCode(int.Parse(arrayResult[0])).Replace("_", " ");

            }
            catch (Exception error)
            {
                res.ResultCode = "-1";
                res.Result = "خطایی رخ داده است";
            }


            return res;


        }


      
        public PaymentRequestResult bpInquiryRequest(long orderId,
         long saleOrderId,
         long saleReferenceId,
         long terminalId, string userName, string password)
        {

            var res = new PaymentRequestResult();

            try
            {
                

                var webServiceResponse = CallbpInquiryRequest(orderId,
                    saleOrderId, saleReferenceId, terminalId
                    , userName, password);


                var result = XmlHelper.GetNodeValueFromXml(webServiceResponse, "return");
                var arrayResult = result.Split(',');
                res.ResultCode = arrayResult[0];
                res.RefId = arrayResult.Length > 1 ? arrayResult[1] : string.Empty;
                res.Result = DesribtionStatusCode(int.Parse(arrayResult[0])).Replace("_", " ");

            }
            catch (Exception error)
            {
                res.ResultCode = "-1";
                res.Result = "خطایی رخ داده است";
            }


            return res; 
        }

        public PaymentRequestResult bpVerifyRequest(long orderId, long saleOrderId, long saleReferenceId  )
        {

            var res = new PaymentRequestResult();

            try
            { 

                var webServiceResponse = CallbpVerifyRequest(orderId, saleOrderId, saleReferenceId, TerminalId, UserName, Password);
                var result = XmlHelper.GetNodeValueFromXml(webServiceResponse, "return");
                var arrayResult = result.Split(',');
                res.ResultCode = arrayResult[0];
                res.RefId = arrayResult.Length > 1 ? arrayResult[1] : string.Empty;
                res.Result = DesribtionStatusCode(int.Parse(arrayResult[0])).Replace("_", " ");

            }
            catch (Exception error)
            {
                res.ResultCode = "-1";
                res.Result = "خطایی رخ داده است";
            }


            return res;


        }

        public PaymentRequestResult bpSettleData(long orderId, long saleOrderId )
        {

            var res = new PaymentRequestResult();

            try
            {


                var webServiceResponse = CallbpSettleData(orderId, saleOrderId ,TerminalId, UserName, Password);
                var result = XmlHelper.GetNodeValueFromXml(webServiceResponse, "return");
                var arrayResult = result.Split(',');
                res.ResultCode = arrayResult[0];
                res.RefId = arrayResult.Length > 1 ? arrayResult[1] : string.Empty;
                res.Result = DesribtionStatusCode(int.Parse(arrayResult[0])).Replace("_", " ");

            }
            catch (Exception error)
            {
                res.ResultCode = "-1";
                res.Result = "خطایی رخ داده است";
            }


            return res;


        }



        public string CallbpPayRequest(long orderId, long priceAmount, string callBackUrl,string note,
            long terminalId, string userName, string password, long payerId)
        {
          
            var _action = "";
            var result = ""; 
            XmlDocument soapEnvelopeXml = Create_bpPayRequest(orderId, priceAmount, callBackUrl, note, terminalId, userName
                , password, payerId
                ); 
            HttpWebRequest webRequest = CreateWebRequest(url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    result = rd.ReadToEnd();
                }
            }



            return result;
        }

        public string CallbpInquiryRequest(long orderId,
         long saleOrderId,
         long saleReferenceId,
         long terminalId, string userName, string password)
        {

            var _action = "";
            var result = "";
            XmlDocument soapEnvelopeXml = Create_bpInquiryRequest(orderId,
              saleOrderId, saleReferenceId,  terminalId, userName
                , password 
                );
            HttpWebRequest webRequest = CreateWebRequest(url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    result = rd.ReadToEnd();
                }
            }



            return result;
        }



        public string CallbpVerifyRequest(long orderId, long saleOrderId, long saleReferenceId,
            long terminalId, string userName, string password )
        {
            
            var _action = "";
            var result = ""; 
            XmlDocument soapEnvelopeXml = Create_bpVerifyRequest(orderId, saleOrderId, saleReferenceId, terminalId, userName, password);

            HttpWebRequest webRequest = CreateWebRequest(url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    result = rd.ReadToEnd();
                }
            }



            return result;
        }
        public string CallbpSettleData(long orderId,  long saleReferenceId, long terminalId, string userName, string password)
        {
            
            var _action = "";
            var result = "";

            XmlDocument soapEnvelopeXml = Create_SettleData(orderId, saleReferenceId,terminalId, userName, password); 
            HttpWebRequest webRequest = CreateWebRequest(url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    result = rd.ReadToEnd();
                }
            }



            return result;
        }


        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }


        private static XmlDocument Create_bpPayRequest(long orderId,
            long priceAmount,string callBackUrl,string note,
            long terminalId, string userName, string password ,long payerId 
            )
        {
            var str=
                "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:int=\"http://interfaces.core.sw.bps.com/\">" +
                "<soapenv:Header/>" +
                "<soapenv:Body>" +
                "<int:bpPayRequest>" +
                $"<terminalId>{terminalId}</terminalId>" +
                "<!--Optional:-->" +
                $"<userName>{userName}</userName>" +
                "<!--Optional:-->" +
                $"<userPassword>{password}</userPassword>" +
                $"<orderId>{orderId}</orderId>" +
                $"<amount>{priceAmount}</amount>" +
                "<!--Optional:-->" +
                $"<localDate>{DateTime.Now:yyyyMMdd}</localDate>" +
                "<!--Optional:-->" +
                $"<localTime>{DateTime.Now:HHmmss}</localTime>" +
                "<!--Optional:-->" +
                $"<additionalData>{note}</additionalData>" +
                "<!--Optional:-->" +
                $"<callBackUrl>{XmlHelper.EncodeXmlValue(callBackUrl)}</callBackUrl>" +
                $"<payerId>{payerId}</payerId>" +
                "'</int:bpPayRequest>" +
                "</soapenv:Body>" +
                "</soapenv:Envelope>";



            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(str);
            return soapEnvelopeDocument;


        }







        private static XmlDocument Create_bpInquiryRequest(long orderId,
         long saleOrderId,
         long saleReferenceId,
         long terminalId, string userName, string password 
         )
        {
            var str =
                "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:int=\"http://interfaces.core.sw.bps.com/\">" +
                "<soapenv:Header/>" +
                "<soapenv:Body>" +
                "<int:bpInquiryRequest>" +
                $"<terminalId>{terminalId}</terminalId>" +
                "<!--Optional:-->" +
                $"<userName>{userName}</userName>" +
                "<!--Optional:-->" +
                $"<userPassword>{password}</userPassword>" +
                $"<orderId>{orderId}</orderId>" +
                $"<saleOrderId>{saleOrderId}</saleOrderId>" +
                "<!--Optional:-->" +
               $"<saleReferenceId>{saleReferenceId}</saleReferenceId>" +
                "'</int:bpInquiryRequest>" + 
                "</soapenv:Body>" +
                "</soapenv:Envelope>"; 

            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(str);
            return soapEnvelopeDocument;


        }







        public static XmlDocument Create_bpVerifyRequest( long orderId, long saleOrderId, long saleReferenceId,
            long terminalId, string userName, string password )
        {
            var res=
                "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:int=\"http://interfaces.core.sw.bps.com/\">" +
                "<soapenv:Header/>" +
                "<soapenv:Body>" +
                "<int:bpVerifyRequest>" +
                $"<terminalId>{terminalId}</terminalId>" +
                "<!--Optional:-->" +
                $"<userName>{userName}</userName>" +
                "<!--Optional:-->" +
                $"<userPassword>{password}</userPassword>" +
                $"<orderId>{orderId}</orderId>" +
                $"<saleOrderId>{saleOrderId}</saleOrderId>" +
                $"<saleReferenceId>{saleReferenceId}</saleReferenceId>" +
                "</int:bpVerifyRequest>" +
                "</soapenv:Body>" +
                "</soapenv:Envelope>";


            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(res);
            return soapEnvelopeDocument;
             

        }

        public static XmlDocument Create_SettleData(long orderId,long saleReferenceId,
            long terminalId, string userName, string password )
        {
            var res =
               "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:int=\"http://interfaces.core.sw.bps.com/\">" +
                "<soapenv:Header/>" +
                "<soapenv:Body>" +
                "<int:bpSettleRequest>" +
                $"<terminalId>{terminalId}</terminalId>" +
                "<!--Optional:-->" +
                $"<userName>{userName}</userName>" +
                "<!--Optional:-->" +
                $"<userPassword>{password}</userPassword>" +
                $"<orderId>{orderId}</orderId>" +
                $"<saleOrderId>{orderId}</saleOrderId>" +
                $"<saleReferenceId>{saleReferenceId}</saleReferenceId>" +
                "</int:bpSettleRequest>" +
                "</soapenv:Body>" +
                "</soapenv:Envelope>";

            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(res);
            return soapEnvelopeDocument;
        }


 




        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

      




        #region Regund WebService
        public string CallRefundWebService(long refNo)
        {
            var _url = "http://bos.bpm.bankmellat.ir/backoffice/Services/bpm/TransactionService.asmx?wsdl";
            var _action = "";
            var result = "-1";
            var error = "";
            var date = "";

            try
            {
                //XmlDocument soapEnvelopeXml = CreateSoapForGetCardNumber(refNo);

                var currentday = DateTime.Now;

                  date = currentday.Year + "-" + ((currentday.Month < 10) ? ("0" + currentday.Month) : currentday.Month.ToString()) + "-" + ((currentday.Day < 10) ? "0" + currentday.Day : currentday.Day.ToString());

                string temp = String.Format(@"<soap:Envelope xmlns:bpm='http://bpmellat.co/' xmlns:soap='http://www.w3.org/2003/05/soap-envelope'>
              <soap:Header>
               <wsse:Security soap:mustUnderstand ='true' xmlns:wsse = 'http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' xmlns:wsu = 'http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>
                  <wsu:Timestamp wsu:Id='TS-6565CBED8C192EA18915941056606412'>
                      <wsu:Created>{1}T07:07:40.640Z</wsu:Created>
                       <wsu:Expires>{1}T07:08:40.640Z</wsu:Expires>
                  </wsu:Timestamp>
                  <wsse:UsernameToken wsu:Id ='UsernameToken-6565CBED8C192EA18915941056472471'>
                  <wsse:Username>esfa67</wsse:Username>
                  <wsse:Password Type ='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'>39258951</wsse:Password>
                  <wsse:Nonce EncodingType='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary'>kmisB+JazEH61IEQW6pcTw==</wsse:Nonce>
                 
              </wsse:UsernameToken>
             </wsse:Security>
           </soap:Header>
         <soap:Body>
                 <bpm:getTransactionByIdFromArchive>
                      <bpm:referenceId>{0}</bpm:referenceId>
                  </bpm:getTransactionByIdFromArchive>
         </soap:Body>
       </soap:Envelope>", refNo, date);
                XmlDocument soapEnvelopeDocument = new XmlDocument();
                soapEnvelopeDocument.LoadXml(temp);

                ErrorMessage = temp;

                 HttpWebRequest webRequest = CreateWebRequest(_url, _action);
                // InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeDocument, webRequest);
                // begin async call to web request.
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
                // suspend this thread until call is complete. You might want to
                // do something usefull here like update your UI.
                asyncResult.AsyncWaitHandle.WaitOne();

                // get the response from the completed web request.

                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        result = rd.ReadToEnd();
                    }
                }
            }
            catch (Exception er)
            {
                return "date:"+ date + " ------------"+  er.Message;
            }
            return result;
        }
      
        private static XmlDocument CreateSoapForGetCardNumber2(long refNo)
        {

          


            var currentday = DateTime.Now;
            var date = currentday.Year + "-" + ((currentday.Month < 10) ? ("0" + currentday.Month) : currentday.Month.ToString()) + "-" + ((currentday.Day < 10) ? "0" + currentday.Day : currentday.Day.ToString());
            //2021-10-23
            string temp = String.Format(@"<soap:Envelope xmlns:bpm='http://bpmellat.co/' xmlns:soap='http://www.w3.org/2003/05/soap-envelope'>
   <soap:Header>
      <wsse:Security soap:mustUnderstand='true' xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>
         <wsse:UsernameToken wsu:Id='UsernameToken-035FAC5C48AEECA5EF16389487192122'>
            <wsse:Username>esfa67</wsse:Username>
            <wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'>39258951</wsse:Password>
            <wsse:Nonce EncodingType='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary'>zGdGQBMeN9PDpGMImEBBuw==</wsse:Nonce>
            <wsu:Created>2021-12-08T07:31:59.212Z</wsu:Created>
         </wsse:UsernameToken>
         <wsu:Timestamp wsu:Id='TS-035FAC5C48AEECA5EF16389485840461'>
            <wsu:Created>2021-12-08T07:29:44.038Z</wsu:Created>
            <wsu:Expires>2021-12-15T00:09:44.038Z</wsu:Expires>
         </wsu:Timestamp>
      </wsse:Security>
   </soap:Header>
   <soap:Body>
      <bpm:getTransactionByIdFromArchive>
         <bpm:referenceId>{0}</bpm:referenceId>
      </bpm:getTransactionByIdFromArchive>
   </soap:Body>
</soap:Envelope>", refNo);




   









            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(temp);
            return soapEnvelopeDocument;
        }

        private static XmlDocument CreateSoapForGetCardNumber(long refNo)
        {
            var currentday = DateTime.Now;

            var date = currentday.Year + "-" + ((currentday.Month < 10) ? ("0" + currentday.Month) : currentday.Month.ToString()) + "-" + ((currentday.Day < 10) ? "0" + currentday.Day : currentday.Day.ToString());

            string temp = String.Format(@"<soap:Envelope xmlns:bpm='http://bpmellat.co/' xmlns:soap='http://www.w3.org/2003/05/soap-envelope'>
              <soap:Header>
               <wsse:Security soap:mustUnderstand ='true' xmlns:wsse = 'http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' xmlns:wsu = 'http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>
                  <wsu:Timestamp wsu:Id='TS-6565CBED8C192EA18915941056606412'>
                      <wsu:Created>{1}T07:07:40.640Z</wsu:Created>
                       <wsu:Expires>{1}T07:08:40.640Z</wsu:Expires>
                  </wsu:Timestamp>
                  <wsse:UsernameToken wsu:Id ='UsernameToken-6565CBED8C192EA18915941056472471'>
                  <wsse:Username>esfa67</wsse:Username>
                  <wsse:Password Type ='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'>39258951</wsse:Password>
                  <wsse:Nonce EncodingType='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary'>kmisB+JazEH61IEQW6pcTw==</wsse:Nonce>
                 
              </wsse:UsernameToken>
             </wsse:Security>
           </soap:Header>
         <soap:Body>
                 <bpm:getTransactionById>
                      <bpm:referenceId>{0}</bpm:referenceId>
                  </bpm:getTransactionById>
         </soap:Body>
       </soap:Envelope>", refNo, date);
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(temp);
            return soapEnvelopeDocument;
        }




        private static XmlDocument CreateSoapForCheckRefund(long refNo)
        {
            var currentday = DateTime.Now;

            var date = currentday.Year + "-" + ((currentday.Month < 10) ? ("0" + currentday.Month) : currentday.Month.ToString()) + "-" + ((currentday.Day < 10) ? "0" + currentday.Day : currentday.Day.ToString());

            string temp = string.Format(@"<soap:Envelope xmlns:bpm='http://bpmellat.co/' xmlns:soap='http://www.w3.org/2003/05/soap-envelope'>
    <soap:Header>
        <wsse:Security soap:mustUnderstand='true' xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd' xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>
            <wsu:Timestamp wsu:Id='TS-AB977B37DD7F123F9C16060625834954'>
                <wsu:Created>2020-11-22T16:29:43.495Z</wsu:Created>
                <wsu:Expires>2020-11-22T16:30:43.495Z</wsu:Expires>
            </wsu:Timestamp>
            <wsse:UsernameToken wsu:Id='UsernameToken-AB977B37DD7F123F9C16060625809713'>
                <wsse:Username>esfa67</wsse:Username>
                <wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'>78782088</wsse:Password>
                <wsse:Nonce EncodingType='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary'>ALXGo7shmXJhX8jVSDbJ8A==</wsse:Nonce>
                <wsu:Created>2020-11-22T16:29:40.971Z</wsu:Created>
            </wsse:UsernameToken>
        </wsse:Security>
    </soap:Header>
    <soap:Body>
        <bpm:getTransactionById>
            <bpm:referenceId>{0}</bpm:referenceId>
        </bpm:getTransactionById>
    </soap:Body>
</soap:Envelope>", refNo, date);
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(temp);
            return soapEnvelopeDocument;
        }

        public PaymentRequestResult bpRefundRequest(long orderId, long saleReferenceId,
         long refundAmount, long terminalId, string userName, string password)
        {

            var res = new PaymentRequestResult();

            try
            {


                var webServiceResponse = CallbpRefundRequest(orderId, saleReferenceId, refundAmount,
                 terminalId,   UserName, Password);

                var result = XmlHelper.GetNodeValueFromXml(webServiceResponse, "return");
                var arrayResult = result.Split(',');
                res.ResultCode = arrayResult[0];
                res.RefId = arrayResult.Length > 1 ? arrayResult[1] : string.Empty;
                res.Result = DesribtionStatusCode(int.Parse(arrayResult[0])).Replace("_", " ");

            }
            catch (Exception error)
            {
                res.ResultCode = "-1";
                res.Result = "خطایی رخ داده است";
            }


            return res;


        }


        public string CallbpRefundRequest(long orderId, long saleReferenceId,
         long refundAmount, long terminalId,
         string userName, string password)
        {

            var _action = "";
            var result = "";
            XmlDocument soapEnvelopeXml = Create_RefundRequest(orderId, saleReferenceId, refundAmount,
                terminalId, userName
                , password  
                );
            HttpWebRequest webRequest = CreateWebRequest(url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    result = rd.ReadToEnd();
                }
            }



            return result;
        }

        public static XmlDocument Create_RefundRequest(long orderId, long saleReferenceId,
         long refundAmount, long terminalId, string userName, string password)
        {
            var res =
               "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:int=\"http://interfaces.core.sw.bps.com/\">" +
                "<soapenv:Header/>" +
                "<soapenv:Body>" +
                "<int:bpRefundRequest>" +
                $"<terminalId>{terminalId}</terminalId>" +
                "<!--Optional:-->" +
                $"<userName>{userName}</userName>" +
                "<!--Optional:-->" +
                $"<userPassword>{password}</userPassword>" +
                $"<orderId>{orderId}</orderId>" +
                $"<saleOrderId>{orderId}</saleOrderId>" +
                $"<saleReferenceId>{saleReferenceId}</saleReferenceId>" +
                $"<refundAmount>{refundAmount}</refundAmount>" +
                "</int:bpRefundRequest>" +
                "</soapenv:Body>" +
                "</soapenv:Envelope>";

            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(res);
            return soapEnvelopeDocument;
        }



        #endregion




















        public enum MellatBankReturnCode
        {
            ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ = 0,
            ﺷﻤﺎره_ﻛﺎرت_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 11,
            ﻣﻮﺟﻮدي_ﻛﺎﻓﻲ_ﻧﻴﺴﺖ = 12,
            رﻣﺰ_ﻧﺎدرﺳﺖ_اﺳﺖ = 13,
            ﺗﻌﺪاد_دﻓﻌﺎت_وارد_ﻛﺮدن_رﻣﺰ_ﺑﻴﺶ_از_ﺣﺪ_ﻣﺠﺎز_اﺳﺖ = 14,
            ﻛﺎرت_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 15,
            دﻓﻌﺎت_ﺑﺮداﺷﺖ_وﺟﻪ_ﺑﻴﺶ_از_ﺣﺪ_ﻣﺠﺎز_اﺳﺖ = 16,
            ﻛﺎرﺑﺮ_از_اﻧﺠﺎم_ﺗﺮاﻛﻨﺶ_ﻣﻨﺼﺮف_ﺷﺪه_اﺳﺖ = 17,
            ﺗﺎرﻳﺦ_اﻧﻘﻀﺎي_ﻛﺎرت_ﮔﺬﺷﺘﻪ_اﺳﺖ = 18,
            ﻣﺒﻠﻎ_ﺑﺮداﺷﺖ_وﺟﻪ_ﺑﻴﺶ_از_ﺣﺪ_ﻣﺠﺎز_اﺳﺖ = 19,


            ﺻﺎدر_ﻛﻨﻨﺪه_ﻛﺎرت_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 111,
            ﺧﻄﺎي_ﺳﻮﻳﻴﭻ_ﺻﺎدر_ﻛﻨﻨﺪه_ﻛﺎرت = 112,
            ﭘﺎﺳﺨﻲ_از_ﺻﺎدر_ﻛﻨﻨﺪه_ﻛﺎرت_درﻳﺎﻓﺖ_ﻧﺸﺪ = 113,
            دارﻧﺪه_ﻛﺎرت_ﻣﺠﺎز_ﺑﻪ_اﻧﺠﺎم_اﻳﻦ_ﺗﺮاﻛﻨﺶ_ﻧﻴﺴﺖ = 114,


            ﭘﺬﻳﺮﻧﺪه_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 21,
            ﺧﻄﺎي_اﻣﻨﻴﺘﻲ_رخ_داده_اﺳﺖ = 23,
            اﻃﻼﻋﺎت_ﻛﺎرﺑﺮي_ﭘﺬﻳﺮﻧﺪه_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 24,
            ﻣﺒﻠﻎ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 25,
            ﭘﺎﺳﺦ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 31,
            ﻓﺮﻣﺖ_اﻃﻼﻋﺎت_وارد_ﺷﺪه_ﺻﺤﻴﺢ_ﻧﻤﻲ_ﺑﺎﺷﺪ = 32,
            ﺣﺴﺎب_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 33,
            ﺧﻄﺎي_ﺳﻴﺴﺘﻤﻲ = 34,
            ﺗﺎرﻳﺦ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 35,
            ﺷﻤﺎره_درﺧﻮاﺳﺖ_ﺗﻜﺮاري_اﺳﺖ = 41,
            ﺗﺮاﻛﻨﺶ_Sale_یافت_نشد_ = 42,
            ﻗﺒﻼ_Verify_درﺧﻮاﺳﺖ_داده_ﺷﺪه_اﺳﺖ = 43,
            درخواست_verify_یافت_نشد = 44,
            ﺗﺮاﻛﻨﺶ_Settle_ﺷﺪه_اﺳﺖ = 45,
            ﺗﺮاﻛﻨﺶ_Settle_نشده_اﺳﺖ = 46,
            ﺗﺮاﻛﻨﺶ_Settle_یافت_نشد = 47,
            تراکنش_Reverse_شده_است = 48,
            تراکنش_Refund_یافت_نشد = 49,


            شناسه_قبض_نادرست_است = 412,
            ﺷﻨﺎﺳﻪ_ﭘﺮداﺧﺖ_ﻧﺎدرﺳﺖ_اﺳﺖ = 413,
            سازﻣﺎن_ﺻﺎدر_ﻛﻨﻨﺪه_ﻗﺒﺾ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 414,
            زﻣﺎن_ﺟﻠﺴﻪ_ﻛﺎري_ﺑﻪ_ﭘﺎﻳﺎن_رسیده_است = 415,
            ﺧﻄﺎ_در_ﺛﺒﺖ_اﻃﻼﻋﺎت = 416,
            ﺷﻨﺎﺳﻪ_ﭘﺮداﺧﺖ_ﻛﻨﻨﺪه_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 417,
            اﺷﻜﺎل_در_ﺗﻌﺮﻳﻒ_اﻃﻼﻋﺎت_ﻣﺸﺘﺮي = 418,
            ﺗﻌﺪاد_دﻓﻌﺎت_ورود_اﻃﻼﻋﺎت_از_ﺣﺪ_ﻣﺠﺎز_ﮔﺬﺷﺘﻪ_اﺳﺖ = 419,
            IP_نامعتبر_است = 421,

            ﺗﺮاﻛﻨﺶ_ﺗﻜﺮاري_اﺳﺖ = 51,
            ﺗﺮاﻛﻨﺶ_ﻣﺮﺟﻊ_ﻣﻮﺟﻮد_ﻧﻴﺴﺖ = 54,
            ﺗﺮاﻛﻨﺶ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ = 55,
            ﺧﻄﺎ_در_واریز = 61
        }
        public string DesribtionStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 0:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺑﺎ_ﻣﻮﻓﻘﻴﺖ_اﻧﺠﺎم_ﺷﺪ.ToString();
                case 11:
                    return MellatBankReturnCode.ﺷﻤﺎره_ﻛﺎرت_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 12:
                    return MellatBankReturnCode.ﻣﻮﺟﻮدي_ﻛﺎﻓﻲ_ﻧﻴﺴﺖ.ToString();
                case 13:
                    return MellatBankReturnCode.رﻣﺰ_ﻧﺎدرﺳﺖ_اﺳﺖ.ToString();
                case 14:
                    return MellatBankReturnCode.ﺗﻌﺪاد_دﻓﻌﺎت_وارد_ﻛﺮدن_رﻣﺰ_ﺑﻴﺶ_از_ﺣﺪ_ﻣﺠﺎز_اﺳﺖ.ToString();
                case 15:
                    return MellatBankReturnCode.ﻛﺎرت_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 16:
                    return MellatBankReturnCode.دﻓﻌﺎت_ﺑﺮداﺷﺖ_وﺟﻪ_ﺑﻴﺶ_از_ﺣﺪ_ﻣﺠﺎز_اﺳﺖ.ToString();
                case 17:
                    return MellatBankReturnCode.ﻛﺎرﺑﺮ_از_اﻧﺠﺎم_ﺗﺮاﻛﻨﺶ_ﻣﻨﺼﺮف_ﺷﺪه_اﺳﺖ.ToString();
                case 18:
                    return MellatBankReturnCode.ﺗﺎرﻳﺦ_اﻧﻘﻀﺎي_ﻛﺎرت_ﮔﺬﺷﺘﻪ_اﺳﺖ.ToString();
                case 19:
                    return MellatBankReturnCode.ﻣﺒﻠﻎ_ﺑﺮداﺷﺖ_وﺟﻪ_ﺑﻴﺶ_از_ﺣﺪ_ﻣﺠﺎز_اﺳﺖ.ToString();
                case 111:
                    return MellatBankReturnCode.ﺻﺎدر_ﻛﻨﻨﺪه_ﻛﺎرت_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 112:
                    return MellatBankReturnCode.ﺧﻄﺎي_ﺳﻮﻳﻴﭻ_ﺻﺎدر_ﻛﻨﻨﺪه_ﻛﺎرت.ToString();
                case 113:
                    return MellatBankReturnCode.ﭘﺎﺳﺨﻲ_از_ﺻﺎدر_ﻛﻨﻨﺪه_ﻛﺎرت_درﻳﺎﻓﺖ_ﻧﺸﺪ.ToString();
                case 114:
                    return MellatBankReturnCode.دارﻧﺪه_ﻛﺎرت_ﻣﺠﺎز_ﺑﻪ_اﻧﺠﺎم_اﻳﻦ_ﺗﺮاﻛﻨﺶ_ﻧﻴﺴﺖ.ToString();
                case 21:
                    return MellatBankReturnCode.ﭘﺬﻳﺮﻧﺪه_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 23:
                    return MellatBankReturnCode.ﺧﻄﺎي_اﻣﻨﻴﺘﻲ_رخ_داده_اﺳﺖ.ToString();
                case 24:
                    return MellatBankReturnCode.اﻃﻼﻋﺎت_ﻛﺎرﺑﺮي_ﭘﺬﻳﺮﻧﺪه_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 25:
                    return MellatBankReturnCode.ﻣﺒﻠﻎ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 31:
                    return MellatBankReturnCode.ﭘﺎﺳﺦ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 32:
                    return MellatBankReturnCode.ﻓﺮﻣﺖ_اﻃﻼﻋﺎت_وارد_ﺷﺪه_ﺻﺤﻴﺢ_ﻧﻤﻲ_ﺑﺎﺷﺪ.ToString();
                case 33:
                    return MellatBankReturnCode.ﺣﺴﺎب_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 34:
                    return MellatBankReturnCode.ﺧﻄﺎي_ﺳﻴﺴﺘﻤﻲ.ToString();
                case 35:
                    return MellatBankReturnCode.ﺗﺎرﻳﺦ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 41:
                    return MellatBankReturnCode.ﺷﻤﺎره_درﺧﻮاﺳﺖ_ﺗﻜﺮاري_اﺳﺖ.ToString();
                case 42:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_Sale_یافت_نشد_.ToString();
                case 43:
                    return MellatBankReturnCode.ﻗﺒﻼ_Verify_درﺧﻮاﺳﺖ_داده_ﺷﺪه_اﺳﺖ.ToString();



                case 44:
                    return MellatBankReturnCode.درخواست_verify_یافت_نشد.ToString();
                case 45:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_Settle_ﺷﺪه_اﺳﺖ.ToString();
                case 46:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_Settle_نشده_اﺳﺖ.ToString();

                case 47:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_Settle_یافت_نشد.ToString();
                case 48:
                    return MellatBankReturnCode.تراکنش_Reverse_شده_است.ToString();
                case 49:
                    return MellatBankReturnCode.تراکنش_Refund_یافت_نشد.ToString();
                case 412:
                    return MellatBankReturnCode.شناسه_قبض_نادرست_است.ToString();
                case 413:
                    return MellatBankReturnCode.ﺷﻨﺎﺳﻪ_ﭘﺮداﺧﺖ_ﻧﺎدرﺳﺖ_اﺳﺖ.ToString();
                case 414:
                    return MellatBankReturnCode.سازﻣﺎن_ﺻﺎدر_ﻛﻨﻨﺪه_ﻗﺒﺾ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 415:
                    return MellatBankReturnCode.زﻣﺎن_ﺟﻠﺴﻪ_ﻛﺎري_ﺑﻪ_ﭘﺎﻳﺎن_رسیده_است.ToString();
                case 416:
                    return MellatBankReturnCode.ﺧﻄﺎ_در_ﺛﺒﺖ_اﻃﻼﻋﺎت.ToString();
                case 417:
                    return MellatBankReturnCode.ﺷﻨﺎﺳﻪ_ﭘﺮداﺧﺖ_ﻛﻨﻨﺪه_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 418:
                    return MellatBankReturnCode.اﺷﻜﺎل_در_ﺗﻌﺮﻳﻒ_اﻃﻼﻋﺎت_ﻣﺸﺘﺮي.ToString();
                case 419:
                    return MellatBankReturnCode.ﺗﻌﺪاد_دﻓﻌﺎت_ورود_اﻃﻼﻋﺎت_از_ﺣﺪ_ﻣﺠﺎز_ﮔﺬﺷﺘﻪ_اﺳﺖ.ToString();
                case 421:
                    return MellatBankReturnCode.IP_نامعتبر_است.ToString();

                case 51:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﺗﻜﺮاري_اﺳﺖ.ToString();
                case 54:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﻣﺮﺟﻊ_ﻣﻮﺟﻮد_ﻧﻴﺴﺖ.ToString();
                case 55:
                    return MellatBankReturnCode.ﺗﺮاﻛﻨﺶ_ﻧﺎﻣﻌﺘﺒﺮ_اﺳﺖ.ToString();
                case 61:
                    return MellatBankReturnCode.ﺧﻄﺎ_در_واریز.ToString();

            }
            return "";
        }
  
    
    }

    public class PaymentRequestResult
    {

        public string ResultCode { get; set; }
        public string RefId { get; set; }
        public string Result { get; set; }



    }



}
