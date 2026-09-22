using RecordDB.API.Data;
using RecordDB.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------
// Services
// -----------------------------------------------------------------------

builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title   = "RecordDB API",
        Version = "v1",
        Description = "RESTful API for the RecordDB music collection database."
    });

    // Include XML comments from this assembly so Swagger shows <summary> docs.
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

// Data layer (singleton — stateless, just holds config)
builder.Services.AddSingleton<IDataAccess, DataAccess>();

// Repositories (scoped — one per HTTP request)
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IRecordRepository, RecordRepository>();
builder.Services.AddScoped<IDiscRepository, DiscRepository>();
builder.Services.AddScoped<ITrackRepository, TrackRepository>();

// -----------------------------------------------------------------------
// Pipeline
// -----------------------------------------------------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "RecordDB API v1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at app root
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
