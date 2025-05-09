using APBD_s31722_TEST_TEMPLATE.Exceptions;
using System.Text.Json;

namespace APBD_s31722_TEST_TEMPLATE.Exceptions;

public class ApiExceptions : Exception
{
    public int StatusCode { get; set; }
    public object Errors { get; set; }
    
    public ApiExceptions(int statusCode, string message, object? errors = null)
        : base(message)
    {
        StatusCode = statusCode;
        Errors = errors;
    }

    public ApiExceptions(ErrorStatusCode statusCode, string message, object? errors = null)
        : this((int)statusCode, message, errors)
    {
    }

    public string ToJson()
    {
        var problem = new
        {
            error = Message,
            details = Errors,
        };
            return JsonSerializer.Serialize(problem);
    }
}