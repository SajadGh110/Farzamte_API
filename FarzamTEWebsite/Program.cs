using FarzamTEWebsite.Data;
using FarzamTEWebsite.Data.Initialization;
using FarzamTEWebsite.Filters;
using FarzamTEWebsite.Services;
using FarzamTEWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var api_key = builder.Configuration.GetConnectionString("api-key");

builder.Services.AddControllers(option =>
{
    option.Filters.Add(new ValidateKey(api_key));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.AddSwaggerGen();

// Add Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => (c.Type == ClaimTypes.Role && (c.Value == "Owner" || c.Value == "Admin")))));

    options.AddPolicy("OwnerPolicy", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Owner"));
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Tokens:Key"])),
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

builder.Services.AddDbContext<FarzamDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IInComingCall_StatService, InComingCall_StatService>();
builder.Services.AddScoped<IBrokerageReports, BrokerageReportsService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{

}

await AdminSeeder.SeedAdminAsync(app.Services);
app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();