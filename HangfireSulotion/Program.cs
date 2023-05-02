using Hangfire;
using HangfireBasicAuthenticationFilter;
using HangfireSulotion.Data;
using HangfireSulotion.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<HangFireContext>(option =>
{
    option.UseSqlServer("Server=.;Database=HangFireContext;Integrated Security=true;Encrypt=False;MultipleActiveResultSets=True;TrustServerCertificate=True;");

});

builder.Services.AddHangfire(x =>
{
    x.UseSqlServerStorage("Server=.;Database=HangFireContext;Integrated Security=true;Encrypt=False;MultipleActiveResultSets=True;TrustServerCertificate=True;");
});
builder.Services.AddHangfireServer();

builder.Services.AddScoped<IHangFireServices, HangFireServices>();


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


//address to see hangfire dashboard => localhost:(port)/hangfire
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "My HangFire DashBoadr",
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            User = "pourya",
            Pass = "1234"
            //put them in appsetting.json
        }
    }     
});


app.Run();
