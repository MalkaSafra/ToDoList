
using Microsoft.EntityFrameworkCore;
using TodoApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddCors(options =>
{
    
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()      
                  .AllowAnyMethod();     
        });
});

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ToDoDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("ToDoDB"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.35-mysql"));
});

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();


app.UseCors("AllowAll");


app.MapGet("/api/items", async (ToDoDbContext db) =>
{
    return await db.Items.ToListAsync();
});



app.MapPost("/api/items", async (ToDoDbContext db, Item item) =>
{
 

    db.Items.Add(item);
    await db.SaveChangesAsync();

    return Results.Created($"/api/items/{item.Id}", item);
});


app.MapPut("/api/items/{id}", async (ToDoDbContext db, int id) =>
{
    var itemToUpdate = await db.Items.FindAsync(id);

    if (itemToUpdate == null)
    {
        return Results.NotFound(); // 404
    }

    
    itemToUpdate.IsComplete = true;

    await db.SaveChangesAsync();
    return Results.NoContent(); // 204
});


app.MapDelete("/api/items/{id}", async (ToDoDbContext db, int id) =>
{
    var itemToDelete = await db.Items.FindAsync(id);

    if (itemToDelete == null)
    {
        return Results.NotFound(); // 404
    }

    db.Items.Remove(itemToDelete);
    await db.SaveChangesAsync();
    return Results.NoContent(); // 204
});

app.MapGet("/", () => "nice to meet you");

app.Run();
