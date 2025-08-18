using System.Net;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System;
using System.Net.Http;

public class JwtAuthorizeAttribute : AuthorizationFilterAttribute
{
    public override void OnAuthorization(HttpActionContext actionContext)
    {
        // 1. Check if Authorization header exists
        var authHeader = actionContext.Request.Headers.Authorization;

        if (authHeader == null || authHeader.Scheme != "Bearer" || string.IsNullOrEmpty(authHeader.Parameter))
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                new { message = "Missing or invalid Authorization header" });
            return;
        }

        // 2. Extract token
        var token = authHeader.Parameter;

        try
        {
            // 3. Validate token
            var principal = JwtTokenManager.ValidateToken(token);

            if (principal == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new { message = "Invalid or expired token" });
                return;
            }

            // 4. Attach user to current context
            actionContext.RequestContext.Principal = principal;
        }
        catch (Exception)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                new { message = "Error while validating token" });
        }
    }
}