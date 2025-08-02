using DoeMais.Domain.Entities;
using DoeMais.DTO.Donation;

namespace DoeMais.Extensions;

public static class CreateDonationDtoExtensions
{
    public static Donation ToEntity(this CreateDonationDto dto, long userId)
    {
        return new Donation
        {
            UserId = userId,
            AddressId = dto.AddressId,
            Title = dto.Title,
            Description = dto.Description ?? "",
            Quantity = dto.Quantity ?? 1,
            Status = dto.Status,
            Images = dto.Images != null ? new List<string>(dto.Images) : new List<string>()
        };
    }
}