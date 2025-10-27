using ESOA.WEBMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ESOA.Model;
using ESOA.Model.View;
using ESOA.Model.Constants;
using ESOA.Common;
using EPDV.Controllers;
using ESOA.Model.Constant;

namespace ESOA.WEBMVC.Controllers
{
    public class ReportController : BaseController
    {
        private readonly ILogger<ReportController> _logger;

        public ReportController(ILogger<ReportController> logger)
        {
            _logger = logger;
        }

        public IActionResult AgentBalances()
        {
            return View();
        }

        public IActionResult AgentListing()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ViewFilterOptions(CancellationToken cancellationToken)
        {
            return PartialView("_filterOptions");
        }

        [HttpPost]
        public async Task<IActionResult> SearchAgentBalances_Payable(string DateAsOf, string InitialPageLoad, CancellationToken cancellationToken)
        {
            List<AgentBalancesView> result;
            if (InitialPageLoad=="yes") 
            {
                result = new List<AgentBalancesView>();
            }
            else
            {
                result = await AgentBalancesData.GetAgentBalancesPayableListAsync(DateAsOf, cancellationToken);
            }
            
            return PartialView("AgentBalances/_List_Payable", result);
        }

        [HttpPost]
        public async Task<IActionResult> ViewChartAgentBalances_Payable(string DateAsOf, string InitialPageLoad, CancellationToken cancellationToken)
        {
            List<AgentBalancesView> result;
            if (InitialPageLoad == "yes")
            {
                result = new List<AgentBalancesView>();
            }
            else
            {
                result = await AgentBalancesData.GetAgentBalancesPayableListAsync(DateAsOf, cancellationToken);
            }

            return PartialView("AgentBalances/_Chart_Payable", result);
        }

        [HttpPost]
        public async Task<IActionResult> SearchAgentBalances_Advance(string DateAsOf, string InitialPageLoad, CancellationToken cancellationToken)
        {

            List<AgentBalancesView> result;
            if (InitialPageLoad == "yes")
            {
                result = new List<AgentBalancesView>();
            }
            else
            {
                result = await AgentBalancesData.GetAgentBalancesAdvanceListAsync(DateAsOf, cancellationToken);
            }

            return PartialView("AgentBalances/_List_Advance", result);
        }

        public async Task<IActionResult> ViewChartAgentBalances_Advance(string DateAsOf, string InitialPageLoad, CancellationToken cancellationToken)
        {
            List<AgentBalancesView> result;
            if (InitialPageLoad == "yes")
            {
                result = new List<AgentBalancesView>();
            }
            else
            {
                result = await AgentBalancesData.GetAgentBalancesAdvanceListAsync(DateAsOf, cancellationToken);
            }

            return PartialView("AgentBalances/_Chart_Advance", result);
        }

        [HttpPost]
        public async Task<IActionResult> SearchAgentListing(string InitialPageLoad, CancellationToken cancellationToken)
        {
            List<AgentListingView> result;
            if (InitialPageLoad == "yes")
            {
                result = new List<AgentListingView>();
            }
            else
            {
                result = await AgentListingData.GetAgentListingViewListAsync(cancellationToken);
            }

            return PartialView("AgentListing/_List", result);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
