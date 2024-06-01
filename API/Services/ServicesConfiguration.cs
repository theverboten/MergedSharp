using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
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

            return services;
        }
    }
}