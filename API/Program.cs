using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
/*using API.Helpers;*/
using dotenv.net;
using API.Services;
using Microsoft.EntityFrameworkCore.Design;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth;
using Google.Apis.Services;
using Newtonsoft.Json;
using Google.Apis;
using Google.Cloud.TextToSpeech;

using iText.Layout.Element;
using Google.Api;
using Google.Cloud.TextToSpeech.V1;
using Google.Apis.Util.Store;
using Google.Apis.Auth.AspNetCore3;
using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Protobuf.Reflection;
using Newtonsoft.Json.Linq;
using System.Text;
using static System.Text.Json.Utf8JsonWriter;
using Google.Apis.Json;
using SQLitePCL;
using Google.Cloud.BigQuery.V2;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.CodeDom;
using Microsoft.OpenApi.Writers;
using Universal.Google.Authentication.OAuth2;








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
/* builder.Services.AddDbContext<DataContext>(
    opt =>
    {
        opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
);*/


//builder.Services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

builder.Services.AddCors();

var connString = "";

if (builder.Environment.IsDevelopment())
    connString = builder.Configuration.GetConnectionString("DefaultConnection");
else
{
    // Use connection string provided at runtime by FlyIO.
    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    Console.WriteLine(connUrl);
    // Parse connection URL to connection string for Npgsql
    connUrl = connUrl.Replace("postgres://", string.Empty);
    var pgUserPass = connUrl.Split("@")[0];
    var pgHostPortDb = connUrl.Split("@")[1];
    var pgHostPort = pgHostPortDb.Split("/")[0];
    var pgDb = pgHostPortDb.Split("/")[1];
    var pgUser = pgUserPass.Split(":")[0];
    var pgPass = pgUserPass.Split(":")[1];
    var pgHost = pgHostPort.Split(":")[0];
    var pgPort = pgHostPort.Split(":")[1];
    var updatedHost = pgHost;
    /*.Replace("flycast", "internal");*/

    connString = $"Server={updatedHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";

}
/*connString = builder.Configuration.GetConnectionString("DefaultConnection");*/

builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseNpgsql(connString);
});
/*
var _type = Environment.GetEnvironmentVariable("TYPE");
var _project_id = Environment.GetEnvironmentVariable("PROJECT_ID");
var _private_key_id = Environment.GetEnvironmentVariable("PRIVATE_KEY_ID");
var _private_key = Environment.GetEnvironmentVariable("PRIVATE_KEY");
var _client_email = Environment.GetEnvironmentVariable("CLIENT_EMAIL");
var _client_id = Environment.GetEnvironmentVariable("CLIENT_ID");
var _auth_uri = Environment.GetEnvironmentVariable("AUTH_URI");
var _token_uri = Environment.GetEnvironmentVariable("TOKEN_URI");
var _auth_provider_x509_cert_url = Environment.GetEnvironmentVariable("AUTH_PROVIDER");
var _client_x509_cert_url = Environment.GetEnvironmentVariable("CLIENT_X");
var _universe_domain = Environment.GetEnvironmentVariable("UNIVERSE_DOMAIN");

//**** ****
var _keyEnvVar = Environment.GetEnvironmentVariable("CREDS");
Console.WriteLine("EnvVar :" + _keyEnvVar);
*/
//var client = Google.Apis.Auth.OAuth2.ServiceAccountCredential.FromServiceAccountData(); možná bude usefull later




