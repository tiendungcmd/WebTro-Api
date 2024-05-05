using Microsoft.EntityFrameworkCore;
using MotelApi.Common.Mapper;
using MotelApi.DBContext;
using MotelApi.Services;
using MotelApi.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<MotelContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("WebMotelConnection")));

builder.Services.AddAutoMapper(typeof(MapperConfig).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IMotelService, MotelService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("http://localhost:4200"));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();

app.Run();
