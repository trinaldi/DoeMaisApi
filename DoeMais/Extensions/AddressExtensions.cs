using DoeMais.Domain.Entities;
using DoeMais.DTOs.Address;

namespace DoeMais.Extensions;

public static class AddressExtensions
{
    public static AddressDto ToDto(this Address address)
    {
        return new AddressDto
        {
            AddressId = address.AddressId,
            Street = address.Street,
            Complement = address.Complement,
            Neighborhood = address.Neighborhood,
            City = address.City,
            State = address.State,
            ZipCode = address.ZipCode,
            IsPrimary = address.IsPrimary
        };
    } 
    
    public static Address ToEntity(this AddressDto dto)
    {
        return new Address
        {
            AddressId = dto.AddressId,
            Street = dto.Street,
            Complement = dto.Complement,
            Neighborhood = dto.Neighborhood,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
            IsPrimary = dto.IsPrimary
        };
    }


    public static void UpdateFromDto(this Address address, AddressDto dto)
    {
        address.Street = dto.Street;
        address.Complement = dto.Complement;
        address.Neighborhood = dto.Neighborhood;
        address.City = dto.City;
        address.State = dto.State;
        address.ZipCode = dto.ZipCode;
        address.IsPrimary = dto.IsPrimary;
    }
    
    public static void UpdateFrom(this Address target, Address source)
    {
        target.Street = source.Street;
        target.Complement = source.Complement;
        target.Neighborhood = source.Neighborhood;
        target.City = source.City;
        target.State = source.State;
        target.ZipCode = source.ZipCode;
        target.IsPrimary = source.IsPrimary;
    }
    
    public static Address Clone(this Address? address)
    {
        if (address == null)
            return null;

        return new Address
        {
            AddressId = address.AddressId,
            UserId = address.UserId,
            Street = address.Street ?? "",
            Complement = address.Complement,
            Neighborhood = address.Neighborhood ?? "",
            City = address.City ?? "",
            State = address.State ?? "",
            ZipCode = address.ZipCode ?? "",
            IsPrimary = address.IsPrimary,
            Donations = address.Donations
        };
    }
    

}