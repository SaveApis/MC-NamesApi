using RestApi.Data.Context;
using RestApi.Data.Models.Rest;

namespace RestApi.Extensions;

public static class DataContextExtension
{
    public static async Task<bool> CheckAgreement(this DataContext context, Guid uuid)
    {
        AgreementModel? agreement = await context.Agreements.FindAsync(uuid);
        if (agreement is not null) return agreement.AgreementValue;
        AgreementModel model = new AgreementModel
        {
            Uuid = uuid,
            AgreementValue = false
        };
        await context.AddAsync(model);
        await context.SaveChangesAsync();
        return false;
    }
}