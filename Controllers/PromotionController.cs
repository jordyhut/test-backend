using API.APIModels;
using API.Helpers;
using Carter;

namespace API.Controllers;

public class PromotionController : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/generateid", GeneratePromotionId);
        app.MapGet("/getallstore", GetAllStore);

        app.MapPost("/add", AddPromotion);
    }

    public IResult GeneratePromotionId()
    {
        try
        {
            string nextSequence;
            string newId;
            string lastId = DatabaseHelper.GetLastSequencePromotionId();

            if (lastId == "0")
                return Results.Ok($"P{DateTime.Now:yyyyMMdd}0001");

            nextSequence = GetNewSequence(lastId);
            newId = $"P{DateTime.Now:yyyyMMdd}{nextSequence}";

            return Results.Ok(newId);
        }
        catch
        {
            return Results.StatusCode(500);
        }
    }

    public IResult GetAllStore()
    {
        try
        {
            List<Store> listResult = DatabaseHelper.GetAllStoreData();

            return Results.Ok(listResult);
        }
        catch
        {
            return Results.StatusCode(500);
        }
    }

    public IResult AddPromotion(Promotion promotionData)
    {
        try
        {
            DatabaseHelper.AddPromotion(promotionData);

            return Results.Ok();
        }
        catch
        {
            return Results.StatusCode(500);
        }
    }

    private static string GetNewSequence(string lastSequence)
    {
        int lastSeq = int.Parse(lastSequence) + 1;
        string zeroAppend = new('0', 4 - lastSeq.ToString().Length);

        return $"{zeroAppend}{lastSeq}";
    }
}
