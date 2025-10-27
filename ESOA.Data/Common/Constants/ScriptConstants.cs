using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOA.Common
{
    internal class Scripts
    {
        internal static string ESOASchema = "[dbo]";

        internal class UserAccount
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_useraccount]";
            internal static string ListSqlPaginated = $"{ESOASchema}.[pc_lst_useraccount_paging]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_useraccount]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_useraccount]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_useraccount]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_useraccount_nvp]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_useraccount]";
            internal static string ListSqlExcel = $"{ESOASchema}.[pc_lst_useraccount_excel]";
        }

        internal class Customer
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_customer]";
            internal static string ListSqlPaginated = $"{ESOASchema}.[pc_lst_customer_paging]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_customer]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_customer]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_customer]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_customer_nvp]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_customer]";
            internal static string ListSqlExcel = $"{ESOASchema}.[pc_lst_customer_excel]";
        }

        internal class CustomerEmailAddress
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_customeremailaddress]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_customeremailaddress]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_customeremailaddress]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_customeremailaddress]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_customeremailaddress]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_customeremailaddress_nvp]";
        }

        internal class CustomerContactNo
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_customercontactno]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_customercontactno]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_customercontactno]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_customercontactno]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_customercontactno]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_customercontactno_nvp]";
        }

        internal class CustomerContactPerson
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_customercontactperson]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_customercontactperson]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_customercontactperson]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_customercontactperson]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_customercontactperson]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_customercontactperson_nvp]";
        }

        internal class CustomerDepositoryAccountNo
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_customerdepositoryaccountno]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_customerdepositoryaccountno]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_customerdepositoryaccountno]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_customerdepositoryaccountno]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_customerdepositoryaccountno]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_customerdepositoryaccountno_nvp]";
        }

        internal class CustomerDepositoryBankAccount
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_customerdepositorybankaccount]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_customerdepositorybankaccount]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_customerdepositorybankaccount]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_customerdepositorybankaccount]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_customerdepositorybankaccount]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_customerdepositorybankaccount_nvp]";
        }

        internal class SoaFormat
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_soaformat]";
            internal static string ListSqlPaginated = $"{ESOASchema}.[pc_lst_soaformat_paging]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_soaformat]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_soaformat]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_soaformat]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_soaformat_nvp]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_soaformat]";
        }

        internal class Rate
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_rate]";
            internal static string ListSqlPaginated = $"{ESOASchema}.[pc_lst_rate_paging]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_rate]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_rate]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_rate]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_rate_nvp]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_rate]";
            internal static string ListSqlExcel = $"{ESOASchema}.[pc_lst_rate_excel]";
        }

        internal class Payment
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_payment]";
            internal static string ListSqlPaginated = $"{ESOASchema}.[pc_lst_payment_paging]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_payment]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_payment]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_payment]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_payment_nvp]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_payment]";
            internal static string ListSqlExcel = $"{ESOASchema}.[pc_lst_payment_excel]";
        }

        internal class Acceptance
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_acceptance]";
            internal static string ListSqlPaginated = $"{ESOASchema}.[pc_lst_acceptance_paging]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_acceptance]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_acceptance]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_acceptance]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_acceptance_nvp]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_acceptance]";
            internal static string ListSqlExcel = $"{ESOASchema}.[pc_lst_acceptance_excel]";
        }

        internal class Encashment
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_encashment]";
            internal static string ListSqlPaginated = $"{ESOASchema}.[pc_lst_encashment_paging]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_encashment]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_encashment]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_encashment]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_encashment_nvp]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_encashment]";
            internal static string ListSqlExcel = $"{ESOASchema}.[pc_lst_encashment_excel]";
        }

        internal class Refund
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_refund]";
            internal static string ListSqlPaginated = $"{ESOASchema}.[pc_lst_refund_paging]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_refund]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_refund]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_refund]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_refund_nvp]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_refund]";
            internal static string ListSqlExcel = $"{ESOASchema}.[pc_lst_refund_excel]";
        }

        internal class Voided
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_voided]";
            internal static string ListSqlPaginated = $"{ESOASchema}.[pc_lst_voided_paging]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_voided]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_voided]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_voided]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_voided_nvp]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_voided]";
            internal static string ListSqlExcel = $"{ESOASchema}.[pc_lst_voided_excel]";
        }
        internal class SoaFormatA
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_soaormatA]";
            internal static string ListSqlPaginated = $"{ESOASchema}.[pc_lst_soaformatA_paging]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_soaformatA]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_soaformatA]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_soaformatA]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_soaformatA_nvp]";
            internal static string GetPrevBalVars = $"{ESOASchema}.[pc_lst_SOAFormatA_PrevBalanceVariables]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_soaformatA]";
        }

        internal class SoaFormatB
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_soaormatB]";
            internal static string ListSqlPaginated = $"{ESOASchema}.[pc_lst_soaformatB_paging]";
            internal static string ListSql = $"{ESOASchema}.[pc_lst_soaformatB]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_soaformatB]";
            internal static string UpdateSql = $"{ESOASchema}.[pc_upd_soaformatB]";
            internal static string UpdateNvpSql = $"{ESOASchema}.[pc_upd_soaformatB_nvp]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_soaformatB]";
        }

        internal class SOAVerifiedDates
        {
            internal static string GetSql = $"{ESOASchema}.[pc_get_SOAVerifiedDates]";
            internal static string InsertSql = $"{ESOASchema}.[pc_ins_SOAVerifiedDates]";
            internal static string DeleteSql = $"{ESOASchema}.[pc_del_SOAVerifiedDates]";
        }

        internal class AgentBalances
        { 
            internal static string ListSql = $"{ESOASchema}.[pc_lst_agentbalances]"; 
        }

    }
}
