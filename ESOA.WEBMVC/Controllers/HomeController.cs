using ESOA.WEBMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ESOA.Model;
using ESOA.Model.Constants;
using ESOA.Common;
using EPDV.Controllers;

namespace ESOA.WEBMVC.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ViewLoginUserDetails(CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            UserAccount result = await UserAccountData.GetUserAccountAsync(userAccountId, cancellationToken: cancellationToken);
            return PartialView("_LoginUserDetails", result);
        }

        [HttpPost]
        public async Task<IActionResult> ViewAdminAccessModules(CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            UserAccount result = await UserAccountData.GetUserAccountAsync(userAccountId, cancellationToken: cancellationToken);
            if (result != null)
            {
                if (result.ModuleAccess_Admin == true)
                {
                    return PartialView("_AdminAccessModules");
                }
            }
            return PartialView("");
        }

        [HttpPost]
        public async Task<IActionResult> ViewGranularAccessModules(CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            UserAccount result = await UserAccountData.GetUserAccountAsync(userAccountId, cancellationToken: cancellationToken);
            if (result != null)
            {
                if (result.ModuleAccess_Granular == true)
                {
                    return PartialView("_GranularAccessModules");
                }
            }
            return PartialView("");
        }

        [HttpPost]
        public async Task<IActionResult> ViewSoaAccessModules(CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            UserAccount result = await UserAccountData.GetUserAccountAsync(userAccountId, cancellationToken: cancellationToken);
            if (result != null)
            {
                if (result.ModuleAccess_SOA == true)
                {
                    return PartialView("_SoaAccessModules");
                }
            }
            return PartialView("");
        }

        [HttpPost]
        public async Task<IActionResult> ViewPaymentAccessModules(CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            UserAccount result = await UserAccountData.GetUserAccountAsync(userAccountId, cancellationToken: cancellationToken);
            if (result != null)
            {
                if (result.ModuleAccess_Payment == true)
                {
                    return PartialView("_PaymentAccessModules");
                }
            }
            return PartialView("");
        }

        [HttpPost]
        public async Task<IActionResult> ViewReportsAccessModules(CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            UserAccount result = await UserAccountData.GetUserAccountAsync(userAccountId, cancellationToken: cancellationToken);
            if (result != null)
            {
                if (result.ModuleAccess_Reports == true)
                {
                    return PartialView("_ReportsAccessModules");
                }
            }
            return PartialView("");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
