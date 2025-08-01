using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library_Mangament_System_API_ClassLibrary.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Library_Mangament_System_API_ClassLibrary.Helpers;

namespace Library_Mangament_System_API_ClassLibrary.DAL
{
    public class Users
    {
        #region Database
        private Database Db;
        #endregion

        #region Users()
        public Users()
        {
            this.Db = DatabaseFactory.CreateDatabase();
        }
        #endregion

        #region Properties
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? LeavingDate { get; set; }
        public int GenderId { get; set; }
        public int LocationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public int UserTypeId { get; set; }
        public string PasswordHash { get; set; }
        public string Password { get; set; }
        #endregion

        //Implement
        /// <summary>
        /// Loads the details for User.
        /// </summary>
        /// <returns>True if Load operation is successful; Else False.</returns>
        #region Load()
        public bool Load()
        {
            return false;
        }
        #endregion


        /// <summary>
        /// Inserts or Updates the User.
        /// </summary>
        /// <returns>True if Save operation is successful; Else False.</returns>
        #region Save()
        public bool Save()
        {
            if (this.UserId == 0)
            {
                return this.Insert();
            }
            else
            {
                if (this.UserId > 0)
                {
                    return this.Update();
                }
                else
                {
                    this.UserId = 0;
                    return false;
                }
            }
        }
        #endregion


        /// <summary>
        /// Inserts New User.
        /// </summary>
        /// <returns>True if Insert operation is successful; Else False.</returns>
        #region Insert()
        public bool Insert()
        {
            try
            {
                string salt = HashGenerator.GenerateSalt();
                string hashedPassword = HashGenerator.HashPasswordWithSalt(this.Password, salt);

                using (DbCommand cmd = Db.GetStoredProcCommand("usersInsert"))
                {
                    Db.AddInParameter(cmd, "@Name", DbType.String, this.Name);
                    Db.AddInParameter(cmd, "@DOB", DbType.Date, this.DOB);
                    Db.AddInParameter(cmd, "@PhoneNo", DbType.String, this.PhoneNo);
                    Db.AddInParameter(cmd, "@Email", DbType.String, this.Email);
                    Db.AddInParameter(cmd, "@Address", DbType.String, this.Address ?? "");
                    Db.AddInParameter(cmd, "@JoiningDate", DbType.Date, this.JoiningDate);
                    Db.AddInParameter(cmd, "@LeavingDate", DbType.Date, this.LeavingDate ?? (object)DBNull.Value);
                    Db.AddInParameter(cmd, "@GenderId", DbType.Int32, this.GenderId);
                    Db.AddInParameter(cmd, "@LocationId", DbType.Int32, this.LocationId);
                    Db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, this.CreatedBy);
                    Db.AddInParameter(cmd, "@ModifiedBy", DbType.Int32, this.ModifiedBy ?? (object)DBNull.Value);
                    Db.AddInParameter(cmd, "@IsActive", DbType.Boolean, this.IsActive);
                    Db.AddInParameter(cmd, "@UserTypeId", DbType.Int32, this.UserTypeId);
                    Db.AddInParameter(cmd, "@PasswordHash", DbType.String, hashedPassword);
                    Db.AddInParameter(cmd, "@PasswordSalt", DbType.String, salt);

                    Db.AddOutParameter(cmd, "@UserId", DbType.Int32, 4);

                    Db.ExecuteNonQuery(cmd);
                    this.UserId = Convert.ToInt32(Db.GetParameterValue(cmd, "@UserId"));

                    if (this.UserId > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.LogErrorToDb(ex, this.CreatedBy);
                Errors.LogErrorToFile(ex);
            }
            return false;
        }
        #endregion

        //Implement
        /// <summary>
        /// Updates Details of User.
        /// </summary>
        /// <returns>True if Update operation is successful; Else False.</returns>
        #region Update()
        public bool Update()
        {
            return true;
        }
        #endregion


        #region ValidateUser(UsersModel model)
        public UsersModel ValidateUser(UsersModel model)
        {
            UsersModel user = null;

            try
            {
                using (var cmd = Db.GetStoredProcCommand("usersValidateLogin"))
                {
                    Db.AddInParameter(cmd, "@Email", DbType.String, model.Email);

                    using (var reader = Db.ExecuteReader(cmd))
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader["PasswordHash"].ToString();
                            string storedSalt = reader["PasswordSalt"].ToString();
                            string enteredHash = HashGenerator.HashPasswordWithSalt(model.Password, storedSalt);
                            if (storedHash == enteredHash)
                            {
                                user = new UsersModel
                                {
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    Name = reader["Name"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    UserTypeId = Convert.ToInt32(reader["UserTypeId"]),
                                    UserTypeName = reader["UserTypeName"].ToString(),
                                    IsActive = Convert.ToBoolean(reader["IsActive"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.LogErrorToDb(ex, model.CreatedBy);
                Errors.LogErrorToFile(ex);
            }
            return user;
        }
        #endregion

    }
}
