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
    public class UserTypes
    {
        #region Database
        private Database Db;
        #endregion

        #region UserTypes()
        public UserTypes()
        {
            this.Db = DatabaseFactory.CreateDatabase();
        }
        #endregion

        #region GetUserTypeList()
        public List<UserTypesModel> GetUserTypeList()
        {
            List<UserTypesModel> list = new List<UserTypesModel>();
            DataSet Ds = new DataSet();

            try
            {
                DbCommand com = Db.GetStoredProcCommand("userTypesGetList");
                Ds = Db.ExecuteDataSet(com);

                if (Ds != null && Ds.Tables.Count > 0)
                {
                    foreach (DataRow row in Ds.Tables[0].Rows)
                    {
                        list.Add(new UserTypesModel
                        {
                            UserTypeId = Convert.ToInt32(row["UserTypeId"]),
                            UserTypeName = row["UserTypeName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in fetching user types: " + ex.Message);
                return null;
            }

            return list;
        }
        #endregion

    }
}
