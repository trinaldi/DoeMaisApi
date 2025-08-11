using DoeMais.Domain.Entities;
using DoeMais.DTOs.Donation;

namespace DoeMais.Extensions;

public static class DonationExtensions
{
    public static DonationDto ToDto(this Donation donation)
    {
        return new DonationDto
        {
            DonationId = donation.DonationId,
            AddressId = donation.AddressId,
            UserId = donation.UserId,
            Title = donation.Title,
            Description = donation.Description,
            Quantity = donation.Quantity,
            Status = donation.Status,
            Images = new List<string>(donation.Images)
        };
    } 
    
    public static void UpdateFromDto(this Donation donation, UpdateDonationDto dto)
    {
        donation.Title = dto.Title ?? donation.Title;
        donation.Description = dto.Description ?? donation.Description;
        donation.Quantity = dto.Quantity ?? donation.Quantity;
        donation.Status = dto.Status ?? donation.Status;

        if (dto.AddressId != 0 && dto.AddressId != donation.AddressId)
            donation.AddressId = dto.AddressId;
    }

    public static void UpdateFrom(this Donation target, Donation source)
    {
        target.Title = source.Title;
        target.Description = source.Description;
        target.Quantity = source.Quantity;
        target.Status = source.Status;
        target.Images = source.Images;
    }
    
    public static Donation Clone(this Donation? donation)
    {
        if (donation is null)
        {
            return null;
        }

        return new Donation
        {
            DonationId = donation.DonationId,
            AddressId = donation.AddressId,
            UserId = donation.UserId,
            Title = donation.Title,
            Description = donation.Description,
            Quantity = donation.Quantity,
            Status = donation.Status,
            Address = donation.Address,
            Images = donation.Images,
            User = donation.User
        };
    }
}