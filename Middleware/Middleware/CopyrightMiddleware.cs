using System.Text;

namespace Middleware;

public class CopyrightMiddleware
{
    private readonly RequestDelegate _next;
    private const string Signature = "\n\nCopyright by Max Babak";

    public CopyrightMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBody = context.Response.Body;

        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        await _next(context);

        memoryStream.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(memoryStream).ReadToEndAsync();

        if (context.Response.ContentType != null &&
            context.Response.ContentType.Contains("text"))
        {
            responseText += Signature;
        }

        var bytes = Encoding.UTF8.GetBytes(responseText);
        context.Response.ContentLength = bytes.Length;

        await originalBody.WriteAsync(bytes);
        context.Response.Body = originalBody;
    }
}