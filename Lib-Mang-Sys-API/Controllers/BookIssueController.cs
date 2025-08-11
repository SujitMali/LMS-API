using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.UI.WebControls;
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
        public HttpResponseMessage DownloadFile(string filePath, string fileName, int issueId)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(fileName))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Missing file path or file name");

            try
            {
                string decodedFileName = HttpUtility.UrlDecode(filePath);

                string basePath = (ConfigurationManager.AppSettings["PathForUploadFileWhileNewPurchaseOrderCreate"]).Replace("{model.IssueId}", issueId.ToString());

                string physicalPath = (System.Web.HttpContext.Current.Server.MapPath(Path.Combine(basePath, decodedFileName))).Replace("\\", "/");

                if (!System.IO.File.Exists(physicalPath))
                    return Request.CreateResponse(HttpStatusCode.NotFound, "File not found at: " + physicalPath);

                byte[] fileBytes = System.IO.File.ReadAllBytes(physicalPath);

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(fileBytes)
                };

                result.Content.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = fileName
                    };

                result.Content.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));

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
