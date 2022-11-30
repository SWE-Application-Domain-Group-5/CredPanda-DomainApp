// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EliApp.Areas.Identity.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Principal;
using EliApp.Areas.Identity.Data;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Text;

namespace EliApp.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<EliAppUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<EliAppUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

  //      public DateTime expireDate = EliAppUser.expireDate;
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public static void SendEmail(string emailBody)
        {
            try
            {
                   MailAddress to = new MailAddress("credpandat5@yahoo.com");
                   MailAddress from = new MailAddress("credpandat5@yahoo.com");
                   MailMessage mailMessage = new MailMessage(from, to);
                   mailMessage.Subject = "Login Confirmation";
                   mailMessage.Body = emailBody;
                   mailMessage.BodyEncoding = Encoding.UTF8;
                   mailMessage.IsBodyHtml = true;

                   SmtpClient smtpClient = new SmtpClient("smtp.mail.yahoo.com", 465);
                   smtpClient.UseDefaultCredentials = false;
                   smtpClient.Credentials = new System.Net.NetworkCredential("credpandat5@yahoo.com", "Te$tMail5");
                   smtpClient.EnableSsl = true;


                       smtpClient.Send(mailMessage); 

           /*     MailMessage MyMailMessage = new MailMessage();
                MyMailMessage.Subject = "Email testing";
                MyMailMessage.From = new MailAddress("credpandat5@yahoo.com", "CredPanda");
                MyMailMessage.To.Add(new MailAddress("credpandat5@yahoo.com", "CredPanda"));

                SmtpClient mySmtpClient = new SmtpClient("smtp.mail.yahoo.com", 465);
                mySmtpClient.EnableSsl = true;
                mySmtpClient.Send(MyMailMessage);   */
            }
            catch(SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
  
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    //                  if (expireDate < DateTime.Now.AddDays(3))
                    //                  {

                    //return LocalRedirect("~/Identity/Account/Manage/ChangePassword");
                    //                 }
                    //                 else {
                    SendEmail("Login Successful");
                    return LocalRedirect(returnUrl);
                    //                }
               
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }


            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
