using System.Net;
using System.Web.Http;

namespace AuthenticationUI.Results
{
    public class FormResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public object Data { get; set; }

        public FormResult(bool success, string message, object data = null)
        {
            IsSuccess = success;
            Message = message;
            Data = data;
        }
    }
}
