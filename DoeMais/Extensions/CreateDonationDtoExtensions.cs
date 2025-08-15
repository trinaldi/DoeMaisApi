using DoeMais.Domain.Entities;
using DoeMais.DTOs.Donation;

namespace DoeMais.Extensions;

public static class CreateDonationDtoExtensions
{
    public static Donation ToEntity(this CreateDonationDto dto)
    {
        return new Donation
        {
            UserId = dto.UserId,
            Title = dto.Title,
            Description = dto.Description ?? "",
            Quantity = dto.Quantity ?? 1,
            Status = dto.Status,
            Category = dto.Category,
            PickupAddress = dto.PickupAddress.ToEntity(),
            DeliveryAddress = dto.DeliveryAddress.ToEntity(),
            Images = dto.Images != null ? new List<string>(dto.Images) : new List<string>()
        };
    }
}