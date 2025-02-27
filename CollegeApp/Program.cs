using CollegeApp.Configurations;
using CollegeApp.Data;
using CollegeApp.Data.Common;
using CollegeApp.Logger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
//using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


//using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection.Metadata;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the Loggers
//builder.Logging.ClearProviders();
//builder.Logging.AddDebug();
//builder.Logging.AddConsole();
// Add services to the container.


// Add Thrid-Party Serilog
Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
    .WriteTo.File("File/File.txt", rollingInterval: RollingInterval.Minute)
    .CreateLogger();
//builder.Services.AddSerilog(); // Add serilog builder
builder.Logging.AddSerilog(); // Add serilog and in-built loggers builder

// Sql Server Options
builder.Services.AddDbContext<CollegeDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeAppDB")));

// Add builder for XML format response return
//builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
builder.Services.AddControllers().AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

//builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true).AddNewtonsoftJson();

builder.Services.AddScoped<IMyLogger, LogToDB>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header the bearer scheme. Enter Bearer [space] add your token in the text input. Example: Bearer saksalksii3j3kej",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Id="Bearer",
                    Type=ReferenceType.SecurityScheme
                },
                Scheme="oauth2",
                Name="Bearer",
                In=ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

//Add StudetnRepo
//builder.Services.AddTransient<IStudentRepository, StudentRepository>();
builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));

// Add AutoMapper Config
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Services.AddCors(options =>
{
    // policy 1
    options.AddDefaultPolicy(policy =>
    {
        // Allow Origins
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        // Access-Control-Allow-Origin: "*"       
    });
    // policy 2
    options.AddPolicy("MyTestCORS", policy =>
    {
        policy.WithOrigins("https://localhost:1234").AllowAnyHeader().AllowAnyMethod();
        // Access-Control-Allow-Origin: "https://localhost:1234"
    });
});
//var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecret"));
var googleKey = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretForGoogle"));
var microsoftKey = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretForMicroSoft"));
var localuserKey = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretForLocalUsers"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("LoginForGoogleUser", options =>
{
    //options.RequireHttpsMetadata = false; // production never make false
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new
        SymmetricSecurityKey(googleKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
})
.AddJwtBearer("LoginForMicroSoftUser", options =>
 {
     //options.RequireHttpsMetadata = false; // production never make false
     options.SaveToken = true;
     options.TokenValidationParameters = new TokenValidationParameters()
     {
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new
         SymmetricSecurityKey(microsoftKey),
         ValidateIssuer = false,
         ValidateAudience = false
     };
 })
.AddJwtBearer("LoginForLocalUsers", options =>
 {
     //options.RequireHttpsMetadata = false; // production never make false
     options.SaveToken = true;
     options.TokenValidationParameters = new TokenValidationParameters()
     {
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new
         SymmetricSecurityKey(localuserKey),
         ValidateIssuer = false,
         ValidateAudience = false
     };
 });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
//app.UseCors("MyTestCORS");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGet("api.testendpoint",
    context => context.Response.WriteAsync(builder.Configuration.GetValue<string>("LoginForLocalUsers")));
});


app.MapControllers();

app.Run();
