using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library_Mangament_System_API_ClassLibrary.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Web;
using System.Configuration;

namespace Library_Mangament_System_API_ClassLibrary.DAL
{
    public class BookIssue
    {
        private Database db;

        #region BookIssue()
        public BookIssue()
        {
            db = DatabaseFactory.CreateDatabase();

        }
        #endregion

        #region Properties
        public int? MemberId { get; set; }
        public int IssueId { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public int TotalRecords { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 4;

        public int CreatedBy { get; set; } = 1;
        public int ModifiedBy { get; set; } = 2;
        public DateTime? ModifiedOn { get; set; }
        public bool IsActive { get; set; }

        public BookIssueModel IssuesList { get; set; }

        public BookIssueModel issue { get; set; }
        public List<HttpPostedFileBase> UploadFiles { get; set; }
        public List<BookIssueFileDocumentModel> fileListForBookIssueId { get; set; }
        public List<BookIssueDetailViewModel> BookDetails { get; set; }
        #endregion

        /// <summary>
        /// Loads the details for BookIssue.
        /// </summary>
        /// <returns>True if Load operation is successful; Else False.</returns>
        #region Load()
        public bool Load()
        {
            try
            {
                if (this.IssueId != 0)
                {

                    DbCommand cmd = db.GetStoredProcCommand("BookIssueGetAllDetails");
                    db.AddInParameter(cmd, "@BookIssueId", DbType.Int32, IssueId);
                    db.AddInParameter(cmd, "@PageNumber", DbType.Int32, 1);
                    db.AddInParameter(cmd, "@PageSize", DbType.Int32, 1000);
                    db.AddInParameter(cmd, "@MemberId", DbType.Int32, DBNull.Value);

                    using (IDataReader reader = db.ExecuteReader(cmd))
                    {

                        while (reader.Read())
                        {
                            if (issue == null)
                            {
                                issue = new BookIssueModel
                                {
                                    IssueId = reader.GetInt32(reader.GetOrdinal("BookIssueId")),
                                    MemberId = reader.GetInt32(reader.GetOrdinal("MemberId")),
                                    MemberName = reader.GetString(reader.GetOrdinal("MemberName")),
                                    IssueDate = reader.GetDateTime(reader.GetOrdinal("IssueDate")),
                                    DueDate = reader.IsDBNull(reader.GetOrdinal("DueDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DueDate")),
                                    ReturnDate = reader.IsDBNull(reader.GetOrdinal("ReturnDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                    BookDetails = new List<BookIssueDetailViewModel>(),
                                    fileListForBookIssueId = new List<BookIssueFileDocumentModel>()
                                };
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("BookId")))
                            {
                                issue.BookDetails.Add(new BookIssueDetailViewModel
                                {
                                    BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                    BookTitle = reader.IsDBNull(reader.GetOrdinal("BookName")) ? "" : reader.GetString(reader.GetOrdinal("BookName")),
                                    Quantity = reader.IsDBNull(reader.GetOrdinal("Quantity")) ? 0 : reader.GetInt32(reader.GetOrdinal("Quantity"))
                                });
                            }

                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                issue?.fileListForBookIssueId.Add(new BookIssueFileDocumentModel
                                {
                                    FileName = Convert.ToString(reader["FileName"]),
                                    FilePath = Convert.ToString(reader["FilePath"]),
                                    FileType = Convert.ToString(reader["FileType"]),
                                    CreatedBy = Convert.ToInt32(reader["CreatedBy"]),
                                    CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                                    ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(reader["ModifiedBy"]) : 2,
                                    ModifiedOn = reader["ModifiedOn"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["ModifiedOn"]) : null,
                                    IsActive = Convert.ToBoolean(reader["IsActive"])
                                });
                            }
                        }
                    }
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                Errors.LogErrorToDb(ex, 2);
                Errors.LogErrorToFile(ex);
                return false;
            }
        }
        #endregion

        /// <summary>
        /// Inserts or Updates the BookIssue.
        /// </summary>
        /// <returns>True if Save operation is successful; Else False.</returns>
        #region Save()
        public bool Save()
        {
            if (this.IssueId == 0)
            {
                return this.Insert();
            }
            else
            {
                if (this.IssueId > 0)
                {
                    return this.Update();
                }
                else
                {
                    this.IssueId = 0;
                    return false;
                }
            }
        }
        #endregion

        /// <summary>
        /// Inserts New BookIssue.
        /// </summary>
        /// <returns>True if Save operation is successful; Else False.</returns>
        #region Insert()
        public bool Insert()
        {
            int newBookIssueId = 0;

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("BookIssueInsert");
                db.AddInParameter(cmd, "@MemberId", DbType.Int32, issue.SelectedMemberId);
                db.AddInParameter(cmd, "@IssueDate", DbType.DateTime, issue.IssueDate);
                db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, issue.CreatedBy);

                DataTable bookDetailsTable = new DataTable();
                bookDetailsTable.Columns.Add("BookId", typeof(int));
                bookDetailsTable.Columns.Add("Quantity", typeof(int));

                foreach (var bookDetail in issue.BookDetails)
                {
                    bookDetailsTable.Rows.Add(bookDetail.BookId, bookDetail.Quantity);
                }

                var tvpParam = new SqlParameter("@BookDetails", SqlDbType.Structured)
                {
                    TypeName = "dbo.BookIssueDetailsType",
                    Value = bookDetailsTable
                };
                cmd.Parameters.Add(tvpParam);


                var outputParam = new SqlParameter("@NewBookIssueId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);


                db.ExecuteNonQuery(cmd);


                newBookIssueId = (int)cmd.Parameters["@NewBookIssueId"].Value;
            }
            catch (Exception ex)
            {
                Errors.LogErrorToDb(ex, 2);
                Errors.LogErrorToFile(ex);
            }
            IssueId = newBookIssueId;
            return newBookIssueId > 0;
        }
        #endregion

        /// <summary>
        /// Updates the BookIssue.
        /// </summary>
        /// <returns>True if Save operation is successful; Else False.</returns>
        #region Update()
        public bool Update()
        {
            bool isUpdated = false;

            try
            {

                List<BookIssueFileDocumentModel> combinedFileList = new List<BookIssueFileDocumentModel>();
                if (fileListForBookIssueId != null)
                {
                    combinedFileList.AddRange(fileListForBookIssueId);
                }


                if (UploadFiles != null)
                {
                    string baseVirtualPath = ConfigurationManager.AppSettings["PathForUploadFileWhileBookIssue"];
                    baseVirtualPath = baseVirtualPath.Replace("{model.IssueId}", IssueId.ToString());
                    string physicalPath = HttpContext.Current.Server.MapPath(baseVirtualPath);

                    if (!Directory.Exists(physicalPath))
                        Directory.CreateDirectory(physicalPath);

                    foreach (var uploadedFile in UploadFiles)
                    {
                        if (uploadedFile != null && uploadedFile.ContentLength > 0)
                        {
                            string extension = Path.GetExtension(uploadedFile.FileName);
                            string uniqueFileName = $"{Guid.NewGuid()}{extension}";
                            string fullPhysicalPath = Path.Combine(physicalPath, uniqueFileName);
                            string virtualFilePath = uniqueFileName;
                            try
                            {
                                uploadedFile.SaveAs(fullPhysicalPath);
                            }
                            catch (Exception ex)
                            {
                                Errors.LogErrorToDb(ex, ModifiedBy);
                                Errors.LogErrorToFile(ex);
                                continue;
                            }

                            var newFile = new BookIssueFileDocumentModel
                            {
                                FileName = Path.GetFileName(uploadedFile.FileName),
                                FilePath = virtualFilePath,
                                FileType = extension,
                                CreatedBy = ModifiedBy,
                                CreatedOn = DateTime.Now,
                                ModifiedBy = 2,
                                ModifiedOn = null,
                                IsActive = true
                            };

                            combinedFileList.Add(newFile);
                        }
                    }
                }


                DbCommand cmd = db.GetStoredProcCommand("BookIssueUpdate");

                db.AddInParameter(cmd, "@BookIssueId", DbType.Int32, IssueId);
                db.AddInParameter(cmd, "@MemberId", DbType.Int32, MemberId);
                db.AddInParameter(cmd, "@DueDate", DbType.DateTime, DueDate);
                db.AddInParameter(cmd, "@ReturnDate", DbType.DateTime, (object)ReturnDate ?? DBNull.Value);
                db.AddInParameter(cmd, "@IsActive", DbType.Boolean, IsActive);
                db.AddInParameter(cmd, "@ModifiedBy", DbType.Int32, ModifiedBy);


                DataTable bookDetailsTable = new DataTable();
                bookDetailsTable.Columns.Add("BookId", typeof(int));
                bookDetailsTable.Columns.Add("Quantity", typeof(int));

                if (BookDetails != null)
                {
                    foreach (var book in BookDetails)
                    {
                        bookDetailsTable.Rows.Add(book.BookId, book.Quantity);
                    }
                }

                SqlParameter bookDetailsParam = new SqlParameter("@BookDetails", SqlDbType.Structured)
                {
                    TypeName = "BookIssueDetailsType",
                    Value = bookDetailsTable
                };
                cmd.Parameters.Add(bookDetailsParam);


                DataTable fileDetailsTable = new DataTable();
                fileDetailsTable.Columns.Add("FileName", typeof(string));
                fileDetailsTable.Columns.Add("FilePath", typeof(string));
                fileDetailsTable.Columns.Add("FileType", typeof(string));
                fileDetailsTable.Columns.Add("CreatedBy", typeof(int));
                fileDetailsTable.Columns.Add("CreatedOn", typeof(DateTime));
                fileDetailsTable.Columns.Add("ModifiedBy", typeof(int));
                fileDetailsTable.Columns.Add("ModifiedOn", typeof(DateTime));
                fileDetailsTable.Columns.Add("IsActive", typeof(bool));

                foreach (var file in combinedFileList)
                {
                    object createdOnVal = (file.CreatedOn == null || file.CreatedOn == DateTime.MinValue)
                        ? (object)DBNull.Value
                        : file.CreatedOn;

                    object modifiedOnVal = (file.ModifiedOn == null || file.ModifiedOn == DateTime.MinValue)
                        ? (object)DBNull.Value
                        : file.ModifiedOn;

                    fileDetailsTable.Rows.Add(
                        file.FileName ?? string.Empty,
                        file.FilePath ?? string.Empty,
                        file.FileType ?? string.Empty,
                        file.CreatedBy,
                        createdOnVal,
                        (object)file.ModifiedBy ?? DBNull.Value,
                        modifiedOnVal,
                        file.IsActive ?? true
                    );
                }

                SqlParameter fileDetailsParam = new SqlParameter("@FileDetails", SqlDbType.Structured)
                {
                    TypeName = "BookIssueFileDocumentsType",
                    Value = fileDetailsTable
                };
                cmd.Parameters.Add(fileDetailsParam);

                db.ExecuteNonQuery(cmd);
                isUpdated = true;
            }
            catch (Exception ex)
            {
                Errors.LogErrorToDb(ex, 2);
                Errors.LogErrorToFile(ex);
            }

            return isUpdated;
        }
        #endregion

        #region GetBookIssueList
        public List<BookIssueModel> GetBookIssueList()
        {
            List<BookIssueModel> issues = new List<BookIssueModel>();
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("BookIssueGetAllDetails");
                db.AddInParameter(cmd, "@PageNumber", DbType.Int32, PageNumber);
                db.AddInParameter(cmd, "@PageSize", DbType.Int32, PageSize);

                //db.AddInParameter(cmd, "@MemberId", DbType.Int32, MemberId == null ? MemberId : DBNull.Value);
                db.AddInParameter(cmd, "@MemberId", DbType.Int32, MemberId.HasValue ? (object)MemberId.Value : DBNull.Value);
                db.AddInParameter(cmd, "@BookIssueId", DbType.Int32, DBNull.Value);

                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    Dictionary<int, BookIssueModel> issueDict = new Dictionary<int, BookIssueModel>();

                    while (reader.Read())
                    {
                        int issueId = Convert.ToInt32(reader["BookIssueId"]);

                        if (!issueDict.ContainsKey(issueId))
                        {
                            BookIssueModel issue = new BookIssueModel
                            {
                                IssueId = issueId,
                                MemberId = Convert.ToInt32(reader["MemberId"]),
                                MemberName = Convert.ToString(reader["MemberName"]),
                                IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                                DueDate = Convert.ToDateTime(reader["DueDate"]),
                                ReturnDate = reader["ReturnDate"] != DBNull.Value ? Convert.ToDateTime(reader["ReturnDate"]) : (DateTime?)null,
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                BookDetails = new List<BookIssueDetailViewModel>(),
                                fileListForBookIssueId = new List<BookIssueFileDocumentModel>()
                            };

                            issueDict[issueId] = issue;
                        }
                        if (reader["BookId"] != DBNull.Value)
                        {
                            issueDict[issueId].BookDetails.Add(new BookIssueDetailViewModel
                            {
                                BookId = Convert.ToInt32(reader["BookId"]),
                                BookTitle = Convert.ToString(reader["BookName"]),
                                Quantity = Convert.ToInt32(reader["Quantity"])
                            });
                        }

                        //issueDict[issueId].BookDetails.Add(new BookIssueDetailViewModel
                        //{
                        //    BookId = Convert.ToInt32(reader["BookId"]),
                        //    BookTitle = Convert.ToString(reader["BookName"]),
                        //    Quantity = Convert.ToInt32(reader["Quantity"])
                        //});
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            int issueId = Convert.ToInt32(reader["BookIssueId"]);

                            if (issueDict.ContainsKey(issueId))
                            {
                                issueDict[issueId].fileListForBookIssueId.Add(new BookIssueFileDocumentModel
                                {
                                    FileName = Convert.ToString(reader["FileName"]),
                                    FilePath = Convert.ToString(reader["FilePath"]),
                                    FileType = Convert.ToString(reader["FileType"]),
                                    CreatedBy = Convert.ToInt32(reader["CreatedBy"]),
                                    CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                                    ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(reader["ModifiedBy"]) : 2,
                                    ModifiedOn = reader["ModifiedOn"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["ModifiedOn"]) : null,
                                    IsActive = Convert.ToBoolean(reader["IsActive"])
                                });
                            }
                        }
                    }

                    if (reader.NextResult() && reader.Read())
                    {
                        TotalRecords = Convert.ToInt32(reader["TotalRecords"]);
                    }

                    issues = issueDict.Values.ToList();
                }
            }
            catch (Exception ex)
            {
                Errors.LogErrorToDb(ex, 2);
                Errors.LogErrorToFile(ex);
            }
            return issues;
        }
        #endregion

