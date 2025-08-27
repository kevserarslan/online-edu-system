using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineEduSystem.Application.Interfaces;
using OnlineEduSystem.Application.Services;
using OnlineEduSystem.Domain.Interfaces;
using OnlineEduSystem.Infrastructure.Data;       // AppDbContext burada tanýmlý
using OnlineEduSystem.Infrastructure.Repositories;
using System.Text;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);
// CORS
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("Client", policy =>
        policy
            .WithOrigins(
                "http://localhost:5173",   // Vite dev server
                "http://127.0.0.1:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
    // .AllowCredentials()  // Cookie tabanlý auth kullanýrsan aç; Bearer JWT'de gerek yok.
    );
});


// 1) DbContext + SQL Server konfigürasyonu
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
//AddScoped, AddTransient AddSingleton
// 2) User için Repository & Service kayýtlarý
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
// 2) Course için Repository & Service kayýtlarý
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddScoped<IAuthService, AuthService>();
// 3) CourseMaterial için Repository & Service kayýtlarý
builder.Services.AddScoped<ICourseMaterialRepository, CourseMaterialRepository>();
builder.Services.AddScoped<ICourseMaterialService, CourseMaterialService>();
//Enrollment
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();


// JWT Authentication
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer            = builder.Configuration["Jwt:Issuer"],
        ValidAudience          = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey       = new SymmetricSecurityKey(key),
        ValidateIssuerSigningKey = true,
        ValidateIssuer         = true,
        ValidateAudience       = true,
        ValidateLifetime       = true
    };
});

// 4) Diðer servisler
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        // enum'larý "Pdf/Video/Document" gibi yazýyla döndürmek için (opsiyonel ama güzel)
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// ??? BU SATIRI EKLE
builder.Services.AddSingleton<OnlineEduSystem.Infrastructure.Services.FileService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// **Seed: Uygulama baþlarken DB’yi tohumla**
//using (var scope = app.Services.CreateScope())
//{
  //  var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //AppDbInitializer.Seed(ctx);
//}

// 5) HTTP isteki pipeline ayarlarý
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); 
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("Client");
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.Run();
