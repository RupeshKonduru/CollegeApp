using CollegeApp.Configurations;
using CollegeApp.Data;
using CollegeApp.Data.Common;
using CollegeApp.Logger;
using Microsoft.EntityFrameworkCore;
//using AutoMapper;

//using Microsoft.EntityFrameworkCore;
using Serilog;

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
builder.Services.AddSwaggerGen();

//Add StudetnRepo
//builder.Services.AddTransient<IStudentRepository, StudentRepository>();
builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));

// Add AutoMapper Config
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
