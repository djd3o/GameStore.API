using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Endpoints;
using GameStore.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

builder.AddGameStoreDb();

var app = builder.Build();

app.MapGameEndpoints();

app.MigrateDb();

app.Run(); 
