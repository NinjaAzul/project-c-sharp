using System.Globalization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Project_C_Sharp.Modules.DependencyInjection;
using Project_C_Sharp.Shared.Configs.Validations.AssemblyMaker;
using Project_C_Sharp.Shared.Filters;
using Project_C_Sharp.Shared.Swagger;
using Microsoft.OpenApi.Models;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("pt-BR"),
        new CultureInfo("en-US")
    };

    options.DefaultRequestCulture = new RequestCulture("pt-BR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new CustomRequestCultureProvider(async context =>
    {
        var lang = context.Request.Query["lang"].ToString();
        if (string.IsNullOrEmpty(lang))
            return await Task.FromResult(new ProviderCultureResult("en-US"));

        return await Task.FromResult(new ProviderCultureResult(lang));
    }));
});

builder.Services
    .AddModules(builder.Configuration);


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<AssemblyMarker>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Project C Sharp API",
        Version = "v1"
    });

    c.DocumentFilter<LowercaseDocumentFilter>();
    c.OperationFilter<AddLanguageHeaderParameter>();

    // Configuração do JWT Bearer no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            Array.Empty<string>()
        }
    });

    // Adiciona os comentários XML
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Configura para usar os nomes dos controllers sem o sufixo "Controller"
    c.CustomSchemaIds(type => type.FullName);
});

var app = builder.Build();

app.UseRequestLocalization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
//Example Middleware
// app.UseMiddleware<AuthGuard>();
app.MapControllers();

app.Run();