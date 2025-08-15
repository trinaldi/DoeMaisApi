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
            PickupAddress = fakeDonation.FakePickupAddress.ToEntity(),
            DeliveryAddress = fakeDonation.FakeDeliveryAddress.ToEntity(),
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
            PickupAddress = fakeDonation.FakePickupAddress.ToDto(),
            DeliveryAddress = fakeDonation.FakeDeliveryAddress.ToDto(),
            Images = fakeDonation.Images
        };
    }
}