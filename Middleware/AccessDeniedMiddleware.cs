//using Microsoft.AspNetCore.Http;
//using System.Threading.Tasks;

//public class AccessDeniedMiddleware
//{
//    private readonly RequestDelegate _next;

//    public AccessDeniedMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        if (!context.User.Identity.IsAuthenticated)
//        {
//            // Người dùng chưa đăng nhập, chuyển hướng đến trang /login
//            context.Response.Redirect("/login");
//            return;
//        }

//        // Pass the request to the next middleware
//        await _next(context);
//    }
//}
