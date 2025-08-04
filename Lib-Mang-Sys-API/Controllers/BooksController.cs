using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Library_Mangament_System_API_ClassLibrary.DAL;
using Library_Mangament_System_API_ClassLibrary.Models;

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
                //List<BooksModel> bookList = objBooksDal.GetBooksList(model);
                model.Books = objBooksDal.GetBooksList(model);
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
            public HttpResponseMessage AddEditBook(BooksModel model)
            {
                try
                {
                    Books objBookDal = new Books
                    {
                        BookId = model.BookId,
                        BookName = model.BookName,
                        PublisherId = model.PublisherId,
                        LanguageId = model.LanguageId,
                        Cost = model.Cost,
                        Pages = model.Pages,
                        TotalQuantity = model.TotalQuantity,
                        AvailableQuantity = model.AvailableQuantity,
                        IsActive = model.IsActive,
                        CreatedBy = model.CreatedBy
                    };

                    bool isSuccess = objBookDal.Save();

                    if (isSuccess && objBookDal.BookId > 0)
                    {
                        string msg = model.BookId == 0 ? "Book inserted successfully." : "Book updated successfully.";
                        return Request.CreateResponse(HttpStatusCode.OK, new { message = msg });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Book Operation Failed." });
                    }
                }
                catch (Exception ex)
                {
                    Errors.LogErrorToFile(ex);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Something went wrong." });
                }
        }

    }
}
