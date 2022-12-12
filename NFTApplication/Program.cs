using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using NFTDatabaseService;
using NFTWalletService;
using TesoraExchange;

using NFTApplication.Infrastructure.Swagger;
using NFTApplication.Services;
using NFTApplication.Background;
using NFTApplication.Utility;

var builder = WebApplication.CreateBuilder(args);

string env = builder.Configuration["Environment:Prefix"];

// Add services to the container.
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<IpfsSettings>(builder.Configuration.GetSection("IpfsSettings"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var authBaseUrl = builder.Configuration[$"Authorization:{env}"];

var configManager = new ConfigurationManager<OpenIdConnectConfiguration>($"{authBaseUrl}/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever());

var openidconfig = configManager.GetConfigurationAsync().Result;

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("Bearer", options =>
{
    options.Authority = authBaseUrl;
    options.RequireHttpsMetadata = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = builder.Configuration[$"Authorization:Audience{env}"],

        ValidateIssuer = true,
        ValidIssuers = new[] { authBaseUrl },

        ValidateIssuerSigningKey = true,
        IssuerSigningKeys = openidconfig.SigningKeys,

        RequireExpirationTime = true,
        ValidateLifetime = true,
        RequireSignedTokens = true
    };
});


builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); ;

builder.Services.AddMemoryCache();

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service  
    // note: the specified format code will format the version as "'v'major[.minor][-status]"  
    options.GroupNameFormat = "'v'VVV";

    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat  
    // can also be used to control the format of the API version in route templates  
    options.SubstituteApiVersionInUrl = true;
});



///////////////////////////////////////////////////////////////////////////////////////////////////////////
// Add the NFTDatabase singleton
var dataAddress = builder.Configuration[$"NFTDatabaseService:{env}BaseUrl"];
builder.Services.AddNFTDatabaseService(dataAddress);

///////////////////////////////////////////////////////////////////////////////////////////////////////////
// Add the NFTWallet singleton
var walletAddress = builder.Configuration[$"NFTWalletService:{env}BaseUrl"];
builder.Services.AddNFTWalletService(walletAddress);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
builder.Services.AddSingleton<ICurrencyUtility, CurrencyUtility>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(c =>
{
    // add JWT Authentication
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // must be lower case
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

    c.OperationFilter<SwaggerDefaultValues>();

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


builder.Services.AddHostedService<AuctionProcessingBackgroundService>();

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
//}

app.UseCors("MyCorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

