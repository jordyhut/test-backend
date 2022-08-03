using API.APIModels;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers;

public static class DatabaseHelper
{
    public static string GetLastSequencePromotionId()
    {
        string lastId;

        using PromotionContext context = new();
        if (!context.TblPromotion.Any())
            return "0";

        lastId = context.TblPromotion
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Select(x => x.PromotionId)
            .First();

        return lastId.Substring(lastId.Length - 4);
    }

    public static List<Store> GetAllStoreData()
    {
        using PromotionContext context = new();
        List<Store> lstResult = context.TblStore
            .AsNoTracking()
            .Select(x => new Store()
            {
                Id = x.Store,
                Name = x.StoreName
            })
            .ToList();

        return lstResult;
    }

    public static void AddPromotion(Promotion promotion)
    {
        List<TblPromotion> lstNewPromotion = new();

        using PromotionContext context = new();
        using var trans = context.Database.BeginTransaction();

        for (int i = 0; i < promotion.ListItem.Count; i++)
        {
            for (int j = 0; j < promotion.ListSelectedStore.Count; j++)
            {
                lstNewPromotion.Add(new()
                {
                    PromotionId = promotion.Id,
                    PromoTypeId = promotion.PromotionType.Id,
                    ValueTypeId = promotion.PromotionType.AmountTypeId,
                    DiscountValue = promotion.PromotionType.Amount,
                    Item = promotion.ListItem[i],
                    StoreId = promotion.ListSelectedStore[j],
                    Description = promotion.Description,
                    StartDate = DateOnly.FromDateTime(promotion.StartDate),
                    EndDate = DateOnly.FromDateTime(promotion.EndDate)
                });
            }
        }

        context.TblPromotion.AddRange(lstNewPromotion);

        context.SaveChanges();

        trans.Commit();
    }
}
