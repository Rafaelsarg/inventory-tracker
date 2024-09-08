using InventoryManagementSystemWeb.Components;
using InventoryManagementSystemWeb.Components.Services;
using System.Net.Http;
using Blazored.LocalStorage;
using InventoryManagementSystemWeb.Components.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
// Configure HttpClient to use your API base address
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:5248/");
});

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
builder.Services.AddScoped<IAvailableProductService, AvailableProductService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IRestockingTransactionService, RestockingTransactionService>();
builder.Services.AddScoped<ISalesTransactionService, SalesTransactionService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddHttpContextAccessor();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseStaticFiles();

app.UseRouting();

// Set up session before the response starts

// Enable session before UseEndpoints or UseAuthorization
app.UseSession();
app.Use(async (context, next) =>
{
    // Set a preliminary session value to initialize the session data store
    context.Session.SetString("Initialized", "true");
    await next.Invoke();
});

app.UseAuthorization();
app.UseAntiforgery();

app.Run();