/*
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System;
using System.Net;
using YHRSys.Models;
*/
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using YHRSys.Models;
using System.Net.Mail;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Net;

namespace YHRSys.Controllers
{
    //[Authorize]
    //public class AccountController : Controller
    //{
    //    //private ApplicationSignInManager _signInManager;
    //    //private ApplicationUserManager _userManager;

    //    public AccountController()
    //    {
    //    }

    //    public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
    //    {
    //        UserManager = userManager;
    //        SignInManager = signInManager;
    //    }

    //    public ApplicationSignInManager SignInManager
    //    {
    //        get
    //        {
    //            return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
    //        }
    //        private set
    //        {
    //            _signInManager = value;
    //        }
    //    }

    //    public ApplicationUserManager UserManager
    //    {
    //        get
    //        {
    //            return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
    //        }
    //        private set
    //        {
    //            _userManager = value;
    //        }
    //    }

    //    //
    //    // GET: /Account/Login
    //    [AllowAnonymous]
    //    public ActionResult Login(string returnUrl)
    //    {
    //        ViewBag.ReturnUrl = returnUrl;
    //        return View();
    //    }

    //    //
    //    // POST: /Account/Login
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return View(model);
    //        }

    //        // This doesn't count login failures towards account lockout
    //        // To enable password failures to trigger account lockout, change to shouldLockout: true
    //        var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
    //        switch (result)
    //        {
    //            case SignInStatus.Success:
    //                return RedirectToLocal(returnUrl);
    //            case SignInStatus.LockedOut:
    //                return View("Lockout");
    //            case SignInStatus.RequiresVerification:
    //                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
    //            case SignInStatus.Failure:
    //            default:
    //                ModelState.AddModelError("", "Invalid login attempt.");
    //                return View(model);
    //        }
    //    }

    //    //
    //    // GET: /Account/VerifyCode
    //    [AllowAnonymous]
    //    public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
    //    {
    //        // Require that the user has already logged in via username/password or external login
    //        if (!await SignInManager.HasBeenVerifiedAsync())
    //        {
    //            return View("Error");
    //        }
    //        return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
    //    }

    //    //
    //    // POST: /Account/VerifyCode
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return View(model);
    //        }

    //        // The following code protects for brute force attacks against the two factor codes. 
    //        // If a user enters incorrect codes for a specified amount of time then the user account 
    //        // will be locked out for a specified amount of time. 
    //        // You can configure the account lockout settings in IdentityConfig
    //        var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
    //        switch (result)
    //        {
    //            case SignInStatus.Success:
    //                return RedirectToLocal(model.ReturnUrl);
    //            case SignInStatus.LockedOut:
    //                return View("Lockout");
    //            case SignInStatus.Failure:
    //            default:
    //                ModelState.AddModelError("", "Invalid code.");
    //                return View(model);
    //        }
    //    }

    //    //
    //    // GET: /Account/Register
    //    [AllowAnonymous]
    //    public ActionResult Register()
    //    {
    //        return View();
    //    }

    //    //
    //    // POST: /Account/Register
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> Register(RegisterViewModel model)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
    //            var result = await UserManager.CreateAsync(user, model.Password);
    //            if (result.Succeeded)
    //            {
    //                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

    //                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
    //                // Send an email with this link
    //                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
    //                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
    //                // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

    //                return RedirectToAction("Index", "Home");
    //            }
    //            AddErrors(result);
    //        }

    //        // If we got this far, something failed, redisplay form
    //        return View(model);
    //    }

    //    //
    //    // GET: /Account/ConfirmEmail
    //    [AllowAnonymous]
    //    public async Task<ActionResult> ConfirmEmail(string userId, string code)
    //    {
    //        if (userId == null || code == null)
    //        {
    //            return View("Error");
    //        }
    //        var result = await UserManager.ConfirmEmailAsync(userId, code);
    //        return View(result.Succeeded ? "ConfirmEmail" : "Error");
    //    }

    //    //
    //    // GET: /Account/ForgotPassword
    //    [AllowAnonymous]
    //    public ActionResult ForgotPassword()
    //    {
    //        return View();
    //    }

    //    //
    //    // POST: /Account/ForgotPassword
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            var user = await UserManager.FindByNameAsync(model.Email);
    //            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
    //            {
    //                // Don't reveal that the user does not exist or is not confirmed
    //                return View("ForgotPasswordConfirmation");
    //            }

    //            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
    //            // Send an email with this link
    //            // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
    //            // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
    //            // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
    //            // return RedirectToAction("ForgotPasswordConfirmation", "Account");
    //        }

