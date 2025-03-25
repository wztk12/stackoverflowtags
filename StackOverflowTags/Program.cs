using Microsoft.EntityFrameworkCore;
using StackOverflowTags.Database;
using StackOverflowTags.Services;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});
builder.Services.AddCors();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=Database/tags.db")
);
builder.Services.AddHttpClient<TagsService>();
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs.txt", rollingInterval: RollingInterval.Month)
    .CreateLogger();
builder.Services.AddSingleton(Log.Logger);
string dbPath = "Database/tags.db";

if (!Directory.Exists(Path.GetDirectoryName(dbPath)))
{
    Directory.CreateDirectory("Database");
}

if (!File.Exists(dbPath))
{
    File.Create(dbPath).Dispose();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(
    b =>
    {
        b.SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    }
);

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}


app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }