using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using LibraryManagementSystemClassLibrary;
using LibraryManagementSystemClassLibrary.DAL;

namespace Lib_Mang_Sys_API.Controllers
{

    public class BooksController : ApiController
    {
        [HttpGet]
        [Route("api/Books/Get")]
        public HttpResponseMessage Get()
        {
            HttpResponseMessage result = null;
            return result = Request.CreateResponse(HttpStatusCode.OK, "Hello Guys!");
        }

        [HttpPost]
        public HttpResponseMessage Bookgetlist([FromBody] BooksModel model)
        {
            HttpResponseMessage result = null;
            try
            {
                Books objBooksDal = new Books();
                List<BooksModel> bookList = objBooksDal.GetBooksList(model);

                Languages objLangaugeDal = new Languages();
                model.Languages = objLangaugeDal.GetLanguageList();

                Publishers objPublisherDal = new Publishers();
                model.Publishers = objPublisherDal.GetPublisherList();

                result = Request.CreateResponse(HttpStatusCode.OK, bookList);
            }
            catch (Exception ex)
            {
                Errors.LogErrorToFile(ex);
                result = Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            return result;

        }
    }
}
