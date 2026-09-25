using PickMeUp.Api.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddDotNetEnv();
builder.Services.AddPickMeUpApi(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseCors("dev");
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
