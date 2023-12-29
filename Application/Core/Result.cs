namespace Application.Core
{
    public class Result<T>
    // W.I.P. can''t send http responses since 
    // Application is separated from the API,
    // TODO find a solution
    {
        public bool IsSuccess { get; set; }
        public T? Value { get; set; }
        public string? Error { get; set; }

        public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };

        public static Result<T> Failure(string error) => new Result<T> { IsSuccess = false, Error = error };
    }
}
