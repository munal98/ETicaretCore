using AppCore.Business.Results.Bases;

namespace AppCore.Business.Results
{
    public class Result
    {
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }

        public Result(bool isSuccessful, string? message)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }
    }

    public class Result<TResultType> : Result, IResultData<TResultType>
    {
        public TResultType Data { get; }

        public Result(bool isSuccessful, string? message, TResultType data) : base(isSuccessful, message)
        {
            Data = data;
        }
    }
}
