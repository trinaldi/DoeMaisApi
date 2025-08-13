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
            AddressSnapshot = new AddressSnapshot
            {
                Street = dto.Address.Street,
                Complement = dto.Address.Complement,
                Neighborhood = dto.Address.Neighborhood,
                City = dto.Address.City,
                State = dto.Address.State,
                ZipCode = dto.Address.ZipCode
            },
            Title = dto.Title,
            Description = dto.Description ?? "",
            Quantity = dto.Quantity ?? 1,
            Status = dto.Status,
            Category = dto.Category,
            Images = dto.Images != null ? new List<string>(dto.Images) : new List<string>()
        };
    }
}