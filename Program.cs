using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Random rnd= new Random();



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

app.MapGet("/download", async (HttpContext context,string name) =>

{
    
    bool img = false;
    var files = new DirectoryInfo(@"data\").GetFiles();
    foreach (var file in files)
    {
        if (file.Name == name)

        {
            await context.Response.SendFileAsync($@"data\{name}");
            img = true;
        }

    }
    if (!img)
    {
        await context.Response.WriteAsync("Image Not Found");  
    }
      



    
});


app.MapGet("/rndimage", async (context) =>
{
    var files = new DirectoryInfo(@"data\").GetFiles();
    int index = new Random().Next(0, files.Length);


    await context.Response.SendFileAsync($@"data\{files[index].Name}");
    Console.WriteLine("Открыт");
});


app.MapGet("/", () => "Hello World!");

app.Run();
