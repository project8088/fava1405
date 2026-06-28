 
 
 
using System.Collections.Generic;
using System.Linq;


namespace Nikan.Common
{
    public enum ErrorType
    {
        ModelStateError = 0,
        ServerError = 1,
        NotFound = 2,
        CanNotDelete = 3,
        MessageShowDialog = 4,
        MessageShowNotification = 5,
        NotAllowChange = 6,
        NotAllowAccess = 7,
    }

    public class GlobalError
    {
        public GlobalError()
        {

        }

        public GlobalError(string title, string message, string key, ErrorType type)
        {
            this.title = title;
            this.message = message;
            errorkey = key;
            errortype = type;

        }
        public string title { get; set; }
        public string message { get; set; }
        public string errorkey { get; set; }
        public ErrorType errortype { get; set; }

    }

    public class SuccessMessage
    {

        public SuccessMessage(string title, string message, string url = "", bool reload = false)
        {
            this.title = title;
            this.message = message;
            this.redirecturl = url;
            this.reload = reload;
        }

        public string title { get; set; }

        public string message { get; set; }

        public string redirecturl { get; set; }


        public bool reload { get; set; }

    }

    public class CommandResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public List<GlobalError> Errors { get; set; }


    }

    public class CommandResult<TData> : CommandResult
    {
        public CommandResult()
        {

        }
        public CommandResult(bool isSuccess, TData data, List<GlobalError> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
            Data = data;
        }
        public TData Data { get; set; }
    }

}
