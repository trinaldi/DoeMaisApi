using DoeMais.Domain.Entities;
using DoeMais.Domain.OwnedTypes;
using DoeMais.DTOs.Address;
using DoeMais.Tests.Domain;

namespace DoeMais.Tests.Extensions;

public static class FakeAddressExtensions
{
    public static Address ToEntity(this FakeAddress fakeAddress)
    {
        var address = new Address
        {
            Street = fakeAddress.Street ?? "",
            Complement = fakeAddress.Complement,
            Neighborhood = fakeAddress.Neighborhood ?? "",
            City = fakeAddress.City ?? "",
            State = fakeAddress.State ?? "",
            ZipCode = fakeAddress.ZipCode ?? "",
            IsPrimary = fakeAddress.IsPrimary,
        };
        
        return address;
    }

    public static AddressDto ToDto(this FakeAddress fakeAddress)
    {
        
        var dto = new AddressDto
        {
            Street = fakeAddress.Street ?? "",
            Complement = fakeAddress.Complement,
            Neighborhood = fakeAddress.Neighborhood ?? "",
            City = fakeAddress.City ?? "",
            State = fakeAddress.State ?? "",
            ZipCode = fakeAddress.ZipCode ?? "",
            IsPrimary = fakeAddress.IsPrimary,
        };
        
        return dto;
    }
    
}