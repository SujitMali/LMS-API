using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Routing;
using LibraryManagementSystemClassLibrary;
using LibraryManagementSystemClassLibrary.DAL;

namespace Lib_Mang_Sys_API.Controllers
{

    public class BooksController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetBookListFilters()
        {
            HttpResponseMessage result = null;
            try
            {
                BooksModel model = new BooksModel();
                Languages objLangaugeDal = new Languages();
                model.Languages = objLangaugeDal.GetLanguageList();
                Publishers objPublisherDal = new Publishers();
                model.Publishers = objPublisherDal.GetPublisherList();
                result = Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                Errors.LogErrorToFile(ex);
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }


        [HttpPost]
        public HttpResponseMessage GetBookList([FromBody] BooksModel model)
        {
            HttpResponseMessage result = null;
            try
            {
                Books objBooksDal = new Books();
                List<BooksModel> bookList = objBooksDal.GetBooksList(model);
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
