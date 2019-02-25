using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using E_Mechanik_Web.Models;
using System.Data.Entity;
using System.IO;
using System.Text.RegularExpressions;

namespace E_Mechanik_Web.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            var userId = User.Identity.GetUserId();
            if (this.User.IsInRole("Mechanic"))
            {
                MechanicProfiles x = _db.MechanicProfiles.Where(c => c.MechanicName == this.User.Identity.Name).FirstOrDefault();
                if (x == null)
                {
                    MechanicProfiles profile = new MechanicProfiles { MechanicName = this.User.Identity.Name };
                    return View("EditMechanicProfile", profile);
                }
                

                if (x.ImagePatch != null || x.ImagePatch != "")
                {
                    var model2 = new IndexViewModel
                    {
                        HasPassword = HasPassword(),
                        PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                        TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                        Logins = await UserManager.GetLoginsAsync(userId),
                        BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
                        File = x.ImagePatch
                    };
                    return View(model2);

                }
                
            }
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }
     

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Wygeneruj i wyślij token
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Twój kod zabezpieczający: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Wyślij wiadomość SMS za pośrednictwem dostawcy usług SMS w celu zweryfikowania numeru telefonu
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // Jeśli dotarliśmy tak daleko, oznacza to, że wystąpił błąd. Wyświetl ponownie formularz
            ModelState.AddModelError("", "Nie można zweryfikować numeru telefonu");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // Jeśli dotarliśmy tak daleko, oznacza to, że wystąpił błąd. Wyświetl ponownie formularz
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "Usunięto logowanie zewnętrzne."
                : message == ManageMessageId.Error ? "Wystąpił błąd."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Żądaj przekierowania do dostawcy logowania zewnętrznego w celu połączenia logowania bieżącego użytkownika
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        public ActionResult EditMechanicProfile()
        {

            MechanicProfiles profile = new MechanicProfiles
            {
                MechanicName = this.User.Identity.Name,
                CompanyName = _db.MechanicProfiles.Where(m => m.MechanicName == this.User.Identity.Name).Select(k => k.CompanyName).FirstOrDefault(),
                City = _db.MechanicProfiles.Where(m => m.MechanicName == this.User.Identity.Name).Select(k => k.City).FirstOrDefault(),
                Address = _db.MechanicProfiles.Where(m => m.MechanicName == this.User.Identity.Name).Select(k => k.Address).FirstOrDefault(),
                ImagePatch = _db.MechanicProfiles.Where(m => m.MechanicName == this.User.Identity.Name).Select(k => k.ImagePatch).FirstOrDefault(),
            };
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMechanicProfile([Bind(Include = "Id,CompanyName,Country,City,Address,PostalCode,MechanicName")] MechanicProfiles profile, HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid)
            {
                string _path = "";
                string _FileName = "";
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    if (!IsImage(postedFile))
                    {
                        ViewBag.image = "Niepawidłowy format zdjęcia";
                        return View(profile);

                    }
                    _FileName = User.Identity.Name + " - " + Path.GetFileName(postedFile.FileName);
                    _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    if (System.IO.File.Exists(_path) &&  _path != "~/UploadedFiles/no-image.png")
                    {
                        System.IO.File.Delete(_path);
                    }
                    postedFile.SaveAs(_path);
                }
                if (_db.MechanicProfiles.Where(c => c.MechanicName == this.User.Identity.Name).FirstOrDefault() != null)
                {
                    MechanicProfiles x = _db.MechanicProfiles.Where(c => c.MechanicName == this.User.Identity.Name).FirstOrDefault();
                    x.CompanyName = profile.CompanyName;
                    x.City = profile.City;
                    x.Address = profile.Address;
                    if (x.ImagePatch != null && _path != "~/UploadedFiles/no-image.png")
                    {
                        if (System.IO.File.Exists(x.ImagePatch))
                        {
                            System.IO.File.Delete(x.ImagePatch);
                        }
                        //prawdopodobnie będzie działać na serwerze -> x.ImagePatch = _path;
                        x.ImagePatch = "~/UploadedFiles/" + _FileName;
                    }
                    else
                    {
                        profile.ImagePatch = "~/UploadedFiles/no-image.png";
                    }
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var Name = this.HttpContext.User.Identity.Name;
                    profile.MechanicName = Name;
                    if (_path != "")
                    {
                        //podobnie jak wyżej - profile.ImagePatch = _path;
                        profile.ImagePatch = "~/UploadedFiles/" + _FileName;
                    }
                    else
                    {
                        profile.ImagePatch = "~/UploadedFiles/no-image.png";
                    }

                    _db.MechanicProfiles.Add(profile);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                // Edycja bazy danych bez użycia Entity Framework

            }

            return View(profile);
        }

        public ActionResult AvatarDelete()
        {
            MechanicProfiles x = _db.MechanicProfiles.Where(c => c.MechanicName == this.User.Identity.Name).FirstOrDefault();
            if (x.ImagePatch != "~/UploadedFiles/no-image.png" && x.ImagePatch !=null)
            {
                x.ImagePatch = Server.MapPath("~/UploadedFiles/") + x.ImagePatch.Substring(x.ImagePatch.LastIndexOf('/'));
                if (System.IO.File.Exists(x.ImagePatch))
                {
                    System.IO.File.Delete(x.ImagePatch);
                }
                x.ImagePatch = "~/UploadedFiles/no-image.png";
                _db.SaveChanges();
                ViewBag.result = "Pomyślnie usunięto miniature";
                return View("EditMechanicProfile");
            }
            else
            {
                ViewBag.result = "Nie można usunąć domyślnej miniatury";
                return View("EditMechanicProfile");
            }
        }

        public ActionResult EditClientProfile()
        {
            AddPhoneNumberViewModel num = new AddPhoneNumberViewModel
            {
                Number = _db.Users.Where(m => m.Email == this.User.Identity.Name).Select(k => k.PhoneNumber).FirstOrDefault()
            };


            return View(num);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditClientProfile([Bind(Include = "Number")] AddPhoneNumberViewModel phone)
        {
            if (ModelState.IsValid)
            {
                // ApplicationUser model = _db.Users.Where(c => c.Email == this.User.Identity.Name).FirstOrDefault();
                // model.PhoneNumber = phone.Number;
                //IdentityResult result = await UserManager.UpdateAsync(model);
                _db.Users.Where(z => z.Email == this.User.Identity.Name).FirstOrDefault().PhoneNumber = phone.Number;
                _db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        public const int ImageMinimumBytes = 512;
        public static bool IsImage(HttpPostedFileBase postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (!string.Equals(postedFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            var postedFileExtension = Path.GetExtension(postedFile.FileName);
            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.InputStream.CanRead)
                {
                    return false;
                }
                //------------------------------------------
                //   Check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (postedFile.ContentLength < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                postedFile.InputStream.Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                postedFile.InputStream.Position = 0;
            }

            return true;
        }

        #region Pomocnicy
        // Służy do ochrony XSRF podczas dodawania logowań zewnętrznych
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
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

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}