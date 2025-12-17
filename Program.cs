using Microsoft.EntityFrameworkCore;
using HW.Data;  

var builder = WebApplication.CreateBuilder(args);
// ADD SERVICES FIRST
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();
// CONFIGURE PIPELINE
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
