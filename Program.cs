using Infra.Data;
using Infra.Documents;
using Application;
using Microsoft.AspNetCore.Authorization;
using Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Hangfire;
using Application.Services;



var builder = WebApplication.CreateBuilder(args);

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = Settings._signingKey,
    ValidateIssuer = false,
    ValidateAudience = false
};

// Add services to the container.
builder.Services.AddTradeContextModule(builder.Configuration);
builder.Services.AddApplicationService();
builder.Services.AddDocumentsService(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
            .AddJwtBearer(options => { options.TokenValidationParameters = tokenValidationParameters; });

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy(Settings._securityDefinition, new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
        .RequireAuthenticatedUser().Build());
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(Settings._swaggerVersion, new OpenApiInfo { Title = Settings._apiName, Version = Settings._swaggerVersion });

    c.AddSecurityDefinition(Settings._securityDefinition,
    new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = $"Please enter into field the word '{Settings._securityDefinition}' following by space and JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                 {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = Settings._securitySchemeId,
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHangfireDashboard();

// Configure os trabalhos Hangfire aqui
RecurringJob.AddOrUpdate<FileGenerationService>("GerarArquivoDiariamente", service => service.GenerateFile(), Cron.Daily);

app.UseHttpsRedirection();

app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
                );

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
