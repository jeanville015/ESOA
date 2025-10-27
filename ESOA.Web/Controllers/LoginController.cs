using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESOA.Model.View;
using ESOA.Model;
using ESOA.Common;
using ESOA.Model.Constants;

namespace ESOA.Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginView loginView, CancellationToken cancellationToken)
        {
            HttpContext.Session.Remove(DefaultValues.SessionUserKeyNameFirstTimeLogin);
            HttpContext.Session.Remove(DefaultValues.SessionUserKeyName);

            UserAccount currentUserAccount = new UserAccount();
            currentUserAccount = await UserAccountData.GetUserAccountAsync(null, loginView.Username, loginView.Password, null, cancellationToken);
            LoginResult result = new LoginResult();
            if (currentUserAccount == null)
            {
                result.IsValidated = false;
                result.ReturnURL = null;
                result.Message = "Incorrect Email/Password";
                return Json(result);
            }
            else if (currentUserAccount != null)
            {
                //if (currentUserAccount.IsFirstTimeLogin)
                //{
                //    result.Message = "New user needs to change password first. ";
                //    result.IsValidated = true;
                //    result.ReturnURL = null;
                //    result.IsFirstTimeLogin = currentUserAccount.IsFirstTimeLogin;
                //    result.CurrentUserId = currentUserAccount.Id.ToString();

                //    //HttpContext.Session.Remove(DefaultValues.SessionUserKeyNameFirstTimeLogin);
                //    HttpContext.Session.SetString(DefaultValues.SessionUserKeyNameFirstTimeLogin, currentUserAccount.Id.ToString());
                //}
                //else
                //{
                //    //HttpContext.Session.Remove(DefaultValues.SessionUserKeyName);
                //    HttpContext.Session.SetString(DefaultValues.SessionUserKeyName, currentUserAccount.Id.ToString());
                //    result.Message = "Login Succesful.";
                //    result.IsValidated = true;
                //    result.ReturnURL = loginView.ReturnURL;
                //    result.IsFirstTimeLogin = currentUserAccount.IsFirstTimeLogin;
                //}

                HttpContext.Session.SetString(DefaultValues.SessionUserKeyName, currentUserAccount.Id.ToString());
                result.Message = "Login Succesful.";
                result.IsValidated = true;
                result.ReturnURL = loginView.ReturnURL;

                return Json(result);
            }

            result.IsValidated = false;
            result.ReturnURL = null;
            result.Message = "Unknown Error.";
            return Json(result);
        }


    }
}
