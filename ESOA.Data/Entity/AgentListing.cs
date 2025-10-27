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
    public static class AgentListingData
    {
        private static readonly string errorMessage = "Please verify the information you provided";

        public static AgentListingView FillAgentListingView(SqlDataReader reader)
        {
            return new AgentListingView
            {
                Name = Data.GetString(reader["name"]),
                OfficeCode = Data.GetString(reader["officeCode"]),
                Status = Data.GetString(reader["status"])
            };
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<List<AgentListingView>> GetAgentListingViewListAsync(CancellationToken cancellationToken = default)
        {
            List<AgentListingView> result = new List<AgentListingView>(); 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Customer.ListSql, conn) { CommandType = CommandType.StoredProcedure };
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {

                        result.Add(FillAgentListingView(reader));
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
