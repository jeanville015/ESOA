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
    public static class EncashmentData
    {
        private static readonly string errorMessage = "Please verify the information you provided";

        public static Encashment FillEncashment(SqlDataReader reader)
        {
            return new Encashment
            {
                Id = Data.GetInt(reader["pkid"]),
                Tagging = Data.GetString(reader["tagging"]),
                OfficeCode = Data.GetString(reader["officeCode"]),
                TransactionDate = Data.GetString(reader["transactionDate"]),
                ProductType = Data.GetString(reader["productType"]),
                TrackingNumber = Data.GetString(reader["trackingNumber"]),
                ReferenceNumber = Data.GetString(reader["referenceNumber"]),
                EncashmentBranch = Data.GetString(reader["encashmentBranch"]),
                ShipperName = Data.GetString(reader["shipperName"]),
                ConsigneeName = Data.GetString(reader["consigneeName"]),
                Unit = Data.GetInt(reader["unit"]),
                PrincipalAmount = Data.GetDecimal(reader["principalAmount"]),
                EncashmentDate = Data.GetString(reader["encashmentDate"]),
                StatusCode = Data.GetString(reader["statusCode"]),
                StatusDescription = Data.GetString(reader["statusDescription"]),
                Status = Data.GetString(reader["status"]),
                EncashmentBranchHub = Data.GetString(reader["encashmentBranchHub"]),
                OriginAgentName = Data.GetString(reader["origin_agent_name"]),
                Country = Data.GetString(reader["country"])
            };
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<List<Encashment>> GetEncashmentListAsync(string CustomerNames = null, string DateFrom = null, string DateTo = null, string ProductType = null, string Search=null, CancellationToken cancellationToken = default)
        {
            List<Encashment> result = new List<Encashment>(); 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Encashment.ListSql, conn) { CommandType = CommandType.StoredProcedure };
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
                        result.Add(FillEncashment(reader));
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
        public static async Task<ResponseMessage> CreateEncashmentAsync(Encashment encashment, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Encashment.InsertSql, conn) { CommandType = CommandType.StoredProcedure };

                Data.AddParameter(cmd, "@origin_agent_name", encashment.OriginAgentName);
                Data.AddParameter(cmd, "@tagging", encashment.Tagging);
                Data.AddParameter(cmd, "@officeCode", encashment.OfficeCode);
                Data.AddParameter(cmd, "@transactionDate", encashment.TransactionDate);
                Data.AddParameter(cmd, "@productType", encashment.ProductType);
                Data.AddParameter(cmd, "@trackingNumber", encashment.TrackingNumber);
                Data.AddParameter(cmd, "@referenceNumber", encashment.ReferenceNumber);
                Data.AddParameter(cmd, "@encashmentBranch", encashment.EncashmentBranch);
                Data.AddParameter(cmd, "@shipperName", encashment.ShipperName);
                Data.AddParameter(cmd, "@consigneeName", encashment.ConsigneeName);
                Data.AddParameter(cmd, "@unit", encashment.Unit);
                Data.AddParameter(cmd, "@principalAmount", encashment.PrincipalAmount);
                Data.AddParameter(cmd, "@encashmentDate", encashment.EncashmentDate);
                Data.AddParameter(cmd, "@statusCode", encashment.StatusCode);
                Data.AddParameter(cmd, "@statusDescription", encashment.StatusDescription);
                Data.AddParameter(cmd, "@encashmentBranchHub", encashment.EncashmentBranchHub);
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
