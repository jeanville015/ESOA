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
    public static class AcceptanceData
    {
        private static readonly string errorMessage = "Please verify the information you provided";

        public static Acceptance FillAcceptance(SqlDataReader reader)
        {
            return new Acceptance
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
                Str_PrincipalAmount = (Data.GetDecimal(reader["principalAmount"])).ToString("N"),
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
        public static async Task<List<Acceptance>> GetAcceptanceListAsync(string CustomerNames = null, string DateFrom = null, string DateTo = null, string ProductType = null, string Search=null, CancellationToken cancellationToken = default)
        {
            List<Acceptance> result = new List<Acceptance>(); 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Acceptance.ListSql, conn) { CommandType = CommandType.StoredProcedure };
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
                        result.Add(FillAcceptance(reader));
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
        public static async Task<ResponseMessage> CreateAcceptanceAsync(Acceptance acceptance, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Acceptance.InsertSql, conn) { CommandType = CommandType.StoredProcedure };

                Data.AddParameter(cmd, "@origin_agent_name", acceptance.OriginAgentName);
                Data.AddParameter(cmd, "@tagging", acceptance.Tagging);
                Data.AddParameter(cmd, "@officeCode", acceptance.OfficeCode);
                Data.AddParameter(cmd, "@transactionDate", acceptance.TransactionDate);
                Data.AddParameter(cmd, "@productType", acceptance.ProductType);
                Data.AddParameter(cmd, "@trackingNumber", acceptance.TrackingNumber);
                Data.AddParameter(cmd, "@referenceNumber", acceptance.ReferenceNumber);
                Data.AddParameter(cmd, "@encashmentBranch", acceptance.EncashmentBranch);
                Data.AddParameter(cmd, "@shipperName", acceptance.ShipperName);
                Data.AddParameter(cmd, "@consigneeName", acceptance.ConsigneeName);
                Data.AddParameter(cmd, "@unit", acceptance.Unit);
                Data.AddParameter(cmd, "@principalAmount", acceptance.PrincipalAmount);
                Data.AddParameter(cmd, "@encashmentDate", acceptance.EncashmentDate);
                Data.AddParameter(cmd, "@statusCode", acceptance.StatusCode);
                Data.AddParameter(cmd, "@statusDescription", acceptance.StatusDescription);
                Data.AddParameter(cmd, "@encashmentBranchHub", acceptance.EncashmentBranchHub);
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
