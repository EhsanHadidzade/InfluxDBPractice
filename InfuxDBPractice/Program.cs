using app.Services;
using Coravel;
using app.Invocables; // You'll create this namespace soon :)

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IInfluxDBService, InfluxDBService>();

// You'll create this class soon :)
//builder.Services.AddTransient<WriteRandomPlaneAltitudeInvocable>();
//builder.Services.AddScheduler();


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

//app.ApplicationServices.UseScheduler(scheduler =>
//{
//    scheduler
//        .Schedule<WriteRandomPlaneAltitudeInvocable>()
//        .EveryFiveSeconds();
//});


app.Run();
