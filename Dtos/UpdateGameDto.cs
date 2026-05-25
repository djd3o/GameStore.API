using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Dtos;

    //Record was added in C# 9 and later, so Class was used instead. 
    //See more info: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record
    public record UpdateGameDto(
        [Required][StringLength(50)]string Name,
        [Range(1, 50 )]int GenreId,
        [Range(0, 100)]decimal Price,
        DateOnly ReleaseDate
    );

