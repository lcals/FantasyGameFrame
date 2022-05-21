var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDirectoryBrowser();
var app = builder.Build();
app.UseFileServer(enableDirectoryBrowsing: true);
app.Run();