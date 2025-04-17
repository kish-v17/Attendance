using Attendance.Data;
using Attendance.Models;
using Attendance.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Attendance.Services;

namespace Attendance.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDBContext _db;
        private readonly EmailService _emailService;
        public AuthController(AppDBContext db, EmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { area = User.FindFirstValue(ClaimTypes.Role)
            });
            }

            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _db.UserTbl.SingleOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString())
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties).Wait();

                    Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                    Response.Headers["Pragma"] = "no-cache";
                    Response.Headers["Expires"] = "0";

                    TempData["ToastType"] = "success";
                    TempData["ToastMessage"] = "Login successful!";

                    switch (user.Role.ToString())
                    {
                        case "Admin":
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        case "Faculty":
                            return RedirectToAction("Index", "Home", new { area = "Faculty" });
                        default:
                            TempData["ToastType"] = "error";
                            TempData["ToastMessage"] = "Unauthorized role.";
                            return RedirectToAction("Login", "Auth");
                    }
                }

                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Invalid email or password.";
            }
            else
            {
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Please fill in all required fields correctly.";
            }

            return View(model);
        }

        

        public async Task<IActionResult> AccessDenied()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _db.UserTbl.SingleOrDefault(u => u.Email == model.Email);
                if (user != null)
                {
                    string otp = new Random().Next(100000, 999999).ToString();
                    TempData["OTP"] = otp;
                    TempData["Email"] = model.Email;
                    HttpContext.Session.SetString("UserEmail", model.Email);

                    string body = $@"
                        <html>
                            <head>
                                <style>
                                    body {{
                                        font-family: Arial, sans-serif;
                                        font-size: 14px;
                                        color: #333;
                                    }}
                                    .otp-container {{
                                        background-color: #f4f4f4;
                                        padding: 20px;
                                        border-radius: 8px;
                                        text-align: center;
                                    }}
                                    .otp-code {{
                                        font-size: 24px;
                                        font-weight: bold;
                                        color: #007BFF;
                                    }}
                                    .footer {{
                                        font-size: 12px;
                                        color: #777;
                                    }}
                                </style>
                            </head>
                            <body>
                                <div class='otp-container'>
                                    <h2>Password Reset Request</h2>
                                    <p>Dear User,</p>
                                    <p>We received a request to reset the password associated with your account.</p>
                                    <p>If you made this request, please use the following One-Time Password (OTP) to proceed:</p>
                                    <p class='otp-code'>{otp}</p>
                                    <p>Note: The OTP is valid for the next 15 minutes. Please use it before it expires.</p>
                                    <p>If you did not request this change, please ignore this message.</p>
                                </div>
                            </body>
                        </html>";

                    try
                    {
                        _emailService.SendEmail(model.Email, "OTP for Password Reset", body);

                        TempData["ToastMessage"] = "OTP has been sent to your email.";
                        TempData["ToastType"] = "success";
                        return RedirectToAction("VerifyOtp");
                    }
                    catch (Exception ex)
                    {
                        TempData["ToastMessage"] = "Error sending OTP. Please try again.";
                        TempData["ToastType"] = "error";
                        return View(model);
                    }
                }

                TempData["ToastMessage"] = "No account found with this email.";
                TempData["ToastType"] = "error";
                return View(model);
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                TempData["ToastMessage"] = error.ErrorMessage;
                TempData["ToastType"] = "error";
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult VerifyOtp()
        {
            string timestampStr = HttpContext.Session.GetString("OTPTimestamp");
            int countdown = 90;

            if (!string.IsNullOrEmpty(timestampStr))
            {
                DateTime sentTime = DateTime.Parse(timestampStr);
                int remaining = 90 - (int)(DateTime.UtcNow - sentTime).TotalSeconds;
                countdown = remaining > 0 ? remaining : 0;
            }

            ViewBag.RemainingTime = countdown;
            return View();
        }
        [HttpPost]
        public IActionResult VerifyOtp(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string email = HttpContext.Session.GetString("UserEmail");
            string storedOtp = TempData["OTP"]?.ToString();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(storedOtp))
            {
                TempData["ToastMessage"] = "OTP expired or session ended. Please try again.";
                TempData["ToastType"] = "error";
                return RedirectToAction("ForgotPassword");
            }

            if (model.OTP != storedOtp)
            {
                TempData["ToastMessage"] = "Invalid OTP. Please try again.";
                TempData["ToastType"] = "error";
                TempData["OTP"] = storedOtp;
                return View(model);
            }

            TempData["ToastMessage"] = "OTP verified. You may now reset your password.";
            TempData["ToastType"] = "success";

            return RedirectToAction("ResetPassword");
        }

        [HttpPost]
        public IActionResult ResendOtp(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["ToastMessage"] = "Email is required to resend OTP.";
                TempData["ToastType"] = "error";
                return RedirectToAction("ForgotPassword");
            }

            int resendAttempts = HttpContext.Session.GetInt32("OtpResendCount") ?? 0;

            if (resendAttempts >= 2)
            {
                TempData["ToastMessage"] = "You have exceeded the maximum number of resend attempts.";
                TempData["ToastType"] = "error";
                return RedirectToAction("VerifyOtp");
            }

            string lastSentStr = HttpContext.Session.GetString("OTPTimestamp");
            if (!string.IsNullOrEmpty(lastSentStr))
            {
                DateTime lastSent = DateTime.Parse(lastSentStr);
                if ((DateTime.UtcNow - lastSent).TotalSeconds < 90)
                {
                    TempData["ToastMessage"] = "Please wait before resending the OTP.";
                    TempData["ToastType"] = "warning";
                    return RedirectToAction("VerifyOtp");
                }
            }

            Random rnd = new Random();
            int otp = rnd.Next(100000, 999999);

            TempData["OTP"] = otp;
            TempData["Email"] = email;

            HttpContext.Session.SetString("OTPTimestamp", DateTime.UtcNow.ToString());
            HttpContext.Session.SetInt32("OtpResendCount", resendAttempts + 1);

            string body = $@"
    <html>
        <head>
            <style>
                .otp-code {{
                    font-size: 24px;
                    font-weight: bold;
                    color: #007BFF;
                }}
            </style>
        </head>
        <body>
            <h3>Resending OTP for Password Reset</h3>
            <p>Your new OTP is:</p>
            <p class='otp-code'>{otp}</p>
            <p>This OTP will expire in 15 minutes. Please use it quickly.</p>
        </body>
    </html>";

            try
            {
                _emailService.SendEmail(email, "Resend OTP - Attendance System", body);

                TempData["ToastMessage"] = "A new OTP has been sent to your email.";
                TempData["ToastType"] = "success";
                return RedirectToAction("VerifyOtp");
            }
            catch (Exception)
            {
                TempData["ToastMessage"] = "Failed to send OTP. Please try again.";
                TempData["ToastType"] = "error";
                return RedirectToAction("ForgotPassword");
            }
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                TempData["ToastMessage"] = "Session expired. Please start again.";
                TempData["ToastType"] = "error";
                return RedirectToAction("ForgotPassword");
            }

            var user = _db.UserTbl.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                TempData["ToastMessage"] = "User not found.";
                TempData["ToastType"] = "error";
                return RedirectToAction("ForgotPassword");
            }

            user.Password = model.NewPassword;

            _db.UserTbl.Update(user);
            _db.SaveChanges();


            TempData["ToastMessage"] = "Password reset successfully. You can now login.";
            TempData["ToastType"] = "success";
            return RedirectToAction("Login");
        }



        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Cookies");
            return RedirectToAction("Login", "Auth");
        }

    }
}
