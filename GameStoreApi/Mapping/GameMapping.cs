using GameStoreApi.Dtos;

namespace GameStoreApi;

public static class GameMapping
{
    public static GameDto ToGameSummaryDto(this Game game){
        return new (
            game.Id,
            game.Name,
            game.Genre!.Name,
            game.Price,
            game.Releasedate
        );
    }
}
