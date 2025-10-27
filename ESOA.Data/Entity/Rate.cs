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
    public static class RateData
    {
        private static readonly string errorMessage = "Please verify the information you provided";

        public static Rate FillRate(SqlDataReader reader)
        {
            return new Rate
            {
                Id = Data.GetGuid(reader["pkid"]),
                Reference = Data.GetString(reader["reference"]),
                RateType_IPP = Data.GetString(reader["rateType_ipp"]),
                RateType_PP_SC = Data.GetString(reader["rateType_pp_sc"]),
                RateType_RTA = Data.GetString(reader["rateType_rta"]),
                RateType_SNS = Data.GetString(reader["rateType_sns"]),
                RateType_IPPX = Data.GetString(reader["rateType_ippx"]),
                IPP = Data.GetDecimal(reader["ipp"]),
                PP_SC = Data.GetDecimal(reader["pp_sc"]),
                RTA = Data.GetDecimal(reader["rta"]),
                SNS = Data.GetDecimal(reader["sns"]),
                IPPX = Data.GetDecimal(reader["ippx"]),
                From = Data.GetString(reader["from"]),
                To = Data.GetString(reader["to"]),
                //Created = Data.GetDateTime(reader["created"]),
                //Updated = Data.GetDateTime(reader["updated"]),
                //UpdatedBy = Data.GetGuid(reader["updatedBy"])
            };
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<List<Rate>> GetRateListAsync(string userAccountId = null, CancellationToken cancellationToken = default)
        {
            List<Rate> result = new List<Rate>(); 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Rate.ListSql, conn) { CommandType = CommandType.StoredProcedure };
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {

                        result.Add(FillRate(reader));
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
        /// <param name="pagination">Pagination model</param>
        /// <returns></returns>
        public static async Task<PaginationResponse<Rate>> RateSearchAsync(RateSearchRequest pagination, CancellationToken cancellationToken = default)
        {
            PaginationResponse<Rate> result = null;

            pagination.FilterTerm = pagination.FilterTerm.EditFilterTermString();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                List<Rate> Rates = new List<Rate>();
                await using var cmd = new SqlCommand(Scripts.Rate.ListSqlPaginated, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@filterTerm", pagination.FilterTerm);
                Data.AddParameter(cmd, "@sortIndex", pagination.SortIndex);
                Data.AddParameter(cmd, "@sortDirection", pagination.SortDirection);
                Data.AddParameter(cmd, "@startRowNum", pagination.StartRowNumber);
                Data.AddParameter(cmd, "@endRowNum", pagination.EndRowNumber);
                Data.AddOutputParameter(cmd, "@totalRows", DbType.Int32);
                Data.AddOutputParameter(cmd, "@filteredRowsCount", DbType.Int32);

                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                {
                    do
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            Rates.Add(new Rate
                            {
                                Id = Data.GetGuid(reader["pkid"]),
                                Reference = Data.GetString(reader["reference"]),
                                RateType_IPP = Data.GetString(reader["rateType_ipp"]),
                                RateType_PP_SC = Data.GetString(reader["rateType_pp_sc"]),
                                RateType_RTA = Data.GetString(reader["rateType_rta"]),
                                RateType_SNS = Data.GetString(reader["rateType_sns"]),
                                RateType_IPPX = Data.GetString(reader["rateType_ippx"]),
                                IPP = Data.GetDecimal(reader["ipp"]),
                                PP_SC = Data.GetDecimal(reader["pp_sc"]),
                                RTA = Data.GetDecimal(reader["rta"]),
                                SNS = Data.GetDecimal(reader["sns"]),
                                IPPX = Data.GetDecimal(reader["ippx"]),
                                From = Data.GetString(reader["from"]),
                                To = Data.GetString(reader["to"])
                            });
                        }
                    }
                    while (await reader.NextResultAsync(cancellationToken));
                    
                }
                

                result = new PaginationResponse<Rate>
                {
                    Paging = pagination,
                    TotalRows = Data.GetInt(cmd.Parameters["@TotalRows"].Value),
                    FilteredRows = Data.GetInt(cmd.Parameters["@filteredRowsCount"].Value),
                    Data = Rates
                };
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
        public static async Task<Rate> GetRateAsync(string rateId = null, CancellationToken cancellationToken = default)
        {
            Rate result = null;

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Rate.GetSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@pkid", rateId);
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                if (reader.HasRows && await reader.ReadAsync(cancellationToken))
                {
                    result = new Rate
                    {
                        Id = Data.GetGuid(reader["pkid"]),
                        Reference = Data.GetString(reader["reference"]),
                        RateType_IPP = Data.GetString(reader["rateType_ipp"]),
                        RateType_PP_SC = Data.GetString(reader["rateType_pp_sc"]),
                        RateType_RTA = Data.GetString(reader["rateType_rta"]),
                        RateType_SNS = Data.GetString(reader["rateType_sns"]),
                        RateType_IPPX = Data.GetString(reader["rateType_ippx"]),
                        IPP = Data.GetDecimal(reader["ipp"]),
                        PP_SC = Data.GetDecimal(reader["pp_sc"]),
                        RTA = Data.GetDecimal(reader["rta"]),
                        SNS = Data.GetDecimal(reader["sns"]),
                        IPPX = Data.GetDecimal(reader["ippx"]),
                        From = Data.GetString(reader["from"]),
                        To = Data.GetString(reader["to"])
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
        public static async Task<ResponseMessage> UpdateRateAsync(Rate rate, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Rate.UpdateSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@pkId", rate.Id);
                Data.AddParameter(cmd, "@reference", rate.Reference);
                Data.AddParameter(cmd, "@rateType_ipp", rate.RateType_IPP);
                Data.AddParameter(cmd, "@rateType_pp_sc", rate.RateType_PP_SC);
                Data.AddParameter(cmd, "@rateType_rta", rate.RateType_RTA);
                Data.AddParameter(cmd, "@rateType_sns", rate.RateType_SNS);
                Data.AddParameter(cmd, "@rateType_ippx", rate.RateType_IPPX);
                Data.AddParameter(cmd, "@ipp", rate.IPP);
                Data.AddParameter(cmd, "@pp_sc", rate.PP_SC);
                Data.AddParameter(cmd, "@rta", rate.RTA);
                Data.AddParameter(cmd, "@sns", rate.SNS);
                Data.AddParameter(cmd, "@ippx", rate.IPPX);
                Data.AddParameter(cmd, "@from", rate.From);
                Data.AddParameter(cmd, "@to", rate.To);
                Data.AddParameter(cmd, "@userAccountId", userAccountId);

                result.Total = await cmd.ExecuteNonQueryAsync(cancellationToken);
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
        public static async Task<ResponseMessage> CreateRateAsync(Rate rate, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Rate.InsertSql, conn) { CommandType = CommandType.StoredProcedure };

                Data.AddParameter(cmd, "@reference", rate.Reference);
                Data.AddParameter(cmd, "@rateType_ipp", rate.RateType_IPP);
                Data.AddParameter(cmd, "@rateType_pp_sc", rate.RateType_PP_SC);
                Data.AddParameter(cmd, "@rateType_rta", rate.RateType_RTA);
                Data.AddParameter(cmd, "@rateType_sns", rate.RateType_SNS);
                Data.AddParameter(cmd, "@rateType_ippx", rate.RateType_IPPX);
                Data.AddParameter(cmd, "@ipp", rate.IPP);
                Data.AddParameter(cmd, "@pp_sc", rate.PP_SC);
                Data.AddParameter(cmd, "@rta", rate.RTA);
                Data.AddParameter(cmd, "@sns", rate.SNS);
                Data.AddParameter(cmd, "@ippx", rate.IPPX);
                Data.AddParameter(cmd, "@from", rate.From);
                Data.AddParameter(cmd, "@to", rate.To);
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
        ///  
        /// </summary>
        /// <param name="nvp">The NVP.</param>
        /// <returns>Task&lt;ResponseMessage&gt;.</returns>
        public static async Task<ResponseMessage> UpdateRateNVPAsync(NameValuePair nvp, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage() { Status = false }; 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Rate.UpdateNvpSql, conn) { CommandType = CommandType.StoredProcedure };
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

        public static async Task<List<Rate_Excel>> GetRequestExcelList(CancellationToken cancellationToken = default)
        {
            List<Rate_Excel> result = new List<Rate_Excel>();
            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Rate.ListSqlExcel, conn) { CommandType = CommandType.StoredProcedure };
                //added fix when viewing 100+ records
                cmd.CommandTimeout = 0;
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                {
                    do
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            result.Add(new Rate_Excel
                            {
                                Id = Data.GetGuid(reader["pkid"]),
                                Reference = Data.GetString(reader["reference"]),
                                IPP = Data.GetDecimal(reader["ipp"]),
                                RateType_IPP = Data.GetString(reader["rateType_ipp"]),
                                PP_SC = Data.GetDecimal(reader["pp_sc"]),
                                RateType_PP_SC = Data.GetString(reader["rateType_pp_sc"]),
                                RTA = Data.GetDecimal(reader["rta"]),
                                RateType_RTA = Data.GetString(reader["rateType_rta"]),
                                SNS = Data.GetDecimal(reader["sns"]),
                                RateType_SNS = Data.GetString(reader["rateType_sns"]),
                                IPPX = Data.GetDecimal(reader["ippx"]),
                                RateType_IPPX = Data.GetString(reader["rateType_ippx"]),
                                From = Data.GetString(reader["from"]),
                                To = Data.GetString(reader["to"])
                            });
                        }
                    }
                    while (await reader.NextResultAsync(cancellationToken));
                }
            }
            catch (Exception ex)
            {
                //applogger
            }
            return result;
        }

    }
}
