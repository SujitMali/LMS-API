using System.Net;
using System.Net.Http;
using System.Web.Http;
using Library_Mangament_System_API_ClassLibrary.DAL;
using Library_Mangament_System_API_ClassLibrary.Models;

namespace Lib_Mang_Sys_API.Controllers
{
    public class UsersController : ApiController
    {


            [HttpPost]
            public HttpResponseMessage ValidateUserLogin(UsersModel model)
            {
                Users objUsersDal = new Users();
                UsersModel tempUser = objUsersDal.ValidateUser(model);

                if (tempUser != null)
                {
                    var token = JwtTokenManager.GenerateToken(tempUser);

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        message = "Login successful",
                        user = tempUser,
                        token
                    });
                }

                return Request.CreateResponse(HttpStatusCode.Unauthorized, new
                {
                    message = "Invalid username or password"
                });


            }
    }
}











/*Request.CreateResponse(HttpStatusCode.OK, new { ... })
    Web API doesn’t send this raw C# object over the network (JavaScript can’t read C# memory).
    Instead:
           1. It runs the JSON serializer (Newtonsoft.Json in old Web API).
           2. That serializer turns your anonymous object into a JSON string.
           3. Every nested property inside tempUser also gets serialized into JSON.*/