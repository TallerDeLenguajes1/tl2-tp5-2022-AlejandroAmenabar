global using static tp5.Sesiones.Session;
global using tp5.Repositories;
global using tp5.Models;
global using tp5.ViewModels;
global using NLog;
global using AutoMapper;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Session;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Data.Sqlite;
global using System.Diagnostics;
global using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program)); //para que ande el automapper
builder.Services.AddLogging();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddTransient<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddTransient<IRepositorioPedido, RepositorioPedido>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(3);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
