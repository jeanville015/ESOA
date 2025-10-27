using ESOA.Model;
using ESOA.Common;
using Microsoft.AspNetCore.Mvc;
using ESOA.Model.Constant;
using ESOA.Model.Constants;
using System.Net.Mail;
using System.Text;
using ArrayToExcel;

namespace ESOA.WEBMVC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Customer()
        {
            return View();
        }

        public IActionResult UserAccount()
        {
            return View();
        }

        public IActionResult Rate()
        {
            //bool test = false;
            //bool verify;
            //if (test == false) 
            //{ 
            //    verify = true;
            //    return RedirectToAction("Index", "Home"); 
            //}

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchUserAccount([FromBody] UserAccountSearchRequest request, CancellationToken cancellationToken)
        {
            PaginationResponse<UserAccount> result = await UserAccountData.UserAccountSearchAsync(request, cancellationToken);
            return PartialView("UserAccount/_List", result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAccount(Guid userAccountId, CancellationToken cancellationToken)
        {
            UserAccount result = await UserAccountData.GetUserAccountAsync(userAccountId.ToString(), cancellationToken: cancellationToken);
            //ViewBag.JobTitleList = JobTitle.List();
            ViewBag.TeamList = Team.List();
            ViewBag.RoleList = Role.List();

            return PartialView("UserAccount/_UserAccountDetail", result);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAccount(UserAccount userAccount, CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);

            ResponseMessage result = new ResponseMessage();
            ResponseMessage result_sendEmail = new ResponseMessage();

            //check if the email provided already exist
            UserAccount checkUserAccount = await UserAccountData.GetUserAccountAsync(null, null, null, userAccount.EmailAddress, cancellationToken: cancellationToken);
            if (checkUserAccount != null) 
            {
                result.Status = false;
                result.Reason = "Email address you entered already exist!";
                return Json(result);
            }

            userAccount.Password = GeneratePassword();
            result = await UserAccountData.CreateUserAccountAsync(userAccount, userAccountId.ToString(), cancellationToken: cancellationToken);
            result_sendEmail = await SendPasswordEmail(result.Guid.ToString(), userAccount.Password, cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserAccount(UserAccount userAccount, CancellationToken cancellationToken)
        {
            //temp; for testing
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName); 

            ResponseMessage result = new ResponseMessage();

            //check if the email provided already exist in other user account
            UserAccount checkUserAccount = await UserAccountData.GetUserAccountAsync(null, null, null, userAccount.EmailAddress, cancellationToken: cancellationToken);
            if ((checkUserAccount != null) && (checkUserAccount.Id != userAccount.Id))
            {
                result.Status = false;
                result.Reason = "Email address you entered already exist!";
                return Json(result);
            }

            result = await UserAccountData.UpdateUserAccountAsync(userAccount, userAccountId.ToString(), cancellationToken: cancellationToken);


            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserAccountNVP([FromBody] NameValuePair nameValuePair, CancellationToken cancellationToken)
        {
            //temp; for testing
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            ResponseMessage result = new ResponseMessage();
            result = await UserAccountData.UpdateUserAccountNVPAsync(nameValuePair, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SearchRate([FromBody] RateSearchRequest request, CancellationToken cancellationToken)
        {
            PaginationResponse<Rate> result = await RateData.RateSearchAsync(request, cancellationToken);
            return PartialView("Rate/_List", result);
        }

        [HttpGet]
        public async Task<IActionResult> GetRate(Guid rateId, CancellationToken cancellationToken)
        {
            Rate result = await RateData.GetRateAsync(rateId.ToString(), cancellationToken: cancellationToken);
            ViewBag.RateTypeList = RateType.List();

            return PartialView("Rate/_RateDetail", result);
        }

        [HttpPost]
        public async Task<IActionResult> AddRate(Rate rate, CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);

            ResponseMessage result = new ResponseMessage();
            result = await RateData.CreateRateAsync(rate, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRate(Rate rate, CancellationToken cancellationToken)
        {
            //temp; for testing
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName); //Guid userAccountId = new Guid(DefaultValues.defaultUserId); ;

            ResponseMessage result = new ResponseMessage();
            result = await RateData.UpdateRateAsync(rate, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRateNVP([FromBody] NameValuePair nameValuePair, CancellationToken cancellationToken)
        {
            //temp; for testing
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            ResponseMessage result = new ResponseMessage();
            result = await RateData.UpdateRateNVPAsync(nameValuePair, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ModuleCustomerEmailAddress(string customerId, CancellationToken cancellationToken)
        {
            ViewBag.CustomerId = customerId;
            return PartialView("Customer/EmailAddress");
        }

        [HttpPost]
        public async Task<IActionResult> SearchCustomerEmailAddress(string customerId, CancellationToken cancellationToken)
        {
            List<CustomerEmailAddress> result = await CustomerEmailAddressData.GetCustomerEmailAddressListAsync(customerId, cancellationToken);
            return PartialView("Customer/EmailAddress/_List", result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerEmailAddress(Guid customerEmailAddressId, CancellationToken cancellationToken)
        {
            CustomerEmailAddress result = await CustomerEmailAddressData.GetCustomerEmailAddressAsync(customerEmailAddressId.ToString(), cancellationToken: cancellationToken); 
            return PartialView("Customer/EmailAddress/_CustomerEmailAddressDetail", result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerEmailAddress(CustomerEmailAddress customerEmailAddress, CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);

            ResponseMessage result = new ResponseMessage();
            result = await CustomerEmailAddressData.CreateCustomerEmailAddressAsync(customerEmailAddress, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomerEmailAddress(CustomerEmailAddress customerEmailAddress, CancellationToken cancellationToken)
        {
            //temp; for testing
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName); //Guid userAccountId = new Guid(DefaultValues.defaultUserId); ;

            ResponseMessage result = new ResponseMessage();
            result = await CustomerEmailAddressData.UpdateCustomerEmailAddressAsync(customerEmailAddress, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomerEmailAddressNVP([FromBody] NameValuePair nameValuePair, CancellationToken cancellationToken)
        {
            //temp; for testing
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            ResponseMessage result = new ResponseMessage();
            result = await CustomerEmailAddressData.UpdateCustomerEmailAddressNVPAsync(nameValuePair, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ModuleCustomerDepositoryAccountNo(string customerId, CancellationToken cancellationToken)
        {
            ViewBag.CustomerId = customerId;
            return PartialView("Customer/DepositoryAccountNo");
        }

        [HttpPost]
        public async Task<IActionResult> SearchCustomerDepositoryAccountNo(string customerId, CancellationToken cancellationToken)
        {
            List<CustomerDepositoryAccountNo> result = await CustomerDepositoryAccountNoData.GetCustomerDepositoryAccountNoListAsync(customerId, cancellationToken);
            return PartialView("Customer/DepositoryAccountNo/_List", result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerDepositoryAccountNo(Guid customerDepositoryAccountNoId, CancellationToken cancellationToken)
        {
            CustomerDepositoryAccountNo result = await CustomerDepositoryAccountNoData.GetCustomerDepositoryAccountNoAsync(customerDepositoryAccountNoId.ToString(), cancellationToken: cancellationToken);
            return PartialView("Customer/DepositoryAccountNo/_CustomerDepositoryAccountNoDetail", result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerDepositoryAccountNo(CustomerDepositoryAccountNo customerDepositoryAccountNo, CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);

            ResponseMessage result = new ResponseMessage();
            result = await CustomerDepositoryAccountNoData.CreateCustomerDepositoryAccountNoAsync(customerDepositoryAccountNo, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomerDepositoryAccountNo(CustomerDepositoryAccountNo customerDepositoryAccountNo, CancellationToken cancellationToken)
        {
            //temp; for testing
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName); //Guid userAccountId = new Guid(DefaultValues.defaultUserId); ;

            ResponseMessage result = new ResponseMessage();
            result = await CustomerDepositoryAccountNoData.UpdateCustomerDepositoryAccountNoAsync(customerDepositoryAccountNo, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomerDepositoryAccountNoNVP([FromBody] NameValuePair nameValuePair, CancellationToken cancellationToken)
        {
            //temp; for testing
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            ResponseMessage result = new ResponseMessage();
            result = await CustomerDepositoryAccountNoData.UpdateCustomerDepositoryAccountNoNVPAsync(nameValuePair, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ModuleCustomerContactNo(string customerId, CancellationToken cancellationToken)
        {
            ViewBag.CustomerId = customerId;
            return PartialView("Customer/ContactNo");
        }

        [HttpPost]
        public async Task<IActionResult> SearchCustomerContactNo(string customerId, CancellationToken cancellationToken)
        {
            List<CustomerContactNo> result = await CustomerContactNoData.GetCustomerContactNoListAsync(customerId, cancellationToken);
            return PartialView("Customer/ContactNo/_List", result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerContactNo(Guid customerContactNoId, CancellationToken cancellationToken)
        {
            CustomerContactNo result = await CustomerContactNoData.GetCustomerContactNoAsync(customerContactNoId.ToString(), cancellationToken: cancellationToken);
            return PartialView("Customer/ContactNo/_CustomerContactNoDetail", result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerContactNo(CustomerContactNo customerContactNo, CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);

            ResponseMessage result = new ResponseMessage();
            result = await CustomerContactNoData.CreateCustomerContactNoAsync(customerContactNo, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomerContactNo(CustomerContactNo customerContactNo, CancellationToken cancellationToken)
        {
            //temp; for testing
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName); //Guid userAccountId = new Guid(DefaultValues.defaultUserId); ;

            ResponseMessage result = new ResponseMessage();
            result = await CustomerContactNoData.UpdateCustomerContactNoAsync(customerContactNo, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomerContactNoNVP([FromBody] NameValuePair nameValuePair, CancellationToken cancellationToken)
        {
            //temp; for testing
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            ResponseMessage result = new ResponseMessage();
            result = await CustomerContactNoData.UpdateCustomerContactNoNVPAsync(nameValuePair, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ModuleCustomerContactPerson(string customerId, CancellationToken cancellationToken)
        {
            ViewBag.CustomerId = customerId;
            return PartialView("Customer/ContactPerson");
        }

        [HttpPost]
        public async Task<IActionResult> SearchCustomerContactPerson(string customerId, CancellationToken cancellationToken)
        {
            List<CustomerContactPerson> result = await CustomerContactPersonData.GetCustomerContactPersonListAsync(customerId, cancellationToken);
            return PartialView("Customer/ContactPerson/_List", result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerContactPerson(Guid customerContactPersonId, CancellationToken cancellationToken)
        {
            CustomerContactPerson result = await CustomerContactPersonData.GetCustomerContactPersonAsync(customerContactPersonId.ToString(), cancellationToken: cancellationToken);
            return PartialView("Customer/ContactPerson/_CustomerContactPersonDetail", result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerContactPerson(CustomerContactPerson customerContactPerson, CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);

            ResponseMessage result = new ResponseMessage();
            result = await CustomerContactPersonData.CreateCustomerContactPersonAsync(customerContactPerson, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomerContactPerson(CustomerContactPerson customerContactPerson, CancellationToken cancellationToken)
        {
            //temp; for testing
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName); //Guid userAccountId = new Guid(DefaultValues.defaultUserId); ;

            ResponseMessage result = new ResponseMessage();
            result = await CustomerContactPersonData.UpdateCustomerContactPersonAsync(customerContactPerson, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomerContactPersonNVP([FromBody] NameValuePair nameValuePair, CancellationToken cancellationToken)
        {
            //temp; for testing
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            ResponseMessage result = new ResponseMessage();
            result = await CustomerContactPersonData.UpdateCustomerContactPersonNVPAsync(nameValuePair, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SearchCustomer([FromBody] CustomerSearchRequest request, CancellationToken cancellationToken)
        {
            PaginationResponse<AgentListing> result = await CustomerData.CustomerSearchAsync(request, cancellationToken);
            return PartialView("Customer/_List_freezePane", result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomer(Guid customerId, CancellationToken cancellationToken)
        {
            AgentListing result = await CustomerData.GetCustomerAsync(customerId.ToString(), cancellationToken: cancellationToken);
            
            List<SOAFormat> soaFormatList = await SOAFormatData.GetSOAFormatListAsync(cancellationToken: cancellationToken);
            ViewBag.SOAFormatList = soaFormatList.Select(x => new NameValuePair { Value = Data.GetString(x.Id), Name = x.FormatName }).ToList();

            List<Rate> rateList = await RateData.GetRateListAsync(cancellationToken: cancellationToken);
            ViewBag.RateList = rateList.Select(x => new NameValuePair { Value = Data.GetString(x.Id), Name = x.Reference }).ToList();

            List<CustomerDepositoryBankAccount> depositoryBankAccountList = await CustomerDepositoryBankAccountData.GetCustomerDepositoryAccountNoListAsync(cancellationToken: cancellationToken);
            ViewBag.DepositoryBankAccountList = depositoryBankAccountList.Select(x => new NameValuePair { Value = Data.GetString(x.Id), Name = x.AccountNo }).ToList();

            ViewBag.Domestic_IntlList = Domestic_Intl.List();
            ViewBag.TransmissionModeList = TransmissionMode.List();
            ViewBag.PaymentCurrencyList = PaymentCurrency.List();
            ViewBag.SFModeOfSettlementList = SFModeOfSettlement.List();
            ViewBag.WithholdingTaxList = WithholdingTax.List();
            ViewBag.VatStatusList = VatStatus.List();
            ViewBag.StatusList = Status.List();

            return PartialView("Customer/_CustomerDetail", result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDepositoryBankAccount_No(string depositoryBankAccountId, CancellationToken cancellationToken)
        { 
            CustomerDepositoryBankAccount customerDepositoryBankAccount = new CustomerDepositoryBankAccount();
            customerDepositoryBankAccount = await CustomerDepositoryBankAccountData.GetCustomerDepositoryBankAccountAsync(depositoryBankAccountId.ToString(), cancellationToken: cancellationToken);

            return Json(customerDepositoryBankAccount);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(AgentListing customer, CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            ResponseMessage result = new ResponseMessage();

            //check for duplicate 'SAP Customer ID'
            AgentListing checkCustomerSAPId = await CustomerData.GetCustomerAsync(null, customer.SAPCustomerId.ToString(), cancellationToken: cancellationToken);
            if(checkCustomerSAPId != null && (customer.SAPCustomerId != 0))
            {
                result.Status = false;
                result.Reason = "SAP Customer ID already exist.";
                return Json(result);
            }

            //check for duplicate 'SAP Vendor ID IPP'
            AgentListing checkCustomerSAPVendorId_IPP = await CustomerData.GetCustomerAsync(null, customer.SAPVendorId_IPP.ToString(), cancellationToken: cancellationToken);
            if (checkCustomerSAPVendorId_IPP != null && (customer.SAPVendorId_IPP != 0))
            {
                result.Status = false;
                result.Reason = "SAP Vendor ID IPP already exist.";
                return Json(result);
            }

            //check for duplicate 'SAP Vendor ID PP/SC'
            AgentListing checkCustomerSAPVendorId_PP_SC = await CustomerData.GetCustomerAsync(null, customer.SAPVendorId_PP_SC.ToString(), cancellationToken: cancellationToken);
            if (checkCustomerSAPVendorId_PP_SC != null && (customer.SAPVendorId_PP_SC != 0))
            {
                result.Status = false;
                result.Reason = "SAP Vendor ID PP/SC already exist.";
                return Json(result);
            }

            //check for duplicate 'SAP Vendor ID RTA'
            AgentListing checkCustomerSAPVendorId_RTA = await CustomerData.GetCustomerAsync(null, customer.SAPVendorId_RTA.ToString(), cancellationToken: cancellationToken);
            if (checkCustomerSAPVendorId_RTA != null && (customer.SAPVendorId_RTA != 0))
            {
                result.Status = false;
                result.Reason = "SAP Vendor ID RTA already exist.";
                return Json(result);
            }

            //check for duplicate 'SAP Vendor ID SNS'
            AgentListing checkCustomerSAPVendorId_SNS = await CustomerData.GetCustomerAsync(null, customer.SAPVendorId_SNS.ToString(), cancellationToken: cancellationToken);
            if (checkCustomerSAPVendorId_SNS != null && (customer.SAPVendorId_SNS != 0))
            {
                result.Status = false;
                result.Reason = "SAP Vendor ID SNS already exist.";
                return Json(result);
            }

            //check for duplicate 'SAP Vendor ID IPPX'
            AgentListing checkCustomerSAPVendorId_IPPX = await CustomerData.GetCustomerAsync(null, customer.SAPVendorId_IPPX.ToString(), cancellationToken: cancellationToken);
            if (checkCustomerSAPVendorId_IPPX != null && (customer.SAPVendorId_IPPX != 0))
            {
                result.Status = false;
                result.Reason = "SAP Vendor ID IPPX already exist.";
                return Json(result);
            }


            result = await CustomerData.CreateCustomerAsync(customer, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(AgentListing customer, CancellationToken cancellationToken)
        {
            //temp; for testing
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName); //Guid userAccountId = new Guid(DefaultValues.defaultUserId); ;
            
            ResponseMessage result = new ResponseMessage();

            //check for duplicate 'SAP Customer ID'
            AgentListing checkCustomerSAPId = await CustomerData.GetCustomerAsync(null, customer.SAPCustomerId.ToString(), cancellationToken: cancellationToken);
            if ((checkCustomerSAPId != null && (customer.SAPCustomerId != 0)) && checkCustomerSAPId.Id != customer.Id)
            {
                result.Status = false;
                result.Reason = "SAP Customer ID already exist.";
                return Json(result);
            }

            //check for duplicate 'SAP Vendor ID IPP'
            AgentListing checkCustomerSAPVendorId_IPP = await CustomerData.GetCustomerAsync(null, customer.SAPVendorId_IPP.ToString(), cancellationToken: cancellationToken);
            if ((checkCustomerSAPVendorId_IPP != null && (customer.SAPVendorId_IPP != 0)) && checkCustomerSAPVendorId_IPP.Id != customer.Id)
            {
                result.Status = false;
                result.Reason = "SAP Vendor ID IPP already exist.";
                return Json(result);
            }

            //check for duplicate 'SAP Vendor ID PP/SC'
            AgentListing checkCustomerSAPVendorId_PP_SC = await CustomerData.GetCustomerAsync(null, customer.SAPVendorId_PP_SC.ToString(), cancellationToken: cancellationToken);
            if ((checkCustomerSAPVendorId_PP_SC != null && (customer.SAPVendorId_PP_SC != 0)) && checkCustomerSAPVendorId_PP_SC.Id != customer.Id)
            {
                result.Status = false;
                result.Reason = "SAP Vendor ID PP/SC already exist.";
                return Json(result);
            }

            //check for duplicate 'SAP Vendor ID RTA'
            AgentListing checkCustomerSAPVendorId_RTA = await CustomerData.GetCustomerAsync(null, customer.SAPVendorId_RTA.ToString(), cancellationToken: cancellationToken);
            if ((checkCustomerSAPVendorId_RTA != null && (customer.SAPVendorId_RTA != 0)) && checkCustomerSAPVendorId_RTA.Id != customer.Id)
            {
                result.Status = false;
                result.Reason = "SAP Vendor ID RTA already exist.";
                return Json(result);
            }

            //check for duplicate 'SAP Vendor ID SNS'
            AgentListing checkCustomerSAPVendorId_SNS = await CustomerData.GetCustomerAsync(null, customer.SAPVendorId_SNS.ToString(), cancellationToken: cancellationToken);
            if ((checkCustomerSAPVendorId_SNS != null && (customer.SAPVendorId_SNS != 0)) && checkCustomerSAPVendorId_SNS.Id != customer.Id)
            {
                result.Status = false;
                result.Reason = "SAP Vendor ID SNS already exist.";
                return Json(result);
            }

            //check for duplicate 'SAP Vendor ID IPPX'
            AgentListing checkCustomerSAPVendorId_IPPX = await CustomerData.GetCustomerAsync(null, customer.SAPVendorId_IPPX.ToString(), cancellationToken: cancellationToken);
            if ((checkCustomerSAPVendorId_IPPX != null && (customer.SAPVendorId_IPPX != 0)) && checkCustomerSAPVendorId_IPPX.Id != customer.Id)
            {
                result.Status = false;
                result.Reason = "SAP Vendor ID IPPX already exist.";
                return Json(result);
            } 

            result = await CustomerData.UpdateCustomerAsync(customer, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomerNVP([FromBody] NameValuePair nameValuePair, CancellationToken cancellationToken)
        {
            //temp; for testing
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            ResponseMessage result = new ResponseMessage();
            result = await CustomerData.UpdateCustomerNVPAsync(nameValuePair, userAccountId.ToString(), cancellationToken: cancellationToken);
            return Json(result);
        }

        static string GeneratePassword()
        {
            const string symbols = "!@#$%^&*()_+-=";
            const string numbers = "0123456789";
            const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var random = new Random();

            // Generate a random password with at least one symbol, one number, and one letter
            string password = new string(Enumerable.Repeat(symbols, 1)
                .Concat(Enumerable.Repeat(numbers, 1))
                .Concat(Enumerable.Repeat(letters, 1))
                .Select(s => s[random.Next(s.Length)])
                .Concat(Enumerable.Repeat(symbols + numbers + letters, 5)
                    .Select(s => s[random.Next(s.Length)]))
                .ToArray());

            return password;
        }

        public static async Task<ResponseMessage> SendPasswordEmail(string userAccountId, string password, CancellationToken cancellationToken = default)
        {
            ResponseMessage result = new ResponseMessage();
            UserAccount userAccount;

            userAccount = await UserAccountData.GetUserAccountAsync(userAccountId, null, null, null, cancellationToken: cancellationToken);

            SmtpClient client = await ESOA_SMTP.SetSmtpClient();
            string subject = "";
            subject = "ESOA: New Account Credentials";

            string emailBody = "";
            emailBody = "<p>Hi " + userAccount.Name + ",</p>"
                + "<p>Please take note of your E-SOA account details below:"
                + "<br><br>"
                + "<b>EMAIL:</b> " + userAccount.EmailAddress
                + "<br>"
                + "<b>PASSWORD:</b> " + password
                + "<br>"
                + "<p>Link: https://devuatlbc-esoa.azurewebsites.net/</p>";

            MailMessage mailMessage = new MailMessage(DefaultValues.smtpClientSenderEmail, userAccount.EmailAddress, subject, emailBody);


            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = UTF8Encoding.UTF8;
            try
            {
                client.Send(mailMessage);
                result.Status = true;
                result.Reason = "New User Password sent succesful.";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Reason = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcelUserAccount(CancellationToken cancellationToken)
        {
            ResponseMessage result = new ResponseMessage();
            List<UserAccount_Excel> _list = await UserAccountData.GetRequestExcelList(cancellationToken);
            try
            {

                byte[] excel = _list.Select(x => new { x.Name, x.JobTitle, x.Team, x.Role, x.ModuleAccess_Admin, x.ModuleAccess_SOA, x.ModuleAccess_Payment, x.ModuleAccess_Reports, x.ModuleAccess_Granular, x.AccessRights_Admin, x.AccessRights_SOA, x.AccessRights_Payment, x.AccessRights_Reports, x.AccessRights_Granular, x.EmailAddress, x.ContactNo, }).ToList().ToExcel();
                //Response.Headers.Add("content-disposition", $"attachment; filename=test");
                return File(excel, "application/octet-stream", "UserAccount.xlsx");
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
        public async Task<IActionResult> ExportToExcelCustomer(CancellationToken cancellationToken)
        {
            ResponseMessage result = new ResponseMessage();
            List<Customer_Excel> _list = await CustomerData.GetRequestExcelList(cancellationToken);

            //INSERT here the customer email, contact no, contact person
            foreach (Customer_Excel customer in _list) {
                List<CustomerEmailAddress> current_customerEmailAddresses = await CustomerEmailAddressData.GetCustomerEmailAddressListAsync(customer.Id.ToString());
                List<CustomerContactNo> current_customerContactNos = await CustomerContactNoData.GetCustomerContactNoListAsync(customer.Id.ToString());
                List<CustomerContactPerson> current_customerContactPersons = await CustomerContactPersonData.GetCustomerContactPersonListAsync(customer.Id.ToString());
                string str_current_customerEmailAddresses= null;
                if (current_customerEmailAddresses.Count > 0)
                {
                    foreach (CustomerEmailAddress customerEmailAddress in current_customerEmailAddresses)
                    {
                        str_current_customerEmailAddresses = str_current_customerEmailAddresses + customerEmailAddress.EmailAddress + ", ";
                    }
                }
                string str_current_customerContactNos = null;
                if (current_customerContactNos.Count > 0) {
                    foreach(CustomerContactNo customerContactNo in current_customerContactNos)
                    {
                        str_current_customerContactNos = str_current_customerContactNos + customerContactNo.ContactNo + ", ";
                    } 
                }
                string str_current_customerContactPersons = null;
                if (current_customerContactPersons.Count > 0)
                {
                    foreach (CustomerContactPerson customerContactPerson in current_customerContactPersons)
                    {
                        str_current_customerContactPersons = str_current_customerContactPersons + customerContactPerson.ContactPerson + ", ";
                    }
                }
                customer.EmailAddresses = str_current_customerEmailAddresses;
                customer.ContactNos = str_current_customerContactNos;
                customer.ContactPersons = str_current_customerContactPersons;
            }

            try
            {

                byte[] excel = _list.Select(x => new { x.Name, x.LegalEntityName, x.Tin, x.Address, x.SalesExec_LBC, x.EmailAddresses, x.ContactNos, x.ContactPersons, x.ApprovedAFC, x.SOAFormat, x.RateCard, x.Domestic_Intl, x.Country, x.TransmissionMode, x.OfficeCode, x.Area, x.SAPCustomerId, x.SAPVendorId_IPP, x.SAPVendorId_PP_SC, x.SAPVendorId_RTA, x.SAPVendorId_SNS, x.SAPVendorId_IPPX, x.PaymentCurrency, x.SFModeOfSettlement, x.WithHoldingTax, x.VatStatus, x.Status }).ToList().ToExcel();
                //Response.Headers.Add("content-disposition", $"attachment; filename=test");
                return File(excel, "application/octet-stream", "Customer.xlsx");
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
        public async Task<IActionResult> ExportToExcelRAte(CancellationToken cancellationToken)
        {
            ResponseMessage result = new ResponseMessage();
            List<Rate_Excel> _list = await RateData.GetRequestExcelList(cancellationToken);
            try
            {

                byte[] excel = _list.Select(x => new { x.Reference, x.IPP, x.RateType_IPP, x.PP_SC, x.RateType_PP_SC, x.RTA, x.RateType_RTA, x.SNS, x.RateType_SNS, x.IPPX, x.RateType_IPPX, x.From, x.To }).ToList().ToExcel();
                //Response.Headers.Add("content-disposition", $"attachment; filename=test");
                return File(excel, "application/octet-stream", "Rate.xlsx");
            }
            catch (Exception ex)
            {
                result.Status = false;
                //result.Reason = "List of Request excel file was not exported";
                result.Reason = ex.Message;
                return Json(result);
            }
        }



    }
}
