using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nikan.Common 
{

    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public ApiResultStatusCode StatusCode { get; set; }


        /// <summary>
        /// به هر دلیلی نال بود توی خروجی نمایش نده
        /// </summary>
      
        public string Messages { get; set; } 
       
        public List<ValidationError> Errors { get; set; }

        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, string message = null, List<ValidationError> messageList = null)
        {

            IsSuccess = isSuccess;
            StatusCode = statusCode;
            if (!string.IsNullOrWhiteSpace(message))
                this.Messages = message;
            else
                this.Messages = statusCode.ToDisplay();

            if (messageList != null)
            {
                this.Errors = messageList;

            }



        }


    }


    public class ApiResult<TData> : ApiResult
        where TData : class
    {
       
        public TData Data { get; set; }

        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, TData data, string message = null)
            : base(isSuccess, statusCode, message)
        {
            Data = data;
        }


    }

    public class ValidationError
    {
       
        public string Field { get; set; }
        public string Message { get; set; }

        public string AdminMessage { get; set; }
        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }
        public ValidationError()
        {

        }
    }
    public enum ApiResultStatusCode
    {
        [Display(Name = "عملیات با موفقیت انجام شد")]
        Success = 0,

        [Display(Name = "خطایی در سرور رخ داده است")]
        ServerError = 1,

        [Display(Name = "پارامتر های ارسالی معتبر نیستند")]
        BadRequest = 2,

        [Display(Name = "یافت نشد")]
        NotFound = 3,

        [Display(Name = "لیست خالی است")]
        ListEmpty = 4,

        [Display(Name = "خطایی در پردازش رخ داد")]
        LogicError = 5,

        [Display(Name = "خطای احراز هویت")]
        UnAuthorized = 6,

        [Display(Name = "عدم دسترسی")]
        NotAllowed =7


    }

}
