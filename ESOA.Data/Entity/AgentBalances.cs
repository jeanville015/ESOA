using ESOA.Model;
using ESOA.Model.View;
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
    public static class AgentBalancesData
    {
        private static readonly string errorMessage = "Please verify the information you provided";

        //public static AgentBalancesView _FillAgentBalancesPayable(SqlDataReader reader)
        //{  
        //        return new AgentBalancesView
        //        {
        //            CustomerName = Data.GetString(reader["CustomerName"]),
        //            OfficeCode = Data.GetString(reader["OfficeCode"]),
        //            _Amt_Total = Data.GetDecimal(reader["Amt_Total"]),
        //            _Sf_IPP = Data.GetDecimal(reader["Sf_IPP"]),
        //            _Sf_PP_SC = Data.GetDecimal(reader["Sf_PP_SC"]),
        //            _Sf_RTA = Data.GetDecimal(reader["Sf_RTA"]),
        //            _Sf_SNS = Data.GetDecimal(reader["Sf_SNS"]),
        //            _Settlement = Data.GetDecimal(reader["Settlement"])
        //        }; 

        //}

        public static AgentBalancesView FillAgentBalancesPayable(SqlDataReader reader)
        {
            if (
                (
                    0
                    -
                    (
                        (
                            Data.GetDecimal(reader["Amt_Total"])
                            +
                            (
                                Data.GetDecimal(reader["Sf_IPP"]) + Data.GetDecimal(reader["Sf_PP_SC"]) + Data.GetDecimal(reader["Sf_RTA"]) + Data.GetDecimal(reader["Sf_SNS"])
                            )
                            +
                            (
                                ((Data.GetDecimal(reader["Sf_IPP"]) + Data.GetDecimal(reader["Sf_PP_SC"]) + Data.GetDecimal(reader["Sf_RTA"]) + Data.GetDecimal(reader["Sf_SNS"])) / Convert.ToDecimal(1.12)) * Convert.ToDecimal(0.02)
                            )
                        )
                        +
                        Data.GetDecimal(reader["Settlement"])
                    )
                ) < 0
              )
            {
                return new AgentBalancesView
                {
                    CustomerName = Data.GetString(reader["CustomerName"]),
                    OfficeCode = Data.GetString(reader["OfficeCode"]),
                    _Amt_Total = Data.GetDecimal(reader["Amt_Total"]),
                    _Sf_IPP = Data.GetDecimal(reader["Sf_IPP"]),
                    _Sf_PP_SC = Data.GetDecimal(reader["Sf_PP_SC"]),
                    _Sf_RTA = Data.GetDecimal(reader["Sf_RTA"]),
                    _Sf_SNS = Data.GetDecimal(reader["Sf_SNS"]),
                    _Settlement = Data.GetDecimal(reader["Settlement"]),
                    AgentBalances =
                        (
                            0
                            -
                            (
                                (
                                    Data.GetDecimal(reader["Amt_Total"])
                                    +
                                    (
                                        Data.GetDecimal(reader["Sf_IPP"]) + Data.GetDecimal(reader["Sf_PP_SC"]) + Data.GetDecimal(reader["Sf_RTA"]) + Data.GetDecimal(reader["Sf_SNS"])
                                    )
                                    +
                                    (
                                        ((Data.GetDecimal(reader["Sf_IPP"]) + Data.GetDecimal(reader["Sf_PP_SC"]) + Data.GetDecimal(reader["Sf_RTA"]) + Data.GetDecimal(reader["Sf_SNS"])) / Convert.ToDecimal(1.12)) * Convert.ToDecimal(0.02)
                                    )
                                )
                                +
                                Data.GetDecimal(reader["Settlement"])
                            )
                        ).ToString("N")
                };
            }
            else { return null; }

        }

        public static AgentBalancesView FillAgentBalancesAdvance(SqlDataReader reader)
        {
            if (
                (
                    0
                    -
                    (
                        (
                            Data.GetDecimal(reader["Amt_Total"])
                            +
                            (
                                Data.GetDecimal(reader["Sf_IPP"]) + Data.GetDecimal(reader["Sf_PP_SC"]) + Data.GetDecimal(reader["Sf_RTA"]) + Data.GetDecimal(reader["Sf_SNS"])
                            )
                            +
                            (
                                ((Data.GetDecimal(reader["Sf_IPP"]) + Data.GetDecimal(reader["Sf_PP_SC"]) + Data.GetDecimal(reader["Sf_RTA"]) + Data.GetDecimal(reader["Sf_SNS"])) / Convert.ToDecimal(1.12)) * Convert.ToDecimal(0.02)
                            )
                        )
                        +
                        Data.GetDecimal(reader["Settlement"])
                    )
                ) > 0
              )
            {
                return new AgentBalancesView
                {
                    CustomerName = Data.GetString(reader["CustomerName"]),
                    OfficeCode = Data.GetString(reader["OfficeCode"]),
                    _Amt_Total = Data.GetDecimal(reader["Amt_Total"]),
                    _Sf_IPP = Data.GetDecimal(reader["Sf_IPP"]),
                    _Sf_PP_SC = Data.GetDecimal(reader["Sf_PP_SC"]),
                    _Sf_RTA = Data.GetDecimal(reader["Sf_RTA"]),
                    _Sf_SNS = Data.GetDecimal(reader["Sf_SNS"]),
                    _Settlement = Data.GetDecimal(reader["Settlement"]),
                    AgentBalances =
                        (
                            0
                            -
                            (
                                (
                                    Data.GetDecimal(reader["Amt_Total"])
                                    +
                                    (
                                        Data.GetDecimal(reader["Sf_IPP"]) + Data.GetDecimal(reader["Sf_PP_SC"]) + Data.GetDecimal(reader["Sf_RTA"]) + Data.GetDecimal(reader["Sf_SNS"])
                                    )
                                    +
                                    (
                                        ((Data.GetDecimal(reader["Sf_IPP"]) + Data.GetDecimal(reader["Sf_PP_SC"]) + Data.GetDecimal(reader["Sf_RTA"]) + Data.GetDecimal(reader["Sf_SNS"])) / Convert.ToDecimal(1.12)) * Convert.ToDecimal(0.02)
                                    )
                                )
                                +
                                Data.GetDecimal(reader["Settlement"])
                            )
                        ).ToString("N")
                };
            }
            else { return null; }

        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<List<AgentBalancesView>> GetAgentBalancesPayableListAsync(string DateAsOf = null,  CancellationToken cancellationToken = default)
        {
            List<AgentBalancesView> result = new List<AgentBalancesView>(); 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.AgentBalances.ListSql, conn) { CommandType = CommandType.StoredProcedure }; 
                Data.AddParameter(cmd, "@DateAsOf", DateAsOf);
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    { 
                        result.Add(FillAgentBalancesPayable(reader));

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
        public static async Task<List<AgentBalancesView>> GetAgentBalancesAdvanceListAsync(string DateAsOf = null, CancellationToken cancellationToken = default)
        {
            List<AgentBalancesView> result = new List<AgentBalancesView>();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.AgentBalances.ListSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@DateAsOf", DateAsOf);
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        result.Add(FillAgentBalancesAdvance(reader));

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
