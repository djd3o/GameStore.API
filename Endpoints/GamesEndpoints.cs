using System;
using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Endpoints
{
    public static class GamesEndpoints
    {
        const string GetGameEndpointName = "GetGame";
        private static readonly List<GameDto> games = [
            new GameDto(1, "The Legend of Zelda: Breath of the Wild", "Action-Adventure", 59.99m, new DateOnly(2017, 3, 3)),
            new GameDto(2, "God of War", "Action-Adventure", 49.99m, new DateOnly(2018, 4, 20)),
            new GameDto(3, "Red Dead Redemption 2", "Action-Adventure", 59.99m, new DateOnly(2018, 10, 26)),
            new GameDto(4, "The Witcher 3: Wild Hunt", "RPG", 39.99m, new DateOnly(2015, 5, 19)),
            new GameDto(5, "Cyberpunk 2077", "RPG", 59.99m, new DateOnly(2020, 12, 10))
        ];


        public static void MapGameEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/games");
            // GET: /games
            group.MapGet("/", async (GameStoreContext dbContext)
                => await dbContext.Games
                            .Include(game => game.Genre)
                            .Select(game => new GameDto(
                                game.Id,
                                game.Name,
                                game.Genre!.Name,
                                game.Price,
                                game.ReleaseDate
                            ))
                            .AsNoTracking()
                            .ToListAsync());

            //Get /games/1
            group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                var game = await dbContext.Games.FindAsync(id);
                return game is null ? Results.NotFound() : Results.Ok(
                    new GameDetailsDto(
                        game.Id,
                        game.Name,
                        game.GenreId,
                        game.Price,
                        game.ReleaseDate
                    
                    )
                );
            })
            .WithName(GetGameEndpointName);

            //POST /games
            group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
            {
                Game game = new()
                {
                    Name = newGame.Name,
                    GenreId = newGame.GenreId,
                    Price = newGame.Price,
                    ReleaseDate = newGame.ReleaseDate
                };

                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();

                GameDetailsDto gameDto = new(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                );

                return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
            });

            //PUT /games/1
            group.MapPut("/{id}", async(int id,
                                        UpdateGameDto updatedGame,
                                        GameStoreContext dbContext) =>
            {
                var existingGame = await dbContext.Games.FindAsync(id);

                if (existingGame is null)
                {
                    return Results.NotFound();
                }

                existingGame.Name = updatedGame.Name;
                existingGame.GenreId = updatedGame.GenreId;
                existingGame.Price = updatedGame.Price;
                existingGame.ReleaseDate = updatedGame.ReleaseDate;

                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            });

            //DELETE/games/1

            group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
            {

                await dbContext.Games
                                .Where(game => game.Id == id)
                                .ExecuteDeleteAsync();

                return Results.NoContent();
            });
        }
    }
}
