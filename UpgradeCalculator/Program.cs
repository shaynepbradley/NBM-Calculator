using AtomicAssets.Classes;
using AtomicAssets.Interfaces;
using AtomicAssets.Models;
using GenericClient;
using UpgradeCalculator.Classes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Host.ConfigureServices(services =>
{
    services.AddSingleton<IAtomicClient<BattleMiners, Construction>, AtomicClient<BattleMiners, Construction>>().
        AddSingleton<IAtomicClient<BattleMiners, Lands>, AtomicClient<BattleMiners, Lands>>().
        AddSingleton<IGenericClient, GenericClient.GenericClient>().
        AddSingleton<BattleMiners>().
        AddSingleton<Alcor.IAlcorClient, Alcor.AlcorClient>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
