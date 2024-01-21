using Domain.Models.ChatModels;

namespace Application.Core
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T? Value { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }

        public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };

        public static Result<T> Failure(string errorCode, string errorMessage = "") => new Result<T> 
            { IsSuccess = false, ErrorCode = errorCode, ErrorMessage = errorMessage };

        internal static Result<Chat> Success(object value)
        {
            throw new NotImplementedException();
        }
    }
}
