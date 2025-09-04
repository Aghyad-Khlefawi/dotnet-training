using System.Net;
using System.Text;

namespace training;

public static class Middlewares
{
  public static void UseBasicAuth(this WebApplication app)
  {
    app.Use(async (context, next) =>
    {

      var headerValue = context.Request.Headers.Authorization;
      if (string.IsNullOrEmpty(headerValue))
      {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.WriteAsync("Unauthorized user");
        return;
      }
      if (!headerValue.ToString().StartsWith("Basic "))
      {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.WriteAsync("Unsupported Authorization Scheme");
        return;
      }

      try
      {
        var base64Value = headerValue.ToString()[5..];
        var cred = Encoding.ASCII.GetString(Convert.FromBase64String(base64Value)).Split(":");

        if (!await context.RequestServices.GetRequiredService<IUserRepository>().IsUserValid(cred[0], cred[1]))
        {
          context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
          await context.Response.WriteAsync("Invalid credentials");
          return;
        }
        // Log invalid login attempt
      }
      catch (Exception)
      {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.WriteAsync("Invalid credentials");
        return;
      }

      await next(context);

    });
  }
}
