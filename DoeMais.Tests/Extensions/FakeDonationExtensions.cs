using DoeMais.Domain.Entities;
using DoeMais.Tests.Domain;

namespace DoeMais.Tests.Extensions;

public static class FakeDonationExtensions
{
    public static Donation ToEntity(this FakeDonation fakeDonation)
    {
        return new Donation()
        {
            DonationId = fakeDonation.DonationId,
            UserId = fakeDonation.UserId,
            AddressId = fakeDonation.AddressId,
            Title = fakeDonation.Title,
            Description = fakeDonation.Description,
            Quantity = fakeDonation.Quantity,
            Status = fakeDonation.Status,
            Images = fakeDonation.Images

        };
    }
    
}