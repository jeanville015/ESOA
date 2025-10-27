using ArrayToExcel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using EPDV.Controllers;
using ESOA.Common;
using ESOA.Model;
using ESOA.Model.Constant;
using ESOA.Model.Constants;
using ESOA.Model.Entity.SOAFormatA;
using ESOA.Model.View;
using ESOA.WEBMVC.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ESOA.WEBMVC.Controllers
{
    public class SOAController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public SOAController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult SOAFormatA()
        { 
            return View();
        }

        public IActionResult SOAFormatB()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ViewFilterOptionsSOAFormatA(CancellationToken cancellationToken)
        { 

            List<AgentListing> customerList = await CustomerData.GetCustomerListAsync(cancellationToken: cancellationToken);
            ViewBag.CustomerList = customerList.Select(x => new NameValuePair { Value = Data.GetString(x.Name), Name = x.Name }).ToList();
            return PartialView("SOAFormatA/_filterOptions");
        }

        [HttpPost]
        public async Task<IActionResult> ViewFilterOptionsSOAFormatB(CancellationToken cancellationToken)
        {

            List<AgentListing> customerList = await CustomerData.GetCustomerListAsync(cancellationToken: cancellationToken);
            ViewBag.CustomerList = customerList.Select(x => new NameValuePair { Value = Data.GetString(x.Name), Name = x.Name }).ToList();
            return PartialView("SOAFormatB/_filterOptions");
        }

        [HttpPost]
        public async Task<IActionResult> SearchSOAFormatA(string[] customers, string dateFrom, string dateTo, string beginningBalance, CancellationToken cancellationToken)
        {
            if ((customers.Length == 0) && (dateFrom == null) && (dateTo == null))
            {
                SOAFormatAMainView<SOAFormatAView> _result = new SOAFormatAMainView<SOAFormatAView>();
                return PartialView("SOAFormatA/_List", _result);
            }

            //get the previous date Balance, check if the prev date has no data for balance (acceptance)
            //(Indicator: just check a day before DateFrom, if has data, call PrevBalSP else just get Customer BeginningBalance from admin module)
            decimal _beginningBalance;
            //DateTime date = DateTime.ParseExact(dateFrom, "yyyy-MM-dd", null);
            //DateTime previousDay = date.AddDays(-1);
            //List<Acceptance> previousDayData = await AcceptanceData.GetAcceptanceListAsync(String.Join(";", customers), previousDay.ToString("yyyy-MM-dd"), previousDay.ToString("yyyy-MM-dd"), null, null, cancellationToken);

            SOAFormatA<SOAFormatAData> result = new SOAFormatA<SOAFormatAData>();
            SOAFormatAMainView<SOAFormatAView> _SOAFormatAMainView = new SOAFormatAMainView<SOAFormatAView>();

            //get current user role
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            UserAccount currentUserAccount = await UserAccountData.GetUserAccountAsync(userAccountId);
            _SOAFormatAMainView.CurrentUserRole = currentUserAccount.Role;

            result.Data = await SOAFormatData.GetSOAFormatAListAsync(System.String.Join(";", customers), dateFrom, dateTo, null, cancellationToken);

            AgentListing agentListing = await CustomerData.GetCustomerAsync(null, null, customers[0].ToString(), cancellationToken);
            SOAFormatAPreviousBalanceVariables sOAFormatAPreviousBalanceVariables = await SOAFormatData.GetSOAFormatAPreviousBalanceVariables(customers[0].ToString(), dateFrom, cancellationToken);
            if (sOAFormatAPreviousBalanceVariables.TotalLBCReceivable == 0)
            {
                _beginningBalance = agentListing.BeginningBalance;
            }
            else
            {

                _beginningBalance = (agentListing.BeginningBalance - sOAFormatAPreviousBalanceVariables.TotalLBCReceivable) 
                                    + (sOAFormatAPreviousBalanceVariables.Settlement) 
                                    + (sOAFormatAPreviousBalanceVariables.All_RVProducttype_Total);
            }

            _SOAFormatAMainView.Data = new List<SOAFormatAView>(); 

            int indexer = -1;
            foreach (SOAFormatAData soaFormatA in result.Data)
            {
                indexer++;
                //result[indexer].Sf_Total = soaFormatA.Sf_IPP + soaFormatA.Sf_PP_SC + soaFormatA.Sf_RTA + soaFormatA.Sf_SNS;
                //result[indexer].WithholdingTax = (result[indexer].Sf_Total / Convert.ToDecimal(1.12)) * Convert.ToDecimal(0.02);
                //5-13-2025: Noticed that the formula in ANNEX is only (Amt_Total + Sf_Total - WithholdingTax) in all rows
                result.Data[indexer].TotalLBCReceivable = result.Data[indexer].Amt_Total + result.Data[indexer].Sf_Total + result.Data[indexer].WithholdingTax;
                if (indexer == 0)
                { 
                    result.Data[indexer].RunningBalance = _beginningBalance - result.Data[indexer].TotalLBCReceivable + result.Data[indexer].Settlement
                                                   + (result.Data[indexer].Rv_IPP + result.Data[indexer].Rv_PP_SC + result.Data[indexer].Rv_RTA + result.Data[indexer].Rv_SNS);
                }
                else
                {
                    result.Data[indexer].RunningBalance = result.Data[indexer-1].RunningBalance - result.Data[indexer].TotalLBCReceivable + result.Data[indexer].Settlement
                                                + (result.Data[indexer].Rv_IPP + result.Data[indexer].Rv_PP_SC + result.Data[indexer].Rv_RTA + result.Data[indexer].Rv_SNS);
                } 
                _SOAFormatAMainView.Data.Add(FillSOAFormatAView(result.Data[indexer]));

                //set the verified details
                SOAVerifiedDates sOAVerifiedDates = await SOAVerifiedDatesData.GetSOAVerifiedDatesAsync(result.Data[indexer].Date, customers[0]); 
                if (sOAVerifiedDates != null)
                {
                    _SOAFormatAMainView.Data[_SOAFormatAMainView.Data.Count-1].VerifiedStatus = sOAVerifiedDates.Status;
                    _SOAFormatAMainView.Data[_SOAFormatAMainView.Data.Count-1].VerifiedRemarks = sOAVerifiedDates.Remarks;
                }

                //get customer settings from Admin module; As criteria in ENCASHMENT formula
                AgentListing customer = await CustomerData.GetCustomerAsync(null, null, customers[0], cancellationToken);

                _SOAFormatAMainView.Email_AFC_Amount = customer.ApprovedAFC.ToString("N");

                //if day minus 2
                if ((indexer + 1) == (result.Data.Count-1))
                {
                    _SOAFormatAMainView.Email_BeginningBalance_Amount = result.Data[indexer].RunningBalance.ToString("N");
                    _SOAFormatAMainView.Email_BeginningBalance_Date = result.Data[indexer].Date;
                }

                //if for the day
                if ((indexer + 1) == (result.Data.Count))
                {
                    _SOAFormatAMainView.Email_Collections = result.Data[indexer].Settlement.ToString("N");

                    //if Service Fee is auto-collect based on customer maintenance table, Encashment should be the total acceptance plus service fee
                    //if Service fee is billed, encashment should be the total acceptance only
                    if (customer.SFModeOfSettlement == SFModeOfSettlement.AutoCollectFromAFC) 
                    {
                        _SOAFormatAMainView.Email_Encashments = (result.Data[indexer].Amt_Total + result.Data[indexer].Sf_Total).ToString("N");
                    }
                    else if (customer.SFModeOfSettlement == SFModeOfSettlement.Billed)
                    {
                        _SOAFormatAMainView.Email_Encashments = result.Data[indexer].Amt_Total.ToString("N");
                    }

                    _SOAFormatAMainView.Email_EWT = result.Data[indexer].WithholdingTax.ToString("N");
                    _SOAFormatAMainView.Email_EndingBalance_Amount = result.Data[indexer].RunningBalance.ToString("N"); ;
                    _SOAFormatAMainView.Email_EndingBalance_Date = result.Data[indexer].Date;
                }

                result.Unit_IPP_Total = result.Unit_IPP_Total + result.Data[indexer].Unit_IPP;
                result.Unit_PP_SC_Total = result.Unit_PP_SC_Total + result.Data[indexer].Unit_PP_SC;
                result.Unit_RTA_Total = result.Unit_RTA_Total + result.Data[indexer].Unit_RTA;
                result.Unit_SNS_Total = result.Unit_SNS_Total + result.Data[indexer].Unit_SNS;
                result.Amt_IPP_Total = result.Amt_IPP_Total + result.Data[indexer].Amt_IPP;
                result.Amt_PP_SC_Total = result.Amt_PP_SC_Total + result.Data[indexer].Amt_PP_SC;
                result.Amt_RTA_Total = result.Amt_RTA_Total + result.Data[indexer].Amt_RTA;
                result.Amt_SNS_Total = result.Amt_SNS_Total + result.Data[indexer].Amt_SNS;
                result.Amt_Total_Total = result.Amt_Total_Total + result.Data[indexer].Amt_Total;
                result.Sf_IPP_Total = result.Sf_IPP_Total + result.Data[indexer].Sf_IPP;
                result.Sf_PP_SC_Total = result.Sf_PP_SC_Total + result.Data[indexer].Sf_PP_SC;
                result.Sf_RTA_Total = result.Sf_RTA_Total + result.Data[indexer].Sf_RTA;
                result.Sf_SNS_Total = result.Sf_SNS_Total + result.Data[indexer].Sf_SNS;
                result.Sf_Total_Total = result.Sf_Total_Total + result.Data[indexer].Sf_Total;
                result.WithholdingTax_Total = result.WithholdingTax_Total + result.Data[indexer].WithholdingTax;
                result.TotalLBCReceivable_Total = result.TotalLBCReceivable_Total + result.Data[indexer].TotalLBCReceivable;
                result.Rv_IPP_Total = result.Rv_IPP_Total + result.Data[indexer].Rv_IPP;
                result.Rv_PP_SC_Total = result.Rv_PP_SC_Total + result.Data[indexer].Rv_PP_SC;
                result.Rv_RTA_Total = result.Rv_RTA_Total + result.Data[indexer].Rv_RTA;
                result.Rv_SNS_Total = result.Rv_SNS_Total + result.Data[indexer].Rv_SNS;
                result.Settlement_Total = result.Settlement_Total + result.Data[indexer].Settlement;
            }

            _SOAFormatAMainView.CustomerName = customers[0].ToString();
            _SOAFormatAMainView.Unit_IPP_Total = result.Unit_IPP_Total.ToString();
            _SOAFormatAMainView.Unit_PP_SC_Total = result.Unit_PP_SC_Total.ToString();
            _SOAFormatAMainView.Unit_RTA_Total = result.Unit_RTA_Total.ToString();
            _SOAFormatAMainView.Unit_SNS_Total = result.Unit_SNS_Total.ToString();
            _SOAFormatAMainView.Amt_IPP_Total = result.Amt_IPP_Total.ToString("N");
            _SOAFormatAMainView.Amt_PP_SC_Total = result.Amt_PP_SC_Total.ToString("N");
            _SOAFormatAMainView.Amt_RTA_Total = result.Amt_RTA_Total.ToString("N");
            _SOAFormatAMainView.Amt_SNS_Total = result.Amt_SNS_Total.ToString("N");
            _SOAFormatAMainView.Amt_Total_Total = result.Amt_Total_Total.ToString("N");
            _SOAFormatAMainView.Sf_IPP_Total = result.Sf_IPP_Total.ToString("N");
            _SOAFormatAMainView.Sf_PP_SC_Total = result.Sf_PP_SC_Total.ToString("N");
            _SOAFormatAMainView.Sf_RTA_Total = result.Sf_RTA_Total.ToString("N");
            _SOAFormatAMainView.Sf_SNS_Total = result.Sf_SNS_Total.ToString("N");
            _SOAFormatAMainView.Sf_Total_Total = result.Sf_Total_Total.ToString("N");
            _SOAFormatAMainView.WithholdingTax_Total = result.WithholdingTax_Total.ToString("N");
            _SOAFormatAMainView.TotalLBCReceivable_Total = result.TotalLBCReceivable_Total.ToString("N");
            _SOAFormatAMainView.Rv_IPP_Total = result.Rv_IPP_Total.ToString("N");
            _SOAFormatAMainView.Rv_PP_SC_Total = result.Rv_PP_SC_Total.ToString("N");
            _SOAFormatAMainView.Rv_RTA_Total = result.Rv_RTA_Total.ToString("N");
            _SOAFormatAMainView.Rv_SNS_Total = result.Rv_SNS_Total.ToString("N");
            _SOAFormatAMainView.Settlement_Total = result.Settlement_Total.ToString("N");
            _SOAFormatAMainView.BeginningBalance = _beginningBalance.ToString("N");

            _SOAFormatAMainView.Email_Adjustment = (result.Rv_IPP_Total + result.Rv_PP_SC_Total + result.Rv_RTA_Total + result.Rv_SNS_Total).ToString("N");
            _SOAFormatAMainView.Email_NumberOfUnitsProcessed = (result.Unit_IPP_Total + result.Unit_PP_SC_Total + result.Unit_RTA_Total + result.Unit_SNS_Total).ToString();

            //prepare storage for the SOA Format A Email
            HttpContext.Session.Remove(DefaultValues.SessionObjectSOAFormatAEmail);
            HttpContext.Session.SetObject(DefaultValues.SessionObjectSOAFormatAEmail, _SOAFormatAMainView);

            //prepare the variable that will hold the values for excel file import 
            HttpContext.Session.Remove(DefaultValues.SessionObjectExcelExportReport); 
            HttpContext.Session.SetObject(DefaultValues.SessionObjectExcelExportReport, _SOAFormatAMainView.Data);

            return PartialView("SOAFormatA/_List", _SOAFormatAMainView); 
        }

        [HttpPost]
        public async Task<IActionResult> SearchSOAFormatB(string[] customers, string dateFrom, string dateTo, string beginningBalance, CancellationToken cancellationToken)
        {
            if ((customers.Length == 0) && (dateFrom == null) && (dateTo == null))
            {
                SOAFormatBMainView<SOAFormatBView> _result = new SOAFormatBMainView<SOAFormatBView>();
                return PartialView("SOAFormatB/_List", _result);
            }
            List<SOAFormatB> result = await SOAFormatData.GetSOAFormatBListAsync(System.String.Join(";", customers), dateFrom, dateTo, beginningBalance, cancellationToken);
            SOAFormatBMainView<SOAFormatBView> _SOAFormatBMainView = new SOAFormatBMainView<SOAFormatBView>();
            _SOAFormatBMainView.Data = new List<SOAFormatBView>();

            int Unit_IPP_Total = 0;
            int Unit_PP_SC_Total = 0;
            int Unit_RTA_Total = 0;
            int Unit_SNS_Total = 0;
            decimal Amt_IPP_Total = 0;
            decimal Amt_PP_SC_Total = 0;
            decimal Amt_RTA_Total = 0;
            decimal Amt_SNS_Total = 0;
            decimal Amt_Total_Total = 0;
            decimal Sf_IPP_Total = 0;
            decimal Sf_PP_SC_Total = 0;
            decimal Sf_RTA_Total = 0;
            decimal Sf_SNS_Total = 0;
            decimal Sf_Total_Total = 0;
            decimal WithholdingTax_Total = 0;
            decimal Total_Total = 0;
            decimal Total_Total_2nd = 0;
            decimal Rv_IPP_Total = 0;
            decimal Rv_PP_SC_Total = 0;
            decimal Rv_RTA_Total = 0;
            decimal Rv_SNS_Total = 0;
            decimal Settlement_Total = 0;
            int indexer = -1;
            foreach (SOAFormatB soaFormatB in result)
            {
                indexer++;
                result[indexer].Sf_Total = soaFormatB.Sf_IPP + soaFormatB.Sf_PP_SC + soaFormatB.Sf_RTA + soaFormatB.Sf_SNS;
                result[indexer].WithholdingTax = (result[indexer].Sf_Total / Convert.ToDecimal(1.12)) * Convert.ToDecimal(0.02);
                result[indexer].Total = result[indexer].Amt_Total;
                result[indexer].Total_2nd = result[indexer].Amt_Total + result[indexer].Sf_Total - result[indexer].WithholdingTax;
                if (indexer == 0)
                {
                    result[indexer].RunningBalance = Convert.ToDecimal(beginningBalance) - result[indexer].Total + result[indexer].Settlement
                                                   + (result[indexer].Rv_SNS);
                    result[indexer].RunningBalance_2nd = Convert.ToDecimal(0) - result[indexer].Total_2nd;

                }
                else
                {
                    result[indexer].RunningBalance = result[indexer - 1].RunningBalance - result[indexer].Total + result[indexer].Settlement
                                                    + (result[indexer].Rv_SNS);
                    result[indexer].RunningBalance_2nd = result[indexer - 1].RunningBalance_2nd - result[indexer].Total_2nd;
                }

                _SOAFormatBMainView.Data.Add(FillSOAFormatBView(result[indexer]));

                //get customer settings from Admin module; As criteria in ENCASHMENT formula
                AgentListing customer = await CustomerData.GetCustomerAsync(null, null, customers[0], cancellationToken);

                _SOAFormatBMainView.Email_AFC_Amount = customer.ApprovedAFC.ToString("N");

                //if day minus 2
                if ((indexer + 1) == (result.Count - 1))
                {
                    _SOAFormatBMainView.Email_BeginningBalance_Amount = result[indexer].RunningBalance.ToString("N");
                    _SOAFormatBMainView.Email_BeginningBalance_Date = result[indexer].Date;
                }

                //if for the day
                if ((indexer + 1) == (result.Count))
                {
                    _SOAFormatBMainView.Email_Collections = result[indexer].Settlement.ToString("N");

                    //if Service Fee is auto-collect based on customer maintenance table, Encashment should be the total acceptance plus service fee
                    //if Service fee is billed, encashment should be the total acceptance only
                    if (customer.SFModeOfSettlement == SFModeOfSettlement.AutoCollectFromAFC)
                    {
                        _SOAFormatBMainView.Email_Encashments = (result[indexer].Amt_Total + result[indexer].Sf_Total).ToString("N");
                    }
                    else if (customer.SFModeOfSettlement == SFModeOfSettlement.Billed)
                    {
                        _SOAFormatBMainView.Email_Encashments = result[indexer].Amt_Total.ToString("N");
                    }

                    _SOAFormatBMainView.Email_EWT = result[indexer].WithholdingTax.ToString("N");
                    _SOAFormatBMainView.Email_EndingBalance_Amount = result[indexer].RunningBalance.ToString("N"); ;
                    _SOAFormatBMainView.Email_EndingBalance_Date = result[indexer].Date;
                }

                Unit_IPP_Total = Unit_IPP_Total + result[indexer].Unit_IPP;
                Unit_PP_SC_Total = Unit_PP_SC_Total + result[indexer].Unit_PP_SC;
                Unit_RTA_Total = Unit_RTA_Total + result[indexer].Unit_RTA;
                Unit_SNS_Total = Unit_SNS_Total + result[indexer].Unit_SNS;
                Amt_IPP_Total = Amt_IPP_Total + result[indexer].Amt_IPP;
                Amt_PP_SC_Total = Amt_PP_SC_Total + result[indexer].Amt_PP_SC;
                Amt_RTA_Total = Amt_RTA_Total + result[indexer].Amt_RTA;
                Amt_SNS_Total = Amt_SNS_Total + result[indexer].Amt_SNS;
                Amt_Total_Total = Amt_Total_Total + result[indexer].Amt_Total;
                Sf_IPP_Total = Sf_IPP_Total + result[indexer].Sf_IPP;
                Sf_PP_SC_Total = Sf_PP_SC_Total + result[indexer].Sf_PP_SC;
                Sf_RTA_Total = Sf_RTA_Total + result[indexer].Sf_RTA;
                Sf_SNS_Total = Sf_SNS_Total + result[indexer].Sf_SNS;
                Sf_Total_Total = Sf_Total_Total + result[indexer].Sf_Total;
                WithholdingTax_Total = WithholdingTax_Total + result[indexer].WithholdingTax;
                Total_Total = Total_Total + result[indexer].Total;
                Total_Total_2nd = Total_Total + result[indexer].Total_2nd;
                Rv_IPP_Total = Rv_IPP_Total + result[indexer].Rv_IPP;
                Rv_PP_SC_Total = Rv_PP_SC_Total + result[indexer].Rv_PP_SC;
                Rv_RTA_Total = Rv_RTA_Total + result[indexer].Rv_RTA;
                Rv_SNS_Total = Rv_SNS_Total + result[indexer].Rv_SNS;
                Settlement_Total = Settlement_Total + result[indexer].Settlement;
            }

            _SOAFormatBMainView.CustomerName = customers[0].ToString();
            _SOAFormatBMainView.Unit_IPP_Total = Unit_IPP_Total.ToString();
            _SOAFormatBMainView.Unit_PP_SC_Total = Unit_PP_SC_Total.ToString();
            _SOAFormatBMainView.Unit_RTA_Total = Unit_RTA_Total.ToString();
            _SOAFormatBMainView.Unit_SNS_Total = Unit_SNS_Total.ToString();
            _SOAFormatBMainView.Amt_PP_SC_Total = Amt_PP_SC_Total.ToString("N");
            _SOAFormatBMainView.Amt_RTA_Total = Amt_RTA_Total.ToString("N");
            _SOAFormatBMainView.Amt_SNS_Total = Amt_SNS_Total.ToString("N");
            _SOAFormatBMainView.Amt_Total_Total = Amt_Total_Total.ToString("N");
            _SOAFormatBMainView.Sf_IPP_Total = Sf_IPP_Total.ToString("N");
            _SOAFormatBMainView.Sf_PP_SC_Total = Sf_PP_SC_Total.ToString("N");
            _SOAFormatBMainView.Sf_RTA_Total = Sf_RTA_Total.ToString("N");
            _SOAFormatBMainView.Sf_SNS_Total = Sf_SNS_Total.ToString("N");
            _SOAFormatBMainView.Sf_Total_Total = Sf_Total_Total.ToString("N");
            _SOAFormatBMainView.WithholdingTax_Total = WithholdingTax_Total.ToString("N");
            _SOAFormatBMainView.Total_Total = Total_Total.ToString("N");
            _SOAFormatBMainView.Total_Total_2nd = Total_Total_2nd.ToString("N");
            _SOAFormatBMainView.Rv_IPP_Total = Rv_IPP_Total.ToString("N");
            _SOAFormatBMainView.Rv_PP_SC_Total = Rv_PP_SC_Total.ToString("N");
            _SOAFormatBMainView.Rv_RTA_Total = Rv_RTA_Total.ToString("N");
            _SOAFormatBMainView.Rv_SNS_Total = Rv_SNS_Total.ToString("N");
            _SOAFormatBMainView.Settlement_Total = Settlement_Total.ToString("N");

            _SOAFormatBMainView.Email_Adjustment = (Rv_IPP_Total + Rv_PP_SC_Total + Rv_RTA_Total + Rv_SNS_Total).ToString("N");
            _SOAFormatBMainView.Email_NumberOfUnitsProcessed = (Unit_IPP_Total + Unit_PP_SC_Total + Unit_RTA_Total + Unit_SNS_Total).ToString();

            //prepare storage for the SOA Format A Email
            HttpContext.Session.Remove(DefaultValues.SessionObjectSOAFormatBEmail);
            HttpContext.Session.SetObject(DefaultValues.SessionObjectSOAFormatBEmail, _SOAFormatBMainView);

            //prepare the variable that will hold the values for excel file import 
            HttpContext.Session.Remove(DefaultValues.SessionObjectExcelExportReport);
            HttpContext.Session.SetObject(DefaultValues.SessionObjectExcelExportReport, _SOAFormatBMainView.Data);

            return PartialView("SOAFormatB/_List", _SOAFormatBMainView);
        }

        private static SOAFormatAView FillSOAFormatAView(SOAFormatAData _SOAFormatA)
        {
            return new SOAFormatAView
            { 
                Date = _SOAFormatA.Date.ToString(),
                Unit_IPP = _SOAFormatA.Unit_IPP.ToString(),
                Unit_PP_SC = _SOAFormatA.Unit_PP_SC.ToString(),
                Unit_RTA = _SOAFormatA.Unit_RTA.ToString(),
                Unit_SNS = _SOAFormatA.Unit_SNS.ToString(),
                Amt_IPP = _SOAFormatA.Amt_IPP.ToString("N"),
                Amt_PP_SC = _SOAFormatA.Amt_PP_SC.ToString("N"),
                Amt_RTA = _SOAFormatA.Amt_RTA.ToString("N"),
                Amt_SNS = _SOAFormatA.Amt_SNS.ToString("N"),
                Amt_Total = _SOAFormatA.Amt_Total.ToString("N"),
                Sf_IPP = _SOAFormatA.Sf_IPP.ToString("N"),
                Sf_PP_SC = _SOAFormatA.Sf_PP_SC.ToString("N"),
                Sf_RTA = _SOAFormatA.Sf_RTA.ToString("N"),
                Sf_SNS = _SOAFormatA.Sf_SNS.ToString("N"),
                Sf_Total = _SOAFormatA.Sf_Total.ToString("N"),
                WithholdingTax = _SOAFormatA.WithholdingTax.ToString("N"),
                TotalLBCReceivable = _SOAFormatA.TotalLBCReceivable.ToString("N"),
                Rv_IPP = _SOAFormatA.Rv_IPP.ToString("N"),
                Rv_PP_SC = _SOAFormatA.Rv_PP_SC.ToString("N"),
                Rv_RTA = _SOAFormatA.Rv_RTA.ToString("N"),
                Rv_SNS = _SOAFormatA.Rv_SNS.ToString("N"),
                Settlement = _SOAFormatA.Settlement.ToString("N"),
                RunningBalance = _SOAFormatA.RunningBalance.ToString("N"),
                BalancePerAgent = _SOAFormatA.BalancePerAgent.ToString("N"),
                Variance = _SOAFormatA.Variance.ToString("N"),
                AcceptanceDocNumber = _SOAFormatA.AcceptanceDocNumber.ToString(),
                ServiceFeeDocNumber = _SOAFormatA.ServiceFeeDocNumber.ToString(),
            };
        }

        private static SOAFormatBView FillSOAFormatBView(SOAFormatB _SOAFormat)
        {
            return new SOAFormatBView
            {
                Date = Convert.ToDateTime(_SOAFormat.Date).ToString("MM/dd/yyyy"),
                Unit_IPP = _SOAFormat.Unit_IPP.ToString(),
                Unit_PP_SC = _SOAFormat.Unit_PP_SC.ToString(),
                Unit_RTA = _SOAFormat.Unit_RTA.ToString(),
                Unit_SNS = _SOAFormat.Unit_SNS.ToString(),
                Amt_IPP = _SOAFormat.Amt_IPP.ToString("N"),
                Amt_PP_SC = _SOAFormat.Amt_PP_SC.ToString("N"),
                Amt_RTA = _SOAFormat.Amt_RTA.ToString("N"),
                Amt_SNS = _SOAFormat.Amt_SNS.ToString("N"),
                Amt_Total = _SOAFormat.Amt_Total.ToString("N"),
                Sf_IPP = _SOAFormat.Sf_IPP.ToString("N"),
                Sf_PP_SC = _SOAFormat.Sf_PP_SC.ToString("N"),
                Sf_RTA = _SOAFormat.Sf_RTA.ToString("N"),
                Sf_SNS = _SOAFormat.Sf_SNS.ToString("N"),
                Sf_Total = _SOAFormat.Sf_Total.ToString("N"),
                WithholdingTax = _SOAFormat.WithholdingTax.ToString("N"),
                Total = _SOAFormat.Total.ToString("N"),
                Total_2nd = _SOAFormat.Total_2nd.ToString("N"),
                Rv_IPP = _SOAFormat.Rv_IPP.ToString("N"),
                Rv_PP_SC = _SOAFormat.Rv_PP_SC.ToString("N"),
                Rv_RTA = _SOAFormat.Rv_RTA.ToString("N"),
                Rv_SNS = _SOAFormat.Rv_SNS.ToString("N"),
                Settlement = _SOAFormat.Settlement.ToString("N"),
                RunningBalance = _SOAFormat.RunningBalance.ToString("N"),
                RunningBalance_2nd = _SOAFormat.RunningBalance_2nd.ToString("N"),
                BalancePerAgent = _SOAFormat.BalancePerAgent.ToString("N"),
                Variance = _SOAFormat.Variance.ToString("N"),
                AcceptanceDocNumber = _SOAFormat.AcceptanceDocNumber.ToString(),
                ServiceFeeDocNumber = _SOAFormat.ServiceFeeDocNumber.ToString(),
            };
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcelSOAFormatA(CancellationToken cancellationToken)
        {
            ResponseMessage result = new ResponseMessage();

            List<SOAFormatAView> _list = HttpContext.Session.GetComplexData<List<SOAFormatAView>>(DefaultValues.SessionObjectExcelExportReport) ?? new List<SOAFormatAView>();
            try
            {

                byte[] excel = _list.Select(x => new { x.Date, x.Unit_IPP, x.Unit_PP_SC, x.Unit_RTA, x.Unit_SNS, x.Amt_IPP, x.Amt_PP_SC, x.Amt_RTA, x.Amt_SNS, x.Amt_Total, x.Sf_IPP, x.Sf_PP_SC, x.Sf_RTA, x.Sf_SNS, x.Sf_Total, x.WithholdingTax, x.TotalLBCReceivable, x.Rv_IPP, x.Rv_PP_SC, x.Rv_RTA, x.Rv_SNS, x.Settlement, x.RunningBalance,x.BalancePerAgent,x.Variance,x.AcceptanceDocNumber,x.ServiceFeeDocNumber }).ToList().ToExcel();
                //Response.Headers.Add("content-disposition", $"attachment; filename=test");
                return File(excel, "application/octet-stream", "SOAFormatA.xlsx");
            }
            catch (Exception ex)
            {
                result.Status = false;
                //result.Reason = "List of Request excel file was not exported";
                result.Reason = ex.Message;
                return Json(result);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcelSOAFormatB(CancellationToken cancellationToken)
        {
            ResponseMessage result = new ResponseMessage();

            List<SOAFormatBView> _list = HttpContext.Session.GetComplexData<List<SOAFormatBView>>(DefaultValues.SessionObjectExcelExportReport) ?? new List<SOAFormatBView>();
            try
            {

                byte[] excel = _list.Select(x => new { x.Date, x.Unit_IPP, x.Unit_PP_SC, x.Unit_RTA, x.Unit_SNS, x.Amt_IPP, x.Amt_PP_SC, x.Amt_RTA, x.Amt_SNS, x.Amt_Total, x.Sf_IPP, x.Sf_PP_SC, x.Sf_RTA, x.Sf_SNS, x.Sf_Total, x.WithholdingTax, x.Total, x.Rv_IPP, x.Rv_PP_SC, x.Rv_RTA, x.Rv_SNS, x.Settlement, x.RunningBalance, x.BalancePerAgent, x.Variance, x.AcceptanceDocNumber, x.ServiceFeeDocNumber }).ToList().ToExcel();
                //Response.Headers.Add("content-disposition", $"attachment; filename=test");
                return File(excel, "application/octet-stream", "SOAFormatB.xlsx");
            }
            catch (Exception ex)
            {
                result.Status = false;
                //result.Reason = "List of Request excel file was not exported";
                result.Reason = ex.Message;
                return Json(result);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> GenerateSOAFormatAEmail(string[] customers, string dateFrom, string dateTo, string beginningBalance, CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName); 
            ResponseMessage result = new ResponseMessage();
            ResponseMessage result_sendEmail = new ResponseMessage();

            //Get customer list of emails
            AgentListing agentListing = await CustomerData.GetCustomerAsync(null, null, customers[0], cancellationToken);
            List<CustomerEmailAddress> customerEmailAddressList = await CustomerEmailAddressData.GetCustomerEmailAddressListAsync(agentListing.Id.ToString(), cancellationToken);
            List<CustomerDepositoryAccountNo> customerDepositoryAccountNoList = await CustomerDepositoryAccountNoData.GetCustomerDepositoryAccountNoListAsync(agentListing.Id.ToString(), cancellationToken);

            //get the depository account no
            CustomerDepositoryBankAccount customerDepositoryBankAccount = await CustomerDepositoryBankAccountData.GetCustomerDepositoryBankAccountAsync(agentListing.DepositoryBankAccountId.ToString(), cancellationToken);


            //result = await SendEmail(customers[0], customerEmailAddressList, customerDepositoryAccountNoList.First().DepositoryAccountNo, cancellationToken: cancellationToken);
            result = await SendEmail(customers[0], customerEmailAddressList, customerDepositoryBankAccount.AccountNo, cancellationToken: cancellationToken);
            return Json(result);
        }

        public async Task<ResponseMessage> SendEmail(string customerNmae, List<CustomerEmailAddress> customerEmailAddressList, string customerDepositoryAccountNo, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();

            SOAFormatAMainView<SOAFormatAView> _SOAFormatAMainView = HttpContext.Session.GetComplexData<SOAFormatAMainView<SOAFormatAView>>(DefaultValues.SessionObjectSOAFormatAEmail) ?? new SOAFormatAMainView<SOAFormatAView>();

            SmtpClient client = await ESOA_SMTP.SetSmtpClient();
            string subject = "";
            subject = "ESOA: SOA Format A";

            string emailBody = "";
            emailBody = "<p>Dear " + customerNmae + ",</p>"
                + "Please be advised that based on our records, hereunder is the summary of your transactions with LBCE as of  <current SOA date>"
                + "<br><br>"
                + "Please note that payments made after 5:00 PM yesterday are not yet reflected in this report. Thank you!"
                + "<br><br>"
                + "<table>"
                + "<tbody>"
                + "<tr>"
                + "<td style='border: 1px solid black;'>Agreed Advance Funding Cover (AFC)</td>"
                + "<td align='right' style='border: 1px solid black;text-aligned:right !important'>" + _SOAFormatAMainView.Email_AFC_Amount + "</td>"
                + "</tr>"
                + "<tr>"
                + "<td style='border: 1px solid black;'>(Deficit) Beginning balance – " + _SOAFormatAMainView.Email_BeginningBalance_Date + "</td>"
                + "<td align='right' style='border: 1px solid black;text-aligned:right !important;'>" + _SOAFormatAMainView.Email_BeginningBalance_Amount + "</td>"
                + "</tr>"
                + "<tr>"
                + "<td style='border: 1px solid black;'>Collections</td>"
                + "<td align='right' style='border: 1px solid black;text-aligned:right !important;'>" + _SOAFormatAMainView.Email_Collections + "</td>"
                + "</tr>"
                + "<tr>"
                + "<td style='border: 1px solid black;'>Encashments</td>"
                + "<td align='right' style='border: 1px solid black;text-aligned:right !important;'>" + _SOAFormatAMainView.Email_Encashments + "</td>"
                + "</tr>"
                + "<tr>"
                + "<td style='border: 1px solid black;'>EWT</td>"
                + "<td align='right' style='border: 1px solid black;text-aligned:right !important;'>" + _SOAFormatAMainView.Email_EWT + "</td>"
                + "</tr>"
                + "<tr>"
                + "<td style='border: 1px solid black;'>Adjustment</td>"
                + "<td align='right' style='border: 1px solid black;text-aligned:right !important;'>" + _SOAFormatAMainView.Email_Adjustment + "</td>"
                + "</tr>"
                + "<tr>"
                + "<td style='border: 1px solid black;'>(Deficit) Ending balance - " + _SOAFormatAMainView.Email_EndingBalance_Date + "</td>"
                + "<td align='right' style='border: 1px solid black; text-aligned:right !importantt;'>" + _SOAFormatAMainView.Email_EndingBalance_Amount + "</td>"
                + "</tr>"
                + "<tr>"
                + "<td style='border: 1px solid black;'>Number Of Units Processed</td>"
                + "<td align='right' style='border: 1px solid black; text-aligned:right !important;'>" + _SOAFormatAMainView.Email_NumberOfUnitsProcessed + "</td>"
                + "</tr>"
                + "</tbody>"
                + "</table>"
                + "<br>"
                + "<br>"
                + "Please settle your account with deficit of Php " + _SOAFormatAMainView.Email_EndingBalance_Amount
                + " not later than end-of-business day today to the following account number: "
                + "<br>"
                + customerDepositoryAccountNo
                + "<br><br>"
                + "For any concerns, please do not hesitate to contact the undersigned for reconciliation."
                + "<br>";


            MailMessage mailMessage = new MailMessage();
            foreach (CustomerEmailAddress customerEmailAddress in customerEmailAddressList) 
            { 
                if (customerEmailAddressList.First() == customerEmailAddress) 
                {
                    mailMessage = new MailMessage(DefaultValues.smtpClientSenderEmail, customerEmailAddress.EmailAddress, subject, emailBody);
                }
                else
                {
                    mailMessage.To.Add(customerEmailAddress.EmailAddress);
                } 
            }

            //include excel as attachment
            List<SOAFormatAView> _list = HttpContext.Session.GetComplexData<List<SOAFormatAView>>(DefaultValues.SessionObjectExcelExportReport) ?? new List<SOAFormatAView>();
            byte[] excelBytes = null;
            try
            {
                 excelBytes = _list.Select(x => new { x.Date, x.Unit_IPP, x.Unit_PP_SC, x.Unit_RTA, x.Unit_SNS, x.Amt_IPP, x.Amt_PP_SC, x.Amt_RTA, x.Amt_SNS, x.Amt_Total, x.Sf_IPP, x.Sf_PP_SC, x.Sf_RTA, x.Sf_SNS, x.Sf_Total, x.WithholdingTax, x.TotalLBCReceivable, x.Rv_IPP, x.Rv_PP_SC, x.Rv_RTA, x.Rv_SNS, x.Settlement, x.RunningBalance, x.BalancePerAgent, x.Variance, x.AcceptanceDocNumber, x.ServiceFeeDocNumber }).ToList().ToExcel();
            }  catch (Exception ex) {}
            using (var stream = new MemoryStream(excelBytes))
            {
                // Create attachment from stream
                var attachment = new Attachment(stream, "SOAFormatA.xlsx", MediaTypeNames.Application.Octet);
                mailMessage.Attachments.Add(attachment);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                try
                {
                    client.Send(mailMessage);
                    result.Status = true;
                    result.Reason = "Email sent succesful.";
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Reason = ex.Message;
                }
            }
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Verify(string customerName, string date, string remarks, CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            ResponseMessage result = new ResponseMessage();
            ResponseMessage result_sendEmail = new ResponseMessage();


            SOAVerifiedDates sOAVerifiedDates = new SOAVerifiedDates { OfficeCode = customerName, Remarks= remarks, Status=SOAStatus.Verified, TransactionDate=date};  
            result = await SOAVerifiedDatesData.CreateSOAVerifiedDatesAsync(sOAVerifiedDates, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result); 
        }

        [HttpPost]
        public async Task<IActionResult> Reverse(string customerName, string date, CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            ResponseMessage result = new ResponseMessage();
            ResponseMessage result_sendEmail = new ResponseMessage();

            result = await SOAVerifiedDatesData.DeleteSOAVerifiedDatesAsync(customerName, date, cancellationToken: cancellationToken);
            return Json(result);
        }

    }
}
