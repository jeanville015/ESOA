using ESOA.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ESOA.Common
{
    /// <summary>
    ///  
    /// </summary>
    public static class SOAVerifiedDatesData
    {
        private static readonly string errorMessage = "Please verify the information you provided"; 

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<SOAVerifiedDates> GetSOAVerifiedDatesAsync(string transactionDate = null, string officeCode = null, CancellationToken cancellationToken = default)
        {
            SOAVerifiedDates result = null;
            DateTime dt = DateTime.ParseExact(transactionDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            transactionDate = dt.ToString("yyyy-MM-dd");

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.SOAVerifiedDates.GetSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@officeCode", officeCode);
                Data.AddParameter(cmd, "@transactionDate", transactionDate);
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                if (reader.HasRows && await reader.ReadAsync(cancellationToken))
                {
                    result = new SOAVerifiedDates
                    {
                        Id = Data.GetInt(reader["pkid"]),
                        OfficeCode = Data.GetString(reader["officeCode"]),
                        TransactionDate = Data.GetString(reader["transactionDate"]),
                        Status = Data.GetString(reader["status"]),
                        Remarks = Data.GetString(reader["remarks"])
                    };
                }
            }
            catch (Exception ex)
            {
                //
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<ResponseMessage> CreateSOAVerifiedDatesAsync(SOAVerifiedDates sOAVerifiedDates, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.SOAVerifiedDates.InsertSql, conn) { CommandType = CommandType.StoredProcedure };

                Data.AddParameter(cmd, "@officeCode", sOAVerifiedDates.OfficeCode);
                Data.AddParameter(cmd, "@transactionDate", sOAVerifiedDates.TransactionDate);
                Data.AddParameter(cmd, "@status", sOAVerifiedDates.Status);
                Data.AddParameter(cmd, "@remarks", sOAVerifiedDates.Remarks); 
                //Data.AddOutputParameter(cmd, "@pkid", DbType.Guid);
                result.Total = await cmd.ExecuteNonQueryAsync(cancellationToken);
                //result.Guid = Data.GetGuid(cmd.Parameters["@pkid"].Value);
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Reason = errorMessage;
            }

            return result;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns>Task&lt;ResponseMessage&gt;.</returns>
        public static async Task<ResponseMessage> DeleteSOAVerifiedDatesAsync(string officeCode, string transactionDate = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.SOAVerifiedDates.DeleteSql, conn) { CommandType = CommandType.StoredProcedure };

                Data.AddParameter(cmd, "@officeCode", officeCode);
                Data.AddParameter(cmd, "@transactionDate", transactionDate);

                result.Total = await cmd.ExecuteNonQueryAsync(cancellationToken);
                result.Status = true;
            }
            catch (Exception ex)
            {
    
            }

            return result;
        }



    }
}
