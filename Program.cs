using GameStore.API.Dtos;
using GameStore.API.Endpoints;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGameEndpoints();

app.Run(); 
