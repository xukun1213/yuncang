
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

Huayu.Oms.Infrastructure.Dependencies.ConfigureServices(builder.Configuration, builder.Services);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "huayu oms HTTP API",
        Version = "v1",
        Description = "huayu oms service HTTP API"
    });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows()
        {
            Implicit = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri($"{configuration.GetValue<string>("IdentityUrl")}/connect/authorize"),
                TokenUrl = new Uri($"{configuration.GetValue<string>("IdentityUrl")}/connect/token"),
                Scopes = new Dictionary<string, string>()
                {
                    {"oms","oms API" }
                }
            }
        }
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.Authority = configuration.GetValue<string>("IdentityUrl");
        options.Audience = "wms";

        options.RequireHttpsMetadata = false; // disble https valid

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
            RequireExpirationTime = true
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


using (var scope = app.Services.CreateScope())
{
    var omsContext = scope.ServiceProvider.GetRequiredService<OmsContext>();
    if (omsContext.Database.GetPendingMigrations().Any())
    {
        Console.WriteLine("Applying migrations ({ApplicationContext})...", nameof(OmsContext));
        omsContext.Database.Migrate(); //Ö´ÐÐÇ¨ÒÆ

    }
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
