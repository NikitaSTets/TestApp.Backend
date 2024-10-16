using MongoDB.Driver;
using TestApp.Core.Interfaces;
using TestApp.Data;
using TestApp.Infrastructure.Security;
using TestApp.Infrastructure.Services;
using TestApp.Infrastrucutre.Data.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, MongoUserRepository>();
builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mongoDBSettings = builder.Configuration.GetSection("MongoDbSettings")
      .Get<MongoDbSettings>();

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = new MongoClient(mongoDBSettings.ConnectionString);
    return client.GetDatabase(mongoDBSettings.DatabaseName);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});


var app = builder.Build();
app.UseCors("AllowAngularApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