    //        // If we got this far, something failed, redisplay form
    //        return View(model);
    //    }

    //    //
    //    // GET: /Account/ForgotPasswordConfirmation
    //    [AllowAnonymous]
    //    public ActionResult ForgotPasswordConfirmation()
    //    {
    //        return View();
    //    }

    //    //
    //    // GET: /Account/ResetPassword
    //    [AllowAnonymous]
    //    public ActionResult ResetPassword(string code)
    //    {
    //        return code == null ? View("Error") : View();
    //    }

    //    //
    //    // POST: /Account/ResetPassword
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return View(model);
    //        }
    //        var user = await UserManager.FindByNameAsync(model.Email);
    //        if (user == null)
    //        {
    //            // Don't reveal that the user does not exist
    //            return RedirectToAction("ResetPasswordConfirmation", "Account");
    //        }
    //        var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
    //        if (result.Succeeded)
    //        {
    //            return RedirectToAction("ResetPasswordConfirmation", "Account");
    //        }
    //        AddErrors(result);
    //        return View();
    //    }

    //    //
    //    // GET: /Account/ResetPasswordConfirmation
    //    [AllowAnonymous]
    //    public ActionResult ResetPasswordConfirmation()
    //    {
    //        return View();
    //    }

    //    //
    //    // POST: /Account/ExternalLogin
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult ExternalLogin(string provider, string returnUrl)
    //    {
    //        // Request a redirect to the external login provider
    //        return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
    //    }

    //    //
    //    // GET: /Account/SendCode
    //    [AllowAnonymous]
    //    public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
    //    {
    //        var userId = await SignInManager.GetVerifiedUserIdAsync();
    //        if (userId == null)
    //        {
    //            return View("Error");
    //        }
    //        var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
    //        var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
    //        return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
    //    }

    //    //
    //    // POST: /Account/SendCode
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> SendCode(SendCodeViewModel model)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return View();
    //        }

    //        // Generate the token and send it
    //        if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
    //        {
    //            return View("Error");
    //        }
    //        return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
    //    }

    //    //
    //    // GET: /Account/ExternalLoginCallback
    //    [AllowAnonymous]
    //    public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
    //    {
    //        var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
    //        if (loginInfo == null)
    //        {
    //            return RedirectToAction("Login");
    //        }

    //        // Sign in the user with this external login provider if the user already has a login
    //        var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
    //        switch (result)
    //        {
    //            case SignInStatus.Success:
    //                return RedirectToLocal(returnUrl);
    //            case SignInStatus.LockedOut:
    //                return View("Lockout");
    //            case SignInStatus.RequiresVerification:
    //                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
    //            case SignInStatus.Failure:
    //            default:
    //                // If the user does not have an account, then prompt the user to create an account
    //                ViewBag.ReturnUrl = returnUrl;
    //                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
    //                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
    //        }
    //    }

    //    //
    //    // POST: /Account/ExternalLoginConfirmation
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
    //    {
    //        if (User.Identity.IsAuthenticated)
    //        {
    //            return RedirectToAction("Index", "Manage");
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            // Get the information about the user from the external login provider
    //            var info = await AuthenticationManager.GetExternalLoginInfoAsync();
    //            if (info == null)
    //            {
    //                return View("ExternalLoginFailure");
    //            }
    //            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
    //            var result = await UserManager.CreateAsync(user);
    //            if (result.Succeeded)
    //            {
    //                result = await UserManager.AddLoginAsync(user.Id, info.Login);
    //                if (result.Succeeded)
    //                {
    //                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
    //                    return RedirectToLocal(returnUrl);
    //                }
    //            }
    //            AddErrors(result);
    //        }

    //        ViewBag.ReturnUrl = returnUrl;
    //        return View(model);
    //    }

    //    //
    //    // POST: /Account/LogOff
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult LogOff()
    //    {
    //        AuthenticationManager.SignOut();
    //        return RedirectToAction("Index", "Home");
    //    }

    //    //
    //    // GET: /Account/ExternalLoginFailure
    //    [AllowAnonymous]
    //    public ActionResult ExternalLoginFailure()
    //    {
    //        return View();
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            if (_userManager != null)
    //            {
    //                _userManager.Dispose();
    //                _userManager = null;
    //            }

    //            if (_signInManager != null)
    //            {
    //                _signInManager.Dispose();
    //                _signInManager = null;
    //            }
    //        }

    //        base.Dispose(disposing);
    //    }

    //    #region Helpers
    //    // Used for XSRF protection when adding external logins
    //    private const string XsrfKey = "XsrfId";

