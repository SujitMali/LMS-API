using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Library_Mangament_System_API_ClassLibrary.DAL;
using Library_Mangament_System_API_ClassLibrary.Models;

namespace Lib_Mang_Sys_API.Controllers
{
    public class BookIssueController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetMembersList()
        {
            HttpResponseMessage result = null;
            Members objMembersDal = new Members();
            List<MembersModel> MemberList = objMembersDal.GetMemberList();
            result = Request.CreateResponse(HttpStatusCode.OK, MemberList);
            return result;
        }

        [HttpGet]
        public HttpResponseMessage GetBooksList()
        {
            HttpResponseMessage result = null;
            Books objBooksDal = new Books();
            BooksModel model = new BooksModel();
            model.PageSize = int.MaxValue;
            model.IsActive = true;
            model.PublisherId = 0;
            model.LanguageId = 0;
            model.BookName = null;
            List<BooksModel> BooksList = objBooksDal.GetBooksList(model);
            result = Request.CreateResponse(HttpStatusCode.OK, BooksList);
            return result;
        }


        [HttpPost]
        public HttpResponseMessage GetBookIssueList(BookIssueModel model)
        {
            HttpResponseMessage result = null;
            try
            {
                BookIssue objBookIssueDal = new BookIssue();
                objBookIssueDal.MemberId = model.MemberId;
                objBookIssueDal.PageNumber = model.PageNumber;
                objBookIssueDal.PageSize = model.PageSize;

                model.issues = objBookIssueDal.GetBookIssueList();
                model.TotalRecords = objBookIssueDal.TotalRecords;
                result = Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                Errors.LogErrorToFile(ex);
            }
            return result;
        }



        [HttpGet]
        public HttpResponseMessage GetBookIssueForm(int IssueId)
        {
            HttpResponseMessage result = null;
            BookIssueModel model = new BookIssueModel();
            BookIssue objBookIssueDal = new BookIssue();

            try
            {
                if (IssueId > 0)
                {
                    objBookIssueDal.IssueId = IssueId;
                    objBookIssueDal.Load();
                    model.Issue = objBookIssueDal.issue;
                }

                result = Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                Errors.LogErrorToDb(ex, 2);
                Errors.LogErrorToFile(ex);
                result = Request.CreateResponse(HttpStatusCode.InternalServerError, "Can't Send !");
            }
            return result;
        }





        [HttpGet]
        public HttpResponseMessage DownloadFile(string filePath, string fileName, int issueId)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(fileName))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Missing file path or file name");
            }
            HttpResponseMessage result = null;
            try
            {
                string basePath = (ConfigurationManager.AppSettings["PathForUploadFileWhileNewPurchaseOrderCreate"]).Replace("{model.IssueId}", issueId.ToString());

                string physicalPath = (HttpContext.Current.Server.MapPath(Path.Combine(basePath, filePath)));

                if (!File.Exists(physicalPath))
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "File not found !");
                }

                byte[] fileBytes = File.ReadAllBytes(physicalPath);
                //result = new HttpResponseMessage(HttpStatusCode.OK) { Content = (fileBytes) };
                result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(fileBytes) };

                return result;
            }
            catch (Exception ex)
            {
                Errors.LogErrorToDb(ex, issueId);
                Errors.LogErrorToFile(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error while processing the file");
            }
        }

    }
}














//result.Content.Headers.ContentDisposition =
//    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
//    {
//        FileName = fileName
//    };
//result.Content.Headers.ContentType =
//    new System.Net.Http.Headers.MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));










//string physicalPath = (System.Web.HttpContext.Current.Server.MapPath(Path.Combine(basePath, filePath))).Replace("\\", "/");
//Members objMembersdal = new Members();
//model.MembersList = objMembersdal.GetMemberList();

//Books objBooksDal = new Books();
//model.PageSize = int.MaxValue;
//model.IsActive = true;
//model.PublisherId = 0;
//model.LanguageId = 0;
//model.BookName = null;
//model.Books=objBooksDal.GetBooksList(model);

//if (model.IssueId > 0)
//{ 
//    BookIssue objBookIssueViewDal = new BookIssue();

//    objBookIssueViewDal.IssueId = model.IssueId;
//    objBookIssueViewDal.Load();

//    BookIssueModel issueDetails = objBookIssueViewDal.issue;

//    model.SelectedMemberId = issueDetails.MemberId;
//    model.DueDate = issueDetails.DueDate;
//    model.ReturnDate = issueDetails.ReturnDate;
//    model.IssuesList = issueDetails;
//    //model.IssuedBookDetails = issueDetails?.BookDetails;
//    //model.fileListForBookIssueId = issueDetails?.fileListForBookIssueId;
//}