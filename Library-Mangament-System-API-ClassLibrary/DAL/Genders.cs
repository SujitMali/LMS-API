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
    public class Genders
    {
        #region Database
        private Database Db;
        #endregion

        #region Genders()
        public Genders()
        {
            Db = DatabaseFactory.CreateDatabase();
        }
        #endregion

        #region GetGenderList()
        public List<GendersModel> GetGenderList()
        {
            List<GendersModel> genderList = new List<GendersModel>();

            try
            {
                DbCommand cmd = Db.GetStoredProcCommand("gendersGetList");
                using (DataSet ds = Db.ExecuteDataSet(cmd))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        foreach (DataRow row in dt.Rows)
                        {
                            GendersModel gender = new GendersModel
                            {
                                GenderId = row["GenderId"] != DBNull.Value ? Convert.ToInt32(row["GenderId"]) : 0,
                                GenderName = row["GenderName"]?.ToString()
                            };

                            genderList.Add(gender);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.LogErrorToDb(ex, 1);
                Errors.LogErrorToFile(ex);
            }

            return genderList;
        }
        #endregion

    }
}
