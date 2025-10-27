using ESOA.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    public static class CustomerContactNoData
    {
        private static readonly string errorMessage = "Please verify the information you provided";

        public static CustomerContactNo FillCustomerContactNo(SqlDataReader reader)
        {
            return new CustomerContactNo
            {
                Id = Data.GetGuid(reader["pkid"]),
                CustomerId = Data.GetGuid(reader["customerId"]),
                ContactNo = Data.GetString(reader["contactNo"]),
            };
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<List<CustomerContactNo>> GetCustomerContactNoListAsync(string customerId = null, CancellationToken cancellationToken = default)
        {
            List<CustomerContactNo> result = new List<CustomerContactNo>(); 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerContactNo.ListSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@customerId", customerId);
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {

                        result.Add(FillCustomerContactNo(reader));
                    }
                }
                while (await reader.NextResultAsync(cancellationToken));
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
        public static async Task<CustomerContactNo> GetCustomerContactNoAsync(string customerContactNoId = null, CancellationToken cancellationToken = default)
        {
            CustomerContactNo result = null;

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerContactNo.GetSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@pkid", customerContactNoId);
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                if (reader.HasRows && await reader.ReadAsync(cancellationToken))
                {
                    result = new CustomerContactNo
                    {
                        Id = Data.GetGuid(reader["pkid"]),
                        CustomerId = Data.GetGuid(reader["customerId"]),
                        ContactNo = Data.GetString(reader["contactNo"]),
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
        public static async Task<ResponseMessage> UpdateCustomerContactNoAsync(CustomerContactNo customerContactNo, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerContactNo.UpdateSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@pkId", customerContactNo.Id);
                Data.AddParameter(cmd, "@contactNo", customerContactNo.ContactNo);
                Data.AddParameter(cmd, "@userAccountId", userAccountId); 
                result.Total = await cmd.ExecuteNonQueryAsync(cancellationToken);
                result.Guid = Data.GetGuid(cmd.Parameters["@pkid"].Value);
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
        /// <returns></returns>
        public static async Task<ResponseMessage> CreateCustomerContactNoAsync(CustomerContactNo customerContactNo, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerContactNo.InsertSql, conn) { CommandType = CommandType.StoredProcedure };

                Data.AddParameter(cmd, "@customerId", customerContactNo.CustomerId);
                Data.AddParameter(cmd, "@contactNo", customerContactNo.ContactNo);
                Data.AddParameter(cmd, "@userAccountId", userAccountId);
                Data.AddOutputParameter(cmd, "@pkid", DbType.Guid);
                result.Total = await cmd.ExecuteNonQueryAsync(cancellationToken);
                result.Guid = Data.GetGuid(cmd.Parameters["@pkid"].Value);
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
        /// Updates the CustomerContactNo NVP.
        /// </summary>
        /// <param name="nvp">The NVP.</param>
        /// <returns>Task&lt;ResponseMessage&gt;.</returns>
        public static async Task<ResponseMessage> UpdateCustomerContactNoNVPAsync(NameValuePair nvp, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage() { Status = false };

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerContactNo.UpdateNvpSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@pkid", nvp.PK);
                Data.AddParameter(cmd, "@name", nvp.Name);
                Data.AddParameter(cmd, "@value", nvp.Value);
                Data.AddParameter(cmd, "@userAccountId", userAccountId);
                result.Total = await cmd.ExecuteNonQueryAsync(cancellationToken);
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Reason = ex.Message;
            }

            return result;
        }


    }
}
