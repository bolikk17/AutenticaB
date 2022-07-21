//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using static Program;

//namespace TMan.Entities.User.Queries
//{
//    public class LoginHandler : IRequestHandler<LoginHandler.LoginQuery, string>
//    {
//        public class LoginQuery : IRequest<string>
//        {
//            public string? UserName { get; set; }
//            public string? Password { get; set; }
//        }

//        private readonly UserManager<User> _userManager;
//        private readonly SignInManager<User> _signInManager;
//        //private readonly DataContext _dataContext;

//        public LoginHandler(
//            UserManager<User> userManager,
//            SignInManager<User> signInManager,
//            DataContext dataContext
//            )
//        {
//            _userManager = userManager;
//            //_dataContext = dataContext;
//            _signInManager = signInManager;
//        }

//        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
//        {
//            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);
//            if (result.Succeeded)
//            {
//                var user = await _userManager.FindByNameAsync(request.UserName);
//                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };
//                // authoption is iet in main() 
//                var jwt = new JwtSecurityToken(
//                        issuer: AuthOptions.ISSUER,
//                        audience: AuthOptions.AUDIENCE,
//                        claims: claims,
//                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
//                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

//                //LocalRedirect("/Authentication/TestRedirection");
//                return new JwtSecurityTokenHandler().WriteToken(jwt);

//                //Console.WriteLine("User logged in.");
//                // TODO Here user should be redirected to somewhere
//                //return LocalRedirect("/Authentication/TestRedirection");
//            }
//            return "Login Error";
//            //if (result.RequiresTwoFactor)
//            //{
//            //    return RedirectToPage("./LoginWith2fa", new
//            //    {
//            //        ReturnUrl = returnUrl,
//            //        RememberMe = Input.RememberMe
//            //    });
//            //}
//            //if (result.IsLockedOut)
//            //{
//            //    Console.WriteLine("User account locked out.");
//            //    return RedirectToPage("./Lockout");
//            //}
//            //else
//            //{
//            //    Console.WriteLine("CWL 4");
//            //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
//            //    return Page();
//            //}

//            //return LocalRedirect("/amonges"); ;
//        }
//    }
//}
