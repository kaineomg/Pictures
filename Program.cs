using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();





app.MapPost("/upload", async (HttpContext context, IConfiguration cfg) =>
{
    var storagePath = cfg.GetValue<string>("Datastorage");
    var files= context.Request.Form.Files;
    var uploadPath = Directory.GetCurrentDirectory() + storagePath;
    if (!Directory.Exists(uploadPath))
    {
        Directory.CreateDirectory(uploadPath);
    }
    foreach (var file in files)
    {
        
        string fullPath = uploadPath+"/"+file.FileName;

        // сохраняем файл в папку uploads
        using (var fileStream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
    }
    

});

app.MapGet("/download", async ( context) =>
{
    await context.Response.SendFileAsync(@"data\file.png");
    Console.WriteLine("Открыт");
});



app.MapGet("/", () => "Hello World!");

app.Run();
