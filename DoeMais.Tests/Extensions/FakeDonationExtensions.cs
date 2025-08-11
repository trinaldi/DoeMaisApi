using DoeMais.Domain.Entities;
using DoeMais.DTOs.Donation;
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
            Address = fakeDonation.FakeAddress.ToEntity(),
            Title = fakeDonation.Title,
            Description = fakeDonation.Description,
            Quantity = fakeDonation.Quantity,
            Status = fakeDonation.Status,
            Images = fakeDonation.Images
        };
    }

    public static DonationDto ToDto(this FakeDonation fakeDonation)
    {
        return new DonationDto()
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