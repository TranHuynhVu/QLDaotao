using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QLDaoTao.Models;

public class StudentSignInManager : SignInManager<AppStudent>
{
    public StudentSignInManager(UserManager<AppStudent> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<AppStudent> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<AppStudent>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<AppStudent> confirmation)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {}
    //Đây là class để bơm các dịch vụ vào của Identity vào sau đó sử dụng ở các controller
    //Vì model AppStudent kế thừa Identity 
}