        #region SaveFilesFromBookIssue(BookIssueViewModel model)
        public bool SaveFilesFromBookIssue(BookIssueModel model)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("BookIssueFileDocumentsInsert");
                db.AddInParameter(cmd, "@BookIssueId", DbType.Int32, model.IssueId);

                List<HttpPostedFileBase> FileList = model.UploadFiles;

                DataTable dt = new DataTable();
                dt.Columns.Add("FileName", typeof(string));
                dt.Columns.Add("FilePath", typeof(string));
                dt.Columns.Add("FileType", typeof(string));
                dt.Columns.Add("CreatedBy", typeof(int));
                dt.Columns.Add("CreatedOn", typeof(DateTime));
                dt.Columns.Add("ModifiedBy", typeof(int));
                dt.Columns.Add("ModifiedOn", typeof(DateTime));
                dt.Columns.Add("IsActive", typeof(bool));


                string baseVirtualPath = ConfigurationManager.AppSettings["PathForUploadFileWhileBookIssue"];
                baseVirtualPath = baseVirtualPath.Replace("{model.IssueId}", model.IssueId.ToString());
                string physicalPath = HttpContext.Current.Server.MapPath(baseVirtualPath);

                if (!Directory.Exists(physicalPath))
                    Directory.CreateDirectory(physicalPath);

