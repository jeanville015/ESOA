using ESOA.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ESOA.Common
{
    /// <summary>
    ///  
    /// </summary>
    public static class UserAccountData
    {
        private static readonly string errorMessage = "Please verify the information you provided";

        public static UserAccount FillUserAccounts(SqlDataReader reader)
        {
            return new UserAccount
            {
                Id = Data.GetGuid(reader["pkid"]),
                Name = Data.GetString(reader["name"]),
                JobTitle = Data.GetString(reader["jobTitle"]),
                Team = Data.GetString(reader["team"]),
                Role = Data.GetString(reader["role"]),
                ModuleAccess_Admin = Data.GetBool(reader["moduleAccess_admin"]),
                ModuleAccess_SOA = Data.GetBool(reader["moduleAccess_soa"]),
                ModuleAccess_Payment = Data.GetBool(reader["moduleAccess_payment"]),
                ModuleAccess_Reports = Data.GetBool(reader["moduleAccess_reports"]),
                ModuleAccess_Granular = Data.GetBool(reader["moduleAccess_granular"]),
                AccessRights_Admin = Data.GetBool(reader["accessRights_admin"]),
                AccessRights_SOA = Data.GetBool(reader["accessRights_soa"]),
                AccessRights_Payment = Data.GetBool(reader["accessRights_payment"]),
                AccessRights_Reports = Data.GetBool(reader["accessRights_reports"]),
                AccessRights_Granular = Data.GetBool(reader["accessRights_granular"]),
                EmailAddress = Data.GetString(reader["emailAddress"]),
                ContactNo = Data.GetString(reader["contactNo"]),
                Created = Data.GetDateTime(reader["created"]),
                Updated = Data.GetDateTime(reader["updated"]),
                UpdatedBy = Data.GetGuid(reader["updatedBy"])
            };
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<List<UserAccount>> GetUserAccountListAsync(string userAccountId = null, CancellationToken cancellationToken = default)
        {
            List<UserAccount> result = new List<UserAccount>(); 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.UserAccount.ListSql, conn) { CommandType = CommandType.StoredProcedure };
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {

                        result.Add(FillUserAccounts(reader));
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
        public static async Task<PaginationResponse<UserAccount>> UserAccountSearchAsync(UserAccountSearchRequest pagination, CancellationToken cancellationToken = default)
        {
            PaginationResponse<UserAccount> result = null;

            pagination.FilterTerm = pagination.FilterTerm.EditFilterTermString();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                List<UserAccount> userAccounts = new List<UserAccount>();
                await using var cmd = new SqlCommand(Scripts.UserAccount.ListSqlPaginated, conn) { CommandType = CommandType.StoredProcedure };
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
                            userAccounts.Add(new UserAccount
                            {
                                Id = Data.GetGuid(reader["pkid"]),
                                Name = Data.GetString(reader["name"]),
                                JobTitle = Data.GetString(reader["jobTitle"]),
                                Team = Data.GetString(reader["team"]),
                                Role = Data.GetString(reader["role"]),
                                ModuleAccess_Admin = Data.GetBool(reader["moduleAccess_admin"]),
                                ModuleAccess_SOA = Data.GetBool(reader["moduleAccess_soa"]),
                                ModuleAccess_Payment = Data.GetBool(reader["moduleAccess_payment"]),
                                ModuleAccess_Reports = Data.GetBool(reader["moduleAccess_reports"]),
                                ModuleAccess_Granular = Data.GetBool(reader["moduleAccess_granular"]),
                                AccessRights_Admin = Data.GetBool(reader["accessRights_admin"]),
                                AccessRights_SOA = Data.GetBool(reader["accessRights_soa"]),
                                AccessRights_Payment = Data.GetBool(reader["accessRights_payment"]),
                                AccessRights_Reports = Data.GetBool(reader["accessRights_reports"]),
                                AccessRights_Granular = Data.GetBool(reader["accessRights_granular"]),
                                EmailAddress = Data.GetString(reader["emailAddress"]),
                                ContactNo = Data.GetString(reader["contactNo"]),
                                Created = Data.GetDateTime(reader["created"]),
                                Updated = Data.GetDateTime(reader["updated"]),
                                UpdatedBy_Name = Data.GetString(reader["updatedBy_Name"])
                            });
                        }
                    }
                    while (await reader.NextResultAsync(cancellationToken));
                }
                

                result = new PaginationResponse<UserAccount>
                {
                    Paging = pagination,
                    TotalRows = Data.GetInt(cmd.Parameters["@TotalRows"].Value),
                    FilteredRows = Data.GetInt(cmd.Parameters["@filteredRowsCount"].Value),
                    Data = userAccounts
                };
            }
            catch (Exception ex)
            {
                // 
            }

            return result;
        }

        /// <summary>
        /// Get UserAccounts by Id OR by UserName & Password
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<UserAccount> GetUserAccountAsync(string userAccountId = null, string email = null, string password = null, string checkEmail = null, CancellationToken cancellationToken = default)
        {
            UserAccount result = null;

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.UserAccount.GetSql, conn) { CommandType = CommandType.StoredProcedure }; 
                Data.AddParameter(cmd, "@pkid", userAccountId);
                Data.AddParameter(cmd, "@email", email);
                Data.AddParameter(cmd, "@password", password);
                Data.AddParameter(cmd, "@checkEmail", checkEmail);
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                if (reader.HasRows && await reader.ReadAsync(cancellationToken))
                {
                    result = new UserAccount
                    {
                        Id = Data.GetGuid(reader["pkid"]),
                        Name = Data.GetString(reader["name"]),
                        JobTitle = Data.GetString(reader["jobTitle"]),
                        Team = Data.GetString(reader["team"]),
                        Role = Data.GetString(reader["role"]),
                        ModuleAccess_Admin = Data.GetBool(reader["moduleAccess_admin"]),
                        ModuleAccess_SOA = Data.GetBool(reader["moduleAccess_soa"]),
                        ModuleAccess_Payment = Data.GetBool(reader["moduleAccess_payment"]),
                        ModuleAccess_Reports = Data.GetBool(reader["moduleAccess_reports"]),
                        ModuleAccess_Granular = Data.GetBool(reader["moduleAccess_granular"]),
                        AccessRights_Admin = Data.GetBool(reader["accessRights_admin"]),
                        AccessRights_SOA = Data.GetBool(reader["accessRights_soa"]),
                        AccessRights_Payment = Data.GetBool(reader["accessRights_payment"]),
                        AccessRights_Reports = Data.GetBool(reader["accessRights_reports"]),
                        AccessRights_Granular = Data.GetBool(reader["accessRights_granular"]),
                        EmailAddress = Data.GetString(reader["emailAddress"]),
                        ContactNo = Data.GetString(reader["contactNo"]),
                        Created = Data.GetDateTime(reader["created"]),
                        Updated = Data.GetDateTime(reader["updated"]),
                        UpdatedBy = Data.GetGuid(reader["updatedBy"])
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
        public static async Task<ResponseMessage> UpdateUserAccountAsync(UserAccount userAccount, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.UserAccount.UpdateSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@pkId", userAccount.Id);
                Data.AddParameter(cmd, "@name", userAccount.Name);
                Data.AddParameter(cmd, "@jobTitle", userAccount.JobTitle);
                Data.AddParameter(cmd, "@team", userAccount.Team);
                Data.AddParameter(cmd, "@role", userAccount.Role);
                Data.AddParameter(cmd, "@moduleAccess_admin", userAccount.ModuleAccess_Admin);
                Data.AddParameter(cmd, "@moduleAccess_soa", userAccount.ModuleAccess_SOA);
                Data.AddParameter(cmd, "@moduleAccess_payment", userAccount.ModuleAccess_Payment);
                Data.AddParameter(cmd, "@moduleAccess_reports", userAccount.ModuleAccess_Reports);
                Data.AddParameter(cmd, "@moduleAccess_granular", userAccount.ModuleAccess_Granular);
                Data.AddParameter(cmd, "@accessRights_admin", userAccount.AccessRights_Admin);
                Data.AddParameter(cmd, "@accessRights_soa", userAccount.AccessRights_SOA);
                Data.AddParameter(cmd, "@accessRights_payment", userAccount.AccessRights_Payment);
                Data.AddParameter(cmd, "@accessRights_reports", userAccount.AccessRights_Reports);
                Data.AddParameter(cmd, "@accessRights_granular", userAccount.AccessRights_Granular);
                Data.AddParameter(cmd, "@emailAddress", userAccount.EmailAddress);
                Data.AddParameter(cmd, "@contactNo", userAccount.ContactNo);
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
        public static async Task<ResponseMessage> CreateUserAccountAsync(UserAccount userAccount, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.UserAccount.InsertSql, conn) { CommandType = CommandType.StoredProcedure };

                Data.AddParameter(cmd, "@name", userAccount.Name);
                Data.AddParameter(cmd, "@jobTitle", userAccount.JobTitle);
                Data.AddParameter(cmd, "@team", userAccount.Team);
                Data.AddParameter(cmd, "@role", userAccount.Role);
                Data.AddParameter(cmd, "@moduleAccess_admin", userAccount.ModuleAccess_Admin);
                Data.AddParameter(cmd, "@moduleAccess_soa", userAccount.ModuleAccess_SOA);
                Data.AddParameter(cmd, "@moduleAccess_payment", userAccount.ModuleAccess_Payment);
                Data.AddParameter(cmd, "@moduleAccess_reports", userAccount.ModuleAccess_Reports);
                Data.AddParameter(cmd, "@moduleAccess_granular", userAccount.ModuleAccess_Granular);
                Data.AddParameter(cmd, "@accessRights_admin", userAccount.AccessRights_Admin);
                Data.AddParameter(cmd, "@accessRights_soa", userAccount.AccessRights_SOA);
                Data.AddParameter(cmd, "@accessRights_payment", userAccount.AccessRights_Payment);
                Data.AddParameter(cmd, "@accessRights_reports", userAccount.AccessRights_Reports);
                Data.AddParameter(cmd, "@accessRights_granular", userAccount.AccessRights_Granular);
                Data.AddParameter(cmd, "@emailAddress", userAccount.EmailAddress);
                Data.AddParameter(cmd, "@contactNo", userAccount.ContactNo);
                Data.AddParameter(cmd, "@password", userAccount.Password);
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
        /// Updates the userAccount NVP.
        /// </summary>
        /// <param name="nvp">The NVP.</param>
        /// <returns>Task&lt;ResponseMessage&gt;.</returns>
        public static async Task<ResponseMessage> UpdateUserAccountNVPAsync(NameValuePair nvp, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage() { Status = false }; 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.UserAccount.UpdateNvpSql, conn) { CommandType = CommandType.StoredProcedure };
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

        public static async Task<List<UserAccount_Excel>> GetRequestExcelList(CancellationToken cancellationToken = default)
        {
            List<UserAccount_Excel> result = new List<UserAccount_Excel>();
            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);

                await using var cmd = new SqlCommand(Scripts.UserAccount.ListSqlExcel, conn) { CommandType = CommandType.StoredProcedure };
                //added fix when viewing 100+ records
                cmd.CommandTimeout = 0;
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                {
                    do
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            result.Add(new UserAccount_Excel
                            {
                                Id = Data.GetGuid(reader["pkid"]),
                                Name = Data.GetString(reader["name"]),
                                JobTitle = Data.GetString(reader["jobTitle"]),
                                Team = Data.GetString(reader["team"]),
                                Role = Data.GetString(reader["role"]),
                                ModuleAccess_Admin = Data.GetBool(reader["moduleAccess_admin"]),
                                ModuleAccess_SOA = Data.GetBool(reader["moduleAccess_soa"]),
                                ModuleAccess_Payment = Data.GetBool(reader["moduleAccess_payment"]),
                                ModuleAccess_Reports = Data.GetBool(reader["moduleAccess_reports"]),
                                ModuleAccess_Granular = Data.GetBool(reader["moduleAccess_granular"]),
                                AccessRights_Admin = Data.GetBool(reader["accessRights_admin"]),
                                AccessRights_SOA = Data.GetBool(reader["accessRights_soa"]),
                                AccessRights_Payment = Data.GetBool(reader["accessRights_payment"]),
                                AccessRights_Reports = Data.GetBool(reader["accessRights_reports"]),
                                AccessRights_Granular = Data.GetBool(reader["accessRights_granular"]),
                                EmailAddress = Data.GetString(reader["emailAddress"]),
                                ContactNo = Data.GetString(reader["contactNo"])
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
