using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
/*using API.Helpers;*/
using dotenv.net;
using API.Services;
using Microsoft.EntityFrameworkCore.Design;
using API.Data;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);
/*
builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy1",
        policy =>
        {
            policy.WithOrigins("http://localhost:5059",
                                "http://localhost:4200");
        });

    options.AddPolicy("AnotherPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});*/



// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(
    opt =>
    {
        opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
);


//builder.Services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

builder.Services.AddCors();


var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));




if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404 || !Path.HasExtension(context.Request.Path.Value))
    {
        context.Request.Path = "/index.html";
        await next();
    }
});*/


app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();


app.UseAuthorization();
/*
app.UseDefaultFiles();
app.UseStaticFiles();
*/
app.MapControllers();
app.MapFallbackToController("Index", "Fallback");

app.Run();