                foreach (var file in FileList)
                {
                    if (file != null && file.ContentLength > 0)
                    {

                        string extension = Path.GetExtension(file.FileName);
                        string uniqueFileName = $"{Guid.NewGuid()}{extension}";
                        string fullPhysicalPath = Path.Combine(physicalPath, uniqueFileName);
                        string virtualFilePath = Path.Combine(uniqueFileName);

                        try
                        {
                            file.SaveAs(fullPhysicalPath);
                        }
                        catch (Exception ex)
                        {
                            Errors.LogErrorToDb(ex, model.CreatedBy);
                            Errors.LogErrorToFile(ex);

                        }

                        dt.Rows.Add(
                            Path.GetFileName(file.FileName),
                            virtualFilePath,
                            extension,
                            model.CreatedBy,
                            DateTime.Now,
                            model.ModifiedBy,
                            model.ModifiedOn,
                            true
                        );
                    }
                }

                var tvpParameter = new SqlParameter("@BookIssueFileDocumentsType", SqlDbType.Structured)
                {
                    TypeName = "BookIssueFileDocumentsType",
                    Value = dt
                };
                cmd.Parameters.Add(tvpParameter);

                var outputParameter = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParameter);

                db.ExecuteNonQuery(cmd);
                return Convert.ToBoolean(outputParameter.Value);
            }
            catch (Exception ex)
            {
                Errors.LogErrorToDb(ex, model.CreatedBy);
                Errors.LogErrorToFile(ex);
                return false;
            }
        }
        #endregion

    }
}
