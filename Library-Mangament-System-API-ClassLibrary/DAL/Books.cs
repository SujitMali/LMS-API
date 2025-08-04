using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Library_Mangament_System_API_ClassLibrary.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Library_Mangament_System_API_ClassLibrary.DAL
{
    public class Books
    {
        private Database db;

        #region Books()
        public Books()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        #endregion

        #region Properties 
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int Pages { get; set; }
        public decimal Cost { get; set; }
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public int PublisherId { get; set; }
        public string PublisherName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public bool? IsActive { get; set; }

        public int TotalRecords { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 4;
        public int? MemberId { get; set; }

        public int IssueId { get; set; }
        public BookIssueModel IssuesList { get; set; }
        #endregion

        /// <summary>
        /// Loads the details for Book.
        /// </summary>
        /// <returns>True if Load operation is successful; Else False.</returns>
        #region Load()
        public bool Load()
        {
            try
            {
                if (this.BookId != 0)
                {
                    DbCommand com = this.db.GetStoredProcCommand("booksGetDetail");
                    this.db.AddInParameter(com, "BookId", DbType.Int32, this.BookId);
                    DataSet ds = this.db.ExecuteDataSet(com);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        this.BookId = Convert.ToInt32(dt.Rows[0]["BookId"]);
                        this.BookName = Convert.ToString(dt.Rows[0]["BookName"]);
                        this.Pages = Convert.ToInt32(dt.Rows[0]["Pages"]);
                        this.Cost = Convert.ToDecimal(dt.Rows[0]["Cost"]);
                        this.TotalQuantity = Convert.ToInt32(dt.Rows[0]["TotalQuantity"]);
                        this.AvailableQuantity = Convert.ToInt32(dt.Rows[0]["AvailableQuantity"]);
                        this.LanguageId = Convert.ToInt32(dt.Rows[0]["LanguageId"]);
                        this.PublisherId = Convert.ToInt32(dt.Rows[0]["PublisherId"]);
                        this.CreatedOn = Convert.ToDateTime(dt.Rows[0]["CreatedOn"]);
                        this.ModifiedOn = dt.Rows[0]["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["ModifiedOn"]) : (DateTime?)null;
                        this.CreatedBy = Convert.ToInt32(dt.Rows[0]["CreatedBy"]);
                        this.ModifiedBy = Convert.ToInt32(dt.Rows[0]["ModifiedBy"]);
                        this.IsActive = dt.Rows[0]["IsActive"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["IsActive"]) : (bool?)null;
                        return true;
                    }
                }

                return false;
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
        /// Inserts or Updates the Book.
        /// </summary>
        /// <returns>True if Save operation is successful; Else False.</returns>
        #region Save()
        public bool Save()
        {
            if (this.BookId == 0)
            {
                return this.Insert();
            }
            else
            {
                if (this.BookId > 0)
                {
                    return this.Update();
                }
                else
                {
                    this.BookId = 0;
                    return false;
                }
            }
        }
        #endregion

        /// <summary>
        /// Inserts Book.
        /// </summary>
        /// <returns>True if Insert operation is successful; Else False.</returns>
        #region Insert()
        private bool Insert()
        {
            try
            {
                DbCommand com = this.db.GetStoredProcCommand("booksInsert");
                this.db.AddOutParameter(com, "BookId", DbType.Int32, 1024);
                this.db.AddInParameter(com, "BookName", DbType.String, this.BookName ?? (object)DBNull.Value);
                this.db.AddInParameter(com, "Pages", DbType.Int32, this.Pages);
                this.db.AddInParameter(com, "Cost", DbType.Decimal, this.Cost);
                this.db.AddInParameter(com, "TotalQuantity", DbType.Int32, this.TotalQuantity);
                this.db.AddInParameter(com, "AvailableQuantity", DbType.Int32, this.AvailableQuantity);
                this.db.AddInParameter(com, "LanguageId", DbType.Int32, this.LanguageId);
                this.db.AddInParameter(com, "PublisherId", DbType.Int32, this.PublisherId);
                this.db.AddInParameter(com, "CreatedBy", DbType.Int32, this.CreatedBy);
                this.db.AddInParameter(com, "IsActive", DbType.Boolean, this.IsActive);

                this.db.ExecuteNonQuery(com);
                this.BookId = Convert.ToInt32(this.db.GetParameterValue(com, "BookId"));
            }
            catch (Exception ex)
            {
                Errors.LogErrorToDb(ex, 2);
                Errors.LogErrorToFile(ex);
                return false;
            }

            return this.BookId > 0;
        }
        #endregion

        /// <summary>
        /// Updates Book.
        /// </summary>
        /// <returns>True if Update operation is successful; Else False.</returns>
        #region Update()
        private bool Update()
        {
            try
            {
                DbCommand com = this.db.GetStoredProcCommand("booksUpdate");
                this.db.AddInParameter(com, "BookId", DbType.Int32, this.BookId);
                this.db.AddInParameter(com, "BookName", DbType.String, this.BookName ?? (object)DBNull.Value);
                this.db.AddInParameter(com, "Pages", DbType.Int32, this.Pages);
                this.db.AddInParameter(com, "Cost", DbType.Decimal, this.Cost);
                this.db.AddInParameter(com, "TotalQuantity", DbType.Int32, this.TotalQuantity);
                this.db.AddInParameter(com, "AvailableQuantity", DbType.Int32, this.AvailableQuantity);
                this.db.AddInParameter(com, "LanguageId", DbType.Int32, this.LanguageId);
                this.db.AddInParameter(com, "PublisherId", DbType.Int32, this.PublisherId);
                this.db.AddInParameter(com, "ModifiedBy", DbType.Int32, this.ModifiedBy);
                this.db.AddInParameter(com, "IsActive", DbType.Boolean, this.IsActive ?? (object)DBNull.Value);
                this.db.ExecuteNonQuery(com);
            }
            catch (Exception ex)
            {
                Errors.LogErrorToDb(ex, 2);
                Errors.LogErrorToFile(ex);
                return false;
            }

            return true;
        }
        #endregion

        /// <summary>
        /// Deletes Book.
        /// </summary>
        /// <returns>True if Delete operation is successful; Else False.</returns>
        #region Delete()
        public bool Delete()
        {
            try
            {
                DbCommand com = this.db.GetStoredProcCommand("BooksDelete");
                this.db.AddInParameter(com, "BookId", DbType.Int32, this.BookId);
                this.db.AddInParameter(com, "ModifiedBy", DbType.String, this.ModifiedBy);
                this.db.ExecuteNonQuery(com);

            }
            catch (Exception ex)
            {
                Errors.LogErrorToDb(ex, 2);
                Errors.LogErrorToFile(ex);
                return false;
            }

            return true;
        }
        #endregion

        /// <summary>
        /// Retrieves a list of books based on search Parameters.
        /// </summary>
        /// <param name="model">An instance of <see cref="BooksModel"/> containing filter criteria and pagination settings.</param>
        /// <returns>A list of <see cref="BooksModel"/> objects matching the provided criteria.</returns>
        /// 
        #region GetBooksList(BooksModel model)

        public List<BooksModel> GetBooksList(BooksModel model)
        {
            List<BooksModel> bookList = new List<BooksModel>();
            try
            {

                DbCommand cmd = db.GetStoredProcCommand("booksGetList");

                // BookName
                if (!string.IsNullOrEmpty(model.SearchBookName))
                    db.AddInParameter(cmd, "@BookName", DbType.String, model.SearchBookName);
                else
                    db.AddInParameter(cmd, "@BookName", DbType.String, DBNull.Value);

                // PublisherId
                //if (model.PublisherId == 0)
                //    db.AddInParameter(cmd, "PublisherId", DbType.Int32, DBNull.Value);
                //else
                //    db.AddInParameter(cmd, "PublisherId", DbType.Int32, model.PublisherId);

                // PublisherIds 
                if (string.IsNullOrEmpty(model.PublisherIds))
                    db.AddInParameter(cmd, "@PublisherIds", DbType.String, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@PublisherIds", DbType.String, model.PublisherIds);


                // LanguageId
                if (model.LanguageId == 0)
                    db.AddInParameter(cmd, "@LanguageId", DbType.Int32, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@LanguageId", DbType.Int32, model.LanguageId);

                // PageNumber
                db.AddInParameter(cmd, "@PageNumber", DbType.Int32, model.PageNumber);

                // PageSize
                db.AddInParameter(cmd, "@PageSize", DbType.Int32, model.PageSize);

                // IsActive
                if (model.IsActive == null)
                    db.AddInParameter(cmd, "@IsActive", DbType.Boolean, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@IsActive", DbType.Boolean, model.IsActive);

                // SortColumn
                if (!string.IsNullOrEmpty(model.SortColumn))
                    db.AddInParameter(cmd, "@SortColumn", DbType.String, model.SortColumn);
                //else
                //    db.AddInParameter(cmd, "@SortColumn", DbType.String, DBNull.Value);

                // SortDirection
                if (!string.IsNullOrEmpty(model.SortDirection))
                    db.AddInParameter(cmd, "@SortDirection", DbType.String, model.SortDirection);
                //else
                //    db.AddInParameter(cmd, "@SortDirection", DbType.String, DBNull.Value);
                

                DataSet ds = db.ExecuteDataSet(cmd);

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        bookList.Add(new BooksModel
                        {
                            BookId = Convert.ToInt32(row["BookId"]),
                            BookName = row["BookName"].ToString(),
                            LanguageName = row["LanguageName"].ToString(),
                            PublisherName = row["PublisherName"].ToString(),
                            Pages = Convert.ToInt32(row["Pages"]),
                            Cost = Convert.ToDecimal(row["Cost"]),
                            TotalQuantity = Convert.ToInt32(row["TotalQuantity"]),
                            AvailableQuantity = Convert.ToInt32(row["AvailableQuantity"]),
                            IsActive = Convert.ToBoolean(row["IsActive"])
                        });
                    }
                }
                model.TotalRecords = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRecords"]);
            }
            catch (Exception ex)
            {
                Errors.LogErrorToDb(ex, model.CreatedBy);
                Errors.LogErrorToFile(ex);
            }

            return bookList;
        }
        #endregion

    }
}
