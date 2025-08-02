using DoeMais.Domain.Entities;
using DoeMais.DTO.Address;

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
}