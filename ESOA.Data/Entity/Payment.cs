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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ESOA.Common
{
    /// <summary>
    ///  
    /// </summary>
    public static class PaymentData
    {
        private static readonly string errorMessage = "Please verify the information you provided";

        public static Payment FillPayment(SqlDataReader reader)
        {
            return new Payment
            {
                Id = Data.GetInt(reader["pkid"]),
                UploadedBy = Data.GetString(reader["uploadedBy"]),
                Date = Data.GetString(reader["date"]),
                OriginAgentName = Data.GetString(reader["origin_agent_name"]),
                CustomerId = Data.GetString(reader["customerId"]),
                BankAccount = Data.GetString(reader["bankAccount"]),
                BankAccountGLCode = Data.GetString(reader["bankAccountGLCode"]),
                USDPayment = Data.GetDecimal(reader["USDPayment"]),
                ExcRate = Data.GetDecimal(reader["excRate"]),
                PHPPayment = Data.GetDecimal(reader["PHPPayment"]),
                Assignment = Data.GetString(reader["assignment"]),
                Text = Data.GetString(reader["text"]),
                SAPDocNumber = Data.GetString(reader["SAPDocNumber"])
            };
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<List<Payment>> GetPaymentListAsync(string CustomerNames = null, string DateFrom = null, string DateTo = null, CancellationToken cancellationToken = default)
        {
            List<Payment> result = new List<Payment>(); 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Payment.ListSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@CustomerNames", CustomerNames);
                Data.AddParameter(cmd, "@DateFrom", DateFrom);
                Data.AddParameter(cmd, "@DateTo", DateTo);
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {

                        result.Add(FillPayment(reader));
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
        public static async Task<ResponseMessage> CreatePaymentAsync(Payment payment, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Payment.InsertSql, conn) { CommandType = CommandType.StoredProcedure };

                Data.AddParameter(cmd, "@uploadedBy", payment.UploadedBy);
                Data.AddParameter(cmd, "@date", payment.Date);
                Data.AddParameter(cmd, "@origin_agent_name", payment.OriginAgentName);
                Data.AddParameter(cmd, "@customerId", payment.CustomerId);
                Data.AddParameter(cmd, "@bankAccount", payment.BankAccount);
                Data.AddParameter(cmd, "@bankAccountGLCode", payment.BankAccountGLCode);
                Data.AddParameter(cmd, "@USDPayment", payment.USDPayment);
                Data.AddParameter(cmd, "@excRate", payment.ExcRate);
                Data.AddParameter(cmd, "@PHPPayment", payment.PHPPayment);
                Data.AddParameter(cmd, "@assignment", payment.Assignment);
                Data.AddParameter(cmd, "@text", payment.Text);
                Data.AddParameter(cmd, "@SAPDocNumber", payment.SAPDocNumber);
                Data.AddOutputParameter(cmd, "@pkid", DbType.Int32);
                result.Total = await cmd.ExecuteNonQueryAsync(cancellationToken);
                result.IntGuid = Data.GetInt(cmd.Parameters["@pkid"].Value);
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Reason = errorMessage;
            }

            return result;
        }

    }
}
