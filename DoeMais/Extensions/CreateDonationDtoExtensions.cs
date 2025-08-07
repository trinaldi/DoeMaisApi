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
            AddressId = dto.AddressId,
            Title = dto.Title,
            Description = dto.Description ?? "",
            Quantity = dto.Quantity ?? 1,
            Status = dto.Status,
            Images = dto.Images != null ? new List<string>(dto.Images) : new List<string>()
        };
    }
}