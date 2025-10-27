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
    public static class CustomerData
    {
        private static readonly string errorMessage = "Please verify the information you provided";

        public static AgentListing FillCustomer(SqlDataReader reader)
        {
            return new AgentListing
            {
                Index = Data.GetInt(reader["index"]),
                Id = Data.GetGuid(reader["pkid"]),
                Name = Data.GetString(reader["name"]),
                LegalEntityName = Data.GetString(reader["legalEntityName"]),
                Tin = Data.GetString(reader["tin"]),
                Address = Data.GetString(reader["address"]),
                SalesExec_LBC = Data.GetString(reader["salesExec_LBC"]),
                ApprovedAFC = Data.GetDecimal(reader["approvedAFC"]),
                SOAFormatId = Data.GetGuid(reader["soaFormatId"]),
                RateCardId = Data.GetGuid(reader["rateCardId"]),
                Domestic_Intl = Data.GetString(reader["domestic_intl"]),
                Country = Data.GetString(reader["country"]),
                TransmissionMode = Data.GetString(reader["transmissionMode"]),
                OfficeCode = Data.GetString(reader["officeCode"]),
                Area = Data.GetString(reader["area"]),
                SAPCustomerId = Data.GetLong(reader["sapCustomerId"]),
                SAPVendorId_IPP = Data.GetLong(reader["sapVendorId_ipp"]),
                SAPVendorId_PP_SC = Data.GetLong(reader["sapVendorId_pp_sc"]),
                SAPVendorId_RTA = Data.GetLong(reader["sapVendorId_rta"]),
                SAPVendorId_SNS = Data.GetLong(reader["sapVendorId_sns"]),
                SAPVendorId_IPPX = Data.GetLong(reader["sapVendorId_ippx"]),
                PaymentCurrency = Data.GetString(reader["paymentCurrency"]),
                SFModeOfSettlement = Data.GetString(reader["SFModeOfSettlement"]),
                WithHoldingTax = Data.GetString(reader["withHoldingTax"]),
                VatStatus = Data.GetString(reader["vatStatus"]),
                Status = Data.GetString(reader["status"])
            };
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<List<AgentListing>> GetCustomerListAsync(string userAccountId = null, CancellationToken cancellationToken = default)
        {
            List<AgentListing> result = new List<AgentListing>(); 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Customer.ListSql, conn) { CommandType = CommandType.StoredProcedure };
                await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
                do
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {

                        result.Add(FillCustomer(reader));
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
        public static async Task<PaginationResponse<AgentListing>> CustomerSearchAsync(CustomerSearchRequest pagination, CancellationToken cancellationToken = default)
        {
            PaginationResponse<AgentListing> result = null;

            pagination.FilterTerm = pagination.FilterTerm.EditFilterTermString();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                List<AgentListing> Customers = new List<AgentListing>();
                await using var cmd = new SqlCommand(Scripts.Customer.ListSqlPaginated, conn) { CommandType = CommandType.StoredProcedure };
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
                            Customers.Add(new AgentListing
                            {
                                Id = Data.GetGuid(reader["pkid"]),
                                Name = Data.GetString(reader["name"]),
                                LegalEntityName = Data.GetString(reader["legalEntityName"]),
                                Tin = Data.GetString(reader["tin"]),
                                Address = Data.GetString(reader["address"]),
                                SalesExec_LBC = Data.GetString(reader["salesExec_LBC"]), 
                                ApprovedAFC_str = (Data.GetDecimal(reader["approvedAFC"])).ToString("N"),
                                SoaTemplateName = Data.GetString(reader["soaTemplateName"]),
                                RateCardName = Data.GetString(reader["rateCardName"]),
                                Domestic_Intl = Data.GetString(reader["domestic_intl"]),
                                Country = Data.GetString(reader["country"]),
                                TransmissionMode = Data.GetString(reader["transmissionMode"]),
                                OfficeCode = Data.GetString(reader["officeCode"]),
                                Area = Data.GetString(reader["area"]),
                                SAPCustomerId = Data.GetLong(reader["sapCustomerId"]),
                                SAPVendorId_IPP = Data.GetLong(reader["sapVendorId_ipp"]),
                                SAPVendorId_PP_SC = Data.GetLong(reader["sapVendorId_pp_sc"]),
                                SAPVendorId_RTA = Data.GetLong(reader["sapVendorId_rta"]),
                                SAPVendorId_SNS = Data.GetLong(reader["sapVendorId_sns"]),
                                SAPVendorId_IPPX = Data.GetLong(reader["sapVendorId_ippx"]),
                                PaymentCurrency = Data.GetString(reader["paymentCurrency"]),
                                SFModeOfSettlement = Data.GetString(reader["SFModeOfSettlement"]),
                                WithHoldingTax = Data.GetString(reader["withHoldingTax"]),
                                VatStatus = Data.GetString(reader["vatStatus"]),
                                Status = Data.GetString(reader["status"]),
                                Created = Data.GetDateTime(reader["created"]),
                                Updated = Data.GetDateTime(reader["updated"]), 
                            });
                        }
                    }
                    while (await reader.NextResultAsync(cancellationToken));
                }
                

                result = new PaginationResponse<AgentListing>
                {
                    Paging = pagination,
                    TotalRows = Data.GetInt(cmd.Parameters["@TotalRows"].Value),
                    FilteredRows = Data.GetInt(cmd.Parameters["@filteredRowsCount"].Value),
                    Data = Customers
                };
            }
            catch (Exception ex)
            {
                // 
            }

            return result;
        }

        /// <summary>
        /// Get Customer by Id 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<AgentListing> GetCustomerAsync(string customerId = null, string SAPCustomerID=null, string CustomerName=null, CancellationToken cancellationToken = default)
        {
            AgentListing result = null;

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Customer.GetSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@pkid", customerId);
                Data.AddParameter(cmd, "@SAPIds", SAPCustomerID);
                Data.AddParameter(cmd, "@CustomerName", CustomerName);
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                if (reader.HasRows && await reader.ReadAsync(cancellationToken))
                {
                    result = new AgentListing
                    {
                        Id = Data.GetGuid(reader["pkid"]),
                        Name = Data.GetString(reader["name"]),
                        LegalEntityName = Data.GetString(reader["legalEntityName"]),
                        Tin = Data.GetString(reader["tin"]),
                        Address = Data.GetString(reader["address"]),
                        SalesExec_LBC = Data.GetString(reader["salesExec_LBC"]),
                        ApprovedAFC = Data.GetDecimal(reader["approvedAFC"]),
                        SOAFormatId = Data.GetGuid(reader["soaFormatId"]),
                        RateCardId = Data.GetGuid(reader["rateCardId"]),
                        Domestic_Intl = Data.GetString(reader["domestic_intl"]),
                        Country = Data.GetString(reader["country"]),
                        TransmissionMode = Data.GetString(reader["transmissionMode"]),
                        OfficeCode = Data.GetString(reader["officeCode"]),
                        Area = Data.GetString(reader["area"]),
                        SAPCustomerId = Data.GetLong(reader["sapCustomerId"]),
                        SAPVendorId_IPP = Data.GetLong(reader["sapVendorId_ipp"]),
                        SAPVendorId_PP_SC = Data.GetLong(reader["sapVendorId_pp_sc"]),
                        SAPVendorId_RTA = Data.GetLong(reader["sapVendorId_rta"]),
                        SAPVendorId_SNS = Data.GetLong(reader["sapVendorId_sns"]),
                        SAPVendorId_IPPX = Data.GetLong(reader["sapVendorId_ippx"]),
                        PaymentCurrency = Data.GetString(reader["paymentCurrency"]),
                        SFModeOfSettlement = Data.GetString(reader["SFModeOfSettlement"]),
                        WithHoldingTax = Data.GetString(reader["withHoldingTax"]),
                        VatStatus = Data.GetString(reader["vatStatus"]),
                        Status = Data.GetString(reader["status"]),
                        DepositoryBankAccountId = Data.GetGuid(reader["depositoryBankAccount"]),
                        BeginningBalance = Data.GetDecimal(reader["beginningBalance"]),
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
        public static async Task<ResponseMessage> UpdateCustomerAsync(AgentListing customer, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            string sample_ = customer.BeginningBalance.ToString();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Customer.UpdateSql, conn) { CommandType = CommandType.StoredProcedure };
                Data.AddParameter(cmd, "@pkId", customer.Id);
                Data.AddParameter(cmd, "@name", customer.Name);
                Data.AddParameter(cmd, "@legalEntityName", customer.LegalEntityName);
                Data.AddParameter(cmd, "@tin", customer.Tin);
                Data.AddParameter(cmd, "@address", customer.Address);
                Data.AddParameter(cmd, "@salesExec_LBC", customer.SalesExec_LBC);
                Data.AddParameter(cmd, "@approvedAFC", customer.ApprovedAFC);
                Data.AddParameter(cmd, "@soaFormatId", customer.SOAFormatId);
                Data.AddParameter(cmd, "@rateCardId", customer.RateCardId);
                Data.AddParameter(cmd, "@domestic_intl", customer.Domestic_Intl);
                Data.AddParameter(cmd, "@country", customer.Country);
                Data.AddParameter(cmd, "@transmissionMode", customer.TransmissionMode);
                Data.AddParameter(cmd, "@officeCode", customer.OfficeCode);
                Data.AddParameter(cmd, "@area", customer.Area);
                Data.AddParameter(cmd, "@sapCustomerId", customer.SAPCustomerId);
                Data.AddParameter(cmd, "@sapVendorId_ipp", customer.SAPVendorId_IPP);
                Data.AddParameter(cmd, "@sapVendorId_pp_sc", customer.SAPVendorId_PP_SC);
                Data.AddParameter(cmd, "@sapVendorId_rta", customer.SAPVendorId_RTA);
                Data.AddParameter(cmd, "@sapVendorId_sns", customer.SAPVendorId_SNS);
                Data.AddParameter(cmd, "@sapVendorId_ippx", customer.SAPVendorId_IPPX);
                Data.AddParameter(cmd, "@paymentCurrency", customer.PaymentCurrency);
                Data.AddParameter(cmd, "@SFModeOfSettlement", customer.SFModeOfSettlement);
                Data.AddParameter(cmd, "@withholdingTax", customer.WithHoldingTax);
                Data.AddParameter(cmd, "@vatStatus", customer.VatStatus);
                Data.AddParameter(cmd, "@status", customer.Status);
                Data.AddParameter(cmd, "@depositoryBankAccount", customer.DepositoryBankAccountId);
                Data.AddParameter(cmd, "@beginningBalance", customer.BeginningBalance);
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
        public static async Task<ResponseMessage> CreateCustomerAsync(AgentListing customer, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Customer.InsertSql, conn) { CommandType = CommandType.StoredProcedure };

                Data.AddParameter(cmd, "@name", customer.Name);
                Data.AddParameter(cmd, "@legalEntityName", customer.LegalEntityName);
                Data.AddParameter(cmd, "@tin", customer.Tin);
                Data.AddParameter(cmd, "@address", customer.Address);
                Data.AddParameter(cmd, "@salesExec_LBC", customer.SalesExec_LBC);
                Data.AddParameter(cmd, "@approvedAFC", customer.ApprovedAFC);
                Data.AddParameter(cmd, "@soaFormatId", customer.SOAFormatId);
                Data.AddParameter(cmd, "@rateCardId", customer.RateCardId);
                Data.AddParameter(cmd, "@domestic_intl", customer.Domestic_Intl);
                Data.AddParameter(cmd, "@country", customer.Country);
                Data.AddParameter(cmd, "@transmissionMode", customer.TransmissionMode);
                Data.AddParameter(cmd, "@officeCode", customer.OfficeCode);
                Data.AddParameter(cmd, "@area", customer.Area);
                Data.AddParameter(cmd, "@sapCustomerId", customer.SAPCustomerId);
                Data.AddParameter(cmd, "@sapVendorId_ipp", customer.SAPVendorId_IPP);
                Data.AddParameter(cmd, "@sapVendorId_pp_sc", customer.SAPVendorId_PP_SC);
                Data.AddParameter(cmd, "@sapVendorId_rta", customer.SAPVendorId_RTA);
                Data.AddParameter(cmd, "@sapVendorId_sns", customer.SAPVendorId_SNS);
                Data.AddParameter(cmd, "@sapVendorId_ippx", customer.SAPVendorId_IPPX);
                Data.AddParameter(cmd, "@paymentCurrency", customer.PaymentCurrency);
                Data.AddParameter(cmd, "@SFModeOfSettlement", customer.SFModeOfSettlement);
                Data.AddParameter(cmd, "@withholdingTax", customer.WithHoldingTax);
                Data.AddParameter(cmd, "@vatStatus", customer.VatStatus);
                Data.AddParameter(cmd, "@status", customer.Status);
                Data.AddParameter(cmd, "@depositoryBankAccount", customer.DepositoryBankAccountId);
                Data.AddParameter(cmd, "@beginningBalance", customer.BeginningBalance);
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
        /// Updates the Customer NVP.
        /// </summary>
        /// <param name="nvp">The NVP.</param>
        /// <returns>Task&lt;ResponseMessage&gt;.</returns>
        public static async Task<ResponseMessage> UpdateCustomerNVPAsync(NameValuePair nvp, string userAccountId = null, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage() { Status = false }; 

            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Customer.UpdateNvpSql, conn) { CommandType = CommandType.StoredProcedure };
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

        public static async Task<List<Customer_Excel>> GetRequestExcelList(CancellationToken cancellationToken = default)
        {
            List<Customer_Excel> result = new List<Customer_Excel>();
            try
            {
                await using var conn = await Data.CreateConnectionAsync(cancellationToken);
                await using var cmd = new SqlCommand(Scripts.Customer.ListSqlExcel, conn) { CommandType = CommandType.StoredProcedure };
                //added fix when viewing 100+ records
                cmd.CommandTimeout = 0;
                await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                {
                    do
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            result.Add(new Customer_Excel
                            {
                                Id = Data.GetGuid(reader["pkid"]),
                                Name = Data.GetString(reader["name"]),
                                LegalEntityName = Data.GetString(reader["legalEntityName"]),
                                Tin = Data.GetString(reader["tin"]),
                                Address = Data.GetString(reader["address"]),
                                SalesExec_LBC = Data.GetString(reader["salesExec_LBC"]),
                                ApprovedAFC = Data.GetDecimal(reader["approvedAFC"]),
                                SOAFormat = Data.GetString(reader["soaFormat"]),
                                RateCard = Data.GetString(reader["rateCard"]),
                                Domestic_Intl = Data.GetString(reader["domestic_intl"]),
                                Country = Data.GetString(reader["country"]),
                                TransmissionMode = Data.GetString(reader["transmissionMode"]),
                                OfficeCode = Data.GetString(reader["officeCode"]),
                                Area = Data.GetString(reader["area"]),
                                SAPCustomerId = Data.GetLong(reader["sapCustomerId"]),
                                SAPVendorId_IPP = Data.GetLong(reader["sapVendorId_ipp"]),
                                SAPVendorId_PP_SC = Data.GetLong(reader["sapVendorId_pp_sc"]),
                                SAPVendorId_RTA = Data.GetLong(reader["sapVendorId_rta"]),
                                SAPVendorId_SNS = Data.GetLong(reader["sapVendorId_sns"]),
                                SAPVendorId_IPPX = Data.GetLong(reader["sapVendorId_ippx"]),
                                PaymentCurrency = Data.GetString(reader["paymentCurrency"]),
                                SFModeOfSettlement = Data.GetString(reader["SFModeOfSettlement"]),
                                WithHoldingTax = Data.GetString(reader["withholdingTax"]),
                                VatStatus = Data.GetString(reader["vatStatus"]),
                                Status = Data.GetString(reader["status"])
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
