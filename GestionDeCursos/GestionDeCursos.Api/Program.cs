using GestionDeCursos.Api.Helpers;
using GestionDeCursos.Api.Services;
using GestionDeCursos.Api.Services.Management;
using GestionDeCursos.Api.TokenConfig;
using GestionDeCursos.Data.Database;
using GestionDeCursos.Data.Helpers;
using GestionDeCursos.Data.Models;
using GestionDeCursos.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using OfficeOpenXml;
using System.Data;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Database Connection
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

//Usar Dapper ORM
builder.Services.AddScoped<IDbConnection>
    (sp => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
// Add Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton(typeof(IPasswordHasher<AppUser>), typeof(PasswordHasher<AppUser>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomToken, CustomToken>();
builder.Services.AddScoped<IStudentService, StudentService>();

//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Manejar archivos excel EPPlus
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

//Agregar configuracion del JWT Bearer Token
var jwtSecretKey = Encoding.UTF8.GetBytes(builder.Configuration["Tokens:JwtSecretKey"]);

builder.Services.AddAuthentication(sharedOptions =>
{
    sharedOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(tokenOptions =>
{
    tokenOptions.RequireHttpsMetadata = false;
    tokenOptions.SaveToken = true;
    tokenOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["Tokens:Issuer"],
        ValidAudience = builder.Configuration["Tokens:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(jwtSecretKey)
    };
});

// Configuracion del idioma o lenguaje

builder.Services.AddLocalization(options => options.ResourcesPath = GlobalHelper.Language.ResourcesFolderName);

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedLanguages = new[]
    {
        new CultureInfo(GlobalHelper.Language.English),
        new CultureInfo(GlobalHelper.Language.Spanish),
    };

    options.DefaultRequestCulture = new RequestCulture(culture: GlobalHelper.Language.English, uiCulture: GlobalHelper.Language.English);
    options.SupportedCultures = supportedLanguages;
    options.SupportedUICultures = supportedLanguages;
    options.RequestCultureProviders.Insert(0, new CustomUrlCultureProvider(options.DefaultRequestCulture));
});

//Configuracion de User Authenticacion
builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

    config.Filters.Add(new AuthorizeFilter(policy));
})
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilitar  Authenticacion y Autorización
app.UseAuthentication();
app.UseAuthorization();

//Habilitar localización
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);

//Ruteo de la aplicación
app.MapControllerRoute(
        name: "defaultArea",
        pattern: "api/{culture}/{area:exists}/{controller}/{action}/{id?}"
    );

app.MapControllerRoute(
        name: "defaultCulture",
        pattern: "api/{culture}/{controller}/{action}/{id?}"
    );

app.MapControllerRoute(
        name: "default",
        pattern: "api/{controller}/{action}/{id?}"
    );

app.Run();
