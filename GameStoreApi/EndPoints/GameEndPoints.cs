using GameStoreApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi;

public static class GameEndPoints
{
    public const string NewGameEndPoint = "GetGame";

    public static RouteGroupBuilder GetGameEndPoints(this WebApplication app){
        var group = app.MapGroup("games").WithParameterValidation();




        group.MapGet("/", async (GameStoreDBContext dBContext)=>
            await dBContext.Games
            .Include(game => game.Genre).Select(game => game.ToGameSummaryDto()).AsNoTracking().ToListAsync()
        );


        group.MapPut("/{id}",async (int id,UpdateGameDtos UpdatedGame,GameStoreDBContext dBContext)=>{
            // Game? fgame = dBContext.Games.Find(id);
            var existingGame  = await dBContext.Games.FindAsync(id);
            if(existingGame is null){
                return Results.NotFound();
            }
            // Entry is used to locate a current game.
            // convert UpdateGameDtos ->Game 
            Game ugame = new(){
                Id = id,
                Name = UpdatedGame.Name,
                GenreId = UpdatedGame.GenreId,
                Price = UpdatedGame.Price,
                Releasedate = UpdatedGame.ReleaseDate
            };

            dBContext.Entry(existingGame).CurrentValues.SetValues(ugame);
            await dBContext.SaveChangesAsync();
            return Results.NoContent();

        });



        group.MapGet("/{id}",async (int id,GameStoreDBContext dBContext)=>{
            Game? game = await dBContext.Games.FindAsync(id);
            GameDetailsDto gameD = new(
                game!.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.Releasedate
            );
            return game is null ? Results.NotFound():Results.Ok(gameD);
        }).WithName(NewGameEndPoint);




        // create Game
        group.MapPost("/",async (CreateGameDto newGame,GameStoreDBContext dBContext) =>{
            Game game = new()
            {
                Name = newGame.Name,
                Genre = dBContext.Genres.Find(newGame.GenreId),
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                Releasedate = newGame.ReleaseDate
            };  

            dBContext.Games.Add(game);
            await dBContext.SaveChangesAsync();

            GameDto newgameDto = new(
                game.Id,
                game.Name,
                game.Genre!.Name,
                game.Price,
                game.Releasedate
            );
            return Results.CreatedAtRoute(NewGameEndPoint,new {id = game.Id},newgameDto);
        });

        group.MapDelete("/{id}",async (int id,GameStoreDBContext dBContext)=>{
            await dBContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });
        return group;
    }

}
