using Microsoft.EntityFrameworkCore;
using PostgreProject.Context;
using PostgreProject.Hubs;
using PostgreProject.Mapping;
using PostgreProject.Services;
using PostgreProject.Services.AboutServices;
using PostgreProject.Services.ProductServices;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
// AutoMapper
builder.Services.AddAutoMapper(typeof(GeneralMapping));


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
		options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
	});


builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// SignalR
builder.Services.AddSignalR();

// HttpClient
builder.Services.AddHttpClient();

// AI Service
builder.Services.AddScoped<IAIService, AIService>();


builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<CsvImportService>();
builder.Services.AddScoped<OrderAnalyticsService>();
builder.Services.AddScoped<SalesForecastService>();

// CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyMethod()
			  .AllowAnyHeader();
	});
});

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<ChatHub>("/chatHub");

app.Run();
