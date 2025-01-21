using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            /* services.AddDbContext<DataContext>(opt =>
             {
                 opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
             });*/

            services.AddScoped<GoogleService>();
            services.AddScoped<IStringConvertService, StringConvertService>();
            services.AddScoped<IPdfStringService, PdfStringService>();
            services.AddScoped<IPdfSpeechService, PdfSpeechService>();
            services.AddHttpClient("databaseapi", client => { client.BaseAddress = new Uri("http://localhost:5059/"); });
            services.AddScoped<ITestingService, TestingService>();
            services.AddHttpClient<Client>();
            /*   services.Configure<CloudinaryService>(config.GetSection("CloudinarySettings"));*/
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IDownloadService, DownloadService>();
            /*  services.AddAuthentication(o =>
          {
              // This forces challenge results to be handled by Google OpenID Handler, so there's no
              // need to add an AccountController that emits challenges for Login.
              o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
              // This forces forbid results to be handled by Google OpenID Handler, which checks if
              // extra scopes are required and does automatic incremental auth.
              o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
              // Default scheme that will handle everything else.
              // Once a user is authenticated, the OAuth2 token info is stored in cookies.
              o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
          })
          .AddCookie()
          .AddGoogleOpenIdConnect(options =>
          {
              options.ClientId = {YOUR_CLIENT_ID};
              options.ClientSecret = {YOUR_CLIENT_SECRET};
          });*/

            return services;
        }
    }
}