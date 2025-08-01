using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Library_Mangament_System_API_ClassLibrary.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Library_Mangament_System_API_ClassLibrary.DAL
{
    public class Locations
    {
        #region Database
        private Database Db;
        #endregion

        #region Locations()
        public Locations()
        {
            this.Db = DatabaseFactory.CreateDatabase();
        }
        #endregion

        #region GetLocationList()
        public List<LocationsModel> GetLocationList()
        {
            List<LocationsModel> list = new List<LocationsModel>();
            DataSet ds = new DataSet();

            try
            {
                DbCommand cmd = Db.GetStoredProcCommand("locationsGetList");
                ds = Db.ExecuteDataSet(cmd);

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        list.Add(new LocationsModel
                        {
                            LocationId = Convert.ToInt32(row["LocationId"]),
                            LocationName = row["LocationName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return list;
        }
        #endregion
    }
}
