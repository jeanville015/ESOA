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
    public static class CustomerDepositoryAccountNoData
    {
        private static readonly string errorMessage = "Please verify the information you provided";

        public static CustomerDepositoryAccountNo FillCustomerDepositoryAccountNo(SqlDataReader reader)
        {
            return new CustomerDepositoryAccountNo
            {
                Id = Data.GetGuid(reader["pkid"]),
                CustomerId = Data.GetGuid(reader["customerId"]),
                DepositoryAccountNo = Data.GetString(reader["depositoryAccountNo"]),
            };
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<List<CustomerDepositoryAccountNo>> GetCustomerDepositoryAccountNoListAsync(string customerId = null, CancellationToken cancellationToken = default)
        {
            List<CustomerDepositoryAccountNo> result = new List<CustomerDepositoryAccountNo>(); 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerDepositoryAccountNo.ListSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@customerId", customerId);
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {

                        result.Add(FillCustomerDepositoryAccountNo(reader));
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
        public static async Task<CustomerDepositoryAccountNo> GetCustomerDepositoryAccountNoAsync(string customerDepositoryAccountNoId = null, CancellationToken cancellationToken = default)
        {
            CustomerDepositoryAccountNo result = null;

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerDepositoryAccountNo.GetSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@pkid", customerDepositoryAccountNoId);
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                if (reader.HasRows && await reader.ReadAsync(cancellationToken))
                {
                    result = new CustomerDepositoryAccountNo
                    {
                        Id = Data.GetGuid(reader["pkid"]),
                        CustomerId = Data.GetGuid(reader["customerId"]),
                        DepositoryAccountNo = Data.GetString(reader["depositoryAccountNo"]),
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
        public static async Task<ResponseMessage> UpdateCustomerDepositoryAccountNoAsync(CustomerDepositoryAccountNo customerDepositoryAccountNo, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerDepositoryAccountNo.UpdateSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@pkId", customerDepositoryAccountNo.Id);
                Data.AddParameter(cmd, "@depositoryAccountNo", customerDepositoryAccountNo.DepositoryAccountNo);
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
        public static async Task<ResponseMessage> CreateCustomerDepositoryAccountNoAsync(CustomerDepositoryAccountNo customerDepositoryAccountNo, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerDepositoryAccountNo.InsertSql, conn) { CommandType = CommandType.StoredProcedure };

                Data.AddParameter(cmd, "@customerId", customerDepositoryAccountNo.CustomerId);
                Data.AddParameter(cmd, "@depositoryAccountNo", customerDepositoryAccountNo.DepositoryAccountNo);
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
        /// Updates the CustomerDepositoryAccountNo NVP.
        /// </summary>
        /// <param name="nvp">The NVP.</param>
        /// <returns>Task&lt;ResponseMessage&gt;.</returns>
        public static async Task<ResponseMessage> UpdateCustomerDepositoryAccountNoNVPAsync(NameValuePair nvp, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage() { Status = false };

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerDepositoryAccountNo.UpdateNvpSql, conn) { CommandType = CommandType.StoredProcedure };
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
