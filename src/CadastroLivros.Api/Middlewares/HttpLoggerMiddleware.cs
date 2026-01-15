using System.Diagnostics;
using System.Text;

namespace CadastroLivros.Api.Middlewares;

public class HttpLoggerMiddleware(RequestDelegate next, ILogger<HttpLoggerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString();
        var activity = Activity.Current;

        await LogRequestAsync(context, requestId, activity);
        
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
            await LogResponseAsync(context, requestId, stopwatch.ElapsedMilliseconds, activity);
            
            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }
    }

    private async Task LogRequestAsync(HttpContext context, string requestId, Activity? activity)
    {
        var request = context.Request;
        var activityId = activity?.Id ?? "N/A";
        var parentId = activity?.ParentId ?? "N/A";
        
        logger.LogInformation(
            "[Request {RequestId}] {Method} {Path}{QueryString} | ActivityId: {ActivityId} | ParentId: {ParentId} | Headers: {Headers}",
            requestId,
            request.Method,
            request.Path,
            request.QueryString,
            activityId,
            parentId,
            FormatHeaders(request.Headers)
        );

        if (request.ContentLength > 0 && request.ContentType?.Contains("application/json") == true)
        {
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            var bytesRead = await request.Body.ReadAsync(buffer);
            request.Body.Position = 0;

            var bodyAsText = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            logger.LogInformation("[Request {RequestId}] Body: {Body}", requestId, bodyAsText);
        }
    }

    private async Task LogResponseAsync(HttpContext context, string requestId, long elapsedMilliseconds, Activity? activity)
    {
        var response = context.Response;
        var activityId = activity?.Id ?? "N/A";
        var parentId = activity?.ParentId ?? "N/A";
        
        response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        logger.LogInformation(
            "[Response {RequestId}] Status: {StatusCode} | ActivityId: {ActivityId} | ParentId: {ParentId} | Headers: {Headers} | Elapsed: {Elapsed}ms",
            requestId,
            response.StatusCode,
            activityId,
            parentId,
            FormatHeaders(response.Headers),
            elapsedMilliseconds
        );

        if (!string.IsNullOrEmpty(responseBody) && response.ContentType?.Contains("application/json") == true)
        {
            logger.LogInformation("[Response {RequestId}] Body: {Body}", requestId, responseBody);
        }
    }

    private static string FormatHeaders(IHeaderDictionary headers)
    {
        var headerList = headers
            .Where(h => !h.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
            .Select(h => $"{h.Key}={string.Join(", ", h.Value.ToString())}")
            .ToList();

        return string.Join("; ", headerList);
    }
}