/*
var _GOOGLE_APPLICATION_CREDENTIALS = new CredentialsModel
{
    type = $"{_type}",
    project_id = $"{_project_id}",
    private_key_id = $"{_private_key_id}",
    private_key =
      "-----BEGIN PRIVATE KEY-----\n" +
      $"{_private_key}"
       + "-----END PRIVATE KEY-----\n",
    client_email = $"{_client_email}",
    client_id = $"{_client_id}",
    auth_uri = $"{_auth_uri}",
    token_uri = $"{_token_uri}",
    auth_provider_x509_cert_url = $"{_auth_provider_x509_cert_url}",
    client_x509_cert_url = $"{_client_x509_cert_url}",
    universe_domain = $"{_universe_domain}"


};*/
/*
ServiceAccountCredential GetCredential()
{
    return new ServiceAccountCredential(
     new ServiceAccountCredential.Initializer(Environment.GetEnvironmentVariable("CLIENT_EMAIL"))
     {
         Scopes = new[]
   {
      "https://texttospeech.googleapis.com",
      "https://www.googleapis.com/auth/cloud-platform"
   },
         User = "medikacerychlaakce@gmail.com",
     }.FromPrivateKey(Environment.GetEnvironmentVariable("PRIVATE_KEY"))
    );
}*/
/*
JsonCredentialParameters credentialParameters = new JsonCredentialParameters
{


};
credentialParameters.Type = _GOOGLE_APPLICATION_CREDENTIALS.type;
credentialParameters.ProjectId = _GOOGLE_APPLICATION_CREDENTIALS.project_id;
credentialParameters.PrivateKeyId = _GOOGLE_APPLICATION_CREDENTIALS.private_key_id;
credentialParameters.PrivateKey = _GOOGLE_APPLICATION_CREDENTIALS.private_key;
credentialParameters.ClientEmail = _GOOGLE_APPLICATION_CREDENTIALS.client_email;
credentialParameters.ClientId = _GOOGLE_APPLICATION_CREDENTIALS.client_id;
credentialParameters.UniverseDomain = _GOOGLE_APPLICATION_CREDENTIALS.universe_domain;*/
/*
Console.WriteLine(_GOOGLE_APPLICATION_CREDENTIALS.type);
Console.WriteLine(credentialParameters.Type);

JsonCredentialParameters GOOGLE_APPLICATION_CREDENTIALS = credentialParameters;
Console.WriteLine(GOOGLE_APPLICATION_CREDENTIALS.Type);
*/
try
{   /*
    // Get active credential
    string credPath = //_exePath + 
    @"\Private-67917519b23f.json";

    var json = File.ReadAllText(credPath);
    var cr = JsonConvert.DeserializeObject<PersonalServiceAccountCred>(json); // "personal" service account credential

    // Create an explicit ServiceAccountCredential credential
    var xCred = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(cr.ClientEmail)
    {
        Scopes = new[] {
            TextToSpeechBase.AnalyticsManageUsersReadonly,
            AnalyticsService.Scope.AnalyticsReadonly
        }
    }.FromPrivateKey(cr.PrivateKey));

    // Create the service
    AnalyticsService service = new AnalyticsService(
        new BaseClientService.Initializer()
        {
            HttpClientInitializer = xCred,
        }
    );

    TextToSpeechClientBuilder client = new TextToSpeechClientBuilder().Credential(new .Initializer()
{
    HttpClientInitializer = credential
}

   

    


*/

    // Console.WriteLine(_GOOGLE_APPLICATION_CREDENTIALS);
    // string GOOGLE_APPLICATION_CREDENTIALS = JsonConvert.SerializeObject(_GOOGLE_APPLICATION_CREDENTIALS);
    // Console.WriteLine(GOOGLE_APPLICATION_CREDENTIALS);
    // TextToSpeechClientBuilder
    /*
    var credential = GoogleCredential.FromJson()
       .CreateScoped(
           //"https://www.googleapis.com/auth/drive", 
           //"https://www.googleapis.com/auth/spreadsheets",
           "https://texttospeech.googleapis.com")
       // This is the email of the user in the domain where
       // the service account has domain wide delegation,
       // that you want to manipulate Drive files/Sheets for.
       .CreateWithUser(_GOOGLE_APPLICATION_CREDENTIALS.client_email);
     */
    //  TextToSpeechClient client = 
    /*
      TextToSpeechClientBuilder client = new TextToSpeechClientBuilder(credential)
{
   HttpClientInitializer = credential
}

);*/ /*static ServiceAccountCredential CreateServiceAccountCredentialFromJson(
            JsonCredentialParameters credentialParameters)
        {
            if (credentialParameters.Type != JsonCredentialParameters.ServiceAccountCredentialType ||
                string.IsNullOrEmpty(credentialParameters.ClientEmail) ||
                string.IsNullOrEmpty(credentialParameters.PrivateKey))
            {
                throw new InvalidOperationException("JSON data does not represent a valid service account credential.");
            }
            var initializer = new ServiceAccountCredential.Initializer(credentialParameters.ClientEmail);
            return new ServiceAccountCredential(initializer.FromPrivateKey(credentialParameters.PrivateKey));
        }
     
      GoogleCredential.GetApplicationDefaultAsync(
                        CreateServiceAccountCredentialFromJson(credentialParameters));*/

    // var credential = GoogleCredential.FromJsonParameters(credentialParameters);

    //var clearCredentialParameters = JsonConvert.DeserializeObject(credentialParameters);
    /*  ServiceAccountCredential serviceAccountCredential= new ServiceAccountCredential.Initializer(_client_email){
          Scopes = new[]
  {
      "https://texttospeech.googleapis.com",
      "https://www.googleapis.com/auth/cloud-platform"
  },
          User ="medikacerychlaakce@gmail.com",
      }.FromPrivateKey(_private_key);

  */
}
catch
{
    // Console.WriteLine("Something went wrong!!");
}
;



