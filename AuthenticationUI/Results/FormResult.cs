using System.Net;
using System.Web.Http;

namespace AuthenticationUI.Results
{
    public class FormResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public FormResult(bool success, string message)
        {
            IsSuccess = success;
            Message = message;
        }
    }
}
