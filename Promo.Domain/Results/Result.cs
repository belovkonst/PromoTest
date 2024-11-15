namespace Promo.Domain.Results;

public class Result
{
    public string? ErrorText { get; set; }
    public int? ErrorCode { get; set; }

    public bool Success => ErrorText == null;

}

public class Result<T> : Result
{
    public T Data { get; set; }

    public Result(string? errorText, int? errorCode, T data)
    {
        ErrorText = errorText;
        ErrorCode = errorCode;
        Data = data;
    }

    public Result()
    {
    }
}
