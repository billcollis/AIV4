using AIV4.Server.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.SlidingExpiration = true;
        //options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
        options.AccessDeniedPath = "/access-denied";
    });
builder.Services.AddAuthorization();

//builder.Services.AddRazorComponents(options =>
//{
//    options.FormMappingUseCurrentCulture = true;
//    options.MaxFormMappingCollectionSize = 1024;
//    options.MaxFormMappingErrorCount = 200;
//    options.MaxFormMappingKeySize = 1024 * 2;
//    options.MaxFormMappingRecursionDepth = 64;
//}).AddInteractiveServerComponents();

//builder.Services.addCascadingAuthenticationState();

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults
        .MimeTypes
        .Concat(new[] { "application/octet-stream" });
});

//var OpenAI_Key = builder.Configuration["OpenAI_key:mykey"];

var app = builder.Build();
app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
//app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();


app.MapRazorPages();
app.MapControllers();

app.MapHub<ChatGPTHub>("/chatgpthub"); //where the clients connects to the server

app.MapFallbackToFile("index.html");



app.Run();
