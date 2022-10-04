using RestApi.Data.Context;
using RestApi.Data.Models.Db;

namespace RestApi.Extensions;

/// <summary>
/// Custom extension class
/// </summary>
public static class AgreementExtension
{
    /// <summary>
    /// Extension Method to check if the Player accepted the Agreement
    /// </summary>
    /// <param name="context">Instance of the DataContext</param>
    /// <param name="uuid">The UUID of the Player</param>
    /// <returns></returns>
    public static async Task<bool> CheckAgreement(this DataContext context, Guid uuid)
    {
        AgreementModel? model = await context.Agreements.FindAsync(uuid);

        if (model is not null) return model.AgreeValue;
        AgreementModel newModel = new AgreementModel()
        {
            Uuid = uuid,
            AgreeValue = false
        };
        await context.Agreements.AddAsync(newModel);
        await context.SaveChangesAsync();
        return await context.CheckAgreement(uuid);
    }
}