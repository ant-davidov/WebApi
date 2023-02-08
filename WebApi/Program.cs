using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApi.Data;
using WebApi.Hellpers;
using WebApi.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Default");
if(String.IsNullOrEmpty(connectionString)) connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<DataContext>(options =>  
              options.UseNpgsql(connectionString),ServiceLifetime.Transient);
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddAuthentication("BasicAuthentication").
        AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null) ; 
builder.Services.AddControllers().AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();


