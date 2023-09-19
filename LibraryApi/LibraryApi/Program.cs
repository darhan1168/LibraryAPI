using LibraryApi.Repositories.Implementations;
using LibraryApi.Repositories.Interfaces;
using LibraryApi.Services.Implementations;
using LibraryApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ISeedValuesService, SeedValuesService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LibraryApi.AppContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var seedValuesService = serviceProvider.GetRequiredService<ISeedValuesService>();
    var addSeedAdminResult = seedValuesService.AddSeedAdmin().Result;
    
    if (!addSeedAdminResult.IsSuccessful)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(addSeedAdminResult.Message);
    }
}

app.Run();