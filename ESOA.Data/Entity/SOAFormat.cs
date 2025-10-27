using ESOA.Model;
using ESOA.Model.Entity.SOAFormatA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ESOA.Common.Scripts;

namespace ESOA.Common
{
    /// <summary>
    ///  
    /// </summary>
    public static class SOAFormatData
    {
        private static readonly string errorMessage = "Please verify the information you provided";

        public static SOAFormat FillSOAFormat(SqlDataReader reader)
        {
            return new SOAFormat
            {

                Id = Data.GetGuid(reader["pkid"]),
                FormatName = Data.GetString(reader["formatName"])
            };
        }

        public static SOAFormatAData FillSOAFormatA(SqlDataReader reader)
        {
            return new SOAFormatAData
            {
                
                Date = Data.GetString(reader["Date"]),
                Unit_IPP = Data.GetInt(reader["Unit_IPP"]),
                Unit_PP_SC = Data.GetInt(reader["Unit_PP_SC"]),
                Unit_RTA = Data.GetInt(reader["Unit_RTA"]),
                Unit_SNS = Data.GetInt(reader["Unit_SNS"]),
                Amt_IPP = Data.GetDecimal(reader["Amt_IPP"]),
                Amt_PP_SC = Data.GetDecimal(reader["Amt_PP_SC"]),
                Amt_RTA = Data.GetDecimal(reader["Amt_RTA"]),
                Amt_SNS = Data.GetDecimal(reader["Amt_SNS"]),
                Amt_Total = Data.GetDecimal(reader["Amt_Total"]),
                Sf_IPP = Data.GetDecimal(reader["Sf_IPP"]),
                Sf_PP_SC = Data.GetDecimal(reader["Sf_PP_SC"]),
                Sf_RTA = Data.GetDecimal(reader["Sf_RTA"]),
                Sf_SNS = Data.GetDecimal(reader["Sf_SNS"]),
                Sf_Total = Data.GetDecimal(reader["Sf_Total"]),
                WithholdingTax = Data.GetDecimal(reader["WithholdingTax"]),
                TotalLBCReceivable = Data.GetDecimal(reader["TotalLBCReceivable"]),
                Rv_IPP = Data.GetDecimal(reader["Rv_IPP"]),
                Rv_PP_SC = Data.GetDecimal(reader["Rv_PP_SC"]),
                Rv_RTA = Data.GetDecimal(reader["Rv_RTA"]),
                Rv_SNS = Data.GetDecimal(reader["Rv_SNS"]),
                Settlement = Data.GetDecimal(reader["Settlement"]),
                RunningBalance = Data.GetDecimal(reader["RunningBalance"]),
                BalancePerAgent = Data.GetDecimal(reader["BalancePerAgent"]),
                Variance = Data.GetDecimal(reader["Variance"]),
                AcceptanceDocNumber = Data.GetString(reader["AcceptanceDocNumber"]),
                ServiceFeeDocNumber = Data.GetString(reader["ServiceFeeDocNumber"])
            };
        }

