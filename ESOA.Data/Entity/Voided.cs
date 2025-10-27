using ESOA.Model;
using ESOA.Model.Constant;
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
    public static class VoidedData
    {
        private static readonly string errorMessage = "Please verify the information you provided";

        public static Voided FillRefund(SqlDataReader reader)
        {
            return new Voided
            {
                Id = Data.GetInt(reader["pkid"]),
                OriginAgentName = Data.GetString(reader["origin_agent_name"]),
                OfficeCode = Data.GetString(reader["officeCode"]),
                Date = Data.GetString(reader["date"]),
                ProductType = Data.GetString(reader["productType"]),
                TrackingNumber = Data.GetString(reader["trackingNumber"]),
                ReferenceNumber = Data.GetString(reader["referenceNumber"]),
                EntBranch = Data.GetString(reader["entBranch"]),
                ShipperName = Data.GetString(reader["shipperName"]),
                ConsigneeName = Data.GetString(reader["consigneeName"]),
                Unit = Data.GetInt(reader["unit"]),
                PrincipalAmount = Data.GetDecimal(reader["principalAmount"]),
                ServiceFee = Data.GetDecimal(reader["serviceFee"]),
                RefundDate = Data.GetString(reader["refundDate"]),
                StatusCode = Data.GetString(reader["statusCode"]),
                StatusDescription= Data.GetString(reader["statusDescription"]),
                Status= Data.GetString(reader["status"]),
                EncashmentBranchHub = Data.GetString(reader["encashmentBranchHub"]),
                Country = Data.GetString(reader["country"])
            };
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<List<Voided>> GetVoidedListAsync(string CustomerNames = null, string DateFrom = null, string DateTo = null, string ProductType = null, string Search = null, CancellationToken cancellationToken = default)
        {
            List<Voided> result = new List<Voided>(); 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Voided.ListSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@CustomerNames", CustomerNames);
                Data.AddParameter(cmd, "@DateFrom", DateFrom);
                Data.AddParameter(cmd, "@DateTo", DateTo);
                Data.AddParameter(cmd, "@ProductType", ProductType);
                Data.AddParameter(cmd, "@Search", Search);
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    { 
                        result.Add(FillRefund(reader));
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
        public static async Task<ResponseMessage> CreateVoidedAsync(Voided voided, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Voided.InsertSql, conn) { CommandType = CommandType.StoredProcedure };

                Data.AddParameter(cmd, "@origin_agent_name", voided.OriginAgentName);
                Data.AddParameter(cmd, "@officeCode", voided.OfficeCode);
                Data.AddParameter(cmd, "@date", voided.Date);
                Data.AddParameter(cmd, "@productType", voided.ProductType);
                Data.AddParameter(cmd, "@trackingNumber", voided.TrackingNumber);
                Data.AddParameter(cmd, "@referenceNumber", voided.ReferenceNumber);
                Data.AddParameter(cmd, "@entBranch", voided.EntBranch);
                Data.AddParameter(cmd, "@shipperName", voided.ShipperName);
                Data.AddParameter(cmd, "@consigneeName", voided.ConsigneeName);
                Data.AddParameter(cmd, "@unit", voided.Unit);
                Data.AddParameter(cmd, "@principalAmount", voided.PrincipalAmount);
                Data.AddParameter(cmd, "@serviceFee", voided.ServiceFee);
                Data.AddParameter(cmd, "@refundDate", voided.RefundDate);
                Data.AddParameter(cmd, "@statusCode", voided.StatusCode);
                Data.AddParameter(cmd, "@statusDescription", voided.StatusDescription);
                Data.AddParameter(cmd, "@encashmentBranchHub", voided.EncashmentBranchHub);
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
