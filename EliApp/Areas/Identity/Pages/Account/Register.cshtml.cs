// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using EliApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace EliApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<EliAppUser> _signInManager;
        private readonly UserManager<EliAppUser> _userManager;
        private readonly IUserStore<EliAppUser> _userStore;
        private readonly IUserEmailStore<EliAppUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager; //added rolemanager - Eli
        private IWebHostEnvironment _environment; // added
        public RegisterModel(
        UserManager<EliAppUser> userManager,
        IUserStore<EliAppUser> userStore,
        SignInManager<EliAppUser> signInManager,
        ILogger<RegisterModel> logger,
        RoleManager<IdentityRole> roleManager, //rolemanager - Eli
        IWebHostEnvironment environment, // added
        IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager; //role manager - Eli
            _environment = environment; // added
            _emailSender = emailSender;
        }

        [BindProperty]
        public IFormFile Upload { get; set; } // added

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
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// 

        public class InputModel
        {
            //Added first name and last name, etc - Eli
            [Required]
            [Display(Name = "First Name")]
            [DataType(DataType.Text)]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            [DataType(DataType.Text)]
            public string LastName { get; set; }

            [Display(Name = "Address")]
            [DataType(DataType.Text)]
            public string Address { get; set; }

            [Display(Name = "Date of Birth")]
            [DataType(DataType.Date)]
            public DateTime DOB { get; set; }

            [Column(TypeName = "Profile Picture")]
            public string ProfilePicture { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            //For Admin's List of old passwords - Rasul
            /*commented out temporarily
            public List<string> oldPassword { get; set; }
            */
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

         /*       MailMessage MyMailMessage = new MailMessage();
                MyMailMessage.Subject = "Email testing";
                MyMailMessage.From = new MailAddress("credpandat5@yahoo.com", "CredPanda");
                MyMailMessage.To.Add(new MailAddress("credpandat5@yahoo.com", "CredPanda"));

                SmtpClient mySmtpClient = new SmtpClient("smtp.mail.yahoo.com", 465);
                mySmtpClient.EnableSsl = true;
                mySmtpClient.Send(MyMailMessage);   */
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["roles"] = _roleManager.Roles.ToList(); // add roles to list - Eli
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.FirstName = Input.FirstName; //added custom inputs - Eli
                user.LastName = Input.LastName;
                user.Address = Input.Address;
                user.DOB = Input.DOB;
                user.RegisterDate = DateTime.Now;
                user.passwordChangedDate = DateTime.Now;
                user.expireDate = user.passwordChangedDate.AddDays(365);

                //Create path name for profile picture
                var file = Path.Combine(_environment.WebRootPath, "uploads", Upload.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                }
                user.ProfilePicture = Upload.FileName;

                user.Email = Input.Email;
                user.isActive = false; //added user's activation status, default is false - Rasul

                #region Generate Username
                //Take the first letter from the first name
                string sb = new string(user.FirstName.ToLower().Substring(0, 1));
                //Take the whole of the last name
                sb += (user.LastName.ToLower());
                //Add in the date as needed
                //Very long and complex, and it's very likely that this can be simplified

                string[] date = user.RegisterDate.ToShortDateString().Split('/');
                //Get the month portion
                if (date[0].Length < 2)
                {
                    string temp = "0";
                    temp += date[0];
                    sb += temp;
                }
                else
                {
                    sb += date[0];
                }
                //Get the last two digits of the year
                sb += date[2].Substring(2, 2);

                //Set the username to the created string and return it
                user.GeneratedUserName = sb.ToString();
                #endregion


                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    SendEmail("Registration Confirmation");
                    _logger.LogInformation("User created a new account with password.");
                    //user.oldPasswords.Add(Input.Password);                   

                    var defaultrole = _roleManager.FindByNameAsync("User").Result; //added default role - Eli

                    if (defaultrole != null) //sets role - Eli
                    {
                        IdentityResult roleresult = await _userManager.AddToRoleAsync(user, defaultrole.Name);
                    }

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

 //                   SendEmail("Please confirm your account by clicking here</a>.");

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


      /*              #region Confirm Email
                    public static void SendEmail(string emailBody)
                    {
                        MailAddress to = new MailAddress("anisleyvera13@gmail.com");
                        MailAddress from = new MailAddress("credpandat5@gmail.com");
                        MailMessage mailMessage = new MailMessage(from, to);
                        mailMessage.Subject = "Login Confirmation";
                        mailMessage.Body = emailBody;
                        mailMessage.BodyEncoding = Encoding.UTF8;
                        mailMessage.IsBodyHtml = true;

                        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new System.Net.NetworkCredential("credpandat5", "Te$tMail5");
                        smtpClient.EnableSsl = true;

                        try
                        {
                            smtpClient.Send(mailMessage);
                        }
                        catch (SmtpException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }

                    }
                    #endregion
      */

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewData["roles"] = _roleManager.Roles.ToList(); // add roles to list - Eli 
            // If we got this far, something failed, redisplay form
            return Page();
        }

        private EliAppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<EliAppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(EliAppUser)}'. " +
                    $"Ensure that '{nameof(EliAppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<EliAppUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<EliAppUser>)_userStore;
        }
    }
}

