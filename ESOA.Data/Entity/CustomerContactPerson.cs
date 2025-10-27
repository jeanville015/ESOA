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
    public static class CustomerContactPersonData
    {
        private static readonly string errorMessage = "Please verify the information you provided";

        public static CustomerContactPerson FillCustomerContactPerson(SqlDataReader reader)
        {
            return new CustomerContactPerson
            {
                Id = Data.GetGuid(reader["pkid"]),
                CustomerId = Data.GetGuid(reader["customerId"]),
                ContactPerson = Data.GetString(reader["contactPerson"]),
                Designation = Data.GetString(reader["designation"])
            };
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<List<CustomerContactPerson>> GetCustomerContactPersonListAsync(string customerId = null, CancellationToken cancellationToken = default)
        {
            List<CustomerContactPerson> result = new List<CustomerContactPerson>(); 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerContactPerson.ListSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@customerId", customerId);
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {

                        result.Add(FillCustomerContactPerson(reader));
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
        public static async Task<CustomerContactPerson> GetCustomerContactPersonAsync(string customerContactPersonId = null, CancellationToken cancellationToken = default)
        {
            CustomerContactPerson result = null;

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerContactPerson.GetSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@pkid", customerContactPersonId);
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                if (reader.HasRows && await reader.ReadAsync(cancellationToken))
                {
                    result = new CustomerContactPerson
                    {
                        Id = Data.GetGuid(reader["pkid"]),
                        CustomerId = Data.GetGuid(reader["customerId"]),
                        ContactPerson = Data.GetString(reader["contactPerson"]),
                        Designation = Data.GetString(reader["designation"])
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
        public static async Task<ResponseMessage> UpdateCustomerContactPersonAsync(CustomerContactPerson customerContactPerson, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerContactPerson.UpdateSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@pkId", customerContactPerson.Id);
                Data.AddParameter(cmd, "@contactPerson", customerContactPerson.ContactPerson);
                Data.AddParameter(cmd, "@designation", customerContactPerson.Designation);
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
        public static async Task<ResponseMessage> CreateCustomerContactPersonAsync(CustomerContactPerson customerContactPerson, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerContactPerson.InsertSql, conn) { CommandType = CommandType.StoredProcedure };

                Data.AddParameter(cmd, "@customerId", customerContactPerson.CustomerId);
                Data.AddParameter(cmd, "@contactPerson", customerContactPerson.ContactPerson);
                Data.AddParameter(cmd, "@designation", customerContactPerson.Designation);
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
        public static async Task<ResponseMessage> UpdateCustomerContactPersonNVPAsync(NameValuePair nvp, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage() { Status = false };

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerContactPerson.UpdateNvpSql, conn) { CommandType = CommandType.StoredProcedure };
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
