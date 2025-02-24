using IResult = ProductManagement.Core.Interfaces;

namespace ProductManagement.API.ApiModels.ActionResult
{
    public class ActionResult : IResult.IResult
    {
        private string _message = string.Empty;

        public ActionResult() { }

        public ActionResult(string message)
        {
            Message = message;
        }

        public bool Success
        {
            get => string.IsNullOrEmpty(Message);
        }

        public string Message
        {
            get => _message;
            internal set => _message = value;
        }

        public void Merge(IResult.IResult other)
        {
            if (other != null && !string.IsNullOrEmpty(other.Message))
            {
                Message = string.IsNullOrEmpty(Message) ? other.Message : $"{Message}; {other.Message}";
            }
        }

        public void AddMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Message = string.IsNullOrEmpty(Message) ? message : $"{Message}; {message}";
            }
        }
    }
}
