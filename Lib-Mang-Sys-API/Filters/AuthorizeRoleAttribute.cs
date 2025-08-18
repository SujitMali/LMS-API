using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public class AuthorizeRoleAttribute : AuthorizeAttribute
{
    private readonly string[] allowedRoles;

    public AuthorizeRoleAttribute(params string[] roles)
    {
        this.allowedRoles = roles;
    }

    protected override bool IsAuthorized(HttpActionContext actionContext)
    {
        var authHeader = actionContext.Request.Headers.Authorization;
        var principal = JwtTokenManager.ValidateToken(authHeader.Parameter);
        actionContext.RequestContext.Principal = principal;
        return allowedRoles.Any(role => principal.IsInRole(role));
    }


}




















//protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
//{
//    var authHeader = actionContext.Request.Headers.Authorization;
//    var principal = authHeader != null ? JwtTokenManager.ValidateToken(authHeader.Parameter) : null;

//    if (principal == null)
//    {

//        actionContext.Response = actionContext.Request.CreateResponse((HttpStatusCode)498,
//            new { message = "Unauthorized. Please login again." });
//    }
//    else
//    {
//        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden,
//            new { message = "Access denied to Resources !" });
//    }
//}