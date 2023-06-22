using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Tonrich.Server.Api;
using YamlDotNet.Core.Tokens;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        // https://github.com/dotnet/aspnetcore/issues/4660
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

        var appsettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var settings = appsettings.JwtSettings;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var certificatePath = Path.Combine(Directory.GetCurrentDirectory(), "IdentityCertificate.pfx");
            RSA? rsaPrivateKey;
            using (X509Certificate2 signingCert = new X509Certificate2(certificatePath, appsettings.JwtSettings.IdentityCertificatePassword, X509KeyStorageFlags.EphemeralKeySet))
            {
                rsaPrivateKey = signingCert.GetRSAPrivateKey();
            }
            var validationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                RequireSignedTokens = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsaPrivateKey),

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ValidateAudience = true,
                ValidAudience = settings.Audience,

                ValidateIssuer = true,
                ValidIssuer = settings.Issuer,
            };

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = async context =>
                {
                    // The server accepts the access_token from either the authorization header, the cookie, or the request URL query string

                    var access_token = context.Request.Cookies["access_token"];

                    if (string.IsNullOrEmpty(access_token))
                    {
                        access_token = context.Request.Query["access_token"];
                    }

                    context.Token = access_token;
                }
            };
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

            options.SaveToken = true;
            options.TokenValidationParameters = validationParameters;
        });

        services.AddAuthorization();
    }

    public static void AddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<ODataOperationFilter>();

            options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearerAuth"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }

    public static void AddHealthChecks(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
    {
        var appsettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var healthCheckSettings = appsettings.HealthCheckSettings;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        if (healthCheckSettings.EnableHealthChecks is false)
            return;

        services.AddHealthChecksUI(setupSettings: setup =>
        {
            setup.AddHealthCheckEndpoint("TonrichHealthChecks", env.IsDevelopment() ? "https://localhost:5001/healthz" : "/healthz");
        }).AddInMemoryStorage();

#pragma warning disable CS8604 // Possible null reference argument.
        var healthChecksBuilder = services.AddHealthChecks()
            .AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 6 * 1024)
            .AddDiskStorageHealthCheck(opt =>
                opt.AddDrive(Path.GetPathRoot(Directory.GetCurrentDirectory()), minimumFreeMegabytes: 5 * 1024))
            //.AddDbContextCheck<AppDbContext>()
            ;
#pragma warning restore CS8604 // Possible null reference argument.

        //var emailSettings = appsettings.EmailSettings;

        //if (emailSettings.UseLocalFolderForEmails is false)
        //{
        //    healthChecksBuilder
        //        .AddSmtpHealthCheck(options =>
        //        {
        //            options.Host = emailSettings.Host;
        //            options.Port = emailSettings.Port;

        //            if (emailSettings.HasCredential)
        //            {
        //                options.LoginWith(emailSettings.UserName, emailSettings.Password);
        //            }
        //        });
        //}
    }
}
