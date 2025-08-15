using DoeMais.Domain.Entities;
using DoeMais.Domain.Enums;
using DoeMais.DTOs.Donation;

namespace DoeMais.Extensions;

public static class DonationExtensions
{
    public static DonationDto ToDto(this Donation donation)
    {
        return new DonationDto
        {
            DonationId = donation.DonationId,
            UserId = donation.UserId,
            Title = donation.Title,
            Description = donation.Description,
            Quantity = donation.Quantity,
            Status = donation.Status,
            Category = donation.Category,
            Images = new List<string>(donation.Images)
        };
    } 
    
    public static void UpdateFromDto(this Donation donation, UpdateDonationDto dto)
    {
        donation.Title = dto.Title ?? donation.Title;
        donation.Description = dto.Description ?? donation.Description;
        donation.Quantity = dto.Quantity ?? donation.Quantity;
        donation.Status = dto.Status ?? donation.Status;
        donation.Category = dto.Category ?? donation.Category;
    }

    public static void UpdateFrom(this Donation target, Donation source)
    {
        target.Title = source.Title;
        target.Description = source.Description;
        target.Quantity = source.Quantity;
        target.Status = source.Status;
        target.Category = source.Category;
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
            UserId = donation.UserId,
            Title = donation.Title,
            Description = donation.Description,
            Quantity = donation.Quantity,
            Status = donation.Status,
            Category = donation.Category,
            Address = donation.Address,
            Images = donation.Images,
            User = donation.User
        };
    }
    public static CreateDonationDto ToCreateDonationDto(this Donation donation)
    {
        return new CreateDonationDto
        {
            UserId = donation.UserId,
            Title = donation.Title,
            Description = donation.Description,
            Quantity = donation.Quantity,
            Status = donation.Status,
            Category = donation.Category,
            Images = donation.Images?.ToList() ?? new List<string>()
        };
    }
}