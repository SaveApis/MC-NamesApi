using RestApi.Data.Context;
using RestApi.Data.Models;

namespace RestApi.Extensions;

public static class AgreementExtension
{
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