/*

List<CredentialsModel> _data = new List<CredentialsModel>();
_data.Add(new CredentialsModel()
{
    type = $"{_type}",
    project_id = $"{_project_id}",
    private_key_id = $"{_private_key_id}",
    private_key =
    "-----BEGIN PRIVATE KEY-----\n" +
    $"{_private_key}"
     + "-----END PRIVATE KEY-----\n",
    client_email = $"{_client_email}",
    client_id = $"{_client_id}",
    auth_uri = $"{_auth_uri}",
    token_uri = $"{_token_uri}",
    auth_provider_x509_cert_url = $"{_auth_provider_x509_cert_url}",
    client_x509_cert_url = $"{_client_x509_cert_url}",
    universe_domain = $"{_universe_domain}"
});
CredentialsModel credentials = new CredentialsModel();
credentials.type = $"{_type}";
credentials.project_id = $"{_project_id}";
credentials.private_key_id = $"{_private_key_id}";
credentials.private_key =
"-----BEGIN PRIVATE KEY-----\n" +
$"{_private_key}"
 + "-----END PRIVATE KEY-----\n";
credentials.client_email = $"{_client_email}";
credentials.client_id = $"{_client_id}";
credentials.auth_uri = $"{_auth_uri}";
credentials.token_uri = $"{_token_uri}";
credentials.auth_provider_x509_cert_url = $"{_auth_provider_x509_cert_url}";
credentials.client_x509_cert_url = $"{_client_x509_cert_url}";
credentials.universe_domain = $"{_universe_domain}";

*/


//var keysEnvVar = System.Environment.GetEnvironmentVariable("CREDS");

//var clientSecrets = await GoogleClientSecrets.FromFileAsync();




/*
string jsonConvert = JsonConvert.SerializeObject(_GOOGLE_APPLICATION_CREDENTIALS);
//var txt = new TextBox() { Name = "Hello" };

Console.WriteLine("Json is :" + jsonConvert);

//string jsonString = System.Text.Json.JsonSerializer.Serialize(_GOOGLE_APPLICATION_CREDENTIALS);
var credentials2 = GoogleCredential.FromJson(jsonConvert);
// var client = BigQueryClient.Create(_project_id, credentials2);

BigQueryClient client = BigQueryClient.Create(_project_id, credentials2);
*/
//string filePath = @"wwwroot/files/credentials.json";
/*
using (StreamWriter file = File.CreateText(filePath))
{

    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
    //serialize object directly into file stream
    serializer.Serialize(file, credentials);
    Console.WriteLine("Obsah JSON:");
    Console.WriteLine($"{credentials.type}");
    Console.WriteLine(File.ReadAllText(filePath));
    
    //**** ****
    /*
    var keysEnvVar = System.Environment.GetEnvironmentVariable("CREDS");

    string jsonString = System.Text.Json.JsonSerializer.Serialize(keysEnvVar);
    var credentials2 = GoogleCredential.FromJson(jsonString);
    // var client = BigQueryClient.Create(_project_id, credentials2);

    BigQueryClient client = BigQueryClient.Create(_project_id, credentials2);*/








/*  byte[] bytes;*/
/* using (var memoryStream = new MemoryStream())
 {
     Stream.CopyTo(memoryStream);
     bytes = memoryStream.ToArray();
 }*/
/* string jsonText = File.ReadAllText(filePath);
 byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(jsonText);
 bytes = byteArray.ToArray();

 string base64 = Convert.ToBase64String(bytes);*/
//  WriteBase64String();
/*   new MemoryStream(System.Text.Encoding.UTF8.GetBytes(base64));*/
/*}*/

/* **** ****
string json = System.Text.Json.JsonSerializer.Serialize(_data);

File.WriteAllText(@"wwwroot/files/credentials.json", json, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);*/
/*wait using FileStream createStream = File.Create(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

using (var stream = File.Create(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
{
}*/

