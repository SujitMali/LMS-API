using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Library_Mangament_System_API_ClassLibrary.DAL
{
    public static class Errors
    {
        #region LogErrorToDb(Exception ex, int createdBy)
        public static bool LogErrorToDb(Exception ex, int createdBy)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand cmd = db.GetStoredProcCommand("ErrorLogsInsert");

                db.AddOutParameter(cmd, "@ErrorLogId", DbType.Int32, 0);
                db.AddInParameter(cmd, "@SourceMessage", DbType.String, ex.Message);
                db.AddInParameter(cmd, "@StackTrace", DbType.String, ex.StackTrace);
                db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, createdBy);

                db.ExecuteNonQuery(cmd);

                int errorLogId = Convert.ToInt32(db.GetParameterValue(cmd, "@ErrorLogId"));
                Console.WriteLine("ErrorLogId: " + errorLogId);
                return true;
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error is : " + exp.Message);
                return false;
            }
        }
        #endregion

        #region LogErrorToFile(Exception ex)
        public static void LogErrorToFile(Exception ex)
        {
            string FilePath = ConfigurationManager.AppSettings["LogFilePath"];

            using (StreamWriter ExceptionWriter = new StreamWriter(FilePath, true))
            {
                ExceptionWriter.WriteLine("===========================================Exception=============================================");
                ExceptionWriter.WriteLine($"Date & Time : {DateTime.Now}");
                ExceptionWriter.WriteLine($"Message : {ex.Message}");
                ExceptionWriter.WriteLine($"Source : {ex.Source}");
                ExceptionWriter.WriteLine($"StackTrace : {ex.StackTrace}");
                ExceptionWriter.WriteLine("=================================================================================================");
                ExceptionWriter.WriteLine();
            }

        }
        #endregion

    }
}
