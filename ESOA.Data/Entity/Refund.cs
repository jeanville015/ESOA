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
    public static class RefundData
    {
        private static readonly string errorMessage = "Please verify the information you provided";

        public static Refund FillRefund(SqlDataReader reader)
        {
            return new Refund
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
                StatusDescription = Data.GetString(reader["statusDescription"]),
                Status = Data.GetString(reader["status"]),
                EncashmentBranchHub = Data.GetString(reader["encashmentBranchHub"]),
                Country = Data.GetString(reader["country"])
            };
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<List<Refund>> GetRefundListAsync(string CustomerNames = null, string DateFrom = null, string DateTo = null, string ProductType = null, string Search=null, CancellationToken cancellationToken = default)
        {
            List<Refund> result = new List<Refund>(); 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Refund.ListSql, conn) { CommandType = CommandType.StoredProcedure };
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
        public static async Task<ResponseMessage> CreateRefundAsync(Refund refund, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Refund.InsertSql, conn) { CommandType = CommandType.StoredProcedure };

                Data.AddParameter(cmd, "@origin_agent_name", refund.OriginAgentName);
                Data.AddParameter(cmd, "@officeCode", refund.OfficeCode);
                Data.AddParameter(cmd, "@date", refund.Date);
                Data.AddParameter(cmd, "@productType", refund.ProductType);
                Data.AddParameter(cmd, "@trackingNumber", refund.TrackingNumber);
                Data.AddParameter(cmd, "@referenceNumber", refund.ReferenceNumber);
                Data.AddParameter(cmd, "@entBranch", refund.EntBranch);
                Data.AddParameter(cmd, "@shipperName", refund.ShipperName);
                Data.AddParameter(cmd, "@consigneeName", refund.ConsigneeName);
                Data.AddParameter(cmd, "@unit", refund.Unit);
                Data.AddParameter(cmd, "@principalAmount", refund.PrincipalAmount);
                Data.AddParameter(cmd, "@serviceFee", refund.ServiceFee);
                Data.AddParameter(cmd, "@refundDate", refund.RefundDate);
                Data.AddParameter(cmd, "@statusCode", refund.StatusCode);
                Data.AddParameter(cmd, "@statusDescription", refund.StatusDescription);
                Data.AddParameter(cmd, "@encashmentBranchHub", refund.EncashmentBranchHub);
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
