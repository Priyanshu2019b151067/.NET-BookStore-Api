using System.ComponentModel.DataAnnotations;

namespace GameStoreApi.Dtos;

public record class UpdateGameDtos(
    [Required][StringLength(50)] string Name,
    int GenreId,
    [Range(1,100)] decimal Price,
    DateOnly ReleaseDate
);