    //    private IAuthenticationManager AuthenticationManager
    //    {
    //        get
    //        {
    //            return HttpContext.GetOwinContext().Authentication;
    //        }
    //    }

    //    private void AddErrors(IdentityResult result)
    //    {
    //        foreach (var error in result.Errors)
    //        {
    //            ModelState.AddModelError("", error);
    //        }
    //    }

    //    private ActionResult RedirectToLocal(string returnUrl)
    //    {
    //        if (Url.IsLocalUrl(returnUrl))
    //        {
    //            return Redirect(returnUrl);
    //        }
    //        return RedirectToAction("Index", "Home");
    //    }

    //    internal class ChallengeResult : HttpUnauthorizedResult
    //    {
    //        public ChallengeResult(string provider, string redirectUri)
    //            : this(provider, redirectUri, null)
    //        {
    //        }

    //        public ChallengeResult(string provider, string redirectUri, string userId)
    //        {
    //            LoginProvider = provider;
    //            RedirectUri = redirectUri;
    //            UserId = userId;
    //        }

    //        public string LoginProvider { get; set; }
    //        public string RedirectUri { get; set; }
    //        public string UserId { get; set; }

    //        public override void ExecuteResult(ControllerContext context)
    //        {
    //            var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
    //            if (UserId != null)
    //            {
    //                properties.Dictionary[XsrfKey] = UserId;
    //            }
    //            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
    //        }
    //    }
    //    #endregion
    //}

    
    [Authorize]
    public class AccountController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();

        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }


        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }


        public UserManager<ApplicationUser> UserManager { get; private set; }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                
                ModelState.AddModelError("", "Invalid username or password.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //[Authorize(Roles = "Admin, CanEditUser, Guest")]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = "Admin, CanEditUser, Guest")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = model.GetUser();
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    MigrateShoppingCart(model.UserName);
                    return RedirectToAction("Index", "Account");
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [Authorize(Roles = "Admin, CanEditUser, User")]
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditUser, User")]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            if (disposing && _db != null)
            {
                _db.Dispose();
                _db = null;
            }
            base.Dispose(disposing);
        }


        [Authorize(Roles = "Admin, CanEditGroup, CanEditUser")]
        public ActionResult Index()
        {
            var users = _db.Users;
            var model = new List<EditUserViewModel>();
            foreach (var user in users)
            {
                var u = new EditUserViewModel(user);
                model.Add(u);
            }
            return View(model);
        }


        [Authorize(Roles = "Admin, CanEditUser")]
        public ActionResult Edit(string id, ManageMessageId? Message = null)
        {
            var user = _db.Users.First(u => u.UserName == id);
            var model = new EditUserViewModel(user);
            ViewBag.partnerId = new SelectList(_db.Partners, "partnerId", "name", model.partnerId);
            ViewBag.MessageId = Message;
            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Admin, CanEditUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _db.Users.First(u => u.UserName == model.UserName);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.partnerId = model.partnerId;
                _db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [Authorize(Roles = "Admin, CanEditUser")]
        public ActionResult Delete(string id = null)
        {
            var user = _db.Users.First(u => u.UserName == id);
            var model = new EditUserViewModel(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CanEditUser")]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = _db.Users.First(u => u.UserName == id);
            _db.Users.Remove(user);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin, CanEditUser")]
        public ActionResult UserGroups(string id)
        {
            var user = _db.Users.First(u => u.UserName == id);
            var model = new SelectUserGroupsViewModel(user);
            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Admin, CanEditUser")]
        [ValidateAntiForgeryToken]
        public ActionResult UserGroups(SelectUserGroupsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var idManager = new IdentityManager();
                var user = _db.Users.First(u => u.UserName == model.UserName);
                idManager.ClearUserGroups(user.Id);
                foreach (var group in model.Groups)
                {
                    if (group.Selected)
                    {
                        idManager.AddUserToGroup(user.Id, group.GroupId);
                    }
                }
                return RedirectToAction("index");
            }
            return View();
        }


        [Authorize(Roles = "Admin, CanEditRole, CanEditGroup, User")]
        public ActionResult UserPermissions(string id)
        {
            var user = _db.Users.First(u => u.UserName == id);
            var model = new UserPermissionsViewModel(user);
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // GET: Account/LostPassword
        [AllowAnonymous]
        public ActionResult LostPassword()
        {
            return View();
        }

        // POST: Account/LostPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LostPassword(LostPasswordModel model)
        {
           if (ModelState.IsValid)
           {
              ApplicationUser user;
              using (var context = new ApplicationDbContext())
              {
                 var foundUserName = (from u in context.Users
                                      where u.Email == model.Email
                                      select u.UserName).FirstOrDefault();
                 if (foundUserName != null)
                 {
                     user = (ApplicationUser)UserManager.FindByName(foundUserName.ToString());
                 }
                 else
                 {
                    user = null;
                 }
              }
              if (user != null)
              {
                 // Generae password token that will be used in the email link to authenticate user
                  var token = GenerateCustomPasswordResetToken.GenerateRandomToken(user.UserName);//WebSecurity.;
                 // Generate the html link sent via email
                 string resetLink = "<a href='"
                    + Url.Action("ResetPassword", "Account", new { rt = token }, "http") 
                    + "'>Reset Password Link</a>";
     
                 // Email stuff
                 string subject = "Reset your password for " + user.UserName + "(" + user.Email + ")";
                 string body = "Your password reset link: " + resetLink + ". <br /><br />Kindly click on it to proceed with resetting your password.<br /><br />Thank you,<br /><br />YHRSys Webmaster";
                 
                  
                  //string from = "k.oraegbunam@cgiar.org";
                  //Mail Server configurations from config file
                 System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/");//HttpContext.Request.ApplicationPath
                 System.Net.Configuration.MailSettingsSectionGroup mailSettings = (System.Net.Configuration.MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

                  if (mailSettings != null)
                  {
                      int port = mailSettings.Smtp.Network.Port;
                      string host = mailSettings.Smtp.Network.Host;
                      string password = mailSettings.Smtp.Network.Password;
                      string username = mailSettings.Smtp.Network.UserName;
                      string from = mailSettings.Smtp.From;

                      using (var message = new MailMessage(from, user.Email))
                      {
                          try
                          {
                              message.Subject = subject;
                              message.Body = body;
                              message.IsBodyHtml = true;
                              using (SmtpClient client = new SmtpClient
                              {
                                  EnableSsl = false,
                                  Host = host,
                                  Port = port,
                                  Credentials = new NetworkCredential(username, password)
                              })
                              {
                                  client.Send(message);
                                  ViewData["Message"] = "Success<br/>" + body;
                              }
                          }
                          catch (Exception e)
                          {
                              ModelState.AddModelError("", "Issue sending email: " + e.Message);
                          }
                      }
                  }
              }         
              else // Email not found
              {
                 // Note: You may not want to provide the following information
                 //* since it gives an intruder information as to whether a
                 //* certain email address is registered with this website or not.
                 //* If you're really concerned about privacy, you may want to
                 //* forward to the same "Success" page regardless whether an
                 //* user was found or not. This is only for illustration purposes.
                 //
                 ModelState.AddModelError("", "No user found by that email.");
              }
           }
     
           // You may want to send the user to a "Success" page upon the successful
           //* sending of the reset email link. Right now, if we are 100% successful
           //* nothing happens on the page. :P
           
           return View(model);
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string rt)
        {
            ResetPasswordModel model = new ResetPasswordModel();
            model.ReturnToken = rt;
            return View(model);
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user  = GenerateCustomPasswordResetToken.ResetOldPassword(model.ReturnToken);

                if (user != null)
                {
                    if (!user.TokenExpired)
                    {
                        try{
                            string pass = UserManager.PasswordHasher.HashPassword(model.Password);
                            //IdentityResult result = UserManager.AddPassword(user.Id, model.Password);
                            var curUser = _db.Users.First(u => u.UserName == user.UserName);
                            curUser.PasswordHash = pass;
                            _db.Entry(curUser).State = System.Data.Entity.EntityState.Modified;
                            _db.SaveChanges();
                        
                            ViewBag.Message = "Password Successfully Changed";
                        }
                        catch(Exception ex)
                        {
                            ViewBag.Message = "Something went wrong making password reset to fail! Please try again or contact the Webmaster. " + ex.Message.ToString();
                        }
                    }
                    else {
                        ViewBag.Message = "Your token has expired! Go through forgot password process once more or contact the Webmaster.";
                    }
                }
                else {
                    ViewBag.Message = "Could not find user with supplied password reset credentials! Please try again or contact the Webmaster.";
                }
            }
            return View(model);
        }

        //Migration of shopping cart
        private void MigrateShoppingCart(string UserName)
        {
            // Associate shopping cart items with logged-in user
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.MigrateCart(UserName);
            Session[ShoppingCart.CartSessionKey] = UserName;
        }

        #region Helpers
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }


        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            return RedirectToAction("Index", "Home");
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}