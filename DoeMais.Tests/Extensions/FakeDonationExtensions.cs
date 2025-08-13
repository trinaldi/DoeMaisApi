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
            AddressSnapshot = fakeDonation.FakeAddress.ToAddressSnapshot(),
            Title = fakeDonation.Title,
            Description = fakeDonation.Description,
            Quantity = fakeDonation.Quantity,
            Status = fakeDonation.Status,
            Category = fakeDonation.Category,
            Images = fakeDonation.Images
        };
    }

    public static DonationDto ToDto(this FakeDonation fakeDonation)
    {
        return new DonationDto()
        {
            DonationId = fakeDonation.DonationId,
            UserId = fakeDonation.UserId,
            Title = fakeDonation.Title,
            Description = fakeDonation.Description,
            Quantity = fakeDonation.Quantity,
            Status = fakeDonation.Status,
            Category = fakeDonation.Category,
            AddressSnapshot = fakeDonation.FakeAddress.ToAddressSnapshot(),
            Images = fakeDonation.Images
        };
    }
}