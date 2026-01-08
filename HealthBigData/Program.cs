using HealthBigData.Context;
using HealthBigData.Controllers;
using HealthBigData.Repositories.ChartRepositories;
using HealthBigData.Repositories.HastaRepositories;
using Polly;
using Polly.Extensions.Http;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient<GeminiService>()
       .SetHandlerLifetime(TimeSpan.FromMinutes(5))
       .AddPolicyHandler(GetRetryPolicy());
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IDashboardChartRepository, DashboardChartRepository>();
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddControllersWithViews();

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


app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}