/*
using (FileStream st = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
{
    File.Create(file);
    await System.Text.Json.JsonSerializer.SerializeAsync(st, _data);
}*/
//await System.Text.Json.JsonSerializer.SerializeAsync(st, _data);



//Console.WriteLine(File.ReadAllText(@"wwwroot/files/credentials.json"));
//Console.WriteLine(_GOOGLE_APPLICATION_CREDENTIALS.private_key);

//string GOOGLE_APPLICATION_CREDENTIALS = System.Text.Json.JsonSerializer.Serialize(_GOOGLE_APPLICATION_CREDENTIALS);
/*string __GOOGLE_APPLICATION_CREDENTIALS = System.Text.Json.JsonSerializer.Serialize(_GOOGLE_APPLICATION_CREDENTIALS);

byte[] bytes = System.Text.Encoding.UTF8.GetBytes(__GOOGLE_APPLICATION_CREDENTIALS);

string GOOGLE_APPLICATION_CREDENTIALS = System.Convert.ToBase64String(bytes);
//var perfectCredentials = "GOOGLE_APPLICATION_CREDENTIALS='" + CREDS + "'";


GoogleCredential credential = GoogleCredential.FromJson(json);
//credential = credential.CreateScoped(new List<string>() { "https://www.googleapis.com/auth/cloud-platform" });

//string textToSpeechScope = "https://www.googleapis.com/auth/cloud-platform";

//public static TextToSpeechService AuthenticateServiceAccount(string ServiceAccountEmail){}
//*//*
var credential = GoogleCredential.FromFile(@"wwwroot/files/credentials.json")
    .CreateScoped(
        //"https://www.googleapis.com/auth/drive", 
        //"https://www.googleapis.com/auth/spreadsheets",
        "https://www.googleapis.com/auth/cloud-platform")
     // This is the email of the user in the domain where
     // the service account has domain wide delegation,
     // that you want to manipulate Drive files/Sheets for.
    .CreateWithUser(credentials.client_email);

var texttospeech = new TextToSpeech(new BaseClientService.Initializer
{
    HttpClientInitializer = credential
});*/
/*

string credential_path = @"wwwroot/files/credentials.json";
System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);
 var client = TextToSpeechClient.Create();
        // Load the image file into memory
        var image = Google.Cloud.Vision.V1.Image.FromFile(@"C:\Users\Maicon\OneDrive\Área de Trabalho\keyboardSantander\keyboard.png");
        // Performs label detection on the image file
        var response = client.DetectLabels(image);
        foreach (var annotation in response)
        {
            if (annotation.Description != null)
                debugOutput(annotation.Description);
        }




/*
using var stream = new FileStream(@"wwwroot/files/credentials.json", FileMode.Open, FileAccess.Read);
var credential = ServiceAccountCredential.FromServiceAccountData(stream);
credential.Scopes = new[]
{
    DriveService.Scope.Drive
};*/


//GoogleCredential credential = GoogleCredential.GetApplicationDefault();
//credential = credential.CreateScoped(new List<string>() { "https://www.googleapis.com/auth/cloud-platform" });
/*
var stringConvertService = new TextToSpeechClient(new Google.Apis.Services.BaseClientService.Initializer()
{
    HttpClientInitializer = credential
});


//
/*
GoogleCredential credential = GoogleCredential.GetApplicationDefault();
credential = credential.CreateScoped(new List<string>() { "https://www.googleapis.com/auth/cloud-platform" });*/
/*TextToSpeechSettings settings = TextToSpeechClientBuilder(credential);*/


//var client = TextToSpeechClient.Create();

/*
var credential = JSON.parse(
   Buffer.from(process.env.GOOGLE_APPLICATION_CREDENTIALS, 'base64').toString()
 );

 var storage = new Microsoft.EntityFrameworkCore.Storage({
   projectId: process.env.GOOGLE_STORAGE_PROJECT_ID,
   credentials: credential
 });*/







//var credentialString = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");


//var client = TextToSpeechClientBuilder.Create(credential);

/*
TextToSpeechClientBuilder client = new TextToSpeechClientBuilder().Credential(new .Initializer()
{
    HttpClientInitializer = credential
}
);*/



var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:8080"));
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://ctecka.fly.dev"));




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

app.UseAuthentication();
app.UseAuthorization();
/*
app.UseDefaultFiles();
app.UseStaticFiles();
*/
app.MapControllers();
app.MapFallbackToController("Index", "Fallback");

app.Run();
