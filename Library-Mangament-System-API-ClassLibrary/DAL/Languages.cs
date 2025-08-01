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
    public class Languages
    {
        #region Database
        private Database Db;
        #endregion

        #region Languages()
        public Languages()
        {
            this.Db = DatabaseFactory.CreateDatabase();
        }
        #endregion

        #region GetLanguageList()
        public List<LanguagesModel> GetLanguageList()
        {
            List<LanguagesModel> listOfLanguages = new List<LanguagesModel>();
            DataSet Ds = new DataSet();

            try
            {
                DbCommand com = Db.GetStoredProcCommand("languagesGetList");
                Ds = Db.ExecuteDataSet(com);

                if (Ds != null && Ds.Tables.Count > 0)
                {
                    DataTable dt = Ds.Tables[0];

                    foreach (DataRow row in dt.Rows)
                    {
                        LanguagesModel lang = new LanguagesModel
                        {
                            LanguageId = row["LanguageId"] != DBNull.Value ? Convert.ToInt32(row["LanguageId"]) : 0,
                            LanguageName = row["LanguageName"] != DBNull.Value ? row["LanguageName"].ToString() : string.Empty
                        };

                        listOfLanguages.Add(lang);
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error in getting the Language List: " + exp.Message);
                listOfLanguages = null;
            }

            return listOfLanguages;
        }
        #endregion

    }
}
