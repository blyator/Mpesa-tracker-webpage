using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNpgsqlDataSource(
    builder.Configuration.GetConnectionString("Postgres")!
);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UsePathBase("/mpesa");
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllers();
app.Run();