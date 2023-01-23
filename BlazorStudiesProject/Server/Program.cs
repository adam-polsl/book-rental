using BlazorStudiesProject.Server;
using BlazorStudiesProject.Server.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddDbContext()
    .RegisterServices()
    .RegisterIdentity()
    .AddAuth()
    .AddAutoMapper(typeof(Program));
builder.Services.AddSwaggerGen();
//.AddAuthorization(); 
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    //options.Cookie.HttpOnly = true;
//    options.Events.OnRedirectToLogin = context =>
//    {
//        context.Response.StatusCode = 401;
//        return Task.CompletedTask;
//    };
//});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

_ = app.Services.GetRequiredService<AppBackgroundWorker>(); //activation

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
    //{
    //    //Note: Microsoft recommends to NOT migrate your database at Startup. 
    //    //You should consider your migration strategy according to the guidelines.
    //    serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
    //    // var xd = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Users.ToList();
    //    //var x = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Sers.ToList();
    //}

    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/documentation", "Blazor API");
});

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
