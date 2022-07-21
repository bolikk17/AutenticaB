//using MediatR;
//using Microsoft.AspNetCore.Identity;

//namespace TMan.Entities.User.Commands
//{
//    public class RegistrationHandler : IRequestHandler<RegistrationHandler.RegistrationQuery, string>
//    {
//        public class RegistrationQuery : IRequest<string> 
//        {
//            public string? UserName { get; set; }
//            public string? Email { get; set; }
//            public string? Password { get; set; }
//        }

//        private readonly UserManager<User> _userManager;

//        public RegistrationHandler(
//            UserManager<User> userManager
//            )
//        {
//            _userManager = userManager;
//        }

//        public async Task<string> Handle(RegistrationQuery request, CancellationToken cancellationToken) 
//        {
//            var user = new User { UserName = request.UserName, Email = request.Email };
//            var result = await _userManager.CreateAsync(user, request.Password);
//            if (result.Succeeded)
//            {
//                return "User created a new account with password.";

//                // TODO Email confirmation
//                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//                //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
//                //var callbackUrl = Url.Page(
//                //    "/Account/ConfirmEmail",
//                //    pageHandler: null,
//                //    values: new { area = "Identity", userId = user.Id, code = code },
//                //    protocol: Request.Scheme);

//                //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
//                //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

//                if (_userManager.Options.SignIn.RequireConfirmedAccount)
//                {
//                    //return RedirectToPage("RegisterConfirmation",
//                    //                  new { email = Input.Email });

//                    Console.WriteLine("CWL 2");
//                }
//                else
//                {
//                    Console.WriteLine("CWL 3");
//                    //await _signInManager.SignInAsync(user, isPersistent: false);
//                    //return LocalRedirect(returnUrl);
//                }
//            }

//            return "Error happened"; ;
//        }
//    }
//}
