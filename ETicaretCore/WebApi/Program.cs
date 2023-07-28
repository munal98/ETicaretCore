using AppCore.Business.Models.JsonWebToken;
using AppCore.DataAccess.Configs;
using AppCore.MvcWebUI;
using Business.Services;
using Business.Services.Bases;
using DataAccess.EntityFramework.Contexts;
using DataAccess.EntityFramework.Repositories;
using DataAccess.EntityFramework.Repositories.Bases;
using Microsoft.EntityFrameworkCore;
using AppCore.Business.Utils.JsonWebToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AppCore.Business.Utils.JsonWebToken.Bases;
using AppCore.MvcWebUI.Bases;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
    });

var appSettingsUtil = new AppSettingsUtil(builder.Configuration);

//var jwtOptions = appSettingsUtil.Bind<JwtOptions>("JwtOptions");
var jwtOptions = appSettingsUtil.Bind<JwtOptions>(nameof(JwtOptions));

var jwtUtil = new JwtUtil(appSettingsUtil);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = jwtUtil.CreateSecurityKey(jwtOptions.SecurityKey)
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    //c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "WebApi",
        Description = "A Web API for E-Trade Core",
        //TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Çaðýl Alsaç",
            Email = string.Empty
            //Url = new Uri("https://www.cagilalsac.com")
        },
        License = new OpenApiLicense
        {
            Name = "Free License"
            //Url = new Uri("https://example.com/license")
        }
    });

    // Swagger üzerinden Authorization yapabilmek için eklendi.
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });

});

ConnectionConfig.ConnectionString = builder.Configuration.GetConnectionString("ETicaretContext");

#region IoC Container: Inversion of Control
builder.Services.AddScoped<DbContext, ETicaretContext>();
builder.Services.AddScoped<UrunRepositoryBase, UrunRepository>();
builder.Services.AddScoped<KategoriRepositoryBase, KategoriRepository>();
builder.Services.AddScoped<KullaniciRepositoryBase, KullaniciRepository>();
builder.Services.AddScoped<IUrunService, UrunService>();
builder.Services.AddScoped<IHesapService, HesapService>();
builder.Services.AddScoped<IKullaniciService, KullaniciService>();

builder.Services.AddSingleton<JwtUtilBase, JwtUtil>();
builder.Services.AddSingleton<AppSettingsUtilBase, AppSettingsUtil>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c => 
    { 
        c.SerializeAsV2 = true; 
    });
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
