using ChatBot.AppCode;
using Microsoft.AspNetCore.Antiforgery;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddScoped<SessionAdmin>();
//builder.Services.AddScoped<RedirectIfLoggedInAttribute>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = ".AspNetCore.Antiforgery";
});


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    // See https://aka.ms/aspnetcore-hsts
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.Use(async (context, next) =>
{
    // If it's a POST request, check CSRF token
    if (context.Request.Method == "POST")
    {
        var origin = context.Request.Headers["Host"].FirstOrDefault();
        if (!string.IsNullOrEmpty(origin) && origin != "localhost:7121")
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Unauthorized.");
            return;
        }

        var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();
        var tokenFromHeader = context.Request.Headers["X-XSRF-TOKEN"].FirstOrDefault();

        // CSRF validation using the token in the header
        if (!string.IsNullOrWhiteSpace(tokenFromHeader))
        {
            try
            {
                await antiforgery.ValidateRequestAsync(context);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid CSRF token.");
                return;
            }
        }
        else
        {
            var tokenFromForm = "";
            try
            {
                tokenFromForm = context.Request.Form["__RequestVerificationToken"].FirstOrDefault();
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Unauthorized.");
                return;
            }

            if (string.IsNullOrWhiteSpace(tokenFromForm))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid or missing CSRF token.");
                return;
            }

            try
            {
                await antiforgery.ValidateRequestAsync(context);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid CSRF token.");
                return;
            }
        }
    }

    await next.Invoke();
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
