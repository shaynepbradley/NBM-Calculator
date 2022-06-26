using AtomicAssets.Classes;
using AtomicAssets.Interfaces;
using AtomicAssets.Models;
using GenericClient;
using UpgradeCalculator.Classes;
using UpgradeCalculator.Interfaces;
using UpgradeCalculator.ToBeValidated;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Host.ConfigureServices(services =>
{
    services.AddSingleton<IAtomicClient<BattleMiners, Construction>, AtomicClient<BattleMiners, Construction>>().
        //AddSingleton<IAtomicClient<BattleMiners, Lands>, AtomicClient<BattleMiners, Lands>>().
        AddSingleton<IGenericClient, GenericClient.GenericClient>().
        AddSingleton<IResourcePrices, ResourcePrices>().
        AddSingleton<Alcor.IAlcorClient, Alcor.AlcorClient>().
        AddSingleton<IAlbums, Albums>().
        AddSingleton<ICollections, Collections>().
        AddSingleton<ISchemas, Schemas>().
        AddSingleton<ITemplates, Templates>().
        AddScoped<IWaxId, WaxId>().
        AddScoped<IMiningInfo, MiningInfo>();
});
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

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