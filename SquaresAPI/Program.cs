using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using NLog.Web;
using SquaresAPI;
using SquaresAPI.DataAcccessLayer.Repository;
using SquaresAPI.MiddleWare;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureNLog(); 
builder.Services.ConfigureServices(builder.Configuration);   
var app = builder.Build();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    IdentityModelEventSource.ShowPII = true;
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDBContext>().Database.Migrate();
}
app.Run();
public partial class Program { }