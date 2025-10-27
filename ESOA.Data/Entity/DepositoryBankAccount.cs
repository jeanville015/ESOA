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
    public static class CustomerDepositoryBankAccountData
    {
        private static readonly string errorMessage = "Please verify the information you provided";

        public static CustomerDepositoryBankAccount FillCustomerDepositoryBankAccount(SqlDataReader reader)
        {
            return new CustomerDepositoryBankAccount
            {
                Id = Data.GetGuid(reader["pkid"]),
                AccountNo = Data.GetString(reader["accountNo"]),
                BankName = Data.GetString(reader["bankName"]),
            };
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<List<CustomerDepositoryBankAccount>> GetCustomerDepositoryAccountNoListAsync(string customerId = null, CancellationToken cancellationToken = default)
        {
            List<CustomerDepositoryBankAccount> result = new List<CustomerDepositoryBankAccount>(); 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerDepositoryBankAccount.ListSql, conn) { CommandType = CommandType.StoredProcedure }; 
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {

                        result.Add(FillCustomerDepositoryBankAccount(reader));
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
        public static async Task<CustomerDepositoryBankAccount> GetCustomerDepositoryBankAccountAsync(string customerDepositoryBankAccountId = null, CancellationToken cancellationToken = default)
        {
            CustomerDepositoryBankAccount result = null;

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.CustomerDepositoryBankAccount.GetSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@pkid", customerDepositoryBankAccountId);
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                if (reader.HasRows && await reader.ReadAsync(cancellationToken))
                {
                    result = new CustomerDepositoryBankAccount
                    {
                        Id = Data.GetGuid(reader["pkid"]),
                        AccountNo = Data.GetString(reader["accountNo"]),
                        BankName = Data.GetString(reader["bankName"])
                    };
                }
            }
            catch (Exception ex)
            {
                //
            }

            return result;
        }


    }
}
