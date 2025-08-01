using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library_Mangament_System_API_ClassLibrary.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Library_Mangament_System_API_ClassLibrary.DAL
{
    public class Members
    {
        #region Database
        private Database Db;
        #endregion

        #region Members()
        public Members()
        {
            this.Db = DatabaseFactory.CreateDatabase();
        }
        #endregion

        #region GetMemberList()
        public List<MembersModel> GetMemberList()
        {
            List<MembersModel> listOfMembers = new List<MembersModel>();
            DataSet ds = new DataSet();

            try
            {
                DbCommand cmd = Db.GetStoredProcCommand("membersGetList");
                ds = Db.ExecuteDataSet(cmd);

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        MembersModel member = new MembersModel
                        {
                            MemberId = row["MemberId"] != DBNull.Value ? Convert.ToInt32(row["MemberId"]) : 0,
                            MemberName = row["Name"]?.ToString(),
                            DOB = row["DOB"] != DBNull.Value ? Convert.ToDateTime(row["DOB"]) : DateTime.MinValue,
                            PhoneNo = row["PhoneNo"]?.ToString(),
                            Email = row["Email"]?.ToString(),
                            Address = row["Address"]?.ToString(),
                            GenderId = row["GenderId"] != DBNull.Value ? Convert.ToInt32(row["GenderId"]) : 0,
                            LocationId = row["LocationId"] != DBNull.Value ? Convert.ToInt32(row["LocationId"]) : 0,
                            DepartmentId = row["DepartmentId"] != DBNull.Value ? (int?)Convert.ToInt32(row["DepartmentId"]) : null,
                            CurrentStudyYear = row["CurrentStudyYear"] != DBNull.Value ? Convert.ToInt32(row["CurrentStudyYear"]) : 0,
                            UserTypeId = row["UserTypeId"] != DBNull.Value ? Convert.ToInt32(row["UserTypeId"]) : 0,
                            NoOfIssues = row["NoOfIssues"] != DBNull.Value ? Convert.ToInt32(row["NoOfIssues"]) : 0,
                            CreatedOn = row["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(row["CreatedOn"]) : DateTime.MinValue,
                            ModifiedOn = row["ModifiedOn"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["ModifiedOn"]) : null,
                            CreatedBy = row["CreatedBy"] != DBNull.Value ? Convert.ToInt32(row["CreatedBy"]) : 0,
                            ModifiedBy = row["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(row["ModifiedBy"]) : 0,
                            IsActive = row["IsActive"] != DBNull.Value ? (bool?)Convert.ToBoolean(row["IsActive"]) : null
                        };

                        listOfMembers.Add(member);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving member list: " + ex.Message);
                listOfMembers = null;
            }

            return listOfMembers;
        }
        #endregion
    }
}
