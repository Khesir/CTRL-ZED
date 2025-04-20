public class Response<T>
{
    public bool IsSuccess { get; }
    public string Title { get; }
    public string Message { get; }
    public T Data { get; }

    public Response(bool isSuccess, string title, string message, T data = default)
    {
        IsSuccess = isSuccess;
        Title = title;
        Message = message;
        Data = data;
    }

    public static Response<T> Success(string message, T data = default) =>
        new Response<T>(true, "Success", message, data);

    public static Response<T> Fail(string message, T data = default) =>
        new Response<T>(false, "Error", message, data);
}

public static class Response
{
    public static Response<object> Success(string message) =>
        new(true, "Success", message);

    public static Response<object> Fail(string message) =>
        new(false, "Error", message);
}


