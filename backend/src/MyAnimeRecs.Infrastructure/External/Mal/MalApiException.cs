using System.Net;

namespace MyAnimeRecs.Infrastructure.External.Mal;

public class MalApiException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public MalApiException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}
