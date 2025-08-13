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
            AddressSnapshot = new AddressSnapshot
            {
                Street = donation.AddressSnapshot.Street,
                Complement = donation.AddressSnapshot.Complement,
                Neighborhood = donation.AddressSnapshot.Neighborhood,
                City = donation.AddressSnapshot.City,
                State = donation.AddressSnapshot.State,
                ZipCode = donation.AddressSnapshot.ZipCode
            },
            UserId = donation.UserId,
            Title = donation.Title,
            Description = donation.Description,
            Quantity = donation.Quantity,
            Status = donation.Status,
            Category = donation.Category,
            Images = donation.Images != null ? new List<string>(donation.Images) : new List<string>()
        };
    }
    
    public static void UpdateFromDto(this Donation donation, UpdateDonationDto dto)
    {
        donation.Title = dto.Title ?? donation.Title;
        donation.Description = dto.Description ?? donation.Description;
        donation.Quantity = dto.Quantity ?? donation.Quantity;
        donation.Status = dto.Status ?? donation.Status;
        donation.Category = dto.Category ?? donation.Category;

        if (dto.AddressSnapshot != null)
        {
            donation.AddressSnapshot = new AddressSnapshot
            {
                Street = dto.AddressSnapshot.Street,
                Complement = dto.AddressSnapshot.Complement,
                Neighborhood = dto.AddressSnapshot.Neighborhood,
                City = dto.AddressSnapshot.City,
                State = dto.AddressSnapshot.State,
                ZipCode = dto.AddressSnapshot.ZipCode
            };
        }
    }

    public static void UpdateFrom(this Donation target, Donation source)
    {
        target.Title = source.Title;
        target.Description = source.Description;
        target.Quantity = source.Quantity;
        target.Status = source.Status;
        target.Category = source.Category;
        target.Images = source.Images != null ? new List<string>(source.Images) : new List<string>();

        if (source.AddressSnapshot != null)
        {
            target.AddressSnapshot = new AddressSnapshot
            {
                Street = source.AddressSnapshot.Street,
                Complement = source.AddressSnapshot.Complement,
                Neighborhood = source.AddressSnapshot.Neighborhood,
                City = source.AddressSnapshot.City,
                State = source.AddressSnapshot.State,
                ZipCode = source.AddressSnapshot.ZipCode
            };
        }
    }

    
    public static Donation Clone(this Donation? donation)
    {
        if (donation is null)
            return null!;

        return new Donation
        {
            DonationId = donation.DonationId,
            UserId = donation.UserId,
            Title = donation.Title,
            Description = donation.Description,
            Quantity = donation.Quantity,
            Status = donation.Status,
            Category = donation.Category,
            Images = donation.Images != null ? new List<string>(donation.Images) : new List<string>(),
            User = donation.User,
            AddressSnapshot = donation.AddressSnapshot != null
                ? new AddressSnapshot
                {
                    Street = donation.AddressSnapshot.Street,
                    Complement = donation.AddressSnapshot.Complement,
                    Neighborhood = donation.AddressSnapshot.Neighborhood,
                    City = donation.AddressSnapshot.City,
                    State = donation.AddressSnapshot.State,
                    ZipCode = donation.AddressSnapshot.ZipCode
                }
                : new AddressSnapshot()
        };
    }
    
    public static CreateDonationDto ToCreateDonationDto(this Donation donation)
    {
        return new CreateDonationDto
        {
            UserId = donation.UserId,
            Address = new AddressSnapshot
            {
                Street = donation.AddressSnapshot.Street,
                Complement = donation.AddressSnapshot.Complement,
                Neighborhood = donation.AddressSnapshot.Neighborhood,
                City = donation.AddressSnapshot.City,
                State = donation.AddressSnapshot.State,
                ZipCode = donation.AddressSnapshot.ZipCode
            },
            Title = donation.Title,
            Description = donation.Description,
            Quantity = donation.Quantity,
            Status = donation.Status,
            Category = donation.Category,
            Images = donation.Images?.ToList() ?? new List<string>()
        };
    }

}