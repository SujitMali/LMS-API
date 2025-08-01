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
    public class Publishers
    {
        #region Database
        private Database Db;
        #endregion

        #region Publishers()
        public Publishers()
        {
            this.Db = DatabaseFactory.CreateDatabase();
        }
        #endregion

        #region GetPublisherList()
        public List<PublishersModel> GetPublisherList()
        {
            List<PublishersModel> listOfPublishers = new List<PublishersModel>();
            DataSet Ds = new DataSet();

            try
            {
                DbCommand com = Db.GetStoredProcCommand("publishersGetList");
                Ds = Db.ExecuteDataSet(com);

                if (Ds != null && Ds.Tables.Count > 0)
                {
                    DataTable dt = Ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        PublishersModel publisher = new PublishersModel
                        {
                            PublisherId = row["PublisherId"] != DBNull.Value ? Convert.ToInt32(row["PublisherId"]) : 0,
                            PublisherName = row["PublisherName"] != DBNull.Value ? row["PublisherName"].ToString() : string.Empty

                        };
                        listOfPublishers.Add(publisher);
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error in Getting the list " + exp);
                listOfPublishers = null;
            }

            return listOfPublishers;
        }
        #endregion

    }
}