        public static SOAFormatB FillSOAFormatB(SqlDataReader reader)
        {
            return new SOAFormatB
            {

                Date = Data.GetString(reader["Date"]),
                Unit_IPP = Data.GetInt(reader["Unit_IPP"]),
                Unit_PP_SC = Data.GetInt(reader["Unit_PP_SC"]),
                Unit_RTA = Data.GetInt(reader["Unit_RTA"]),
                Unit_SNS = Data.GetInt(reader["Unit_SNS"]),
                Amt_IPP = Data.GetDecimal(reader["Amt_IPP"]),
                Amt_PP_SC = Data.GetDecimal(reader["Amt_PP_SC"]),
                Amt_RTA = Data.GetDecimal(reader["Amt_RTA"]),
                Amt_SNS = Data.GetDecimal(reader["Amt_SNS"]),
                Amt_Total = Data.GetDecimal(reader["Amt_Total"]),
                Sf_IPP = Data.GetDecimal(reader["Sf_IPP"]),
                Sf_PP_SC = Data.GetDecimal(reader["Sf_PP_SC"]),
                Sf_RTA = Data.GetDecimal(reader["Sf_RTA"]),
                Sf_SNS = Data.GetDecimal(reader["Sf_SNS"]),
                Sf_Total = Data.GetDecimal(reader["Sf_Total"]),
                WithholdingTax = Data.GetDecimal(reader["WithholdingTax"]),
                Total = Data.GetDecimal(reader["Total"]),
                Rv_IPP = Data.GetDecimal(reader["Rv_IPP"]),
                Rv_PP_SC = Data.GetDecimal(reader["Rv_PP_SC"]),
                Rv_RTA = Data.GetDecimal(reader["Rv_RTA"]),
                Rv_SNS = Data.GetDecimal(reader["Rv_SNS"]),
                Settlement = Data.GetDecimal(reader["Settlement"]),
                RunningBalance = Data.GetDecimal(reader["RunningBalance"]),
                BalancePerAgent = Data.GetDecimal(reader["BalancePerAgent"]),
                Variance = Data.GetDecimal(reader["Variance"]),
                AcceptanceDocNumber = Data.GetString(reader["AcceptanceDocNumber"]),
                ServiceFeeDocNumber = Data.GetString(reader["ServiceFeeDocNumber"])
            };
        }


        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<List<SOAFormat>> GetSOAFormatListAsync(string CustomerNames = null, string DateFrom = null, string DateTo = null, CancellationToken cancellationToken = default)
        {
            List<SOAFormat> result = new List<SOAFormat>();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.SoaFormat.ListSql, conn) { CommandType = CommandType.StoredProcedure }; 
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {

                        result.Add(FillSOAFormat(reader));
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
        public static async Task<List<SOAFormatAData>> GetSOAFormatAListAsync(string CustomerNames = null, string DateFrom = null, string DateTo = null, string beginningBalance=null, CancellationToken cancellationToken = default)
        {
            List<SOAFormatAData> result = new List<SOAFormatAData>();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.SoaFormatA.ListSql, conn) { CommandType = CommandType.StoredProcedure };
                cmd.CommandTimeout = 0;
                Data.AddParameter(cmd, "@CustomerNames", CustomerNames);
                Data.AddParameter(cmd, "@DateFrom", DateFrom);
                Data.AddParameter(cmd, "@DateTo", DateTo);
                Data.AddParameter(cmd, "@BeginningBalance", beginningBalance);
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {

                        result.Add(FillSOAFormatA(reader));
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
        public static async Task<SOAFormatAPreviousBalanceVariables> GetSOAFormatAPreviousBalanceVariables(string CustomerName = null, string DateFrom = null, CancellationToken cancellationToken = default)
        {
            SOAFormatAPreviousBalanceVariables result = new SOAFormatAPreviousBalanceVariables();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.SoaFormatA.GetPrevBalVars, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@CustomerName", CustomerName);
                Data.AddParameter(cmd, "@DateFrom", DateFrom);
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {

                        result.TotalLBCReceivable = Data.GetDecimal(reader["TotalRecievable"]);
                        result.Settlement = Data.GetDecimal(reader["Settlement"]);
                        result.All_RVProducttype_Total = Data.GetDecimal(reader["All_RVProductype_Total"]);
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
        public static async Task<List<SOAFormatB>> GetSOAFormatBListAsync(string CustomerNames = null, string DateFrom = null, string DateTo = null, string beginningBalance = null, CancellationToken cancellationToken = default)
        {
            List<SOAFormatB> result = new List<SOAFormatB>();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.SoaFormatB.ListSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@CustomerNames", CustomerNames);
                Data.AddParameter(cmd, "@DateFrom", DateFrom);
                Data.AddParameter(cmd, "@DateTo", DateTo);
                Data.AddParameter(cmd, "@BeginningBalance", beginningBalance);
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {

                        result.Add(FillSOAFormatB(reader));
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

    }
}
