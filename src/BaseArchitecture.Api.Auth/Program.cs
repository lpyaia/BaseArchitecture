using System.Security.Claims;
using System.Text;
using BasicArchitecture.Api.Auth;
using BasicArchitecture.Api.Auth.Data;
using BasicArchitecture.Api.Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder
    .Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 5;

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.MaxFailedAccessAttempts = 5;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
});

builder.Services.AddScoped<TokenService>();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Authentication Service API", Version = "v1" });

    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization heahder using the Bearer scheme. Example \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
        }
    );

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                Array.Empty<string>()
            },
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Auth API");
    });

    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
    dbContext.Database.Migrate();

    if (!roleManager.Roles.Any())
    {
        foreach (var role in new[] { "Admin", "User" })
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    if (await userManager.FindByEmailAsync("admin@example.com") == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            FirstName = "Admin",
            LastName = "Super",
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(adminUser, "Admin123!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    if (await userManager.FindByEmailAsync("jorge.ben@example.com") == null)
    {
        var jorgeBen = new ApplicationUser
        {
            UserName = "jorge.ben@example.com",
            Email = "jorge.ben@example.com",
            FirstName = "Jorge",
            LastName = "Ben",
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(jorgeBen, "Jorge123!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(jorgeBen, "User");
            await userManager.AddClaimAsync(jorgeBen, new Claim("department", "Sales"));
        }
    }

    if (await userManager.FindByEmailAsync("raul.seixas@example.com") == null)
    {
        var raulSeixas = new ApplicationUser
        {
            UserName = "raul.seixas@example.com",
            Email = "raul.seixas@example.com",
            FirstName = "Raul",
            LastName = "Seixas",
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(raulSeixas, "Raul123!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(raulSeixas, "User");
            await userManager.AddClaimAsync(raulSeixas, new Claim("department", "IT"));
        }
    }

    if (await userManager.FindByEmailAsync("tim.maia@example.com") == null)
    {
        var timMaia = new ApplicationUser
        {
            UserName = "tim.maia@example.com",
            Email = "tim.maia@example.com",
            FirstName = "Tim",
            LastName = "Maia",
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(timMaia, "Tim123!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(timMaia, "User");
            await userManager.AddClaimAsync(timMaia, new Claim("department", "IT"));
            await userManager.AddClaimAsync(timMaia, new Claim("department", "Sales"));
        }
    }
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
