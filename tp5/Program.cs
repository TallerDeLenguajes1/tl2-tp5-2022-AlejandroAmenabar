// global using static tp5.Util.CsvHelper;
global using tp5.ViewModels.Cadete;
global using tp5.ViewModels.Pedido;
// global using tp5.ViewModels.Pedido;
global using tp5.Models;
global using Microsoft.AspNetCore.Mvc;
global using AutoMapper;
global using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program));

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
