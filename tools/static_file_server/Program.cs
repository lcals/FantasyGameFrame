var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDirectoryBrowser();
var app = builder.Build();
app.UseFileServer(new FileServerOptions()
{
    EnableDirectoryBrowsing = true,
    StaticFileOptions =
    {
        ServeUnknownFileTypes = true,
        DefaultContentType = "application/octet-stream"
    }
});
app.Run();