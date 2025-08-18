using System.Net;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System;
using System.Net.Http;

public class JwtAuthorizeAttribute : AuthorizationFilterAttribute
{
    public override void OnAuthorization(HttpActionContext actionContext)
    {
        var authHeader = actionContext.Request.Headers.Authorization;

        if (authHeader == null || authHeader.Scheme != "Bearer" || string.IsNullOrEmpty(authHeader.Parameter))
        {
            actionContext.Response = actionContext.Request.CreateResponse((HttpStatusCode)498,
                new { message = "Missing or invalid Authorization header" });
            return;
        }

        var token = authHeader.Parameter;

        try
        {
            var principal = JwtTokenManager.ValidateToken(token);

            if (principal == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse((HttpStatusCode)498,
                    new { message = "Invalid or expired JWT token" });
                return;
            }

            actionContext.RequestContext.Principal = principal;
        }
        catch (Exception)
        {
            actionContext.Response = actionContext.Request.CreateResponse((HttpStatusCode)498,
                new { message = "Error while validating JWT token" });
        }
    }
}
