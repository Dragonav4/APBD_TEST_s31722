namespace APBD_s31722_TEST_TEMPLATE.Exceptions;

public enum ErrorStatusCode
{
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    UnprocessableEntity = 422,
    InternalServerError = 500
}

public static class ErrorStatusMap
{

    private static readonly Dictionary<ErrorStatusCode, string> _descriptions =
        new()
        {
            { ErrorStatusCode.BadRequest, "BadRequest" },
            { ErrorStatusCode.Unauthorized, "Unauthorized" },
            { ErrorStatusCode.Forbidden, "Forbidden" },
            { ErrorStatusCode.NotFound, "NotFound" },
            { ErrorStatusCode.Conflict, "Conflict" },
            { ErrorStatusCode.UnprocessableEntity, "Unprocessable Entity" },
            { ErrorStatusCode.InternalServerError, "Internal Server Error" }
        };
    
    public static string GetDiscription(this ErrorStatusCode statusCode) => _descriptions.
        TryGetValue(statusCode,out var description) 
        ? description : statusCode.ToString();

}
