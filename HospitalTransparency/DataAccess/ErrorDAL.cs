using HospitalTransparency.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HospitalTransparency.DataAccess
{
    public class ErrorDAL
    {
        #region Method For Error log
        /// <summary>
        /// Inserts the error log.
        /// </summary>
        /// <param name="URL">The URL.</param>
        /// <param name="ErrorMessage">The error message.</param>
        /// <param name="StackTrace">The stack trace.</param>
        /// <returns></returns>
        public bool InsertErrorLog(string URL, string ErrorMessage, string StackTrace)
        {
            SqlParameter[] sqlParam = new SqlParameter[3];
            sqlParam[0] = new SqlParameter("@URL", SqlDbType.VarChar);
            sqlParam[0].Value = URL;
            sqlParam[1] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar);
            sqlParam[1].Value = ErrorMessage;
            sqlParam[2] = new SqlParameter("@StackTrace", SqlDbType.VarChar);
            sqlParam[2].Value = StackTrace;

            return new DatabaseHelper().ExecuteNonQuery("Insert_ErrorLog", sqlParam);
        }
        #endregion
    }
}