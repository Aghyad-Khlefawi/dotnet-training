using System.Net;
using System.Text;

namespace training;

public static class Middleware
{

  public static void UserBasicAuth(this WebApplication app)
  {
    app.Use(async (context, next) =>
    {

      var headerValue = context.Request.Headers.Authorization;

      if (string.IsNullOrEmpty(headerValue))
      {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.WriteAsync("Authorization header is required.");
        return;
      }

      if (!headerValue.ToString().StartsWith("Basic "))
      {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.WriteAsync("Unsupported Authorization scheme");
        return;
      }

      var cred = Encoding.ASCII.GetString(Convert.FromBase64String(headerValue.ToString()[5..])).Split(":");

      if (!await context.RequestServices.GetRequiredService<IUserRepository>().IsUserValid(cred[0], cred[1]))
      {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.WriteAsync("Incorrect credentials");
        return;
      }

      await next(context);
    });

  }
}
