using GameStore.API.Dtos;
const string GetGameEndpointName = "GetGame";
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
    new GameDto(1, "The Legend of Zelda: Breath of the Wild", "Action-Adventure", 59.99m, new DateOnly(2017, 3, 3)),
    new GameDto(2, "God of War", "Action-Adventure", 49.99m, new DateOnly(2018, 4, 20)),
    new GameDto(3, "Red Dead Redemption 2", "Action-Adventure", 59.99m, new DateOnly(2018, 10, 26)),
    new GameDto(4, "The Witcher 3: Wild Hunt", "RPG", 39.99m, new DateOnly(2015, 5, 19)),
    new GameDto(5, "Cyberpunk 2077", "RPG", 59.99m, new DateOnly(2020, 12, 10))
];

// GET: /games
app.MapGet("/games", () => games);

//Get /games/1
app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id))
.WithName(GetGameEndpointName);

//POST /games
app.MapPost("/games", (CreateGameDto newGame) =>
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

app.Run();
