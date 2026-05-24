using System;
using GameStore.API.Dtos;

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
            group.MapGet("/", () => games);

            //Get /games/1
            group.MapGet("/{id}", (int id) =>
            {
                var game = games.Find(game => game.Id == id);
                return game is null ? Results.NotFound() : Results.Ok(games);
            })
            .WithName(GetGameEndpointName);

            //POST /games
            group.MapPost("/", (CreateGameDto newGame) =>
            {
                GameDto game = new GameDto(
                    games.Count + 1, // Auto-increment ID
                    newGame.Name,
                    newGame.Genre,
                    newGame.Price,
                    newGame.ReleaseDate
                );

                games.Add(game);

                return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
            });

            //PUT /games/1
            group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
            {
                var index = games.FindIndex(game => game.Id == id);

                if (index == -1)
                {
                    return Results.NotFound();
                }

                games[index] = new GameDto(
                    id,
                    updatedGame.Name,
                    updatedGame.Genre,
                    updatedGame.Price,
                    updatedGame.ReleaseDate
                );

                return Results.NoContent();
            });

            //DELETE/games/1

            group.MapDelete("/{id}", (int id) =>
            {
                games.RemoveAll(game => game.Id == id);

                return Results.NoContent();
            });
        }
    }
}
