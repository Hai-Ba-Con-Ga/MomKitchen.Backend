using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MK.Service.Common
{
    public static class ExceptionExtensions
    {
        public static string GetSqlExceptionMessage(this SqlException ex)
        {
            var lsMessage = new List<string>();
            if (!string.IsNullOrEmpty(ex.Procedure))
            {
                lsMessage.Add("Procedure: " + ex.Procedure);
            }
            if (!string.IsNullOrEmpty(ex.Server))
            {
                int startIndex = ex.Server.Length - 3;
                if (startIndex < 0)
                    startIndex = 0;
                lsMessage.Add("Server: *.*.*." + ex.Server.Substring(startIndex));
            }
            if (!string.IsNullOrEmpty(ex.Message))
            {
                lsMessage.Add("SqlException Message: " + ex.Message);
            }
            if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
            {
                lsMessage.Add("\tInnerException Message: " + ex.InnerException.Message);
                if (ex.InnerException.InnerException != null
                    && !string.IsNullOrEmpty(ex.InnerException.InnerException.Message))
                {
                    lsMessage.Add("\tInnerException Message: " + ex.InnerException.InnerException.Message);
                    if (ex.InnerException.InnerException.InnerException != null
                    && !string.IsNullOrEmpty(ex.InnerException.InnerException.InnerException.Message))
                    {
                        lsMessage.Add("\tInnerException Message: " + ex.InnerException.InnerException.InnerException.Message);
                    }
                }
            }
            return string.Join("\r\n", lsMessage);
        }

        public static string GetExceptionMessage(this Exception ex)
        {
            var lsMessage = new List<string>();
            if (!string.IsNullOrEmpty(ex.Message))
            {
                lsMessage.Add("Exception Message: " + ex.Message);
            }

            if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
            {
                lsMessage.Add("\tInnerException Message: " + ex.InnerException.Message);
                if (ex.InnerException.InnerException != null
                    && !string.IsNullOrEmpty(ex.InnerException.InnerException.Message))
                {
                    lsMessage.Add("\tInnerException Message: " + ex.InnerException.InnerException.Message);
                    if (ex.InnerException.InnerException.InnerException != null
                    && !string.IsNullOrEmpty(ex.InnerException.InnerException.InnerException.Message))
                    {
                        lsMessage.Add("\tInnerException Message: " + ex.InnerException.InnerException.InnerException.Message);
                    }
                }
            }
            return string.Join("\r\n", lsMessage);
        }

        public static string GetWebExceptionMessage(this WebException ex)
        {
            string errorMessage = ex.Message;
            if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
            {
                errorMessage += (" => " + ex.InnerException.Message);
                if (ex.InnerException.InnerException != null
                    && !string.IsNullOrEmpty(ex.InnerException.InnerException.Message))
                {
                    errorMessage += (" => " + ex.InnerException.InnerException.Message);
                    if (ex.InnerException.InnerException.InnerException != null
                    && !string.IsNullOrEmpty(ex.InnerException.InnerException.InnerException.Message))
                    {
                        errorMessage += (" => " + ex.InnerException.InnerException.InnerException.Message);
                    }
                }
            }
            return errorMessage;
        }
    }
}
