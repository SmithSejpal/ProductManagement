using ProductManagement.Core.Interfaces;

namespace ProductManagement.Core
{
    public class Result : IResult
    {
        private string _message = string.Empty;

        public Result() { }

        public Result(string message)
        {
            Message = message;
        }

        public bool Success => string.IsNullOrEmpty(Message);

        public string Message
        {
            get => _message;
            private set => _message = value;
        }

        public void Merge(IResult other)
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

        public static Result SuccessResult() => new Result();

        public static Result Error(string message) => new Result(message);
    }
}